using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;

public partial class Transactions_Common_StockAdjustment : PageBase
{
    string strSalesChannelName;


    protected void Page_Load(object sender, EventArgs e)
    {

        string strSalesChannelTypeId = cmbChannelType.SelectedValue;
        btnCheck.Attributes.Add("OnClick", "return popup();");
        ucMessage1.ShowControl = false;
        strSalesChannelName = hdnName.Value;
        txtSalesChannelName.Text = strSalesChannelName;
        txtSalesChannelName.Enabled = false;


        if (!IsPostBack)
        {
            ViewState["DtGrid"] = null;
            //cmbSalesChannel.Items.Insert(0, new ListItem("Select", "0"));
            pnlGrid.Visible = false;
            PnlHide.Visible = false;
            FillsalesChannelType();
        }


    }
    void FillsalesChannelType()
    {
        using (SalesChannelData obj = new SalesChannelData())
        {
            DataTable dt = obj.GetSalesChannelType();
            String[] colArray = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref cmbChannelType, dt, colArray);

        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (cmbChannelType.SelectedIndex != 0)
        {
            OpeningStockDate();
            if (lblOpeningdate.Text == "")
            {
                ucMessage1.ShowInfo("Please Insert Opening Stock of SalesChannel");
                return;
            }
            btnCheck.Enabled = false;
            DataTable DtData = new DataTable();
            using (SalesChannelData obj = new SalesChannelData())
            {
                obj.SalesChannelID = Convert.ToInt32(hdnID.Value);

                DataTable dt = obj.GetSalesChannelStockInfo();
                if (dt != null && dt.Rows.Count > 0)
                {
                    PnlHide.Visible = true;
                    ViewState["DtGrid"] = dt;
                    pnlGrid.Visible = true;
                    gvStockEntry.DataSource = dt;
                    gvStockEntry.DataBind();

                }
                else
                {
                    PnlHide.Visible = false;
                    gvStockEntry.DataSource = null;
                    gvStockEntry.DataBind();
                    pnlGrid.Visible = false;
                    ViewState["DtGrid"] = null;
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                }

            }
        }
        else
        {
            ucMessage1.ShowInfo(Resources.Messages.MandatoryField);
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }
        if (!PageValidatesave())
        {
            return;
        }
        DataTable DtDetail = new DataTable();
        using (CommonData ObjCommom = new CommonData())
        {
            DtDetail = ObjCommom.GettvpTableStockAdjustment();
        }
        Int16 Count = 0;
        DataTable DtAdjsutment = new DataTable();
        if (ViewState["DtGrid"] != null)
        {
            DtAdjsutment = (DataTable)ViewState["DtGrid"];

            for (int i = 0; i <= gvStockEntry.Rows.Count - 1; i++)
            {
                TextBox txtQuantityFooter = (TextBox)gvStockEntry.Rows[i].FindControl("txtQuantity");
                if (txtQuantityFooter.Text != "0" && txtQuantityFooter.Text != "")
                {
                    if (ServerValidation.IsInteger(txtQuantityFooter.Text, true) > 0)
                    {
                        ucMessage1.ShowInfo(Resources.Messages.Entervalidqty);
                        return;

                    }

                    DtAdjsutment.Rows[i]["Quantity"] = txtQuantityFooter.Text;

                    Count = 1;
                }

            }
        }
        if (Count == 0)
        {
            ucMessage1.ShowInfo(Resources.Messages.Entersalesqty);
            return;
        }
        DtAdjsutment.DefaultView.RowFilter = "Quantity<>0 ";
        DtAdjsutment = DtAdjsutment.DefaultView.ToTable();
        foreach (DataRow dr in DtAdjsutment.Rows)
        {
            DataRow drow = DtDetail.NewRow();
            drow[0] = Convert.ToInt32(hdnID.Value);
            drow[1] = ucDatePicker.Date;
            drow[2] = dr["SKUID"].ToString();
            drow[3] = dr["Quantity"].ToString();
            DtDetail.Rows.Add(drow);
        }

        DtDetail.AcceptChanges();
        using (SalesData ObjSales = new SalesData())
        {

            ObjSales.Error = "";
            ObjSales.UserID = PageBase.UserId;
            ObjSales.InsertStockAdjustment(DtDetail);

            if (ObjSales.ErrorDetailXML != null && ObjSales.ErrorDetailXML != string.Empty)
            {
                ucMessage1.XmlErrorSource = ObjSales.ErrorDetailXML;
                return;
            }
            else if (ObjSales.Error != null && ObjSales.Error != "" && ObjSales.Error != "0")
            {
                ucMessage1.ShowError(ObjSales.Error);
                return;
            }

            ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
            pnlGrid.Visible = false;
            ClearForm();
            btnCheck.Enabled = true;
            txtSalesChannelName.Text = "";


        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;
        ClearForm();
        btnCheck.Enabled = true;
        txtSalesChannelName.Text = "";

    }
    void ClearForm()
    {
        PnlHide.Visible = false;
        pnlGrid.Visible = false;
        ucDatePicker.Date = "";
        cmbChannelType.SelectedValue = "0";

        ViewState["DtGrid"] = null;
    }
    bool PageValidatesave()
    {

        if (ucDatePicker.Date == "")
        {
            ucMessage1.ShowInfo(Resources.Messages.MandatoryField);
            return false;
        }
        if (Convert.ToDateTime(ucDatePicker.Date) > System.DateTime.Now)
        {
            ucMessage1.ShowInfo(Resources.Messages.DateRangeValidation);
            return false;
        }
        if (Convert.ToDateTime(ucDatePicker.Date) < Convert.ToDateTime(lblOpeningdate.Text.Trim()))
        {
            ucMessage1.ShowInfo(Resources.Messages.StockAdjustmentDateValidation);
            return false;
        }
        return true;
    }
    protected void gvStockEntry_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        {
            DataTable DtSalesData = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdD = (HiddenField)e.Row.FindControl("hdnid");

                TextBox txtQuantityFooter = (TextBox)e.Row.FindControl("txtQuantity");
                Label lblSkuID = (Label)e.Row.FindControl("lblSKUID");
                Label lblStockInhand = (Label)e.Row.FindControl("lblStockInhand");

                txtQuantityFooter.Attributes.Add("OnChange", "StockCheckAdjustment(this);");



            }
        }
    }



    protected void BtnCheck_Click(object sender, EventArgs e)
    {

    }

    private void OpeningStockDate()
    {
        PnlHide.Visible = false;
        pnlGrid.Visible = false;
        if (Convert.ToInt32(hdnID.Value) != 0)
        {
            // PnlOpeningDate.Visible = true;
            using (SalesChannelData ObjsalesChanel = new SalesChannelData())
            {
                ObjsalesChanel.SalesChannelID = Convert.ToInt32(hdnID.Value);
                DataTable Dt = ObjsalesChanel.GetSalesChannelOpeningStockInfo();
                lblOpeningdate.Text = Dt.Rows[0]["OpeningStockDate"].ToString();
            }

        }
        else
        {
            lblOpeningdate.Text = "";
            // PnlOpeningDate.Visible = false;
        }
    }
}

