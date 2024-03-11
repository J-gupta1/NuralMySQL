/*
====================================================================================================================================
 *Change Log: 
 *DD-MMM-YYYY, Name , #CCXX - Description
------------------------------------------------------------------------------------------------------------------------------------
 * 22-Mar-2016, Sumit Maurya, #CC01, Issue of page getting blocked on the execution of event(s) due to update panel resolved.
 * 03-Aug-2016, Karam Chand Sharma, #CC02, Emailid enabled on in update
 * 16-Aug-2016, Rakesh Goel, #CC03 - Added Location code in export to excel and grid view
 * 22-Sep-2016, Sumit Maurya, #CC04, New Label name First Name changed to Full Name, Last name and display name is commente as it is not required. Mobile number field added to save and display in grid. Search field from mobile number added.
 * 26-Dec-2016,Karam Chand Sharma, #CC05, User login name editable according to configurable which is define in applicationconfiguration table name with ISLOGINEDITABLE
 * 07-Jul-2017, Sumit Maurya, #CC06,if this interface access is given to any other user except aplication admin / SuperAdmin then remove ApplicationAdmin option from User dropdown option for creating user. (Done for Comio)
 * 18-Jul-2017, Sumit Maurya, #CC07, UserID supplied to get dropdown value according
 *  20 Dec 2017,Vijay Kumar Prajapati,#CC08,User Cap Creation For Comio.
====================================================================================================================================
 */
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;

