/*
 * Change Log
 * -----------------------------------------------------------------------
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 15-Jun-2018, Sumit Maurya, #CC01, Special characters was causing issue which is rectified now ( Done for Comio).
 * -----------------------------------------------------------------------
 * 
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

public partial class Transactions_SalesChannel_SearchSalesChannelCode :  PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
		
		 StyleCss.Attributes.Add("href", "~/" + strAssets + "/CSS/popup.css");
        if (!IsPostBack)
        {

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
        using (SalesChannelData ObjSearch = new SalesChannelData())
        {
            ObjSearch.SalesChannelCode = txtSalesChannelCode.Text.Trim();
            ObjSearch.SalesChannelName = txtSalesChannelName.Text.Trim();
            ObjSearch.SalesChannelID = PageBase.SalesChanelID;
            Dt = ObjSearch.GetSalesChannelListForP1Sales();
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
                Label lblSalesCode = ((Label)GVR.FindControl("lblSalesChanneCode"));
                Label lblSalesChannelName = ((Label)GVR.FindControl("lblSalesChanneName"));
                Label lblSalesChannelID = ((Label)GVR.FindControl("lblSalesChannelID"));
                btnSelect.Attributes.Add("onClick", string.Format("return passvalue('" + lblSalesCode.Text + "','" + lblSalesChannelName.Text + "','" + lblSalesChannelID.Text + "')"));
            }
        }
        catch (Exception ex)
        {
        }
    }
}
