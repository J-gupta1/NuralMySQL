using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using System.Data;

public partial class ClientServices_Common_ReportQueue : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindList(0);
        }
    }
  public  void BindList(int Index)
    {
        try
        {
            using(clsReportQueue objReport = new clsReportQueue())
            {
                Index = Index == 0 ? 1 : Index;
                objReport.PageSize = Convert.ToInt32(PageBase.PageSize);
                objReport.PageIndex = Index;
                objReport.UserId = UserId;
                DataTable dt = objReport.SelectAll();
                grdvList.DataSource = dt;
                grdvList.DataBind();
                grdvList.Visible = true;
                divgrd.Visible = true;

                if (dt == null || dt.Rows.Count == 0)
                {
                    ucPagingControl1.Visible = false;
                }
                else
                {
                    ucPagingControl1.Visible = true;
                    ucPagingControl1.PageSize = Convert.ToInt32(PageBase.PageSize);
                    ucPagingControl1.TotalRecords = objReport.TotalRecords;
                    ucPagingControl1.FillPageInfo();
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
  protected void LnkExporttoExcel_Click(object sender, EventArgs e)
    {
        try
        {
            clsReportQueue obj = new clsReportQueue();
            obj.UserId = UserId;
            obj.PageIndex = -1;
            obj.PageSize = 1000;
            DataTable dt = obj.SelectAll();
            if (dt.Rows.Count > 0)
            {
                DataSet ds = new DataSet();
                ds.Merge(dt);
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "ReportQueueData";
                PageBase.RootFilePath = FilePath;
                ZedService.Utility.ZedServiceUtil.ExportToExecl(ds, FilenameToexport);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
    protected void grdvList_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToInt16(grdvList.DataKeys[e.Row.RowIndex][1]) == 1)
            {
                LinkButton lnkdownload = (LinkButton)e.Row.FindControl("lnkbtnDownload");
                lnkdownload.Visible = true;
            }
            else
            {
                e.Row.Cells[6].Text = "";
            }

        }
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        using (clsReportQueue objCountry = new clsReportQueue())
        {
            int intPageNumber = ucPagingControl1.CurrentPage;
            ViewState["PageIndex"] = intPageNumber;
            objCountry.PageIndex = intPageNumber;
            BindList(ucPagingControl1.CurrentPage);
        }
    }
    protected void grdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Download")
            {
                string path = Server.MapPath("~/UploadDownload/UploadPersistent/RequestQueue/" + e.CommandArgument);
                string type = "";
                type = "Application/excel";
                Response.AppendHeader("content-disposition", "attachment; filename=" + e.CommandArgument);
                if ((type != ""))
                {
                    Response.ContentType = type;
                    Response.WriteFile(path);
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            
        }
    }
    protected void lnkbtnDownload_Click(object sender, EventArgs e)
    {

    }
}