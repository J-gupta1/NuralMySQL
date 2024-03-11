/*
====================================================================================================================================
 *Change Log: 
 *DD-MMM-YYYY, Name , #CCXX - Description
------------------------------------------------------------------------------------------------------------------------------------
 * 14-Mar-2018, Sumit Maurya, #CC01, default button set as it was firing chnage status while pressing enter button.
====================================================================================================================================
 */
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

public partial class Masters_HO_Common_ManageDistrict_ :PageBase
{

  
    DataTable districtinfo;
    DataTable districtserch = new DataTable();
    protected void Page_Load(object sender, EventArgs e)

    {
        if (!IsPostBack)
        {
            Page.Form.DefaultButton = getalldata.UniqueID; /* #CC01 Added */  
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
            objmaster.DistrictName = txtSerName.Text.Trim();
            objmaster.DistrictCode = txtSerCode.Text.Trim();
            objmaster.DistrictCountryId = Convert.ToInt32(cmbSerCountry.SelectedValue);
            if (cmbSerState.SelectedValue != "0")
            {
                objmaster.DistrictStateId = Convert.ToInt16(cmbSerState.SelectedValue.ToString());
            }
            else
            {
                objmaster.DistrictStateId = 0;
            }
            try
            {
                 objmaster.DistrictSelectionMode  = 2;
                 districtserch = objmaster.SelectDistrictInfo();
                 grdDistrict.DataSource = districtserch;
                 grdDistrict.DataBind();
                 updgrid.Update();
             }
            catch (Exception ex)
            {
                ucMessage1.ShowInfo(ex.Message.ToString());
                PageBase.Errorhandling(ex);
            }

        
        }
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

        }

    }

    public void blanksertext()
    {
        txtSerName.Text = "";
        cmbSerState.Items.Clear();
        txtSerCode.Text = "";
        cmbSerState.Items.Insert(0, new ListItem("Select", "0"));
        cmbSerCountry.SelectedValue = "0";
        UpdSearch.Update();
        cmbSerCountry.SelectedValue = "0"; 
    }
    public void blankinserttext()
    {

        txtInsertCode.Text = "";
        txtInsertName.Text = "";
        cmbInsertState.Items.Clear();
        cmbInsertState.Items.Insert(0, new ListItem("Select", "0"));
        cmbInsertState.SelectedValue = "0";
        cmbInsCountry.SelectedValue = "0";
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

        if (txtInsertCode.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert District Code");
            return false;
        }
        if (txtInsertName.Text == "")
        {
            ucMessage1.ShowInfo("Please Insert District Name");
            return false;
        }
        return true;
    }

    #endregion


    # region user control 

    protected void btninsert_click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
           return;
        }           //Pankaj Dhingra

        using (MastersData objmaster = new MastersData())
        {
            if (!insertvalidate())
            {
                return;
            }
            else
            {

                objmaster.DistrictName = txtInsertName.Text.Trim();
                objmaster.DistrictCode = txtInsertCode.Text.Trim();
                objmaster.DistrictStateId = Convert.ToInt16(cmbInsertState.SelectedValue.ToString());


                if (chkstatus.Checked == true)
                {
                    objmaster.DistrictStatus = 1;
                }
                else
                {
                    objmaster.DistrictStatus = 0;
                }
                if (ViewState["DistrictID"] == null || (int)ViewState["DistrictID"] == 0)
                {
                    try
                    {
                        blanksertext();
                        objmaster.error = "";
                        objmaster.InsertDistrictInfo();
                        if (objmaster.error == "")
                        {
                            databind();
                            ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                            blankinserttext();

                            updgrid.Update();
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
                        updAddUserMain.Update();
                        objmaster.error = "";
                        objmaster.DistrictId = (int)ViewState["DistrictID"];
                        objmaster.DistrictName = txtInsertName.Text.ToString();
                        objmaster.DistrictCode = txtInsertCode.Text.ToString();
                        objmaster.DistrictStateId = Convert.ToInt16(cmbInsertState.SelectedValue.ToString());

                        objmaster.UpdateDistrictInfo();
                        if (objmaster.error == "")
                        {
                            databind();
                            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                            blankinserttext();
                            ViewState["DistrictID"] = null;



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
        databind ();
      }
    protected void btnSerchDistrict_Click(object sender, EventArgs e)
    {
        blankinserttext();
        if (txtSerName.Text == ""  &&  txtSerCode.Text == "" && cmbSerState.SelectedValue == "0" && cmbSerCountry.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please enter atleast one searching parameter");
            return;
        }
        databind();
    }
    protected void grdState_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (MastersData objmaster = new MastersData())
        {

            if (e.CommandName == "Active")
            {
                try
                {
                    ucMessage1.Visible = false;
                    blankinserttext();
                   
                    objmaster.DistrictId = Convert.ToInt32(e.CommandArgument);
                    objmaster.DistrictSelectionMode = 2;
                    districtinfo = objmaster.SelectDistrictInfo();
                    objmaster.DistrictName = Convert.ToString(districtinfo.Rows[0]["DistrictName"]);
                    objmaster.DistrictCode = Convert.ToString(districtinfo.Rows[0]["DistrictCode"]);
                    objmaster.DistrictStateId = Convert.ToInt16(districtinfo.Rows[0]["StateId"]);





                    objmaster.DistrictStatus = Convert.ToInt16(districtinfo.Rows[0]["Status"]);

                    if (objmaster.DistrictStatus == 1)
                    {
                        objmaster.DistrictStatus = 0;
                    }
                    else
                    {
                        objmaster.DistrictStatus = 1;
                    }
                    objmaster.error = "";
                    objmaster.UpdateDistrictInfo();

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
                  

                    objmaster.DistrictId = Convert.ToInt32(e.CommandArgument);
                    ViewState["DistrictID"] = objmaster.DistrictId;
                    objmaster.DistrictSelectionMode = 2;
                    districtinfo = objmaster.SelectDistrictInfo();
                    txtInsertName.Text = Convert.ToString(districtinfo.Rows[0]["DistrictName"]);
                    txtInsertCode.Text = Convert.ToString(districtinfo.Rows[0]["DistrictCode"]);
                    chkstatus.Checked = Convert.ToBoolean(districtinfo.Rows[0]["Status"].ToString());
                    cmbInsCountry.ClearSelection();
                    cmbInsCountry.SelectedValue = (districtinfo.Rows[0]["CountryID"].ToString());
                    cmbInsertState.ClearSelection();
                    cmbInsCountry_SelectedIndexChanged(cmbInsertState,new EventArgs());
                    cmbInsertState.SelectedValue = Convert.ToString(districtinfo.Rows[0]["StateID"]);
                    btnSubmit.Text = "Update";
                    updgrid.Update();
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

    protected void btngetalldata_click(object sender, EventArgs e)
    {
        blankinserttext();
        blanksertext();
        databind();
        ucMessage1.Visible = false;

    }
    protected void grdDistrict_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdDistrict.PageIndex = e.NewPageIndex;
        databind();
    }

    # endregion 

    # region export to excel 


    protected void exporttoexel_Click(object sender, EventArgs e)
    {
        //if (ViewState["Table"] != null)
        //{
        //DataTable dt = (DataTable)ViewState["Table"];
        databind();         //Pankaj Dhingra
        DataTable dt = districtserch.Copy();
        string[] DsCol = new string[] { "DistrictCode", "DistrictName", "StateName","CountryName" ,"CurrentStatus" };
        DataTable DsCopy = new DataTable();
        dt = dt.DefaultView.ToTable(true, DsCol);
        dt.Columns["CurrentStatus"].ColumnName = "Status";

        if (dt.Rows.Count > 0)
        {
            DataSet dtcopy = new DataSet();
            dtcopy.Merge(dt);
            dtcopy.Tables[0].AcceptChanges();
            String FilePath = Server.MapPath("../../");
            string FilenameToexport = "DistrictDetails";
            PageBase.RootFilePath = FilePath;
            PageBase.ExportToExecl(dtcopy, FilenameToexport);
            //ViewState["Table"] = null;
        }
        else
        {
            ucMessage1.ShowError(Resources.Messages.NoRecord);
        }
        //ViewState["Table"] = null;
        //}
    }

# endregion 
    
    protected void cmbInsCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
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
                dt =  obj.SelectStateInfo();
                String[] colArray = { "StateID", "StateName" };
                PageBase.DropdownBinding(ref cmbInsertState, dt, colArray);
            }
        }

    }
    protected void cmbSerCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
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

