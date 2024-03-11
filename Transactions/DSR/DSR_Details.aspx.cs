using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess;
using BussinessLogic;

public partial class Transactions_DSR_DSR_Details : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        if (!IsPostBack)
        {
            conditionfield.Value = Convert.ToString(1);
            ucPagingControl1.SetCurrentPage = 1;
            Bind_ddlMonth();
            Bind_DSRDetails(Convert.ToInt16(conditionfield.Value),1);
            BindHierarchy();
            BindParentHierarchy();
           
        }
        
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        conditionfield.Value = Convert.ToString(0);
        ucPagingControl1.SetCurrentPage = 1;
        Bind_DSRDetails(Convert.ToInt16(conditionfield.Value), 1);
    }
    private void Bind_ddlMonth()
    {
        try
        {
            ddlDSR_Month.Items.Clear();
            string[] monthOfYear = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames;
            int i = 1;
            foreach (string monthName in monthOfYear)
            {
                if (monthName == "")
                    break;
                ddlDSR_Month.Items.Insert(0, new ListItem(monthName, i.ToString()));
                i++;

            }
            ddlDSR_Month.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void Bind_DSRDetails(int Condition,int index)
    {

        try
        {
            index = index == 0 ? 1 : index;
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
                objuser.PageSize = Convert.ToInt32(PageBase.PageSize);
                objuser.PageIndex = index;
                objuser.Src_HierarchyLvlNm = ddlHierarchy_Level.Text;
                objuser.Src_LocatioNm = ddlHierarchy_Name.Text;
                objuser.Src_month = Convert.ToInt16(ddlDSR_Month.SelectedValue);
                objuser.Src_year = txtDSR_Year.Text == "" ? Convert.ToInt16(0) : Convert.ToInt16(txtDSR_Year.Text);
                DataTable dt = objuser.getDSRDetail(Condition);
                GvDSRDetails.DataSource = dt;
                GvDSRDetails.DataBind();
                if (dt == null || dt.Rows.Count == 0)
                {
                    ucPagingControl1.Visible = false;
                    ucMsg.ShowError(Resources.Messages.NoRecord);
                }
                else
                {
                    ucMsg.Visible = false;
                    ucPagingControl1.Visible = true;
                    ucPagingControl1.PageSize = Convert.ToInt32(PageBase.PageSize);
                    ucPagingControl1.TotalRecords = objuser.TotalRecords;
                    ucPagingControl1.FillPageInfo();
                }
                //DataTable dt = new DataTable();
                //using (OrgHierarchyData objuser = new OrgHierarchyData())
                //{
                //    int pageindex= ucPagingControl1.CurrentPage;
                //    objuser.PageIndex = pageindex;
                //    objuser.Src_HierarchyLvlNm = ddlHierarchy_Level.Text;
                //    objuser.Src_LocatioNm = ddlHierarchy_Name.Text;
                //    objuser.Src_month = Convert.ToInt16(ddlDSR_Month.SelectedValue);
                //    objuser.Src_year = txtDSR_Year.Text==""?Convert.ToInt16(0):Convert.ToInt16(txtDSR_Year.Text);
                //    dt = objuser.getDSRDetail(Condition);
                //};
                //GvDSRDetails.DataSource = dt;
                //GvDSRDetails.DataBind();

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindHierarchy()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlHierarchy_Level.Items.Clear();
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
                //dt = objuser.GetAllHierarchyLevel();
                dt = objuser.GetHierarchyLevelConditional(2);
            };
            String[] colArray = { "HierarchyLevelID", "HierarchyLevelName" };
            PageBase.DropdownBinding(ref ddlHierarchy_Level, dt, colArray);
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindParentHierarchy()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlHierarchy_Name.Items.Clear();
            using (OrgHierarchyData objOrg = new OrgHierarchyData())
            {
                objOrg.HierarchyLevelID = Convert.ToInt16(ddlHierarchy_Level.SelectedValue);
                objOrg.UserID = PageBase.UserId;
                dt = objOrg.GetSelectedHierachyID_forDSR();
            };
            String[] colArray = { "OrgnhierarchyID", "LocationName" };
            PageBase.DropdownBinding(ref ddlHierarchy_Name, dt, colArray);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void ddlHierarchy_Level_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindParentHierarchy();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ucMsg.Visible = false;
        conditionfield.Value = Convert.ToString(1);
        Bind_ddlMonth();
        Bind_DSRDetails(Convert.ToInt16(conditionfield.Value), 1);
        BindHierarchy();
        BindParentHierarchy();
        txtDSR_Year.Text = "";
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        using (OrgHierarchyData objuser = new OrgHierarchyData())
        {
            int intPageNumber = ucPagingControl1.CurrentPage;
            ViewState["PageIndex"] = intPageNumber;
            objuser.PageIndex = intPageNumber;
            Bind_DSRDetails(Convert.ToInt16(conditionfield.Value), ucPagingControl1.CurrentPage);
        }
    }
}
