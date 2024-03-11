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

public partial class Reports_SalesChannel_SalesReportOnidaOFWise : PageBase
{
    #region PageLoad Event
    protected void Page_Load(object sender, EventArgs e)
    {
           fncHide();
      
           
        if (!IsPostBack)
        {
            //ShowScrollBar(true);
          
            SalesType();
        }
        if (ddlSalesType.SelectedValue != "0")
        {
            BindGrid();
        }
        else
        {
            pnlSearch.Visible = false;
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
                if (ddlSalesType.SelectedIndex == 0)
                {
                    ucMsg.ShowInfo("Select Sales Type");
                    return;
                }

                
                objRD.SalesChannelID = PageBase.SalesChanelID;
                objRD.UserId = PageBase.UserId;
                objRD.SalesType = Convert.ToInt16(ddlSalesType.SelectedValue);
                DsSalesInfo = objRD.GetOFWiseSalesReportOnida();
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
                if (ddlSalesType.SelectedValue == "3")
                    ASPxPvtGrd.Fields["PvtRetailerType"].Visible = true;
                else
                    ASPxPvtGrd.Fields["PvtRetailerType"].Visible = false;
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


    private void SalesType()
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
   
    #endregion

    #region Page Control Events

    protected void btnSearch_Click(object sender, EventArgs e)
    {
       

    }
    #endregion
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        hfSearch.Value = "0";
        pnlSearch.Visible = false;
        ddlSalesType.SelectedIndex = 0;
    }
    protected void ddlSalesType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}

