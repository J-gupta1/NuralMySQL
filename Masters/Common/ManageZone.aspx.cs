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

#region Copyright(c) 2010 Zed-Axis Technologies All rights are reserved
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
* Created On : 22-May-2018
* Role : SE
* Module : Admin
* Description :This page is used for Setting Service Charge Type of the price
 * limit to set the reason is 100 words
 * Table Name: ZoneMaster
* ====================================================================================================
* Reviewed By :
 ====================================================================================================
Modification On       Modified By          Modification    
---------------      -----------          -------------------------------------------------------------  
 ====================================================================================================
*/
#endregion
public partial class Masters_Common_ManageZone : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                getCountry(0);
                BindZoneStatus();
                binddata(1);
            }
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }



    #region user functions
    public void blankinsert()
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

    public bool insertvalidate()
    {
        if (txtInsertZoneName.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert City Name");
            return false;
        }
        return true;

    }


    #endregion

    #region control functions

    protected void btninsert_click(object sender, EventArgs e)
    {
        try
        {
            int result = 1;

            using (clsZoneMaster objZone = new clsZoneMaster())
            {
                objZone.Active = Convert.ToInt16(chkActive.Checked);
                objZone.ZoneName = txtInsertZoneName.Text.Trim();
                objZone.CountryID = Convert.ToInt32(ddlCountryName.SelectedValue);
                objZone.Remarks = txtRemarks.Text.Trim();
                objZone.CreatedBy = Convert.ToInt32(PageBase.UserId);
                objZone.DisplayOrder = txtDisplayOrder.Text.Trim() != "" ? Convert.ToInt32(txtDisplayOrder.Text.Trim()) : objZone.DisplayOrder;
                if (btnsubmit.Text == "Submit")
                {
                    result = objZone.Save();
                    if (result == 0)
                    {
                        ClearInsertControls();
                        ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);

                    }
                    else if (result == 1)
                    {
                        ucMessage1.ShowError(Resources.Messages.ErrorMsgTryAfterSometime);

                    }
                    else if (result == 2)
                        ucMessage1.ShowError("Duplicate Zone Insert.");

                }
                else if (btnsubmit.Text == "Update")
                {
                    objZone.ModifiedBy = UserId;
                    objZone.ZoneID = Convert.ToInt32(ViewState["ZoneID"]);
                    result = objZone.Update();
                    btnsubmit.Text = "Submit";
                    if (result == 0)
                    {
                        ClearInsertControls();
                        binddata(1);
                        ucMessage1.ShowSuccess("Record Updated Successfully.");

                    }
                    else if (result == 1)
                        ucMessage1.ShowError("Record Not Updated");
                    else if (result == 2)
                        ucMessage1.ShowError("Duplicate Zone.");
                }
                // updMsg.Update();
                //  updgrid.Update();
                //  updAddUserMain.Update();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {


        blankinsert();
        blanksearch();
        binddata(1);
        ucMessage1.Visible = false;
        ClearInsertControls();


    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        blankinsert();
        binddata(1);
    }

    protected void grdCountry_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {

            if (e.CommandName == "Active")
            {
                int id = Convert.ToByte(e.CommandArgument);

                try
                {

                    using (clsZoneMaster objZone = new clsZoneMaster())
                    {
                        objZone.ZoneID = id;
                        objZone.ModifiedBy = UserId;
                        Int16 result = 0;
                        result = objZone.ToggleActivation();
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
                        //  updAddUserMain.Update();
                        // updgrid.Update();
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
                    using (clsZoneMaster objZone = new clsZoneMaster())
                    {
                        ViewState["ZoneID"] = id;
                        objZone.ZoneID = id;
                        DataSet ds = objZone.SelectForEdit();
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            if (Convert.ToInt16(ds.Tables[0].Rows[0]["CountryStatus"]) == 0)
                            {
                                getCountry(Convert.ToInt16(ds.Tables[0].Rows[0]["CountryId"]));
                                ViewState["CountryEditStatus"] = 1;
                            }
                            btnsubmit.Text = "Update";
                            chkActive.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Active"]);
                            txtInsertZoneName.Text = Convert.ToString(ds.Tables[0].Rows[0]["ZoneName"]);
                            txtRemarks.Text = Convert.ToString(ds.Tables[0].Rows[0]["Remarks"]);
                            txtDisplayOrder.Text = ds.Tables[0].Rows[0]["DisplayOrder"].ToString();
                            ddlCountryName.ClearSelection();
                            ddlCountryName.Items.FindByValue(ds.Tables[0].Rows[0]["CountryID"].ToString()).Selected = true;
                            // updAddUserMain.Update();
                            // updgrid.Update();
                            //  updMsg.Update();
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
    protected void UCPagingControl1_SetControlRefresh()
    {
        ucMessage1.Visible = false;
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        binddata(ucPagingControl1.CurrentPage);


    }
    protected void grdCountry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    #endregion

    # region export to excel
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            using (clsZoneMaster objZone = new clsZoneMaster())
            {
                objZone.CountryID = Convert.ToInt32(ddlCountrySearch.SelectedValue);
                objZone.ZoneName = txtSerzoneName.Text.Trim();
                objZone.Active = Convert.ToInt16(ddlStatus.SelectedValue);
                DataTable dt = objZone.SelectAllExportinExcel();

                if (dt.Rows.Count > 0)
                {
                    DataSet dtcopy = new DataSet();
                    dtcopy.Merge(dt);
                    dtcopy.Tables[0].AcceptChanges();
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "ZoneDetails";
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
    private void BindZoneStatus()
    {
        DataTable dtresult = new DataTable();
        try
        {
            dtresult = MastersData.GetEnumbyTableName("XML_Enum", "ZoneMasterStatus");
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
    void ClearInsertControls()
    {
        txtRemarks.Text = "";
        txtDisplayOrder.Text = "";
        txtInsertZoneName.Text = "";
        if (ViewState["CountryEditStatus"] != null)
        {
            getCountry(0);
            ViewState["CountryEditStatus"] = null;
        }

        ddlCountryName.SelectedValue = "0";
        chkActive.Checked = true;
        ucMessage1.Visible = false;
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
            using (clsZoneMaster objZone = new clsZoneMaster())
            {
                objZone.CountryID = Convert.ToInt32(ddlCountrySearch.SelectedValue);
                objZone.PageIndex = pageno;
                objZone.PageSize = Convert.ToInt32(PageBase.PageSize);
                objZone.ZoneName = txtSerzoneName.Text.Trim();
                objZone.Active = Convert.ToInt16(ddlStatus.SelectedValue);
                DataTable dt = objZone.SelectAll();
                if (dt == null || dt.Rows.Count == 0)
                {
                    grdzone.DataSource = dt;
                    grdzone.DataBind();
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                    Zonegrid.Visible = false;
                    dvFooter.Visible = false;
                }
                else
                {
                    Zonegrid.Visible = true;
                    dvFooter.Visible = true;
                    grdzone.DataSource = dt;
                    grdzone.DataBind();
                    ViewState["TotalRecords"] = objZone.TotalRecords;

                    ucPagingControl1.TotalRecords = objZone.TotalRecords;
                    ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                    ucPagingControl1.SetCurrentPage = pageno;
                    ucPagingControl1.FillPageInfo();
                }
                // updgrid.Update();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    #endregion
}

