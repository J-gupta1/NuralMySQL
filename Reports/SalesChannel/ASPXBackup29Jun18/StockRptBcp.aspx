<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StockRptBcp.aspx.cs" Inherits="Reports_SalesChannel_StockRptBcp" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
        <tr>
            <td align="left" valign="top" height="420">
                <table cellspacing="0" cellpadding="0" width="965" border="0">
                    <tr>
                        <td valign="top" align="left">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td align="left" valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" valign="top">
                                                    <uc1:ucMessage ID="ucMsg" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" class="tableposition">
                                                    <div class="mainheading">
                                                        Search Stock Report</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" class="tableposition">
                                                    <div class="contentbox">
                                                        <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                            <tr>
                                                                <td colspan="5" height="20" class="mandatory" valign="top">
                                                                    (<font class="error">*</font>) marked fields are mandatory.
                                                                </td>
                                                            </tr>
                                                            <%--   <asp:Panel ID="pnlSales" runat="server" Visible="false">
                                                                            <tr>
                                                                                <td valign="top" align="right" width="10%" height="35" class="formtext">
                                                                                    SalesChannel Type:<font class="error">*</font>
                                                                                </td>
                                                                                <td align="left" valign="top" width="20%" class="formtext">
                                                                                    <asp:DropDownList ID="ddlSalesChannelType" runat="server" AutoPostBack="true" CssClass="form_select"
                                                                                        OnSelectedIndexChanged="ddlSalesChannelType_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                    <asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="ddlSalesChannelType"
                                                                                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel type."
                                                                                        SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                                <td valign="top" align="right" width="10%" class="formtext">
                                                                                    SalesChannel:<font class="error">*</font>
                                                                                </td>
                                                                                <td valign="top" align="left" width="20%" class="formtext">
                                                                                    <asp:DropDownList ID="ddlChannelName" runat="server" CssClass="form_select">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                    <asp:RequiredFieldValidator ID="reqVChannelName" runat="server" ControlToValidate="ddlChannelName"
                                                                                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel"
                                                                                        SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                                <td valign="top" align="left" width="40%">
                                                                                </td>
                                                                            </tr>
                                                                        </asp:Panel>--%>
                                                            <tr>
                                                                <td valign="top" align="right" height="35" width="15%" class="formtext">
                                                                    Sales Channel Type:<font class="error">*</font>
                                                                </td>
                                                                <td align="left" valign="top" width="20%" class="formtext">
                                                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form_select4">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlType"
                                                                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel type."
                                                                        SetFocusOnError="true" ValidationGroup="vgStockRpt"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td valign="top" align="right" height="35" width="15%" class="formtext">
                                                                    Closing as on date:<font class="error">*</font>
                                                                </td>
                                                                <td align="left" valign="top" width="20%" class="formtext">
                                                                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="vgStockRpt"
                                                                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                </td>
                                                                <td valign="top" align="right" width="10%" class="formtext">
                                                                    &nbsp;
                                                                </td>
                                                                <td valign="top" align="left" width="20%" class="formtext">
                                                                    &nbsp;
                                                                    <asp:Button ID="btnSearch" Text="Download" runat="server" ValidationGroup="vgStockRpt"
                                                                        ToolTip="Search" CssClass="buttonbg" CausesValidation="true" OnClick="btnSearch_Click" />
                                                                </td>
                                                                <td valign="top" align="left" width="35%">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="right" height="35" width="15%" class="formtext">
                                                                </td>
                                                                <td align="left" valign="top" width="20%" class="formtext">
                                                                </td>
                                                                <td valign="top" align="right" height="35" width="15%" class="formtext">
                                                                    &nbsp;</td>
                                                                <td align="left" valign="top" width="20%" class="formtext">
                                                                    
                                                                    <asp:HyperLink ID="btnExportToExcel" runat="server" Visible="false" NavigateUrl="~/Excel/Download/BcpFile/Stock.csv">Open File</asp:HyperLink>
                                                                </td>
                                                                <td valign="top" align="right" width="10%" class="formtext">
                                                                </td>
                                                                <td valign="top" align="left" width="20%" class="formtext">
                                                                </td>
                                                                <td valign="top" align="left" width="35%">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
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
