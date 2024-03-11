using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
using System.Data.SqlClient;
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By : Vijay Kumar Prajapati
* Created On : 21-July-2018
* Role : SE
* Module : Admin
* Description :This page is used for Setting Service Charge Type of the price
 * limit to set the reason is 100 words
 * Table Name: RegionMaster
* ====================================================================================================
* Reviewed By :
 ====================================================================================================
Modification On       Modified By          Modification    
---------------      -----------          -------------------------------------------------------------  
 ====================================================================================================
*/
public partial class Masters_Common_ManageRegion : PageBase
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            getCountry(0);
            BindRegionStatus();
            binddata(1);
        }
    }
    protected void btninsert_click(object sender, EventArgs e)
    {
        try
        {
            int result = 1;

            using (clsRegionMaster objRegion = new clsRegionMaster())
            {
                objRegion.Active = Convert.ToInt16(chkActive.Checked);
                objRegion.ZoneID = Convert.ToInt32(ddlZoneName.SelectedValue);
                objRegion.CountryID = Convert.ToInt32(ddlCountryName.SelectedValue);
                objRegion.RegionName = txtInsertRegionName.Text.Trim();
                objRegion.RegionCode = txtInsertRegionCode.Text.Trim();
                objRegion.Remarks = txtRemarks.Text.Trim();
                objRegion.CreatedBy = Convert.ToInt32(PageBase.UserId);
                objRegion.DisplayOrder = txtDisplayOrder.Text.Trim() != "" ? Convert.ToInt32(txtDisplayOrder.Text.Trim()) : objRegion.DisplayOrder;
                if (btnsubmit.Text == "Submit")
                {
                    result = objRegion.Save();
                    if (result == 0)
                    {
                        blankInsert();
                        binddata(1);
                        ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                       

                    }
                    else if (result == 1)
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);

                    }
                    else if (result == 2)
                        ucMessage1.ShowError("Duplicate Region Insert.");

                }
                else if (btnsubmit.Text == "Update")
                {
                    objRegion.ModifiedBy = UserId;
                    objRegion.RegionId = Convert.ToInt32(ViewState["RegionID"]);
                    result = objRegion.Update();
                    btnsubmit.Text = "Submit";
                    if (result == 0)
                    {
                        blankInsert();
                        binddata(1);
                        ucMessage1.ShowSuccess("Record Updated Successfully.");
                       

                    }
                    else if (result == 1)
                        ucMessage1.ShowError("Record Not Updated");
                    else if (result == 2)
                        ucMessage1.ShowError("Duplicate Region.");
                }
              
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        blankInsert();
        blankinsertbutton();
        blanksearch();
        binddata(1);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
       
        binddata(1);
       
    }
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            using (clsRegionMaster objRegion = new clsRegionMaster())
            {
                objRegion.CountryID = Convert.ToInt32(ddlCountrySearch.SelectedValue);
                objRegion.RegionName = txtSerregionName.Text.Trim();
                objRegion.Active = Convert.ToInt16(ddlStatus.SelectedValue);
                DataTable dt = objRegion.SelectAllExportinExcel();

                if (dt.Rows.Count > 0)
                {
                    DataSet dtcopy = new DataSet();
                    dtcopy.Merge(dt);
                    dtcopy.Tables[0].AcceptChanges();
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "RegionDetails";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);
                }
                else
                {
                    ucMessage1.ShowError(Resources.Messages.NoRecord);

                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    protected void grdRegion_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {
            if (e.CommandName == "Active")
            {
                int id = Convert.ToByte(e.CommandArgument);
                try
                {

                    using (clsRegionMaster objRegion = new clsRegionMaster())
                    {
                        objRegion.RegionId = id;
                        objRegion.ModifiedBy = UserId;
                        Int16 result = 0;
                        result = objRegion.ToggleActivation();
                        if (result == 0)
                        {
                            binddata(Convert.ToInt32(ViewState["CurrentPage"]));
                            ucMessage1.Visible = true;
                            ucMessage1.ShowSuccess(Resources.Messages.StatusChanged);
                        }
                        else if (result == 1)
                        {
                            ucMessage1.ShowError(objmaster.error);
                            binddata(Convert.ToInt32(ViewState["CurrentPage"]));
                        }
                    }
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                    PageBase.Errorhandling(ex);
                }
            }

            if (e.CommandName == "cmdEdit")
            {
                try
                {
                    int id = Convert.ToByte(e.CommandArgument);
                    using (clsRegionMaster objRegion = new clsRegionMaster())
                    {
                        ViewState["RegionID"] = id;
                        objRegion.RegionId = id;
                        DataSet ds = objRegion.SelectForEdit();
                        if (ds != null && ds.Tables.Count > 0)
                        {   
                            btnsubmit.Text = "Update";
                            chkActive.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Active"]);
                            txtInsertRegionName.Text = Convert.ToString(ds.Tables[0].Rows[0]["RegionName"]);
                            txtRemarks.Text = Convert.ToString(ds.Tables[0].Rows[0]["Remarks"]);
                            txtDisplayOrder.Text = ds.Tables[0].Rows[0]["DisplayOrder"].ToString();
                            ddlCountryName.ClearSelection();
                            ddlCountryName.Items.FindByValue(ds.Tables[0].Rows[0]["CountryID"].ToString()).Selected = true;
                            ddlCountryName_SelectedIndexChanged(new object(), new EventArgs());
                            if (!string.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["ZoneID"])))
                            {
                                ddlZoneName.Items.FindByValue(ds.Tables[0].Rows[0]["ZoneID"].ToString()).Selected = true;
                            }
                            ucMessage1.Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                    PageBase.Errorhandling(ex);
                }
            }
        }
    }
    protected void grdRegion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ucMessage1.Visible = false;
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        binddata(ucPagingControl1.CurrentPage);
    }
    public void getCountry(int countryid)
    {
        using (MastersData objcountry = new MastersData())
        {
            objcountry.Active = 1;
            objcountry.CountryID = countryid;
            DataTable dt = objcountry.SelectList();
            ddlCountryName.DataSource = dt;
            ddlCountryName.DataTextField = "CountryName";
            ddlCountryName.DataValueField = "CountryID";
            ddlCountryName.DataBind();
            ddlCountryName.Items.Insert(0, new ListItem("Select", "0"));
            ddlCountrySearch.DataSource = dt;
            ddlCountrySearch.DataTextField = "CountryName";
            ddlCountrySearch.DataValueField = "CountryID";
            ddlCountrySearch.DataBind();
            ddlCountrySearch.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    protected void ddlCountryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlZoneName.Items.Clear();
        using (MastersData obj = new MastersData())
        {
            DataTable dt;
            obj.CountryId = Convert.ToInt32(ddlCountryName.SelectedValue);
            dt = obj.SelectZoneInfo();
            String[] colArray = { "ZoneID", "ZoneName" };
            PageBase.DropdownBinding(ref ddlZoneName, dt, colArray);
            //if (Convert.ToString(dt.Rows[0]["ZoneID"]) != "")
            //{
            //    ddlZoneName.Items.FindByValue(dt.Rows[0]["ZoneID"].ToString()).Selected = true;
            //}
            //else
            //{
            //    ddlZoneName.Items.Insert(0, new ListItem("Select", "0"));
            //}
        }

    }
    public void blankInsert()
    {
        btnsubmit.Text = "Submit";
        txtInsertRegionName.Text = "";

        ddlCountryName.SelectedValue = "0";
        ddlZoneName.Items.Clear();
        ddlZoneName.Items.Insert(0, new ListItem("Select", "0"));
        txtRemarks.Text = "";
        txtDisplayOrder.Text = "";
        txtInsertRegionCode.Text = "";
        chkActive.Checked = true;
        if (ViewState["CountryEditStatus"] != null)
        {
            getCountry(0);
            ViewState["CountryEditStatus"] = null;
        }
    }
    public void binddata(int pageno)
    {
        try
        {
            ucMessage1.Visible = false;
            if (ViewState["CurrentPage"] == null)
            {
                pageno = 1;
                ViewState["CurrentPage"] = pageno;
            }
            using (clsRegionMaster objRegion = new clsRegionMaster())
            {
                objRegion.CountryID = Convert.ToInt32(ddlCountrySearch.SelectedValue);
                objRegion.PageIndex = pageno;
                objRegion.PageSize = Convert.ToInt32(PageBase.PageSize);
                objRegion.ZoneName = txtSerregionName.Text.Trim();
                objRegion.Active = Convert.ToInt16(ddlStatus.SelectedValue);
                DataTable dt = objRegion.SelectAll();
                if (dt == null || dt.Rows.Count == 0)
                {
                    grdRegion.DataSource = dt;
                    grdRegion.DataBind();
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                    grdRegion.Visible = false;
                    dvFooter.Visible = false;
                }
                else
                {
                    Regiongrid.Visible = true;
                    dvFooter.Visible = true;
                    grdRegion.Visible = true;
                    grdRegion.DataSource = dt;
                    grdRegion.DataBind();
                    ViewState["TotalRecords"] = objRegion.TotalRecords;
                    ucPagingControl1.TotalRecords = objRegion.TotalRecords;
                    ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                    ucPagingControl1.SetCurrentPage = pageno;
                    ucPagingControl1.FillPageInfo();
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    private void BindRegionStatus()
    {
        DataTable dtresult = new DataTable();
        try
        {
            dtresult = MastersData.GetEnumbyTableName("XML_Enum", "RegionMasterStatus");
            if (dtresult.Rows.Count > 0)
            {
                ddlStatus.DataSource = dtresult;
                ddlStatus.DataTextField = "Description";
                ddlStatus.DataValueField = "Value";
                ddlStatus.DataBind();
                ddlStatus.Items.Insert(0, new ListItem("Select", "255"));
            }
            else
            {
                ddlStatus.Items.Insert(0, new ListItem("Select", "255"));
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
    public void blankinsertbutton()
    {
        try
        {
            btnsubmit.Text = "Submit";
            //  updAddUserMain.Update();
        }
        catch (Exception ex)
        {
            string err = ex.Message.ToString();
            ucMessage1.ShowError(err);
        }
    }
    public void blanksearch()
    {
        try
        {

            //  UpdSearch.Update();
        }
        catch (Exception ex)
        {
            string err = ex.Message.ToString();
            ucMessage1.ShowError(err);
        }
    }

    protected void btnSearchALL_Click(object sender, EventArgs e)
    {
        txtSerregionName.Text = "";
        ddlCountryName.SelectedValue = "0";
        ddlStatus.SelectedValue = "255";
        binddata(1);
    }
}