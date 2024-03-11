using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using System.Web.UI.WebControls;
using BussinessLogic;


public partial class CommonMasterPages_AdminPage : System.Web.UI.MasterPage
{
    protected string strSiteUrl = PageBase.siteURL;
    protected string strAssets = PageBase.strAssets;
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Header.DataBind();
        if (!IsPostBack)
            bindAssets();
    }
    void bindAssets()
    {
        hypfooterlogo.ImageUrl = "~/" + strAssets + "/CSS/Images/footerimg.gif";
        hypfooterlogo.NavigateUrl = PageBase.redirectURL;
        hypfooterlogo.Target = "_blank";
    }
}
