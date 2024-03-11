using System;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Web.Configuration;
using System.Configuration;
using System.Web;
/*
 * Change Log:
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 21-Apr-2016, Sumit Maurya, #CC01, Check added to stop login multiple users according to following conditions.
 *                                   1. Multiple user cannot login in same browser.
 *                                   2. According to config ( in database Application configuration master if value of Config key "Multilogin" is set to 0 then a user can login at single location only.)
 * 29-Apr-2016, Sumit Maurya, #CC02, Logout link was displaying unnecessarily while checking same user login (if Config value is 0)                                  
 */
public partial class Login_Beetel_Login : System.Web.UI.Page
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
                {

                    // added by amit agarwa;
                    /* #CC01 code Start */
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
                    PageBase.SetSessionForLoginUser(dtUser);

                    InitializePublicVariables();
                    if (dtUser.Tables[0].Rows[0]["SalesChannelTypeID"].ToString() != "")
                    {
                        if (Convert.ToBoolean(dtUser.Tables[0].Rows[0]["IsOpeningStockEntered"]) == false)
                            //Response.Redirect("~/Transactions/Common/ManageOpeningStock.aspx");
                            Response.Redirect("~/Transactions/CommanSerial/ManageOpeningStockSerialWise.aspx");

                        else
                            //Response.Redirect("~/Default.aspx", false);
                            Response.Redirect("~/Masters/SalesChannel/SalesChannelBranding.aspx", false);   //Pankaj dhingra
                    }
                    else
                    {

                        string RedirectURL = PageBase.DefaultRedirection(PageBase.DefaultRedirectionFlag);
                        Response.Redirect(RedirectURL, false);

                        //if (PageBase.OtherEntityType == 1)
                        //{
                        //    Response.Redirect("~/Retailer/specific/sonydefault.aspx", false);
                        //}
                        //else if (PageBase.OtherEntityType == 2)
                        //{
                        //    Response.Redirect("~/Retailer/Common/RetailerOpeningStock.aspx", false);
                        //}
                        //else
                        //{
                        //    Response.Redirect("~/Default.aspx", false);
                        //}
                    }
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
    void InitializePublicVariables()
    {
        try
        {
            HttpContext.Current.Session["PhysicalPath"] = Server.MapPath("~/Excel/Download/");
            HttpContext.Current.Session["VirtualPath"] = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "/";
            HttpContext.Current.Session["PhysicalPathUpload"] = Server.MapPath("~/Excel/Upload/UploadExcelFiles/");
        }
        catch (Exception ex)
        {

        }
    }
}
