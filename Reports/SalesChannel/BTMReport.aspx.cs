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
using DevExpress.Utils;
using System.Configuration;
using DevExpress.Web.ASPxPivotGrid;

public partial class Reports_SalesChannel_BTMReport : PageBase
{
    

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            pnlSearch.Visible = false;
            ucMsg.Visible = false;
            if ( hfSearch.Value == "1")
                binddata();

            if (!IsPostBack)
            {
                cmbfill();
                btnSearch.CausesValidation = false;
            }

        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.Message.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }
    }


    void cmbfill()
    {
        using (SalesChannelData obj = new SalesChannelData())
        {
            ucDateFrom.Date = PageBase.Fromdate;
            ucDateTo.Date = PageBase.ToDate;

            if (PageBase.HierarchyLevelID == 2 || PageBase.HierarchyLevelID == 4)
            {
                obj.isHOZSM = 1;
            }

            obj.SalesChannelTypeID = Convert.ToInt16(PageBase.SalesChanelTypeID);
            obj.isReport = 1;
            DataTable dt = obj.GetSalesChannelTypeFromUser();
            String[] colArray = { "SalesChannelTypeID", "SalesChannelTypeName" };
            PageBase.DropdownBinding(ref cmbFrom, dt, colArray);
            cmbFrom.SelectedValue = Convert.ToString(PageBase.SalesChanelTypeID);
            cmbFrom.Enabled = false;
            if (PageBase.HierarchyLevelID == 2 || PageBase.HierarchyLevelID == 4)
            {
                cmbFrom.Enabled = true;
            }
        }
    }

   

    bool isvalidate()
    {
        hfSearch.Value = "0";
        int _date;
        if (cmbFrom.SelectedValue != "0" )
        {
           
           _date = DateTime.Compare(Convert.ToDateTime(ucDateTo.Date), Convert.ToDateTime(ucDateFrom.Date));
           if (_date < 0)
           {
               ucMsg.ShowInfo(Resources.Messages.InvalidDate);
               return false;
           }

        }
        if(cmbFrom.SelectedValue == "0" && (ucDateFrom .Date != "" || ucDateTo.Date != ""))
        {
            ucMsg.ShowInfo("Please select the sales channel type  before you select transaction date range");
            return false ;
        }
       

         return true;

    }

    void binddata()
    {
        try
        {
            DataTable dt;
            //if (ViewState["Export"] != null)
            //{
            //    grdBTM.DataSource = (DataTable)ViewState["Export"];
            //    grdBTM.DataBind();
            //    pnlSearch.Visible = true;
            //}

            using (ReportData obj = new ReportData())
            {
                obj.UserId = PageBase.UserId; 
                obj.SalesChannelId = PageBase.SalesChanelID;
                obj.SalesTypeID = Convert.ToInt16(cmbFrom.SelectedValue);
               
                obj.FromDate = ucDateFrom.Date;
                obj.ToDate = ucDateTo.Date;
                dt = obj.GetBTMInfo();
               
                if (dt.Rows.Count > 0)
                {
                    hfSearch.Value = "1";
                    grdBTM.DataSource = dt;
                    grdBTM.DataBind();
                    //ViewState["Export"] = dt;
                    pnlSearch.Visible = true;
                  

                }
                else
                {
                    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                    pnlSearch.Visible = false;
                }

            }
        }
        catch (Exception ex)
        {

            ucMsg.ShowError(ex.Message.ToString(), PageBase.GlobalErrorDisplay());
            PageBase.Errorhandling(ex);
        }

    }



    protected void btnSearch_Click(object sender, EventArgs e)
    {
            if (!isvalidate())
            {
                pnlSearch.Visible = false;
                return;
            }
            binddata();

        }
    
    void   blankall()
    {
        hfSearch.Value = "0";
        ucMsg.Visible = false;
        ucDateFrom.Date = PageBase.Fromdate;
        ucDateTo.Date = PageBase.ToDate;
        btnSearch.CausesValidation = false;
       
        
}

    protected void btnCancel_Click(object sender, EventArgs e)
    {
       // ViewState["Export"] = null;
        blankall();
        pnlSearch.Visible = false;
        if (PageBase.HierarchyLevelID == 2 || PageBase.HierarchyLevelID == 4)
        {
            cmbFrom.SelectedValue = "0";
        }

    }
    

    protected void cmbFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        ucMsg.Visible = false;

    }


    private void Export(bool saveAs)
    {


        ASPxPivotGridExporter1.OptionsPrint.PrintHeadersOnEveryPage = checkPrintHeadersOnEveryPage.Checked;
        ASPxPivotGridExporter1.OptionsPrint.PrintFilterHeaders = checkPrintFilterHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintColumnHeaders = checkPrintColumnHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintRowHeaders = checkPrintRowHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintDataHeaders = checkPrintDataHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;

        string fileName = string.Format("StockTransferReport_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
        switch (ddlExportFormat.SelectedIndex)
        {
            case 0:
                ASPxPivotGridExporter1.ExportXlsToResponse(fileName, saveAs);
                break;
            case 1:
                ASPxPivotGridExporter1.ExportXlsxToResponse(fileName, saveAs);
                break;
        }
    }

    protected void buttonOpen_Click(object sender, EventArgs e)
    {
        Export(false);
    }

    protected void buttonSaveAs_Click(object sender, EventArgs e)
    {
        Export(true);
    }


}
