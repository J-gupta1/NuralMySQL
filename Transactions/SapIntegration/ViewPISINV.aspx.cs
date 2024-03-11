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
/*Change Log:
 * Created on: 03-Feb-20, 
 * Created By:Balram Jha
 * Description : View SAP PIS and Invoice data
 */

public partial class ViewPISINV : PageBase
{
    DataTable DtSalesChannelDetail = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ucMessage1.ShowControl = false;
            if (!IsPostBack)
            {

                HideControls();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    void HideControls()
    {

        GridPSIInv.Visible = false;


    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {

            BindGrid(1);

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    protected void GridPSIInv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridPSIInv.PageIndex = e.NewPageIndex;
            BindGrid(e.NewPageIndex);
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            txtDN.Text = "";
            txtInvoice.Text = "";
            cmbDateType.SelectedValue = "0";
            txtFromCode.Text = "";
            txtToCode.Text = "";
            ucDateFrom.Date = "";
            UcDateTo.Date = "";
            BindGrid(1);
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    void BindGrid(int pageno)
    {
        try
        {
            ViewState["TotalRecords"] = 0;
            using (ClsSAPIntegrationDNPSI ObjPSI = new ClsSAPIntegrationDNPSI())
            {
                ObjPSI.DNNumber = txtDN.Text.Trim();
                ObjPSI.PageSize = Convert.ToInt32(PageSize);
                ObjPSI.PageIndex = Convert.ToInt32(pageno);
                ObjPSI.InvoiceNumber = txtInvoice.Text.Trim();
                ObjPSI.FromCode = txtFromCode.Text.Trim();
                ObjPSI.ToCode = txtToCode.Text.Trim();
                ObjPSI.DateType = Convert.ToInt32(cmbDateType.SelectedValue);
                ObjPSI.UserID = PageBase.UserId;
                ObjPSI.FromDate = Convert.ToString(ucDateFrom.Date);
                ObjPSI.ToDate = Convert.ToString(UcDateTo.Date);
                DataSet ds = new DataSet();
                ds = ObjPSI.GetPSIInvoiceData();
                DtSalesChannelDetail = ds.Tables[0];
                if (pageno > 0)
                {
                    if (DtSalesChannelDetail != null && DtSalesChannelDetail.Rows.Count > 0)
                    {

                        dvFooter.Visible = true;
                        ViewState["TotalRecords"] = ObjPSI.TotalRecords;
                        ucPagingControl1.TotalRecords = ObjPSI.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();

                        ExportToExcel.Visible = true;
                        GridPSIInv.Visible = true;
                        GridPSIInv.DataSource = DtSalesChannelDetail;
                        GridPSIInv.DataBind();
                        dvhide.Visible = true;
                    }
                    else
                    {
                        dvFooter.Visible = false;
                        HideControls();
                        GridPSIInv.Visible = false;
                        GridPSIInv.DataSource = null;
                        GridPSIInv.DataBind();
                        ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                        dvhide.Visible = false;
                    }
                }
                else
                {
                    DataSet dtcopy = new DataSet();
                    dtcopy.Merge(DtSalesChannelDetail);
                    dtcopy.Tables[0].AcceptChanges();
                    String FilePath = Server.MapPath("../../");
                    string FilenameToexport = "PSIDetails";
                    PageBase.RootFilePath = FilePath;
                    PageBase.ExportToExecl(dtcopy, FilenameToexport);
                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
        }
    }

    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;

        BindGrid(ucPagingControl1.CurrentPage);

    }
    protected void ExportToExcel_Click2(object sender, EventArgs e)
    {
        try
        {

            BindGrid(-1);

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    protected void GridPSIInv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button BtnPrint = (Button)e.Row.FindControl("BtnPrint");
                string strId = Convert.ToString(Crypto.Encrypt(GridPSIInv.DataKeys[e.Row.RowIndex]["SAPDNPSIId"].ToString().Replace("+", " "), PageBase.KeyStr));
                BtnPrint.Attributes.Add("OnClick", "window.open('FrmPrintAnnexure.aspx?StockDispatchID=" + strId + " ','mywindow3','menubar=0,width=1000,height=600,left=10,top=10,scrollbars=yes');");



                string IsPrint = GridPSIInv.DataKeys[e.Row.RowIndex]["InvoicePdfPath"].ToString();
                LinkButton BtnDownload = (LinkButton)e.Row.FindControl("lnkDownload");
                if (IsPrint != "")
                {
                    BtnDownload.Visible = true;
                }
                else
                {
                    BtnDownload.Visible = false;

                }
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);
           
        }
    }
    protected void DownloadFile(object sender, EventArgs e)
    {
        try
        {
            string filePath = (sender as LinkButton).CommandArgument;

           // string filePath = "E:\\WebProject\\ZedSalesV5\\Web\\Excel\\Upload\\SAPIntegration\\UploadedPdfInvoice\\Invoice13Feb2020.pdf";
            string ExportFileLocation = HttpContext.Current.Server.MapPath("~");
          //  string fullpath = ExportFileLocation + "/" + filePath;
            string fullpath = filePath;
            Response.ContentType = "image/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fullpath));
            Response.WriteFile(fullpath);
            Response.End();
        }
        catch (Exception ex)
        {

           
        }

    }

    protected void BtnCancelPSI_Click(object sender, EventArgs e)
    {
        try
        {
            int outParam;
            using (ClsSAPIntegrationDNPSI ObjPSI = new ClsSAPIntegrationDNPSI())
            {
                Int64 PSIID = Convert.ToInt64( (sender as Button).CommandArgument);
                ObjPSI.PSIID = PSIID;
                
                ObjPSI.UserID = PageBase.UserId;


                outParam = ObjPSI.CancelPsi();
                if (outParam == 0)
                {
                    ucMessage1.ShowSuccess("PSI cancelled successfully.");
                    BindGrid(1);
                }
                else if (outParam > 0 && !string.IsNullOrEmpty(ObjPSI.OutError))
                {
                    ucMessage1.ShowError(ObjPSI.OutError);
                }
                else if (outParam > 0 && string.IsNullOrEmpty(ObjPSI.OutError))
                {
                    ucMessage1.ShowError("Error occured in cancelation of PSI.");
                }

            }
        }
        catch (Exception ex)
        {


        }
    }
}