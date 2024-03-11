using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Globalization;
using Microsoft.ApplicationBlocks.Data;
using System.Resources;
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Data.SqlClient;
using Resources;
using BussinessLogic;
using DataAccess;



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
* Created By    : Pankaj Dhigra
* Role          : SE
* Module        : Admin
* Description   : This page is used for  creation of tax 
* Table Name    : ServiceChargesPriceMaster
* ====================================================================================================
* Reviewed By : 
 ====================================================================================================
Change Log:
-----------
 * 07-Mar-2013, Shilpi Sharma, #CC01 - Picking Up Country Name From Global Resource File in Design And In Grid.
 * 31-Jul-2013, Shilpi Sharma, #CC02 - Remove column and rename column as there is no need of it can not modify prc because this function 
 *                                     already user somewhere.
 * 28-Aug-2017, Kalpana,       #CC03 - hardcoded style removed and applied responsive css.                                    
 * 19-DEC-2018, Rakesh Raj,       #CC04 - Imported From ZEDEBS 
 ====================================================================================================
*/
#endregion

public partial class Common_TaxMaster : PageBase
{
    public void blankInsert()
        {
            Save.Text = "Save";
            txtRemarks.Text = "";
            txtTaxName.Text = "";
            cmbTaxGroup.SelectedValue = "0";
            cmbTaxType.SelectedValue = "0";
            txtDisplayOrder.Text = "";
            chkActive.Checked = true;
            cmbCountry.SelectedValue = "0";
            ucMessage1.Visible = false;
            updMain.Update();
        }
        public void blankSer()
        {
            txtSerTax.Text = "";
            ucMessage1.Visible = false;
            divgrd.Visible = false;
            csdCountry.SelectedValue = "";
            ucPagingControl1.SetCurrentPage = 1;
            updGrid.Update();
            ucStatusSer.SelectedIndex = 0;
            ViewState["PageIndex"] = 1;
        }

        public DataTable serTax(Int16 mode, clsTaxmaster obj)
        {
            obj.PageSize = Convert.ToInt32(PageBase.PageSize);
            obj.PageIndex = -1;
            obj.TaxName = txtSerTax.Text;
            obj.TaxGroupID = Convert.ToInt16(cmbTaxGroup.SelectedValue);
            obj.TaxTypeID = Convert.ToInt16(cmbTaxType.SelectedValue);
            obj.Status = (mode == 255 || mode == 233) ? Convert.ToInt16(255) : Convert.ToInt16(ucStatusSer.SelectedValue);
            DataTable dt = obj.SelectAll();
            return dt;
        }


