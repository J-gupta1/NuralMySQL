using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;

public partial class Transactions_SalesChannel_PrimarySales1ReturnInterface : PageBase
{
   
    string strSalesChannelName, strSalesChannelCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            strSalesChannelCode = hdnCode.Value;
            strSalesChannelName = hdnName.Value;
            txtSearchedName.Text = strSalesChannelName;
            btnsearch.Attributes.Add("onclick", "return popup();");
           ucDatePicker.MaxRangeValue = System.DateTime.Now.Date;
           ucDatePicker.RangeErrorMessage = "Invalid Date Range";
           lblInfo.Visible = true;
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
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);

        }

    }
    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "1")
        {
            Response.Redirect("PrimarySalesReturnUploadNew.aspx");
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
                objSalesData.Brand = PageBase.Brand;    
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
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        PnlHide.Visible = true;
        txtSearchedName.Text = strSalesChannelName;
        
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
                objSales.SalesChannelCode = strSalesChannelCode;
                objSales.ReturnDate = Convert.ToDateTime(ucDatePicker.Date);
                objSales.Brand = PageBase.Brand;
                objSales.ReturnToSalesChannelCode = ddlWarehouse.SelectedValue;
                DataTable dtSales = objSales.GetPrimarySales1Return();

                if (dtSales.Rows.Count > 0)
                {
                    btnsearch.Enabled = false;
                    if (dtSales.Select("PrimarySalesID >0").Length > 0)
                    {
                        DataRow[] drowArray = dtSales.Select("PrimarySalesID > 0");
                        if (drowArray.Length > 0)
                        {

                            ddlWarehouse.Enabled = false;
                            txtInvoiceNo.Enabled = false;
                          
                            lblInvoiceDate.TextBoxDate.Text = Convert.ToDateTime(((DataRow)drowArray.GetValue(0))["InvoiceDate"].ToString()).ToString ();
                            lblInvoiceDate.TextBoxDate.Enabled = false;
                            lblInvoiceDate.imgCal.Enabled = false;
                            
                        }
                       
                    }
                    else
                    {
                        ddlWarehouse.Enabled = true;
                        txtInvoiceNo.Enabled = true;
                        lblInvoiceDate.Date = "";
                        lblInvoiceDate.TextBoxDate.Text = "";
                        lblInvoiceDate.TextBoxDate.Enabled = true;
                        lblInvoiceDate.imgCal.Enabled = true;
                    }
                    if (dtSales.Select("PrimarySalesReturnID >0").Length > 0)
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
                        ddlWarehouse.Enabled = false;
                        txtInvoiceNo.Enabled = false;
                    }
                    else
                    {
                        lblInvoiceDate.Date = "";
                        lblInvoiceDate.TextBoxDate.Enabled = true ;
                        lblInvoiceDate.imgCal.Enabled = true ;
                        ddlWarehouse.Enabled = false;
                        txtInvoiceNo.Enabled = false;
                    }
                    ucSalesReturnGrid.Source = dtSales;
                    pnlGrid.Visible = true;
                }
                else
                {
                    btnsearch.Enabled = true;
                    ucDatePicker.imgCal.Enabled = true;
                    ddlWarehouse.Enabled = true;
                    ucDatePicker.TextBoxDate.Enabled = true;
                    txtInvoiceNo.Enabled = true;

                    PnlHide.Visible = false;
                    pnlGrid.Visible = false;
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

        if (ddlWarehouse.SelectedIndex == 0 || txtInvoiceNo.Text.Trim() == string.Empty || ServerValidation.IsDate(ucDatePicker.Date, true) != 0 || ServerValidation.IsDate(lblInvoiceDate .Date, true) != 0 )
        {
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }

        if (Convert.ToDateTime(ucDatePicker.Date) > DateTime.Now.Date)
        {
            ucMsg.ShowWarning(Resources.Messages.DateRangeValidation);
            return false;
        }
        if (Convert.ToDateTime(lblInvoiceDate .Date) > DateTime.Now.Date)
        {
            ucMsg.ShowWarning(Resources.Messages.DateRangeValidation);
            return false;
        }
        //if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(ucDatePicker.Date))
        //{
        //    ucMsg.ShowInfo("Invalid Date Range");
        //    return false;
        //} 

        return true;
    }
    bool pageValidateGo()
    {

        if (txtInvoiceNo.Text.Trim() == string.Empty)
        {
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }
        if (ddlWarehouse.SelectedValue == "0")
        {
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }
        if (ucDatePicker.Date == string.Empty)
        {
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }
        //if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(ucDatePicker.Date))
        //{
        //    ucMsg.ShowInfo("Invalid Date Range");
        //    return false;
        //} 

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
                dtSalesReturn = ObjCommom.GettvpTablePrimarySalesReturnNew();
            }

            ucSalesReturnGrid.Salestype = EnumData.eControlRequestTypeForEntry.ePrimary1Sales;

            DataTable Dt = ucSalesReturnGrid.ReturnGridSource();

            if (Dt == null || Dt.Rows.Count == 0)
            {
                ucMsg.ShowInfo(Resources.Messages.Entersalesqty);
                return;
            }
            DataRow[] drowArray = Dt.Select("PrimarySalesReturnID > 0");
            string PrimarySalesReturnID = "0";
            if (drowArray.Length > 0)
            {
                PrimarySalesReturnID = ((DataRow)drowArray.GetValue(0))["PrimarySalesReturnID"].ToString();

            }
            foreach (DataRow dr in Dt.Rows)
            {
                DataRow drow = dtSalesReturn.NewRow();
                drow[0] = PrimarySalesReturnID;
                drow[1] = strSalesChannelCode;
                drow[2] = txtInvoiceNo .Text .Trim ();
                drow[3] = Convert.ToDateTime(lblInvoiceDate.Date);
                drow[4] = dr["SKUCode"].ToString();
                drow[5] = dr["ReturnQty"].ToString();
                drow[6] = Convert.ToString(PageBase.SalesChanelID);
                drow[7] = Convert.ToDateTime(ucDatePicker.Date);
                drow[8] = Convert.ToString(ddlWarehouse.SelectedItem.Value);
                dtSalesReturn.Rows.Add(drow);
            }
            dtSalesReturn.AcceptChanges();
            using (SalesData ObjSales = new SalesData())
            {

                ObjSales.Error = "";
                ObjSales.EntryType = EnumData.eEntryType.eInterface;
                Result = ObjSales.InsertUpdatePrimarySalesReturnInfoInterface(dtSalesReturn);

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
                btnsearch.Enabled = true;
                txtSearchedName.Text = "";
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


        ddlWarehouse.ClearSelection();
        ddlWarehouse.SelectedValue = "0";
        txtInvoiceNo.Text = "";
        ucDatePicker.Date = "";
      
        ddlWarehouse.Enabled = true;
        ucDatePicker.TextBoxDate.Enabled = true;
        pnlGrid.Visible = false;
        PnlHide.Visible = false;
        txtInvoiceNo.Enabled = true;
        ucDatePicker.imgCal.Enabled = true;

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        btnsearch.Enabled = true;
        ClearForm();
        txtSearchedName.Text = "";
    }
   
}
