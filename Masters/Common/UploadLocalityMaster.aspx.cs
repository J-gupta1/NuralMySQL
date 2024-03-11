﻿#region Copyright(c) 2017 Zed-Axis Technologies All rights are reserved
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
* Created On: 23-March-2020
 * Description: This is  Upload Locality master   Page .
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
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;

public partial class Masters_Common_LocalityMaster : PageBase
{
    public static string _ConnectionString = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;
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
    private object ucmsg;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fillcountry();
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
                    else if (DsExcel.Tables[0].Columns.Contains("ActionType") && Rbtdownloadtemplate.SelectedValue == "1")
                    {
                        ucMsg.ShowInfo("You are uploading an update template, please upload save template.");
                        return;
                    }
                    else if (!DsExcel.Tables[0].Columns.Contains("ActionType") && Rbtdownloadtemplate.SelectedValue == "2")
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
                            objValidateFile.ExcelFileNameInTable = "SaveLocalityMaster";
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
                            objValidateFile.ExcelFileNameInTable = "UpdateLocalityMaster";
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
                dsTemplateCode = objSalesData.GetLocalityTemplate();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "SaveLocalityMaster";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "SaveLocalityMaster" };
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
                                DataSet dtInvalidRecordSet = objDetail.UploadLocalityMaster();
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
                                DataSet dtInvalidRecordSet = objDetail.UploadLocalityMaster();
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

    /*
    // new code
    public bool UserDetailBCP(DataTable dtUpload)
    {
        try
        {
            string connectMySQL = _ConnectionString;
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

            //Generate csv file from where data read
            CreateCSVfile(dtUpload, Server.MapPath(virtualFilePath));

            using (MySqlConnection cn1 = new MySqlConnection(connectMySQL))
            {
                cn1.Open();
                MySqlBulkLoader bcp1 = new MySqlBulkLoader(cn1);
                bcp1.TableName = "BulkUploadLocalityMaster"; //Create ProductOrder table into MYSQL database...
                bcp1.FieldTerminator = ",";
                bcp1.LineTerminator = "\r\n";
                bcp1.FileName = Server.MapPath(virtualFilePath);
                bcp1.NumberOfLinesToSkip = 0;
                bcp1.Load();

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
    */
    
       // new code
       public bool UserDetailBCP(DataTable dtUpload)
       {
           try
           {
               string connectMySQL = _ConnectionString;
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

            //dtUpload.Columns.Add("ErrorMessage", typeof(string)).DefaultValue = "";
            //dtUpload.Columns.Add("ErrorMessageStatus", typeof(System.Int16)).DefaultValue = 0;
            //dtUpload.Columns.Add("ActionType", typeof(string)).DefaultValue = "";
            //dtUpload.Columns.Add("CountryId", typeof(System.Int32)).DefaultValue = 0;

            //dtUpload.Columns.Add("ErrorMessage", typeof(System.String));
            //dtUpload.Columns["ErrorMessage"].Expression = "''";
            //dtUpload.Columns.Add("ErrorMessageStatus", typeof(System.Int16));
            //dtUpload.Columns["ErrorMessageStatus"].Expression = "'0'";
            //dtUpload.Columns.Add("ActionType", typeof(System.String));
            //dtUpload.Columns["ActionType"].Expression = null;
            //dtUpload.Columns.Add("CountryId", typeof(System.Int32));
            //dtUpload.Columns["CountryId"].Expression = "'0'";

            //Generate csv file from where data read
            CreateCSVfile(dtUpload, Server.MapPath(virtualFilePath));
                
               using (MySqlConnection cn1 = new MySqlConnection(connectMySQL))
               {
                   cn1.Open();
                   MySqlBulkLoader bulkCopy = new MySqlBulkLoader(cn1);
                bulkCopy.TableName = "BulkUploadLocalityMaster"; //Create ProductOrder table into MYSQL database...
                bulkCopy.FieldTerminator = ",";
                bulkCopy.LineTerminator = "\r\n";
                bulkCopy.FileName = Server.MapPath(virtualFilePath);
                bulkCopy.NumberOfLinesToSkip = 0;
                if (dtUpload.Columns.Contains("ActionType"))
                {
                    bulkCopy.Columns.AddRange(new[] {

                    "Country",
                    "Zone",
                    "RegionCode",
                    "Region",
                    "StateCode",
                    "State",
                    "DistrictCode",
                    "District",
                    "TehsilCode",
                    "Tehsil",
                    "CityCode",
                    "City",
                    "AreaCode",
                    "Area",
                    "Pincode",
                    "ActionType",
                    "TransactionUploadSessionId"
                    
                //"ErrorMessage",
                //"ErrorMessageStatus",
                //"ActionType",
                //"CountryId"
                });
                }
                else
                {
                    bulkCopy.Columns.AddRange(new[] {

                    "Country",
                    "Zone",
                    "RegionCode",
                    "Region",
                    "StateCode",
                    "State",
                    "DistrictCode",
                    "District",
                    "TehsilCode",
                    "Tehsil",
                    "CityCode",
                    "City",
                    "AreaCode",
                    "Area",
                    "Pincode",
                    "TransactionUploadSessionId"

                    }); }

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
       

    /*
        public bool UserDetailBCP(DataTable dtUpload)
        {
            try
            {
                string connectMySQL = _ConnectionString;
                //string strFile = "/TempFolder/MySQL" + DateTime.Now.Ticks.ToString() + ".csv";
                string strFile = "C:/NuralWork/MySQLNuralSFA/Excel/Upload/UploadExcelFiles/" + DateTime.Now.Ticks.ToString() + ".csv";

                //Create directory if not exist... Make sure directory has required rights..
                if (!Directory.Exists(Server.MapPath(strFile)))
                    Directory.CreateDirectory(Server.MapPath(strFile));

                //If file does not exist then create it and right data into it..
                if (!File.Exists(Server.MapPath(strFile)))
                {
                    FileStream fs = new FileStream(Server.MapPath(strFile), FileMode.Create, FileAccess.Write);
                    fs.Close();
                    fs.Dispose();
                }

                //Generate csv file from where data read
                CreateCSVfile(dtUpload, Server.MapPath(strFile));
                using (MySqlConnection cn1 = new MySqlConnection(connectMySQL))
                {
                    cn1.Open();
                    MySqlBulkLoader bcp1 = new MySqlBulkLoader(cn1);
                    bcp1.TableName = "BulkUploadLocalityMaster"; //Create ProductOrder table into MYSQL database...
                    bcp1.FieldTerminator = ",";

                    bcp1.LineTerminator = "\r\n";
                    bcp1.FileName = Server.MapPath(strFile);
                    bcp1.NumberOfLinesToSkip = 0;
                    bcp1.Load();

                    //Once data write into db then delete file..
                    try
                    {
                        File.Delete(Server.MapPath(strFile));
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

    */

    #region comment old method
    /*
      public bool UserDetailBCP(DataTable dtUpload)
      {
       try
      {

                  
                 using (SqlBulkCopy bulkCopy = new SqlBulkCopy(_ConnectionString, SqlBulkCopyOptions.KeepIdentity))
                {
              bulkCopy.BatchSize = 20000;
              bulkCopy.DestinationTableName = "BulkUploadLocalityMaster";
              if (dtUpload.Columns.Contains("CountryId"))
              {
                  bulkCopy.ColumnMappings.Add("CountryId", "CountryId");
              }
              if (dtUpload.Columns.Contains("Country"))
              {
                  bulkCopy.ColumnMappings.Add("Country", "Country");
              }
              if (dtUpload.Columns.Contains("Zone"))
              {
                  bulkCopy.ColumnMappings.Add("Zone", "Zone");
              }
              if (dtUpload.Columns.Contains("RegionCode"))
              {
                  bulkCopy.ColumnMappings.Add("RegionCode", "RegionCode");
              }
              if (dtUpload.Columns.Contains("Region"))
              {
                  bulkCopy.ColumnMappings.Add("Region", "Region");
              }
              if (dtUpload.Columns.Contains("StateCode"))
              {
                  bulkCopy.ColumnMappings.Add("StateCode", "StateCode");
              }
              if (dtUpload.Columns.Contains("State"))
              {
                  bulkCopy.ColumnMappings.Add("State", "State");
              }
              if (dtUpload.Columns.Contains("DistrictCode"))
              {
                  bulkCopy.ColumnMappings.Add("DistrictCode", "DistrictCode");
              }
              if (dtUpload.Columns.Contains("District"))
              {
                  bulkCopy.ColumnMappings.Add("District", "District");
              }
              if (dtUpload.Columns.Contains("TehsilCode"))
              {
                  bulkCopy.ColumnMappings.Add("TehsilCode", "TehsilCode");
              }

              if (dtUpload.Columns.Contains("Tehsil"))
              {
                  bulkCopy.ColumnMappings.Add("Tehsil", "Tehsil");
              }
              if (dtUpload.Columns.Contains("CityCode"))
              {
                  bulkCopy.ColumnMappings.Add("CityCode", "CityCode");
              }
              if (dtUpload.Columns.Contains("City"))
              {
                  bulkCopy.ColumnMappings.Add("City", "City");
              }
              if (dtUpload.Columns.Contains("AreaCode"))
              {
                  bulkCopy.ColumnMappings.Add("AreaCode", "AreaCode");
              }
              if (dtUpload.Columns.Contains("Area"))
              {
                  bulkCopy.ColumnMappings.Add("Area", "Area");
              }
              if (dtUpload.Columns.Contains("Pincode"))
              {
                  bulkCopy.ColumnMappings.Add("Pincode", "Pincode");
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


      }*/

    #endregion
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
                dsTemplateCode = objSalesData.GetLocalityTemplate();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "UpdateLocalityMaster";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "UpdateLocalityMaster" };
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
    protected void DownloadReferenceCodeForSave_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 5;
                dsTemplateCode = objSalesData.GetMaterialMasterReferenceData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {
                    if (dsTemplateCode.Tables[0].Columns.Contains("CountryID"))
                    {
                        dsTemplateCode.Tables[0].Columns.Remove("CountryID");
                        dsTemplateCode.Tables[0].AcceptChanges();
                    }
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ReferenceCode";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "LocalityDetail" };
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
    protected void DownloadReferenceCodeForUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsTemplateCode = new DataSet();
            using (CommonMaster objSalesData = new CommonMaster())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.CompanyID = PageBase.ClientId;
                objSalesData.TemplateType = 5;
                dsTemplateCode = objSalesData.GetMaterialMasterReferenceData();
                if (dsTemplateCode != null && dsTemplateCode.Tables.Count > 0)
                {

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ReferenceCode";
                    PageBase.RootFilePath = FilePath;
                    string[] strExcelSheetName = { "LocalityDetail" };
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
    protected void btnSerCode_Click(object sender, EventArgs e)
    {
        if (cmbSerState.SelectedValue == "0" && cmbSerCountry.SelectedValue == "0")
        {

            if (PageBase.TehsillDisplayMode == "1" && cmbSerTehsil.SelectedValue == "0")
            {
                ucMsg.ShowInfo("Please enter atleast one searching parameter ");
                return;
            }

            ucMsg.ShowInfo("Please enter atleast one searching parameter ");
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
    void fillcountry()
    {
        using (MastersData obj = new MastersData())
        {
            DataTable dt;

            obj.CountrySelectionMode = 1;
            obj.CompanyId = PageBase.ClientId;
            dt = obj.SelectCountryInfo();
            String[] colArray = { "CountryID", "CountryName" };
            PageBase.DropdownBinding(ref cmbSerCountry, dt, colArray);
            cmbSerState.Items.Insert(0, new ListItem("Select", "0"));
            cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));
            cmbSerCity.Items.Insert(0, new ListItem("Select", "0"));
            cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));

        }

    }
    protected void cmbSerCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {
            try
            {
                if (cmbSerCity.SelectedValue == "0")
                {
                    cmbSerTehsil.Items.Clear();
                    cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    if (PageBase.TehsillDisplayMode == "1")
                    {
                        cmbSerTehsil.ClearSelection();
                        objmaster.tehsilCityId = Convert.ToInt16(cmbSerCity.SelectedValue.ToString());
                        objmaster.tehsillselectionmode = 1;
                        objmaster.CompanyId = PageBase.ClientId;
                        DataTable dtdistfil = objmaster.SelectTahsillInfo();
                        cmbSerTehsil.DataSource = dtdistfil;
                        cmbSerTehsil.DataTextField = "TehsilName";
                        cmbSerTehsil.DataValueField = "TehsilID";
                        cmbSerTehsil.DataBind();
                        cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
                    }
                }
            }
            catch (Exception ex)
            {
                ucMsg.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    protected void cmbSerDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {

            try
            {
                if (cmbSerDistrict.SelectedValue == "0")
                {
                    cmbSerCity.Items.Clear();
                    cmbSerCity.Items.Insert(0, new ListItem("Select", "0"));

                }
                else
                {
                    objmaster.CityDistrictId = Convert.ToInt16(cmbSerDistrict.SelectedValue);
                    objmaster.CitySelectionMode = 1;
                    objmaster.CompanyId = PageBase.ClientId;
                    DataTable dtcityfil = objmaster.SelectCityInfo();
                    String[] colArray = { "CityID", "CityName" };
                    PageBase.DropdownBinding(ref cmbSerCity, dtcityfil, colArray);
                    cmbSerCity.SelectedValue = "0";

                }

            }
            catch (Exception ex)
            {
                ucMsg.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    protected void cmbSerState_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {
            cmbSerTehsil.Items.Clear();
            cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
            cmbSerCity.Items.Clear();
            cmbSerCity.Items.Insert(0, new ListItem("Select", "0"));
            try
            {
                if (cmbSerState.SelectedValue == "0")
                {
                    cmbSerDistrict.Items.Clear();

                    cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));


                }
                else
                {

                    cmbSerDistrict.Items.Clear();
                    objmaster.DistrictStateId = Convert.ToInt16(cmbSerState.SelectedValue.ToString());
                    objmaster.DistrictSelectionMode = 1;
                    objmaster.CompanyId = PageBase.ClientId;
                    DataTable dtdistfil = objmaster.SelectDistrictInfo();
                    String[] colArray = { "DistrictID", "DistrictName" };
                    PageBase.DropdownBinding(ref cmbSerDistrict, dtdistfil, colArray);
                    cmbSerDistrict.SelectedValue = "0";
                }
            }
            catch (Exception ex)
            {
                ucMsg.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    protected void cmbSerCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        cmbSerDistrict.Items.Clear();
        cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));
        cmbSerCity.Items.Clear();
        cmbSerCity.Items.Insert(0, new ListItem("Select", "0"));
        cmbSerTehsil.Items.Clear();
        cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
        if (cmbSerCountry.SelectedValue == "0")
        {
            cmbSerState.Items.Clear();
            cmbSerState.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            cmbSerState.Items.Clear();
            using (MastersData obj = new MastersData())
            {
                DataTable dt;
                obj.StateSelectionMode = 1;
                obj.CompanyId = PageBase.ClientId;
                obj.StateCountryid = Convert.ToInt32(cmbSerCountry.SelectedValue);
                dt = obj.SelectStateInfo();
                String[] colArray = { "StateID", "StateName" };
                PageBase.DropdownBinding(ref cmbSerState, dt, colArray);
            }
        }
    }
    protected void exportToExel_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        using (MastersData objmaster = new MastersData())
        {

            ucMsg.Visible = false;
            objmaster.CountryId = Convert.ToInt32(cmbSerCountry.SelectedValue);
            if (cmbSerState.SelectedValue != "0")
            {
                objmaster.StateId = Convert.ToInt16(cmbSerState.SelectedValue.ToString());
            }
            else
            {
                objmaster.StateId = 0;
            }

            if (cmbSerDistrict.SelectedValue != "0")
            {
                objmaster.DistrictId = Convert.ToInt16(cmbSerDistrict.SelectedValue.ToString());

            }
            else
            {
                objmaster.DistrictId = 0;
            }
            if (cmbSerCity.SelectedValue != "0")
            {
                objmaster.CityId = Convert.ToInt16(cmbSerCity.SelectedValue.ToString());

            }
            else
            {
                objmaster.CityId = 0;
            }
            if (cmbSerTehsil.SelectedValue != "0")
            {
                objmaster.tehsillid = Convert.ToInt16(cmbSerTehsil.SelectedValue.ToString());

            }
            else
            {
                objmaster.tehsillid = 0;
            }
            objmaster.CompanyId = PageBase.ClientId;

            try
            {
                dt = objmaster.SelectAllLocalityInfo();
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "LocalityDetails";
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
    protected void grdArea_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdArea.PageIndex = e.NewPageIndex;
        databind();
    }
    public void databind()
    {
        DataTable dt = new DataTable();
        using (MastersData objmaster = new MastersData())
        {

            ucMsg.Visible = false;
            objmaster.CountryId = Convert.ToInt32(cmbSerCountry.SelectedValue);
            if (cmbSerState.SelectedValue != "0")
            {
                objmaster.StateId = Convert.ToInt16(cmbSerState.SelectedValue.ToString());
            }
            else
            {
                objmaster.StateId = 0;
            }

            if (cmbSerDistrict.SelectedValue != "0")
            {
                objmaster.DistrictId = Convert.ToInt16(cmbSerDistrict.SelectedValue.ToString());

            }
            else
            {
                objmaster.DistrictId = 0;
            }
            if (cmbSerCity.SelectedValue != "0")
            {
                objmaster.CityId = Convert.ToInt16(cmbSerCity.SelectedValue.ToString());

            }
            else
            {
                objmaster.CityId = 0;
            }
            if (cmbSerTehsil.SelectedValue != "0")
            {
                objmaster.tehsillid = Convert.ToInt16(cmbSerTehsil.SelectedValue.ToString());

            }
            else
            {
                objmaster.tehsillid = 0;
            }
            objmaster.CompanyId = PageBase.ClientId;

            try
            {
                dt = objmaster.SelectAllLocalityInfo();
                grdArea.DataSource = dt;
                grdArea.DataBind();

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

        cmbSerCountry.SelectedValue = "0";
        cmbSerState.Items.Clear();
        cmbSerState.Items.Insert(0, new ListItem("Select", "0"));
        cmbSerCity.Items.Clear();
        cmbSerCity.Items.Insert(0, new ListItem("Select", "0"));
        cmbSerDistrict.Items.Clear();
        cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));
        cmbSerDistrict.SelectedValue = "0";
        cmbSerState.ClearSelection();

        cmbSerTehsil.Items.Clear();
        cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));

        UpdSearch.Update();

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

