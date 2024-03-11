using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraReports.UI;
using System.Data;
using ZedService;
using System.Web.Security;
using ZedService;
using BussinessLogic;

public partial class Transactions_SAPAnnexure_FrmPrintAnnexure : PageBase
{
    ClsSAPIntegrationDNPSI objDefectiveDispatch = new ClsSAPIntegrationDNPSI();
    xrRptDispatchImeiDetail DispatchImeiDetail = new xrRptDispatchImeiDetail();
    protected void Page_Load(object sender, EventArgs e)
    {
        // TimeoutCheck();

        fnFillAnnuxerDetail();
    }

    private DataSet GetData(Int64 PSIID)
    {

        try
        {
            DataSet ds = new DataSet();

            ds = objDefectiveDispatch.GetAnnuxerPrint(PSIID);

            return ds;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void fnFillAnnuxerDetail()
    {
        try
        {
            String strQuery = Convert.ToString(Cryptography.Crypto.Decrypt(Request.QueryString["StockDispatchID"].ToString().Replace(" ", "+"), PageBase.KeyStr));
            Int64 intStockDispatchID = 0;
            Int64.TryParse(strQuery, out intStockDispatchID);
            DataSet ds = new DataSet();
            ds = GetData(intStockDispatchID);
            if (ds.Tables.Count > 0)
            {
                DispatchImeiDetail.DataSource = ds.Tables[0];


                DispatchImeiDetail.xrTableCell12.DataBindings.Add("Text", null, "ModelDetail");
                DispatchImeiDetail.DataMember = ds.Tables[0].TableName;
               // DispatchImeiDetail.xrPrintDateTime.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                DispatchImeiDetail.xrLabel4InvoiceDate.Text = Convert.ToString(ds.Tables[0].Rows[0]["InvoiceDate"]);
                DispatchImeiDetail.xrLabel2InvoiceNo.Text = Convert.ToString(ds.Tables[0].Rows[0]["InvoiceNumber"]);
                DispatchImeiDetail.xrSku.DataBindings.Add("Text", null, "SKUDetail");
                DispatchImeiDetail.xrInvoice.DataBindings.Add("Text", null, "PSIQuantity");
                DispatchImeiDetail.xrModel.DataBindings.Add("Text", null, "ModelCode");
                DispatchImeiDetail.xrSkuname.DataBindings.Add("Text", null, "SKUDesc");
                DispatchImeiDetail.xrTableSerial.DataBindings.Add("Text", null, "IMEIDetail");
                DispatchImeiDetail.CreateDocument();
                DispatchImeiDetail.Landscape = false;
                
                DispatchImeiDetail.PrintingSystem.ContinuousPageNumbering = true;
                rptViewer.Report = DispatchImeiDetail;
                rptToolBar.ReportViewer = rptViewer;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message.ToString();

        }
    }
}
