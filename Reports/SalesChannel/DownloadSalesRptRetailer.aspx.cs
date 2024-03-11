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
public partial class Reports_SalesChannel_DownloadSalesRptRetailer : PageBase
{
    string strExportFileName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        fncHide();
        if (!IsPostBack)
        {

            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;
            SalesType();
            ddlSalesType.Enabled=false;
            BindModel();
            ddlSku.Items.Insert(0, new ListItem("Select", "0"));
        }

    }
    void SalesType()
    {
        try
        {
            if (PageBase.SalesChanelID == 0 & PageBase.BaseEntityTypeID == 3)
            {
                ddlSalesType.Items.Clear();
                ddlSalesType.Items.Insert(0, new ListItem("TertiarySales", "101"));
            }

            else
            {
                ddlSalesType.Items.Add(new ListItem("TertiarySales", "101"));

            }

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

            using (ReportData objRD = new ReportData())
            {
                if (pageValidate())
                {
                    Int32 intResult = 1;
                    strExportFileName = PageBase.importExportCSVFileName;
                    objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                    objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                    objRD.SalesChannelID = PageBase.SalesChanelID;
                    objRD.UserId = PageBase.UserId;
                    objRD.SalesType = Convert.ToInt16(ddlSalesType.SelectedValue);
                    objRD.FilePath = "RetailerSalesReport" + strExportFileName;
                    objRD.ModelId = Convert.ToInt32(ddlModelName.SelectedValue);
                    objRD.SkuId = Convert.ToInt32(ddlSku.SelectedValue);
                    if (ddlSalesType.SelectedValue == "101")
                    {
                        objRD.BaseEntityTypeID = PageBase.BaseEntityTypeID;
                        if(ChkSB.Checked)
                        intResult = objRD.GetFlatSalesReportbybcpRetailerSB();
                        else
                            intResult = objRD.GetFlatSalesReportbybcpRetailer();
                    }
                   
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
   
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
        ddlSalesType.SelectedIndex = 0;
        ddlModelName.SelectedValue = "0";
        ddlSku.ClearSelection();
        ddlSku.Items.Clear();
        ddlSku.Items.Insert(0, new ListItem("Select", "0"));
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
    void BindModel()
    {
        using (ProductData objproduct = new ProductData())
        {
            objproduct.ModelProdCatId = 0;
            objproduct.ModelSelectionMode = 1;
            DataTable dtmodelfil = objproduct.SelectModelInfo();
            String[] colArray1 = { "ModelID", "ModelName" };
            PageBase.DropdownBinding(ref ddlModelName, dtmodelfil, colArray1);


        }
    }

    void BindSku()
    {
        using (RetailerData objsku = new RetailerData())
        {
            objsku.ModelId = Convert.ToInt32(ddlModelName.SelectedValue);
            DataTable dtmodelfil = objsku.GetAllActiveSKU();
            String[] colArray1 = { "Skuid", "SkuName" };
            PageBase.DropdownBinding(ref ddlSku, dtmodelfil, colArray1);


        }
    }
    protected void ddlModelName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlModelName.SelectedValue == "0")
            {
                ddlSku.Items.Clear();
                ddlSku.Items.Insert(0, new ListItem("Select", "0"));
                ddlSku.SelectedValue = "0";
            }
            else
            {
                BindSku();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
}