public partial class Masters_HO_Admin_ManageUser : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            ucMsg.Visible = false;
            if (!IsPostBack)
            {
                chkActive.Checked = true;
                if (ViewState["EditUserId"] != null)
                { ViewState.Remove("EditUserId"); }
                BindRoles();
                FillUserGrid();
                BindHierarchy();
                BindState();
                BindBrandCategory();
                BindParentHierarchy();
                fillBrandCategoryDDL();
                if (PageBase.IsSuperAdmin == true)
                    pnlAllowAllHierarchy.Visible = false;

                if(PageBase.UserOtherDetail==1)
                {
                    OtherDetail.Visible = true;
                }
                else
                {
                    OtherDetail.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    public void fillBrandCategoryDDL()
    {

        using (ProductData objproduct = new ProductData())
        {

            try
            {
                objproduct.CompanyId = PageBase.ClientId;
                DataTable dtbrandfil = objproduct.SelectAllBrandInfo();
                String[] colArray = { "BrandID", "BrandName" };
                PageBase.DropdownBinding(ref ddlBrand, dtbrandfil, colArray);
                ddlBrand.SelectedValue = "0";
                
                DataTable dtprodcatfil = objproduct.SelectAllProdCatInfo();
                String[] colArray1 = { "ProductCategoryID", "ProductCategoryName" };
                PageBase.DropdownBinding(ref ddlProdCat, dtprodcatfil, colArray1);
                ddlProdCat.SelectedValue = "0";

            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);

            }
        }
    }

    protected void btnActiveDeactive_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btnActiveDeactive = (ImageButton)sender;
            Int32 Result = 0;
            Int32 UserID = Convert.ToInt32(btnActiveDeactive.CommandArgument);
            using (UserData ObjUser = new UserData())
            {

                ObjUser.UserID = UserID;
                ObjUser.CompanyID = PageBase.ClientId;
                Result = ObjUser.UpdateStatusUserInfo();
            };
            if (Result >= 1)
            {
                ucMsg.ShowSuccess(Resources.Messages.StatusChanged);
            }
            else
            {
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }

            if (ViewState["Search"] != null)
            {
                btnSearchUser_Click(btnSearchUser, new EventArgs());
            }
            else
            {
                FillUserGrid(); ClearForm(); ViewState["Search"] = null;
            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
        //updgrid.Update();
    }
    protected void btnCreateUser_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }

            if (txtFUserName.Text.Trim().Length == 0 || ddlUserRole.SelectedIndex == 0 /* || txtLUserName.Text.Trim().Length == 0 #CC04 Commented */ ||
         txtEmail.Text.Trim().Length == 0 || txtLoginName.Text.Trim().Length == 0)
            {
                ucMsg.ShowInfo(Resources.Messages.MandatoryField);

                return;
            }
            if (Convert.ToInt32(ViewState["EditUserId"]) == 0)
            {
                if (txtPassword.Text.Trim().Length == 0)
                {
                    ucMsg.ShowInfo(Resources.Messages.Enterpassword);
                    return;
                }
            }
            if (Convert.ToInt16(ddlUserRole.SelectedValue) > 0 && txtLoginName.Text.Trim().Length >= 0 &&
                 txtFUserName.Text.Trim().Length > 0 && txtEmail.Text.Trim().Length > 0)
            {
                string strSelectedRegion = "";
                if (Convert.ToString(ViewState["ReportHierarchyLevel"]) != "1" && Convert.ToString(ViewState["DefaultRole"]) != "0")
                {
                    strSelectedRegion = xmlStringGenerator(chkRegion);
                    if (strSelectedRegion == string.Empty)
                    {
                        ucMsg.ShowInfo(Resources.Messages.SelectUserLocation);
                        return;
                    }
                }

                Int32 result = 0;
                string StrPSalt = "", StrPwd = "";
                if (Convert.ToInt32(ViewState["EditUserId"]) == 0)
                {

                    using (Authenticates objAuthenticate = new Authenticates())
                    {
                        StrPSalt = objAuthenticate.GenerateSalt(txtPassword.Text.Trim().Length);
                        StrPwd = objAuthenticate.EncryptPassword(txtPassword.Text.Trim(), StrPSalt);
                    };
                    if (txtLoginName.Text.Trim() != "")
                    {
                        if (txtLoginName.Text.Trim().Replace(" ", "").Length != txtLoginName.Text.Trim().Length)
                        {
                            ucMsg.ShowWarning("Blank space not allowed in user name.");
                            return;
                        }
                    }
                    if (txtPassword.Text.Trim() != "")
                    {
                        if (txtPassword.Text.Trim().Replace(" ", "").Length != txtPassword.Text.Trim().Length)
                        {
                            ucMsg.ShowWarning("Blank space not allowed in password.");
                            return;
                        }
                    }
                }
                using (UserData objuser = new UserData())
                {

                    if (Convert.ToInt32(ViewState["EditUserId"]) == 0)
                    {
                        objuser.UserID = 0;
                        objuser.Password = StrPwd;
                        objuser.PasswordSalt = StrPSalt;
                    }
                    else
                    {
                        objuser.UserID = Convert.ToInt32(ViewState["EditUserId"]);
                    }
                    /*  objuser.DisplayName = txtDUserName.Text.Trim(); #CC04 Commented */
                    objuser.CompanyID = Convert.ToInt16(Session["CompanyId"]);
                    objuser.EmailID = txtEmail.Text.Trim();
                    objuser.FirstName = txtFUserName.Text.Trim();
                    /* objuser.LastName = txtLUserName.Text.Trim();  #CC04 Commented */
                    objuser.UserLoginName = txtLoginName.Text.Trim();
                    objuser.UserRoleID = Convert.ToInt16(ddlUserRole.SelectedValue);
                    objuser.Status = Convert.ToBoolean(chkActive.Checked);
                    objuser.AllowHierarchy = chkAllowAllHierarchy.Checked;
                    objuser.SelectedRegions = strSelectedRegion;
                    objuser.IsUserMapped = 1;
                    objuser.PasswordExpiryDays = 365;
                    objuser.StrPassword = txtPassword.Text.Trim();
                    /* #CC03 Add Start */
                    if (!string.IsNullOrEmpty(txtmobile.Text.Trim()))
                        objuser.MobileNumber = txtmobile.Text.Trim();
                    objuser.LastName = null;
                    objuser.CompanyID = PageBase.ClientId;
                    objuser.Lat = txtLatitude.Text.Trim();
                    objuser.Long = txtLongitude.Text.Trim();
                    objuser.GeoRadius = txtGeoFancingRadius.Text.Trim();
                    /* #CC03 Add End */
                    result = objuser.InsertUpdateUserinfo();
                    if (objuser.ReturnValue == 2 || objuser.ReturnValue == 3)
                    {
                        ucMsg.ShowError(Resources.Messages.DuplicateInformation);
                        return;
                    }
                    if (objuser.ReturnValue == 1)
                    {
                        ucMsg.ShowError(Resources.Messages.NoMapping);
                        return;
                    }
                    if (objuser.ErrorMessage != "")
                    {/*#CC08 Added Started*/
                        if (objuser.ErrorMessage.ToLower().Contains("active user count"))
                        {
                            ucMsg.ShowError("Active user count is exceeding the limit defined. Please Contact administrator.");
                        }
                        else/*#CC08 Added End*/
                            ucMsg.ShowError(objuser.ErrorMessage);
                        return;
                    }
                };

                if (result > 0)
                {
                    if (ViewState["EditUserId"] == null)
                    {
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        //SendMailToUser(result);******************************************************
                    }
                    else
                    {
                        ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                    }


                    updAddUserMain.Update();
                    chkActive.Checked = true;
                    if (ViewState["Search"] != null)
                    {
                        btnSearchUser_Click(btnSearchUser, new EventArgs());
                    }
                    else
                    {
                        FillUserGrid(); ViewState["Search"] = null;
                    }
                    ClearForm();

                    return;
                }
                else
                {
                    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    return;
                }

            }

        }

        catch (Exception ex)
        {
            /*#CC08 Added Started*/
            if (ex.Message.ToLower().Contains("trgusercount"))
            {
                ucMsg.ShowError("Active user count is exceeding the limit defined. Please Contact administrator.");
            }
            else/*#CC08 Added End*/
                ucMsg.ShowError(ex.Message.ToString());
            ucMsg.Visible = true;
            PageBase.Errorhandling(ex);

        }


    }
    protected override void OnPreRender(EventArgs e)
    {
        txtPassword.Attributes.Add("value", txtPassword.Text);
        base.OnPreRender(e);
    }
    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        ddlUserRole.SelectedIndex = -1;

        txtLoginName.Enabled = false;
        /*#CC05 START COMMENTED txtLoginName.Enabled = false; #CC05 START END*/
        /*#CC05 START ADDED*/
        chkActive.Enabled = true;
        if (PageBase.ISLOGINEDITABLE == "0")
            txtLoginName.Enabled = false;
        else if (PageBase.ISLOGINEDITABLE == "1")
            txtLoginName.Enabled = true;
        /*#CC05 START END*/
        /*#CC02 START COMMENTED txtEmail.Enabled = false;#CC02 END COMMENTED */
        ImageButton btnEdit = (ImageButton)sender;
        DataTable dtUser;
        using (UserData objuser = new UserData())
        {
            objuser.RestrictUserID = Convert.ToInt32(PageBase.UserId);
            objuser.CompanyID = PageBase.ClientId;
            dtUser = objuser.GetUsersInfo(Convert.ToInt32(btnEdit.CommandArgument), Convert.ToInt16(2));
        };
        ViewState["EditUserId"] = Convert.ToInt32(btnEdit.CommandArgument);

        if (dtUser != null && dtUser.Rows.Count > 0)
        {
            if (ddlUserRole.Items.FindByValue(dtUser.Rows[0]["RoleID"].ToString()) != null)
            {
                ddlUserRole.Items.FindByValue(dtUser.Rows[0]["RoleID"].ToString()).Selected = true;
                ddlUserRole_SelectedIndexChanged(ddlUserRole, new EventArgs());
                ddlUserRole.Enabled = false;

                DataTable dtRegion;
                using (UserData objuser = new UserData())
                {
                    dtRegion = objuser.prcGetAvailedLocationsByUserId(Convert.ToInt32(ViewState["EditUserId"]));
                };
                foreach (ListItem lst in chkRegion.Items)
                {
                    DataRow[] dr = dtRegion.Select("OrgnhierarchyID=" + lst.Value);
                    if (dr.Length > 0)
                    { lst.Selected = true; lst.Enabled = true; }
                    else
                    { lst.Selected = false; }
                }

            }
            else
            {
                ddlUserRole.Enabled = false;
                pnlRegion.Visible = false;
            }
            txtFUserName.Text = (dtUser.Rows[0]["FirstName"].ToString());
            /* #CC04 Comment Start txtLUserName.Text = (dtUser.Rows[0]["LastName"].ToString()); 
               txtDUserName.Text = (dtUser.Rows[0]["DisplayName"].ToString());  #CC04 Comment End */
            txtmobile.Text = (dtUser.Rows[0]["MobileNo"].ToString()); /* #CC04 Added */
            txtEmail.Text = (dtUser.Rows[0]["Email"].ToString());
            txtLoginName.Text = Convert.ToString(dtUser.Rows[0]["LoginName"].ToString());
            chkActive.Checked = Convert.ToBoolean(dtUser.Rows[0]["Status"]);
            chkAllowAllHierarchy.Checked = Convert.ToBoolean(dtUser.Rows[0]["AllowAllHierarchy"]);
            txtLatitude.Text = dtUser.Rows[0]["Latitude"].ToString();
            txtLongitude.Text = dtUser.Rows[0]["Longitude"].ToString();
            txtGeoFancingRadius.Text = dtUser.Rows[0]["GeoFancingRadius"].ToString();
            btnCreateUser.Text = "Update";
            reqRfv.ErrorMessage = "";
            updAddUserMain.Update();

        }
    }
    protected void grdvwUserList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvwUserList.PageIndex = e.NewPageIndex;
        FillUserGrid();

    }
    protected void btnSearchUser_Click(object sender, EventArgs e)
    {
        ViewState["Search"] = "Search";
        FillUserGrid();

    }
    protected void ddlUserRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindLocation();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearForm();
        FillUserGrid();

    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        ClearForm();
        FillUserGrid();
    }
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string Password = string.Empty;
            DataTable dtUsers;
            using (UserData objuser = new UserData())
            
            {
                objuser.RestrictUserID = Convert.ToInt32(PageBase.UserId);
                /* #CC04 Add Start */
                objuser.EmailID = txtEmailIDSearch.Text.Trim();
                objuser.MobileNumber = string.IsNullOrEmpty(txtMobileNumberSearch.Text.Trim()) ? "" : txtMobileNumberSearch.Text.Trim(); /* #CC04 Add End */ /* #CC04 Add End */
                objuser.CompanyID = PageBase.ClientId;
                if (!string.IsNullOrEmpty(ddlBrand.SelectedValue))
                    objuser.BrandId = Convert.ToInt32(ddlBrand.SelectedValue);
                if (!string.IsNullOrEmpty(ddlProdCat.SelectedValue))
                    objuser.ProdCatId = Convert.ToInt32(ddlProdCat.SelectedValue);
                dtUsers = objuser.GetUsersInfo(Convert.ToInt16(ddlUserType.SelectedValue), Convert.ToInt16(ddlUserStatus.SelectedValue), (txtDisplayname.Text.Trim()));
            };
            if (dtUsers != null && dtUsers.Rows.Count > 0)
            {
                foreach (DataRow dr in dtUsers.Rows)
                {
                    using (Authenticates ObjAuth = new Authenticates())
                    {
                        Password = ObjAuth.DecryptPassword(Convert.ToString(dr["Password"]), Convert.ToString(dr["PasswordSalt"]));
                    };
                    dr["Password"] = Password;
                }
                string[] DsCol = new string[] { "RoleName", "Email", "MobileNo", "DisplayName", "LoginName", "Password", "StatusText", "UserLocationCodes", "Latitude", "Longitude", "GeoFancingRadius","Brands","ProdCats" }; /* #CC04 new column MobileNo added to export in excel. Column "Name" removed from export to excel. */
                DataTable DsCopy = new DataTable();
                dtUsers = dtUsers.DefaultView.ToTable(true, DsCol);
                dtUsers.Columns["StatusText"].ColumnName = "Status";
                dtUsers.Columns["UserLocationCodes"].ColumnName = "Location Codes";  //#CC03 added
                dtUsers.Columns["DisplayName"].ColumnName = "Full Name"; /* #CC04 Added */
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dtUsers);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../../");
                string FilenameToexport = "UserDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);

            }
            else
            {
                ucMsg.ShowInfo(Resources.Messages.NoRecord);
            }
        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    #region User Defind Function
    private void ClearForm()
    {
        ViewState["EditUserId"] = null;
        ddlUserRole.Enabled = true;
        if (ddlUserRole.SelectedIndex!=-1)
        ddlUserRole.SelectedIndex = 0;
        ddlUserType.SelectedIndex = 0;
        ddlBrand.SelectedIndex = 0;
        ddlProdCat.SelectedIndex = 0;
        txtFUserName.Text = "";
        /*  txtLUserName.Text = ""; #CC04 Commented */
        txtDisplayname.Text = "";
        /*  txtDUserName.Text = ""; #CC04 Commented */
        txtEmail.Enabled = true;
        txtEmail.Text = "";
        txtLoginName.Text = "";
        txtPassword.Text = "";
        txtLoginName.Enabled = true;
        pnlRegion.Visible = false;
        chkActive.Checked = true;
        btnCreateUser.Text = "Submit";
        ddlUserStatus.SelectedIndex = 0;
        ViewState["Search"] = null;
        chkAllowAllHierarchy.Checked = false;
        /* #CC04 Add Start  */
        txtMobileNumberSearch.Text = "";
        txtEmailIDSearch.Text = "";
        txtmobile.Text = "";
        txtGeoFancingRadius.Text = "";
        txtLatitude.Text = "";
        txtLongitude.Text = "";

        /* #CC04 Add End */
    }
    private void BindLocation()
    {
        try
        {
            chkRegion.Items.Clear();
            DataTable dtavailed;
            DataTable dtLocations;
            using (UserData objUser = new UserData())
            
            {
                objUser.UserRoleID = Convert.ToInt16(ddlUserRole.SelectedValue);
                objUser.CompanyID = PageBase.ClientId;
                dtLocations = objUser.GetLocationListbyUserRoleID();
                chkRegion.DataSource = dtLocations;
                chkRegion.DataTextField = "LocationName";
                chkRegion.DataValueField = "OrgnhierarchyID";
                chkRegion.DataBind();
                
                dtavailed = objUser.GetAvailedLocations();
                if (dtLocations.Rows.Count > 0)
                {
                    ViewState["ReportHierarchyLevel"] = Convert.ToString(dtLocations.Rows[0]["ReportHierarchyLevel"]);
                    ViewState["DefaultRole"] = Convert.ToString(dtLocations.Rows[0]["DefaultRole"]);
                    pnlRegion.Visible = true;
                }
                else
                {
                    ViewState["ReportHierarchyLevel"] = "";
                    ViewState["DefaultRole"] = "";
                    pnlRegion.Visible = false;
                }
            };
            foreach (ListItem lst in chkRegion.Items)
            {
                DataRow[] dr = dtavailed.Select("Status=1 AND OrgnhierarchyID=" + lst.Value);
                if (dr.Length > 0)
                { lst.Enabled = false; }
                else
                { lst.Enabled = true; }
            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    private void BindRoles()
    {
        try
        {
            DataTable dt = new DataTable();

            ddlUserRole.Items.Clear();
            using (UserData objuser = new UserData())
           
            {
                objuser.SearchType = 1;
                objuser.Status = true;
                objuser.UserID = PageBase.UserId; /* #temp code Added */
                objuser.CompanyID = PageBase.ClientId;
                dt = objuser.GetUserRoleUserMaster();
                /* #CC06 Add Start */
                /*if (PageBase.IsSuperAdmin == false)  if(PageBase.RoleID!=1) 
                {
                    if(dt!=null && dt.Rows.Count>0)
                    {                        
                        for(int i=0;i<dt.Rows.Count;i++)
                        {
                            //if(dt.Rows[i]["RoleID"].ToString()=="1")
                            if (dt.Rows[i]["RoleName"].ToString() == "Application Admin")
                            {
                                dt.Rows[i].Delete();
                                dt.AcceptChanges();
                                break;
                            }
                        }
                    }
                }*//* #CC06 Add End */
            };
            String[] colArray = { "RoleId", "RoleName" };
            PageBase.DropdownBinding(ref ddlUserRole, dt, colArray);
            PageBase.DropdownBinding(ref ddlUserType, dt, colArray);
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
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
    private void FillUserGrid()
    {
        DataTable dtUsers;
        using (UserData objuser = new UserData())
        {
            objuser.RestrictUserID = Convert.ToInt32(PageBase.UserId);
            /* #CC04 Add Start */
            objuser.EmailID = txtEmailIDSearch.Text.Trim();
            objuser.MobileNumber = string.IsNullOrEmpty(txtMobileNumberSearch.Text.Trim()) ? "" : txtMobileNumberSearch.Text.Trim(); /* #CC04 Add End */
            objuser.CompanyID = PageBase.ClientId;
            if (!string.IsNullOrEmpty( ddlBrand.SelectedValue))
            objuser.BrandId =Convert.ToInt32( ddlBrand.SelectedValue);
            if (!string.IsNullOrEmpty(ddlProdCat.SelectedValue))
            objuser.ProdCatId = Convert.ToInt32(ddlProdCat.SelectedValue);
            dtUsers = objuser.GetUsersInfo(Convert.ToInt16(ddlUserType.SelectedValue), Convert.ToInt16(ddlUserStatus.SelectedValue), (txtDisplayname.Text.Trim()));
        };
        if (dtUsers != null && dtUsers.Rows.Count > 0)
        {
            grdvwUserList.DataSource = dtUsers;
            //ViewState["Dtexport"] = dtUsers;
        }
        else
        {
            //ViewState["Dtexport"] = null;
            grdvwUserList.DataSource = null;
        }
        grdvwUserList.DataBind();
        grdvwUserList.Visible = true;
        UpdSearch.Update();
    }
    private string xmlStringGenerator(CheckBoxList chkBxLst)
    {
        // XML string generator on behalf of Parent checkboxlist
        DataTable _dtValueHolder = new DataTable();
        _dtValueHolder.TableName = "tblData";
        _dtValueHolder.Columns.Add(new DataColumn("Value", typeof(string)));
        _dtValueHolder.Columns["Value"].DefaultValue = DBNull.Value;

        foreach (ListItem lstItm in chkBxLst.Items)
        {
            if (lstItm.Selected)
            {
                DataRow _dRow = _dtValueHolder.NewRow();
                _dRow["Value"] = lstItm.Value.Trim();
                _dtValueHolder.Rows.Add(_dRow);
            }
        }


        MemoryStream mStream = new MemoryStream();
        _dtValueHolder.WriteXml(mStream, true);

        //Retrieve the text from the stream
        mStream.Seek(0, SeekOrigin.Begin);

        StreamReader sReader = new StreamReader(mStream);
        string xmlString = string.Empty;
        xmlString = sReader.ReadToEnd();
        sReader.Close(); sReader.Dispose();

        xmlString = xmlString.Trim().Equals("<DocumentElement />") ? string.Empty : xmlString;
        return xmlString;
    }

    private void SendMailToUser(int userId)
    {
        try
        {
            DataTable dtUserInfo;
            using (UserData objUsers = new UserData())
            {
                objUsers.RestrictUserID = Convert.ToInt32(PageBase.UserId);
                objUsers.CompanyID = PageBase.ClientId;
                dtUserInfo = objUsers.GetUsersInfo(userId, 1);
            };
            if (dtUserInfo != null && dtUserInfo.Rows.Count > 0)
            {
                string ErrDesc = string.Empty;
                string Password = string.Empty;
                using (Authenticates ObjAuth = new Authenticates())
                {
                    Password = ObjAuth.DecryptPassword(Convert.ToString(dtUserInfo.Rows[0]["Password"]), Convert.ToString(dtUserInfo.Rows[0]["PasswordSalt"]));
                };
                bool mail = false;
                Mailer.LoginName = Convert.ToString(dtUserInfo.Rows[0]["LoginName"].ToString());
                Mailer.Password = Password;
                Mailer.EmailID = dtUserInfo.Rows[0]["Email"].ToString();
                Mailer.UserName = Convert.ToString(dtUserInfo.Rows[0]["DisplayName"].ToString());
                mail = Mailer.sendmail("../../../" + strAssets + "/Mailer/CreateUser.htm");
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    #endregion

    protected void grdvwUserList_RowDataBound(object sender, GridViewRowEventArgs e)
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
                Label lblOnline = (Label)GVR.FindControl("lblOnline");
                ImageButton Online = (ImageButton)GVR.FindControl("imgOnline");
                if (lblOnline.Text.ToLower() == "false")
                {
                    Online.Attributes.Add("Onclick", "javascript:alert('User already logged off.');{return false;}");
                }
                ImageButton btnActiveDeactive = (ImageButton)GVR.FindControl("btnActiveDeactive");
                Label lblStatus = (Label)GVR.FindControl("lblStatus");
                if (lblStatus.Text.ToLower() == "false")
                {
                    btnActiveDeactive.Attributes.Add("Onclick", "javascript:alert('Use edit to active this user.');{return false;}");
                }
                ImageButton btnLock = (ImageButton)GVR.FindControl("imgLocked");
                Label lblLock = (Label)GVR.FindControl("lblLocked");
                if (lblLock.Text.ToLower() == "true")
                {
                    btnLock.Visible = true;
                    btnLock.ImageUrl = PageBase.siteURL + "/" + strAssets + "/CSS/images/Lock.png";
                    btnLock.ToolTip = "unlock user";
                }
                else
                    btnLock.Visible = false;

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    public string LoginStatus(Int16 IsActive)
    {
        string imgUrl = PageBase.siteURL + "/" + strAssets + "/CSS/images/status_offline.png";
        if (IsActive == 1)
        { imgUrl = PageBase.siteURL + "/" + strAssets + "/CSS/images/status_online.png"; }
        return imgUrl;
    }
    public string LoginToolTip(Int16 IsActive)
    {
        string ToolTip = "Online";
        if (IsActive == 0)
        { ToolTip = "Offline"; }
        return ToolTip;
    }
    protected void grdvwUserList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Result = 0;
        Int32 UserId = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "Online")
        {
            if (UserId > 0)
            {
                using (UserData ObjUser = new UserData())
                {
                    ObjUser.UserID = UserId;
                    ObjUser.CompanyID = PageBase.ClientId;
                    Result = ObjUser.UpdateUserLoginStatus();
                };
                if (Result > 0)
                {

                    ucMsg.ShowSuccess(Resources.Messages.LogOff);
                }
                else
                {
                    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                }
                FillUserGrid();

            }
        }
        if (e.CommandName.ToLower() == "unlock")
        {

            if (UserId > 0)
            {
                using (UserData objuser = new UserData())
                {
                    objuser.UserID = UserId;
                    objuser.ActionId = 1;
                    objuser.CompanyID = PageBase.ClientId;
                    Result = objuser.UpdateUserLoginStatus();
                };
                if (Result > 0)
                {
                    ucMsg.ShowSuccess(Resources.Messages.LockedOut);
                }
                else
                {
                    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                }
                FillUserGrid();

            }

        }
    }


    protected void btnCreate_Click(object sender, EventArgs e)
    {
        string MappingMessage = "";/*#CC06 Added*/
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            if (ddlHierarchyLevel.SelectedIndex == 0 || txtLocationCode.Text.Trim().Length == 0 || txtLocationName.Text.Trim().Length == 0)
            {
                ucMsg.ShowWarning(Resources.Messages.MandatoryField);

                return;
            }

            if (ddlState.SelectedValue != "0")
            {
                if (ddlCity.SelectedValue == "0")
                {
                    ucMsg.ShowWarning("Please Select City.");
                    return;
                }
            }
            if (Convert.ToInt16(ddlHierarchyLevel.SelectedValue) > 0 && txtLocationCode.Text.Trim().Length >= 0 && txtLocationCode.Text.Trim().Length > 0
                 )
            {
                Int32 result = 0;
                int CheckResult = 0;/*#CC04*/
                string brandCategoryIds = "";
                using (OrgHierarchyData objOrg = new OrgHierarchyData())
                {
                    if (Convert.ToInt32(ViewState["EditUserId"]) == 0)
                    {
                        objOrg.OrgnhierarchyID = 0;
                    }
                    else
                    {
                        objOrg.OrgnhierarchyID = Convert.ToInt32(ViewState["EditUserId"]);
                    }
                    objOrg.LocationName = txtLocationName.Text.Trim();
                    objOrg.LocationCode = txtLocationCode.Text.Trim();
                    objOrg.HierarchyLevelID = Convert.ToInt16(ddlHierarchyLevel.SelectedValue);
                    objOrg.ParentOrgnhierarchyID = Convert.ToInt32(ddlParentHierarchy.SelectedValue);
                    objOrg.Status = Convert.ToBoolean(chkorg.Checked);
                    CheckResult = objOrg.CheckStatusForExistLocation(); /*#CC04*/
                    objOrg.UserID = PageBase.UserId;/*#CC06 Added*/
                    objOrg.CompanyId = PageBase.ClientId;/*#CC07 Added*/
                    if (ddlState.SelectedValue != "0")
                    {
                        objOrg.StateID = Convert.ToInt32(ddlState.SelectedValue);
                    }
                    else
                    {
                        objOrg.StateID = 0;
                    }
                    if (ddlCity.SelectedValue != "0")
                    {
                        objOrg.CityID = Convert.ToInt32(ddlCity.SelectedValue);
                    }
                    else
                    {
                        objOrg.CityID = 0;
                    }
                    for (int cntr = 0; cntr < chkBrandCategory.Items.Count;cntr++ )
                    {
                        if (chkBrandCategory.Items[cntr].Selected)
                            brandCategoryIds = brandCategoryIds + chkBrandCategory.Items[cntr].Value + ",";

                    }
                    objOrg.BrandCategoryIds=brandCategoryIds;
                    result = objOrg.InsertUpdateOrgnHierarchyinfo();
                   
                    if ((CheckResult > 0) && (objOrg.Status == false))
                    {
                        ucMsg.ShowError("This location is map with existing Location or Saleschannel.You can not deactivate it");
                        return;
                    }
                    if (objOrg.ReturnValue == 1)
                    {
                        ucMsg.ShowError(Resources.Messages.DuplicateLocationName);
                        return;
                    }
                    if (objOrg.ReturnValue == 2)
                    {
                        ucMsg.ShowError(objOrg.Error.ToString());
                        return;
                    }

                    MappingMessage = objOrg.Error;
                };


                if (result > 0)
                {
                    if (ViewState["EditUserId"] == null)
                    {
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    }
                    else
                    {
                        ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull + '-' + MappingMessage/*#CC06 Added*/);
                    }

                    ClearFormorg();
                    chkorg.Checked = true;
                    updAddUserMain.Update();
                    return;
                }
                else
                {
                    return;
                }
            }      
            updMsg.Update();
           
        }

        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            //updMsg.Update();
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnOrgCancle_Click(object sender, EventArgs e)
    {
        ClearFormorg();
    }
    protected void ddlHierarchyLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindParentHierarchy();
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            ddlCity.Items.Clear();
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
                objuser.CompanyId = PageBase.ClientId;
                objuser.StateID = Convert.ToInt32(ddlState.SelectedValue);
                dt = objuser.GetStateCityList(1);
            };
            String[] colArray = { "CityID", "CityName" };
            PageBase.DropdownBinding(ref ddlCity, dt, colArray);
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindHierarchy()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlHierarchyLevel.Items.Clear();
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
                objuser.CompanyId = PageBase.ClientId;
                dt = objuser.GetHierarchyLevelConditional(2);
            };
            String[] colArray = { "HierarchyLevelID", "HierarchyLevelName" };
            PageBase.DropdownBinding(ref ddlHierarchyLevel, dt, colArray);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindState()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlState.Items.Clear();
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
                objuser.CompanyId = PageBase.ClientId;
                dt = objuser.GetStateCityList(0);
            };
            String[] colArray = { "StateID", "StateName" };
            PageBase.DropdownBinding(ref ddlState, dt, colArray);
            ddlCity.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindParentHierarchy()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlParentHierarchy.Items.Clear();
            using (OrgHierarchyData objOrg = new OrgHierarchyData())
            {
                objOrg.HierarchyLevelID = Convert.ToInt16(ddlHierarchyLevel.SelectedValue);
                objOrg.CompanyId = PageBase.ClientId;
                dt = objOrg.GetAllHierarchyLevelLocation();
            };
            String[] colArray = { "OrgnhierarchyID", "LocationName" };
            PageBase.DropdownBinding(ref ddlParentHierarchy, dt, colArray);
            ddlParentHierarchy.Enabled = dt.Rows.Count == 0 ? false : true;     //Pankaj Dhingra


        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    void ClearFormorg()
    {
        ddlHierarchyLevel.SelectedIndex = 0;
        ddlParentHierarchy.SelectedIndex = 0;    
        BindParentHierarchy();
        ddlHierarchyLevel.Enabled = true;
        ddlParentHierarchy.Enabled = true;
        txtLocationName.Text = "";
        txtLocationCode.Text = "";
        txtLocationName.Text = "";
        btnCreate.Text = "Submit";
        ViewState["EditUserId"] = null;
        ddlState.SelectedValue = "0";
        if (ddlState.SelectedValue == "0")
        {
            ddlCity.Items.Clear();
            ddlCity.Items.Insert(0, new ListItem("Select", "0"));
        }
        updAddUserMain.Update();
    }
    protected void btnEditLocation_Click(object sender, ImageClickEventArgs e)
    {
        ddlHierarchyLevel.SelectedIndex = -1;
        ImageButton btnEditLocation = (ImageButton)sender;
        DataTable dtOrgn;
        using (OrgHierarchyData objOrgn = new OrgHierarchyData())
        {
            dtOrgn = objOrgn.GetOrgnHierarchyInfo(Convert.ToInt32(btnEditLocation.CommandArgument));
        };
        ViewState["EditUserId"] = Convert.ToInt32(btnEditLocation.CommandArgument);

        if (dtOrgn != null && dtOrgn.Rows.Count > 0)
        {
            if (ddlHierarchyLevel.Items.FindByValue(dtOrgn.Rows[0]["HierarchyLevelID"].ToString()) != null)
            {
                ddlHierarchyLevel.Items.FindByValue(dtOrgn.Rows[0]["HierarchyLevelID"].ToString()).Selected = true;
                ddlHierarchyLevel_SelectedIndexChanged(ddlHierarchyLevel, new EventArgs());
                ddlHierarchyLevel.Enabled = false;
            }
            if (ddlParentHierarchy.Items.FindByValue(dtOrgn.Rows[0]["ParentOrgnhierarchyID"].ToString()) != null)
            {
                ddlParentHierarchy.Items.FindByValue(dtOrgn.Rows[0]["ParentOrgnhierarchyID"].ToString()).Selected = true;
            }

            if (ddlState.Items.FindByValue(dtOrgn.Rows[0]["StateID"].ToString()) != null)
            {

                ddlState.SelectedValue = Convert.ToString(dtOrgn.Rows[0]["StateID"]);

                ddlState_SelectedIndexChanged(ddlState, new EventArgs());
                if (ddlCity.Items.FindByValue(dtOrgn.Rows[0]["CityID"].ToString()) != null)
                {
                    ddlCity.SelectedValue = Convert.ToString(dtOrgn.Rows[0]["CityID"]);
                }

            }
            else
            {
                ddlState.SelectedValue = "0";
                if (ddlState.SelectedValue == "0")
                {
                    ddlCity.SelectedValue = "0";
                }
            }
            txtLocationName.Text = (dtOrgn.Rows[0]["LocationName"].ToString());
            txtLocationName.Focus();
            txtLocationCode.Text = (dtOrgn.Rows[0]["LocationCode"].ToString());
            chkorg.Checked = Convert.ToBoolean(dtOrgn.Rows[0]["Status"]);
            btnCreate.Text = "Update";
            
            string strSelectedBrCt=Convert.ToString(dtOrgn.Rows[0]["BrandCategoryIDs"]);
            if(!string.IsNullOrEmpty( strSelectedBrCt))
            {
                for (int cntr2 = 0; cntr2 < chkBrandCategory.Items.Count; cntr2++)
                {
                    chkBrandCategory.Items[cntr2].Selected = false;
                }
                string[] strBrandCat = strSelectedBrCt.Split(',');
                for (int cntr1 = 0; cntr1 < strBrandCat.Length; cntr1++)
                {
                    for (int cntr2 = 0; cntr2 < chkBrandCategory.Items.Count; cntr2++)
                    {
                        if (strBrandCat[cntr1] == chkBrandCategory.Items[cntr2].Value)
                            chkBrandCategory.Items[cntr2].Selected = true;
                    }
                }
            }

            updAddUserMain.Update();
        }
    }
    private void BindBrandCategory()
    {
        try
        {
            DataTable dt = new DataTable();
            using (UserData objuser = new UserData())
            {
                objuser.SearchType = 1;
                objuser.Status = true;
                objuser.UserID = PageBase.UserId; 
                objuser.CompanyID = PageBase.ClientId;
                dt = objuser.GetBrandCategoryMaster();
            };
            chkBrandCategory.DataSource = dt;
            chkBrandCategory.DataValueField = "BrandCategoryMasterID";
            chkBrandCategory.DataTextField = "BrandCategory";
            chkBrandCategory.DataBind();
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListItem item in chkBrandCategory.Items)
        {
            item.Selected = chkAll.Checked;
        }
    }
}
