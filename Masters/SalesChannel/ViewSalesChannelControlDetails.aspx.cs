using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;

public partial class Masters_SalesChannel_ViewSalesChannelControlDetails : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lnkStyle.Attributes.Add("href", "~/" + strAssets + "/CSS/Style.css?" + DateTime.Now.Ticks);
        LnkPopUP.Attributes.Add("href", "~/" + strAssets + "/CSS/popup.css?" + DateTime.Now.Ticks);
       
        string strURL = Request.Url.ToString();
        char c = '?';
        string[] strp = strURL.Split(c);
        Int16 saleschanneltypeid = Convert.ToInt16(strp[1]);
        if (saleschanneltypeid == 0)
        {
            return;
        }
        ucSalesChannel.SaleschannelTypeID = saleschanneltypeid;
        if (!IsPostBack)
        {
           
           
        }
        
    }
}
