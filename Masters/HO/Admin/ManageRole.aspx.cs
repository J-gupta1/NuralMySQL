#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*
 * ====================================================================================================
 * Change Log
 * DD-MMM-YYYY, Name, #CCXX, Description
 * 21-Oct-2016, Sumit Maurya, #CC01, New other entity type added "ParallelOrgnHierarchy". and  Error message gets displayed from procedure while updating staus.
 * 02-April-2020,Vijay Kumar Prajapati,#CC02,Added ClientId In Rolemaster.
 * ====================================================================================================
 * 
 */

#endregion
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Resources;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class Masters_HO_Admin_ManageRole : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.Visible = false;
            if (!IsPostBack)
            {
                BindHierarchy();
                //BindSalesChanelType();
                chkActive.Checked = true;
                if (ViewState["EditUserId"] != null)
                { ViewState.Remove("EditUserId"); }
                FillRoleGrid();

            }
        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);

        }
    }
    private void BindHierarchy()
    {
        try
        {
            DataTable dt = new DataTable();

            ddlHierarchyLevel.Items.Clear();
            using (OrgHierarchyData objuser = new OrgHierarchyData())
            {
                objuser.CompanyId = PageBase.ClientId;/*#CC02 Added*/
                dt = objuser.GetAllHierarchyLevel();
            };
            String[] colArray = { "HierarchyLevelID", "HierarchyLevelName" };
            PageBase.DropdownBinding(ref ddlHierarchyLevel, dt, colArray);
            PageBase.DropdownBinding(ref ddlHierarchLevelSearch, dt, colArray);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    private void FillRoleGrid()
    {
        DataTable dtUsers;
        using (UserData objuser = new UserData())
        {

            objuser.RoleName = txtRoleSearch.Text.Trim();
            if (ddlHierarchLevelSearch.SelectedIndex > 0)
            {
                objuser.HierarchyLevelID = Convert.ToInt16(ddlHierarchLevelSearch.SelectedValue);
                objuser.SearchType = 2;
            }
            objuser.CompanyID = PageBase.ClientId;/*#CC02 Added*/
            dtUsers = objuser.GetUserRole();
        };
        if (dtUsers != null && dtUsers.Rows.Count > 0)
        {

            grdvwRoleList.DataSource = dtUsers;
            ViewState["Dtexport"] = dtUsers;
        }
        else
        {
            ViewState["Dtexport"] = null;
            grdvwRoleList.DataSource = null;
        }
        grdvwRoleList.DataBind();
        grdvwRoleList.Visible = true;
        updgrid.Update();


    }
    /*private void BindSalesChanelType()
    {
        try
        {
            DataTable dt = new DataTable();

            ddlSalesChanelType.Items.Clear();
            using (SalesChannelData objSalesChanel = new SalesChannelData())
            {
                dt = objSalesChanel.GetSalesChannelType();
            };
            String[] colArray = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref ddlSalesChanelType, dt, colArray);
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }*/
    protected void btnCreateRole_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPageRefereshed == true)
            {
                return;
            }
            if (txtRoleName.Text.Trim().Length == 0)
            {
                ucMsg.ShowWarning(Resources.Messages.MandatoryField);
                return;
            }
            if (txtRoleName.Text.Trim().Length > 0)
            {

                if (ddlHierarchyLevel.SelectedIndex == 0)
                {
                    ucMsg.ShowInfo("Role can map with Hierarchy Level ");
                    return;
                }
                Int32 result = 0;

                using (UserData objuser = new UserData())
                {

                    if (Convert.ToInt32(ViewState["EditUserId"]) == 0)
                    {
                        objuser.UserID = 0;
                    }
                    else
                    {
                        objuser.UserRoleID = Convert.ToInt16(ViewState["EditUserId"]);
                    }
                    objuser.RoleName = txtRoleName.Text.Trim();
                    if (ddlHierarchyLevel.SelectedIndex > 0)
                    {
                        objuser.HierarchyLevelID = Convert.ToInt16(ddlHierarchyLevel.SelectedValue);
                    }
                   /* else if (ddlSalesChanelType.SelectedIndex > 0)
                    {
                        objuser.SalesChanelTypeID = Convert.ToInt16(ddlSalesChanelType.SelectedValue);
                    }
                    else if (ddlOtherentityType.SelectedIndex > 0)
                    {
                        objuser.OtherEntityType = Convert.ToInt16(ddlOtherentityType.SelectedValue);
                    }*/
                    else
                    {
                        ucMsg.ShowInfo("Select Entity Type");
                        return;
                    }

                    /* #CC01 Add Start */
                    /*if (ddlOtherentityType.SelectedValue == "4" && ddlHierarchyLevel.SelectedValue == "0")
                    {
                        ucMsg.ShowInfo("Please select Hierarchy Level");
                        return;
                    }
                    if (ddlOtherentityType.SelectedValue == "4")
                    {
                        objuser.OtherEntityType = Convert.ToInt16(ddlOtherentityType.SelectedValue);
                    }*/

                    /* #CC01 Add End */
                    objuser.Status = Convert.ToBoolean(chkActive.Checked);
                    objuser.WAPAccess = Convert.ToInt32(chkWAPAccess.Checked);
                    objuser.CompanyID = PageBase.ClientId;/*#CC02 Added*/
                    result = objuser.InsertUpdateRoleinfo();
                    if (result > 0 && (objuser.ErrorMessage == null || objuser.ErrorMessage == ""))
                    {
                        if (ViewState["EditUserId"] == null)
                        {
                            ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                        }
                        else
                        {
                            ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                        }
                        ClearForm();
                        FillRoleGrid();
                        updgrid.Update();
                        chkActive.Checked = true;
                        return;
                    }
                    else
                    {
                        if (objuser.ErrorMessage != null && objuser.ErrorMessage != "")
                        {
                            ucMsg.ShowInfo(objuser.ErrorMessage);
                        }
                        else
                        {
                            ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                        }
                    }
                };
            }

        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);

        }
    }
    protected void btnActiveDeactive_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btnActiveDeactive = (ImageButton)sender;
            Int32 Result = 0;
            Int16 RoleID = Convert.ToInt16(btnActiveDeactive.CommandArgument);
            using (UserData ObjUser = new UserData())
            {

                ObjUser.UserRoleID = RoleID;
                Result = ObjUser.UpdateStatusRoleInfo();              
                if (ObjUser.ErrorMessage != null && ObjUser.ErrorMessage != "")
                {
                    ucMsg.ShowInfo(ObjUser.ErrorMessage);
                    updgrid.Update();
                    return;
                }

            };
            if (Result == 1)
            {

                ucMsg.ShowSuccess(Resources.Messages.StatusChanged);
            }
            else
            {
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            }
            FillRoleGrid();

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
        updgrid.Update();
    }
    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        chkActive.Enabled = true;
        ImageButton btnEdit = (ImageButton)sender;
        DataTable dtUser;
        using (UserData objuser = new UserData())
        {
            dtUser = objuser.GetUserRole(Convert.ToInt32(btnEdit.CommandArgument));
        };
        ViewState["EditUserId"] = Convert.ToInt32(btnEdit.CommandArgument);
        if (dtUser != null && dtUser.Rows.Count > 0)
        {
            BindHierarchy();
            //BindSalesChanelType();
            if (dtUser.Rows[0]["HierarchyLevelID"].ToString() != null && dtUser.Rows[0]["HierarchyLevelID"].ToString() != "")
            {

                ddlHierarchyLevel.Items.FindByValue(dtUser.Rows[0]["HierarchyLevelID"].ToString()).Selected = true;

            }
            /*if (dtUser.Rows[0]["SalesChanelTypeID"].ToString() != null && dtUser.Rows[0]["SalesChanelTypeID"].ToString() != "")
            {

                ddlSalesChanelType.Items.FindByValue(dtUser.Rows[0]["SalesChanelTypeID"].ToString()).Selected = true;
            }
            if (dtUser.Rows[0]["OtherEntityTypeID"].ToString() != null && dtUser.Rows[0]["OtherEntityTypeID"].ToString() != "")
            {
                ddlOtherentityType.SelectedValue = dtUser.Rows[0]["OtherEntityTypeID"].ToString();
                //ddlSalesChanelType.Items.FindByValue(dtUser.Rows[0]["SalesChanelTypeID"].ToString()).Selected = true;
            }
            ddlHierarchyLevel.Enabled = false;*/
            /*ddlSalesChanelType.Enabled = false;
            ddlOtherentityType.Enabled = false;*/
            txtRoleName.Text = (dtUser.Rows[0]["RoleName"].ToString());
            chkActive.Checked = Convert.ToBoolean(dtUser.Rows[0]["Status"]);
            chkWAPAccess.Checked = Convert.ToString(dtUser.Rows[0]["HasWAPAccess"]) == "Yes" ? true : false;
            btnCreateRole.Text = "Update";

        }
    }
    protected void btnSearchUser_Click(object sender, EventArgs e)
    {
        if (txtRoleSearch.Text != "" || ddlHierarchLevelSearch.SelectedIndex > 0)
            FillRoleGrid();
        else
            ucMsg.ShowInfo("Enter searching parameter(s)");
    }
    void ClearForm()
    {
        txtRoleSearch.Text = "";
        txtRoleName.Text = "";
        btnCreateRole.Text = "Submit";
        ViewState["Dtexport"] = null;
        ViewState["EditUserId"] = null;
        ddlHierarchyLevel.SelectedIndex = 0;
        /*ddlSalesChanelType.SelectedIndex = 0;
        ddlOtherentityType.SelectedIndex = 0;*/
        ddlHierarchyLevel.Enabled = true;
        /*ddlSalesChanelType.Enabled = true;
        ddlOtherentityType.Enabled = true;*/
        ddlHierarchLevelSearch.SelectedIndex = 0;
        FillRoleGrid();
        UpdSearch.Update();
        updgrid.Update();
    }
    protected void grdvwRoleList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        grdvwRoleList.PageIndex = e.NewPageIndex;
        FillRoleGrid();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearForm();
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        ClearForm();
    }
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {

            if (ViewState["Dtexport"] != null)
            {
                DataTable dt = (DataTable)ViewState["Dtexport"];
                string[] DsCol = new string[] { "RoleName", "HierarchyLevelName", "StatusText", "HasWAPAccess" };
                DataTable DsCopy = new DataTable();
                dt = dt.DefaultView.ToTable(true, DsCol);
                dt.Columns["RoleName"].ColumnName = "Role Name";
                dt.Columns["HierarchyLevelName"].ColumnName = "Hierarchy Level Name";
                //dt.Columns["SalesChannelTypeName"].ColumnName = "Sales Channel Type Name";
                //dt.Columns["OtherEntityTypeName"].ColumnName = "Other Entity Type Name";
                dt.Columns["StatusText"].ColumnName = "Status";
                dt.Columns["HasWAPAccess"].ColumnName = "Has WAP Access";
                if (dt.Rows.Count > 0)
                {
                    DataSet dtcopy = new DataSet();
                    dtcopy.Merge(dt);
                    dtcopy.Tables[0].AcceptChanges();
                    String FilePath = Server.MapPath("../../../");
                    string FilenameToexport = "RoleDetails";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);
                    ViewState["Dtexport"] = null;
                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                }
            }
            else
            {
                ucMsg.ShowInfo(Resources.Messages.NoRecord);
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }


    }
    protected void grdvwRoleList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Int32 CheckResult = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Int16 RoleID = Convert.ToInt16(grdvwRoleList.DataKeys[e.Row.RowIndex].Value);
            using (UserData ObjUser = new UserData())
            {
                ObjUser.UserRoleID = RoleID;
                CheckResult = ObjUser.CheckRoleExistence();
            };
            GridViewRow GVR = e.Row;
            ImageButton btnStatus = (ImageButton)GVR.FindControl("btnActiveDeactive");
            if (CheckResult > 0)
            {
                if (btnStatus != null)
                {
                    btnStatus.Attributes.Add("Onclick", "javascript:alert('Users already map with this role.You can not deactivate it.');{return false;}");

                }
                
            }
        }

    }
    //protected void ddlHierarchyLevel_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlHierarchyLevel.SelectedIndex != 0)
    //    {
    //        ddlSalesChanelType.SelectedValue = "0";
    //        ddlOtherentityType.SelectedValue = "0";
    //    }
    //}
    //protected void ddlSalesChanelType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlSalesChanelType.SelectedIndex != 0)
    //    {
    //        ddlOtherentityType.SelectedValue = "0";
    //        ddlHierarchyLevel.SelectedValue = "0";
    //    }
    //}
    //protected void ddlOtherentityType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlOtherentityType.SelectedIndex != 0 && ddlOtherentityType.SelectedValue != "4") /*#CC01 check Of ddlOtherentityType.SelectedValue != "4" added */
    //    {
    //        ddlHierarchyLevel.SelectedValue = "0";
    //        ddlSalesChanelType.SelectedValue = "0";
    //    }
    //}
}
