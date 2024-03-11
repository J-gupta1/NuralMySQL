using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using System.Data;
using DevExpress.Web.ASPxClasses.Internal;
using System.IO;

public partial class Reports_DSR_ReportToPendingDSR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            BindReprot_MissingDSR();
            string columnIndexValue = Page.Request["ColumnIndex"], rowIndexValue = Page.Request["RowIndex"];
            if (!string.IsNullOrEmpty(columnIndexValue) && !string.IsNullOrEmpty(rowIndexValue))
                BindGridView(columnIndexValue, rowIndexValue);
            //rptPendingDSR.ClientSideEvents.CellClick = GetJSCellClickHandler();
            //ASPxPopupControl1.ClientSideEvents.Closing = GetJSPopupClosingHandler();
        }
        catch (Exception ex)
        {}

    }
    private void BindReprot_MissingDSR()
    {
        try
        {
            using (ReportData objPendingDSR = new ReportData())
            {
                DataTable dt = objPendingDSR.ReportToPendingDSR();
                rptPendingDSR.DataSource = dt;
                rptPendingDSR.DataBind();
            }
        }
        catch (Exception ex)
        {throw ex;}
    }
    protected void ASPxGridView1_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
    {
        try
        {
            string[] param = e.Parameters.Split('|');      
            //ASPxGridView1.DataSource = rptPendingDSR.CreateDrillDownDataSource(Int32.Parse(param[1]), Int32.Parse(param[2]));
            //ASPxGridView1.DataBind();
            //ASPxGridView1.PageIndex = 0;
            BindGridView(param[1], param[2]);
            Int32 contaRighe = rptPendingDSR.RowCount;
            if (param.Length != 3) return;
            rptPendingDSR.DataBind();
        }
        catch (Exception ex)
        { throw ex; }
    }
    protected void BindGridView(string columnIndex, string rowIndex)
    {
        try
        {
            ASPxGridView1.DataSource = rptPendingDSR.CreateDrillDownDataSource(Int32.Parse(columnIndex), Int32.Parse(rowIndex));
            ASPxGridView1.DataBind();
            ASPxGridView1.PageIndex = 0;
            //ASPxGridView1.DataSource = rptPendingDSR.CreateDrillDownDataSource(0,3);
            //ASPxGridView1.DataBind();
        }
        catch (Exception ex)
        { throw ex; }
    }
//    protected string GetJSCellClickHandler()
//    {
//        return string.Format(@"function (s, e) {{
//         var columnIndex = document.getElementById('{0}'),
//             rowIndex = document.getElementById('{1}');
//         columnIndex.value = e.ColumnIndex;
//         rowIndex.value = e.RowIndex;
//         GridView.PerformCallback('D');
//         ShowDrillDown();
//        }}", ColumnIndex.ClientID, RowIndex.ClientID);
//    }
//    protected string GetJSPopupClosingHandler()
//    {
//        return string.Format(@"function (s, e) {{
//         var columnIndex = document.getElementById('{0}'),
//             rowIndex = document.getElementById('{1}');
//         columnIndex.value = '';
//         rowIndex.value = '';    
//        }}", ColumnIndex.ClientID, RowIndex.ClientID);
//    }
    protected void buttonSaveAsDrillDown_Click(object sender, EventArgs e)
    {
        Export("GridViewExp");
    }
    void Export(string exporterName)
    {

        DevExpress.Utils.Paint.XPaint.ForceGDIPlusPaint();
        using (MemoryStream stream = new MemoryStream())
        {

            string contentType = "", fileName = "";

            contentType = "application/ms-excel";

            if (exporterName == "PivotGridExp")
            {
                fileName = "CallTatReport.xls";
                ASPxPivotGridExporter1.ExportToXls(stream);
            }
            else
            {
                fileName = "Details.xls";
                ASPxGridViewExporter1.WriteXls(stream);
            }

            byte[] buffer = stream.GetBuffer();

            string disposition = "attachment";
            Response.Clear();
            Response.Buffer = false;
            Response.AppendHeader("Content-Type", contentType);
            Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Response.AppendHeader("Content-Disposition", disposition + "; filename=" + fileName);
            Response.BinaryWrite(buffer);
            Response.End();
        }
    }
    void Export()
    {

        DevExpress.Utils.Paint.XPaint.ForceGDIPlusPaint();
        using (MemoryStream stream = new MemoryStream())
        {

            string contentType = "", fileName = "";
            contentType = "application/ms-excel";
            fileName = "CallTatReport.xls";
            ASPxPivotGridExporter1.ExportToXls(stream, false);
            byte[] buffer = stream.GetBuffer();

            string disposition = "attachment";
            Response.Clear();
            Response.Buffer = false;
            Response.AppendHeader("Content-Type", contentType);
            Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Response.AppendHeader("Content-Disposition", disposition + "; filename=" + fileName);
            Response.BinaryWrite(buffer);
            Response.End();
        }
    }
}
