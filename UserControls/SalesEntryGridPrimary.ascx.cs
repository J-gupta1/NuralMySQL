using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using System.Data;
using DataAccess;

public partial class UserControls_SalesEntryGridPrimary : System.Web.UI.UserControl
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
    //private void AddTableContraint()
    //{

    //    if (dtSource != null && dtSource.Rows.Count != 0)
    //    {
    //        if (dtSource.Constraints.Count == 0)
    //        {
    //            dtSource.Constraints.Add("SKUCons", dtSource.Columns["SKUCode"], true);
    //        }
    //    }
    //    else
    //    {
    //        if (dtSource.Constraints.Count > 0)
    //        {
    //            dtSource.Constraints.Clear();
    //            dtSource.Columns["SKUCode"].AllowDBNull = true;
    //        }
    //    }
    //    if (dtSource.Columns.Contains("StockInhand") != true)
    //    {
    //        dtSource.Columns.Add("StockInhand", System.Type.GetType("System.Int64"));
    //    }

    //        GrdDeatil.Columns[3].Visible = true;

    //}

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
            if (txtQuantityFooter.Text == "0" || txtQuantityFooter.Text != "")
            {
                Detail.Rows[i]["Quantity"] = txtQuantityFooter.Text;
            }

        }

        Detail.AcceptChanges();
        if (eSalesType == EnumData.eControlRequestTypeForEntry.ePrimary2Sales)
        {
            Detail.DefaultView.RowFilter = "IntermediarySalesID <> 0 or Quantity > 0";
        }
        else if (eSalesType == EnumData.eControlRequestTypeForEntry.eSecondarySales)
        {
            Detail.DefaultView.RowFilter = "SecondarySalesID <> 0 or Quantity > 0";
        }
        else if (eSalesType == EnumData.eControlRequestTypeForEntry.eStockTransfer || eSalesType == EnumData.eControlRequestTypeForEntry.ePrimary1Sales)
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
            Label lblOrderedQuantity = (Label)e.Row.FindControl("lblOrderedQty");

            txtQuantityFooter.Attributes.Add("OnChange", "StockCheck(this);");
            //using (SalesData objsales = new SalesData())
            //{
            //    RangeValidator RangeValidator = (RangeValidator)e.Row.FindControl("rng");
            //    RangeValidator.MaximumValue = lblOrderedQuantity.Text;
            //    RangeValidator.MinimumValue = "0";
            //    if (Convert.ToInt32(RangeValidator.MaximumValue) < Convert.ToInt32(txtQuantityFooter.Text))
            //    {
            //    RangeValidator.ErrorMessage = "Quantity should be less than stock in hand";
            //    }
            //    //RangeValidator.ControlToValidate = "txtQuantityFooter";
            //}


        }

    }
    //protected void GrdDeatil_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GrdDeatil.EditIndex = -1;
    //    BindDetail();
    //}
    //protected void GrdDeatil_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    GrdDeatil.EditIndex = (int)e.NewEditIndex;


    //    BindDetail();
    //}
    //protected void GrdDeatil_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridViewRow grdRow = GrdDeatil.Rows[e.RowIndex];
    //        TextBox txtItemQuantity = (TextBox)grdRow.FindControl("txtItemQuantity");
    //        //TextBox txtItemValue = (TextBox)grdRow.FindControl("txtItemValue");
    //        dtSource = (DataTable)ViewState["detail"];

    //        DataRow drow = dtSource.Rows[e.RowIndex];
    //        drow.BeginEdit();




    //        if (GrdDeatil.Columns[3].Visible == true)
    //        {
    //            int diffQuantity = Convert.ToInt32(txtItemQuantity.Text) - Convert.ToInt32(drow["Quantity"]);
    //            if (Convert.ToInt32(drow["StockInHand"]) - diffQuantity < 0)
    //            {
    //                ucMessage1.ShowError(Resources.Messages.StockInHandCheck);
    //                return;
    //            }
    //        }
    //        drow["Quantity"] = Convert.ToInt32(txtItemQuantity.Text);
    //        //drow["Value"] = Convert.ToDecimal(txtItemValue.Text);

    //        drow.EndEdit();
    //        dtSource.AcceptChanges();
    //        ViewState["detail"] = dtSource;
    //        GrdDeatil.EditIndex = -1;
    //        BindDetail();
    //        ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
    //    }
    //    catch (Exception ex)
    //    {

    //        ucMessage1.ShowError(ex.Message);
    //        BussinessLogic.PageBase.Errorhandling(ex);
    //    }

    //}


    #endregion
}
