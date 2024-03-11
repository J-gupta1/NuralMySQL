﻿/*
====================================================================================================================================
 *Change Log: 
 *DD-MMM-YYYY, Name , #CCXX - Description
------------------------------------------------------------------------------------------------------------------------------------
 ====================================================================================================================================
 */
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;

public partial class CreateTask : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            ucMsg.Visible = false;
            if (!IsPostBack)
            {
                //chkActive.Checked = true;
                if (ViewState["EditTaskUserId"] != null)
                { ViewState.Remove("EditTaskUserId"); }
                BindUser();
                BindTaskPriority_Status(4);//CategoryForId 5= Task Status
                BindTaskPriority_Status(5);//CategoryForId 4= Task Priority
                BindTaskPriority_Status(7);//CategoryForId 7= Task Group
                
                FillGrid(1);
                
                
            }
            FillGrid(1);
        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    
    protected void btnCreateTask_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            DataTable dtUser = new DataTable();
            dtUser.Columns.Add("UserId", typeof(Int64));
            dtUser.AcceptChanges();
            
            for (int cntr = 0; cntr < chkUser.Items.Count;cntr++ )
            {
                if (chkUser.Items[cntr].Selected)
                {
                    DataRow dr = dtUser.NewRow();
                    dr["UserId"] = Convert.ToInt64( chkUser.Items[cntr].Value);
                    dtUser.Rows.Add(dr);
                    dtUser.AcceptChanges();
                }
            }

                if (txtTask.Text.Trim().Length == 0 || ddlPriority.SelectedIndex == 0 || ddlTaskStatus.SelectedIndex==0 ||
             ucStartDate.Date.Trim().Length == 0 || UcEndDate.Date.Trim().Length == 0 || dtUser.Rows.Count == 0)
                {
                    ucMsg.ShowInfo(Resources.Messages.MandatoryField);

                    return;
                }
                using (Task objTask = new Task())
                {

                        objTask.UserID = PageBase.UserId;
                       
                    
                    objTask.StartDate = ucStartDate.GetDate;
                    objTask.EndDate = UcEndDate.GetDate;
                    objTask.TaskDescription = Server.HtmlEncode( txtTask.Text.Trim());
                    
                    objTask.Remark =  Server.HtmlEncode( txtRemark.Text.Trim());
                    objTask.TaskPriorityId = Convert.ToInt32(ddlPriority.SelectedValue);
                    objTask.TaskStatusId = Convert.ToInt32(ddlTaskStatus.SelectedValue);
                    objTask.TaskGroupId = Convert.ToInt32(ddlTaskGroup.SelectedValue);
                    objTask.Dt = dtUser;
                    
                    objTask.InsertTask();
                    if (objTask.OutParam == 0)
                    {
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        ClearForm();
                        FillGrid(1);
                        return;
                    }
                    else if (objTask.OutParam > 0)
                    {
                        if(!string.IsNullOrEmpty( objTask.error))
                        {
                            ucMsg.ShowError( objTask.error);
                        }
                        else
                        {
                            ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                        }
                        
                    }
                    
                };

                
            

        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            ucMsg.Visible = true;
            PageBase.Errorhandling(ex);

        }


    }
    protected override void OnPreRender(EventArgs e)
    {
        
        base.OnPreRender(e);
    }
    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btnEdit = (ImageButton)sender;
        
        
    }
    
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (UcStartDateSearch.Date=="" && UcEndDateSearch.Date=="" && ddlPrioritySearch.SelectedIndex==0 && ddlTaskStatusSearch.SelectedIndex==0 && ddlUserSearch.SelectedIndex==0 && ddlTaskGroupSearch.SelectedIndex==0)
        {
            ucMsg.ShowWarning("Please select any search criteria.");
            return;
        }
        if ((UcStartDateSearch.Date != "" && UcEndDateSearch.Date == "") || (UcStartDateSearch.Date == "" && UcEndDateSearch.Date != ""))
        {
            ucMsg.ShowWarning("Please select start and end date.");
            return;
        }
        if (UcStartDateSearch.GetDate > UcEndDateSearch.GetDate )
        {
            ucMsg.ShowWarning("End date can't be before start date.");
            return;
        }
        ViewState["Search"] = "Search";
        FillGrid(1);

    }
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearForm();
        FillGrid(1);

    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        ddlUserSearch.SelectedIndex = 0;
        ddlPrioritySearch.SelectedIndex = 0;
        UcStartDateSearch.Date = "";
        UcEndDateSearch.Date = "";
        ddlTaskStatusSearch.SelectedIndex = 0;
        ddlTaskGroupSearch.SelectedIndex = 0;
        FillGrid(1);
    }
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        FillGrid(-1);
        
    }
    #region User Defind Function
    private void ClearForm()
    {
        ViewState["EditTaskUserId"] = null;
        ddlPriority.Enabled = true;
        ddlPriority.SelectedIndex = 0;
        txtTask.Text = "";
        txtRemark.Text = "";
        ucStartDate.Date="";
        UcEndDate.Date = "";
        btnCreateTask.Text = "Submit";
        if(ddlTaskGroup.Items.Count>1)
        {
            ddlTaskGroup.SelectedValue = "0";
        }
       
        chkUser.SelectedIndex = -1;
       
        ddlTaskStatus.SelectedIndex = 0;
        
    }
   
    private void BindUser()
    {
        try
        {
            DataTable dt = new DataTable();

            chkUser.Items.Clear();
            using (Task objTask = new Task())
            {

                objTask.UserID = PageBase.UserId;
                dt = objTask.GetTaskUserList();
                chkUser.DataSource = dt;
                chkUser.DataTextField = "DisplayName";
                chkUser.DataValueField = "UserID";
                chkUser.DataBind();
                String[] StrCol1 = new String[] { "UserID", "DisplayName" };
                PageBase.DropdownBinding(ref ddlUserSearch, dt, StrCol1);
                ddlUserSearch.Items.Add(new ListItem("Self Task", PageBase.UserId.ToString()));

            }
            
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    
    private void BindTaskPriority_Status(int CategoryForId)
    {
        try
        {
            //DataTable dt = new DataTable();

            
            using (Task objTask = new Task())
           
            {

                objTask.UserID = PageBase.UserId;
                objTask.CategoryForId = CategoryForId;
                String[] StrCol1 = new String[] { "CategoryID", "CategoryName" };
                DataSet dsResult = objTask.GetCategoryList();
                if (CategoryForId == 4)//CategoryForId 4= Task Priority
                {
                    PageBase.DropdownBinding(ref ddlPriority, dsResult.Tables[0], StrCol1);
                    PageBase.DropdownBinding(ref ddlPrioritySearch, dsResult.Tables[0], StrCol1);
                }
                else if (CategoryForId == 5)//CategoryForId 5= Task Status
                {
                    PageBase.DropdownBinding(ref ddlTaskStatus, dsResult.Tables[0], StrCol1);
                    PageBase.DropdownBinding(ref ddlTaskStatusSearch, dsResult.Tables[0], StrCol1);
                    
                }
                else if(CategoryForId==7)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        if (dsResult.Tables[0].Rows.Count == 1)
                        {
                            ddlTaskGroup.DataSource = dsResult.Tables[0];
                            ddlTaskGroup.DataTextField = "CategoryName";
                            ddlTaskGroup.DataValueField = "CategoryID";
                            ddlTaskGroup.DataBind();
                            
                            ddlTaskGroup.SelectedValue = dsResult.Tables[0].Rows[0]["CategoryName"].ToString();
                            

                            ddlTaskGroupSearch.DataSource = dsResult.Tables[0];
                            ddlTaskGroupSearch.DataTextField = "CategoryName";
                            ddlTaskGroupSearch.DataValueField = "CategoryID";
                            ddlTaskGroupSearch.DataBind();
                        }
                        else
                        {
                            ddlTaskGroup.DataSource = dsResult.Tables[0];
                            ddlTaskGroup.DataTextField = "CategoryName";
                            ddlTaskGroup.DataValueField = "CategoryID";
                            ddlTaskGroup.DataBind();
                            ddlTaskGroup.Items.Insert(0, new ListItem("Select", "0"));

                            ddlTaskGroupSearch.DataSource = dsResult.Tables[0];
                            ddlTaskGroupSearch.DataTextField = "CategoryName";
                            ddlTaskGroupSearch.DataValueField = "CategoryID";
                            ddlTaskGroupSearch.DataBind();
                            ddlTaskGroupSearch.Items.Insert(0, new ListItem("Select", "0"));
                        }
                    }
                    else
                    {
                        ddlTaskGroup.Items.Insert(0, new ListItem("Select", "0"));
                        ddlTaskGroupSearch.Items.Insert(0, new ListItem("Select", "0"));
                    }
                }
            };
            
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }

    private void FillGrid(int pageno)
    {
        ViewState["TotalRecords"] = 0;
       // ViewState["CurrentPage"] = pageno;
        DataSet dsTask=new DataSet();
        using (Task objTask = new Task())
        {
            objTask.UserID = PageBase.UserId;
            objTask.TaskPriorityId = Convert.ToInt32( ddlPrioritySearch.SelectedValue);
            objTask.TaskStatusId = Convert.ToInt32(ddlTaskStatusSearch.SelectedValue);
            if (!string.IsNullOrEmpty( UcStartDateSearch.Date))
                objTask.StartDate = UcStartDateSearch.GetDate;
            if (!string.IsNullOrEmpty(UcEndDateSearch.Date))
                objTask.EndDate = UcEndDateSearch.GetDate;
            objTask.TaskUserID = 0;
            if (ddlUserSearch.SelectedIndex>0)
            objTask.TaskForUserId =Convert.ToInt64( ddlUserSearch.SelectedValue);
            objTask.PageIndex = pageno;
            objTask.PageSize = Convert.ToInt32(PageBase.PageSize);
            objTask.CompanyId = PageBase.ClientId;
            objTask.TaskGroupId = Convert.ToInt32(ddlTaskGroupSearch.SelectedValue);
            dsTask = objTask.getTaskData();

            if (dsTask != null && dsTask.Tables[0].Rows.Count > 0)
            {
                if (pageno > 0)
                {
                    ViewState["TotalRecords"] = objTask.TotalRecords;
                    ucPagingControl1.TotalRecords = objTask.TotalRecords;
                    ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                   // ucPagingControl1.SetCurrentPage = pageno;
                    ucPagingControl1.FillPageInfo();
                    grdvwTask.DataSource = dsTask.Tables[0];
                    ViewState["TotalRecords"] = objTask.TotalRecords;
                    grdvwTask.DataBind();
                    grdvwTask.Visible = true;
                }
                else
                {
                    
                    string FilenameToexport = "TaskDetailData";
                    PageBase.ExportToExecl(dsTask, FilenameToexport);
                }
            }
            else
            {
                //ViewState["Dtexport"] = null;
                grdvwTask.DataSource = null;
                grdvwTask.DataBind();
            }
            
            //UpdSearch.Update();
        };
        
    }
   

   
    #endregion

    protected void grdvwTask_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridViewRow GVR = e.Row;
                Int64 TaksUserID = Convert.ToInt32(grdvwTask.DataKeys[e.Row.RowIndex].Value);
                HyperLink HLDetails = default(HyperLink);
                HLDetails = (HyperLink)GVR.FindControl("HLDetails");
                string strViewDBranchDtlURL = null;

                //strViewDBranchDtlURL = "ViewRetailerDetail.aspx?RetailerId=" + Crypto.Encrypt(Convert.ToString(RetailerID), PageBase.KeyStr);

                strViewDBranchDtlURL = Server.UrlEncode(Cryptography.Crypto.Encrypt(Convert.ToString(TaksUserID), PageBase.KeyStr)).ToString().Replace("+", " ");
                {
                    HLDetails.Text = "View / Update";
                    HLDetails.Attributes.Add("onClick", string.Format("return popup('" + strViewDBranchDtlURL + "')"));
                }
                
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    
    protected void grdvwTask_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int Result = 0;
        Int64 TaskUserId = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "ViewTask")
        {
            if (TaskUserId > 0)
            {
                

            }
        }
        if (e.CommandName.ToLower() == "UpdateTaskStatus")
        {

            if (TaskUserId > 0)
            {
                
                FillGrid(1);

            }

        }
    }

    protected void UCPagingControl1_SetControlRefresh()
    {
       // ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        FillGrid(ucPagingControl1.CurrentPage);
    }

   
}