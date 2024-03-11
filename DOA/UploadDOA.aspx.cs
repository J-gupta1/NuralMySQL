/*
 * Changes Log
 * DD-MMM-YYYY, Name, #CCXX, Description.
 * 01-Jun-2018, Sumit Maurya, #CC01, State Details also gets downloaded in ref code(Done For Motorola).
 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.IO;
using System.Text.RegularExpressions;
using ZedService;
using System.Net;
using System.Data.SqlClient;


public partial class DOA_UploadDOA : PageBase
{
    string strUploadedFileName = string.Empty;
    UploadFile UploadFile = new UploadFile();
    OpenXMLExcel objexcel = new OpenXMLExcel();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindSalesChannel();
        }
    }
    protected void DwnldReferenceCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsReferenceCode = new DataSet();
            using (SalesChannelData objsaleschannel = new SalesChannelData())
            {
                objsaleschannel.SalesChannelID = PageBase.SalesChanelID;
                objsaleschannel.UserID = PageBase.UserId;
                dsReferenceCode = objsaleschannel.GetReferenceCodeRetailer();
                if (dsReferenceCode.Tables.Count > 0 && dsReferenceCode != null)
                {
                    String[] strExcelSheetName = { "Retailer", "SalesChannel","State" }; /* #CC01 State Added */
                    string FilenameToexport = "Reference Code List";
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsReferenceCode, FilenameToexport, 3, strExcelSheetName); /* #CC01  replaced 3 from 2 */
                }
                else
                {
                    uclblMessage.ShowInfo(Resources.Messages.NoRecord);
                }
            }

        }
        catch (Exception ex)
        {

            uclblMessage.ShowError(ex.ToString());
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        DataTable dtError = new DataTable();
        DataSet objDS = new DataSet();
        string SerialMinLength = "";
        string SerialMaxLength = "";
        try
        {
            if (ddlSaleschannel.SelectedValue == "0")
            {
                uclblMessage.ShowError("Please Select SalesChannel.");
            }
            else
            {
                byte isSuccess = 1;
                Int16 UploadCheck = 0;
                String RootPath = Server.MapPath("~/");
                UploadFile.RootFolerPath = RootPath;

                UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
                ViewState["TobeUploaded"] = strUploadedFileName;
                if (UploadCheck == 1)
                {

                    UploadFile.UploadedFileName = strUploadedFileName;
                    UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eRetailerUpload;
                    isSuccess = UploadFile.uploadValidExceluploadDOA(ref objDS, "UploadDOA");
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
                    int intError = 0;
                    if (objDS.Tables[0].Columns.Contains("Error"))
                    {
                        intError = 1;
                    }
                    if (!objDS.Tables[0].Columns.Contains("Error"))
                    {
                        objDS.Tables[0].Columns.Add("Error", typeof(string));
                    }
                    foreach (DataRow dr in objDS.Tables[0].Rows)
                    {
                        if (IsDate(dr["DOACertificateDate"].ToString().Trim()) == false)
                        {
                            dr["Error"] = "Invalid DOA Certificate Date";
                            intError = 1;
                        }
                        if (dr["IMEINumber"].ToString().Trim() != string.Empty)
                        {
                            if (Convert.ToInt32(dr["IMEINumber"].ToString().Trim().Length) > Convert.ToInt32(SerialMaxLength) || Convert.ToInt32(dr["IMEINumber"].ToString().Trim().Length) < Convert.ToInt32(SerialMinLength))
                            {
                                dr["Error"] = "IMEI Number should not be greater then 15 character.";
                                intError = 1;
                            }
                        }
                    }

                    if (intError == 1)
                    {
                        isSuccess = 2;
                    }
                    if (intError == 0)
                    {
                        objDS.Tables[0].Columns.Remove("Error");
                    }
                    switch (isSuccess)
                    {
                        case 0:
                            uclblMessage.ShowInfo(UploadFile.Message);
                            break;
                        case 2:
                            uclblMessage.XmlErrorSource = objDS.GetXml();
                            break;
                        case 1:

                            uclblMessage.Visible = false;
                            InsertData(objDS.Tables[0]);
                            break;
                    }
                }
                else
                {
                    uclblMessage.ShowInfo(Resources.Messages.UploadXlxs);
                }
            }
        }
        catch (Exception ex)
        {
            uclblMessage.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindSalesChannel()
    {
        DataSet dsResultSalesChannel = new DataSet();
        try
        {
            using (SalesChannelData objSalesChannel = new SalesChannelData())
            {
                objSalesChannel.SalesChannelID = PageBase.SalesChanelID;
                objSalesChannel.UserID = PageBase.UserId;
                dsResultSalesChannel = objSalesChannel.GetDOASalesChannel();
                if (dsResultSalesChannel != null)
                {
                    if (dsResultSalesChannel.Tables.Count > 0)
                    {
                        ddlSaleschannel.Items.Clear();
                        String[] StrCol = new String[] { "SalesChannelID", "SalesChannelName" };
                        PageBase.DropdownBinding(ref ddlSaleschannel, dsResultSalesChannel.Tables[0], StrCol);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            uclblMessage.ShowError(ex.ToString());
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
    public bool BulkCopyDumpDOAUpload(DataTable dtTempTable)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 10000;
                bulkCopy.DestinationTableName = "DumpDOAUpload";
                bulkCopy.BulkCopyTimeout = 300;
                bulkCopy.ColumnMappings.Add("SKUCode", "SKUCode");
                bulkCopy.ColumnMappings.Add("IMEINumber", "IMEI");
                bulkCopy.ColumnMappings.Add("DOACertificateGenerateState", "DOACertificateGenerateState");
                bulkCopy.ColumnMappings.Add("ServiceCenterCode", "ServiceCenterCode");
                bulkCopy.ColumnMappings.Add("ServiceCenterName", "ServiceCenterName");
                bulkCopy.ColumnMappings.Add("DOACertificateDate", "DOACertificateDate");
                bulkCopy.ColumnMappings.Add("DOACertificateNumber", "DOACertificateNumber");
                bulkCopy.ColumnMappings.Add("RetailerCode", "RetailerCode");
                bulkCopy.ColumnMappings.Add("SalesChannelCode", "RDSCode");
                bulkCopy.ColumnMappings.Add("SessionID", "WAGuid");
                bulkCopy.WriteToServer(dtTempTable);
                return true;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public void InsertData(DataTable dt)
    {
        try
        {
            string guid = Guid.NewGuid().ToString();
            dt.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));

            if (BulkCopyDumpDOAUpload(dt) == true)
            {
                SalesChannelData obj = new SalesChannelData();
                obj.SessionID = Convert.ToString(dt.Rows[0]["SessionID"]);
                obj.UserID = PageBase.UserId;
                obj.SalesChannelID = Convert.ToInt32(ddlSaleschannel.SelectedValue);
                DataSet dsResult = new DataSet();
                dsResult = obj.SaveDOAUpload();
                int result = obj.intOutParam;
                if (result == 0)
                {

                    uclblMessage.ShowSuccess("DOA Data uploaded successfully.");

                }
                else if (result == 1 && dsResult != null && dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {     
                            uclblMessage.XmlErrorSource = dsResult.GetXml();
                            return;
                    }
                }

            }
            else
            {
                uclblMessage.ShowInfo("Error in saving data.");
            }
        }
        catch (Exception ex)
        {
            uclblMessage.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void DwnldSKUCodeTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dtSKuCode = new DataSet();
            using (SalesChannelData objSkucode = new SalesChannelData())
            {
                objSkucode.SalesChannelID = PageBase.SalesChanelID;
                objSkucode.UserID = PageBase.UserId;
                objSkucode.SkuCodeDownload = 1;
                dtSKuCode = objSkucode.GetReferenceCodeRetailer();
                if (dtSKuCode.Tables.Count > 0 && dtSKuCode != null)
                {
                    String[] strExcelSheetName = { "SKU" };
                    string FilenameToexport = "SKU Reference Code List";
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dtSKuCode, FilenameToexport, 1, strExcelSheetName);
                }
                else
                {
                    uclblMessage.ShowInfo(Resources.Messages.NoRecord);
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    private void ExportInExcel(DataSet DsExport, string strFileName)
    {
        try
        {
            if (DsExport != null && DsExport.Tables.Count > 0)
            {
                PageBase.ExportToExeclV2(DsExport, strFileName, DsExport.Tables.Count);
            }
        }
        catch (Exception ex)
        {
            uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
}