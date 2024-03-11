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

public partial class Masters_Common_UploadSKUWiseScheme : PageBase
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
    string strPrimarySessionName = "SKUSchemeUploadSession";
    string strPrimarySessionNameupdate = "SKUSchemeUploadSessionUpdate";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindSKUCODE();
            BindState();
            BindRegion();
            ddlCity.Items.Insert(0, new ListItem("Select", "0"));
            SearchSKUSchemeData(1);
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
    protected void DwnldTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 9;
                dsTemplateCode = objSalesData.GetMaterialMasterTemplate();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {
                    DataTable dt01 = dsTemplateCode.Tables[0].Copy();
                    if (dt01.Columns.Contains("SrNo"))
                    {
                        dt01.Columns["SrNo"].SetOrdinal(0);
                    }
                    if (dt01.Columns.Contains("SkuCode"))
                    {
                        dt01.Columns["SkuCode"].SetOrdinal(1);
                    }
                    if (dt01.Columns.Contains("SchemeDescription"))
                    {
                        dt01.Columns["SchemeDescription"].SetOrdinal(2);
                    }
                    if (dt01.Columns.Contains("StartDate"))
                    {
                        dt01.Columns["StartDate"].SetOrdinal(3);
                    }
                    if (dt01.Columns.Contains("EndDate"))
                    {
                        dt01.Columns["EndDate"].SetOrdinal(4);
                    }
                    if (dt01.Columns.Contains("Status"))
                    {
                        dt01.Columns["Status"].SetOrdinal(5);
                    }
                    if (dt01.Columns.Contains("Region"))
                    {
                        dt01.Columns["Region"].SetOrdinal(6);
                    }
                    if (dt01.Columns.Contains("State"))
                    {
                        dt01.Columns["State"].SetOrdinal(7);
                    }
                    if (dt01.Columns.Contains("City"))
                    {
                        dt01.Columns["City"].SetOrdinal(8);
                    }
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt01);
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "SaveSKUScheme";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SaveSKUScheme" };
                    ChangedExcelSheetNames(ref ds, strExcelSheetName, 1);
                    if (ds.Tables.Count > 1)
                        ds.Tables.RemoveAt(1);
                    for (int i = ds.Tables[0].Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = ds.Tables[0].Rows[i];

                        ds.Tables[0].Rows.Remove(dr);

                    }

                    ZedService.Utility.ZedServiceUtil.ExportToExecl(ds, FilenameToexport, 1, strExcelSheetName);
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
                objSalesData.TemplateType = 10;
                dsTemplateCode = objSalesData.GetMaterialMasterTemplate();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "UploadSkuWiseSchemeUpdate";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "UploadSkuWiseSchemeUpdate" };
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
                objSalesData.TemplateType = 9;
                dsTemplateCode = objSalesData.GetMaterialMasterReferenceData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ReferenceCode";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SKUDetail", "RegionDetail", "StateandCityDetail"};
                    ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 3);
                    if (dsTemplateCode.Tables.Count > 3)
                        dsTemplateCode.Tables.RemoveAt(3);
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(dsTemplateCode, FilenameToexport, 3, strExcelSheetName);
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
    protected void DownloadReferenceCodeForUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 9;
                dsTemplateCode = objSalesData.GetMaterialMasterReferenceData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ReferenceCode";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SKUDetail", "RegionDetail", "StateDetail", "CityDetail" };
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
                    else if (DsExcel.Tables[0].Columns.Contains("SKUSchemeID") && Rbtdownloadtemplate.SelectedValue == "1")
                    {
                        ucMsg.ShowInfo("You are uploading an update template, please upload save template.");
                        return;
                    }
                    else if (!DsExcel.Tables[0].Columns.Contains("SKUSchemeID") && Rbtdownloadtemplate.SelectedValue == "2")
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
                            objValidateFile.ExcelFileNameInTable = "UploadSkuWiseScheme";
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
                            objValidateFile.ExcelFileNameInTable = "UploadSkuWiseSchemeUpdate";
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
                                DataSet dtInvalidRecordSet = objDetail.UploadSKUWisechemeForSaveUpdate();
                                Int32 result = objDetail.Out_Param;
                                if (result == 0)
                                {
                                    SearchSKUSchemeData(1);
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
                                DataSet dtInvalidRecordSet = objDetail.UploadSKUWisechemeForSaveUpdate();
                                Int32 result = objDetail.Out_Param;
                                if (result == 0)
                                {
                                     SearchSKUSchemeData(1);
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
    public bool UserDetailBCP(DataTable dtUpload)
    {
        try
        {

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkUploadSKUWiseScheme";
                if (dtUpload.Columns.Contains("SkuCode"))
                {
                    bulkCopy.ColumnMappings.Add("SkuCode", "SkuCode");
                }
                if (dtUpload.Columns.Contains("SchemeDescription"))
                {
                    bulkCopy.ColumnMappings.Add("SchemeDescription", "SchemeDescription");
                }
                if (dtUpload.Columns.Contains("StartDate"))
                {
                    bulkCopy.ColumnMappings.Add("StartDate", "StartDate");
                }
                if (dtUpload.Columns.Contains("EndDate"))
                {
                    bulkCopy.ColumnMappings.Add("EndDate", "EndDate");
                }
                if (dtUpload.Columns.Contains("Status"))
                {
                    bulkCopy.ColumnMappings.Add("Status", "Status");
                }
                if (dtUpload.Columns.Contains("Region"))
                {
                    bulkCopy.ColumnMappings.Add("Region", "Region");
                }
                if (dtUpload.Columns.Contains("State"))
                {
                    bulkCopy.ColumnMappings.Add("State", "State");
                }
                if (dtUpload.Columns.Contains("City"))
                {
                    bulkCopy.ColumnMappings.Add("City", "City");
                }
                if (dtUpload.Columns.Contains("SKUSchemeID"))
                {
                    bulkCopy.ColumnMappings.Add("SKUSchemeID", "SKUSchemeID");
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
    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
            PageBase.ExportToExeclV2(DsError, strFileName, DsError.Tables.Count);
        }
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (CommonMaster objData = new CommonMaster())
        {

            try
            {
                objData.CompanyID = PageBase.ClientId;
                objData.UserID = PageBase.UserId;
                objData.CallingMode = 7;
                objData.StateID = Convert.ToInt32(ddlState.SelectedValue);
                DataTable dt = objData.BindAllDropdownForEntityBrandMapping();
                if (dt.Rows.Count > 0)
                {
                    String[] colArray1 = { "CityID", "CityName" };
                    PageBase.DropdownBinding(ref ddlCity, dt, colArray1);
                }
                else
                {
                    ddlCity.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }

        }
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        SearchSKUSchemeData(ucPagingControl1.CurrentPage);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if ((ddlSKUCODE.SelectedValue == "0" && ddlRegion.SelectedValue == "0" && ddlState.SelectedValue == "0" && ddlCity.SelectedValue == "0" && ucStartDate.Date == "" && UcEndDate.Date == "" && ddlStatus.SelectedValue == "255"))
            {
                ucMsg.ShowWarning("Select OR Enter any search criteria!!!");
                return;

            }
            else if (ucStartDate.Date != "" && UcEndDate.Date == "")
            {
                ucMsg.ShowWarning("Please enter To Date.");
                return;
            }
            else if (ucStartDate.Date == "" && UcEndDate.Date != "")
            {
                ucMsg.ShowWarning("Please enter  From Date.");
                return;
            }
            
                ucMsg.Visible = false;
                SearchSKUSchemeData(1);
           
        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.Message.ToString());
        }
    }
    protected void fillallgrid_Click(object sender, EventArgs e)
    {
        ClearControl();
        SearchSKUSchemeData(1);
    }
    protected void exportToExel_Click(object sender, EventArgs e)
    {
        try
        {
            if (ucStartDate.Date != "" && UcEndDate.Date == "")
            {
                ucMsg.ShowWarning("Please enter To Date.");
                return;
            }
            else if (ucStartDate.Date == "" && UcEndDate.Date != "")
            {
                ucMsg.ShowWarning("Please enter  From Date.");
                return;
            }

            ucMsg.Visible = false;
            SearchSKUSchemeData(-1);

        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.Message.ToString());
        }
    }
    public void BindSKUCODE()
    {
        using (CommonMaster objData = new CommonMaster())
        {

            try
            {
                objData.CompanyID = PageBase.ClientId;
                objData.UserID = PageBase.UserId;
                objData.CallingMode = 4;
                DataTable dt = objData.BindAllDropdownForEntityBrandMapping();
                if (dt.Rows.Count > 0)
                {
                    String[] colArray1 = { "SKUID", "SKUCode" };
                    PageBase.DropdownBinding(ref ddlSKUCODE, dt, colArray1);
                }
                else
                {
                    ddlSKUCODE.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }

        }
    }
    public void BindRegion()
    {
        using (CommonMaster objData = new CommonMaster())
        {

            try
            {
                objData.CompanyID = PageBase.ClientId;
                objData.UserID = PageBase.UserId;
                objData.CallingMode = 5;
                DataTable dt = objData.BindAllDropdownForEntityBrandMapping();
                if (dt.Rows.Count > 0)
                {
                    String[] colArray1 = { "RegionID", "RegionName" };
                    PageBase.DropdownBinding(ref ddlRegion, dt, colArray1);
                }
                else
                {
                    ddlRegion.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }

        }
    }
    public void BindState()
    {
        using (CommonMaster objData = new CommonMaster())
        {

            try
            {
                objData.CompanyID = PageBase.ClientId;
                objData.UserID = PageBase.UserId;
                objData.CallingMode = 6;
                DataTable dt = objData.BindAllDropdownForEntityBrandMapping();
                if (dt.Rows.Count > 0)
                {
                    String[] colArray1 = { "StateID", "StateName" };
                    PageBase.DropdownBinding(ref ddlState, dt, colArray1);
                }
                else
                {
                    ddlState.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }

        }
    }
    public void SearchSKUSchemeData(int pageno)
    {
        CommonMaster objData;
        try
        {
            ViewState["TotalRecords"] = 0;
            ViewState["CurrentPage"] = pageno;
            using (objData = new CommonMaster())
            {
                objData.UserID = PageBase.UserId;
                objData.CompanyID = PageBase.ClientId;
                objData.SKUID = Convert.ToInt32(ddlSKUCODE.SelectedValue);
                objData.RegionId = Convert.ToInt32(ddlRegion.SelectedValue);
                objData.StateID = Convert.ToInt32(ddlState.SelectedValue);
                objData.CityId = Convert.ToInt32(ddlCity.SelectedValue);
                objData.PageIndex = pageno;
                objData.PageSize = Convert.ToInt32(PageBase.PageSize);

                if (ucStartDate.Date == "" && UcEndDate.Date == "")
                { ;}
                else
                {
                    objData.DateFrom = Convert.ToDateTime(ucStartDate.Date);
                    objData.DateTo = Convert.ToDateTime(UcEndDate.Date);
                }
                
                objData.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                DataSet ds = objData.GetReportSKUSchemeData();
                if (objData.TotalRecords > 0)
                {
                    PnlGrid.Visible = true;
                    if (pageno > 0)
                    {
                        gvSKUSchemeDetail.DataSource = ds;
                        gvSKUSchemeDetail.DataBind();
                        PnlGrid.Visible = true;
                        dvFooter.Visible = true;
                        ViewState["TotalRecords"] = objData.TotalRecords;
                        ucPagingControl1.TotalRecords = objData.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {

                        string FilenameToexport = "SKUSchemeData";
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    }
                }
                else
                {
                    ds = null;
                    gvSKUSchemeDetail.DataSource = null;
                    gvSKUSchemeDetail.DataBind();
                   // PnlGrid.Visible = false;
                    dvFooter.Visible = false;
                    
                    

                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    private void ClearControl()
    {
        ddlSKUCODE.SelectedValue = "0";
        ddlRegion.SelectedValue = "0";
        ddlState.SelectedValue = "0";

        if (ddlState.SelectedValue == "0")
        {
            ddlCity.Items.Clear();
            ddlCity.Items.Insert(0, new ListItem("Select", "0"));
        }
        ucStartDate.TextBoxDate.Text = "";
        UcEndDate.TextBoxDate.Text = "";
        ddlStatus.SelectedValue = "255";

    }
}