using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;

public partial class Transactions_SalesChannel_PrimarySalesInterface : PageBase
{
    DateTime dt = new DateTime();
    string strSalesChannelName,strSalesChannelCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        strSalesChannelCode = hdnCode.Value;
        strSalesChannelName = hdnName.Value;
        txtSearchedName.Text = strSalesChannelName;
        
        btnsearch.Attributes.Add("onclick", "return popup();");
        try
        {
           dt = System.DateTime.Now.Date;
           ucDatePicker.MaxRangeValue = dt;
           ucDatePicker.RangeErrorMessage = "Invalid Date Range";
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                
                lblChange.Text = Resources.Messages.SalesEntity;
                lblChangeTo.Text = Resources.Messages.SalesEntity;
                BindWareshouse();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "1")
        {
            Response.Redirect("UploadPrimarySalesWithoutOrder.aspx");
        }
    }
    
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        PnlHide.Visible = true;
        txtSearchedName.Text = strSalesChannelName;
        btnsearch.Enabled = false;
        try
        {
            if (!pageValidateGo())
            {
                return;
            }
            using (SalesData objSales = new SalesData())
            {
                objSales.SalesChannelID = PageBase.SalesChanelID;
                objSales.InvoiceNumber = txtInvoiceNo.Text.Trim();
                objSales.SalesChannelCode = strSalesChannelCode;        //will get from the child page
                objSales.Brand = PageBase.Brand;
                objSales.FromSalesChannelCode = ddlWarehouse.SelectedValue;
                DataSet dsSales = objSales.GetPrimarySales1Sales();
                if (dsSales.Tables[0] != null && dsSales.Tables[0].Rows.Count > 0)
                {
                    PnlHide.Visible = false;
                    ucMsg.ShowInfo(Resources.Messages.SalesNotModify);
                    return;
                }
                if (dsSales.Tables[1] != null && dsSales.Tables[1].Rows.Count > 0)
                {
                    if (dsSales.Tables[1].Select("PrimarySalesID >0").Length > 0)
                    {
                        DataRow[] drowArray = dsSales.Tables[1].Select("PrimarySalesID > 0");
                        if (drowArray.Length > 0)
                        {
                            ddlWarehouse.Enabled = false;
                            txtInvoiceNo.Enabled = false;
                            ucDatePicker.TextBoxDate.Text = ((DataRow)drowArray.GetValue(0))["InvoiceDate"].ToString();
                            ucDatePicker.TextBoxDate.Enabled = false;
                            ucDatePicker.imgCal.Enabled = false;
                        }
                    }
                    else
                    {
                       ucDatePicker.imgCal.Enabled = true;
                        ucDatePicker.TextBoxDate.Enabled = true;
                        ddlWarehouse.Enabled = true;
                        txtInvoiceNo.Enabled = true;
                    }
                }
                else
                {
                    ucDatePicker.imgCal.Enabled = true;
                    ddlWarehouse.Enabled = true;
                    ucDatePicker.TextBoxDate.Enabled = true;
                    txtInvoiceNo.Enabled = true;
                }
                ucSalesEntryGrid1.Source = dsSales.Tables[1];
                pnlGrid.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    bool pageValidateSave()
    {

        if (ddlWarehouse.SelectedIndex == 0 || txtInvoiceNo.Text.Trim() == string.Empty || ServerValidation.IsDate(ucDatePicker.Date, true) != 0)
        {
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }

        if (Convert.ToDateTime(ucDatePicker.Date) > DateTime.Now.Date)
        {
            ucMsg.ShowWarning(Resources.Messages.DateRangeValidation);
            return false;
        }

        return true;
    }
    bool pageValidateGo()
    {

        if (txtInvoiceNo.Text.Trim() == string.Empty)
        {
            ucMsg.ShowWarning(Resources.Messages.EnterInvoiceNo);
            return false;
        }
        return true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            int Result = 0;
            if (!pageValidateSave())
            {
                return;
            }
            DataTable DtDetail = new DataTable();
            using (CommonData ObjCommom = new CommonData())
            {
                DtDetail = ObjCommom.GettvpTablePrimarySalesNew1WithoutOrder();
            }
            ucSalesEntryGrid1.Salestype = EnumData.eControlRequestTypeForEntry.ePrimary1Sales;
            DataTable Dt = ucSalesEntryGrid1.ReturnGridSource();
            string SumQTY = Dt.Compute("sum(Quantity)", "1=1").ToString();

            if (Dt == null || Dt.Rows.Count == 0 || SumQTY == "0")
            {
                ucMsg.ShowInfo(Resources.Messages.Entersalesqty);
                return;
            }
            DataRow[] drowArray = Dt.Select("PrimarySalesID > 0");
            string PrimarySalesID = "0";
            if (drowArray.Length > 0)
            {
                PrimarySalesID = ((DataRow)drowArray.GetValue(0))["PrimarySalesID"].ToString();

            }
            foreach (DataRow dr in Dt.Rows)
            {
                DataRow drow = DtDetail.NewRow();
                drow[0] = PrimarySalesID;
                drow[1] = strSalesChannelCode;
                drow[2] = txtInvoiceNo.Text;
                drow[3] = ucDatePicker.Date;
                drow[4] = dr["SKUCode"].ToString();
                drow[5] = dr["Quantity"].ToString();
                drow[6] = "";
                drow[7] = PageBase.SalesChanelID;
                drow[8] = Convert.ToString(ddlWarehouse.SelectedItem.Value);
                DtDetail.Rows.Add(drow);
            }
            DtDetail.AcceptChanges();
            using (SalesData ObjSales = new SalesData())
            {
                ObjSales.Error = "";
                ObjSales.EntryType = EnumData.eEntryType.eInterface;
                ObjSales.UserID = PageBase.UserId;
                Result = ObjSales.InsertPrimarySales1Info(DtDetail);

                if (ObjSales.ErrorDetailXML != null && ObjSales.ErrorDetailXML != string.Empty)
                {
                    ucMsg.XmlErrorSource = ObjSales.ErrorDetailXML;
                    return;
                }
                else if (ObjSales.Error != null && ObjSales.Error != "")
                {
                    ucMsg.ShowError(ObjSales.Error);
                    return;
                }
                if (Result == 2)
                {
                    ucMsg.ShowSuccess("Duplicate Invoice Number");
                    return;
                }

                ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                pnlGrid.Visible = false;
                ClearForm();
                btnsearch.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    void ClearForm()
    {
        ddlWarehouse.ClearSelection();
        ddlWarehouse.SelectedValue = "0";
        txtInvoiceNo.Text = "";
        ucDatePicker.Date = "";
        ucDatePicker.imgCal.Enabled = true;
        ddlWarehouse.Enabled = true;
        ucDatePicker.TextBoxDate.Enabled = true;
        pnlGrid.Visible = false;
        PnlHide.Visible = false;
        txtInvoiceNo.Enabled = true;
        txtSearchedName.Text = "";
        if (PageBase.SalesChanelID != 0)
        {

            ddlWarehouse.SelectedValue = PageBase.SalesChanelCode.ToString();
            ddlWarehouse.Enabled = false;

        }
        else
        {
            ddlWarehouse.ClearSelection();
            ddlWarehouse.SelectedValue = "0";
            ddlWarehouse.Enabled = true;

        }
        }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();
        btnsearch.Enabled = true;
    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {

        }
    }
    void BindWareshouse()
    {
        DataSet dsWarehosue = new DataSet();
        try
        {
            using (SalesChannelData objSalesData = new SalesChannelData())
            {
                objSalesData.UserID = PageBase.UserId;
                objSalesData.ReqType = EnumData.eControlRequestTypeForEntry.ePrimary1Sales;
                objSalesData.SalesChannelID = PageBase.SalesChanelID;
                objSalesData.Brand = PageBase.Brand;    //Pankaj Dhingra
                dsWarehosue = objSalesData.GetAllTemplateData();
                if (dsWarehosue != null && dsWarehosue.Tables.Count > 0)
                {
                    string[] str = { "SalesChannelCode", "SalesChannelName" };
                    PageBase.DropdownBinding(ref ddlWarehouse, dsWarehosue.Tables[0], str);
                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                }
                if (PageBase.SalesChanelID == 0)
                {
                    ddlWarehouse.Enabled = true;
                    ddlWarehouse.SelectedValue = "0";
                }
                else
                {
                    ddlWarehouse.SelectedValue = PageBase.SalesChanelCode;
                    ddlWarehouse.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
}
