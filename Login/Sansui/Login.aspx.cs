using System;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Web.Configuration;
using System.Configuration;
using System.Web;

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
                };
                if (PsLockd == 4)
                {
                    lblHeader.Text = Resources.Messages.UserLogin;
                    return;
                }
                if (Validuser == true)
                {

                    // added by amit agarwal
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
