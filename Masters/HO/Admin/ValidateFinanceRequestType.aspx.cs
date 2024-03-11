using BussinessLogic;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
/*=============================================      
-- Author:  <Vijay Kumar Prajapati>      
-- Create date: <13-July-2017>      
-- Description: <Used to save and update API User Request Type Mapping> 
 * Change Log:DD-MM-YYYY,DeveloperName,#CC0X,Description
-- ============================================= */
public partial class Masters_HO_Admin_ValidateFinanceRequestType : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            ucMsg.Visible = false;
            if (!IsPostBack)
            {
               
                if (ViewState["EditUserId"] != null)
                { ViewState.Remove("EditUserId"); }
                DisableButton();
                BindApiUserRequest();
                BindUserListGrid();
              
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnCreateUser_Click(object sender, EventArgs e)
    {
        int Result = 0;
       
        try
        {
            using (UserData objuser = new UserData())
            {
               
                objuser.APIUserId = Convert.ToInt32(ViewState["EditUserId"]);
                objuser.UserID = PageBase.UserId;

                foreach (ListItem item in ChkListAPIUserRequestTypeMapping.Items)
                {
                    if (item.Text == "Can Mark As Sold" && item.Selected==true)
                    {
                        objuser.BITRequestTypeID1 = 1;
                    }
                    else if (item.Text == "Can Mark As Sold" && item.Selected == false)
                    {
                         objuser.BITRequestTypeID1 = 0;
                    }
                    else if(item.Text == "Can Mark As Un-Sold" && item.Selected==true)
                    {
                        objuser.BITRequestTypeID2 = 1;
                    }
                    else if (item.Text == "Can Mark As Un-Sold" && item.Selected == false)
                    {
                        objuser.BITRequestTypeID2 = 0;
                    }
                }
                Result = objuser.InsertUpdateAPIUserRequestTypeMapping();
                if (Result > 0)
                {
                    ucMsg.ShowSuccess(Resources.Messages.CreateSuccessfull);
                    DisableButton();
                    
                }
                else
                {
                    ucMsg.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);
                    return;
                }
            }
            BindUserListGrid();
            updAddUserMain.Update();
        }
        catch (Exception ex)
        {
            
        }
       
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        DisableButton();
        BindUserListGrid();
    }
    private void BindApiUserRequest()
    {        
        try
        {
            DataTable dt = new DataTable();
            dt = GetEnumByTableName("ApiUserRequest", "ApiUserEnumType");
            if (dt.Rows.Count > 0)
            {
                ChkListAPIUserRequestTypeMapping.DataSource = dt;
                ChkListAPIUserRequestTypeMapping.DataTextField = "ApiUserType";
                ChkListAPIUserRequestTypeMapping.DataValueField = "ID";
                ChkListAPIUserRequestTypeMapping.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btnEdit = (ImageButton)sender;
        DataTable dtValidateUser=new DataTable();
        ViewState["EditUserId"] = Convert.ToInt32(btnEdit.CommandArgument);
        try
        {
            foreach (ListItem item in ChkListAPIUserRequestTypeMapping.Items)
            {
                item.Selected = false;
            }
            using (UserData objuser = new UserData())
            {
                 
                dtValidateUser = objuser.prcGetValidateByUserId(Convert.ToInt32(ViewState["EditUserId"]));
                if (dtValidateUser.Rows.Count > 0)
                {

                    for (int i = 0; i < dtValidateUser.Rows.Count; i++)
                    {
                        ChkListAPIUserRequestTypeMapping.Items.FindByValue(dtValidateUser.Rows[i]["RequestTypeId"].ToString()).Selected = true;
                    }
                    ChkListTrue();               
                }
                else { BindApiUserRequest(); ChkListTrue(); }              
                updAddUserMain.Update();
            };
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }    
    }
    protected void grdvwUserList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvwUserList.PageIndex = e.NewPageIndex;
        BindUserListGrid();
    }
    public static DataTable GetEnumByTableName(string FileName, string TableName)
    {
        System.Data.DataTable dt = new DataTable();
        using (DataSet theDataSet = new DataSet())
        {
            string strPath = HttpContext.Current.Server.MapPath("~/Assets/XML/" + FileName + ".xml");
            theDataSet.ReadXml(strPath);
            dt = theDataSet.Tables[TableName];
            if (dt == null || dt.Rows.Count == 0)
                return null;
        }
        try
        {
            dt = dt.Select("Active = 1").CopyToDataTable();
            return dt;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    private void BindUserListGrid()
    {       
        try
        {
            DataTable dt = new DataTable();
            using (UserData objuser = new UserData())
            {
                dt = objuser.GetUserInfoForValidateFinanceRequestType();
                if (dt.Rows.Count > 0)
                {
                    grdvwUserList.DataSource = dt;
                    grdvwUserList.DataBind();
                }
                else
                {
                    grdvwUserList.DataSource = null;
                    grdvwUserList.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }   
    }
    private void ChkListTrue()
    {
        ChkListAPIUserRequestTypeMapping.Enabled = true;
        btnCreateUser.Enabled = true;
        btnCancel.Enabled = true;
    }
    private void DisableButton()
    {
        ChkListAPIUserRequestTypeMapping.Enabled = false;
        btnCreateUser.Enabled = false;
        btnCancel.Enabled = false;
        foreach (ListItem item in ChkListAPIUserRequestTypeMapping.Items)
        {
            if (item.Selected)
                item.Selected = false;
        }
    }
}