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
/*
 * 27 July 2016, Karam Chand Sharma, #CC01, Show stock bin type in pivot
 */
public partial class Reports_SalesChannel_OPSIRpt :PageBase
{
    
    
    #region PageLoad Event
    protected void Page_Load(object sender, EventArgs e)
    {
        fncHide();
        if ( hfSearch.Value == "1")
            BindGrid();
        if (!IsPostBack)
        {
            hdnReportSalesChannelCode.Text = "0";
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;
            FillsalesChannelType();
            if (PageBase.SalesChanelID == 0)
            {
                dvSalesChannel.Attributes.Add("style", "display:block");
                reqSales.ValidationGroup = "SalesReport";
            }
            else
            {
                dvSalesChannel.Attributes.Add("style", "display:none");
                reqSales.ValidationGroup = "dummy";
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

        //if (hdnReportSalesChannelCode.Text == "0" & PageBase.SalesChanelID==0)
        //{
        //    ucMsg.ShowInfo("Please Select any SalesChannelCode for Report");
        //    return false;
        //}
        if (ddlType.SelectedValue == "0" & PageBase.SalesChanelID == 0)
        {
            ucMsg.ShowInfo("Please Select any SalesChannelType for Report");
            return false;
        }
        return true;
        
    }
    private void BindGrid()
    {
        try
        {
            DataSet DsPurchaseInfo=new DataSet();
            DataTable dt;
       
                using (ReportData objRD = new ReportData())
                {
                    objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                    objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                    objRD.SalesChannelID = PageBase.SalesChanelID;
                    objRD.SalesChannelTypeID = PageBase.SalesChanelCode == string.Empty ? Convert.ToInt32(ddlType.SelectedValue) : PageBase.SalesChanelTypeID;
                    objRD.UserId = PageBase.UserId;
                    //objRD.SalesChannelCode = PageBase.SalesChanelCode==string.Empty ? Convert.ToString(hdnReportSalesChannelCode.Text) : PageBase.SalesChanelCode;

                    objRD.SalesChannelCode = txtReportFor.Text.Trim();
                    DsPurchaseInfo = objRD.GetOPSIReport();
                    dt = objRD.headerReplacement(DsPurchaseInfo.Tables[0]);
                    if (!string.IsNullOrEmpty(objRD.error))
                    {
                        ucMsg.ShowError(objRD.error);
                        return;
                    }
                    if (DsPurchaseInfo.Tables[0].Rows.Count > 0)
                    {
                        hfSearch.Value = "1";
                        ASPxPvtGrd.DataSource = DsPurchaseInfo.Tables[0];
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

        string fileName = string.Format("OPSIReport_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
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
        ddlType.SelectedValue = "0";
        txtReportFor.Text = string.Empty;
        hdnReportSalesChannelCode.Text = "0";

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (pageValidate())
            BindGrid();
          else
            pnlSearch.Visible = false;
    }
    #endregion
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ucMsg.Visible = false;
        //AutoCompleteExtender1.ContextKey = ddlType.SelectedValue;
        hdnReportSalesChannelCode.Text = "0";
    }
    void FillsalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {

            ddlType.Items.Clear();
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16( PageBase.SalesChanelTypeID);
            ObjSalesChannel.GetRetailerType = 0;
            ObjSalesChannel.UserID = PageBase.UserId;
            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref ddlType, ObjSalesChannel.GetSalesChannelTypeForReport(), str);
            if (PageBase.SalesChanelID != 0)
            {
                ddlType.SelectedValue = PageBase.SalesChanelTypeID.ToString();
                //if (PageBase.SalesChanelTypeID != 5 && PageBase.IsRetailerStockTrack == 1)
                //    ddlType.Items.Add(new ListItem("Retailer", "101"));
            }
            //else if (PageBase.SalesChanelID == 0 & PageBase.OtherEntityType == 1)
            //{
            //    ddlType.Items.Clear();
            //    ddlType.Items.Insert(0, new ListItem("Retailer", "101"));
            //    ddlType.Enabled = false;
            //}
            //else if (PageBase.IsRetailerStockTrack == 1)
            //{
            //    ddlType.Items.Add(new ListItem("Retailer", "101"));
            //    ddlType.Enabled = true;
            //}
        };
    }
}
