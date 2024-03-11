#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ========================================================================================================================================
 * <copyright company="Zed Axis Technologies">
 * COPYRIGHT (c) 2018 Zed Axis Technologies (P) Ltd. 
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
 * WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * </copyright>
 * ========================================================================================================================================
 * Created By : Sumit Maurya
 * Role :  Software Developer.
 * Date : 27-Dec-2018
 * Descriptiopn: this interface is copy of "ISPMasterUploadV2.aspx"
 * ========================================================================================================================================
 * Reviewed By : 
 * ========================================================================================================================================
 * Change Log
 * Date , Name , #CCxx, Description
 * ========================================================================================================================================
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using BussinessLogic;
using DataAccess;
using System.Data;
using System.Collections;
using BusinessLogics;
using ZedService;
using System.Data.SqlClient;

public partial class Masters_Common_ISPMasterUploadV2 : PageBase
{
    Int16 CompanyID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblUploadMsg.Text = "";
        //  CompanyID = PageBase.CompanyID;
        if (!IsPostBack)
        {
            FillISPGrid();
        }
    }
    #region Upload Variables
    string strUploadedFileName = string.Empty;
    string strMsg = string.Empty;
    bool blnIsValid;
    UploadFile UploadFile = new UploadFile();
    List<String> lstDuplicate = new List<String>();
    DataSet dsErrorProne = new DataSet();
    #endregion

    protected void LnkDownloadRefCode_Click(object sender, EventArgs e)
    {

        DataSet DsSku = new DataSet();
        try
        {
            using (RetailerData ObjSales = new RetailerData())
            {
                ObjSales.CompanyId = PageBase.ClientId;
                DsSku = ObjSales.RetailersCodeDownload();
            };
            if (DsSku.Tables.Count > 0)
            {
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "Retailer Code list";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(DsSku, FilenameToexport, EnumData.eTemplateCount.eTarget);
            }
        }
        catch (Exception ex)
        {

            PageBase.Errorhandling(ex);
            ucMsg.ShowError(ex.ToString(), GlobalErrorDisplay());

        }

    }
    public Int16 IsExcelFile(System.Web.UI.WebControls.FileUpload UploadControl, ref string FileName)
    {
        Int16 MessageforValidation = 0;
        try
        {

            if (UploadControl.HasFile)
            {
                if ((Path.GetExtension(UploadControl.FileName).ToLower() == ".xlsx") || (Path.GetExtension(UploadControl.FileName).ToLower() == ".xls"))
                {
                    try
                    {
                        double dblMaxFileSize = Convert.ToDouble(PageBase.ValidExcelLength);
                        int intFileSize = UploadControl.PostedFile.ContentLength;  //file size is obtained in bytes
                        double dblFileSizeinKB = intFileSize / 1024.0; //convert the file size into kilobytes
                        if ((dblFileSizeinKB > dblMaxFileSize))
                        {
                            MessageforValidation = 4;
                            return MessageforValidation;
                        }
                        strUploadedFileName = PageBase.importExportExcelFileName;
                        //UploadControl.SaveAs(RootFolerPath + PageBase.strGlobalUploadExcelPathRoot + strUploadedFileName);
                        UploadControl.SaveAs(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);

                        MessageforValidation = 1;
                        FileName = strUploadedFileName;
                        return MessageforValidation;
                    }
                    catch (Exception objEx)
                    {

                        MessageforValidation = 0;
                        throw new Exception(objEx.Message.ToString());
                        return MessageforValidation;

                    }
                }
                else
                {


                    MessageforValidation = 2;
                    return MessageforValidation;
                }
            }
            else
            {


                MessageforValidation = 3;
                return MessageforValidation;
            }
        }
        catch (HttpException objHttpException)
        {
            //return MessageforValidation;
            throw objHttpException;
        }
        catch (Exception ex)
        {
            //return MessageforValidation;
            throw ex;

        }


    }
    public DataTable GetBlankTableError()
    {
        DataTable Detail = new DataTable();
        Detail.Columns.Add("RetailerCode");
        Detail.Columns.Add("ISPCode");
        Detail.Columns.Add("ISPName");
        Detail.Columns.Add("ISPMobile");
        return Detail;
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {

            // GridISP.Visible = false;
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
                DataSet DsExcel = objexcel.ImportExcelFile(PageBase.strExcelPhysicalUploadPathSB + strUploadedFileName);

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
                            if (ddlUploadType.SelectedValue == "1")
                            {
                                objValidateFile.CompanyId = PageBase.ClientId;
                                objValidateFile.ExcelFileNameInTable = "ISPUploadAdd";
                            }
                            else if (ddlUploadType.SelectedValue == "2")
                            {
                                objValidateFile.ExcelFileNameInTable = "ISPUploadUpdate";
                            }
                            /* objValidateFile.ExcelFileNameInTable = "ISPUpload"; */
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
                                        //string strFileName = "invalidData" + DateTime.Now.Ticks;
                                        //ExportInExcel(objDS.Tables["DtExcelSheet"], strFileName);
                                        //hlnkInvalid.NavigateUrl = strExcelVirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                        dsErrorProne.Merge(objDS.Tables["DtExcelSheet"]);
                                        hlnkInvalid.Text = "Invalid Data";
                                        ucMsg.ShowInfo("There is error in data. Please refer Invalid Data link.");

                                        blnIsUpload = false;
                                    }
                                    blnIsUpload = false;
                                }


                                if (objDS.Tables["DtDuplicateRecord"] != null && objDS.Tables["DtDuplicateRecord"].Rows.Count > 0)
                                {
                                    //hlnkDuplicate.Visible = true;
                                    dsErrorProne.Merge(objDS.Tables["DtDuplicateRecord"]);
                                    //string strFileName = "DuplicateData" + DateTime.Now.Ticks;
                                    //ExportInExcel(objDS.Tables["DtDuplicateRecord"], strFileName);
                                    //hlnkDuplicate.NavigateUrl = strExcelVirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    //hlnkDuplicate.Text = "Duplicate Data";
                                    blnIsUpload = false;
                                }
                                if (objDS.Tables["DtBlankData"] != null && objDS.Tables["DtBlankData"].Rows.Count > 0)
                                {
                                    // hlnkBlank.Visible = true;
                                    dsErrorProne.Merge(objDS.Tables["DtBlankData"]);
                                    // string strFileName = "BlankData" + DateTime.Now.Ticks;
                                    // ExportInExcel(objDS.Tables["DtBlankData"], strFileName);
                                    //hlnkBlank.NavigateUrl = strExcelVirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                                    //hlnkBlank.Text = "Blank Data";
                                    blnIsUpload = false;
                                }
                                if (blnIsUpload)
                                {
                                    if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables["DtExcelSheet"].Rows.Count > 0)
                                    {
                                        InsertData(objDS);
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
                                    ucMsg.ShowInfo("There is error in data. Please refer Invalid Data link.");
                                }



                            }
                        }
                    }
                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.BlankFileMessage);
                }
            }
            else if (Upload == 2)
            {
                ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
            }
            else if (Upload == 3)
            {
                ucMsg.ShowInfo(Resources.Messages.SelectFile);
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    private void ExportInExcel(DataSet DsError, string strFileName)
    {
        if (DsError != null && DsError.Tables.Count > 0)
        {
            PageBase.ExportToExeclV2(DsError, strFileName, DsError.Tables.Count);
        }
    }

    private void InsertData(DataSet objds)
    {
        DataTable dt = new DataTable();
        dt = objds.Tables[0];
        if (Convert.ToString(Session["ISDLogin"]) != "0" && ddlUploadType.SelectedValue == "1")
        {
            if (Convert.ToInt16(Session["ISDLogin"]) == 0)
            {
                string expression;
                expression = "ISPUsername is null or ISPPassword is null";
                DataRow[] foundRows;
                foundRows = dt.Select(expression);
                if (foundRows != null)
                {
                    if (foundRows.Length > 0)
                    {
                        ucMsg.ShowInfo("ISPUsername or ISPPassword can not be blank.");
                        return;
                    }
                }
            }
            DataColumn dcPasswordSalt = new DataColumn("PasswordSalt", typeof(string));
            dcPasswordSalt.DefaultValue = string.Empty;
            dt.Columns.Add(dcPasswordSalt);
            using (Authenticates ObjAuth = new Authenticates())
            {
                foreach (DataRow item in dt.Rows)
                {
                    string pws = string.Empty;
                    if (item["ISPUsername"].ToString() != "" & item["ISPPassword"].ToString() != "")
                    {
                        pws = ObjAuth.GenerateSalt(item["ISPPassword"].ToString().Length);
                        item["PasswordSalt"] = pws;
                        item["ISPPassword"] = ObjAuth.EncryptPassword(item["ISPPassword"].ToString(), pws);
                    }
                }
            };
        }
        else
        {
            if (!dt.Columns.Contains("ISPUserName"))
            {
                dt.Columns.Add("ISPUsername");
            }
            if (!dt.Columns.Contains("ISPPassword"))
            {
                dt.Columns.Add("ISPPassword");
            }
            if (!dt.Columns.Contains("PasswordSalt"))
            {
                dt.Columns.Add("PasswordSalt");
            }
        }
        dt.AcceptChanges();
        objds = new DataSet();
        objds.Tables.Add(dt.Copy());
        if ((CreatedBcpData(objds.Tables[0])) == true)
        {
            using (BeautyAdvisorData ObjCommon = new BeautyAdvisorData())
            {
                DataSet dsResult = new DataSet();
                ObjCommon.Userid = PageBase.UserId;
                ObjCommon.UploadType = Convert.ToInt16(ddlUploadType.SelectedValue);
                ObjCommon.SessionID = objds.Tables[0].Rows[0]["SessionID"].ToString();
                ObjCommon.CompanyID = PageBase.ClientId;
                dsResult = ObjCommon.SaveISPBulkUpload();

                int result = ObjCommon.intOutParam;
                if (result == 0)
                {
                    ucMsg.ShowSuccess("Data uploaded successfully.");
                    ddlUploadType.SelectedValue = "0";

                }
                else if (result == 1 && dsResult != null && dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        hlnkInvalid.Visible = true;
                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                        ucMsg.ShowInfo(ObjCommon.Error);
                        ExportInExcel(dsResult, strFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                    }
                }
                else
                {
                    //ucMsg.ShowError(objSales.Error);
                }

            }
        }
    }
    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "2")
        {
            Response.Redirect("ISPMasterInterface.aspx");
        }
    }



    public bool UploadBcp(DataTable dtUpload)
    {
        try
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "ISPBulkUpload";
                bulkCopy.ColumnMappings.Add("RetailerCode", "RetailerCode");
                bulkCopy.ColumnMappings.Add("ISPName", "ISPName");
                bulkCopy.ColumnMappings.Add("ISPCode", "ISPCode");
                bulkCopy.ColumnMappings.Add("StoreCode", "StoreCode");
                bulkCopy.ColumnMappings.Add("ISPMobile", "ISPMobile");
                bulkCopy.ColumnMappings.Add("Email", "Email");
                bulkCopy.ColumnMappings.Add("ISPUserName", "UserName");
                bulkCopy.ColumnMappings.Add("ISPPassword", "Password");
                bulkCopy.ColumnMappings.Add("PasswordSalt", "PasswordSalt");
                bulkCopy.ColumnMappings.Add("Status", "Status");
                bulkCopy.ColumnMappings.Add("SessionID", "SessionID");
                bulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                bulkCopy.ColumnMappings.Add("UploadType", "UploadType");
                bulkCopy.WriteToServer(dtUpload);
                return true;
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo("Error in transferring data to server.");
            return false;
        }
    }

    public bool CreatedBcpData(DataTable dtUpload)
    {
        bool result = false;
        try
        {
            string guid = Guid.NewGuid().ToString();
            dtUpload.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
            dtUpload.Columns.Add(AddColumn(Convert.ToString(PageBase.UserId), "CreatedBy", typeof(int)));
            dtUpload.Columns.Add(AddColumn(Convert.ToString(ddlUploadType.SelectedValue), "UploadType", typeof(int)));
            if (ddlUploadType.SelectedValue == "1")
            {
                if (!dtUpload.Columns.Contains("Status"))
                {
                    dtUpload.Columns.Add("Status");
                }
            }
            else if (ddlUploadType.SelectedValue == "2")
            {
                if (!dtUpload.Columns.Contains("ISPUserName"))
                {
                    dtUpload.Columns.Add("ISPUsername");
                }
                if (!dtUpload.Columns.Contains("ISPPassword"))
                {
                    dtUpload.Columns.Add("ISPPassword");
                }
            }

            dtUpload.AcceptChanges();
            int i = PageBase.UserId;

            if (UploadBcp(dtUpload) == true)
            {
                result = true;
            }
            else
            {
                ucMsg.ShowInfo("Error in transerferring data to server.");
            }
            return result;
        }
        catch (Exception ex)
        {
            return result;
        }

    }

    protected void lnkDownloadTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            if (ddlUploadType.SelectedValue == "1")
            {


                using (CommonMaster objSalesData = new CommonMaster())
                {
                    objSalesData.UserID = PageBase.UserId;
                    objSalesData.CompanyID = PageBase.ClientId;
                    objSalesData.TemplateType = 1;
                    dsTemplateCode = objSalesData.GetISPUploadTemplate();
                    if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                    {

                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "ISPUploadAdd";
                        PageBase.RootFilePath = FilePath;
                        string[] strExcelSheetName = { "ISPUploadAdd" };
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
            if (ddlUploadType.SelectedValue == "2")
            {

                string Password = string.Empty;
                using (CommonMaster objSalesData = new CommonMaster())
                {
                    objSalesData.UserID = PageBase.UserId;
                    objSalesData.CompanyID = PageBase.ClientId;
                    objSalesData.TemplateType = 2;
                    dsTemplateCode = objSalesData.GetISPUploadTemplate();
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ISPUploadUpdate";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "ISPUploadUpdate" };
                    if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                    {
                        ChangedExcelSheetNames(ref dsTemplateCode, strExcelSheetName, 1);
                        DataTable dt = dsTemplateCode.Tables[0].Copy();
                        if (dt.Rows.Count > 0 && dt.Columns.Contains("PasswordSalt"))
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                using (Authenticates ObjAuth = new Authenticates())
                                {
                                    Password = ObjAuth.DecryptPassword(Convert.ToString(dr["ISPPassword"]), Convert.ToString(dr["PasswordSalt"]));
                                };
                                dr["ISPPassword"] = Password;
                            }
                            if (dt.Columns.Contains("PasswordSalt"))
                            {
                                dt.Columns.Remove("PasswordSalt");
                                dt.AcceptChanges();
                            }
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
                    else
                    {
                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());

        }
    }

    private DataTable ISPTemplateExcelFile()
    {
        try
        {
            OpenXMLExcel objexcel = new OpenXMLExcel();
            DataSet dsTemplate = new DataSet();
            string strTemplatePath = Server.MapPath(PageBase.strExcelTemplatePathSB);
            DirectoryInfo dirXLS = new DirectoryInfo(strTemplatePath);
            FileInfo[] drFilesXLS = dirXLS.GetFiles("*.xlsx");
            foreach (FileInfo fi in drFilesXLS)
            {
                if (fi.Name.ToLower().Contains("ispmaster") == true)
                    dsTemplate = objexcel.ImportExcelFile(strTemplatePath + "ISPMaster.xlsx");
            }
            if (Convert.ToString(Session["ISDLogin"]) == "0")
            {
                if (dsTemplate.Tables[0].Columns.Contains("ISPUserName"))
                {
                    dsTemplate.Tables[0].Columns.Remove("ISPUsername");
                }
                if (dsTemplate.Tables[0].Columns.Contains("ISPPassword"))
                {
                    dsTemplate.Tables[0].Columns.Remove("ISPPassword");
                }
                if (dsTemplate.Tables[0].Columns.Contains("ISPEmail"))
                {
                    dsTemplate.Tables[0].Columns.Remove("ISPEmail");
                }
            }

            if (ddlUploadType.SelectedValue == "1")
            {
                if (dsTemplate.Tables[0].Columns.Contains("Status"))
                {
                    dsTemplate.Tables[0].Columns.Remove("Status");
                }
            }
            else if (ddlUploadType.SelectedValue == "2")
            {
                if (dsTemplate.Tables[0].Columns.Contains("ISPUserName"))
                {
                    dsTemplate.Tables[0].Columns.Remove("ISPUsername");
                }
                if (dsTemplate.Tables[0].Columns.Contains("ISPPassword"))
                {
                    dsTemplate.Tables[0].Columns.Remove("ISPPassword");
                }
            }

            dsTemplate.Tables[0].AcceptChanges();
            return dsTemplate.Tables[0];
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
            throw ex;
        }

    }

    protected void LnkDownloadISPMasterData_Click(object sender, EventArgs e)
    {
        try
        {

            DataTable dt = DataToExport();
            string Password = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    using (Authenticates ObjAuth = new Authenticates())
                    {
                        Password = ObjAuth.DecryptPassword(Convert.ToString(dr["Password"]), Convert.ToString(dr["PasswordSalt"]));
                    };
                    dr["Password"] = Password;
                    Password = string.Empty;
                }
            }
            dt.Columns.Remove("PasswordSalt");
            dt.AcceptChanges();
            if (dt.Rows.Count > 0)
            {
                try
                {
                    DataSet dtcopy = new DataSet();
                    dtcopy.Merge(dt);
                    dtcopy.Tables[0].AcceptChanges();
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ISPDetails";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);
                    ViewState["DtExport"] = null;
                }
                catch (Exception ex)
                {
                    ucMsg.ShowInfo(ex.Message);
                    PageBase.Errorhandling(ex);
                }
            }
            else
            {
                ucMsg.ShowInfo(Resources.GlobalMessages.NoRecordexport);
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);

        }
    }

    DataTable DataToExport()
    {

        DataTable DtBeautyAdvisor = new DataTable();
        using (BeautyAdvisorData ObjBA = new BeautyAdvisorData())
        {

            ObjBA.ISPCode = "";
            ObjBA.StoreCode = "";
            ObjBA.ISPName = "";
            ObjBA.CompanyID = PageBase.ClientId;
            DtBeautyAdvisor = ObjBA.ISPExport();
        }

        return DtBeautyAdvisor;
    }
    protected void btnSearchUser_Click(object sender, EventArgs e)
    {
        FillISPGrid();
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        ClearSearchField();
        FillISPGrid();
    }
    private void FillISPGrid()
    {
        DataTable dtUsers;
        using (BeautyAdvisorData objuser = new BeautyAdvisorData())
        {
            objuser.Userid = Convert.ToInt32(PageBase.UserId);
            objuser.Email = txtEmailIDSearch.Text.Trim();
            objuser.Mobile = string.IsNullOrEmpty(txtMobileNumberSearch.Text.Trim()) ? "" : txtMobileNumberSearch.Text.Trim();
            objuser.CompanyID = PageBase.ClientId;
            objuser.ISPName = txtISPname.Text.Trim();
            objuser.ISPCode = txtISPCode.Text.Trim();
            objuser.ActiveStatus = Convert.ToInt32(ddlUserStatus.SelectedValue);
            dtUsers = objuser.GetISPInfo();
        };
        if (dtUsers != null && dtUsers.Rows.Count > 0)
        {
            GridISP.DataSource = dtUsers;
        }
        else
        {

            GridISP.DataSource = null;
        }
        GridISP.DataBind();
        GridISP.Visible = true;
        UpdSearch.Update();
    }
    protected void GridISP_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridViewRow GVR = e.Row;
                LinkButton hlPassword = default(LinkButton);
                hlPassword = (LinkButton)GVR.FindControl("hlPassword");
                string strPassword = null;
                Label lblPassword = (Label)GVR.FindControl("lblPassword");
                Label lblPasswordSalt = (Label)GVR.FindControl("lblPasswordSalt");
                strPassword = fncChangePwd(lblPassword.Text, lblPasswordSalt.Text);
                hlPassword.Attributes.Add("Onclick", "javascript:alert('User password is : " + strPassword + "');{return false;}");

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void GridISP_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void GridISP_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridISP.PageIndex = e.NewPageIndex;
        FillISPGrid();
    }
    public string fncChangePwd(string vPassword, string vPasswordSalt)
    {
        string vMailPassword = string.Empty;
        try
        {
            using (Authenticates objAuth = new Authenticates())
            {
                vMailPassword = objAuth.DecryptPassword(vPassword, vPasswordSalt);
            };
        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
        return vMailPassword;
    }
    private void ClearSearchField()
    {
        txtEmailIDSearch.Text = "";
        txtISPCode.Text = "";
        txtISPname.Text = "";
        txtMobileNumberSearch.Text = "";
        ddlUserStatus.SelectedValue = "2";
    }
}
