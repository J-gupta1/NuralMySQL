using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for SalesOrderReportRpt
/// </summary>
public class SalesOrderReportRpt : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private SalesOrder1 salesOrder11;
    private SalesOrder1TableAdapters.vwSalesOrderRptTableAdapter vwSalesOrderRptTableAdapter1;
    private ReportHeaderBand ReportHeader;
    private XRLabel xrLabel1;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell6;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell12;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell14;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell23;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell29;
    private XRTable xrTable2;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCell28;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell32;
    private XRTableCell xrTableCell33;
    private XRTableCell xrTableCell34;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell36;
    private XRTable xrTable3;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell37;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell39;
    public XRTableCell xrrate;
    private XRTableCell xrTableCell42;
    private XRTableCell xrTableCell43;
    public XRTableCell xrSErviceTax;
    public XRTableCell xrAmount;
    private XRTable xrTable4;
    private XRTableRow xrTableRow8;
    private XRTableCell xrTableCell46;
    private XRTableCell xrTableCell48;
    private XRTableRow xrTableRow9;
    private XRTableCell xrTableCell49;
    private XRTableCell xrTableCell50;
    private XRTableCell xrTableCell51;
    private XRTableRow xrTableRow10;
    private XRTableCell xrTableCell52;
    private XRTableCell xrTableCell53;
    private XRTableCell xrTableCell54;
    private XRTableRow xrTableRow11;
    private XRTableCell xrTableCell55;
    private XRTableCell xrTableCell56;
    private XRTableCell xrTableCell57;
    private XRTableRow xrTableRow12;
    private XRTableCell xrTableCell58;
    private XRTableCell xrTableCell59;
    private XRTableCell xrTableCell60;
    private XRTableRow xrTableRow13;
    private XRTableCell xrTableCell61;
    private XRTableCell xrTableCell62;
    private XRTableCell xrTableCell63;
    private XRTableRow xrTableRow14;
    private XRTableCell xrTableCell65;
    private XRTableCell xrTableCell66;
    private XRTableCell xrTableCell67;
    private XRTableCell xrTableCell68;
    private XRTableCell xrTableCell74;
    private XRTableCell xrTableCell75;
    public XRTableCell xrLedger;
    public XRTableCell xrPending;
    private XRTableCell xrTableCell79;
    private XRTableCell xrTableCell80;
    private XRTableCell xrTableCell78;
    private XRTable xrTable5;
    private XRTableRow xrTableRow15;
    private XRTableCell xrTableCell81;
    private XRTableCell xrTableCell82;
    private XRTableRow xrTableRow16;
    private XRTableCell xrTableCell84;
    private XRTableCell xrTableCell85;
    private XRTableRow xrTableRow17;
    private XRTableCell xrTableCell87;
    private XRTableCell xrTableCell88;
    private XRTableRow xrTableRow18;
    private XRTableCell xrTableCell90;
    private XRTableCell xrTableCell91;
    private XRLabel xrLabel2;
    private XRLabel xrLabel3;
    private ReportFooterBand ReportFooter;
    private XRTable xrTable6;
    private XRTableRow xrTableRow19;
    private XRTableCell xrTableCell5;
    public XRTableCell xrTableCell30;
    private XRTableCell xrTableCell24;
    public XRTableCell xrTotalst;
    public XRTableCell xrTotalAmount;
    private XRTableCell xrTableCell17;
    private XRTable xrTable7;
    private XRTableRow xrTableRow20;
    private XRTableCell xrTableCell64;
    private XRPictureBox xrPictureBox1;
    private XRTableRow xrTableRow21;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell2;
    private FormattingRule formattingRule1;
    private XRTableCell xrTableCell16;
    private XRLabel xrLabel4;
    private PageHeaderBand PageHeader;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public SalesOrderReportRpt()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        string resourceFileName = "SalesOrderReportRpt.resx";
        DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrrate = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrSErviceTax = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrAmount = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell67 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell74 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell54 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell68 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell75 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell55 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell56 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell57 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrLedger = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell49 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell50 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrPending = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell58 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell60 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell78 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell62 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell63 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell79 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell65 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell80 = new DevExpress.XtraReports.UI.XRTableCell();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow15 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell81 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell82 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow16 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell84 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell85 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow17 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell87 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell88 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow18 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell90 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell91 = new DevExpress.XtraReports.UI.XRTableCell();
        this.salesOrder11 = new SalesOrder1();
        this.vwSalesOrderRptTableAdapter1 = new SalesOrder1TableAdapters.vwSalesOrderRptTableAdapter();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrTable7 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow20 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell64 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow21 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
        this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
        this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow19 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTotalst = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTotalAmount = new DevExpress.XtraReports.UI.XRTableCell();
        this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
        this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.salesOrder11)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3});
        this.Detail.HeightF = 25F;
        this.Detail.KeepTogether = true;
        this.Detail.KeepTogetherWithDetailReports = true;
        this.Detail.Name = "Detail";
        this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrTable3
        // 
        this.xrTable3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(6.00001F, 0F);
        this.xrTable3.Name = "xrTable3";
        this.xrTable3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7});
        this.xrTable3.SizeF = new System.Drawing.SizeF(759F, 25F);
        this.xrTable3.StylePriority.UseBorders = false;
        this.xrTable3.StylePriority.UsePadding = false;
        // 
        // xrTableRow7
        // 
        this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell37,
            this.xrTableCell38,
            this.xrTableCell39,
            this.xrTableCell16,
            this.xrTableCell2,
            this.xrrate,
            this.xrTableCell42,
            this.xrTableCell43,
            this.xrSErviceTax,
            this.xrAmount});
        this.xrTableRow7.Name = "xrTableRow7";
        this.xrTableRow7.Weight = 1;
        // 
        // xrTableCell37
        // 
        this.xrTableCell37.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell37.BorderWidth = 1;
        this.xrTableCell37.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.SerialNumber")});
        this.xrTableCell37.Name = "xrTableCell37";
        this.xrTableCell37.StylePriority.UseBorders = false;
        this.xrTableCell37.StylePriority.UseBorderWidth = false;
        xrSummary1.FormatString = "{0:#}";
        xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
        xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell37.Summary = xrSummary1;
        this.xrTableCell37.Text = "xrTableCell37";
        this.xrTableCell37.Weight = 0.16918809778564711;
        // 
        // xrTableCell38
        // 
        this.xrTableCell38.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.Model")});
        this.xrTableCell38.Name = "xrTableCell38";
        this.xrTableCell38.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell38.StylePriority.UsePadding = false;
        this.xrTableCell38.Text = "xrTableCell38";
        this.xrTableCell38.Weight = 0.42628854667383032;
        // 
        // xrTableCell39
        // 
        this.xrTableCell39.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.OrderDESCRIPTION")});
        this.xrTableCell39.Name = "xrTableCell39";
        this.xrTableCell39.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell39.StylePriority.UsePadding = false;
        this.xrTableCell39.Text = "xrTableCell39";
        this.xrTableCell39.Weight = 0.58603558248721188;
        // 
        // xrTableCell16
        // 
        this.xrTableCell16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.Quantity")});
        this.xrTableCell16.Name = "xrTableCell16";
        this.xrTableCell16.Text = "xrTableCell16";
        this.xrTableCell16.Weight = 0.18633420838338838;
        // 
        // xrTableCell2
        // 
        this.xrTableCell2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.UOMDescription")});
        this.xrTableCell2.Name = "xrTableCell2";
        this.xrTableCell2.Text = "xrTableCell2";
        this.xrTableCell2.Weight = 0.33600157892311672;
        // 
        // xrrate
        // 
        this.xrrate.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.BasicRate")});
        this.xrrate.Name = "xrrate";
        this.xrrate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrrate.StylePriority.UsePadding = false;
        this.xrrate.Text = "xrrate";
        this.xrrate.Weight = 0.34148456116260478;
        // 
        // xrTableCell42
        // 
        this.xrTableCell42.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.Cd")});
        this.xrTableCell42.Name = "xrTableCell42";
        this.xrTableCell42.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell42.StylePriority.UsePadding = false;
        this.xrTableCell42.Text = "xrTableCell42";
        this.xrTableCell42.Weight = 0.18085415671590355;
        // 
        // xrTableCell43
        // 
        this.xrTableCell43.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.Discount")});
        this.xrTableCell43.Name = "xrTableCell43";
        this.xrTableCell43.Text = "[Discount]";
        this.xrTableCell43.Weight = 0.31583579025806;
        // 
        // xrSErviceTax
        // 
        this.xrSErviceTax.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.ST")});
        this.xrSErviceTax.Name = "xrSErviceTax";
        this.xrSErviceTax.Text = "xrSErviceTax";
        this.xrSErviceTax.Weight = 0.30239989275980372;
        // 
        // xrAmount
        // 
        this.xrAmount.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.Amount")});
        this.xrAmount.Name = "xrAmount";
        this.xrAmount.Text = "xrAmount";
        this.xrAmount.Weight = 0.36540584044658542;
        // 
        // xrTable4
        // 
        this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(6.00001F, 37.91669F);
        this.xrTable4.Name = "xrTable4";
        this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow8,
            this.xrTableRow10,
            this.xrTableRow11,
            this.xrTableRow9,
            this.xrTableRow12,
            this.xrTableRow13,
            this.xrTableRow14});
        this.xrTable4.SizeF = new System.Drawing.SizeF(758.9998F, 175F);
        this.xrTable4.StylePriority.UseBorders = false;
        this.xrTable4.StylePriority.UseFont = false;
        // 
        // xrTableRow8
        // 
        this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell46,
            this.xrTableCell48,
            this.xrTableCell67,
            this.xrTableCell74});
        this.xrTableRow8.Name = "xrTableRow8";
        this.xrTableRow8.Weight = 1;
        // 
        // xrTableCell46
        // 
        this.xrTableCell46.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell46.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell46.Name = "xrTableCell46";
        this.xrTableCell46.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell46.StylePriority.UseBorders = false;
        this.xrTableCell46.StylePriority.UseFont = false;
        this.xrTableCell46.StylePriority.UsePadding = false;
        this.xrTableCell46.Text = "Freight";
        this.xrTableCell46.Weight = 0.81258201349140935;
        // 
        // xrTableCell48
        // 
        this.xrTableCell48.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.Frieght")});
        this.xrTableCell48.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrTableCell48.Name = "xrTableCell48";
        this.xrTableCell48.StylePriority.UseFont = false;
        this.xrTableCell48.Text = "[Frieght]";
        this.xrTableCell48.Weight = 1.1389252548917719;
        // 
        // xrTableCell67
        // 
        this.xrTableCell67.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell67.Name = "xrTableCell67";
        this.xrTableCell67.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell67.StylePriority.UseFont = false;
        this.xrTableCell67.StylePriority.UsePadding = false;
        this.xrTableCell67.Text = "Days:";
        this.xrTableCell67.Weight = 0.53877322295702612;
        // 
        // xrTableCell74
        // 
        this.xrTableCell74.Name = "xrTableCell74";
        this.xrTableCell74.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell74.StylePriority.UsePadding = false;
        this.xrTableCell74.Text = "Amount";
        this.xrTableCell74.Weight = 0.51758260559300784;
        // 
        // xrTableRow10
        // 
        this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell52,
            this.xrTableCell53,
            this.xrTableCell54,
            this.xrTableCell68,
            this.xrTableCell75});
        this.xrTableRow10.Name = "xrTableRow10";
        this.xrTableRow10.Weight = 1;
        // 
        // xrTableCell52
        // 
        this.xrTableCell52.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell52.Name = "xrTableCell52";
        this.xrTableCell52.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell52.StylePriority.UseFont = false;
        this.xrTableCell52.StylePriority.UsePadding = false;
        this.xrTableCell52.Text = "Cargo/TOT";
        this.xrTableCell52.Weight = 0.8125821334818869;
        // 
        // xrTableCell53
        // 
        this.xrTableCell53.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.Cargo")});
        this.xrTableCell53.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrTableCell53.Name = "xrTableCell53";
        this.xrTableCell53.StylePriority.UseFont = false;
        this.xrTableCell53.Text = "xrTableCell53";
        this.xrTableCell53.Weight = 0.514606900796353;
        // 
        // xrTableCell54
        // 
        this.xrTableCell54.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell54.Name = "xrTableCell54";
        this.xrTableCell54.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell54.StylePriority.UseFont = false;
        this.xrTableCell54.StylePriority.UsePadding = false;
        this.xrTableCell54.Text = "Payments Terms:";
        this.xrTableCell54.Weight = 0.62431823410494136;
        // 
        // xrTableCell68
        // 
        this.xrTableCell68.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrTableCell68.Name = "xrTableCell68";
        this.xrTableCell68.StylePriority.UseFont = false;
        this.xrTableCell68.Text = "[CreditDays]";
        this.xrTableCell68.Weight = 0.53877346293798123;
        // 
        // xrTableCell75
        // 
        this.xrTableCell75.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrTableCell75.Name = "xrTableCell75";
        this.xrTableCell75.StylePriority.UseFont = false;
        this.xrTableCell75.Text = "[CreditLimit]";
        this.xrTableCell75.Weight = 0.51758236561205273;
        // 
        // xrTableRow11
        // 
        this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell55,
            this.xrTableCell56,
            this.xrTableCell57,
            this.xrLedger});
        this.xrTableRow11.Name = "xrTableRow11";
        this.xrTableRow11.Weight = 1;
        // 
        // xrTableCell55
        // 
        this.xrTableCell55.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell55.Name = "xrTableCell55";
        this.xrTableCell55.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell55.StylePriority.UseFont = false;
        this.xrTableCell55.StylePriority.UsePadding = false;
        this.xrTableCell55.Text = "Booking Details No:";
        this.xrTableCell55.Weight = 0.8125821334818869;
        // 
        // xrTableCell56
        // 
        this.xrTableCell56.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.BookingDetails")});
        this.xrTableCell56.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrTableCell56.Name = "xrTableCell56";
        this.xrTableCell56.StylePriority.UseFont = false;
        this.xrTableCell56.Text = "xrTableCell56";
        this.xrTableCell56.Weight = 0.51460702078683052;
        // 
        // xrTableCell57
        // 
        this.xrTableCell57.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell57.Name = "xrTableCell57";
        this.xrTableCell57.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell57.StylePriority.UseFont = false;
        this.xrTableCell57.StylePriority.UsePadding = false;
        this.xrTableCell57.Text = "Ledger Bal:";
        this.xrTableCell57.Weight = 0.62431811411446381;
        // 
        // xrLedger
        // 
        this.xrLedger.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.LedgerBal")});
        this.xrLedger.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrLedger.Name = "xrLedger";
        this.xrLedger.StylePriority.UseFont = false;
        this.xrLedger.Text = "xrLedger";
        this.xrLedger.Weight = 1.0563558285500339;
        // 
        // xrTableRow9
        // 
        this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell49,
            this.xrTableCell50,
            this.xrTableCell51,
            this.xrPending});
        this.xrTableRow9.Name = "xrTableRow9";
        this.xrTableRow9.Weight = 1;
        // 
        // xrTableCell49
        // 
        this.xrTableCell49.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell49.Name = "xrTableCell49";
        this.xrTableCell49.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell49.StylePriority.UseFont = false;
        this.xrTableCell49.StylePriority.UsePadding = false;
        this.xrTableCell49.Text = "Excise Reg No";
        this.xrTableCell49.Weight = 0.81258225347236446;
        // 
        // xrTableCell50
        // 
        this.xrTableCell50.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.ExciseRegNo")});
        this.xrTableCell50.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrTableCell50.Name = "xrTableCell50";
        this.xrTableCell50.StylePriority.UseFont = false;
        this.xrTableCell50.Text = "xrTableCell50";
        this.xrTableCell50.Weight = 0.51460678080587541;
        // 
        // xrTableCell51
        // 
        this.xrTableCell51.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell51.Name = "xrTableCell51";
        this.xrTableCell51.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell51.StylePriority.UseFont = false;
        this.xrTableCell51.StylePriority.UsePadding = false;
        this.xrTableCell51.Text = "SO Pending:";
        this.xrTableCell51.Weight = 0.62431823410494136;
        // 
        // xrPending
        // 
        this.xrPending.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.SoPending")});
        this.xrPending.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrPending.Name = "xrPending";
        this.xrPending.StylePriority.UseFont = false;
        this.xrPending.Text = "xrPending";
        this.xrPending.Weight = 1.0563558285500339;
        // 
        // xrTableRow12
        // 
        this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell58,
            this.xrTableCell59,
            this.xrTableCell60,
            this.xrTableCell78});
        this.xrTableRow12.Name = "xrTableRow12";
        this.xrTableRow12.Weight = 1;
        // 
        // xrTableCell58
        // 
        this.xrTableCell58.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell58.Name = "xrTableCell58";
        this.xrTableCell58.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell58.StylePriority.UseFont = false;
        this.xrTableCell58.StylePriority.UsePadding = false;
        this.xrTableCell58.Text = "ECC No.";
        this.xrTableCell58.Weight = 0.812582373462842;
        // 
        // xrTableCell59
        // 
        this.xrTableCell59.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.ECCNo")});
        this.xrTableCell59.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrTableCell59.Name = "xrTableCell59";
        this.xrTableCell59.StylePriority.UseFont = false;
        this.xrTableCell59.Text = "xrTableCell59";
        this.xrTableCell59.Weight = 0.51460678080587541;
        // 
        // xrTableCell60
        // 
        this.xrTableCell60.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell60.Name = "xrTableCell60";
        this.xrTableCell60.StylePriority.UseFont = false;
        this.xrTableCell60.Text = "Deviation:";
        this.xrTableCell60.Weight = 0.62431811411446381;
        // 
        // xrTableCell78
        // 
        this.xrTableCell78.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.Deviation")});
        this.xrTableCell78.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrTableCell78.Name = "xrTableCell78";
        this.xrTableCell78.StylePriority.UseFont = false;
        this.xrTableCell78.Text = "xrTableCell78";
        this.xrTableCell78.Weight = 1.0563558285500339;
        // 
        // xrTableRow13
        // 
        this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell61,
            this.xrTableCell62,
            this.xrTableCell63,
            this.xrTableCell79});
        this.xrTableRow13.Name = "xrTableRow13";
        this.xrTableRow13.Weight = 1;
        // 
        // xrTableCell61
        // 
        this.xrTableCell61.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell61.Name = "xrTableCell61";
        this.xrTableCell61.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell61.StylePriority.UseFont = false;
        this.xrTableCell61.StylePriority.UsePadding = false;
        this.xrTableCell61.Text = "CST No.";
        this.xrTableCell61.Weight = 0.8125821334818869;
        // 
        // xrTableCell62
        // 
        this.xrTableCell62.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.CSTNo")});
        this.xrTableCell62.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrTableCell62.Name = "xrTableCell62";
        this.xrTableCell62.StylePriority.UseFont = false;
        this.xrTableCell62.Text = "xrTableCell62";
        this.xrTableCell62.Weight = 0.514606900796353;
        // 
        // xrTableCell63
        // 
        this.xrTableCell63.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell63.Name = "xrTableCell63";
        this.xrTableCell63.StylePriority.UseFont = false;
        this.xrTableCell63.Text = "Tin Number:";
        this.xrTableCell63.Weight = 0.62431823410494136;
        // 
        // xrTableCell79
        // 
        this.xrTableCell79.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.TinNumber")});
        this.xrTableCell79.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrTableCell79.Name = "xrTableCell79";
        this.xrTableCell79.StylePriority.UseFont = false;
        this.xrTableCell79.Text = "xrTableCell79";
        this.xrTableCell79.Weight = 1.0563558285500339;
        // 
        // xrTableRow14
        // 
        this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell65,
            this.xrTableCell17,
            this.xrTableCell66,
            this.xrTableCell80});
        this.xrTableRow14.Name = "xrTableRow14";
        this.xrTableRow14.Weight = 1;
        // 
        // xrTableCell65
        // 
        this.xrTableCell65.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell65.Name = "xrTableCell65";
        this.xrTableCell65.StylePriority.UseFont = false;
        this.xrTableCell65.Text = "LST No.";
        this.xrTableCell65.Weight = 0.59132550696113306;
        // 
        // xrTableCell17
        // 
        this.xrTableCell17.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrTableCell17.Name = "xrTableCell17";
        this.xrTableCell17.StylePriority.UseFont = false;
        this.xrTableCell17.Text = "[LSTNo]";
        this.xrTableCell17.Weight = 0.37448550374195516;
        // 
        // xrTableCell66
        // 
        this.xrTableCell66.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell66.Name = "xrTableCell66";
        this.xrTableCell66.StylePriority.UseFont = false;
        this.xrTableCell66.Text = "Party Group";
        this.xrTableCell66.Weight = 0.45432384624867844;
        // 
        // xrTableCell80
        // 
        this.xrTableCell80.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.PartyGroup")});
        this.xrTableCell80.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrTableCell80.Name = "xrTableCell80";
        this.xrTableCell80.StylePriority.UseFont = false;
        this.xrTableCell80.Text = "xrTableCell80";
        this.xrTableCell80.Weight = 0.76872236553679607;
        // 
        // TopMargin
        // 
        this.TopMargin.HeightF = 3F;
        this.TopMargin.Name = "TopMargin";
        this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel3
        // 
        this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.OrderToAddress")});
        this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 6.249992F);
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 2, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(553.9542F, 25F);
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.StylePriority.UsePadding = false;
        this.xrLabel3.StylePriority.UseTextAlignment = false;
        this.xrLabel3.Text = "[OrderToAddress]";
        this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel1
        // 
        this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.OrderTo")});
        this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0.0003662109F, 1.250015F);
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(553.9539F, 29.99998F);
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.StylePriority.UsePadding = false;
        this.xrLabel1.StylePriority.UseTextAlignment = false;
        this.xrLabel1.Text = "[OrderTo]";
        this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // BottomMargin
        // 
        this.BottomMargin.HeightF = 28F;
        this.BottomMargin.Name = "BottomMargin";
        this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.BottomMargin.StylePriority.UsePadding = false;
        this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel2
        // 
        this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(594.5898F, 354.5F);
        this.xrLabel2.Name = "xrLabel2";
        this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 2, 8, 0, 100F);
        this.xrLabel2.SizeF = new System.Drawing.SizeF(167.7084F, 23F);
        this.xrLabel2.StylePriority.UseFont = false;
        this.xrLabel2.StylePriority.UsePadding = false;
        this.xrLabel2.Text = "Head Commercial";
        // 
        // xrTable5
        // 
        this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(6.00001F, 227.5F);
        this.xrTable5.Name = "xrTable5";
        this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow15,
            this.xrTableRow16,
            this.xrTableRow17,
            this.xrTableRow18});
        this.xrTable5.SizeF = new System.Drawing.SizeF(528.4069F, 150F);
        // 
        // xrTableRow15
        // 
        this.xrTableRow15.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell81,
            this.xrTableCell82});
        this.xrTableRow15.Name = "xrTableRow15";
        this.xrTableRow15.Weight = 1;
        // 
        // xrTableCell81
        // 
        this.xrTableCell81.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell81.Name = "xrTableCell81";
        this.xrTableCell81.StylePriority.UseFont = false;
        this.xrTableCell81.Text = "Remarks:";
        this.xrTableCell81.Weight = 1.5784525310944313;
        // 
        // xrTableCell82
        // 
        this.xrTableCell82.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.Remarks")});
        this.xrTableCell82.Name = "xrTableCell82";
        this.xrTableCell82.Text = "xrTableCell82";
        this.xrTableCell82.Weight = 4.5181985658745125;
        // 
        // xrTableRow16
        // 
        this.xrTableRow16.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell84,
            this.xrTableCell85});
        this.xrTableRow16.Name = "xrTableRow16";
        this.xrTableRow16.Weight = 1;
        // 
        // xrTableCell84
        // 
        this.xrTableCell84.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell84.Name = "xrTableCell84";
        this.xrTableCell84.StylePriority.UseFont = false;
        this.xrTableCell84.Text = "Special Remarks:";
        this.xrTableCell84.Weight = 1.3680706787109376;
        // 
        // xrTableCell85
        // 
        this.xrTableCell85.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.SplRemarks")});
        this.xrTableCell85.Name = "xrTableCell85";
        this.xrTableCell85.Text = "xrTableCell85";
        this.xrTableCell85.Weight = 3.9159979248046874;
        // 
        // xrTableRow17
        // 
        this.xrTableRow17.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell87,
            this.xrTableCell88});
        this.xrTableRow17.Name = "xrTableRow17";
        this.xrTableRow17.Weight = 1;
        // 
        // xrTableCell87
        // 
        this.xrTableCell87.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell87.Name = "xrTableCell87";
        this.xrTableCell87.StylePriority.UseFont = false;
        this.xrTableCell87.Text = "Delivery Address:";
        this.xrTableCell87.Weight = 1.3680709838867187;
        // 
        // xrTableCell88
        // 
        this.xrTableCell88.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.DeliveryAddress")});
        this.xrTableCell88.Name = "xrTableCell88";
        this.xrTableCell88.Text = "xrTableCell88";
        this.xrTableCell88.Weight = 3.9159976196289064;
        // 
        // xrTableRow18
        // 
        this.xrTableRow18.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell90,
            this.xrTableCell91});
        this.xrTableRow18.Name = "xrTableRow18";
        this.xrTableRow18.Weight = 1;
        // 
        // xrTableCell90
        // 
        this.xrTableCell90.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell90.Name = "xrTableCell90";
        this.xrTableCell90.StylePriority.UseFont = false;
        this.xrTableCell90.Text = "Raised By:";
        this.xrTableCell90.Weight = 1.3680709838867187;
        // 
        // xrTableCell91
        // 
        this.xrTableCell91.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.RaisedBy")});
        this.xrTableCell91.Name = "xrTableCell91";
        this.xrTableCell91.Text = "xrTableCell91";
        this.xrTableCell91.Weight = 3.9159976196289064;
        // 
        // salesOrder11
        // 
        this.salesOrder11.DataSetName = "SalesOrder1";
        this.salesOrder11.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // vwSalesOrderRptTableAdapter1
        // 
        this.vwSalesOrderRptTableAdapter1.ClearBeforeFill = true;
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrPictureBox1,
            this.xrTable7,
            this.xrTable2,
            this.xrTable1});
        this.ReportHeader.HeightF = 398.9166F;
        this.ReportHeader.Name = "ReportHeader";
        this.ReportHeader.StylePriority.UsePadding = false;
        // 
        // xrLabel4
        // 
        this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Bold);
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(211.0458F, 8.000007F);
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(553.9539F, 19.99998F);
        this.xrLabel4.StylePriority.UseFont = false;
        this.xrLabel4.StylePriority.UsePadding = false;
        this.xrLabel4.StylePriority.UseTextAlignment = false;
        this.xrLabel4.Text = "Sales Order";
        this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrPictureBox1
        // 
        this.xrPictureBox1.ImageUrl = "~\\Assets\\Images\\logo.jpg";
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(6.00001F, 29.99997F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(194.7238F, 54.99998F);
        // 
        // xrTable7
        // 
        this.xrTable7.LocationFloat = new DevExpress.Utils.PointFloat(211.0459F, 29.99996F);
        this.xrTable7.Name = "xrTable7";
        this.xrTable7.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow20,
            this.xrTableRow21});
        this.xrTable7.SizeF = new System.Drawing.SizeF(553.9539F, 62.49998F);
        // 
        // xrTableRow20
        // 
        this.xrTableRow20.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell64});
        this.xrTableRow20.Name = "xrTableRow20";
        this.xrTableRow20.Weight = 1;
        // 
        // xrTableCell64
        // 
        this.xrTableCell64.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
        this.xrTableCell64.Name = "xrTableCell64";
        this.xrTableCell64.StylePriority.UseTextAlignment = false;
        this.xrTableCell64.Text = "xrTableCell64";
        this.xrTableCell64.Weight = 7.5899981689453124;
        // 
        // xrTableRow21
        // 
        this.xrTableRow21.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell18});
        this.xrTableRow21.Name = "xrTableRow21";
        this.xrTableRow21.Weight = 1;
        // 
        // xrTableCell18
        // 
        this.xrTableCell18.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3});
        this.xrTableCell18.Name = "xrTableCell18";
        this.xrTableCell18.Text = "xrTableCell18";
        this.xrTableCell18.Weight = 7.5899981689453124;
        // 
        // xrTable2
        // 
        this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable2.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(6.00001F, 373.9166F);
        this.xrTable2.Name = "xrTable2";
        this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
        this.xrTable2.SizeF = new System.Drawing.SizeF(759F, 25F);
        this.xrTable2.StylePriority.UseBorders = false;
        this.xrTable2.StylePriority.UseFont = false;
        // 
        // xrTableRow6
        // 
        this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell34,
            this.xrTableCell33,
            this.xrTableCell26,
            this.xrTableCell35,
            this.xrTableCell10,
            this.xrTableCell27,
            this.xrTableCell31,
            this.xrTableCell28,
            this.xrTableCell32,
            this.xrTableCell36});
        this.xrTableRow6.Name = "xrTableRow6";
        this.xrTableRow6.Weight = 1;
        // 
        // xrTableCell34
        // 
        this.xrTableCell34.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell34.Name = "xrTableCell34";
        this.xrTableCell34.StylePriority.UseFont = false;
        this.xrTableCell34.Text = "SNo.";
        this.xrTableCell34.Weight = 0.16829642255579203;
        // 
        // xrTableCell33
        // 
        this.xrTableCell33.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell33.Name = "xrTableCell33";
        this.xrTableCell33.StylePriority.UseFont = false;
        this.xrTableCell33.Text = "Model Name";
        this.xrTableCell33.Weight = 0.42404196166881281;
        // 
        // xrTableCell26
        // 
        this.xrTableCell26.Name = "xrTableCell26";
        this.xrTableCell26.Text = "Description";
        this.xrTableCell26.Weight = 0.58294703096511413;
        // 
        // xrTableCell35
        // 
        this.xrTableCell35.Name = "xrTableCell35";
        this.xrTableCell35.Text = "Qty";
        this.xrTableCell35.Weight = 0.18535235047087886;
        // 
        // xrTableCell10
        // 
        this.xrTableCell10.Name = "xrTableCell10";
        this.xrTableCell10.Text = "Uom";
        this.xrTableCell10.Weight = 0.33423096240931821;
        // 
        // xrTableCell27
        // 
        this.xrTableCell27.Name = "xrTableCell27";
        this.xrTableCell27.Text = "Basic Rate";
        this.xrTableCell27.Weight = 0.33968478242813216;
        // 
        // xrTableCell31
        // 
        this.xrTableCell31.Name = "xrTableCell31";
        this.xrTableCell31.Text = "CD%";
        this.xrTableCell31.Weight = 0.17990093559015374;
        // 
        // xrTableCell28
        // 
        this.xrTableCell28.Name = "xrTableCell28";
        this.xrTableCell28.Text = "Discount";
        this.xrTableCell28.Weight = 0.31417155223598664;
        // 
        // xrTableCell32
        // 
        this.xrTableCell32.Name = "xrTableCell32";
        this.xrTableCell32.Text = "S.T.";
        this.xrTableCell32.Weight = 0.30080633904086446;
        // 
        // xrTableCell36
        // 
        this.xrTableCell36.Name = "xrTableCell36";
        this.xrTableCell36.Text = "Amount";
        this.xrTableCell36.Weight = 0.36347987543478261;
        // 
        // xrTable1
        // 
        this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(6.000122F, 106.2084F);
        this.xrTable1.Name = "xrTable1";
        this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow2,
            this.xrTableRow3,
            this.xrTableRow5,
            this.xrTableRow4});
        this.xrTable1.SizeF = new System.Drawing.SizeF(759F, 255.625F);
        this.xrTable1.StylePriority.UseBorders = false;
        // 
        // xrTableRow1
        // 
        this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell1,
            this.xrTableCell6,
            this.xrTableCell3});
        this.xrTableRow1.Name = "xrTableRow1";
        this.xrTableRow1.Weight = 1;
        // 
        // xrTableCell4
        // 
        this.xrTableCell4.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell4.Name = "xrTableCell4";
        this.xrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell4.StylePriority.UseFont = false;
        this.xrTableCell4.StylePriority.UsePadding = false;
        this.xrTableCell4.Text = "Sales Order";
        this.xrTableCell4.Weight = 0.56942998856460492;
        // 
        // xrTableCell1
        // 
        this.xrTableCell1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.SalesOrder")});
        this.xrTableCell1.Name = "xrTableCell1";
        this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell1.StylePriority.UsePadding = false;
        this.xrTableCell1.Text = "[SalesOrder]";
        this.xrTableCell1.Weight = 0.9999998320331307;
        // 
        // xrTableCell6
        // 
        this.xrTableCell6.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell6.Name = "xrTableCell6";
        this.xrTableCell6.StylePriority.UseFont = false;
        this.xrTableCell6.StylePriority.UseTextAlignment = false;
        this.xrTableCell6.Text = "Date &Time:";
        this.xrTableCell6.Weight = 0.63772099180832065;
        // 
        // xrTableCell3
        // 
        this.xrTableCell3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.SalesOrderDate")});
        this.xrTableCell3.Font = new System.Drawing.Font("Times New Roman", 10F);
        this.xrTableCell3.Name = "xrTableCell3";
        this.xrTableCell3.StylePriority.UseFont = false;
        this.xrTableCell3.Text = "xrTableCell3";
        this.xrTableCell3.Weight = 0.86227917615854865;
        // 
        // xrTableRow2
        // 
        this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell8,
            this.xrTableCell11,
            this.xrTableCell12});
        this.xrTableRow2.Name = "xrTableRow2";
        this.xrTableRow2.Weight = 1;
        // 
        // xrTableCell7
        // 
        this.xrTableCell7.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell7.Name = "xrTableCell7";
        this.xrTableCell7.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.xrTableCell7.StylePriority.UseFont = false;
        this.xrTableCell7.StylePriority.UsePadding = false;
        this.xrTableCell7.Text = "Purchase Order";
        this.xrTableCell7.Weight = 0.56942998856460492;
        // 
        // xrTableCell8
        // 
        this.xrTableCell8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.PurchaseOrder")});
        this.xrTableCell8.Font = new System.Drawing.Font("Times New Roman", 10F);
        this.xrTableCell8.Name = "xrTableCell8";
        this.xrTableCell8.StylePriority.UseFont = false;
        this.xrTableCell8.Text = "xrTableCell8";
        this.xrTableCell8.Weight = 0.9999998320331307;
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.StylePriority.UseFont = false;
        this.xrTableCell11.Text = "Purchase Order Date";
        this.xrTableCell11.Weight = 0.63772099180832065;
        // 
        // xrTableCell12
        // 
        this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.PurchaseOrderDate")});
        this.xrTableCell12.Font = new System.Drawing.Font("Times New Roman", 10F);
        this.xrTableCell12.Name = "xrTableCell12";
        this.xrTableCell12.StylePriority.UseFont = false;
        this.xrTableCell12.Text = "xrTableCell12";
        this.xrTableCell12.Weight = 0.86227917615854865;
        // 
        // xrTableRow3
        // 
        this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell13,
            this.xrTableCell14});
        this.xrTableRow3.Name = "xrTableRow3";
        this.xrTableRow3.Weight = 1;
        // 
        // xrTableCell13
        // 
        this.xrTableCell13.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell13.Name = "xrTableCell13";
        this.xrTableCell13.StylePriority.UseFont = false;
        this.xrTableCell13.Text = "Party Name";
        this.xrTableCell13.Weight = 0.56942998856460492;
        // 
        // xrTableCell14
        // 
        this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.PartyName")});
        this.xrTableCell14.Font = new System.Drawing.Font("Times New Roman", 10F);
        this.xrTableCell14.Name = "xrTableCell14";
        this.xrTableCell14.StylePriority.UseFont = false;
        this.xrTableCell14.Text = "xrTableCell14";
        this.xrTableCell14.Weight = 2.5;
        // 
        // xrTableRow5
        // 
        this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell25,
            this.xrTableCell29});
        this.xrTableRow5.Name = "xrTableRow5";
        this.xrTableRow5.Weight = 1;
        // 
        // xrTableCell25
        // 
        this.xrTableCell25.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell25.Name = "xrTableCell25";
        this.xrTableCell25.StylePriority.UseFont = false;
        this.xrTableCell25.Text = "Party Address";
        this.xrTableCell25.Weight = 0.56942998856460492;
        // 
        // xrTableCell29
        // 
        this.xrTableCell29.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.PartyAddress")});
        this.xrTableCell29.Font = new System.Drawing.Font("Times New Roman", 10F);
        this.xrTableCell29.Name = "xrTableCell29";
        this.xrTableCell29.StylePriority.UseFont = false;
        this.xrTableCell29.Text = "xrTableCell29";
        this.xrTableCell29.Weight = 2.5;
        // 
        // xrTableRow4
        // 
        this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell19,
            this.xrTableCell20,
            this.xrTableCell22,
            this.xrTableCell23});
        this.xrTableRow4.Name = "xrTableRow4";
        this.xrTableRow4.Weight = 1;
        // 
        // xrTableCell19
        // 
        this.xrTableCell19.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell19.Name = "xrTableCell19";
        this.xrTableCell19.StylePriority.UseFont = false;
        this.xrTableCell19.Text = "Contact Person";
        this.xrTableCell19.Weight = 0.56942998856460492;
        // 
        // xrTableCell20
        // 
        this.xrTableCell20.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.contactPerson")});
        this.xrTableCell20.Name = "xrTableCell20";
        this.xrTableCell20.Text = "xrTableCell20";
        this.xrTableCell20.Weight = 0.999999893740357;
        // 
        // xrTableCell22
        // 
        this.xrTableCell22.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell22.Name = "xrTableCell22";
        this.xrTableCell22.StylePriority.UseFont = false;
        this.xrTableCell22.Text = "Mobile No:";
        this.xrTableCell22.Weight = 0.63772085469506445;
        // 
        // xrTableCell23
        // 
        this.xrTableCell23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.MobileNumber")});
        this.xrTableCell23.Font = new System.Drawing.Font("Times New Roman", 10F);
        this.xrTableCell23.Name = "xrTableCell23";
        this.xrTableCell23.StylePriority.UseFont = false;
        this.xrTableCell23.Text = "xrTableCell23";
        this.xrTableCell23.Weight = 0.86227925156457852;
        // 
        // ReportFooter
        // 
        this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable6,
            this.xrTable4,
            this.xrTable5,
            this.xrLabel2});
        this.ReportFooter.HeightF = 401F;
        this.ReportFooter.Name = "ReportFooter";
        // 
        // xrTable6
        // 
        this.xrTable6.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(6.000009F, 0F);
        this.xrTable6.Name = "xrTable6";
        this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow19});
        this.xrTable6.SizeF = new System.Drawing.SizeF(759.0001F, 24.99997F);
        this.xrTable6.StylePriority.UseBorders = false;
        // 
        // xrTableRow19
        // 
        this.xrTableRow19.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5,
            this.xrTableCell30,
            this.xrTableCell24,
            this.xrTotalst,
            this.xrTotalAmount});
        this.xrTableRow19.Name = "xrTableRow19";
        this.xrTableRow19.Weight = 1;
        // 
        // xrTableCell5
        // 
        this.xrTableCell5.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
        this.xrTableCell5.Name = "xrTableCell5";
        this.xrTableCell5.StylePriority.UseFont = false;
        this.xrTableCell5.Text = "Total";
        this.xrTableCell5.Weight = 0.78000230582936836;
        // 
        // xrTableCell30
        // 
        this.xrTableCell30.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "vwSalesOrderRpt.Quantity")});
        this.xrTableCell30.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell30.Name = "xrTableCell30";
        this.xrTableCell30.StylePriority.UseFont = false;
        xrSummary2.FormatString = "{0:#.00}";
        this.xrTableCell30.Summary = xrSummary2;
        this.xrTableCell30.Text = "xrTableCell30";
        this.xrTableCell30.Weight = 0.12301268836126111;
        // 
        // xrTableCell24
        // 
        this.xrTableCell24.Name = "xrTableCell24";
        this.xrTableCell24.Weight = 0.77515885221813074;
        // 
        // xrTotalst
        // 
        this.xrTotalst.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTotalst.Name = "xrTotalst";
        this.xrTotalst.StylePriority.UseFont = false;
        this.xrTotalst.Text = "[TotalTaxAmount]";
        this.xrTotalst.Weight = 0.19963634933203051;
        // 
        // xrTotalAmount
        // 
        this.xrTotalAmount.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTotalAmount.Name = "xrTotalAmount";
        this.xrTotalAmount.StylePriority.UseFont = false;
        this.xrTotalAmount.Text = "[TotalNetPrice]";
        this.xrTotalAmount.Weight = 0.24123128616581485;
        // 
        // formattingRule1
        // 
        this.formattingRule1.Name = "formattingRule1";
        // 
        // PageHeader
        // 
        this.PageHeader.Name = "PageHeader";
        // 
        // SalesOrderReportRpt
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.ReportFooter,
            this.PageHeader});
        this.DataAdapter = this.vwSalesOrderRptTableAdapter1;
        this.DataMember = "vwSalesOrderRpt";
        this.DataSource = this.salesOrder11;
        this.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRule1});
        this.Margins = new System.Drawing.Printing.Margins(40, 34, 3, 28);
        this.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 2, 0, 100F);
        this.Version = "10.1";
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.salesOrder11)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
