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
 * Created By : Karam Chand Sharma
 * Role       : Sr. Team Lead
 * Date       : 03-Oct-2016
 * ===================================================================================================
 * Reviewed By : 
 * ===================================================================================================
 * Change Log
 * Date , Name , #CCXX, Description
 * 12-Oct-216, Sumit Maurya, #CC01, Implementaion and functionality changed.
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



public partial class HO_Admin_CBHNDMapping : PageBase
{
    int intshowGrid = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {

                /*BindOrgnHierarchy(); #CC01 Commented */
                /* #CC01 Add Start */

                BindSalesChannel();
                ddlSalesChannel_SelectedIndexChanged(null, null);
                bindGrid(1);
                ClearBindCBH();
                ucMessage1.Visible = false;
                /* #CC01 Add End */
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /* #CC01 Comment Start 
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

                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.ToString());
            }
        }#CC01 Comment End 
        */


    /* #CC01 Add Start */
    #region NewCode
    void BindSalesChannel()
    {
        try
        {
            ddlSalesChannel.Items.Clear();
            ddlSalesChannel.Items.Clear();
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                ObjSalesChannel.SalesChannelTypeID = 6;
                ObjSalesChannel.ActiveStatus = 255;
                ObjSalesChannel.BaseEntityTypeID = Convert.ToInt16(PageBase.BaseEntityTypeID);
                ObjSalesChannel.UserID = PageBase.UserId;
                ObjSalesChannel.ActiveStatus = 1;
                string[] str = { "SalesChannelid", "SalesChannelName" };
                PageBase.DropdownBinding(ref ddlSalesChannel, ObjSalesChannel.GetSalesChannelListWithRetailer(), str);
                PageBase.DropdownBinding(ref ddlSalesChannelSearch, ObjSalesChannel.GetSalesChannelListWithRetailer(), str);
                ViewState["LoggedIn"] = ObjSalesChannel.LoggedInSalesChannelID;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }

    public void ClearBindCBH()
    {
        try
        {
            foreach (DataListItem item in dtListCBNDMapping.Items)
            {
                CheckBox chck = (CheckBox)item.FindControl("chckList");
                HiddenField hdnLocationID = (HiddenField)item.FindControl("HdnLocaionID");
                chck.Checked = false;
                hdnLocationID.Value = "";
                chck.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    public void BindCBH(DataTable dt)
    {
        try
        {
            foreach (DataListItem item in dtListCBNDMapping.Items)
            {
                CheckBox chck = (CheckBox)item.FindControl("chckList");
                HiddenField hdnLocationID = (HiddenField)item.FindControl("HdnLocaionID");
                foreach (DataRow dr in dt.Rows)
                {
                    if ((dr["OrgnhierarchyID"].ToString() == hdnLocationID.Value) && dr["Status"].ToString() == "1")
                    {
                        chck.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            int intOrgnHierarchyID = 0;
            OrgHierarchyData objOrgn = new OrgHierarchyData();
            objOrgn.SalesChannelID = Convert.ToInt32(ddlSalesChannelSearch.SelectedValue);
            objOrgn.OrgnhierarchyID = intOrgnHierarchyID;
            objOrgn.PageSize = Convert.ToInt32(ViewState["TotalRecords"]);
            objOrgn.PageIndex = -1;
            // updSave.Update();
            DataSet dsExp = objOrgn.GetOrgnhierarchyNDMappingData();

            if (objOrgn.TotalRecords > 0)
            {
                DataTable dt = dsExp.Tables[1].Copy();
                DataSet dsTemp = new DataSet();
                dsTemp.Tables.Add(dt);
                String FilePath = Server.MapPath("~");
                string FilenameToexport = "CBHNDMapping";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dsTemp, FilenameToexport);

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
            OrgHierarchyData objOrgn = new OrgHierarchyData();
            int intSaleschannelID = Convert.ToInt32(ddlSalesChannelSearch.SelectedValue);
            //updSave.Update();
            objOrgn.SalesChannelID = intSaleschannelID;
            objOrgn.PageIndex = pageno;
            objOrgn.PageSize = Convert.ToInt32(PageSize);

            DataSet ds = objOrgn.GetOrgnhierarchyNDMappingData();
            if (objOrgn.TotalRecords > 0)
            {
                gvCBHToND.DataSource = ds.Tables[1];
                gvCBHToND.DataBind();
                dvhide.Visible = true;
                dvFooter.Visible = true;
                ViewState["TotalRecords"] = objOrgn.TotalRecords;
                ucPagingControl1.TotalRecords = objOrgn.TotalRecords;
                ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                ucPagingControl1.SetCurrentPage = pageno;
                ucPagingControl1.FillPageInfo();
                BindCBH(ds.Tables[1]);

            }
            else
            {
                ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                gvCBHToND.DataSource = null;
                gvCBHToND.DataBind();
                dvhide.Visible = false;
                dvFooter.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable DtNew = CreateTempTable();
            Int16 updStatus = 0;
            foreach (DataListItem item in dtListCBNDMapping.Items)
            {
                CheckBox chck = (CheckBox)item.FindControl("chckList");
                HiddenField hdnLocationID = (HiddenField)item.FindControl("HdnLocaionID");
                DataRow dr = DtNew.NewRow();
                dr["ID"] = hdnLocationID.Value;
                dr["Status"] = chck.Checked == true ? 1 : 0;
                DtNew.Rows.Add(dr);
                DtNew.AcceptChanges();

            }
            if (ddlSalesChannel.SelectedValue == "0")
            {
                ucMessage1.ShowInfo("Please select ND.");
                ddlSalesChannel.Focus();
                return;
            }

            OrgHierarchyData objOrgn = new OrgHierarchyData();

            objOrgn.NDID = Convert.ToInt32(ddlSalesChannel.SelectedValue);
            objOrgn.DTCBHID = DtNew;
            objOrgn.UserID = PageBase.UserId;
            int intResult = objOrgn.InsertOrgnSalechannelMapping();
            if (intResult == 0)
            {
                intshowGrid = 1;
                int pageno = ViewState["CurrentPage"] == null ? 0 : Convert.ToInt32(ViewState["CurrentPage"].ToString());
                bindGrid(pageno);
                ddlSalesChannel.SelectedValue = "0";
                ddlSalesChannel_SelectedIndexChanged(null, null);
                ClearBindCBH();
                ucMessage1.ShowSuccess("Mapping saved successfully.");
            }
            else
            {
                if (string.IsNullOrEmpty(objOrgn.Error))
                    ucMessage1.ShowError(objOrgn.Error);
                else
                {
                    ucMessage1.ShowError("Error in saving records. Please try after some time.");
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }

    }

    protected void gvCBHToND_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Int32 intOrgnHierarchyNDMappingID = Convert.ToInt32(e.CommandArgument);
            Int32 RowIndex = Convert.ToInt32(e.CommandArgument) - 1;
            if (e.CommandName == "Edit")
            {
                ucMessage1.Visible = false;
                {
                    HiddenField hdnNDID = gvCBHToND.Rows[RowIndex].FindControl("hdnNDIDEdit") as HiddenField;
                    HiddenField HdnLocaionID = gvCBHToND.Rows[RowIndex].FindControl("HdnLocaionIDEdit") as HiddenField;
                    HiddenField HdnOrgnNDMappingEdit = gvCBHToND.Rows[RowIndex].FindControl("hdnOrgnNDMappingEdit") as HiddenField;
                    HiddenField HdnOrgnNDMappingStatusEdit = gvCBHToND.Rows[RowIndex].FindControl("hdnOrgnNDMappingStatusEdit") as HiddenField;


                    ddlSalesChannel.SelectedValue = hdnNDID.Value;
                    ddlSalesChannel_SelectedIndexChanged(null, null);


                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }

    protected void ddlSalesChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            OrgHierarchyData objOrgn = new OrgHierarchyData();
            int intSaleschannelID = Convert.ToInt32(ddlSalesChannel.SelectedValue);
            objOrgn.SalesChannelID = intSaleschannelID;
            objOrgn.PageIndex = 1;
            objOrgn.PageSize = Convert.ToInt32(PageSize);
            DataSet ds = objOrgn.GetOrgnhierarchyNDMappingData();
            if (ds != null)
            {
                dvBtn.Visible = true;
                dtListCBNDMapping.DataSource = ds.Tables[0];
                dtListCBNDMapping.DataBind();
                BindCBH(ds.Tables[1]);
                if (intSaleschannelID == 0)
                {
                    ClearBindCBH();
                }
                if (intshowGrid == 0)
                {
                    ucMessage1.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }

    DataTable CreateTempTable()
    {
        DataTable dtTemp = new DataTable();
        dtTemp.Columns.Add("ID", typeof(System.Int32));
        dtTemp.Columns.Add("Status", typeof(System.Int32));
        return dtTemp;
    }

    #endregion NewCode
    /* #CC01 Add End */

    protected void btncancel_Click(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.Visible = false;
            Response.Redirect("CBHNDMapping.aspx", false);
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
            /*  BindGridNDList(); #CC01 Commented  */
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ucMessage1.Visible = false;
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        bindGrid(ucPagingControl1.CurrentPage);

    }
    #region OldCode
    /* #CC01 Comment Start
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            int intOrgnHierarchyID = 0;
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
            DataSet dsExp = objOrgn.GetOrgnhierarchyNDMappingData();
            if (objOrgn.TotalRecords > 0)
            {
                String FilePath = Server.MapPath("~");
                string FilenameToexport = "CBHNDMapping";
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
            OrgHierarchyData objOrgn = new OrgHierarchyData();
           // int intOrgnHierarchyID = Convert.ToInt32(ddlOrgnHierarchy.SelectedValue);
            //updsearch.Update();
            objOrgn.OrgnhierarchyID = intOrgnHierarchyID;
            objOrgn.PageIndex = pageno;
            objOrgn.PageSize = Convert.ToInt32(PageSize);


            DataSet dsResult = objOrgn.GetOrgnhierarchyNDMappingData();
            if (objOrgn.TotalRecords > 0)
            {
                ucMessage1.Visible = false;
                ViewState["TotalRecords"] = objOrgn.TotalRecords;
                ucPagingControl1.TotalRecords = objOrgn.TotalRecords;
                ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                ucPagingControl1.SetCurrentPage = pageno;
                ucPagingControl1.FillPageInfo();
                gvCBHToND.DataSource = dsResult.Tables[0];
                gvCBHToND.DataBind();
                dvhide.Visible = true;
                dvFooter.Visible = true;
            }
            else
            {

                DataTable dtnew = dtBlank();
                dtnew.Rows.Add(dtnew.NewRow());
                dtnew.Rows[0]["Status"] = 1;
                gvCBHToND.DataSource = dtnew;
                gvCBHToND.DataBind();
                gvCBHToND.Rows[0].Visible = false;
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
            dtTemp.Columns.Add("OrgnHierarchySalechannelMappingID", typeof(System.Int32));
            dtTemp.Columns.Add("ND", typeof(System.Int64));
            dtTemp.Columns.Add("Status", typeof(string));

            DataRow drTemp = dtTemp.NewRow();
        }
        catch (Exception ex)
        {

        }
        return dtTemp;

    }

    public void BindGridNDList()
    {
        try
        {
            DropDownList ddlgridND = (DropDownList)gvCBHToND.HeaderRow.FindControl("ddlND");
            using (ReportData obj = new ReportData())
            {
                DataSet ds = new DataSet();
                obj.UserId = PageBase.UserId;
                ds = obj.GetSalesChannel();
                String[] colArray = { "SalesChannelID", "SalesChannelName" };
                PageBase.DropdownBinding(ref ddlgridND, ds.Tables[0], colArray);
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowWarning(ex.Message);

        }
    }

    protected void gvCBHToND_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "AddCBHtoND")
            {
                OrgHierarchyData obj = new OrgHierarchyData();
                DropDownList ddlgridState = (DropDownList)gvCBHToND.HeaderRow.FindControl("ddlND");
                Int16 intNDID = Convert.ToInt16(((DropDownList)gvCBHToND.HeaderRow.FindControl("ddlND")).SelectedValue);
                int intOrgnHierarchyID = Convert.ToInt32(ddlOrgnHierarchy.SelectedValue);
                obj.OrgnhierarchyID = intOrgnHierarchyID;
                obj.NDID = intNDID;
                obj.UserID = PageBase.UserId;
                int result = obj.InsertOrgnSalechannelMapping();
                if (result == 0)
                {
                    bindGrid(Convert.ToInt32(ViewState["CurrentPage"]));
                    BindGridNDList();
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
                int OrgnHierarchySaleChannelMappingID = Convert.ToInt32(e.CommandArgument);
                obj.intOrgnHierarchySaleChannelMappingID = OrgnHierarchySaleChannelMappingID;
                obj.UserID = PageBase.UserId;
                int result = obj.UpdateOrgnSaleChannelMappingStatus();
                if (result == 0)
                {
                    bindGrid(Convert.ToInt32(ViewState["CurrentPage"]));
                    BindGridNDList();
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
        BindGridNDList();
    }
   #CC01 Comment End */
    #endregion OldCode



}
