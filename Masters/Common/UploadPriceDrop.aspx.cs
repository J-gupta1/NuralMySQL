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

public partial class Masters_Common_UploadPriceDrop : PageBase
{
    ZedService.OpenXMLExcel objexcel = new ZedService.OpenXMLExcel();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindPriceDropType();
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataSet objDS = new DataSet();
        try
        {
            int Id;
            uclblMessage.Visible = false;
            DataSet dterror = new DataSet();
            DataTable dtResult = new DataTable();
            dterror.Tables.Add(new DataTable());
            dterror.Tables[0].Columns.Add("RowNum", typeof(int));
            dterror.Tables[0].Columns.Add("ERROR", typeof(string));
            string strFileName = null;
            string strExtension = null;
            string SerialMinLength = "";
            string SerialMaxLength = "";
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
            DsExcel.Tables[0].Columns.Add("CreatedOn", typeof(DateTime));
            DsExcel.Tables[0].Columns.Add("CreatedBy", typeof(Int32));
            int Rwcounter = 1;
            string Waguid = Guid.NewGuid().ToString();
            int userid = PageBase.UserId;
            string createdOn = System.DateTime.Now.ToString();
            foreach (DataRow drow in DsExcel.Tables[0].Select())
            {
                drow["RowNum"] = Rwcounter;
                drow["WAGuid"] = Waguid;
                drow["CreatedOn"] = createdOn;
                drow["CreatedBy"] = userid;
                DsExcel.AcceptChanges();
                Rwcounter++;
            }
            DsExcel.AcceptChanges();
            using (clsDoaReport objImeiNumber = new clsDoaReport())
            {
                DataSet dsImeilength = new DataSet();
                dsImeilength = objImeiNumber.GetSerialNumberLength();
                if (dsImeilength.Tables[0].Rows.Count > 0)
                {
                    SerialMinLength = dsImeilength.Tables[0].Rows[0]["MinLength"].ToString();
                    SerialMaxLength = dsImeilength.Tables[0].Rows[0]["MaxLength"].ToString();
                }
                else
                {
                    uclblMessage.ShowError("Serial Length Procedure not Found");
                    return;
                }
            }
            foreach (DataRow Drow in DsExcel.Tables[0].Rows)
            {
                if (Drow["PriceDropDate"].ToString().Trim() == string.Empty)
                {
                    dterror.Tables[0].Rows.Add(Drow["RowNum"].ToString(), "Price Drop Date should not be blank");
                }
                if (IsDate(Drow["PriceDropDate"].ToString().Trim()) == false)
                {
                    dterror.Tables[0].Rows.Add(Drow["RowNum"].ToString(), "Invalid Price Drop Date Valid Format shoud be: MM/dd/YYYY.");
                }
                if ((Drow["SerialNumber1"].ToString().Trim() == string.Empty) && (Drow["SerialNumber2"].ToString().Trim() == string.Empty))
                {
                    dterror.Tables[0].Rows.Add(Drow["RowNum"].ToString(), "Serial Number1 should not be blank");
                }
                if (Drow["SerialNumber1"].ToString().Trim() != string.Empty)
                {
                    if (Convert.ToInt32(Drow["SerialNumber1"].ToString().Trim().Length) > Convert.ToInt32(SerialMaxLength) || Convert.ToInt32(Drow["SerialNumber1"].ToString().Trim().Length) < Convert.ToInt32(SerialMinLength))
                    {
                        dterror.Tables[0].Rows.Add(Drow["RowNum"].ToString(), "Serial Number1 should be Min " + SerialMinLength + " and Max " + SerialMaxLength + " characters.");
                    }
                }
                if (Drow["SerialNumber2"].ToString().Trim() != string.Empty)
                {
                    if (Convert.ToInt32(Drow["SerialNumber2"].ToString().Trim().Length) > Convert.ToInt32(SerialMaxLength) || Convert.ToInt32(Drow["SerialNumber2"].ToString().Trim().Length) < Convert.ToInt32(SerialMinLength))
                    {
                        dterror.Tables[0].Rows.Add(Drow["RowNum"].ToString(), "Serial Number2 Should be Min " + SerialMinLength + " and Max " + SerialMaxLength + " characters.");
                    }
                }
                /*if (Drow["Price"].ToString().Trim() == string.Empty)
                {
                    dterror.Tables[0].Rows.Add(Drow["RowNum"].ToString(), "Price should not be blank");
                }*/
                if (Drow["Price"].ToString().Trim() != "" && Drow["Price"].ToString().Trim() != string.Empty)
                {
                    if (int.TryParse(Drow["Price"].ToString().Trim(), out Id))
                    {

                    }
                    else
                    {
                        dterror.Tables[0].Rows.Add(Drow["RowNum"].ToString(), "Invalid Price.");
                    }
                }
                if (Drow["Type"].ToString().Trim() == string.Empty)
                {
                    dterror.Tables[0].Rows.Add(Drow["RowNum"].ToString(), "Type should not be blank");
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
                if (BulkCopyDumpPriceDrop(dtResult, objTrans, objcon))
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
                objreport.WAGuid = Waguid;
                ds = objreport.uploadPriceDrop();
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
                    uclblMessage.ShowSuccess("Price Drop uploaded successfully.");
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
    public bool BulkCopyDumpPriceDrop(DataTable dtTempTable, SqlTransaction sqlTran, SqlConnection sqlCon)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlCon, SqlBulkCopyOptions.KeepIdentity, sqlTran))
            {
                bulkCopy.BatchSize = 10000;
                bulkCopy.DestinationTableName = "PriceDropDump";
                bulkCopy.BulkCopyTimeout = 300;
                bulkCopy.ColumnMappings.Add("PriceDropDate", "PriceDropDate");
                bulkCopy.ColumnMappings.Add("SerialNumber1", "SerialNumber1");
                bulkCopy.ColumnMappings.Add("Price", "Price");
                bulkCopy.ColumnMappings.Add("Type", "Type");
                bulkCopy.ColumnMappings.Add("CreatedOn", "CreatedOn");
                bulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
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
    public bool IsDate(string theValue)
    {
        try
        {
            DateTime dateValue;
            if (DateTime.TryParse(theValue, out dateValue))
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
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (ucFromDate.Date == "" && ucToDate.Date == "" && txtserialnumber.Text.Trim() == "" && ddlType.SelectedValue == "0")
            {
                uclblMessage.Visible = true;
                uclblMessage.ShowWarning("Please enter at least one field.");
                return;
            }
            else
            {

                ExportPriceDropData();
            }

        }
        catch (Exception ex)
        {
            uclblMessage.Visible = true;
            uclblMessage.ShowError(ex.Message);
        }
    }
    private void BindPriceDropType()
    {
        DataTable dtresult = new DataTable();
        try
        {
            dtresult = clsDoaReport.GetEnumbyTableName("XML_Enum", "PriceDropType");
            if (dtresult.Rows.Count > 0)
            {
                ddlType.DataSource = dtresult;
                ddlType.DataTextField = "Description";
                ddlType.DataValueField = "Value";
                ddlType.DataBind();
                ddlType.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddlType.Items.Insert(0, new ListItem("Select", "0"));
            }

        }
        catch (Exception ex)
        {
            uclblMessage.ShowError(ex.Message.ToString());
        }
    }
    private DataSet ExportPriceDropData()
    {
        DataSet ds = new DataSet();
        clsDoaReport objreport = new clsDoaReport();
        try
        {

            if (ucFromDate.Date == "" && ucToDate.Date == "")
            { ;}
            else
            {
                objreport.RequestDateFrom = Convert.ToDateTime(ucFromDate.Date);
                objreport.RequestDateTo = Convert.ToDateTime(ucToDate.Date);
            }
            if (txtserialnumber.Text.Trim() != "" && txtserialnumber.Text.Trim() != null)
            {
                objreport.IMEINumber = txtserialnumber.Text.Trim();
            }
            objreport.PriceDropType = Convert.ToInt16(ddlType.SelectedValue);
            objreport.SalesChannelId = PageBase.SalesChanelID;
            objreport.LoginUserId = PageBase.UserId;
            ds = objreport.ViewPriceDropReports();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {

                string FilenameToexport = "PriceDropReports";
                PageBase.ExportToExecl(ds, FilenameToexport);
            }
            else
            {
                uclblMessage.ShowInfo("NO Record Found.");

            }
            return ds;
        }
        catch (Exception ex)
        {
            return ds;
        }

    }
}