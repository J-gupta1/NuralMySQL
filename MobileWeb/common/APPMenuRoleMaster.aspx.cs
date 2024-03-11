﻿#region Copyright(c) 2020 Zed-Axis Technologies All rights are reserved
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

public partial class MobileWeb_common_APPMenuRoleMaster : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BindCompanyName();
            BindMenuName();
            ddlRoleName.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    protected void ddlCompanyName_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        ucMsg.Visible = false;
        BindRoles();
        BindMenuName();
    }
    protected void btnSearch_Click(object sender, System.EventArgs e)
    {

    }
    protected void btnShowAll_Click(object sender, System.EventArgs e)
    {

    }
    private void BindMenuName()
    {
        try
        {

            DataTable dtMenuName = new DataTable();
            using (clsEntityMappingTypeRelationMaster objget = new clsEntityMappingTypeRelationMaster())
            {
                objget.CompanyId = Convert.ToInt32(ddlCompanyName.SelectedValue);
                objget.UserID = PageBase.UserId;
                if (ddlRoleName.SelectedValue!="")
                {
                    objget.RoleId = Convert.ToInt32(ddlRoleName.SelectedValue);
                }
                else
                {
                    objget.RoleId = 0;
                }
               
                dtMenuName = objget.getMenuNameDropdowns();
                ViewState["MenuTypelist"] = dtMenuName;
                if (dtMenuName.Rows.Count > 0)
                {
                    GvMenuName.DataSource = dtMenuName;
                    GvMenuName.DataBind();
                }
                else
                {
                    GvMenuName.DataSource = null;
                    GvMenuName.DataBind();
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
            using (clsEntityMappingTypeRelationMaster objrole = new clsEntityMappingTypeRelationMaster())
            {
                objrole.SearchType = 1;
                objrole.Status = true;
                objrole.UserID = PageBase.UserId;
                objrole.CompanyId =Convert.ToInt32(ddlCompanyName.SelectedValue);
                dt = objrole.GetUserRoleUserMaster();
                if(dt.Rows.Count>0)
                {
                    String[] colArray = { "RoleId", "RoleName" };
                    PageBase.DropdownBinding(ref ddlRoleName, dt, colArray);
                }
                else
                {
                    String[] colArray = { "RoleId", "RoleName" };
                    PageBase.DropdownBinding(ref ddlRoleName, dt, colArray);
                    BindMenuName();
                }
            };
           
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnUpdateMenu_Click(object sender, EventArgs e)
    {
        try
        {
            using (clsEntityMappingTypeRelationMaster objSave = new clsEntityMappingTypeRelationMaster())
            {

                DataTable dtAddtolist;
                dtAddtolist = new DataTable();
                dtAddtolist.Columns.Add("MenuId", typeof(Int64));
                dtAddtolist.Columns.Add("Status", typeof(bool));
                dtAddtolist.Columns.Add("MenuTypeId", typeof(Int16));
                dtAddtolist.Columns.Add("DisplayOrder", typeof(Int16));
                dtAddtolist.AcceptChanges();

                CheckBox chkaddlist = new CheckBox();
                string MenuId = string.Empty;
                foreach (GridViewRow grv in GvMenuName.Rows)
                {
                    if (grv.RowType == DataControlRowType.DataRow)
                    {
                        chkaddlist = (grv.FindControl("chkboxMenuID") as CheckBox);

                        if (chkaddlist.Checked)
                        {
                            DropDownList ddlMenuType = (DropDownList)grv.FindControl("ddlMenuType") as DropDownList;
                            TextBox txtdisplaymode = (TextBox)grv.FindControl("txtdisplaymode") as TextBox;
                            Label lblMenuStatus = (Label)grv.FindControl("lblMenuStatus") as Label;
                            MenuId = GvMenuName.DataKeys[grv.RowIndex].Value.ToString();
                            if (ddlMenuType.SelectedValue != "0")
                            {
                                if (txtdisplaymode.Text != "")
                                {
                                    if (System.Text.RegularExpressions.Regex.IsMatch(txtdisplaymode.Text, "^[0-9]"))
                                    {
                                        DataRow dr = null;
                                        dr = dtAddtolist.NewRow();
                                        dr["MenuId"] = Convert.ToInt64(MenuId);
                                        dr["Status"] = Convert.ToBoolean(chkaddlist.Checked);
                                        dr["MenuTypeId"] = ddlMenuType.SelectedValue;
                                        dr["DisplayOrder"] = Convert.ToInt16(txtdisplaymode.Text);
                                        dtAddtolist.Rows.Add(dr);
                                        dtAddtolist.AcceptChanges();
                                    }
                                    else
                                    {
                                        ucMsg.Visible = true;
                                        ucMsg.ShowWarning("Display Mode Should be Numeric.");
                                        return;
                                    }
                                }
                                else
                                {
                                    ucMsg.Visible = true;
                                    ucMsg.ShowWarning("Please Enter Display Mode.");
                                    return;
                                }
                            }
                            else
                            {
                                ucMsg.Visible = true;
                                ucMsg.ShowWarning("Please Select MenuType.");
                                return;

                            }
                        }
                        if (MenuId == string.Empty && MenuId != null)
                        {
                            ucMsg.Visible = true;
                            ucMsg.ShowWarning("No record in list for Save/Update.");
                        }
                    }
                }
                if (dtAddtolist.Rows.Count > 0)
                {
                    objSave.CompanyId = Convert.ToInt32(ddlCompanyName.SelectedValue);
                    objSave.RoleId = Convert.ToInt32(ddlRoleName.SelectedValue);
                    objSave.UserID = UserId;
                    objSave.dtSaveManulist = dtAddtolist;
                    objSave.SaveAPPMenuRoleMasterList();
                    if (objSave.Out_Param == 0)
                    {
                        ucMsg.ShowSuccess(Resources.SuccessMessages.SaveSuccess);
                    }
                    else
                    {
                        ucMsg.ShowError(objSave.Error);
                    }
                    dvMsg.Style.Add("display", "block");
                    updMessage.Update();
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
    protected void GvMenuName_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dtMenuName = new DataTable();
        dtMenuName = (DataTable)ViewState["MenuTypelist"];
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlMenuType = (DropDownList)e.Row.FindControl("ddlMenuType");
            HiddenField hdnmenutypeid = (HiddenField)e.Row.FindControl("hdnmenutypeid");
            ddlMenuType.Items.FindByValue(hdnmenutypeid.Value).Selected = true;
        }
    }
    private void BindCompanyName()
    {
        try
        {

            DataTable dtCompanyName = new DataTable();
            using (clsEntityMappingTypeRelationMaster objget = new clsEntityMappingTypeRelationMaster())
            {
                objget.CompanyId = PageBase.ClientId;
                objget.UserID = PageBase.UserId;
                dtCompanyName = objget.getCompanyNameDropdowns();
                if (dtCompanyName.Rows.Count > 0)
                {
                    ddlCompanyName.DataSource = dtCompanyName;
                    ddlCompanyName.DataTextField = "ClientName";
                    ddlCompanyName.DataValueField = "ClientID";
                    ddlCompanyName.DataBind();
                    ddlCompanyName.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    ddlCompanyName.Items.Insert(0, new ListItem("Select", "0"));
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
    protected void ddlRoleName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ucMsg.Visible = false;
        BindMenuName();
    }
    protected void btnCancle_Click(object sender, EventArgs e)
    {
        ucMsg.Visible = false;
        BindCompanyName();
        ddlRoleName.Items.Clear();
        ddlRoleName.Items.Insert(0, new ListItem("Select", "0"));
        BindMenuName();

    }
}