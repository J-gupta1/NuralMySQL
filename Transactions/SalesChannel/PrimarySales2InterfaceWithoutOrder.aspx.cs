using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
public partial class Transactions_SalesChannel_PrimarySales2Interface : PageBase
{
    DateTime dt = new DateTime();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Pankaj Kumar
            dt = System.DateTime.Now.Date;      
            //ucDatePicker.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
            //Added By Mamta Singh for checking back date before opening date
            if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(dt.AddDays(PageBase.NumberofBackDaysAllowed)))
            {


                ucDatePicker.MinRangeValue = Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-PageBase.BackDaysAllowedBeforeOpening);
                lblValidationDays.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(dt.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
            }
            else
            {
                ucDatePicker.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
                lblValidationDays.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
            }
           
            ucDatePicker.MaxRangeValue = dt;
            ucDatePicker.RangeErrorMessage = "Invalid Date Range";
           // lblValidationDays.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
            //Pankaj Kumar
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                lblChange.Text = Resources.Messages.SalesEntity;
                FillsalesChannel();
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
            Response.Redirect("UploadPrimarySales2WithoutOrder.aspx");
        }
       // updMain.Update();
    }
    void FillsalesChannel()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(EnumData.eSalesChannelType.TD);
            ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
            ObjSalesChannel.SearchType = EnumData.eSearchConditions.Active;
            ObjSalesChannel.StatusValue = 1;//always active
            string[] str = { "SalesChannelCode", "DisplayName" };
            PageBase.DropdownBinding(ref ddlTD, ObjSalesChannel.GetSalesChannelInfo(), str);
        };
        //updMain.Update();
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        PnlHide.Visible = true;
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
                objSales.SalesChannelCode = ddlTD.SelectedValue;
                objSales.Brand = PageBase.Brand;
                DataSet dsSales = objSales.GetPrimarySales2Sales();
                if (dsSales.Tables[0] != null && dsSales.Tables[0].Rows.Count>0)
                {
                    PnlHide.Visible = false;
                    ucMsg.ShowInfo(Resources.Messages.SalesNotModify);
                    return;
                    

                }
                
                if (dsSales.Tables[1]!=null && dsSales.Tables[1].Rows.Count> 0)
                {
                    if (dsSales.Tables[1].Select("IntermediarySalesID >0").Length > 0)
                    {
                        DataRow[] drowArray = dsSales.Tables[1].Select("IntermediarySalesID > 0");
                        if (drowArray.Length > 0)
                        {
                          //  ddlTD.SelectedValue = ((DataRow)drowArray.GetValue(0))["SalesToId"].ToString();
                         
                            ddlTD.Enabled = false;

                            //txtSalesman.Text = ((DataRow)drowArray.GetValue(0))["Salesman"].ToString();
                            //txtSalesman.Enabled = false;
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
                        
                        //txtSalesman.Enabled = true;
                        ddlTD.Enabled = true;
                        txtInvoiceNo.Enabled = true;
                    }
                }
                else
                {
                  
                    ucDatePicker.imgCal.Enabled = true;
                    //txtSalesman.Enabled = true;
                    ddlTD.Enabled = true;
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

        if (ddlTD.SelectedIndex == 0 || txtInvoiceNo.Text.Trim() == string.Empty || ServerValidation.IsDate(ucDatePicker.Date, true) != 0 )
        {
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }

        if (Convert.ToDateTime(ucDatePicker.Date) > DateTime.Now.Date)
        {
            ucMsg.ShowWarning(Resources.Messages.DateRangeValidation);
            return false;
        }

        //Pankaj Kumar
        if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(ucDatePicker.Date))
        {
            ucMsg.ShowInfo("Invalid Date Range");
            return false;
        } //Pankaj Kumar

        return true;
    }
    bool pageValidateGo()
    {

        if (txtInvoiceNo.Text.Trim() == string.Empty )
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
                DtDetail = ObjCommom.GettvpTablePrimarySales2WitoutOrder();
            }
            ucSalesEntryGrid1.Salestype = EnumData.eControlRequestTypeForEntry.ePrimary2Sales;
           // string Error="";
            DataTable Dt = ucSalesEntryGrid1.ReturnGridSource();
            string SumQTY = Dt.Compute("sum(Quantity)", "1=1").ToString();

            if (Dt == null || Dt.Rows.Count == 0 || SumQTY == "0")
            {
                ucMsg.ShowInfo(Resources.Messages.Entersalesqty);
                return;
            }
            //if (Dt == null || Dt.Rows.Count == 0)
            //{
            //    ucMsg.ShowInfo(Resources.Messages.Entersalesqty);
            //    return;
            //}
            DataRow[] drowArray = Dt.Select("IntermediarySalesID > 0");
            string IntermediarySalesID = "0";
            if (drowArray.Length > 0)
            {
                IntermediarySalesID = ((DataRow)drowArray.GetValue(0))["IntermediarySalesID"].ToString();

            }
            foreach (DataRow dr in Dt.Rows)
            {
                DataRow drow = DtDetail.NewRow();
                drow[0] = IntermediarySalesID;
                drow[1] = Convert.ToString(ddlTD.SelectedItem.Value);
                drow[2] = txtInvoiceNo.Text;
                drow[3] = ucDatePicker.Date;
                drow[4] = dr["SKUCode"].ToString();
                drow[5] = dr["Quantity"].ToString();
                drow[6] = "";
                drow[7] = PageBase.SalesChanelID;
                DtDetail.Rows.Add(drow);
            }

            DtDetail.AcceptChanges();
            using (SalesData ObjSales = new SalesData())
            {

                ObjSales.Error = "";
                ObjSales.EntryType = EnumData.eEntryType.eInterface;
                ObjSales.UserID = PageBase.UserId;
                Result = ObjSales.InsertPrimarySales2Info(DtDetail);

                if (ObjSales.ErrorDetailXML != null && ObjSales.ErrorDetailXML != string.Empty)
                {
                    ucMsg.XmlErrorSource = ObjSales.ErrorDetailXML;
                    return;
                }
                else if (ObjSales.Error != null && ObjSales.Error != "" && ObjSales.Error != "0")
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

              // updGrid.Update();
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
      
        ddlTD.ClearSelection();
        ddlTD.SelectedValue = "0";
        //txtSalesman.Text = "";
        txtInvoiceNo.Text = "";
        ucDatePicker.Date = "";
        ucDatePicker.imgCal.Enabled = true;
        //txtSalesman.Enabled = true;
        ddlTD.Enabled = true;
        ucDatePicker.TextBoxDate.Enabled = true;
        pnlGrid.Visible = false;
        PnlHide.Visible = false;
        txtInvoiceNo.Enabled = true;
        
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl=false;
        ClearForm();
    }
   
    
}

