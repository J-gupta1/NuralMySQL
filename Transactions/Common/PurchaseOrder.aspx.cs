using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;
public partial class Transactions_PurchaseOrder : PageBase
{
    DataTable DtDetail = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            uclblMessage.ShowControl = false;
            //ucDatePicker.MinRangeValue = System.DateTime.Now;
            ucDatePicker.MaxRangeValue = System.DateTime.Now;
            ucDatePicker.RangeErrorMessage = "Invalid Date Range";
            if (!IsPostBack)
            {
                FillBrandProdCat();
                FillFromSC();
                FillModel();
                FillSKU();
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    
    DataTable CreateDataTable()
    {
        DataTable DtSKU = new DataTable();
        DtSKU.Columns.Add("SKUCode", typeof(string));
        DtSKU.Columns.Add("SKUName", typeof(string));
        DtSKU.Columns.Add("Quantity", typeof(int));
        DtSKU.Columns.Add("Model", typeof(string));

        return DtSKU;
    }
    void FillFromSC()
    {
        using (PO objPo = new PO())
        {//0 Brand and category,1=Model,2=SKU,3= Po From Channel,4=PO to channel
            objPo.MasterType = 3;
            objPo.BrandID = 0;
            objPo.ProductCategoryID = 0;
            objPo.ModelID = 0;
            objPo.UserID = PageBase.UserId;
            objPo.SalesChannelID = PageBase.SalesChanelID;

            string[] str = { "SalesChannelID", "SalesChannelName" };
            
            DataSet Dsnew = new DataSet();
            Dsnew = objPo.LoadMaster();

            PageBase.DropdownBinding(ref ddlPOFrom, Dsnew.Tables[0], str);
            
        };
    }
    void FillToSC()
    {
        using (PO objPo = new PO())
        {//0 Brand and category,1=Model,2=SKU,3= Po From Channel,4=PO to channel
            objPo.MasterType = 4;
            objPo.BrandID = 0;
            objPo.ProductCategoryID = 0;
            objPo.ModelID = 0;
            objPo.UserID = PageBase.UserId;
            objPo.SalesChannelID = PageBase.SalesChanelID;
            objPo.POFromSalesChannelID = Convert.ToInt32(ddlPOFrom.SelectedValue);

            string[] str = { "SalesChannelID", "SalesChannelName" };

            DataSet Dsnew = new DataSet();
            Dsnew = objPo.LoadMaster();

            PageBase.DropdownBinding(ref ddlWareHouse, Dsnew.Tables[0], str);

        };
    }
    void FillBrandProdCat()
    {
        using (PO objPo = new PO())
        {//0 Brand and category,1=Model,2=SKU,3= Po From Channel,4=PO to channel
            objPo.MasterType = 0;
            objPo.BrandID = 0;
            objPo.ProductCategoryID = 0;
            objPo.ModelID = 0;
            objPo.UserID = PageBase.UserId;
            objPo.SalesChannelID = PageBase.SalesChanelID;

            string[] strBrand = { "BrandID", "BrandName" };
            string[] strProdCat = { "ProductCategoryID", "ProductCategoryName" };
            DataSet Dsnew = new DataSet();
            Dsnew = objPo.LoadMaster();
            
            PageBase.DropdownBinding(ref ddlBrand, Dsnew.Tables[0], strBrand);
            PageBase.DropdownBinding(ref ddlProdCat, Dsnew.Tables[1], strProdCat);
        };
    }
    void FillModel()
    {
        Int16 brandId = 0;
        Int16 prodCatId = 0;
        

        if (ddlBrand.SelectedIndex > 0)
            brandId = Convert.ToInt16(ddlBrand.SelectedValue);
        if (ddlProdCat.SelectedIndex > 0)
            prodCatId = Convert.ToInt16(ddlProdCat.SelectedValue);
        
        using (PO objPo = new PO())
        {//0 Brand and category,1=Model,2=SKU,3= Po From Channel,4=PO to channel
            objPo.MasterType = 1;
            objPo.BrandID = brandId;
            objPo.ProductCategoryID = prodCatId;
            objPo.ModelID = 0;
            objPo.UserID = PageBase.UserId;
            objPo.SalesChannelID = PageBase.SalesChanelID;

            string[] str = { "ModelID", "ModelName" };
            DataSet Dsnew = new DataSet();
            Dsnew = objPo.LoadMaster();
            
            PageBase.DropdownBinding(ref ddlModel, Dsnew.Tables[0], str);
        };

    }
    void FillSKU()
    {
        Int16 brandId = 0;
        Int16 prodCatId = 0;
        int modelId = 0;

        if (ddlBrand.SelectedIndex>0)
            brandId = Convert.ToInt16(ddlBrand.SelectedValue);
        if(ddlProdCat.SelectedIndex>0)
            prodCatId = Convert.ToInt16(ddlProdCat.SelectedValue);
        if(ddlModel.SelectedIndex>0)
            modelId=Convert.ToInt32(ddlModel.SelectedValue);

        using (PO objPo = new PO())
        {//0 Brand and category,1=Model,2=SKU,3= Po From Channel,4=PO to channel
            objPo.MasterType = 2;
            objPo.BrandID = brandId;
            objPo.ProductCategoryID = prodCatId;
            objPo.ModelID = modelId;
            objPo.UserID = PageBase.UserId;
            objPo.SalesChannelID = PageBase.SalesChanelID;

            string[] str = { "SKUCode", "SKUName" };
            DataSet Dsnew = new DataSet ();
            Dsnew = objPo.LoadMaster();
            
            PageBase.DropdownBinding(ref ddlSKU, Dsnew.Tables[0], str);
        };

    }
    
    
    //protected void BtnSubmit_Click(object sender, EventArgs e)
    
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable POItem= (DataTable)ViewState["POItem"];

            string SKUCode = Convert.ToString((sender as Button).CommandArgument);
            DataRow[] dr = POItem.Select("SKUCode='" + SKUCode+ "'");
            if(dr.Length>0)
            {
                POItem.Rows.Remove(dr[0]);
                POItem.AcceptChanges();
                ViewState["POItem"] = POItem;
                if (POItem.Rows.Count > 0)
                    GridItem.DataSource = POItem;
                else
                {
                    GridItem.DataSource = null;
                    pnlGrid.Visible = false; 
                }
                GridItem.DataBind();

            }
        }
        catch(Exception ex)
        { }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable POItem;
            if (ViewState["POItem"] == null)
                POItem = CreateDataTable();
            else
                POItem = (DataTable)ViewState["POItem"];

