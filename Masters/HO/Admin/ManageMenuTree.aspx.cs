using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Web.ASPxEditors;

public partial class Masters_HO_Admin_ManageMenuTree : System.Web.UI.Page
{
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        hdnNavigation.Value = null;
        hdnMenuType.Value = null;
        btnDelete.Attributes.Add("Onclick", "if(!confirm('Are you sure you want to delete selected node?')){return false;}");
        ucMsg.Visible = false;
        MenuTreeBind();
    }
    private void MenuTreeBind()
    {
        using (MenuData Mtreedata = new MenuData())
        {
            try
            {
                dt = Mtreedata.GetTreeMenuInfo();
                ASPxTreeList1.DataSource = dt;
                ASPxTreeList1.DataBind();
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
    protected void ASPxTreeList1_NodeUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        TextBox txtMenuName = ASPxTreeList1.FindEditFormTemplateControl("txtMenuName") as TextBox;
        TextBox txtMenuDescription = ASPxTreeList1.FindEditFormTemplateControl("txtMenuDescription") as TextBox;
        TextBox txtNavigationURL = ASPxTreeList1.FindEditFormTemplateControl("txtNavigationURL") as TextBox;
        CheckBox chkStatus = (CheckBox)ASPxTreeList1.FindEditFormTemplateControl("chkStatus"); //as CheckBox;
        CheckBox chkAllowInMenu = ASPxTreeList1.FindEditFormTemplateControl("chkAllowInMenu") as CheckBox;
        CheckBox chkAccessFor = ASPxTreeList1.FindEditFormTemplateControl("chkAccessFor") as CheckBox;
        DropDownList ddlMenuType = ASPxTreeList1.FindEditFormTemplateControl("ddlMenuType") as DropDownList;
        CheckBox chkAccessRole = ASPxTreeList1.FindEditFormTemplateControl("chkAccessRole") as CheckBox;
        HiddenField hdnMenuID = ASPxTreeList1.FindEditFormTemplateControl("hdnMenuID") as HiddenField;
        HiddenField hdnParentMenuID = ASPxTreeList1.FindEditFormTemplateControl("hdnParentMenuID") as HiddenField;
        if (txtMenuName.Text != "" && txtMenuDescription.Text != "")
        {
            using (MenuData MtreeUpd = new MenuData())
            {
                try
                {
                    MtreeUpd.MenuName = txtMenuName.Text;
                    MtreeUpd.MenuDescription = txtMenuDescription.Text;
                    MtreeUpd.NavigationURL = txtNavigationURL.Text;
                    if (chkStatus.Checked) { MtreeUpd.Status = 1; } else { MtreeUpd.Status = 0; }
                    if (chkAllowInMenu.Checked) { MtreeUpd.AllowInMenu = 1; } else { MtreeUpd.AllowInMenu = 0; }
                    if (chkAccessFor.Checked) { MtreeUpd.AccessFor = 1; } else { MtreeUpd.AccessFor = 0; }
                    if (chkAccessRole.Checked) { MtreeUpd.AccessRole = 1; } else { MtreeUpd.AccessRole = 0; }
                    MtreeUpd.ParentMenuID = Convert.ToInt32(hdnParentMenuID.Value);
                    MtreeUpd.MenuID = Convert.ToInt32(hdnMenuID.Value);
                    MtreeUpd.MenuType = ddlMenuType.SelectedIndex;
                    MtreeUpd.UpdateTreeMenuInfo();
                    if (MtreeUpd.InsError != null && txtNavigationURL.Text != "")
                    {
                        ucMsg.Visible = true;
                        if (MtreeUpd.InsError == "And The NavigationURL Must Be NULL Becouse This Node Contain Child")
                        {
                            ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull +" "+ MtreeUpd.InsError);
                        }
                        else
                        {
                            ucMsg.ShowError(MtreeUpd.InsError);
                        }
                    }
                    else
                    {
                        if (chkAccessRole.Checked)
                        {
                            ucMsg.Visible = true;
                            ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull+" "+"And Map Again for HO Role");
                        }
                        else
                        {
                            ucMsg.Visible = true;
                            ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (MtreeUpd.Error != null && MtreeUpd.Error != "" && MtreeUpd.Error != "0")
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowError(MtreeUpd.Error);
                    }
                }
            }
            e.Cancel = true;
            ASPxTreeList1.CancelEdit();
            MenuTreeBind();
        }
        else
        {
            ucMsg.Visible = true;
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            e.Cancel = true;
        }
    }
    protected void ASPxTreeList1_NodeInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        TextBox txtMenuName = ASPxTreeList1.FindEditFormTemplateControl("txtMenuName") as TextBox;
        TextBox txtMenuDescription = ASPxTreeList1.FindEditFormTemplateControl("txtMenuDescription") as TextBox;
        TextBox txtNavigationURL = ASPxTreeList1.FindEditFormTemplateControl("txtNavigationURL") as TextBox;
        CheckBox chkStatus = ASPxTreeList1.FindEditFormTemplateControl("chkStatus") as CheckBox;
        CheckBox chkAllowInMenu = ASPxTreeList1.FindEditFormTemplateControl("chkAllowInMenu") as CheckBox;
        CheckBox chkAccessFor = ASPxTreeList1.FindEditFormTemplateControl("chkAccessFor") as CheckBox;
        DropDownList ddlMenuType = ASPxTreeList1.FindEditFormTemplateControl("ddlMenuType") as DropDownList;
        CheckBox chkAccessRole = ASPxTreeList1.FindEditFormTemplateControl("chkAccessRole") as CheckBox;
        TreeListNode parentNode = ASPxTreeList1.FindNodeByKeyValue(ASPxTreeList1.NewNodeParentKey);
        if (txtMenuName.Text != "" && txtMenuDescription.Text != "")
        {
            using (MenuData MtreeIns = new MenuData())
            {
                try
                {
                    MtreeIns.MenuName = txtMenuName.Text;
                    MtreeIns.MenuDescription = txtMenuDescription.Text;
                    MtreeIns.NavigationURL = txtNavigationURL.Text;
                    if (chkStatus.Checked) { MtreeIns.Status = 1; } else { MtreeIns.Status = 0; }
                    if (chkAllowInMenu.Checked) { MtreeIns.AllowInMenu = 1; } else { MtreeIns.AllowInMenu = 0; }
                    if (chkAccessFor.Checked) { MtreeIns.AccessFor = 1; } else { MtreeIns.AccessFor = 0; }
                    if (chkAccessRole.Checked) { MtreeIns.AccessRole = 1; } else { MtreeIns.AccessRole = 0; }
                    if (parentNode.Key.Equals("")) { MtreeIns.ParentMenuID = 0; } else { MtreeIns.ParentMenuID = Convert.ToInt32(parentNode.Key); }
                    MtreeIns.DisplayOrderNumber = parentNode.ChildNodes.Count;//theare is a bug
                    if (MtreeIns.NavigationURL.Equals("") && ddlMenuType.SelectedIndex == 0)
                    {
                        MtreeIns.MenuType = ddlMenuType.SelectedIndex;
                    }
                    else if (MtreeIns.NavigationURL != "" && ddlMenuType.SelectedIndex!=0)
                    {
                        MtreeIns.MenuType = ddlMenuType.SelectedIndex;
                    }
                    else
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowWarning("Missmatch condition for NavigationURL and Menu Type");
                        e.Cancel = true;
                        return;
                    }
                    MtreeIns.FillTreeMenuInfo();
                    if (MtreeIns.InsError != null)
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowError(MtreeIns.InsError);
                    }
                    else
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    }
                }
                catch (Exception ex)
                {
                    if (MtreeIns.Error != null && MtreeIns.Error != "" && MtreeIns.Error != "0")
                    {
                        ucMsg.Visible = true;
                        ucMsg.ShowError(MtreeIns.Error);
                    }
                }
            }
            e.Cancel = true;
            ASPxTreeList1.CancelEdit();
            MenuTreeBind();
        }
        else
        {
            ucMsg.Visible = true;
            ucMsg.ShowWarning(Resources.Messages.MandatoryField);
            e.Cancel = true;
        }
    }
    //protected void ASPxTreeList1_NodeDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    //{
    //    using (MenuData MtreeDel = new MenuData())
    //    {
    //        try
    //        {
    //            string key = e.Keys[0].ToString();
    //            TreeListNode node = ASPxTreeList1.FindNodeByKeyValue(key);
    //            MtreeDel.MenuID =Convert.ToInt32(node.Key);
    //            //MtreeDel.DelTreeMenu();
    //            ucMsg.Visible = true;
    //            ucMsg.ShowSuccess(Resources.Messages.Delete);
    //        }
    //        catch (Exception ex)
    //        {
    //            ucMsg.Visible = true;
    //            ucMsg.ShowError(ex.ToString());
    //        }
    //    }
    //    e.Cancel = true;
    //    ASPxTreeList1.CancelEdit();
    //    MenuTreeBind();
    //}
    protected void ASPxTreeList1_NodeValidating(object sender, TreeListNodeValidationEventArgs e)
    {
        //TextBox txtMenuName = ASPxTreeList1.FindEditFormTemplateControl("txtMenuName") as TextBox;
        //TextBox txtMenuDescription = ASPxTreeList1.FindEditFormTemplateControl("txtMenuDescription") as TextBox;        
    }
    protected void ASPxTreeList1_ProcessDragNode(object sender, TreeListNodeDragEventArgs e)
    {
        using (MenuData dragNode = new MenuData())
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
                dragNode.DragNode();
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
        MenuTreeBind();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        CheckBox chkStatus = (CheckBox)ASPxTreeList1.FindEditFormTemplateControl("chkStatus"); //as CheckBox;
        CheckBox chkAllowInMenu = ASPxTreeList1.FindEditFormTemplateControl("chkAllowInMenu") as CheckBox;
        CheckBox chkAccessFor = ASPxTreeList1.FindEditFormTemplateControl("chkAccessFor") as CheckBox;
        CheckBox chkAccessRole = ASPxTreeList1.FindEditFormTemplateControl("chkAccessRole") as CheckBox;
        if (chkStatus == null && chkAccessFor == null && chkAllowInMenu == null && chkAccessRole==null)
        {
            using (MenuData MtreeDel = new MenuData())
            {
                try
                {
                    foreach (TreeListNode node in ASPxTreeList1.GetSelectedNodes())
                    {
                        if (node.HasChildren)
                        {
                            ucMsg.ShowInfo("The node which MenuID is '" + node.Key + "', Contain child");
                            return;
                        }
                        MtreeDel.MenuID = Convert.ToInt32(node.Key);
                        MtreeDel.DelTreeMenu();
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
        }
        else
        {
            ucMsg.ShowWarning("Please Select Proper Node For Deleting Node and close the Edit/New");
            ASPxTreeList1.CollapseAll();
        }
    }
    protected void imgbtnUP_Click(object sender, ImageClickEventArgs e)
    {
        CheckBox chkStatus = (CheckBox)ASPxTreeList1.FindEditFormTemplateControl("chkStatus"); //as CheckBox;
        CheckBox chkAllowInMenu = ASPxTreeList1.FindEditFormTemplateControl("chkAllowInMenu") as CheckBox;
        CheckBox chkAccessFor = ASPxTreeList1.FindEditFormTemplateControl("chkAccessFor") as CheckBox;
        CheckBox chkAccessRole = ASPxTreeList1.FindEditFormTemplateControl("chkAccessRole") as CheckBox;
        if (chkStatus == null && chkAccessFor == null && chkAllowInMenu == null && chkAccessRole==null)
        {
            using (MenuData objChengePosition = new MenuData())
            {
                try
                {
                    //ASPxTreeList1.GetSelectedNodes().Count;
                    foreach (TreeListNode node in ASPxTreeList1.GetSelectedNodes())
                    {
                        if (ASPxTreeList1.GetSelectedNodes().Count!=1)
                        {
                            ucMsg.ShowInfo("Plese Select One Node");
                            return;
                        }
                        objChengePosition.MenuID = Convert.ToInt32(node.Key);
                        objChengePosition.ParentMenuID = node.ParentNode.Key == "" ? 0 : Convert.ToInt32(node.ParentNode.Key);
                        objChengePosition.ChengePositionTreeMenu("UP");
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
            MenuTreeBind();
        }
        else
        {
            ucMsg.ShowWarning("Please Select Proper Node To Change Position and close the Edit/New");
            ASPxTreeList1.CollapseAll();
        }
    }
    protected void imgbtnDown_Click(object sender, ImageClickEventArgs e)
    {
        CheckBox chkStatus = (CheckBox)ASPxTreeList1.FindEditFormTemplateControl("chkStatus"); //as CheckBox;
        CheckBox chkAllowInMenu = ASPxTreeList1.FindEditFormTemplateControl("chkAllowInMenu") as CheckBox;
        CheckBox chkAccessFor = ASPxTreeList1.FindEditFormTemplateControl("chkAccessFor") as CheckBox;
        CheckBox chkAccessRole = ASPxTreeList1.FindEditFormTemplateControl("chkAccessRole") as CheckBox;
        if (chkStatus == null && chkAccessFor == null && chkAllowInMenu == null && chkAccessRole==null)
        {
            using (MenuData objChengePosition = new MenuData())
            {
                try
                {
                    //ASPxTreeList1.GetSelectedNodes().Count;
                    foreach (TreeListNode node in ASPxTreeList1.GetSelectedNodes())
                    {
                        if (ASPxTreeList1.GetSelectedNodes().Count != 1)
                        {
                            ucMsg.ShowInfo("Plese Select One Node");
                            return;
                        }
                        objChengePosition.MenuID = Convert.ToInt32(node.Key);
                        objChengePosition.ParentMenuID = node.ParentNode.Key == "" ? 0 : Convert.ToInt32(node.ParentNode.Key);
                        objChengePosition.ChengePositionTreeMenu("Down");
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
            MenuTreeBind();
        }
        else
        {
            ucMsg.ShowWarning("Please Select Proper Node To Change Position and close the Edit/New");
            ASPxTreeList1.CollapseAll();
        }
    }
    protected void ASPxTreeList1_CommandColumnButtonInitialize(object sender, TreeListCommandColumnButtonEventArgs e)
    {
        TreeListNode Node = ASPxTreeList1.FindNodeByKeyValue(e.NodeKey);
        if (((System.Data.DataRowView)(Node.DataItem)).Row.ItemArray[7].ToString() != "")
        {
            e.CommandColumn.NewButton.Visible = false;
        }
        if (((System.Data.DataRowView)(Node.DataItem)).Row.ItemArray[7].ToString() == "")
        {
            e.CommandColumn.NewButton.Visible = true;
        }
    }
    protected void ASPxTreeList1_StartNodeEditing(object sender, TreeListNodeEditingEventArgs e)
    {
        TreeListNode Node = ASPxTreeList1.FindNodeByKeyValue(e.NodeKey);
        if (((System.Data.DataRowView)(Node.DataItem)).Row.ItemArray[7].ToString() == "")
        {
            hdnNavigation.Value = "true";
        }
        hdnMenuType.Value = ((System.Data.DataRowView)(Node.DataItem)).Row.ItemArray[10].ToString();
    }
    protected void ASPxTreeList1_PreRender(object sender, EventArgs e)
    {
        TextBox txtNavigationURL = ASPxTreeList1.FindEditFormTemplateControl("txtNavigationURL") as TextBox;
        DropDownList ddlMenuType = ASPxTreeList1.FindEditFormTemplateControl("ddlMenuType") as DropDownList;
        if (hdnNavigation.Value.Equals("true"))
        {
            txtNavigationURL.Enabled = false;
        }
        if (hdnMenuType.Value.Equals("Not applicable"))
        {
            ddlMenuType.SelectedValue = "0";
            ddlMenuType.Enabled = false;
        }
        if (hdnMenuType.Value.Equals("Web"))
        {
            ddlMenuType.SelectedValue = "1";
        }
        if (hdnMenuType.Value.Equals("WAP"))
        {
            ddlMenuType.SelectedValue = "2";
        }
    }
}

