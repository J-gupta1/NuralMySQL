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

public partial class Reports_SalesChannel_ModelRegionMonthWiseReport : System.Web.UI.Page
{
     DataSet DsSalesInfo;
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
            //SalesType();
        }
    }
    #endregion

    #region User defined function
    private void BindGrid()
    {
        try
        {

           
            using (ReportData objReport = new ReportData())
            {
                objReport.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                objReport.DateTo = Convert.ToDateTime(ucDateTo.Date);
                objReport.SalesChannelID = PageBase.SalesChanelID;
                objReport.UserId = PageBase.UserId;
                DsSalesInfo = objReport.GetMonthRegionModelWiseReport();
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

        string fileName = string.Format("ModelRegionMonthWise_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
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
    protected void exportToExel_Click(object sender, EventArgs e)
    {
        BindGrid();    
        DataTable dt = DsSalesInfo.Tables[0].Copy();
        if (dt.Rows.Count > 0)
        {
            try
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "ModelWiseReport";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }
        }
        else
        {
            ucMsg.ShowError(Resources.Messages.NoRecord);
        }
    }
}
