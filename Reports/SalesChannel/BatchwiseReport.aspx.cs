using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BussinessLogic;
using DataAccess;
using System.Data.SqlClient;
/*Change Log:
 * 08-May-2014, Rakesh Goel, #CC01 - Changed export to excel call to EPP method instead of OpenXML.
 * Also added exception display in catch block
 * 
 */

public partial class Reports_SalesChannel_BatchwiseReport : PageBase
{
    DataTable dtBatch;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ucMsg.Visible = false;
            if (!ValidateControls())
            {
                pnlGrid.Visible = false;
                return;
            }
            GridData();
        }
        catch (Exception ex)
        {
            pnlGrid.Visible = false;
            ucMsg.ShowError(ex.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearContents();

    }
    bool ValidateControls()
    {
        if (txtBatchNumber.Text.Trim() == "" && txtSkuCode.Text.Trim() == "" && ucBatchDateFrom.Date == "" && ucBatchDateTo.Date == "")
        {
            ucMsg.ShowInfo("Please insert any search Criteria!");
            return false;

        }
        if (ucBatchDateTo.Date != "")
        {
            if (ucBatchDateFrom.Date == "")
            {
                ucMsg.ShowInfo("Invalid Date Range!");
                return false;
            }
        }

        if (ucBatchDateFrom.Date != "")
        {
            if (ucBatchDateTo.Date == "")
            {
                ucMsg.ShowInfo("Invalid Date Range!");
                return false;
            }
        }
        if (ucBatchDateFrom.Date != "" && ucBatchDateTo.Date != "")
        {
            int intdate;
            intdate = DateTime.Compare(Convert.ToDateTime(ucBatchDateTo.Date), Convert.ToDateTime(ucBatchDateFrom.Date));
            if (intdate < 0)
            {
                ucMsg.ShowInfo(Resources.Messages.InvalidDate);
                return false;
            }
        }
        return true;

    }

    public void GridData()
    {
        try
        {
            dtBatch = new DataTable();
            using (POC objBatch = new POC())
            {

                if (ucBatchDateFrom.Date != "")
                    objBatch.DateFrom = ucBatchDateFrom.Date;
                if (ucBatchDateTo.Date != "")
                    objBatch.DateTo = ucBatchDateTo.Date;

                objBatch.BatchCode = txtBatchNumber.Text.Trim();
                objBatch.SkuCode = txtSkuCode.Text.Trim();
                objBatch.SalesChannelId = PageBase.SalesChanelID;
                objBatch.UserId = PageBase.UserId;
                dtBatch = objBatch.GetBatchStockDetailInfo();
                if (dtBatch.Rows.Count > 0)
                {
                    GridBatchStock.DataSource = dtBatch;
                    GridBatchStock.DataBind();
                    pnlGrid.Visible = true;
                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                    pnlGrid.Visible = false;
                }

            }
        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.Message.ToString(), PageBase.GlobalErrorDisplay());
        }

    }
    protected void GridBatchStock_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridBatchStock.PageIndex = e.NewPageIndex;
        GridData();
    }
    void ClearContents()
    {
        ucMsg.Visible = false;
        ucBatchDateFrom.Date = "";
        ucBatchDateTo.Date = "";
        txtBatchNumber.Text = "";
        txtSkuCode.Text = "";
        pnlGrid.Visible = false;
        GridBatchStock.DataSource = null;
        GridBatchStock.DataBind();
        updGrid.Update();

    }
    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            dtBatch = new DataTable();

            using (POC objBatch = new POC())
            {
                objBatch.BatchCode = txtBatchNumber.Text.Trim();
                objBatch.SkuCode = txtSkuCode.Text.Trim();
                objBatch.SalesChannelId = PageBase.SalesChanelID;
                objBatch.UserId = PageBase.UserId;
                dtBatch = objBatch.GetBatchStockDetailInfo();

            }
            if (dtBatch.Rows.Count > 0)
            {

                /*#CC01 comment start
                DataSet dtcopy = new DataSet();

                dtcopy.Merge(dtBatch);
                dtcopy.Tables[0].AcceptChanges();
                 #CC01 comment end*/ 
                String FilePath = Server.MapPath("../../");
                string FilenameToexport = "BatchWiseReport";
                PageBase.RootFilePath = FilePath;
                //PageBase.ExportToExeclUsingOPENXMLV2(dtcopy.Tables[0], FilenameToexport);  //#CC01 commented
                PageBase.ExportToExecl(dtBatch.DataSet, FilenameToexport);  //#CC01 added
            }
            else
            {
                ucMsg.ShowError(Resources.Messages.NoRecord);

            }
        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
        }
    }
}