using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
using Cryptography;


public partial class Transactions_POC_ViewOrdersApproved : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;
        if (!IsPostBack)
        {
            ddlSalesChannelName.Items.Insert(0, new ListItem("Select", "0"));
//            pnlGrid.Visible = false;
            FillsalesChannelType();
            cmbChannelType.SelectedValue = "6";
            cmbChannelType.Enabled = false;
            cmbChannelType_SelectedIndexChanged(sender, null);

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
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        if (!Validation())
        {
            ucMessage1.ShowInfo("Invalid Date Range");
            return;
        }
        if (cmbChannelType.SelectedIndex != 0)
        {
            DataTable DtData = new DataTable();
            using (SalesChannelData obj = new SalesChannelData())
            {
                obj.SalesChannelID = Convert.ToInt32(ddlSalesChannelName.SelectedValue);
                obj.OrderFromDate = ucDateFrom.Date == "" ? obj.OrderFromDate : Convert.ToDateTime(ucDateFrom.Date);
                obj.OrderToDate = ucDateTo.Date == "" ? obj.OrderToDate : Convert.ToDateTime(ucDateTo.Date);
                obj.LoggedInSalesChannelID = PageBase.SalesChanelID;
                obj.StatusValue = Convert.ToInt32(ddlStatus.SelectedValue);
                obj.OrderNumber = txtOrderNumber.Text.Trim();
                obj.Flag = 1;
                DataTable dt = obj.GetSalesChannelOrderDetailInfo();
                if (dt != null && dt.Rows.Count > 0)
                {
                    //pnlGrid.Visible = true;
                    gvOrder.DataSource = dt;
                    gvOrder.DataBind();
                    dvAction.Visible = true;
                    dvheading.Visible = true;
                }
                else
                {
                    gvOrder.DataSource = null;
                    gvOrder.DataBind();
                    dvAction.Visible = false;
                    dvheading.Visible = false;
                    //pnlGrid.Visible = false;
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                }
                UpdatePanel1.Update();

            }
        }
        else
        {
            ucMessage1.ShowInfo(Resources.Messages.MandatoryField);
        }
    }


    void ClearForm()
    {
        dvheading.Visible = false;
        dvAction.Visible = false;
        ddlSalesChannelName.Items.Clear();
        ddlSalesChannelName.Items.Insert(0, new ListItem("Select", "0"));
        dvDetail.Visible = false;
        gvDetails.DataSource = null;
        gvDetails.DataBind();
        gvOrder.DataSource = null;
        gvOrder.DataBind();
        txtOrderNumber.Text = "";
        ucDateFrom.Date = "";
        ucDateTo.Date = "";
        updgrid.Update();
        updDetail.Update();
        UpdatePanel1.Update();

    }
   
    private bool Validation()
    {
        if (ucDateFrom.Date != "")
        {
            if (ucDateTo.Date == "")
            {
                return false;
            }
        }

        if (ucDateTo.Date != "")
        {
            if (ucDateFrom.Date == "")
            {
                return false;
            }
        }
        if (ucDateTo.Date != "" && ucDateFrom.Date != "")
        {
            if (Convert.ToDateTime(ucDateTo.Date) > Convert.ToDateTime(ucDateFrom.Date))
            {
                return false;
            }
        }

        return true;

    }
    protected void cmbChannelType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlSalesChannelName.Items.Clear();
            using (SalesChannelData obj = new SalesChannelData())
            {
                obj.SalesChannelTypeID = Convert.ToInt16(cmbChannelType.SelectedValue);
                DataTable dt = obj.GetSalesChannelBasedOnType();
                String[] colArray = { "SalesChannelID", "SalesChannelName" };
                PageBase.DropdownBinding(ref ddlSalesChannelName, dt, colArray);
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearForm();

    }
    protected void gvOrder_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(e.CommandArgument);
            ViewState["OrderId"] = id;
            if (e.CommandName == "Details")
            {

                using (SalesChannelData obj = new SalesChannelData())
                {
                    obj.SalesChannelID = Convert.ToInt32(ddlSalesChannelName.SelectedValue);
                    obj.LoggedInSalesChannelID = PageBase.SalesChanelID;
                    obj.OrderID = id;
                    obj.Flag = 2;
                    DataTable dt = obj.GetSalesChannelOrderDetailInfo();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //pnlGrid.Visible = true;
                        //dvDetail.Visible = true;
                        gvDetails.DataSource = dt;
                        gvDetails.DataBind();
                        dvDetail.Visible = true;
                        //updDetail.Update();
                        //dvAction.Visible = true;
                        //dvheading.Visible = true;
                

                    }
                    else
                    {
                        gvDetails.DataSource = null;
                        gvDetails.DataBind();
                        dvDetail.Visible = false;
                        // pnlGrid.Visible = false;
                        ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                    }
                    updDetail.Update();
                }
            }
        }

        catch (Exception ex)
        {
        }
    }

}
