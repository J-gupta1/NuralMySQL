#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ===================================================================================================
 * <copyright company="Zed Axis Technologies">
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd. 
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
 * WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * </copyright>
 * ===================================================================================================
 * Created By : Sumit Maurya
 * Role       : Software Developer.
 * Date       : 02-Aug-2016
 * ===================================================================================================
 * Reviewed By : 
 * ===================================================================================================
 * Change Log
 * Date , Name , #CCXX, Description
 *  ===================================================================================================
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



public partial class HO_Admin_CBHStateMapping : PageBase
{




    DataTable roleinfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                BindOrgnHierarchy();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    void BindOrgnHierarchy()
    {
        try
        {
            ddlOrgnHierarchy.Items.Clear();
            using (OrgHierarchyData Objorgn = new OrgHierarchyData())
            {
                Objorgn.HierarchyLevelID = 3;
                Objorgn.intStatus = 1;
                string[] str = { "OrgnhierarchyID", "LocationName" };
                PageBase.DropdownBinding(ref ddlOrgnHierarchy, Objorgn.GetOrgnHierarchyData(), str);
                // ViewState["LoggedIn"] = ObjSalesChannel.LoggedInSalesChannelID;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.Visible = false;
            Response.Redirect("CBHStateMapping.aspx", false);

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.Visible = false;
            bindGrid(1);
            BindGridState();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            int intOrgnHierarchyID = 0;
            // SalesChannelData objsales = new SalesChannelData();
            OrgHierarchyData objOrgn = new OrgHierarchyData();

            if (ddlExportoption.SelectedValue == "1")
            {
                intOrgnHierarchyID = Convert.ToInt32(ddlOrgnHierarchy.SelectedValue);
            }
            if (ddlExportoption.SelectedValue == "1" && intOrgnHierarchyID == 0)
            {
                ucMessage1.ShowInfo("Please select CBH");
                return;
            }
            objOrgn.OrgnhierarchyID = intOrgnHierarchyID;
            objOrgn.PageSize = Convert.ToInt32(ViewState["TotalRecords"]);
            objOrgn.PageIndex = -1;
            DataSet dsExp = objOrgn.GetOrgnhierarchyStateMappingData();
            if (objOrgn.TotalRecords > 0)
            {
                String FilePath = Server.MapPath("~");
                string FilenameToexport = "CBHStateMapping";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dsExp, FilenameToexport);
            }

            else
            {
                ucMessage1.ShowInfo(Resources.Messages.NoRecord);

            }
        }
        catch (Exception ex)
        {

        }

    }
    public void bindGrid(int pageno)
    {
        try
        {
            ViewState["TotalRecords"] = 0;
            if (ViewState["CurrentPage"] == null)
            {
                pageno = 1;
                ViewState["CurrentPage"] = pageno;
            }
            //SalesChannelData objsales = new SalesChannelData();
            OrgHierarchyData objOrgn = new OrgHierarchyData();
            int intOrgnHierarchyID = Convert.ToInt32(ddlOrgnHierarchy.SelectedValue);
            updsearch.Update();
            objOrgn.OrgnhierarchyID = intOrgnHierarchyID;
            objOrgn.PageIndex = pageno;
            objOrgn.PageSize = Convert.ToInt32(PageSize);


            DataSet dsResult = objOrgn.GetOrgnhierarchyStateMappingData();
            if (objOrgn.TotalRecords > 0)
            {
                ucMessage1.Visible = false;
                ViewState["TotalRecords"] = objOrgn.TotalRecords;
                ucPagingControl1.TotalRecords = objOrgn.TotalRecords;
                ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                ucPagingControl1.SetCurrentPage = pageno;
                ucPagingControl1.FillPageInfo();
                gvCBHToState.DataSource = dsResult.Tables[0];
                gvCBHToState.DataBind();
                dvhide.Visible = true;
                dvFooter.Visible = true;
            }
            else
            {

                DataTable dtnew = dtBlank();
                dtnew.Rows.Add(dtnew.NewRow());
                dtnew.Rows[0]["Status"] = 1;
                gvCBHToState.DataSource = dtnew;
                gvCBHToState.DataBind();
                gvCBHToState.Rows[0].Visible = false;
                ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                dvhide.Visible = true;
                dvFooter.Visible = false;

            }
        }
        catch (Exception ex)
        {

        }


    }
    public DataTable dtBlank()
    {
        DataTable dtTemp = new DataTable();
        try
        {
            dtTemp.Columns.Add("OrgnHierarchyStateMappingID", typeof(System.Int32));
            dtTemp.Columns.Add("StateName", typeof(System.Int64));
            // dtTemp.Columns.Add("SalesChannelName", typeof(string));
            dtTemp.Columns.Add("Status", typeof(string));

            DataRow drTemp = dtTemp.NewRow();
        }
        catch (Exception ex)
        {

        }
        return dtTemp;

    }
    public void BindGridState()
    {
        try
        {
            DropDownList ddlgridState = (DropDownList)gvCBHToState.HeaderRow.FindControl("ddlState");
            using (GeographyData obj = new GeographyData())
            {
                DataTable dt;
                //obj.SalesChannelID = Convert.ToInt32(ddlSalesChannel.SelectedValue);
                obj.countryid = 1;
                dt = obj.GetAllActiveStates();
                String[] colArray = { "StateID", "StateName" };
                PageBase.DropdownBinding(ref ddlgridState, dt, colArray);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowWarning(ex.Message);

        }
    }

    protected void gvCBHToState_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "AddCBHtoState")
            {
                OrgHierarchyData obj = new OrgHierarchyData();
                DropDownList ddlgridState = (DropDownList)gvCBHToState.HeaderRow.FindControl("ddlState");
                Int16 intStateID = Convert.ToInt16(((DropDownList)gvCBHToState.HeaderRow.FindControl("ddlState")).SelectedValue);
                int intOrgnHierarchyID = Convert.ToInt32(ddlOrgnHierarchy.SelectedValue);
                obj.OrgnhierarchyID = intOrgnHierarchyID;
                obj.StateID = intStateID;
                obj.UserID = PageBase.UserId;
                int result = obj.InsertOrgnStateMapping();
                if (result == 0)
                {
                    bindGrid(Convert.ToInt32(ViewState["CurrentPage"]));
                    BindGridState();
                    ucMessage1.ShowSuccess("Mapping done sucessfully");

                }
                else if (result == 2)
                {
                    ucMessage1.ShowInfo("Mapping already exist for selected Organisation hierarchy and State.");
                }
                else if (result == 1)
                {
                    ucMessage1.ShowInfo("Error.");
                }
            }
            else if (e.CommandName == "Active")
            {
                OrgHierarchyData obj = new OrgHierarchyData();
                int OrgnHierarchyStateMappingID = Convert.ToInt32(e.CommandArgument);
                obj.intOrgnHierarchyStateMappingID = OrgnHierarchyStateMappingID;
                obj.UserID = PageBase.UserId;
                int result = obj.UpdateOrgnStateMappingStatus();
                if (result == 0)
                {
                    bindGrid(Convert.ToInt32(ViewState["CurrentPage"]));
                    BindGridState();
                    ucMessage1.ShowSuccess("Mapping status updated sucessfully");
                }
                else if (result == 2)
                {
                    ucMessage1.ShowInfo("Status couldn't updated. Active record already exists for selected CBH.");
                }
                else if (result == 1)
                {
                    ucMessage1.ShowInfo("Error in updating status.");
                }

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowWarning(ex.Message);

        }
    }


    protected void UCPagingControl1_SetControlRefresh()
    {
        ucMessage1.Visible = false;
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        bindGrid(ucPagingControl1.CurrentPage);
        BindGridState();
    }

}
