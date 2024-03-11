#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ====================================================================================================
 * <copyright company="Zed Axis Technologies">
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd. 
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
 * WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * </copyright>
 * ====================================================================================================
 * Created By : Sumit Maurya
 * Created On: 18-Oct-2016 
 * Description: This module helps to create bulk Orgn Hierarchy User.
 * ====================================================================================================
 * Reviewed By :
 * DD-MMM-YYYY, Name, #CCXX, Description
 ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 10-Nov-2016, Sumit Maurya, #CC01, Check added to validate correct file.
 * 28-Apr-2018, Sumit Maurya, #CC02, Incorrect column name was checked. Same has been changed (Done for Motorola).
 * 30-Apr-2018, Sumit Maurya, #CC03, New column RoleName has been added (Done for Motorola).
 * 16-April-2020,Vijay Kumar Prajapati,#CC04,Added CompanyId in this page for Saas based.
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
using System.Text.RegularExpressions;
using System.Data.SqlClient;

public partial class Masters_HO_Admin_OrgnHierarchyUserUpload : PageBase
{
    string strUploadedFileName = string.Empty;
    DataTable dtNew = new DataTable();
    int counter = 0;
    UploadFile UploadFile = new UploadFile();
    protected void Page_Load(object sender, EventArgs e)
    {
        hlnkInvalid.Text = "";  /* #CC04 Added */
        ucMsg.ShowControl = false;
        if (!IsPostBack)
        {
            ucMsg.Visible = false;

        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlUploadType.SelectedValue == "0")
            {
                ucMsg.ShowInfo("Please Select Activity.");
                return;
            }
            else
            {
                int intLoginIDMinLength = Convert.ToInt16(Resources.AppConfig.LoginIDMinLength.ToString());
                DataSet objDS = new DataSet();
                byte isSuccess = 1;
                Int16 UploadCheck = 0;
                String RootPath = Server.MapPath("~/");
                UploadFile.RootFolerPath = RootPath;

                UploadCheck = UploadFile.IsExcelFile(FileUpload1, ref strUploadedFileName);
                ViewState["TobeUploaded"] = strUploadedFileName;
                string strActivity = ddlUploadType.SelectedValue == "1" ? "AddOrgnHierarchyUser" : "UpdateOrgnHierarchyUser";
                if (UploadCheck == 1)
                {
                    //UploadFile.UploadedFileName = strUploadedFileName;
                    // UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eRetailerUpload;

                    isSuccess = UploadFile.uploadValidExcelRetailerWithCompanyId(ref objDS, strActivity);
                    int intError = 0;
                    /* #CC01 Add Start */
                    if (isSuccess != 0)
                    { /* #CC01 Add End */
                        if (objDS.Tables[0].Columns.Contains("Error"))
                        {
                            intError = 1;
                        }
                        if (!objDS.Tables[0].Columns.Contains("Error"))
                        {
                            objDS.Tables[0].Columns.Add("Error", typeof(string));
                        }
                        if (!objDS.Tables[0].Columns.Contains("Password"))
                            objDS.Tables[0].Columns.Add(AddColumn("", "Password", typeof(string)));
                        if (!objDS.Tables[0].Columns.Contains("PasswordSalt"))
                            objDS.Tables[0].Columns.Add(AddColumn("", "PasswordSalt", typeof(string)));
                        if (!objDS.Tables[0].Columns.Contains("Active"))
                            objDS.Tables[0].Columns.Add(AddColumn("", "Active", typeof(string)));

                        if (ddlUploadType.SelectedValue == "1")
                        {
                            int IsLoginIDUnique = Convert.ToInt16(Resources.AppConfig.IsLoginIDUnique.ToString());
                            string strDuplicateLoginID = string.Empty;
                            if (IsLoginIDUnique == 1)
                            {
                                string[] Columns = { "LoginID" }; //here we are fetching distinct record//
                                DataTable objDtUniqueLoginIDFile = objDS.Tables[0].DefaultView.ToTable(true, Columns);
                                DataView dvLoginIDError = objDtUniqueLoginIDFile.DefaultView;
                                dvLoginIDError.RowFilter = "LoginID<>''";
                                DataTable dtLoginIDError = dvLoginIDError.ToTable();
                                foreach (DataRow drLoginIDError in dtLoginIDError.Rows)
                                {
                                    string srtwe = "LoginID= '" + Convert.ToString(drLoginIDError["LoginID"]) + "'";
                                    DataView dvLoginIDError2 = objDS.Tables[0].DefaultView;
                                    dvLoginIDError2.RowFilter = "LoginID= '" + Convert.ToString(drLoginIDError["LoginID"]) + "'";
                                    if (dvLoginIDError2.Count > 1)
                                    {
                                        DataRow[] dr1 = objDS.Tables[0].Select(srtwe);
                                        foreach (DataRow dr3 in dr1)
                                        {
                                            dr3["Error"] = dr3["Error"].ToString() + "'" + drLoginIDError["LoginID"].ToString() + "' LoginID already present. ";
                                            isSuccess = 2;
                                        }
                                    }
                                }
                            }
                        } /* #CC01 Added */

                        foreach (DataRow dr in objDS.Tables[0].Rows)
                        {
                            if (ddlUploadType.SelectedValue == "1")
                            {
                                /* if (dr["PasswordSalt"].ToString().Length > 0) #CC02 Commented */
                                if (dr["Password"].ToString().Length > 0) /* #CC02 Added */
                                {
                                    using (Authenticates objAuthenticate = new Authenticates())
                                    {
                                        dr["PasswordSalt"] = objAuthenticate.GenerateSalt(dr["Password"].ToString().Trim().Length);
                                        dr["Password"] = objAuthenticate.EncryptPassword(dr["Password"].ToString(), dr["PasswordSalt"].ToString().Trim());
                                    }
                                }

                                if (dr["LoginID"].ToString().Length > 0 && dr["LoginID"].ToString().Length < intLoginIDMinLength)
                                {
                                    dr["Error"] = dr["Error"].ToString() + " LoginID length should be atleast " + intLoginIDMinLength + ". ";
                                    isSuccess = 2;
                                }
                            }

                            else if (ddlUploadType.SelectedValue == "2")
                            {
                                if (dr["Active"].ToString().Trim().Length > 0)
                                {
                                    if (dr["Active"].ToString().ToLower() != "yes")
                                    {
                                        if (dr["Active"].ToString().ToLower() != "no")
                                        {
                                            dr["Error"] = dr["Error"].ToString() + "Active must be only Yes or No. ";
                                            isSuccess = 2;
                                        }
                                    }
                                }
                                if (dr["Active"].ToString().Trim().Length == 0 && dr["LocationCode"].ToString().Trim().Length == 0 && dr["PersonName"].ToString().Trim().Length == 0 && dr["Email"].ToString().Trim().Length == 0 && dr["Mobile"].ToString().Trim().Length == 0)
                                {
                                    dr["Error"] = dr["Error"].ToString() + "Please provide atleast one record to update. ";
                                    isSuccess = 2;
                                }

                            }

                            string Regexpression = @"^[1-9]([0-9]{9})$";
                            string RegexpressionNum = @"^[0-9]*$";
                            string PhoneNumberRegex = Resources.AppConfig.PhoneNumberRegex.ToString().Replace("@", "");
                            int PhoneNumberMaxLength = Convert.ToInt16(Resources.AppConfig.RetailerPhoneNoMaxLength.ToString());
                            string PhoneNumberErrorMsg = Resources.AppConfig.RetailerPhoneNoErrorMsg.ToString();
                            int PhoneNoMinLength = Convert.ToInt16(Resources.AppConfig.RetailerPhoneNoMinLength.ToString());

                            if (Convert.ToInt64(dr["Mobile"].ToString().Length) > 0)// && Convert.ToInt64(dr["Mobile"].ToString().Length) < 10)
                            {
                                if (!Regex.IsMatch(dr["Mobile"].ToString(), RegexpressionNum))
                                {
                                    dr["Error"] = dr["Error"].ToString() + "Only numbers allowed in Mobile Number. ";
                                    isSuccess = 2;
                                }
                                else if (!Regex.IsMatch(dr["Mobile"].ToString(), Regexpression))
                                {
                                    dr["Error"] = dr["Error"].ToString() + "Please enter 10 digit Mobile number without 0 prefix. ";
                                    isSuccess = 2;
                                }
                            }
                        }
                        int IsEmailIDUnique = Convert.ToInt16(Resources.AppConfig.IsEmailUnique.ToString());
                        if (IsEmailIDUnique == 1)
                        {
                            string[] Columns = { "Email", "LocationCode" }; //here we are fetching distinct record//
                            
                            DataTable objDtUniqueEmailFile = objDS.Tables[0].DefaultView.ToTable(true, Columns);
                            DataView dvEmailError = objDtUniqueEmailFile.DefaultView;
                            dvEmailError.RowFilter = "Email<>''";
                            DataTable dtEmailError = dvEmailError.ToTable();
                            foreach (DataRow drEmailError in dtEmailError.Rows)
                            {
                                string srtwe = "Email= '" + Convert.ToString(drEmailError["Email"]) + "'";
                                DataView dvEmailError2 = objDS.Tables[0].DefaultView;
                                dvEmailError2.RowFilter = "Email= '" + Convert.ToString(drEmailError["Email"]) + "' and LocationCode='" + Convert.ToString(drEmailError["LocationCode"]) + "'";
                                if (dvEmailError2.Count > 1)
                                {
                                    DataRow[] dr1 = objDS.Tables[0].Select(srtwe);
                                    foreach (DataRow dr3 in dr1)
                                    {
                                        dr3["Error"] = dr3["Error"].ToString() + "'" + drEmailError["Email"].ToString() + "' Duplicate EmailID. ";
                                        isSuccess = 2;
                                    }
                                }
                            }
                        }
                    }
                    switch (isSuccess)
                    {
                        case 0:
                            ucMsg.ShowInfo(UploadFile.Message);
                            break;
                        case 2:

                            DataView dvError = objDS.Tables[0].DefaultView;
                            dvError.RowFilter = "Error<>''";
                            DataTable dtError = dvError.ToTable();
                            if (dtError.Columns.Contains("Password"))
                            {
                                dtError.Columns.Remove("Password");
                            }
                            if (dtError.Columns.Contains("PasswordSalt"))
                            {
                                dtError.Columns.Remove("PasswordSalt");
                            }
                            if (dtError.Columns.Contains("Active") && ddlUploadType.SelectedValue == "1")
                            {
                                dtError.Columns.Remove("Active");
                            }
                            if (dtError.Columns.Contains("UserID") && ddlUploadType.SelectedValue == "1")
                            {
                                dtError.Columns.Remove("UserID");
                            }
                            DataSet dsError = new DataSet();
                            dsError.Tables.Add(dtError);

                            ucMsg.ShowInfo("Click on Invalid Data link for invalid data.");
                            hlnkInvalid.Visible = true;
                            string strFileName = "InvalidData" + DateTime.Now.Ticks;

                            ExportInExcel(dsError, strFileName);
                            hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                            hlnkInvalid.Text = "Invalid Data";
                            break;
                        case 1:
                            //InsertData(objDS);
                            SaveOrgnhierarchy(objDS.Tables[0]);
                            break;
                    }
                    UploadFile.UploadedFileName = strUploadedFileName;
                    UploadFile.UploadValidationType = EnumData.eUploadExcelValidationType.eSales;
                    if (PageBase.SalesChanelID != 0)
                    {
                        UploadFile.issaleschannel = true;
                    }

                }
                else if (UploadCheck == 2)
                {
                    ucMsg.ShowInfo(Resources.Messages.UploadXlxs);
                }
                else if (UploadCheck == 3)
                {
                    ucMsg.ShowInfo(Resources.Messages.SelectFile);

                }

            }

        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.Message.ToString());
            //clsException.clsHandleException.fncHandleException(ex, "");
        }
    }




    protected void DwnldReferenceCode_Click(object sender, EventArgs e)
    {
        DataTable dt;
        DataSet ds = new DataSet();
        DataSet dsReferenceCode = new DataSet();
        OrgHierarchyData objOrgn = new OrgHierarchyData();
        
        objOrgn.UserID = PageBase.UserId;
        objOrgn.CompanyId = PageBase.ClientId;/*#CC04 Added*/
        objOrgn.PageIndex = -1;
        ds = objOrgn.GetOrgnhierarchyUserUploadData();
        if (objOrgn.TotalRecords > 0)
        {
            String FilePath = Server.MapPath("~/");
            string FilenameToexport = "HierarchyUserData";
            PageBase.RootFilePath = FilePath;
            PageBase.ExportToExecl(ds, FilenameToexport, EnumData.eTemplateCount.ePrimarysales1);
        }
        else
        {
            ucMsg.ShowInfo(Resources.Messages.NoRecord);
        }



    }
    /* #CC04 Add Start */

    public void SaveOrgnhierarchy(DataTable dt)
    {
        try
        {
            string guid = Guid.NewGuid().ToString();
            if (!dt.Columns.Contains("SessionID"))
                dt.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
            else
            {
                dt.Columns.Remove("SessionID");
                dt.Columns.Add(AddColumn(guid, "SessionID", typeof(System.String)));
            }
            if (!dt.Columns.Contains("CreatedBy"))
                dt.Columns.Add(AddColumn(Convert.ToString(PageBase.UserId), "CreatedBy", typeof(int)));
            if (!dt.Columns.Contains("Type"))
                dt.Columns.Add(AddColumn(ddlUploadType.SelectedValue, "Type", typeof(string)));
            if (!dt.Columns.Contains("UserID"))
                dt.Columns.Add(AddColumn(ddlUploadType.SelectedValue, "UserID", typeof(string)));
            if (!dt.Columns.Contains("LoginID"))
                dt.Columns.Add(AddColumn(ddlUploadType.SelectedValue, "LoginID", typeof(string)));


            if ((SaveOrgnhierarchyBCP(dt)) == true)
            {

                using (OrgHierarchyData objOrgn = new OrgHierarchyData())
                {
                    objOrgn.intType = Convert.ToInt16(ddlUploadType.SelectedValue);
                    objOrgn.SessionID = Convert.ToString(dt.Rows[0]["SessionID"]);
                    objOrgn.CompanyId = PageBase.ClientId;/*#CC04 Added*/
                    DataSet dsResult = objOrgn.OrgnHierarchyUserUpload();
                    if (objOrgn.intOutParam == 0)
                    {
                        ucMsg.ShowSuccess("Records updated successfully.");
                        ddlUploadType.SelectedValue = "0";

                    }

                    else if (objOrgn.intOutParam == 1)
                    {
                        ucMsg.ShowInfo("Click Invalid data link for invalid data.");
                        hlnkInvalid.Visible = true;
                        string strFileName = "InvalidData" + DateTime.Now.Ticks;
                        ExportInExcel(dsResult, strFileName);
                        hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                        hlnkInvalid.Text = "Invalid Data";
                        ViewState["SalesMan"] = null;
                    }
                    else
                    {
                        ucMsg.ShowError(objOrgn.Error);

                    }
                }

                /* using (SalesmanData objsalesman = new SalesmanData())
                 {
                     objsalesman.SessionID = Convert.ToString(dt.Rows[0]["SessionID"]);
                     objsalesman.SalesChannelID = Convert.ToInt32(PageBase.SalesChanelID);
                     DataSet dsResult = objsalesman.BulkSalesmanUpdate();
                     if (objsalesman.intOutParam == 0)
                     {
                         ucMsg.ShowSuccess("Records updated successfully.");

                     }
                     else if (objsalesman.intOutParam == 1)
                     {
                         ucMsg.ShowError(objsalesman.Error);

                         ViewState["SalesMan"] = null;
                     }
                     else if (objsalesman.intOutParam == 2)
                     {
                         ucMsg.ShowInfo("Click Invalid data link for invalid data.");
                         hlnkInvalid.Visible = true;
                         string strFileName = "InvalidData" + DateTime.Now.Ticks;
                         ExportInExcel(dsResult, strFileName);
                         hlnkInvalid.NavigateUrl = VirtualPath + strGlobalDownloadExcelPathRoot + strFileName + ".xlsx";
                         hlnkInvalid.Text = "Invalid Data";
                         ViewState["SalesMan"] = null;
                     }
                 }*/

            }
            else
            {
                ucMsg.ShowInfo("Unable to update data.");
            }


        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

    public bool SaveOrgnhierarchyBCP(DataTable dtUpload)
    {
        try
        {

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(PageBase.ConStr, SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = 20000;
                bulkCopy.DestinationTableName = "BulkUploadOrgnHierarchyUser";
                bulkCopy.ColumnMappings.Add("Type", "Type");
                bulkCopy.ColumnMappings.Add("UserID", "UserID");
                bulkCopy.ColumnMappings.Add("LocationCode", "LocationCode");
                bulkCopy.ColumnMappings.Add("PersonName", "PersonName");
                bulkCopy.ColumnMappings.Add("Email", "Email");
                bulkCopy.ColumnMappings.Add("Mobile", "Mobile");
                bulkCopy.ColumnMappings.Add("LoginID", "LoginID");
                bulkCopy.ColumnMappings.Add("Password", "Password");
                bulkCopy.ColumnMappings.Add("PasswordSalt", "PasswordSalt");
                bulkCopy.ColumnMappings.Add("Active", "Active");
                bulkCopy.ColumnMappings.Add("SessionID", "SessionID");
                bulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                bulkCopy.ColumnMappings.Add("RoleName", "RoleName"); /* #CC03 Added  */
                bulkCopy.WriteToServer(dtUpload);
                return true;
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            return false;
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
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }



    protected void BtnReset_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("OrgnHierarchyUserUpload.aspx", false);
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
}
