using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;

public partial class Transactions_SalesChannel_SecondarySalesInterface : PageBase
{
    DateTime dt = new DateTime();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            dt = System.DateTime.Now.Date;      //Pankaj Kumar
            ucDatePicker.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
            ucDatePicker.MaxRangeValue = dt;
            ucDatePicker.RangeErrorMessage = "Invalid Date.";
            //lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", intBackDaysAllowForTD.ToString().Replace("-", ""));
            lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                FillSalesman();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            //clsException.clsHandleException.fncHandleException(ex, "");
        }

    }

    void FillSalesman()
    {
        using (SalesmanData ObjSalesman = new SalesmanData())
        {
            ObjSalesman.Type = EnumData.eSearchConditions.Active;
            ObjSalesman.SalesChannelID = PageBase.SalesChanelID;
            ObjSalesman.MapwithRetailer = 1;
            String[] StrCol = new String[] { "SalesmanID", "Salesmanname" };
            PageBase.DropdownBinding(ref ddlSalesman, ObjSalesman.GetSalesmanInfo(), StrCol);

        };
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //PnlHide.Visible = true;
            if (Convert.ToDateTime(ucDatePicker.Date) > DateTime.Now)
            {
                ucMsg.ShowInfo(Resources.Messages.DateRangeValidation);
                return;
            }
            using (SalesData objSales = new SalesData())
            {
                objSales.SalesChannelID = PageBase.SalesChanelID;
                if (ddlRetailer.SelectedIndex != 0)
                {
                    objSales.RetailerCode = ddlRetailer.SelectedValue;
                }
                if (ucDatePicker.Date != "")
                {
                    objSales.InvoiceDate = Convert.ToDateTime(ucDatePicker.Date);
                }
                objSales.Brand = PageBase.Brand;
                DataTable dtSales = objSales.GetSecondarySales();
                if (dtSales.Rows.Count > 0)
                {
                    if (dtSales.Select("SecondarySalesID >0").Length > 0)
                    {
                        DataRow[] drowArray = dtSales.Select("SecondarySalesID > 0");
                        if (drowArray.Length > 0)
                        {
                            //   ddlRetailer.SelectedValue =((DataRow)drowArray.GetValue(0))["SalesToCode"].ToString();
                            ddlRetailer.Enabled = false;
                            ddlSalesman.SelectedValue = ((DataRow)drowArray.GetValue(0))["SalesmanID"].ToString();
                            ddlSalesman.Enabled = false;
                            ucDatePicker.TextBoxDate.Text = ((DataRow)drowArray.GetValue(0))["InvoiceDate"].ToString();
                            ucDatePicker.TextBoxDate.Enabled = false;
                            ucDatePicker.imgCal.Enabled = false;

                        }
                    }
                    else
                    {
                        ucDatePicker.imgCal.Enabled = true;
                        ucDatePicker.TextBoxDate.Enabled = true;

                        ddlSalesman.Enabled = true;
                        ddlRetailer.Enabled = true;
                    }
                }
                else
                {
                    ucDatePicker.imgCal.Enabled = true;
                    ddlSalesman.Enabled = true;
                    ddlRetailer.Enabled = true;
                    ucDatePicker.TextBoxDate.Enabled = true;
                }
                ucSalesEntryGrid1.Source = dtSales;
                pnlGrid.Visible = true;
                updGrid.Update();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            // clsException.clsHandleException.fncHandleException(ex, "");
        }
    }
    bool pageValidateSave()
    {

        if (ddlRetailer.SelectedIndex == 0  || ServerValidation.IsDate(ucDatePicker.Date, true) != 0 || ddlSalesman.SelectedValue=="0")
        {
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }

        if (Convert.ToDateTime(ucDatePicker.Date) > DateTime.Now.Date)
        {
            ucMsg.ShowWarning(Resources.Messages.DateRangeValidation);
            return false;
        }

        if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(ucDatePicker.Date))
        {
            ucMsg.ShowInfo("Invalid Date!");
            return false;
        } //Pankaj Kumar

        return true;
    }
    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "1")
        {
            Response.Redirect("UploadSecondarySalesWithType.aspx");
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }//Pankaj Kumar
            int Result = 0;
            if (!pageValidateSave())
            {
                return;
            }


            DataTable DtDetail = new DataTable();
            using (CommonData ObjCommom = new CommonData())
            {
                DtDetail = ObjCommom.GettvpTableSecondarySales();
            }
            ucSalesEntryGrid1.Salestype = EnumData.eControlRequestTypeForEntry.eSecondarySales;

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
            DataRow[] drowArray = Dt.Select("SecondarySalesID > 0");
            string SecondarySalesID = "0";
            if (drowArray.Length > 0)
            {
                SecondarySalesID = ((DataRow)drowArray.GetValue(0))["SecondarySalesID"].ToString();

            }

            foreach (DataRow dr in Dt.Rows)
            {
                DataRow drow = DtDetail.NewRow();

                drow[0] = SecondarySalesID;
                drow[1] = Convert.ToString(ddlRetailer.SelectedItem.Value);
                drow[2] = ucDatePicker.Date;
                drow[3] = dr["SKUCode"].ToString();
                drow[4] = dr["Quantity"].ToString();
                drow[5] = ddlSalesman.SelectedItem.Text;
                drow[6] = PageBase.SalesChanelID;
                drow[7] = ddlSalesman.SelectedValue;
                DtDetail.Rows.Add(drow);
            }

            DtDetail.AcceptChanges();
            using (SalesData ObjSales = new SalesData())
            {

                ObjSales.Error = "";
                ObjSales.EntryType = EnumData.eEntryType.eInterface;
                Result = ObjSales.InsertUpdateSecondarySalesInfo(DtDetail);

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

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
             clsException.clsHandleException.fncHandleException(ex, "");
        }
    }
    void ClearForm()
    {
        ddlRetailer.Items.Clear();
        ddlRetailer.Items.Insert(0,new ListItem("Select","0"));
        ddlSalesman.SelectedValue = "0";
        ddlSalesman.Enabled = true;
        ucDatePicker.imgCal.Enabled = true;
        ucDatePicker.TextBoxDate.Enabled = true;
        ucDatePicker.TextBoxDate.Text = "";
        ddlRetailer.Enabled = true;
        pnlGrid.Visible = false;
        updGrid.Update();
        //PnlHide.Visible = false;
    }

    protected void ddlRetailer_SelectedIndexChanged(object sender, EventArgs e)
    {

        pnlGrid.Visible = false;

        updGrid.Update();
    }
    protected void ddlSalesman_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRetailer.Items.Clear();
        ddlRetailer.Items.Insert(0, new ListItem("Select", "0"));

        using (RetailerData ObjRetailer = new RetailerData())
        {
            ObjRetailer.SalesChannelID = PageBase.SalesChanelID;
            ObjRetailer.SalesmanID = Convert.ToInt32(ddlSalesman.SelectedValue);
            ObjRetailer.Type = 1;               //For 1 because we are going to get only active Retailers
            string[] str = { "RetailerCode", "Retailer" };
            PageBase.DropdownBinding(ref ddlRetailer, ObjRetailer.GetRetailerInfo(), str);
        };


        pnlGrid.Visible = false;

        updGrid.Update();
    }
}
