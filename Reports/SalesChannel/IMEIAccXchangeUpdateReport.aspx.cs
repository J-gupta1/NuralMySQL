using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataAccess;
using BussinessLogic;


public partial class Reports_SalesChannel_IMEIAccXchangeUpdateReport : PageBase //System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ucMessage1.Visible = false;
        if (!IsPostBack)
        {
            FillND();
            ucFromDate.Date = PageBase.Fromdate;
            ucToDate.Date = PageBase.ToDate;
            imgDownloadMappingInfo.ImageUrl = "~/" + PageBase.strAssets + "/CSS/Images/page_excel.png";

        }
    }

    void FillND()
    {
        try
        {
            ddlND.Enabled = true;
            using (SalesChannelData ObjSalesChannel = new SalesChannelData())
            {
                ObjSalesChannel.SalesChannelTypeID = 7;
                DataSet ds = ObjSalesChannel.GetNDRDS();
                DataTable dt = ds.Tables[0];
                String[] StrCol = new String[] { "SalesChannelID", "SalesChannelName" };
                ddlND.Items.Clear();
                PageBase.DropdownBinding(ref ddlND, dt, StrCol);
                if (Convert.ToInt32(PageBase.SalesChanelID) != 0)
                {
                    ddlND.SelectedValue = PageBase.SalesChanelID.ToString();
                    ddlND.Enabled = false;
                }
                ddlND_SelectedIndexChanged(null, null);

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }

    protected void ddlND_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlND.SelectedValue) > 0)
            {
                ddlRDS.Enabled = true;
                using (SalesChannelData ObjSalesChannel = new SalesChannelData())
                {
                    ObjSalesChannel.ParentSalesChannelID = Convert.ToInt32(ddlND.SelectedValue);
                    DataSet ds = ObjSalesChannel.GetNDRDS();
                    if (ds.Tables.Count > 1)
                    {
                        ddlRDS.Items.Clear();
                        DataTable dt = ds.Tables[1];
                        String[] StrCol = new String[] { "SalesChannelID", "SalesChannelName" };
                        PageBase.DropdownBinding(ref ddlRDS, dt, StrCol);
                    }
                }
            }
            else
            {
                ddlRDS.Items.Clear();
                ddlRDS.Items.Insert(0, new ListItem("Select", "0"));
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }
    protected void lnkExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToDateTime(ucFromDate.Date) < Convert.ToDateTime(ucToDate.Date))
            {
                // System.TimeSpan Days = Convert.ToDateTime(ucToDate.Date).Subtract(Convert.ToDateTime(ucFromDate.Date));
                string daysDiff = (Convert.ToDateTime(ucToDate.Date) - Convert.ToDateTime(ucFromDate.Date)).TotalDays.ToString();
                //ucMessage1.ShowInfo("No. of days are:" + diff2);
                if (Convert.ToInt32(daysDiff) > 31)
                {
                    ucMessage1.Visible = true;
                    ucMessage1.ShowInfo("Maximum 31 days diffrence allowed.");
                    return;
                }
                else
                {
                    ucMessage1.Visible = false;
                    SalesChannelData objsales = new SalesChannelData();
                    objsales.NDID = Convert.ToInt32(ddlND.SelectedValue);
                    objsales.RDSID = Convert.ToInt32(ddlRDS.SelectedValue);
                    objsales.Fromdate = Convert.ToDateTime(ucFromDate.Date);
                    objsales.Todate = Convert.ToDateTime(ucToDate.Date);
                    DataSet ds = objsales.GetImeiAccExchangeReportData();
                    if (objsales.TotalRecords > 0)
                    {
                        DataTable dtCloned = ds.Tables[0].Clone();
                        for (int j = 2; j < ds.Tables[0].Columns.Count; j++)
                        {
                            dtCloned.Columns[j].DataType = typeof(string);

                        }
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            dtCloned.ImportRow(row);
                        }

                        DataSet ds1 = new DataSet();
                        ds1.Tables.Add(dtCloned);
                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                        {

                            for (int j = 3; j < ds1.Tables[0].Columns.Count; j++)
                            {


                                ds1.Tables[0].Rows[i][j] = (ds1.Tables[0].Rows[i][j].ToString() == "0" ? "N" : "Y").ToString();
                                ds1.Tables[0].AcceptChanges();
                            }
                        }

                        String FilePath = Server.MapPath("~/");
                        string FilenameToexport = "IMEIAccXchangeReport";
                        PageBase.RootFilePath = FilePath;
                        PageBase.ExportToExecl(ds1, FilenameToexport);
                    }
                    else
                    {
                        ucMessage1.ShowInfo("No record found.");
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }
}
