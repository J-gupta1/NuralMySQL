using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;

public partial class Reports_Common_ViewPhysicalReports : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillsalesChannelType();
            FillSalesmanName();
           //SearchPhysicalReportData(1);
           tblTransactions.Visible = false;
        }

    }
    void FillsalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {

            ddlsaleschanneltype.Items.Clear();
            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            ObjSalesChannel.UserID = PageBase.UserId;
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            PageBase.DropdownBinding(ref ddlsaleschanneltype, ObjSalesChannel.GetSalesChannelTypeV5API(), str);
          
        };
    }
    void FillSaleschannelName()
    {

        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(ddlsaleschanneltype.SelectedValue);
            ObjSalesChannel.UserID = PageBase.UserId;
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            ddlsaleschannelname.Items.Clear();
            string[] str = { "SalesChannelID", "SalesChannelName" };
            PageBase.DropdownBinding(ref ddlsaleschannelname, ObjSalesChannel.BindSalesChannelName(), str);
        };
    }
    protected void ddlsaleschanneltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSaleschannelName();
    }
    void FillSalesmanName()
    {

        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.UserID = PageBase.UserId;
            ObjSalesChannel.CompanyId = PageBase.ClientId;
            ddlfosandtsm.Items.Clear();
            string[] str = { "UserID", "Name" };
            PageBase.DropdownBinding(ref ddlfosandtsm, ObjSalesChannel.BindSalesManName(), str);
        };
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
        PnlGrid.Visible = true;
        tblTransactions.Visible = false;
        SearchPhysicalReportData(1);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        SearchPhysicalReportData(-1);
       
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        SearchPhysicalReportData(ucPagingControl1.CurrentPage);
    }
    public void SearchPhysicalReportData(int pageno)
    {
        SalesChannelData objsaleschannel;
        try
        {
            ViewState["TotalRecords"] = 0;
            ViewState["CurrentPage"] = pageno;
            using (objsaleschannel = new SalesChannelData())
            {


                if (ucFromDate.Date == "" && ucToDate.Date == "")
                { ;}
                else
                {
                    objsaleschannel.Fromdate = Convert.ToDateTime(ucFromDate.Date);
                    objsaleschannel.Todate = Convert.ToDateTime(ucToDate.Date);
                }
                objsaleschannel.UserID = PageBase.UserId;
                objsaleschannel.CompanyId = PageBase.ClientId;
                objsaleschannel.SelectedFOSTSM = Convert.ToInt32(ddlfosandtsm.SelectedValue);
                objsaleschannel.SalesChannelTypeID = Convert.ToInt16(ddlsaleschanneltype.SelectedValue);
                objsaleschannel.SalesChannelID = Convert.ToInt32(ddlsaleschannelname.SelectedValue);
                objsaleschannel.PageIndex = pageno;
                objsaleschannel.PageSize = Convert.ToInt32(PageBase.PageSize);
                DataSet ds = objsaleschannel.GetReportPhysicalData();
                if (objsaleschannel.TotalRecords > 0)
                {
                    if (pageno > 0)
                    {
                        gvPhysicalReportsDetail.DataSource = ds;
                        gvPhysicalReportsDetail.DataBind();

                        ViewState["TotalRecords"] = objsaleschannel.TotalRecords;
                        ucPagingControl1.TotalRecords = objsaleschannel.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {
                        string FilenameToexport = "PhysicalStockData";
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    }
                }
                else
                {
                    ds = null;
                    gvPhysicalReportsDetail.DataSource = null;
                    gvPhysicalReportsDetail.DataBind();
                    ucMessage1.ShowInfo("No Record found.");

                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    private void Cancel()
    {
        PnlGrid.Visible = false;
        ddlfosandtsm.SelectedValue = "0";
        ddlsaleschannelname.SelectedValue = "0";
        ddlsaleschanneltype.SelectedValue = "0";
        ucFromDate.Date = "";
        ucToDate.Date = "";
        tblTransactions.Visible = false;

    }
    protected void gvPhysicalReportsDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("ViewSerialNumber") && e.CommandArgument != "")
        {
            GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int RowIndex = oItem.RowIndex;
            for (int i = 0; i < gvPhysicalReportsDetail.Rows.Count; i++)
            {
                gvPhysicalReportsDetail.Rows[i].BackColor = System.Drawing.Color.White;
            }
            gvPhysicalReportsDetail.Rows[RowIndex].BackColor = System.Drawing.Color.YellowGreen;

            int rowIndex = Convert.ToInt32(e.CommandArgument);
            ViewState["EntityTypeID"] = gvPhysicalReportsDetail.DataKeys[RowIndex].Values[0];
            ViewState["SKUID"] = gvPhysicalReportsDetail.DataKeys[RowIndex].Values[1].ToString();
            ViewState["CreatedBy"] = gvPhysicalReportsDetail.DataKeys[RowIndex].Values[2].ToString();
            ViewState["CreationOn"] = gvPhysicalReportsDetail.DataKeys[RowIndex].Values[3].ToString();
            ViewState["EntityId"] = rowIndex;
           int EntityTypeID=Convert.ToInt32(ViewState["EntityTypeID"]);
           int SKUID = Convert.ToInt32(ViewState["SKUID"]);
           int CreatedBy = Convert.ToInt32(ViewState["CreatedBy"]);
           DateTime CreationOn = Convert.ToDateTime(ViewState["CreationOn"]);


            gvTransactionsBind(Convert.ToInt64(e.CommandArgument), EntityTypeID, SKUID, CreatedBy, CreationOn);
        }
        else
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowError("Data Not In System");
            tblTransactions.Visible = false;
        }
    }
    private void gvTransactionsBind(Int64 EntityId, int EntityTypeID, int SKUID, int CreatedBy, DateTime CreationOn)
    {
        try
        {
            using (SalesChannelData objViewSN = new SalesChannelData())
            {

                DataTable dtResult = new DataTable();

                dtResult = objViewSN.GetSerialTransactions( EntityId, EntityTypeID, SKUID, CreatedBy, CreationOn);
                
                tblTransactions.Visible = true;
                if (dtResult.Rows.Count == 0)
                {
                    ucMessage1.Visible = true;
                    ucMessage1.ShowError(Resources.Messages.NoRecord);
                    tblTransactions.Visible = false;
                }
                gvTransactions.DataSource = null;
                gvTransactions.DataBind();
                gvTransactions.DataSource = dtResult;
                gvTransactions.DataBind();
                ucMessage1.Visible = false;
                tblTransactions.Visible = true;
               
            };
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString());
        }
    }
    protected void gvTransactions_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
         int EntityTypeID=Convert.ToInt32(ViewState["EntityTypeID"]);
           int SKUID = Convert.ToInt32(ViewState["SKUID"]);
           int CreatedBy = Convert.ToInt32(ViewState["CreatedBy"]);
           DateTime CreationOn = Convert.ToDateTime(ViewState["CreationOn"]);
           int EntityId = Convert.ToInt32(ViewState["EntityId"]);
        gvTransactions.PageIndex = e.NewPageIndex;
        gvTransactionsBind(EntityId, EntityTypeID, SKUID, CreatedBy, CreationOn);
    }
}