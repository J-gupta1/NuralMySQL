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
* Created On: 01-June-2020
 * Description: This is a Category Master Interface.
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

public partial class Masters_Common_CategoryMaster : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindParentCategoryName();
            BindCategoryFor();
            BindList(1);
        }
    }
    protected void btnSubmit_Click(object sender, System.EventArgs e)
    {
        using (clsEntityMappingTypeRelationMaster objSave = new clsEntityMappingTypeRelationMaster())
        {
            if (ddlParentCategoryName.SelectedValue == "10000")
            {
                if(txtParentCategory.Text.Trim()=="")
                {
                    ucMsg.Visible = true;
                    ucMsg.ShowInfo("Please Enter Parent Category");
                    dvMsg.Style.Add("display", "block");
                    updMessage.Update();
                    return;
                }
            }
            objSave.ParentCategoryId = Convert.ToInt32(ddlParentCategoryName.SelectedValue);
            objSave.CategoryForId = Convert.ToInt16(ddlCategoryFor.SelectedValue);
            objSave.ParentCategoryName = txtParentCategory.Text.Trim();
            objSave.CategoryName = txtCategoryName.Text.Trim();
            objSave.UserID = UserId;
            objSave.CompanyId = PageBase.ClientId;
            objSave.Active = Convert.ToInt16(chkActive.Checked);
            objSave.Condition = 0;
            if (btnSubmit.Text == "Update")
            {
                objSave.CategoryId = Convert.ToInt32(ViewState["CategoryMasterId"]);
                objSave.Condition = 1;
            }  
            objSave.SaveCategoryMaster();
            if (objSave.Out_Param == 0)
            {
                if (btnSubmit.Text == "Submit")
                    ucMsg.ShowSuccess(Resources.SuccessMessages.SaveSuccess);
                else
                    ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                BindList(Convert.ToInt32(ViewState["PageIndex"]));
                BindParentCategoryName();
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
    protected void btnShowAll_Click(object sender, System.EventArgs e)
    {
        try
        {
            ddlCategoryForSearch.SelectedValue = "255";
            ddlParentCategoryNameSearch.SelectedIndex = 0;
            txtCategoryNameSearch.Text = "";
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
    protected void btnSearch_Click(object sender, System.EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlCategoryForSearch.SelectedValue) == 255 && Convert.ToInt16(ddlParentCategoryNameSearch.SelectedValue) == 0 && txtCategoryNameSearch.Text.Trim() == "")
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
    protected void grdvList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "cmdEdit")
            {
                ucMsg.Visible = false;
                using (clsEntityMappingTypeRelationMaster objGet = new clsEntityMappingTypeRelationMaster())
                {
                    objGet.PageSize = Convert.ToInt32(PageBase.PageSize);
                    objGet.PageIndex = 1;
                    objGet.CompanyId = PageBase.ClientId;
                    objGet.CategoryId = Convert.ToInt32(e.CommandArgument);
                    ViewState["CategoryMasterId"] = Convert.ToInt32(e.CommandArgument);
                    DataTable dt = objGet.SelectCategoryMasterBYID();
                   ListItem Other= ddlParentCategoryName.Items.FindByValue("10000");
                   ddlParentCategoryName.Items.Remove(Other);
                    ddlParentCategoryName.SelectedIndex = ddlParentCategoryName.Items.IndexOf(ddlParentCategoryName.Items.FindByValue(dt.Rows[0]["ParentCategoryID"].ToString()));
                    ddlCategoryFor.SelectedIndex = ddlCategoryFor.Items.IndexOf(ddlCategoryFor.Items.FindByValue(dt.Rows[0]["CategoryFor"].ToString()));
                    txtCategoryName.Text = dt.Rows[0]["CategoryName"].ToString();
                    chkActive.Checked = dt.Rows[0]["Active"].ToString() == "1" ? true : false;
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
                    objActive.CategoryId = Convert.ToInt32(e.CommandArgument);
                    objActive.Condition = 2;
                    objActive.CompanyId = PageBase.ClientId;
                    objActive.SaveCategoryMaster();
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
    protected void Exporttoexcel_Click(object sender, System.EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            using (clsEntityMappingTypeRelationMaster objGet = new clsEntityMappingTypeRelationMaster())
            {
                objGet.PageSize = Convert.ToInt32(ViewState["TotalRecords"]);
                objGet.PageIndex = -1;
                objGet.CategoryName = txtCategoryNameSearch.Text.Trim();
                objGet.ParentCategoryId = Convert.ToInt32(ddlParentCategoryNameSearch.SelectedValue);
                objGet.CategoryForId = Convert.ToInt32(ddlCategoryForSearch.SelectedValue);
                objGet.CompanyId = PageBase.ClientId;
                dt = objGet.GetCategoryMasterData();
            }
            DataSet ds = new DataSet();
            ds.Merge(dt);
            string FilenameToexport = "CategoryMasterDetails";
            PageBase.ExportToExecl(ds, FilenameToexport);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
            dvMsg.Style.Add("display", "block");

        }
    }
    private void BindParentCategoryName()
    {
        try
        {
          
            DataTable dtParentCategoryName = new DataTable();
            using (clsEntityMappingTypeRelationMaster objget = new clsEntityMappingTypeRelationMaster())
            {
                objget.CompanyId = PageBase.ClientId;
                objget.UserID = PageBase.UserId;
                dtParentCategoryName = objget.getParentCategoryNameDropdowns();
                if (dtParentCategoryName.Rows.Count > 0)
                {
                    ddlParentCategoryName.DataSource = dtParentCategoryName;
                    ddlParentCategoryName.DataTextField = "ParentCategoryName";
                    ddlParentCategoryName.DataValueField = "ParentCategoryID";
                    ddlParentCategoryName.DataBind();
                    ddlParentCategoryName.Items.Insert(0, new ListItem("Other", "10000"));
                    ddlParentCategoryName.Items.Insert(0, new ListItem("Select", "0"));
                   

                    ddlParentCategoryNameSearch.DataSource = dtParentCategoryName;
                    ddlParentCategoryNameSearch.DataTextField = "ParentCategoryName";
                    ddlParentCategoryNameSearch.DataValueField = "ParentCategoryID";
                    ddlParentCategoryNameSearch.DataBind();
                    ddlParentCategoryNameSearch.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    ddlParentCategoryName.Items.Insert(0, new ListItem("Select", "0"));
                    ddlParentCategoryNameSearch.Items.Insert(0, new ListItem("Select", "0"));
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

    private void BindCategoryFor()
    {
        try
        {

            DataTable dtParentCategoryFor = new DataTable();
            using (clsEntityMappingTypeRelationMaster objget = new clsEntityMappingTypeRelationMaster())
            {
                objget.CompanyId = PageBase.ClientId;
                objget.UserID = PageBase.UserId;
                dtParentCategoryFor = objget.getCategoryForBindDropdowns();
                if (dtParentCategoryFor.Rows.Count > 0)
                {
                    ddlCategoryFor.DataSource = dtParentCategoryFor;
                    ddlCategoryFor.DataTextField = "CategoryForName";
                    ddlCategoryFor.DataValueField = "CategoryForId";
                    ddlCategoryFor.DataBind();
                    ddlCategoryFor.Items.Insert(0, new ListItem("Select", "255"));

                    ddlCategoryForSearch.DataSource = dtParentCategoryFor;
                    ddlCategoryForSearch.DataTextField = "CategoryForName";
                    ddlCategoryForSearch.DataValueField = "CategoryForId";
                    ddlCategoryForSearch.DataBind();
                    ddlCategoryForSearch.Items.Insert(0, new ListItem("Select", "255"));
                  
                }
                else
                {
                    ddlCategoryFor.Items.Insert(0, new ListItem("Select", "255"));
                    ddlCategoryForSearch.Items.Insert(0, new ListItem("Select", "255"));
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
    private void CancelSubmit()
    {
        
        ddlCategoryFor.SelectedValue="255";
        BindParentCategoryName();
        ddlParentCategoryName.SelectedIndex = 0;
        chkActive.Checked = true;
        txtCategoryName.Text = "";
        btnSubmit.Text = "Submit";
        lblparentcat.Visible = false;
        txtparent.Visible = false;
        txtParentCategory.Text = "";
        
        updSave.Update();
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
                objGet.CategoryName = txtCategoryNameSearch.Text.Trim();
                objGet.ParentCategoryId = Convert.ToInt32(ddlParentCategoryNameSearch.SelectedValue);
                objGet.CategoryForId = Convert.ToInt32(ddlCategoryForSearch.SelectedValue);
                objGet.CompanyId = PageBase.ClientId;
                DataTable dt = objGet.GetCategoryMasterData();
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
    protected void ddlParentCategoryName_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        if (ddlParentCategoryName.SelectedValue == "10000")
        {
            lblparentcat.Visible = true;
            txtparent.Visible = true;
            
        }
        else
        {
            lblparentcat.Visible = false;
            txtparent.Visible = false;
            ucMsg.Visible = false;
            dvMsg.Style.Add("display", "none");
            updMessage.Update();
        }
    }
}