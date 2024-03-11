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
 * Date       : 19-May-2016
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

/*Change Log:
 
 * 21-May-2018, Rajnish Kumar, #CC01, SaleschannelType dropdown is added and Based on that saleschannel dropdown is binded
 */

public partial class SalesChannel_NDStateMapping : PageBase
{




    DataTable roleinfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {

                BindSaleschannelType();/* #CC01*/
                BindSaleschannelTypeFroExport();
                // bindGrid();
                // BindGridState();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    void BindSaleschannelType() /* #CC01*/
    {
        try
        {
            ddlSalesChannel.Items.Clear();
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
             
                ObjSalesChannel.BaseEntityTypeID = Convert.ToInt16(PageBase.BaseEntityTypeID);
                ObjSalesChannel.UserID = PageBase.UserId;
                ObjSalesChannel.ActiveStatus = 1;
                string[] str = { "EntityTypeID", "EntityType" };
                PageBase.DropdownBinding(ref DdlSaleschannelType, ObjSalesChannel.GetSalesChannelTypeForStateMapping(), str);
                ViewState["LoggedIn"] = ObjSalesChannel.LoggedInSalesChannelID;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    void BindSaleschannelTypeFroExport()/* #CC01*/
    {
        try
        {
            ddlExportoption.Items.Clear();
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {

                ObjSalesChannel.BaseEntityTypeID = Convert.ToInt16(PageBase.BaseEntityTypeID);
                ObjSalesChannel.UserID = PageBase.UserId;
                ObjSalesChannel.ActiveStatus = 1;
                string[] str = { "EntityTypeID", "EntityType" };
                PageBase.DropdownBinding(ref ddlExportoption, ObjSalesChannel.GetSalesChannelTypeForStateMapping(), str);
                ViewState["LoggedIn"] = ObjSalesChannel.LoggedInSalesChannelID;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    void BindSalesChannel()
    {
        try
        {
            ddlSalesChannel.Items.Clear();
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(DdlSaleschannelType.SelectedValue);
                ObjSalesChannel.ActiveStatus = 255;
                ObjSalesChannel.BaseEntityTypeID = Convert.ToInt16(PageBase.BaseEntityTypeID);
                ObjSalesChannel.UserID = PageBase.UserId;
                ObjSalesChannel.ActiveStatus = 1;
                string[] str = { "SalesChannelid", "SalesChannelName" };
                PageBase.DropdownBinding(ref ddlSalesChannel, ObjSalesChannel.GetSalesChannelListWithRetailer(), str);
                ViewState["LoggedIn"] = ObjSalesChannel.LoggedInSalesChannelID;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        BindSalesChannel();
        //bindGrid();
        BindGridState();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        bindGrid(1);
        BindGridState();
    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        try

        {
            int intSaleschannelID = 0;
            SalesChannelData objsales = new SalesChannelData();


            //if (ddlExportoption.SelectedValue == "1") /* #CC01*/
            //{
            //    intSaleschannelID = Convert.ToInt32(ddlSalesChannel.SelectedValue);
            //}
            if (ddlExportoption.SelectedValue == "1" && intSaleschannelID == 0)
            {
                ucMessage1.ShowInfo("Please Select SaleschannelType");
                return;
            }
            objsales.SalesChannelTypeID = Convert.ToInt16(ddlExportoption.SelectedValue);
            objsales.SalesChannelID = intSaleschannelID;
            objsales.PageSize = Convert.ToInt32(ViewState["TotalRecords"]);
            objsales.PageIndex = -1;
            DataTable dtExp = objsales.GetChannelStateMapping();

            if (dtExp.Rows.Count > 0)
            {
                DataSet dscopy = new DataSet();
                dscopy.Merge(dtExp);
                dscopy.Tables[0].Columns.Add("NewStatus", typeof(string));
                foreach (DataRow dr in dscopy.Tables[0].Rows)
                {
                    if (dr["Status"].ToString() == "True")
                    {
                        dr["NewStatus"] = "Active";
                    }
                    else
                    {
                        dr["NewStatus"] = "InActive";
                    }
                }
                dscopy.Tables[0].Columns.Remove("Status");
                dscopy.Tables[0].Columns["NewStatus"].ColumnName = "Status";
                dscopy.Tables[0].AcceptChanges();
                dscopy.Tables[0].Columns.Remove("SalesChannelStateMappingID");

                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "SalesChannelStateMapping";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dscopy, FilenameToexport);
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
            SalesChannelData objsales = new SalesChannelData();
            int intSaleschannelID = Convert.ToInt32(ddlSalesChannel.SelectedValue);
            updsearch.Update();
            objsales.SalesChannelID = intSaleschannelID;
            objsales.PageIndex = pageno;
            objsales.PageSize = Convert.ToInt32(PageSize);
            objsales.SalesChannelTypeID = Convert.ToInt16(DdlSaleschannelType.SelectedValue);

            DataTable dt = objsales.GetChannelStateMapping();
            if (dt.Rows.Count > 0)
            {
                ucMessage1.Visible = false;
                ViewState["TotalRecords"] = objsales.TotalRecords;
                ucPagingControl1.TotalRecords = objsales.TotalRecords;
                ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                ucPagingControl1.SetCurrentPage = pageno;
                ucPagingControl1.FillPageInfo();
                gvNDToState.DataSource = dt;
                gvNDToState.DataBind();
                dvhide.Visible = true;
                dvFooter.Visible = true;
                // updgrid.Update();
                Label lblsaleChannelnameheading = (Label)gvNDToState.HeaderRow.FindControl("lblSaleChannelNameHeading");
                lblsaleChannelnameheading.Text = dt.Rows[0]["SalesChannelName"].ToString();


            }
            else
            {

                DataTable dtnew = dtBlank();
                dtnew.Rows.Add(dtnew.NewRow());
                dtnew.Rows[0]["Status"] = 1;
                gvNDToState.DataSource = dtnew;
                gvNDToState.DataBind();
                gvNDToState.Rows[0].Visible = false;
                //gvNDToState.Rows[0].Controls.Clear();
                ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                //updgrid.Update();
                dvhide.Visible = true;
                dvFooter.Visible = false;
                Label lblsaleChannelnameheading = (Label)gvNDToState.HeaderRow.FindControl("lblSaleChannelNameHeading");
                lblsaleChannelnameheading.Text = ddlSalesChannel.SelectedItem.ToString();
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
            dtTemp.Columns.Add("SalesChannelStateMappingID", typeof(System.Int32));
            dtTemp.Columns.Add("StateName", typeof(System.Int64));
            dtTemp.Columns.Add("SalesChannelName", typeof(string));
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
            DropDownList ddlgridState = (DropDownList)gvNDToState.HeaderRow.FindControl("ddlState");
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

    protected void gvNDToState_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "AddNDtoState")
            {
                SalesChannelData obj = new SalesChannelData();
                DropDownList ddlgridState = (DropDownList)gvNDToState.HeaderRow.FindControl("ddlState");
                Int16 intStateID = Convert.ToInt16(((DropDownList)gvNDToState.HeaderRow.FindControl("ddlState")).SelectedValue);
                int intSaleschannelID = Convert.ToInt32(ddlSalesChannel.SelectedValue);
                obj.Status = true;
                obj.SalesChannelID = intSaleschannelID;
                obj.StateID = intStateID;
                obj.UserID = PageBase.UserId;
                int result = obj.SaveSalechanneltostatemapping();
                if (result == 0)
                {

                    bindGrid(Convert.ToInt32(ViewState["CurrentPage"]));
                    BindGridState();
                    ucMessage1.ShowSuccess("Mapping done sucessfully");

                }
                else if (result == 2)
                {
                    ucMessage1.ShowInfo("Mapping already exists for selected ND and State.");
                }
                else if (result == 1)
                {
                    ucMessage1.ShowInfo("Error.");
                }
            }
            else if (e.CommandName == "Active")
            {
                SalesChannelData objsales = new SalesChannelData();
                int SalesChannelStateMappingID = Convert.ToInt32(e.CommandArgument);
                objsales.SalesChannelStateMappingID = SalesChannelStateMappingID;
                objsales.UserID = PageBase.UserId;
                objsales.ParentSalesChannelID = Convert.ToInt32(ddlSalesChannel.SelectedValue);
                int result = objsales.UpdateSalechanneltostatemapping();
                if (result == 0)
                {

                    bindGrid(Convert.ToInt32(ViewState["CurrentPage"]));
                    BindGridState();
                    ucMessage1.ShowSuccess("Mapping status updated sucessfully");

                }
                else if (result == 2)
                {
                    ucMessage1.ShowInfo("Status couldn't updated. Record exists for salechannel");
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

    protected void DdlSaleschannelType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSalesChannel();
    }
    
}
