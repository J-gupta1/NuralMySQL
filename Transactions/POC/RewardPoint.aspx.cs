using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess;
using BussinessLogic;

public partial class Transactions_POC_RewardPoint :PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        StyleCss.Attributes.Add("href", "~/" + strAssets + "/CSS/popup.css");
        if (Request.QueryString["iD"]!=null &&Request.QueryString["iD"]!="")
        {
            string Str = Request.QueryString["iD"].ToString ();
            string[] arr = Str.Split(',');
            Int32 skuid = 0;
            Int32 OfferBasedOnDetailID = 0;
           
            skuid = Convert.ToInt32( arr[0]);
            OfferBasedOnDetailID = Convert.ToInt32(arr[1]);
          
            using (POC objPoc = new POC())
            {

                objPoc.InvoiceDate = arr[2].ToString();
                objPoc.SKUID = skuid;
                objPoc.OfferBasedOnDetailID = OfferBasedOnDetailID;
                DataTable DtReward = objPoc.GetOfferBySkuReward();
                GridSales.DataSource = DtReward;
                GridSales.DataBind();
              
            }
        }
    }
}
