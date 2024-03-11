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



public partial class Masters_HO_Common_ManageStateMaster : PageBase
{


    DataTable stateinfo;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
            {
                cmbSerPriceList.Items.Insert(0, new ListItem("Select", "0"));
                fillpricelist();
                fillcountry();
                binddata();
             }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }


    }

    #region user functions
    public void binddata()
    {
        try
        {
            ucMessage1.Visible = false;
            using (MastersData objmaster = new MastersData())
            {
                objmaster.StateCode = txtSerCode.Text.Trim();
                objmaster.StateName = txtSerName.Text.Trim();
                if (Convert.ToInt16(cmbSerPriceList.SelectedValue.ToString()) == 0)
                {
                    objmaster.StatePriceListId = 0;
                }
                else
                {
                    objmaster.StatePriceListId = Convert.ToInt16(cmbSerPriceList.SelectedValue.ToString());
                }
                objmaster.StateCountryid = Convert.ToInt32(cmbSerCountry.SelectedValue);
                objmaster.StateSelectionMode = 2;
                stateinfo = objmaster.SelectStateInfo();
                grdState.DataSource = stateinfo;
                grdState.DataBind();
                updgrid.Update();
                   

             }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }

    public void fillcountry()
    {
        using (MastersData obj = new MastersData())
        {
            DataTable dt;
            cmbInsCountry.Items.Clear();
            obj.CountrySelectionMode = 1;
            dt = obj.SelectCountryInfo();
            String[] colArray = { "CountryID", "CountryName" };
            PageBase.DropdownBinding(ref cmbInsCountry, dt, colArray);
            PageBase.DropdownBinding(ref cmbSerCountry , dt, colArray);

        }

    }

    private void fillpricelist()
    {
        using (ProductData objproduct = new ProductData())
        {
            try
            {
                DataTable dt = new DataTable();
                cmbInsertPriceList.Items.Clear();
                cmbSerPriceList.Items.Clear();
                dt = objproduct.SelectAllPriceListInfo();
                String[] colArray = { "PriceListID", "PriceListName" };
                PageBase.DropdownBinding(ref cmbInsertPriceList, dt, colArray);
                PageBase.DropdownBinding(ref cmbSerPriceList, dt, colArray);

            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);

            }
        }
    }

    public void blankinsert()
    {
        try
        {

            txtInsertCode.Text = "";
            txtInsertName.Text = "";
            cmbInsCountry.SelectedValue = "0";
            cmbInsertPriceList.SelectedIndex = 0;
             btnsubmit.Text = "Submit";
            ucDatePicker.Date = "";
            updAddUserMain.Update();
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
            cmbSerCountry.SelectedValue = "0";
            txtSerName.Text = "";
            txtSerCode.Text = "";
            cmbSerPriceList.SelectedIndex = 0;
            UpdSearch.Update();
        }
        catch (Exception ex)
        {
            string err = ex.Message.ToString();
            ucMessage1.ShowError(err);
        }
    }

    public bool insertvalidate()
    {
        if (cmbInsertPriceList.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please Select a Price List");
            return false;
        }
        if (ucDatePicker.Date == "")
        {
            ucMessage1.ShowInfo("Please select a date");
            return false;
        }

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


    #endregion

    #region control functions

    protected void btninsert_click(object sender, EventArgs e)
    {
        if (IsPageRefereshed == true)
        {
            return;
        }       //Pankaj Dhingra
        using (MastersData objmaster = new MastersData())
        {

            updAddUserMain.Update();
            if (!insertvalidate())
            {
                return;
            }
            else
            {

                objmaster.StateName = txtInsertName.Text.Trim();
                objmaster.StateCode = txtInsertCode.Text.Trim();
                objmaster.StateCountryid = Convert.ToInt32(cmbInsCountry.SelectedValue);
                objmaster.StatePriceListId = Convert.ToInt16(cmbInsertPriceList.SelectedValue.ToString());
                objmaster.StatePriceEffDate = ucDatePicker.Date;
                if (chkstatus.Checked == true)
                {
                    objmaster.StateStatus = 1;
                }
                else
                {
                    objmaster.StateStatus = 0;
                }
                if (ViewState["StateID"] == null || (int)ViewState["StateID"] == 0)
                {
                    try
                    {
                        objmaster.error = "";
                        objmaster.InsertStateInfo();
                        if (objmaster.error == "")
                        {
                            blanksearch();
                            binddata();
                            ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                            blankinsert();
                            updgrid.Update();
                        }
                        else
                        {
                            ucMessage1.ShowInfo(objmaster.error);
                        }
                    }
                    catch (Exception ex)
                    {
                        ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                        PageBase.Errorhandling(ex);
                    }
                }
                else
                {
                    try
                    {

                        objmaster.error = "";
                        updAddUserMain.Update();
                        objmaster.StatePreviousPriceListId = (Int16)ViewState["PriceListId"];
                        objmaster.StateId = (int)ViewState["StateID"];
                        if (Convert.ToDateTime(ucDatePicker.Date) < Convert.ToDateTime((string)ViewState["PriceListDate"]))
                        {
                            ucMessage1.ShowInfo("Updated Price Effective date must be greater than previous one");
                            return;
                        }

                        objmaster.UpdateStateInfo();
                        if (objmaster.error == "")
                        {
                            
                            binddata();
                            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                            ViewState["StateID"] = null;
                            blankinsert();
                            updAddUserMain.Update();
                        }
                        else
                        {
                            ucMessage1.ShowInfo(objmaster.error);
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
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {

        
        blankinsert();
        blanksearch();
        binddata();
        ucMessage1.Visible = false;


    }




    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtSerCode.Text == "" && txtSerName.Text == "" && cmbSerPriceList.SelectedValue == "0"  && cmbSerCountry.SelectedValue =="0")
        {
            ucMessage1.ShowInfo("Please Enter atleast one searching parameter");
            return;
        }
        
        blankinsert();
        binddata();
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
                    blankinsert();
                    objmaster.StateId = Convert.ToInt32(e.CommandArgument);
                    objmaster.StateSelectionMode = 2;
                    stateinfo = objmaster.SelectStateInfo();
                    objmaster.StateName = Convert.ToString(stateinfo.Rows[0]["StateName"]);
                    objmaster.StateCode = Convert.ToString(stateinfo.Rows[0]["StateCode"]);
                    objmaster.StatePriceListId = Convert.ToInt16(stateinfo.Rows[0]["PriceListID"]);
                    objmaster .StatePreviousPriceListId  = Convert.ToInt16(stateinfo.Rows[0]["PriceListID"]);
                    objmaster.StatePriceEffDate = Convert.ToString(stateinfo.Rows[0]["PriceListEffectiveDate"]);
                    objmaster.StateStatus = Convert.ToInt16(stateinfo.Rows[0]["Status"]);
                    objmaster.StateCountryid = Convert.ToInt32(stateinfo.Rows[0]["CountryID"]);
                    if (objmaster.StateStatus == 1)
                    {
                        objmaster.StateStatus = 0;
                    }
                    else
                    {
                        objmaster.StateStatus = 1;
                    }
                    objmaster.error = "";
                    objmaster.UpdateStateInfo();

                    if (objmaster.error == "")
                    {
                        binddata();
                        ucMessage1.ShowSuccess(Resources.Messages.StatusChanged);

                    }
                    else
                    {
                        ucMessage1.ShowInfo(objmaster.error);

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
                    ucMessage1.Visible = false;


                    objmaster.StateId = Convert.ToInt32(e.CommandArgument);
                    ViewState["StateID"] = objmaster.StateId;
                    objmaster.StateSelectionMode = 2;
                    stateinfo = objmaster.SelectStateInfo();

                    txtInsertName.Text = Convert.ToString(stateinfo.Rows[0]["StateName"]);
                    txtInsertCode.Text = Convert.ToString(stateinfo.Rows[0]["StateCode"]);
                    ucDatePicker.Date = Convert.ToString(stateinfo.Rows[0]["PriceListEffectiveDate"]);
                    ViewState["PriceListDate"] = Convert.ToString(stateinfo.Rows[0]["PriceListEffectiveDate"]);
                    chkstatus.Checked = Convert.ToBoolean(stateinfo.Rows[0]["Status"].ToString());
                    cmbInsCountry.ClearSelection();
                    cmbInsCountry.SelectedValue = Convert.ToString(stateinfo.Rows[0]["CountryID"]);

                    //  cmbInsertState.SelectedValue = cityinfo.Rows[0]["StateID"].ToString();
                    if (cmbInsertPriceList.Items.FindByValue(stateinfo.Rows[0]["PriceListID"].ToString()) != null)
                    {
                        cmbInsertPriceList.ClearSelection();
                    }

                    cmbInsertPriceList.Items.FindByValue(stateinfo.Rows[0]["PriceListID"].ToString()).Selected = true;
                    ViewState["PriceListId"] = Convert.ToInt16(stateinfo.Rows[0]["PriceListID"].ToString());

                    btnsubmit.Text = "Update";
                    updAddUserMain.Update();

                    updgrid.Update();
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                    PageBase.Errorhandling(ex);
                }


            }
        }





    }




    protected void btnGetallData_Click(object sender, EventArgs e)
    {
       
        blankinsert();
        blanksearch();
        binddata();

        ucMessage1.Visible = false;

    }
    protected void grdState_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdState.PageIndex = e.NewPageIndex;
        binddata();
    }

    #endregion 

    # region export to excel 
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {

            //if (ViewState["Table"] != null)
            //{
            //DataTable dt = (DataTable)ViewState["Table"];
            binddata();         //Pankaj Dhingra
            DataTable dt = stateinfo.Copy();
            string[] DsCol = new string[] { "StateCode", "StateName","CountryName", "ExcelPriceListDate", "PriceListName", "CurrentStatus" };
            DataTable DsCopy = new DataTable();
            dt = dt.DefaultView.ToTable(true, DsCol);
            dt.Columns["CurrentStatus"].ColumnName = "Status";
            dt.Columns["ExcelPriceListDate"].ColumnName = "Price Effective Date";

            if (dt.Rows.Count > 0)
            {
                DataSet dtcopy = new DataSet();
                dtcopy.Merge(dt);
                dtcopy.Tables[0].AcceptChanges();
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "StateDetails";
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
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }

    #endregion 


   
}


   


