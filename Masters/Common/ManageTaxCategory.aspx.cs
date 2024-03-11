/*=====================================================================================================
change Log:
----------
 * 19-April-2017, Kalpana, #CH01: removed table tags and hardcoded styles 
 * 28-Aug-2017,   Kalpana, #CC02, hardcoded style removed and applied responsive css.
 * 19-Dec-2018,   Rakesh Raj, #CC03, Imported from ZEDERP
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using LuminousSMS.Data; /*#CC03*/
using System.Data;
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;/*#CC03*/
using ZedControlLib;

using BussinessLogic;
using DataAccess;

    public partial class ManageTaxCategory : PageBase
    {  
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                    BindList(1);
                Exporttoexcel.ImageUrl = PageBase.siteURL + PageBase.strAssets + "/CSS/images/excel.gif";
            }
            catch (Exception ex)
            {
                ucMessage1.Visible= true;
                ucMessage1.ShowError(ex.Message);
                PageBase.Errorhandling(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (clsTaxCategoryMaster objTaxGroup = new clsTaxCategoryMaster())
                {
                    objTaxGroup.CreatedBy = PageBase.UserId;
                    objTaxGroup.TaxCategoryName = txtTaxGroup.Text.Trim();
                    Int16 result = objTaxGroup.Save();
                    //ucMessage1.hideMessagelink = true;/*#CC03*/
                    ucMessage1.Visible= true;
                    if (result == 0)
                    {
                        txtTaxGroup.Text = string.Empty;
                        ucMessage1.ShowSuccess(Resources.SuccessMessages.SaveSuccess);
                        BindList(Convert.ToInt32(hdfCurrentPage.Value));
                    }
                    else if (result == 1)
                        ucMessage1.ShowError(objTaxGroup.Error.ToString());
                    else if (result == 2)
                        ucMessage1.ShowInfo(Resources.ErrorMessages.DuplicateTaxGroup);
                }
            }
            catch (Exception ex)
            {
                ucMessage1.Visible= true;
                ucMessage1.ShowError(ex.Message);
                PageBase.Errorhandling(ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ucMessage1.Visible = false;
                txtTaxGroup.Text = string.Empty;
            }
            catch (Exception ex)
            {
                ucMessage1.Visible= true;;
                ucMessage1.ShowWarning(ex.Message);
                PageBase.Errorhandling(ex);
            }
        }

        protected void Exporttoexcel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataSet dsNew = new DataSet();
                using (clsTaxCategoryMaster objTaxGroup = new clsTaxCategoryMaster())
                {
                    objTaxGroup.PageIndex = -1;
                    objTaxGroup.PageSize = 20;// PageBase.PageSizeDropDown;/*#CC03*/
                    objTaxGroup.ActiveStatus = ucStatus.SelectedValue == "233" ? Convert.ToInt16(2) : (ucStatus.SelectedValue == "255" ? Convert.ToInt16(2) : Convert.ToInt16(ucStatus.SelectedValue));

                    objTaxGroup.TaxCategoryName = txtSearchTaxCategoryName.Text.Trim();

                    DataTable dtNew = objTaxGroup.SelectAll();
                    //DataTable dtExcel = dt.Copy();
                    //dtExcel.Columns.Remove("TaxCategoryID");
                    //dtExcel.Columns.Remove("Active");
                    //dtExcel.Columns.Remove("Row");
                    dsNew.Merge(dtNew);
                    ExportToExecl(dsNew, "TaxCategory");
                }
            }
            catch (Exception ex)
            {
                ucMessage1.Visible= true;
                ucMessage1.ShowWarning(ex.Message);
                PageBase.Errorhandling(ex);
            }
        }

        protected void dlList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
                {
                    Label lblactive = (Label)e.Item.FindControl("lblactive");
                    ZedImageButton imgbtnActive = (ZedImageButton)e.Item.FindControl("Active");
                    ZedImageButton imgbtnEdit = (ZedImageButton)e.Item.FindControl("img1");
                 
                    if (lblactive.Text == "0")
                    {
                        //Assets\ZedSales\CSS\Images
                        imgbtnActive.ImageUrl = PageBase.siteURL + PageBase.strAssets +  "/CSS/images/decative.png";
                        imgbtnActive.ToolTip = "Inactive";
                    }
                    else
                    {
                        imgbtnActive.ImageUrl = PageBase.siteURL + PageBase.strAssets + "/CSS/images/active.png";
                        imgbtnActive.ToolTip = "Active";
                    }
                    imgbtnEdit.ImageUrl = PageBase.siteURL + PageBase.strAssets + "/CSS/images/edit.png";
                   
                }
              
              
            }
            catch (Exception ex)
            {
                ucMessage1.Visible= true;
                ucMessage1.ShowError(ex.Message);
                PageBase.Errorhandling(ex);
            }
        }

        protected void dlList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            try
            {
                //if (e.Item.ItemType == ListItemType.EditItem || e.Item.ItemType == ListItemType.Item)
                //{
                //    ZedImageButton imgbtnUpdate1 = (ZedImageButton)e.Item.FindControl("imgbtnUpdate");
                //    ZedImageButton imgbtnCancelUpdate1 = (ZedImageButton)e.Item.FindControl("imgbtnCancelUpdate");

                //    imgbtnUpdate1.ImageUrl = PageBase.siteURL + PageBase.strAssets + "/CSS/images/icon_update.gif";
                //    imgbtnCancelUpdate1.ImageUrl = PageBase.siteURL + PageBase.strAssets + "/CSS/images/icon_cancel.gif";
                //}

                if (e.CommandName == "EditTaxGroup")
                {
                    dlList.EditItemIndex = (int)e.Item.ItemIndex;
                    BindList(Convert.ToInt32(hdfCurrentPage.Value));
                }
                else if (e.CommandName == "CancelUpdate")
                {
                    dlList.EditItemIndex = -1;
                    
                    BindList(Convert.ToInt32(hdfCurrentPage.Value));
                }
                else if (e.CommandName == "ActiveInactive")
                {
                    using (clsTaxCategoryMaster objTaxGroup = new clsTaxCategoryMaster())
                    {
                        objTaxGroup.TaxCategoryID = Convert.ToInt32(dlList.DataKeys[e.Item.ItemIndex]);
                        DataListItem dlItm = (DataListItem)(((ZedImageButton)e.CommandSource).NamingContainer);
                        Label lblActive = (Label)e.Item.FindControl("lblActive");
                        objTaxGroup.ModifiedBy = PageBase.UserId;
                        objTaxGroup.CreatedBy = PageBase.UserId;
                        Int16 result = objTaxGroup.ActiveInactive();

                        ucMessage1.Visible= true;
                        if (result == 0)
                        {
                            if (lblActive.Text.Trim() == "1")
                                ucMessage1.ShowSuccess(Resources.SuccessMessages.ToggleSuccess);
                            else
                                ucMessage1.ShowSuccess(Resources.SuccessMessages.ToggleSuccess);

                            BindList(Convert.ToInt32(hdfCurrentPage.Value));
                        }
                            //Pankaj Dhingra
                            //There was no check in the procedure in this regard
                        //else if (result == 2)
                        //    ucMessage1.ShowInfo("Tax group mapped with part. You cannot change its status!");
                        else if (result == 1)
                            ucMessage1.ShowError(objTaxGroup.Error.ToString());
                    }
                }
                else if (e.CommandName == "UpdateTaxGroup")
                {
                    using (clsTaxCategoryMaster objTaxGroup = new clsTaxCategoryMaster())
                    {
                        objTaxGroup.TaxCategoryID = Convert.ToInt32(dlList.DataKeys[e.Item.ItemIndex]);
                        objTaxGroup.TaxCategoryName = ((ZedTextBox)e.Item.FindControl("txtTaxGroupE")).Text.Trim();
                        objTaxGroup.CreatedBy = PageBase.UserId;
                        Int16 result = objTaxGroup.Save();
                        ucMessage1.Visible= true;
                        if (result == 0)
                        {
                            ((TextBox)e.Item.FindControl("txtTaxGroupE")).Text = string.Empty;
                            ucMessage1.ShowSuccess(Resources.SuccessMessages.SaveSuccess);
                            dlList.EditItemIndex = -1;
                            BindList(Convert.ToInt32(hdfCurrentPage.Value));
                        }
                        else if (result == 1)
                            ucMessage1.ShowError(objTaxGroup.Error.ToString());
                        else if (result == 2)
                            ucMessage1.ShowError(Resources.ErrorMessages.DuplicateTaxGroup);
                    }
                }
            }
            catch (Exception ex)
            {
                ucMessage1.Visible= true;
                ucMessage1.ShowError(ex.Message);
                PageBase.Errorhandling(ex);
            }
        }
       
        protected void UCPagingControl1_SetControlRefresh()
        {
            try
            {
                hdfCurrentPage.Value = Convert.ToString(ucPagingControl1.CurrentPage);
                BindList(ucPagingControl1.CurrentPage);
            }
            catch (Exception ex)
            {
                ucMessage1.Visible= true;
                ucMessage1.ShowError(ex.Message);
                PageBase.Errorhandling(ex);
            }
        }
        #endregion

        #region Function

        void BindList(int index)
        {
            try
            {
                using (clsTaxCategoryMaster objTaxGroup = new clsTaxCategoryMaster())
                {
                    objTaxGroup.PageSize =Convert.ToInt32(PageBase.PageSize);
                    objTaxGroup.PageIndex = index;
                    objTaxGroup.ActiveStatus = 2;
                    objTaxGroup.ActiveStatus = ucStatus.SelectedValue == "233" ? Convert.ToInt16(2) : (ucStatus.SelectedValue == "255" ? Convert.ToInt16(2) : Convert.ToInt16(ucStatus.SelectedValue));
                    
                    objTaxGroup.TaxCategoryName = txtSearchTaxCategoryName.Text.Trim();
                    DataTable dt = objTaxGroup.SelectAll();
                    if (dt.Rows.Count > 0)
                    {
                        dlList.Visible = true;
                        dlList.DataSource = dt;
                        dlList.DataBind();

                        if (dt == null || dt.Rows.Count == 0)
                        {
                            ucPagingControl1.Visible = false;
                        }
                        else
                        {
                            //Paging
                            ucPagingControl1.CurrentPage = index;
                            ucPagingControl1.Visible = true;
                            ucPagingControl1.PageSize = 20;
                            ucPagingControl1.TotalRecords = objTaxGroup.TotalRecords;
                            ucPagingControl1.FillPageInfo();
                        }
                        updpnlGrid.Update();
                    }
                    else
                    {
                        ucMessage1.ShowInfo(Resources.GlobalMessages.NoRecord);
                        Exporttoexcel.Visible = false;
                        dlList.Visible = true;
                        dlList.DataSource = dt;
                        dlList.DataBind();
                        ucPagingControl1.Visible = false;
                        updpnlGrid.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                ucMessage1.ShowWarning(ex.Message);
                updpnlSaveData.Update();
                PageBase.Errorhandling(ex);
            }
        }
        #endregion
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (ucStatus.SelectedValue == "233" && txtSearchTaxCategoryName.Text.Trim() == "")
                {
                    ucMessage1.ShowWarning(Resources.WarningMessages.FillAnyParameter);
                    return;
                }
                BindList(1);
            }
            catch (Exception ex)
            {
                ucMessage1.ShowWarning(ex.Message);
                updpnlGrid.Update();

            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ucStatus.SelectedValue = "255";
                txtSearchTaxCategoryName.Text = "";
                BindList(1);
            }
            catch (Exception ex)
            {
                ucMessage1.ShowWarning(ex.Message);
                updpnlGrid.Update();

            }
        }
}