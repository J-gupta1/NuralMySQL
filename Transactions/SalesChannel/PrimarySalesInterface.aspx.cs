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
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                FillsalesChannel();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    void FillsalesChannel()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(EnumData.eSalesChannelType.SS);
            ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
           
            string[] str = { "SalesChannelID", "DisplayName" };
            PageBase.DropdownBinding(ref ddlSS, ObjSalesChannel.GetSalesChannelInfo(), str);
        };
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
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
                objSales.DistributorSalesChannelID = Convert.ToInt32(ddlSS.SelectedValue);
                DataTable dtSales = objSales.GetPrimarySalesInfo();
                if (dtSales.Rows.Count > 0)
                {

                    if (dtSales.Select("PrimarySalesID >0").Length > 0)
                    {
                        DataRow[] drowArray = dtSales.Select("PrimarySalesID > 0");
                        if (drowArray.Length > 0)
                        {
                            ddlSS.SelectedValue = ((DataRow)drowArray.GetValue(0))["SalesToId"].ToString();

                            ddlSS.Enabled = false;

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
                        ddlSS.Enabled = true;
                        txtInvoiceNo.Enabled = true;
                    }
                }
                else
                {

                    ucDatePicker.imgCal.Enabled = true;
                    //txtSalesman.Enabled = true;
                    ddlSS.Enabled = true;
                    ucDatePicker.TextBoxDate.Enabled = true;
                    txtInvoiceNo.Enabled = true;


                }
                ucSalesEntryGrid1.Source = dtSales;

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

        if (ddlSS.SelectedIndex == 0 || txtInvoiceNo.Text.Trim() == string.Empty || ServerValidation.IsDate(ucDatePicker.Date, true) != 0)
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
                DtDetail = ObjCommom.GettvpTablePrimarySales();
            }
            ucSalesEntryGrid1.Salestype = EnumData.eControlRequestTypeForEntry.ePrimary1Sales;
            DataTable Dt = ucSalesEntryGrid1.ReturnGridSource();
            if (Dt == null || Dt.Rows.Count == 0)
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
                drow[1] = Convert.ToString(ddlSS.SelectedValue);        
                drow[2] = txtInvoiceNo.Text;
                drow[3] = ucDatePicker.Date;
                drow[4] = dr["SKUCode"].ToString();
                drow[5] = dr["Quantity"].ToString();
                drow[6] = PageBase.SalesChanelID;
                drow[7] = dr["PrimaryOrderDetailID"];
                drow[7] = dr["PrimaryOrderDetailID"];
                //drow[8] = dr["OrderQuantity"];
                DtDetail.Rows.Add(drow);
            }

            DtDetail.AcceptChanges();
            using (SalesData ObjSales = new SalesData())
            {

                ObjSales.Error = "";
                ObjSales.EntryType = EnumData.eEntryType.eInterface;
                Result = ObjSales.InsertPrimarySalesInfoDetail(DtDetail);

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

        ddlSS.ClearSelection();
        ddlSS.SelectedValue = "0";
        //txtSalesman.Text = "";
        txtInvoiceNo.Text = "";
        ucDatePicker.Date = "";
        ucDatePicker.imgCal.Enabled = true;
       // txtSalesman.Enabled = true;
        ddlSS.Enabled = true;
        ucDatePicker.TextBoxDate.Enabled = true;
        pnlGrid.Visible = false;
        //PnlHide.Visible = false;
        txtInvoiceNo.Enabled = true;

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();
    }
   
}
