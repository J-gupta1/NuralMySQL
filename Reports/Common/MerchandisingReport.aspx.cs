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

public partial class Reports_Common_MerchandisingReport : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillsalesChannelType();
            FillSalesmanName();
            BindStatus();
            ucFromDate.Date = PageBase.Fromdate;
            ucToDate.Date = PageBase.ToDate;
            
        }
    }
    void FillsalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {

            ddlsaleschanneltype.Items.Clear();
            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref ddlsaleschanneltype, ObjSalesChannel.GetSalesChannelTypeV5API(), str);
        };
    }
    void FillSalesmanName()
    {

        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.UserID = PageBase.UserId;
            ddlfosandtsm.Items.Clear();
            string[] str = { "UserID", "Name" };
            PageBase.DropdownBinding(ref ddlfosandtsm, ObjSalesChannel.BindSalesManName(), str);
        };
    }
    void FillSaleschannelName()
    {

        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {
            ObjSalesChannel.SalesChannelTypeID = Convert.ToInt16(ddlsaleschanneltype.SelectedValue);
            ddlsaleschannelname.Items.Clear();
            string[] str = { "SalesChannelID", "SalesChannelName" };
            PageBase.DropdownBinding(ref ddlsaleschannelname, ObjSalesChannel.BindSalesChannelName(), str);
        };
    }
    protected void ddlsaleschanneltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        FillSaleschannelName();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchMerchandisingData(1);
    }
    protected void btnExportexcel_Click(object sender, EventArgs e)
    {
        SearchMerchandisingData(-1);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    protected void UCPagingControl1_SetControlRefresh()
    {
        ucMessage1.Visible = false;
        ViewState["CurrentPage"] = ucPagingControl1.CurrentPage;
        SearchMerchandisingData(ucPagingControl1.CurrentPage);
    }
    private void BindStatus()
    {
        DataTable dtresult = new DataTable();
        try
        {
            dtresult = GetEnumbyTableName("XML_Enum", "MerchandisingStatus");
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
    private void Cancel()
    {
      
        ddlfosandtsm.SelectedValue = "0";
       
        ddlsaleschanneltype.SelectedValue = "0";
        if(ddlsaleschanneltype.SelectedValue=="0")
        {
            ddlsaleschannelname.SelectedValue = "0";
            ddlsaleschannelname.Items.Clear();
            ddlsaleschannelname.Items.Insert(0,new ListItem("Select","0"));
        }
        ucMessage1.Visible = false;

    }
    public void SearchMerchandisingData(int pageno)
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
                objsaleschannel.SelectedFOSTSM = Convert.ToInt32(ddlfosandtsm.SelectedValue);
                objsaleschannel.SalesChannelTypeID = Convert.ToInt16(ddlsaleschanneltype.SelectedValue);
                objsaleschannel.SalesChannelID = Convert.ToInt32(ddlsaleschannelname.SelectedValue);
                objsaleschannel.PageIndex = pageno;
                objsaleschannel.PageSize = Convert.ToInt32(PageBase.PageSize);
                objsaleschannel.ActiveStatus = Convert.ToInt16(ddlStatus.SelectedValue);
                DataSet ds = objsaleschannel.GetReportMerchandsingData();
                if (objsaleschannel.TotalRecords > 0)
                {
                    if (pageno > 0)
                    {
                        gvMerchandisingDetail.DataSource = ds;
                        gvMerchandisingDetail.DataBind();
                        PnlGrid.Visible = true;
                        dvFooter.Visible = true;
                        ucMessage1.Visible = false;
                        ViewState["TotalRecords"] = objsaleschannel.TotalRecords;
                        ucPagingControl1.TotalRecords = objsaleschannel.TotalRecords;
                        ucPagingControl1.PageSize = Convert.ToInt32(PageSize);
                        ucPagingControl1.SetCurrentPage = pageno;
                        ucPagingControl1.FillPageInfo();
                    }
                    else
                    {
                       
                            string FilenameToexport = "MerchandisingData";
                            PageBase.ExportToExecl(ds, FilenameToexport);
                       
                    }
                }
                else
                {
                    ds = null;
                    gvMerchandisingDetail.DataSource = null;
                    gvMerchandisingDetail.DataBind();
                    dvFooter.Visible = false;
                    PnlGrid.Visible = false;
                    ucMessage1.Visible = true;
                    ucMessage1.ShowInfo("No Record Found!");  

                }
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void gvMerchandisingDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string IsPrint = gvMerchandisingDetail.DataKeys[e.Row.RowIndex]["ImagePath"].ToString();
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
    protected void DownloadFile(object sender, EventArgs e)
    {
        try
        {
            string filePath = (sender as LinkButton).CommandArgument;
            string ExportFileLocation = HttpContext.Current.Server.MapPath("~");
            string fullpath = ExportFileLocation + "/" + filePath;
            Response.ContentType = "image/jpg";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fullpath));
            Response.WriteFile(fullpath);
            Response.End();
        }
        catch (Exception ex)
        {
        }

    }
    public static DataTable GetEnumbyTableName(string Filename, string TableName)
    {
        DataTable dt = new DataTable();
        using (DataSet ds = new DataSet())
        {
            string filename = HttpContext.Current.Server.MapPath("~/Assets/XML/" + Filename + ".xml");
            ds.ReadXml(filename);
            dt = ds.Tables[TableName];
            if (dt == null || dt.Rows.Count == 0)
                return null;
        }
        try
        {
            dt = dt.Select("Active=1").CopyToDataTable();
            return dt;
        }
        catch (Exception)
        {
            return null;
        }
    }
}