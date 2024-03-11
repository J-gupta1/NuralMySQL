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
//using DevExpress.XtraPivotGrid;
using DevExpress.Utils;
#endregion

public partial class Reports_SalesChannel_CircleWiseTertioryReport :PageBase
{
    
    decimal decPrimary, decTertiory;
    #region PageLoad Event
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (hfSearch.Value == "1")
            BindGrid();
        if (!IsPostBack)
        {
            //ShowScrollBar(true);
            fncHide();
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;
        }
    }
    #endregion

    #region User defined function
    private void BindGrid()
    {
        try
        {

            DataSet DsSalesInfo;
            using (ReportData objReport = new ReportData())
            {
                objReport.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                objReport.DateTo = Convert.ToDateTime(ucDateTo.Date);
                objReport.SalesChannelID = PageBase.SalesChanelID;
                objReport.UserId = PageBase.UserId;
                DsSalesInfo = objReport.GetCircleWiseStateWiseReport();
                hfSearch.Value = "1";

                if (DsSalesInfo.Tables[0].Rows.Count > 0)
                {
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
    private void Export(bool saveAs)
    {
        ASPxPivotGridExporter1.OptionsPrint.PrintHeadersOnEveryPage = checkPrintHeadersOnEveryPage.Checked;
        ASPxPivotGridExporter1.OptionsPrint.PrintFilterHeaders = checkPrintFilterHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintColumnHeaders = checkPrintColumnHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintRowHeaders = checkPrintRowHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintDataHeaders = checkPrintDataHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;

        string fileName = string.Format("Dashboard_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
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
            ucMsg.ShowInfo(Resources.Messages.DateRangeValidation);
            return false;
        }

        _date = DateTime.Compare(Convert.ToDateTime(ucDateTo.Date), Convert.ToDateTime(ucDateFrom.Date));
        if (_date < 0)
        {
            ucMsg.ShowInfo(Resources.Messages.InvalidDate);
            return false;
        }

        return true;
    }
    #endregion

    #region Page Control Events

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (pageValidate())
            BindGrid();
        else
            pnlSearch.Visible = false;

    }
    #endregion
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        hfSearch.Value = "0";
        pnlSearch.Visible = false;
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
    }


    protected void ASPxPvtGrd_CustomUnboundFieldData(object sender, CustomFieldDataEventArgs e)
    {
        decimal decTertiary = Convert.ToDecimal(e.GetListSourceColumnValue("Tertiary"));
        decimal decPrimary = Convert.ToDecimal(e.GetListSourceColumnValue("Primary"));
        if (decPrimary > 0)
            e.Value = Math.Round(((decTertiary / decPrimary) * 100), 2);
        else
            e.Value = 0;
    }
    protected void ASPxPvtGrd_CustomCellDisplayText(object sender, DevExpress.Web.ASPxPivotGrid.PivotCellDisplayTextEventArgs e)
    {

        try
        {
            DevExpress.Web.ASPxPivotGrid.PivotGridField objTertiary = ASPxPvtGrd.Fields["Tertiary"];
            DevExpress.Web.ASPxPivotGrid.PivotGridField objPrimary = ASPxPvtGrd.Fields["Primary"];

            DevExpress.Web.ASPxPivotGrid.PivotGridField colPercentage = ASPxPvtGrd.Fields["Percentage"];
            if ((e.ColumnValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal) || (e.RowValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal))
            {

                if (e.DataField.FieldName == "Primary")
                {
                    decPrimary = Convert.ToDecimal(e.GetCellValue(e.DataField));
                }
                if (e.DataField.FieldName == "Tertiary")
                {
                    decTertiory = Convert.ToDecimal(e.GetCellValue(e.DataField));
                }
                if (e.DataField.FieldName == "Percentage")
                {
                    if (decPrimary > 0)
                    {
                        e.DisplayText = Convert.ToString(Math.Round((decTertiory * 100 / decPrimary),2));
                    }
                    else
                    {
                        e.DisplayText = "0";
                    }
                }
            }


        }
        catch (Exception ex)
        {

        }
    }
}
 

   



