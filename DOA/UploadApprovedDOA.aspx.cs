using BussinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZedService;

public partial class DOA_UploadApprovedDOA : PageBase
{
    OpenXMLExcel objexcel = new OpenXMLExcel();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataSet objDS = new DataSet();
        try
        {
            uclblMessage.Visible = false;
            DataSet dterror = new DataSet();
            DataTable dtResult = new DataTable();
            dterror.Tables.Add(new DataTable());
            dterror.Tables[0].Columns.Add("RowNum", typeof(int));
            dterror.Tables[0].Columns.Add("ERROR", typeof(string));
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
            string SaveLocation = Server.MapPath("~/UploadDownload/UploadPersistent/" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ffff") + finalnamewithoutext + strExtension);
            FileUpload1.SaveAs(SaveLocation);
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
            var duplicates = DsExcel.Tables[0].AsEnumerable()
    .GroupBy(dr => dr.Field<string>("DOACertificateNumber"))
    .Where(g => g.Count() > 1)
    .Select(g => g.First())
    .ToList();
            foreach (var dupcerificatenumber in duplicates)
            {
                dterror.Tables[0].Rows.Add(dupcerificatenumber["RowNum"].ToString(), "Duplicates DOA Certificate No in Excel Sheet.");
            }
            foreach (DataRow Drow in DsExcel.Tables[0].Rows)
            {

                if (Drow["DOACertificateNumber"].ToString().Trim() == string.Empty)
                {
                    dterror.Tables[0].Rows.Add(Drow["RowNum"].ToString(), "DOA Certificate No should not be blank");

                }
                if (Drow["ApproveStatus"].ToString().Trim() == string.Empty)
                {
                    dterror.Tables[0].Rows.Add(Drow["RowNum"].ToString(), "Approve Status should not be blank");
                }

                if (Drow["CreditNoteNumber"].ToString().Trim() == string.Empty)
                {
                    dterror.Tables[0].Rows.Add(Drow["RowNum"].ToString(), "Credit Note Number should not be blank");
                }
                if (Drow["ApproveStatus"].ToString().Trim() != "" || Drow["ApproveStatus"].ToString().Trim() != string.Empty)
                {
                    if (Drow["ApproveStatus"].ToString().Trim() == "Approved" || Drow["ApproveStatus"].ToString().Trim() == "Reject")
                    {

                    }
                    else
                    {
                        dterror.Tables[0].Rows.Add(Drow["RowNum"].ToString(), "Please Enter valid Approvestatus.");
                    }

                    if (Drow["ApproveStatus"].ToString().Trim() == "Reject")
                    {
                        if (Drow["Remarks"].ToString().Trim() == string.Empty)
                        {
                            dterror.Tables[0].Rows.Add(Drow["RowNum"].ToString(), "Remarks should not be blank");
                        }
                    }
                }

            }
            dterror.AcceptChanges();
            if (dterror.Tables[0].Rows.Count > 0)
            {
                uclblMessage.XmlErrorSource = dterror.GetXml();
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
                if (BulkCopyDumpApprovedDOA(dtResult, objTrans, objcon))
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
                ds = objreport.uploadDOAApprovedReject();
                if (ds != null && objreport.OutParam == 1)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            uclblMessage.XmlErrorSource = ds.GetXml();
                            return;
                        }
                    }
                }
                if (objreport.OutParam == 0)
                {
                    uclblMessage.ShowSuccess("Approved/Reject DOA uploaded successfully.");
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
    public bool BulkCopyDumpApprovedDOA(DataTable dtTempTable, SqlTransaction sqlTran, SqlConnection sqlCon)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlCon, SqlBulkCopyOptions.KeepIdentity, sqlTran))
            {
                bulkCopy.BatchSize = 10000;
                bulkCopy.DestinationTableName = "DumpDOAUploadApproveReject";
                bulkCopy.BulkCopyTimeout = 300;
                bulkCopy.ColumnMappings.Add("DOACertificateNumber", "DOACertificateNumber");
                bulkCopy.ColumnMappings.Add("ApproveStatus", "ApproveStatus");
                bulkCopy.ColumnMappings.Add("CreditNoteNumber", "CreditNoteNumber");
                bulkCopy.ColumnMappings.Add("Remarks", "Remarks");
                bulkCopy.ColumnMappings.Add("WAGuid", "SessionId");
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