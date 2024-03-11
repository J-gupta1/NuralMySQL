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
 * Created on: 09-Mar-2020, 
 * Created By:Balram Jha
 * Description : Sales return request approval
 */

public partial class SalesReturnApproval : PageBase
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

        GridSRQ.Visible = false;


    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            dvDetail.Visible = false;
            ViewState["SalesReturnReqID"] = null;
            BindGrid(1);

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }
    }
    protected void GridSRQ_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridSRQ.PageIndex = e.NewPageIndex;
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
            dvDetail.Visible = false;
            ViewState["SalesReturnReqID"] = null;
            txtSalesChannel.Text = "";
            txtSCCode.Text = "";
            cmbRequestStatus.SelectedIndex = 0;
            cmbReturnType.SelectedIndex = 0;
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
            using (SalesData ObjSale = new SalesData())
            {
                ObjSale.PageSize = Convert.ToInt32(PageSize);
                ObjSale.PageIndex = Convert.ToInt32(pageno);
                ObjSale.SalesChannelName = txtSalesChannel.Text.Trim();
                ObjSale.SalesChannelCode = txtSCCode.Text.Trim();
                ObjSale.SalesChannelID = PageBase.SalesChanelID;
                ObjSale.ReturnType = Convert.ToInt32(cmbReturnType.SelectedValue);
                ObjSale.UserID = PageBase.UserId;
                if (!string.IsNullOrEmpty( ucDateFrom.Date))
                ObjSale.FromDate = Convert.ToDateTime(ucDateFrom.Date);
                if (!string.IsNullOrEmpty(UcDateTo.Date))
                ObjSale.ToDate = Convert.ToDateTime(UcDateTo.Date);
                ObjSale.Status = Convert.ToInt32( cmbRequestStatus.SelectedValue);
                DataSet ds = new DataSet();
                ds = ObjSale.dsSalesReturnRequestData();
                DtSalesChannelDetail = ds.Tables[0];
                if (pageno > 0)
                {
                    if (DtSalesChannelDetail != null && DtSalesChannelDetail.Rows.Count > 0)
                    {

                        dvFooter.Visible = true;
                        ViewState["TotalRecords"] = ObjSale.TotalRecords;
                        ucPagingControl1.TotalRecords = ObjSale.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();

                        ExportToExcel.Visible = true;
                        GridSRQ.Visible = true;
                        GridSRQ.DataSource = DtSalesChannelDetail;
                        GridSRQ.DataBind();
                        dvhide.Visible = true;
                    }
                    else
                    {
                        dvFooter.Visible = false;
                        HideControls();
                        GridSRQ.Visible = false;
                        GridSRQ.DataSource = null;
                        GridSRQ.DataBind();
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
    protected void BtnDetail_Click(object sender, EventArgs e)
    {
        try
        {
            
            using (SalesData ObjSale = new SalesData())
            {
                Int64 SalesReturnReqID = Convert.ToInt64( (sender as Button).CommandArgument);
                ObjSale.SecondarySalesReturnMainID = SalesReturnReqID;
                
                ObjSale.UserID = PageBase.UserId;


                DataSet dsDetail = ObjSale.dsSalesReturnRequestDetailData();
                if (dsDetail != null && dsDetail.Tables.Count > 0 && dsDetail.Tables[0].Rows.Count > 0)
                {
                    GridDetail.DataSource = dsDetail.Tables[0];
                    GridDetail.DataBind();
                    ViewState["SalesReturnReqID"] = SalesReturnReqID.ToString();
                    lblReturnRequestNo.Text = dsDetail.Tables[0].Rows[0]["ReturnRequestNumber"].ToString();
                    dvDetail.Visible = true;
                    
                }
                else
                {
                    ViewState["SalesReturnReqID"] = null;
                    dvDetail.Visible = false;
                }

            }
        }
        catch (Exception ex)
        {


        }
    }
    protected void btnAccept_Click(object sender, EventArgs e)
    {
        try
        {

            using (SalesData ObjSale = new SalesData())
            {
                if(string.IsNullOrEmpty( Convert.ToString( ViewState["SalesReturnReqID"])))
                {
                    ucMessage1.ShowError("No Detail for Sales Return approval.");
                    return;
                }
                Int64 SalesReturnReqID = Convert.ToInt64(ViewState["SalesReturnReqID"]);
                ObjSale.SecondarySalesReturnMainID = SalesReturnReqID;

                ObjSale.UserID = PageBase.UserId;
                ObjSale.ApproveStatus = 1;
                ObjSale.Remark = txtApprovalRemark.Text.Trim();

                int intOutParam= ObjSale.ApproveRejectSalesReturnRequest();

                if (intOutParam == 0)
                {
                    BindGrid(1);
                    ucMessage1.ShowSuccess("Record updated successfully.");
                }
                else
                {
                    if (!string.IsNullOrEmpty(ObjSale.Error))
                        ucMessage1.ShowError(ObjSale.Error);
                    else
                        ucMessage1.ShowError("Error occured in approval.");
                }

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message);

        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {

            using (SalesData ObjSale = new SalesData())
            {
                if (string.IsNullOrEmpty(Convert.ToString(ViewState["SalesReturnReqID"])))
                {
                    ucMessage1.Visible = true;
                    ucMessage1.ShowError("No Detail for Sales Return rejection.");
                    return;
                }
                else if(txtApprovalRemark.Text=="")
                {
                    ucMessage1.Visible = true;
                    ucMessage1.ShowError("Please Enter Remarks Field.");
                    return;
                }
                Int64 SalesReturnReqID = Convert.ToInt64(ViewState["SalesReturnReqID"]);
                ObjSale.SecondarySalesReturnMainID = SalesReturnReqID;

                ObjSale.UserID = PageBase.UserId;
                ObjSale.ApproveStatus = 2;
                ObjSale.Remark = txtApprovalRemark.Text.Trim();

                int intOutParam = ObjSale.ApproveRejectSalesReturnRequest();

                if (intOutParam == 0)
                {
                    BindGrid(1);
                    ucMessage1.ShowSuccess("Record updated successfully.");
                }
                else
                {
                    if (!string.IsNullOrEmpty(ObjSale.Error))
                        ucMessage1.ShowError(ObjSale.Error);
                    else
                        ucMessage1.ShowError("Error occured in rejection.");
                }

            }
        }
        catch (Exception ex)
        {

            ucMessage1.ShowError(ex.Message);
        }
    }
}