using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess;

public partial class UserControls_WebUserControl : System.Web.UI.UserControl
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

        for( int i=0 ;i<= gvStockEntry.Rows.Count-1;i++)
        {
            TextBox txtQuantityFooter = (TextBox)gvStockEntry.Rows[i].FindControl("txtQuantity");
           
            Detail = (DataTable)ViewState["detail"];
            if (txtQuantityFooter.Text == "0" || txtQuantityFooter.Text != "")
            {
                Detail.Rows[i]["Quantity"] = txtQuantityFooter.Text;
            }
            
        }
      
      Detail.AcceptChanges();
      //if (Detail.Select("Quantity =0").Length > 0)
      //{
      //    Error = (Resources.Messages.ZeroQtyNotAllowed);
      //    return Detail;

      //}
      if (eSalesType == EnumData.eControlRequestTypeForEntry.ePrimary1Sales)            ///Pankaj Dhingra For P1 Interface
      {
          Detail.DefaultView.RowFilter = "PrimarySalesID <> 0 or Quantity > 0";
      }
    
      if (eSalesType == EnumData.eControlRequestTypeForEntry.ePrimary2Sales)
      {
          Detail.DefaultView.RowFilter = "IntermediarySalesID <> 0 or Quantity > 0";
      }
      else if (eSalesType == EnumData.eControlRequestTypeForEntry.eSecondarySales)
      {
          Detail.DefaultView.RowFilter = "SecondarySalesID <> 0 or Quantity > 0";
      } 
      else if (eSalesType == EnumData.eControlRequestTypeForEntry.eStockTransfer)
      {
          Detail.DefaultView.RowFilter = "Quantity > 0";
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
            if (Salestype == EnumData.eControlRequestTypeForEntry.eStockTransfer)
            {
                txtQuantityFooter.Attributes.Add("OnChange", "StockCheck(this);");

            }
            else
            {
                txtQuantityFooter.Attributes.Add("OnChange", "StockCheckSales(this);");
            }

        }

    }
    


    #endregion
}

