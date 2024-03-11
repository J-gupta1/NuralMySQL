using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using System.Data;

public partial class DOA_frmGRNPrint : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetDispatchDoaData();
        }
    }
    private void GetDispatchDoaData()
    {
        Int64 stockReceiveid = 0;

        try
        {
            if (Request.QueryString != null)
            {
                String strQuery = Convert.ToString(Cryptography.Crypto.Decrypt(Request.QueryString["stockReceiveId"].ToString().Replace(" ", "+"), PageBase.KeyStr));
                stockReceiveid = Convert.ToInt32(strQuery);
            }

            DataSet ds = new DataSet();
            clsDoaReport objreport = new clsDoaReport();
            objreport.SalesChannelId = PageBase.SalesChanelID;
            objreport.StockReceiveIdForPrint = stockReceiveid;
            DataSet dsresult = objreport.GetDoaGRNPrint();
            int countreceive = objreport.Receivedcountprint;

            if (countreceive > 0)
            {
                dvhiddenGrid.Visible = true;
                divnorecord.Visible = false;
                if (dsresult.Tables[0].Rows.Count > 0)
                {
                    lblSaleschannelname.Text = dsresult.Tables[0].Rows[0]["HO Name"].ToString();
                    lblWarehouseAddress.Text = dsresult.Tables[0].Rows[0]["HOAddress"].ToString();
                    lblWarehouseCity.Text = dsresult.Tables[0].Rows[0]["HoCity"].ToString();
                    lblWarehousePincode.Text = dsresult.Tables[0].Rows[0]["HoPincode"].ToString();
                    lblWarehouseStateName.Text = dsresult.Tables[0].Rows[0]["HoStateName"].ToString();
                    lblGstnid.Text = dsresult.Tables[0].Rows[0]["HOGSTIN"].ToString();
                    lblWarehouseStateCode.Text = dsresult.Tables[0].Rows[0]["HoStateCode"].ToString();
                    lblWarehousemobileno.Text = dsresult.Tables[0].Rows[0]["HoMobileNumber"].ToString();

                    lblconsigneename.Text = dsresult.Tables[0].Rows[0]["SalesChannelName"].ToString();
                    lblconsigneeaddress.Text = dsresult.Tables[0].Rows[0]["DistributorAddress"].ToString();
                    lblDistributorCity.Text = dsresult.Tables[0].Rows[0]["DistributorCityName"].ToString();
                    lbldispincode.Text = dsresult.Tables[0].Rows[0]["DistributorPinCode"].ToString();
                    lblconsigneestatename.Text = dsresult.Tables[0].Rows[0]["DistributorStatename"].ToString();
                    lblStateCode.Text = dsresult.Tables[0].Rows[0]["Distributorstatecode"].ToString();              
                    lblTinnumber.Text = dsresult.Tables[0].Rows[0]["DistributorGSTIN"].ToString();
                    lblMobilenumber.Text = dsresult.Tables[0].Rows[0]["DistributorMobileNumber"].ToString();
                    lblSTNNumber.Text = dsresult.Tables[0].Rows[0]["GRNNumber"].ToString();
                    lblSTNDate.Text = dsresult.Tables[0].Rows[0]["GRNdate"].ToString();
                    lblInvoicenumber.Text = dsresult.Tables[0].Rows[0]["InvoiceNo"].ToString();
                    lbldocketno.Text = dsresult.Tables[0].Rows[0]["GCNNo"].ToString();
                    lblstorelocation.Text = dsresult.Tables[0].Rows[0]["StoreLocation"].ToString();
                    lblRemark.Text = dsresult.Tables[0].Rows[0]["ReceiveRemark"].ToString();
                    lblreceiveby.Text = dsresult.Tables[0].Rows[0]["ReceivedBy"].ToString();
                }
                else
                {
                    lblSaleschannelname.Text = "";
                    lblWarehouseAddress.Text = "";
                    lblconsigneename.Text = "";
                    lblconsigneeaddress.Text = "";
                    lblTinnumber.Text = "";
                    lblTinnumber.Text = "";
                    lblSTNNumber.Text = "";
                    lblSTNDate.Text = "";
                    lbldocketno.Text = "";
                }
                if (dsresult.Tables[1].Rows.Count > 0)
                {
                    lblTotalAmount.Text = dsresult.Tables[1].Compute("SUM(PrimarySale_Price)", "").ToString();
                    lblQuantity.Text = dsresult.Tables[1].Compute("SUM(ReceiveQuantity)", "").ToString();
                  //  lblGrandTotal.Text = dsresult.Tables[1].Compute("SUM(PrimarySale_Price)", "").ToString();
                   // lblTotalinWords.Text = dsresult.Tables[1].Rows[0]["TotalPriceinwords"].ToString();
                    gvDispatch.DataSource = dsresult.Tables[1];
                    gvDispatch.DataBind();

                }
                else
                {
                    lblTotalAmount.Text = "";
                    lblQuantity.Text = "";
                    gvDispatch.DataSource = null;
                    gvDispatch.DataBind();
                }
                if(dsresult.Tables[3].Rows.Count>0)
                {
                    lblTotalaftertax.Text = dsresult.Tables[3].Rows[0]["TotalAfterTax"].ToString();
                    lblGrandTotal.Text = dsresult.Tables[3].Rows[0]["TotalAfterTax"].ToString();
                    lblTotalinWords.Text = dsresult.Tables[3].Rows[0]["GrantTotalinwords"].ToString();
                }
                if(dsresult.Tables[2].Rows.Count>0)
                {
                    gvTaxdetail.DataSource = dsresult.Tables[2];
                    gvTaxdetail.DataBind();
                }
                else
                {
                    
                    gvTaxdetail.DataSource = null;
                    gvTaxdetail.DataBind();
                }
            }
            else
            {
                dvhiddenGrid.Visible = false;
                divnorecord.Visible = true;
                lblnorecord.Text = "All item Short received.";
            }

        }
        catch (Exception ex)
        {

        }
    }
}