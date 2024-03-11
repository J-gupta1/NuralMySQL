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

public partial class Reports_Common_ManageSapInfo : PageBase
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
        if (!isvalidate())
        {
            pnlGrid.Visible = false;
            return;
        }
        binddata();
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        
        blankall();
        pnlGrid.Visible = false;
        updgrid.Update();
    }
    protected void gvSapInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSapInfo.PageIndex = e.NewPageIndex;
        binddata();
    }
}
