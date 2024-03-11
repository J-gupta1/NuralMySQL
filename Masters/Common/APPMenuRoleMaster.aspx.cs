#region Copyright(c) 2020 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2017 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Vijay Kumar Prajapati
* Created On: 07-June-2020
 * Description: This is a APP Menu Interface.
* ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 
 ====================================================================================================
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using BussinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters_Common_APPMenuRoleMaster : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindMenuName();
            BindRoles();
            BindList(1);
        }
    }
    protected void btnSubmit_Click(object sender, System.EventArgs e)
    {
        using (clsEntityMappingTypeRelationMaster objSave = new clsEntityMappingTypeRelationMaster())
        {
            objSave.MenuId = Convert.ToInt32(ddlMenuName.SelectedValue);
            objSave.RoleId = Convert.ToInt32(ddlRoleName.SelectedValue);
            objSave.MenuTypeId = Convert.ToInt16(ddlMenuType.SelectedValue);
            objSave.DisplayOrder =Convert.ToInt16(txtDisplayOrder.Text.Trim());
            objSave.UserID = UserId;
            objSave.CompanyId = PageBase.ClientId;
            objSave.Active = Convert.ToInt16(chkActive.Checked);
            objSave.Condition = 0;
            if (btnSubmit.Text == "Update")
            {
                objSave.AppMenuRoleId = Convert.ToInt32(ViewState["MenuRoleId"]);
                objSave.Condition = 1;
            }
            objSave.SaveAPPMenuRoleMaster();
            if (objSave.Out_Param == 0)
            {
                if (btnSubmit.Text == "Submit")
                    ucMsg.ShowSuccess(Resources.SuccessMessages.SaveSuccess);
                else
                    ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                BindList(Convert.ToInt32(ViewState["PageIndex"]));
                CancelSubmit();
            }
            else
            {
                ucMsg.ShowError(objSave.Error);
            }
            dvMsg.Style.Add("display", "block");
            updMessage.Update();
        }
    }
    protected void btnCancel_Click(object sender, System.EventArgs e)
    {
        CancelSubmit();
        ucMsg.Visible = false;
        updMessage.Update();
    }
    protected void btnSearch_Click(object sender, System.EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlSearchMenu.SelectedValue) == 0 && Convert.ToInt16(ddlRoleNameSearch.SelectedValue) == 0 && Convert.ToInt16(ddlStatusSearch.SelectedValue) == 255)
            {
                ucMsg.ShowWarning("Please select at least one search parameter..!!");
                dvMsg.Style.Add("display", "block");
                updMessage.Update();
                return;
            }
            ucPagingControl1.SetCurrentPage = 1;
            BindList(1);
        }
        catch (Exception ex)
        {
            ucMsg.Visible = true;
            ucMsg.ShowError(ex.ToString());
        }
    }
    protected void btnShowAll_Click(object sender, System.EventArgs e)
    {
        try
        {
            ddlSearchMenu.SelectedIndex = 0;
            ddlRoleNameSearch.SelectedIndex = 0;
            ddlStatusSearch.SelectedValue = "255";
            updSearch.Update();
            ucPagingControl1.SetCurrentPage = 1;
            BindList(1);
        }
        catch (Exception ex)
        {
            ucMsg.Visible = true;
            ucMsg.ShowError(ex.ToString());
        }
    }
    protected void Exporttoexcel_Click(object sender, System.EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            using (clsEntityMappingTypeRelationMaster objGet = new clsEntityMappingTypeRelationMaster())
            {
                objGet.PageSize = Convert.ToInt32(ViewState["TotalRecords"]);
                objGet.PageIndex = -1;
                objGet.MenuId = Convert.ToInt32(ddlSearchMenu.SelectedValue);
                objGet.RoleId = Convert.ToInt32(ddlRoleNameSearch.SelectedValue);
                objGet.Active = Convert.ToInt16(ddlStatusSearch.SelectedValue);
                objGet.CompanyId = PageBase.ClientId;
                dt = objGet.GetAppMenuMasterData();
            }
            DataSet ds = new DataSet();
            ds.Merge(dt);
            string FilenameToexport = "APPMenuRoleDetails";
            PageBase.ExportToExecl(ds, FilenameToexport);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
            dvMsg.Style.Add("display", "block");

        }
    }
    protected void grdvList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        try
        {
            ucMsg.Visible = false;
            dvMsg.Style.Add("display", "none");
            if (e.CommandName == "cmdEdit")
            {
                using (clsEntityMappingTypeRelationMaster objGet = new clsEntityMappingTypeRelationMaster())
                {
                    objGet.PageSize = Convert.ToInt32(PageBase.PageSize);
                    objGet.PageIndex = 1;
                    objGet.CompanyId = PageBase.ClientId;
                    objGet.AppMenuRoleId = Convert.ToInt32(e.CommandArgument);
                    ViewState["MenuRoleId"] = Convert.ToInt32(e.CommandArgument);
                    DataTable dt = objGet.SelectAPPMenuRoleBYID();
                    ddlMenuName.SelectedIndex = ddlMenuName.Items.IndexOf(ddlMenuName.Items.FindByValue(dt.Rows[0]["MenuId"].ToString()));
                    ddlRoleName.SelectedIndex = ddlRoleName.Items.IndexOf(ddlRoleName.Items.FindByValue(dt.Rows[0]["RoleId"].ToString()));
                    ddlMenuType.SelectedIndex = ddlMenuType.Items.IndexOf(ddlMenuType.Items.FindByValue(dt.Rows[0]["MenuTypeID"].ToString()));
                    txtDisplayOrder.Text = dt.Rows[0]["DisplayOrder"].ToString();
                    chkActive.Checked = dt.Rows[0]["Status"].ToString() == "1" ? true : false;
                    btnSubmit.Text = "Update";
                    updSave.Update();
                    updpnlGrid.Update();
                }
            }
            if (e.CommandName == "Active")
            {
                using (clsEntityMappingTypeRelationMaster objActive = new clsEntityMappingTypeRelationMaster())
                {
                    objActive.UserID = UserId;
                    objActive.AppMenuRoleId = Convert.ToInt32(e.CommandArgument);
                    objActive.Condition = 2;
                    objActive.CompanyId = PageBase.ClientId;
                    objActive.SaveAPPMenuRoleMaster();
                    if (objActive.Out_Param == 0)
                    {
                        ucMsg.ShowSuccess(Resources.SuccessMessages.ActiveInActive);
                        BindList(Convert.ToInt32(ViewState["PageIndex"]));
                        CancelSubmit();
                    }
                    else
                    {
                        ucMsg.ShowError(objActive.Error);
                    }
                    dvMsg.Style.Add("display", "block");
                    updMessage.Update();
                    updpnlGrid.Update();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        try
        {

            BindList(ucPagingControl1.CurrentPage);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
            dvMsg.Style.Add("display", "block");
            updMessage.Update();
        }
    }
    private void BindMenuName()
    {
        try
        {

            DataTable dtMenuName = new DataTable();
            using (clsEntityMappingTypeRelationMaster objget = new clsEntityMappingTypeRelationMaster())
            {
                objget.CompanyId = PageBase.ClientId;
                objget.UserID = PageBase.UserId;
                dtMenuName = objget.getMenuNameDropdowns();
                if (dtMenuName.Rows.Count > 0)
                {
                    ddlMenuName.DataSource = dtMenuName;
                    ddlMenuName.DataTextField = "MenuName";
                    ddlMenuName.DataValueField = "MeniId";
                    ddlMenuName.DataBind();
                    ddlMenuName.Items.Insert(0, new ListItem("Select", "0"));

                    ddlSearchMenu.DataSource = dtMenuName;
                    ddlSearchMenu.DataTextField = "MenuName";
                    ddlSearchMenu.DataValueField = "MeniId";
                    ddlSearchMenu.DataBind();
                    ddlSearchMenu.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    ddlMenuName.Items.Insert(0, new ListItem("Select", "0"));
                    ddlSearchMenu.Items.Insert(0, new ListItem("Select", "0"));
                }
            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
            dvMsg.Style.Add("display", "block");
            updMessage.Update();
        }
    }
    private void BindRoles()
    {
        try
        {
            DataTable dt = new DataTable();

            ddlRoleName.Items.Clear();
            ddlRoleNameSearch.Items.Clear();
            using (clsEntityMappingTypeRelationMaster objrole = new clsEntityMappingTypeRelationMaster())
            {
                objrole.SearchType = 1;
                objrole.Status = true;
                objrole.UserID = PageBase.UserId;
                objrole.CompanyId = PageBase.ClientId;
                dt = objrole.GetUserRoleUserMaster();
            };
            String[] colArray = { "RoleId", "RoleName" };
            PageBase.DropdownBinding(ref ddlRoleName, dt, colArray);
            PageBase.DropdownBinding(ref ddlRoleNameSearch, dt, colArray);
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void BindList(int index)
    {
        try
        {
            index = index == 0 ? 1 : index;
            using (clsEntityMappingTypeRelationMaster objGet = new clsEntityMappingTypeRelationMaster())
            {
                objGet.PageSize = Convert.ToInt32(PageBase.PageSize);
                objGet.PageIndex = index;
                objGet.MenuId =Convert.ToInt32(ddlSearchMenu.SelectedValue);
                objGet.RoleId = Convert.ToInt32(ddlRoleNameSearch.SelectedValue);
                objGet.Active = Convert.ToInt16(ddlStatusSearch.SelectedValue);
                objGet.CompanyId = PageBase.ClientId;
                DataTable dt = objGet.GetAppMenuMasterData();
                ViewState["TotalRecords"] = objGet.TotalRecords;
                dvMsg.Style.Add("display", "none");
                divgrd.Style.Add("display", "bolck");
                grdvList.DataSource = dt;
                grdvList.DataBind();
                if (dt == null || dt.Rows.Count == 0)
                {
                    ucPagingControl1.Visible = false;
                }
                else
                {
                    ucPagingControl1.Visible = true;
                    ucPagingControl1.PageSize = Convert.ToInt32(PageBase.PageSize);
                    ucPagingControl1.TotalRecords = objGet.TotalRecords;
                    ucPagingControl1.FillPageInfo();
                    ViewState["PageIndex"] = index;
                }
                updpnlGrid.Update();
                updMessage.Update();
            }
        }
        catch (Exception ex)
        {
            ucMsg.Visible = true;
            ucMsg.ShowError(ex.ToString());
        }
    }
    private void CancelSubmit()
    {

        ddlMenuType.SelectedIndex = 0;
        ddlRoleName.SelectedIndex = 0;
        ddlMenuName.SelectedIndex = 0;
        chkActive.Checked = true;
        txtDisplayOrder.Text = "";
        btnSubmit.Text = "Submit";      
        updSave.Update();
    }
}