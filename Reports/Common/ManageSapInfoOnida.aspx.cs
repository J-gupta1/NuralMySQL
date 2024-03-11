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


public partial class Reports_Common_ManageSapInfoOnida : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindModule();

        }
    }
    bool isvalidate()
    {
        if (ucDateFrom.Date != "" && ucDateTo.Date == "")
        {

            ucMessage1.ShowInfo("Please select from date");
            return false;
        }
        if (ucDateFrom.Date == "" && ucDateTo.Date != "")
        {
            ucMessage1.ShowInfo("Please select to date");
            return false;
        }
        if (Convert.ToDateTime(ucDateFrom.Date) > Convert.ToDateTime(ucDateTo.Date))
        {
            ucMessage1.ShowInfo("The date from cant exceed the to  date");
            return false;
        }

        return true;
    }
    void blankall()
    {
        ucDateFrom.Date = "";
        ucDateTo.Date = "";
        DataTable dtblank = new DataTable();
        dtblank = new DataTable();
        dtblank = null;
        ddlModule.SelectedIndex = 0;
        gvSapInfo.DataSource = dtblank;
        gvSapInfo.DataBind();
    }
    void binddata()
    {
        try
        {
            ucMessage1.Visible = false;
            DataTable dt;
            using (ReportData obj = new ReportData())
            {
                obj.FromDate = ucDateFrom.Date;
                obj.ToDate = ucDateTo.Date;

                obj.SalesChannelId = PageBase.SalesChanelID;
                obj.FromDate = ucDateFrom.Date;
                obj.ToDate = ucDateTo.Date;
                obj.ModuleID = Convert.ToInt32(ddlModule.SelectedValue);

                dt = obj.GetSapInfo();

                if (dt.Rows.Count > 0)
                {

                    gvSapInfo.DataSource = dt;
                    gvSapInfo.DataBind();
                    //ViewState["Export"] = dt;
                    pnlGrid.Visible = true;
                    updgrid.Update();

                }
                else
                {
                    dt = new DataTable();
                    dt = null;
                    gvSapInfo.DataSource = dt;
                    gvSapInfo.DataBind();
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                    pnlGrid.Visible = false;
                    updgrid.Update();
                }

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }


    }
    void BindModule()
    {
        using (ReportData obj = new ReportData())
        {
            DataTable dt = new DataTable();
            dt = obj.GetModuleNameInfoForSap();
            if (dt.Rows.Count > 0)
            {
                String[] colArray = { "SapServiceListID", "ModuleName" };
                PageBase.DropdownBinding(ref ddlModule, dt, colArray);
                updgrid.Update();

            }
            else
            {
                dt = new DataTable();
                dt = null;
                gvSapInfo.DataSource = dt;
                gvSapInfo.DataBind();
                ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                pnlGrid.Visible = false;
                updgrid.Update();
            }

        }
    }

    protected void btnSerch_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlModule.SelectedValue) == 0)
        {
            ucMessage1.ShowInfo("Please select Module Name");
            return;
        }
        if (!isvalidate())
        {
            pnlGrid.Visible = false;
            
            return;
        }
        dvgrdDetail.Visible = false;
        gvSapInfoDetail.DataSource = null;
        gvSapInfoDetail.DataBind();
        updgrid.Update();
        binddata();
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        ViewState["ServiceDoc"] = null;
        blankall();
        pnlGrid.Visible = false;
        updgrid.Update();
        ViewState["ServiceDoc"] = null;
    }
    protected void gvSapInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSapInfo.PageIndex = e.NewPageIndex;
        binddata();
    }
    protected void gvSapInfo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ViewState["ServiceDoc"] = null;
       
            try
            {
                using (POC objDetailReport = new POC())
                {
                   GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    ucMessage1.Visible = false;
                    ViewState["ServiceDoc"] = Convert.ToString(gvSapInfo.DataKeys[row.RowIndex].Values["ErrorDocNo"]);
                    objDetailReport.GenServiceDocNo = Convert.ToString(gvSapInfo.DataKeys[row.RowIndex].Values["ErrorDocNo"]);
                    
                    DataTable dt = new DataTable();
                    dt = objDetailReport.GetDetailwiseDataForSap(Convert.ToInt32(ddlModule.SelectedValue));
                    if (dt.Rows.Count > 0)
                    {
                        exporttoexel.Visible = true;
                        gvSapInfoDetail.DataSource = dt;
                        gvSapInfoDetail.DataBind();
                        updgrid.Update();
                        dvDetail.Visible = true;
                        dvgrdDetail.Visible = true;
                    }
                    else
                    {
                        ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                        updgrid.Update();
                    }

                }
            }
            catch (Exception ex)
            {
            }
        
    }
    //protected void gvSapInfoDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    gvSapInfo.PageIndex = e.NewPageIndex;
    //    bindDetaildata();
    //}
    protected void gvSapInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblServiceDocno = (Label)e.Row.FindControl("lblserviceDoc");
            Button txtdetail = (Button)e.Row.FindControl("btnDetail");
            if (lblServiceDocno.Text == "0")
            {
                txtdetail.Visible = false;
            }
        }
    }
    protected void gvSapInfoDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSapInfoDetail.PageIndex = e.NewPageIndex;
        bindDetailGridData(0);
    }
    protected void exporttoexel_Click(object sender, EventArgs e)
    {
        try{
            bindDetailGridData(1);
        }
        catch(Exception ex)
        {

        }
    }
    private void bindDetailGridData(int value)
    {
        using (POC objDetailReport = new POC())
        {
            ucMessage1.Visible = false;
            if (ViewState["ServiceDoc"] != null)
                objDetailReport.GenServiceDocNo = ViewState["ServiceDoc"].ToString();

            DataTable dt = new DataTable();
            dt = objDetailReport.GetDetailwiseDataForSap(Convert.ToInt32(ddlModule.SelectedValue));
            if (dt.Rows.Count > 0)
            {
                exporttoexel.Visible = true;
                if (value == 0)     //For filling the grid
                {
                    gvSapInfoDetail.DataSource = dt;
                    gvSapInfoDetail.DataBind();
                    updgrid.Update();
                    dvDetail.Visible = true;
                    dvgrdDetail.Visible = true;
                }
                else   //For export to excel
                {
                    dt.Columns.Remove("RecordID");
                    DataSet dtcopy = new DataSet();
                    dtcopy.Merge(dt);
                    dtcopy.Tables[0].AcceptChanges();
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ErrorDetails";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);
                }
                
            }
            else
            {
                ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                updgrid.Update();
            }

        }
    }
}
