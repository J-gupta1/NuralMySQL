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

public partial class Reports_Common_PaymentReport : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            FillEntityType();
            FillsalesChannelType();
            SearchPaymentDetailData(1);
            //PnlGrid.Visible = false;
        }
    }
    
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        if(ucFromDate.TextBoxDate.Text!="" && ucToDate.TextBoxDate.Text=="")
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowInfo("Please Enter To Date.");
            return;
        }
        if (ucFromDate.TextBoxDate.Text == "" && ucToDate.TextBoxDate.Text != "")
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowInfo("Please Enter From Date.");
            return;
        }
        SearchPaymentDetailData(1);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        ddlEntityType.SelectedValue = "0";
        ucFromDate.TextBoxDate.Text = "";
        ucToDate.TextBoxDate.Text = "";
        ddlEntityTypeName.SelectedValue = "0";
        ddlsaleschannelType.SelectedValue = "0";
        ddlsaleschannelname.SelectedValue = "0";
    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        SearchPaymentDetailData(-1);
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        SearchPaymentDetailData(ucPagingControl1.CurrentPage);
    }
    void FillEntityType()
    {
        using (ClsPaymentReport ObjEntityType = new ClsPaymentReport())
        {

            ddlEntityType.Items.Clear();
            string[] str = { "EntityTypeID", "EntityType" };
            ObjEntityType.UserId = PageBase.UserId;
            ObjEntityType.CompanyId = PageBase.ClientId;
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
    void FillsalesChannelType()
    {
        using (ClsPaymentReport ObjSalesChannel = new ClsPaymentReport())
        {

            ddlsaleschannelType.Items.Clear();
            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            ObjSalesChannel.UserId = PageBase.UserId;
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            PageBase.DropdownBinding(ref ddlsaleschannelType, ObjSalesChannel.GetSalesChannelTypeV5API(), str);
           // ddlsaleschannelType.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Retailer", "101"));
        };
    }
    void FillSaleschannelName()
    {

        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(ddlsaleschannelType.SelectedValue);
            ddlsaleschannelname.Items.Clear();
            string[] str = { "SalesChannelID", "SalesChannelName" };
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            ObjSalesChannel.UserID = PageBase.UserId;
            PageBase.DropdownBinding(ref ddlsaleschannelname, ObjSalesChannel.BindSalesChannelName(), str);
        };
    }
    protected void ddlEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEntityTypeName(Convert.ToInt32(ddlEntityType.SelectedValue));
    }
    protected void ddlsaleschannelType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSaleschannelName();
    }
    public void SearchPaymentDetailData(int pageno)
    {
        ClsPaymentReport objPayment;
        try
        {
            ViewState["TotalRecords"] = 0;
            ViewState["CurrentPage"] = pageno;
            using (objPayment = new ClsPaymentReport())
            {


                if (ucFromDate.Date == "" && ucToDate.Date == "")
                { ;}
                else
                {
                    objPayment.FromDate = Convert.ToDateTime(ucFromDate.Date);
                    objPayment.Todate = Convert.ToDateTime(ucToDate.Date);
                }
                objPayment.UserId = PageBase.UserId;
                objPayment.CompanyId = PageBase.ClientId;
                objPayment.EntityTypeId = Convert.ToInt32(ddlEntityType.SelectedValue);
                objPayment.EntitytypeUserId = Convert.ToInt16(ddlEntityTypeName.SelectedValue);
                objPayment.SalesChannelTypeID = Convert.ToInt32(ddlsaleschannelType.SelectedValue);
                objPayment.SalesChannelRetailerID = Convert.ToInt32(ddlsaleschannelname.SelectedValue);
                objPayment.PageIndex = pageno;
                objPayment.PageSize = Convert.ToInt32(PageBase.PageSize);

                DataSet ds = objPayment.GetReportPaymentData();
                if (objPayment.TotalRecords > 0)
                {
                    PnlGrid.Visible = true;
                    if (pageno > 0)
                    {
                        gvPaymentDetail.DataSource = ds;
                        gvPaymentDetail.DataBind();
                        ViewState["TotalRecords"] = objPayment.TotalRecords;
                        ucPagingControl1.TotalRecords = objPayment.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {
                        string FilenameToexport = "PaymentDetailData";
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    }
                }
                else
                {
                    ds = null;
                    gvPaymentDetail.DataSource = null;
                    gvPaymentDetail.DataBind();
                    ucMessage1.Visible = true;
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
    protected void DownloadFile(object sender, EventArgs e)
    {
        string filePath = (sender as LinkButton).CommandArgument;
        string ExportFileLocation = HttpContext.Current.Server.MapPath("~");
        string fullpath = ExportFileLocation+"/" + filePath;
        Response.ContentType = "image/jpg";
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fullpath));
        Response.WriteFile(fullpath);
        Response.End();
    }
    protected void gvPaymentDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string IsPrint = gvPaymentDetail.DataKeys[e.Row.RowIndex]["ImageCapture"].ToString();
                LinkButton BtnPrint = (LinkButton)e.Row.FindControl("lnkDownload");
                if (IsPrint != "")
                {   
                    BtnPrint.Visible = true; 
                }
                else
                {
                    BtnPrint.Visible = false;
                   
                }

            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }
}