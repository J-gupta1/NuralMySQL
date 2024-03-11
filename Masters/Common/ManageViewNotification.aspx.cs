using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Data;
using System.Configuration;

public partial class Masters_Common_ManageViewNotification : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ucDateFrom.Date = PageBase.Fromdate;
                ucDateTo.Date = PageBase.ToDate;
                pageValidate();
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            ucMsg.ShowError(ex.Message, GlobalErrorDisplay());
        }
    }
    bool pageValidate()
    {

        int _date = 0;
        if ((Convert.ToDateTime(ucDateFrom.Date) > DateTime.Now.Date) || (Convert.ToDateTime(ucDateTo.Date) > DateTime.Now.Date))
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

        return true;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
       
            BindNotification();
      
    }
    void BindNotification()
    {
        using (CommonMaster obj = new CommonMaster())
        {
            obj.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
            obj.DateTo = Convert.ToDateTime(ucDateTo.Date);
            obj.CallingMode = 0;
            obj.notificationId = 0;
            DataTable dt = obj.ViewNotification();
            if (dt.Rows.Count > 0)
            {
                grdNotification.DataSource = dt;
                grdNotification.DataBind();
            }
            else
            {
                grdNotification.DataSource = null;
                grdNotification.DataBind();


            }
            grdNotificationDetails.DataSource = null;
            grdNotificationDetails.DataBind();
            grdNotificationDetails.Visible = false;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageViewNotification.aspx", false);
    }
    protected void grdNotification_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Notification")
        {
            try
            {
                ViewState["Index"] = e.CommandArgument;
                BindNotificationDetails();
            }
            catch (Exception ex)
            {

            }
        }
    }
    void BindNotificationDetails()
    {
        using (CommonMaster obj = new CommonMaster())
        {
            obj.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
            obj.DateTo = Convert.ToDateTime(ucDateTo.Date);
            obj.CallingMode = 1;
            obj.notificationId = Convert.ToInt32(ViewState["Index"]);
            DataTable dt = obj.ViewNotification();
            if (dt.Rows.Count > 0)
            {
                grdNotificationDetails.DataSource = dt;
                grdNotificationDetails.DataBind();
                grdNotificationDetails.Visible = true;
            }
            else
            {
                grdNotificationDetails.DataSource = null;
                grdNotificationDetails.DataBind();
                grdNotificationDetails.Visible = false;
            }
        }
    }
    protected void grdNotification_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdNotification.PageIndex = e.NewPageIndex;
        BindNotification();
    }
    protected void grdNotificationDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdNotificationDetails.PageIndex = e.NewPageIndex;
        BindNotificationDetails();
    }
}
