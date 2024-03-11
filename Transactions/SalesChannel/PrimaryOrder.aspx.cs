﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;

public partial class Transactions_SalesChannel_PrimaryOrder : PageBase
{
    #region Page Load event
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.ShowControl = false;
            divMsg.Visible = false;
            if (BackDaysAllowedOrder == 0)
            {
                ucDatePicker.Date = DateTime.Now.ToString();
                ucDatePicker.TextBoxDate.Enabled = false;
                ucDatePicker.imgCal.Enabled = false;
            }
            else
            {
                ucDatePicker.MinRangeValue = System.DateTime.Now.AddDays(-(BackDaysAllowedOrder));
                ucDatePicker.MaxRangeValue = System.DateTime.Now;
                ucDatePicker.RangeErrorMessage = "Invalid Date Range";
            }
                txtOrderNo.Text = "Autogenerated";
                txtOrderNo.Enabled = false; 
            
            
            if (!IsPostBack)
            {
                FillParentsalesChannel();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    #endregion
    #region Button Event
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (!pageValidateGo())
            {
                return;
            }
            if (ucDatePicker.Date == "" )
            {
                ucMsg.ShowInfo(Resources.Messages.InvalidDate);
                return;
            }
            using (SalesData objSales = new SalesData())
            {
                objSales.SalesChannelID = PageBase.SalesChanelID;
                objSales.Brand = PageBase.Brand;
                objSales.InvoiceDate = Convert.ToDateTime(ucDatePicker.Date);
                DataTable dtOrder = objSales.GetOrderDetail();
                if (dtOrder != null && dtOrder.Rows.Count > 0)
                {
                    ddlWarehouse.Enabled = false;

                }
                else
                {
                   ddlWarehouse.Enabled = true;
                }
                Ucgrid.Salestype = EnumData.eControlRequestTypeForEntry.eOrder;
                Ucgrid.Source = dtOrder;
                pnlGrid.Visible = true;

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
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
            Ucgrid.Salestype = EnumData.eControlRequestTypeForEntry.eOrder;
            DataTable Dt = Ucgrid.ReturnGridSource();
            string SumQTY = Dt.Compute("sum(Quantity)", "1=1").ToString();

            if (Dt == null || Dt.Rows.Count == 0 || SumQTY == "0")
            {
                ucMsg.ShowInfo(Resources.Messages.Entersalesqty);
                return;
            }
            DataTable DtDetail = new DataTable();
            using (CommonData ObjCommom = new CommonData())
            {
                DtDetail = ObjCommom.GettvpTableOrder();
            }

            foreach (DataRow dr in Dt.Rows)
            {
                DataRow drow = DtDetail.NewRow();
                drow[0] = 0;
                if (txtOrderNo.Text.ToLower() == "autogenerated")
                    drow[1] = "";
                else
                    drow[1] = txtOrderNo.Text;
               
                drow[2] = ucDatePicker.Date;
                drow[3] = PageBase.SalesChanelID;
                drow[4] = Convert.ToString(ddlWarehouse.SelectedValue);
                drow[5] = dr["SKUCode"].ToString();
                drow[6] = dr["Quantity"].ToString();
                drow[7] = dr["Amount"].ToString();
                DtDetail.Rows.Add(drow);
            }


            DtDetail.AcceptChanges();

            DtDetail = DtDetail.DefaultView.ToTable();
            if (DtDetail == null || DtDetail.Rows.Count == 0)
            {
                ucMsg.ShowInfo(Resources.Messages.Entersalesqty);
                return;
            }
            using (SalesData ObjSales = new SalesData())
            {

                ObjSales.Error = "";
                ObjSales.UserID = PageBase.UserId;
                ObjSales.InsertPrimaryOrderInfo(DtDetail);
                if (ObjSales.Error != null && ObjSales.Error != "")
                {
                    ucMsg.ShowInfo(ObjSales.Error);
                    return;
                }
                divMsg.Visible = true;
                hlkFinal.Text = ObjSales.OrderNumber;
                hlkFinal.Attributes.Add("OnClick", string.Format("return popup({0})", ObjSales.OrderId));
                //hlkFinal.NavigateUrl = string.Format("~/Transactions/POC/OrderForm.aspx?{0}",ObjSales.OrderId);
                pnlGrid.Visible = false;
                ClearForm();

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();
    }
    #endregion
    #region User defined Function
    void ClearForm()
    {

        ddlWarehouse.ClearSelection();
        ddlWarehouse.SelectedValue = "0";
        //txtOrderNo.Text = "";
        ucDatePicker.Date = "";
        ddlWarehouse.Enabled = true;
        pnlGrid.Visible = false;
        if (PageBase.IsPrimaryOrderNoAutogenerate == 0)
        {

            txtOrderNo.Text = "";

        }
       

    }
    bool pageValidateSave()
    {
        if (ServerValidation.IsDate(ucDatePicker.Date) == false)
        {
            ucMsg.ShowWarning(Resources.Messages.InvalidDateEntered);
            return false;
        }
        if (ddlWarehouse.SelectedIndex == 0 || ServerValidation.IsDate(ucDatePicker.Date, true) != 0)
        {
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }

        //if (Convert.ToDateTime(ucDatePicker.Date) != DateTime.Now.Date)
        //{
        //    ucMsg.ShowWarning(Resources.Messages.EnterCurrentDate);
        //    return false;
        //}

        return true;
    }
    bool pageValidateGo()
    {
        if (ServerValidation.IsDate(ucDatePicker.Date) == false)
        {
            ucMsg.ShowWarning(Resources.Messages.InvalidDateEntered);
            return false;
        }
        //if (Convert.ToDateTime(ucDatePicker.Date) != DateTime.Now.Date)
        //{
        //    ucMsg.ShowWarning(Resources.Messages.EnterCurrentDate);
        //     return false;

        //}

        //if (txtOrderNo.Text.Trim() == string.Empty)
        //{
        //    ucMsg.ShowWarning(Resources.Messages.EnterInvoiceNo);
        //    return false;
        //}
        return true;
    }
    void FillParentsalesChannel()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16( PageBase.SalesChanelTypeID);
            ObjSalesChannel.BlnShowDetail = true;
            ObjSalesChannel.UserID = PageBase.UserId;
            string[] str = { "SalesChannelID", "NameWithCode" };
            PageBase.DropdownBinding(ref ddlWarehouse, ObjSalesChannel.GetSalesChannelParent(), str);
        };

    }
    #endregion



    protected void btn_Click(object sender, EventArgs e)
    {
        divMsg.Visible = false;
    }
}