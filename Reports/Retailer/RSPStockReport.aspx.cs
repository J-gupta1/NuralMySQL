#region NameSpace
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
using DevExpress.Web.ASPxPivotGrid;
using System.Configuration;
#endregion

public partial class Reports_Retailer_RSPStockReport : PageBase
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

    protected void exportToExel_Click(object sender, EventArgs e)
    {
        try
        {

            if (validation())
            {
                //{
                //    ucMsg.ShowInfo(Resources.Messages.MandatoryField);
                //    return;
                //}
                using (ReportData objRD = new ReportData())
                //using (TempClass objRD = new TempClass())
                {
                    Int32 intResult = 1;

                    strExportFileName = PageBase.importExportCSVFileName;
                    objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                    objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                    objRD.UserId = PageBase.UserId;
                    objRD.SalesChannelID = PageBase.SalesChanelID;
                    objRD.SalesChannelTypeID = PageBase.SalesChanelTypeID;
                    objRD.FilePath = "RSPStock" + strExportFileName;
                    objRD.ISPCode = ISPCode();
                    intResult = objRD.GetRSPStockReport();

                    if (intResult == 0)
                    {
                        Response.Redirect((siteURL + strBCPFilePath + objRD.FilePath));

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


    bool validation()
    {
        if (ucDateFrom.Date == "" & ucDateTo.Date != "")
        {
            ucMsg.ShowInfo(Resources.Messages.MandatoryField);
            return false;
        }
        if (ucDateFrom.Date != "" & ucDateTo.Date == "")
        {
            ucMsg.ShowInfo("Date is not in the proper format");
            return false;
        }
        if (ucDateTo.Date != "" & ucDateFrom.Date != "")
        {
            if (Convert.ToDateTime(ucDateTo.Date) < Convert.ToDateTime(ucDateFrom.Date))
            {
                ucMsg.ShowInfo("Date is not in the proper format");
                return false;
            }
        }
        return true;
    }


    string ISPCode()
    {
        string[] str = hdnISPCode.Value.Split('-');
        if (str.Length > 1)
        {
            return str[1];
        }
        else
            return "0";
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
        txtISPCode.Text = string.Empty;
    }
}
