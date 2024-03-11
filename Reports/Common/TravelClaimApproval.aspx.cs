using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;
using System.IO;

public partial class Reports_Common_TravelClaimApproval :PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate; 
            ddlEntityName.Items.Insert(0, new ListItem("Select", "0"));
            FillEntityType();
            BindApprovalStatus();
        }

    }
    
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchTravelClaimData(1);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        SearchTravelClaimData(-1);
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        SearchTravelClaimData(ucPagingControl1.CurrentPage);
    }
    void FillEntityType()
    {
        using (ClsTravelClaimApproval ObjTravelClaim = new ClsTravelClaimApproval())
        {

            ddlEntityType.Items.Clear();
            string[] str = { "UserID", "Name" };
            ObjTravelClaim.UserId = PageBase.UserId;
            PageBase.DropdownBinding(ref ddlEntityType, ObjTravelClaim.EntityType(), str);
        };
    }
    void FillEntityName()
    {
        using (ClsTravelClaimApproval ObjMappedEntity = new ClsTravelClaimApproval())
        {
            ObjMappedEntity.UserId = Convert.ToInt16(ddlEntityType.SelectedValue);
            ddlEntityName.Items.Clear();
            string[] str = { "EntityId", "Name" };
            PageBase.DropdownBinding(ref ddlEntityName, ObjMappedEntity.BindMappedName(), str);
        };
    }
    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEntityName();
    }
    public static DataTable GetEnumbyTableName(string Filename, string TableName)
    {
        DataTable dt = new DataTable();
        using (DataSet ds = new DataSet())
        {
            string filename = HttpContext.Current.Server.MapPath("~/Assets/XML/" + Filename + ".xml");
            ds.ReadXml(filename);
            dt = ds.Tables[TableName];
            if (dt == null || dt.Rows.Count == 0)
                return null;
        }
        try
        {
            dt = dt.Select("Active=1").CopyToDataTable();
            return dt;
        }
        catch (Exception)
        {
            return null;
        }
    }
    private void BindApprovalStatus()
    {
        DataTable dtresult = new DataTable();
        try
        {
            dtresult = GetEnumbyTableName("XML_Enum", "TravelClaimStatus");
            if (dtresult.Rows.Count > 0)
            {
                ddlApprovalStatus.DataSource = dtresult;
                ddlApprovalStatus.DataTextField = "Description";
                ddlApprovalStatus.DataValueField = "Value";
                ddlApprovalStatus.DataBind();
                ddlApprovalStatus.Items.Insert(0, new ListItem("Select", "255"));
            }
            else
            {
                ddlApprovalStatus.Items.Insert(0, new ListItem("Select", "255"));
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    public void SearchTravelClaimData(int pageno)
    {
        ClsTravelClaimApproval objtravelclaim;
        try
        {
            ViewState["TotalRecords"] = 0;
            ViewState["CurrentPage"] = pageno;
            using (objtravelclaim = new ClsTravelClaimApproval())
            {

                objtravelclaim.Fromdate = Convert.ToDateTime(ucDateFrom.Date);
                objtravelclaim.Todate = Convert.ToDateTime(ucDateTo.Date);

                objtravelclaim.UserId = PageBase.UserId;
                objtravelclaim.TravelClaimApprovedById = Convert.ToInt32(ddlEntityType.SelectedValue);
                objtravelclaim.TravelClaimCreatedById = Convert.ToInt16(ddlEntityName.SelectedValue);
                objtravelclaim.ApprovalStatus = Convert.ToInt32(ddlApprovalStatus.SelectedValue);
                objtravelclaim.PageIndex = pageno;
                objtravelclaim.PageSize = Convert.ToInt32(PageBase.PageSize);
                DataSet ds = objtravelclaim.GetReportTravelClaimData();
                if (objtravelclaim.TotalRecords > 0)
                {
                    if (pageno > 0)
                    {
                        GridTravelApproval.DataSource = ds;
                        GridTravelApproval.DataBind();
                        PnlGrid.Visible = true;
                        dvFooter.Visible = true;
                        ucMessage1.Visible = false;
                        dvhide.Visible = true;
                        ViewState["TotalRecords"] = objtravelclaim.TotalRecords;
                        ucPagingControl1.TotalRecords = objtravelclaim.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {

                        string FilenameToexport = "TravelClaimData";
                        PageBase.ExportToExecl(ds, FilenameToexport);

                    }
                }
                else
                {
                    ds = null;
                    GridTravelApproval.DataSource = null;
                    GridTravelApproval.DataBind();
                    dvFooter.Visible = false;
                    PnlGrid.Visible = false;
                    ucMessage1.Visible = true;
                    ucMessage1.ShowInfo("No Record Found!");

                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    private void Cancel()
    {
        dvhide.Visible = false;
        ddlApprovalStatus.SelectedValue = "255";
        ddlEntityType.SelectedValue = "0";
        ddlEntityName.SelectedValue = "0";
        GridTravelApproval.DataSource = null;
        GridTravelApproval.DataBind();
    }
}