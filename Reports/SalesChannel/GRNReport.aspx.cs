/*
 * Change log
 * DD-MMM-YYYY, Name, #CCXX, DEscription
 * 22-Jul-2016, Sumit Maurya, #CC01, fieldName changed from ReportQuantity to Quantity because there was no column named "ReportQuantity".
 * 24-Jul-2016, Karam Chand Sharma, #CC02, Added Stocj Desc in pivot.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Data;
using DevExpress.Utils;
using System.Configuration;
using DevExpress.Web.ASPxPivotGrid;

public partial class Reports_SalesChannel_GRNReport : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (hfSearch.Value=="1")
            binddata();
        ucMsg.ShowControl = false;
        if (!IsPostBack)
        {
            ucDateTo.Date = PageBase.ToDate;
            ucDateFrom.Date = PageBase.Fromdate;
            pnlSearch.Visible = false;
 
        }
    }
    bool isvalidate()
    {
        hfSearch.Value = "0";
        if (ucDateFrom.Date != "" && ucDateTo.Date == "")
        {
            ucMsg.ShowInfo("Please select from date");
            return false;
         }
        if (ucDateFrom.Date == "" && ucDateTo.Date != "")
        {
            ucMsg.ShowInfo("Please select to date");
            return false;
        }
        if (Convert.ToDateTime(ucDateFrom.Date) > Convert.ToDateTime(ucDateTo.Date))
        {
            ucMsg.ShowInfo("The date from cant exceed the to  date");
            return false;
        }

        return true;
    }

    void blankall()
    {
        hfSearch.Value = "0";
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
        ucMsg.Visible = false;
    }


    void binddata()
    {
        try
        {
            DataTable dt;
            //if (ViewState["Export"] != null)
            //{
            //    grdGRN.DataSource = (DataTable)ViewState["Export"];
            //    grdGRN.DataBind();
            //    pnlSearch.Visible = true;
            //}

            using (ReportData obj = new ReportData())
            {
                obj.UserId = PageBase.UserId;
                obj.SalesChannelId = PageBase.SalesChanelID;
                obj.SalesChannelID = PageBase.SalesChanelID;
                obj.FromDate = ucDateFrom.Date;
                obj.ToDate = ucDateTo.Date;

                if (ChkSB.Checked)
                    dt = obj.GetGRNInfoSB();
                else
                    dt = obj.GetGRNInfo();

                if (dt.Rows.Count > 0)
                {
                    hfSearch.Value = "1";
                    grdGRN.DataSource = dt;
                    grdGRN.DataBind();
                    //ViewState["Export"] = dt;
                    pnlSearch.Visible = true;


                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                    pnlSearch.Visible = false;
                }

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }


    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!isvalidate())
        {
            pnlSearch.Visible = false;
            return;
        }
        binddata();


    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        blankall();
        pnlSearch.Visible = false;
    }
    

    private void Export(bool saveAs)
    {


        ASPxPivotGridExporter1.OptionsPrint.PrintHeadersOnEveryPage = checkPrintHeadersOnEveryPage.Checked;
        ASPxPivotGridExporter1.OptionsPrint.PrintFilterHeaders = checkPrintFilterHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintColumnHeaders = checkPrintColumnHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintRowHeaders = checkPrintRowHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintDataHeaders = checkPrintDataHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;

        string fileName = string.Format("GRNReport_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
        switch (ddlExportFormat.SelectedIndex)
        {
            case 0:
                ASPxPivotGridExporter1.ExportXlsToResponse(fileName, saveAs);
                break;
            case 1:
                ASPxPivotGridExporter1.ExportXlsxToResponse(fileName, saveAs);
                break;
        }
    }

    protected void buttonOpen_Click(object sender, EventArgs e)
    {
        Export(false);
    }

    protected void buttonSaveAs_Click(object sender, EventArgs e)
    {
        Export(true);
    }


}
