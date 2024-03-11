using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess;
using BussinessLogic;
using DevExpress.Utils;
using DevExpress.Web.ASPxPivotGrid;
/*14-Nov-2015,#CC01, Vijay Kumar Prajapati,Get Report For Distributor and Admin.*/
/*
 03-Feb-2018, Sumit Maurya, #CC02, New dropdown added to select herarchy according to  level type. Implemented report for orgn hierarchy also (Done for Comio).
 * 19-Feb-2018, Sumit Maurya, #CC03A, New column added to filter data (Done for Comio).
 * 11-May-2018, Sumit Maurya, #CC03, Location Name moved from DataArea to filterarea (Informed by Rawat Sir) and Saleschannel type binding method changed.
 * 22-May-2018, Sumit Maurya, #CC04, Userid provided to get data according to user (Done for motorola).
 */

public partial class Reports_Common_TargetVsAchievementRpt : PageBase
{
    private double _dblCallClosed1 = 0;
    private double _dblCallClosed2 = 0;
    private double _dblCallClosed3 = 0;
    private double _dblCallClosed4 = 0;
    private double _dblJobReceived = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        EnableViewState = false;
        fncHide();

        if (!IsPostBack)
        {
            try
            {
                ucDateFrom.Date = PageBase.Fromdate;
                string LastDayOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();
                //ucDateTo.Date = PageBase.ToDate;
                ucDateTo.Date = DateTime.Now.Month.ToString() + "/" + LastDayOfMonth + "/" + DateTime.Now.Year.ToString();
                BindType();/* #CC02 Added */
                /*BindChannelType(); #CC02 Commented */
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }
        }
        if (hfSearch.Value == "1")
            BindGrid();
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        fncHide();

