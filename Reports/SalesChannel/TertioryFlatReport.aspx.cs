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
/*Change Log:
 * 08-May-2014, Rakesh Goel, #CC01 - Changed export to excel call to EPP method instead of OpenXML.
 * Also added exception display in catch block
 * REPORT NOT IN USE - CREATED SPECIFIC TO BEETEL
 */
public partial class Reports_SalesChannel_TertioryFlatReport : PageBase
{
 
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSearch.Visible = false;
        btncancel.Visible = false;
        ucDateFrom.Visible = false;
        ucDateTo.Visible = false;
        if (!IsPostBack)
        {
            binddata();
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
                dt = obj.GetTeritoryInfo();

                if (dt.Rows.Count > 0)
                {
                    grdTertioryReport.DataSource = dt;
                    grdTertioryReport.DataBind();
                    ViewState["Export"] = dt;
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
    protected void btnSerch_Click(object sender, EventArgs e)
    {
        if (!isvalidate())
        {
            pnlGrid.Visible = false;
            updsearch.Update();
            return;
        }
        binddata();
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        blankall();
        updgrid.Update();
        pnlGrid.Visible = false;

    }
    protected void exportToExel_Click(object sender, EventArgs e)
    {
        if (ViewState["Export"] != null)
        {


            DataTable dt = (DataTable)ViewState["Export"];
            string[] DsCol = new string[] { "SalesChannelCode", "HOName", "RBHName", "ZSMName", "SBHName", "ASOName", "GRNNumber", "GRNDate", "PONumber", "PODate", "InvoiceNumber", "InvoiceDate", "SKUName", "Quantity", "Model", "Color", "Brand", "Product" };
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);

            if (dt.Rows.Count > 0)
            {
                try
                {
                    DataSet dtcopy = new DataSet();
                    dt.Columns["HOName"].ColumnName = Resources.SalesHierarchy.HierarchyName1;
                    dt.Columns["RBHName"].ColumnName = Resources.SalesHierarchy.HierarchyName2;
                    dt.Columns["ZSMName"].ColumnName = Resources.SalesHierarchy.HierarchyName3;
                    dt.Columns["SBHName"].ColumnName = Resources.SalesHierarchy.HierarchyName4;
                    dt.Columns["ASOName"].ColumnName = Resources.SalesHierarchy.HierarchyName5;
                    dtcopy.Merge(dt);
                    dtcopy.Tables[0].AcceptChanges();
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "GRNReport";
                    PageBase.RootFilePath = FilePath;
                    //PageBase.ExportToExeclUsingOPENXMLV2(dtcopy.Tables[0], FilenameToexport);   //#CC01 commented

                    PageBase.ExportToExecl(dtcopy, FilenameToexport);  //#CC01 added

                    ViewState["Export"] = null;
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.Message.ToString());
                }
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);

            }
            ViewState["Export"] = null;
        }
    }
    protected void grdTertioryReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        grdTertioryReport.PageIndex = e.NewPageIndex;
        binddata();

    }

}
