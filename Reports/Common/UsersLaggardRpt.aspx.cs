using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using BussinessLogic;
/*Change Log:
 * * 19-Jun-14, Rakesh Goel, #CC01 - Removed viewstate and page inherited from PageBase
 * 31-oct-15, Karam Chand Sharma, #CC02 - (Hide all filter report will come according to user logged in ). please check more detail from given URL (https://zed-axis.basecamphq.com/projects/5476690-zed-salestrack/todo_items/181632832/comments#comment_274631379)
 * 15-Feb-16, Rakesh Goel, #CC03 - Added status column in output in export to excel
 * 18-Feb-2016, Sumit Maurya, #CC04, New filter ND and Status added , Search button and Search result grid is not displayed, new download buttton added to directly download result in excel format (in National Distributor dropdown , For HO all Entity will be displayed, For RBH only all mapped sales channal gets displayed, and for National distributor login only Self Entity / Sales Channal gets binded  ).
 * 10-Oct-2016, Sumit Maurya, #CC05, Instead of "ExportToExeclEPPTemplate" "ExportToExecl" is now use to download report as it was not downloading data in Google chrome browser.
 * 07-Feb-2018, Sumit Maurya, #CC06, Implementation done according to 
 * 04-Jul-2018, Balram Jha, #CC07, Dataview comented and dataset received from class file used for export.
 * 30-Aug-2018, Rakesh Raj, #CC08 - Flat Report Export - Dynamic Header for Hierarchy Column Names   
 */

