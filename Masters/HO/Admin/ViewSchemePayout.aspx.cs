using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using System.Data;
using BussinessLogic;

public partial class Masters_HO_Admin_ViewSchemePayout : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.Visible = false;
            if (PageBase.BaseEntityTypeID == 3 && PageBase.SalesChanelID == 0)
            {
                txtsrcRetailerName.Visible = false;
                lblRetailerName.Visible = false;
            }
            if (!IsPostBack)
            {
                hdnCase.Value = "0";
                gvBind();
            }
        }
        catch (Exception ex)
        {
            ucMsg.Visible = true;
            ucMsg.ShowError(ex.ToString());
        }
    }
    private void gvBind()
    {
        DataTable dt=new DataTable();
        using (SchemeData objSchemeData = new SchemeData())
        {
            if (hdnCase.Value.Equals("0"))
            {
                if (PageBase.BaseEntityTypeID == 3 && PageBase.SalesChanelID == 0)
                    objSchemeData.nullableUserID = PageBase.UserId;
                dt=objSchemeData.GetOfflineSchemePayout();
            }
            else
                if (hdnCase.Value.Equals("1"))
                {
                    if (PageBase.BaseEntityTypeID == 3 && PageBase.SalesChanelID == 0)
                        objSchemeData.nullableUserID = PageBase.UserId;
                    if (txtsrcRetailerName.Text != "")
                        objSchemeData.RetailerName = txtsrcRetailerName.Text.Trim();
                    if (txtsrcSchemeName.Text != "")
                        objSchemeData.SchemeName = txtsrcSchemeName.Text.Trim();
                    dt = objSchemeData.GetOfflineSchemePayout();
                }
            gvViewOfflineSchemePayout.DataSource = dt;
            gvViewOfflineSchemePayout.DataBind();
            if (PageBase.BaseEntityTypeID == 3 && PageBase.SalesChanelID == 0)
            {
                gvViewOfflineSchemePayout.Columns[1].Visible = false;
            }
            if (dt.Rows.Count == 0)
            {
                ucMsg.Visible = true;
                ucMsg.ShowInfo(Resources.Messages.NoRecord);
            }
        }
    }
    protected void gvViewOfflineSchemePayout_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvViewOfflineSchemePayout.PageIndex = e.NewPageIndex;
            gvBind();
        }
        catch (Exception ex)
        {
            ucMsg.Visible = true;
            ucMsg.ShowError(ex.ToString());
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            hdnCase.Value = "1";
            gvBind();
        }
        catch (Exception ex)
        {
            ucMsg.Visible = true;
            ucMsg.ShowError(ex.ToString());
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            txtsrcRetailerName.Text = "";
            txtsrcSchemeName.Text = "";
            hdnCase.Value = "0";
            gvBind();
        }
        catch (Exception ex)
        {
            ucMsg.Visible = true;
            ucMsg.ShowError(ex.ToString());
        }
    }
}
