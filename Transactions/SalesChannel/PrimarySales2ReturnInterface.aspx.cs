using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
public partial class Transactions_SalesChannel_PrimarySales2ReturnInterface : PageBase
{
    DateTime dt = new DateTime();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
        
            if (Convert.ToInt32(PageBase.SalesChanelTypeID) == 6)
            {

                dt = System.DateTime.Now.Date;
                //ucDatePicker.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
                //Added By Mamta Singh for checking back date before opening date
                if (Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening) >= Convert.ToDateTime(dt.AddDays(PageBase.NumberofBackDaysAllowed)))
                {


                    ucDatePicker.MinRangeValue = Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-PageBase.BackDaysAllowedBeforeOpening);
                    lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", (Convert.ToDateTime(dt.ToShortDateString()) - Convert.ToDateTime(Convert.ToDateTime(SalesChannelOpeningStockDate).AddDays(-BackDaysAllowedBeforeOpening).ToShortDateString())).TotalDays.ToString());
                }
                else
                {
                    ucDatePicker.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
                    lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
                }
           
                ucDatePicker.MaxRangeValue = dt;
                ucDatePicker.RangeErrorMessage = "Invalid Date Range";
               // lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
                lblInfo.Visible = true;
            }
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                lblChange.Text = Resources.Messages.SalesEntity;
                FillsalesChannel();
                lblInvoiceDate.Date = "";
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
            
        }

    } 
    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "1")
        {
            Server.Transfer("PrimarySalesReturnUpload.aspx");
        }

    }

    void FillsalesChannel()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            //ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(EnumData.eSalesChannelType.TD);
            ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
            ObjSalesChannel.SearchType = EnumData.eSearchConditions.Active;
            ObjSalesChannel.BlnShowDetail = false;
            string[] str = { "SalesChannelCode", "DisplayName" };
            PageBase.DropdownBinding(ref ddlTD, ObjSalesChannel.GetSalesChannelInfo(), str);
        };
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
                objSales.ReturnDate = Convert.ToDateTime(ucDatePicker.Date);
                objSales.Brand = PageBase.Brand;
                DataTable dtSales = objSales.GetPrimarySales2Return();

                if (dtSales.Rows.Count > 0)
                {
                    if (dtSales.Select("IntermediarySalesID >0").Length > 0)
                    {
                        DataRow[] drowArray = dtSales.Select("IntermediarySalesID > 0");
                        if (drowArray.Length > 0)
                        {
                           
                            ddlTD.Enabled = false;
                            txtInvoiceNo.Enabled = false;
                            lblInvoiceDate.Date = Convert.ToDateTime ( ((DataRow)drowArray.GetValue(0))["InvoiceDate"].ToString()).ToString ();
                            lblInvoiceDate.TextBoxDate.Enabled = false;
                            lblInvoiceDate.imgCal.Enabled = false;
                        }
                       
                    }
                    else
                    {
                        ddlTD.Enabled = true;
                        txtInvoiceNo.Enabled = true;
                        lblInvoiceDate.Date = "";
                        lblInvoiceDate.TextBoxDate.Text = "";
                        lblInvoiceDate.TextBoxDate.Enabled = true;
                        lblInvoiceDate.imgCal.Enabled = true;

                    }
                    if (dtSales.Select("IntermediarySalesReturnID >0").Length > 0)
                    {
                        ucDatePicker.imgCal.Enabled = false;
                        ucDatePicker.TextBoxDate.Enabled = false;
                    }
                    else
                    {
                        ucDatePicker.imgCal.Enabled = true;
                        ucDatePicker.TextBoxDate.Enabled = true;
                    }
                    if (dtSales.Select("InvoiceNumber <>' '").Length > 0)
                    {
                        DataRow[] drowArray1 = dtSales.Select("InvoiceNumber <> '' ");
                        lblInvoiceDate.TextBoxDate.Text = Convert.ToDateTime(((DataRow)drowArray1.GetValue(0))["InvoiceDate"].ToString()).ToString();
                        lblInvoiceDate.TextBoxDate.Enabled = false;
                        lblInvoiceDate.imgCal.Enabled = false;
                        ddlTD.Enabled = false;
                        txtInvoiceNo.Enabled = false;
                    }
                    else
                    {
                        lblInvoiceDate.Date = "";
                        lblInvoiceDate.TextBoxDate.Enabled = true;
                        lblInvoiceDate.imgCal.Enabled = true;
                        ddlTD.Enabled = true ;
                        txtInvoiceNo.Enabled = true ;
                    }
                    ucSalesReturnGrid.Source = dtSales;
                    pnlGrid.Visible = true;
                }
                else
                {
                    ucDatePicker.imgCal.Enabled = true;
                    ddlTD.Enabled = true;
                    ucDatePicker.TextBoxDate.Enabled = true;
                    txtInvoiceNo.Enabled = true;
                    PnlHide.Visible = false;
                    pnlGrid.Visible = false;
                    lblInvoiceDate.Date = "";
                    ucSalesReturnGrid.Source = null;
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                }
                

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    bool pageValidateSave()
    {

        if (ddlTD.SelectedIndex == 0 || txtInvoiceNo.Text.Trim() == string.Empty || ServerValidation.IsDate(ucDatePicker.Date, true) != 0 || ServerValidation.IsDate(lblInvoiceDate.Date, true) != 0)
        {
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }

        if (Convert.ToDateTime(ucDatePicker.Date) > DateTime.Now.Date)
        {
            ucMsg.ShowWarning(Resources.Messages.DateRangeValidation);
            return false;
        }
        //Pankaj Kumar          18-04-2011
        if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(ucDatePicker.Date))
        {
            ucMsg.ShowInfo("Invalid Date Range");
            return false;
        } //Pankaj Kumar
        if (Convert.ToDateTime(lblInvoiceDate.Date) > DateTime.Now.Date)
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
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }
        if (ddlTD.SelectedValue == "0")
        {
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }
        if (ucDatePicker.Date == string.Empty)
        {
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }
        if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(ucDatePicker.Date))
        {
            ucMsg.ShowInfo("Invalid Date Range");
            return false;
        } //Pankaj Kumar

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

            if (!pageValidateSave())
            {
                return;
            }
            int Result = 0;
        
                DataTable DtDetail = new DataTable();
                DataTable dtSalesReturn = new DataTable();
                using (CommonData ObjCommom = new CommonData())
                {
                    dtSalesReturn = ObjCommom.GettvpTablePrimarySalesReturn();
                }

                ucSalesReturnGrid.Salestype = EnumData.eControlRequestTypeForEntry.ePrimary2Sales;
               
                DataTable Dt = ucSalesReturnGrid.ReturnGridSource();
                
                if (Dt == null || Dt.Rows.Count == 0)
                {
                    ucMsg.ShowInfo(Resources.Messages.Entersalesqty);
                    return;
                }
                DataRow[] drowArray = Dt.Select("IntermediarySalesReturnID > 0");
                string IntermediarySalesReturnID = "0";
                if (drowArray.Length > 0)
                {
                    IntermediarySalesReturnID = ((DataRow)drowArray.GetValue(0))["IntermediarySalesReturnID"].ToString();

                }
                foreach (DataRow dr in Dt.Rows)
                {
                    DataRow drow = dtSalesReturn.NewRow();
                    drow[0] = IntermediarySalesReturnID;
                    drow[1] = Convert.ToString(ddlTD.SelectedItem.Value);
                    drow[2] = txtInvoiceNo .Text .Trim ();
                    drow[3] = Convert.ToDateTime(lblInvoiceDate.Date); ;
                    drow[4] = dr["SKUCode"].ToString();
                    drow[5] = dr["ReturnQty"].ToString();
                    drow[6] = Convert.ToString(PageBase.SalesChanelID);
                    drow[7] = Convert.ToDateTime(ucDatePicker.Date);
                    dtSalesReturn.Rows.Add(drow);
                }
                dtSalesReturn.AcceptChanges();
                using (SalesData ObjSales = new SalesData())
                {

                    ObjSales.Error = "";
                   ObjSales.EntryType = EnumData.eEntryType.eInterface;
                   ObjSales.UserID = PageBase.UserId;
                    Result = ObjSales.InsertUpdatePrimarySalesReturnInfo(dtSalesReturn);

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


                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    ClearForm();

                    // updGrid.Update();
                }
        }
    

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    void ClearForm()
    {


        ddlTD.ClearSelection();
        ddlTD.SelectedValue = "0";
        txtInvoiceNo.Text = "";
        ucDatePicker.Date = "";
        lblInvoiceDate.Date = "";
        ddlTD.Enabled = true;
        ucDatePicker.TextBoxDate.Enabled = true;
        pnlGrid.Visible = false;
        PnlHide.Visible = false;
        txtInvoiceNo.Enabled = true;
        ucDatePicker.imgCal.Enabled = true;

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();

    }
   
}
