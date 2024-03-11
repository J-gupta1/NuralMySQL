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

public partial class Transactions_POC_ViewOrder : PageBase
{
    string strSalesChannelName;
    protected void Page_Load(object sender, EventArgs e)
    {
        //string strSalesChannelTypeId = cmbChannelType.SelectedValue;
        ucMessage1.ShowControl = false;
       if (!IsPostBack)
        {
            //pnlGrid.Visible = false;
            //ddlSalesChannelName.Items.Insert(0, new ListItem("Select", "0"));
            //FillsalesChannelType();
            //cmbChannelType.SelectedValue = "6";
            //cmbChannelType.Enabled = false;
            //cmbChannelType_SelectedIndexChanged(sender, null);
        }


    }
    //void FillsalesChannelType()
    //{
    //    using (SalesChannelData obj = new SalesChannelData())
    //    {
    //        DataTable dt = obj.GetSalesChannelType();
    //        String[] colArray = { "SalesChannelTypeID", "SalesChannelTypeName" };
    //        PageBase.DropdownBinding(ref cmbChannelType, dt, colArray);

    //    }
    //}
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        
        if ((txtRetailerCode.Text == "") && (ucDateTo.Date == ""))
        {
            ucMessage1.ShowInfo("Enter searching parameter(s)");
            return;
        }
           
        
        if (!Validation())
        {
            ucMessage1.ShowInfo("Invalid Date Range");
            return;
        }
        //if (cmbChannelType.SelectedIndex != 0)
        //{
            //btnCheck.Enabled = false;
            DataTable DtData = new DataTable();
            using (SalesChannelData obj = new SalesChannelData())
            {
                //obj.SalesChannelID = Convert.ToInt32(ddlSalesChannelName.SelectedValue);
                obj.RetailerCode = txtRetailerCode.Text;
                obj.OrderFromDate = ucDateFrom.Date == "" ? obj.OrderFromDate : Convert.ToDateTime(ucDateFrom.Date);
                obj.OrderToDate = ucDateTo.Date == "" ? obj.OrderToDate : Convert.ToDateTime(ucDateTo.Date);
                obj.LoggedInSalesChannelID = PageBase.SalesChanelID;
                if (ucDateTo.Date!="")
                { obj.SearchType1 = 2; 
                        }
                
                //obj.StatusValue = Convert.ToInt32(ddlStatus.SelectedValue);
                DataTable dt = obj.GetSalesChannelOrderInfoV1();
                if (dt != null && dt.Rows.Count > 0)
                {
                    //PnlHide.Visible = true;
                    //pnlGrid.Visible = true;
                    dvAction.Visible = true;
                    dvheading.Visible = true;
                    gvOrder.DataSource = dt;
                    gvOrder.DataBind();

                }
                else
                {
                    //PnlHide.Visible = false;
                    gvOrder.DataSource = null;
                    gvOrder.DataBind();
                    //pnlGrid.Visible = false;
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                }
                UpdatePanel1.Update();

            }
        //}
        //else
        //{
        //    ucMessage1.ShowInfo(Resources.Messages.MandatoryField);
        //}
    }


    protected void btnApprove_Click(object sender, EventArgs e)
    {
        object objSum;
        DataTable DtOrder = new DataTable();
        DataTable dtNew;
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
            DtDetail = ObjCommom.GettvpOrderInformation();
        }

        for (int i = 0; i <= gvDetail.Rows.Count - 1; i++)
        {
            Label lblSKUID = (Label)gvDetail.Rows[i].FindControl("lblSKUID");
            TextBox txtApprovedQuantity = (TextBox)gvDetail.Rows[i].FindControl("txtApprovedQuantity");
            DataRow dr = DtDetail.NewRow();
           // dr["OrderFromSalesChannelID"] = Convert.ToInt32(ddlSalesChannelName.SelectedValue);
            dr["ApprovedQuantity"] = Convert.ToInt32(txtApprovedQuantity.Text);
            dr["SKUID"] = Convert.ToInt32(lblSKUID.Text);
            dr["OrderID"] = Convert.ToInt32(ViewState["OrderId"]);
            DtDetail.Rows.Add(dr);
        }


        DtDetail.AcceptChanges();
        dtNew = DtDetail.Clone();
        foreach (DataColumn dc in dtNew.Columns)
        {
            if (dc.DataType == typeof(string) && dc.ColumnName == "ApprovedQuantity")
            {
                dc.DataType = typeof(System.Int32);
                break;
            }
        }
        foreach (DataRow dr in DtDetail.Rows)
        {
            dtNew.ImportRow(dr);
        }
        objSum = dtNew.Compute("sum(ApprovedQuantity)", "");
        if (Convert.ToInt32(objSum) <= 0)
        {
            ucMessage1.ShowInfo("Please Insert right Quantity");
            return;
        }
        using (SalesChannelData ObjOrder = new SalesChannelData())
        {
            Int16 result;
            ObjOrder.Error = "";
            ObjOrder.StatusValue = 2;
            ObjOrder.Flag = 2;
            ObjOrder.OrderID = Convert.ToInt32(ViewState["OrderId"]);
            result = ObjOrder.InsertOrderInformationV1(DtDetail);
            if (result == 1)
            {
                ucMessage1.ShowError(ObjOrder.Error);
                return;
            }

            ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
            ClearForm();
         
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        using (SalesChannelData ObjOrder = new SalesChannelData())
        {
            Int16 result;
            ObjOrder.Error = "";
            ObjOrder.StatusValue = 3;
            ObjOrder.Flag = 3;
            ObjOrder.OrderID = Convert.ToInt32(ViewState["OrderId"]);
            DataTable DtDetail = new DataTable();
            using (CommonData ObjCommom = new CommonData())
            {
                DtDetail = ObjCommom.GettvpOrderInformation();
            }
            result = ObjOrder.InsertOrderInformationV1(DtDetail);
            if (result == 1)
            {
                ucMessage1.ShowError(ObjOrder.Error);
                return;
            }
            ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
            //pnlGrid.Visible = false;
            ClearForm();
          
        }

    }
    void ClearForm()
    {
        dvAction.Visible = false;
        dvheading.Visible = false;
        ucDateFrom.Date = "";
        ucDateTo.Date = "";
        dvDetail.Visible = false;
        //cmbChannelType.SelectedValue = "0";
        gvOrder.DataSource = null;
        gvOrder.DataBind();
        gvDetail.DataSource = null;
        gvDetail.DataBind();
        //ddlSalesChannelName.Items.Clear();
        //ddlSalesChannelName.Items.Insert(0,new ListItem("Select","0"));
        UpdatePanel1.Update();
        updgrid.Update();
        updDetail.Update();
    }
    bool PageValidatesave()
    {

        //if (ucDatePicker.Date == "")
        //{
        //    ucMessage1.ShowInfo(Resources.Messages.MandatoryField);
        //    return false;
        //}
        //if (Convert.ToDateTime(ucDatePicker.Date) > System.DateTime.Now)
        //{
        //    ucMessage1.ShowInfo(Resources.Messages.DateRangeValidation);
        //    return false;
        //}
        //if (Convert.ToDateTime(ucDatePicker.Date) < Convert.ToDateTime(lblOpeningdate.Text.Trim()))
        //{
        //    ucMessage1.ShowInfo(Resources.Messages.StockAdjustmentDateValidation);
        //    return false;
        //}
        return true;
    }
    protected void gvOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        {
            DataTable DtSalesData = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdD = (HiddenField)e.Row.FindControl("hdnid");

                TextBox txtQuantityFooter = (TextBox)e.Row.FindControl("txtQuantity");
                Label lblSkuID = (Label)e.Row.FindControl("lblSKUID");
                Label lblStockInhand = (Label)e.Row.FindControl("lblStockInhand");

                //txtQuantityFooter.Attributes.Add("OnChange", "StockCheckAdjustment(this);");



            }
        }
    }
    private bool Validation()
    {

        if (txtRetailerCode.Text == "" && ucDateTo.Date == "")
        { return false; 
        }
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
        
        //if (ucDateTo.Date != "" && ucDateFrom.Date != "")
        //{
        //    if (Convert.ToDateTime(ucDateTo.Date) > Convert.ToDateTime(ucDateFrom.Date))
        //    {
        //        return false;
        //    }
        //}

        return true;

    }
    protected void gvDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblOrderedQuantity = (Label)e.Row.FindControl("lblOrderedQuantity");
                if (lblOrderedQuantity.Text != "")
                {
                    RangeValidator rvQty = ((RangeValidator)e.Row.FindControl("valReceQty"));
                    rvQty.MinimumValue = "0";
                    rvQty.MaximumValue = lblOrderedQuantity.Text;
                    rvQty.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
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
                    //obj.SalesChannelID = Convert.ToInt32(ddlSalesChannelName.SelectedValue);
                    /*obj.OrderFromDate = ucDateFrom.Date == "" ? obj.OrderFromDate : Convert.ToDateTime(ucDateFrom.Date);
                    obj.OrderToDate = ucDateTo.Date == "" ? obj.OrderToDate : Convert.ToDateTime(ucDateTo.Date);
                    obj.LoggedInSalesChannelID = PageBase.SalesChanelID;
                    obj.OrderID = id;
                    obj.Flag = 1;*/
                    obj.OrderID = id;
                    DataTable dt = obj.GetSalesChannelOrderDetailInfoV1();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0]["Status"]) == 2)
                        {
                            btnApprove.Enabled = false;
                            btnReject.Enabled = false;
                        }
                        else
                        {
                            btnApprove.Enabled = true;
                            btnReject.Enabled = true;
                        }
                        //pnlGrid.Visible = true;
                        dvDetail.Visible = true;
                        gvDetail.DataSource = dt;
                        gvDetail.DataBind();
                        dvDetail.Visible = true;
                        updDetail.Update();
                        dvAction.Visible = true;
                        dvheading.Visible = true;
                        UpdatePanel1.Update();

                    }
                    else
                    {
                        gvDetail.DataSource = null;
                        gvDetail.DataBind();
                       // pnlGrid.Visible = false;
                        ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                    }
                }
            }
        }

        catch (Exception ex)
        {
        }
    }
   /* protected void cmbChannelType_SelectedIndexChanged(object sender, EventArgs e)
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
    }*/
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearForm();
    }
}
