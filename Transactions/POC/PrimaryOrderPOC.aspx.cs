using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;

public partial class Transactions_POC_PrimaryOrderPOC : PageBase 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.ShowControl = false;
           
            if (!IsPostBack)
            {
                if (BackDaysAllowedOpeningStock == 0)
                {
                    ucDatePicker.Date = DateTime.Now.ToString();
                    ucDatePicker.TextBoxDate.Enabled = false;
                }

                else
                {
                    ucDatePicker.MinRangeValue = System.DateTime.Now.AddDays(-(BackDaysAllowedOpeningStock));
                    ucDatePicker.MaxRangeValue = System.DateTime.Now;
                    ucDatePicker.RangeErrorMessage = "Invalid Date Range";

                }
                FillParentsalesChannel();
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
                drow[1] = "";
                drow[2] = ucDatePicker.Date;
                drow[3] = PageBase.SalesChanelID;
                drow[4] = Convert.ToString(ddlWarehouse.SelectedValue);
                drow[5] = dr["SKUCode"].ToString();
                drow[6] = dr["Quantity"];
                drow[7] = dr["Amount"];
               
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

                ObjSales.InsertPrimaryOrderInfo(DtDetail);

                if (ObjSales.Error != null && ObjSales.Error != "")
                {
                    ucMsg.ShowInfo(ObjSales.Error);
                    return;
                }

                string str = string.Format("Order No. {0} created sucessfully",ObjSales.OrderNumber);
                ucMsg.ShowSuccess(str);
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
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
          
            if (ucDatePicker.Date == "")
            {
                ucMsg.ShowInfo(Resources.Messages.InvalidDate);
                return;
            }
            using (SalesData objSales = new SalesData())
            {
                objSales.SalesChannelID = PageBase.SalesChanelID;
                objSales.Brand = PageBase.Brand;
                objSales.InvoiceDate = Convert.ToDateTime(ucDatePicker.Date);
                DataTable dtOrder = objSales.GetOrderDetail ();
                if (dtOrder != null && dtOrder.Rows.Count > 0)
                {
                    ddlWarehouse.Enabled = false;

                }
                else
                {


                    ddlWarehouse.Enabled = true;
                }
              
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
     
    #region User defined Function
    void ClearForm()
    {

        ddlWarehouse.ClearSelection();
        ddlWarehouse.SelectedValue = "0";
        //txtOrderNo.Text = "";
        ucDatePicker.Date = "";
        ddlWarehouse.Enabled = true;
        pnlGrid.Visible = false;


    }
    bool pageValidateSave()
    {

        if (ddlWarehouse.SelectedIndex == 0 || ServerValidation.IsDate(ucDatePicker.Date, true) != 0)
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
   
    void FillParentsalesChannel()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {

            ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
            //ObjSalesChannel.BlnShowDetail = true;
            string[] str = { "ParentId", "ParentName" };
            PageBase.DropdownBinding(ref ddlWarehouse, ObjSalesChannel.GetParentSalesChannel(), str);
        };

    }
    #endregion
}
