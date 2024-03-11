﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;
using System.IO;
using Cryptography;
//======================================================================================
//* Developed By : Balram Jha 

//* Module       : Salesman visit summary.
//* Description  : Reports(number dealers visited  with how count of visits in a given time period). 
//* ====================================================================================
/* Change Log
 * -------------------------------------------------------------------------------------
 * DD-MMM-YYYY, Name, #CCXX, Description
 
 
 */

public partial class ReportsDealerVisitAnalysis : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            FillEntityType();
            ucFromDate.Date = PageBase.Fromdate;
            ucToDate.Date = PageBase.ToDate;
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
        SearchTravelClaimDetailData(1);
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
        gvVisitAnalysis.DataSource = null;
        gvVisitAnalysis.DataBind();
        ucMessage1.Visible = true;
        PnlGrid.Visible = false;
    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        SearchTravelClaimDetailData(-1);
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        SearchTravelClaimDetailData(ucPagingControl1.CurrentPage);
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
    public void SearchTravelClaimDetailData(int pageno)
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

                DataSet ds = objAttendance.GetReportDealerVisitAnalysis();
                if (objAttendance.TotalRecords > 0)
                {
                    PnlGrid.Visible = true;
                    if (pageno > 0)
                    {
                        gvVisitAnalysis.DataSource = ds;
                        gvVisitAnalysis.DataBind();
                        PnlGrid.Visible = true;
                        ViewState["TotalRecords"] = objAttendance.TotalRecords;
                        ucPagingControl1.TotalRecords = objAttendance.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {

                        string FilenameToexport = "DealerVisitAnalysis";
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    }
                }
                else
                {
                    ds = null;
                    gvVisitAnalysis.DataSource = null;
                    gvVisitAnalysis.DataBind();
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
    protected void gvVisitAnalysis_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Int64 SelectedTSMUserId = Convert.ToInt64(e.CommandArgument);
        Session["Mydata"] = null;
        string strCommandName = e.CommandName;
        string DetailType = "";
        LinkButton btn = (LinkButton)e.CommandSource;
        DataSet dsDetail = new DataSet();
        string strdrilldownURL = null;
        using (ClsPaymentReport objAttendance = new ClsPaymentReport())
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
            objAttendance.PageIndex = 0;
            objAttendance.PageSize = Convert.ToInt32(PageBase.PageSize);

            objAttendance.EntitytypeUserId = SelectedTSMUserId;

            strdrilldownURL = Server.UrlEncode(Crypto.Encrypt(Convert.ToString(SelectedTSMUserId), PageBase.KeyStr)).ToString().Replace("+", " ");
            if (strCommandName == "btnDealerCount" || strCommandName == "btnDealerCountTSM")
            {

                if ((SelectedTSMUserId > 0 && btn.Text != "0") || SelectedTSMUserId == 0)
                {
                    DetailType = "DealerDetail";
                }
            }
            if (strCommandName == "btnMoreThan5Visit" || strCommandName == "btnMoreThan5VisitTSM")
            {

                if ((SelectedTSMUserId > 0 && btn.Text != "0") || SelectedTSMUserId == 0)
                {
                    DetailType = "MoreThan5Visit";
                }
            }
            if (strCommandName == "btn5Visit" || strCommandName == "btn5VisitTSM")
            {

                if ((SelectedTSMUserId > 0 && btn.Text != "0") || SelectedTSMUserId == 0)
                {
                    DetailType = "5Visit";
                }
            }
            if (strCommandName == "btn4Visit" || strCommandName == "btn4VisitTSM")
            {

                if ((SelectedTSMUserId > 0 && btn.Text != "0") || SelectedTSMUserId == 0)
                {
                    DetailType = "4Visit";
                }
            }
            if (strCommandName == "btn3Visit" || strCommandName == "btn3VisitTSM")
            {

                if ((SelectedTSMUserId > 0 && btn.Text != "0") || SelectedTSMUserId == 0)
                {
                    DetailType = "3Visit";
                }
            }
            if (strCommandName == "btn2Visit" || strCommandName == "btn2VisitTSM")
            {

                if ((SelectedTSMUserId > 0 && btn.Text != "0") || SelectedTSMUserId == 0)
                {
                    DetailType = "2Visit";
                }
            }
            if (strCommandName == "btn1Visit" || strCommandName == "btn1VisitTSM")
            {

                if ((SelectedTSMUserId > 0 && btn.Text != "0") || SelectedTSMUserId == 0)
                {
                    DetailType = "1Visit";
                }
            }
            if (strCommandName == "btn0Visit" || strCommandName == "btn0VisitTSM")
            {

                if ((SelectedTSMUserId > 0 && btn.Text != "0") || SelectedTSMUserId == 0)
                {
                    DetailType = "0Visit";
                }
            }
            if (DetailType != "")
            {
                objAttendance.DetailDrillType = DetailType;
                dsDetail = objAttendance.GetReportDealerVisitAnalysisDrill();
                int c = dsDetail.Tables[0].Rows.Count;
                if (objAttendance.TotalRecords > 0)
                {
                    Session["Mydata"] = dsDetail;
                }
                else
                {
                    Session["Mydata"] = null;

                }
                string message = "popup('" + strdrilldownURL + "')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "Popup", message, true);
                // PageBase.ExportToExecl(dsDetail, DetailType);
            }
        }
    }
}
