#region  NameSpace
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


public partial class Reports_SAP_DownloadSMSLogRpt : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        fncHide();
        if (!IsPostBack)
        {
           
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;
            SalesType();
        }

    }
    void SalesType()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlSalesType.Items.Clear();
            using (ReportData objSalesType = new ReportData())
            {
                objSalesType.SalesChannelTypeID = PageBase.SalesChanelTypeID;
                objSalesType.HierarchyLevelId = PageBase.HierarchyLevelID;
                dt = objSalesType.GetSalesTypeforReport();
            };
            String[] colArray = { "SalesTypeID", "SalesTypeName" };
            PageBase.DropdownBinding(ref ddlSalesType, dt, colArray);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
        DataSet DsSalesInfo;
        using (ReportData objRD = new ReportData())
        {
            if (pageValidate())
            {
                //EnumData.eReportType ReportType = (EnumData.eReportType)Enum.Parse(typeof(EnumData.eReportType), ddlSalesType.SelectedValue);
                //Int16 value = (Int16)ReportType;
                objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                objRD.SalesChannelID = PageBase.SalesChanelID;
                objRD.UserId = PageBase.UserId;
                objRD.SalesType = Convert.ToInt16(ddlSalesType.SelectedValue);
                DsSalesInfo = objRD.GetFlatSalesReport();
                if (DsSalesInfo.Tables[0].Rows.Count > 0)
                {
                    fncExportToExcel(DsSalesInfo);
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
    private void fncExportToExcel(DataSet DS)
    {
  
        if (DS.Tables[0].Rows.Count > 0)
        {
            DataTable DsCopy = new DataTable();
            DsCopy = DS.Tables[0];
            DS.Tables.Clear();
            DS.Tables.Add(DsCopy);
            DsCopy.Columns["HO"].ColumnName = Resources.SalesHierarchy.HierarchyName1;
            DsCopy.Columns["RBH"].ColumnName = Resources.SalesHierarchy.HierarchyName2;
            DsCopy.Columns["ZBH"].ColumnName = Resources.SalesHierarchy.HierarchyName3;
            DsCopy.Columns["SBH"].ColumnName = Resources.SalesHierarchy.HierarchyName4;
            DsCopy.Columns["ASO"].ColumnName = Resources.SalesHierarchy.HierarchyName5;
            String FilePath = Server.MapPath("../../");
            string FilenameToexport = "SalesReport";
            PageBase.RootFilePath = FilePath;
            PageBase.ExportToExeclUsingOPENXMLV2(DS.Tables[0], FilenameToexport);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
        ddlSalesType.SelectedIndex = 0;
    }
    private void fncHide()
    {
        ucMsg.ShowControl = false;
      
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
