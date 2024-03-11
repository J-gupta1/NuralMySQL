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

public partial class Reports_SalesChannel_ModelWiseFlatReport : PageBase
{
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        if (!IsPostBack)
        {
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;

        }

    }
    bool isvalidate()
    {
        if ((ucDateFrom.Date == "" && ucDateTo.Date == ""))
        {
            ucMessage1.ShowInfo("Please select atleast one searching parameter");
            return false;
        }
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
        ucMessage1.Visible = false;
    }
    void binddata()
    {
        try
        {

            DataTable dt;
            using (ReportData obj = new ReportData())
            {

                obj.SalesChannelId = PageBase.SalesChanelID;
                obj.FromDate = ucDateFrom.Date;
                obj.ToDate = ucDateTo.Date;
                dt = obj.GetModelwiseInfo();

                if (dt.Rows.Count > 0)
                {
                    grdTertioryReport.DataSource = dt;
                    grdTertioryReport.DataBind();
                    updgrid.Update();
                    pnlGrid.Visible = true;


                }
                else
                {
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                    pnlGrid.Visible = false;
                }

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }


    }
 
 
    protected void exportToExel_Click(object sender, EventArgs e)
    {
        try
        {
            dt = new DataTable();

            using (ReportData obj = new ReportData())
            {

                obj.SalesChannelId = PageBase.SalesChanelID;
                obj.FromDate = ucDateFrom.Date;
                obj.ToDate = ucDateTo.Date;
                dt = obj.GetModelwiseInfo();

            }
            if (dt.Rows.Count > 0)
            {

                DataSet dtcopy = new DataSet();

                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "ModelWiseReport";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExeclUsingOPENXMLV2(dtcopy.Tables[0], FilenameToexport);



            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);

            }
        }

        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    protected void grdTertioryReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        grdTertioryReport.PageIndex = e.NewPageIndex;
        binddata();

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!isvalidate())
        {
            pnlGrid.Visible = false;
            return;
        }
        binddata();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        blankall();
        updgrid.Update();
        pnlGrid.Visible = false;
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;

    }
}
