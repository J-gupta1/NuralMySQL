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
public partial class InterfaceHealthReport : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            ucFromDate.Date = PageBase.Fromdate;
            ucToDate.Date = PageBase.ToDate;

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
       
        SearchAttendanceDetailData(1);
    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        SearchAttendanceDetailData(-1);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
       
        gvPatchHealth.DataSource = null;
        gvPatchHealth.DataBind();
        ucMessage1.Visible = true;
        PnlGrid.Visible = false;
        
    }
    
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        SearchAttendanceDetailData(ucPagingControl1.CurrentPage);
    }
    public void SearchAttendanceDetailData(int pageno)
    {
        
        try
        {
            ViewState["TotalRecords"] = 0;
            ViewState["CurrentPage"] = pageno;
            using (ReportData objPatchHealth = new ReportData())
            {
                
                objPatchHealth.UserId = PageBase.UserId;
                if (!string.IsNullOrEmpty( rdbLastDays.SelectedValue))
                objPatchHealth.ActivationFrom = Convert.ToInt16(rdbLastDays.SelectedValue);
                else
                {
                    if (ucFromDate.Date == "" && ucToDate.Date == "")
                    { ;}
                    else
                    {
                        objPatchHealth.FromDate = ucFromDate.Date;
                        objPatchHealth.ToDate = ucToDate.Date;
                    }
                }
                objPatchHealth.PageIndex = pageno;
                objPatchHealth.PageSize = Convert.ToInt32(PageBase.PageSize);

                DataSet ds = objPatchHealth.getPatchHealthReport();
                if (objPatchHealth.TotalRecords > 0)
                {
                    PnlGrid.Visible = true;
                    if (pageno > 0)
                    {
                        gvPatchHealth.DataSource = ds;
                        gvPatchHealth.DataBind();
                        PnlGrid.Visible = true;
                        ViewState["TotalRecords"] = objPatchHealth.TotalRecords;
                        ucPagingControl1.TotalRecords = objPatchHealth.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {

                        string FilenameToexport = "TallyPatchHealth";
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    }
                }
                else
                {
                    ds = null;
                    gvPatchHealth.DataSource = null;
                    gvPatchHealth.DataBind();
                    ucMessage1.Visible = true;
                    PnlGrid.Visible = false;
                    if (!string.IsNullOrEmpty( objPatchHealth.error))
                        ucMessage1.ShowError(objPatchHealth.error);
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