        void BindList(int index, Int16 mode)
        {
            try
            {
                index = index == 0 ? 1 : index;
                using (clsTaxmaster obj = new clsTaxmaster())
                {
                    obj.PageSize =  Convert.ToInt32(PageBase.PageSize);
                    obj.PageIndex = index;
                    obj.TaxName = txtSerTax.Text;
                    if (cmbSerCountry.SelectedValue == "")
                    {
                        obj.CountryId = 0;
                    }
                    else
                    {
                        obj.CountryId = Convert.ToInt16(cmbSerCountry.SelectedValue);
                    }
                    obj.TaxGroupID = Convert.ToInt16(cmbTaxGroup.SelectedValue);
                    obj.TaxTypeID = Convert.ToInt16(cmbTaxType.SelectedValue);
                    obj.Status = (mode == 255 || mode == 233) ? Convert.ToInt16(255) : Convert.ToInt16(ucStatusSer.SelectedValue);
                    DataTable dt = obj.SelectAll();
                    grdvList.Visible = true;
                    grdvList.DataSource = dt;
                    grdvList.DataBind();
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        ucPagingControl1.Visible = false;
                    }
                    else
                    {
                        ucPagingControl1.Visible = true;
                        ucPagingControl1.PageSize =  Convert.ToInt32(PageBase.PageSize);
                        ucPagingControl1.TotalRecords = obj.TotalRecords;
                        ucPagingControl1.FillPageInfo();
                    }
                    divgrd.Visible = true;
                    updGrid.Update();
                }
            }
            catch (Exception ex)
            {   
                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);/*#CC04*/
            }
        }
        
        void BindTaxType()
        {
            try
            {
                using (clsTaxmaster obj = new clsTaxmaster())
                {
                    DataTable dt = obj.SelectTaxType();
                    cmbTaxType.DataSource = dt;
                    cmbTaxType.DataTextField = "TaxTypeName";
                    cmbTaxType.DataValueField = "TaxTypeID";
                    cmbTaxType.DataBind();
                    cmbTaxType.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {

                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);/*#CC04*/
            }
        }

        //#Ch01: New function
        void BindTaxGroup()
        {
            try
            {
                using (clsTaxmaster obj = new clsTaxmaster())
                {
                    DataTable dt = obj.SelectTaxGroup();
                    cmbTaxGroup.DataSource = dt;
                    cmbTaxGroup.DataTextField = "TaxGroupName";
                    cmbTaxGroup.DataValueField = "TaxGroupID";
                    cmbTaxGroup.DataBind();
                    cmbTaxGroup.Items.Insert(0, new ListItem("Select", "0"));
                } 
            }
            catch (Exception ex)
            {

                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }
        }

        public void getCountry(int countryid)
        {

            using (MastersData objcountry = new MastersData())
            {
                objcountry.Active = 1;
                objcountry.CountryID = countryid;
                objcountry.CountrySelectionMode = 1;
                DataTable dt = objcountry.SelectCountryInfo();
                cmbCountry.DataSource = dt;
                cmbCountry.DataTextField = "CountryName";
                cmbCountry.DataValueField = "CountryID";
                cmbCountry.DataBind();
                cmbCountry.Items.Insert(0, new ListItem("Select", "0"));
                

            }
        }
       

        //#Ch01: New function
       
       
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {

            /* #CC01: added (Start). */

            grdvList.Columns[2].HeaderText = "Country Name";// raj Resources.ApplicationKeyword.Country + " Name";/*#CC04*/
            
            /* #CC01: added (End). Picking Up City,Country Name From Global Resource File in Design And In Grid.*/           

            if (!IsPostBack)
            {
                BindTaxType();
                getCountry(0);
                BindTaxGroup();
                divgrd.Visible = false;
            }
            Exporttoexcel.ImageUrl = PageBase.siteURL + PageBase.strAssets + "/CSS/images/excel.gif";
        }
        
        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                int result = 1;
                using (clsTaxmaster obj = new clsTaxmaster())
                {
                    obj.Active = chkActive.Checked;
                    obj.CountryId = Convert.ToInt16(cmbCountry.SelectedValue);
                    obj.TaxName= txtTaxName.Text.Trim();
                    obj.TaxTypeID = Convert.ToInt16(cmbTaxType.SelectedValue);
                    obj.TaxGroupID = Convert.ToInt16(cmbTaxGroup.SelectedValue);
                    obj.Remarks = txtRemarks.Text.Trim();
                    obj.DisplayOrder = txtDisplayOrder.Text.Trim() != "" ? Convert.ToInt32(txtDisplayOrder.Text.Trim()) : 0;
                    obj.CreatedBy = Convert.ToInt32(PageBase.UserId);
                    result = obj.Save();
                    if (result == 0)
                    {
                        blankInsert();
                        ucMessage1.ShowSuccess(SuccessMessages.SaveSuccess);
                    }
                    else if (result == 1)
                        ucMessage1.ShowError(ErrorMessages.NotSaved);
                    else if (result == 2)
                        ucMessage1.ShowError(ErrorMessages.DuplicateTax);

                    updMain.Update();
                
                }
                
               
            }
            catch (Exception ex)  
            {
                ucMessage1.ShowError(ex.Message);   // Ch01: added
                PageBase.Errorhandling(ex);
            }
        }
        
        protected void UCPagingControl1_SetControlRefresh()
        {
            using (clsTaxmaster clstaxmaster = new clsTaxmaster())
            {
                int intPageNumber = ucPagingControl1.CurrentPage;
                ViewState["PageIndex"] = intPageNumber;
                clstaxmaster.PageIndex = intPageNumber;
                BindList(ucPagingControl1.CurrentPage, Convert.ToInt16(ucStatusSer.SelectedValue));

            }
            updGrid.Update();
        }
        
        protected void grdvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToByte(e.CommandArgument);
            if (e.CommandName == "activeTax")
            {
                try
                {
                    using (clsTaxmaster obj = new clsTaxmaster())
                    {
                        blankInsert();
                        obj.TaxMasterID = id;
                        obj.ModifiedBy = UserId;
                        Int16 chkActive = obj.UpdateActive();
                        if (chkActive == 0)
                        {
                            ucMessage1.ShowSuccess(SuccessMessages.ToggleSuccess);
                        }
                        BindList(Convert.ToInt32(ViewState["PageIndex"]), Convert.ToInt16(ucStatusSer.SelectedValue));
                    }
                }
                catch (Exception ex)
                {   
                    PageBase.Errorhandling(ex);
                }
            }
        }
        
        protected void grdvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    ImageButton objBtnActive = (ImageButton)e.Row.FindControl("Active");
                    Label objlblActive = (Label)e.Row.FindControl("lblactive");

                    if (objlblActive.Text == "0")
                    {
                        objBtnActive.ImageUrl = PageBase.siteURL + PageBase.strAssets + "/CSS/images/decative.png"; //*CC04*/
                        objBtnActive.ToolTip = "Inactive";
                    }
                    else
                    {
                        objBtnActive.ImageUrl = PageBase.siteURL + PageBase.strAssets +  "/CSS/images/active.png";
                        objBtnActive.ToolTip = "Active";
                    }
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.Message);
                    PageBase.Errorhandling(ex);
                }
            }

        }
        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            blankInsert();

        }
        #endregion
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSerTax.Text == "" && ucStatusSer.SelectedIndex == 0  && cmbSerCountry.SelectedValue == "")
                {
                    blankSer();
                    ucMessage1.ShowWarning(WarningMessages.EnterSearchCriteria);
                    updMain.Update();
                    return;
                }
                ucMessage1.Visible = false;
                updMain.Update();
                ucPagingControl1.SetCurrentPage = 1;
                BindList(1, Convert.ToInt16(ucStatusSer.SelectedValue));
            }
            catch (Exception ex)
            {

                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }

        }
        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                blankSer();
                ucMessage1.Visible = false;
                updGrid.Update();
                BindList(1, 255);
            }
            catch (Exception ex)
            {

                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }
        }
         protected void  Exporttoexcel_Click(object sender, ImageClickEventArgs e)
         {
             DataTable dt = serTax(Convert.ToInt16(ucStatusSer.SelectedValue),new clsTaxmaster()) ;
             dt.Columns["TaxName"].ColumnName = "Tax Name";/*#CC02: added*/
             dt.Columns.Remove("TaxMasterID");/*#CC02: added*/
             dt.AcceptChanges();
             DataSet ds = new DataSet();
            ds.Merge(dt);
            ExportToExecl(ds, "TaxMasterExcel");
         }
}
