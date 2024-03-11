﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;

public partial class Transactions_SalesChannel_PrimaryOrderV2 : PageBase
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["CurrentPage"] = 1; 
            ucMsg.ShowControl = false;
            string str = string.Format("return hithereforOutwards({0})", grdSalesChannel.ID);
           
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
            //if (PageBase.IsPrimaryOrderNoAutogenerate == 0)
            //{
            //    txtOrderNo.Enabled = true;
            //}
            //else
            //{
            //    txtOrderNo.Text = "Autogenerated";
            //    txtOrderNo.Enabled = false;
            //}

            if (!IsPostBack)
            {
                FillParentsalesChannel();
                Button1.Attributes.Add("onclick", "return hithereforOutwards('" + grdSalesChannel.ID + "','" + Button1.ID + "')");
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
   
    #region Button Event
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            fillcombos();
            pnlSearch.Visible = true;
            UpdSearch.Update();
            //if (!pageValidateGo())
            //{
            //    return;
            //}
            //if (ucDatePicker.Date == "" )
            //{
            //    ucMsg.ShowInfo(Resources.Messages.InvalidDate);
            //    return;
            //}
            //using (SalesData objSales = new SalesData())
            //{
            //    objSales.SalesChannelID = PageBase.SalesChanelID;
            //    objSales.Brand = PageBase.Brand;
            //    objSales.InvoiceDate = Convert.ToDateTime(ucDatePicker.Date);
            //    DataTable dtOrder = objSales.GetOrderDetail();
            //    if (dtOrder != null && dtOrder.Rows.Count > 0)
            //    {
            //        ddlWarehouse.Enabled = false;

            //    }
            //    else
            //    {
            //       ddlWarehouse.Enabled = true;
            //    }
            //    grdSalesChannel.DataSource = dtOrder;
            //    grdSalesChannel.DataBind();
            //    pnlGrid.Visible = true;

            //}
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }


    public void finddata()
    {
        int k = 3;
        string[] str = new string[10];
        int i = 1;
        DataTable dt = new DataTable();
        DataColumn[] dc = new DataColumn[k];

        for (int c = 0; c < k; c++)
        {
            dc[c] = new DataColumn(c.ToString());
        }
        dt.Columns.AddRange(dc);
        while (Page.Request.Form[string.Format("TextBox{0}1", i)] != null)
        {
            DataRow dr = dt.NewRow();
            for (int c = 0; c < k; c++)
            {
                dr[c.ToString()] = Page.Request.Form[string.Format("TextBox{0}{1}", i, (c + 1))];
            }
           
            i++;
            dt.Rows.Add(dr);
        }
       DataTable dt1  = getfinaltable(dt);
         ViewState["Table1"] = dt1;

    }


    public DataTable getfinaltable(DataTable dt)
    {
        DataTable dtout = new DataTable();
        DataColumn[] dc = new DataColumn[3];
        dc[0] = new DataColumn("Quantity");
        dc[1] = new DataColumn("Amount");
        dc[2] = new DataColumn("SKUCode");
        dtout.Columns.AddRange(dc);
        foreach (DataRow dr in dt.AsEnumerable())
        {
            DataRow dr1 = dtout.NewRow();
            dr1["Quantity"] = dr["0"].ToString();
            dr1["Amount"] = dr["1"].ToString();
            dr1["SKUCode"] = dr["2"].ToString();
            dtout.Rows.Add(dr1);
        }
        return dtout;
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        
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
    //bool pageValidateGo()
    //{

    //    //if (txtOrderNo.Text.Trim() == string.Empty)
    //    //{
    //    //    ucMsg.ShowWarning(Resources.Messages.EnterInvoiceNo);
    //    //    return false;
    //    //}
    //    //return true;
    //}
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

    protected void btnFind_Click(object sender, EventArgs e)
    {
        finddata();
        pnlGrid.Visible = false;
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

            DataTable Dt = (DataTable)ViewState["Table1"];

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

                ObjSales.InsertPrimaryOrderInfo(DtDetail);

                if (ObjSales.Error != null && ObjSales.Error != "")
                {
                    ucMsg.ShowInfo(ObjSales.Error);
                    return;
                }

                string s = string.Format("The order {0} created sucessfully", ObjSales.OrderNumber);
                ucMsg.ShowSuccess(s);
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


    public void fillcombos()
    {
        using (ProductData objproduct = new ProductData())
        {

            try
            {
                DataTable dt = objproduct.SelectAllProdCatInfo();
                
                
                cmbSerProdCat.Items.Clear();
                String[] colArray1 = { "ProductCategoryID", "ProductCategoryName" };
                PageBase.DropdownBinding(ref cmbSerProdCat, dt, colArray1);
                cmbSerModel.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }

        }
    }


    //public void BindList()
    //{

    //    using (SalesData objSales = new SalesData())
    //    {
    //        objSales.SalesChannelID = PageBase.SalesChanelID;
    //        objSales.Brand = PageBase.Brand;
    //        objSales.InvoiceDate = Convert.ToDateTime(ucDatePicker.Date);
    //        objSales.SKUCode = txtSerCode.Text;
    //        objSales.SKUName = txtSerName.txt;
    //        objSales.ProductCategoryID = cmbSerProdCat.SelectedValue;
    //        DataTable dtOrder = objSales.GetOrderDetail();
    //        if (dtOrder != null && dtOrder.Rows.Count > 0)
    //        {
    //            ddlWarehouse.Enabled = false;

    //        }
    //        else
    //        {
    //            ddlWarehouse.Enabled = true;
    //        }
    //        grdSalesChannel.DataSource = dtOrder;
    //        grdSalesChannel.DataBind();
    //        pnlGrid.Visible = true;

    //    }

    //}


    //void BindList(int index)
    //{
    //    try
    //    {
    //        using (clsCityMaster objcity = new clsCityMaster())
    //        {
    //            objcity.StateID = Convert.ToInt16(ddlState.SelectedValue);
    //            objcity.PageSize = PageSize;
    //            objcity.PageIndex = index;
    //            objcity.ActiveStatus = 2;
    //            DataTable dt;
    //            if (objcity.StateID == 0)
    //                dt = objcity.SelectAll();
    //            else
    //                dt = objcity.SelectByStateId();

    //            if (dt.Rows.Count == 0 && index > 1)
    //            {
    //                index--;
    //                objcity.PageIndex = index;
    //                hdfCurrentPage.Value = Convert.ToString(index);
    //                if (objcity.StateID == 0)
    //                    dt = objcity.SelectAll();
    //                else
    //                    dt = objcity.SelectByStateId();
    //            }
    //            grdvList.Visible = true;
    //            grdvList.DataSource = dt;
    //            grdvList.DataBind();
    //            if (dt == null || dt.Rows.Count == 0)
    //            {
    //                ucPagingControl1.Visible = false;
    //            }
    //            else
    //            {
    //                //Paging
    //                ucPagingControl1.CurrentPage = index;
    //                ucPagingControl1.Visible = true;
    //                ucPagingControl1.PageSize = PageSize;
    //                ucPagingControl1.TotalRecords = objcity.TotalRecords;
    //                ucPagingControl1.FillPageInfo();
    //            }
    //            updpnlGrid.Update();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucMessage1.ShowWarning(Resources.AdminMessages.ExceptionError);
    //        updpnlSaveData.Update();
    //        ExceptionPolicy.HandleException(ex, ExceptionPolicyName);
    //    }
    //}







    //protected void UCPagingControl1_SetControlRefresh()
    //{
    //    try
    //    {
    //        hdfCurrentPage.Value = Convert.ToString(ucPagingControl1.CurrentPage);
    //        BindList(ucPagingControl1.CurrentPage);
    //    }
    //    catch (Exception ex)
    //    {
    //        ucMessage1.ShowWarning(Resources.AdminMessages.ExceptionError);
    //        updpnlSaveData.Update();
    //        ExceptionPolicy.HandleException(ex, ExceptionPolicyName);
    //    }
    //}


    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        GetSearchData(ucPagingControl1.CurrentPage);
        

    }


    protected void btnSerchSku_Click(object sender, EventArgs e)
    {
        GetSearchData(1);
        pnlGrid.Visible = true;
        updGrid.Update();

    }


    public void GetSearchData(int pageno)
    {
        using (ProductData obj = new ProductData())
        {
            obj.SKUProdCatId = Convert.ToInt32(cmbSerProdCat.SelectedValue);
            obj.SKUModelId = Convert.ToInt32(cmbSerModel.SelectedValue);
            obj.SKUName = txtSerName.Text.ToString();
            obj.SKUCode = txtSerCode.Text.ToString();
            obj.PageNo = 1;
            obj.PageSize = 5;
            DataTable dt = obj.SelectSKUBlockwiseInfo();
            ucPagingControl1.TotalRecords = obj.Elements;
            ucPagingControl1.PageSize = 5;
            ucPagingControl1.SetCurrentPage = pageno;
            grdSalesChannel.DataSource = dt;
            grdSalesChannel.DataBind();
            pnlGrid.Visible = true;
        }
    }


    protected void cmbSerProdcat_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (ProductData objproduct = new ProductData())
        {

            try
            {
                if (cmbSerProdCat.SelectedValue == "0")
                {
                    cmbSerModel.Items.Clear();
                    cmbSerModel.Items.Insert(0, new ListItem("Select", "0"));
                    cmbSerModel.SelectedValue = "0";
                }
                else
                {
                    objproduct.ModelProdCatId = Convert.ToInt16(cmbSerProdCat.SelectedValue.ToString());
                    objproduct.ModelSelectionMode = 1;
                    DataTable dtmodelfil = objproduct.SelectModelInfo();
                    String[] colArray1 = { "ModelID", "ModelName" };
                    PageBase.DropdownBinding(ref cmbSerModel, dtmodelfil, colArray1);
                    cmbSerModel.SelectedValue = "0";
                    UpdSearch.Update();

                }
            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            }
        }
    }
}