            if (POItem.Rows.Count > 0)
            {
                DataRow[] drow = POItem.Select("SKUCode='" + ddlSKU.SelectedItem.Value+ "'");
                if (drow.Length > 0)
                {
                    uclblMessage.ShowError("SKU already added.");
                    return;
                }

            }

            int Qty = 0;
            bool isNumeric;
            int i;
            isNumeric = int.TryParse(string.IsNullOrEmpty(txtQuantity.Text.Trim()) != true ? txtQuantity.Text.Trim() : "", out i);
            Qty = isNumeric ? i : 0;
            
            if(Qty<1)
            {
                uclblMessage.ShowError("Quantity should be more than 0.");
                return;
            }
            DataRow drow2 = POItem.NewRow();


            drow2["SKUCode"] = ddlSKU.SelectedValue;
            drow2["SKUName"] = ddlSKU.SelectedItem.Text;
            drow2["Quantity"] = Qty;
            drow2["Model"] = ddlModel.SelectedItem.Text;
            
            POItem.Rows.Add(drow2);
            POItem.AcceptChanges();
            
            GridItem.DataSource = POItem;
            GridItem.DataBind();

            ViewState["POItem"] = POItem;
            ddlSKU.SelectedValue = "0";
            ddlModel.SelectedIndex = 0;
            ddlProdCat.SelectedIndex = 0;
            ddlBrand.SelectedIndex = 0;

