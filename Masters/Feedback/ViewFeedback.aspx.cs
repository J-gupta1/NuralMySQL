/*
====================================================================================================================================
Copyright	: Zed-Axis Technologies, 2016
Created By	: Sumit Maurya
Create date	: 16-Mar-2016
Description	: This interface is created to reply on feedback.
====================================================================================================================================
Change Log:
DD-MMM-YYYY, Name , #CCXX - Description
 * 29-Mar-2016, Sumit Maurya, #CC01, New html encoding added to prevent html scripting.
------------------------------------------------------------------------------------------------------------------------------------
 */

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


public partial class Masters_Feedback_ViewFeedback : PageBase //System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;

        if (!IsPostBack)
        {
            BindDates();
            /* BindFeedBack(1);*/
        }
    }

    public void BindDates()
    {
        DateTime currDate = DateTime.Now;
        DateTime FirstDayOfMonth = new DateTime(currDate.Year, currDate.Month, 1);
        ucFromDate.Date = FirstDayOfMonth.ToShortDateString();
        UcToDate.Date = currDate.ToShortDateString();
    }
    public void BindFeedBack(int index)
    {
        try
        {
            updFeedbackgrd.Update();
            Feedback objfeedback = new Feedback();
            ViewState["TotalRecords"] = 0;
            index = index == 0 ? 1 : index;
            objfeedback.PageSize = Convert.ToInt32(PageSize);
            objfeedback.PageIndex = index;
            btnCancel_Click(null, null);
            objfeedback.FromDate = Convert.ToDateTime(ucFromDate.Date);
            objfeedback.ToDate = Convert.ToDateTime(UcToDate.Date);
            objfeedback.FilterType = Convert.ToInt16(ddlDateFilterType.SelectedValue);

            DataSet ds = new DataSet();
            objfeedback.FeedbackStatus = Convert.ToInt16(ddlFeedbackStatus.SelectedValue);
            objfeedback.web_user_id = Convert.ToInt32(Session["UserID"]);
            ds = objfeedback.GetFeedback();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    GvViewFeedback.Visible = true;
                    dvFooter.Visible = true;
                    ViewState["TotalRecords"] = objfeedback.TotalRecords;
                    ucPagingControl1.TotalRecords = objfeedback.TotalRecords;
                    ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                    ucPagingControl1.SetCurrentPage = index;
                    ucPagingControl1.FillPageInfo();
                    GvViewFeedback.DataSource = ds.Tables[0];
                    GvViewFeedback.DataBind();
                    dvhide.Visible = true;

                }
                else
                {
                    ucMessage1.ShowInfo("No Record Found.");
                    GvViewFeedback.Visible = false;
                    dvFooter.Visible = false;
                    dvhide.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindFeedBack(1);
    }

    protected void GvViewFeedback_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblStatus = (Label)e.Row.FindControl("lblFeedbackStatus");
            Button btnrevert = (Button)e.Row.FindControl("btnRevert");
            if (Convert.ToInt32(Session["RoleID"]) == 1)
            {

                if (lblStatus.Text.ToLower() != "new")
                {
                    btnrevert.Visible = false;
                }
            }
            else
            {
                btnrevert.Visible = false;
            }
        }

    }
    protected void GvViewFeedback_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "cmdEdit")
        {
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            if (ViewState["PreviousRowIndex"] != null)
            {
                var previousRowIndex = (int)ViewState["PreviousRowIndex"];
                GridViewRow PreviousRow = GvViewFeedback.Rows[previousRowIndex];
                PreviousRow.BackColor = System.Drawing.Color.White;
            }
            row.BackColor = System.Drawing.Color.LightGray;
            ViewState["PreviousRowIndex"] = row.RowIndex;
            dvUpdFeedback.Attributes.CssStyle.Add("display", "block");
            hdnFeedbackID.Value = e.CommandArgument.ToString();
            updFeedbackgrd.Update();

        }

    }
    protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            Feedback objfeedback = new Feedback();
            ViewState["TotalRecords"] = 0;
            objfeedback.PageSize = Convert.ToInt32(PageSize);
            objfeedback.PageIndex = -1;
            btnCancel_Click(null, null);
            DataSet ds = new DataSet();

            objfeedback.FromDate = Convert.ToDateTime(ucFromDate.Date);
            objfeedback.ToDate = Convert.ToDateTime(UcToDate.Date);
            objfeedback.FeedbackStatus = Convert.ToInt16(ddlFeedbackStatus.SelectedValue);
            objfeedback.FilterType = Convert.ToInt16(ddlDateFilterType.SelectedValue);
            objfeedback.web_user_id = Convert.ToInt32(Session["UserID"]);
            ds = objfeedback.GetFeedback();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string[] DsCol = new string[] { "FeedbackText", "FeedbackRevertText", "FeedbackRevertDate", "FeedbackStatus", "FeedbackCreatedBy", "CreatedOn" };
                    DataTable dtNew = new DataTable();
                    dtNew = ds.Tables[0].DefaultView.ToTable(true, DsCol);
                    if (dtNew.Rows.Count > 0)
                    {
                        DataSet dtcopy = new DataSet();
                        dtcopy.Merge(dtNew);
                        dtcopy.Tables[0].AcceptChanges();
                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "Feedback";
                        PageBase.RootFilePath = FilePath;
                        PageBase.ExportToExecl(dtcopy, FilenameToexport);
                    }

                }
                else
                {
                    ucMessage1.ShowInfo("No Record Found.");
                    GvViewFeedback.Visible = false;
                    dvFooter.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void ucPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        BindFeedBack(ucPagingControl1.CurrentPage);
    }
    protected void btnRevertFeedback_Click(object sender, EventArgs e)
    {
        try
        {
            Feedback objfeedback = new Feedback();
            objfeedback.FeedbackRevertText = txtRevertfeedback.TextBoxText.Trim();
            objfeedback.FeedbackID = Convert.ToInt32(hdnFeedbackID.Value);
            objfeedback.web_user_id = Convert.ToInt32(Session["UserID"]);
            int result = objfeedback.UpdateFeedback();
            if (result == 0)
            {
                ucMessage1.ShowSuccess("Feedback reverted successfully.");
                txtRevertfeedback.TextBoxText = string.Empty;
                btnCancel_Click(null, null);
                ucPagingControl1_SetControlRefresh();
            }
            else
            {
                ucMessage1.ShowInfo("Error in reverting feedback.");
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            dvUpdFeedback.Attributes.CssStyle.Add("display", "none");
            updFeedbackgrd.Update();

        }
        catch (Exception ex)
        {

        }
    }
}
