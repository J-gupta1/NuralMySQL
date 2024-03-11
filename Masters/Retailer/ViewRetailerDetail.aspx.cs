/* Change Log
 * 20 Oct 2015, Sumit Maurya, #CC01, Text "Counter Potential" changed to "Counter Potential in Volume"
 * 16-Jun-2016, Sumit Maurya, #CC02, Label Name of Tin Number gets displayed as VAT No. according to config.
 * 09-July-2018,Vijay Kumar Prajapati,#CC03,Add Gridview for display saleschannelname and salesmanname for karbonn mobile for retailer multiple mapping.
 */

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

                    SalesChannelId = Convert.ToInt32(Convert.ToString(Crypto.Decrypt((Request.QueryString["RetailerId"].ToString().Replace(" ", "+")), PageBase.KeyStr)));
                    FillDetails(SalesChannelId);
                    FillGridDetails(SalesChannelId);


                    /* #CC02 Add Start */
                    if (PageBase.ChangeTinLabel == 1)
                    {
                        foreach (DataListItem li in RetailerDetailList.Items)
                        {
                            Label lbltinNumberheading = (Label)li.FindControl("txtTinNoHeading") as Label;
                            lbltinNumberheading.Text = "VAT No:";
                        }
                    }
                    /* #CC02 Add Start */

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
    /*#CC03 Added Started*/
    public void FillGridDetails(int RetailerId)
    {
        DataTable DtRetailer = new DataTable();
        try
        {
            using (RetailerData ObjRetailer = new RetailerData())
            {
                ObjRetailer.RetailerID = RetailerId;
                ObjRetailer.Type = 2;

                DtRetailer = ObjRetailer.GetRetailerInfo();
            };

            if (DtRetailer.Rows.Count > 0)
            {


                Gridsaleschanneldetails.Visible = true;
                Gridsaleschanneldetails.DataSource = DtRetailer;
                Gridsaleschanneldetails.DataBind();

            }
            else
            {
                {
                    Gridsaleschanneldetails.Visible = false;
                    Gridsaleschanneldetails.DataSource = null;
                }
            }
        }
        catch (Exception ex)
        {
            lblHeader.Text = Resources.Messages.ErrorMsgTryAfterSometime;
            PageBase.Errorhandling(ex);

        }

    }
    /*#CC03 Added End*/
}
