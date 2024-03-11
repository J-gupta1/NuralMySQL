#region NameSpace
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

public partial class Reports_SalesChannel_TertioryReport : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
      
        fncHide();
        if (hfSearch.Value == "1" )
            BindGrid();
        if (!IsPostBack)
        {

            ShowScrollBar(true);
           
            ucDateTo.Date = PageBase.ToDate;
            ucDateFrom.Date = PageBase.Fromdate;
        }
    }
    private void BindGrid()
    {
        try
        {
            DataSet DsTertioryInfo;
          
                using (ReportData objRD = new ReportData())
                {
                    objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                    objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                    objRD.SalesChannelID = PageBase.SalesChanelID;
                    objRD.UserId = PageBase.UserId;
                    DsTertioryInfo = objRD.GetTertioryReport();
                    if (DsTertioryInfo.Tables[0].Rows.Count > 0)
                    {
                       
                        hfSearch.Value = "1";
                        ASPxPvtGrd.DataSource = DsTertioryInfo;
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
    bool pageValidate()
    {
        //if (ViewState["DsTertioryInfo"] != null)
        //    ViewState["DsTertioryInfo"] = null;
        hfSearch.Value = "0";
        if ((Convert.ToDateTime(ucDateTo.Date) > DateTime.Now.Date))
        {
            ucMsg.ShowInfo("Date should be less than or equal to current date.");
            return false;
        }
        return true;
    }
    private void Export(bool saveAs)
    {


        ASPxPivotGridExporter1.OptionsPrint.PrintHeadersOnEveryPage = checkPrintHeadersOnEveryPage.Checked;
        ASPxPivotGridExporter1.OptionsPrint.PrintFilterHeaders = checkPrintFilterHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintColumnHeaders = checkPrintColumnHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintRowHeaders = checkPrintRowHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintDataHeaders = checkPrintDataHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;

        string fileName = string.Format("TeritoryReport_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //ViewState["DsTertioryInfo"] = null;
        hfSearch.Value = "0";
        pnlSearch.Visible = false;
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (pageValidate())
            BindGrid();
        else
            pnlSearch.Visible = false;
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
}
