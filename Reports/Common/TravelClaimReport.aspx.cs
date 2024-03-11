using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;
using System.IO;
//======================================================================================
//* Developed By : Vijay Prajapati 
//* Role         : Software Developer
//* Module       : Reports(Travel Claim)  
//* Description  :  This page is used for View Travel Claim reports 
//* ====================================================================================
/* Change Log
 * -------------------------------------------------------------------------------------
 * DD-MMM-YYYY, Name, #CCXX, Description
 
 
 */

public partial class Reports_Common_TravelClaimReport : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillEntityType();
            fillBrandCategoryDDL();
            ucFromDate.Date = PageBase.Fromdate;
            ucToDate.Date = PageBase.ToDate;
        }
    }
    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEntityTypeName(Convert.ToInt32(ddlEntityType.SelectedValue));
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ucFromDate.Date != "" && ucToDate.Date == "")
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowWarning("Please Enter  To Date.");
            return;
        }
        if (ucToDate.Date != "" && ucFromDate.Date == "")
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowWarning("Please Enter  From Date.");
            return;
        }
        ucMessage1.Visible = false;
        SearchTravelClaimDetailData(1);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ddlEntityType.SelectedValue = "0";
        if (ddlEntityType.SelectedValue == "0")
        {
            ddlEntityTypeName.SelectedValue = "0";
            ddlEntityTypeName.Items.Clear();
            ddlEntityTypeName.Items.Insert(0, new ListItem("Select", "0"));
        }
        gvTravelClaimDetail.DataSource = null;
        gvTravelClaimDetail.DataBind();
        ucMessage1.Visible = true;
        PnlGrid.Visible = false;
    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        SearchTravelClaimDetailData(-1);
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        SearchTravelClaimDetailData(ucPagingControl1.CurrentPage);
    }
    void FillEntityType()
    {
        using (ClsPaymentReport ObjEntityType = new ClsPaymentReport())
        {

            ddlEntityType.Items.Clear();
            string[] str = { "EntityTypeID", "EntityType" };
            ObjEntityType.CompanyId = PageBase.ClientId;
            ObjEntityType.UserId = PageBase.UserId;
            PageBase.DropdownBinding(ref ddlEntityType, ObjEntityType.GetEntityTypeV5API(), str);

        };
    }
    void FillEntityTypeName(int EntityTypeID)
    {
        using (ClsPaymentReport ObjEntityTypeName = new ClsPaymentReport())
        {

            ddlEntityTypeName.Items.Clear();
            ObjEntityTypeName.EntityTypeId = EntityTypeID;
            ObjEntityTypeName.UserId = PageBase.UserId;
            ObjEntityTypeName.CompanyId = PageBase.ClientId;
            string[] str = { "UserID", "EntityTypeName" };
            PageBase.DropdownBinding(ref ddlEntityTypeName, ObjEntityTypeName.GetEntityTypeName(), str);

        };
    }
    public void SearchTravelClaimDetailData(int pageno)
    {
        ClsPaymentReport objAttendance;
        try
        {
            ViewState["TotalRecords"] = 0;
            ViewState["CurrentPage"] = pageno;
            using (objAttendance = new ClsPaymentReport())
            {
                if (ucFromDate.Date == "" && ucToDate.Date == "")
                { ;}
                else
                {
                    objAttendance.FromDate = Convert.ToDateTime(ucFromDate.Date);
                    objAttendance.Todate = Convert.ToDateTime(ucToDate.Date);
                }
                objAttendance.UserId = PageBase.UserId;
                objAttendance.CompanyId = PageBase.ClientId;
                objAttendance.EntityTypeId = Convert.ToInt32(ddlEntityType.SelectedValue);
                objAttendance.EntitytypeUserId = Convert.ToInt16(ddlEntityTypeName.SelectedValue);
                objAttendance.PageIndex = pageno;
                objAttendance.PageSize = Convert.ToInt32(PageBase.PageSize);
                objAttendance.BrandId = Convert.ToInt32(ddlBrand.SelectedValue);
                objAttendance.ProductCategoryId = Convert.ToInt32(ddlproductcategory.SelectedValue);
                DataSet ds = objAttendance.GetReportTravelData();
                if (objAttendance.TotalRecords > 0)
                {
                    PnlGrid.Visible = true;
                    if (pageno > 0)
                    {
                        gvTravelClaimDetail.DataSource = ds;
                        gvTravelClaimDetail.DataBind();
                        PnlGrid.Visible = true;
                        ViewState["TotalRecords"] = objAttendance.TotalRecords;
                        ucPagingControl1.TotalRecords = objAttendance.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {

                        string FilenameToexport = "TravelDateSummary";
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    }
                }
                else
                {
                    ds = null;
                    gvTravelClaimDetail.DataSource = null;
                    gvTravelClaimDetail.DataBind();
                    ucMessage1.Visible = true;
                    PnlGrid.Visible = false;
                    ucMessage1.ShowInfo("No Record Found.");

                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
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
                ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                PageBase.Errorhandling(ex);

            }
        }
    }
}