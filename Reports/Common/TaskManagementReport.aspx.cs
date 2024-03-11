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
//======================================================================================
//* Developed By : Vijay Prajapati 
//* Role         : Software Engineer
//* Module       : Reports(Task Management)  
//* Description  :  This page is used for View Task Management reports 
//* ====================================================================================
/* Change Log
 * -------------------------------------------------------------------------------------
 * DD-MMM-YYYY, Name, #CCXX, Description
 */

public partial class Reports_Common_TaskManagementReport : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            FillEntityType();
            BindTaskPriority_Status(4);//CategoryForId 5= Task Status
            BindTaskPriority_Status(5);//CategoryForId 4= Task Priority
            BindTaskPriority_Status(7);//CategoryForId 7= Task Group
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ucFromDate.Date != "" && ucToDate.Date == "")
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowWarning("Please Enter  End Date.");
            return;
        }
        if (ucToDate.Date != "" && ucFromDate.Date == "")
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowWarning("Please Enter  Start Date.");
            return;
        }
        ucMessage1.Visible = false;
        SearchTaskDetailData(1);
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
        ddlTaskPriority.SelectedValue = "0";
        ddlTaskStatus.SelectedValue = "0";
        ucFromDate.Date = "";
        ucToDate.Date = "";
        gvTaskDetail.DataSource = null;
        gvTaskDetail.DataBind();
        gvResponseDetail.DataSource = null;
        gvResponseDetail.DataBind();
        PnlGrid.Visible = false;
        tblResponsedetail.Visible = false;
    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        SearchTaskDetailData(-1);
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        SearchTaskDetailData(ucPagingControl1.CurrentPage);
    }
    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEntityTypeName(Convert.ToInt32(ddlEntityType.SelectedValue));
    }
    private void BindTaskPriority_Status(int CategoryForId)
    {
        try
        {
            
            using (Task objTask = new Task())
            {

                objTask.UserID = PageBase.UserId;
                objTask.CategoryForId = CategoryForId;
                String[] StrCol1 = new String[] { "CategoryID", "CategoryName" };
                DataSet dsResult = objTask.GetCategoryList();
                if (CategoryForId == 4)//CategoryForId 4= Task Priority
                {
                    PageBase.DropdownBinding(ref ddlTaskPriority, dsResult.Tables[0], StrCol1);
                }
                else if (CategoryForId == 5)//CategoryForId 5= Task Status
                {
                    PageBase.DropdownBinding(ref ddlTaskStatus, dsResult.Tables[0], StrCol1);
                    

                }
                else if (CategoryForId == 7)
                {
                    PageBase.DropdownBinding(ref ddlTaskGroup, dsResult.Tables[0], StrCol1);
                }
            };

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    void FillEntityType()
    {
        using (Task ObjEntityType = new Task())
        {

            ddlEntityType.Items.Clear();
            string[] str = { "EntityTypeID", "EntityType" };
            ObjEntityType.CompanyId = PageBase.ClientId;
            ObjEntityType.UserID = PageBase.UserId;
            PageBase.DropdownBinding(ref ddlEntityType, ObjEntityType.GetEntityTypeV5API(), str);

        };
    }
    void FillEntityTypeName(int EntityTypeID)
    {
        using (Task ObjEntityTypeName = new Task())
        {

            ddlEntityTypeName.Items.Clear();
            ObjEntityTypeName.EntityTypeId = EntityTypeID;
            ObjEntityTypeName.UserID = PageBase.UserId;
            ObjEntityTypeName.CompanyId = PageBase.ClientId;
            string[] str = { "UserID", "EntityTypeName" };
            PageBase.DropdownBinding(ref ddlEntityTypeName, ObjEntityTypeName.GetEntityTypeName(), str);

        };
    }
    public void SearchTaskDetailData(int pageno)
    {
        Task objTask;
        try
        {
            ViewState["TotalRecords"] = 0;
            ViewState["CurrentPage"] = pageno;
            using (objTask = new Task())
            {
                if (ucFromDate.Date == "" && ucToDate.Date == "")
                { ;}
                else
                {
                    objTask.StartDate = Convert.ToDateTime(ucFromDate.Date);
                    objTask.EndDate = Convert.ToDateTime(ucToDate.Date);
                }
                objTask.UserID = PageBase.UserId;
                objTask.CompanyId = PageBase.ClientId;
                objTask.EntityTypeId = Convert.ToInt32(ddlEntityType.SelectedValue);
                objTask.EntitytypeUserId = Convert.ToInt16(ddlEntityTypeName.SelectedValue);
                objTask.PageIndex = pageno;
                objTask.PageSize = Convert.ToInt32(PageBase.PageSize);
                objTask.TaskPriorityId = Convert.ToInt32(ddlTaskPriority.SelectedValue);
                objTask.TaskStatusId = Convert.ToInt32(ddlTaskStatus.SelectedValue);
                objTask.TaskGroupId = Convert.ToInt32(ddlTaskGroup.SelectedValue);
                DataSet ds = objTask.GetReportTaskData();
                if (objTask.TotalRecords > 0)
                {
                    PnlGrid.Visible = true;
                    if (pageno > 0)
                    {
                        gvTaskDetail.DataSource = ds;
                        gvTaskDetail.DataBind();
                        PnlGrid.Visible = true;
                        ViewState["TotalRecords"] = objTask.TotalRecords;
                        ucPagingControl1.TotalRecords = objTask.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {
                        ds.Tables[0].TableName = "TaskUserDetail";//here will always be two tables
                        ds.Tables[1].TableName = "TaskResponseDetail";
                        int SheetCount = 2;
                        if (ds.Tables.Count > 1)
                        {

                            if (ds.Tables[1].Rows.Count <= 0)
                            {
                                SheetCount = 1;
                                ds.Tables.Remove(ds.Tables[1]);// we are removing because it was throwing exception
                                ds.AcceptChanges();

                            }
                        }

                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "TaskDetailData";
                        PageBase.RootFilePath = FilePath;
                        string[] strExcelSheetName = { "TaskDetail","TaskResponseDetail" };
                        ChangedExcelSheetNames(ref ds, strExcelSheetName, 2);

                        ZedService.Utility.ZedServiceUtil.ExportToExecl(ds, FilenameToexport, 2, strExcelSheetName);
                       
                    }
                }
                else
                {
                    ds = null;
                    gvTaskDetail.DataSource = null;
                    gvTaskDetail.DataBind();
                    ucMessage1.Visible = true;
                    PnlGrid.Visible = false;
                   

                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void gvTaskDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("ViewDetail") && e.CommandArgument != "")
        {
            GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int RowIndex = oItem.RowIndex;
            for (int i = 0; i < gvTaskDetail.Rows.Count; i++)
            {
                gvTaskDetail.Rows[i].BackColor = System.Drawing.Color.White;
            }
            gvTaskDetail.Rows[RowIndex].BackColor = System.Drawing.Color.YellowGreen;
            gvResponseBind(Convert.ToInt64(e.CommandArgument));
        }
        else
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError("Data Not In System");
            
        }
    }
    private void gvResponseBind(Int64 TaskUserId)
    {
        try
        {
            using (Task objViewResponseDetail = new Task())
            {

                DataTable dtResult = new DataTable();
                objViewResponseDetail.UserID = PageBase.UserId;
                objViewResponseDetail.CompanyId = PageBase.ClientId;
                dtResult = objViewResponseDetail.GetResponseDetail(TaskUserId);
                tblResponsedetail.Visible = true;
                if (dtResult.Rows.Count == 0)
                {
                    ucMessage1.ShowError(Resources.Messages.NoRecord);
                }
                gvResponseDetail.DataSource = dtResult;
                gvResponseDetail.DataBind();
                ucMessage1.Visible = false;
            };
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    protected void DownloadFile(object sender, EventArgs e)
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
    protected void gvResponseDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Int64 ResponseID = Convert.ToInt32(gvResponseDetail.DataKeys[e.Row.RowIndex].Value);
                DataTable dtImageDetails = new DataTable();
                using (Task objdetail = new Task())
                {
                    objdetail.UserID = PageBase.UserId;
                    objdetail.CompanyId = PageBase.ClientId;
                    objdetail.TaskResponseId = ResponseID;
                    dtImageDetails = objdetail.GetResponseViewImageInfo();

                }
                DataRow[] drv = dtImageDetails.Select("TaskResponseID=" + ResponseID);
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
}