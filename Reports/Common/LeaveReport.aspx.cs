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

public partial class Reports_Common_LeaveReport : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillEntityType();
            SearchLeaveDetailData(1);
        }
    }
    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEntityTypeName(Convert.ToInt32(ddlEntityType.SelectedValue));
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        if (ucFromDate.TextBoxDate.Text != "" && ucToDate.TextBoxDate.Text == "")
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowInfo("Please Enter To Date.");
            return;
        }
        if (ucFromDate.TextBoxDate.Text == "" && ucToDate.TextBoxDate.Text != "")
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowInfo("Please Enter From Date.");
            return;
        }
        SearchLeaveDetailData(1);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        ddlEntityType.SelectedValue = "0";
        ucFromDate.TextBoxDate.Text = "";
        ucToDate.TextBoxDate.Text = "";
        ddlEntityTypeName.SelectedValue = "0";
        ddlApprovalStatus.SelectedValue = "255";
        SearchLeaveDetailData(1);
    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        SearchLeaveDetailData(-1);
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        SearchLeaveDetailData(ucPagingControl1.CurrentPage);
    }
    void FillEntityType()
    {
        using (ClsPaymentReport ObjEntityType = new ClsPaymentReport())
        {

            ddlEntityType.Items.Clear();
            string[] str = { "EntityTypeID", "EntityType" };
            ObjEntityType.UserId = PageBase.UserId;
            ObjEntityType.CompanyId = PageBase.ClientId;
            PageBase.DropdownBinding(ref ddlEntityType, ObjEntityType.GetEntityTypeV5API(), str);

        };
    }
    void FillEntityTypeName(int EntityTypeID)
    {
        using (ClsPaymentReport ObjEntityTypeName = new ClsPaymentReport())
        {

            ddlEntityTypeName.Items.Clear();
            ObjEntityTypeName.EntityTypeId = EntityTypeID;
            ObjEntityTypeName.UserId = PageBase.UserId;
            ObjEntityTypeName.CompanyId = PageBase.ClientId;
            string[] str = { "UserID", "EntityTypeName" };
            PageBase.DropdownBinding(ref ddlEntityTypeName, ObjEntityTypeName.GetEntityTypeName(), str);

        };
    }
    public void SearchLeaveDetailData(int pageno)
    {
        ClsPaymentReport objLeave;
        try
        {
            ViewState["TotalRecords"] = 0;
            ViewState["CurrentPage"] = pageno;
            using (objLeave = new ClsPaymentReport())
            {
                if (ucFromDate.Date == "" && ucToDate.Date == "")
                { ;}
                else
                {
                    objLeave.FromDate = Convert.ToDateTime(ucFromDate.Date);
                    objLeave.Todate = Convert.ToDateTime(ucToDate.Date);
                }
                objLeave.UserId = PageBase.UserId;
                objLeave.CompanyId = PageBase.ClientId;
                objLeave.EntityTypeId = Convert.ToInt32(ddlEntityType.SelectedValue);
                objLeave.EntitytypeUserId = Convert.ToInt16(ddlEntityTypeName.SelectedValue);
                objLeave.PageIndex = pageno;
                objLeave.PageSize = Convert.ToInt32(PageBase.PageSize);
                objLeave.LeaveStatus = Convert.ToInt16(ddlApprovalStatus.SelectedValue);
                DataSet ds = objLeave.GetReportLeaveData();
                if (objLeave.TotalRecords > 0)
                {
                    PnlGrid.Visible = true;
                    if (pageno > 0)
                    {
                        gvLeaveDetail.DataSource = ds;
                        gvLeaveDetail.DataBind();
                        ViewState["TotalRecords"] = objLeave.TotalRecords;
                        ucPagingControl1.TotalRecords = objLeave.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {
                        string FilenameToexport = "LeaveDetailData";
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    }
                }
                else
                {
                    ds = null;
                    gvLeaveDetail.DataSource = null;
                    gvLeaveDetail.DataBind();
                    ucMessage1.Visible = true;
                    ucMessage1.ShowInfo("No Record Found.");

                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
}