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

public partial class Masters_HO_Common_ManageUserMenuModuleWise : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindModules();
    }


    private void BindModules()
    {
        using (MenuData objMenu = new MenuData())
        {
            objMenu.ActiveStatus = 2;
            objMenu.UserID = PageBase.UserId;
            DataTable dtMenu = objMenu.Select();
            ddlMenu.DataSource = dtMenu;
            ddlMenu.DataTextField = "MenuName";
            ddlMenu.DataValueField = "MenuID";
            ddlMenu.DataBind();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string Search = string.Empty;
            if (ddlMenu.SelectedValue == "0")
            {
                ucMsg.ShowInfo("Please select module!");
                return;
            }
            else
            {
                ucMsg.Visible = false;
                Search = "Search";
            }
            BindList();
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
            //ExceptionPolicy.HandleException(ex, ExceptionPolicyName);
        }
    }

    private void BindList()
    {
        using (MenuData objEntityTypeModuleMapping = new MenuData())
        {
            objEntityTypeModuleMapping.MenuID = Convert.ToInt32(ddlMenu.SelectedValue);
            DataSet ds = objEntityTypeModuleMapping.SearchByEntityTypeRoleModuleMappingByMenuID();

            if (ds != null && ds.Tables.Count == 3)
            {
                DataTable dtEntityTypeRoles = ds.Tables[0];
                ViewState["Permissions"] = ds.Tables[1];
                ViewState["PermissionsModuleMapping"] = ds.Tables[2];

                if (dtEntityTypeRoles.Rows.Count > 0)
                {
                    Panel2.Visible = true;
                    gvList.DataSource = dtEntityTypeRoles;
                    gvList.DataBind();
                }
                else
                {
                    ucMsg.ShowInfo("No record(s) found.");
                    Panel2.Visible = false;
                }
            }
            else
            {
                ucMsg.ShowInfo("No record(s) found.");
            }
        }
    }
    protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataTable dtPermissions = null;
            if (ViewState["Permissions"] != null)
                dtPermissions = (DataTable)ViewState["Permissions"];

            CheckBoxList chklstACLTag = (CheckBoxList)e.Row.FindControl("chklstACLTag");
            if (chklstACLTag != null)
            {
                BindPermissions(dtPermissions, chklstACLTag);
            }

            CheckBox chkBoxModule = (CheckBox)e.Row.FindControl("chkBoxModule");
            Label lblusermoduleid = (Label)e.Row.FindControl("lblusermoduleid");
            if (lblusermoduleid != null)
            {
                int _intusermoduleid = Convert.ToInt32(lblusermoduleid.Text.Trim());
                if (_intusermoduleid > 0)
                {
                    chkBoxModule.Checked = true;
                    if (ViewState["PermissionsModuleMapping"] != null)
                    {
                        DataTable dtAllMapping = (DataTable)ViewState["PermissionsModuleMapping"];
                        if (dtAllMapping != null && dtAllMapping.Rows.Count > 0)
                        {
                            var lstMapping = dtAllMapping.AsEnumerable().Where(r => Convert.ToInt32(r["usermoduleid"]) == _intusermoduleid).ToList();
                            if (lstMapping.Count > 0)
                            {
                                DataTable dtMapping = lstMapping.CopyToDataTable();
                                if (dtMapping != null && dtMapping.Rows.Count > 0)
                                {
                                    BindMappedModulePermissions(chklstACLTag, dtMapping);
                                }
                            }
                        }
                    }
                }
                else
                    chkBoxModule.Checked = false;
            }
        }
    }
    private static void BindPermissions(DataTable dtPermissions, CheckBoxList chklstACLTag)
    {
        chklstACLTag.Items.Clear();
        chklstACLTag.DataSource = dtPermissions;
        chklstACLTag.DataTextField = "TagValue";
        chklstACLTag.DataValueField = "ACLTagID";
        chklstACLTag.DataBind();
    }
    private void BindMappedModulePermissions(CheckBoxList objchklst, DataTable dt)
    {
        foreach (ListItem lst in objchklst.Items)
        {
            Int32 count = dt.AsEnumerable().Where(r => Convert.ToInt32(r["ACLTagID"]) == Convert.ToInt32(lst.Value)).Count();
            if (count > 0)
                lst.Selected = true;
            else
                lst.Selected = false;
        }
    }
    #region Methods
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            using (MenuData objEntityTypeModuleMapping = new MenuData())
            {
                objEntityTypeModuleMapping.MenuID = Convert.ToInt32(ddlMenu.SelectedValue);
                DataTable dtMenu_Permission = GenerateModule_PermissionTable();
                objEntityTypeModuleMapping.UserID = PageBase.UserId;
                Int16 result = objEntityTypeModuleMapping.UpdateMapping_Permission_ModuleWise(dtMenu_Permission);
                if (result == 0)
                {
                    ucMsg.ShowSuccess("Record Update Successfully.");
                    BindList();
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
        }
    }

    private DataTable GenerateModule_PermissionTable()
    {
        DataTable dt = CreateTable();
        foreach (GridViewRow gvRow in gvList.Rows)
        {
            CheckBox chkBoxModule = (CheckBox)gvRow.FindControl("chkBoxModule");
            Label lblEntityTypeRoleID = (Label)gvRow.FindControl("lblEntityTypeRoleID");
            CheckBoxList chklstACLTag = (CheckBoxList)gvRow.FindControl("chklstACLTag");

            if (chkBoxModule.Checked)
            {
                Int32 EntityTypeRoleID = Convert.ToInt32(lblEntityTypeRoleID.Text.Trim());
                dt = GeneratePermissionTable(chklstACLTag, EntityTypeRoleID, dt);
            }
        }
        return dt;
    }
    private DataTable GeneratePermissionTable(CheckBoxList objChkLst, int EntityTypeRoleID, DataTable dt)
    {
        Int16 count = 0;
        foreach (ListItem lst in objChkLst.Items)
        {
            if (lst.Selected)
            {
                count++;
                Int16 ACLTagID = Convert.ToInt16(lst.Value.Trim());
                dt = InsertData(dt, EntityTypeRoleID, ACLTagID);
            }
        }
        if (count == 0)
            InsertData(dt, EntityTypeRoleID, 0);
        return dt;
    }
    private DataTable InsertData(DataTable dt, int _intEntityTypeRoleID, Int16 _intACLTagID)
    {
        if (_intEntityTypeRoleID > 0)
        {
            DataRow dr = dt.NewRow();
            dr["EntityTypeRoleID"] = _intEntityTypeRoleID;
            dr["ACLTagID"] = _intACLTagID;
            dt.Rows.Add(dr);
        }
        return dt;
    }
    private DataTable CreateTable()
    {
        DataTable dt = new DataTable();
        DataColumn dc = new DataColumn("EntityTypeRoleID", typeof(int));
        dt.Columns.Add(dc);
        dc = new DataColumn("ACLTagID", typeof(Int16));
        dt.Columns.Add(dc);
        return dt;
    }
    #endregion
}




