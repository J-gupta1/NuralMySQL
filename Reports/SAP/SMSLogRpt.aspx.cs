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
#endregion


public partial class Reports_SAP_SMSLogRpt : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        if (!IsPostBack)
        {
           
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;
        }
    }
  
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid(1);
    }
    private void BindGrid(Int32 Search)
    {
        try
        {  
        DataSet DsSMSLog;
        using (ReportData objRD = new ReportData())
        {
            if (pageValidate())
            {
                objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                objRD.MobileNumber = txtMobileNo.Text.Trim();
                objRD.IMEINo = txtIMEINo.Text.Trim();
                DsSMSLog = objRD.GetSMSLogReport();
                if (DsSMSLog.Tables[0].Rows.Count > 0)
                {
                    if (Search == 1)
                    {
                        grdSMSReport.DataSource = DsSMSLog;
                        pnlGrid.Visible = true;
                    }
                    else
                        fncExportToExcel(DsSMSLog);
                }
                else
                {
                    pnlGrid.Visible = false;
                    grdSMSReport.DataSource = null;
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                }
                grdSMSReport.DataBind();            
            }
        }
      }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void fncExportToExcel(DataSet DS)
    {
  
        if (DS.Tables[0].Rows.Count > 0)
        {
            DataTable dt = null;
            dt = DS.Tables[0];
            DataSet dtcopy = new DataSet();
            dtcopy.Merge(dt);
            dtcopy.Tables[0].AcceptChanges();
            String FilePath = Server.MapPath("../../");
            string FilenameToexport = "SMSLog";
            PageBase.RootFilePath = FilePath;
            PageBase.ExportToExecl(dtcopy, FilenameToexport);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
        txtIMEINo.Text = "";
        txtMobileNo.Text = "";
    }
    bool pageValidate()
    {

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
    protected void grdSMSReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdSMSReport.PageIndex = e.NewPageIndex;
        BindGrid(1);

    }
    protected void exportToExel_Click(object sender, EventArgs e)
    {
        BindGrid(2);
    }
}
