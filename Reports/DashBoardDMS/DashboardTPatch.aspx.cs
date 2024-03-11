using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;
using System.IO;

//======================================================================================
//* Developed By : Balram Jha
//* Module       : Reports(Interface health)  
//* Description  :  This page is used for View interface sale(tally patch health report) 
//* ====================================================================================
/* Change Log
 * -------------------------------------------------------------------------------------
 * DD-MMM-YYYY, Name, #CCXX, Description
 
 
 */
public partial class DashboardTPatch : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            ucFromDate.Date = PageBase.Fromdate;
            ucToDate.Date = PageBase.ToDate;
            bindRegion();

        }
    }
    void bindRegion()
    {


        using (MastersData objRegion = new MastersData())
        {
            DataTable dt = objRegion.SelectRegionList();
            ddlRegion.Items.Clear();

            String[] colArray1 = { "RegionID", "RegionName" };

            PageBase.DropdownBinding(ref ddlRegion, dt, colArray1);
            
        }

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ucFromDate.Date != "" && ucToDate.Date == "")
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowWarning("Please Enter  To Date.");
            return;
        }
        if (ucToDate.Date != "" && ucFromDate.Date == "")
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowWarning("Please Enter  From Date.");
            return;
        }
        if (ucFromDate.Date=="" && rdbLastDays.SelectedIndex==-1)
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowWarning("Please Enter date range or select last days.");
            return;
        }
        ucMessage1.Visible = false;
       
        SearchDashboardData(1);
    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        SearchDashboardData(-1);
    }
    
    
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        SearchDashboardData(ucPagingControl1.CurrentPage);
    }
    public void SearchDashboardData(int pageno)
    {
        
        try
        {
            ViewState["TotalRecords"] = 0;
            ViewState["CurrentPage"] = pageno;
            using (ReportData objDashboard = new ReportData())
            {
                
                objDashboard.UserId = PageBase.UserId;
                if (!string.IsNullOrEmpty( rdbLastDays.SelectedValue))
                objDashboard.ActivationFrom = Convert.ToInt16(rdbLastDays.SelectedValue);
                else
                {
                    if (ucFromDate.Date == "" && ucToDate.Date == "")
                    { ;}
                    else
                    {
                        objDashboard.FromDate = ucFromDate.Date;
                        objDashboard.ToDate = ucToDate.Date;
                    }
                }
                objDashboard.PageIndex = pageno;
                objDashboard.PageSize = Convert.ToInt32(PageBase.PageSize);
                objDashboard.SalesChannelCode = txtSChannelCode.Text.Trim();
                objDashboard.SalesChannelName = txtSChannelName.Text.Trim();
                objDashboard.RegionId = Convert.ToInt16( ddlRegion.SelectedValue);

                DataSet ds = objDashboard.getDashBoardTallyPatch();
                if (objDashboard.TotalRecords > 0)
                {
                    PnlGrid.Visible = true;
                    if (pageno > 0)
                    {
                        gvDashboard.DataSource = ds;
                        gvDashboard.DataBind();
                        PnlGrid.Visible = true;
                        ViewState["TotalRecords"] = objDashboard.TotalRecords;
                        ucPagingControl1.TotalRecords = objDashboard.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {

                        string FilenameToexport = "DashBoard";
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    }
                }
                else
                {
                    ds = null;
                    gvDashboard.DataSource = null;
                    gvDashboard.DataBind();
                    ucMessage1.Visible = true;
                    PnlGrid.Visible = false;
                    if (!string.IsNullOrEmpty( objDashboard.error))
                        ucMessage1.ShowError(objDashboard.error);
                    else
                    ucMessage1.ShowInfo("No Record Found.");

                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    
    
}