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
* Created On: 12-April-2020
 * Description: This is a View Entity Mapping Interface.
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

public partial class Masters_Common_ViewEntityMapping : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindEntityMappingType();
            ddlEntityMappingTypeRelationSearch.Items.Insert(0, new ListItem("All", "0"));
            ddlPrimaryEntitySearch.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    protected void ddlEntityMappingTypeSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindEntityMappingTypeRelation(ddlEntityMappingTypeRelationSearch, short.Parse(ddlEntityMappingTypeSearch.SelectedValue), "All");
        ddlPrimaryEntitySearch.Items.Clear();
        ddlPrimaryEntitySearch.Items.Insert(0, new ListItem("Select", "0"));
    }
    protected void ddlEntityMappingTypeRelationSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindPrimaryEntityBy_EntityMappingTypeRelation(ddlPrimaryEntitySearch, short.Parse(ddlEntityMappingTypeSearch.SelectedValue), 0, short.Parse(ddlEntityMappingTypeRelationSearch.SelectedValue));
    }
    protected void Exporttoexcel_Click(object sender, EventArgs e)
    {
        try
        {
            using (clsEntityMappingTypeRelationMaster obj = new clsEntityMappingTypeRelationMaster())
            {
                obj.PageIndex = -1;
                obj.PageSize = Convert.ToInt32(PageBase.PageSize);
                obj.CompanyId = PageBase.ClientId;
                DataTable dt;
                obj.SecondaryParty = txtsecondaryparty.Text;
                if (ddlEntityMappingTypeRelationSearch.SelectedValue != "")
                    obj.EntityMappingTypeRelationID = short.Parse(ddlEntityMappingTypeRelationSearch.SelectedValue);
                if (txtsecondaryparty.Text == "")
                {
                    dt = obj.SelectEntityMappingSearch(short.Parse(ddlEntityMappingTypeSearch.SelectedValue), Convert.ToInt64(ddlPrimaryEntitySearch.SelectedValue));
                }
                if (ddlPrimaryEntitySearch.SelectedValue == "")
                {
                    ddlPrimaryEntitySearch.SelectedValue = "0";
                    dt = obj.SelectEntityMappingSearch(short.Parse(ddlEntityMappingTypeSearch.SelectedValue), Convert.ToInt64(ddlPrimaryEntitySearch.SelectedValue));
                }
                else
                    dt = obj.SelectEntityMappingSearch(short.Parse(ddlEntityMappingTypeSearch.SelectedValue), Convert.ToInt64(ddlPrimaryEntitySearch.SelectedValue));

                if (dt != null || dt.Rows.Count > 0)
                {
                    DataSet ds = new DataSet();
                    ds.Merge(dt);
                    PageBase.ExportToExecl(ds, "EntityMappingDetail");
                }
                updpnlData.Update();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
            updpnlData.Update();
           
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindList(1);
    }
    void BindEntityMappingType()
    {

        try
        {
            using (clsEntityMappingTypeMaster obj = new clsEntityMappingTypeMaster())
            {

                obj.EntityMappingTypeID = 0;
                obj.CompanyId = PageBase.ClientId;
                DataTable dt = obj.Select();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        ddlEntityMappingTypeSearch.DataSource = dt;
                        ddlEntityMappingTypeSearch.DataTextField = "Description";
                        ddlEntityMappingTypeSearch.DataValueField = "EntityMappingTypeID";
                        ddlEntityMappingTypeSearch.DataBind();
                        ddlEntityMappingTypeSearch.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {
                        ddlEntityMappingTypeSearch.Items.Insert(0, new ListItem("Select", "0"));

                    }
                }
                else
                {
                    ddlEntityMappingTypeSearch.Items.Insert(0, new ListItem("Select", "0"));

                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
            updpnlData.Update();
           
        }
    }
    void BindEntityMappingTypeRelation(DropDownList ddl, short EntityMappingTypeID, string FirstItem)
    {
        try
        {
            using (clsEntityMappingTypeRelationMaster obj = new clsEntityMappingTypeRelationMaster())
            {

                obj.EntityMappingTypeRelationID = 0;
                obj.CompanyId = PageBase.ClientId;
                DataTable dt = obj.Select(EntityMappingTypeID);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        ddl.DataSource = dt;
                        ddl.DataTextField = "EntityTypeRelation";
                        ddl.DataValueField = "EntityMappingTypeRelationID";
                        ddl.DataBind();
                        ddl.Items.Insert(0, new ListItem(FirstItem, "0"));
                    }
                    else
                    {
                        ddl.Items.Clear();
                        ddl.Items.Insert(0, new ListItem(FirstItem, "0"));

                    }

                }
                else
                {
                    ddl.Items.Clear();
                    ddl.Items.Insert(0, new ListItem(FirstItem, "0"));

                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
            updpnlData.Update();
            
        }


    }
    void BindPrimaryEntityBy_EntityMappingTypeRelation(DropDownList ddl, short EntityMappingTypeID, short PrimaryEntityID, short EntityMappingTypeRelationID)
    {
        try
        {
            using (clsEntityMappingTypeRelationMaster obj = new clsEntityMappingTypeRelationMaster())
            {
                obj.LoginEntityTypeId = PageBase.EntityTypeID;
                obj.UserID = PageBase.UserId;
                obj.EntityMappingTypeRelationID = EntityMappingTypeRelationID;
                obj.CompanyId = PageBase.ClientId;
                DataTable dt = obj.SelectByEntityMapping(EntityMappingTypeID, PrimaryEntityID);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        ddl.DataSource = dt;
                        ddl.DataTextField = "CompanyDisplayName";
                        ddl.DataValueField = "PrimaryEntityID";
                        ddl.DataBind();
                        ddl.Items.Insert(0, new ListItem("Select", "0"));
                    }
                    else
                    {
                        ddl.Items.Clear();
                        ddl.Items.Insert(0, new ListItem("Select", "0"));

                    }
                }
                else
                {
                    ddl.Items.Clear();
                    ddl.Items.Insert(0, new ListItem("Select", "0"));

                }
               
            }
            updpnlData.Update();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
            updpnlData.Update();

        }
    }
    void BindList(int index)
    {
        try
        {
            dvSearch.Visible = true;
            using (clsEntityMappingTypeRelationMaster obj = new clsEntityMappingTypeRelationMaster())
            {
                obj.PageIndex = index;
                obj.PageSize = Convert.ToInt32(PageBase.PageSize);
                DataTable dt;

                obj.EntityMappingTypeRelationID = short.Parse(ddlEntityMappingTypeRelationSearch.SelectedValue);
                obj.SecondaryParty = txtsecondaryparty.Text;
                obj.CompanyId = PageBase.ClientId;
                dt = obj.SelectEntityMappingSearch(short.Parse(ddlEntityMappingTypeSearch.SelectedValue), Convert.ToInt64(ddlPrimaryEntitySearch.SelectedValue));

                if (dt.Rows.Count == 0 && index > 1)
                {
                    index--;
                    obj.PageIndex = index;
                    obj.PageNumber = index;
                    obj.SecondaryParty = txtsecondaryparty.Text;
                    obj.CompanyId = PageBase.ClientId;
                    dt = obj.SelectEntityMappingSearch(short.Parse(ddlEntityMappingTypeSearch.SelectedValue), Convert.ToInt64(ddlPrimaryEntitySearch.SelectedValue));

                }

                grdvList.Visible = true;
                grdvList.DataSource = dt;
                grdvList.DataBind();

                if (dt == null || dt.Rows.Count == 0)
                {
                    ucPagingControl1.Visible = false;
                }
                else
                {
                    //Paging
                    ucPagingControl1.Visible = true;
                    ucPagingControl1.PageSize =Convert.ToInt32(PageBase.PageSize);
                    ucPagingControl1.TotalRecords = obj.TotalRecords;
                    ucPagingControl1.FillPageInfo();
                }



            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        using (clsEntityMappingTypeRelationMaster clsApplicationUsedMaster = new clsEntityMappingTypeRelationMaster())
        {
            int intPageNumber = ucPagingControl1.CurrentPage;
            clsApplicationUsedMaster.PageIndex = intPageNumber;
            BindList(ucPagingControl1.CurrentPage);
            clsApplicationUsedMaster.PageNumber = intPageNumber;
        }

    }
}