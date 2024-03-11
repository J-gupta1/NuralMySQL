<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="DownloadSalesRptLastSale.aspx.cs" Inherits="Reports_SalesChannel_DownloadSalesRptLastSale" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0">
        <tr>
            <td align="left" valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
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
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td align="left" valign="top" class="tableposition">
                                                    <div class="mainheading">
                                                        Sales Report Search</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" valign="top">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" class="tableposition">
                                                    <div class="contentbox">
                                                        <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                            <tr>
                                                                <td colspan="6" class="mandatory" valign="top">
                                                                    (<span class="error">*</span>) marked fields are mandatory.
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="right" width="15%" height="35">
                                                                    Sales Type:<span class="error">*</span>
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <div style="width: 145px;">
                                                                        <asp:DropDownList ID="ddlSalesType" runat="server" CssClass="form_select4">
                                                                        </asp:DropDownList>
                                                                        <br />
                                                                        <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlSalesType"
                                                                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales type."
                                                                            SetFocusOnError="true" ValidationGroup="SalesReport"></asp:RequiredFieldValidator></div>
                                                                </td>
                                                                <td valign="top" align="right" width="10%">
                                                                    Date From: <span class="error">*</span>
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                                                                        ValidationGroup="SalesReport" defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                    <br />
                                                                </td>
                                                                <td valign="top" align="right" width="10%">
                                                                    Date To: <span class="error">*</span>
                                                                </td>
                                                                <td valign="top" align="left">
                                                                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="SalesReport"
                                                                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="right" height="35">
                                                                    <asp:Label ID="lbllocation" runat="server" Text="">Region:  <font class="error">&nbsp;</font></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:DropDownList ID="ddllocation" runat="server" CssClass="form_select4" AutoPostBack="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td valign="top" align="right">
                                                                    <asp:Label ID="lblState" runat="server" Text="">State:  <font class="error">&nbsp;</font></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <div style="width: 135px;">
                                                                        <asp:DropDownList ID="ddlState" runat="server" CssClass="form_select4" AutoPostBack="false">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </td>
                                                                <td valign="top" align="right">
                                                                    <asp:Label ID="lblProductCategory" runat="server" Text="">Product Category:  <font class="error">&nbsp;</font></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <div style="width: 135px;">
                                                                        <asp:DropDownList ID="ddlProductCategory" runat="server" CssClass="form_select4"
                                                                            AutoPostBack="false">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="right">
                                                                    <asp:Label ID="lblsku" runat="server" Text="">Model Name: <span class="error">&nbsp;</span></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <div style="width: 135px;">
                                                                        <asp:DropDownList ID="ddlModelName" CssClass="form_select4" runat="server" AutoPostBack="True"
                                                                            OnSelectedIndexChanged="ddlModelName_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div style="width: 180px;">
                                                                        <asp:RequiredFieldValidator runat="server" ID="valprodcat" ControlToValidate="ddlModelName"
                                                                            CssClass="error" ValidationGroup="SalesReport1" ErrorMessage="Please select a Model Name "
                                                                            InitialValue="0" /></div>
                                                                </td>
                                                                <td valign="top" align="right">
                                                                    <asp:Label ID="lblSkuName" runat="server" Text="">Sku Name: <span class="error">&nbsp;</span></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <div style="width: 135px;">
                                                                        <asp:DropDownList ID="ddlSku" runat="server" CssClass="form_select4" AutoPostBack="false">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div style="width: 140px;">
                                                                        <asp:RequiredFieldValidator runat="server" ID="valModel" ControlToValidate="ddlSku"
                                                                            CssClass="error" ValidationGroup="SalesReport1" ErrorMessage="Please select a SKU "
                                                                            InitialValue="0" /></div>
                                                                </td>
                                                                <td align="right" valign="top">
                                                                    <asp:CheckBox ID="chkZeroQuantity" runat="server" />
                                                                </td>
                                                                <td valign="top" align="left">
                                                                    <div style="margin-top: 3px;">
                                                                        <asp:Label ID="lblZeroQuantity" runat="server" Text="">Show Zero Qty Records</asp:Label>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="right">
                                                                    <asp:CheckBox ID="chkSB" runat="server" Text="With Serial /Batch" TextAlign="Right" Checked="True" Enabled="False" />
                                                                    <span class="error">&nbsp;</span>
                                                                </td>
                                                                <td valign="top" align="left" colspan="3">
                                                                    <asp:HyperLink ID="btnExportToExcel" runat="server" Visible="false" NavigateUrl="~/Excel/Download/BcpFile/Stock.csv">Open File</asp:HyperLink>
                                                                     <div class="float-margin">
                                                                    <asp:Button ID="btnSearch" Text="Download" runat="server" ValidationGroup="SalesReport"
                                                                        ToolTip="Download" CssClass="buttonbg" CausesValidation="true" OnClick="btnSearch_Click" />
                                                                        </div>
                                                                         <div class="float-left">
                                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                                                        OnClick="btnCancel_Click" />
                                                                        </div>
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
