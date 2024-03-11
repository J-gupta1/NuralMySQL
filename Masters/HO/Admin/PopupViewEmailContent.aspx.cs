using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using System.Data;

public partial class Masters_HO_Admin_PopupViewEmailContent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string EmailKeyword = Request.QueryString["Key"].ToString();
        BindViewEmailContent(EmailKeyword);

    }
    private void BindViewEmailContent(string emailkey)
    {
        DataTable dt = new DataTable();
        using (clsMailer objmailer = new clsMailer())
        {
            try
            {
                objmailer.Emailkeyword = emailkey;
                dt = objmailer.getViewEmailMaster(2);
                lblViewEmailContent.Text = dt.Rows[0][4].ToString();
                
            }
            catch (Exception ex)
            {
                if (objmailer.ErrorDetailXML != null && objmailer.ErrorDetailXML != string.Empty)
                {
                    //ucMsg.XmlErrorSource = objmailer.ErrorDetailXML;
                }
                else if (objmailer.Error != null && objmailer.Error != "" && objmailer.Error != "0")
                {
                    //ucMsg.ShowError(objmailer.Error);
                }
            }
        }
    }
}
