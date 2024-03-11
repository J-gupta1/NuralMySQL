using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cryptography;
using Resources;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Data;
using DevExpress.XtraPrinting;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Globalization;
using Microsoft.ApplicationBlocks.Data;
using System.Resources;
using System.Data.SqlClient;
using BussinessLogic;

public partial class Transactions_Billing_SalesOrderPrint : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ReferenceID"] != null)
        {
            Int32 ReferenceID = Convert.ToInt32(Request.QueryString["ReferenceID"]);
            string strInvoiceNumber = Convert.ToString(Request.QueryString["InvoiceNumber"]);
            BindDataForPrint(ReferenceID, strInvoiceNumber);
        }
    }


    private void BindDataForPrint(Int32 ReferenceID, string InvoicNumber)
    {
        DataSet dsSalesOrder;
        using (clsSalesOrder obj = new clsSalesOrder())
        {
            obj.OrderId = ReferenceID;
            dsSalesOrder = obj.SelectOrderetailsForPrint();
          
            if (dsSalesOrder.Tables.Count==0)
            {
                ucMessage1.ShowInfo("No record Found");
                updMessage.Update();
                ReportToolbar1.Visible = false;
                return;
            }
            else
            {
                DataTable dt = dsSalesOrder.Tables[0];
                SalesOrderReportRpt objreport = new SalesOrderReportRpt();
                objreport.DataSource = dt;
                objreport.DataMember = "Table";
                if (dsSalesOrder.Tables.Count > 1)
                {
                    objreport.xrTableCell30.DataBindings.Add("Text", dsSalesOrder.Tables[1], "TotalQty");
                }
               
                objreport.xrrate.DataBindings.Add("Text", dsSalesOrder.Tables[0], "BasicRate", "{0:0.00}");
                objreport.xrSErviceTax.DataBindings.Add("Text", dsSalesOrder.Tables[0], "ST", "{0:0.00}");
                objreport.xrAmount.DataBindings.Add("Text", dsSalesOrder.Tables[0], "Amount", "{0:0.00}");
                objreport.xrLedger.DataBindings.Add("Text", dsSalesOrder.Tables[0], "LedgerBal", "{0:0.00}");
                objreport.xrPending.DataBindings.Add("Text", dsSalesOrder.Tables[0], "SoPending", "{0:0.00}");
                objreport.xrTotalst.DataBindings.Add("Text", dsSalesOrder.Tables[1], "TotalSt", "{0:0.00}");
                objreport.xrTotalAmount.DataBindings.Add("Text", dsSalesOrder.Tables[1], "TotalAmount", "{0:0.00}");
              
                ReportToolbar1.Visible = true;
                ReportViewer1.Report = null;
                ReportViewer1.DataBind();
                ReportViewer1.Report = objreport;
               
                ReportViewer1.DataBind();

            }
        }

    }


    protected void ReportViewer1_CacheReportDocument(object sender, DevExpress.XtraReports.Web.CacheReportDocumentEventArgs e)
    {
        e.Key = Guid.NewGuid().ToString();
        Page.Session[e.Key] = e.SaveDocumentToMemoryStream();

    }
    protected void ReportViewer1_RestoreReportDocumentFromCache(object sender, DevExpress.XtraReports.Web.RestoreReportDocumentFromCacheEventArgs e)
    {
        Stream stream = Page.Session[e.Key] as Stream;
        if (stream != null)
            e.RestoreDocumentFromStream(stream);
    }

    protected void dd_Click(object sender, EventArgs e)
    {
      
    }
    private void printingSystem_StartPrint(object sender, PrintDocumentEventArgs e)
    {
       
        e.PrintDocument.PrinterSettings.FromPage = 1;
        e.PrintDocument.PrinterSettings.ToPage = 3;
    }


}
