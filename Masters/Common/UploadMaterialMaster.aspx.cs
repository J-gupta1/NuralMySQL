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
using MySql.Data.MySqlClient;
using ZedService;
using System.IO;

public partial class Masters_Common_UploadMaterialMaster : PageBase
{
    DataTable skuinfo;
    int mode3 = 0;
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();
    string strPrimarySessionName = "MaterialUploadSession";
    string strPrimarySessionNameupdate = "MaterialUploadSessionUpdate";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fillcombos();
            databind();
            if (Rbtdownloadtemplate.SelectedValue == "1")
            {
                ForSaveTemplateheading.Visible = true;
                ForSaveTemplatedownload.Visible = true;
                ReferenceIdForsaveheading.Visible = true;
                ReferenceIdForsave.Visible = true;
            }
            else
            {
                ForSaveTemplateheading.Visible = false;
                ForSaveTemplatedownload.Visible = false;
                ReferenceIdForsaveheading.Visible = false;
                ReferenceIdForsave.Visible = false;
            }
        }
    }
    protected void DwnldTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 1;
                dsTemplateCode = objSalesData.GetMaterialMasterTemplate();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "SaveMaterialMaster";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SaveMaterialMaster" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 1);
                    if (dsTemplateCode.Tables.Count > 1)
                        dsTemplateCode.Tables.RemoveAt(1);
                    for (int i = dsTemplateCode.Tables[0].Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dsTemplateCode.Tables[0].Rows[i];

                        dsTemplateCode.Tables[0].Rows.Remove(dr);

                    }

                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 1, strExcelSheetName);
                }
                else
                {
                    ucMsg.ShowInfo(objSalesData.OutError.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }
    protected void UpdateTemplateFile_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 2;
                dsTemplateCode = objSalesData.GetMaterialMasterTemplate();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "UpdateMaterialMasterTemplate";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "UpdateMaterialMasterTemplate" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 1);
                    if (dsTemplateCode.Tables.Count > 1)
                        dsTemplateCode.Tables.RemoveAt(1);


                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 1, strExcelSheetName);
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
                    {
                        ucMsg.ShowInfo("Limit Crossed");
                    }
                    else if (DsExcel.Tables[0].Columns.Contains("SKUID") && Rbtdownloadtemplate.SelectedValue == "1")
                    {
                        ucMsg.ShowInfo("You are uploading an update template, please upload save template.");
                        return;
                    }
                    else if (!DsExcel.Tables[0].Columns.Contains("SKUID") && Rbtdownloadtemplate.SelectedValue == "2")
                    {
                        ucMsg.ShowInfo("You are uploading an save template, please upload update template.");
                        return;
                    }


                    if (Rbtdownloadtemplate.SelectedValue == "1")
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile();
                        {
                            DataSet objDS = DsExcel;
                            DataTable dt1 = DsExcel.Tables[0];
                            SortedList objSL = new SortedList();
                            SortedList objSLCorrData = new SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "AllMaterialMaster";
                            objValidateFile.CompanyId = PageBase.ClientId;

                            objValidateFile.ValidateFileWithCompanyId(false, out objDS, out objSL);
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
                                        dsErrorProne.Merge(objDS.Tables[0]);
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
                    else if (Rbtdownloadtemplate.SelectedValue == "2")
                    {
                        ValidateUploadFile objValidateFile = new ValidateUploadFile();
                        {
                            DataSet objDS = DsExcel;
                            DataTable dt1 = DsExcel.Tables[0];
                            SortedList objSL = new SortedList();
                            SortedList objSLCorrData = new SortedList();
                            objValidateFile.UploadedFileName = strUploadedFileName;
                            objValidateFile.ExcelFileNameInTable = "AllMaterialMasterUpdate";
                            objValidateFile.CompanyId = PageBase.ClientId;


                            objValidateFile.ValidateFileWithCompanyId(false, out objDS, out objSL);
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
                                        dsErrorProne.Merge(objDS.Tables[0]);
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
            else
            {
                ucMsg.Visible = true;
                ucMsg.ShowInfo("Please Browse File !");
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
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
                    if (Rbtdownloadtemplate.SelectedValue == "1")
                    {
                        if (ViewState["TobeUploaded"] != null)
                        {
                            OpenXMLExcel objexcel = new OpenXMLExcel();
                            string strUploadedFileNameFromViewState = ViewState["TobeUploaded"].ToString();

                            DataSet DsXML = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());
                            string guid = Guid.NewGuid().ToString();
                            ViewState[strPrimarySessionName] = guid;
                            string Radiobuttonid = Rbtdownloadtemplate.SelectedValue;
                            DsXML.Tables[0].Columns.Add(AddColumn(guid, "TransactionUploadSessionId", typeof(System.String)));
                            DsXML.Tables[0].Columns.Add(AddColumn(Radiobuttonid, "ActionType", typeof(System.String)));
                            DataTable dtUploadQueue = DsXML.Tables[0].Copy();
                            if (dtUploadQueue.Rows.Count > 0)
                            {
                                if (!UserDetailBCP(dtUploadQueue))
                                {
                                    ucMsg.ShowError("Error Occured While transferring the data to the server");
                                    return;
                                }
                            }

                            using (CommonMaster objDetail = new CommonMaster())
                            {
                                objDetail.UserID = PageBase.UserId;
                                objDetail.SessionId = guid;
                                objDetail.CompanyID = PageBase.ClientId;
                                objDetail.TemplateType = 1;
                                DataSet dtInvalidRecordSet = objDetail.UploadMaterialMasterForSaveUpdate();
                                Int32 result = objDetail.Out_Param;
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
                    else if (Rbtdownloadtemplate.SelectedValue == "2")
                    {
                        if (ViewState["TobeUploaded"] != null)
                        {
                            OpenXMLExcel objexcel = new OpenXMLExcel();
                            string strUploadedFileNameFromViewState = ViewState["TobeUploaded"].ToString();

                            DataSet DsXML = objexcel.ImportExcelFileV2(PageBase.strExcelPhysicalUploadPathSB + ViewState["TobeUploaded"].ToString());
                            string guid = Guid.NewGuid().ToString();
                            ViewState[strPrimarySessionNameupdate] = guid;
                            string Radiobuttonid = Rbtdownloadtemplate.SelectedValue;
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

                            using (CommonMaster objDetail = new CommonMaster())
                            {
                                objDetail.UserID = PageBase.UserId;
                                objDetail.SessionId = guid;
                                objDetail.CompanyID = PageBase.ClientId;
                                objDetail.TemplateType = 2;
                                DataSet dtInvalidRecordSet = objDetail.UploadMaterialMasterForSaveUpdate();
                                Int32 result = objDetail.Out_Param;
                                if (result == 0)
                                {
                                    ucMsg.ShowSuccess("Data Update Successfully");
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


    #region
    public bool UserDetailBCP(DataTable dtUpload)
    {
        try
        {
            string connectMySQL = PageBase.ConStr;
            string virtualFilePath = "~/Excel/Upload/UploadExcelFiles/" + DateTime.Now.Ticks.ToString() + ".csv";

            //Create directory if not exist... Make sure directory has required rights..
            if (!Directory.Exists(Server.MapPath("~/Excel/Upload/UploadExcelFiles/")))
                Directory.CreateDirectory(Server.MapPath("~/Excel/Upload/UploadExcelFiles/"));

            //If file does not exist then create it and write data into it..
            if (!File.Exists(Server.MapPath(virtualFilePath)))
            {
                FileStream fs = new FileStream(Server.MapPath(virtualFilePath), FileMode.Create, FileAccess.Write);
                fs.Close();
                fs.Dispose();
            }
            if (dtUpload.Columns.Contains("SrNo"))
                dtUpload.Columns.Remove("SrNo");



            //Generate csv file from where data read
                    CreateCSVfile(dtUpload, Server.MapPath(virtualFilePath));

            using (MySqlConnection cn1 = new MySqlConnection(connectMySQL))
            {
                cn1.Open();
                MySqlBulkLoader bulkCopy = new MySqlBulkLoader(cn1);
                bulkCopy.TableName = "BulkUploadAllMaster"; //Create ProductOrder table into MYSQL database...
                bulkCopy.FieldTerminator = ",";
                bulkCopy.LineTerminator = "\r\n";
                bulkCopy.FileName = Server.MapPath(virtualFilePath);
                bulkCopy.NumberOfLinesToSkip = 0;
                if (dtUpload.Columns.Contains("SKUID") && dtUpload.Columns.Contains("ActionType"))
                {
                    bulkCopy.Columns.AddRange(new[] {
                        "SKUID"
                        ,"BrandCode",
                        "BrandName",
                        "ProductCategoryCode",
                        "ProductCategory",
                        "ProductSubCategoryCode",
                        "ProductSubCategory",
                        "ModelCode",
                        "ModelName",
                        "Color",
                        "SKUCode",
                        "SKUName",
                        "IsSerialBatch",
                        "ActionType",
                        "EanCode",
                        "TransactionUploadSessionId"
                });
                }
                else
                {
                    bulkCopy.Columns.AddRange(new[] {
                        "BrandCode",
                        "BrandName",
                        "ProductCategoryCode",
                        "ProductCategory",
                        "ProductSubCategoryCode",
                        "ProductSubCategory",
                        "ModelCode",
                        "ModelName",
                        "Color",
                        "SKUCode",
                        "SKUName",
                        "IsSerialBatch",
                        "EanCode",
                        "TransactionUploadSessionId","ActionType"
                    });
                }

                //});

                bulkCopy.Load();



                //Once data write into db then delete file..
                try
                {
                    File.Delete(Server.MapPath(virtualFilePath));
                }
                catch (Exception ex)
                {
                    ucMsg.ShowError(ex.Message);
                    return false;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
            return false;
        }
    }

    #endregion
    /*
    public bool UserDetailBCP(DataTable dtUpload)
    {
        try
        {

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkUploadAllMaster";
                if (dtUpload.Columns.Contains("BrandCode"))
                {
                    bulkCopy.ColumnMappings.Add("BrandCode", "BrandCode");
                }
                if (dtUpload.Columns.Contains("BrandName"))
                {
                    bulkCopy.ColumnMappings.Add("BrandName", "BrandName");
                }
                if (dtUpload.Columns.Contains("ProductCategoryCode"))
                {
                    bulkCopy.ColumnMappings.Add("ProductCategoryCode", "ProductCategoryCode");
                }
                if (dtUpload.Columns.Contains("ProductCategory"))
                {
                    bulkCopy.ColumnMappings.Add("ProductCategory", "ProductCategory");
                }
                if (dtUpload.Columns.Contains("ProductSubCategoryCode"))
                {
                    bulkCopy.ColumnMappings.Add("ProductSubCategoryCode", "ProductSubCategoryCode");
                }
                if (dtUpload.Columns.Contains("ProductSubCategory"))
                {
                    bulkCopy.ColumnMappings.Add("ProductSubCategory", "ProductSubCategory");
                }
                if (dtUpload.Columns.Contains("ModelCode"))
                {
                    bulkCopy.ColumnMappings.Add("ModelCode", "ModelCode");
                }
                if (dtUpload.Columns.Contains("ModelName"))
                {
                    bulkCopy.ColumnMappings.Add("ModelName", "ModelName");
                }
                if (dtUpload.Columns.Contains("Color"))
                {
                    bulkCopy.ColumnMappings.Add("Color", "Color");
                }
                if (dtUpload.Columns.Contains("SKUCode"))
                {
                    bulkCopy.ColumnMappings.Add("SKUCode", "SKUCode");
                }
                if (dtUpload.Columns.Contains("SKUName"))
                {
                    bulkCopy.ColumnMappings.Add("SKUName", "SKUName");
                }
                if (dtUpload.Columns.Contains("IsSerialBatch"))
                {
                    bulkCopy.ColumnMappings.Add("IsSerialBatch", "IsSerialBatch");
                }
                if (dtUpload.Columns.Contains("SKUID"))
                {
                    bulkCopy.ColumnMappings.Add("SKUID", "SKUID");
                }
                if (dtUpload.Columns.Contains("ActionType"))
                {
                    bulkCopy.ColumnMappings.Add("ActionType", "ActionType");
                }
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
    */
    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
            PageBase.ExportToExeclV2(DsError, strFileName, DsError.Tables.Count);
        }
    }

    protected void Rbtdownloadtemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        ucMsg.Visible = false;
        if (Rbtdownloadtemplate.SelectedValue == "1")
        {
            ForSaveTemplateheading.Visible = true;
            ForSaveTemplatedownload.Visible = true;
            ReferenceIdForsaveheading.Visible = true;
            ReferenceIdForsave.Visible = true;
        }
        else
        {
            ForSaveTemplateheading.Visible = false;
            ForSaveTemplatedownload.Visible = false;
            ReferenceIdForsaveheading.Visible = false;
            ReferenceIdForsave.Visible = false;
        }
        if (Rbtdownloadtemplate.SelectedValue == "2")
        {
            ForUploadTemplateheading.Visible = true;
            ForUpdateTemplatedownload.Visible = true;
            ReferenceIdForupdateheading.Visible = true;
            ReferenceIdForupdate.Visible = true;
        }
        else
        {
            ForUploadTemplateheading.Visible = false;
            ForUpdateTemplatedownload.Visible = false;
            ReferenceIdForupdateheading.Visible = false;
            ReferenceIdForupdate.Visible = false;
        }
    }
    protected void DownloadReferenceCodeForUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 2;
                dsTemplateCode = objSalesData.GetMaterialMasterReferenceData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ReferenceCode";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SKUDetail" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 1);
                    if (dsTemplateCode.Tables.Count > 1)
                        dsTemplateCode.Tables.RemoveAt(1);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 1, strExcelSheetName);
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
    protected void DownloadReferenceCodeForSave_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 1;
                dsTemplateCode = objSalesData.GetMaterialMasterReferenceData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ReferenceCode";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "Brand", "ProductCategory", "ProductSubCategory", "Color" };
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 4);
                    if (dsTemplateCode.Tables.Count > 4)
                        dsTemplateCode.Tables.RemoveAt(4);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 4, strExcelSheetName);
                }
                else
                {
                    ucMsg.ShowInfo(objSalesData.OutError);
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }
    protected void exportToExel_Click(object sender, EventArgs e)
    {
        mode3 = 3;
        databind();
        mode3 = 0;
        DataTable dt = skuinfo;

        if (PageBase.SHOWCARTONSIZE == 0)
        {
            string[] DsCol = new string[] { "SKU Code", "SKU Name", "SKU Description", "Product Category Name", "Product Name", "Color Name", "Brand Name", "Model Name", "Attribute1", "Attribute2", "Current Status", "keywords"};/*#CC05 Added ExpiryDateStatus*/
            dt = dt.DefaultView.ToTable(true, DsCol);
        }
        else
        {
            string[] DsCol = new string[] { "SKU Code", "SKU Name", "SKU Description", "Product Category Name", "Product Name", "Color Name", "Brand Name", "Model Name", "Attribute1", "Attribute2", "Carton Size", "Current Status", "keywords"};/*#CC05 Added ExpiryDateStatus*/
            dt = dt.DefaultView.ToTable(true, DsCol);
        }

        DataTable DsCopy = new DataTable();

        dt.Columns["Current Status"].ColumnName = "Status";
        if (dt.Rows.Count > 0)
        {
            try
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "SKUDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);



            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            }
        }
        else
        {
            ucMsg.ShowInfo(Resources.Messages.NoRecord);

        }
    }
    public void databind()
    {
        using (ProductData objproduct = new ProductData())
        {
            try
            {
                ucMsg.Visible = false;
                objproduct.SKUName = txtSerName.Text.Trim();
                objproduct.SKUCode = txtSerCode.Text.Trim();

                if (cmbSerModel.SelectedValue != "0")
                {
                    objproduct.SKUModelId = Convert.ToInt16(cmbSerModel.SelectedValue.ToString());
                }
                else
                {
                    objproduct.SKUModelId = 0;
                }
                if (cmbSerProdCat.SelectedValue != "0")
                {
                    objproduct.SKUProdCatId = Convert.ToInt16(cmbSerProdCat.SelectedValue.ToString());
                }
                else
                {
                    objproduct.SKUProdCatId = 0;
                }
                if (cmbSercolor.SelectedValue != "0")
                {
                    objproduct.SKUColorId = Convert.ToInt16(cmbSercolor.SelectedValue.ToString());
                }
                else
                {
                    objproduct.SKUColorId = 0;
                }
                if (mode3 == 3)
                {
                    objproduct.CompanyId = PageBase.ClientId;
                    objproduct.SKUSelectionMode = 3;
                    skuinfo = objproduct.SelectSKUInfo();
                    return;
                }
                else
                {
                    objproduct.SKUSelectionMode = 2;
                }
                objproduct.CompanyId = PageBase.ClientId;
                skuinfo = objproduct.SelectSKUInfo();


                grdSKU.DataSource = skuinfo;
                grdSKU.DataBind();
                updgrid.Update();


            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }

        }

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtSerCode.Text == "" && txtSerName.Text == "" && cmbSercolor.SelectedValue == "0"
           && cmbSerProdCat.SelectedValue == "0")
        {
            ucMsg.ShowInfo("Please enter atleast one searching parameters");
            return;
        }

        //blanksertext();
        databind();
    }
    protected void fillallgrid_Click(object sender, EventArgs e)
    {
        blanksertext();

        databind();
        ucMsg.Visible = false;
    }
    public void blanksertext()
    {
        txtSerName.Text = "";
        txtSerCode.Text = "";
        cmbSercolor.SelectedIndex = 0;
        cmbSerModel.Items.Clear();
        cmbSerModel.Items.Insert(0, new ListItem("Select", "0"));
        cmbSerProdCat.SelectedIndex = 0;
        UpdSearch.Update();
    }
    public void fillcombos()
    {
        using (ProductData objproduct = new ProductData())
        {

            try
            {
                objproduct.CompanyId = PageBase.ClientId;
                DataTable dt = objproduct.SelectAllProdCatInfo();
                DataTable ds = objproduct.SelectAllColorInfo();

                cmbSerProdCat.Items.Clear();

                cmbSercolor.Items.Clear();
                String[] colArray1 = { "ProductCategoryID", "ProductCategoryName" };
                String[] colArray = { "ColorID", "ColorName" };

                PageBase.DropdownBinding(ref cmbSerProdCat, dt, colArray1);

                PageBase.DropdownBinding(ref cmbSercolor, ds, colArray);

                cmbSerModel.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }

        }
    }
    protected void cmbSerProdCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (ProductData objproduct = new ProductData())
        {

            try
            {
                if (cmbSerProdCat.SelectedValue == "0")
                {
                    cmbSerModel.Items.Clear();
                    cmbSerModel.Items.Insert(0, new ListItem("Select", "0"));
                    cmbSerModel.SelectedValue = "0";
                }
                else
                {
                    objproduct.ModelProdCatId = Convert.ToInt16(cmbSerProdCat.SelectedValue.ToString());
                    objproduct.ModelSelectionMode = 1;
                    objproduct.CompanyId = PageBase.ClientId;
                    DataTable dtmodelfil = objproduct.SelectModelInfo();
                    String[] colArray1 = { "ModelID", "ModelName" };
                    PageBase.DropdownBinding(ref cmbSerModel, dtmodelfil, colArray1);
                    cmbSerModel.SelectedValue = "0";
                    UpdSearch.Update();

                }
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            }
        }
    }
    protected void grdSKUpage_indexchanging(object sender, GridViewPageEventArgs e)
    {

        grdSKU.PageIndex = e.NewPageIndex;
        databind();

    }
    public static void CreateCSVfile(DataTable dtable, string strFilePath)
    {
        StreamWriter sw = new StreamWriter(strFilePath, false);
        int icolcount = dtable.Columns.Count;
        foreach (DataRow drow in dtable.Rows)
        {
            for (int i = 0; i < icolcount; i++)
            {
                if (!Convert.IsDBNull(drow[i]))
                {
                    sw.Write(drow[i].ToString());
                }
                if (i < icolcount - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
        }
        sw.Close();
        sw.Dispose();
    }
}
