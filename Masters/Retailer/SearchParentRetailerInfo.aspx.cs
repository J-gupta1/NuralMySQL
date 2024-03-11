/*
 * Change Log,
 * DD-MMM-YYYY, Name, #CCXX, Description.
 * 14-May-2018, Sumit Maurya, #CC01, Button Name changed from "Search Sales channel" to  "Search Reatiler".
 * 26-Dec-2018, Sumit Maurya, #CC02, RetailerId provided (Done for ZedsalesV5).
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using Cryptography;
using System.Data;
using DataAccess;

public partial class Masters_Retailer_SearchParentRetailerInfo : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

        StyleCss.Attributes.Add("href", "~/" + strAssets + "/CSS/popup.css");
        if (!IsPostBack)
        {
            btnSearch.Text = "Search Retailer"; /* #CC01 Added */
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.Visible = false;
            if (txtSalesChannelCode.Text.Trim() == "" && txtSalesChannelName.Text.Trim() == "")
            {
                GridSales.DataSource = null;
                GridSales.DataBind();
                updGrid.Update();
                ucMessage1.ShowInfo("Please Enter Atleast One Searching parameter ");
                return;
            }
            else
            {
                FillGrid();
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);

        }
    }
    void FillGrid()
    {
        DataTable Dt = new DataTable();
        //using (SalesChannelData ObjSearch = new SalesChannelData())
        //{
        //    ObjSearch.SalesChannelCode = txtSalesChannelCode.Text.Trim();
        //    ObjSearch.SalesChannelName = txtSalesChannelName.Text.Trim();
        //    ObjSearch.SalesChannelID = PageBase.SalesChanelID;
        //    Dt = ObjSearch.GetSalesChannelListForP1Sales();
        //};
            using (SalesChannelData objSalesChannel = new SalesChannelData())
            {
                objSalesChannel.RoleType = EnumData.RoleType.Retailer;
                objSalesChannel.SalesChannelID = PageBase.SalesChanelID;
                objSalesChannel.RetailerCode = txtSalesChannelCode.Text.Trim();
                objSalesChannel.RetailerName = txtSalesChannelName.Text.Trim();
                String[] StrCol = new String[] { "RetailerID", "RetailerName" };
                objSalesChannel.RetailerID = Convert.ToInt32(Convert.ToString(Crypto.Decrypt(Convert.ToString(Request.QueryString["RetID"]).Replace("%2","+"), PageBase.KeyStr))); /* #CC02 Added */
               Dt= objSalesChannel.GetSalesChannelParentForGroup();
                //if
                //PageBase.DropdownBinding(ref ddlParentRetailer, , StrCol);
                //ReqParent.Enabled = true;
                //ReqParent.ValidationGroup = "Add";
            };
     
        if (Dt != null && Dt.Rows.Count > 0)
        {
            GridSales.Visible = true;
            GridSales.DataSource = Dt;
            GridSales.DataBind();
            dvGrid.Visible = true;
            updGrid.Update();
        }
        else
        {
            dvGrid.Visible = false;
            GridSales.DataSource = null;
            GridSales.DataBind();
            updGrid.Update();
            ucMessage1.ShowInfo(Resources.Messages.NoRecord);
        }
    }
    protected void GridSales_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridSales.PageIndex = e.NewPageIndex;
        FillGrid();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtSalesChannelName.Text = "";
        txtSalesChannelCode.Text = "";
        GridSales.DataSource = null;
        GridSales.DataBind();
        updGrid.Update();
        dvGrid.Visible = false;
    }
    protected void GridSales_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridViewRow GVR = e.Row;
                Button btnSelect = (Button)GVR.FindControl("btnSelect");
                Label lblRetailerId = ((Label)GVR.FindControl("lblRetailerID"));
                Label lblRetailerName = ((Label)GVR.FindControl("lblRetailerName"));
                btnSelect.Attributes.Add("onClick", string.Format("return passvalue('" + lblRetailerId.Text + "','" + lblRetailerName.Text + "')"));
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
}
