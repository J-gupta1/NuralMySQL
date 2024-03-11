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
 * 20 Mar 2016 , Karam Chand Sharma, #CC01, Insert / Update tehsil detail according to configuration  
 * 23-Mar-2016, Sumit Maurya, #CC02, On Show all button of search panel Country dropdown set to select option.
 * 25 May 2016, Karam Chand Sharma, #CC03, Remove tehsil due to cliect requirement 
 * 13-Sep-2016, Sumit Maurya, #CC04, Code connected with TempCode.
 * 14-Mar-2018, Sumit Maurya, #CC05, default button set as it was firing chnage status while pressing enter button.
 */
public partial class Masters_HO_Common_ManageCity : PageBase
{

    DataTable cityinfo;
    DataTable cityserch = new DataTable();



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Page.Form.DefaultButton = getalldata.UniqueID; /* #CC05 Added */  
            fillcountry();
            databind();
            chkstatus.Checked = true;
            /* #CC03 START COMMENTED /* #CC01 START ADDED
            cmbSerTehsil.Items.Clear();
            cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
            if (PageBase.TehsillDisplayMode == "1")
            {
                dvSerTehsil.Style.Add("display", "block");
                dvTehsil.Style.Add("display", "block");
                grdCity.Columns[3].Visible = true;
                cmbInsertDistrict.AutoPostBack = true;
                cmbSerDistrict.AutoPostBack = true;
            }
            else
            {
                dvSerTehsil.Style.Add("display", "none");
                dvTehsil.Style.Add("display", "none");
                grdCity.Columns[3].Visible = false;
                cmbInsertDistrict.AutoPostBack = false;
                cmbSerDistrict.AutoPostBack = false;
                RequiredFieldValidator2.ValidationGroup = "Nothing";
            }
            #CC01 START END   #CC03 END COMMENTED*/
            updgrid.Update();
            updAddUserMain.Update();
        }
    }

    # region user defined functions

    public void databind()
    {
        using (MastersData objmaster = new MastersData()) /* #CC04 Added */
        {
            ucMessage1.Visible = false;
            blankinserttext();
            objmaster.CityName = txtSerName.Text.Trim();
            objmaster.CityCode = txtSerCode.Text.Trim();
            objmaster.CityCountryId = Convert.ToInt32(cmbSerCountry.SelectedValue);
            if (cmbSerState.SelectedValue != "0")
            {
                objmaster.CityStateId = Convert.ToInt16(cmbSerState.SelectedValue.ToString());
            }
            else
            {
                objmaster.CityStateId = 0;
            }

            if (cmbSerDistrict.SelectedValue != "0")
            {
                objmaster.CityDistrictId = Convert.ToInt16(cmbSerDistrict.SelectedValue.ToString());

            }
            else
            {
                objmaster.CityDistrictId = 0;
            }
            /*#CC03 START COMMENTED
            /* #CC01 START ADDED
            if (cmbSerTehsil.SelectedValue != "0")
            {
                objmaster.tehsillid = Convert.ToInt16(cmbSerTehsil.SelectedValue.ToString());

            }
            else
            {
                objmaster.tehsillid = 0;
            }
            #CC01 START END #CC03 END COMMENTED*/
            objmaster.CitySelectionMode = 2;

            try
            {
                cityserch = objmaster.SelectCityInfo();
                grdCity.DataSource = cityserch;
                grdCity.DataBind();
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
        /*using (MastersData objmaster = new MastersData()) #CC04 Commented */
        using (MastersData objmaster = new MastersData()) /* #CC04 Added */
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
                /*#CC03 START COMMENTED
                /* #CC01 START ADDED
                cmbSerTehsil.Items.Clear();
                cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
                /* #CC01 START END  #CC03 END COMMENTED*/

                UpdSearch.Update();
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
        cmbSerCountry.SelectedValue = "0";  /* #CC02 Added */
        cmbSerState.Items.Clear();
        cmbSerState.Items.Insert(0, new ListItem("Select", "0"));
        cmbSerDistrict.Items.Clear();
        cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));
        cmbSerDistrict.SelectedValue = "0";
        cmbSerState.ClearSelection();
        /*#CC03 START COMMENTED
        /* #CC01 START ADDED 
        cmbSerTehsil.Items.Clear();
        cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
        /* #CC01 START END   #CC03 END COMMENTED*/

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
        /*#CC03 START COMMENTED
        /* #CC01 START ADDED
        drpTehsil.Items.Clear();
        drpTehsil.Items.Insert(0, new ListItem("Select", "0"));
        #CC01 START END   #CC03 END COMMENTED*/

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
        /*#CC03 START COMMENTED
        /* #CC01 START ADDED
        if (PageBase.TehsillDisplayMode == "1")
        {
            if (drpTehsil.SelectedValue == "0")
            {
                ucMessage1.ShowInfo("Please select a Tehsil");
                return false;
            }
        }
         #CC01 START END   #CC03 END COMMENTED*/
        if (txtInsertCode.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert City Code");
            return false;
        }
        if (txtInsertName.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert City Name");
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
        /*using (MastersData objmaster = new MastersData()) #CC04 Commented */
        using (MastersData objmaster = new MastersData()) /* #CC04 Added */
        {
            updAddUserMain.Update();
            if (!insertvalidate())
            {
                return;
            }
            else
            {
                objmaster.CityName = txtInsertName.Text.Trim();
                objmaster.CityCode = txtInsertCode.Text.Trim();
                objmaster.CityStateId = Convert.ToInt16(cmbInsertState.SelectedValue.ToString());
                objmaster.CityDistrictId = Convert.ToInt16(cmbInsertDistrict.SelectedValue.ToString());
                /*#CC03 START COMMENTED
                /* #CC01 START ADDED 
                if (PageBase.TehsillDisplayMode == "1")
                    objmaster.tehsillid = Convert.ToInt16(drpTehsil.SelectedValue.ToString());
                else
                    objmaster.tehsillid = 0;
                 #CC01 START END   #CC03 END COMMENTED*/
                if (chkstatus.Checked == true)
                {
                    objmaster.CityStatus = 1;
                }
                else
                {
                    objmaster.CityStatus = 0;
                }
                if (ViewState["CityID"] == null || (int)ViewState["CityID"] == 0)
                {
                    try
                    {
                        objmaster.error = "";
                        objmaster.InsertCityInfo();
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
                        objmaster.CityId = (int)ViewState["CityID"];
                        objmaster.UpdateCityInfo();
                        if (objmaster.error == "")
                        {
                            databind();
                            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                            blankinserttext();
                            ViewState["CityID"] = null;


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


    protected void btnSerchCity_Click(object sender, EventArgs e)
    {
        blankinserttext();
        if (txtSerCode.Text == "" && txtSerName.Text == "" && cmbSerState.SelectedValue == "0" && cmbSerCountry.SelectedValue == "0" &&
            cmbSerDistrict.SelectedValue == "0" /* #CC01 START ADDED */&& drpTehsil.SelectedValue == "0" /* #CC01 START END */)
        {
            ucMessage1.ShowInfo("Please enter atleast one searching parameter");
            return;
        }

        databind();
    }
    protected void grdCity_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        /*using (MastersData objmaster = new MastersData()) #CC04 Commented */
        using (MastersData objmaster = new MastersData()) /* #CC04 Added */
        {

            if (e.CommandName == "Active")
            {
                try
                {
                    ucMessage1.Visible = false;
                    blankinserttext();
                    objmaster.CityId = Convert.ToInt32(e.CommandArgument);
                    objmaster.CitySelectionMode = 2;
                    cityinfo = objmaster.SelectCityInfo();
                    objmaster.CityName = Convert.ToString(cityinfo.Rows[0]["CityName"]);
                    objmaster.CityCode = Convert.ToString(cityinfo.Rows[0]["CityCode"]);
                    objmaster.CityStateId = Convert.ToInt16(cityinfo.Rows[0]["StateId"]);
                    objmaster.CityDistrictId = Convert.ToInt16(cityinfo.Rows[0]["DistrictId"]);
                    objmaster.CityStatus = Convert.ToInt16(cityinfo.Rows[0]["Status"]);
                    if (objmaster.CityStatus == 1)
                    {
                        objmaster.CityStatus = 0;
                    }
                    else
                    {
                        objmaster.CityStatus = 1;
                    }
                    objmaster.error = "";
                    objmaster.UpdateCityInfo();

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

                    objmaster.CityId = Convert.ToInt32(e.CommandArgument);
                    ViewState["CityID"] = objmaster.CityId;
                    objmaster.CitySelectionMode = 2;
                    cityinfo = objmaster.SelectCityInfo();
                    txtInsertName.Text = Convert.ToString(cityinfo.Rows[0]["CityName"]);
                    txtInsertCode.Text = Convert.ToString(cityinfo.Rows[0]["CityCode"]);
                    chkstatus.Checked = Convert.ToBoolean(cityinfo.Rows[0]["Status"].ToString());
                    cmbinsCountry.SelectedValue = Convert.ToString(cityinfo.Rows[0]["CountryID"]);
                    cmbInsertState.ClearSelection();
                    cmbInsertCountry_SelectedIndexChanged(cmbinsCountry, new EventArgs());
                    cmbInsertState.SelectedValue = Convert.ToString(cityinfo.Rows[0]["StateId"]);
                    cmbInsertDistrict.ClearSelection();
                    cmbInsertState_SelectedIndexChanged(cmbInsertState, new EventArgs());
                    if (cmbInsertDistrict.Items.FindByValue(cityinfo.Rows[0]["DistrictID"].ToString()) != null)
                    {
                        cmbInsertDistrict.Items.FindByValue(cityinfo.Rows[0]["DistrictID"].ToString()).Selected = true;

                    }

                    else
                    {
                        cmbInsertDistrict.SelectedValue = "0";
                    }
                    /*#CC03 START COMMENTED
                    cmbInsertDistrict_SelectedIndexChanged(null, null);
                    /* #CC01 START ADDED 
                    if (drpTehsil.Items.FindByValue(cityinfo.Rows[0]["TehsilID"].ToString()) != null)
                    {
                        drpTehsil.Items.FindByValue(cityinfo.Rows[0]["TehsilID"].ToString()).Selected = true;

                    }

                    else
                    {
                        drpTehsil.SelectedValue = "0";
                    }
                    #CC01 START END   #CC03 END COMMENTED*/
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
    protected void grdCity_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCity.PageIndex = e.NewPageIndex;
        databind();
    }

    # endregion

    #region combobox index functions


    protected void cmbInsertState_SelectedIndexChanged(object sender, EventArgs e)
    {
        /*using (MastersData objmaster = new MastersData()) #CC04 Commented */
        using (MastersData objmaster = new MastersData()) /* #CC04 Added */
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
                    updAddUserMain.Update();
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

        /*using (MastersData objmaster = new MastersData()) #CC04 Commented */
        using (MastersData objmaster = new MastersData()) /* #CC04 Added */
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

                    //cmbSerDistrict.SelectedValue = "0";
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
        //if (ViewState["Table"] != null)
        //{
        //DataTable dt = (DataTable)ViewState["Table"];
        databind();             //Pankaj Dhingra
        DataTable dt = cityserch.Copy();
        /*#CC03 START COMMENTED
        /* #CC01 START ADDED 
        if (PageBase.TehsillDisplayMode == "1")
        {
            string[] DsCol = new string[] { "CityCode", "CityName", "StateName", "DistrictName", "TehsilName", "Countryname", "CurrentStatus" };
            dt = dt.DefaultView.ToTable(true, DsCol);
        }
        else
        {
            string[] DsCol = new string[] { "CityCode", "CityName", "StateName", "DistrictName", "Countryname", "CurrentStatus" };
            dt = dt.DefaultView.ToTable(true, DsCol);
        }
        /* #CC01 START END */
        /* #CC01 START COMMENTED string[] DsCol = new string[] { "CityCode", "CityName", "StateName", "DistrictName", "Countryname", "CurrentStatus" };
        dt = dt.DefaultView.ToTable(true, DsCol); #CC01 END COMMENTED    #CC03 END COMMENTED*/

        /*#CC03 START ADDED */
        string[] DsCol = new string[] { "CityCode", "CityName", "StateName", "DistrictName", "Countryname", "CurrentStatus" };
        dt = dt.DefaultView.ToTable(true, DsCol);
        /*#CC03 END ADDED */
        DataTable DsCopy = new DataTable();

        dt.Columns["CurrentStatus"].ColumnName = "Status";

        if (dt.Rows.Count > 0)
        {
            try
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "CityDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
                // ViewState["Table"] = null;
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
        //ViewState["Table"] = null;
        //}

    }

    #endregion

    protected void cmbInsertCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        cmbInsertDistrict.Items.Clear();
        cmbInsertDistrict.Items.Insert(0, new ListItem("Select", "0"));
        /*#CC03 START COMMENTED 
        /*#CC01 START ADDED 
        drpTehsil.Items.Clear();
        drpTehsil.Items.Insert(0, new ListItem("Select", "0"));
        #CC01 START ADDED #CC03 END COMMENTED*/
        if (cmbinsCountry.SelectedValue == "0")
        {
            cmbInsertState.Items.Clear();
            cmbInsertState.Items.Insert(0, new ListItem("Select", "0"));

        }
        else
        {
            cmbInsertState.Items.Clear();
            /*using (MastersData obj = new MastersData()) #CC04 Commented */
            using (MastersData obj = new MastersData()) /* #CC04 Added */            
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
        /*#CC03 START COMMENTED
        /*#CC01 START ADDED
        cmbSerTehsil.Items.Clear();
        cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
        #CC01 START END  #CC03 END COMMENTED*/
        if (cmbSerCountry.SelectedValue == "0")
        {
            cmbSerState.Items.Clear();
            cmbSerState.Items.Insert(0, new ListItem("Select", "0"));

        }
        else
        {
            cmbSerState.Items.Clear();
            /*using (MastersData obj = new MastersData()) #CC04 Commented */
            using (MastersData obj = new MastersData()) /* #CC04 Added */
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
    /*#CC03 START COMMENTED
    /*#CC01 START ADDED 
    protected void cmbInsertDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {

            try
            {
                if (cmbInsertDistrict.SelectedValue == "0")
                {
                    drpTehsil.Items.Clear();
                    drpTehsil.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    drpTehsil.ClearSelection();
                    objmaster.tehsilldistrictid = Convert.ToInt16(cmbInsertDistrict.SelectedValue.ToString());
                    objmaster.tehsillselectionmode = 1;
                    DataTable dtdistfil = objmaster.SelectTahsillInfo();
                    drpTehsil.DataSource = dtdistfil;
                    drpTehsil.DataTextField = "TehsilName";
                    drpTehsil.DataValueField = "TehsilID";
                    drpTehsil.DataBind();
                    drpTehsil.Items.Insert(0, new ListItem("Select", "0"));
                }

            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    protected void cmbSerDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {
            try
            {
                if (cmbSerDistrict.SelectedValue == "0")
                {
                    drpTehsil.Items.Clear();
                    drpTehsil.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    drpTehsil.ClearSelection();
                    objmaster.tehsilldistrictid = Convert.ToInt16(cmbSerDistrict.SelectedValue.ToString());
                    objmaster.tehsillselectionmode = 1;
                    DataTable dtTehsil = objmaster.SelectTahsillInfo();
                    cmbSerTehsil.DataSource = dtTehsil;
                    cmbSerTehsil.DataTextField = "TehsilName";
                    cmbSerTehsil.DataValueField = "TehsilID";
                    cmbSerTehsil.DataBind();
                    cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    /*#CC01 START END   #CC03 END COMMENTED*/


}





