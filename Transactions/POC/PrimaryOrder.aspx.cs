using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;
public partial class Transactions_POC_PrimaryOrder : PageBase
{
    DataTable DtDetail = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            uclblMessage.ShowControl = false;
            ucDatePicker.MinRangeValue = System.DateTime.Now;
            ucDatePicker.MaxRangeValue = System.DateTime.Now;
            ucDatePicker.RangeErrorMessage = "Invalid Date Range";
            if (!IsPostBack)
            {
                pnl1.Visible = false;
                Panel1.Visible = false;
                ddlSKU.Items.Insert(0, new ListItem("Select", "0"));
                FillParentsalesChannel();
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    void FillParentsalesChannel()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {

            ObjSalesChannel.SalesChannelID = PageBase.SalesChanelID;
            ObjSalesChannel.BlnShowDetail = true;
            string[] str = { "ParentId", "ParentName" };
            PageBase.DropdownBinding(ref ddlWareHouse, ObjSalesChannel.GetSalesChannelInfo(), str);
        };

    }
    void FillSKU()
    {
        using (ProductData objProduct = new ProductData())
        {
           objProduct.DateFrom = (ucDatePicker.Date);

            string[] str = { "SKUID", "SKUCode" };
            DataTable Dtnew = new DataTable ();
            Dtnew=objProduct.GetSKUInfoOffer();
            ViewState["SKU"]=Dtnew;
            PageBase.DropdownBinding(ref ddlSKU , Dtnew, str);
        };

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        uclblMessage.ShowControl = false;
        btnSaveTarget.Visible = false;
        btnProcess.Visible = true;

    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (ucDatePicker.Date != "" && ddlWareHouse.SelectedValue != "0")
            {
                using (SalesData objSales = new SalesData())
                {

                    objSales.SalesChannelID = PageBase.SalesChanelID;
                    objSales.Brand = PageBase.Brand;
                    objSales.InvoiceDate = Convert.ToDateTime(ucDatePicker.Date);
                    DataTable dtOrder = objSales.GetOrderDetail();
                    pnlGrid.Visible = true;
                }
            }
            else
            {
                pnlGrid.Visible = false;
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    protected void Go_Click(object sender, EventArgs e)
    {
        try
        {
            if (ucDatePicker.Date != "")
            {
                pnl1.Visible = true;
                FillSKU();
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
       
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            using (POC objPoc = new POC())
            {
                DataTable DtReward = new DataTable();
                if (ViewState["SKU"] != null)
                {
                    DataTable DtSku = (DataTable)ViewState["SKU"];
                    if (DtSku.Rows.Count > 0)
                    {
                        DataRow[] drow = DtSku.Select("SKUID=" + ddlSKU.SelectedItem.Value);
                        if (drow.Length > 0)
                        {
                            if (((DataRow)drow.GetValue(0))["OfferBasedOnDetailID"] != DBNull.Value)
                            {
                                objPoc.InvoiceDate = (ucDatePicker.Date);
                                objPoc.SKUID = Convert.ToInt32(ddlSKU.SelectedValue);
                                DtReward = objPoc.GetOfferBySkuReward();
                            }

                            string str = ddlSKU.SelectedItem.Value + "," + ((DataRow)drow.GetValue(0))["OfferBasedOnDetailID"].ToString() + "," + ucDatePicker.Date;
                            lnkOffer.Attributes.Add("onClick", string.Format("return popup('" + str + "')"));
                        }


                    }

                    int RemainQTy = 0;

                    string Elligibilty = lblElligibily.Text;
                   
                     DtDetail.Columns.Add("SKUCode");
                    DtDetail.Columns.Add("OfferRewardDetailID");
                    DtDetail.Columns.Add("OfferName");

                    DataColumn dcAmount = new DataColumn();
                    dcAmount.DataType = System.Type.GetType("System.Decimal");
                    dcAmount.ColumnName = "Amount";
                    DtDetail.Columns.Add(dcAmount);
                    DtDetail.Columns.Add("Quantity");
                    
                    if (DtDetail != null && DtDetail.Rows.Count > 0)
                    {
                        if (DtDetail.Select("SkUId=" + ddlSKU.SelectedValue + " and OfferRewardDetailID=0").Length > 0)
                        {
                            uclblMessage.ShowInfo("SKUCode already added");
                            return;
                        }
                    }
                    if (Convert.ToInt32(Convert.ToDecimal(Elligibilty))>0 && Convert.ToInt32(txtQuantity.Text)>=Convert.ToInt32(Convert.ToDecimal(Elligibilty)))
                    {
                        if (Elligibilty != "0")
                        {
                            RemainQTy = (Convert.ToInt32(txtQuantity.Text)) / Convert.ToInt32(Convert.ToDecimal(Elligibilty));
                        }
                        else
                        {
                            RemainQTy = 0;
                        }
                    DataRow drow1 = DtDetail.NewRow();
                    foreach (DataRow dr in DtReward.Rows)
                    {
                        drow1[0] = dr["SKUCode"].ToString();
                        drow1[1] = dr["OfferRewardDetailID"].ToString();
                        drow1[2] = dr["OfferName"].ToString();
                        drow1[3] = 0;
                        drow1[4] = Convert.ToInt32(dr["RewardValue"]) * RemainQTy;
                    }
                    DtDetail.Rows.Add(drow1);
                    DtDetail.AcceptChanges();
                    }

                    DataRow drow2 = DtDetail.NewRow();
                   
                    DataRow[] drow3 = DtSku.Select("SKUID=" + ddlSKU.SelectedItem.Value);
                    string Skucode = "";
                    if (drow3.Length > 0)
                    {

                        if (((DataRow)drow3.GetValue(0))["Skucode1"] != DBNull.Value)
                        {
                            Skucode = (((DataRow)drow3.GetValue(0))["Skucode1"].ToString());
                        }
                    }
                    drow2[0] = Skucode;
                    drow2[1] = 0;
                    drow2[2] = "";
                    drow2[3] = lblAmount.Text;
                    drow2[4] = txtQuantity.Text;
                    DtDetail.Rows.Add(drow2);
                    DtDetail.AcceptChanges();
                    DataTable dt = (DataTable)ViewState["detail"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DtDetail.Merge(dt);
                    }
                    GridTarget.DataSource = DtDetail;
                    GridTarget.DataBind();

                    ViewState["detail"] = DtDetail;
                    ddlSKU.SelectedValue = "0";
                    lblAmount.Text = "";
                    lblElligibily.Text = "";
                    lblRate.Text = "";
                    txtQuantity.Text = "0";
                    Panel1.Visible = false;
                    pnlGrid.Visible = true;
                    updgrid.Update();


                }

            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    protected void ddlSKU_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSKU.SelectedIndex != 0)
            {
                if (ucDatePicker.Date != "")
                {
                    using (POC objPoc = new POC())
                    {
                        objPoc.InvoiceDate = (ucDatePicker.Date);
                        objPoc.SKUID = Convert.ToInt32(ddlSKU.SelectedValue);
                        DataTable dt = objPoc.GetOfferBySku();

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            Panel1.Visible = true;
                            lblElligibily.Text = Convert.ToInt32(dt.Rows[0]["RequiredValue"]).ToString();
                        }
                        else
                        {
                            Panel1.Visible = false;
                            lblElligibily.Text = "0";
                        }
                        objPoc.SKUID = Convert.ToInt32(ddlSKU.SelectedValue);
                        DataTable Dtprice = objPoc.GetSKUPrice();
                        lblRate.Text = Dtprice.Rows[0]["rate"].ToString();
                        lblRate.Visible = true;
                        lblAmount.Visible = true;
                        lblElligibily.Visible = true;
                    }
                }

                using (POC objPoc = new POC())
                {
                    if (ViewState["SKU"] != null)
                    {
                        DataTable DtSku = (DataTable)ViewState["SKU"];
                        if (DtSku.Rows.Count > 0)
                        {
                            DataRow[] drow = DtSku.Select("SKUID=" + ddlSKU.SelectedItem.Value);
                            if (drow.Length > 0)
                            {
                                if (((DataRow)drow.GetValue(0))["OfferBasedOnDetailID"] != DBNull.Value)
                                {
                                    objPoc.InvoiceDate = (ucDatePicker.Date);
                                    objPoc.SKUID = Convert.ToInt32(ddlSKU.SelectedValue);
                                    DataTable DtReward = objPoc.GetOfferBySkuReward();
                                }

                                string str = ddlSKU.SelectedItem.Value + "," + ((DataRow)drow.GetValue(0))["OfferBasedOnDetailID"].ToString() + "," + ucDatePicker.Date;
                                lnkOffer.Attributes.Add("onClick", string.Format("return popup('" + str + "')"));
                            }


                        }
                    }

                }
            }
            else
            {
                uclblMessage.ShowInfo(Resources.Messages.MandatoryField);
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    protected void txtQuantity_TextChanged(object sender, EventArgs e)
    {
        if (txtQuantity.Text != "")
        {
            lblAmount.Text = (Convert.ToInt32(txtQuantity.Text) * Convert.ToDecimal(lblRate.Text)).ToString();
        }
        else
        {
            lblAmount.Text = "0";
        }
    }
    protected void btnSaveTarget_Click(object sender, EventArgs e)
    {
        uclblMessage.ShowSuccess(Resources.Messages.CreateSuccessfull);
        clearform();
    }
    void clearform()
    {

        ddlWareHouse.SelectedValue = "0";
        ddlSKU.SelectedValue = "0";
        lblAmount.Text = "";
        lblElligibily.Text = "";
        lblRate.Text = "";
        pnl1.Visible = false;
        Panel1.Visible = false;
        pnlGrid.Visible = false;
        ucDatePicker.Date = "";
        btnSaveTarget.Visible = false ;
        updgrid.Update();
        btnProcess.Visible = true;
       ViewState["detail"]=null;
    }
    protected void lnkOffer_Click(object sender, EventArgs e)
    {
        
    }
    protected void btnProcess_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["detail"] != null)
            {
                DataTable dt1 = (DataTable)ViewState["detail"];
                int sum = Convert.ToInt32(dt1.Compute("sum(Amount)", "1=1"));
                using (POC objPOC = new POC())
                {
                    objPOC.InvoiceDate = (ucDatePicker.Date);
                    DataTable DtOffer = objPOC.GetNetOfferAmount();

                    if (Convert.ToInt32(DtOffer.Rows[0]["RequiredValue"]) < sum)
                    {
                        uclblMessage.ShowInfo("You are eligible for " + DtOffer.Rows[0]["NetValeu"].ToString());


                    }
                    else
                    {
                        uclblMessage.ShowInfo(" Your Order Value is Rs."+sum.ToString ()+ "and you are short by Rs."+(Convert.ToInt32(DtOffer.Rows[0]["RequiredValue"]) - sum).ToString()+" to qualify for flat 15% discount.");



                    }

                }
                btnSaveTarget.Visible = true;
                btnProcess.Visible = false;
                updgrid.Update();

            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
       
      
    }
}
