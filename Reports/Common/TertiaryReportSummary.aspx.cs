/*
 28-Feb-2020, Balram Jha #CC01 changes to add userId in reports
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
using DevExpress.Web.ASPxPivotGrid;
using System.Configuration;
using DevExpress.XtraPivotGrid;
using DevExpress.Utils;

public partial class Reports_Common_TertiaryReportSummary : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        ucMsg.Visible = false;
        if (!IsPostBack)
        {

            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;
            BindND();
            BindModel();
            ddlSku.Items.Insert(0, new ListItem("Select", "0"));
        }
        if (hfSearch.Value == "1")
            BindPivotGrid();
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (ValidateSearchFilter() == true)
            {
                BindPivotGrid();
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        hfSearch.Value = "0";
        clearfields();
    }
    protected void ddlModelName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlModelName.SelectedValue == "0")
            {
                ddlSku.Items.Clear();
                ddlSku.Items.Insert(0, new ListItem("Select", "0"));
                ddlSku.SelectedValue = "0";
            }
            else
            {
                BindSku();
            }

            ucMsg.Visible = false;

        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    void BindSku()
    {
        using (RetailerData objsku = new RetailerData())
        {
            objsku.ModelId = Convert.ToInt32(ddlModelName.SelectedValue);
            DataTable dtmodelfil = objsku.GetAllActiveSKU();
            String[] colArray1 = { "Skuid", "SkuName" };
            PageBase.DropdownBinding(ref ddlSku, dtmodelfil, colArray1);


        }
    }
    void BindND()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            string[] strWarehouse = { "SalesChannelID", "SalesChannelName" };
            DataSet dsData = ObjSalesChannel.GetWarehouseAndNDS();

            string[] strNDS = { "SalesChannelID", "SalesChannelName" };
            PageBase.DropdownBinding(ref ddlNDS, dsData.Tables[1], strNDS);

        }
    }
    void BindModel()
    {
        using (ProductData objproduct = new ProductData())
        {
            objproduct.ModelProdCatId = 0;
            objproduct.ModelSelectionMode = 1;
            DataTable dtmodelfil = objproduct.SelectModelInfo();
            String[] colArray1 = { "ModelID", "ModelName" };
            PageBase.DropdownBinding(ref ddlModelName, dtmodelfil, colArray1);


        }
    }

    public bool ValidateSearchFilter()
    {
        bool result = true;
        try
        {
            hfSearch.Value = "0";
            if (Convert.ToDateTime(ucDateTo.Date) < Convert.ToDateTime(ucDateFrom.Date))
            {
                ucMsg.ShowInfo("from date cannot be greater than to date.");
                result = false;
                return result;
            }
        }
        catch (Exception ex)
        {

        }
        return result;
    }
    protected void buttonSaveAs_Click(object sender, EventArgs e)
    {
        Export(true);
    }
    protected void buttonOpen_Click(object sender, EventArgs e)
    {
        Export(false);
    }
    private void Export(bool saveAs)
    {

        try
        {
            // btnSearch_Click(null, null);
            ASPxPivotGridExporter1.OptionsPrint.PrintHeadersOnEveryPage = checkPrintHeadersOnEveryPage.Checked;
            ASPxPivotGridExporter1.OptionsPrint.PrintFilterHeaders = checkPrintFilterHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
            ASPxPivotGridExporter1.OptionsPrint.PrintColumnHeaders = checkPrintColumnHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
            ASPxPivotGridExporter1.OptionsPrint.PrintRowHeaders = checkPrintRowHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
            ASPxPivotGridExporter1.OptionsPrint.PrintDataHeaders = checkPrintDataHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;

            string fileName = string.Format("TeritarySummary_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
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
        catch (Exception ex)
        {

        }

    }

    public void BindPivotGrid()
    {
        try
        {
            using (ReportData objReport = new ReportData())
            {
                //objReport.NDID = Convert.ToInt32(ddlNDS.SelectedValue);//#CC01 comented
                objReport.NDID = PageBase.SalesChanelID;//#CC01 added
                objReport.UserId = PageBase.UserId;//#CC01 added
                objReport.ModelId = Convert.ToInt32(ddlModelName.SelectedValue);
                objReport.SkuId = Convert.ToInt32(ddlSku.SelectedValue);
                objReport.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                objReport.DateTo = Convert.ToDateTime(ucDateTo.Date);
                DataSet dsresult = objReport.GetTertiaryReportSummaryDetail();

                if (dsresult.Tables.Count > 0 && dsresult.Tables[0].Rows.Count > 0)
                {
                    hfSearch.Value = "1";
                    pnlSearch.Visible = true;
                    ASPxPvtGrd.DataSource = dsresult;
                    ASPxPvtGrd.DataBind();
                }
                else
                {
                    pnlSearch.Visible = false;
                    ASPxPvtGrd.DataSource = null;
                    ASPxPvtGrd.DataBind();
                    ucMsg.ShowInfo("No record found.");
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void clearfields()
    {
        ucMsg.Visible = false;
        pnlSearch.Visible = false;
        ddlNDS.SelectedIndex = 0;
        ddlModelName.SelectedIndex = 0;
        ddlModelName.SelectedIndex = 0;
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
        ddlModelName_SelectedIndexChanged(null, null);

    }
}
