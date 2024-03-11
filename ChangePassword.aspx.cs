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
using Microsoft.ApplicationBlocks.Data;
using System.Globalization;
using DataAccess;
using BussinessLogic;


public partial class ChangePassword :PageBase 
{
    string mailError = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Menu siteMenu = new Menu();
        siteMenu = (Menu)Master.FindControl("ctl00$ucMenu1$HeaderMenu");
        if (Session["Count"] == "2")
            
        if (siteMenu!= null)
        {
            siteMenu.Visible = false;
        }
                if (Session["ChangePasswordMsg"] != null)
                { lblmsg.Text = (string)Session["ChangePasswordMsg"]; Session.Remove("ChangePasswordMsg"); 
                }
                else { lblmsg.Text = string.Empty;
                }
    }
  bool CompareString(string strA, string strB, bool ignoreCase)
        { return (string.Compare(strA, strB, ignoreCase) == 0); }

protected void  btnsubmit_Click(object sender, EventArgs e)
{
    if (IsPageRefereshed == true)
    {
        return;
    } 
    if (Page.IsValid)
    {
        Int16 Result = 0;
        DataSet _dbPassword = new DataSet();
        Authenticates ObjAuth = new Authenticates();
        try
        {
            if (!Convert.ToString(txtNewpass.Text).Contains("'"))
            {
                ObjAuth.CompanyId = PageBase.ClientId;
                _dbPassword = ObjAuth.RemindUserPasswordLog(Convert.ToString(Session["LoginName"]).ToString());
                if (_dbPassword != null)
                {
                    if (_dbPassword.Tables[0].Rows.Count > 0)
                    {
                        if (_dbPassword.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in _dbPassword.Tables[1].Rows)
                            {
                                string PasswordLog = ObjAuth.DecryptPassword(Convert.ToString(_dbPassword.Tables[1].Rows[0]["Password"]), Convert.ToString(_dbPassword.Tables[1].Rows[0]["PasswordSalt"]));
                                bool _PasswordExist =CompareString(txtNewpass.Text.Trim(), PasswordLog,true);
                                if (_PasswordExist == true)
                                {
                                        ucMsg.ShowInfo("New password match with already entered password.Please enter another..!");
                                    return ;
                                }
                            }
                         }
                        string oldPassword=ObjAuth.DecryptPassword(Convert.ToString(_dbPassword.Tables[0].Rows[0]["Password"]), Convert.ToString(_dbPassword.Tables[0].Rows[0]["PasswordSalt"]));
                        if (CompareString(txtOldpass.Text.Trim(), oldPassword, true))
                           {
                        if (!CompareString(oldPassword, txtNewpass.Text.Trim(), false))
                        {
                            string PasswordSalt = ObjAuth.GenerateSalt(Convert.ToInt32(txtNewpass.Text.Trim().Length));
                            string Password = ObjAuth.EncryptPassword(txtNewpass.Text.Trim(), PasswordSalt);
                            if (Application["ExpiryDays"] != null)
                                ObjAuth.NextPasswordExpiryDays = Convert.ToInt16(Application["ExpiryDays"].ToString());
                            else
                                ObjAuth.NextPasswordExpiryDays = 90;
                            ObjAuth.StrPassword = txtNewpass.Text.Trim();
                            ObjAuth.CompanyId = PageBase.ClientId;
                            Result = ObjAuth.ChangePassword(Session["LoginName"].ToString(), Password, PasswordSalt);
                            if (ObjAuth.ErrorMessage != "")
                            {
                                mailError = ObjAuth.ErrorMessage;
                                Result = 5;
                            }
                        }
                        else
                        {
                            Result = 2;
                        }
                    }
                    else
                    {
                        Result = 3;
                    }
 
                 }
               }
             }   
            string strMsg = string.Empty;
            switch (Result)
            {
                case 0: ucMsg.ShowSuccess(Resources.Messages.PasswordChanged); Session["IsPasswordExpired"] = false; Session["Count"] = ""; break;
                //case 0: ucMsg.ShowSuccess(Resources.Messages.PasswordChanged); Session["IsPasswordExpired"] = false; Session["Count"] = ""; this.Master.FindControl("ctl00_ucMenu_HeaderMenu").Visible = true; break;
                    
                        //this.Master.FindControl("ctl00$ucHeader$HeaderMenu").Visible = true
                case 1: ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime); break;
                //case 2: ucMsg.ShowInfo("Enter same"); break;
                case 2: ucMsg.ShowInfo("New password can not be same as old password."); break;
                case 3: ucMsg.ShowInfo("Old password not matched"); break;
                case 4: ucMsg.ShowInfo("Old password special character"); break;
                case 5: ucMsg.ShowInfo(mailError); break;
            }
            ucMsg.Visible = true;
            mailError = "";
            if (Result == 0)
            {
                //DataTable _dtPassword;
                //_dtPassword=_dbPassword.Tables[0];
                //if (_dbPassword != null)
                    //SendMailToUser(_dtPassword, txtNewpass.Text.Trim());
               
            }
            if (Session["DefaultChangePassword"] != null && Convert.ToBoolean(Session["DefaultChangePassword"]) == true)
            {
                Session["ChangePasswordMsg"] = strMsg;
                string resstr = siteURL + "Default.aspx";
                try { Response.Redirect(resstr); }
                catch { Response.Redirect(resstr); }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowWarning(ex.Message);
            PageBase.Errorhandling(ex);
        }
       
    }
}
protected void btnReset_Click(object sender, EventArgs e)
{
    txtNewpass.Text = "";
    txtOldpass.Text = "";
    txtRetype.Text = "";
    ucMsg.ShowControl = false;
}
public void SendMailToUser(DataTable dtUserInfo, string NewPassword)
{
    try
    {
            string Password = NewPassword;
            Mailer.LoginName = Convert.ToString(dtUserInfo.Rows[0]["LoginName"].ToString());
            Mailer.Password = Password;
            Mailer.EmailID = dtUserInfo.Rows[0]["Email"].ToString();
            Mailer.UserName = Convert.ToString(dtUserInfo.Rows[0]["DisplayName"].ToString());
            Mailer.sendmail(strAssets + "/Mailer/UserChangePassword.htm");
    }
    catch (Exception ex)
    {
        ucMsg.ShowWarning(ex.Message);
        PageBase.Errorhandling(ex);
    }
}

}
