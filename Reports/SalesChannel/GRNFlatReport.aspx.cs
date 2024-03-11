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
 * 18-Feb-2016, Sumit Maurya, #CC02, Search button and search grid (grdGRNReport) visibility sets false as it is not required. Instead of search Export To Excel is used. While Export to excel new method used to fetch data in CSV file.
 * 
 */
public partial class Reports_SalesChannel_GRNFlatReport : PageBase
{
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        /* #CC02 Add Start */
        pnlGrid.Visible = false;
        /* #CC02 Add End */
        ucMessage1.ShowControl = false;
        if (!IsPostBack)
        {
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;
        }
    }


    bool isvalidate()
    {
        if (txtGRNNumber.Text == "" && (ucDateFrom.Date == "" && ucDateTo.Date == ""))
        {
            ucMessage1.ShowInfo("Please select atleast one searching parameter");
            return false;
        }

        if (txtGRNNumber.Text == "")
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
        }
        return true;
    }

    void blankall()
    {
        txtGRNNumber.Text = "";
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
        ucMessage1.Visible = false;
    }

    void binddata()
    {
        try
        {

            //DataTable dt;
            using (ReportData obj = new ReportData())
            {

                if (PageBase.SalesChanelID != 0)
                {
                    obj.SalesChannelId = PageBase.SalesChanelID;
                    obj.SalesChannelID = PageBase.SalesChanelID;
                }
                obj.GRNNumber = txtGRNNumber.Text.Trim();
                obj.FromDate = ucDateFrom.Date;
                obj.ToDate = ucDateTo.Date;
                obj.ReportType = 1;
                if (chksb.Checked)
                    dt = obj.GetGRNInfoSB();
                else
                    dt = obj.GetGRNInfo();

                if (!dt.Columns.Contains("SerialNumber1"))
                    dt.Columns.Add("SerialNumber1");
                if (!dt.Columns.Contains("BatchCode"))
                    dt.Columns.Add("BatchCode");

                if (dt.Rows.Count > 0)
                {

                    grdGRNReport.DataSource = dt;
                    grdGRNReport.DataBind();
                    //ViewState["Export"] = dt;
                    updgrid.Update();
                    pnlGrid.Visible = true;
                    if (chksb.Checked)
                    {

                        grdGRNReport.Columns[GetColumnIndexOf(grdGRNReport, "SerialNumber")].Visible = true;
                        grdGRNReport.Columns[GetColumnIndexOf(grdGRNReport, "BatchCode")].Visible = true;

                    }
                    else
                    {
                        grdGRNReport.Columns[GetColumnIndexOf(grdGRNReport, "SerialNumber")].Visible = false;
                        grdGRNReport.Columns[GetColumnIndexOf(grdGRNReport, "BatchCode")].Visible = false;
                    }

                }
                else
                {
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
        updgrid.Update();
        pnlGrid.Visible = false;

    }
    /* #CC02 Add Start */
    public void GetBCPDataFile()
    {
        try
        {
            using (TempClass obj = new TempClass())
            {
                string strExportFileName = string.Empty;
                strExportFileName = PageBase.importExportCSVFileName;
                if (PageBase.SalesChanelID != 0)
                {
                    obj.SalesChannelID = PageBase.SalesChanelID;
                }
                obj.GRNNumber = txtGRNNumber.Text.Trim();
                if (chksb.Checked == true)
                {
                    obj.IsSerialRequired = 1;
                }
                else
                {
                    obj.IsSerialRequired = 0;
                }
                obj.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                obj.DateTo = Convert.ToDateTime(ucDateTo.Date);
                obj.UserId = PageBase.UserId;
                obj.FilePath = "GRNFlatReport" + strExportFileName;
                DataSet ds = new DataSet();
                ds = obj.GetGRNReportDataCSV();
                if (ds.Tables[0].Rows.Count > 0)
                {

                    PageBase.ExportToExecl(ds, "GRNReport");

                    //if (obj.Result == 0)
                    //{
                    //    ucMessage1.Visible = false;
                    //    Response.Redirect((siteURL + strBCPFilePath + obj.FilePath));

                    //}
                    //else if (obj.Result == 1)
                    //{
                    //    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                    //}
                    //else
                    //{
                    //    ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    //}
                }
            }
        }
        catch (Exception ex)
        {
        }

    }
    /* #CC02 Add End */
    protected void exportToExel_Click(object sender, EventArgs e)
    {
        /* #CC02 Add Start */
        GetBCPDataFile();
        return;
        /* #CC02 Add End */

        //if (ViewState["Export"] != null)
        //{
        binddata();                 //Pankaj Dhingra
        DataTable dtExp = dt.Copy();

        //DataTable dt = (DataTable)ViewState["Export"];
        //string[] DsCol = new string[] { "SalesChannelCode", "HOName", "RBHName", "ZSMName", "SBHName", "ASOName","Country","State", "GRNNumber", "GRNDate", "PONumber", "PODate", "InvoiceNumber", "InvoiceDate", "SKUName", "Quantity", "Model", "Color", "Brand", "Product" };
        string[] DsCol = new string[] { "SalesChannelName", "SalesChannelCode", "HOName", "RBHName", "ZSMName", "SBHName", "ASOName", "Country", "State", "GRNNumber", "GRNDate", "SKUName", "Quantity", "Model", "Color", "Brand", "Product" };

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
                //PageBase.ExportToExeclUsingOPENXMLV2(dtcopy.Tables[0], FilenameToexport);  //#CC01 commented

                PageBase.ExportToExecl(dtcopy, FilenameToexport);  //#CC01 added
                //ViewState["Export"] = null;
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
        //ViewState["Export"] = null;
        // }
    }



    protected void grdGRNReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        grdGRNReport.PageIndex = e.NewPageIndex;
        binddata();

    }
    public Int32 GetColumnIndexOf(GridView gv, string ColumnHeaderName)
    {
        try
        {
            foreach (DataControlField column in gv.Columns)
            {
                if (column.HeaderText.ToUpper().Trim() == ColumnHeaderName.ToUpper().Trim())
                {
                    return gv.Columns.IndexOf(column);
                }
            }
            return -1;


        }
        catch (Exception ex)
        {
            return -1;
        }
    }
}

