/*
====================================================================================================================================
 *Change Log: 
 *DD-MMM-YYYY, Name , #CCXX - Description
------------------------------------------------------------------------------------------------------------------------------------
 * 21-Mar-2016, Sumit Maurya, #CC01, Issue of page getting blocked on the execution of event(s) duue to update panel resolved.
 * 14-Mar-2018, Sumit Maurya, #CC02, default button set as it was firing chnage status while pressing enter button.
 * 20-July-2018,Vijay Kumar Prajapati,#CC03,Add Region  Dropdown on interface.
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
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
public partial class Masters_Common_ManageStateMasterVer2 : PageBase
{

    DataTable stateinfo;
    int intSelectedValue;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            EnableViewState = false;
            if (!IsPostBack)
            {
                Page.Form.DefaultButton = btnGetallData.UniqueID; /* #CC02 Added */
                cmbSerPriceList.Items.Insert(0, new ListItem("Select", "0"));
                cmbSerPriceList.Items.Insert(0, new ListItem("Select", "0"));
                fillpricelist();
                fillcountry();
            
            }
            binddata();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }


    }
    protected void Page_Init(object sender, EventArgs e)
    {
        Page.Form.DefaultButton = btnGetallData.UniqueID; /* #CC02 Added */
        cmbSerPriceList.Items.Insert(0, new ListItem("Select", "0"));
        cmbSerPriceList.Items.Insert(0, new ListItem("Select", "0"));
        fillpricelist();
        //ddlRegion.SelectedValue = Session["RegionID"].ToString();
        //Session["RegionID"] = ddlRegion.SelectedValue;
        fillcountry();
        binddata();
    }

    /*
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
            {
                Page.Form.DefaultButton = btnGetallData.UniqueID; // #CC02 Added //
                cmbSerPriceList.Items.Insert(0, new ListItem("Select", "0"));
                fillpricelist();
                fillcountry();
            }
            binddata();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }


    } */

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
                objmaster.CompanyId = PageBase.ClientId;/* #CC04 Added */
                stateinfo = objmaster.SelectStateInfoVer2();
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
            obj.CompanyId = PageBase.ClientId;  /* #CC04 Added */
            dt = obj.SelectCountryInfo();
            String[] colArray = { "CountryID", "CountryName" };
            PageBase.DropdownBinding(ref cmbInsCountry, dt, colArray);
            PageBase.DropdownBinding(ref cmbSerCountry, dt, colArray);

        }

    }

    private void fillpricelist()
    {
        using (ProductData objproduct = new ProductData())
        {
            try
            {
                DataTable dt = new DataTable();

                cmbSerPriceList.Items.Clear();
                objproduct.CompanyId = PageBase.ClientId;
                dt = objproduct.SelectAllPriceListInfo();
                String[] colArray = { "PriceListID", "PriceListName" };

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
            ddlRegion.Items.Clear();
            ddlRegion.Items.Insert(0, new ListItem("Select", "0"));
            btnsubmit.Text = "Submit";
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
        //if (IsPageRefereshed == true)
        //{
        //    return;
        //}
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


                //objmaster.RegionId = Convert.ToInt32(Session["RegionID"]);

                objmaster.RegionId = Convert.ToInt32(ddlRegion.SelectedValue);/*#CC03 Added*/
                objmaster.StateStatus = chkstatus.Checked ? 1 : 0;
                objmaster.CompanyId = PageBase.ClientId;/* #CC04 Added */
                //objmaster.StatePriceListId = Convert.ToInt16(cmbInsertPriceList.SelectedValue.ToString());
                //objmaster.StatePriceEffDate = ucDatePicker.Date;
                //if (Session["StateID"] == null || (int)Session["StateID"] == 0)
                if (ViewState["StateID"] == null || (int)ViewState["StateID"] == 0)
                {
                    try
                    {
                        objmaster.error = "";
                        objmaster.InsertStateInfoVer2();
                        if (objmaster.error == "")
                        {
                            blanksearch();
                            binddata();
                            ucMessage1.ShowSuccess(Resources.Messages.CreateSuccessfull);
                            blankinsert();
                            updgrid.Update();
                            updAddUserMain.Update();/*#CC03 Added*/
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
                        objmaster.CompanyId = PageBase.ClientId;/* #CC04 Added */
                        //objmaster.StateId = (int)Session["StateID"];
                        objmaster.StateId = (int)ViewState["StateID"];
                        objmaster.UpdateStateInfoVer2();
                        if (objmaster.error == "")
                        {

                            binddata();
                            ucMessage1.ShowSuccess(Resources.Messages.EditSuccessfull);
                            //Session["StateID"] = null;
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
        chkstatus.Checked = true;


    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["PageIndex"] = 0;
        //Session["PageIndex"] = 0;
        if (txtSerCode.Text == "" && txtSerName.Text == "" && cmbSerPriceList.SelectedValue == "0" && cmbSerCountry.SelectedValue == "0")
        {
            ucMessage1.ShowInfo("Please Enter atleast one searching parameter");
            return;
        }
        blankinsert();
        binddata();
        updgrid.Update();
    }
    protected void btnGetallData_Click(object sender, EventArgs e)
    {
        //Session["PageIndex"] = 0;
        ViewState["PageIndex"] = 0;
        blankinsert();
        blanksearch();
        binddata();
        ucMessage1.Visible = false;
        updgrid.Update();

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
            string[] DsCol = new string[] { "StateCode", "StateName", "CountryName", "RegionName", "ExcelPriceListDate", "PriceListName", "CurrentStatus" };
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
    #region NewFunctionality
    protected void grdState_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
    {
        try
        {
            if (!e.Expanded)
                return;
            Session["Visible"] = e.VisibleIndex;
            grdState.DetailRows.CollapseAllRows();
            grdState.DetailRows.ExpandRow(e.VisibleIndex);
            grdState.DetailRows.IsVisible(e.VisibleIndex);
            ASPxGridView objDetail = (ASPxGridView)(sender as ASPxGridView).FindDetailRowTemplateControl(e.VisibleIndex, "detailGrid");
            objDetail.DataSource = Session["Detail"];
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }

    }
    protected void grdState_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
    {

        try
        {
            //Session["PageIndex"] = null;
            ViewState["PageIndex"] = null;
            using (MastersData objmaster = new MastersData())
            {
                if (e.CommandArgs.CommandName == "cmdEdit")
                {
                    ucMessage1.Visible = false;
                    objmaster.StateId = Int16.Parse(Convert.ToString(e.CommandArgs.CommandArgument));
                    ViewState["StateID"] = objmaster.StateId;
                    //Session["StateID"] = objmaster.StateId;
                    objmaster.StateSelectionMode = 2;
                    objmaster.CompanyId = PageBase.ClientId;/* #CC04 Added */
                    stateinfo = objmaster.SelectStateInfoVer2();
                    txtInsertName.Text = Convert.ToString(stateinfo.Rows[0]["StateName"]);
                    txtInsertCode.Text = Convert.ToString(stateinfo.Rows[0]["StateCode"]);
                    chkstatus.Checked = Convert.ToBoolean(stateinfo.Rows[0]["Status"]);
                    cmbInsCountry.ClearSelection();

                    cmbInsCountry.Items.FindByValue(stateinfo.Rows[0]["CountryID"].ToString()).Selected = true;
                    /*#CC03 Added Started*/
                    cmbInsCountry_SelectedIndexChanged(new object(), new EventArgs());
                   
                    ddlRegion.Items.FindByValue(stateinfo.Rows[0]["RegionID"].ToString()).Selected = true;
                    Session["RegionID"] = stateinfo.Rows[0]["RegionID"].ToString();
                    /*#CC03 Added End*/

                    btnsubmit.Text = "Update";
                    ucMessage1.Visible = false;
                    grdState.DetailRows.CollapseAllRows();
                    updAddUserMain.Update();
                    updgrid.Update();
                }
                else if (e.CommandArgs.CommandName == "activeState")
                {

                    try
                    {
                        ucMessage1.Visible = false;
                        blankinsert();
                        objmaster.StateId = Int32.Parse(Convert.ToString(e.CommandArgs.CommandArgument));
                        objmaster.Condition = 2;
                        int result = objmaster.DeleteStatePriceListInfo();
                        if (result == 0)
                            ucMessage1.ShowSuccess(Resources.GlobalMessages.StatusChanged);
                        else
                            ucMessage1.ShowError(objmaster.error);
                        binddata();
                        ucMessage1.Visible = true;
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
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
            //updpnlSaveData.Update();

        }

    }
    protected void grdState_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        try
        {
            if (e.RowType != DevExpress.Web.ASPxGridView.GridViewRowType.Data) return;
            ASPxGridView grid = (ASPxGridView)sender;
            object obj = grid.GetRow(e.VisibleIndex);
            ASPxButton btnActive = (ASPxButton)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "btnActive");
            ASPxButton btnMap = (ASPxButton)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "btnMap");
            if (btnMap != null)
            {
                btnMap.Attributes.Add("onclick", "return popup('" + btnMap.CommandArgument.ToString() + "','" + 0 + "')");
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }

    }
    protected void detailGrid_DataSelect(object sender, EventArgs e)
    {
        try
        {
            intSelectedValue = Convert.ToInt32((sender as ASPxGridView).GetMasterRowKeyValue());
            using (MastersData objmaster = new MastersData())
            {
                ViewState["UniqueID"] = intSelectedValue;
                //Session["UniqueID"] = intSelectedValue;
                ASPxGridView objDetail = (ASPxGridView)(sender as ASPxGridView);

                objmaster.StateSelectionMode = 3;
                //objmaster.StateId = Convert.ToInt16(Session["UniqueID"]);
                objmaster.StateId = Convert.ToInt16(ViewState["UniqueID"]);
                DataTable dt_Modess = objmaster.SelectStateInfoVer2();
                Session["Detail"] = dt_Modess;
                objDetail.DataSource = Session["Detail"];
            }


        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
            //updpnlSaveData.Update();
        }

    }
    protected void detailGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        try
        {
            if (e.RowType != DevExpress.Web.ASPxGridView.GridViewRowType.Data) return;
            ASPxGridView grid = (ASPxGridView)sender;
            object obj = grid.GetRow(e.VisibleIndex);
            ASPxButton btnEdit = (ASPxButton)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "btnEditPriceList");
            ASPxLabel lblEffectiveDate = (ASPxLabel)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "lblEffectiveDate");
            ASPxButton btnDelete = (ASPxButton)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "btnDeletePriceList");
            if (lblEffectiveDate != null)
            {
                if (Convert.ToDateTime(lblEffectiveDate.Text) > System.DateTime.Now)
                {
                    if (btnEdit != null)
                    {
                        btnEdit.Visible = true;
                        btnDelete.Visible = true;
                        btnEdit.Attributes.Add("onclick", "return popup('" + 0 + "','" + btnEdit.CommandArgument.ToString() + "')");
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
            //updpnlSaveData.Update();
        }
    }
    protected void detailGrid_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
    {
        try
        {
            //Session["PageIndex"] = null;
            ViewState["PageIndex"] = null;
            ASPxGridView detailGridView = sender as ASPxGridView;
            //RebindDetailGrid(Convert.ToInt16(Session["UniqueID"]), detailGridView);
            RebindDetailGrid(Convert.ToInt16(ViewState["UniqueID"]), detailGridView);
            if (e.CommandArgs.CommandName == "DeletePriceList")
            {
                using (MastersData objmaster = new MastersData())
                {
                    objmaster.UniqueID = Convert.ToInt32(detailGridView.GetRowValues(e.VisibleIndex, new string[] { "PriceListChangeLogID" }));
                    objmaster.Condition = 1;
                    int result = objmaster.DeleteStatePriceListInfo();
                    if (result == 0)
                        ucMessage1.ShowSuccess(Resources.Messages.Delete);
                    else
                        ucMessage1.ShowError(objmaster.error);
                }
            }
            //RebindDetailGrid(Convert.ToInt16(Session["UniqueID"]), detailGridView);
            RebindDetailGrid(Convert.ToInt16(ViewState["UniqueID"]), detailGridView);
            updgrid.Update();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    #endregion ending
    protected void grdState_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
    {
        try
        {
            ASPxGridView grid = (ASPxGridView)sender;
            object obj = grid.GetRow(e.VisibleIndex);
            ASPxButton btnActive = (ASPxButton)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "btnActive");
            ASPxButton btnMap = (ASPxButton)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "btnMap");
            if (btnMap != null)
            {
                btnMap.Attributes.Add("onclick", "return doWin('" + btnMap.CommandArgument.ToString() + "')");
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }

    }

    void RebindDetailGrid(Int16 ID, ASPxGridView detail)
    {
        using (MastersData objmaster = new MastersData())
        {
            objmaster.StateSelectionMode = 3;
            //objmaster.StateId = Convert.ToInt16(Session["UniqueID"]);
            objmaster.StateId = Convert.ToInt16(ViewState["UniqueID"]);
            detail.DataSource = objmaster.SelectStateInfoVer2();
            detail.DataBind();

        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (ViewState["PageIndex"] != null)
        //if (Session["PageIndex"] != null)
            grdState.PageIndex = 0;
    }
    /*#CC03 Added Started*/
    protected void cmbInsCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillRegion();
    }
    private void FillRegion()
    {
        ddlRegion.Items.Clear();
        using (MastersData obj = new MastersData())
        {
            DataTable dt;
            ddlRegion.Items.Clear();
            obj.CountryId = Convert.ToInt32(cmbInsCountry.SelectedValue);
            dt = obj.SelectRegionInfo();
            String[] colArray = { "RegionID", "RegionName" };
            PageBase.DropdownBinding(ref ddlRegion, dt, colArray);
            if (Session["RegionID"] != null && Session["RegionID"].ToString() != "")
            {
                ddlRegion.SelectedValue = Session["RegionID"].ToString();
            }
            // ddlRegion.Items.Insert(0, new ListItem("Select", "0"));

        }

    }
    /*#CC03 Added End*/

}
