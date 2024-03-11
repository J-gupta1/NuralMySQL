<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalesOrderPrint.aspx.cs"
    Inherits="Transactions_Billing_SalesOrderPrint" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>

<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v10.1.Export, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraPivotGrid.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraReports.v10.1.Web, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%--<%@ Register Src="~/UserControls/ucPopUpTag.ascx" TagName="ucPopUp" TagPrefix="uc5" %>--%>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">--%>
<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <title></title>

    <script>
        function Print() {
            ReportViewer1.Print()
        }
    </script>
<%--#CC01: path changed START--%>
    <link href="../../../Assets/Css/printstyle.css" rel="stylesheet" type="text/css">
    <link href="../../../Assets/Css/print.css" rel="stylesheet" type="text/css" media="print">
    <link href="../../../Assets/Css/popup.css" rel="stylesheet" type="text/css" />
 <%--#CC01: path changed END--%>
</head>
<body>
    <form id="form1" runat="server">
<%--#CC01: Added table tags START--%>
   <table width="90%" align="center">
       <tr>
           <td align="center" >
<%--#CC01: Added table tags END--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <div class="float-right"><%--#CC01: style replaced with class--%>
        <input type="button" value="Close" class="button2" name="btnClose23" onclick="parent.WinPopup1.hide();" />
    </div>
    <div class="clear">
    </div>
    <div>
        <dx:ReportToolbar ID="ReportToolbar1" runat="server" ReportViewer="<%# ReportViewer1 %>"
            ShowDefaultButtons="False" Width="100%">
            <Items>
                <dx:ReportToolbarButton ItemKind="Search" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton ItemKind="PrintReport" />
                <dx:ReportToolbarButton ItemKind="PrintPage" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
                <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
                <dx:ReportToolbarLabel ItemKind="PageLabel" />
                <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                </dx:ReportToolbarComboBox>
                <dx:ReportToolbarLabel ItemKind="OfLabel" />
                <dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                <dx:ReportToolbarButton ItemKind="NextPage" />
                <dx:ReportToolbarButton ItemKind="LastPage" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                <dx:ReportToolbarButton ItemKind="SaveToWindow" />
                <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                    <Elements>
                        <dx:ListElement Value="pdf" />
                        
                    </Elements>
                </dx:ReportToolbarComboBox>
            </Items>
            <Styles>
                <LabelStyle>
                    <Margins MarginLeft="3px" MarginRight="3px" />
                </LabelStyle>
            </Styles>
        </dx:ReportToolbar>
    </div>
     <div class="clear">
    </div>
    <div>
        <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMessage1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
     <div class="clear">
    </div>
    <div>
        <asp:Panel ID="pnlReport" runat="server" Height="">
            <dx:ReportViewer ID="ReportViewer1" runat="server" PrintUsingAdobePlugIn="False"
                EnableViewState="False">
            </dx:ReportViewer>
        </asp:Panel>
    </div>

            </td>
        </tr>
    </table>

    </form>
</body>
</html>
