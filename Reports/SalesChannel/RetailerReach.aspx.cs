﻿using System;
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

public partial class Reports_SalesChannel_RetailerReach : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (hfSearch.Value=="1")
            binddata();

        if (!IsPostBack)
        {
            ucDateFrom.Date = Fromdate;
            ucDateTo.Date = ToDate;

        }



    }

    bool isvalidate()
    {
        hfSearch.Value = "0";
        if (Convert.ToDateTime(ucDateFrom.Date) > Convert.ToDateTime(ucDateTo.Date))
        {
            ucMsg.ShowInfo("From Date can't be gretaer then the to date ");
            return false;
        }
        return true; 
    }


    void binddata()
      
    {
        try
        {
            DataTable dt;
            DataTable dt1;

           // if (ViewState["Export"] != null)
            //{
            //    grdRetailerReach.DataSource = (DataTable)ViewState["Export"];
            //    grdRetailerReach.DataBind();
            //    pnlSearch.Visible = true;
            //}


            using (ReportData obj = new ReportData())
            {
                obj.FromDate = ucDateFrom.Date;
                obj.ToDate = ucDateTo.Date;
                obj.UserId = PageBase.UserId;
                 dt = obj.GetRetailerReachInfo();
                 dt1=obj.headerReplacement(dt);
                 if (dt1.Rows.Count > 0)
                {
                    hfSearch.Value = "1";
                    grdRetailerReach.DataSource = dt1;
                    grdRetailerReach.DataBind();
                   // ViewState["Export"] = dt;
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



    void blankall()
    {
        hfSearch.Value = "0";
        ucMsg.Visible = false;
        ucDateTo.Date = ToDate;
        ucDateFrom.Date = Fromdate;
        pnlSearch.Visible = false;
    }


    


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!isvalidate())
        {
            return;
        }

        binddata();

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        blankall();

    }

    private void Export(bool saveAs)
    {


        ASPxPivotGridExporter1.OptionsPrint.PrintHeadersOnEveryPage = checkPrintHeadersOnEveryPage.Checked;
        ASPxPivotGridExporter1.OptionsPrint.PrintFilterHeaders = checkPrintFilterHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintColumnHeaders = checkPrintColumnHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintRowHeaders = checkPrintRowHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintDataHeaders = checkPrintDataHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;

        string fileName = string.Format("RetailerReachReport_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
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