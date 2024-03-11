using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using BussinessLogic;

public partial class Transactions_POC_BatchWiseSecondrySale : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fillRetailer();
            ucInvoiceDate.Date = PageBase.ToDate;
            ViewState["StockTable"] = Quantitytable();
        }

    }

    public void fillRetailer()
    {
        using (RetailerData obj = new RetailerData())
        {
            obj.SalesChannelID = SalesChanelID;
            obj.Type = 1;
            DataTable dt = obj.GetRetailerInfo();
            String[] colArray = {"RetailerID", "Retailer"};
            PageBase.DropdownBinding(ref cmbParty, dt, colArray);

        }

    }     

    public void fillproduct()
    {
        using (POC obj = new POC())
        {
            obj.SalesChannelId = PageBase.SalesChanelID;
            DataTable dt = obj.SelectBatchWiseProductInfo();
            String[] colArray = { "ProductID", "ProductCode" };
            PageBase.DropdownBinding(ref cmbProduct, dt, colArray);
        }

    }


    protected void cmbProduct_selectedindexChanged(object sender, EventArgs e)
    {

        if (cmbProduct.SelectedValue == "0")
        {
            cmbSKUCode.Items.Clear();
            cmbSKUCode.Items.Insert(0, new ListItem("Select", "0"));
            return;
        }
        else
        {
            using (POC obj = new POC())
            {
                obj.ProductID = Convert.ToInt16(cmbProduct.SelectedValue);
                obj.SalesChannelId = PageBase.SalesChanelID;
                DataTable dt = obj.SelectBatchWiseSKUInfo();
                String[] colArray = {"SKUID","SKUCode"};
                PageBase.DropdownBinding(ref cmbSKUCode, dt, colArray);
            }

        }


    }
    protected void btnProduct_click(object sender, EventArgs e)
    {
        using (POC obj = new POC())
        {
            ViewState["Product"] = cmbProduct.SelectedItem.ToString();
            ViewState["SKU"] = cmbSKUCode.SelectedItem.ToString();
            obj.SalesChannelId = PageBase.SalesChanelID;
            obj.SKUID = Convert.ToInt16(cmbSKUCode.SelectedValue);
            DataTable dt = obj.SelectBatchInfoFromSKU();
            grdBatchInfo.DataSource = dt;
            grdBatchInfo.DataBind();
            if (pnlBatchInfo.Visible == false)
            {
                pnlBatchInfo.Visible = true;
            }
          //  updBatchInfo.Update();

        }
        cmbProduct.SelectedValue = "0";
        cmbSKUCode.Items.Clear();
        cmbSKUCode.Items.Insert(0, new ListItem("Select", "0"));
        //updProduct.Update();


    }
    protected void btncancelProduct_Click(object sender, EventArgs e)
    {
        pnlBatchInfo.Visible = false;
      //  updBatchInfo.Update();


    }
    protected void btnInitial_click(object sender, EventArgs e)
    {
        cmbSKUCode.Items.Insert(0, new ListItem("Select", "0"));
        pnlProduct.Visible = true;
      //  updProduct.Update();
        cmbParty.Enabled = false;
        txtInvoiceNumber.Enabled = false;
        ucInvoiceDate.TextBoxDate.Enabled = false;

        fillproduct();

    }
    protected void btncancelInitial_Click(object sender, EventArgs e)
    {
        pnlProduct.Visible = false;
        //updProduct.Update();
        blankall();
        cmbParty.SelectedValue = "0";
        cmbParty.Enabled = true;
        txtInvoiceNumber.Text = "";
        txtInvoiceNumber.Enabled = true; 

        
    }

    public DataTable getfinalrtable()
    {
        DataTable dt = new DataTable();
        DataColumn[] dc = new DataColumn[6];
        dc[0] = new DataColumn("SKUCode");
        dc[1] = new DataColumn("BatchNumber");
        dc[2] = new DataColumn("BatchDate");
        dc[3] = new DataColumn("InvoiceNumber");
        dc[4] = new DataColumn("BatchStockId");
        dc[5] = new DataColumn("Quantity");
        dt.Columns.AddRange(dc);
        return dt;
    }

    protected void btnFinal_click(object sender, EventArgs e)
    {
        DataTable dt = getfinalrtable();
        foreach (GridViewRow grv in grdFinal.Rows)
        {
            DataRow dr = dt.NewRow();
            Label lblSkuCode = (Label)grv.FindControl("lblSku");
            Label lblBatchNumber = (Label)grv.FindControl("lblBatchNumber");
            Label lblBatchDate = (Label)grv.FindControl("lblBatchDate");
            Label lblBatchStockID = (Label)grv.FindControl("lblBatchStockID");
            TextBox txtQuantity = (TextBox)grv.FindControl("txtQuantity");
            dr["SKUCode"] = lblSkuCode.Text;
            dr["BatchNumber"] = lblBatchNumber.Text;
            dr["BatchDate"] = lblBatchDate.Text;
            dr["InvoiceNumber"] = txtInvoiceNumber.Text;
            dr["BatchStockId"] = lblBatchStockID.Text;
            dr["Quantity"] = txtQuantity.Text; 
            dt.Rows.Add(dr);

        }
        using (POC obj = new POC())
        {
            obj.SalesFromId = PageBase.SalesChanelID;
            obj.SalesToId = Convert.ToInt16(cmbParty.SelectedValue);
            obj.InvoiceNumber = txtInvoiceNumber.Text;
            obj.InvoiceDate = ucInvoiceDate.Date;
            obj.InsertBatchwiseSecondarySales(dt);
            if (obj.ErrorDetailXML != null && obj.ErrorDetailXML != string.Empty)
            {
                ucMessage1.XmlErrorSource = obj.ErrorDetailXML;
                return;
            }
            if (obj.Error != null && obj.Error != "")
            {
                ucMessage1.ShowError(obj.Error);
                return;
            }
            ucMessage1.ShowSuccess(Resources.Messages.InsertSuccessfull);
            blankall();
            ((DataTable)ViewState["StockTable"]).Rows.Clear();
            ucInvoiceDate.TextBoxDate.Enabled = true; 
        }

  
    }


    public void blankall()
    {
        pnlBatchInfo.Visible = false;
        //updBatchInfo.Update();
        pnlFinalGrid.Visible = false;
        //updFinalGrid.Update();
        cmbProduct.SelectedValue = "0";
        cmbSKUCode.SelectedValue = "0";
        pnlProduct.Visible = false;
        //updProduct.Update();
        cmbParty.SelectedValue = "0";
        txtInvoiceNumber.Text = "";
        ucInvoiceDate.Date = PageBase.ToDate;



    }


    public DataTable getaBatchTable()
    {

        DataTable dt = new DataTable();
        DataColumn[] dc = new DataColumn[4];
         dc[0] = new DataColumn("BatchNumber");
         dc[1] = new DataColumn("BatchDate");
         dc[2] = new DataColumn("Stock");
         dc[3] = new DataColumn("BatchStockId");
         dt.Columns.AddRange(dc);
         return dt;

    }


    public DataTable Quantitytable()
    {
        DataTable dt = new DataTable();
        DataColumn[]  dc =  new DataColumn[5];
        dc[0] = new DataColumn("BatchNumber");
        dc[1] = new DataColumn("SKUCode");
        dc[2] = new DataColumn("Batchdate");
        dc[3] = new DataColumn("BatchStockID");
        dc[4] = new DataColumn("Stock");
        dt.Columns.AddRange(dc);
        return dt;

    }


    protected void btncancelBatchInfo_Click(object sender, EventArgs e)
    {
        pnlBatchInfo.Visible = false;
       // updBatchInfo.Update();
    }
    protected void btnBatchInfo_click(object sender, EventArgs e)
    {
        DataTable dt = getaBatchTable();
        foreach (GridViewRow grv in grdBatchInfo.Rows)
        {
            CheckBox chk = (CheckBox)grv.FindControl("chkBatch");
            if (chk.Checked == true)
            {
                DataRow dr = dt.NewRow();
                Label lblBatchNo = (Label)grv.FindControl("lblBatchNumber");
                dr["BatchNumber"] = lblBatchNo.Text;
                Label lblBatchDate = (Label)grv.FindControl("lblBatchDate");
                dr["BatchDate"] = lblBatchDate.Text;
                Label lblStock = (Label)grv.FindControl("lblStock");
                dr["Stock"] = lblStock.Text;
                Label lblBatchStkID = (Label)grv.FindControl("lblBatchStockID");
                dr["BatchStockId"] = lblBatchStkID.Text; 
                dt.Rows.Add(dr);
            }

        }
        
        foreach (DataRow dr in dt.Rows)
        {
            DataRow drc = ((DataTable)ViewState["StockTable"]).NewRow();
            drc["BatchNumber"] = dr["BatchNumber"];
            drc["BatchDate"] = dr["BatchDate"];
            drc["SKUCode"] = (string)ViewState["SKU"];
            drc["BatchStockId"] = dr["BatchStockId"];
            drc["Stock"] = dr["Stock"]; 
            ((DataTable)ViewState["StockTable"]).Rows.Add(drc);

        }

        grdFinal.DataSource = (DataTable)ViewState["StockTable"];
        grdFinal.DataBind();
        if (pnlFinalGrid.Visible == false)
        {
            pnlFinalGrid.Visible = true;
        }
         //   updFinalGrid.Update();
            pnlBatchInfo.Visible = false;
           // updBatchInfo.Update();

    }



    protected void btncancelfinal_Click(object sender, EventArgs e)
    {

        pnlBatchInfo.Visible = false;
       // updBatchInfo.Update();
        pnlFinalGrid.Visible = false;
   //     updFinalGrid.Update();
        cmbProduct.SelectedValue = "0";
        cmbSKUCode.SelectedValue = "0";
     //   updProduct.Update();
    }
}
