using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;
using DataAccess;
using System.Data;


public partial class Reports_Common_ViewBeatPlanDetail : PageBase 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            FillsalesChannelType();
            FillSalesmanName();
            BindStatus();
            SearchBeatPlanData(1);
            //PnlGrid.Visible = false;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if(ucFromDate.Date!="" && ucToDate.Date=="")
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowWarning("Please Enter Beat Plan To Date.");
            return;
        }
        if(ucToDate.Date!="" && ucFromDate.Date=="")
        {
            ucMessage1.Visible = true;
            ucMessage1.ShowWarning("Please Enter Beat Plan From Date.");
            return;
        }
        ucMessage1.Visible = false;
        PnlGrid.Visible = true;
        SearchBeatPlanData(1);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        SearchBeatPlanData(-1);
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        SearchBeatPlanData(ucPagingControl1.CurrentPage);
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
            //ddlsaleschanneltype.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Retailer", "101"));
        };
    }
    void FillSaleschannelName()
    {
       
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(ddlsaleschanneltype.SelectedValue);
            ddlsaleschannelname.Items.Clear();
            ObjSalesChannel.UserID = PageBase.UserId;
            ObjSalesChannel.CompanyId = PageBase.ClientId;
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
    public void SearchBeatPlanData(int pageno)
    {
        SalesChannelData objsaleschannel ;
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
                objsaleschannel.SelectedFOSTSM = Convert.ToInt32(ddlfosandtsm.SelectedValue);
                objsaleschannel.SalesChannelTypeID = Convert.ToInt16(ddlsaleschanneltype.SelectedValue);
                objsaleschannel.SalesChannelID = Convert.ToInt32(ddlsaleschannelname.SelectedValue);
                objsaleschannel.PageIndex = pageno;
                objsaleschannel.PageSize = Convert.ToInt32(PageBase.PageSize);
                objsaleschannel.ActiveStatus = Convert.ToInt16(ddlStatus.SelectedValue);
                objsaleschannel.CompanyId = PageBase.ClientId;

                DataSet ds = objsaleschannel.GetReportBeatPlanData();
                if (objsaleschannel.TotalRecords > 0)
                {
                    if (pageno > 0)
                    {
                        gvBeatPlanDetail.DataSource = ds;
                        gvBeatPlanDetail.DataBind();

                        ViewState["TotalRecords"] = objsaleschannel.TotalRecords;
                        ucPagingControl1.TotalRecords = objsaleschannel.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {
                        string FilenameToexport = "BeatPlanData";
                        PageBase.ExportToExecl(ds, FilenameToexport);
                    }
                }
                else
                {
                    ds = null;
                    gvBeatPlanDetail.DataSource = null;
                    gvBeatPlanDetail.DataBind();

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

    }
    private void BindStatus()
    {
        DataTable dtresult = new DataTable();
        try
        {
            dtresult = clsDoaReport.GetEnumbyTableName("XML_Enum", "BeatPlanStatus");
            if (dtresult.Rows.Count > 0)
            {
                ddlStatus.DataSource = dtresult;
                ddlStatus.DataTextField = "Description";
                ddlStatus.DataValueField = "Value";
                ddlStatus.DataBind();
                ddlStatus.Items.Insert(0, new ListItem("Select", "255"));
            }
            else
            {
                ddlStatus.Items.Insert(0, new ListItem("Select", "255"));
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString());
        }
    }
}