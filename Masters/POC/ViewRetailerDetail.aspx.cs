using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using Cryptography;
using System.Data;
using DataAccess;

public partial class Masters_HO_Retailer_ViewRetailerDetail : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
         try
         {
             StyleCss.Attributes.Add("href", "~/" + strAssets + "/CSS/popup.css");
            if (!IsPostBack)
            {
                int SalesChannelId = 0;
                if ((Request.QueryString["RetailerId"] != null) && (Request.QueryString["RetailerId"] != ""))
                {

                    SalesChannelId = Convert.ToInt32(Convert.ToString(Crypto.Decrypt((Request.QueryString["RetailerId"].ToString().Replace(" ","+")), PageBase.KeyStr)));
                    FillDetails(SalesChannelId);
                }
            }
        }
        catch (Exception ex)
        {
            lblHeader.Text = Resources.Messages.ErrorMsgTryAfterSometime;
            PageBase.Errorhandling(ex);
           
        }
    }
    public void FillDetails(int RetailerId)
    {
        DataTable DtRetailer = new DataTable();
        try
        {
            using (RetailerData ObjRetailer = new RetailerData())
            {
                ObjRetailer.RetailerID = RetailerId;

                DtRetailer = ObjRetailer.GetRetailerInfo();
            };

            if (DtRetailer.Rows.Count > 0)
            {
                
           
                RetailerDetailList.Visible = true;
                RetailerDetailList.DataSource = DtRetailer;
                RetailerDetailList.DataBind();

            }
            else
            {
                {
                    RetailerDetailList.Visible = false;
                    RetailerDetailList.DataSource = null;
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
