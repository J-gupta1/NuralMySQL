using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cryptography;
using BussinessLogic;
using DataAccess;
using System.Data;


//Modified by:Pankaj Kumar
//Reason: At querystring there was space so I removed that space with "+"
public partial class Masters_HO_SalesChannel_ViewSalesChannelDetail :PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            StyleCss.Attributes.Add("href", "~/" + strAssets + "/CSS/popup.css");
            if (!IsPostBack)
            {
                int SalesChannelId = 0;
                if ((Request.QueryString["SalesChannelId"] != null) && (Request.QueryString["SalesChannelId"] != ""))
                {

                    SalesChannelId = Convert.ToInt32(Convert.ToString(Crypto.Decrypt((Request.QueryString["SalesChannelId"]).ToString().Replace(" ","+"), PageBase.KeyStr)));   //Pankaj Kumar
                    FillDetails(SalesChannelId);
                }
            }
        }
        catch (Exception ex)
        {
            lblHeader.Text =Resources.Messages.ErrorMsgTryAfterSometime;
            PageBase.Errorhandling(ex);
          
        }
    }
    public void FillDetails(int SalesChannelID)
    {
        DataTable DtRetailer = new DataTable();
        try
        {
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                ObjSalesChannel.SalesChannelID = SalesChannelID;
                ObjSalesChannel.BlnShowDetail = true;
                ObjSalesChannel.StatusValue = 2;
                ObjSalesChannel.CompanyId = PageBase.ClientId;
                ObjSalesChannel.UserID = PageBase.UserId;
                DtRetailer = ObjSalesChannel.GetSalesChannelInfo();
            };

            if (DtRetailer.Rows.Count > 0)
            {
                
           
                SalesChannelDetailList.Visible = true;
                SalesChannelDetailList.DataSource = DtRetailer;
                SalesChannelDetailList.DataBind();

            }
            else
            {
                {
                    SalesChannelDetailList.Visible = false;
                    SalesChannelDetailList.DataSource = null;
                }
            }
        }
        catch (Exception ex)
        {
            lblHeader.Text = Resources.Messages.ErrorMsgTryAfterSometime;
            PageBase.Errorhandling(ex);
            
        }

    }
}
