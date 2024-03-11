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
 * 21 Mar 2016, Karam Chand Sharma, #CC01, Implement tehshil and it will handled through application configuration
 * 28 Mar 2016, Sumit Maurya, #CC02, Information message gets displed if user click on Edit button and tehsil doesnot exists for the record.
 * 26 May 2016, Karam Chand Sharma, #CC03, Apply tehsil changes according to client requirement and now tehsil load according to city but tehsil will show or not accoring to application configuration in 
 * 14-Mar-2018, Sumit Maurya, #CC04, default button set as it was firing chnage status while pressing enter button.
 */
public partial class Masters_HO_Common_ManageArea : PageBase
{
    DataTable areainfo;
    DataTable areaserch = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Page.Form.DefaultButton = getalldata.UniqueID; /* #CC04 Added */  
            /* #CC01 START ADDED */
            cmbSerTehsil.Items.Clear();
            cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
            /* #CC01 END ADDED */
            fillcountry();
            databind();
            chkstatus.Checked = true;
            /* #CC01 START ADDED */
            if (PageBase.TehsillDisplayMode == "1")
            {
                dvSerTehsil.Style.Add("display", "block");
                dvTehsil.Style.Add("display", "block");
                /*#CC03 START COMMENTED grdArea.Columns[3].Visible = true; #CC03 END COMMENTED */
                grdArea.Columns[4].Visible = true;/*#CC03 ADDED*/
                //cmbInsertDistrict.AutoPostBack = true;
                //cmbSerDistrict.AutoPostBack = true;
            }
            else
            {
                dvSerTehsil.Style.Add("display", "none");
                dvTehsil.Style.Add("display", "none");
                /*#CC03 START COMMENTED grdArea.Columns[3].Visible = true; #CC03 END COMMENTED */
                grdArea.Columns[4].Visible = false;/*#CC03 ADDED*/
                //cmbInsertDistrict.AutoPostBack = false;
                //cmbSerDistrict.AutoPostBack = false;
            }
            /* #CC01 END ADDED */
        }
    }
    # region user defined functions

    public void databind()
    {
        using (MastersData objmaster = new MastersData())
        {

            ucMessage1.Visible = false;
            blankinserttext();
            objmaster.AreaName = txtSerName.Text.Trim();
            objmaster.AreaCode = txtSerCode.Text.Trim();
            objmaster.AreaCountryId = Convert.ToInt32(cmbSerCountry.SelectedValue);
            if (cmbSerState.SelectedValue != "0")
            {
                objmaster.AreaStateId = Convert.ToInt16(cmbSerState.SelectedValue.ToString());
            }
            else
            {
                objmaster.AreaStateId = 0;
            }

            if (cmbSerDistrict.SelectedValue != "0")
            {
                objmaster.AreaDistrictId = Convert.ToInt16(cmbSerDistrict.SelectedValue.ToString());

            }
            else
            {
                objmaster.AreaDistrictId = 0;
            }
            if (cmbSerCity.SelectedValue != "0")
            {
                objmaster.AreaCityId = Convert.ToInt16(cmbSerCity.SelectedValue.ToString());

            }
            else
            {
                objmaster.AreaCityId = 0;
            }
            /* #CC01 START ADDED */
            if (cmbSerTehsil.SelectedValue != "0")
            {
                objmaster.tehsillid = Convert.ToInt16(cmbSerTehsil.SelectedValue.ToString());

            }
            else
            {
                objmaster.tehsillid = 0;
            }
            /* #CC01 END ADDED */

            objmaster.AreaSelectionMode = 2;

            try
            {
                areaserch = objmaster.SelectAreaInfo();


                //ViewState["Table"] = areaserch;         //Pankaj Dhingra
                grdArea.DataSource = areaserch;
                grdArea.DataBind();
                blankinserttext();
                updgrid.Update();


            }
            catch (Exception ex)
            {

                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
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
        /* #CC01 START ADDED */
        if (PageBase.TehsillDisplayMode == "1")
        {
            if (drpTehsil.SelectedValue == "0")
            {
                ucMessage1.ShowInfo("Please select a Tehsil");
                return false;
            }
        }
        /* #CC01 END ADDED */
        if (cmbInsertCity.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please select a City");
            return false;
        }
        if (txtInsertCode.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert Area Code");
            return false;
        }
        if (txtInsertName.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert Area Name");
            return false;
        }
        return true;

    }

    void fillcountry()
    {
        using (MastersData obj = new MastersData())
        {
            DataTable dt;
            cmbInsCountry.Items.Clear();
            obj.CountrySelectionMode = 1;
            dt = obj.SelectCountryInfo();
            String[] colArray = { "CountryID", "CountryName" };
            PageBase.DropdownBinding(ref cmbInsCountry, dt, colArray);
            PageBase.DropdownBinding(ref cmbSerCountry, dt, colArray);
            //cmbInsertState.Items.Insert(0,new ListItem("Select","0"));
            cmbSerState.Items.Insert(0, new ListItem("Select", "0"));
            cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));
            cmbSerCity.Items.Insert(0, new ListItem("Select", "0"));

        }

    }


    public void blanksertext()
    {
        txtSerName.Text = "";
        txtSerCode.Text = "";
        cmbSerCountry.SelectedValue = "0";
        cmbSerState.Items.Clear();
        cmbSerState.Items.Insert(0, new ListItem("Select", "0"));
        cmbSerCity.Items.Clear();
        cmbSerCity.Items.Insert(0, new ListItem("Select", "0"));
        cmbSerDistrict.Items.Clear();
        cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));
        cmbSerDistrict.SelectedValue = "0";
        cmbSerState.ClearSelection();
        /* #CC01 START ADDED */
        cmbSerTehsil.Items.Clear();
        cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
        /* #CC01 END ADDED */
        UpdSearch.Update();

    }
    public void blankinserttext()
    {
        txtInsertCode.Text = "";
        txtInsertName.Text = "";
        cmbInsCountry.SelectedValue = "0";
        cmbInsertState.Items.Clear();
        cmbInsertState.Items.Insert(0, new ListItem("Select", "0"));
        cmbInsertCity.Items.Clear();
        cmbInsertCity.Items.Insert(0, new ListItem("Select", "0"));
        cmbInsertDistrict.Items.Clear();
        cmbInsertDistrict.Items.Insert(0, new ListItem("Select", "0"));
        /* #CC01 START ADDED */
        drpTehsil.Items.Clear();
        drpTehsil.Items.Insert(0, new ListItem("Select", "0"));
        /* #CC01 END ADDED */
        btnSubmit.Text = "Submit";
        updAddUserMain.Update();

    }


    # endregion

    #region control functions

    protected void btninsert_click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }   //Pankaj Kumar

        using (MastersData objmaster = new MastersData())
        {
            updAddUserMain.Update();
            if (!insertvalidate())
            {
                return;
            }
            else
            {
                objmaster.AreaName = txtInsertName.Text.Trim();
                objmaster.AreaCode = txtInsertCode.Text.Trim();
                objmaster.AreaStateId = Convert.ToInt16(cmbInsertState.SelectedValue.ToString());
                objmaster.AreaCityId = Convert.ToInt16(cmbInsertCity.SelectedValue.ToString());
                objmaster.tehsillid = Convert.ToInt16(drpTehsil.SelectedValue.ToString());/*#CC03 ADDED*/
                if (chkstatus.Checked == true)
                {
                    objmaster.AreaStatus = 1;
                }
                else
                {
                    objmaster.AreaStatus = 0;
                }
                if (ViewState["AreaID"] == null || (int)ViewState["AreaID"] == 0)
                {
                    try
                    {
                        objmaster.error = "";
                        objmaster.InsertAreaInfo();
                        if (objmaster.error == "")
                        {
                            databind();
                            ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                            blankinserttext();

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
                        objmaster.AreaId = (int)ViewState["AreaID"];

                        objmaster.UpdateAreaInfo();
                        if (objmaster.error == "")
                        {
                            databind();
                            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                            ViewState["AreaID"] = null;
                            blankinserttext();

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

    }




    protected void btnSerchArea_Click(object sender, EventArgs e)
    {
        blankinserttext();
        if (txtSerCode.Text == "" && txtSerName.Text == "" && cmbSerState.SelectedValue == "0" && cmbSerCountry.SelectedValue == "0")
        {
            /* #CC01 START ADDED */
            if (PageBase.TehsillDisplayMode == "1" && cmbSerTehsil.SelectedValue == "0")
            {
                ucMessage1.ShowInfo("Please enter atleast one searching parameter ");
                return;
            }
            /* #CC01 END ADDED */
            ucMessage1.ShowInfo("Please enter atleast one searching parameter ");
            return;
        }
        databind();

    }

    protected void grdArea_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        using (MastersData objmaster = new MastersData())
        {

            if (e.CommandName == "Active")
            {
                try
                {
                    ucMessage1.Visible = false;
                    blankinserttext();


                    objmaster.AreaId = Convert.ToInt32(e.CommandArgument);
                    objmaster.AreaSelectionMode = 2;
                    areainfo = objmaster.SelectAreaInfo();
                    objmaster.AreaName = Convert.ToString(areainfo.Rows[0]["AreaName"]);
                    objmaster.AreaCode = Convert.ToString(areainfo.Rows[0]["AreaCode"]);
                    objmaster.AreaStateId = Convert.ToInt16(areainfo.Rows[0]["StateId"]);
                    objmaster.AreaCityId = Convert.ToInt16(areainfo.Rows[0]["CityId"]);
                    objmaster.AreaStatus = Convert.ToInt16(areainfo.Rows[0]["Status"]);

                    if (objmaster.AreaStatus == 1)
                    {
                        objmaster.AreaStatus = 0;
                    }
                    else
                    {
                        objmaster.AreaStatus = 1;
                    }
                    /* #CC03 START ADDED */
                    if (PageBase.TehsillDisplayMode == "1")
                    {
                        objmaster.tehsillid = Convert.ToInt16(areainfo.Rows[0]["TehsilID"].ToString());
                    }

                    /* #CC03 START END */
                    objmaster.error = "";
                    objmaster.UpdateAreaInfo();

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
                    objmaster.AreaId = Convert.ToInt32(e.CommandArgument);
                    ViewState["AreaID"] = objmaster.AreaId;
                    objmaster.AreaSelectionMode = 2;
                    areainfo = objmaster.SelectAreaInfo();
                    txtInsertName.Text = Convert.ToString(areainfo.Rows[0]["AreaName"]);
                    txtInsertCode.Text = Convert.ToString(areainfo.Rows[0]["AreaCode"]);
                    chkstatus.Checked = Convert.ToBoolean(areainfo.Rows[0]["Status"].ToString());
                    cmbInsCountry.SelectedValue = (areainfo.Rows[0]["CountryID"].ToString());
                    cmbInsertState.ClearSelection();
                    cmbInsertCountry_SelectedIndexChanged(cmbInsCountry, new EventArgs());
                    cmbInsertState.SelectedValue = areainfo.Rows[0]["StateID"].ToString();
                    cmbInsertDistrict.Items.Clear();
                    cmbInsertState_SelectedIndexChanged(cmbInsertState, new EventArgs());

                    if (cmbInsertDistrict.Items.FindByValue(areainfo.Rows[0]["DistrictID"].ToString()) != null)
                    {

                        cmbInsertDistrict.SelectedValue = areainfo.Rows[0]["DistrictID"].ToString();
                    }
                    else
                    {

                        cmbInsertDistrict.SelectedValue = "0";
                    }
                    /*#CC03 START COMMENTED*/
                    ///* #CC01 START ADDED */
                    //if (PageBase.TehsillDisplayMode == "1")
                    //{
                    //    cmbInsertDist_SelectedIndexChanged(null, null);
                    //}
                    //if (drpTehsil.Items.FindByValue(areainfo.Rows[0]["TehsilID"].ToString()) != null && areainfo.Rows[0]["TehsilID"].ToString() != "0") /* #CC02 Added areainfo.Rows[0]["TehsilID"].ToString() != "0" */
                    //{

                    //    drpTehsil.SelectedValue = areainfo.Rows[0]["TehsilID"].ToString();
                    //}
                    //else
                    //{
                    //    /* #CC02 Add Start */
                    //    if (PageBase.TehsillDisplayMode == "1")
                    //    {
                    //        ucMessage1.ShowInfo("Tehsil doesnot exist for the record, Please create or edit Tehsil.");
                    //    }

                    //    /* #CC02 Add End */
                    //    // drpTehsil.SelectedValue = "0"; /* #CC02 Comment */
                    //}


                    /* #CC01 END ADDED */
                    /*#CC03 END COMMENTED*/
                    cmbInsertCity.Items.Clear();
                    cmbInsertDist_SelectedIndexChanged(cmbInsertDistrict, new EventArgs());
                    /*#CC03 START COMMENTED */
                    ///* #CC01 START ADDED */
                    //if (PageBase.TehsillDisplayMode == "1")
                    //{
                    //    drpTehsil_SelectedIndexChanged(null, null);
                    //}
                    ///* #CC01 END ADDED */
                    ////*#CC03 END COMMENTED */
                    // cmbInsertCity.Items.Insert(0, new ListItem("select", "0"));
                    if (cmbInsertCity.Items.FindByValue(areainfo.Rows[0]["CityID"].ToString()) != null)
                    {

                        cmbInsertCity.SelectedValue = areainfo.Rows[0]["CityID"].ToString();
                    }
                    else
                    {
                        cmbInsertCity.SelectedValue = "0";
                    }
                  
                    /* #CC03 START ADDED */
                    if (PageBase.TehsillDisplayMode == "1")
                    {
                        cmbInsertCity_SelectedIndexChanged(null, null);
                    }
                    if (drpTehsil.Items.FindByValue(areainfo.Rows[0]["TehsilID"].ToString()) != null && areainfo.Rows[0]["TehsilID"].ToString() != "0")
                    {

                        drpTehsil.SelectedValue = areainfo.Rows[0]["TehsilID"].ToString();
                    }
                    else
                    {
                        if (PageBase.TehsillDisplayMode == "1")
                        {
                            ucMessage1.ShowInfo("Tehsil doesnot exist for the record, Please create or edit Tehsil.");
                        }
                    }

                    /* #CC03 START END */

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


    protected void grdArea_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdArea.PageIndex = e.NewPageIndex;
        databind();
    }
    #endregion


    # region combobox index changed

    protected void cmbInsertState_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {

            cmbInsertCity.Items.Clear();
            cmbInsertCity.Items.Insert(0, new ListItem("Select", "0"));

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
                    /////////cmbInsertCity.SelectedValue = "0";
                    objmaster.DistrictStateId = Convert.ToInt16(cmbInsertState.SelectedValue.ToString());
                    objmaster.DistrictSelectionMode = 1;
                    DataTable dtdistfil = objmaster.SelectDistrictInfo();
                    String[] colArray = { "DistrictID", "DistrictName" };
                    PageBase.DropdownBinding(ref cmbInsertDistrict, dtdistfil, colArray);
                    //cmbInsertDistrict.SelectedValue = "0";
                }

            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }

        }
    }
    protected void cmbSerState_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {
            cmbSerTehsil.Items.Clear();
            cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
            cmbSerCity.Items.Clear();
            cmbSerCity.Items.Insert(0, new ListItem("Select", "0"));
            try
            {
                if (cmbSerState.SelectedValue == "0")
                {
                    cmbSerDistrict.Items.Clear();

                    cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));


                }
                else
                {

                    cmbSerDistrict.Items.Clear();
                    objmaster.DistrictStateId = Convert.ToInt16(cmbSerState.SelectedValue.ToString());
                    objmaster.DistrictSelectionMode = 1;
                    DataTable dtdistfil = objmaster.SelectDistrictInfo();
                    String[] colArray = { "DistrictID", "DistrictName" };
                    PageBase.DropdownBinding(ref cmbSerDistrict, dtdistfil, colArray);
                    cmbSerDistrict.SelectedValue = "0";
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
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
                    /*#CC03 START COMMENTED*/
                    ///* #CC01 START ADDED */
                    //if (PageBase.TehsillDisplayMode == "1")
                    //{
                    //    if (cmbInsertDistrict.SelectedValue == "0")
                    //    {
                    //        drpTehsil.Items.Clear();
                    //        drpTehsil.Items.Insert(0, new ListItem("Select", "0"));
                    //    }
                    //    else
                    //    {
                    //        drpTehsil.ClearSelection();
                    //        objmaster.tehsilldistrictid = Convert.ToInt16(cmbInsertDistrict.SelectedValue.ToString());
                    //        objmaster.tehsillselectionmode = 1;
                    //        DataTable dtdistfil = objmaster.SelectTahsillInfo();
                    //        drpTehsil.DataSource = dtdistfil;
                    //        drpTehsil.DataTextField = "TehsilName";
                    //        drpTehsil.DataValueField = "TehsilID";
                    //        drpTehsil.DataBind();
                    //        drpTehsil.Items.Insert(0, new ListItem("Select", "0"));


                    //    }
                    //}
                    //else
                    //{/* #CC01 END ADDED */

                    //    objmaster.CityDistrictId = Convert.ToInt16(cmbInsertDistrict.SelectedValue.ToString());
                    //    objmaster.CitySelectionMode = 1;
                    //    DataTable dtcityfil = objmaster.SelectCityInfo();
                    //    String[] colArray = { "CityID", "CityName" };
                    //    PageBase.DropdownBinding(ref cmbInsertCity, dtcityfil, colArray);
                    //}
                    /*#CC03 END COMMENTED*/
                    /*#CC03 START ADDED*/
                    objmaster.CityDistrictId = Convert.ToInt16(cmbInsertDistrict.SelectedValue.ToString());
                    objmaster.CitySelectionMode = 1;
                    DataTable dtcityfil = objmaster.SelectCityInfo();
                    String[] colArray = { "CityID", "CityName" };
                    PageBase.DropdownBinding(ref cmbInsertCity, dtcityfil, colArray);
                    /*#CC03 START END*/
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
                    /*#CC03 START COMMENTED*/
                    ///*#CC01 START ADDED*/
                    //if (PageBase.TehsillDisplayMode == "1")
                    //{
                    //    if (cmbSerDistrict.SelectedValue == "0")
                    //    {
                    //        cmbSerTehsil.Items.Clear();
                    //        cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
                    //    }
                    //    else
                    //    {
                    //        cmbSerTehsil.ClearSelection();
                    //        objmaster.tehsilldistrictid = Convert.ToInt16(cmbSerDistrict.SelectedValue.ToString());
                    //        objmaster.tehsillselectionmode = 1;
                    //        DataTable dtdistfil = objmaster.SelectTahsillInfo();
                    //        cmbSerTehsil.DataSource = dtdistfil;
                    //        cmbSerTehsil.DataTextField = "TehsilName";
                    //        cmbSerTehsil.DataValueField = "TehsilID";
                    //        cmbSerTehsil.DataBind();
                    //        cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));


                    //    }
                    //}
                    //else
                    //{/*#CC01 END ADDED*/
                    //    objmaster.CityDistrictId = Convert.ToInt16(cmbSerDistrict.SelectedValue);

                    //    objmaster.CitySelectionMode = 1;

                    //    DataTable dtcityfil = objmaster.SelectCityInfo();
                    //    String[] colArray = { "CityID", "CityName" };
                    //    PageBase.DropdownBinding(ref cmbSerCity, dtcityfil, colArray);
                    //    cmbSerCity.SelectedValue = "0";

                    //}                    
                    /*#CC03 END COMMENTED*/
                    /*#CC03 START ADDED*/
                    objmaster.CityDistrictId = Convert.ToInt16(cmbSerDistrict.SelectedValue);
                    objmaster.CitySelectionMode = 1;
                    DataTable dtcityfil = objmaster.SelectCityInfo();
                    String[] colArray = { "CityID", "CityName" };
                    PageBase.DropdownBinding(ref cmbSerCity, dtcityfil, colArray);
                    cmbSerCity.SelectedValue = "0";
                    /*#CC03 START END*/
                }

            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }

    # endregion


    # region export to excel

    protected void exportToExel_Click(object sender, EventArgs e)
    {

        databind();                         //Pankaj Dhingra
        //if (ViewState["Table"] != null)
        //{
        //DataTable dt = (DataTable)ViewState["Table"];
        DataTable dt = areaserch.Copy();
        /*#CC01 START COMMENTED  string[] DsCol = new string[] { "AreaCode", "AreaName", "CityName", "StateName", "DistrictName", "CountryName", "CurrentStatus" };
        DataTable DsCopy = new DataTable(); #CC01 END COMMENTED */
        /*#CC01 START ADDED*/
        if (PageBase.TehsillDisplayMode == "1")
        {
            string[] DsCol = new string[] { "AreaCode", "AreaName", "CityName", "StateName", "DistrictName", "TehsilName", "CountryName", "CurrentStatus" };
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
        }
        else
        {
            string[] DsCol = new string[] { "AreaCode", "AreaName", "CityName", "StateName", "DistrictName", "CountryName", "CurrentStatus" };
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
        }
        /*#CC01 END ADDED*/
        dt.Columns["CurrentStatus"].ColumnName = "Status";

        if (dt.Rows.Count > 0)
        {
            try
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "AreaDetails";
                PageBase.RootFilePath = FilePath;
                PageBase.ExportToExecl(dtcopy, FilenameToexport);
                //ViewState["Table"] = null;
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
        else
        {
            ucMessage1.ShowError(Resources.Messages.NoRecord);

        }
        //ViewState["Table"] = null;
        //}
    }

    # endregion

    protected void cmbInsertCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        cmbInsertDistrict.Items.Clear();
        cmbInsertCity.Items.Clear();
        cmbInsertDistrict.Items.Insert(0, new ListItem("Select", "0"));
        cmbInsertCity.Items.Insert(0, new ListItem("Select", "0"));
        drpTehsil.Items.Clear();
        drpTehsil.Items.Insert(0, new ListItem("Select", "0"));
        if (cmbInsCountry.SelectedValue == "0")
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
                obj.StateCountryid = Convert.ToInt32(cmbInsCountry.SelectedValue);
                dt = obj.SelectStateInfo();
                String[] colArray = { "StateID", "StateName" };
                PageBase.DropdownBinding(ref cmbInsertState, dt, colArray);
            }
        }

    }
    protected void cmbSerCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        cmbSerDistrict.Items.Clear();
        cmbSerDistrict.Items.Insert(0, new ListItem("Select", "0"));
        cmbSerCity.Items.Clear();
        cmbSerCity.Items.Insert(0, new ListItem("Select", "0"));
        cmbSerTehsil.Items.Clear();
        cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
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

    /*#CC03 START COMMENTED*/
    ///*#CC01 START ADDED*/
    //protected void drpTehsil_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    using (MastersData objmaster = new MastersData())
    //    {

    //        try
    //        {
    //            if (drpTehsil.SelectedValue == "0")
    //            {
    //                cmbInsertCity.Items.Clear();
    //                cmbInsertCity.Items.Insert(0, new ListItem("Select", "0"));
    //            }
    //            else
    //            {
    //                /*
    //                 * #CC02 Comment Start 
    //                if (PageBase.TehsillDisplayMode == "1")
    //                {
    //                    drpTehsil.Items.Clear();
    //                    drpTehsil.Items.Insert(0, new ListItem("Select", "0"));
    //                }
    //                 * #CC02 Commente End
    //                 */
    //                objmaster.CityDistrictId = Convert.ToInt16(cmbInsertDistrict.SelectedValue.ToString());
    //                objmaster.CitySelectionMode = 1;
    //                objmaster.tehsillid = Convert.ToInt16((drpTehsil.SelectedValue.ToString() == "" ? "0" : drpTehsil.SelectedValue.ToString()));
    //                DataTable dtcityfil = objmaster.SelectCityInfoTehsilWise();
    //                String[] colArray = { "CityID", "CityName" };
    //                PageBase.DropdownBinding(ref cmbInsertCity, dtcityfil, colArray);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            ucMessage1.ShowInfo(ex.Message.ToString());
    //            PageBase.Errorhandling(ex);
    //        }
    //    }
    //}
    //protected void cmbSerTehsil_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    using (MastersData objmaster = new MastersData())
    //    {

    //        try
    //        {
    //            if (cmbSerDistrict.SelectedValue == "0")
    //            {
    //                cmbInsertCity.Items.Clear();
    //                cmbInsertCity.Items.Insert(0, new ListItem("Select", "0"));
    //            }
    //            else
    //            {
    //                objmaster.CityDistrictId = Convert.ToInt16(cmbSerDistrict.SelectedValue.ToString());
    //                objmaster.CitySelectionMode = 1;
    //                objmaster.tehsillid = Convert.ToInt16(cmbSerTehsil.SelectedValue.ToString());
    //                DataTable dtcityfil = objmaster.SelectCityInfoTehsilWise();
    //                String[] colArray = { "CityID", "CityName" };
    //                PageBase.DropdownBinding(ref cmbSerCity, dtcityfil, colArray);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            ucMessage1.ShowInfo(ex.Message.ToString());
    //            PageBase.Errorhandling(ex);
    //        }
    //    }
    //}
    ///*#CC01 END ADDED*/
    /// /*#CC03 END COMMENTED*/
    protected void cmbInsertCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {
            try
            {
                if (cmbInsertCity.SelectedValue == "0")
                {
                    drpTehsil.Items.Clear();
                    drpTehsil.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    if (PageBase.TehsillDisplayMode == "1")
                    {
                        drpTehsil.ClearSelection();
                        objmaster.tehsilCityId = Convert.ToInt16(cmbInsertCity.SelectedValue.ToString());
                        objmaster.tehsillselectionmode = 1;
                        DataTable dtdistfil = objmaster.SelectTahsillInfo();
                        drpTehsil.DataSource = dtdistfil;
                        drpTehsil.DataTextField = "TehsilName";
                        drpTehsil.DataValueField = "TehsilID";
                        drpTehsil.DataBind();
                        drpTehsil.Items.Insert(0, new ListItem("Select", "0"));
                    }
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }
        }
    }
    protected void cmbSerCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {
            try
            {
                if (cmbSerCity.SelectedValue == "0")
                {
                    cmbSerTehsil.Items.Clear();
                    cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    if (PageBase.TehsillDisplayMode == "1")
                    {
                        cmbSerTehsil.ClearSelection();
                        objmaster.tehsilCityId = Convert.ToInt16(cmbSerCity.SelectedValue.ToString());
                        objmaster.tehsillselectionmode = 1;
                        DataTable dtdistfil = objmaster.SelectTahsillInfo();
                        cmbSerTehsil.DataSource = dtdistfil;
                        cmbSerTehsil.DataTextField = "TehsilName";
                        cmbSerTehsil.DataValueField = "TehsilID";
                        cmbSerTehsil.DataBind();
                        cmbSerTehsil.Items.Insert(0, new ListItem("Select", "0"));
                    }
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
