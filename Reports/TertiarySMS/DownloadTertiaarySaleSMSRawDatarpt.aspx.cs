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

public partial class Reports_TertiarySMS_DownloadTertiaarySaleSMSRawDatarpt : PageBase
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
                //using (TempClass objRD = new TempClass())
                {
                    strExportFileName = PageBase.importExportCSVFileName;
                    objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                    objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                    objRD.UserId = PageBase.UserId;
                    string FilenameToexport = "TertiarySMSSaleRawData";
                    DataSet ds = new DataSet();
                    ds = objRD.GetTertiarySMSSaleRawData();
                    if (objRD.Result == 0)
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    else if (objRD.Result == 1)
                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                    else
                        ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
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
