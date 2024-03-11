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
//* Module       : Reports(Attendance)  
//* Description  :  This page is used for View Attendance reports 
//* ====================================================================================
/* Change Log
 * -------------------------------------------------------------------------------------
 * DD-MMM-YYYY, Name, #CCXX, Description
 01-may-2019 , Gaurav Tyagi,#CC01 Imagepath added in grid to download image
 
 */
public partial class Reports_Common_ViewAttendanceReport : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            FillEntityType();
           // SearchAttendanceDetailData(1);
            ucFromDate.Date = PageBase.Fromdate;
            ucToDate.Date = PageBase.ToDate;

        }
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
       
        SearchAttendanceDetailData(1);
    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        SearchAttendanceDetailData(-1);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ddlEntityType.SelectedValue = "0";
        if(ddlEntityType.SelectedValue == "0")
        {
            ddlEntityTypeName.SelectedValue = "0";
            ddlEntityTypeName.Items.Clear();
            ddlEntityTypeName.Items.Insert(0, new ListItem("Select","0"));
        }
        gvAttendanceDetail.DataSource = null;
        gvAttendanceDetail.DataBind();
        ucMessage1.Visible = true;
        PnlGrid.Visible = false;
        
    }
    void FillEntityType()
    {
        using (ClsPaymentReport ObjEntityType = new ClsPaymentReport())
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
    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEntityTypeName(Convert.ToInt32(ddlEntityType.SelectedValue));
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        SearchAttendanceDetailData(ucPagingControl1.CurrentPage);
    }
    public void SearchAttendanceDetailData(int pageno)
    {
        ClsPaymentReport objAttendance;
        try
        {
            ViewState["TotalRecords"] = 0;
            ViewState["CurrentPage"] = pageno;
            using (objAttendance = new ClsPaymentReport())
            {
                if (ucFromDate.Date == "" && ucToDate.Date == "")
                { ;}
                else
                {
                    objAttendance.FromDate = Convert.ToDateTime(ucFromDate.Date);
                    objAttendance.Todate = Convert.ToDateTime(ucToDate.Date);
                }
                objAttendance.UserId = PageBase.UserId;
                objAttendance.CompanyId = PageBase.ClientId;
                objAttendance.EntityTypeId = Convert.ToInt32(ddlEntityType.SelectedValue);
                objAttendance.EntitytypeUserId = Convert.ToInt16(ddlEntityTypeName.SelectedValue);
                objAttendance.PageIndex = pageno;
                objAttendance.PageSize = Convert.ToInt32(PageBase.PageSize);

                DataSet ds = objAttendance.GetReportAttendanceData();
                if (objAttendance.TotalRecords > 0)
                {
                    PnlGrid.Visible = true;
                    if (pageno > 0)
                    {
                        gvAttendanceDetail.DataSource = ds;
                        gvAttendanceDetail.DataBind();
                        PnlGrid.Visible = true;
                        ViewState["TotalRecords"] = objAttendance.TotalRecords;
                        ucPagingControl1.TotalRecords = objAttendance.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {

                        string FilenameToexport = "AttendanceDetailData";
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    }
                }
                else
                {
                    ds = null;
                    gvAttendanceDetail.DataSource = null;
                    gvAttendanceDetail.DataBind();
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
    protected void DownloadFile(object sender, EventArgs e)
    {
        try
        {
            string filePath = (sender as LinkButton).CommandArgument;
            string ExportFileLocation = HttpContext.Current.Server.MapPath("~");
            string fullpath = ExportFileLocation + "/" + filePath;
            Response.ContentType = "image/jpg";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fullpath));
            Response.WriteFile(fullpath);
            Response.End();
        }
        catch (Exception ex)
        {

        }

    }
    protected void gvAttendanceDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string IsPrint = gvAttendanceDetail.DataKeys[e.Row.RowIndex]["ImagePath"].ToString();
                LinkButton BtnPrint = (LinkButton)e.Row.FindControl("lnkDownload");
                if (IsPrint != "")
                {
                    BtnPrint.Visible = true;
                }
                else
                {
                    BtnPrint.Visible = false;

                }

            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
}