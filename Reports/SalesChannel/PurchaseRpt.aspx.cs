#region NameSpace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
using DevExpress.Web.ASPxPivotGrid;
using System.Configuration;
using DevExpress.XtraPivotGrid;
using DevExpress.Utils;
#endregion

public partial class Reports_SalesChannel_PurchaseRpt : PageBase
{
    
    #region PageLoad Event
    protected void Page_Load(object sender, EventArgs e)
    {
        fncHide();
        if (!IsPostBack)
        {
            //ShowScrollBar(true);
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;
        }
        if (hfSearch.Value =="1")
            BindGrid();
    }
    #endregion

    #region User defined function
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

        string fileName = string.Format("PurchaseReport_{0}",DateTime.Today.ToString("dd_MM_yyyy"));
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

    bool pageValidate()
    {
        //if (ViewState["DsPurchaseInfo"] != null)
        //    ViewState["DsPurchaseInfo"] = null;
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
            DataTable dt;
            //if (ViewState["DsPurchaseInfo"] != null)
            //{
            //    ASPxPvtGrd.DataSource = (DataSet)ViewState["DsPurchaseInfo"];
            //    ASPxPvtGrd.DataBind();
            //    pnlSearch.Visible = true;
            //}
            //else
            //{                
                using (ReportData objRD = new ReportData())
                {
                    objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                    objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                    objRD.SalesChannelID = PageBase.SalesChanelID;
                    objRD.UserId = PageBase.UserId;
                    objRD.BaseEntityTypeID = PageBase.BaseEntityTypeID;
                    DsPurchaseInfo = objRD.GetPurchaseReport();
                    dt = DsPurchaseInfo.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        hfSearch.Value = "1";
                        //ViewState["DsPurchaseInfo"] = DsPurchaseInfo;
                        ASPxPvtGrd.DataSource = dt;
                        ASPxPvtGrd.DataBind();
                        pnlSearch.Visible = true;
                    }
                    else
                    {

                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                        pnlSearch.Visible = false;
                    }
                //}
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

    #endregion

    #region Page Control Events
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //ViewState["DsPurchaseInfo"] = null;
        hfSearch.Value = "0";
        pnlSearch.Visible = false;
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (pageValidate())
        {
            BindGrid();
        }
        else
            pnlSearch.Visible = false;
    }
    #endregion


}
