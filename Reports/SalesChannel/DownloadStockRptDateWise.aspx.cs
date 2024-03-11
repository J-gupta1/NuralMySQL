using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;

public partial class Reports_SalesChannel_DownloadStockRptDateWise : PageBase
{
    string strExportFileName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {
                ucDateFrom.Date = PageBase.Fromdate;
                ucDateTo.Date = PageBase.ToDate;
                
               
                FillsalesChannelType();
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString(),PageBase.GlobalErrorDisplay());
        }
    }
   
    protected void btnSerch_Click(object sender, EventArgs e)
    {
        try
        {

            if (cmbSalesChannelType.SelectedValue == "0")
            {
                ucMessage1.ShowInfo(Resources.Messages.MandatoryField);
                return;
            }
            using (ReportData objRD = new ReportData())
            {
                Int32 intResult = 1;
                //string[] strFilePath = PageBase.strBCPFilePath.Split(new char[] { '\\' });
                //string path = strFilePath[4].ToString();  Pankaj Dhingra
                strExportFileName = PageBase.importExportCSVFileName;
                objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                objRD.UserId = PageBase.UserId;
                objRD.SalesChannelID = Convert.ToInt32(hdnReportForSalesChannelid.Text);
                objRD.SalesChannelTypeID = Convert.ToInt16(cmbSalesChannelType.SelectedValue);
                //objRD.ModelId = Convert.ToInt32(ddlModelName.SelectedValue);
                //objRD.SkuId = Convert.ToInt32(ddlSku.SelectedValue);
                objRD.FilePath = "DateWiseStockReport" + strExportFileName;   //Pankaj Dhingra
                //objRD.OrgHierarchyId = Convert.ToInt32(ddllocation.SelectedValue);
                //objRD.intWantZeroQuantity = chkZeroQuantity.Checked ? Convert.ToInt16(1) : Convert.ToInt16(0);
                //objRD.FilePath = PageBase.strBCPFilePath + strExportFileName; Pankaj Dhingra
                //objRD.stateid = Convert.ToInt32(ddlState.SelectedValue);
                //objRD.ProductCategtoryid = Convert.ToInt32(ddlProductCategory.SelectedValue);
                //if (ddlType.SelectedValue == "101")
               // {
               //     objRD.OtherEntityType = PageBase.OtherEntityType;
               //     intResult = objRD.GetStockReportExcelbybcpRetailer();
               // }
               // else
               // {
               //     intResult = objRD.GetStockReportExcelbybcp();
               // }

                intResult = objRD.GetStockReportDateWiseExcelbybcp();

                if (intResult == 0)
                {
                    Response.Redirect((siteURL + strBCPFilePath + objRD.FilePath));
                    //string filePath = "../../" + PageBase.strGlobalDownloadExcelPathRoot + path + "/" + strExportFileName;
                    //HttpContext.Current.Response.Clear();
                    //HttpContext.Current.Response.Charset = "";
                    //HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=StockReport" + strExportFileName);
                    //HttpContext.Current.Response.ContentType = "application/vnd.csv";
                    //PageBase.ClearBuffer();
                    //HttpContext.Current.Response.WriteFile(filePath);
                    //HttpContext.Current.Response.End();
                }
                else if (intResult == 1)
                {
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);

                }
                else if (intResult == 3)
                {
                    ucMessage1.ShowWarning(objRD.error);

                }
                else
                {
                    ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);

                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
        
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;
       
        
        cmbSalesChannelType.SelectedValue = "0";
        ucDateFrom.Date = "";
        ucDateTo.Date = "";
        txtsaleschannel.Text = "";
       
    }
   
   
   
    bool isvalidate()
    {
        
            if (cmbSalesChannelType.SelectedValue == "0")
            {
                ucMessage1.ShowInfo("Please enter sales channel type");
                return false;
            }
              
            if (ucDateFrom.Date == "" && ucDateTo.Date == "")
            {
                ucMessage1.ShowInfo("Please enter date");
                return false;
            }

            if (ucDateFrom.Date == "" && ucDateTo.Date != "")
            {
                ucMessage1.ShowInfo("Please select from date ");
                return false;
            }

            if (ucDateFrom.Date != "" && ucDateTo.Date == "")
            {
                ucMessage1.ShowInfo("Please select to date");
                return false;
            }
       
            if (Convert.ToDateTime(ucDateFrom.Date) > Convert.ToDateTime(ucDateTo.Date))
            {
                ucMessage1.ShowInfo("The from date should be less than or equal to  To date ");
                return false;
            }
     
        return true;

    }
    void FillsalesChannelType()
    {
        using (SalesChannelData obj = new SalesChannelData())
        {
            DataTable dt = obj.GetSalesChannelType();
          
            String[] colArray = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref cmbSalesChannelType, dt.DefaultView.ToTable(), colArray);

        }
    }
   
    protected void cmbSalesChannelType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        AutoCompleteExtender1.ContextKey = cmbSalesChannelType.SelectedValue;
        hdnReportForSalesChannelid.Text = "0";
        txtsaleschannel.Text = "";
    }
}
