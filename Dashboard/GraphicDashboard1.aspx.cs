using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;


public partial class Dashboard_GraphicDashboard1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        //if(Convert.ToString(BussinessLogic.PageBase.UserId)=="")
        //{
        //    Response.Redirect(ConfigurationManager.AppSettings["siteurl"] + "/logout.aspx", false);
        //}
        //SetAPiValues();
    }

    //private void SetAPiValues()
    //{
    //    try
    //    {
    //        hdnConstr.Value = Convert.ToString(ConfigurationManager.AppSettings["APIConnStringKey"]);
    //        hdnAPIURL.Value = Convert.ToString(ConfigurationManager.AppSettings["APIurl"]);
    //        hdnUserID.Value = Convert.ToString(BussinessLogic.PageBase.UserId);
    //        hdnRoleID.Value = Convert.ToString(BussinessLogic.PageBase.RoleID);

    //        lnkBootstrap.Attributes.Add("href", PageBase.siteURL  + Convert.ToString(ConfigurationManager.AppSettings["AssetsPath"]) +"css/bootstrap.min.css");
    //        lnkGraphics.Attributes.Add("href", PageBase.siteURL + Convert.ToString(ConfigurationManager.AppSettings["AssetsPath"]) + "/css/GraphicDashboard.css");
    //    }
    //    catch (Exception ex)
    //    {            

    //        throw ex;
    //    }
    //}
}