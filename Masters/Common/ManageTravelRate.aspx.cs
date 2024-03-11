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
* Created By : Dhiraj Kumar
* Role : Software Engineer
* Module : Admin
* Description :This page is used for Managing Travel Rates.
--------Change Log---------------
 DATE           NAME                DESCRIPTION
 * 16-Jan-2013, Shilpi Sharma,      #CH01: A Field Name CurrencyId Added in Save ANd Update .
 * 31-Oct-2017, Kalpana,            #CC02: hardcoded style removed and applied responsive css. 
====================================================================================================
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Resources;
using DataAccess;
using BussinessLogic;
using System.Collections;


public partial class Admin_ManageTravelRate : PageBase
{
    #region Properties
    private Int32 PageNumber
    {
        get
        {
            if (ViewState["PageNumber"] != null)
            {
                return Convert.ToInt32(ViewState["PageNumber"]);
            }
            else
            {
                return 1;
            }
        }
        set
        {
            if (value == 0)
            {
                ViewState["PageNumber"] = 1;
            }
            else
            {
                ViewState["PageNumber"] = value;
            }
        }
    }
    #endregion Properties
    private static bool SearchFlag = false;

    #region Methods
    void ClearData()
    {
        ucRenualDate.Date = string.Empty;
        ddlProductCategory.SelectedIndex = 0;
        //txtProducts.Text = "";                // #Ch01: removed
        ddlrole.SelectedIndex = 0;
        ddlVehicalType.SelectedIndex = 0;
        txtamount.Text = string.Empty;
        //txtprorata.Text = string.Empty;
        //ucRenualDate.Date = DateTime.Now.ToShortDateString();
    }

