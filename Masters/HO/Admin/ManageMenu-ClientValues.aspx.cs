using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;
using DevExpress.Web.ASPxTreeList;

public partial class Masters_HO_Admin_ManageMenu_ClientValues : PageBase
{
    DataTable dtclient = new DataTable();
    DataTable dtDefault = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMsg.Visible = false;
        if (!IsPostBack)
        {
            BindDDLClient();
            btnload.Enabled = false;
        }
        if (ddlClientName.SelectedIndex != 0)
        {
            MenuTreeBind();
            ClientTreeMenuBind();
        }
    }

    # region User_Define_Function
    private void BindDDLClient()
    {
        using (ClientManageMenu objClientName = new ClientManageMenu())
        {
            try
            {
                string[] str = { "ClientID", "ClientName" };
                objClientName.Condition = 0;
                DataTable dt = objClientName.GetClientName();
                PageBase.DropdownBinding(ref ddlClientName, dt, str);
            }
            catch (Exception ex)
            {
                ucMsg.Visible = true;
                ucMsg.ShowError(ex.ToString());
            }
        }
    }
    private void MenuTreeBind()
    {
        using (ClientManageMenu objDefaultMenu = new ClientManageMenu())
        {
            try
            {
                objDefaultMenu.UserId = PageBase.UserId;
                dtDefault = objDefaultMenu.GetDeflaultMenuData();
                ASPxTreeList1.DataSource = dtDefault;
                ASPxTreeList1.DataBind();
            }
            catch (Exception ex)
            {
                ucMsg.Visible = true;
                ucMsg.ShowError(objDefaultMenu.Error);
            }
        }
    }
    private void ClientTreeMenuBind()
    {
        DataTable dt;
        using (ClientManageMenu objClientMenu = new ClientManageMenu())
        {
            try
            {
                dt = objClientMenu.GetClientMenuData(Convert.ToInt32(ddlClientName.SelectedValue));
                ASPxTreeList2.DataSource = dt;
                ASPxTreeList2.DataBind();
            }
            catch (Exception ex)
            {
                ucMsg.Visible = true;
                ucMsg.ShowError(objClientMenu.Error);
            }
        }
    }
    //private DataTable NewClientData(int ClientId)
    //{
    //    DataTable dt;
    //    using (ClientManageMenu objMenuData = new ClientManageMenu())
    //    {
    //        objMenuData.UserId = PageBase.UserId;
    //        dtclient = objMenuData.GetClientMenuData(ClientId);
    //        dtDefault = objMenuData.GetDeflaultMenuData();
    //    }
    //    dt = dtDefault.Clone();
    //    string[] strclm = new string[dtDefault.Columns.Count];
    //    foreach (TreeListNode node in ASPxTreeList1.GetSelectedNodes())
    //    {
    //        foreach (DataRow dtr in dtDefault.Rows)
    //        {
    //            if (node.Key == dtr["MenuID"].ToString())
    //            {
    //                strclm[0] = dtr["MenuID"].ToString();
    //                strclm[1] = dtr["ParentMenuID"].ToString();
    //                strclm[2] = dtr["MenuName"].ToString();
    //                strclm[3] = dtr["MenuDescription"].ToString();
    //                strclm[4] = dtr["AccessRole"].ToString();
    //                strclm[5] = dtr["AllowInMenu"].ToString();
    //                strclm[6] = dtr["Status"].ToString();
    //                dt.Rows.Add(strclm);
    //            }
    //        }
    //    }
    //    string[] strColumns = new string[dtDefault.Columns.Count];
    //    int i = 0;
    //    foreach (DataColumn dc in dtDefault.Columns)
    //    {
    //        strColumns[i] = dc.ColumnName;
    //        i++;
    //    }
    //    DataTable dtDummy;
    //    dtDummy = dtclient.Clone();
    //    dtDummy.Merge(dtclient);
    //    dtDummy.Merge(dt);
    //    dtclient.Clear();
    //    dtclient = dtDummy.DefaultView.ToTable(true, strColumns);
    //    dtclient.AcceptChanges();
    //    return dtclient;
    //}
    private DataTable SelectedData(int ClientId)
    {
        DataTable dtSelect = new DataTable();
        try
        {
            using (ClientManageMenu objMenuData = new ClientManageMenu())
            {
                try
                {
                    objMenuData.UserId = PageBase.UserId;
                    dtDefault = objMenuData.GetDeflaultMenuData();
                }
                catch (Exception ex)
                {
                    ucMsg.Visible = true;
                    ucMsg.ShowError(objMenuData.Error);
                }
            }
            dtSelect.Columns.Add("MenuID", typeof(Int32));
            dtSelect.Columns.Add("ClientID", typeof(Int32));
            dtSelect.Columns.Add("MenuName", typeof(String));
            dtSelect.Columns.Add("MenuDescription", typeof(String));
            dtSelect.Columns.Add("DisplayOrderNumber", typeof(Int32));
            dtSelect.Columns.Add("ParentMenuID", typeof(Int32));
            dtSelect.Columns.Add("Status", typeof(Boolean));
            dtSelect.Columns.Add("AllowInMenu", typeof(Boolean));
            dtSelect.Columns.Add("AccessRole", typeof(Int32));
            foreach (TreeListNode node in ASPxTreeList1.GetSelectedNodes())
            {
                foreach (DataRow dtr in dtDefault.Rows)
                {
                    if (node.Key == dtr["MenuID"].ToString())
                    {
                        DataRow dtrow = dtSelect.NewRow();
                        dtrow["MenuID"] = Convert.ToInt32(dtr["MenuID"]);
                        dtrow["ClientID"] = ClientId;
                        dtrow["MenuName"] = dtr["MenuName"].ToString();
                        dtrow["MenuDescription"] = dtr["MenuDescription"].ToString();
                        dtrow["DisplayOrderNumber"] = Convert.ToInt32(dtr["DisplayOrderNumber"]);
                        dtrow["ParentMenuID"] = Convert.ToInt32(dtr["ParentMenuID"]);
                        dtrow["Status"] = Convert.ToBoolean(dtr["Status"]);
                        dtrow["AllowInMenu"] = Convert.ToBoolean(dtr["AllowInMenu"]);
                        dtrow["AccessRole"] = dtr["AccessRole"].ToString() == "All" ? 0 : 1;
                        dtSelect.Rows.Add(dtrow);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.Visible = true;
            ucMsg.ShowError(ex.ToString());
        }
        return dtSelect;
    }
    # endregion
    # region Control_Function
    protected void btnProceed_Click(object sender, EventArgs e)
    {
        ucMsg.Visible = false;
        ddlClientName.Enabled = false;
        MenuTreeBind();
        ClientTreeMenuBind();
        btnload.Enabled = true;
        ASPxTreeList2.CancelEdit();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ucMsg.Visible = false;
        btnload.Enabled = false;
        foreach (TreeListNode node in ASPxTreeList1.GetSelectedNodes())
        {
            node.Selected = false;
        }
        ddlClientName.Enabled = true;
        BindDDLClient();
        DataTable dt=new DataTable();
        ASPxTreeList1.DataSource = dt;
        ASPxTreeList1.DataBind();
        ASPxTreeList2.DataSource = dt;
        ASPxTreeList2.DataBind();
        ASPxTreeList2.CancelEdit();
        btnload.Enabled = false;
    }
    protected void btnload_Click(object sender, EventArgs e)
    {
        using (ClientManageMenu objInsData = new ClientManageMenu())
        {
            try
            {
                objInsData.InsClientMenuValue(SelectedData(Convert.ToInt32(ddlClientName.SelectedValue)));
            }
            catch (Exception ex)
            {
                ucMsg.Visible = true;
                ucMsg.ShowError(objInsData.Error);
            }
        }
        ClientTreeMenuBind();
        ASPxTreeList2.CancelEdit();
    }
    protected void ASPxTreeList2_NodeUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        TextBox txtMenuName = ASPxTreeList2.FindEditFormTemplateControl("txtMenuName") as TextBox;
        TextBox txtMenuDescription = ASPxTreeList2.FindEditFormTemplateControl("txtMenuDescription") as TextBox;
        CheckBox chkStatus = (CheckBox)ASPxTreeList2.FindEditFormTemplateControl("chkStatus"); //as CheckBox;
        CheckBox chkAllowInMenu = ASPxTreeList2.FindEditFormTemplateControl("chkAllowInMenu") as CheckBox;
        CheckBox chkAccessRole = ASPxTreeList2.FindEditFormTemplateControl("chkAccessRole") as CheckBox;
        HiddenField hdnMenuID = ASPxTreeList2.FindEditFormTemplateControl("hdnMenuID") as HiddenField;
        HiddenField hdnParentMenuID = ASPxTreeList2.FindEditFormTemplateControl("hdnParentMenuID") as HiddenField;
        if (txtMenuName.Text != "" && txtMenuDescription.Text != "")
        {
            using (ClientManageMenu objUpdClientMenu = new ClientManageMenu())
            {
                try
                {
                    objUpdClientMenu.MenuName = txtMenuName.Text;
                    objUpdClientMenu.MenuDesc= txtMenuDescription.Text;
                    if (chkStatus.Checked) { objUpdClientMenu.Status = 1; } else { objUpdClientMenu.Status = 0; }
                    if (chkAllowInMenu.Checked) { objUpdClientMenu.AllowInMenu = 1; } else { objUpdClientMenu.AllowInMenu = 0; }
                    if (chkAccessRole.Checked) { objUpdClientMenu.AccessRole = 1; } else { objUpdClientMenu.AccessRole = 0; }
                    objUpdClientMenu.ParentMenuID = Convert.ToInt32(hdnParentMenuID.Value);
                    objUpdClientMenu.MenuID = Convert.ToInt32(hdnMenuID.Value);
                    objUpdClientMenu.ClientId = Convert.ToInt32(ddlClientName.SelectedValue);
                    objUpdClientMenu.UpdClientMenuValue();
                    ucMsg.Visible = true;
                    ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                }
                catch (Exception ex)
                {
                    if (objUpdClientMenu.Error != null && objUpdClientMenu.Error != "" && objUpdClientMenu.Error != "0")
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowError(objUpdClientMenu.Error);
                    }
                }
            }
            e.Cancel = true;
            ASPxTreeList1.CancelEdit();
            MenuTreeBind();
            ClientTreeMenuBind();
        }
        else
        {
            ucMsg.Visible = true;
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            e.Cancel = true;
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        CheckBox chkStatus = (CheckBox)ASPxTreeList2.FindEditFormTemplateControl("chkStatus"); //as CheckBox;
        CheckBox chkAllowInMenu = ASPxTreeList2.FindEditFormTemplateControl("chkAllowInMenu") as CheckBox;
        CheckBox chkAccessRole = ASPxTreeList2.FindEditFormTemplateControl("chkAccessRole") as CheckBox;
        if (chkStatus == null && chkAccessRole == null && chkAllowInMenu == null)
        {
            using (ClientManageMenu objDelClientMenu = new ClientManageMenu())
            {
                try
                {
                    foreach (TreeListNode node in ASPxTreeList2.GetSelectedNodes())
                    {
                        if (node.HasChildren)
                        {
                            ucMsg.Visible = true;
                            ucMsg.ShowInfo("The deleting node has Contain child");
                            return;
                        }
                        objDelClientMenu.MenuID = Convert.ToInt32(node.Key);
                        objDelClientMenu.ClientId = Convert.ToInt32(ddlClientName.SelectedValue);
                        objDelClientMenu.DelClientMenuValue();
                        ucMsg.Visible = true;
                        ucMsg.ShowSuccess(Resources.Messages.Delete);
                    }
                }
                catch (Exception ex)
                {
                    ucMsg.Visible = true;
                    ucMsg.ShowError(ex.ToString());
                }
            }
            MenuTreeBind();
            ClientTreeMenuBind();
        }
        else
        {
            ucMsg.Visible = true;
            ucMsg.ShowWarning("Please Select Proper Node For Deleting Node and close the Edit/New");
            ASPxTreeList1.CollapseAll();
        }
    }
    protected void imgbtnUP_Click(object sender, ImageClickEventArgs e)
    {
        CheckBox chkStatus = (CheckBox)ASPxTreeList2.FindEditFormTemplateControl("chkStatus"); //as CheckBox;
        CheckBox chkAllowInMenu = ASPxTreeList2.FindEditFormTemplateControl("chkAllowInMenu") as CheckBox;
        CheckBox chkAccessRole = ASPxTreeList2.FindEditFormTemplateControl("chkAccessRole") as CheckBox;
        if (chkStatus == null && chkAllowInMenu == null && chkAccessRole == null)
        {
            using (ClientManageMenu objChengePosition = new ClientManageMenu())
            {
                try
                {
                    foreach (TreeListNode node in ASPxTreeList2.GetSelectedNodes())
                    {
                        if (ASPxTreeList2.GetSelectedNodes().Count != 1)
                        {
                            ucMsg.Visible = true;
                            ucMsg.ShowInfo("Plese Select One Node");
                            return;
                        }
                        objChengePosition.MenuID = Convert.ToInt32(node.Key);
                        objChengePosition.ParentMenuID = node.ParentNode.Key == "" ? 0 : Convert.ToInt32(node.ParentNode.Key);
                        objChengePosition.ClientId = Convert.ToInt32(ddlClientName.SelectedValue);
                        objChengePosition.ChengePositionClientTreeMenu("UP");
                        if (objChengePosition.InsError != null)
                        {
                            ucMsg.Visible = true;
                            ucMsg.ShowError(objChengePosition.InsError);
                        }
                        else
                        {
                            ucMsg.Visible = true;
                            ucMsg.ShowSuccess("Order Changed Successfully");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ucMsg.Visible = true;
                    ucMsg.ShowError(ex.ToString());
                }
            }
            ClientTreeMenuBind();
        }
        else
        {
            ucMsg.Visible = true;
            ucMsg.ShowWarning("Please Select Proper Node To Change Position and close the Edit/New");
            ASPxTreeList1.CollapseAll();
        }
    }
    protected void imgbtnDown_Click(object sender, ImageClickEventArgs e)
    {
        CheckBox chkStatus = (CheckBox)ASPxTreeList2.FindEditFormTemplateControl("chkStatus"); //as CheckBox;
        CheckBox chkAllowInMenu = ASPxTreeList2.FindEditFormTemplateControl("chkAllowInMenu") as CheckBox;
        CheckBox chkAccessRole = ASPxTreeList2.FindEditFormTemplateControl("chkAccessRole") as CheckBox;
        if (chkStatus == null && chkAllowInMenu == null && chkAccessRole == null)
        {
            using (ClientManageMenu objChengePosition = new ClientManageMenu())
            {
                try
                {
                    foreach (TreeListNode node in ASPxTreeList2.GetSelectedNodes())
                    {
                        if (ASPxTreeList2.GetSelectedNodes().Count != 1)
                        {
                            ucMsg.Visible = true;
                            ucMsg.ShowInfo("Plese Select One Node");
                            return;
                        }
                        objChengePosition.MenuID = Convert.ToInt32(node.Key);
                        objChengePosition.ParentMenuID = node.ParentNode.Key == "" ? 0 : Convert.ToInt32(node.ParentNode.Key);
                        objChengePosition.ClientId = Convert.ToInt32(ddlClientName.SelectedValue);
                        objChengePosition.ChengePositionClientTreeMenu("Down");
                        if (objChengePosition.InsError != null)
                        {
                            ucMsg.Visible = true;
                            ucMsg.ShowError(objChengePosition.InsError);
                        }
                        else
                        {
                            ucMsg.Visible = true;
                            ucMsg.ShowSuccess("Order Changed Successfully");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ucMsg.Visible = true;
                    ucMsg.ShowError(ex.ToString());
                }
            }
            ClientTreeMenuBind();
        }
        else
        {
            ucMsg.Visible = true;
            ucMsg.ShowWarning("Please Select Proper Node To Change Position and close the Edit/New");
            ASPxTreeList1.CollapseAll();
        }
    }
    protected void ASPxTreeList2_ProcessDragNode(object sender, TreeListNodeDragEventArgs e)
    {
        using (ClientManageMenu dragNode = new ClientManageMenu())
        {
            dragNode.MenuID = Convert.ToInt32(e.Node.Key);
            dragNode.DisplayOrderNumber = e.NewParentNode.ChildNodes.Count + 1;
            if (e.NewParentNode.Key != "")
                dragNode.NewParent = Convert.ToInt32(e.NewParentNode.Key);
            else
                dragNode.NewParent = 0;

            if (e.Node.ParentNode.Key != "")
                dragNode.OldParent = Convert.ToInt32(e.Node.ParentNode.Key);
            else
                dragNode.OldParent = 0;
            dragNode.ClientId = Convert.ToInt32(ddlClientName.SelectedValue);
            dragNode.ClientMenuDragNode();
            if (dragNode.InsError != null)
            {
                ucMsg.Visible = true;
                ucMsg.ShowError(dragNode.InsError);
            }
            else
            {
                ucMsg.Visible = true;
                ucMsg.ShowSuccess("Node Moved Successfully");
                //ucMsg.ShowSuccess("The Node, Which Menu ID Is '" + e.Node.Key + "' Moved From Parent Menu Id '" + e.Node.ParentNode.Key + "' To '" + e.NewParentNode.Key + "'");
            }
        }
        e.Cancel = true;
        ClientTreeMenuBind();
    }
    # endregion
}
