using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using System.Data;
using DataAccess;

public partial class UserControls_SalesEntryGrid : System.Web.UI.UserControl
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
            if (eSalesType == EnumData.eControlRequestTypeForEntry.ePrimary1Sales || eSalesType == EnumData.eControlRequestTypeForEntry.ePrimary2Sales)
            {
               
                gvStockEntry.Columns[4].Visible=true ;
                gvStockEntry.Columns[5].Visible = true ;
                
            }
            else
            {
                
                gvStockEntry.Columns[4].Visible = false;
                gvStockEntry.Columns[5].Visible = false;
                gvStockEntry.Columns[6].Visible = false;
            }
            if (eSalesType == EnumData.eControlRequestTypeForEntry.eOrder)
            {
                gvStockEntry.Columns[3].Visible = true;
                if (PageBase.SalesChannelLevel == 2)
                {
                    gvStockEntry.Columns[3].HeaderText = "Stock In Hand(Warehouse)";
                }
                if (PageBase.SalesChannelLevel == 3)
                {
                    gvStockEntry.Columns[3].HeaderText = "Stock In Hand(Distributor)";
                }
                gvStockEntry.Columns[8].Visible = true;
                gvStockEntry.Columns[7].Visible=true  ;
                lblinfo.Visible = true ;
                lbl.Visible = true;
                lblinfo.Text = "0";
            }
            else
            {
                gvStockEntry.Columns[3].Visible=false  ;
                gvStockEntry.Columns[9].Visible = false;
                gvStockEntry.Columns[7].Visible = false;
                lblinfo.Visible = false ;
                lbl.Visible = false ;
                
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

        for( int i=0 ;i<= gvStockEntry.Rows.Count-1;i++)
        {
            TextBox txtQuantityFooter = (TextBox)gvStockEntry.Rows[i].FindControl("txtQuantity");
            TextBox lblAmount = (TextBox)gvStockEntry.Rows[i].FindControl("lblAmount");
            Label lblRate = (Label)gvStockEntry.Rows[i].FindControl("lblRate");
            Detail = (DataTable)ViewState["detail"];
            if (txtQuantityFooter.Text == "0" || txtQuantityFooter.Text != "")
            {
                Detail.Rows[i]["Quantity"] = txtQuantityFooter.Text;
              if ( eSalesType == EnumData.eControlRequestTypeForEntry.eOrder)
                {
                    Detail.Rows[i]["Amount"] = Convert.ToDecimal(lblRate.Text) * Convert.ToInt32(txtQuantityFooter.Text);
                }
               

            }
            
        }
      
      Detail.AcceptChanges();
    
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
     else  if (eSalesType == EnumData.eControlRequestTypeForEntry.eOrder)           
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
            Label lblRate = (Label)e.Row.FindControl("lblRate");
            TextBox lblAmount = (TextBox)e.Row.FindControl("lblAmount");
           
            if (Salestype == EnumData.eControlRequestTypeForEntry.eStockTransfer)
            {
                txtQuantityFooter.Attributes.Add("OnChange", "StockCheck(this);");

            }
            else 
            {
                if (Salestype == EnumData.eControlRequestTypeForEntry.eOrder)
                {
                    txtQuantityFooter.Attributes.Add("OnChange", "ChangeMe('" + lblRate.ClientID + "','" + lblAmount.ClientID + "','" + txtQuantityFooter.ClientID + "')");
                }
                else
                {
                    txtQuantityFooter.Attributes.Add("OnChange", "StockCheckSales(this);");
                }
                
            }

        }

    }
    #endregion
}
