using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using ExportExcelOpenXML;
using System.Text;
using BusinessLogics;
using System.Collections;
using System.Data.SqlClient;
using ZedService;
using System.IO;

public partial class Reports_Common_ExpenseReports : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillEntityType();
            FillApprovalStatus();
        }
    }
    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEntityTypeName(Convert.ToInt32(ddlEntityType.SelectedValue));
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ucFromDate.Date != "" && ucToDate.Date == "")
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowWarning("Please Enter  To Date.");
            return;
        }
        if (ucToDate.Date != "" && ucFromDate.Date == "")
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowWarning("Please Enter  From Date.");
            return;
        }
        ucMessage1.Visible = false;

        SearchExpenseDetailData(1);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ddlEntityType.SelectedValue = "0";
        if (ddlEntityType.SelectedValue == "0")
        {
            ddlEntityTypeName.SelectedValue = "0";
            ddlEntityTypeName.Items.Clear();
            ddlEntityTypeName.Items.Insert(0, new ListItem("Select", "0"));
        }
        ddlApprovalStatus.SelectedValue = "0";
        gvExpenseDetail.DataSource = null;
        gvExpenseDetail.DataBind();
        ucFromDate.TextBoxDate.Text = "";
        ucToDate.TextBoxDate.Text = "";
        ucMessage1.Visible = false;
        PnlGrid.Visible = false;
    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        SearchExpenseDetailData(-1);
    }
    protected void gvExpenseDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Int64 ExpenseID = Convert.ToInt32(gvExpenseDetail.DataKeys[e.Row.RowIndex].Value);
                DataTable dtImageDetails = new DataTable();
                using (ClsExpense objdetail = new ClsExpense())
                {
                    objdetail.UserId = PageBase.UserId;
                    objdetail.CompanyId = PageBase.ClientId;
                    objdetail.ExpenseId = ExpenseID;
                    dtImageDetails = objdetail.GetExpenswViewImageInfo();

                }
                DataRow[] drv = dtImageDetails.Select("ExpenseID=" + ExpenseID);
                if (drv.Length > 0)
                {
                    DataTable dtTemp = dtImageDetails.Clone();

                    for (int cntr = 0; cntr < drv.Length; cntr++)
                    {
                        dtTemp.ImportRow(drv[cntr]);
                    }
                    GridView gvAttachedImages = (GridView)e.Row.FindControl("gvAttachedImages");
                    gvAttachedImages.DataSource = dtTemp;
                    gvAttachedImages.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    protected void lnkDownload_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = "image/jpg";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.TransmitFile(Server.MapPath(filePath));
            Response.End();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
    }
    void FillEntityType()
    {
        using (ClsExpense ObjEntityType = new ClsExpense())
        {

            ddlEntityType.Items.Clear();
            string[] str = { "EntityTypeID", "EntityType" };
            ObjEntityType.CompanyId = PageBase.ClientId;
            ObjEntityType.UserId = PageBase.UserId;
            PageBase.DropdownBinding(ref ddlEntityType, ObjEntityType.GetEntityTypeV5API(), str);

        };
    }
    void FillEntityTypeName(int EntityTypeID)
    {
        using (ClsExpense ObjEntityTypeName = new ClsExpense())
        {

            ddlEntityTypeName.Items.Clear();
            ObjEntityTypeName.EntityTypeId = EntityTypeID;
            ObjEntityTypeName.UserId = PageBase.UserId;
            ObjEntityTypeName.CompanyId = PageBase.ClientId;
            string[] str = { "UserID", "EntityTypeName" };
            PageBase.DropdownBinding(ref ddlEntityTypeName, ObjEntityTypeName.GetEntityTypeName(), str);

        };
    }
    void FillApprovalStatus()
    {
        using (ClsExpense ObjEntityType = new ClsExpense())
        {

            ddlApprovalStatus.Items.Clear();
            string[] str = { "ExpenseStatusId", "ExpenseStatus" };
            ObjEntityType.CompanyId = PageBase.ClientId;
            ObjEntityType.UserId = PageBase.UserId;
            PageBase.DropdownBinding(ref ddlApprovalStatus, ObjEntityType.GetApprovalStatus(), str);

        };
    }
    public void SearchExpenseDetailData(int pageno)
    {
        ClsExpense objExpense;
        try
        {
            ViewState["TotalRecords"] = 0;
            ViewState["CurrentPage"] = pageno;
            using (objExpense = new ClsExpense())
            {
                if (ucFromDate.Date == "" && ucToDate.Date == "")
                { ;}
                else
                {
                    objExpense.FromDate = Convert.ToDateTime(ucFromDate.Date);
                    objExpense.Todate = Convert.ToDateTime(ucToDate.Date);
                }
                objExpense.UserId = PageBase.UserId;
                objExpense.CompanyId = PageBase.ClientId;
                objExpense.EntityTypeId = Convert.ToInt32(ddlEntityType.SelectedValue);
                objExpense.EntitytypeUserId = Convert.ToInt16(ddlEntityTypeName.SelectedValue);
                objExpense.ExpenseStatus = Convert.ToInt32(ddlApprovalStatus.SelectedValue);
                objExpense.PageIndex = pageno;
                objExpense.PageSize = Convert.ToInt32(PageBase.PageSize);
                DataSet ds = objExpense.GetExpenseReportData();
                if (objExpense.TotalRecords > 0)
                {
                    PnlGrid.Visible = true;
                    if (pageno > 0)
                    {
                        gvExpenseDetail.DataSource = ds;
                        gvExpenseDetail.DataBind();
                        PnlGrid.Visible = true;
                        ViewState["TotalRecords"] = objExpense.TotalRecords;
                        ucPagingControl1.TotalRecords = objExpense.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {

                        string FilenameToexport = "ExpenseDetailData";
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    }
                }
                else
                {
                    ds = null;
                    gvExpenseDetail.DataSource = null;
                    gvExpenseDetail.DataBind();
                    ucMessage1.Visible = true;
                    PnlGrid.Visible = false;
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