<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/ReportPage.master" AutoEventWireup="true" CodeFile="RetailerLaggardReport.aspx.cs" Inherits="Reports_New_Reports_RetailerLaggardReport" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" Runat="Server">
      <table cellspacing="0" cellpadding="0" width="965" border="0">
        <tr>
            <td align="left" valign="top" height="420">
                <table cellspacing="0" cellpadding="0" width="965" border="0">
                    <tr>
                        <td valign="top" align="left">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td align="left" valign="top">
                                        <uc1:ucMessage ID="ucMsg" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" class="tableposition">
                                                    <div class="mainheading">
                                                        Retailer Laggard Report</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" class="tableposition">
                                                    <div class="contentbox">
                                                        <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                            <tr>
                                                                <td colspan="6" height="20" class="mandatory" valign="top">
                                                                    (<font class="error">*</font>) marked fields are mandatory.
                                                                </td>
                                                            </tr>
                                                          <%--  <tr>
                                                                <td valign="top" align="right" width="15%" height="35">
                                                                    Sales Channel Type:<font class="error">*</font>
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form_select4">
                                                                    </asp:DropDownList>
                                                                    <div style="width: 180px;">
                                                                        <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlType"
                                                                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel type."
                                                                            SetFocusOnError="true" ValidationGroup="vgStockRpt"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </td>
                                                                <td valign="top" align="right" width="18%">
                                                                    Closing as on date:<font class="error">*</font>
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="vgStockRpt"
                                                                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                </td>
                                                                <td valign="top" align="right"  width="10%">
                                                                    <asp:Label ID="lbllocation" runat="server" Text="">Region:  <font class="error">&nbsp;</font></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:DropDownList ID="ddllocation" runat="server" CssClass="form_select4" AutoPostBack="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>--%>
                                                            <tr>
                                                                <%--<td valign="top" align="right" height="35">
                                                                    <asp:Label ID="lblState" runat="server" Text="">State:  <font class="error">&nbsp;</font></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <div style="width: 135px;">
                                                                        <asp:DropDownList ID="ddlState" runat="server" CssClass="form_select4" AutoPostBack="false">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </td>--%>
                                                                <td valign="top" align="right" width="18%">
                                                                   From date:<font class="error">*</font>
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="To date required." ValidationGroup="vgStockRpt"
                                                                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                </td>
                                                                <td valign="top" align="right" width="18%">
                                                                 To date:<font class="error">*</font>
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="vgStockRpt"
                                                                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                </td>
                                                                <%--<td valign="top" align="right">
                                                                    <asp:Label ID="lblsku" runat="server" Text="">Model Name:<span class="error">&nbsp;</span></asp:Label>
                                                                </td>--%>
                                                               <%-- <td width="13%" align="right">
                                                             <td valign="top" align="left" width="38%">--%>

                                                           <%--<asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" />--%>
                                                                 <%--<asp:LinkButton ID="lnkExportToExcel" runat="server" Text="Export to Excel"
                                                                     CssClass="elink2" Style="color: green" OnClick="lnkExportToExcel_Click"></asp:LinkButton>--%>

                                                                    <%--    </td>
                                                            </td>--%>
                                                            </tr>
                                                           <tr>
                                                               <td></td>
                                                               <td>
                                                                <asp:LinkButton ID="lnkExportToExcel" runat="server" Text="Export to Excel"
                                                                     CssClass="elink2" Style="color: green" OnClick="lnkExportToExcel_Click"></asp:LinkButton></td></tr>
                                                                <%--<td valign="top" align="right">
                                                                    <asp:Label ID="lblSkuName" runat="server" Text="">Sku Name:<span class="error">&nbsp;</span></asp:Label>
                                                                </td>--%>        
                                                             <%--<td align="left" valign="top">
                                                                    <div style="width: 135px;">
                                                                        <asp:DropDownList ID="ddlSku" runat="server" CssClass="form_select4" AutoPostBack="false">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div style="width: 140px;">
                                                                        <asp:RequiredFieldValidator runat="server" ID="valModel" ControlToValidate="ddlSku"
                                                                            CssClass="error" ValidationGroup="SalesReport1" ErrorMessage="Please select a SKU "
                                                                            InitialValue="0" /></div>
                                                                </td>
                                                                <td valign="top" align="right">
                                                                    <div class="float-left">
                                                                        <asp:CheckBox ID="chkZeroQuantity" runat="server" />
                                                                    </div>
                                                                    <div class="float-left" style="margin-top: 3px;">
                                                                        <asp:Label ID="lblZeroQuantity" runat="server" Text="">Show Zero Qty Records</asp:Label></div>
                                                                </td>
                                                             
                                                                <td valign="top" align="left">
                                                                    <asp:Button ID="btnSearch" Text="Download" runat="server" ValidationGroup="vgStockRpt"
                                                                        ToolTip="Search" CssClass="buttonbg" CausesValidation="true" OnClick="btnSearch_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="left">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>