public partial class Reports_Common_UsersLaggardRpt : PageBase //System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        if (!IsPostBack)
        {
            ddllocation.Enabled = false;
            BindHierarchy();
            /*  btnSearch_Click(null, null); */
            /*#CC02 ADDED*/
            /* #CC04 Commented*/

        }
    }
    private void BindData()
    {
        try
        {
            ViewState["DSexport"] = null;
            DataSet DsLaggardInfo;
            using (ReportData objLaggard = new ReportData())
            {

                objLaggard.UserId = PageBase.UserId;
                objLaggard.OrgHierarchyId = Convert.ToInt32(ddllocation.SelectedValue);


                DsLaggardInfo = objLaggard.GetLaggardReport();
                if (DsLaggardInfo.Tables[0].Rows.Count > 0)
                {
                    pnlGrid.Visible = true;
                    gridLaggard.DataSource = DsLaggardInfo.Tables[0];
                    gridLaggard.DataBind();
                    //#CC01 comment start
                    //if (DsLaggardInfo.Tables[0].Rows.Count>0)
                    //{
                    //    DataTable dtFilter = new DataTable();
                    //    dtFilter = DsLaggardInfo.Tables[0];
                    //    if (dtFilter.Columns.Contains("SalesChannelID") == true)
                    //        dtFilter.Columns.Remove("SalesChannelID");
                    //    ViewState["DSexport"] = dtFilter;
                    //}
                    //#CC01 comment end


                }
                else
                {
                    pnlGrid.Visible = false;
                    gridLaggard.DataSource = null;
                    gridLaggard.DataBind();
                    ViewState["DSexport"] = null;
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                }
                updgrid.Update();
            }
        }

        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }

    protected void gridLaggard_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridLaggard.PageIndex = e.NewPageIndex;
        BindData();
    }
    private void BindHierarchy()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlHierarchyLevel.Items.Clear();
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
                objuser.HierarchyLevelID = Convert.ToInt16(PageBase.HierarchyLevelID);
                dt = objuser.GetAllLowerHierarchyLevel();
            };
            String[] colArray = { "HierarchyLevelID", "HierarchyLevelName" };
            PageBase.DropdownBinding(ref ddlHierarchyLevel, dt, colArray);
            ddlHierarchyLevel.SelectedValue = Convert.ToString(PageBase.HierarchyLevelID);
            BindOwnHierarchyLevelLocation();
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindOwnHierarchyLevelLocation()
    {
        try
        {
            DataTable dt = new DataTable();
            ddllocation.Items.Clear();
            using (OrgHierarchyData objOrg = new OrgHierarchyData())
            {
                objOrg.HierarchyLevelID = Convert.ToInt16(ddlHierarchyLevel.SelectedValue);
                dt = objOrg.GetOwnHierarchyLevelLocation();

            };
            String[] colArray = { "OrgnhierarchyID", "LocationName" };
            PageBase.DropdownBinding(ref ddllocation, dt, colArray);
            if (dt.Rows.Count > 0)
                ddllocation.Enabled = true;
            else
                ddllocation.Enabled = false;
            pnlGrid.Visible = false;
            gridLaggard.DataSource = null;
            gridLaggard.DataBind();
            updgrid.Update();
            updInput.Update();

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void ddlHierarchyLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindOwnHierarchyLevelLocation();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        //#CC01 add start
        DataSet DsLaggardInfo;
        using (ReportData objLaggard = new ReportData())
        {

            objLaggard.UserId = PageBase.UserId;
            objLaggard.OrgHierarchyId = Convert.ToInt32(ddllocation.SelectedValue);
            /* #CC04 Add Start */
            objLaggard.Status = Convert.ToInt16(ddlStatus.SelectedValue);
            objLaggard.SalesChannelID = Convert.ToInt32(ucServiceEntity.SelectedValue);
            /* #CC04 Add End */
            DsLaggardInfo = objLaggard.GetLaggardReport();
            if (DsLaggardInfo.Tables[0].Rows.Count > 0)
            {
                try
                {
                    //#CC07 comented
                    //DataSet dscopy = new DataSet();
                    //DataTable DTFinal = new DataTable();
                    //DataTable DTFinal1 = new DataTable();

                    //DataView view = new DataView(DsLaggardInfo.Tables[0]);
                    //#CC07 comented end
                   // DTFinal = view.ToTable(true, "SalesChannelTypeName", "ParentSalesChannelName" /*#CC02 ADDED*/ , "SalesChannelCode", "SalesChannelName", "HOName", "RBHName", "ZBHName", "SBHName", "ASOName", "MaximumTransDate", "OverDueDays", "AgingSlabText"  /*#CC02 ADDED*/, "LastTransactionCreationDate", "LastLoginOn", "Status" /*#CC03 added*/); /*#CC06 Commented  */
                    //DTFinal = view.ToTable(true, "SalesChannelTypeName", "ParentSalesChannelName", "SalesChannelCode", "SalesChannelName", "HL1Name", "HL2Name", "HL3Name", "HL4Name", "HL5Name", "MaximumTransDate", "OverDueDays", "AgingSlabText", "LastTransactionCreationDate", "LastLoginOn", "Status"); /*#CC06 Added  */

                    /*DTFinal.Columns["SalesChannelTypeName"].ColumnName = "Type";*/
                    //DTFinal.Columns["SalesChannelTypeName"].ColumnName = "Type of Entity"; /*#CC02 ADDED TYPE*/
                    //DTFinal.Columns["ParentSalesChannelName"].ColumnName = "Mapped Parent Entity Name";
                    //DTFinal.Columns["SalesChannelCode"].ColumnName = "Sales Channel Code";
                    //DTFinal.Columns["SalesChannelName"].ColumnName = "Sales Channel Name";
                   /* #CC06 Comment Start 
                    DTFinal.Columns["HOName"].ColumnName = Resources.SalesHierarchy.HierarchyName1;
                    DTFinal.Columns["RBHName"].ColumnName = Resources.SalesHierarchy.HierarchyName2;
                    DTFinal.Columns["ZBHName"].ColumnName = Resources.SalesHierarchy.HierarchyName3;
                    DTFinal.Columns["SBHName"].ColumnName = Resources.SalesHierarchy.HierarchyName4;
                    DTFinal.Columns["ASOName"].ColumnName = Resources.SalesHierarchy.HierarchyName5;*/
                    /*DTFinal.Columns["MaximumTransDate"].ColumnName = "Max Transaction Date";
                     DTFinal.Columns["OverDueDays"].ColumnName = "Overdue Days";
                    #CC06 Comment End  */

                    /* #CC06 Add Start */
                    //DTFinal1 = objLaggard.headerReplacement(DsLaggardInfo.Tables[0]);
                    /*#CC08*/// objLaggard.headerReplacement(DsLaggardInfo.Tables[0]);
                    /*if (DTFinal.Columns.Contains("HL1Name"))
                    {
                        DTFinal.Columns["HL1Name"].ColumnName = Resources.SalesHierarchy.HierarchyName1;
                    }
                    if (DTFinal.Columns.Contains("HL2Name"))
                    {
                        DTFinal.Columns["HL2Name"].ColumnName = Resources.SalesHierarchy.HierarchyName2;
                    }
                    if (DTFinal.Columns.Contains("HL3Name"))
                    {
                        DTFinal.Columns["HL3Name"].ColumnName = Resources.SalesHierarchy.HierarchyName3;
                    }
                    if (DTFinal.Columns.Contains("HL4Name"))
                    {
                        DTFinal.Columns["HL4Name"].ColumnName = Resources.SalesHierarchy.HierarchyName4;

                    }
                    if (DTFinal.Columns.Contains("HL5Name"))
                    {
                        DTFinal.Columns["HL5Name"].ColumnName = Resources.SalesHierarchy.HierarchyName5;
                    }*/

                    /* #CC06 Add End */
                    //#CC07 comented below
                    //DTFinal1.Columns["MaximumTransDate"].ColumnName = "Last Transaction Date"; /*#CC02 ADDED Max Transaction Date*/
                    //DTFinal1.Columns["OverDueDays"].ColumnName = "Ageing in Days"; /*#CC02 ADDED Overdue Days*/
                    //DTFinal1.Columns["AgingSlabText"].ColumnName = "Ageing In Slab";
                    //DTFinal1.Columns["LastTransactionCreationDate"].ColumnName = "Last Transaction Creation Date";
                    //DTFinal1.Columns["LastLoginOn"].ColumnName = "Last Login Date";
                    DsLaggardInfo.Tables[0].Columns["MaximumTransDate"].ColumnName = "Last Transaction Date"; /*#CC02 ADDED Max Transaction Date*/
                    DsLaggardInfo.Tables[0].Columns["OverDueDays"].ColumnName = "Ageing in Days"; /*#CC02 ADDED Overdue Days*/
                    DsLaggardInfo.Tables[0].Columns["AgingSlabText"].ColumnName = "Ageing In Slab";
                    DsLaggardInfo.Tables[0].Columns["LastTransactionCreationDate"].ColumnName = "Last Transaction Creation Date";
                    DsLaggardInfo.Tables[0].Columns["LastLoginOn"].ColumnName = "Last Login Date";

                    DsLaggardInfo.Tables[0].AcceptChanges();
                    //dscopy.Tables.Add(DsLaggardInfo.Tables[0]);



                    //dscopy.Merge(DsLaggardInfo);
                    //if (dscopy.Tables[0].Columns.Contains("LastTransactionCreationDate"))
                    //    dscopy.Tables[0].Columns["LastTransactionCreationDate"].ColumnName = "Last Transaction Creation Date";
                    //if (dscopy.Tables[0].Columns.Contains("lastloginon"))
                    //    dscopy.Tables[0].Columns["lastloginon"].ColumnName = "Last Login On";
                    //if (dscopy.Tables[0].Columns.Contains("SalesChannelID"))
                    //    dscopy.Tables[0].Columns.Remove("SalesChannelID");
                    //dscopy.Tables[0].AcceptChanges();

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "LaggardReport";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(DsLaggardInfo, FilenameToexport); /* #CC05 Uncommented */
                    /*PageBase.ExportToExeclEPPTemplate(dscopy, FilenameToexport, Server.MapPath(PageBase.strExcelTemplatePathSB) + "BlankExportTemplateLaggardReport.xlsx"); #CC05 Commented */
                    ViewState["DSexport"] = null;
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                }


            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);
            }

        }
        //#CC01 add end

        //#CC01 comment start
        //if (ViewState["DSexport"] != null)
        //{
        //    DataTable dt = new DataTable();
        //    dt = (DataTable)ViewState["DSexport"];
        //    DataTable DtCopy = new DataTable();
        //    if (dt.Rows.Count > 0)
        //    {
        //        try
        //        {
        //            DataSet dscopy = new DataSet();
        //            dscopy.Merge(dt);
        //            if (dscopy.Tables[0].Columns.Contains("LastTransactionCreationDate"))
        //                dscopy.Tables[0].Columns["LastTransactionCreationDate"].ColumnName = "Last Transaction Creation Date";
        //            if (dscopy.Tables[0].Columns.Contains("lastloginon"))
        //                dscopy.Tables[0].Columns["lastloginon"].ColumnName = "Last Login On";
        //            dscopy.Tables[0].AcceptChanges();
        //            String FilePath = Server.MapPath("../../");
        //            string FilenameToexport = "LaggardReport";
        //            PageBase.RootFilePath = FilePath;
        //            PageBase.ExportToExecl(dscopy, FilenameToexport);
        //            ViewState["DSexport"] = null;
        //        }
        //        catch (Exception ex)
        //        {
        //            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        //        }
        //    }
        //    else
        //    {
        //        ucMessage1.ShowError(Resources.Messages.NoRecord);

        //    }
        //    ViewState["DSexport"] = null;
        //}
        //#CC01 comment end
    }

    /* #CC04 Add Start */
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            btnExportToExcel_Click(null, null);
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    /* #CC04 Add End */
}
