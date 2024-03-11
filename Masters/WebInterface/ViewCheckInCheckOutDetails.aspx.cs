using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using BussinessLogic;
using System.Data;
using Cryptography;
using System.IO;

public partial class Masters_WebInterface_ViewCheckInCheckOutDetails : PageBase//System.Web.UI.Page
{
    DataSet DtSalesChannelDetail = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {
                FillsalesChannelTypeTsm();
                BindFosTsmName();
                fillBrandCategoryDDL();
                FillGrid(1);

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

    public void BindFosTsmName()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {

            try
            {
                DataTable dt = new DataTable();
                ObjSalesChannel.UserID = Convert.ToInt16(PageBase.UserId);
                ObjSalesChannel.CompanyId = PageBase.ClientId;

                dt = ObjSalesChannel.BindSalesManName();
                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlFosTsmName.DataSource = dt;
                    ddlFosTsmName.DataValueField = "UserID";
                    ddlFosTsmName.DataTextField = "Name";
                    ddlFosTsmName.DataBind();
                    ddlFosTsmName.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    ddlFosTsmName.Items.Insert(0, new ListItem("Select", "0"));
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public void FillsalesChannelTypeTsm()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            try
            {

                if (Convert.ToInt32(PageBase.SalesChanelTypeID) != 0)
                {
                    ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
                }
                DataTable dt = new DataTable();

              //  dt = ObjSalesChannel.GetSalesChannelTypeTsm();
                ObjSalesChannel.UserID = PageBase.UserId;
                ObjSalesChannel.CompanyId = PageBase.ClientId;
                dt = ObjSalesChannel.GetSalesChannelTypeV5API();
                if (dt != null && dt.Rows.Count > 0)
                {
                    cmbsaleschanneltype.DataSource = dt;
                    cmbsaleschanneltype.DataValueField = "SalesChannelTypeID";
                    cmbsaleschanneltype.DataTextField = "SalesChannelTypeName";
                    cmbsaleschanneltype.DataBind();
                    cmbsaleschanneltype.Items.Insert(0, new ListItem("Select", "0"));
                    //cmbsaleschanneltype.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Retailer", "101"));
                }
                else
                {
                    cmbsaleschanneltype.Items.Insert(0, new ListItem("Select", "0"));
                }

            }


            catch (Exception ex)
            {

                throw ex;
            }
        };
    }
    protected void cmbsaleschanneltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSalesChannel();
    }

