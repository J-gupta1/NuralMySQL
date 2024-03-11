using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using BussinessLogic;
using ZedService.Utility;

public partial class Reports_TargetAndStockDashboard : PageBase//System.Web.UI.Page
{
    DataTable dt;
    DataSet Ds;
    protected void Page_Load(object sender, EventArgs e)
    {
        Server.ScriptTimeout = 900;
        ucMsg.Visible = false;
        if (!IsPostBack)
        {
            try
            {
                MonthAndYear();
                ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
                ddlYear.SelectedValue = DateTime.Now.Year.ToString();
                ddlTargetType.SelectedValue = "1";//1=Target on Quantity,2=Target on Value(amount)
                ddlDashBoardType.SelectedValue = "3";//1=Target,2=Stock,3=Both
                btnSearch_Click(null, null);
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
   
    void MonthAndYear()
    {
        try
        {
            DataSet dsMonthYr = new DataSet();
            ddlMonth.Items.Clear();
            using (ReportData objRepData = new ReportData())
            {
                objRepData.UserId = PageBase.UserId;

                dsMonthYr = objRepData.getMonthAndYear();
            };
            if (dsMonthYr.Tables.Count > 1)
            {
                String[] colArrayYr = { "YearNumber", "YearNumber" };
                PageBase.DropdownBinding(ref ddlYear, dsMonthYr.Tables[0], colArrayYr);

                String[] colArrayMonth = { "MonthNumber", "month_Name" };
                PageBase.DropdownBinding(ref ddlMonth, dsMonthYr.Tables[1], colArrayMonth);
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if ((ddlDashBoardType.SelectedValue == "1" || ddlDashBoardType.SelectedValue == "3")
                && ddlTargetType.SelectedIndex==0)
            {
                ucMsg.ShowWarning("Please select target type.");
                return;
            }
            using (ReportData obDashboard = new ReportData())
            {
                obDashboard.UserId = PageBase.UserId;
                obDashboard.DashBoardMonth = Convert.ToInt32(ddlMonth.SelectedValue);
                obDashboard.DashBoardYear = Convert.ToInt32(ddlYear.SelectedValue);
                obDashboard.DashBoardTargetType = Convert.ToInt16(ddlTargetType.SelectedValue);
                obDashboard.DashBoardType = Convert.ToInt16(ddlDashBoardType.SelectedValue);
                obDashboard.DashBoardExportType = 0;
                DataSet ds = obDashboard.getTargetStockDashboardReport();
                //DashboardType 0=Export action,1= TargetDashboard,2=Stock Dashboard,3=Both Target and stock Dashboard*/
                if (ddlDashBoardType.SelectedValue == "1" || ddlDashBoardType.SelectedValue == "3")
                {
                    grdTarget.DataSource = ds.Tables[0];
                    grdTarget.DataBind();
                    btnExprtTarget.Visible = true;

                    if (ddlDashBoardType.SelectedValue == "1")
                    {
                        grdStockDashBoard.DataSource = null;
                        grdStockDashBoard.DataBind();
                        btnExportStockDashBoard.Visible = false;
                    }
                }
                if (ddlDashBoardType.SelectedValue == "2" )
                {
                    grdStockDashBoard.DataSource = ds.Tables[0];
                    grdStockDashBoard.DataBind();
                    btnExportStockDashBoard.Visible = true;

                    grdTarget.DataSource = null;
                    grdTarget.DataBind();
                    btnExprtTarget.Visible = false ;
                }
                else if (ddlDashBoardType.SelectedValue == "3")
                {
                    grdStockDashBoard.DataSource = ds.Tables[1];
                    grdStockDashBoard.DataBind();
                    btnExportStockDashBoard.Visible = true;
                    btnExprtTarget.Visible = true;
                }
            }
        }
        catch(Exception ex)
        {
            ucMsg.ShowError(ex.Message);
        }
    }
    protected void btnExprtTarget_Click(object sender, EventArgs e)
    {
        if (ddlMonth.SelectedIndex == 0 || ddlYear.SelectedIndex == 0 || ddlTargetType.SelectedIndex==0 )
        {
            ucMsg.ShowWarning("Please select month, year and target type.");
            return;
        }
        try
        {
            using (ReportData obDashboard = new ReportData())
            {
                obDashboard.UserId = PageBase.UserId;
                obDashboard.DashBoardMonth = Convert.ToInt32(ddlMonth.SelectedValue);
                obDashboard.DashBoardYear = Convert.ToInt32(ddlYear.SelectedValue);
                obDashboard.DashBoardTargetType = Convert.ToInt16(ddlTargetType.SelectedValue);
                obDashboard.DashBoardType = 0;
                obDashboard.DashBoardExportType = 1;
                DataSet ds = obDashboard.getTargetStockDashboardReport();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string FilenameToexport = "BMTargetDashBoard";
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(ds, FilenameToexport);
                }
                else
                {
                    ucMsg.ShowInfo("No record found.");
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
        }
    }
    protected void btnExportStockDashBoard_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlMonth.SelectedIndex == 0 || ddlYear.SelectedIndex == 0 )
            {
                ucMsg.ShowWarning("Please select month and year both.");
                return;
            }
            using (ReportData obDashboard = new ReportData())
            {
                obDashboard.UserId = PageBase.UserId;
                obDashboard.DashBoardMonth = Convert.ToInt32(ddlMonth.SelectedValue);
                obDashboard.DashBoardYear = Convert.ToInt32(ddlYear.SelectedValue);
                obDashboard.DashBoardTargetType = Convert.ToInt16(ddlTargetType.SelectedValue);
                obDashboard.DashBoardType = 0;
                obDashboard.DashBoardExportType = 2;
                DataSet ds = obDashboard.getTargetStockDashboardReport();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string FilenameToexport = "BMStockDashBoard";
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(ds, FilenameToexport);
                }
                else
                {
                    ucMsg.ShowInfo("No record found.");
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
        }
    }
    protected void lnkExportStockDetail_Click(object sender, EventArgs e)
    {
        try
        {
            if(ddlMonth.SelectedIndex==0 || ddlYear.SelectedIndex==0)
            {
                ucMsg.ShowWarning("Please select month and year both.");
                return;
            }
            using (ReportData obDashboard = new ReportData())
            {
                obDashboard.UserId = PageBase.UserId;
                obDashboard.DashBoardMonth = Convert.ToInt32(ddlMonth.SelectedValue);
                obDashboard.DashBoardYear = Convert.ToInt32(ddlYear.SelectedValue);
                obDashboard.DashBoardTargetType = Convert.ToInt16(ddlTargetType.SelectedValue);
                obDashboard.DashBoardType = 0;
                obDashboard.DashBoardExportType = 3;
                DataSet ds = obDashboard.getTargetStockDashboardReport();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string FilenameToexport = "BMStockDetail";
                    ZedService.Utility.ZedServiceUtil.ExportToExecl(ds, FilenameToexport);
                }
                else
                {
                    ucMsg.ShowInfo("No record found.");
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
        }
    }
    protected void grdStockDashBoard_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdStockDashBoard.PageIndex = e.NewPageIndex;
        btnSearch_Click(null,null);
    }
    protected void grdTarget_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdTarget.PageIndex = e.NewPageIndex;
        btnSearch_Click(null, null);
    }
}