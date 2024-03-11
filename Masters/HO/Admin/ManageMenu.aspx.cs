using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;

public partial class Masters_HO_Admin_ManageMenu :PageBase
{
    MenuData MMOb = new MenuData();
    string urlImg;
    DataTable  dt;
    string EditParentName;
    string AllowInMenu;
    protected string AppAsset = PageBase.siteURL + "/" + strAssets; // used to get Application Assets Infomation 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["SearchMode"] = "false";
            ViewState["EditMode"] = "false";
            BindList();

            ViewState["EditMode1"] = "false";
            ViewState["EditMode2"] = "false";



        }
        ucMsg.ShowControl = false;
    }
    protected string ImageSwap(Int16 IsActive)
    {
        string imgUrl = AppAsset + "/CSS/images/decative.png";
        if (IsActive == 1)
        {
            imgUrl = AppAsset + "/CSS/images/active.png";
        }

        if (ViewState["EditMode"] == "false")
            ViewState["URLIMG"] = imgUrl;
        return imgUrl;
    }

  

    void BindList()
    {
        try
        {
            txtKeyword.Text = "";
            dt = MMOb.GetMenuInfo();
            if (dt.Rows.Count > 0)
            {
                dlstPrc.Visible = true;
                DataView MenuDV = new DataView(dt, "NavigationURL is null and ParentMenuID=0", "", DataViewRowState.CurrentRows);
                dlstPrc.DataSource = MenuDV;
                dlstPrc.DataBind();

                dlstPrc1.Visible = true;
                DataView SubMenuDV = new DataView(dt, "NavigationURL is null and ParentMenuID<>0", "", DataViewRowState.CurrentRows);
                dlstPrc1.DataSource = SubMenuDV;
                dlstPrc1.DataBind();

                dlstPrc2.Visible = true;
                DataView ModuleDV = new DataView(dt, "NavigationURL is not null", "", DataViewRowState.CurrentRows);
                dlstPrc2.DataSource = ModuleDV;
                dlstPrc2.DataBind();
            }
            else
            {
                ucMsg.ShowInfo(Resources.Messages.NoRecord);
                dlstPrc.Visible = true;
                dlstPrc.DataSource = null;
                dlstPrc.DataBind();

                dlstPrc1.Visible = true;
                dlstPrc1.DataSource = null;
                dlstPrc1.DataBind();

                dlstPrc2.Visible = true;
                dlstPrc2.DataSource = null;
                dlstPrc2.DataBind();
            }

        }



        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ResetGrid();
        bindSearch();

    }

    void bindSearch()
    {
        try
        {
            ViewState["SearchMode"] = "true";
            dt = null;
            dt = MMOb.GetMenuInfo(txtKeyword.Text.Trim());
            dlstPrc.DataSource = null;
            if (dt.Rows.Count > 0)
            {
                dlstPrc.Visible = true;
                DataView MenuDV = new DataView(dt, "NavigationURL is null and ParentMenuID=0", "", DataViewRowState.CurrentRows);
                dlstPrc.DataSource = MenuDV;
                dlstPrc.DataBind();

                dlstPrc1.Visible = true;
                DataView SubMenuDV = new DataView(dt, "NavigationURL is null and ParentMenuID<>0", "", DataViewRowState.CurrentRows);
                dlstPrc1.DataSource = SubMenuDV;
                dlstPrc1.DataBind();

                dlstPrc2.Visible = true;
                DataView ModuleDV = new DataView(dt, "NavigationURL is not null", "", DataViewRowState.CurrentRows);
                dlstPrc2.DataSource = ModuleDV;
                dlstPrc2.DataBind();
            }
            else
            {
                ucMsg.ShowInfo(Resources.Messages.NoRecord);
                dlstPrc.Visible = true;
                dlstPrc.DataSource = null;
                dlstPrc.DataBind();

                dlstPrc1.Visible = true;
                dlstPrc1.DataSource = null;
                dlstPrc1.DataBind();

                dlstPrc2.Visible = true;
                dlstPrc2.DataSource = null;
                dlstPrc2.DataBind();
            }
        }



        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnViewAll_Click(object sender, EventArgs e)
    {
        ResetGrid();
        BindList();
       
    }
    #region Tab First
    protected void dlstPrc_CancelCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        dlstPrc.EditItemIndex = -1;
        BindList();
    }
    protected void dlstPrc_EditCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {

        ViewState["EditMode"] = "true";
        ImageButton ImgActive = (ImageButton)e.Item.FindControl("imgActive2");
        if (ImgActive.ImageUrl != null)
            ViewState["URLIMG"] = ImgActive.ImageUrl;
        dlstPrc.EditItemIndex = (int)e.Item.ItemIndex;
        if (ViewState["SearchMode"].ToString() != "true") BindList();
        else bindSearch();
    }
    protected void dlstPrc_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        // Int16 Result = 0;
        if (e.CommandName == "addMenu")
        {
            string MenuName = ((TextBox)e.Item.FindControl("txtAdd")).Text.Trim();
            string Description = ((TextBox)e.Item.FindControl("txtDesc2")).Text.Trim();
            int DisplayOrder = Convert.ToInt32(((TextBox)e.Item.FindControl("txtDO2")).Text.Trim());
            int ID = MMOb.InsertUpdateMenuInfo(0, MenuName, Description, 0, null, true, DisplayOrder, 1, false);
            if (ID > 0)
            {
                ucMsg.Visible = true;
                ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);

                BindList();
            }
            else if (ID == -1)
                ucMsg.ShowInfo(Resources.Messages.DuplicateMenu);
        }
        if (e.CommandName == "Active")
        {
            int Result;
            int MenuID = Convert.ToInt32(dlstPrc.DataKeys[e.Item.ItemIndex]);

            // ViewState["URLIMG"] = null;
            ImageButton ImgActive = (ImageButton)e.Item.FindControl("imgActive2");

            //if (ViewState["URLIMG"] != null)
            //    urlImg = ViewState["URLIMG"].ToString();
            //else
            //{
            if (ImgActive.ImageUrl != null)
                urlImg = ImgActive.ImageUrl;

            //}
            bool status = false;
            if (urlImg == AppAsset + "/CSS/images/decative.png")
            {
                status = true;
            }
            if (urlImg == AppAsset + "/CSS/images/active.png")
            {

                status = false;
            }
            //DataSet CheckStateDs = new DataSet();

            try
            {

                Result = MMOb.ToggleOtherStatus(MenuID, status, 7);


                if (Result != 0)
                {
                    //((Label)lblMessage.FindControl("lblMessage")).Visible = true;
                    ucMsg.Visible = true;
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);

                    if (status == true)
                    {
                        ImgActive.ImageUrl = PageBase.ImageChange(1);
                        ucMsg.ShowSuccess(Resources.Messages.Activate);

                    }
                    else
                    {
                        ImgActive.ImageUrl = PageBase.ImageChange(0);
                        ucMsg.ShowSuccess(Resources.Messages.Inactivate);

                    }
                    ViewState["EditMode"] = "false";
                }
                else
                {
                    ucMsg.Visible = true;
                    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                }

            }
            catch (Exception ex)
            {
                ucMsg.Visible = true;
                ucMsg.ShowWarning(ex.Message);

            }


        }
        if (e.CommandName == "Delete")
        {
            try
            {
                int MenuID = Convert.ToInt32(dlstPrc.DataKeys[e.Item.ItemIndex]);
                int result = MMOb.DeleteMenuInfo(MenuID, 1);
                BindList();
            }
            catch (Exception ex)
            {
                ucMsg.ShowWarning(ex.Message);
                ucMsg.Visible = true;
            }
        }

    }
    protected void dlstPrc_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {

        }
        if (e.Item.ItemType == ListItemType.EditItem)
        {

        }

    }
    protected void dlstPrc_UpdateCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        int Result;
        int MenuID = Convert.ToInt32(dlstPrc.DataKeys[e.Item.ItemIndex]);
        string MenuName = ((TextBox)e.Item.FindControl("txtMenuE")).Text.Trim();
        string Description = ((TextBox)e.Item.FindControl("txtDescE2")).Text.Trim();
        int DisplayOrder = Convert.ToInt32(((TextBox)e.Item.FindControl("txtDOE2")).Text.Trim());

        ImageButton ImgActive = (ImageButton)e.Item.FindControl("imgActive2");
        if (ViewState["URLIMG"] != null)
            urlImg = ViewState["URLIMG"].ToString();
        if (urlImg == null)
            urlImg = ImgActive.ImageUrl;
        bool status = false;
        if (urlImg == AppAsset + "/CSS/images/decative.png")
        {
            status = false;
        }
        if (urlImg == AppAsset + "/CSS/images/active.png")
        {
            status = true;
        }

        try
        {
            if (MenuName.Trim().Length == 0)
            {
                //lblMessage.Text = "Please enter new State name to update.";
                return;
            }

            if (MenuName.Length > 0 && MenuName.Length < 101)
            {
                
                    Result = MMOb.InsertUpdateMenuInfo(MenuID, MenuName, Description, 0, null, true, DisplayOrder, 1, false);
                    if (Result > 0)
                    {

                        ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                        if (status == true)
                            ImgActive.ImageUrl = ImageChange(1);
                        else
                            ImgActive.ImageUrl = ImageChange(0);
                        ViewState["EditMode"] = "false";
                    }
                    else if (Result == -1)
                        ucMsg.ShowInfo(Resources.Messages.DuplicateMenu);
            }
            else
            {
                ucMsg.Visible = true;
                ucMsg.ShowInfo("Length is more than allowed limit.");
            }

        }
        catch (Exception ex)
        {
           clsException.clsHandleException.fncHandleException(ex, "");
        }
        dlstPrc.EditItemIndex = -1;
        if (ViewState["SearchMode"] != "true")
            BindList();
        else
            bindSearch();

    }
    #endregion
    #region Tab Second
    protected void dlstPrc1_CancelCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        dlstPrc1.EditItemIndex = -1;
        BindList();
    }
    protected void dlstPrc1_DeleteCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        //try
        //{
        //    int CircleID = Convert.ToInt32(dlstCircle.DataKeys[e.Item.ItemIndex]);
        //    int Result = cmdOb.DeleteMasterRecordsByType(4, CircleID);

        //    if (Result > 0)
        //    {
        //        lblMessage.Visible = true; lblMessage.Text = "Records Deleted Successfully.";
        //    }
        //    dlstCircle.EditItemIndex = -1;
        //    BindList();
        //}
        //catch (Exception ex)
        //{
        //    clsException.clsHandleException.fncHandleException(ex, "");
        //}

    }
    protected void dlstPrc1_EditCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        EditParentName = ((Label)e.Item.FindControl("lblParentName")).Text.Trim();

        ViewState["EditMode1"] = "true";
        ImageButton ImgActive = (ImageButton)e.Item.FindControl("imgActive1");
        //if (ImgActive.ImageUrl != null)
        //    ViewState["URLIMG1"] = ImgActive.ImageUrl;
        dlstPrc1.EditItemIndex = (int)e.Item.ItemIndex;
        if (ViewState["SearchMode"].ToString() != "true") BindList();
        else bindSearch();
    }
    protected void dlstPrc1_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        // Int16 Result = 0;
        if (e.CommandName == "addParent")
        {

            string MenuName = ((TextBox)e.Item.FindControl("txtSubmenu")).Text.Trim();
            string Description = ((TextBox)e.Item.FindControl("txtDesc")).Text.Trim();
            int DisplayOrder = Convert.ToInt32(((TextBox)e.Item.FindControl("txtDO")).Text.Trim());
            int ParentID = Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlParentName")).SelectedValue);

            try
            {
                int ID = MMOb.InsertUpdateMenuInfo(0, MenuName, Description, ParentID, null, true, DisplayOrder, 2, false);
                if (ID > 0)
                {
                    ucMsg.Visible = true;
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);

                    BindList();
                }
                else if (ID == -1)
                    ucMsg.ShowInfo(Resources.Messages.DuplicateMenu);
            }
            catch (Exception ex)
            {
                ucMsg.Visible = true;
                ucMsg.ShowSuccess(Resources.Messages.Invalid);
            }

        }
        if (e.CommandName == "Delete")
        {
            try
            {
                int MenuID = Convert.ToInt32(dlstPrc1.DataKeys[e.Item.ItemIndex]);
                int result = MMOb.DeleteMenuInfo(MenuID, 1);
                BindList();
            }
            catch (Exception ex)
            {
                //lblMessage.Text = ex.Message;
                //lblMessage.Visible = true;
            }
        }

        if (e.CommandName == "Active")
        {
            int Result;
            int MenuID = Convert.ToInt32(dlstPrc1.DataKeys[e.Item.ItemIndex]);

            // ViewState["URLIMG"] = null;
            ImageButton ImgActive = (ImageButton)e.Item.FindControl("imgActive");

            //if (ViewState["URLIMG"] != null)
            //    urlImg = ViewState["URLIMG"].ToString();
            //else
            //{
            if (ImgActive.ImageUrl != null)
                urlImg = ImgActive.ImageUrl;

            //}
            bool status = false;
            if (urlImg == AppAsset + "/CSS/images/decative.png")
            {
                status = true;
            }
            if (urlImg == AppAsset + "/CSS/images/active.png")
            {
                status = false;
            }
       
            try
            {
                Result = MMOb.ToggleOtherStatus(MenuID, status, 7);
            
                    if (Result != 0)
                    {

                        ucMsg.Visible = true;

                        if (status == true)
                        {
                            ImgActive.ImageUrl =ImageChange(1);
                            ucMsg.ShowSuccess(Resources.Messages.Activate);

                        }
                        else
                        {
                            ImgActive.ImageUrl =ImageChange(0);
                            ucMsg.ShowSuccess(Resources.Messages.Inactivate);

                        }
                        ViewState["EditMode"] = "false";
                    }
                    else
                    {

                        ucMsg.Visible = true;
                        ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);

                    }
                }
 
            catch (Exception Ex)
            {
                ucMsg.Visible = true;
                ucMsg.ShowWarning(Ex.Message);
                PageBase.Errorhandling(Ex);

            }
        }
    }
    protected void dlstPrc1_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
    {
        try
        {
            DataTable dtEdit = new DataTable();
            dtEdit = MMOb.GetMenuInfo();
            if (e.Item.ItemType == ListItemType.Footer)
            {

                DropDownList ddlParentName = (DropDownList)e.Item.FindControl("ddlParentName");
                ddlParentName.DataTextField = "MenuName";
                ddlParentName.DataValueField = "MenuID";
                DataView MenuDV = new DataView(dtEdit, "NavigationURL is null and ParentMenuID=0", "", DataViewRowState.CurrentRows);
                ddlParentName.DataSource = MenuDV;
                ddlParentName.DataBind();

            }
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                DropDownList ddlParentName = (DropDownList)e.Item.FindControl("ddlParentNameE");
                ddlParentName.DataTextField = "MenuName";
                ddlParentName.DataValueField = "MenuID";
                DataView MenuDV = new DataView(dtEdit, "NavigationURL is null and ParentMenuID=0", "", DataViewRowState.CurrentRows);
                ddlParentName.DataSource = MenuDV;
                ddlParentName.DataBind();
                ddlParentName.Items.Insert(0, "Select");
                if (EditParentName.ToLower() != "no parent")
                    ddlParentName.SelectedValue = ddlParentName.Items.FindByText(EditParentName).Value;
            }
        }
        catch (Exception ex)
        {
            //lblMessage.Text = ex.Message.ToString();
        }
    }
    protected void dlstPrc1_UpdateCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        int MenuID = Convert.ToInt32(dlstPrc1.DataKeys[e.Item.ItemIndex]);
        string MenuName = ((TextBox)e.Item.FindControl("txtSubmenuE")).Text.Trim();
        string Description = ((TextBox)e.Item.FindControl("txtDescE")).Text.Trim();
        int DisplayOrder = Convert.ToInt32(((TextBox)e.Item.FindControl("txtDOE")).Text.Trim());
        int ParentID = Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlParentNameE")).SelectedValue);

        try
        {
            int ID = MMOb.InsertUpdateMenuInfo(MenuID, MenuName, Description, ParentID, null, true, DisplayOrder, 2, false);
            if (ID > 0)
            {
                ucMsg.Visible = true;
                ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                ViewState["SearchMode"] = "false";
                //BindList();
            }
            else if (ID == -1)
                ucMsg.ShowInfo(Resources.Messages.DuplicateMenu);
        }
        catch (Exception ex)
        {
            ucMsg.Visible = true;
            ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);

     
        }
        dlstPrc1.EditItemIndex = -1;
        if (ViewState["SearchMode"] != "true")
            BindList();
        else
            bindSearch();

    }
    #endregion

    #region Tab Third
    protected void dlstPrc2_CancelCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        dlstPrc2.EditItemIndex = -1;
        ViewState["SearchMode"] = "false";
        BindList();
    }
    protected void dlstPrc2_DeleteCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        //try
        //{
        //    int CircleID = Convert.ToInt32(dlstCircle.DataKeys[e.Item.ItemIndex]);
        //    int Result = cmdOb.DeleteMasterRecordsByType(4, CircleID);

        //    if (Result > 0)
        //    {
        //        lblMessage.Visible = true; lblMessage.Text = "Records Deleted Successfully.";
        //    }
        //    dlstCircle.EditItemIndex = -1;
        //    BindList();
        //}
        //catch (Exception ex)
        //{
        //    clsException.clsHandleException.fncHandleException(ex, "");
        //}

    }
    protected void dlstPrc2_EditCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        EditParentName = ((Label)e.Item.FindControl("lblParentName")).Text.Trim();
        AllowInMenu = ((Label)e.Item.FindControl("lblAllowInMenu")).Text.Trim();
        ViewState["EditMode2"] = "true";
              dlstPrc2.EditItemIndex = (int)e.Item.ItemIndex;
        if (ViewState["SearchMode"].ToString() != "true") BindList();
        else bindSearch();
    }
    protected void dlstPrc2_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
    {
        // Int16 Result = 0;
        if (e.CommandName == "addModule")
        {

            string MenuName = ((TextBox)e.Item.FindControl("txtModuleName")).Text.Trim();
            string Description = ((TextBox)e.Item.FindControl("txtDesc")).Text.Trim();
            int DisplayOrder = Convert.ToInt32(((TextBox)e.Item.FindControl("txtDO")).Text.Trim());
            int ParentID = Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlParentName")).SelectedValue);
            string NavigationURl = ((TextBox)e.Item.FindControl("txtNavigationURL")).Text.Trim();
            bool AllowInMenu = Convert.ToBoolean(((CheckBox)e.Item.FindControl("chkAllowInMenu")).Checked);

            try
            {
                int ID = MMOb.InsertUpdateMenuInfo(0, MenuName, Description, ParentID, NavigationURl, true, DisplayOrder, 3, AllowInMenu);
                if (ID > 0)
                {
                    ucMsg.Visible = true;
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);

                    BindList();
                }
                else if (ID == -1)
                    ucMsg.ShowInfo(Resources.Messages.DuplicateMenu);
            }
            catch (Exception ex)
            {
                ucMsg.Visible = true;
                ucMsg.ShowInfo(Resources.Messages.Invalid);

            }

        }

        if (e.CommandName == "Delete")
        {
            try
            {
                int MenuID = Convert.ToInt32(dlstPrc2.DataKeys[e.Item.ItemIndex]);
                int result = MMOb.DeleteMenuInfo(MenuID, 2);
                BindList();
            }
            catch (Exception ex)
            {
                ucMsg.Visible = true;
                ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
            
            }
        }
        if (e.CommandName == "Active")
        {
            int Result;
            int MenuID = Convert.ToInt32(dlstPrc2.DataKeys[e.Item.ItemIndex]);

                ImageButton ImgActive = (ImageButton)e.Item.FindControl("imgActive");

    
            if (ImgActive.ImageUrl != null)
                urlImg = ImgActive.ImageUrl;
            bool status = false;
            if (urlImg ==  AppAsset + "/CSS/images/decative.png")
            {
                status = true;
            }
            if (urlImg == AppAsset + "/CSS/images/active.png")
            {
                status = false;
            }
      
            try
            {

              Result = MMOb.ToggleOtherStatus(MenuID, status, 7);
         
                if (Result != 0)
                {
         
                    ucMsg.Visible = true;

                    if (status == true)
                    {
                        ImgActive.ImageUrl =ImageSwap(1);
                        ucMsg.ShowSuccess(Resources.Messages.Activate);
                       
                    }
                    else
                    {
                        ImgActive.ImageUrl =ImageSwap(0);
                        ucMsg.ShowSuccess(Resources.Messages.Inactivate);
                 
                    }
                    ViewState["EditMode"] = "false";
                }
                else
                {
                    ucMsg.Visible = true;
                    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);

                }

        
            }
            catch (Exception ex)
            {
                ucMsg.Visible = true;
                ucMsg.ShowWarning(ex.Message);
                PageBase.Errorhandling(ex);
            }

        }
    }
    protected void dlstPrc2_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
    {
        try
        {
            DataTable dtEdit1 = new DataTable();
            dtEdit1 = MMOb.GetMenuInfo();
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlMenuName = (DropDownList)e.Item.FindControl("ddlParentName");
                ddlMenuName.DataTextField = "MenuName";
                ddlMenuName.DataValueField = "MenuID";
                DataView MenuDV = new DataView(dtEdit1, "NavigationURL is null", "", DataViewRowState.CurrentRows);
                ddlMenuName.DataSource = MenuDV;
                ddlMenuName.DataBind();
                ddlMenuName.Items.Insert(0, "Select");
            }
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                DropDownList ddlMenuName = (DropDownList)e.Item.FindControl("ddlParentNameE");
                ddlMenuName.DataTextField = "MenuName";
                ddlMenuName.DataValueField = "MenuID";
                DataView MenuDV = new DataView(dtEdit1, "NavigationURL is null", "", DataViewRowState.CurrentRows);
                ddlMenuName.DataSource = MenuDV;
                ddlMenuName.DataBind();
                ddlMenuName.Items.Insert(0, "Select");
                if (EditParentName.ToLower() != "no parent")
                    ddlMenuName.SelectedValue = ddlMenuName.Items.FindByText(EditParentName).Value;
                CheckBox chk = (CheckBox)e.Item.FindControl("chkAllowInMenuE");
                if (AllowInMenu.ToLower() == "yes")
                {
                    chk.Checked = true;
                }
                else
                {
                    chk.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            //lblMessage.Text = ex.Message.ToString();
        }
    }

    protected void ddlParentName_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        DataListItem Item = (DataListItem)((DropDownList)sender).Parent;
        Int16 ParentID = Convert.ToInt16(((DropDownList)Item.FindControl("ddlParentName")).SelectedValue);
        Int32 DisOrder = new MenuData().GetNextDisplayOrder(ParentID);
        TextBox txtDisplayOrder = (TextBox)Item.FindControl("txtDO");
        txtDisplayOrder.Text = Convert.ToString(DisOrder);
        txtDisplayOrder.Enabled = false;

    }
    protected void ddlParentNameE_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataListItem Item = (DataListItem)((DropDownList)sender).Parent;
        Int16 ParentID = Convert.ToInt16(((DropDownList)Item.FindControl("ddlParentNameE")).SelectedValue);
        Int32 DisOrder = new MenuData().GetNextDisplayOrder(ParentID);
        TextBox txtDisplayOrder = (TextBox)Item.FindControl("txtDOE");
        txtDisplayOrder.Text = Convert.ToString(DisOrder);
        txtDisplayOrder.Enabled = false;
 
    }
    protected void dlstPrc2_UpdateCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e) 
    {
        int MenuID = Convert.ToInt32(dlstPrc2.DataKeys[e.Item.ItemIndex]);
        string MenuName = ((TextBox)e.Item.FindControl("txtModuleNameE")).Text.Trim();
        string Description = ((TextBox)e.Item.FindControl("txtDescE")).Text.Trim();
        int DisplayOrder = Convert.ToInt32(((TextBox)e.Item.FindControl("txtDOE")).Text.Trim());
        int ParentID = Convert.ToInt32(((DropDownList)e.Item.FindControl("ddlParentNameE")).SelectedValue);
        string NavigationURl = ((TextBox)e.Item.FindControl("txtNavigationURLE")).Text.Trim();
        bool AllowinMenu = ((CheckBox)e.Item.FindControl("chkAllowInMenuE")).Checked;

        try
        {
            int ID = MMOb.InsertUpdateMenuInfo(MenuID, MenuName, Description, ParentID, NavigationURl, true, DisplayOrder, 3, AllowinMenu);
            if (ID > 0)
            {
                ucMsg.Visible = true;
                ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                //BindList();
                ViewState["SearchMode"] = "false";
            }
            else if (ID == -1)
                ucMsg.ShowInfo(Resources.Messages.DuplicateMenu);
        }
        catch (Exception ex)
        {

            //lblMessage.Visible = true;
            //lblMessage.Text = ex.Message.ToString();
        }

        dlstPrc2.EditItemIndex = -1;
        if (ViewState["SearchMode"] != "true")
            BindList();
        else
            bindSearch();

    }

    #endregion


    protected void TabMenu_ActiveTabChanged1(object sender, EventArgs e)
    {
        ResetGrid();
        if (ViewState["SearchMode"] != "true")
            BindList();
        else
            bindSearch();
    }

    private void ResetGrid()
    {
        ViewState["SearchMode"] = "False";
        dlstPrc.EditItemIndex = -1;
        dlstPrc1.EditItemIndex = -1;
        dlstPrc2.EditItemIndex = -1;
      
    }
}
