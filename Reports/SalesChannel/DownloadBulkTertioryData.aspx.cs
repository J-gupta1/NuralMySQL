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
/*REPORT NOT IN USE - CREATED SPECIFIC TO BEETEL
 */
public partial class Reports_SalesChannel_DownloadBulkTertioryData : System.Web.UI.Page
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
            DataSet DsTertioryInfo;
            if (pageValidate())
            {
                using (ReportData objRD = new ReportData())
                {
                    objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                    objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                    objRD.SalesChannelID = PageBase.SalesChanelID;
                    objRD.UserId = PageBase.UserId;
                    DsTertioryInfo = objRD.GetFlatBulkTertioryReport();
                    if (DsTertioryInfo.Tables[0].Rows.Count > 0)
                    {
                        fncExportToExcel(DsTertioryInfo);
                    }
                    else
                    {
                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
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
    private void fncExportToExcel(DataSet DS)
    {
        try
        {
            if (DS.Tables[0].Rows.Count > 0)
            {
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "TertioryReport";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExeclUsingOPENXMLV2(DS.Tables[0], FilenameToexport);
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
}
