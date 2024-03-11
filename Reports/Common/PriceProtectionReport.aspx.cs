using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess;
using BussinessLogic;
using DevExpress.Utils;

public partial class Reports_Common_PriceProtectionReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        fncHide();
        if (hfSearch.Value == "1")
            BindGrid();
        if (!IsPostBack)
        {
            FillDropdown();
            ShowScrollBar(true);
           
            
        }
    }
    void FillDropdown()
    {
        DataTable Dt= new DataTable();
        using(ProductData ObjProduct= new ProductData())
        {
            ObjProduct.UserId = PageBase.UserId;
            Dt = ObjProduct.GetPriceDropDate();
            string[] Col = { "DateofChange", "DateofChangeValue" };
            PageBase.DropdownBinding(ref ddlDate, Dt, Col);
        }
       
        
    }

    protected void ShowScrollBar(bool show)
    {
        if (show)
        {

            ASPxPvtGrd.Width = Unit.Parse("950px");
            ASPxPvtGrd.OptionsView.ShowHorizontalScrollBar = true;
        }
        else
        {
            ASPxPvtGrd.Width = Unit.Parse("100%");
            ASPxPvtGrd.OptionsView.ShowHorizontalScrollBar = false;
        }

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (pageValidate())
            BindGrid();
        else
            pnlSearch.Visible = false;
  
    }
    private void BindGrid()
    {
        try
        {

            DataSet DsInfo;
            //if (ViewState["DsInfo"] != null)
            //{
            //    ASPxPvtGrd.DataSource = (DataSet)ViewState["DsInfo"];
            //    ASPxPvtGrd.DataBind();
            //    pnlSearch.Visible = true;
            //}
            //else
            //{
                using (ReportData objRD = new ReportData())
                {
                    objRD.DateFrom = Convert.ToDateTime(ddlDate.SelectedValue);
                    objRD.UserId = PageBase.UserId;
                    

                    DsInfo = objRD.GetPriceProtectionReport();
                    //ViewState["DsInfo"] = DsInfo;
                    if (DsInfo.Tables[0].Rows.Count > 0)
                    {
                        hfSearch.Value = "1";
                        ASPxPvtGrd.DataSource = DsInfo;
                        ASPxPvtGrd.DataBind();
                        pnlSearch.Visible = true;
                    }
                    else
                    {
                        ucMsg.ShowInfo(Resources.Messages.NoRecord);
                        pnlSearch.Visible = false;

                    }
               // }
            }

        }
        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        hfSearch.Value = "0";
        ddlDate.SelectedValue = "0";
       // ViewState["DsInfo"] = null;
        pnlSearch.Visible = false;
        
        
    }
    private void fncHide()
    {
        ucMsg.ShowControl = false;
        pnlSearch.Visible = false;
    }
    bool pageValidate()
    {
        //if (ViewState["DsInfo"] != null)
        //    ViewState["DsInfo"] = null;
        hfSearch.Value = "0";
        if (ddlDate.SelectedValue == "0")
        {
            ucMsg.ShowInfo(Resources.Messages.MandatoryField);
            return false;
        }
        

        return true;
    }
    protected void buttonOpen_Click(object sender, EventArgs e)
    {
        Export(false);
    }
    protected void buttonSaveAs_Click(object sender, EventArgs e)
    {
        Export(true);
    }
    private void Export(bool saveAs)
    {


        ASPxPivotGridExporter1.OptionsPrint.PrintHeadersOnEveryPage = checkPrintHeadersOnEveryPage.Checked;
        ASPxPivotGridExporter1.OptionsPrint.PrintFilterHeaders = checkPrintFilterHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintColumnHeaders = checkPrintColumnHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintRowHeaders = checkPrintRowHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintDataHeaders = checkPrintDataHeaders.Checked ? DefaultBoolean.True : DefaultBoolean.False;

        string fileName = string.Format("PriceProtectionReport_{0}", DateTime.Today.ToString("dd_MM_yyyy"));
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
}