    void BindList(int index)
    {
        try
        {
            using (clsTravelRate ObjTravelRate = new clsTravelRate())
            {
                ObjTravelRate.TransportTypeMasterId = Convert.ToInt16(ddlvehicletypesearch.SelectedValue);
                ObjTravelRate.Roleid = Convert.ToInt32(ddlrolesearch.SelectedValue);
                ObjTravelRate.PageIndex = index;
                ObjTravelRate.PageSize = Convert.ToInt32(PageBase.PageSize);
                ObjTravelRate.CompanyId = PageBase.ClientId;
                DataTable dt;
                grdvList.Visible = true;

                dt = ObjTravelRate.SelectAll();
                if (dt.Rows.Count == 0 && index > 1)
                {
                    index--;
                    ObjTravelRate.PageIndex = index;
                    PageNumber = index;
                    dt = ObjTravelRate.SelectAll();
                }
                grdvList.DataSource = dt;
                grdvList.DataBind();
                if (dt == null || dt.Rows.Count == 0)
                {
                    Exporttoexcel.Visible = false;
                    ucPagingControl1.Visible = false;
                }
                else
                {
                    //Paging
                    Exporttoexcel.Visible = true;
                    ucPagingControl1.CurrentPage = index;
                    ucPagingControl1.Visible = true;
                    ucPagingControl1.PageSize = Convert.ToInt32(PageBase.PageSize);
                    ucPagingControl1.TotalRecords = ObjTravelRate.TotalRecords;
                    ucPagingControl1.FillPageInfo();
                }
                udtPnlGrd.Update();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }

    void BindSearchList(int index)
    {
        try
        {
            using (clsTravelRate ObjTravelRate = new clsTravelRate())
            {
                ucMessage1.Visible = false;
                ObjTravelRate.PageIndex = index;
                ObjTravelRate.PageSize = Convert.ToInt32(PageBase.PageSize);
                ObjTravelRate.ValidFrom_Search = UcDatePickerSearch.Date;
                ObjTravelRate.TransportTypeMasterId = Convert.ToInt16(ddlvehicletypesearch.SelectedValue);
                ObjTravelRate.Roleid = Convert.ToInt32(ddlrolesearch.SelectedValue);
                ObjTravelRate.CompanyId = PageBase.ClientId;

                grdvList.Visible = true;
                //SearchFlag
                DataTable dt = ObjTravelRate.SelectAll();
                grdvList.DataSource = dt;
                grdvList.DataBind();
                divGrd.Visible = true;
                if (dt == null || dt.Rows.Count == 0)
                {
                    Exporttoexcel.Visible = false;
                    Exporttoexcel.Enabled = false;
                    // Exporttoexcel.Attributes.Add("style", "display:none");
                    ucPagingControl1.Visible = false;
                    PageNumber = 1;


                }
                else
                {
                    //Paging
                    //dvheader.Visible = true;
                    Exporttoexcel.Visible = true;
                    Exporttoexcel.Enabled = true;
                    PageNumber = index;
                    ucPagingControl1.CurrentPage = index;
                    ucPagingControl1.Visible = true;
                    ucPagingControl1.PageSize = Convert.ToInt32(PageBase.PageSize);
                    ucPagingControl1.TotalRecords = ObjTravelRate.TotalRecords;
                    ucPagingControl1.FillPageInfo();
                }
                udtPnlGrd.Update();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }

    //void BindDropDownVehicleType()
    //{
    //    try
    //    {
    //        Hashtable htsource = GetEnumCollection();
    //        ddlVehicalType.DataSource = htsource;
    //        ddlVehicalType.DataValueField = "Key";
    //        ddlVehicalType.DataTextField = "Value";
    //        ddlVehicalType.DataBind();
    //        ddlVehicalType.Items.Insert(0, new ListItem("Select"));

    //        ddlvehicletypesearch.DataSource = htsource;
    //        ddlvehicletypesearch.DataValueField = "Key";
    //        ddlvehicletypesearch.DataTextField = "Value";
    //        ddlvehicletypesearch.DataBind();
    //        ddlvehicletypesearch.Items.Insert(0, new ListItem("Select","0"));
    //    }
    //    catch (Exception ex)
    //    {
    //        ExceptionPolicy.HandleException(ex, ExceptionPolicyName);
    //    }
    //}

    void BindDropDownnRoles()
    {
        using (clsTravelRate objRole = new clsTravelRate())
        {
            objRole.PageIndex = 1;
            objRole.EntityTypeID = Convert.ToInt16(EntityTypeID);
            objRole.PageSize = Convert.ToInt32(2000);
            objRole.CompanyId = PageBase.ClientId;

            DataTable dtRoletype = objRole.SelectUserRole();

            if (dtRoletype.Rows.Count > 0)
            {
                ddlrole.DataTextField = "EntityTypeRoleName";
                ddlrole.DataValueField = "RoleID";
                ddlrole.DataSource = dtRoletype;
                ddlrole.DataBind();

                ddlrolesearch.DataTextField = "EntityTypeRoleName";
                ddlrolesearch.DataValueField = "RoleID";
                ddlrolesearch.DataSource = dtRoletype;
                ddlrolesearch.DataBind();
            }
            ddlrole.Items.Insert(0, new ListItem("All", "0"));
            ddlrolesearch.Items.Insert(0, new ListItem("All", "0"));
            ddlrolesearch.Items.Insert(0, new ListItem("Select", "-1"));
        };
    }
    //public Hashtable GetEnumCollection()
    //{
    //    var val = new Hashtable();
    //    foreach (int value in Enum.GetValues(typeof(ZedAxis.ZedEBS.Enums.EnumVehicle)))
    //    {
    //        var name = Enum.GetName(typeof(ZedAxis.ZedEBS.Enums.EnumVehicle), value);
    //        val.Add(value, name);
    //    }
    //    return val;
    //}
    public bool ShowDelete(object ValidFrom)
    {
        DateTime DateToday = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        DateTime RecordDate = Convert.ToDateTime(ValidFrom);

        if (ValidFrom == null)
            return true;
        else
        {
            if (RecordDate < DateToday)
                return false;
            else
                return true;
        }
    }

    public void BindTransportList()
    {
        using (clsTravelRate ObjTravelRate = new clsTravelRate())
        {
            ObjTravelRate.PageIndex = 1;
            ObjTravelRate.PageSize = 1000;
            ObjTravelRate.TransportType = null;
            ObjTravelRate.CompanyId = PageBase.ClientId;

            DataTable dt = ObjTravelRate.GetTransportTypeList();
            if (dt.Rows.Count > 0)
            {
                ddlVehicalType.DataSource = dt;
                ddlVehicalType.DataTextField = "TransportName";
                ddlVehicalType.DataValueField = "TransportTypeMasterId";
                ddlVehicalType.DataBind();
                ddlVehicalType.Items.Insert(0, new ListItem("Select", "0"));

                ddlvehicletypesearch.DataSource = dt;
                ddlvehicletypesearch.DataTextField = "TransportName";
                ddlvehicletypesearch.DataValueField = "TransportTypeMasterId";
                ddlvehicletypesearch.DataBind();
                ddlvehicletypesearch.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
    }

    //protected void BindDropDownProductCategory()
    //{
    //    try
    //    {
    //        using (clsProductCategoryMaster objProductCategoryMaster = new clsProductCategoryMaster())
    //        {
    //            objProductCategoryMaster.PageIndex = 1;
    //            objProductCategoryMaster.PageSize = 1000;
    //            objProductCategoryMaster.ActiveInSap = true;

    //            DataTable ds = objProductCategoryMaster.SelectAll();
    //            if (ds != null)
    //            {
    //                ddlProductCategory.DataSource = ds;
    //                ddlProductCategory.DataTextField = "CategoryDescription";
    //                ddlProductCategory.DataValueField = "productcategoryId";
    //                ddlProductCategory.DataBind();

    //                ddlProductCategory.Items.Insert(0, new ListItem("Select", "0"));
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ExceptionPolicy.HandleException(ex, ExceptionPolicyName);
    //    }

    //}     //Now it is not in use
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //dvheader.Visible = false;
            ucRenualDate.TextBoxDate.Text = DateTime.Now.AddDays(1).ToShortDateString();
            ucRenualDate.MinRangeValue = DateTime.Now.AddDays(1);
            // ucRenualDate.MaxRangeValue = DateTime.Now.AddYears(100);
            ucRenualDate.IsRequired = true;
            //BindDropDownProductCategory();        //Pankaj Dhingra Now ProductCategory is not in use
            SearchFlag = false;
            //BindList(1);
            BindTransportList();
            //BindDropDownVehicleType();
            BindDropDownnRoles();
            divGrd.Visible = false;
        }
    }

    protected void UCPagingControl1_SetControlRefresh()
    {
        using (clsTravelRate ObjTravelRate = new clsTravelRate())
        {
            int intPageNumber = ucPagingControl1.CurrentPage;
            ObjTravelRate.PageIndex = intPageNumber;
            if (SearchFlag)
                BindSearchList(ucPagingControl1.CurrentPage);
            else
                BindList(ucPagingControl1.CurrentPage);
            PageNumber = intPageNumber;
        }
        udtPnlGrd.Update();
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        try
        {
            if (Save.Text == "Save")
            {
                using (clsTravelRate ObjTravelRate = new clsTravelRate())
                {
                    ObjTravelRate.Roleid = Convert.ToInt32(ddlrole.SelectedValue.ToString());
                    ObjTravelRate.TransportTypeMasterId = Convert.ToInt16(ddlVehicalType.SelectedValue.ToString());
                    ObjTravelRate.TravelRateAmount = Convert.ToDecimal(txtamount.Text.ToString());
                    ObjTravelRate.ValidFrom = Convert.ToDateTime(ucRenualDate.Date);
                    ObjTravelRate.CreatedBy = UserId;
                    string currency = UCCurrency1.SelectedValue;
                    ObjTravelRate.CurrencyId = Convert.ToInt16(UCCurrency1.SelectedValue); //#CH01:added.
                    ObjTravelRate.CompanyId = PageBase.ClientId;

                    Int16 result = ObjTravelRate.Insert();
                    if (result == 0)
                    {
                        ucMessage1.ShowSuccess(SuccessMessages.SaveSuccess);
                        ClearData();
                        BindList(PageNumber);
                    }
                    else if (result == 1)
                        ucMessage1.ShowError(ErrorMessages.NotSaved);
                    else if (result == 2)
                        ucMessage1.ShowWarning(ErrorMessages.DuplicateRate);
                }
            }
            else if (Save.Text == "Update")
            {
                using (clsTravelRate ObjTravelRate = new clsTravelRate())
                {
                    ObjTravelRate.TravelRateID = int.Parse(Convert.ToString(ViewState["TravelRateID"]));
                    ObjTravelRate.Roleid = Convert.ToInt32(ddlrole.SelectedValue.ToString());
                    ObjTravelRate.TransportTypeMasterId = Convert.ToInt16(ddlVehicalType.SelectedValue.ToString());
                    ObjTravelRate.TravelRateAmount = Convert.ToDecimal(txtamount.Text.ToString());
                    ObjTravelRate.CurrencyId = Convert.ToInt16(UCCurrency1.SelectedValue.ToString());             //#CH01:added.
                    ObjTravelRate.ValidFrom = Convert.ToDateTime(ucRenualDate.Date);
                    ObjTravelRate.ModifiedOn = DateTime.Now;
                    ObjTravelRate.ModifiedBy = UserId;
                    ObjTravelRate.CompanyId = PageBase.ClientId;

                    Int32 result = ObjTravelRate.Update();
                    if (result == 0)
                    {
                        ucMessage1.ShowSuccess(SuccessMessages.SaveSuccess);
                        ClearData();
                        ViewState["TravelRateID"] = null;
                        Save.Text = "Save";
                        //btnCancel.Visible = false;
                        if (SearchFlag)
                            BindSearchList(PageNumber);
                        else
                            BindList(PageNumber);

                    }
                    else if (result == 1)
                        ucMessage1.ShowInfo(ObjTravelRate.Error.ToString());
                    else if (result == 2)
                        ucMessage1.ShowWarning(ErrorMessages.DuplicateRate);
                    else if (result == 3)
                        ucMessage1.ShowInfo(ErrorMessages.NotExistPartCode);
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowInfo(ex.Message);
           
        }

        updPnl2.Update();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        Save.Text = "Save";
        ClearData();
        //btnCancel.Visible = false;
        ucRenualDate.TextBoxDate.Text = DateTime.Now.ToShortDateString();

        if (SearchFlag)
            BindSearchList(PageNumber);
        else
            BindList(PageNumber);
        updPnl2.Update();
        udtPnlGrd.Update();
    }

    protected void grdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            using (clsTravelRate ObjTravelRate = new clsTravelRate())
            {
                if (e.CommandName == "editUsed")
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    ViewState["TravelRateID"] = id;
                    ObjTravelRate.TravelRateID = id;
                    ObjTravelRate.CompanyId = PageBase.ClientId;

                    DataTable dt = ObjTravelRate.SelectById();
                    Save.Text = "Update";
                    btnCancel.Visible = true;
                    if (dt.Rows[0]["Roleid"] == null || Convert.ToString(dt.Rows[0]["Roleid"]) == "")
                    {
                        ddlrole.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlrole.SelectedValue = Convert.ToString(dt.Rows[0]["Roleid"]);
                    }
                    ddlVehicalType.SelectedValue = Convert.ToString(dt.Rows[0]["TransportTypeMasterId"]);
                    txtamount.Text = Convert.ToString(dt.Rows[0]["TravelRateAmount"]);
                    UCCurrency1.SelectedValue = Convert.ToString(dt.Rows[0]["CurrencyID"]);                 //#CH01:added.                       
                    ucRenualDate.TextBoxDate.Text = Convert.ToString(dt.Rows[0]["ValidFrom"]);
                    ucMessage1.Visible = false;
                }
                else if (e.CommandName == "deleteUsed")
                {
                    int id = Convert.ToInt32(e.CommandArgument);

                    ObjTravelRate.TravelRateID = id;
                    ObjTravelRate.CompanyId = PageBase.ClientId;

                    bool result = ObjTravelRate.Delete();
                    // bool result = true;
                    if (result)
                    {
                        ucMessage1.ShowSuccess("Record deleted successfully.");
                        BindList(PageNumber);
                    }
                    else
                        ucMessage1.ShowError("Record is not delete successfully.");
                }

                updPnl2.Update();
            }
        }


        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }

    protected void grdvList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                ImageButton objBtnActive = (ImageButton)e.Row.FindControl("Active");
                //Label objlblActive = (Label)e.Row.FindControl("lblactive");
                Label lblvalidfrom = (Label)e.Row.FindControl("lblvalidfrom");
                ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");
                ImageButton imgbtnDelete = (ImageButton)e.Row.FindControl("imgbtnDelete");      //Pankaj Dhingra

                if (e.Row.Cells[1].Text != "&nbsp;")
                {
                    e.Row.Cells[1].Text = ddlrole.Items.FindByValue(e.Row.Cells[1].Text).Text;
                }
                else
                {
                    e.Row.Cells[1].Text = "All";
                }

                //Hashtable hshvehicle = GetEnumCollection();
                //foreach (DictionaryEntry entry in hshvehicle)
                //{
                //    if (entry.Key.ToString() == e.Row.Cells[2].Text)
                //    {
                //        e.Row.Cells[2].Text = entry.Value.ToString();
                //    }
                //}
                if (lblvalidfrom.Text.Trim() != string.Empty)
                {
                    DateTime dt_Today = DateTime.Now.AddDays(1);
                    DateTime dt_validfrom = Convert.ToDateTime(lblvalidfrom.Text.Trim());
                    if (dt_validfrom.CompareTo(dt_Today) > 0)
                    {
                        imgbtnEdit.Visible = true;
                        imgbtnDelete.Visible = true;        //Pankaj Dhingra
                    }
                    else
                    {
                        imgbtnEdit.Visible = false;
                        imgbtnDelete.Visible = false;       //Pankaj Dhingra
                    }
                }
                else
                    imgbtnEdit.Visible = true;

                //if (objlblActive.Text == "False")
                //{
                //    objBtnActive.ImageUrl = "~/Assets/images/decative.png";//objBtnActive.Text = "Activate";
                //    objBtnActive.ToolTip = "Inactive";
                //}
                //else
                //{
                //    objBtnActive.ImageUrl = "~/Assets/images/active.png";//objBtnActive.Text = "InActivate";*/
                //    objBtnActive.ToolTip = "Active";
                //}
                ImageButton objDeleteConfirm = (ImageButton)e.Row.FindControl("imgbtnDelete");
                if (objDeleteConfirm != null)
                {
                    objDeleteConfirm.Attributes.Add("Onclick", "if(!confirm('Are you sure you want to delete this record?')){return false;}");
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowError(ex.ToString());
            }
        }

    }

    //protected void ddlProductCategory_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    AutoCompleteExtenderModel5.ContextKey = ddlProductCategory.SelectedValue;
    //    //txtProducts.Text = "";                // #Ch01: removed
    //    txtProductSAPCode.Text = string.Empty;  // #Ch01: added
    //}     //Pankaj Dhingra

    // #Ch01: removed
    //protected void txtProducts_TextChanged(object sender, EventArgs e)
    //{
    //    if (txtProducts.Text == "No Match Found" || txtProducts.Text == "Select Product Category")
    //    {
    //        txtProducts.Text = "";
    //    }
    //}

    // #Ch01: added
    //This is not in use
    protected void txtProductSAPCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlProductCategory.SelectedIndex == 0)
            {
                //txtPartSAPCode.Text = string.Empty;
                ucMessage1.ShowInfo("Select Product Category");
            }
            //if (txtPartSAPCode.Text == "No Match Found")
            //    txtPartSAPCode.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
          
        }
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlrolesearch.SelectedIndex == 0 && ddlvehicletypesearch.SelectedIndex == 0 && UcDatePickerSearch.Date == "")
            {
                divGrd.Visible = false;
                udtPnlGrd.Update();
                ucMessage1.ShowInfo(WarningMessages.FillAnyParameter);
                updPnl2.Update();
                return;
            }

            SearchFlag = true;
            PageNumber = 1;
            BindSearchList(1);
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
           
        }
    }
    protected void btnSearchCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.Visible = false;
            updPnl2.Update();

            UcDatePickerSearch.Date = "";
            ddlrolesearch.SelectedIndex = 0;
            //ddlvehicletypesearch.SelectedIndex = 0;
            SearchFlag = true;
            PageNumber = 1;
            BindSearchList(1);
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
          
        }
    }

    //DataTable GetVehicleName(DataTable dt)
    //{
    //    Hashtable hstbl = GetEnumCollection();

    //    DataColumn col = dt.Columns.Add("Vehicle Type", typeof(string));
    //    col.SetOrdinal(1);
    //    foreach (DataRow row in dt.Rows)
    //    {
    //        int id=Convert.ToInt32(row["Vehical Type1"]);
    //        if(hstbl.ContainsKey(id))
    //        {

    //            string value = hstbl[id].ToString();
    //            row[1] = hstbl[id].ToString();

    //        }
    //    }
    //    dt.Columns.Remove("Vehical Type1");
    //    return dt;
    //}
    protected void Exporttoexcel_Click(object sender, System.EventArgs e)
    {
        try
        {
            using (clsTravelRate objTravelRate = new clsTravelRate())
            {
                if (SearchFlag)
                {
                    objTravelRate.ValidFrom_Search = UcDatePickerSearch.Date;
                    objTravelRate.TransportTypeMasterId = Convert.ToInt16(ddlvehicletypesearch.SelectedValue);
                    objTravelRate.Roleid = Convert.ToInt32(ddlrolesearch.SelectedValue);
                    objTravelRate.PageIndex = -1;
                    objTravelRate.CompanyId = PageBase.ClientId;

                }
                else
                {
                    objTravelRate.ValidFrom_Search = UcDatePickerSearch.Date;
                    objTravelRate.TransportTypeMasterId = 0;
                    objTravelRate.Roleid = -1;
                    objTravelRate.PageIndex = -1;
                    objTravelRate.CompanyId = PageBase.ClientId;
                }

                DataTable dt = objTravelRate.SelectAll();
                //dt = GetVehicleName(dt);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                PageBase.ExportToExecl(ds, "TravelRateExcel" + DateTime.Now.Ticks.ToString());               
                // UpdatePanel1.Update();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
    #endregion

}
