﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
using System.Xml;
/*
 * 27 May 2015, Karam Chand Sharma, #CC01, Read menu id and set into page header for zedcontrol manage
 */
namespace Web.Controls
{
    public partial class ucMenu : System.Web.UI.UserControl
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                bindMenu();

                if (!IsPostBack)
                {
                    /*#CC01 COMMENTED PageBase.UsertrackingAndRequestValidate(); */
                    /*#CC01 START ADDED*/
                    PageBase obj = new PageBase();
                    obj.UsertrackingAndRequestValidate();
                    Page.Header.Attributes.Add("MenuID", obj.MenuID.ToString());
                    /*#CC01 START END*/
                    //  BussinessLogic.PageBase.GetvalidSession();

                    //    if (PageBase.PageHeading != "" || PageBase.PageHeading != string.Empty)
                    //    {
                    //        lblPageHeading.Text = PageBase.PageHeading;
                    //        lblPageHeading.Visible = true;
                    //    }
                    //    else
                    //        tblHeading.Visible = false;
                    //}

                    string strLogInUserName = Convert.ToString(Session["DisplayName"]) + "(" + Convert.ToString(Session["RoleName"]) + ")";
                    // lblUserNameDesc.Text = strLogInUserName;
                    //if (PageBase.SalesChannelOpeningStockDate.HasValue ==true)
                    //{
                    //    lblOpeningDate.Text = PageBase.SalesChannelOpeningStockDate.Value.ToString("dd-MMM-yyyy");
                    //    divstock.Visible = true;
                    //}
                    //Pankaj Dhingra            This will show the link at the heading of the every page
                    //if (PageBase.MultipleBrandName != "")
                    //{
                    //    LBSwitchToBrand.Text = "Current Brand " + PageBase.MultipleBrandName+ " " + "(Switch To)";
                    //    LBSwitchToBrand.Visible = true;

                }

            }
            catch (Exception ex)
            {
                PageBase.Errorhandling(ex);
            }

        }
        void bindMenu()
        {

            string sbMenu = string.Empty;



            if (Session["MenuXML"] != null)
            {
                sbMenu = Session["MenuXML"].ToString();
                xmlDataSource.Data = sbMenu;
                xmlDataSource.DataBind();
            }
            else
            {
                if (BussinessLogic.PageBase.UserId > 0)
                {
                    DataSet ds;
                    using (MenuData ObjMenu = new MenuData())
                    {

                        ds = ObjMenu.getMenuHirechyByUserID(BussinessLogic.PageBase.UserId);
                        sbMenu = ds.GetXml().ToString();
                        Session["MenuXML"] = sbMenu;
                        xmlDataSource.Data = sbMenu;
                        xmlDataSource.DataBind();
                    };
                }
            }
            //xmlDataSource.Data = sbMenu;
            //xmlDataSource.DataBind();
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
    }
}