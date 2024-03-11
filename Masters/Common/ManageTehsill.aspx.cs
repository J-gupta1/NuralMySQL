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

/*
 * 23-Mar-2016, Sumit Maurya, #CC01, On Show all button of search panel Country dropdown set to select option.
 * 24-May-2016, Karam Chand Sharma, #CC02, As per client requriment tehsil will come under city now 
 * 14-Mar-2018, Sumit Maurya, #CC03, default button set as it was firing chnage status while pressing enter button.
 */

public partial class Masters_HO_Common_ManageTehsill : PageBase
{

    DataTable tehsillinfo;
    DataTable tehsillserch = new DataTable();



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Page.Form.DefaultButton = getalldata.UniqueID; /* #CC03 Added */  
            fillcountry();
            databind();
            chkstatus.Checked = true;
        }
    }

    # region user defined functions

    public void databind()
    {
        using (MastersData objmaster = new MastersData())
        {
            ucMessage1.Visible = false;
            blankinserttext();
            objmaster.tehsillname = txtSerName.Text.Trim();
            objmaster.tehsillcode = txtSerCode.Text.Trim();
            objmaster.tehsillcountryid = Convert.ToInt32(cmbSerCountry.SelectedValue);
            if (cmbSerState.SelectedValue != "0")
            {
                objmaster.tehsillstateid = Convert.ToInt16(cmbSerState.SelectedValue.ToString());
            }
            else
            {
                objmaster.tehsillstateid = 0;
            }

            if (cmbSerDistrict.SelectedValue != "0")
            {
                objmaster.tehsilldistrictid = Convert.ToInt16(cmbSerDistrict.SelectedValue.ToString());

            }
            else
            {
                objmaster.tehsilldistrictid = 0;
            }
            if (cmbSerCity.SelectedValue != "0")
            {
                objmaster.tehsilCityId = Convert.ToInt16(cmbSerCity.SelectedValue.ToString());

            }
            else
            {
                objmaster.tehsilldistrictid = 0;
            }
            objmaster.tehsillselectionmode = 2;

            try
            {
                tehsillserch = objmaster.SelectTahsillInfo();
                grdTehsill.DataSource = tehsillserch;
                grdTehsill.DataBind();
                updgrid.Update();


            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }


    public void fillcountry()
    {
        using (MastersData objmaster = new MastersData())
        {

            try
            {
                DataTable dt;
                objmaster.CountrySelectionMode = 1;
                dt = objmaster.SelectCountryInfo();
                String[] colArray = { "CountryID", "CountryName" };
                PageBase.DropdownBinding(ref cmbinsCountry, dt, colArray);
                PageBase.DropdownBinding(ref cmbSerCountry, dt, colArray);
                cmbSerState.Items.Insert(0, new ListItem("Select", "0"));
                cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));
                cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));
                cmbSerCity.Items.Insert(0, new ListItem("Select", "0"));/*#CC02 ADDED*/
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }

        }
    }
    public void blanksertext()
    {
        txtSerName.Text = "";
        txtSerCode.Text = "";
        cmbinsCountry.SelectedValue = "0";
        cmbSerCountry.SelectedValue = "0";  /* #CC01 Added */
        cmbSerState.Items.Clear();
        cmbSerState.Items.Insert(0, new ListItem("Select", "0"));
        cmbSerDistrict.Items.Clear();
        cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));
        /*#CC02 ADDED START*/
        cmbSerCity.Items.Clear();
        cmbSerCity.Items.Insert(0, new ListItem("Select", "0"));
        /*#CC02 ADDED END*/
        UpdSearch.Update();
    }

    public void blankinserttext()
    {
        txtInsertCode.Text = "";
        txtInsertName.Text = "";
        cmbinsCountry.SelectedValue = "0";
        cmbInsertState.Items.Clear();
        cmbInsertState.Items.Insert(0, new ListItem("Select", "0"));
        cmbInsertDistrict.Items.Clear();
        cmbInsertDistrict.Items.Insert(0, new ListItem("Select", "0"));
        /*#CC02 ADDED START*/
        cmbInsertCity.Items.Clear();
        cmbInsertCity.Items.Insert(0, new ListItem("Select", "0"));
        /*#CC02 ADDED END*/
        btnSubmit.Text = "Submit";
        updAddUserMain.Update();
    }

    public bool insertvalidate()
    {
        if (cmbInsertState.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please select a State");
            return false;
        }
        if (cmbInsertDistrict.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please select a District");
            return false;
        }
        /*#CC02 ADDED START*/
        if (cmbInsertCity.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please select a City");
            return false;
        }
        /*#CC02 ADDED END*/
        if (txtInsertCode.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert Tehsil Code");
            return false;
        }
        if (txtInsertName.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert Tehsil Name");
            return false;
        }
        return true;


    }

    # endregion

    # region controlfunctions

    protected void btninsert_click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }           //Pankaj Kumar

        using (MastersData objmaster = new MastersData())
        {
            updAddUserMain.Update();
            if (!insertvalidate())
            {
                return;
            }
            else
            {
                objmaster.tehsillname = txtInsertName.Text.Trim();
                objmaster.tehsillcode = txtInsertCode.Text.Trim();
                objmaster.tehsillstateid = Convert.ToInt16(cmbInsertState.SelectedValue.ToString());
                objmaster.tehsilldistrictid = Convert.ToInt16(cmbInsertDistrict.SelectedValue.ToString());
                objmaster.tehsilCityId = Convert.ToInt16(cmbInsertCity.SelectedValue.ToString());/*#CC02 ADDED*/


                if (chkstatus.Checked == true)
                {
                    objmaster.tehsillstatus = 1;
                }
                else
                {
                    objmaster.tehsillstatus = 0;
                }
                if (ViewState["TehsilID"] == null || (int)ViewState["TehsilID"] == 0)
                {
                    try
                    {
                        objmaster.error = "";
                        objmaster.InsertTehsilInfo();
                        if (objmaster.error == "")
                        {

                            databind();
                            ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                            blankinserttext();
                            blanksertext();

                        }
                        else
                        {
                            ucMessage1.ShowInfo(objmaster.error);
                        }
                    }
                    catch (Exception ex)
                    {
                        ucMessage1.ShowInfo(ex.Message.ToString());
                        PageBase.Errorhandling(ex);
                    }
                }
                else
                {
                    try
                    {

                        objmaster.error = "";
                        objmaster.tehsillid = (int)ViewState["TehsilID"];
                        objmaster.UpdateTehsilInfo();
                        if (objmaster.error == "")
                        {
                            databind();
                            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                            blankinserttext();
                            ViewState["TehsilID"] = null;
                        }
                        else
                        {
                            ucMessage1.ShowInfo(objmaster.error);
                        }
                    }
                    catch (Exception ex)
                    {
                        ucMessage1.ShowInfo(ex.Message.ToString());
                        PageBase.Errorhandling(ex);
                    }

                }
            }
        }
    }


    protected void btncancel_Click(object sender, EventArgs e)
    {

        blankinserttext();
        blanksertext();
        databind();
        ucMessage1.Visible = false;
    }


    protected void btnSerchTeshsill_Click(object sender, EventArgs e)
    {
        blankinserttext();
        if (txtSerCode.Text == "" && txtSerName.Text == "" && cmbSerState.SelectedValue == "0" && cmbSerCountry.SelectedValue == "0" &&
            cmbSerDistrict.SelectedValue == "0" /*#CC02 ADDED START*/ && cmbSerCity.SelectedValue == "0" /*#CC02 ADDED END*/)
        {
            ucMessage1.ShowInfo("Please enter atleast one searching parameter");
            return;
        }

        databind();
    }
    protected void grdTehsill_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {

            if (e.CommandName == "Active")
            {
                try
                {
                    ucMessage1.Visible = false;
                    blankinserttext();
                    objmaster.tehsillid = Convert.ToInt32(e.CommandArgument);
                    objmaster.tehsillselectionmode = 2;
                    tehsillinfo = objmaster.SelectTahsillInfo();
                    objmaster.tehsillname = Convert.ToString(tehsillinfo.Rows[0]["TehsilName"]);
                    objmaster.tehsillcode = Convert.ToString(tehsillinfo.Rows[0]["TehsilCode"]);
                    objmaster.tehsillstateid = Convert.ToInt16(tehsillinfo.Rows[0]["StateId"]);
                    objmaster.tehsilldistrictid = Convert.ToInt16(tehsillinfo.Rows[0]["DistrictId"]);
                    objmaster.tehsilCityId = Convert.ToInt16(tehsillinfo.Rows[0]["CityId"]);/*#CC02 ADDED*/
                    objmaster.tehsillstatus = Convert.ToInt16(tehsillinfo.Rows[0]["Status"]);
                    if (objmaster.tehsillstatus == 1)
                    {
                        objmaster.tehsillstatus = 0;
                    }
                    else
                    {
                        objmaster.tehsillstatus = 1;
                    }
                    objmaster.error = "";
                    objmaster.UpdateTehsilInfo();

                    if (objmaster.error == "")
                    {
                        databind();
                        ucMessage1.ShowSuccess(Resources.Messages.StatusChanged);
                    }
                    else
                    {
                        ucMessage1.ShowInfo(objmaster.error);
                    }
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }
            }

            if (e.CommandName == "cmdEdit")
            {
                try
                {
                    ucMessage1.Visible = false;

                    objmaster.tehsillid = Convert.ToInt32(e.CommandArgument);
                    ViewState["TehsilID"] = objmaster.tehsillid;
                    objmaster.tehsillselectionmode = 2;
                    tehsillinfo = objmaster.SelectTahsillInfo();
                    txtInsertName.Text = Convert.ToString(tehsillinfo.Rows[0]["TehsilName"]);
                    txtInsertCode.Text = Convert.ToString(tehsillinfo.Rows[0]["TehsilCode"]);
                    chkstatus.Checked = Convert.ToBoolean(tehsillinfo.Rows[0]["Status"].ToString());
                    cmbinsCountry.SelectedValue = Convert.ToString(tehsillinfo.Rows[0]["CountryID"]);
                    cmbInsertState.ClearSelection();
                    cmbInsertCountry_SelectedIndexChanged(cmbinsCountry, new EventArgs());
                    cmbInsertState.SelectedValue = Convert.ToString(tehsillinfo.Rows[0]["StateId"]);
                    cmbInsertDistrict.ClearSelection();
                    cmbInsertState_SelectedIndexChanged(cmbInsertState, new EventArgs());
                    if (cmbInsertDistrict.Items.FindByValue(tehsillinfo.Rows[0]["DistrictID"].ToString()) != null)
                    {
                        cmbInsertDistrict.Items.FindByValue(tehsillinfo.Rows[0]["DistrictID"].ToString()).Selected = true;
                    }
                    else
                    {
                        cmbInsertDistrict.SelectedValue = "0";
                    }
                    /*#CC02 ADDED START*/
                    cmbInsertDist_SelectedIndexChanged(cmbInsertDistrict, new EventArgs());
                    if (cmbInsertCity.Items.FindByValue(tehsillinfo.Rows[0]["CityID"].ToString()) != null)
                    {
                        cmbInsertCity.Items.FindByValue(tehsillinfo.Rows[0]["CityID"].ToString()).Selected = true;
                    }
                    else
                    {
                        cmbInsertCity.SelectedValue = "0";
                    }
                    /*#CC02 ADDED END*/
                    btnSubmit.Text = "Update";
                    updAddUserMain.Update();
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowInfo(ex.Message.ToString());
                    PageBase.Errorhandling(ex);
                }
            }
        }
    }


    protected void btngetalldata_Click(object sender, EventArgs e)
    {
        blankinserttext();
        blanksertext();
        databind();
        ucMessage1.Visible = false;
    }
    protected void grdTehsill_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdTehsill.PageIndex = e.NewPageIndex;
        databind();
    }

    # endregion

    #region combobox index functions


    protected void cmbInsertState_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {

            try
            {
                if (cmbInsertState.SelectedValue == "0")
                {
                    cmbInsertDistrict.Items.Clear();
                    cmbInsertDistrict.Items.Insert(0, new ListItem("Select", "0"));

                }
                else
                {
                    cmbInsertDistrict.Items.Clear();
                    objmaster.DistrictStateId = Convert.ToInt16(cmbInsertState.SelectedValue.ToString());
                    objmaster.DistrictSelectionMode = 1;
                    DataTable dtdistfil = objmaster.SelectDistrictInfo();
                    cmbInsertDistrict.DataSource = dtdistfil;
                    cmbInsertDistrict.DataTextField = "DistrictName";
                    cmbInsertDistrict.DataValueField = "DistrictID";
                    cmbInsertDistrict.DataBind();
                    cmbInsertDistrict.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }

    protected void cmbserstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {
            try
            {
                if (cmbSerState.SelectedValue == "0")
                {
                    cmbSerDistrict.Items.Clear();
                    cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    cmbSerDistrict.ClearSelection();
                    objmaster.DistrictStateId = Convert.ToInt16(cmbSerState.SelectedValue.ToString());
                    objmaster.DistrictSelectionMode = 1;
                    DataTable dtdistfil = objmaster.SelectDistrictInfo();
                    cmbSerDistrict.DataSource = dtdistfil;
                    cmbSerDistrict.DataTextField = "DistrictName";
                    cmbSerDistrict.DataValueField = "DistrictID";
                    cmbSerDistrict.DataBind();
                    cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }

    #endregion

    #region export to excel
    protected void exportToExel_Click(object sender, EventArgs e)
    {
        databind();
        DataTable dt = tehsillserch.Copy();
        string[] DsCol = new string[] { "TehsilCode", "TehsilName",/*#CC02 ADDED START*/ "CityName" /*#CC02 ADDED START*/, "DistrictName", "StateName", "Countryname", "CurrentStatus" };
        DataTable DsCopy = new DataTable();
        dt = dt.DefaultView.ToTable(true, DsCol);
        dt.Columns["CurrentStatus"].ColumnName = "Status";

        if (dt.Rows.Count > 0)
        {
            try
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "TehsilDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else
        {
            ucMessage1.ShowError(Resources.Messages.NoRecord);
        }
    }

    #endregion
    /*#CC02 ADDED START*/
    protected void cmbInsertDist_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {

            try
            {
                if (cmbInsertDistrict.SelectedValue == "0")
                {
                    cmbInsertCity.Items.Clear();
                    cmbInsertCity.Items.Insert(0, new ListItem("Select", "0"));

                }
                else
                {

                    objmaster.CityDistrictId = Convert.ToInt16(cmbInsertDistrict.SelectedValue.ToString());
                    objmaster.CitySelectionMode = 1;
                    DataTable dtcityfil = objmaster.SelectCityInfo();
                    String[] colArray = { "CityID", "CityName" };
                    PageBase.DropdownBinding(ref cmbInsertCity, dtcityfil, colArray);
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }

    }

    protected void cmbSerdist_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {

            try
            {
                if (cmbSerDistrict.SelectedValue == "0")
                {
                    cmbSerCity.Items.Clear();
                    cmbSerCity.Items.Insert(0, new ListItem("Select", "0"));

                }

                else
                {
                    objmaster.CityDistrictId = Convert.ToInt16(cmbSerDistrict.SelectedValue);

                    objmaster.CitySelectionMode = 1;

                    DataTable dtcityfil = objmaster.SelectCityInfo();
                    String[] colArray = { "CityID", "CityName" };
                    PageBase.DropdownBinding(ref cmbSerCity, dtcityfil, colArray);
                    cmbSerCity.SelectedValue = "0";

                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    /*#CC02 ADDED END*/
    protected void cmbInsertCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        cmbInsertDistrict.Items.Clear();
        cmbInsertDistrict.Items.Insert(0, new ListItem("Select", "0"));
        /*#CC02 ADDED START*/
        cmbInsertCity.Items.Clear();
        cmbInsertCity.Items.Insert(0, new ListItem("Select", "0"));
        /*#CC02 ADDED END*/
        if (cmbinsCountry.SelectedValue == "0")
        {
            cmbInsertState.Items.Clear();
            cmbInsertState.Items.Insert(0, new ListItem("Select", "0"));

        }
        else
        {
            cmbInsertState.Items.Clear();
            using (MastersData obj = new MastersData())
            {
                DataTable dt;
                obj.StateSelectionMode = 1;
                obj.StateCountryid = Convert.ToInt32(cmbinsCountry.SelectedValue);
                dt = obj.SelectStateInfo();
                String[] colArray = { "StateID", "StateName" };
                PageBase.DropdownBinding(ref cmbInsertState, dt, colArray);
            }
        }
    }
    protected void cmbserCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        cmbSerDistrict.Items.Clear();
        cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));
        /*#CC02 ADDED START*/
        cmbSerCity.Items.Clear();
        cmbSerCity.Items.Insert(0, new ListItem("Select", "0"));
        /*#CC02 ADDED END*/
        if (cmbSerCountry.SelectedValue == "0")
        {
            cmbSerState.Items.Clear();
            cmbSerState.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            cmbSerState.Items.Clear();
            using (MastersData obj = new MastersData())
            {
                DataTable dt;
                obj.StateSelectionMode = 1;
                obj.StateCountryid = Convert.ToInt32(cmbSerCountry.SelectedValue);
                dt = obj.SelectStateInfo();
                String[] colArray = { "StateID", "StateName" };
                PageBase.DropdownBinding(ref cmbSerState, dt, colArray);
            }
        }
    }
}





