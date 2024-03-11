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

public partial class Reports_SalesChannel_SalesRptRetailer : PageBase
{
    #region PageLoad Event
    protected void Page_Load(object sender, EventArgs e)
    {
        fncHide();
        if (hfSearch.Value == "1")
            BindGrid();
        if (!IsPostBack)
        {
            //ShowScrollBar(true);
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;
            SalesType();
            BindModel();
            ddlSku.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    #endregion

    #region User defined function
    private void BindGrid()
    {
        try
        {

            DataSet DsSalesInfo;
            using (ReportData objRD = new ReportData())
            {

                objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                objRD.SalesChannelID = PageBase.SalesChanelID;
                objRD.UserId = PageBase.UserId;
                objRD.SalesType = Convert.ToInt16(ddlSalesType.SelectedValue);
                objRD.BaseEntityTypeID = PageBase.BaseEntityTypeID;
                objRD.ModelId = Convert.ToInt32(ddlModelName.SelectedValue);
                objRD.SkuId = Convert.ToInt32(ddlSku.SelectedValue);
                DsSalesInfo = objRD.GetSalesReportRetailer();

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

        string fileName = string.Format("SalesReport_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
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


    void SalesType()
    {
        try
        {
            if (PageBase.SalesChanelID == 0 & PageBase.BaseEntityTypeID == 3)
            {
                ddlSalesType.Items.Clear();
                ddlSalesType.Items.Insert(0, new ListItem("TertiarySales", "101"));
            }

            else
            {
                ddlSalesType.Items.Add(new ListItem("TertiarySales", "101"));

            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
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
        ddlSalesType.SelectedIndex = 0;
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
        ddlModelName.SelectedValue = "0";
        ddlSku.Items.Clear();
        ddlSku.ClearSelection();
        ddlSku.Items.Insert(0, new ListItem("Select", "0"));
       


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
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
}
