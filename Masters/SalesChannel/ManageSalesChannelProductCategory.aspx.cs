using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
using Cryptography;
using System.Text;

public partial class Masters_SalesChannel_ManageSalesChannelProductCategory : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

        ucMsg.ShowControl = false;
        if (!IsPostBack)
        {
            FillState();

            FillBrand();
            FillSalesChannelType();

        }
    }
    void FillBrand()
    {
        using (ProductData ObjProduct = new ProductData())
        {
            ObjProduct.SearchType = EnumData.eSearchConditions.Active;
            String[] StrCol = new String[] { "ProductCategoryID", "ProductCategoryName" };
            PageBase.DropdownBinding(ref ddlProductCategory, ObjProduct.GetAllProductCategoryByParameters(), StrCol);

        };
    }
    void FillState()
    {
        using (GeographyData ObjState = new GeographyData())
        {
            ObjState.SearchCondition = EnumData.eSearchConditions.Active;
            String[] StrCol = new String[] { "StateID", "StateName" };
            PageBase.DropdownBinding(ref ddlState, ObjState.GetAllStateByParameters(), StrCol);

        };
    }
    void FillSalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelTypeID = -1;
            String[] StrCol = new String[] { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref ddlSalesChannelType, ObjSalesChannel.GetSalesChannelTypeForBrand(), StrCol);

        };
    }

    void FillGrid()
    {
        DataTable Dt = new DataTable();
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            if (ddlSalesChannelType.SelectedValue != "0")
            {
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(ddlSalesChannelType.SelectedValue);
            }
            ObjSalesChannel.ProductCategoryId = Convert.ToInt32(ddlProductCategory.SelectedValue);
            ObjSalesChannel.StateID = Convert.ToInt16(ddlState.SelectedValue);
            ObjSalesChannel.CityID = Convert.ToInt16(ddlCity.SelectedValue);
            ObjSalesChannel.SalesChannelName = txtSalesChannelName.Text.Trim();
            ObjSalesChannel.SalesChannelCode = txtSalesChannelCode.Text.Trim();
            Dt = ObjSalesChannel.GetSalesChannelInfoForProductCategory();
        };
        if (Dt != null && Dt.Rows.Count > 0)
        {
            grdSalesChannelList.Visible = true;
            grdSalesChannelList.DataSource = Dt;
            grdSalesChannelList.DataBind();
            pnlHide.Visible = true;

        }
        else
        {
            grdSalesChannelList.Visible = false;
            grdSalesChannelList.DataSource = null;
            grdSalesChannelList.DataBind();
            ucMsg.ShowInfo(Resources.Messages.NoRecord);
            pnlHide.Visible = false;

        }
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        grdSalesChannelList.PageIndex = 0;
        FillGrid();

    }
    protected void grdSalesChannelList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void grdSalesChannelList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdSalesChannelList.PageIndex = e.NewPageIndex;
        FillGrid();

    }
    protected void grdSalesChannelList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridViewRow GVR = e.Row;
                Label lblBrandId = (Label)GVR.FindControl("lblBrandId");
                Label lblStatus = (Label)GVR.FindControl("lblStatus");
                CheckBox chkBrand = (CheckBox)GVR.FindControl("chkBxSelect");
                if (lblStatus.Text.ToLower() != "false")
                {
                    chkBrand.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    public StringBuilder GenerateXML(GridView gv)
    {

        StringBuilder ProductCategoryDetailXML = new StringBuilder();
        ProductCategoryDetailXML.AppendLine("<table>");
        foreach (GridViewRow gvRow in gv.Rows)
        {
            Label lblProductCategoryID = ((Label)gvRow.FindControl("lblProductCategoryID"));
            Label lblSalesChannelID = ((Label)gvRow.FindControl("lblSalesChannelID"));
            Label lblStatus = ((Label)gvRow.FindControl("lblStatus"));
            CheckBox chkBxSelect = ((CheckBox)gvRow.FindControl("chkBxSelect"));
            if (lblSalesChannelID.Text != "")
            {


                ProductCategoryDetailXML.AppendLine("<rowse>");
                ProductCategoryDetailXML.AppendFormat("<SalesChannelID>{0}</SalesChannelID>{1}", lblSalesChannelID.Text.Trim(), Environment.NewLine);
                ProductCategoryDetailXML.AppendFormat("<ProductCategoryID>{0}</ProductCategoryID>{1}", ddlProductCategory.SelectedValue, Environment.NewLine);
                ProductCategoryDetailXML.AppendFormat("<Status>{0}</Status>{1}", chkBxSelect.Checked, Environment.NewLine);
                ProductCategoryDetailXML.AppendLine("</rowse>");

            }
        }
        ProductCategoryDetailXML.AppendLine("</table>");
        return ProductCategoryDetailXML;
    }

    protected void chkBxHeader_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkAll = (CheckBox)grdSalesChannelList.HeaderRow.FindControl("chkBxHeader");
        if (chkAll.Checked == true)
        {
            foreach (GridViewRow gvRow in grdSalesChannelList.Rows)
            {
                CheckBox chkSel = (CheckBox)gvRow.FindControl("chkBxSelect");
                chkSel.Checked = true;
            }
        }
        else
        {
            foreach (GridViewRow gvRow in grdSalesChannelList.Rows)
            {
                CheckBox chkSel = (CheckBox)gvRow.FindControl("chkBxSelect");
                chkSel.Checked = false;
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            using (SalesChannelData ObjSales = new SalesChannelData())
            {
                ObjSales.XMLList = GenerateXML(grdSalesChannelList).ToString();
                Int32 result = ObjSales.InsertSalesChannelProductCategoryMapping();

                if (result == 0)
                {
                    ucMsg.ShowSuccess(Resources.Messages.InsertSuccessfull);
                }
                else if (result == 1)
                    ucMsg.ShowInfo(Resources.Messages.ErrorMsgTryAfterSometime);
                Clear();

            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);

        }
    }
    void Clear()
    {
        ddlProductCategory.SelectedIndex = 0;
        ddlSalesChannelType.SelectedIndex = 0;
        ddlState.SelectedIndex = 0;
        ddlState_SelectedIndexChanged(ddlState, new EventArgs());
        //ddlCity.SelectedIndex = 0;
        txtSalesChannelName.Text = "";
        txtSalesChannelCode.Text = "";
        pnlHide.Visible = false;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }
    protected void grdSalesChannelList_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
        //{
        //    CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
        //    CheckBox chkBxHeader = (CheckBox)this.grdSalesChannelList.HeaderRow.FindControl("chkBxHeader");
        //    chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,'{0}');", chkBxHeader.ClientID);
        //}
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        pnlHide.Visible = false;

    }

    protected void btnCancel_Click1(object sender, EventArgs e)
    {
        Clear();
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (GeographyData ObjGeography = new GeographyData())
            {
                if (ddlState.SelectedIndex > 0)
                {
                    ObjGeography.SearchCondition = EnumData.eSearchConditions.Active;
                    ObjGeography.StateId = Convert.ToInt16(ddlState.SelectedValue);
                    String[] StrCol = new String[] { "CityId", "CityName" };
                    PageBase.DropdownBinding(ref ddlCity, ObjGeography.GetAllCityByParameters(), StrCol);
                }
                else if (ddlState.SelectedIndex == 0)
                {
                    ddlCity.Items.Clear();
                    ddlCity.Items.Insert(0, new ListItem("Select", "0"));
                }

            };
        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
}
