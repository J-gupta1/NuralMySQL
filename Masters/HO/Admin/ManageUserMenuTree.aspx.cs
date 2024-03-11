using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Internal;
using BussinessLogic;
using DataAccess;
using DevExpress.Web.ASPxTreeList;

/*
 * ======================================================================================================================
 * Change Log 
 * DD-MMM-YYYY, Name, #CCXX, Description
 * ======================================================================================================================
 * 22-Aug-2017, Sumit Maurya, #CC01, New check Added to get Role detail according to login user.
 * ======================================================================================================================
 */

public partial class Masters_HO_Admin_ManageUserMenuTree : PageBase
{
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMsg.Visible = false;
        MenuTreeBind();
        if (!IsPostBack)
        { //BindRoles();
            BindCompanyName();
            ddlRole.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    private void selecrfree()
    {
        foreach (TreeListNode node in ASPxTreeList1.GetSelectedNodes())
        {
            node.Selected = false;
        }
    }
    private void BindRoles()
    {
        try
        {
            DataTable dt = new DataTable();
            ddlRole.Items.Clear();
            /* using (DataAccess.UserData objuser = new DataAccess.UserData()) #CC01 Commented  */
            using (UserData objuser = new UserData())  /* #CC01 Added */
            {
                objuser.SearchType = 4;
                objuser.UserID = PageBase.UserId; /* #CC01 Added */
                objuser.Status = true;
                objuser.CompanyID = /*PageBase.ClientId*/Convert.ToInt32(ddlCompanyName.SelectedValue) ;
                dt = objuser.GetUserRole();
                
            };
            String[] colArray = { "RoleId", "RoleName" };
            PageBase.DropdownBinding(ref ddlRole, dt, colArray);
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }
    private void MenuTreeBind()
    {
        using (MenuData Mtreedata = new MenuData())
        {
            try
            {
                ASPxTreeList1.DataSource = null;
                ASPxTreeList1.DataBind();
                Mtreedata.UserID = PageBase.UserId;
                Mtreedata.RoleID = ddlRole.SelectedValue == "" ? Mtreedata.RoleID : Convert.ToInt32(ddlRole.SelectedValue);
                Mtreedata.CompanyId=ddlCompanyName.SelectedValue == "" ? Mtreedata.CompanyId : Convert.ToInt32(ddlCompanyName.SelectedValue);
                dt = Mtreedata.GetTreeMenuMapping();
                ASPxTreeList1.DataSource = dt;
                ASPxTreeList1.DataBind();
            }
            catch (Exception ex)
            {
                ucMsg.Visible = true;
                ucMsg.ShowError(Mtreedata.Error);
            }
        }
    }
    //protected void btnSearch_Click(object sender, EventArgs e)
    //{
    //    //selecrfree();
    //    //using (MenuData Mtreedata = new MenuData())
    //    //{
    //    //    try
    //    //    {
    //    //        Mtreedata.RoleID = Convert.ToInt32(ddlRole.SelectedValue);
    //    //        dt = Mtreedata.GetRoleMenuMapping();
    //    //        foreach (TreeListNode node in ASPxTreeList1.GetAllNodes())
    //    //        {
    //    //            foreach (DataRow dtr in dt.Rows)
    //    //            {
    //    //                if (node.Key == dtr["MenuID"].ToString())
    //    //                {
    //    //                    node.Selected = true;
    //    //                }
    //    //            }
    //    //        }
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        if (Mtreedata.Error != null && Mtreedata.Error != "" && Mtreedata.Error != "0")
    //    //        {
    //    //            ucMsg.Visible = true;
    //    //            ucMsg.ShowError(Mtreedata.Error);
    //    //        }
    //    //    }
    //    //}
    //}
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        using (MenuData Mtreedata = new MenuData())
        {
            try
            {
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("MenuID");
                foreach (TreeListNode node in ASPxTreeList1.GetSelectedNodes())
                {
                    dtNew.Rows.Add(Convert.ToInt32(node.Key));
                }
                Mtreedata.RoleID = Convert.ToInt32(ddlRole.SelectedValue);
                Mtreedata.CompanyId = Convert.ToInt32(ddlCompanyName.SelectedValue);
                Mtreedata.TreeMenuMapping(dtNew);
                ucMsg.Visible = true;
                ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
            }
            catch (Exception ex)
            {
                ucMsg.Visible = true;
                ucMsg.ShowError(Mtreedata.Error);
            }
        }
    }
    protected void ASPxTreeList1_HtmlDataCellPrepared(object sender, TreeListHtmlDataCellEventArgs e)
    {
        if (e.Column.FieldName.ToString() == "Status")
        {
            if (e.CellValue.ToString() != "True")
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
            }
        }
        if (e.Column.FieldName.ToString() == "AllowInMenu")
        {
            if (e.CellValue.ToString() != "True")
            {
                e.Cell.BackColor = System.Drawing.Color.Aquamarine;
            }
        }
        if (e.Column.FieldName.ToString() == "AccessFor")
        {
            if (e.CellValue.ToString() != "All")
            {
                e.Cell.BackColor = System.Drawing.Color.SkyBlue;
            }
        }
    }
    protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        selecrfree();
        using (MenuData Mtreedata = new MenuData())
        {
            try
            {
                Mtreedata.RoleID = Convert.ToInt32(ddlRole.SelectedValue);
                Mtreedata.CompanyId = Convert.ToInt32(ddlCompanyName.SelectedValue);
                dt = Mtreedata.GetRoleMenuMapping();
                foreach (TreeListNode node in ASPxTreeList1.GetAllNodes())
                {
                    foreach (DataRow dtr in dt.Rows)
                    {
                        if (node.Key == dtr["MenuID"].ToString())
                        {
                            node.Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Mtreedata.Error != null && Mtreedata.Error != "" && Mtreedata.Error != "0")
                {
                    ucMsg.Visible = true;
                    ucMsg.ShowError(Mtreedata.Error);
                }
            }
        }
    }
    protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindRoles();
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
            ucMsg.Visible = true;
            ucMsg.ShowError(ex.Message);
        }
    }
}
