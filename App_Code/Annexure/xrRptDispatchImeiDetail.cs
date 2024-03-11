using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for xrRptDispatchImeiDetail
/// </summary>
public class xrRptDispatchImeiDetail : DevExpress.XtraReports.UI.XtraReport
{
    public DevExpress.XtraReports.UI.DetailBand Detail;
    private TopMarginBand topMarginBand1;
    private BottomMarginBand bottomMarginBand1;
    private ReportHeaderBand ReportHeader;
    private XRLabel lbTitle;
    private XRTable xrTable3;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell8;
    public XRTableCell xrTableCell9;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell11;
    private XRTable xrTable4;
    private XRTableRow xrTableRow4;
    public XRTableCell xrTableCell12;
    public XRTableCell xrModel;
    public XRTableCell xrSkuname;
    public XRTableCell xrSku;
    public XRTableCell xrInvoice;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTable xrTable7;
    private XRTableRow xrTableRow7;
    public XRTableCell xrTableSerial;
    private XRLabel xrLabel1Invoiceno;
    public XRLabel xrLabel2InvoiceNo;
    private XRLabel xrLabel3InvoiceDate;
    public XRLabel xrLabel4InvoiceDate;
    private PageFooterBand PageFooter;
    private XRPageInfo xrPageInfo1;
    private XRPictureBox xrPictureBox1;
    private FormattingRule formattingRule1;
	/// <summary>
	/// Required designer variable.
	/// </summary>
	public System.ComponentModel.IContainer components = null;

