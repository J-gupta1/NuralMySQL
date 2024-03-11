using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
using System.Xml;

namespace Web.Controls
{
    public partial class Header : System.Web.UI.UserControl
    {


        protected bool _IsMenuVisible = false;
        protected string strSiteUrl = PageBase.siteURL;
        protected string strAssets = PageBase.strAssets;

        private string sLastLogin
        {
            get
            {
                if (Session["LastLoginOn"] != null)
                { return (Session["LoginTime"] == null || ServerValidation.IsDate(Session["LoginTime"], true) == 2) ? DateTime.Now.ToString() : Session["LoginTime"].ToString(); }
                else
                { return string.Empty; }
            }
        }
        void bindAssets()
        {
           // ImgSideLogo.Src ="~/" + strAssets + "/CSS/Images/zedsaleslogo.gif";
            hyplogo.ImageUrl ="~/" + strAssets + "/CSS/Images/savexlogo.png";

            if (PageBase.BaseEntityTypeID != 3)
            {
                hyplogo.NavigateUrl = "~/Default.aspx";
            }
            else
            {
                hyplogo.NavigateUrl = "~/Retailer/Specific/SonyDefault.aspx";
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                
                if (!IsPostBack)
                {

                    /* if Opening stock is not entered for the Sales Channel then hide change password visibility
                     * 
                     Added by amit  agarwal
                     */
                    if (PageBase.IsOpeningStockEntered == false && PageBase.SalesChanelID!=0)
                    {
                        dvChngPssLnk.Visible = false;
                    }



                    bindAssets();
                  // BussinessLogic.PageBase.GetvalidSession();
               
                }
		          
                    string strLogInUserName = Convert.ToString(Session["DisplayName"]) + "(" + Convert.ToString(Session["RoleName"]) + ")";
                    lblUserNameDesc.Text = strLogInUserName;
                    if (PageBase.SalesChannelOpeningStockDate.HasValue ==true)
                    {
                        lblOpeningDate.Text = PageBase.SalesChannelOpeningStockDate.Value.ToString("dd-MMM-yyyy");
                        divstock.Visible = true;
                    }
                    //Pankaj Dhingra            This will show the link at the heading of the every page
                   
	        }
	        catch (Exception ex)
	        {
                PageBase.Errorhandling(ex);
	        }
            
        }
       
        protected void HeaderMenu_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            if (e.Item.NavigateUrl == "" || e.Item.NavigateUrl == null)
            {
                e.Item.Selectable = false;
            }
        }

        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //    string script = "window.setTimeout(\"alert('Your session expired.You will be redirected to login page!'); window.location = 'Logout.aspx';\", " + (2 - 1) * 60000 + ")";
        //    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "SessionManager", "<script language=\"javascript\">" + script + "</script>");
        //}
   
}
}
