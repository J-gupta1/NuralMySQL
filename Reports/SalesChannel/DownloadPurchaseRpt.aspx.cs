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

/* ==============================================================================================================
 * Change Log
 * ==============================================================================================================
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 28-Aug-2018, Rakesh Raj, #CC04, Display Column Dynamically from database/Dynamic Header for hierarchy column names 
 * ==============================================================================================================
 */
public partial class Reports_SalesChannel_DownloadPurchaseRpt :PageBase
{
    string strExportFileName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;

        if (!IsPostBack)
        {
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;
        }

    } 
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
        if (pageValidate())
        {
            using (ReportData objRD = new ReportData())
            {
                Int32 intResult = 1;
                //string[] strFilePath = PageBase.strBCPFilePath.Split(new char[] { '\\' });
                //string path = strFilePath[4].ToString();
                strExportFileName = PageBase.importExportCSVFileName;
                objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                objRD.SalesChannelID = PageBase.SalesChanelID;
                //objRD.FilePath = PageBase.strBCPFilePath + strExportFileName;
                objRD.FilePath ="PurchaseReport"+strExportFileName;
                objRD.UserId = PageBase.UserId;
                objRD.BaseEntityTypeID = PageBase.BaseEntityTypeID;
                DataSet ds = new DataSet();
                if(chkSB.Checked)
                    ds = objRD.GetFlatPurchaseReportbybcpSB(out intResult);
                else
                    ds = objRD.GetFlatPurchaseReportbybcp(out intResult);
                if (intResult == 0)
                {
                    
                    //Response.Redirect((siteURL + strBCPFilePath + objRD.FilePath));
                 /*CC04*///  objRD.headerReplacement(ds.Tables[0]);
                    PageBase.ExportToExecl(ds, "PurchaseReport");
                    //string filePath = siteURL + PageBase.strGlobalDownloadExcelPathRoot  + strExportFileName;
                    //HttpContext.Current.Response.Clear();
                    //HttpContext.Current.Response.Charset = "";
                    //HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=PurchaseReport" + strExportFileName);
                    //HttpContext.Current.Response.ContentType = "application/vnd.csv";
                    //PageBase.ClearBuffer();
                    //HttpContext.Current.Response.WriteFile(filePath);
                    //HttpContext.Current.Response.End();
                }
                else if (intResult == 1)
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);

                }
                else
                {
                    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);

                }
            }
        }
        }
         catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
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
  
  
}
