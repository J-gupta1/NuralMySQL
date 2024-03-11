<%@ Page Language="C#" MasterPageFile="~/CommonMasterPages/ReportPage.master" AutoEventWireup="true"
    CodeFile="RetailerWiseSecTerStockVol.aspx.cs" Inherits="Reports_DashBoardDMS_RetailerWiseSecTerStockVol" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMsg" runat="server" />
    <div class="mainheading">
        Retailer Wise Sec/Ter Stock Volume
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H30-C3-S">
            <ul>
                <li class="text">From Date:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From Date require!" ValidationGroup="vgStockRpt"
                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                </li>
                <li class="text">To Date:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To Date require!" ValidationGroup="vgStockRpt"
                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                </li>
                <%--<td valign="top" align="right">
                                                                    <asp:Label ID="lblsku" runat="server" Text="">Model Name:<span class="error">&nbsp;</span></asp:Label>
                                                                </td>--%>
                <%-- <td width="13%" align="right">
                                                             <td valign="top" align="left" width="38%">--%>

                <%--<asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" />--%>
                <%-- <asp:LinkButton ID="lnkExportToExcel" runat="server" Text="Export to Excel"
                                                                     CssClass="elink2" Style="color: green" OnClick="lnkExportToExcel_Click"></asp:LinkButton>

                                                                        </td>--%>
                <li class="field">
                    <asp:LinkButton ID="LinkButton1" runat="server" Text="Export to Excel"
                        CssClass="elink2" Style="color: green" ValidationGroup="vgStockRpt" CausesValidation="true" OnClick="lnkExportToExcel_Click"></asp:LinkButton>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
