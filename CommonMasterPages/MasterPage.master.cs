#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Reviewed By :
 *  ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 19-Apr-2016, Sumit Maurya, #CC01, New usercontrol (ucCheckSessionTimeout.ascx) added to check session time and logout automatically after session time gets over.
 ====================================================================================================
*/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BussinessLogic;
using System.Web.UI.HtmlControls;

public partial class CommonMasterPages_MasterPage : System.Web.UI.MasterPage
{
    protected string strAssets = PageBase.strAssets;
    protected string strSiteUrl = PageBase.siteURL;
    protected string ResolveURL(string url)
    {
        return Page.ResolveClientUrl(url);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
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
        }
        catch (Exception ex)
        {

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {



        if (!IsPostBack)
        {



            //if (PageBase.PageHeading != "" || PageBase.PageHeading != string.Empty)
            //{
            //    lblPageHeading.Text = PageBase.PageHeading;
            //    lblPageHeading.Visible = true;

            //}

            //else
            //    tblHeading.Visible = false;        
        }
        if (PageBase.MultipleBrandName != "")
        {
            LBSwitchToBrand.Text = "Current Brand " + PageBase.MultipleBrandName + " " + "(Switch To)";
            LBSwitchToBrand.Visible = true;
        }

    }
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
    protected void Page_Init(object s, EventArgs a)
    {

        litScripts.Text = @"<script type=""text/javascript"">var baseUrl= '" + ResolveUrl(PageBase.siteURL) + "'</script>";
        //  litScripts.Text = @"<script type=""text/javascript""   src=""" + strAssets + @"Jscript/JsValidate.js""></script>";
        //  litScripts.Text = litScripts.Text + @"<script type=""text/javascript""   src=""" + strAssets + @"Jscript/JSAsynRequest.js""></script>";
        //  litScripts.Text = litScripts.Text + @"<script type=""text/javascript""   src=""" + strAssets + @"Jscript/jquery-1.4.4.min.js""></script>";

        // litScripts.Text = litScripts.Text + @"<link rel=""stylesheet"" rel=""text/csst""   href=""" + strAssets + @"CSS/Menu.css"" />";


        Page.Header.DataBind();
        _bindAssets();

    }
    void _bindAssets()
    {
        StyleCss.Attributes.Add("href", "~/" + strAssets + "/CSS/Style.css?" + DateTime.Now.Ticks);
        menuCss.Attributes.Add("href", "~/" + strAssets + "/CSS/Menu.css?" + DateTime.Now.Ticks);
        modalCss.Attributes.Add("href", "~/" + strAssets + "/CSS/modal.css?" + DateTime.Now.Ticks);
        bootstrapCss.Attributes.Add("href", "~/" + strAssets + "/CSS/bootstrap.min.css?" + DateTime.Now.Ticks);

        StyleDMSCss.Attributes.Add("href", "~/" + strAssets + "/CSS/Style-DMS.css?" + DateTime.Now.Ticks);/* Added by Adnan */

        hypfooterlogo.ImageUrl = "~/" + strAssets + "/CSS/Images/footerimg.gif";
        hypfooterlogo.NavigateUrl = PageBase.redirectURL;
        hypfooterlogo.Target = "_blank";
    }






}