        try
        {
            ucDateFrom.Date = PageBase.Fromdate;
            string LastDayOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();
            //ucDateTo.Date = PageBase.ToDate;
            ucDateTo.Date = DateTime.Now.Month.ToString() + "/" + LastDayOfMonth + "/" + DateTime.Now.Year.ToString();
            BindType();/* #CC02 Added */
                       /*BindChannelType(); #CC02 Commented */
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

        if (hfSearch.Value == "1")
            BindGrid();
    }
    private void fncHide()
    {
        ucMsg.ShowControl = false;
        pnlSearch.Visible = false;
    }
    public void BindChannelType()
    {
        using (TargetData objTarget = new TargetData())
        {
            objTarget.UserType = 2;
            objTarget.showLevel = true;
            objTarget.UserId = PageBase.UserId; /* #CC07 Added  */
            if (PageBase.SalesChanelID == 0)
            {
                objTarget.UserTypeID = Convert.ToInt16(PageBase.HierarchyLevelID);

                objTarget.OwnLevel = 1;
                /*#CC01 Added Started*/
                String[] colArray = { "ID", "TYPEName" };
                DataTable dt = objTarget.GetTargetLevelUser();
                ddlHierarchy.DataSource = dt;
                ddlHierarchy.DataTextField = "TYPEName";
                ddlHierarchy.DataValueField = "ID";
                ddlHierarchy.DataBind();
                ddlHierarchy.Items.Insert(0, new ListItem("Select", "0"));
                ddlHierarchy.Items.Add(new ListItem("Retailer", "101"));/*#CC01 Added End*/
            }
            else
            {
                objTarget.UserTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                objTarget.OwnLevel = 2;
                /*#CC01 Added Started*/
                String[] colArray = { "ID", "TYPEName" };
                DataTable dt = objTarget.GetTargetLevelUser();
                ddlHierarchy.DataSource = dt;
                ddlHierarchy.DataTextField = "TYPEName";
                ddlHierarchy.DataValueField = "ID";
                ddlHierarchy.DataBind();
                ddlHierarchy.Items.Insert(0, new ListItem("Select", "0"));/*#CC01 Added End*/

            }



        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (pageValidate())

                BindGrid();
            else
                pnlSearch.Visible = false;
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    private void BindGrid()
    {
        using (ReportData objRpt = new ReportData())
        {
            objRpt.UserId = PageBase.UserId;
            objRpt.HierarchyLevelId = Convert.ToInt16(ddlHierarchy.SelectedValue);
            objRpt.TagetBasedOn = Convert.ToInt16(ddlTagetBased.SelectedValue);
            objRpt.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
            objRpt.DateTo = Convert.ToDateTime(ucDateTo.Date);
            objRpt.SalesChannelId = PageBase.SalesChanelID;
            objRpt.CompanyId = PageBase.ClientId;
            DataSet DsRpt = objRpt.GetTargetVsAchivementRpt();
            hfSearch.Value = "1";
            if (DsRpt.Tables[0].Rows.Count > 0)
            {
                ASPxPvtGrd.DataSource = DsRpt;
                ASPxPvtGrd.DataBind();
                pnlSearch.Visible = true;
            }
            else
            {
                ucMsg.ShowInfo(Resources.Messages.NoRecord);
                pnlSearch.Visible = false;

            }

        };
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        hfSearch.Value = "0";
        pnlSearch.Visible = false;
        ddlHierarchy.SelectedIndex = 0;
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;

    }
    bool pageValidate()
    {
        hfSearch.Value = "0";
        int _date = 0;
        if ((Convert.ToDateTime(ucDateFrom.Date) > DateTime.Now.Date))
        {
            ucMsg.ShowInfo(Resources.Messages.DateRangeValidation);
            return false;
        }

        _date = DateTime.Compare(Convert.ToDateTime(ucDateTo.Date), Convert.ToDateTime(ucDateFrom.Date));
        if (_date < 0)
        {
            ucMsg.ShowInfo(Resources.Messages.InvalidDate);
            return false;
        }
        if (ddlHierarchy.SelectedValue == "0" || ddlTagetBased.SelectedValue == "0" || ucDateFrom.Date == "" || ucDateTo.Date == "")
        {
            ucMsg.ShowInfo(Resources.Messages.MandatoryField);
            return false;

        }

        return true;
    }
    protected void buttonSaveAs_Click(object sender, EventArgs e)
    {
        try
        {
            Export(true);
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }
    protected void buttonOpen_Click(object sender, EventArgs e)
    {
        try
        {
            Export(false);
        }
        catch (Exception ex)
        {

            PageBase.Errorhandling(ex);
        }
    }
    private void Export(bool saveAs)
    {


        ASPxPivotGridExporter1.OptionsPrint.PrintHeadersOnEveryPage = checkPrintHeadersOnEveryPage.Checked;
        ASPxPivotGridExporter1.OptionsPrint.PrintFilterHeaders = checkPrintFilterHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintColumnHeaders = checkPrintColumnHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintRowHeaders = checkPrintRowHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintDataHeaders = checkPrintDataHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;

        string fileName = string.Format("TargetVsAchievementReport_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
        switch (ddlExportFormat.SelectedIndex)
        {
            case 0:
                ASPxPivotGridExporter1.ExportXlsToResponse(fileName, saveAs);
                break;
            case 1:
                ASPxPivotGridExporter1.ExportXlsxToResponse(fileName, saveAs);
                break;
        }
    }


    protected void ASPxPvtGrd_CustomSummary(object sender, DevExpress.Web.ASPxPivotGrid.PivotGridCustomSummaryEventArgs e)
    {
        try
        {
            DevExpress.XtraPivotGrid.PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();
            decimal totalTarget = 0, totalAchievement = 0;
            if (e.DataField.ToString() == "Achievement %")
            {
                decimal customValue = 0;
                /*This(SubtotalOrnot) will tell coming cell is sub total or not*/
                if (e.DataField.ToString() == "Achievement %" && !SubtotalOrnot(e.ColumnField, e.RowField))
                {
                    e.CustomValue = e.SummaryValue.Summary;
                }
                else
                {
                    decimal totalache = 0, Target = 0;
                    for (int i = 0; i < ds.RowCount; i++)
                    {
                        if ((Convert.ToDecimal(ds[i]["TARGET"])) > 0)
                        {
                            Target = Target + Convert.ToDecimal(ds[i]["TARGET"]);
                            totalache = totalache + Convert.ToDecimal(ds[i]["Achievement"]);

                        }
                    }
                    if (Target > 0)
                    {
                        customValue = System.Math.Round((totalache / Target * 100), 3);
                    }
                    else
                    {
                        customValue = 0;
                    }
                    e.CustomValue = customValue;
                }
                /*This will calculate the grand total Cell percentage
                 * and will override the above values in the grand total
                 */
                if (e.ColumnField == null && e.RowField == null)
                {

                    for (int i = 0; i < ds.RowCount; i++)
                    {
                        totalTarget = totalTarget + Convert.ToDecimal(Convert.ToDecimal(ds[i]["TARGET"]));
                        totalAchievement = totalAchievement + Convert.ToDecimal(Convert.ToDecimal(ds[i]["Achievement"]));
                    }

                    if (totalTarget > 0)
                        e.CustomValue = System.Math.Round(((totalAchievement / totalTarget) * 100), 3);
                    else
                        e.CustomValue = 0;
                }


            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());

        }
    }

    private bool SubtotalOrnot(PivotGridField columnField, PivotGridField rowField)
    {
        if (PivotGridField5.Area == DevExpress.XtraPivotGrid.PivotArea.ColumnArea && columnField != null && columnField.AreaIndex >= PivotGridField5.AreaIndex)
            return false;
        if (PivotGridField5.Area == DevExpress.XtraPivotGrid.PivotArea.RowArea && rowField != null && rowField.AreaIndex >= PivotGridField5.AreaIndex)
            return false;
        return true;

    }

    /* #CC02 Add Start */


    public void BindType()
    {
        try
        {
            //ddlType.Items.Clear();
            using (TargetData objTarget = new TargetData())
            {
                objTarget.UserId = PageBase.UserId; /* #CC04 Added  */
                objTarget.UserType = 1;
                objTarget.showLevel = false;
                objTarget.UserTypeID = 1;
                objTarget.OwnLevel = 1;
                String[] colArray = { "TargetToType", "TargetUserType" };
                DataTable dt = objTarget.GetTargetLevelUser();
                //PageBase.DropdownBinding(ref ddlType, dt, colArray);
                PageBase.DropdownBinding(ref ddlHierarchy, dt, colArray);
            }
        }
        catch (Exception ex)
        {
            // uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

    //protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ddlHierarchy.Items.Clear();         
    //         using (TargetData objTarget = new TargetData())
    //        {
    //            objTarget.UserType = Convert.ToInt16(ddlType.SelectedValue); 
    //            objTarget.showLevel = true;
    //            objTarget.UserId = PageBase.UserId; /* #CC04 Added  */

    //            if (ddlType.SelectedValue == PageBase.RetailerEntityTypeID.ToString())
    //            {
    //                ddlHierarchy.Items.Add(new ListItem("Retailer", PageBase.RetailerEntityTypeID.ToString()));
    //            }
    //            else
    //            {
    //                objTarget.SelectedTypeID = Convert.ToInt16(ddlType.SelectedValue);

    //                objTarget.OwnLevel = 1;
    //                objTarget.UserTypeID = 1;
    //                objTarget.UserType = Convert.ToInt16(ddlType.SelectedValue);                    

    //                String[] colArray = { "ID", "TYPEName" };
    //                PageBase.DropdownBinding(ref ddlHierarchy, objTarget.GetTargetLevelUser(), colArray);
    //            }
    //            /* #CC03 Add End */
    //         }

    //    }
    //    catch (Exception ex)
    //    {            
    //       ucMsg.ShowError(ex.ToString());
    //    }
    //}

}
