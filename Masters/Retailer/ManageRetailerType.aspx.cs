using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;

public partial class Masters_Retailer_ManageRetailerType : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewPanel();
        }
        if (hdnRetailerTypeID.Value.Equals(""))
        {
            btnSubmit.Text = "Submit";
            btnClear.Text = "Cancel";
        }
    }
    #region UserDifine Method

    private void ClearAll()
    {
        txtRetailerType.Text = "";
    }
    private void Clearucmsg()
    {
        ucMsg.Visible = false;
    }
    private void Showucmsg()
    {
        ucMsg.Visible = true;
    }
    private void AddPanel()
    {
        using (RetailerData objInsertMR = new RetailerData())
        {
            try
            {
                objInsertMR.RetailerType = txtRetailerType.Text.Trim();
                objInsertMR.Status = Convert.ToBoolean(chkStatus.Checked);
                objInsertMR.CompanyId = PageBase.ClientId;
                objInsertMR.InsManageRetailer();
                if (objInsertMR.InsError != null)
                {
                    ucMsg.ShowError(objInsertMR.InsError);
                }
                else
                {
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                }
            }
            catch (Exception ex)
            {
                if (objInsertMR.Error != null && objInsertMR.Error != "" && objInsertMR.Error != "0")
                {
                    ucMsg.ShowError(objInsertMR.Error);
                }
            }
        }
    }
    private void ViewPanel()
    {
        using (RetailerData objGetMR = new RetailerData())
        {
            try
            {
                objGetMR.CompanyId = PageBase.ClientId;
                gvRetailerType.DataSource = objGetMR.GetManageRetailer(0);
                gvRetailerType.DataBind();
            }
            catch (Exception ex)
            {
                if (objGetMR.Error != null && objGetMR.Error != "" && objGetMR.Error != "0")
                {
                    ucMsg.ShowError(objGetMR.Error);
                }
            }
        }
    }
    private void UpdateAll()
    {
        using (RetailerData objUpdateMR = new RetailerData())
        {
            try
            {
                objUpdateMR.RetailerType = txtRetailerType.Text.Trim();
                objUpdateMR.Status = Convert.ToBoolean(chkStatus.Checked);
                objUpdateMR.RetailerID = Convert.ToInt32(hdnRetailerTypeID.Value);
                objUpdateMR.CompanyId = PageBase.ClientId;
                objUpdateMR.UpdManageRetailer(0);
                if (objUpdateMR.InsError != null)
                {
                    ucMsg.ShowError(objUpdateMR.InsError);
                }
                else
                {
                    ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                    btnSubmit.Text = "Submit";
                    btnClear.Text = "Cancel";
                }
            }
            catch (Exception ex)
            {
                if (objUpdateMR.Error != null && objUpdateMR.Error != "" && objUpdateMR.Error != "0")
                {
                    ucMsg.ShowError(objUpdateMR.Error);
                }
            }
        }
    }

    #endregion
    #region Control Method

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (hdnRetailerTypeID.Value.Equals(""))
        {
            AddPanel();
            ClearAll();
            ViewPanel();
        }
        else
        {
            UpdateAll();
            ClearAll();
            hdnRetailerTypeID.Value = "";
            ViewPanel();
        }
    }
    protected void gvRetailerType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRetailerType.PageIndex = e.NewPageIndex;
        ViewPanel();
    }
    protected void gvRetailerType_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ucMsg.Visible = false;
        if (e.CommandName.Equals("editcmd"))
        {
            using (RetailerData objCmdMR = new RetailerData())
            {
                try
                {
                    objCmdMR.RetailerID = Convert.ToInt32(e.CommandArgument);
                    objCmdMR.CompanyId = PageBase.ClientId;
                    DataTable dt = objCmdMR.GetManageRetailer(1);
                    txtRetailerType.Text = dt.Rows[0]["RetailerTypeName"].ToString();
                    if (dt.Rows[0]["Status"].ToString().Equals("True")) { chkStatus.Checked = true; }
                    if (dt.Rows[0]["Status"].ToString().Equals("False")) { chkStatus.Checked = false; }
                    hdnRetailerTypeID.Value = e.CommandArgument.ToString();
                    if (hdnRetailerTypeID.Value.Equals(""))
                    {
                        btnSubmit.Text = "Submit";
                        btnClear.Text = "Cancel";
                    }
                    else
                    {
                        btnSubmit.Text = "Update";
                        btnClear.Text = "Clear";
                    }
                }
                catch (Exception ex)
                {
                    if (objCmdMR.Error != null && objCmdMR.Error != "" && objCmdMR.Error != "0")
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowError(objCmdMR.Error);
                    }
                }
            }
        }
        if (e.CommandName.Equals("togglecmd"))
        {
            using (RetailerData objCmdMR = new RetailerData())
            {
                try
                {
                    objCmdMR.RetailerID = Convert.ToInt32(e.CommandArgument);
                    objCmdMR.CompanyId = PageBase.ClientId;
                    objCmdMR.UpdManageRetailer(1);
                    ViewPanel();
                    ucMsg.Visible = true;
                    ucMsg.ShowSuccess(Resources.Messages.StatusChanged);

                }
                catch (Exception ex)
                {
                    if (objCmdMR.Error != null && objCmdMR.Error != "" && objCmdMR.Error != "0")
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowError(objCmdMR.Error);
                    }
                }
            }
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtRetailerType.Text = "";
        chkStatus.Checked = true;
        ucMsg.Visible = false;
        hdnRetailerTypeID.Value = "";
        if (hdnRetailerTypeID.Value.Equals(""))
        {
            btnSubmit.Text = "Submit";
            btnClear.Text = "Cancel";
        }
    }

    #endregion
}
