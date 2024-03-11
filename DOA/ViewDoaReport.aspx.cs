/*
 * Change Log
 * 17-Jul-2018, Sumit Maurya, #CC01, report data gets downloaded according to config (Done for Comio).
 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using System.Data;

public partial class DOA_ViewDoaDispatches : PageBase
{
    clsDoaReport objreport = new clsDoaReport();
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            BindDOAStatus();
            BindStockReceiveStatus();
            divupmessage.Visible = false;
            ucFromDate.Date = PageBase.Fromdate;
            ucToDate.Date = PageBase.ToDate;
        }
    }
    protected void Search_Click(object sender, EventArgs e)
    {
        try
        {
            divupmessage.Visible = false;

            if ((txtDoaCertificateno.Text.Trim() == "" || txtDoaCertificateno.Text.Trim() == null) && (txtIMEINo.Text.Trim() == "" || txtIMEINo.Text.Trim() == null) && (ddldoastatus.SelectedValue == "0") && (ucFromDate.Date == "") && (ucToDate.Date == "") && (ddlStockReceiveStatus.SelectedValue=="255"))
            {
                divupmessage.Visible = true;
                ucMessage1.ShowWarning("Select OR Enter any search criteria!!!");

                return;
            }
            else
            {
                divupmessage.Visible = false;
                bindDOAData();
            }
        }
        catch (Exception)
        {

        }

    }
    protected void Cancel_Click(object sender, EventArgs e)
    {
        Clear();
    }
    private void BindDOAStatus()
    {
        DataTable dtresult = new DataTable();
        try
        {
            dtresult = clsDoaReport.GetEnumbyTableName("XML_Enum", "DOAStatus");
            if (dtresult.Rows.Count > 0)
            {
                ddldoastatus.DataSource = dtresult;
                ddldoastatus.DataTextField = "Description";
                ddldoastatus.DataValueField = "Value";
                ddldoastatus.DataBind();
                ddldoastatus.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddldoastatus.Items.Insert(0, new ListItem("Select", "0"));
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    private void BindStockReceiveStatus()
    {
        DataTable dtresult = new DataTable();
        try
        {
            dtresult = clsDoaReport.GetEnumbyTableName("XML_Enum", "StockReceiveType");
            if (dtresult.Rows.Count > 0)
            {
                ddlStockReceiveStatus.DataSource = dtresult;
                ddlStockReceiveStatus.DataTextField = "Description";
                ddlStockReceiveStatus.DataValueField = "Value";
                ddlStockReceiveStatus.DataBind();
                ddlStockReceiveStatus.Items.Insert(0, new ListItem("Select", "255"));
            }
            else
            {
                ddlStockReceiveStatus.Items.Insert(0, new ListItem("Select", "255"));
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    private DataSet bindDOAData()
    {
        DataSet ds = new DataSet();
        try
        {

            if (ucFromDate.Date == "" && ucToDate.Date == "")
            { ;}
            else
            {
                objreport.RequestDateFrom = Convert.ToDateTime(ucFromDate.Date);
                objreport.RequestDateTo = Convert.ToDateTime(ucToDate.Date);
            }
            if (txtIMEINo.Text.Trim() != "" && txtIMEINo.Text.Trim() != null)
            {
                objreport.IMEINumber = txtIMEINo.Text.Trim();
            }
            if (txtDoaCertificateno.Text.Trim() != "" && txtDoaCertificateno.Text.Trim() != null)
            {
                objreport.DOACertificateNumber = txtDoaCertificateno.Text.Trim();
            }
            objreport.DOAStatus = Convert.ToInt32(ddldoastatus.SelectedValue);
            objreport.SalesChannelId = PageBase.SalesChanelID;
            objreport.LoginUserId = PageBase.UserId;
            objreport.Stockreceivestatus = Convert.ToInt16(ddlStockReceiveStatus.SelectedValue);


            /* #CC01 Add Start */
            Int16 intViewDOAReport = 0;
            if(Resources.AppConfig.ViewDOAReport.ToString()!=null )
            {
                intViewDOAReport = Convert.ToInt16(Resources.AppConfig.ViewDOAReport.ToString());
            }
            if (intViewDOAReport == 0 || intViewDOAReport == 1)
            {
                ds = objreport.GetReportDOA();
            }
            else if (intViewDOAReport == 2)
            {
                ds = objreport.GetReportDOAWithSTN();
            }
                /* #CC01 Add End*/
            /*ds = objreport.GetReportDOA(); #CC01 Commented */
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                divupmessage.Visible = false;
                string FilenameToexport = "DOAReports";
                PageBase.ExportToExecl(ds, FilenameToexport);
            }
            else
            {

                divupmessage.Visible = true;
                ucMessage1.ShowInfo("NO Record Found.");

            }
            return ds;
        }
        catch (Exception ex)
        {
            return ds;
        }

    }
    private void Clear()
    {
        txtDoaCertificateno.Text = string.Empty;
        txtIMEINo.Text = string.Empty;
        ddldoastatus.SelectedValue = "0";
        ucFromDate.Date = "";
        ucToDate.Date = "";
        ucMessage1.Visible = false;
        ucFromDate.Date = PageBase.Fromdate;
        ucToDate.Date = PageBase.ToDate;
    }
}