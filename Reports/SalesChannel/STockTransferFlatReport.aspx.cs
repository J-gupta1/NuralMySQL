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

public partial class Reports_SalesChannel_STockTransferFlatReport : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                btnSearch.CausesValidation = false;
                cmbfill();
            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString(), PageBase.GlobalErrorDisplay());
        }
    }

    void cmbfill()
    {
        using (SalesChannelData obj = new SalesChannelData())
        {
           

            if (PageBase.SalesChanelID == 0 )
            {
                obj.isHOZSM = 1;
            }

            obj.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            obj.isReport = 1;
            DataTable dt = obj.GetSalesChannelTypeFromUser();
            String[] colArray = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref cmbSalesChannelType, dt, colArray);
            cmbSalesChannelType.SelectedValue = Convert.ToString(PageBase.SalesChanelTypeID);
            cmbSalesChannelType.Enabled = false;
            if (PageBase.SalesChanelID == 0)
            {
                cmbSalesChannelType.Enabled = true;
            }
        }
    }


    bool isvalidate()
    {
        if (PageBase.SalesChanelID == 0)
        {

            if (txtSTNNumber.Text == "" && cmbSalesChannelType.SelectedValue == "0")
            {
                ucMessage1.ShowInfo("Please either select  Sales Channel  or   STN Number  as Searching parameter");
                return false;
            }
        }

        else
        {
            if (string.IsNullOrEmpty( ucDateFrom.Date)  && string.IsNullOrEmpty(ucDateTo.Date) && txtSTNNumber.Text.Trim() == "")
            {
                ucMessage1.ShowInfo("Please either select STN No. or a proper date range  ");
                return false;
            }

            if (ucDateFrom.Date == "" && ucDateTo.Date != "")
            {
                ucMessage1.ShowInfo("Please select from date ");
                return false;
            }

            if (ucDateFrom.Date != "" && ucDateTo.Date == "")
            {
                ucMessage1.ShowInfo("Please select from date");
                return false;
            }


        }
        
        if (txtSTNNumber.Text == "")
        {
            if (!string.IsNullOrEmpty(ucDateFrom.Date) && !string.IsNullOrEmpty(ucDateTo.Date))
            {
                if (Convert.ToDateTime(ucDateFrom.Date) > Convert.ToDateTime(ucDateTo.Date))
                {
                    ucMessage1.ShowInfo("The from date must be lesser or equall to the to date ");
                    return false;
                }
            }
            else
            {
                ucMessage1.ShowInfo("Please either select STN No. or a proper date range ");
                return false;
            }

        }

      

        return true;

    }


    void binddata()
    {
        try
        {
            DataTable dt;
            

            using (ReportData obj = new ReportData())
            {
                      obj.STNNo = txtSTNNumber.Text.Trim();
                      obj.SalesChannelId = PageBase.SalesChanelID;
                      obj.SalesTypeID = Convert.ToInt32(cmbSalesChannelType.SelectedValue);
                      obj.FromDate = ucDateFrom.Date;
                      obj.ToDate = ucDateTo.Date;
                      obj.UserId = PageBase.UserId;
                      if (chkSB.Checked)
                      obj.ReportType = 1;
                      else
                          obj.ReportType = 0;
                      dt = obj.GetBTMInfo();

                   if (dt.Rows.Count > 0)
                  {
                    //grdStockTransfer.DataSource = dt;
                    //grdStockTransfer.DataBind();
                    //ViewState["Export"] = dt;
                   
                    //pnlGrid.Visible = true;
                    ucMessage1.Visible = false;
                    //updgrid.Update();
                    exportToExel_Click(dt);

                }
                else
                {
                    ucMessage1.ShowInfo(Resources.Messages.NoRecord);
                    pnlGrid.Visible = false;
                    //updgrid.Update();
                }

            }
        }
        catch (Exception ex)
        {
            ucMessage1.ShowError(ex.Message.ToString(), PageBase.GlobalErrorDisplay());
        }
    }


    protected void btnSerch_Click(object sender, EventArgs e)
    {

        if (!isvalidate())
        {
            pnlGrid.Visible = false;
            return;
        }
        binddata();

    }

    void blankall()
    {
        ucMessage1.Visible = false;
        txtSTNNumber.Text = "";
        ucDateFrom.Date = "";
        ucDateTo.Date = "";
        pnlGrid.Visible = false;
        

    }


    protected void btncancel_Click(object sender, EventArgs e)
    {
        ViewState["Export"] = null;
        blankall();
        pnlGrid.Visible = false;
        //updgrid.Update();
        
        if (PageBase.SalesChanelID == 0)
        {
            cmbSalesChannelType.SelectedValue = "0";
            btnSearch.CausesValidation = false;
        }

    }
    //protected void exportToExel_Click(object sender, EventArgs e)
    void exportToExel_Click(DataTable dt)
    {
        //if (ViewState["Export"] != null)
        //{


            //DataTable dt = (DataTable)ViewState["Export"];
            //string[] DsCol = new string[] { "HOName", "RBHName", "ZSMName", "SBHName", "ASOName","STNNumber", "DocketNumber", "FromSalesChannelName", "ToSalesChannelName", "TransactionDate", "SKUName", "Quantity", "ModelName", "ColorName", "BrandName" };
            DataSet DsCopy = new DataSet();
            //dt = dt.DefaultView.ToTable(true, DsCol);

            if (dt.Rows.Count > 0)
            {
                try
                {
                    //DataSet dtcopy = new DataSet();
                    //dt.Columns["HOName"].ColumnName = Resources.SalesHierarchy.HierarchyName1;
                    //dt.Columns["RBHName"].ColumnName = Resources.SalesHierarchy.HierarchyName2;
                    //dt.Columns["ZSMName"].ColumnName = Resources.SalesHierarchy.HierarchyName3;
                    //dt.Columns["SBHName"].ColumnName = Resources.SalesHierarchy.HierarchyName4;
                    //dt.Columns["ASOName"].ColumnName = Resources.SalesHierarchy.HierarchyName5;
                    DsCopy.Merge(dt);
                    DsCopy.AcceptChanges();
                    //String FilePath = Server.MapPath("../../");
                    //string FilenameToexport = "StockTransferReport";
                    //PageBase.RootFilePath = FilePath;
                    //PageBase.ExportToExeclUsingOPENXMLV2(dtcopy.Tables[0], FilenameToexport);
                    PageBase.ExportToExecl(DsCopy, "StockTransferReport");
                    //ViewState["Export"] = null;
                }
                catch (Exception ex)
                {
                    ucMessage1.ShowError(ex.Message.ToString(), PageBase.GlobalErrorDisplay());
                }
            }
            else
            {
                ucMessage1.ShowError(Resources.Messages.NoRecord);

            }
            ViewState["Export"] = null;
        //}
    }
    protected void grdStockTransfer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdStockTransfer.PageIndex  = e.NewPageIndex;
        binddata();
    }
    protected void cmbSalesChannelType_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlGrid.Visible = false;
        ucMessage1.Visible = false;
        if (cmbSalesChannelType.SelectedValue == "0")
        {
            btnSearch.CausesValidation = false;
           

        }
        else
        {
            btnSearch.CausesValidation = true;

        }


    }
}
