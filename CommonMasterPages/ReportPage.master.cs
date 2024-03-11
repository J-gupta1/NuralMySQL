using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using System.Web.UI.WebControls;
using BussinessLogic;


public partial class CommonMasterPages_ReportPage : System.Web.UI.MasterPage
{
    protected string strSiteUrl = PageBase.siteURL;
    protected string strAssets = PageBase.strAssets;
    protected void LBSwitchToBrand_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/SalesChannel/SalesChannelBranding.aspx", false);
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);

        }
    }
    protected void Page_PreRender(object s, EventArgs a)
    {

        litScripts.Text = @"<script type=""text/javascript"">var baseUrl= '" + ResolveUrl(PageBase.siteURL) + "'</script>";
        //  litScripts.Text = @"<script type=""text/javascript""   src=""" + strAssets + @"Jscript/JsValidate.js""></script>";
        //  litScripts.Text = litScripts.Text + @"<script type=""text/javascript""   src=""" + strAssets + @"Jscript/JSAsynRequest.js""></script>";
        //  litScripts.Text = litScripts.Text + @"<script type=""text/javascript""   src=""" + strAssets + @"Jscript/jquery-1.4.4.min.js""></script>";

        // litScripts.Text = litScripts.Text + @"<link rel=""stylesheet"" rel=""text/csst""   href=""" + strAssets + @"CSS/Menu.css"" />";

        Page.Header.DataBind();
        _bindAssets();

        if (!IsPostBack)
        {
            if (PageBase.PageHeading != "" || PageBase.PageHeading != string.Empty)
            {
                lblPageHeading.Text = PageBase.PageHeading;
                lblPageHeading.Visible = true;

            }

            else
                tblHeading.Visible = false;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Page.Header.DataBind();
        //_bindAssets();

        if (!IsPostBack)
        {
            if (!IsPostBack)
            {
                if (PageBase.PageHeading != "" || PageBase.PageHeading != string.Empty)
                {
                    lblPageHeading.Text = PageBase.PageHeading;
                    lblPageHeading.Visible = true;

                }

                else
                    tblHeading.Visible = false;
            }

            /* #26-Mar-14, Rakesh Goel - Code moved to PreRender method. Otherwise it comes wrong
            if (PageBase.PageHeading != "" || PageBase.PageHeading != string.Empty)
            {
                lblPageHeading.Text = PageBase.PageHeading;
                lblPageHeading.Visible = true;
            }
            else
                tblHeading.Visible = false;
             */
        }
        if (PageBase.MultipleBrandName != "")
        {
            LBSwitchToBrand.Text = "Current Brand " + PageBase.MultipleBrandName + " " + "(Switch To)";
            LBSwitchToBrand.Visible = true;
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        litScripts.Text = @"<script type=""text/javascript"">var baseUrl= '" + ResolveUrl(PageBase.siteURL) + "'</script>";

        //Image txt = ((UserControl)Master.FindControl("ucHeader")).FindControl("hyplogo") as Image;
        UserControl ucHeader = (UserControl)FindControl("ucHeader");
        HyperLink hyplogo = (HyperLink)ucHeader.FindControl("hyplogo");
        hyplogo.ImageUrl = "~/" + strAssets + "/CSS/Images/savexlogo.png";

        Page.Header.DataBind();
        _bindAssets();

        if (PageBase.PageHeading != "" || PageBase.PageHeading != string.Empty)
        {
            lblPageHeading.Text = PageBase.PageHeading;
            lblPageHeading.Visible = true;

        }

        else
            tblHeading.Visible = false;

    }
    void _bindAssets()
    {
        bootstrapCss.Attributes.Add("href", "~/" + strAssets + "/CSS/bootstrap.min.css?" + DateTime.Now.Ticks);
        StyleCss.Attributes.Add("href", "~/" + strAssets + "/CSS/Style.css?" + DateTime.Now.Ticks);
        MenuCss.Attributes.Add("href", "~/" + strAssets + "/CSS/Menu.css?" + DateTime.Now.Ticks);
        hypfooterlogo.ImageUrl = "~/" + strAssets + "/CSS/Images/footerimg.gif";
        hypfooterlogo.NavigateUrl = PageBase.redirectURL;
        hypfooterlogo.Target = "_blank";
    }
}
