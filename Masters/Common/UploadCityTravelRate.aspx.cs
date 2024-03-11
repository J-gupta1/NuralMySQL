#region Copyright(c) 2017 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2017 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Vijay Kumar Prajapati
* Created On: 28-June-2020
 * Description: This is  Upload CityTravelRate  Page .
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 ====================================================================================================
*/
#endregion
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

public partial class Masters_Common_UploadCityTravelRate : PageBase
{
    DataTable dtNew = new DataTable();
    object objSum;
    int counter = 0;
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();
    string strPrimarySessionName = "SaveUploadSession";
    string strPrimarySessionNameupdate = "UpdateUploadSessionUpdate";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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
            FillRoleandCityGroup();
            databind();
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
                objSalesData.TemplateType = 3;
                dsTemplateCode = objSalesData.GetLocalityTemplate();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "SaveCityTravelMaster";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SaveCityTravelMaster" };
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
                    ucMsg.ShowInfo(objSalesData.OutError);
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
                objSalesData.TemplateType = 4;
                dsTemplateCode = objSalesData.GetLocalityTemplate();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "UpdateCityTravelRate";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "UpdateCityTravelRate" };
                    if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                    {
                        ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 1);
                        DataTable dt = dsTemplateCode.Tables[0].Copy();
                            DataSet dtcopy = new DataSet();
                            dtcopy.Merge(dt);
                            dtcopy.Tables[0].AcceptChanges();
                            ZedService.Utility.ZedServiceUtil.ExportToExecl(dtcopy, FilenameToexport, 1, strExcelSheetName);
                        }
                        else
                        {

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
                objSalesData.TemplateType = 7;
                dsTemplateCode = objSalesData.GetMaterialMasterReferenceData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {
                    if (dsTemplateCode.Tables[0].Columns.Contains("CityTravelRateDetailID"))
                    {
                        dsTemplateCode.Tables[0].Columns.Remove("CityTravelRateDetailID");
                        dsTemplateCode.Tables[0].AcceptChanges();
                    }
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ReferenceCode";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "RoleDetail","CityGroupDetail" };
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
    protected void DownloadReferenceCodeForUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 7;
                dsTemplateCode = objSalesData.GetMaterialMasterReferenceData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ReferenceCode";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "RoleDetail", "CityGroupDetail" };
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
                    else if (DsExcel.Tables[0].Columns.Contains("Status") && Rbtdownloadtemplate.SelectedValue == "1")
                    {
                        ucMsg.ShowInfo("You are uploading an update template, please upload save template.");
                        return;
                    }
                    else if (!DsExcel.Tables[0].Columns.Contains("Status") && Rbtdownloadtemplate.SelectedValue == "2")
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
                            objValidateFile.ExcelFileNameInTable = "SaveCityTravelRate";

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
                            objValidateFile.ExcelFileNameInTable = "UpdateCityTravelRate";
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
                                objDetail.TemplateType = Convert.ToInt16(Rbtdownloadtemplate.SelectedValue);
                                DataSet dtInvalidRecordSet = objDetail.UploadCityTravelData();
                                Int32 result = objDetail.Out_Param;
                                if (result == 0)
                                {
                                    databind();
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
                                objDetail.TemplateType = Convert.ToInt16(Rbtdownloadtemplate.SelectedValue);
                                DataSet dtInvalidRecordSet = objDetail.UploadCityTravelData();
                                Int32 result = objDetail.Out_Param;
                                if (result == 0)
                                {
                                    databind();
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
                bulkCopy.DestinationTableName = "BulkUploadCityTravelRate";
                if (dtUpload.Columns.Contains("Role"))
                {
                    bulkCopy.ColumnMappings.Add("Role", "Role");
                }
                if (dtUpload.Columns.Contains("CityCategory"))
                {
                    bulkCopy.ColumnMappings.Add("CityCategory", "CityCategory");
                }
                if (dtUpload.Columns.Contains("CityCategoryDA"))
                {
                    bulkCopy.ColumnMappings.Add("CityCategoryDA", "CityCategoryDA");
                }
                if (dtUpload.Columns.Contains("FixConveynce"))
                {
                    bulkCopy.ColumnMappings.Add("FixConveynce", "FixConveynce");
                }
                if (dtUpload.Columns.Contains("InCityKM"))
                {
                    bulkCopy.ColumnMappings.Add("InCityKM", "InCityKM");
                }
                if (dtUpload.Columns.Contains("XTownKM"))
                {
                    bulkCopy.ColumnMappings.Add("XTownKM", "XTownKM");
                }
                if (dtUpload.Columns.Contains("OutCityKM"))
                {
                    bulkCopy.ColumnMappings.Add("OutCityKM", "OutCityKM");
                }
                if (dtUpload.Columns.Contains("Status"))
                {
                    bulkCopy.ColumnMappings.Add("Status", "Status");
                }
                if (dtUpload.Columns.Contains("CityTravelRateDetailID"))
                {
                    bulkCopy.ColumnMappings.Add("CityTravelRateDetailID", "CityTravelRateDetailID");
                }
                if (dtUpload.Columns.Contains("TransactionUploadSessionId"))
                {
                    bulkCopy.ColumnMappings.Add("TransactionUploadSessionId", "TransactionUploadSessionId");
                }
                if (dtUpload.Columns.Contains("ValidFrom"))
                {
                    bulkCopy.ColumnMappings.Add("ValidFrom", "ValidFrom");
                }
                if (dtUpload.Columns.Contains("ValidTo"))
                {
                    bulkCopy.ColumnMappings.Add("ValidTo", "ValidTo");
                }
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
    protected void btnSerCode_Click(object sender, EventArgs e)
    {
        if (ddlRole.SelectedValue == "0" && ddlCityGroup.SelectedValue == "0" && ucFromDate.Date == "" && ucToDate.Date == "")
        {
            ucMsg.ShowInfo("Please enter atleast one searching parameter ");
            return;
        }
        if (ucFromDate.Date != "" && ucToDate.Date == "")
        {
            ucMsg.Visible = true;
            ucMsg.ShowWarning("Please Enter  To Date.");
            return;
        }
        if (ucToDate.Date != "" && ucFromDate.Date == "")
        {
            ucMsg.Visible = true;
            ucMsg.ShowWarning("Please Enter  From Date.");
            return;
        }
        databind();
    }
    protected void getalldata_Click(object sender, EventArgs e)
    {
        blanksertext();
        databind();
        ucMsg.Visible = false;
    }
    protected void grdCityTravelRate_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCityTravelRate.PageIndex = e.NewPageIndex;
        databind();
    }
    protected void exportToExel_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        using (CommonMaster objmaster = new CommonMaster())
        {

            ucMsg.Visible = false;
            objmaster.RoleId = Convert.ToInt64(ddlRole.SelectedValue);
            objmaster.CityGroupId = Convert.ToInt32(ddlCityGroup.SelectedValue);
            objmaster.CompanyID = PageBase.ClientId;
            objmaster.TemplateType = 1;
            if (ucFromDate.Date == "" && ucToDate.Date == "")
            { ;}
            else
            {
                objmaster.DateFrom = Convert.ToDateTime(ucFromDate.Date);
                objmaster.DateTo = Convert.ToDateTime(ucToDate.Date);
            }
            try
            {
                dt = objmaster.SelectAllCityTravelRate();
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "CityTravelRateDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
                updgrid.Update();


            }
            catch (Exception ex)
            {

                ucMsg.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    private void FillRoleandCityGroup()
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 7;
                objSalesData.ddlFill = 1;
                dsTemplateCode = objSalesData.GetMaterialMasterReferenceData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {
                    ddlRole.DataSource = dsTemplateCode.Tables[0];
                    ddlRole.DataTextField = "RoleName";
                    ddlRole.DataValueField = "RoleID";
                    ddlRole.DataBind();
                    ddlRole.Items.Insert(0, new ListItem("Select", "0"));


                    ddlCityGroup.DataSource = dsTemplateCode.Tables[1];
                    ddlCityGroup.DataTextField = "CityCategory";
                    ddlCityGroup.DataValueField = "CityGroupId";
                    ddlCityGroup.DataBind();
                    ddlCityGroup.Items.Insert(0, new ListItem("Select", "0"));
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
    public void databind()
    {
        DataTable dt = new DataTable();
        using (CommonMaster objmaster = new CommonMaster())
        {

            ucMsg.Visible = false;
            objmaster.RoleId = Convert.ToInt64(ddlRole.SelectedValue);
            objmaster.CityGroupId = Convert.ToInt32(ddlCityGroup.SelectedValue);
            objmaster.CompanyID =PageBase.ClientId;
            if (ucFromDate.Date == "" && ucToDate.Date == "")
            { ;}
            else
            {
                objmaster.DateFrom = Convert.ToDateTime(ucFromDate.Date);
                objmaster.DateTo = Convert.ToDateTime(ucToDate.Date);
            }
            try
            {
                dt = objmaster.SelectAllCityTravelRate();
                grdCityTravelRate.DataSource = dt;
                grdCityTravelRate.DataBind();
                updgrid.Update();


            }
            catch (Exception ex)
            {

                ucMsg.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    public void blanksertext()
    {

        ddlRole.SelectedValue = "0";
        ddlCityGroup.SelectedValue = "0";
        ucFromDate.TextBoxDate.Text = "";
        ucToDate.TextBoxDate.Text = "";
        UpdSearch.Update();

    }
}