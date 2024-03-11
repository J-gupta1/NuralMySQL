/*
============================================================================================================================================
Copyright	: Zed-Axis Technologies, 2015
Created By	: Sumit Maurya
Create date	: 14-Oct-2015
Description	: This interface is use to download/fetch Non Activated IMEIs.
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

public partial class Reports_Common_DownloadNonActivatedIMEI : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        if (!IsPostBack)
        {


        }
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
                    string FilenameToexport = "NonActivatedIMEIs";
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
    

  
}
