using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Globalization;
using DataAccess;
using BussinessLogic;

public partial class ForgotPassword : System.Web.UI.Page
{
    protected string strSiteUrl = PageBase.siteURL;
    protected string strAssets = PageBase.strAssets;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            LblHeader.Text = "";
            BindCSS();
        }
    }
    public void BindCSS()
    {
        LoginCSS.Attributes.Add("href", "~/" + strAssets + "/CSS/login.css?" + DateTime.Now.Ticks);
        BootstrapCSS.Attributes.Add("href", "~/" + strAssets + "/CSS/bootstrap.min.css?" + DateTime.Now.Ticks);
        StyleCSS.Attributes.Add("href", "~/" + strAssets + "/CSS/style.css?" + DateTime.Now.Ticks);
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string strLoginName = null;
            string strEmail = null;

            if (TxtLoginName.Text.Trim().Length == 0 & TxtEMail.Text.Trim().Length == 0)
            {
                LblHeader.Text = "Please enter LoginID or EmailID";
            }
            else
            {
                strLoginName = clsRemoveHTMLTags.clsRemoveHTMLTags.fncRemoveHTMLTags(TxtLoginName.Text);
                strEmail = clsRemoveHTMLTags.clsRemoveHTMLTags.fncRemoveHTMLTags(TxtEMail.Text);
                subchkloginName(strLoginName, strEmail);
                subclearFlds();
            }
        }
        catch (Exception ex)
        {
            LblHeader.Text = ex.Message.ToString();
            PageBase.Errorhandling(ex);
        }
    }
    protected void BttnClear_Click(object sender, EventArgs e)
    {
        subclearFlds();
    }
    public void subchkloginName(string loginname, string EMail)
    {
        try
        {
            DataTable dtUserInfo;
            using (UserData objUsers = new UserData())
            {
                dtUserInfo = objUsers.GetUsersInfoForgotPassword(loginname, EMail);
            };
            if (dtUserInfo == null || dtUserInfo.Rows.Count == 0)
            {

                LblHeader.Text = Resources.Messages.Invaliduser;
            }
            else
            {
                string ErrDesc = string.Empty;
                string Password = string.Empty;
                using (Authenticates ObjAuth = new Authenticates())
                {
                    Password = ObjAuth.DecryptPassword(Convert.ToString(dtUserInfo.Rows[0]["Password"]), Convert.ToString(dtUserInfo.Rows[0]["PasswordSalt"]));
                };

                bool SendMailStatus;
                Mailer.LoginName = Convert.ToString(dtUserInfo.Rows[0]["LoginName"].ToString());
                Mailer.Password = Password;
                Mailer.EmailID = dtUserInfo.Rows[0]["Email"].ToString();
                Mailer.UserName = Convert.ToString(dtUserInfo.Rows[0]["DisplayName"].ToString());
                SendMailStatus = Mailer.sendmail("../../" + strAssets + "/Mailer/UserForgetPassword.htm");
                if (SendMailStatus == false)
                    LblHeader.Text = "Mail not send due to invalid email Id";
                else
                    LblHeader.Text = "Password has been send to your email Id";
            }
        }
        catch (Exception ex)
        {

            LblHeader.Text = ex.ToString();
            PageBase.Errorhandling(ex);
        }
    }
    public void subclearFlds()
    {
        try
        {
            TxtLoginName.Text = "";
            TxtEMail.Text = "";
        }
        catch (Exception ex)
        {

            LblHeader.Text = ex.Message.ToString();
            PageBase.Errorhandling(ex);
        }
    }

}
