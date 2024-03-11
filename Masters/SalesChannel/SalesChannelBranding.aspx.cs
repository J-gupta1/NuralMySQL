using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Text;

public partial class Masters_SalesChannel_SalesChannelBranding : PageBase
{
    protected string strSiteUrl = PageBase.siteURL;
    protected string strAssets = PageBase.strAssets;
    protected void Page_Load(object sender, EventArgs e)
    {
        string strLogInUserName = Convert.ToString(Session["DisplayName"]) + "(" + Convert.ToString(Session["RoleName"]) + ")";
        lblUserNameDesc.Text = strLogInUserName;
        if (!IsPostBack)
        {
            Session["MultipleBrandName"] = "";      //At the time of checking i checked the value ""
            if (PageBase.SalesChanelTypeID.ToString() != "0")
            {
                CheckBrandSalesChannelMapping(PageBase.SalesChanelTypeID);
            }
        }
        bindAssets();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Session["MultipleBrandName"] = ddlBrandMapping.SelectedItem;
        Session["Brand"] = ddlBrandMapping.SelectedValue;
        Response.Redirect("~/Default.aspx", false);
    }
    public void CheckBrandSalesChannelMapping(int SalesChannelTypeID)
    {
        using (SalesChannelData ObjBrandMapping = new SalesChannelData())
        {
            DataTable dtBrand = new DataTable();
            ObjBrandMapping.SalesChannelTypeID = Convert.ToInt16(SalesChannelTypeID);
            ObjBrandMapping.ShowBranding = true;
            ObjBrandMapping.SalesChannelID = PageBase.SalesChanelID;
            dtBrand = ObjBrandMapping.GetSalesChannelInfoForBrand();
            if (dtBrand.Rows.Count > 1)
            {
                ddlBrandMapping.Items.Clear();
                ddlBrandMapping.DataSource = dtBrand;
                ddlBrandMapping.DataTextField="BrandName";
                ddlBrandMapping.DataValueField = "BrandID";
                ddlBrandMapping.DataBind();
                ddlBrandMapping.Items.Insert(0, new ListItem("All", "-1"));

            }
            else if (dtBrand.Rows.Count == 1)
            {
                Session["MultipleBrandName"] = "";     //For user having multiplebrand mapping or not
                Session["Brand"] = dtBrand.Rows[0]["BrandID"].ToString();
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                Session["MultipleBrandName"] = "";     //For user having multiplebrand mapping or not
                Session["Brand"] = 0;
                Response.Redirect("~/Default.aspx", false);
            }
           


        }
    }
    void bindAssets()
    {
        ImgSideLogo.ImageUrl = "~/" + strAssets + "/CSS/Images/zedsaleslogo.gif";
        hyplogo.ImageUrl = "~/" + strAssets + "/CSS/Images/innerlogo.gif";
        hypfooterlogo.ImageUrl = "~/" + strAssets + "/CSS/Images/footerimg.gif";
        hypfooterlogo.NavigateUrl = PageBase.redirectURL;
        hypfooterlogo.Target = "_blank";
        cssStyle.Attributes.Add("href", "~/" + strAssets + "/CSS/style.css");
        //csswithoutmaster.Attributes.Add("href", "~/" + strAssets + "/CSS/withoutmaster.css");
        bootstrapCss.Attributes.Add("href", "~/" + strAssets + "/CSS/bootstrap.min.css");
       
    }


}
