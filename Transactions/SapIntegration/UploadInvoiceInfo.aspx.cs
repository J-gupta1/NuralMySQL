using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using ExportExcelOpenXML;
using System.Text;
using BusinessLogics;
using System.Collections;
using System.Data.SqlClient;
using ZedService;
using DevExpress.Web.ASPxUploadControl;
using DevExpress.Web.ASPxClasses.Internal;
using System.IO;

public partial class Transactions_SapIntegration_UploadInvoiceInfo : PageBase
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strOriginalFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();
    string strPrimarySessionName = "PSIUploadSession";
    DataTable dtImageDataActual;
    DataTable dtImageData;
    public string XMLImage
    {
        get;
        set;

    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void DwnldReferenceCode_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (ClsSAPIntegrationDNPSI objSalesData = new ClsSAPIntegrationDNPSI())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                dsTemplateCode = objSalesData.GetAllDNDetailData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "DNDetail", "SKUDetail" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 2);
                    if (dsTemplateCode.Tables.Count > 4)
                        dsTemplateCode.Tables.RemoveAt(4);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 2, strExcelSheetName);
                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed)
            {
                return;

            }
            DataTable dtErrorTable = GetBlankTableError();
            DataTable dtError = new DataTable();
            HttpContext.Current.Session["PkeyColumns"] = null;
            string strKey = string.Empty;
            hlnkInvalid.Visible = false;
            strOriginalFileName = FileUpload1.PostedFile.FileName;

            Int16 Upload = 0;
            Upload = UploadFile.IsExcelFilePSISAPIntegration(FileUpload1, ref strUploadedFileName, "InvoiceInfoUpload");
            ViewState["TobeUploaded"] = strUploadedFileName;
            if (Upload == 1)
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();

                DataSet DsExcel = objexcel.ImportExcelFileV2(PageBase.strExcelBulkUploadPSIInvoiceInfoPath + strUploadedFileName);
                if (DsExcel != null && DsExcel.Tables.Count > 0 && DsExcel.Tables[0].Rows.Count > 0)
                {

                    if (DsExcel.Tables[0].Rows.Count > Convert.ToInt32(PageBase.ValidExcelRows))
                        ucMsg.ShowInfo("Limit Crossed");
                    else
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile();
                        {
                            DataSet objDS = DsExcel;
                            DataTable dt1 = DsExcel.Tables[0];
                            SortedList objSL = new SortedList();
                            SortedList objSLCorrData = new SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "UploadSAPInvoiceInfo";

                            objValidateFile.ValidatePSIFile(false, out objDS, out objSL, "InvoiceInfo");
                            if (objValidateFile.Message != null && objValidateFile.Message.Trim() != "")
                                ucMsg.ShowInfo(objValidateFile.Message);
                            else
                            {
                                ucMsg.Visible = false;
                                bool blnIsUpload = true;
                                if (objSL.Count >= 1 && objSL.Keys.Count >= 1)
                                {
                                    objDS.Tables["DtExcelSheet"].Columns.Add(new DataColumn("ReasonForInvalid"));
                                    IDictionaryEnumerator objIDicEnum = objSL.GetEnumerator();
                                    while (objIDicEnum.MoveNext())
                                    {
                                        string[] strpkeyColumnName = Convert.ToString(HttpContext.Current.Session["PkeyColumns"]).Split(',');
                                        if (HttpContext.Current.Session["PkeyColumns"] != null)
                                        {
                                            for (int i = 0; i <= objDS.Tables["DtExcelSheet"].Rows.Count - 1; i++)
                                            {
                                                strKey = string.Empty;
                                                for (int j = 0; j <= strpkeyColumnName.Length - 1; j++)
                                                {
                                                    if (strKey == string.Empty)
                                                        strKey = objDS.Tables["DtExcelSheet"].Rows[i][strpkeyColumnName[j]].ToString();
                                                    else
                                                        strKey = strKey + objDS.Tables["DtExcelSheet"].Rows[i][strpkeyColumnName[j]].ToString();
                                                }
                                                if (strKey == objIDicEnum.Key.ToString())
                                                {
                                                    objDS.Tables["DtExcelSheet"].Rows[i]["ReasonForInvalid"] = objIDicEnum.Value.ToString();
                                                }
                                            }
                                        }
                                    }
                                    objDS.Tables[0].AcceptChanges();
                                    if (objDS.Tables["DtExcelSheet"] != null && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        hlnkInvalid.Visible = true;
                                        dsErrorProne.Merge(objDS.Tables["DtExcelSheet"]);
                                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                        ExportInExcel(dsErrorProne, strFileName);
                                        hlnkInvalid.Text = "Invalid Data";
                                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                        blnIsUpload = false;
                                    }
                                    blnIsUpload = false;
                                }
                                if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                {
                                    int counter = 0;
                                    if (!objDS.Tables[0].Columns.Contains("ReasonForInvalid"))
                                        objDS.Tables[0].Columns.Add(new DataColumn("ReasonForInvalid"));

                                    if (counter > 0)
                                    {
                                        ucMsg.ShowInfo("Invalid Records");
                                        dsErrorProne.Merge(dtErrorTable);
                                        blnIsUpload = false;
                                    }
                                    else
                                    {
                                        objDS.Tables[0].Columns.Remove("ReasonForInvalid");
                                    }
                                }

                                if (objDS.Tables["DtDuplicateRecord"] != null && objDS.Tables["DtDuplicateRecord"].Rows.Count > 0)
                                {

                                    dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                                    blnIsUpload = false;
                                }
                                if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                                {

                                    dsErrorProne.Merge(objDS.Tables["DtBlankData"]);

                                    blnIsUpload = false;
                                }
                                if (blnIsUpload)
                                {
                                    if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        InsertData(objDS.Tables[0]);
                                    }
                                    else
                                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                                }
                                else
                                {
                                    hlnkInvalid.Visible = true;
                                    string strFileName = "InvalidData" + DateTime.Now.Ticks;
                                    ExportInExcel(dsErrorProne, strFileName);
                                    hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    hlnkInvalid.Text = "Invalid Data";
                                }



                            }
                        }
                    }
                }
                else
                {
                    ucMsg.ShowInfo("File is empty! Some Mandatory columns has no required data!");
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("DNNumber");
        Detail.Columns.Add("InvoiceNumber");
        Detail.Columns.Add("InvoiceDate");
        Detail.Columns.Add("SKUcode");
        Detail.Columns.Add("UOM");
        Detail.Columns.Add("Price");
        Detail.Columns.Add("Discount");
        Detail.Columns.Add("Amount");
        Detail.Columns.Add("CGSTRate");
        Detail.Columns.Add("SGSTRate");
        Detail.Columns.Add("IGSTRate");
        Detail.Columns.Add("UTGSTRate");
        Detail.Columns.Add("ReasonForInvalid");
        return Detail;
    }
    private void InsertData(DataTable objdt)
    {
        if (objdt != null)
        {
            DataSet objds = new DataSet();

            if (objdt != null && objdt.Rows.Count > 0)
            {

                try
                {
                    if (ViewState["TobeUploaded"] != null)
                    {
                        OpenXMLExcel objexcel = new OpenXMLExcel();
                        string strUploadedFileNameFromViewState = ViewState["TobeUploaded"].ToString();
                        DataSet DsXML = objexcel.ImportExcelFileV2(PageBase.strExcelBulkUploadPSIInvoiceInfoPath + ViewState["TobeUploaded"].ToString());
                        string guid = Guid.NewGuid().ToString();
                        ViewState[strPrimarySessionName] = guid;
                        DsXML.Tables[0].Columns.Add(AddColumn(guid, "TransactionUploadSessionId", typeof(System.String)));
                        DataTable dtUploadQueue = DsXML.Tables[0].Copy();

                        if (dtUploadQueue.Rows.Count > 0)
                        {
                            if (!PSIInvoiceInfoDetailBCP(dtUploadQueue))
                            {
                                ucMsg.ShowError("Error Occured While transferring the data to the server");
                                return;
                            }
                        }

                        using (ClsSAPIntegrationDNPSI objDetail = new ClsSAPIntegrationDNPSI())
                        {
                            objDetail.LoginUserId = PageBase.UserId;
                            objDetail.SessionId = guid;
                            objDetail.strOriginalFileName = strOriginalFileName;
                            objDetail.strUploadedFileName = strUploadedFileName;
                            DataSet dtInvalidRecordSet = objDetail.InsertInvoiceInfo();
                            Int32 result = objDetail.OutParam;
                            if (result == 0)
                            {
                                ucMsg.ShowSuccess("Data Uploaded Successfully");
                                return;
                            }
                            if (result == 1 && objDetail.OutError != "")
                            {
                                ucMsg.ShowError(objDetail.OutError);
                                return;
                            }
                            if (result == 1 && dtInvalidRecordSet != null && dtInvalidRecordSet.Tables[0].Rows.Count > 0)
                            {
                                DataSet ds = new DataSet();
                                string strFileName = "Invalid Data" + DateTime.Now.Ticks;
                                ExportInExcel(dtInvalidRecordSet, strFileName);
                                hlnkInvalid.Visible = true;
                                hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                hlnkInvalid.Text = "Invalid Data";
                                ucMsg.Visible = true;
                                ucMsg.ShowInfo("Please click on Invalid data to check the error obtained");

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ucMsg.ShowError(ex.Message);
                }
            }
            else
            {
                ucMsg.ShowInfo("File is empty!");
            }
        }

    }
    public bool PSIInvoiceInfoDetailBCP(DataTable dtUpload)
    {
        try
        {

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkUploadSAPInvoiceDetail";
                bulkCopy.ColumnMappings.Add("DNNumber", "DNNumber");
                bulkCopy.ColumnMappings.Add("InvoiceNumber", "InvoiceNumber");
                bulkCopy.ColumnMappings.Add("InvoiceDate", "InvoiceDate");
                bulkCopy.ColumnMappings.Add("SKUcode", "SKUcode");
                bulkCopy.ColumnMappings.Add("UOM", "UOM");
                bulkCopy.ColumnMappings.Add("Price", "Price");
                bulkCopy.ColumnMappings.Add("Discount", "Discount");
                bulkCopy.ColumnMappings.Add("Amount", "Amount");
                bulkCopy.ColumnMappings.Add("CGSTRate", "CGSTRate");
                bulkCopy.ColumnMappings.Add("SGSTRate", "SGSTRate");
                bulkCopy.ColumnMappings.Add("IGSTRate", "IGSTRate");
                bulkCopy.ColumnMappings.Add("UTGSTRate", "UTGSTRate");
                bulkCopy.ColumnMappings.Add("TransactionUploadSessionId", "TransactionUploadSessionId");
                bulkCopy.WriteToServer(dtUpload);
                return true;
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
            return false;
        }
    }
    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
            PageBase.ExportToExeclV2(DsError, strFileName, DsError.Tables.Count);
        }
    }
    protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
    {
        try
        {
            e.CallbackData = SavePostedFiles(e.UploadedFile);
        }
        catch (Exception ex)
        {
            e.IsValid = false;
            e.ErrorText = ex.Message;
        }
    }
    string SavePostedFiles(UploadedFile uploadedFile)
    {


        string strFilePath;

        strFilePath = ZedService.Utility.ZedServiceUtil.GetUploadFilePathV2(Convert.ToDateTime(System.DateTime.Now), uploadedFile.FileName);
        if (!uploadedFile.IsValid)
            return string.Empty;
        FileInfo fileInfo = new FileInfo(uploadedFile.FileName);
        string strTicks = System.DateTime.Now.Ticks.ToString();

        string strFileUploadedName = fileInfo.Name;

        string strTempPath = Server.MapPath("~/Excel/Upload/SAPIntegration/UploadedPdfInvoice/");
        if (!Directory.Exists(Server.MapPath(strFilePath)))
            Directory.CreateDirectory(Server.MapPath(strFilePath));


        uploadedFile.SaveAs(strTempPath + strFileUploadedName);
        if (Session["Table"] == null)
        {
            string removeextention=strFileUploadedName.Replace(".pdf","");
            dtImageDataActual = new DataTable();
            dtImageDataActual = CreateImageDataTable();
            DataRow dr = dtImageDataActual.NewRow();
            dr["FileLocation"] = strFilePath.Replace("../../", "/");
            dr["ImageTypeId"] = 14;
            dr["ImageRelativePath"] = strTempPath + strFileUploadedName;
            dr["TempFileLocation"] = strTempPath + strFileUploadedName;
            dr["ImageTypeName"] = "InvoicePdf";
            dr["ImageName"] = removeextention;
            dtImageDataActual.Rows.Add(dr);
            dtImageDataActual.AcceptChanges();
            Session["Table"] = dtImageDataActual;

        }
        else
        {
            string removeextention = strFileUploadedName.Replace(".pdf", "");
            dtImageDataActual = (DataTable)Session["Table"];
            DataRow dr = dtImageDataActual.NewRow();

            dr["FileLocation"] = strFilePath.Replace("../../", "/");
            dr["ImageTypeId"] = 14;
            dr["ImageRelativePath"] = strTempPath + strFileUploadedName;
            dr["TempFileLocation"] = strTempPath + strFileUploadedName;
            dr["ImageTypeName"] = "InvoicePdf";
            dr["ImageName"] = removeextention;
            dtImageDataActual.Rows.Add(dr);
            dtImageDataActual.AcceptChanges();
            Session["Table"] = dtImageDataActual;


        }

        string fileLabel = fileInfo.Name;
        string fileLength = uploadedFile.FileBytes.Length / 1024 + "K";

        return string.Format("{0} ({1})|{2}", fileLabel, fileLength, VirtualPath + "/Excel/Upload/SAPIntegration/UploadedPdfInvoice/" + strFileUploadedName);

    }
    DataTable CreateImageDataTable()
    {


        dtImageData = new DataTable();
        DataColumn dc = new DataColumn("CtrlID");
        dc.DataType = System.Type.GetType("System.Int32");
        dc.AutoIncrement = true;
        dc.AutoIncrementSeed = 1;
        dtImageData.Columns.Add(dc);
        dtImageData.Columns.Add("FileLocation", typeof(string));
        dtImageData.Columns.Add("TempFileLocation", typeof(string));
        dtImageData.Columns.Add("ImageTypeId", typeof(Int16));
        dtImageData.Columns.Add("ImageRelativePath", typeof(string));
        dtImageData.Columns.Add("ImageName", typeof(string));

        dtImageData.Columns.Add("ImageTypeName", typeof(string));
        return dtImageData;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataTable FinalFileData = new DataTable();
       // DataSet ds = new DataSet();
        if (Session["Table"] != null)
        {
            FinalFileData = (DataTable)Session["Table"];
            FinalFileData.TableName = "Table1";
            dt = FinalFileData.Copy();
           // ds.Tables.Add(dt);
        }
        else
        {
            ucMsg.Visible = true;
            ucMsg.ShowInfo("Please Upload File!");
        }
       // XMLImage = ds.GetXml();
        using (clsEnquiryDetail ObjDetail = new clsEnquiryDetail())
        {
            if (dt.Rows.Count == 0)
            {
                ucMsg.Visible = true;
                ucMsg.ShowInfo("Please Upload File!");
            }
            else
            {
                using (ClsSAPIntegrationDNPSI objuploadfile = new ClsSAPIntegrationDNPSI())
                {
                    int resultimg = 1;
                    objuploadfile.SalesChannelId = PageBase.SalesChanelID;
                    objuploadfile.LoginUserId = PageBase.UserId;
                    objuploadfile.ReferenceType_id = 0;
                    objuploadfile.UploadImageData = dt;
                    resultimg = objuploadfile.SaveImgSaperateByProcess();
                    if (resultimg == 0)
                    {
                        PageBase objbase = new PageBase();
                        objbase.MoveFileFromTemp(FinalFileData);
                        ucMsg.Visible = true;
                        ucMsg.ShowSuccess("File Uploaded Successfully.!");
                        Session["FinalFileData"] = null;
                    }
                }
            }
            Session["Table"] = null;
        }
    }
}