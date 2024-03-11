#region Copyright(c) 2017 Zed-Axis Technologies All rights are reserved
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
* Created On: 03-April-2020
 * Description: This is a Entity type Master Relation Interface.
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

public partial class Masters_Common_ManageEntityTypeRelation : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dvMsg.Style.Add("display", "none");
            divgrd.Style.Add("display", "none");
            DataSet ds = new DataSet();
            using (clsEntityMappingTypeRelationMaster objget = new clsEntityMappingTypeRelationMaster())
            {
                objget.CompanyId = PageBase.ClientId;
                objget.EntityMappingID = 5;
                ds = objget.getEntityMappingTypeRelationMasterDropdowns();
            }
            DataTable dtMappingMode = PageBase.GetEnumByTableName("XML_Enum", "GetMappingMode");
            DataTable dtEntityType = null;
            DataTable dtEntityMappingType = ds.Tables[0];
            BindDDl(dtMappingMode, ddlMappingMode, "ID", "Description");
            BindDDl(dtEntityType, ddlPrimaryEntityType, "EntityTypeID", "EntityType");
            BindDDl(dtEntityType, ddlSecondaryEntityType, "EntityTypeID", "EntityType");
            BindDDl(dtEntityMappingType, ddlMappingType, "EntityMappingTypeID", "Description");
            BindDDl(dtEntityType, ddlPrimaryEntityTypeSearch, "EntityTypeID", "EntityType");
            BindDDl(dtEntityType, ddlSecondaryEntityTypeSearch, "EntityTypeID", "EntityType");
            BindDDl(dtEntityMappingType, ddlMappingTypeSearch, "EntityMappingTypeID", "Description");
        }
    }
    private void BindDDl(DataTable dt, DropDownList DDLNew, string ID, string Name)
    {
        try
        {
            DDLNew.DataSource = dt;
            DDLNew.DataValueField = ID;
            DDLNew.DataTextField = Name;
            DDLNew.DataBind();
            ListItem oListItemModel = DDLNew.Items.FindByValue("0");
            if (oListItemModel == null)
            {
                DDLNew.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnSubmit_Click(object sender, System.EventArgs e)
    {
        using (clsEntityMappingTypeRelationMaster objSave = new clsEntityMappingTypeRelationMaster())
        {
            objSave.EntityMappingTypeID = Convert.ToInt32(ddlMappingType.SelectedValue);
            objSave.PrimaryEntityTypeID = Convert.ToInt16(ddlPrimaryEntityType.SelectedValue);
            objSave.SecondaryEntityTypeID = Convert.ToInt16(ddlSecondaryEntityType.SelectedValue);
            objSave.MappingMode = Convert.ToInt16(ddlMappingMode.SelectedValue);
            objSave.UserID = UserId;
            objSave.CompanyId =PageBase.ClientId;
            objSave.Active = Convert.ToInt16(chkActive.Checked);
            if (txtRmark.Text.Trim() != string.Empty)
                objSave.Remarks = txtRmark.Text.Trim();
            if (btnSubmit.Text == "Update")
            {
                objSave.EntityMappingTypeRelationID = Convert.ToInt32(ViewState["EntityMappingTypeRelationID"]);
                objSave.Condition = 0;
            }
            objSave.SaveEntityMappingTypeRelationMaster();
            if (objSave.Out_Param == 0)
            {
                if (btnSubmit.Text == "Submit")
                    ucMsg.ShowSuccess(Resources.SuccessMessages.SaveSuccess);
                else
                    ucMsg.ShowSuccess(Resources.Messages.EditSuccessfull);
                BindList(Convert.ToInt32(ViewState["PageIndex"]));
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
    }
    private void CancelSubmit()
    {
        ddlMappingType.SelectedIndex = 0;
        if (ddlMappingType.SelectedIndex == 0)
        {
            ddlPrimaryEntityType.Items.Clear();
            ddlSecondaryEntityType.Items.Clear();
            ddlPrimaryEntityType.Items.Insert(0,new ListItem("Select","0"));
            ddlSecondaryEntityType.Items.Insert(0, new ListItem("Select","0"));
        }
       
        ddlMappingMode.SelectedIndex = 0;
        chkActive.Checked = true;
        txtRmark.Text = "";
        btnSubmit.Text = "Submit";
        updSave.Update();
    }
    protected void btnSearch_Click(object sender, System.EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlMappingTypeSearch.SelectedValue) == 0 && Convert.ToInt16(ddlPrimaryEntityTypeSearch.SelectedValue) == 0 && Convert.ToInt16(ddlSecondaryEntityTypeSearch.SelectedValue) == 0)
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
    protected void btnShowAll_Click(object sender, System.EventArgs e)
    {
        try
        {
            ddlMappingTypeSearch.SelectedIndex = 0;
            ddlPrimaryEntityTypeSearch.SelectedIndex = 0;
            ddlSecondaryEntityTypeSearch.SelectedIndex = 0;
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
    private void BindList(int index)
    {
        try
        {
            index = index == 0 ? 1 : index;
            using (clsEntityMappingTypeRelationMaster objGet = new clsEntityMappingTypeRelationMaster())
            {
                objGet.PageSize = Convert.ToInt32(PageBase.PageSize);
                objGet.PageIndex = index;
                objGet.EntityMappingTypeID = Convert.ToInt32(ddlMappingTypeSearch.SelectedValue);
                objGet.PrimaryEntityTypeID = Convert.ToInt16(ddlPrimaryEntityTypeSearch.SelectedValue);
                objGet.SecondaryEntityTypeID = Convert.ToInt16(ddlSecondaryEntityTypeSearch.SelectedValue);
                objGet.CompanyId = PageBase.ClientId;
                DataTable dt = objGet.GetEntityMappingTypeRelationMaste();
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
    protected void grdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "cmdEdit")
            {
                using (clsEntityMappingTypeRelationMaster objGet = new clsEntityMappingTypeRelationMaster())
                {
                    objGet.PageSize = Convert.ToInt32(PageBase.PageSize);
                    objGet.PageIndex = 1;
                    objGet.EntityMappingTypeRelationID = Convert.ToInt32(e.CommandArgument);
                    ViewState["EntityMappingTypeRelationID"] = Convert.ToInt32(e.CommandArgument);
                    DataTable dt = objGet.GetEntityMappingTypeRelationMaste();
                    ddlMappingType.SelectedIndex = ddlMappingType.Items.IndexOf(ddlMappingType.Items.FindByValue(dt.Rows[0]["EntityMappingTypeID"].ToString()));
                    ddlMappingType_SelectedIndexChanged(new object(), new EventArgs());
                    ddlPrimaryEntityType.SelectedIndex = ddlPrimaryEntityType.Items.IndexOf(ddlPrimaryEntityType.Items.FindByValue(dt.Rows[0]["PrimaryEntityTypeID"].ToString()));
                    ddlSecondaryEntityType.SelectedIndex = ddlSecondaryEntityType.Items.IndexOf(ddlSecondaryEntityType.Items.FindByValue(dt.Rows[0]["SecondaryEntityTypeID"].ToString()));
                    ddlMappingMode.SelectedIndex = ddlMappingMode.Items.IndexOf(ddlMappingMode.Items.FindByValue(dt.Rows[0]["MappingModeID"].ToString()));
                    chkActive.Checked = dt.Rows[0]["Active"].ToString() == "1" ? true : false;
                    txtRmark.Text = dt.Rows[0]["Remarks"].ToString();
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
                    objActive.EntityMappingTypeRelationID = Convert.ToInt32(e.CommandArgument);
                    objActive.Condition = 1;
                    objActive.CompanyId = PageBase.ClientId;
                    objActive.SaveEntityMappingTypeRelationMaster();
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
    protected void Exporttoexcel_Click(object sender, System.EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            using (clsEntityMappingTypeRelationMaster objGet = new clsEntityMappingTypeRelationMaster())
            {
                objGet.PageSize = Convert.ToInt32(ViewState["TotalRecords"]);
                objGet.PageIndex = -1;
                objGet.EntityMappingTypeID = Convert.ToInt32(ddlMappingTypeSearch.SelectedValue);
                objGet.PrimaryEntityTypeID = Convert.ToInt16(ddlPrimaryEntityTypeSearch.SelectedValue);
                objGet.SecondaryEntityTypeID = Convert.ToInt16(ddlSecondaryEntityTypeSearch.SelectedValue);
                objGet.CompanyId = PageBase.ClientId;
                dt = objGet.GetEntityMappingTypeRelationMaste();
                dt.Columns.Remove("Row");
                dt.Columns.Remove("EntityMappingTypeRelationID");
                dt.Columns.Remove("EntityMappingTypeID");
                dt.Columns.Remove("PrimaryEntityTypeID");
                dt.Columns.Remove("SecondaryEntityTypeID");
                dt.Columns.Remove("MappingModeID");
                dt.AcceptChanges();
            }
            DataSet ds = new DataSet();
            ds.Merge(dt);
            string FilenameToexport = "EntityMappingRelationTypeDetails";
            PageBase.ExportToExecl(ds, FilenameToexport);

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message);
            dvMsg.Style.Add("display", "block");

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
    protected void ddlMappingType_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            DataTable dtPrimaryEntityType = null;
            DataTable dtSecondaryEntityType = null;
            using (clsEntityMappingTypeRelationMaster objget = new clsEntityMappingTypeRelationMaster())
            {
                objget.CompanyId = PageBase.ClientId;
                objget.EntityMappingID = Convert.ToInt16(ddlMappingType.SelectedValue);
                ds = objget.getEntityMappingTypeRelationMasterDropdowns();
                if(ds.Tables.Count>0)
                {
                    dtPrimaryEntityType = ds.Tables[0];
                 dtSecondaryEntityType = ds.Tables[1];
                BindDDl(dtPrimaryEntityType, ddlPrimaryEntityType, "EntityTypeID", "EntityType");
                BindDDl(dtSecondaryEntityType, ddlSecondaryEntityType, "EntityTypeID", "EntityType");
                }
                else
                {
                    ddlPrimaryEntityType.Items.Clear();
                    ddlSecondaryEntityType.Items.Clear();
                    BindDDl(dtPrimaryEntityType, ddlPrimaryEntityType, "EntityTypeID", "EntityType");
                    BindDDl(dtSecondaryEntityType, ddlSecondaryEntityType, "EntityTypeID", "EntityType");
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
    protected void ddlMappingTypeSearch_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            DataTable dtPrimaryEntityType = null;
            DataTable dtSecondaryEntityType = null;
            using (clsEntityMappingTypeRelationMaster objget = new clsEntityMappingTypeRelationMaster())
            {
               
                objget.CompanyId = PageBase.ClientId;
                objget.EntityMappingID = Convert.ToInt16(ddlMappingTypeSearch.SelectedValue);
                ds = objget.getEntityMappingTypeRelationMasterDropdowns();
                if(ds.Tables.Count>0)
                {
                     dtPrimaryEntityType = ds.Tables[0];
                     dtSecondaryEntityType = ds.Tables[1];
                    BindDDl(dtPrimaryEntityType, ddlPrimaryEntityTypeSearch, "EntityTypeID", "EntityType");
                    BindDDl(dtSecondaryEntityType, ddlSecondaryEntityTypeSearch, "EntityTypeID", "EntityType");
                }
                else
                {
                    ddlPrimaryEntityTypeSearch.Items.Clear();
                    ddlSecondaryEntityTypeSearch.Items.Clear();
                    BindDDl(dtPrimaryEntityType, ddlPrimaryEntityTypeSearch, "EntityTypeID", "EntityType");
                    BindDDl(dtSecondaryEntityType, ddlSecondaryEntityTypeSearch, "EntityTypeID", "EntityType");
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
}