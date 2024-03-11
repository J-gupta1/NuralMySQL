using System;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Web.Configuration;
using System.Configuration;
/*
 * Change Log:
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 21-Apr-2016, Sumit Maurya, #CC01, Check added to stop login multiple users according to following conditions.
 *                                   1. Multiple user cannot login in same browser.
 *                                   2. According to config ( in database Application configuration master if value of Config key "Multilogin" is set to 0 then a user can login at single location only.)
 * 29-Apr-2016, Sumit Maurya, #CC02, Logout link was displaying unnecessarily while checking same user login (if Config value is 0)
 */
public partial class Login_Gfive_Login : System.Web.UI.Page
{
    private Int16 siteflag = Convert.ToInt16(ConfigurationManager.AppSettings["siteMaintenance"]);
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Response.Expires = -1500;
            Response.CacheControl = "no-cache";
            if (!IsPostBack)
            {

            }

        }
        catch (Exception ex)
        {
            lblHeader.Text = ex.Message.ToString();
        }

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (siteflag == 0)
                Response.Redirect("~/Maintenance.htm", false);
            else
                subCheckLogin(txtUsername.Text.Trim(), txtPassword.Text.Trim());

        }
        catch (Exception ex)
        {
            lblHeader.Text = ex.Message.ToString();
        }
    }
    public void subCheckLogin(string strLoginName, string strPassword)
    {
        try
        {
            if ((txtUsername.Text.Trim().Length == 0) || (txtPassword.Text.Trim().Length == 0))
            {
                lblHeader.Text = "";
                txtUsername.Text = "";
                txtPassword.Text = "";
                txtUsername.Focus();
            }

            else
            {
                DataSet dtUser = new DataSet();
                Int16 PsLockd = 0;
                bool Validuser;
                using (Authenticates ObjAuth = new Authenticates())
                {
                    ObjAuth.CheckLogin = true;
                    Validuser = ObjAuth.AuthenticateUser(clsRemoveHTMLTags.clsRemoveHTMLTags.fncRemoveHTMLTags(txtUsername.Text.Trim()), clsRemoveHTMLTags.clsRemoveHTMLTags.fncRemoveHTMLTags(txtPassword.Text.Trim()), ref dtUser, ref PsLockd);
                    /* #CC01 code Start */
                    if (ObjAuth.IsLoggedIn == 1)
                    {
                        lblHeader.Text = "You are already logged in this application using same or another login credentials. If you want to login as another user, log out from previous window first!";
                        HyplkLogOut.Visible = false; /* #CC02 Added */
                        return;
                    }
                    /* #CC01 code End */
                };
                if (PsLockd == 4)
                {
                    lblHeader.Text = Resources.Messages.UserLogin;
                    return;
                }
                if (Validuser == true)
                {/* #CC01 code Start */
                    if (Request.Cookies["MyCookie4"] != null)
                    {
                        if (Request.Cookies["MyCookie4"].Value == "checkLogin=1")
                        {
                            Authenticates ObjAuth = new Authenticates();
                            ObjAuth.LoginAttemp(txtUsername.Text.Trim(), 3);
                            HyplkLogOut.Visible = true;
                            HyplkLogOut.HRef = PageBase.siteURL + "/Logout.aspx";
                            string logout = PageBase.siteURL + "/Logout.aspx";
                            lblHeader.Text = "User already logged in this application, click on logout and try to login again.";//"You are already logged in this application.";
                            return;
                        }
                    }
                    else
                        Response.Cookies["MyCookie4"]["checkLogin"] = "1";
                    /* #CC01 Add End */
                    if (dtUser.Tables[1] != null && dtUser.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow Dr in dtUser.Tables[1].Rows)
                        {

                            if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "OPDATE")
                            {
                                if (dtUser.Tables[0].Rows[0]["SalesChannelTypeID"].ToString() != "")
                                {
                                    if (Convert.ToBoolean(dtUser.Tables[0].Rows[0]["IsOpeningStockEntered"]) == false)
                                    {
                                        Session["BackDaysAllowedOpeningStock"] = Dr["ConfigValue"].ToString();
                                    }
                                    else
                                    {
                                        Session["BackDaysAllowedOpeningStock"] = null;
                                    }
                                }
                            }

                            else if
                            (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "PRETRN")
                            {
                                Session["BackDaysAllowedBeforeOpening"] = Dr["ConfigValue"].ToString();
                            }
                            else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "ORDATE")
                            {
                                Session["BackDaysAllowedOrder"] = Dr["ConfigValue"].ToString();
                            }
                            else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "PONOAUTO")
                            {
                                if (dtUser.Tables[0].Rows[0]["SalesChannelLevel"].ToString() != null && dtUser.Tables[0].Rows[0]["SalesChannelLevel"].ToString() == "2")
                                {
                                    Session["IsPrimaryOrderNoAutogenerate"] = Dr["ConfigValue"].ToString();
                                }
                                else
                                {
                                    Session["IsPrimaryOrderNoAutogenerate"] = null;
                                }


                            }
                            else if (Dr["ConfigKey"] != null && Dr["ConfigKey"].ToString() == "IONOAUTO")
                            {
                                if (dtUser.Tables[0].Rows[0]["SalesChannelLevel"].ToString() != null && dtUser.Tables[0].Rows[0]["SalesChannelLevel"].ToString() == "3")
                                {
                                    Session["IsIntermediaryOrderNoAutogenerate"] = Dr["ConfigValue"].ToString();
                                }
                                else
                                {
                                    Session["IsIntermediaryOrderNoAutogenerate"] = null;
                                }
                            }



                        }

                    }
                    Session["PasswordExpired"] = dtUser.Tables[0].Rows[0]["PasswordExpiredOn"].ToString();
                    Session["UserID"] = dtUser.Tables[0].Rows[0]["UserID"].ToString();
                    Session["RoleID"] = dtUser.Tables[0].Rows[0]["RoleID"].ToString();
                    Session["HierarchyLevelID"] = dtUser.Tables[0].Rows[0]["HierarchyLevelID"].ToString();
                    Session["SalesChanelTypeID"] = dtUser.Tables[0].Rows[0]["SalesChannelTypeID"].ToString();
                    Session["ParentHierarchyLevelID"] = dtUser.Tables[0].Rows[0]["ParentHierarchyLevelID"].ToString();
                    Session["LoginName"] = dtUser.Tables[0].Rows[0]["LoginName"].ToString();
                    Session["SalesChannelID"] = dtUser.Tables[0].Rows[0]["SalesChannelID"].ToString();
                    Session["DisplayName"] = Server.HtmlEncode(dtUser.Tables[0].Rows[0]["DisplayName"].ToString().Length > 0 ? dtUser.Tables[0].Rows[0]["DisplayName"].ToString() : dtUser.Tables[0].Rows[0]["Name"].ToString());
                    Session["RoleName"] = dtUser.Tables[0].Rows[0]["RoleName"].ToString();
                    Session["SalesChannelLevel"] = dtUser.Tables[0].Rows[0]["SalesChannelLevel"].ToString();
                    Session["SalesChannelCode"] = dtUser.Tables[0].Rows[0]["SalesChannelCode"].ToString();
                    Session["NumberofBackDaysAllowed"] = dtUser.Tables[0].Rows[0]["NumberOfBackDays"].ToString();
                    Session["IsSuperAdmin"] = dtUser.Tables[0].Rows[0]["IsSuperAdmin"].ToString();
                    Session["AllowAllHierarchy"] = dtUser.Tables[0].Rows[0]["AllowAllHierarchy"].ToString();
                    Session["OpeningStockdate"] = dtUser.Tables[0].Rows[0]["OpeningStockdate"].ToString();
                    Session["Brand"] = 0;               //Default Value it will be initialized from default page  //Pankaj Dhingra
                    Session["MultipleBrandName"] = ""; //Default Value it will be initialized from default page  //Pankaj Dhingra
                    if (dtUser.Tables[0].Rows[0]["SalesChannelTypeID"].ToString() != "")
                    {
                        if (Convert.ToBoolean(dtUser.Tables[0].Rows[0]["IsOpeningStockEntered"]) == false)
                            Response.Redirect("~/Transactions/Common/ManageOpeningStock.aspx");
                        else
                            //Response.Redirect("~/Default.aspx", false);
                            Response.Redirect("~/Masters/SalesChannel/SalesChannelBranding.aspx", false);   //Pankaj dhingra
                    }
                    else
                        Response.Redirect("~/Default.aspx", false);
                }
                else
                {
                    if (PsLockd == 2)
                    {
                        lblHeader.Text = Resources.Messages.PasswordBlocked;
                    }
                    else
                    {
                        lblHeader.Text = Resources.Messages.Invaliduser; ;
                    }

                }

            }
        }
        catch (Exception ex)
        {
            lblHeader.Text = ex.Message.ToString();

        }

    }
}