    public void BindSalesChannel()
    {
        ddlSaleschannelName.Items.Clear();
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            try
            {
                DataTable dt = new DataTable();
                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(cmbsaleschanneltype.SelectedValue);
                ObjSalesChannel.UserID = PageBase.UserId;
                ObjSalesChannel.CompanyId = PageBase.ClientId;
                dt = ObjSalesChannel.BindSalesChannelName();
                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlSaleschannelName.DataSource = dt;
                    ddlSaleschannelName.DataValueField = "SalesChannelID";
                    ddlSaleschannelName.DataTextField = "SalesChannelName";
                    ddlSaleschannelName.DataBind();
                    ddlSaleschannelName.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    ddlSaleschannelName.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (ucDateFrom.Date != "" && ucDateTo.Date == "")
            {
                ucMessage1.Visible = true;
                ucMessage1.ShowWarning("Please Enter Check-Out Date Date.");
                return;
            }
            if (ucDateTo.Date != "" && ucDateFrom.Date == "")
            {
                ucMessage1.Visible = true;
                ucMessage1.ShowWarning("Please Enter Check-In Date.");
                return;
            }
            FillGrid(1);
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }


    void FillGrid(int pageno)
    {
        SalesChannelData ObjSalesChannel;
        ViewState["TotalRecords"] = 0;
        ViewState["CurrentPage"] = pageno;
        using (ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.UserID = PageBase.UserId;
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            if (cmbsaleschanneltype.SelectedValue != "0")
            {

                ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(cmbsaleschanneltype.SelectedValue);
            }
            if (ddlSaleschannelName.SelectedValue != "0")
            {
                ObjSalesChannel.SalesChannelID = Convert.ToInt32(ddlSaleschannelName.SelectedValue);
            }
            if (ddlFosTsmName.SelectedValue != "0")
            {
                ObjSalesChannel.FosTsmName = Convert.ToInt32(ddlFosTsmName.SelectedValue);
            }
            if (ucDateFrom.Date == "" && ucDateTo.Date == "")
            { ;}
            else
            {
                ObjSalesChannel.ToDate1 = Convert.ToDateTime(ucDateTo.Date);
                ObjSalesChannel.FromDate1 = Convert.ToDateTime(ucDateFrom.Date);
            }
            ObjSalesChannel.UserIdapi = Convert.ToInt32(PageBase.UserId);
            ObjSalesChannel.SelectedFOSTSM = Convert.ToInt32(PageBase.UserId);
            ObjSalesChannel.PageIndex = pageno;
            ObjSalesChannel.PageSize = Convert.ToInt32(PageSize);
            ObjSalesChannel.BrandId = Convert.ToInt32(ddlBrand.SelectedValue);
            ObjSalesChannel.ProductCategoryId = Convert.ToInt32(ddlproductcategory.SelectedValue);
            DtSalesChannelDetail = ObjSalesChannel.GetCheckInCheckOutDetails();
            Session["CompanyImageFolder"] = ObjSalesChannel.CompanyImageFolder;
            if (!string.IsNullOrEmpty(ObjSalesChannel.Error))
                ucMessage1.ShowError(ObjSalesChannel.Error);
        };
        if (DtSalesChannelDetail != null && DtSalesChannelDetail.Tables[0].Rows.Count > 0)
        {
            if (pageno > 0)
            {
                ExportToExcel.Visible = true;
                GridSalesChannel.Visible = true;
                GridSalesChannel.DataSource = DtSalesChannelDetail;
                GridSalesChannel.DataBind();
                dvhide.Visible = true;
                ViewState["TotalRecords"] = ObjSalesChannel.TotalRecords;
                ucPagingControl1.TotalRecords = ObjSalesChannel.TotalRecords;
                ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                ucPagingControl1.SetCurrentPage = pageno;
                ucPagingControl1.FillPageInfo();
            }
            else
            {
                string FilenameToexport =  "Check-In Check-out Details";
                PageBase.ExportToExecl(DtSalesChannelDetail, FilenameToexport);
                
            }
        }
        else
        {

            GridSalesChannel.Visible = false;
            GridSalesChannel.DataSource = null;
            GridSalesChannel.DataBind();
            ucMessage1.ShowInfo(Resources.Messages.NoRecord);
            dvhide.Visible = false;
           
        }
    }
    protected void ExportToExcel_Click(object sender, EventArgs e)
    {
        FillGrid(-1);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ucMessage1.ShowControl = false;
        cmbsaleschanneltype.SelectedValue = "0";
        ddlSaleschannelName.SelectedValue = "0";
        ddlFosTsmName.SelectedValue = "0";
        ucDateFrom.Date = "";
        ucDateTo.Date = "";
         dvhide.Visible = false;

    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        FillGrid(ucPagingControl1.CurrentPage);
    }
    protected void DownloadFile(object sender, EventArgs e)
    {
        try
        {
            
            string filePath = (sender as LinkButton).CommandArgument;
            string ExportFileLocation = Server.MapPath(Session["CompanyImageFolder"].ToString());
            string fullpath = ExportFileLocation + "/" + filePath;
            Response.ContentType = "image/jpg";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fullpath));
            Response.WriteFile(fullpath);
            Response.End();
        }
        catch (Exception ex)
        {

            //ucMessage1.Visible = true;
            //ucMessage1.ShowError(ex.Message);
        }
       
    }
    protected void GridSalesChannel_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string IsPrint = GridSalesChannel.DataKeys[e.Row.RowIndex]["ImageUpload"].ToString();
                LinkButton BtnPrint = (LinkButton)e.Row.FindControl("lnkDownload");
                if (IsPrint != "")
                {
                    BtnPrint.Visible = true;
                }
                else
                {
                    BtnPrint.Visible = false;

                }


                Int64 AppvisitId = Convert.ToInt32(GridSalesChannel.DataKeys[e.Row.RowIndex].Value);
                DataTable dtImageDetails = new DataTable();
                using (SalesChannelData objdetail = new SalesChannelData())
                {
                    objdetail.UserID = PageBase.UserId;
                    objdetail.CompanyId = PageBase.ClientId;
                    objdetail.AppVisitId = AppvisitId;
                    dtImageDetails = objdetail.GetUserCheckOutImageInfo();

                }
                DataRow[] drv = dtImageDetails.Select("AppvisitId=" + AppvisitId);
                if (drv.Length > 0)
                {
                    DataTable dtTemp = dtImageDetails.Clone();

                    for (int cntr = 0; cntr < drv.Length; cntr++)
                    {
                        dtTemp.ImportRow(drv[cntr]);
                    }
                    GridView gvAttachedImages = (GridView)e.Row.FindControl("gvCheckoutAttachedImages");
                    gvAttachedImages.DataSource = dtTemp;
                    gvAttachedImages.DataBind();
                }

            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
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
    protected void lnkDownloadCheckoutimage_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = "image/jpg";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.TransmitFile(Server.MapPath(filePath));
            Response.End();
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
}