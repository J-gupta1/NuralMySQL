﻿#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ================================================================================================
 * ================================================================================================
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd.
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * ================================================================================================
 * Created By : Rakesh Raj
 * Created On : 23 July 2018
 * Description : Display Feedback Details added from Mobile App
 * ================================================================================================
 * Change Log: 
 * ------------- 
 * DD-MMM-YYYY, Name, #CCXX, Description
 * ====================================================================================================
 
 */
#endregion

using System;
using System.Data;
using BussinessLogic;
using DataAccess;

public partial class Masters_SalesMan_ViewFeedbackDetails : PageBase
{
    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            ucMsg.Visible = false;
            if (!IsPostBack)
            {
                FillsalesChannelType();
                FillSalesmanName();
                fillBrandCategoryDDL();
                FillModel();
                FillCategory();
                GetSearchData(1);
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }

    }
    protected void btnExprtToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            GetSearchData(-1);
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }
    protected void btnSearchUser_Click(object sender, EventArgs e)
    {
        //if (ddlFOSTSM.SelectedIndex == 0 & ddlSalesChannelType.SelectedIndex == 0
        //    & txtCompanyName.Text.Trim().Length == 0 & ucToDate.Date == "" & ucFromDate.Date == "" & 
        //    ddlModel.SelectedIndex ==0 & ddlCategory.SelectedIndex ==0 &txtFeedbackNo.Text == "" )
        //{
        //    //Please select at least one search criteria!
        //    ucMsg.Visible = true;
        //    ucMsg.ShowInfo(Resources.Messages.SearchCriteriaBlank);
        //    return;
        //}

        GetSearchData(1);

    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        ClearForm();
        GetSearchData(1);
    }

    #endregion

    #region Functions

    void ClearForm()
    {
        txtFeedbackNo.Text = "";
        txtCompanyName.Text = "";
        ddlModel.SelectedIndex=0;
        ddlCategory.SelectedIndex = 0;
        ddlFOSTSM.SelectedIndex = 0;
        ddlSalesChannelType.SelectedIndex = -1;
        ucFromDate.Date = "";
        ucToDate.Date = "";
        
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        GetSearchData(ucPagingControl1.CurrentPage);

    }
    void FillSalesmanName()
    {

        using (SalesmanData ObjSalesChannel = new SalesmanData())
        {
            ObjSalesChannel.UserID = PageBase.UserId;
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            ddlFOSTSM.Items.Clear();
            string[] str = { "UserID", "Name" };
            PageBase.DropdownBinding(ref ddlFOSTSM, ObjSalesChannel.BindSalesManNameForFeedback(), str);
        };
    }

    void FillCategory()
    {

        using (SalesmanData ObjSalesChannel = new SalesmanData())
        {
            ObjSalesChannel.UserID = PageBase.UserId;
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            ddlCategory.Items.Clear();
            string[] str = { "CategoryID", "CategoryName" };
            PageBase.DropdownBinding(ref ddlCategory, ObjSalesChannel.BindDropDown(1), str);
        };
    }

    void FillModel()
    {

        using (SalesmanData ObjSalesChannel = new SalesmanData())
        {
            ObjSalesChannel.UserID = PageBase.UserId;
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            ddlModel.Items.Clear();
            string[] str = { "ModelID", "ModelName" };
            PageBase.DropdownBinding(ref ddlModel, ObjSalesChannel.BindDropDown(2), str);
        };
    }

    void FillsalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {

            ddlSalesChannelType.Items.Clear();
            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            ObjSalesChannel.UserID = PageBase.UserId;
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            PageBase.DropdownBinding(ref ddlSalesChannelType, ObjSalesChannel.GetSalesChannelTypeV5API(), str);
            

        };
    }
    private void GetSearchData(int pageno)
    {
        ViewState["TotalRecords"] = 0;
        

        if (ViewState["CurrentPage"] == null)
        {
            pageno = 1;
            ViewState["CurrentPage"] = pageno;
        }
        try{
        DataSet dtSalesMan;
        using (SalesmanData objSalesMan = new SalesmanData())
        {
            if (ucFromDate.Date == "" && ucToDate.Date == "")
            { }
            else
            {
                objSalesMan.FromDate= Convert.ToDateTime(ucFromDate.Date);
                objSalesMan.ToDate = Convert.ToDateTime(ucToDate.Date);
            }

            if (txtFeedbackNo.Text.Trim().Length > 0)
            {
                objSalesMan.ProspectId = Convert.ToInt32(txtFeedbackNo.Text.Trim());
            }
           
            objSalesMan.CompanyName = txtCompanyName.Text.Trim(); // SalesChannelName


            if (ddlModel.SelectedIndex > 0)
            {
                objSalesMan.Address = ddlModel.SelectedItem.Text.Trim(); // ModelName
            }
           
            if (ddlFOSTSM.SelectedIndex > 0)
            {
                objSalesMan.Salesmanname = ddlFOSTSM.SelectedValue.ToString();
            }
           
            if (ddlSalesChannelType.SelectedIndex > 0)
            {
                objSalesMan.SalesChannelType = ddlSalesChannelType.SelectedItem.Text.Trim();
            }
           
            if (ddlCategory.SelectedIndex > 0)
            {
                objSalesMan.ContactNo = ddlCategory.SelectedItem.Text; // Category
            }
           
            //objSalesMan.ProspectId = PageBase.SalesChanelID;
            objSalesMan.UserID = PageBase.UserId;
            objSalesMan.CompanyId = PageBase.ClientId;
            objSalesMan.PageIndex = pageno;
            objSalesMan.PageSize = Convert.ToInt32(PageSize);
            objSalesMan.BrandId = Convert.ToInt32(ddlBrand.SelectedValue);
            objSalesMan.ProductCategoryId = Convert.ToInt32(ddlproductcategory.SelectedValue);
            dtSalesMan = objSalesMan.GeFeedbackDetails();

                if (objSalesMan.TotalRecords > 0)
                {
                    if (pageno > 0)
                    {
                        ucPagingControl1.Visible = true;
                        btnExprtToExcel.Visible = true;
                        gvSalesMan.DataSource = dtSalesMan;
                        gvSalesMan.DataBind();
                        ViewState["TotalRecords"] = objSalesMan.TotalRecords;
                        ucPagingControl1.TotalRecords = objSalesMan.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {
                        ucPagingControl1.Visible = true;
                        btnExprtToExcel.Visible = true;
                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "FeedbackList";
                        PageBase.RootFilePath = FilePath;
                        PageBase.ExportToExecl(dtSalesMan, FilenameToexport);

                    }

                }
                else
                {
                    dtSalesMan = null;
                    gvSalesMan.DataSource = null;
                    gvSalesMan.DataBind();
                    btnExprtToExcel.Visible = false;
                    ucPagingControl1.Visible = false;
                    ucMsg.ShowInfo("No record found.");
                }
        
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }
    public void fillBrandCategoryDDL()
    {

        using (ProductData objproduct = new ProductData())
        {

            try
            {
                objproduct.CompanyId = PageBase.ClientId;
                DataTable dtbrandfil = objproduct.SelectAllBrandInfo();
                String[] colArray = { "BrandID", "BrandName" };
                PageBase.DropdownBinding(ref ddlBrand, dtbrandfil, colArray);
                ddlBrand.SelectedValue = "0";

                DataTable dtprodcatfil = objproduct.SelectAllProdCatInfo();
                String[] colArray1 = { "ProductCategoryID", "ProductCategoryName" };
                PageBase.DropdownBinding(ref ddlproductcategory, dtprodcatfil, colArray1);
                ddlproductcategory.SelectedValue = "0";

            }
            catch (Exception ex)
            {
                ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);
            }
        }
    }

    #endregion
}