	public xrRptDispatchImeiDetail()
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
	protected override void Dispose(bool disposing) {
		if (disposing && (components != null)) {
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent() {
        string resourceFileName = "xrRptDispatchImeiDetail.resx";
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable7 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableSerial = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrModel = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrSkuname = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrSku = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrInvoice = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
        this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
        this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.lbTitle = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel1Invoiceno = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel2InvoiceNo = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel3InvoiceDate = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel4InvoiceDate = new DevExpress.XtraReports.UI.XRLabel();
        this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
        this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // Detail
        // 
        this.Detail.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable7,
            this.xrTable1,
            this.xrTable4,
            this.xrTable3});
        this.Detail.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.Detail.HeightF = 133F;
        this.Detail.KeepTogether = true;
        this.Detail.KeepTogetherWithDetailReports = true;
        this.Detail.Name = "Detail";
        this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.StylePriority.UseBorders = false;
        this.Detail.StylePriority.UseFont = false;
        this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrTable7
        // 
        this.xrTable7.LocationFloat = new DevExpress.Utils.PointFloat(1.000023F, 75F);
        this.xrTable7.Name = "xrTable7";
        this.xrTable7.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7});
        this.xrTable7.SizeF = new System.Drawing.SizeF(1000F, 25F);
        // 
        // xrTableRow7
        // 
        this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableSerial});
        this.xrTableRow7.Name = "xrTableRow7";
        this.xrTableRow7.Weight = 1;
        // 
        // xrTableSerial
        // 
        this.xrTableSerial.BackColor = System.Drawing.Color.White;
        this.xrTableSerial.ForeColor = System.Drawing.Color.Black;
        this.xrTableSerial.KeepTogether = true;
        this.xrTableSerial.Multiline = true;
        this.xrTableSerial.Name = "xrTableSerial";
        this.xrTableSerial.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
        this.xrTableSerial.StylePriority.UseBackColor = false;
        this.xrTableSerial.StylePriority.UseForeColor = false;
        this.xrTableSerial.StylePriority.UsePadding = false;
        this.xrTableSerial.StylePriority.UseTextAlignment = false;
        this.xrTableSerial.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableSerial.Weight = 10;
        // 
        // xrTable1
        // 
        this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(1.000023F, 50F);
        this.xrTable1.Name = "xrTable1";
        this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
        this.xrTable1.SizeF = new System.Drawing.SizeF(1000F, 25F);
        // 
        // xrTableRow1
        // 
        this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1});
        this.xrTableRow1.Name = "xrTableRow1";
        this.xrTableRow1.Weight = 1;
        // 
        // xrTableCell1
        // 
        this.xrTableCell1.BackColor = System.Drawing.Color.White;
        this.xrTableCell1.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell1.ForeColor = System.Drawing.Color.Black;
        this.xrTableCell1.Name = "xrTableCell1";
        this.xrTableCell1.StylePriority.UseBackColor = false;
        this.xrTableCell1.StylePriority.UseFont = false;
        this.xrTableCell1.StylePriority.UseForeColor = false;
        this.xrTableCell1.Text = "SerialNumber";
        this.xrTableCell1.Weight = 10;
        // 
        // xrTable4
        // 
        this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 25F);
        this.xrTable4.Name = "xrTable4";
        this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
        this.xrTable4.SizeF = new System.Drawing.SizeF(1000F, 25F);
        // 
        // xrTableRow4
        // 
        this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell12,
            this.xrModel,
            this.xrSkuname,
            this.xrSku,
            this.xrInvoice});
        this.xrTableRow4.Name = "xrTableRow4";
        this.xrTableRow4.Weight = 1;
        // 
        // xrTableCell12
        // 
        this.xrTableCell12.BackColor = System.Drawing.Color.White;
        this.xrTableCell12.ForeColor = System.Drawing.Color.Black;
        this.xrTableCell12.Name = "xrTableCell12";
        this.xrTableCell12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
        this.xrTableCell12.StylePriority.UseBackColor = false;
        this.xrTableCell12.StylePriority.UseForeColor = false;
        this.xrTableCell12.StylePriority.UsePadding = false;
        this.xrTableCell12.StylePriority.UseTextAlignment = false;
        this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell12.Weight = 2.0000001450340625;
        // 
        // xrModel
        // 
        this.xrModel.BackColor = System.Drawing.Color.White;
        this.xrModel.ForeColor = System.Drawing.Color.Black;
        this.xrModel.Name = "xrModel";
        this.xrModel.StylePriority.UseBackColor = false;
        this.xrModel.StylePriority.UseForeColor = false;
        this.xrModel.StylePriority.UseTextAlignment = false;
        this.xrModel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrModel.Weight = 2.000000221328015;
        // 
        // xrSkuname
        // 
        this.xrSkuname.BackColor = System.Drawing.Color.White;
        this.xrSkuname.ForeColor = System.Drawing.Color.Black;
        this.xrSkuname.Name = "xrSkuname";
        this.xrSkuname.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
        this.xrSkuname.StylePriority.UseBackColor = false;
        this.xrSkuname.StylePriority.UseForeColor = false;
        this.xrSkuname.StylePriority.UsePadding = false;
        this.xrSkuname.StylePriority.UseTextAlignment = false;
        this.xrSkuname.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrSkuname.Weight = 2.0000001794041027;
        // 
        // xrSku
        // 
        this.xrSku.BackColor = System.Drawing.Color.White;
        this.xrSku.ForeColor = System.Drawing.Color.Black;
        this.xrSku.Name = "xrSku";
        this.xrSku.StylePriority.UseBackColor = false;
        this.xrSku.StylePriority.UseForeColor = false;
        this.xrSku.StylePriority.UseTextAlignment = false;
        this.xrSku.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrSku.Weight = 2.0000001794041031;
        // 
        // xrInvoice
        // 
        this.xrInvoice.BackColor = System.Drawing.Color.White;
        this.xrInvoice.ForeColor = System.Drawing.Color.Black;
        this.xrInvoice.Name = "xrInvoice";
        this.xrInvoice.StylePriority.UseBackColor = false;
        this.xrInvoice.StylePriority.UseForeColor = false;
        this.xrInvoice.StylePriority.UseTextAlignment = false;
        this.xrInvoice.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrInvoice.Weight = 2.0000002417234666;
        // 
        // xrTable3
        // 
        this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrTable3.Name = "xrTable3";
        this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
        this.xrTable3.SizeF = new System.Drawing.SizeF(1000F, 25F);
        // 
        // xrTableRow3
        // 
        this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell8,
            this.xrTableCell9,
            this.xrTableCell10,
            this.xrTableCell11});
        this.xrTableRow3.Name = "xrTableRow3";
        this.xrTableRow3.Weight = 1;
        // 
        // xrTableCell7
        // 
        this.xrTableCell7.BackColor = System.Drawing.Color.White;
        this.xrTableCell7.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell7.ForeColor = System.Drawing.Color.Black;
        this.xrTableCell7.Name = "xrTableCell7";
        this.xrTableCell7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
        this.xrTableCell7.StylePriority.UseBackColor = false;
        this.xrTableCell7.StylePriority.UseFont = false;
        this.xrTableCell7.StylePriority.UseForeColor = false;
        this.xrTableCell7.StylePriority.UsePadding = false;
        this.xrTableCell7.StylePriority.UseTextAlignment = false;
        this.xrTableCell7.Text = "ModelName";
        this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell7.Weight = 2.0000001450340625;
        // 
        // xrTableCell8
        // 
        this.xrTableCell8.BackColor = System.Drawing.Color.White;
        this.xrTableCell8.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell8.ForeColor = System.Drawing.Color.Black;
        this.xrTableCell8.Name = "xrTableCell8";
        this.xrTableCell8.StylePriority.UseBackColor = false;
        this.xrTableCell8.StylePriority.UseFont = false;
        this.xrTableCell8.StylePriority.UseForeColor = false;
        this.xrTableCell8.StylePriority.UseTextAlignment = false;
        this.xrTableCell8.Text = "ModelCode";
        this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell8.Weight = 2.000000221328015;
        // 
        // xrTableCell9
        // 
        this.xrTableCell9.BackColor = System.Drawing.Color.White;
        this.xrTableCell9.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell9.ForeColor = System.Drawing.Color.Black;
        this.xrTableCell9.Name = "xrTableCell9";
        this.xrTableCell9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
        this.xrTableCell9.StylePriority.UseBackColor = false;
        this.xrTableCell9.StylePriority.UseFont = false;
        this.xrTableCell9.StylePriority.UseForeColor = false;
        this.xrTableCell9.StylePriority.UsePadding = false;
        this.xrTableCell9.StylePriority.UseTextAlignment = false;
        this.xrTableCell9.Text = "SKUName";
        this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell9.Weight = 2.0000001794041027;
        // 
        // xrTableCell10
        // 
        this.xrTableCell10.BackColor = System.Drawing.Color.White;
        this.xrTableCell10.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell10.ForeColor = System.Drawing.Color.Black;
        this.xrTableCell10.Name = "xrTableCell10";
        this.xrTableCell10.StylePriority.UseBackColor = false;
        this.xrTableCell10.StylePriority.UseFont = false;
        this.xrTableCell10.StylePriority.UseForeColor = false;
        this.xrTableCell10.StylePriority.UseTextAlignment = false;
        this.xrTableCell10.Text = "SKUCode";
        this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell10.Weight = 2.0000001794041031;
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.BackColor = System.Drawing.Color.White;
        this.xrTableCell11.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell11.ForeColor = System.Drawing.Color.Black;
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.StylePriority.UseBackColor = false;
        this.xrTableCell11.StylePriority.UseFont = false;
        this.xrTableCell11.StylePriority.UseForeColor = false;
        this.xrTableCell11.StylePriority.UseTextAlignment = false;
        this.xrTableCell11.Text = "Quantity";
        this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell11.Weight = 2.0000002417234666;
        this.xrTableCell11.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell11_BeforePrint);
        // 
        // topMarginBand1
        // 
        this.topMarginBand1.HeightF = 10F;
        this.topMarginBand1.Name = "topMarginBand1";
        // 
        // bottomMarginBand1
        // 
        this.bottomMarginBand1.HeightF = 10F;
        this.bottomMarginBand1.Name = "bottomMarginBand1";
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1,
            this.xrLabel4InvoiceDate,
            this.xrLabel3InvoiceDate,
            this.xrLabel2InvoiceNo,
            this.xrLabel1Invoiceno,
            this.lbTitle});
        this.ReportHeader.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.ReportHeader.HeightF = 235F;
        this.ReportHeader.Name = "ReportHeader";
        this.ReportHeader.StylePriority.UseFont = false;
        // 
        // lbTitle
        // 
        this.lbTitle.BackColor = System.Drawing.Color.White;
        this.lbTitle.ForeColor = System.Drawing.Color.Black;
        this.lbTitle.LocationFloat = new DevExpress.Utils.PointFloat(385.4167F, 0F);
        this.lbTitle.Name = "lbTitle";
        this.lbTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.lbTitle.SizeF = new System.Drawing.SizeF(181.25F, 42.12502F);
        this.lbTitle.StylePriority.UseBackColor = false;
        this.lbTitle.StylePriority.UseForeColor = false;
        this.lbTitle.Text = "Annexure";
        // 
        // xrLabel1Invoiceno
        // 
        this.xrLabel1Invoiceno.BackColor = System.Drawing.Color.White;
        this.xrLabel1Invoiceno.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel1Invoiceno.ForeColor = System.Drawing.Color.Black;
        this.xrLabel1Invoiceno.LocationFloat = new DevExpress.Utils.PointFloat(0F, 108.7917F);
        this.xrLabel1Invoiceno.Name = "xrLabel1Invoiceno";
        this.xrLabel1Invoiceno.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
        this.xrLabel1Invoiceno.SizeF = new System.Drawing.SizeF(180.25F, 23F);
        this.xrLabel1Invoiceno.StylePriority.UseBackColor = false;
        this.xrLabel1Invoiceno.StylePriority.UseFont = false;
        this.xrLabel1Invoiceno.StylePriority.UseForeColor = false;
        this.xrLabel1Invoiceno.Text = "Invoice Number :";
        // 
        // xrLabel2InvoiceNo
        // 
        this.xrLabel2InvoiceNo.BackColor = System.Drawing.Color.White;
        this.xrLabel2InvoiceNo.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel2InvoiceNo.ForeColor = System.Drawing.Color.Black;
        this.xrLabel2InvoiceNo.LocationFloat = new DevExpress.Utils.PointFloat(200F, 108.7917F);
        this.xrLabel2InvoiceNo.Name = "xrLabel2InvoiceNo";
        this.xrLabel2InvoiceNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
        this.xrLabel2InvoiceNo.SizeF = new System.Drawing.SizeF(200F, 23F);
        this.xrLabel2InvoiceNo.StylePriority.UseBackColor = false;
        this.xrLabel2InvoiceNo.StylePriority.UseFont = false;
        this.xrLabel2InvoiceNo.StylePriority.UseForeColor = false;
        // 
        // xrLabel3InvoiceDate
        // 
        this.xrLabel3InvoiceDate.BorderColor = System.Drawing.Color.White;
        this.xrLabel3InvoiceDate.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel3InvoiceDate.ForeColor = System.Drawing.Color.Black;
        this.xrLabel3InvoiceDate.LocationFloat = new DevExpress.Utils.PointFloat(0F, 158.7917F);
        this.xrLabel3InvoiceDate.Name = "xrLabel3InvoiceDate";
        this.xrLabel3InvoiceDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
        this.xrLabel3InvoiceDate.SizeF = new System.Drawing.SizeF(180.25F, 23F);
        this.xrLabel3InvoiceDate.StylePriority.UseBorderColor = false;
        this.xrLabel3InvoiceDate.StylePriority.UseFont = false;
        this.xrLabel3InvoiceDate.StylePriority.UseForeColor = false;
        this.xrLabel3InvoiceDate.Text = "Invoice Date :";
        // 
        // xrLabel4InvoiceDate
        // 
        this.xrLabel4InvoiceDate.BackColor = System.Drawing.Color.White;
        this.xrLabel4InvoiceDate.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel4InvoiceDate.ForeColor = System.Drawing.Color.Black;
        this.xrLabel4InvoiceDate.LocationFloat = new DevExpress.Utils.PointFloat(200F, 158.7917F);
        this.xrLabel4InvoiceDate.Name = "xrLabel4InvoiceDate";
        this.xrLabel4InvoiceDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
        this.xrLabel4InvoiceDate.SizeF = new System.Drawing.SizeF(177.0833F, 23F);
        this.xrLabel4InvoiceDate.StylePriority.UseBackColor = false;
        this.xrLabel4InvoiceDate.StylePriority.UseFont = false;
        this.xrLabel4InvoiceDate.StylePriority.UseForeColor = false;
        // 
        // PageFooter
        // 
        this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1});
        this.PageFooter.HeightF = 64.62502F;
        this.PageFooter.Name = "PageFooter";
        // 
        // xrPageInfo1
        // 
        this.xrPageInfo1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(400F, 10.00001F);
        this.xrPageInfo1.Name = "xrPageInfo1";
        this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo1.SizeF = new System.Drawing.SizeF(100F, 23F);
        this.xrPageInfo1.StylePriority.UseFont = false;
        this.xrPageInfo1.StylePriority.UseTextAlignment = false;
        this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrPictureBox1
        // 
        this.xrPictureBox1.ImageUrl = "~\\Transactions\\SapIntegration\\innerlogo.gif";
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(1.000023F, 19.12502F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(150F, 78.20834F);
        // 
        // formattingRule1
        // 
        this.formattingRule1.Name = "formattingRule1";
        // 
        // xrRptDispatchImeiDetail
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.topMarginBand1,
            this.bottomMarginBand1,
            this.ReportHeader,
            this.PageFooter});
        this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRule1});
        this.Landscape = true;
        this.Margins = new System.Drawing.Printing.Margins(73, 95, 10, 10);
        this.PageHeight = 827;
        this.PageWidth = 1169;
        this.PaperKind = System.Drawing.Printing.PaperKind.A4;
        this.Version = "10.1";
        ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

	}

	#endregion

    private void xrTableCell11_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

    }
}