            txtQuantity.Text = "0";
            pnlGrid.Visible = true;
            updgrid.Update();





        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
       
    }
    
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            
            if (ucDatePicker.Date != "" && ddlPOFrom.SelectedValue != "0" && ddlWareHouse.SelectedValue != "0")
            {
                DataTable POItem;
                if (ViewState["POItem"] == null)
                {
                    uclblMessage.ShowError("No Item Added for po.");
                    return;
                }

                POItem = (DataTable)ViewState["POItem"];
                if (POItem.Rows.Count == 0)
                {
                    uclblMessage.ShowError("No Item Added for po.");
                    return;
                }

                DataTable DtPO = new DataTable();
                DtPO.Columns.Add("OrderID", typeof(int));
                DtPO.Columns.Add("OrderNumber", typeof(string));
                DtPO.Columns.Add("OrderDate", typeof(string));
                DtPO.Columns.Add("FromSalesChannelID", typeof(int));
                DtPO.Columns.Add("ToSalesChannelID", typeof(int));
                DtPO.Columns.Add("SKUCode", typeof(string));
                DtPO.Columns.Add("Quantity", typeof(int));
                DtPO.Columns.Add("Amount", typeof(decimal));
                foreach (DataRow dr in POItem.Rows)
                {
                    DataRow drpo = DtPO.NewRow();
                    drpo["OrderID"] = 0;
                    drpo["OrderNumber"] = "";
                    drpo["OrderDate"] = "";
                    drpo["FromSalesChannelID"] = 0;
                    drpo["ToSalesChannelID"] = 0;
                    drpo["SKUCode"] = dr["SKUCode"];
                    drpo["Quantity"] = dr["Quantity"];

                    DtPO.Rows.Add(drpo);
                    DtPO.AcceptChanges();

                }
                using (PO objPO = new PO())
                {

                    objPO.POFromSalesChannelID = Convert.ToInt32(ddlPOFrom.SelectedValue);
                    objPO.POToSalesChannelID = Convert.ToInt32(ddlWareHouse.SelectedValue);
                    objPO.UserID = PageBase.UserId;
                    objPO.Dt = DtPO;
                    objPO.OrderDate = Convert.ToDateTime(ucDatePicker.Date);
                    objPO.InsertPO();
                    if (objPO.OutParam == 0 && !string.IsNullOrEmpty(objPO.PONumber))
                    {
                        ViewState["POItem"] = null;
                        uclblMessage.ShowSuccess("PO generated succefully. PO Number: " + objPO.PONumber);
                        GridItem.DataSource = null;
                        GridItem.DataBind();
                        pnlGrid.Visible = false; 

                    }
                    else if (objPO.OutParam != 0 && !string.IsNullOrEmpty(objPO.error))
                    {
                        uclblMessage.ShowError(objPO.error);
                    }
                    else
                        uclblMessage.ShowError("Error in PO generation.");
                    pnlGrid.Visible = true;
                }
            }
            else
            {
                uclblMessage.ShowError("PO From, PO To and Order Date are required.");
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
            uclblMessage.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    void clearform()
    {

        ddlWareHouse.SelectedValue = "0";
        ddlSKU.SelectedValue = "0";
        
        pnlGrid.Visible = false;
        ucDatePicker.Date = "";
        btnSave.Visible = false ;
        updgrid.Update();
        
       ViewState["detail"]=null;
    }
    protected void lnkOffer_Click(object sender, EventArgs e)
    {
        
    }

    protected void ddlPOFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPOFrom.SelectedIndex > 0)
        {
            FillToSC();
        }
        else
            ddlWareHouse.Items.Clear();
    }
    protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillModel();
        FillSKU();
    }
    protected void ddlProdCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillModel();
        FillSKU();
    }
    protected void ddlModel_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSKU();
    }
    
}
