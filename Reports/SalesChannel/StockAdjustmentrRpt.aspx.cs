using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
/*Change Log:
 * 08-May-2014, Rakesh Goel, #CC01 - Changed export to excel call to EPP method instead of OpenXML.
 * Also added exception display in catch block
 * 22-Jan-2016, Rakesh Goel, #CC02 - Added Adjustment reason and remarks in export to excel
 * 27-July-2016, Karam Chand Sharma, #CC03 - Added Stock Type in export to excel
 * 19-Jul-2018, Balram Jha, #CC04 - ViewState removed for export.
 * 29-Aug-2018, Rakesh Raj, #CC05, Flat Report Export - Dynamic Header for Hierarchy Column Names   
 */
public partial class Reports_SalesChannel_StockAdjustmentrRpt : PageBase//System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {
                ucDateFrom.Date = PageBase.Fromdate;
                ucDateTo.Date = PageBase.ToDate;
                ViewState["Detail"] = null;
                Hide();
                FillsalesChannelType();
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString(),PageBase.GlobalErrorDisplay());
        }
    }
    void Hide()
    {
        pnlGrid.Visible = false;
        btnExportToExcel.Visible = false;
    }
    protected void btnSerch_Click(object sender, EventArgs e)
    {
        pnlGrid.Visible = false;
       // updgrid.Update();
        try
        {
            if (!isvalidate())
            {
                return;
            }
            FillGrid();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString(), PageBase.GlobalErrorDisplay());
        }
        
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;
        Hide();
        ViewState["Detail"] = null;
        cmbSalesChannelType.SelectedValue = "0";
        ucDateFrom.Date = "";
        ucDateTo.Date = "";
        txtsaleschannel.Text = "";
       // updgrid.Update();
    }
    void FillGrid()
    {
        using (ReportData ObjReport = new ReportData())
        {
            ObjReport.SalesChannelTypeID = Convert.ToInt16(cmbSalesChannelType.SelectedValue);
            ObjReport.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
            ObjReport.DateTo= Convert.ToDateTime(ucDateTo.Date);
            ObjReport.SalesChannelID = PageBase.SalesChanelID;
            ObjReport.UserId = PageBase.UserId;
            ObjReport.DownloadType = 0;
            ObjReport.SalesChannelName = txtsaleschannel.Text.Trim();
            DataTable Dt = ObjReport.GetStockAdjustmentReport();
            if (Dt != null && Dt.Rows.Count > 0)
            {
                pnlGrid.Visible = true;
                btnExportToExcel.Visible = true;
                //ViewState["Detail"] = Dt; #CC04 comented
                grdStockAdjustment.DataSource = Dt;
                grdStockAdjustment.DataBind();

            }
            else
            {
                ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                pnlGrid.Visible = false;
                grdStockAdjustment.DataSource = null;
                grdStockAdjustment.DataBind();
                //ViewState["Detail"] = null;#CC04 comented
            }
          //  updgrid.Update();
        }
    }
   
    protected void grdStockAdjustment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdStockAdjustment.PageIndex = e.NewPageIndex;
        FillGrid();
    }
    bool isvalidate()
    {
        
            if (cmbSalesChannelType.SelectedValue == "0")
            {
                ucMessage1.ShowInfo("Please enter sales channel type");
                return false;
            }
              
            if (ucDateFrom.Date == "" && ucDateTo.Date == "")
            {
                ucMessage1.ShowInfo("Please enter date");
                return false;
            }

            if (ucDateFrom.Date == "" && ucDateTo.Date != "")
            {
                ucMessage1.ShowInfo("Please select from date ");
                return false;
            }

            if (ucDateFrom.Date != "" && ucDateTo.Date == "")
            {
                ucMessage1.ShowInfo("Please select to date");
                return false;
            }
       
            if (Convert.ToDateTime(ucDateFrom.Date) > Convert.ToDateTime(ucDateTo.Date))
            {
                ucMessage1.ShowInfo("The from date should be less than or equal to  To date ");
                return false;
            }
     
        return true;

    }
    void FillsalesChannelType()
    {
        using (SalesChannelData obj = new SalesChannelData())
        {
            DataTable dt = obj.GetSalesChannelType();
          
            String[] colArray = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref cmbSalesChannelType, dt.DefaultView.ToTable(), colArray);

        }
    }
    protected void exportToExel_Click(object sender, EventArgs e)
    {
        try
        {
            using (ReportData ObjReport = new ReportData())
            {
                ObjReport.SalesChannelTypeID = Convert.ToInt16(cmbSalesChannelType.SelectedValue);
                ObjReport.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                ObjReport.DateTo = Convert.ToDateTime(ucDateTo.Date);
                ObjReport.SalesChannelID = PageBase.SalesChanelID;
                ObjReport.UserId = PageBase.UserId;
                ObjReport.DownloadType = 1;
                ObjReport.SalesChannelName = txtsaleschannel.Text.Trim();
                DataTable Dt = ObjReport.GetStockAdjustmentReport();
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    //#CC05 ObjReport.headerReplacement(Dt);
                    DataSet dtcopy = new DataSet();
                    //DataTable dtcopy1 = new DataTable();
                    dtcopy.Merge(Dt);
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "StockAdjustmentReport";
                    PageBase.RootFilePath = FilePath;

                    PageBase.ExportToExecl(dtcopy, FilenameToexport);  //#CC01 added

                }
            }
            /*#CC04 coment start
            //using (ReportData ObjReport = new ReportData())
            //{
            //    if (ViewState["Detail"] != null)
            //    {
            //        DataTable dt = (DataTable)ViewState["Detail"];
            //        string[] DsCol = new string[] { "HL1Name", "HL2Name", "HL3Name", "HL4Name", "HL5Name", "StockAdjustmentNo", "SalesChannel", "StockAdjustmentDate", "ProductCategoryName", "BrandName", "ProductName", "ModelName", "ColorName", "SKUCode", "SKUName", "Quantity", "Reason", "Remarks", "StockType" };  /*#CC02 added reason and remarks*/ /*#CC03 added StockType*/
            //        DataTable DsCopy = new DataTable();
            //        dt = dt.DefaultView.ToTable(true, DsCol);

            //        if (dt.Rows.Count > 0)
            //        {
            
            //            DataSet dtcopy = new DataSet();
            //            DataTable dtcopy1 = new DataTable();
            //            //dt.Columns["HL1Name"].ColumnName = Resources.SalesHierarchy.HierarchyName1;

            //            //dt.Columns["HL2Name"].ColumnName = Resources.SalesHierarchy.HierarchyName2;
            //            //dt.Columns["HL3Name"].ColumnName = Resources.SalesHierarchy.HierarchyName3;
            //            //dt.Columns["HL4Name"].ColumnName = Resources.SalesHierarchy.HierarchyName4;
            //            //dt.Columns["HL5Name"].ColumnName = Resources.SalesHierarchy.HierarchyName5;
            //            //dtcopy.Merge(dt);
            //            //dtcopy.Tables[0].AcceptChanges();
            //            dtcopy1 = ObjReport.headerReplacement(dt);
            //            dtcopy.Merge(dtcopy1);
            //            dtcopy.Tables[0].AcceptChanges();
            //            String FilePath = Server.MapPath("../../");
            //            string FilenameToexport = "StockAdjustmentReport";
            //            PageBase.RootFilePath = FilePath;
            //            //PageBase.ExportToExeclUsingOPENXMLV2(dtcopy.Tables[0], FilenameToexport);  //#CC01 commented


            //            PageBase.ExportToExecl(dtcopy, FilenameToexport);  //#CC01 added

            //        }

            //    }

            //    else
            //    {
            //        ucMessage1.ShowError(Resources.Messages.NoRecord);

            //    }
            //    ViewState["Detail"] = null;
            //} #CC04 coment end*/
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
}
