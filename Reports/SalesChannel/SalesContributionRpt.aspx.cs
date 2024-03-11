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
/*
 * 27 July 2016, Karam Chand Sharma, #CC01, Show stock bin type in pivot 
 */
public partial class Reports_SalesChannel_SalesContributionRpt : PageBase
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
            //ShowScrollBar(true);
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;
            SalesType();
        }
    }
    #endregion

    #region User defined function
    private void BindGrid()
    {
        try
        {

            DataSet DsSalesInfo;
            DataTable dt,dt1;
 
                using (ReportData objRD = new ReportData())
                {
                    if (ddlSalesType.SelectedIndex == 0)
                    {
                        ucMsg.ShowInfo("Select Sales Type");
                        return;
                    }
                    //EnumData.eReportType ReportType = (EnumData.eReportType)Enum.Parse(typeof(EnumData.eReportType), ddlSalesType.SelectedValue);
                    //Int16 value = (Int16)ReportType;
                    objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                    objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                    objRD.SalesChannelID = PageBase.SalesChanelID;
                    objRD.UserId = PageBase.UserId;
                    objRD.SalesType = Convert.ToInt16(ddlSalesType.SelectedValue);
                    DsSalesInfo = objRD.GetSalesContributionReport();
                    dt = DsSalesInfo.Tables[0];
                    //dt1=objRD.headerReplacement(dt);
                    if (dt.Rows.Count > 0)
                    {
                        hfSearch.Value = "1";
                        ASPxPvtGrd.DataSource = dt;
                        ASPxPvtGrd.DataBind();
                        pnlSearch.Visible = true;
                    }
                    //if (DsSalesInfo.Tables[0].Rows.Count > 0)
                    //{
                    //    hfSearch.Value = "1";
                    //    ASPxPvtGrd.DataSource = DsSalesInfo;
                    //    ASPxPvtGrd.DataBind();
                    //    pnlSearch.Visible = true;
                    //}
                    else
                    {
                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                        pnlSearch.Visible = false;

                    }
                    if (ddlSalesType.SelectedValue == "3")
                        ASPxPvtGrd.Fields["PvtRetailerType"].Visible = true;
                    else
                        ASPxPvtGrd.Fields["PvtRetailerType"].Visible = false;

                //}
             }
           
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    //protected void ShowScrollBar(bool show)
    //{
    //    if (show)
    //    {

    //        ASPxPvtGrd.Width = Unit.Parse("950px");
    //        ASPxPvtGrd.OptionsView.ShowHorizontalScrollBar = true;
    //    }
    //    else
    //    {
    //        ASPxPvtGrd.Width = Unit.Parse("100%");
    //        ASPxPvtGrd.OptionsView.ShowHorizontalScrollBar = false;
    //    }

    //}
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
            DataTable dt = new DataTable();
            ddlSalesType.Items.Clear();
            using (ReportData objSalesType = new ReportData())
            {
                objSalesType.SalesChannelTypeID = PageBase.SalesChanelTypeID;
                objSalesType.HierarchyLevelId = PageBase.HierarchyLevelID;
                dt = objSalesType.GetSalesTypeforReport();
            };
            String[] colArray = { "SalesTypeID", "SalesTypeName" };
            PageBase.DropdownBinding(ref ddlSalesType, dt, colArray);

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
        //if (ViewState["DsSalesInfo"] != null)
        //    ViewState["DsSalesInfo"] = null;
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
        //ViewState["DsSalesInfo"] = null;
        hfSearch.Value = "0";
        pnlSearch.Visible = false;
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;


    }
}
