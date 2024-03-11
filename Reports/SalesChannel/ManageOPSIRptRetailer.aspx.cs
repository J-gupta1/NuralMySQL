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
using DevExpress.Utils;
#endregion

public partial class Reports_SalesChannel_ManageOPSIRptRetailer : PageBase
{
    #region PageLoad Event
    protected void Page_Load(object sender, EventArgs e)
    {
        Server.ScriptTimeout = 600;
        fncHide();
        if (hfSearch.Value == "1")
            BindGrid();
        if (!IsPostBack)
        {
            FillType();
            ucMsg.Visible = false;
            AutoCompleteExtender1.ContextKey = ddlType.SelectedValue;
            hdnReportRetailerCode.Text = "0";
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;
            if (PageBase.BaseEntityTypeID == 0)
            {
                dvRetailer.Attributes.Add("style", "display:block");
            }
            else
            {
                dvRetailer.Attributes.Add("style", "display:none");
         
            }
         
        }


    }
    #endregion

    #region User defined function
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
    private void BindGrid()
    {
        try
        {
            DataSet DsPurchaseInfo;

            using (ReportData objRD = new ReportData())
            {
                objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                objRD.SalesChannelID = PageBase.SalesChanelID;
                objRD.SalesChannelTypeID = (PageBase.SalesChanelCode == string.Empty & PageBase.BaseEntityTypeID == 3) ? Convert.ToInt32(ddlType.SelectedValue) : PageBase.SalesChanelTypeID;
                //objRD.SalesChannelTypeID = PageBase.SalesChanelTypeID;
                objRD.UserId = PageBase.UserId;
                objRD.SalesChannelCode = Convert.ToString(hdnReportRetailerCode.Text);
                objRD.BaseEntityTypeID = PageBase.BaseEntityTypeID;
                DsPurchaseInfo = objRD.GetOPSIReportRetailer();

                if (DsPurchaseInfo.Tables[0].Rows.Count > 0)
                {
                    hfSearch.Value = "1";
                    ASPxPvtGrd.DataSource = /* DsPurchaseInfo;*/ objRD.headerReplacement(DsPurchaseInfo.Tables[0]);
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

        string fileName = string.Format("RetailerOPSIReport_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
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


    #endregion

    #region Page Control Events

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        hfSearch.Value = "0";
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
        ucMsg.ShowControl = false;
        pnlSearch.Visible = false;
        hdnReportRetailerCode.Text = "0";

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (pageValidate())
            BindGrid();
        else
            pnlSearch.Visible = false;
    }

    void FillType()
    {
        ddlType.Items.Clear();
        ddlType.Items.Add(new ListItem("Retailer", "101"));
    }
    #endregion
}
