using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class UserControls_PrimaryOrder : System.Web.UI.UserControl
{
    private DataTable dtSource;
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
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;
    }
    #region GridView Events

    public DataTable ReturnGridSource()
    {
        DataTable Detail = new DataTable();

        for (int i = 0; i <= gvStockEntry.Rows.Count - 1; i++)
        {
            TextBox txtQuantityFooter = (TextBox)gvStockEntry.Rows[i].FindControl("txtQuantity");
            TextBox lblAmount = (TextBox)gvStockEntry.Rows[i].FindControl("lblAmount");
            Label lblRate = (Label)gvStockEntry.Rows[i].FindControl("lblRate");
            Detail = (DataTable)ViewState["detail"];
            if (txtQuantityFooter.Text == "0" || txtQuantityFooter.Text != "")
            {
                Detail.Rows[i]["Quantity"] = txtQuantityFooter.Text;
                Detail.Rows[i]["Amount"] = Convert.ToDecimal(lblRate.Text) * Convert.ToInt32(txtQuantityFooter.Text);
            }

        }

        Detail.AcceptChanges();     
        Detail.DefaultView.RowFilter = "Quantity > 0";
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

            txtQuantityFooter.Attributes.Add("OnChange", "ChangeMe('" + lblRate.ClientID + "','" + lblAmount.ClientID + "','" + txtQuantityFooter.ClientID + "')");

        }
    }

    #endregion
}
