<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmPrintAnnexure.aspx.cs" Inherits="Transactions_SAPAnnexure_FrmPrintAnnexure" %>

<%@ Register Assembly="DevExpress.XtraReports.v10.1.Web, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dxxr" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
       <%=strApplicationTitle%> </title>
</head>
   <body>
    <form id="form1" runat="server">
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <div>
        <dxxr:ReportToolbar ID="rptToolBar" runat="server" ShowDefaultButtons="False" ReportViewer="<%# rptViewer %>"
            EnableViewState="False" Width="100%">
            <items>
                <dxxr:ReportToolbarButton ItemKind="Search" />
                <dxxr:ReportToolbarSeparator />
                <dxxr:ReportToolbarButton ItemKind="PrintReport" />
                <dxxr:ReportToolbarButton ItemKind="PrintPage" />
                <dxxr:ReportToolbarSeparator />
                <dxxr:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
                <dxxr:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
                <dxxr:ReportToolbarLabel ItemKind="PageLabel" />
                <dxxr:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                </dxxr:ReportToolbarComboBox>
                <dxxr:ReportToolbarLabel ItemKind="OfLabel" />
                <dxxr:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                <dxxr:ReportToolbarButton ItemKind="NextPage" />
                <dxxr:ReportToolbarButton ItemKind="LastPage" />
                <dxxr:ReportToolbarSeparator />
                <dxxr:ReportToolbarButton ItemKind="SaveToDisk" />
                <dxxr:ReportToolbarButton ItemKind="SaveToWindow" />
                <dxxr:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                    <Elements>
                        <dxxr:ListElement Value="pdf" />
                        <dxxr:ListElement Value="xls" />
                        <dxxr:ListElement Value="xlsx" />
                        <dxxr:ListElement Value="rtf" />
                        <dxxr:ListElement Value="mht" />
                        <dxxr:ListElement Value="txt" />
                        <dxxr:ListElement Value="csv" />
                        <dxxr:ListElement Value="png" />
                    </Elements>
                </dxxr:ReportToolbarComboBox>
                <dxxr:ReportToolbarButton Name="rtbClose" Text="Close" />
            </items>
            <styles>
                <LabelStyle>
                    <Margins MarginLeft="3px" MarginRight="3px" />
                </LabelStyle>
            </styles>
            <clientsideevents itemclick="function(s, e) {
    if(e.item.name == 'rtbClose')
    {
		 window.close();
    }
}" />
        </dxxr:ReportToolbar>
        <dxxr:ReportViewer ID="rptViewer" runat="server" ClientInstanceName="rptViewer" EnableViewState="False">
        </dxxr:ReportViewer>
    </div>
    </form>
</body>

</html>
