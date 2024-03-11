#region Copyright and page info
/*===================================================================
Copyright	: Zed-Axis Technologies, 2022
Author		: Adnan Mubeen
Create date	: 23-Nov-2022
Description	: To Export Target Report
=====================================================================
Review Log:

-----------------------------------------------------------------------------------------------------------------
Change Log:

-----------------------------------------------------------------------------------------------------------------
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess;
using BussinessLogic;

public partial class Reports_Common_TargetReport : PageBase
{
    DataSet DsReportData = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        EnableViewState = false;
        if (!IsPostBack)
        {
            try
            {
                ucDateFrom.Date = PageBase.Fromdate;
                string LastDayOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();
                //ucDateTo.Date = PageBase.ToDate;
                ucDateTo.Date = DateTime.Now.Month.ToString() + "/" + LastDayOfMonth + "/" + DateTime.Now.Year.ToString();
                fillTSI();
                //FillGrid(1);
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        ucDateFrom.Date = PageBase.Fromdate;
        string LastDayOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();
        ucDateTo.Date = DateTime.Now.Month.ToString() + "/" + LastDayOfMonth + "/" + DateTime.Now.Year.ToString();
        fillTSI();
        //FillGrid(1);
    }
    public void fillTSI()
    {
        try
        {
            using (TargetData objTarget = new TargetData())
            {
                objTarget.UserId = PageBase.UserId;
                objTarget.CompanyId = PageBase.ClientId;
                String[] colArray = { "UserID", "TSIName" };
                DataTable dt = objTarget.GetTSIList();

                PageBase.DropdownBinding(ref ddlTSIList, dt, colArray);
            }
        }
        catch (Exception ex)
        {
            // uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

    private void fncHide()
    {
        ucMsg.ShowControl = false;
    }

    private DataSet FillGrid(int PageIndex)
    {
        try
        {
            using (ReportData objReportData = new ReportData())
            {
                objReportData.UserId = PageBase.UserId;
                objReportData.EntityTypeUserId = Convert.ToInt32(ddlTSIList.SelectedValue);
                objReportData.ProductCategtoryid = 0;
                objReportData.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                objReportData.DateTo = Convert.ToDateTime(ucDateTo.Date);
                objReportData.PageIndex = PageIndex;
                objReportData.PageSize = Convert.ToInt32(PageBase.PageSize);
                objReportData.CompanyId = PageBase.ClientId;

                DsReportData = objReportData.GetTargetReport();
                PnlGrid.Visible = true;
                if (DsReportData != null && DsReportData.Tables[0].Rows.Count > 0)
                {
                    GridDetails.DataSource = DsReportData.Tables[0];
                    GridDetails.DataBind();
                    ViewState["TotalRecords"] = objReportData.TotalRecords;
                    ucPagingControl1.TotalRecords = objReportData.TotalRecords;
                    ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                    ucPagingControl1.SetCurrentPage = PageIndex;
                    ucPagingControl1.FillPageInfo();
                }
                else
                {
                    GridDetails.DataSource = null;
                    GridDetails.DataBind();
                    ViewState["TotalRecords"] = 0;
                    ucPagingControl1.TotalRecords = 0;
                    ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                    ucPagingControl1.SetCurrentPage = PageIndex;
                    ucPagingControl1.FillPageInfo();
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
        return DsReportData;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        hfSearch.Value = "0";
        PnlGrid.Visible = false;
        ucDateFrom.Date = PageBase.Fromdate;
        string LastDayOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();
        //ucDateTo.Date = PageBase.ToDate;
        ucDateTo.Date = DateTime.Now.Month.ToString() + "/" + LastDayOfMonth + "/" + DateTime.Now.Year.ToString();
        Page_Load(sender, e);
    }

    bool pageValidate()
    {
        hfSearch.Value = "0";
        return true;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string Password = string.Empty;
            DataSet DsReportData = FillGrid(-1);

            if (DsReportData.Tables[0].Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(DsReportData.Tables[0]);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "TargetReport";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
            }
            else
            {
                ucMsg.ShowError(Resources.Messages.NoRecord);
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillGrid(1);
    }

    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        FillGrid(ucPagingControl1.CurrentPage);
    }
}