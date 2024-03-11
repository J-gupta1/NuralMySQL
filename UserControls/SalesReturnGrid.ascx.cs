using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using System.Data;
using DataAccess;

public partial class UserControls_SalesReturnGrid : System.Web.UI.UserControl
{
    private DataTable dtSource;
    private EnumData.eControlRequestTypeForEntry eSalesType;
  
    #region Puplic Property
    public DataTable Source
    {
        get { return dtSource; }
        set
        {
            dtSource = value;
            ViewState["detail"] = null;
            ViewState["detail"] = dtSource;
            //BindDetail();
            BindGrid();
        }
    }
    public EnumData.eControlRequestTypeForEntry Salestype
    {
        get { return eSalesType; }
        set
        {
            eSalesType = value;
        }
    }
    public DataTable GetGridSource()
    {
        return (DataTable)ViewState["detail"];
    }

    #endregion 
    #region User Defined

    void BindGrid()
    {

        if (ViewState["detail"] != null)
        {
            if (eSalesType == EnumData.eControlRequestTypeForEntry.eSecondarySales)
            {
                gvStockEntry.Columns[3].Visible = false;
               // gvStockEntry.Columns[4].Visible = false;
                gvStockEntry.Columns[4].HeaderText = "Already Return Quantity";
                
            }
            else
            {
                
                gvStockEntry.Columns[3].Visible = true;
                //gvStockEntry.Columns[4].Visible = true;
                gvStockEntry.Columns[4].HeaderText = "Permissible Return Quantity";
               
            }
            gvStockEntry.DataSource = dtSource;
            gvStockEntry.DataBind();
        }


    }
    protected void gvStockEntry_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;

    }
    #endregion 
    #region GridView Events

    public DataTable ReturnGridSource()
    {
        DataTable Detail = new DataTable();
        for (int i = 0; i <= gvStockEntry.Rows.Count - 1; i++)
        {
            TextBox txtQuantityFooter = (TextBox)gvStockEntry.Rows[i].FindControl("txtQuantity");
            // TextBox txtSalesMan = (TextBox)gvStockEntry.Rows[i].FindControl("txtSalesMan");
            Detail = (DataTable)ViewState["detail"];
            if (txtQuantityFooter.Text != "" || txtQuantityFooter.Text =="0")
            {
               
                Detail.Rows[i]["ReturnQty"] = txtQuantityFooter.Text;
            }

        }

           Detail.AcceptChanges();
           if (eSalesType == EnumData.eControlRequestTypeForEntry.ePrimary1Sales)       //For P1 Sales Return Interface
           {

               Detail.DefaultView.RowFilter = "ReturnQty > 0";
           }
          
            if (eSalesType == EnumData.eControlRequestTypeForEntry.ePrimary2Sales)
            {
                
                Detail.DefaultView.RowFilter = "ReturnQty > 0";
            }
            else if (eSalesType == EnumData.eControlRequestTypeForEntry.eSecondarySales)
            {
                
                Detail.DefaultView.RowFilter = "ReturnQty > 0";
            }
            
        
        Detail = Detail.DefaultView.ToTable();
        return Detail;

    }
    protected void gvStockEntry_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable DtSalesData = new DataTable();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdD = (HiddenField)e.Row.FindControl("hdnid");

            TextBox txtQuantityFooter = (TextBox)e.Row.FindControl("txtQuantity");
            Label lblSkuID = (Label)e.Row.FindControl("lblSKUID");
            Label lblStockInhand = (Label)e.Row.FindControl("lblStockInhand");
            if (eSalesType == EnumData.eControlRequestTypeForEntry.eSecondarySales)
            {
                //txtQuantityFooter.Attributes.Add("OnChange", "StockInhandCheck(this);");
            }
            else
            {
                txtQuantityFooter.Attributes.Add("OnChange", "SalesQtyCheck(this);");
            }

        }

    }
 
    #endregion
}
