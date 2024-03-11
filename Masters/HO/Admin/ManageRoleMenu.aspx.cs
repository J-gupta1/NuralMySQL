using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DataAccess;
using BussinessLogic;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

public partial class Masters_HO_Admin_ManageRoleMenu :PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMsg.ShowControl = false;
            if (!IsPostBack)
            {
                BindRoles();
                fnFillUserModuleMapping("");
                
            }
           
          }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }

    }
    private void BindRoles()
    {
        try
        {
            DataTable dt = new DataTable();


            ddlRole.Items.Clear();
            using (UserData objuser = new UserData())
            {
                objuser.SearchType = 4;
                objuser.Status = true;
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
    

    private void fnFillUserModuleMapping(string strSearchCriteria)
    {
        try
        {
            string qry = "select RoleName,MenuName,RoleID from vwRoleTypeMenuMapping";

            if (strSearchCriteria != "")
            {
                qry += " where RoleID = " +  ddlRole.SelectedValue + " ";
                ViewState["TypeModuleMappingQuery"] = "Search";
            }
            else
            {
                ViewState["TypeModuleMappingQuery"] = "";
            }

            qry += " group by RoleName,MenuName,RoleID order by RoleName ";

            using (DataSet ds = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.Text, qry))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvMapping.DataSource = ds;
                    gvMapping.DataBind();
                }
                else
                {
                    ShowNoResultFound(ds, gvMapping);
                    if (strSearchCriteria == "Search")
                    {
                        //lblTypeModuleMappingMessage.Text = "No record(s) found..";

                    }
                    //fnMappingDataTable();
                }
            }
        }
        catch (Exception ex)
        {
            //ExceptionPolicy.HandleException(ex, Pagebase.ExceptionPolicyName);
        }
    }

    private void ShowNoResultFound(DataSet source, GridView gv)
    {
        source.Tables[0].Rows.Add(source.Tables[0].NewRow()); // create a new blank row to the DataTable
        gvMapping.DataSource = source;
        gvMapping.DataBind();
        int columnsCount = gv.Columns.Count;
        gvMapping.Rows[0].Cells.Clear();// clear all the cells in the row
        gvMapping.Rows[0].Cells.Add(new TableCell()); //add a new blank cell
        gvMapping.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell
        gvMapping.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gvMapping.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gvMapping.Rows[0].Cells[0].Font.Bold = true;
        gvMapping.Rows[0].Cells[0].Text = "NO RESULT FOUND!";

    }
    #region "User Defined Function"

    private void fnMappingDataTable()
    {
        try
        {
            DataTable dtMappingItem = new DataTable();

            dtMappingItem.Columns.Add("RoleID", typeof(int));
            dtMappingItem.Columns.Add("RoleName", typeof(string));
            dtMappingItem.Columns.Add("MenuName", typeof(string));


            ViewState["dtMappingItem"] = dtMappingItem;
            gvMapping.DataSource = ViewState["dtMappingItem"];
            gvMapping.DataBind();
            //PageBase.EmptyGridFix(gvMapping);
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }

    public bool fnAddUpdateMapping(Int32 TypeId, string strModuleIds, Int16 EditFlag)
    {
        try
        {
            SqlParameter[] prm = new SqlParameter[4];
            prm[0] = new SqlParameter("@RoleId", TypeId);
            prm[1] = new SqlParameter("@ModuleId", strModuleIds);
            prm[2] = new SqlParameter("@EditFlag", EditFlag);
            prm[3] = new SqlParameter("@result", SqlDbType.TinyInt, 5);
            prm[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(PageBase.ConStr, CommandType.StoredProcedure, "prcInsUpdUserMenuMapping", prm);
            if (Convert.ToInt16(prm[3].Value) == 1)
            {
                if (EditFlag == 0)
                {
                    
                    ucMsg.ShowSuccess("Type module mapping added successfully.");
                }
                else
                {
                    ucMsg.ShowSuccess("Type module mapping updated successfully.");
                  
                }
                
                //using (MenuData ObjMenu = new MenuData())
                //{
                //    DataSet ds;
                //    ds = ObjMenu.getMenuByRoleID(TypeId);
                //    //if (System.IO.File.Exists(Server.MapPath("..\\..\\..\\App_Data\\" + ds.Tables[0].Rows[0]["RoleName"].ToString().Replace(" ", "") + ".xml")))
                //    //    System.IO.File.Delete(Server.MapPath("..\\..\\..\\App_Data\\" + ds.Tables[0].Rows[0]["RoleName"].ToString().Replace(" ", "") + ".xml"));
                //    ds.WriteXml(Server.MapPath("..\\..\\..\\App_Data\\" + ds.Tables[0].Rows[0]["RoleName"].ToString().Replace(" ", "") + ".xml"));
                //}
                ucMsg.Visible = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            //ExceptionPolicy.HandleException(ex, Pagebase.ExceptionPolicyName);
            return false;
        }
    }

    #endregion

    #region "Grid Events and Selected index changed"

    protected void gvMapping_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlUserRole = (DropDownList)e.Row.FindControl("ddlUserType");

                string qry = "SELECT RoleID ,RoleName FROM Role WHERE  Status =1 ORDER BY RoleID ASC";
                using (DataSet ds = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.Text, qry))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlUserRole.DataTextField = "RoleName";
                        ddlUserRole.DataValueField = "RoleID";
                        ddlUserRole.DataSource = ds;
                        ddlUserRole.DataBind();
                        ddlUserRole.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {
                        ddlUserRole.Items.Insert(0, new ListItem("Select", "0"));
                    }
                }
                DataList dlFooterMenu = (DataList)e.Row.FindControl("dlFooterMenu");
                string strMenuQuery = "select menuID,MenuName from Menu WHERE   NavigationURL is null order BY DisplayOrderNumber";
                using (DataSet ds = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.Text, strMenuQuery))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dlFooterMenu.DataSource = ds;
                        dlFooterMenu.DataBind();
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
            {
                Label lblUsertypeId = (Label)e.Row.FindControl("lblUsertypeId");
                ViewState["UsertypeId"] = lblUsertypeId.Text.Trim();
                DataList dlMenu = (DataList)e.Row.FindControl("dlMenu");
                //   string strMenuQuery = "select distinct main_menu_id,tbl_mainmenu_master.menu_name from tbl_module_master inner join tbl_mainmenu_master on tbl_module_master.main_menu_id = tbl_mainmenu_master.menu_id  order by menu_name ";
                //string strMenuQuery = "select  menuID,MenuName from Menu where parentid is null order by displayorder";
                string strMenuQuery = "select menuID,MenuName from Menu WHERE   NavigationURL is null order BY DisplayOrderNumber";
                using (DataSet ds = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.Text, strMenuQuery))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dlMenu.DataSource = ds;
                        dlMenu.DataBind();
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    e.Row.BackColor = System.Drawing.Color.FromName("#dddddd");
                    DropDownList ddlEditUserRole = (DropDownList)e.Row.FindControl("ddlEditUserType");
                    Label lblUsertypeId = (Label)e.Row.FindControl("lblUsertypeId");
                    ViewState["EditUsertypeId"] = lblUsertypeId.Text.Trim();

                    string qry = "SELECT RoleID ,RoleName FROM Role WHERE  Status =1 ORDER BY RoleID ASC";
                    using (DataSet dsUserType = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.Text, qry))
                        if (dsUserType.Tables[0].Rows.Count > 0)
                        {
                            ddlEditUserRole.DataTextField = "RoleName";
                            ddlEditUserRole.DataValueField = "RoleID";
                            ddlEditUserRole.DataSource = dsUserType;
                            ddlEditUserRole.DataBind();
                            ddlEditUserRole.Items.Insert(0, new ListItem("Select", "0"));
                            ddlEditUserRole.SelectedValue = lblUsertypeId.Text;
                        }
                        else
                        {
                            ddlEditUserRole.Items.Insert(0, new ListItem("Select", "0"));
                        }

                    DataList dlEditMenu = (DataList)e.Row.FindControl("dlEditMenu");
                    string strMenuQuery = "select menuID,MenuName from Menu WHERE   NavigationURL is null order BY DisplayOrderNumber";
                    using (DataSet ds = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.Text, strMenuQuery))
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            dlEditMenu.DataSource = ds;
                            dlEditMenu.DataBind();
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }

    protected void gvMapping_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Button btnAdd = (Button)e.Row.FindControl("btnAdd");
                btnAdd.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }

    protected void gvMapping_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            //lblTypeModuleMappingMessage.Text = "";
            int index;
            if (e.CommandName == "btnAdd")
            {
                string strModuleIds = "";
                Int16 EditFlag = 0;// for new add
                index = Convert.ToInt32(e.CommandArgument);
                DropDownList ddlUserType = ((DropDownList)gvMapping.FooterRow.FindControl("ddlUserType"));

                //*********check validation***********
                if (ddlUserType.SelectedValue == "0")
                {
                    ucMsg.ShowInfo("Please select user type");
                    
                    return;
                }

                DataList dlMenu = ((DataList)gvMapping.FooterRow.FindControl("dlFooterMenu"));

                for (int i = 0; i < dlMenu.Items.Count; i++)
                {
                    CheckBoxList chkModule = (CheckBoxList)dlMenu.Items[i].FindControl("chkFooterModules");
                    for (int j = 0; j < chkModule.Items.Count; j++)
                    {
                        if (chkModule.Items[j].Selected == true)
                        {
                            strModuleIds += "," + chkModule.Items[j].Value.ToString();
                        }
                    }
                }
                if (strModuleIds == "")
                {
                   ucMsg.ShowInfo("Please select module.");
                   ucMsg.Visible = true;

                    
                    return;
                }

                fnAddUpdateMapping(Convert.ToInt32(ddlUserType.SelectedValue), strModuleIds, EditFlag);

                gvMapping.EditIndex = -1;
                fnFillUserModuleMapping(ViewState["TypeModuleMappingQuery"].ToString());

            }
        }

        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }

    protected void gvMapping_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
           
            gvMapping.EditIndex = e.NewEditIndex;
            fnFillUserModuleMapping(ViewState["TypeModuleMappingQuery"].ToString());
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }

    protected void gvMapping_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
           
            gvMapping.EditIndex = -1;
            fnFillUserModuleMapping(ViewState["TypeModuleMappingQuery"].ToString());
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }
    protected void gvMapping_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
          
            Int16 EditFlag = 1;

            GridViewRow row = gvMapping.Rows[e.RowIndex];
            string strModuleIds = "";

            string lblUsertypeId = ((Label)(row.Cells[0].FindControl("lblUsertypeId"))).Text;
            string ddlEditUserType = ((DropDownList)(row.Cells[1].FindControl("ddlEditUserType"))).SelectedItem.Text;

            //***********validation code***************************************************************************
            if (ddlEditUserType == "Select")
            {
                ucMsg.ShowInfo("Please select  Role.");
                ucMsg.Visible = true;
                
                return;
            }
            DataList dlMenu = ((DataList)(row.Cells[4].FindControl("dlEditMenu")));
            for (int i = 0; i < dlMenu.Items.Count; i++)
            {
                CheckBoxList chkModule = (CheckBoxList)dlMenu.Items[i].FindControl("chkEditModules");
                for (int j = 0; j < chkModule.Items.Count; j++)
                {
                    if (chkModule.Items[j].Selected == true)
                    {
                        strModuleIds += "," + chkModule.Items[j].Value.ToString();
                    }
                }
            }
            if (strModuleIds == "")
            {
                ucMsg.ShowInfo("Please select module.");
                return;
            }

            fnAddUpdateMapping(Convert.ToInt32(lblUsertypeId), strModuleIds, EditFlag);
            gvMapping.EditIndex = -1;
            fnFillUserModuleMapping(ViewState["TypeModuleMappingQuery"].ToString());

        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }

    protected void gvMapping_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
           
            gvMapping.PageIndex = e.NewPageIndex;
            gvMapping.EditIndex = -1;
            fnFillUserModuleMapping(ViewState["TypeModuleMappingQuery"].ToString());
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }
    #endregion

    #region "Data List Events"

    protected void dlMenu_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label lblMenuId = (Label)e.Item.FindControl("lblMenuId");
        int intMainMenuId = Convert.ToInt32((((DataList)sender).DataKeys[e.Item.ItemIndex].ToString()));

        CheckBoxList chkModule = (CheckBoxList)e.Item.FindControl("chkModules");
        chkModule.Items.Clear();
        string strModuleQuery = "select MenuID,MenuName  from menu where ParentMenuID=" + intMainMenuId + " order by DisplayOrderNumber ";
        using (DataSet ds = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.Text, strModuleQuery))
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkModule.DataTextField = "MenuName";
                chkModule.DataValueField = "MenuID";

                chkModule.DataSource = ds;
                chkModule.DataBind();
                chkModule.Enabled = false;
            }
        }
        // check previous items
        if (ViewState["UsertypeId"].ToString().Trim() != "")
        {

            string sql = "select Menu.MenuID from Menu inner join RoleMenuMapping on RoleMenuMapping.menuID=Menu.MenuID where RoleID=" + ViewState["UsertypeId"].ToString() + " and Menu.ParentMenuID=" + intMainMenuId + "";

            using (DataSet ds = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.Text, sql))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < chkModule.Items.Count; j++)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (chkModule.Items[j].Value.Trim() == ds.Tables[0].Rows[i]["MenuID"].ToString().Trim())
                            {
                                chkModule.Items[j].Selected = true;
                            }
                        }
                    }//end of outer for loop
                }//end of table row count
            }//end of using
        }
    }

    protected void dlEditMenu_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label lblEditMenuId = (Label)e.Item.FindControl("lblEditMenuId");
        int intEditMenuId = Convert.ToInt32((((DataList)sender).DataKeys[e.Item.ItemIndex].ToString()));

        CheckBoxList chkModule = (CheckBoxList)e.Item.FindControl("chkEditModules");
        chkModule.Items.Clear();
        string strModuleQuery = "select MenuID,MenuName  from Menu where ParentMenuID=" + intEditMenuId + " order by DisplayOrderNumber ";
        using (DataSet ds = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.Text, strModuleQuery))
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkModule.DataTextField = "MenuName";
                chkModule.DataValueField = "MenuID";

                chkModule.DataSource = ds;
                chkModule.DataBind();
            }
        }

        // check previous items
        string sql = "select menu.MenuID from Menu inner join RoleMenuMapping on RoleMenuMapping.menuID=Menu.MenuID where RoleID=" + ViewState["EditUsertypeId"].ToString() + " and menu.ParentMenuID=" + intEditMenuId + "";
        using (DataSet ds = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.Text, sql))
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < chkModule.Items.Count; j++)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (chkModule.Items[j].Value.Trim() == ds.Tables[0].Rows[i]["MenuID"].ToString().Trim())
                        {
                            chkModule.Items[j].Selected = true;
                        }
                    }
                }//end of outer for loop
            }//end of table row count
        }//end of using
    }

    protected void dlFooterMenu_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label lblFooterMenuId = (Label)e.Item.FindControl("lblFooterMenuId");
        int intFooterMenuId = Convert.ToInt32((((DataList)sender).DataKeys[e.Item.ItemIndex].ToString()));

        CheckBoxList chkModule = (CheckBoxList)e.Item.FindControl("chkFooterModules");
        chkModule.Items.Clear();
        string strModuleQuery = "select MenuID,MenuName  from Menu where ParentMenuID=" + intFooterMenuId + " order by DisplayOrderNumber ";
        using (DataSet ds = SqlHelper.ExecuteDataset(PageBase.ConStr, CommandType.Text, strModuleQuery))
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkModule.DataTextField = "MenuName";
                chkModule.DataValueField = "MenuID";

                chkModule.DataSource = ds;
                chkModule.DataBind();
            }
        }
    }
    #endregion

    protected void btnModuleSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string Search = string.Empty;
            if (ddlRole.SelectedValue == "")
            {
                ucMsg.ShowInfo("Please select User Type.");
          ;
                return;
            }
            else
            {
                ucMsg.Visible = false;
                Search = "Search";
            }
            ucMsg.Visible = true;
            gvMapping.EditIndex = -1;
            gvMapping.PageIndex = 0;
            fnFillUserModuleMapping(Search);

        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnModuleShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            ddlRole.SelectedValue = "0";
            ucMsg.Visible = false;
            string search = string.Empty;
            gvMapping.EditIndex = -1;
            gvMapping.PageIndex = 0;
            fnFillUserModuleMapping(search);
        }
        catch (Exception ex)
        {
            PageBase.Errorhandling(ex);
        }

    }
}
