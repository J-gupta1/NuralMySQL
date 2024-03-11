using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using ExportExcelOpenXML;
using BussinessLogic;
using System.Data;

public partial class Reports_SalesChannel_StockHoldingUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMsg.ShowControl = false;

        if (!IsPostBack)
        {
            FillsalesChannelType();
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;
        }


    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (!pageValidate())
            {
                return;
            }
           
            hfSearch.Value = "0";
            bindGrid();
        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
      }

    void bindGrid()
    {
        try
        {
            DataSet DsUserInfo;
                using (ReportData objRD = new ReportData())
                {
                    objRD.SalesChannelTypeID =Convert.ToInt32(ddlType.SelectedValue);
                    objRD.DateFrom = Convert.ToDateTime(ucDateFrom.Date);
                    objRD.DateTo = Convert.ToDateTime(ucDateTo.Date);
                    objRD.UserId = PageBase.UserId;
                    DsUserInfo = objRD.GetStockingUserInfo();
                    if (DsUserInfo.Tables[0].Rows.Count > 0)
                    {
                        if (hfSearch.Value == "0")
                        {
                            pnlGrid.Visible = true;
                            grdUser.DataSource = DsUserInfo;
                            grdUser.DataBind();
                        }

                        else
                        {
                            
                                try
                                {
                                    DataTable DsCopy = new DataTable();
                                    DsCopy = DsUserInfo.Tables[0];
                                    DsUserInfo.Tables.Clear();
                                    DsCopy.Columns["HO"].ColumnName = Resources.SalesHierarchy.HierarchyName1;
                                    DsCopy.Columns["RBH"].ColumnName = Resources.SalesHierarchy.HierarchyName2;
                                    DsCopy.Columns["ZBH"].ColumnName = Resources.SalesHierarchy.HierarchyName3;
                                    DsCopy.Columns["SBH"].ColumnName = Resources.SalesHierarchy.HierarchyName4;
                                    DsCopy.Columns["ASO"].ColumnName = Resources.SalesHierarchy.HierarchyName5;
                                    DsUserInfo.Tables.Add(DsCopy);
                                    DsUserInfo.Tables[0].Columns.Remove("SalesChannelID");      //Pankaj Dhingra 18-10-2011
                                    String FilePath = Server.MapPath("../../");
                                    string FilenameToexport = "StockHoldingUsers";
                                    PageBase.RootFilePath = FilePath;
                                    PageBase.ExportToExecl(DsUserInfo, FilenameToexport);
                                }
                                catch (Exception ex)
                                {
                                    ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
                                }
                        }

                    }
                    else
                    {
                        pnlGrid.Visible = false;
                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                    }
            }
        }
        catch (Exception ex)
        {
            ucMsg.ShowInfo(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void exportToExel_Click(object sender, EventArgs e)
    {
        hfSearch.Value = "1";
        bindGrid();

    }
    void FillsalesChannelType()
    {
        using (SalesChannelData ObjSalesChannel = new SalesChannelData())
        {


            string[] str = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref ddlType, ObjSalesChannel.GetSalesChannelType(), str);
            if (PageBase.SalesChanelID != 0)
            {
                ddlType.SelectedValue = PageBase.SalesChanelTypeID.ToString();
                ddlType.Enabled = false;
            }
            else
            {
                ddlType.Enabled = true;
            }
        };
    }
    bool pageValidate()
    {
        int _date = 0;
        if ((Convert.ToDateTime(ucDateFrom.Date) > DateTime.Now.Date) || (Convert.ToDateTime(ucDateTo.Date) > DateTime.Now.Date))
        {
            ucMsg.ShowInfo(Resources.Messages.DateRangeValidation);
            return false;
        }

        _date = DateTime.Compare(Convert.ToDateTime(ucDateTo.Date), Convert.ToDateTime(ucDateFrom.Date));
        if (_date < 0)
        {
            ucMsg.ShowInfo(Resources.Messages.InvalidDate);
            return false;
        }
        return true;
    }
    protected void grdUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdUser.PageIndex = e.NewPageIndex;
        bindGrid();
    }
    
    protected void btncancel_Click(object sender, EventArgs e)
    {
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
        pnlGrid.Visible = false;
        ddlType.SelectedIndex = 0;

    }
  
}
