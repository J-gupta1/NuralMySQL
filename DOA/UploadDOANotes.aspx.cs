/*
 * ------------------------------------------------------------------------------------------------------------------------
 * Change Log
 * ------------------------------------------------------------------------------------------------------------------------
 * 19-Jul-2018, Sumit Maurya, #CC01, Directory created if it doesnt exists (Done for ComioV5)
 * ------------------------------------------------------------------------------------------------------------------------
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using ZedService;



public partial class DOA_UploadDOANotes : PageBase
{
    string strUploadedFileName = string.Empty;
    UploadFile UploadFile = new UploadFile();
    OpenXMLExcel objexcel = new OpenXMLExcel();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["Detail"] = null;
            uclblMessage.Visible = false;
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataSet objDS = new DataSet();
        try
        {
            uclblMessage.Visible = false;
            PnlErrorinExecl.Visible = false;
            PnlErrorprocedure.Visible = false;
            DataTable dterror = new DataTable();
            DataTable dtResult = new DataTable();
            dterror.Columns.Add("RowNum", typeof(int));
            dterror.Columns.Add("ERROR", typeof(string));
            string strFileName = null;
            string strExtension = null;
            strFileName = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
            string finalnamewithoutext = Path.GetFileNameWithoutExtension(FileUpload1.PostedFile.FileName);
            strExtension = System.IO.Path.GetExtension(strFileName);
            if (strExtension != ".xlsx" && strExtension != ".XLSX")
            {
                uclblMessage.Visible = true;
                uclblMessage.ShowInfo("Please Upload .xlsx File!");
                return;
            }
            /* #CC01 Add Start */
            string FilePath = AppDomain.CurrentDomain.BaseDirectory+"/UploadDownload/UploadPersistent/DOAUploadNotes/";
            System.IO.DirectoryInfo dr = new System.IO.DirectoryInfo(FilePath);
            if (!dr.Exists)
            {
                dr.Create();
            }
            /* #CC01 Add End */

            string SaveLocation = Server.MapPath("~/UploadDownload/UploadPersistent/DOAUploadNotes/" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ffff")+ finalnamewithoutext + strExtension);
            FileUpload1.SaveAs(SaveLocation);
            
          //  OpenXMLExcel objexcel = new OpenXMLExcel();
            DataSet DsExcel = objexcel.ImportExcelFileV2(SaveLocation);
            DsExcel.Tables[0].Columns.Add("RowNum", typeof(Int32));
            DsExcel.Tables[0].Columns.Add("WAGuid", typeof(string));
            int Rwcounter = 1;
            string Waguid = Guid.NewGuid().ToString();
            foreach (DataRow drow in DsExcel.Tables[0].Select())
            {
                drow["RowNum"] = Rwcounter;
                drow["WAGuid"] = Waguid;
                DsExcel.AcceptChanges();
                Rwcounter++;
            }
            DsExcel.AcceptChanges();
            foreach (DataRow Drow in DsExcel.Tables[0].Rows)
            {

                if (Drow["DOACertificateNo"].ToString().Trim() == string.Empty)
                {
                    dterror.Rows.Add(Drow["RowNum"].ToString(), "DOA Certificate No should not be blank");

                }
                if (Drow["Credit Note"].ToString().Trim() == string.Empty)
                {
                    dterror.Rows.Add(Drow["RowNum"].ToString(), "Credit Note should not be blank");
                }

                if (Drow["Account Posting Date"].ToString().Trim() == string.Empty)
                {
                    dterror.Rows.Add(Drow["RowNum"].ToString(), "Account Posting Date should not be blank");
                }
                else if (IsDate(Drow["Account Posting Date"].ToString().Trim()) == false)
                {
                    dterror.Rows.Add(Drow["RowNum"].ToString(), "Enter valid date format for 'Valid From date' as 'YYYY-MM-DD'");
                }
            }
            dterror.AcceptChanges();
            if (dterror.Rows.Count > 0)
            {
                PnlErrorinExecl.Visible = true;
                PnlErrorprocedure.Visible = false;
                grvError.DataSource = dterror;
                grvError.DataBind();
                return;
            }
            DsExcel.AcceptChanges();
            dtResult = DsExcel.Tables[0];
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                SqlTransaction objTrans;
                SqlConnection objcon = new SqlConnection(PageBase.ConStr);
                objcon.Open();
                objTrans = objcon.BeginTransaction();
                if (BulkCopyDumpCreditNotes(dtResult, objTrans, objcon))
                {
                    objTrans.Commit();

                }
                else
                {
                    objTrans.Rollback();
                }
                objcon.Close();
            }
            else
            {
                uclblMessage.Visible = true;
                uclblMessage.ShowError("Can Not upload blank xlsx File.");
                return;
            }
            using (clsDoaReport objreport = new clsDoaReport())
            {
                objreport.LoginUserId = PageBase.UserId;
                objreport.SalesChannelId = PageBase.SalesChanelID;
                objreport.WAGuid = Waguid;
                ds = objreport.uploadDOANote();
                if (ds != null && objreport.OutParam == 1)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            PnlErrorinExecl.Visible = false;
                            PnlErrorprocedure.Visible = true;
                            gvError.DataSource = ds;
                            gvError.DataBind();
                            return;
                        }
                    }
                }
                if (objreport.OutParam == 0)
                {
                    uclblMessage.ShowSuccess("Credit Note uploaded successfully.");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            uclblMessage.Visible = true;
            uclblMessage.ShowError(ex.Message);
        }
    }
    public bool IsDate(string theValue)
    {
        try
        {
            DateTime dateValue;
            if (DateTime.TryParseExact(theValue, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateValue))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        catch
        {
            return false;
        }
    }
    public bool BulkCopyDumpCreditNotes(DataTable dtTempTable, SqlTransaction sqlTran, SqlConnection sqlCon)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlCon, SqlBulkCopyOptions.KeepIdentity, sqlTran))
            {
                bulkCopy.BatchSize = 10000;
                bulkCopy.DestinationTableName = "DumpDoaCreditNotes";
                bulkCopy.BulkCopyTimeout = 300;
                bulkCopy.ColumnMappings.Add("DOACertificateNo", "DoaCertificateNo");
                bulkCopy.ColumnMappings.Add("Credit Note", "CreditNote");
                bulkCopy.ColumnMappings.Add("Account Posting Date", "AccountPostingDate");
                bulkCopy.ColumnMappings.Add("WAGuid", "WAGuid");
                bulkCopy.WriteToServer(dtTempTable);
                return true;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}