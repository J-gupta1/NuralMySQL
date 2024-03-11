using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;

public partial class Transactions_POC_SecondarySalesReturnInterfaceWithType :PageBase
{
    DateTime dt = new DateTime();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.ShowControl = false;

            dt = System.DateTime.Now.Date;      //Pankaj Kumar
            ucReturnDate.MinRangeValue = dt.AddDays(PageBase.NumberofBackDaysAllowed);
            ucReturnDate.MaxRangeValue = dt;
            ucReturnDate.RangeErrorMessage = "Invalid Date Range";

            if (!IsPostBack)
            {

                FillSalesman();
                lblInfo.Text = Resources.Messages.ValidationSalesDays.ToString().Replace("Number", PageBase.NumberofBackDaysAllowed.ToString().Replace("-", ""));
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
            if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(ucReturnDate.Date))
            {
                ucMsg.ShowInfo("Invalid Date Range");
                return;
            } //Pankaj Kumar


            using (SalesData objSales = new SalesData())
            {
                objSales.SalesChannelID = PageBase.SalesChanelID;
                objSales.RetailerCode = ddlRetailer.SelectedValue;

                objSales.ReturnDate = Convert.ToDateTime(ucReturnDate.Date);
                objSales.Brand = PageBase.Brand;
                DataTable dtSales = objSales.GetSecondarySalesReturn();
                if (dtSales.Rows.Count > 0)
                {
                    if (dtSales.Select("SecondarySalesReturnID >0").Length > 0)
                    {
                        DataRow[] drowArray = dtSales.Select("SecondarySalesReturnID > 0");
                        if (drowArray.Length > 0)
                        {
                            ddlRetailer.Enabled = false;
                            ddlSalesman.SelectedValue = ((DataRow)drowArray.GetValue(0))["SalesmanID"].ToString();

                            ddlSalesman.Enabled = false;

                            ucReturnDate.TextBoxDate.Enabled = false;
                            ucReturnDate.imgCal.Enabled = false;

                        }
                    }
                    else
                    {

                        ucReturnDate.TextBoxDate.Enabled = true;
                        ucReturnDate.imgCal.Enabled = true;
                        ddlSalesman.Enabled = true;
                        ddlRetailer.Enabled = true;
                    }
                    ucSalesReturnGrid.Salestype = EnumData.eControlRequestTypeForEntry.eSecondarySales;
                    ucSalesReturnGrid.Source = dtSales;

                    pnlGrid.Visible = true;

                }
                else
                {

                    ddlRetailer.Enabled = true;
                    ddlSalesman.Enabled = true;
                    ucReturnDate.imgCal.Enabled = true;
                    ucReturnDate.TextBoxDate.Enabled = true;
                    ucSalesReturnGrid.Source = null;

                }


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

        if (ddlRetailer.SelectedIndex == 0 || ServerValidation.IsDate(ucReturnDate.Date, true) != 0)
        {
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            return false;
        }

        if (Convert.ToDateTime(ucReturnDate.Date) > DateTime.Now.Date)
        {
            ucMsg.ShowWarning(Resources.Messages.DateRangeValidation);
            return false;
        }
        //Pankaj Kumar
        if (Convert.ToDateTime(DateTime.Now.Date).AddDays(PageBase.NumberofBackDaysAllowed) > Convert.ToDateTime(ucReturnDate.Date))
        {
            ucMsg.ShowInfo("Invalid Date Range");
            return false;
        } //Pankaj Kumar

        return true;
    }
    bool pageValidateGo()
    {

        //if (txtInvoiceNo.Text.Trim() == string.Empty)
        //{
        //    ucMsg.ShowWarning(Resources.Messages.EnterInvoiceNo);
        //    return false;
        //}
        return true;
    }
    protected void rdoSelectMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSelectMode.SelectedValue == "1")
        {
            Response.Redirect("UploadSecondarySalesReturnWithType.aspx");
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
            int Result = 0;

            DataTable DtDetail = new DataTable();
            DataTable dtSalesReturn = new DataTable();
            using (CommonData ObjCommom = new CommonData())
            {
                dtSalesReturn = ObjCommom.GettvpTableSecondarySalesReturn();
            }
            ucSalesReturnGrid.Salestype = EnumData.eControlRequestTypeForEntry.eSecondarySales;

            DataTable Dt = ucSalesReturnGrid.ReturnGridSource();


            if (Dt == null || Dt.Rows.Count == 0)
            {
                ucMsg.ShowInfo(Resources.Messages.Entersalesqty);
                return;
            }

            foreach (DataRow dr in Dt.Rows)
            {
                DataRow drow = dtSalesReturn.NewRow();
                drow[0] = Convert.ToString(ddlRetailer.SelectedItem.Value);
                drow[1] = dr["SKUCode"].ToString();
                drow[2] = dr["ReturnQty"].ToString();
                drow[3] = Convert.ToString(PageBase.SalesChanelID);
                drow[4] = ddlSalesman.SelectedItem.Text;
                drow[5] = Convert.ToDateTime(ucReturnDate.Date);
                drow[6] = ddlSalesman.SelectedValue;
                dtSalesReturn.Rows.Add(drow);
            }
            dtSalesReturn.AcceptChanges();
            using (SalesData ObjSales = new SalesData())
            {

                ObjSales.Error = "";
                ObjSales.EntryType = EnumData.eEntryType.eInterface;
                Result = ObjSales.InsertUpdateSecondarySalesReturnInfo(dtSalesReturn);

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
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    void ClearForm()
    {



        ddlRetailer.Items.Clear();
        ddlRetailer.Items.Insert(0, new ListItem("Select", "0"));
        ddlSalesman.SelectedValue = "0";
        ucReturnDate.Date = "";
        ucReturnDate.imgCal.Enabled = true;
        ddlRetailer.Enabled = true;
        ucReturnDate.TextBoxDate.Enabled = true;
        pnlGrid.Visible = false;

        ddlSalesman.Enabled = true;

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;
        ClearForm();

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



    }
    protected void ddlRetailer_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlGrid.Visible = false;
    }
}
