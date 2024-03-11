using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using System.IO;
using System.Data;
using BussinessLogic;

public partial class Masters_HO_Admin_ViewSchemeDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.Visible = false;
            if (!IsPostBack)
            {
                hdnCase.Value = "0";
                gvBind();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString());
        }
    }
    # region UserDefine Function
    private void Cancel()
    {
        txtsrcSchemeCode.Text = "";
        txtsrcSchememName.Text = "";
    }
    private void gvBind()
    {
        try
        {
            DataTable dt = new DataTable();
            using (SchemeData objOfflineScheme = new SchemeData())
            {
                if (hdnCase.Value == "0")
                {
                    objOfflineScheme.OfflineStatus = 1;
                    dt = objOfflineScheme.GetOfflineScheme(0);
                }
                else if (hdnCase.Value == "1")
                {
                    if (txtsrcSchememName.Text != "")
                        objOfflineScheme.SchemeName = txtsrcSchememName.Text;
                    if (txtsrcSchemeCode.Text != "")
                        objOfflineScheme.OfflineSchemeCode = txtsrcSchemeCode.Text;
                    objOfflineScheme.OfflineStatus = 1;
                    dt = objOfflineScheme.GetOfflineScheme(1);
                }
                gvViewOfflineScheme.DataSource = dt;
                gvViewOfflineScheme.DataBind();
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    # endregion
    # region Control Function
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtsrcSchememName.Text != "" || txtsrcSchemeCode.Text != "")
            hdnCase.Value = "1";
        else
            hdnCase.Value = "0";
        gvBind();
    }
    protected void btnCancelSearch_Click(object sender, EventArgs e)
    {
        hdnCase.Value = "0";
        gvBind();
        Cancel();
    }
    protected void gvViewOfflineScheme_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvViewOfflineScheme.PageIndex = e.NewPageIndex;
        gvBind();
    }
    # endregion
}
