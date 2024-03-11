using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess;
using BussinessLogic;

public partial class Retailer_Specific_ViewAdCampaigns : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindList();
        }
    }
    private void BindList()
    {
        DataTable dt;
        using (Advertisement objAd = new Advertisement())
        {
            objAd.adStatus = 1;
            dt = objAd.GetAdvertisement();
            dtlistAd.DataSource = dt;
            dtlistAd.DataBind();
            if (objAd.ErrorMsg != "")
            {
 
            }
        };
    }
}
