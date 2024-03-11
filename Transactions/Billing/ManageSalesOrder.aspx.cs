#region Copyright(c) 2010 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Vijay Kumar Prajapati
* Module :      04-March-2019
* Description : 
 * Table Name: 
* ====================================================================================================
* Reviewed By :
 ====================================================================================================
Modification On       Modified By          Modification    
---------------      -----------          -------------------------------------------------------------  
 * ====================================================================================================
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using ExportExcelOpenXML;
using System.Text;
using BusinessLogics;
using System.Collections;
using System.Reflection;
using System.Data.SqlClient;
using ZedService;
using System.IO;

public partial class Transactions_Billing_ManageSalesOrder : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                fillOrderFrom();
                cmbOrderTo1.Items.Insert(0, new ListItem("Select", "0"));
                ViewState["PurchsedInvSalesType"] = null;
                AutoCompleteExtenderPartName.ServicePath = PageBase.siteURL + "CommonService.asmx";
                AutoCompleteExtenderPartCode.ServicePath = PageBase.siteURL + "CommonService.asmx";
                div2.Visible = false;
                div3.Visible = false;
                ucOrderDate.Date = PageBase.ToDate;
                ucOrderDate.IsEnabled = false;
                ScriptManager.GetCurrent(this.Page).Scripts.Add(new ScriptReference(PageBase.siteURL + "/Assets/JScript/DevExValidationCampatibility.js"));
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }

    }
    protected void DownloadProductPartMappingList_Click(object sender, EventArgs e)
    {
        try
        {
            using (clsSalesOrder ObjProductPartMapping = new clsSalesOrder())
            {

                DataSet dt = ObjProductPartMapping.GetSKUList();
                PageBase.ExportToExecl(dt, "SKU List");
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindList(1, 1);
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
    public void BindList(int index, int Suggindex)
    {
        using (clsSalesOrder obj = new clsSalesOrder())
        {

            obj.PageIndex = index == 0 ? 1 : index;
            obj.SuggPageIndex = Suggindex == 0 ? 1 : Suggindex;
            obj.PageSize = 5;
            if (hdnfOrderToId.Value == "")
            {
                obj.ToID = Convert.ToInt32(cmbOrderTo1.SelectedValue);
            }
            else
            {
                obj.ToID = Convert.ToInt32(hdnfOrderToId.Value);
            }

            obj.FromID = Convert.ToInt32(cmbOrderFrom.SelectedValue);
            obj.EntityId = PageBase.SalesChanelID;
            obj.PartName = txtSerPartName.Text.Trim();
            obj.PartCode = txtPartCode.Text.Trim();
            obj.ProductName = txtModelNumber.Text.Trim();
            obj.PartType = 1;
            obj.OrderFromEntitytypeId = Convert.ToInt32(cmbOrderFrom.Attributes["saleschanneltypeid" + cmbOrderFrom.SelectedValue]);
            obj.OrderToEntityTypeId = Convert.ToInt32(cmbOrderTo1.Attributes["saleschanneltypeid" + cmbOrderTo1.SelectedValue]);
            DataSet ds = obj.SelectPartInfoForOrder();
            DataTable dt = new DataTable();
            DataTable dtSugg = new DataTable();
            if (obj.Ledgermaintainedbysystem == 0)
            {

                Currentledgerbalanceid.Attributes.Add("style", "display:none;");
                lblCurrLedgerBal.Attributes.Add("style", "display:none;");
            }
            else
            {
                lblCurrLedgerBal.Text = Convert.ToDecimal(obj.EntityCurrLegBal).ToString("0.00");
            }

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                if (ds.Tables.Count > 1)
                {
                    dtSugg = ds.Tables[1];
                }
            }
            grdvList.DataSource = dt;
            grdvList.DataBind();
            divgrd.Visible = true;
            if (dt == null || dt.Rows.Count == 0)
            {
                ucPagingControl1.Visible = false;
            }
            else
            {
                ucPagingControl1.Visible = true;
                ucPagingControl1.PageSize = 5;
                ucPagingControl1.TotalRecords = obj.TotalRecords;
                ucPagingControl1.FillPageInfo();
            }
            div2.Visible = true;
            updSearch.Update();
        }
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        try
        {

            int intPageNumber = ucPagingControl1.CurrentPage;
            ViewState["PageIndex"] = intPageNumber;

            BindList(ucPagingControl1.CurrentPage, Convert.ToInt32(ViewState["PageIndexSugg"]));

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
    protected void btnLoad_Click(object sender, EventArgs e)
    {
        try
        {

            FreezeInitials(0);

            BindList(1, 1);
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
    protected void btnCancelGrd_Click(object sender, EventArgs e)
    {
        try
        {
            grdvPart.DataSource = null;
            grdvPart.DataBind();
            divlast.Visible = false;
            div3.Visible = false;
            ViewState["dtAddtolist"] = null;
            ucMessage1.Visible = false;
            divMsg.Visible = false;
            foreach (GridViewRow gvrow in grdvList.Rows)
            {
                    ((TextBox)(gvrow.FindControl("txtQuantity"))).Text = "";
            }
            UpdPnlPart.Update();
            updSearch.Update();

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable dtSavelist = new DataTable();

        dtSavelist = (DataTable)ViewState["dtAddtolist"];


        if (dtSavelist != null)
        {
            try
            {
                ucMessage1.Visible = false;
                if (dtSavelist.Columns.Contains("PartID"))
                    dtSavelist.Columns["PartID"].SetOrdinal(0);

                if (dtSavelist.Columns.Contains("Quantity"))
                    dtSavelist.Columns["Quantity"].SetOrdinal(1);

                if (dtSavelist.Columns.Contains("GrossAmount"))
                    dtSavelist.Columns["GrossAmount"].SetOrdinal(2);
                if (dtSavelist.Columns.Contains("NetAmount"))
                    dtSavelist.Columns["NetAmount"].SetOrdinal(3);


                if (dtSavelist.Columns.Contains("Discount"))
                    dtSavelist.Columns["Discount"].SetOrdinal(4);

                if (dtSavelist.Columns.Contains("UOMID"))
                    dtSavelist.Columns["UOMID"].SetOrdinal(5);

                dtSavelist.Columns.Remove("SKUName");
                dtSavelist.Columns.Remove("SKUCode");
                dtSavelist.AcceptChanges();

                using (clsSalesOrder Obj = new clsSalesOrder())
                {
                    Obj.Error = "";
                    Obj.CreatedBy = UserId;

                    Obj.FromID = Convert.ToInt32(cmbOrderFrom.SelectedValue);
                    Obj.ToID = Convert.ToInt32(cmbOrderTo1.SelectedValue);
                    Obj.PONumber = txtPoNumber.Text;
                    Obj.Remarks = txtRemarks.TextBoxText;
                    Obj.Orderdate = ucOrderDate.Date;
                    Obj.strSalesInvoiceXML = "<NewDataSet><Table></Table></NewDataSet>";
                    Obj.OrignalInvNo = "";
                    Obj.BaseEntityTypeId = Convert.ToInt16(PageBase.BaseEntityTypeID);
                    Obj.ToBaseEntityTypeId = Convert.ToInt16(PageBase.BaseEntityTypeID);
                    Obj.OrderToEntityTypeId = Convert.ToInt32(cmbOrderTo1.Attributes["saleschanneltypeid" + cmbOrderTo1.SelectedValue]);
                    if (btnSave.Text == "Save")
                    {
                        Obj.InsertSalesOrderInfo(dtSavelist);

                        if (Obj.strSalesInvoiceXML != null && Obj.strSalesInvoiceXML != string.Empty)
                        {
                            ucMessage1.XmlErrorSource = Obj.strSalesInvoiceXML;
                            return;
                        }
                        if (Obj.Error != null && Obj.Error != "" && Obj.Result > 0)
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowInfo(Obj.Error);

                            return;
                        }
                        ucMessage1.Visible = true;
                        ucMessage1.ShowSuccess("Order Created Successfully.");

                        divMsg.Attributes.Add("style", "display:block");
                        hlkFinal.Text = "Click here to print order detail  " + Obj.OrderNumber;
                        hlkFinal.Attributes.Add("OnClick", string.Format("return popup({0})", Obj.OrderId));


                    }
                    grdvPart.Visible = false;
                    divlast.Visible = false;
                    divMsg.Visible = true;
                    FreezeInitials(1);
                    cmbOrderFrom.Enabled = true;
                    cmbOrderTo1.Enabled = true;
                    ViewState["dtAddtolist"] = null;
                    div3.Visible = false;
                    foreach (GridViewRow gvrow in grdvList.Rows)
                    {
                        ((TextBox)(gvrow.FindControl("txtQuantity"))).Text = "";
                    }
                    updSearch.Update();
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.ToString());
            }
        }
        else
        {
            ucMessage1.Visible = true;

        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {

            txtPartCode.Text = "";
            txtSerPartName.Text = "";
            BlankInsert();
            cmbOrderFrom.Enabled = true;
            cmbOrderTo1.Enabled = true;
            ucOrderDate.Date = DateTime.Now.ToString("MM/dd/yyyy");
            ucOrderDate.IsEnabled = false;
            cmbOrderFrom.Items.Clear();
            cmbOrderFrom.Items.Insert(0, new ListItem("Select", "0"));
            fillOrderFrom();
            cmbOrderTo1.Items.Clear();
            cmbOrderTo1.Items.Insert(0, new ListItem("Select", "0"));
            div3.Visible = false;
            ucMessage1.Visible = false;
            divMsg.Visible = false;

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
    public void BlankInsert()
    {

        FreezeInitials(1);
        txtPoNumber.Text = "";
        txtRemarks.TextBoxText = "";
        cmbOrderFrom.Enabled = true;
        div2.Visible = false;

        updSearch.Update();

    }
    public void FreezeInitials(int i)
    {
        if (i == 0)
        {
            if (ViewState["SelectionBitMode"] == null)
            {
                cmbOrderFrom.Enabled = false;
            }
            cmbOrderTo1.Enabled = false;
            txtRemarks.Enabled = false;
            txtPoNumber.Enabled = false;

        }
        else
        {
            if (ViewState["SelectionBitMode"] == null)
            {
                cmbOrderFrom.Enabled = true;
            }
            cmbOrderTo1.Enabled = true;
            txtRemarks.Enabled = true;
            txtPoNumber.Enabled = true;
        }
    }
    protected void grdvList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                Label lblPartId = (Label)e.Row.FindControl("Label51");
                CheckBox chkboxPartID = (CheckBox)e.Row.FindControl("chkboxPartID");
                TextBox txtQuantity = (TextBox)e.Row.FindControl("txtQuantity");
                HiddenField hdfGrossAMount = (HiddenField)e.Row.FindControl("hdfGrossAmount");
                HiddenField hdfnetAmount = (HiddenField)e.Row.FindControl("hdfnetAmount");
                RequiredFieldValidator rfvQuantity = (RequiredFieldValidator)e.Row.FindControl("rfvQuantity");
                chkboxPartID.Attributes.Add("onClick", "javascript:OnCheckUnCheck('" + chkboxPartID.ClientID + "','" + txtQuantity.ClientID + "','" + rfvQuantity.ClientID + "')");


            }

            updSearch.Update();

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }

    public void fillOrderFrom()
    {
        using (SalesChannelData objSaleschannel = new SalesChannelData())
        {
            try
            {
                cmbOrderFrom.Items.Clear();
                DataTable dt;
                if (BaseEntityTypeID == 1)
                    objSaleschannel.SalesChannelID = PageBase.SalesChanelID;
                if (BaseEntityTypeID == 3)
                    objSaleschannel.RetailerID = 0;
                objSaleschannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                objSaleschannel.UserID = PageBase.UserId;
                objSaleschannel.BaseEntityTypeID = Convert.ToInt16(PageBase.BaseEntityTypeID);
                objSaleschannel.GetParentOrChild = 0;

                dt = objSaleschannel.GetParentSalesChannel();
                cmbOrderFrom.DataSource = dt;
                cmbOrderFrom.DataTextField = "SalesChannelName";
                cmbOrderFrom.DataValueField = "SalesChannelID";
                cmbOrderFrom.DataBind();
                cmbOrderFrom.Items.Insert(0, new ListItem("Select", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbOrderFrom.Attributes.Add("SalesChannelTypeID" + dt.Rows[i]["SalesChannelID"].ToString(), dt.Rows[i]["SalesChannelTypeID"].ToString());
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    public void fillOrderTo()
    {
        using (SalesChannelData objSaleschannel = new SalesChannelData())
        {
            try
            {
                if (Convert.ToInt32(cmbOrderFrom.SelectedValue) > 0)
                {
                    cmbOrderTo1.Items.Clear();
                    DataTable dt;
                    if (BaseEntityTypeID == 1)
                        objSaleschannel.SalesChannelID = Convert.ToInt32(cmbOrderFrom.SelectedValue);
                    if (BaseEntityTypeID == 3)
                        objSaleschannel.RetailerID = Convert.ToInt32(cmbOrderFrom.SelectedValue);
                    objSaleschannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                    objSaleschannel.UserID = PageBase.UserId;
                    objSaleschannel.BaseEntityTypeID = Convert.ToInt16(PageBase.BaseEntityTypeID);
                    objSaleschannel.GetParentOrChild = 1;
                    dt = objSaleschannel.GetParentSalesChannel();
                    cmbOrderTo1.DataSource = dt;
                    cmbOrderTo1.DataTextField = "SalesChannelName";
                    cmbOrderTo1.DataValueField = "SalesChannelID";
                    cmbOrderTo1.DataBind();
                    cmbOrderTo1.Items.Insert(0, new ListItem("Select", "0"));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cmbOrderTo1.Attributes.Add("SalesChannelTypeID" + dt.Rows[i]["SalesChannelID"].ToString(), dt.Rows[i]["SalesChannelTypeID"].ToString());
                    }

                }
                else
                {
                    cmbOrderTo1.Items.Clear();
                    cmbOrderTo1.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    protected void cmbOrderFrom_SelectedIndexChanged1(object sender, EventArgs e)
    {
        fillOrderTo();
    }
    protected void btnAddtolist_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtAddtolist;
            if (ViewState["dtAddtolist"] != null)
                dtAddtolist = (DataTable)ViewState["dtAddtolist"];
            else
            {
                dtAddtolist = new DataTable();
                dtAddtolist.Columns.Add("PartId", typeof(Int64));
                dtAddtolist.Columns.Add("SKUName", typeof(string));
                dtAddtolist.Columns.Add("SKUCode", typeof(string));
                dtAddtolist.Columns.Add("Quantity", typeof(Int64));
                dtAddtolist.Columns.Add("GrossAmount", typeof(decimal));
                dtAddtolist.Columns.Add("NetAmount", typeof(decimal));
                dtAddtolist.Columns.Add("Discount", typeof(decimal));
                dtAddtolist.Columns.Add("UOMID", typeof(Int64));
                dtAddtolist.Columns.Add("PromotionDetailSKUID", typeof(Int64));
                dtAddtolist.Columns.Add("SuggQty", typeof(Int64));
                dtAddtolist.Columns.Add("FreeSku", typeof(Int16));
                dtAddtolist.AcceptChanges();
            }
            CheckBox chkaddlist = new CheckBox();
            string orderskuid = string.Empty;
            int Quantity = 0;
            string price = "";
            decimal netprice = 0;
            foreach (GridViewRow grv in grdvList.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    chkaddlist = (grv.FindControl("chkboxPartID") as CheckBox);

                    if (chkaddlist.Checked)
                    {
                        Label lblSkuname = (Label)grv.FindControl("lblPartname") as Label;
                        Label lblSkucode = (Label)grv.FindControl("lblPartCode") as Label;
                        TextBox lblOrderQuantity = (TextBox)grv.FindControl("txtQuantity") as TextBox;
                        TextBox txtprice = (TextBox)grv.FindControl("txtPrice") as TextBox;
                        Label lblSKUId = (Label)grv.FindControl("Label51") as Label;
                        Label lblNetPrice = (Label)grv.FindControl("lblTaxPrice") as Label;
                        orderskuid = grdvList.DataKeys[grv.RowIndex].Value.ToString();
                        if (dtAddtolist.Select("PartId=" + orderskuid).Length > 0)
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowWarning(lblSkucode.Text + " SKU already added for order.");
                            return;

                        }
                        if (lblOrderQuantity.Text != "")
                        {
                            if (lblOrderQuantity.Text != "0")
                            {
                                if (lblSKUId.Text != "")
                                {
                                    orderskuid = lblSKUId.Text;
                                }

                                if (lblOrderQuantity.Text != "")
                                {
                                    Quantity = Convert.ToInt32(lblOrderQuantity.Text);
                                }
                                else
                                {
                                    Quantity = 0;
                                }
                                if (txtprice.Text != "")
                                {
                                    price = txtprice.Text;
                                }
                                else
                                {
                                    price = "0";
                                }
                                if (lblNetPrice.Text != "")
                                {
                                    netprice = Convert.ToDecimal(lblNetPrice.Text);
                                }
                                else
                                {
                                    netprice = 0;
                                }
                                decimal Amount = Convert.ToDecimal(Quantity) * Convert.ToDecimal(price);
                                DataRow dr = null;
                                dr = dtAddtolist.NewRow();
                                dr["PartId"] = Convert.ToInt64(orderskuid);
                                dr["SKUName"] = lblSkuname.Text;
                                dr["SKUCode"] = lblSkucode.Text;
                                dr["Quantity"] = Quantity;
                                dr["GrossAmount"] = Amount;
                                dr["NetAmount"] = netprice;
                                dr["Discount"] = 0;
                                dr["UOMID"] = 0;
                                dr["PromotionDetailSKUID"] = 0;
                                dr["SuggQty"] = 0;
                                dr["FreeSku"] = 0;
                                dtAddtolist.Rows.Add(dr);
                                dtAddtolist.AcceptChanges();
                            }
                            else
                            {
                                ucMessage1.Visible = true;
                                ucMessage1.ShowWarning("Quantity should be greater than Zero.");
                                return;
                            }
                        }
                        else
                        {
                            ucMessage1.Visible = true;
                            ucMessage1.ShowWarning("Quantity can not be blank.");
                            return;

                        }
                    }
                    if (orderskuid == string.Empty && orderskuid != null)
                    {
                        ucMessage1.Visible = true;
                        ucMessage1.ShowWarning("No record in list for Add to List.");
                    }
                }
            }
            ViewState["dtAddtolist"] = dtAddtolist;
            if (dtAddtolist.Rows.Count > 0)
            {
                divlast.Visible = true;
                div3.Visible = true;
                ucMessage1.Visible = false;
                grdvPart.DataSource = dtAddtolist;
                grdvPart.DataBind();
                CheckState(false);
                UpdPnlPart.Update();
            }
            else
                divlast.Visible = false;
        }
        catch (Exception ex)
        {

            ucMessage1.ShowInfo(ex.Message.ToString());
        }
    }
    protected void grdvPart_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            DataTable dtAddtolist = (DataTable)ViewState["dtAddtolist"];
            DataRow[] dr = dtAddtolist.Select("PartId=" + e.CommandArgument.ToString());
            dtAddtolist.Rows.Remove(dr[0]);
            dtAddtolist.AcceptChanges();
            ViewState["dtAddtolist"] = dtAddtolist;
            if (dtAddtolist.Rows.Count > 0)
            {
                divlast.Visible = true;
                div3.Visible = true;
                ucMessage1.Visible = false;
                grdvPart.DataSource = dtAddtolist;
                grdvPart.DataBind();
                UpdPnlPart.Update();
            }
            else
            {
                grdvPart.DataSource = null;
                grdvPart.DataBind();
                divlast.Visible = false;
                div3.Visible = false;
                UpdPnlPart.Update();
            }
            foreach (GridViewRow gvrow in grdvList.Rows)
            {
                ((TextBox)(gvrow.FindControl("txtQuantity"))).Text = "";
            }
            updSearch.Update();

        }
    }
    private void CheckState(bool p)
    {
        foreach (GridViewRow row in grdvList.Rows)
        {
            CheckBox chkcheck = (CheckBox)row.FindControl("chkboxPartID");
            chkcheck.Checked = p;
        }
    }
}
