/*
============================================================================================================================================
Copyright	: Zed-Axis Technologies, 2015
Created By	: Sumit Maurya
Create date	: 19-Oct-2015
Description	: This interface is use to download/fetch Activated IMEIs whos sales record is missing.
Module      : Report
============================================================================================================================================
Change Log:
dd-MMM-yy, Name , #CCxx - Description
--------------------------------------------------------------------------------------------------------------------------------------------

 */

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

public partial class Reports_Common_ActivatedIMEISalesRecordMissing : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        ucMessage1.Attributes.CssStyle.Add("display", "none");
        if (!IsPostBack)
        {
            bindSaleChannel();
            BindDates();

        }
    }
    public void BindDates()
    {
        try
        {
            ucDateTo.Date = PageBase.ToDate;
            ucDateFrom.Date = PageBase.Fromdate;
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    public void bindSaleChannel()
    {
        try
        {
            DataSet ds = new DataSet();
            using (ReportData objReportData = new ReportData())
            {
                objReportData.UserId = PageBase.UserId;
                ds = objReportData.GetSalesChannel();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlSaleChannal.DataSource = ds.Tables[0];
                        ddlSaleChannal.DataTextField = "SalesChannelName";
                        ddlSaleChannal.DataValueField = "SalesChannelID";
                        ddlSaleChannal.DataBind();
                        ddlSaleChannal.Items.Insert(0, new ListItem("Select", "0"));
                        //if (PageBase.RoleID == 1)
                        //{
                        //    ddlSaleChannal.Items.Insert(0, new ListItem("Select", "0"));
                        //}
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToDateTime(ucDateFrom.Date) > Convert.ToDateTime(ucDateTo.Date))
            {
                ucMessage1.ShowWarning("Select valid date range");
                return;
            }
            DateTime startdate = Convert.ToDateTime(ucDateFrom.Date);
            DateTime enddate = Convert.ToDateTime(ucDateTo.Date);
            TimeSpan result;
            result = enddate - startdate;
            if (Convert.ToInt32(result.Days) > 365)
            {
                ucMessage1.ShowInfo("Date range should not be greater than one year");
                return;
            }
            else
            {
                DataSet ds = new DataSet();
                using (ReportData objReportData = new ReportData())
                {
                    objReportData.SalesChannelID = Convert.ToInt32(ddlSaleChannal.SelectedValue);
                    objReportData.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                    objReportData.DateTo = Convert.ToDateTime(ucDateTo.Date);
                    ds = objReportData.GetActivatedIMEISalesRecodMissing();
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ucMessage1.Visible = false;
                            String FilePath = Server.MapPath("../../");
                            string FilenameToexport = "ActivatedIMEISalesRecordsMissing";
                            PageBase.RootFilePath = FilePath;
                            //PageBase.ExportToExeclV2(ds, FilenameToexport);
                            PageBase.ExportToExecl(ds, FilenameToexport);
                        }
                    }
                    else
                    {
                        ucMessage1.ShowInfo("No Record Found");
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }


    #region ExtraCode
    /*
    public DataSet BreakTable(DataTable DtAllRecords, int TotalRecods, int SplitRecords, string UniqueRow)
    {
        DataSet dsTemp = new DataSet();
        DataTable TempTables = new DataTable();
        int Count = 0;
        int TableNo = 0;
        int StartCount = 0;
        int FinishCount = SplitRecords;
        while (Count < TotalRecods)
        {
            TableNo++;
            TempTables = DtAllRecords.Select(UniqueRow + " >" + StartCount + "AND " + UniqueRow + " <=" + FinishCount).CopyToDataTable();
            TempTables.TableName = "Table_" + TableNo;
            dsTemp.Tables.Add(TempTables);
            Count = FinishCount;
            StartCount = StartCount + SplitRecords;
            FinishCount = FinishCount + SplitRecords;
        }
        return dsTemp;
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            int TotalRecords;
            int SplitByBatch = 1000000;
            using (TempClass objTemp = new TempClass())
            {

                dt = objTemp.GetNonActivatedIMEI();
                TotalRecords = objTemp.TotalRecords;
                if (dt.Rows.Count > 0)
                {
                    ucMessage1.Visible = false;
                    ds = BreakTable(dt, TotalRecords, SplitByBatch, "Row");

                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ActivatedIMEISalesRecordsMissing";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExeclV2(ds, FilenameToexport);
                    //PageBase.ExportToExecl(dtcopy, FilenameToexport);
                }
                else
                {
                    ucMessage1.ShowInfo("No Record Found");
                }

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
     * */

    #endregion ExtraCode
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ActivatedIMEISalesRecordMissing.aspx", false);
    }
}
