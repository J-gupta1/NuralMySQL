using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;

/*
 * Change Log:
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 18-Apr-2016, Sumit Maurya, #CC01, New code added to clear Cookies.
 */

public partial class Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /* #CC01 Add Start */
        Response.Cookies["MyCookie4"].Expires = DateTime.Now.AddDays(-1d);
        /* #CC01 Add End */

        Session.Abandon();
        string[] strAssets = PageBase.strAssets.Split(new char[] { '/' });
        string path = strAssets[1].ToString();
        string _strRedirectLogin = PageBase.siteURL + "Login/" + path + "/Login.aspx";
        Response.Redirect(_strRedirectLogin, false);



    }
}
