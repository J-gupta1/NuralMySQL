#region  NameSpace
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
using DevExpress.Web.ASPxPivotGrid;
using System.Configuration;
using DevExpress.XtraPivotGrid;
using DevExpress.Utils;
#endregion

public partial class Reports_SalesChannel_PrimaryOrderRpt : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (hfSearch.Value == "1")
            BindGrid();

        fncHide();
       if (!IsPostBack)
        {
            ShowScrollBar(true);
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;

        }
    }
    #region User defined function
    private void BindGrid()
    {
        try
        {
            DataSet DsSalesInfo;

            using (ReportData objOrder = new ReportData())
            {

                objOrder.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                objOrder.DateTo = Convert.ToDateTime(ucDateTo.Date);
                objOrder.SalesChannelID = PageBase.SalesChanelID;
                objOrder.UserId = PageBase.UserId;
                DsSalesInfo = objOrder.GetPrimaryOrderReport();
                if (DsSalesInfo.Tables[0].Rows.Count > 0)
                {
                    hfSearch.Value = "1";
                    ASPxPvtGrd.DataSource = DsSalesInfo;
                    ASPxPvtGrd.DataBind();
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
    protected void ShowScrollBar(bool show)
    {
        if (show)
        {

            ASPxPvtGrd.Width = Unit.Parse("950px");
            ASPxPvtGrd.OptionsView.ShowHorizontalScrollBar = true;
        }
        else
        {
            ASPxPvtGrd.Width = Unit.Parse("100%");
            ASPxPvtGrd.OptionsView.ShowHorizontalScrollBar = false;
        }

    }
    private void Export(bool saveAs)
    {
        ASPxPivotGridExporter1.OptionsPrint.PrintHeadersOnEveryPage = checkPrintHeadersOnEveryPage.Checked;
        ASPxPivotGridExporter1.OptionsPrint.PrintFilterHeaders = checkPrintFilterHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintColumnHeaders = checkPrintColumnHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintRowHeaders = checkPrintRowHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintDataHeaders = checkPrintDataHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;

        string fileName = string.Format("OrderReport_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
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

    private void fncHide()
    {
        ucMsg.ShowControl = false;
        pnlSearch.Visible = false;
    }
    bool pageValidate()
    {
        hfSearch.Value = "0";
        int _date = 0;
        if ((Convert.ToDateTime(ucDateFrom.Date) > DateTime.Now.Date) || (Convert.ToDateTime(ucDateTo.Date) > DateTime.Now.Date))
        {
            ucMsg.ShowInfo("Date should be less than or equal to current date.");
            return false;
        }

        _date = DateTime.Compare(Convert.ToDateTime(ucDateTo.Date), Convert.ToDateTime(ucDateFrom.Date));
        if (_date < 0)
        {
            ucMsg.ShowInfo("Invalid date range");
            return false;
        }
        return true;
    }
    #endregion
    #region Page Control Events
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        pnlSearch.Visible = false;
        ucMsg.ShowControl = false;
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
        hfSearch.Value = "0";
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (pageValidate())
            BindGrid();
        else
            pnlSearch.Visible = false;

    }
    #endregion
}
