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
using System.Windows.Forms;
using System.Globalization;
using System.Collections;
using DevExpress.Utils;

public partial class Reports_SalesChannel_PSIReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (hfSearch.Value == "1")
            BindGrid();
        if (!IsPostBack)
        {
            fillyear();
        }
    }
    public void fillyear()
    {
        cmbYear.Items.Clear();
        int j = 0;
        Dictionary<int, string> year = new Dictionary<int, string>();
        for (int i = (DateTime.Now.Year) - 5; i <= DateTime.Now.Year; i++)
        {
            year.Add(j, i.ToString());
            j++;
        }
        cmbYear.DataSource = new BindingSource(year, null);
        cmbYear.DataTextField = "Value";
        cmbYear.DataValueField = "Key";
        cmbYear.DataBind();
        cmbYear.Items.FindByText(DateTime.Now.Year.ToString()).Selected = true;
        updYear.Update();
        cmbYear_SelectedIndexChanged(cmbYear, new EventArgs());

    }
    public void fillmonth(int j)
    {
        Dictionary<int, string> mon = new Dictionary<int, string>();
        DateTimeFormatInfo dtx = new DateTimeFormatInfo();
        mon.Add(0, "All");

        for (int i = 1; i < j; i++)
        {
            mon.Add(i, dtx.GetMonthName(i).ToString());
        }
        cmbMonth.DataSource = new BindingSource(mon, null);
        cmbMonth.DataValueField = "Key";
        cmbMonth.DataTextField = "Value";
        cmbMonth.DataBind();
        cmbMonth.SelectedValue = DateTime.Now.Month.ToString();
        updYear.Update();
    }
    protected void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbYear.SelectedItem.ToString() == DateTime.Now.Year.ToString())
        {
            fillmonth(DateTime.Now.Month + 1);
        }
        else
        {
            fillmonth(13);
        }

    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        BindGrid();
    }


    private void BindGrid()
    {
        try
        {

            DataSet DsSalesInfo;
            /*using (ReportData objRD = new ReportData())*/
            using (ReportData objRD = new ReportData())
            {
                if (cmbMonth.SelectedValue == "0")
                {
                    objRD.FromDate = string.Format("1-1-{0}", (cmbYear.SelectedItem.ToString()));
                    objRD.ToDate = string.Format("12-31-{0}", (cmbYear.SelectedItem.ToString()));
                }
                else
                {
                    objRD.FromDate = string.Format("{0}-1-{1}", cmbMonth.SelectedValue, cmbYear.SelectedItem.ToString());
                    objRD.ToDate = string.Format("{0}-{1}-{2}", cmbMonth.SelectedValue, DateTime.DaysInMonth(Convert.ToInt16(cmbYear.SelectedItem.ToString()), Convert.ToInt16(cmbMonth.SelectedValue)), cmbYear.SelectedItem.ToString());
                }


                DsSalesInfo = objRD.PSIReport();

                hfSearch.Value = "1";

                if (DsSalesInfo.Tables[0].Rows.Count > 0)
                {
                    ASPxPvtGrd.DataSource = DsSalesInfo;
                    ASPxPvtGrd.DataBind();

                    Export(true);
                }
                //else
                //{
                //    ucMsg.ShowInfo(Resources.Messages.NoRecord);
                //    pnlSearch.Visible = false;

                //}
                //if (ddlSalesType.SelectedValue == "3")
                //    ASPxPvtGrd.Fields["PvtRetailerType"].Visible = true;
                //else
                //    ASPxPvtGrd.Fields["PvtRetailerType"].Visible = false;
            }

        }
        catch (Exception ex)
        {
            //ucMsg.ShowError(ex.Message.ToString());
            PageBase.Errorhandling(ex);
        }
    }


    private void Export(bool saveAs)
    {


        ASPxPivotGridExporter1.OptionsPrint.PrintHeadersOnEveryPage = false;
        ASPxPivotGridExporter1.OptionsPrint.PrintFilterHeaders = DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintColumnHeaders = DefaultBoolean.False;
        ASPxPivotGridExporter1.OptionsPrint.PrintRowHeaders = DefaultBoolean.True;
        ASPxPivotGridExporter1.OptionsPrint.PrintDataHeaders = DefaultBoolean.False;

        string fileName = string.Format("SalesReport_{0}", DateTime.Today.ToString("dd_MM_yyyy"));

        ASPxPivotGridExporter1.ExportXlsxToResponse(fileName, saveAs);
    }


}