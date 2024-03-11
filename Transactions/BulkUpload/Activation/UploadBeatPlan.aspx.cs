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
public partial class Transactions_BulkUpload_Activation_UploadBeatPlan : PageBase
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();
    string strPrimarySessionName = "BeatPlanUploadSession";
    protected void Page_Load(object sender, EventArgs e)
    {

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

            Int16 Upload = 0;
            Upload = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
            ViewState["TobeUploaded"] = strUploadedFileName;
            if (Upload == 1)
            {
                OpenXMLExcel objexcel = new OpenXMLExcel();
                DataSet DsExcel = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);
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
                            objValidateFile.ExcelFileNameInTable = "UploadBeatPlan";

                            objValidateFile.ValidateFile(false, out objDS, out objSL);
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
        Detail.Columns.Add("BeatPlanDate");
        Detail.Columns.Add("BeatPlanForTypeCode");
        Detail.Columns.Add("BeatPlanForCode");
        Detail.Columns.Add("SaleChannelType");
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

                        DataSet DsXML = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());
                        string guid = Guid.NewGuid().ToString();
                        ViewState[strPrimarySessionName] = guid;
                        DsXML.Tables[0].Columns.Add(AddColumn(guid, "TransactionUploadSessionId", typeof(System.String)));
                        DataTable dtUploadQueue = DsXML.Tables[0].Copy();

                        if (dtUploadQueue.Rows.Count > 0)
                        {
                            if (!UserDetailBCP(dtUploadQueue))
                            {
                                ucMsg.ShowError("Error Occured While transferring the data to the server");
                                return;
                            }
                        }
                     
                        using (clsBeatPlan objDetail = new clsBeatPlan())
                        {
                            objDetail.LoginUserId = PageBase.UserId;

                            objDetail.SessionId = guid;
                            DataSet dtInvalidRecordSet = objDetail.Insert();
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
    public bool UserDetailBCP(DataTable dtUpload)
    {
        try
        {

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkUploadBeatPlan";
                bulkCopy.ColumnMappings.Add("BeatPlanDate", "BeatPlanDate");
                bulkCopy.ColumnMappings.Add("BeatPlanForType", "BeatPlanForTypeCode");
                bulkCopy.ColumnMappings.Add("BeatPlanForCode", "BeatPlanForCode");
                bulkCopy.ColumnMappings.Add("SaleChannelType", "SaleChannelType");
                bulkCopy.ColumnMappings.Add("SaleChannelCode", "SaleChannelCode");
                bulkCopy.ColumnMappings.Add("BeatPlanPurpose", "BeatPlanPurpose");
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
    protected void DwnldReferenceCode_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (clsBeatPlan objSalesData = new clsBeatPlan())
            {
                objSalesData.LoginUserId = PageBase.UserId;
                dsTemplateCode = objSalesData.GetAllTemplateData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "Reference Code List";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "ASM-TSM-FOSDetails", "SalesChannel-RetailerDetails"};
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 2);
                    if (dsTemplateCode.Tables.Count > 2)
                        dsTemplateCode.Tables.RemoveAt(2);
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
    public void ChangedExcelSheetNames(ref DataSet dsExcel, string[] strExcelSheetName, Int16 intSheetCount)
    {
        Int16 index;
        for (index = 0; index < intSheetCount; index++)
            dsExcel.Tables[index].TableName = strExcelSheetName[index];
        dsExcel.AcceptChanges();

    }
}