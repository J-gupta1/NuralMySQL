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

public partial class Reports_SalesChannel_StockRptBcp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;

        if (!IsPostBack)
        {
            FillsalesChannelType();
            ucDateTo.Date = PageBase.ToDate;
        }
    }
    void FillsalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {


            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref ddlType, ObjSalesChannel.GetSalesChannelType(), str);
            if (PageBase.SalesChanelID != 0)
            {
                ddlType.SelectedValue = PageBase.SalesChanelTypeID.ToString();
                ddlType.Enabled = false;
            }
            else
            {
                ddlType.Enabled = true;
            }
        };
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
           
            if (ddlType.SelectedValue == "0")
            {
                ucMsg.ShowInfo(Resources.Messages.MandatoryField);
                return;
            }
            using (ReportData objRD = new ReportData())
            {
                Int32 intResult = 1;
                objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                objRD.UserId = PageBase.UserId;
                objRD.SalesChannelTypeID = Convert.ToInt16(ddlType.SelectedValue);
                intResult = objRD.GetStockReportExcelbybcp();
                if (intResult == 0)
                {
                    if(objRD.FilePath !="")
                    {
                         btnExportToExcel.Visible = true;
                         btnExportToExcel.NavigateUrl = PageBase.siteURL + "Excel/Download/BcpFile/StockReport" + PageBase.UserId + ".CSV";
                    }
                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.ErrorMsgTryAfterSometime);

                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
   
   
}
