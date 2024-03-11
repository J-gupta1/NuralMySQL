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

public partial class Reports_SalesChannel_BatchwiseStockRpt : PageBase
{
    #region PageLoad Event
    protected void Page_Load(object sender, EventArgs e)
    {
        fncHide();
        if (ViewState["DsSalesInfo"] != null)
            BindGrid();
        if (!IsPostBack)
        {

            ucDateFrom.Date = PageBase.Fromdate;

        }
    }
    #endregion

    #region User defined function
    private void BindGrid()
    {
        try
        {

            DataSet DsSalesInfo;
            if (ViewState["DsSalesInfo"] != null)
            {
                ASPxPvtGrd.DataSource = (DataSet)ViewState["DsSalesInfo"];
                ASPxPvtGrd.DataBind();
                pnlSearch.Visible = true;
            }
            else
            {
                using (POC objRD = new POC())
                {

                    objRD.DateFrom = ucDateFrom.Date;

                    objRD.SalesChannelId = PageBase.SalesChanelID;
                    objRD.UserId = PageBase.UserId;

                    DsSalesInfo = objRD.GetBatchWiseStockReport();
                    ViewState["DsSalesInfo"] = DsSalesInfo;
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

        string fileName = string.Format("BatchWiseStockReport_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
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
        if (ViewState["DsSalesInfo"] != null)
            ViewState["DsSalesInfo"] = null;
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
        ViewState["DsSalesInfo"] = null;
        pnlSearch.Visible = false;
        ucDateFrom.Date = PageBase.Fromdate;


    }
}
