using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using BussinessLogic;

public partial class DOA_frmPrintDispatchDOA : PageBase
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
        Int64 stockDispatchid = 0;
        try
        {
            if (Request.QueryString != null)
            {
                String strQuery = Convert.ToString(Cryptography.Crypto.Decrypt(Request.QueryString["StockDispatchID"].ToString().Replace(" ", "+"), PageBase.KeyStr));
                stockDispatchid = Convert.ToInt32(strQuery);
            }
            DataSet ds = new DataSet();
            clsDoaReport objreport = new clsDoaReport();
            objreport.SalesChannelId = PageBase.SalesChanelID;
            objreport.Dispatchid = stockDispatchid;
            DataSet dsresult = objreport.GetDoaDispatchPrint();
            if (dsresult.Tables[0].Rows.Count > 0)
            {
                lblSaleschannelname.Text = dsresult.Tables[0].Rows[0]["DistributorName"].ToString();
                lblWarehouseAddress.Text = dsresult.Tables[0].Rows[0]["DistributorAddress"].ToString();
                lblGstnid.Text = dsresult.Tables[0].Rows[0]["DistributorGSTNID"].ToString();
                lblmobilenodistributor.Text = dsresult.Tables[0].Rows[0]["DistributorMobilenumber"].ToString();
                lblWarehouseStateCode.Text = dsresult.Tables[0].Rows[0]["DistributorStateCode"].ToString();
                lblWarehouseStateName.Text = dsresult.Tables[0].Rows[0]["DistributorStateName"].ToString();
                lblpincodewarehouse.Text = dsresult.Tables[0].Rows[0]["DistributorPincode"].ToString();
                lblconsigneename.Text = dsresult.Tables[0].Rows[0]["HoName"].ToString();
                lblconsigneeaddress.Text = dsresult.Tables[0].Rows[0]["HOAddress"].ToString();
                lblconsigneestatecode.Text = dsresult.Tables[0].Rows[0]["HOStateCode"].ToString();
                lblconsigneestatename.Text = dsresult.Tables[0].Rows[0]["HOStateName"].ToString();
                lbldispincode.Text = dsresult.Tables[0].Rows[0]["HOPincode"].ToString();
                lblCstnumber.Text = dsresult.Tables[0].Rows[0]["HOGSTIN"].ToString();
                lblMobilenumber.Text = dsresult.Tables[0].Rows[0]["HOMobileno"].ToString();
                lblSTNNumber.Text = dsresult.Tables[0].Rows[0]["STNNumber"].ToString();
                lblSTNDate.Text = Convert.ToDateTime(dsresult.Tables[0].Rows[0]["GCNDate"]).ToString("MM/dd/yyyy");
                lbldocketno.Text = dsresult.Tables[0].Rows[0]["GCNNo"].ToString();
                lblinvoicedate.Text = Convert.ToDateTime(dsresult.Tables[0].Rows[0]["DispatchDate"]).ToString("MM/dd/yyyy");
                lblinvoiceno.Text = dsresult.Tables[0].Rows[0]["InvoiceNo"].ToString();
                lblWarehouseCity.Text = dsresult.Tables[0].Rows[0]["HOCityName"].ToString();
                lblDistributorCity.Text = dsresult.Tables[0].Rows[0]["DistributorCity"].ToString();

            }
            else
            {
                lblSaleschannelname.Text = "";
                lblWarehouseAddress.Text = "";
                lblconsigneename.Text = "";
                lblconsigneeaddress.Text = "";
                lbldispincode.Text = "";
                lblCstnumber.Text = "";
                lblMobilenumber.Text = "";
                lblSTNNumber.Text = "";
                lblSTNDate.Text = "";
                lbldocketno.Text = "";
                lblinvoicedate.Text = "";
                lblinvoiceno.Text = "";
                lblpincodewarehouse.Text = "";
                lblWarehouseStateCode.Text = "";
                lblWarehouseStateName.Text = "";
                lblconsigneestatecode.Text = "";
                lblconsigneestatename.Text = "";
            }
            if (dsresult.Tables[1].Rows.Count > 0)
            {
                lblTotalAmount.Text = dsresult.Tables[1].Compute("SUM(PrimarySale_Price)", "").ToString();
                lblQuantity.Text = dsresult.Tables[1].Compute("SUM(StockQuantity)", "").ToString();
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

        }
        catch (Exception ex)
        {

        }
    }
}