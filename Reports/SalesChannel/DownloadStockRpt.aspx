<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="DownloadStockRpt.aspx.cs" Inherits="Reports_SalesChannel_DownloadStock" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<%--05 June 2018,Rajnish Kumar,#CC01,SalesChannelId and CityId Filter.--%>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMsg" runat="server" />
    <div class="mainheading">
        Search Stock Report
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Sales Channel Type: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlType"
                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel type."
                            SetFocusOnError="true" ValidationGroup="vgStockRpt"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">
                    <asp:Label ID="Label2" runat="server" Text="">Sales Channel: <span class="error">*</span></asp:Label>
                </li>
                <li class="field">
                    <asp:DropDownList ID="DdlSaleschannel" CssClass="formselect" runat="server">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </li>
                <li class="text">Closing as on date: <span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="vgStockRpt"
                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                </li>
            </ul>
            <ul>
                <li class="text">
                    <asp:Label ID="lbllocation" runat="server" Text="">Region:  <span class="error">&nbsp;</span></asp:Label>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddllocation" runat="server" CssClass="formselect" AutoPostBack="false">
                    </asp:DropDownList>
                </li>
                <li class="text">
                    <asp:Label ID="lblState" runat="server" Text="">State:  <span class="error">&nbsp;</span></asp:Label>
                </li>
                <li class="field">

                    <asp:DropDownList ID="ddlState" runat="server" CssClass="formselect" AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                    </asp:DropDownList>

                </li>
                <%--  #CC01--%>

                <li class="text">
                    <asp:Label ID="Label3" runat="server" Text="">City: <span class="error">*</span></asp:Label>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlCity" CssClass="formselect" runat="server">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </li>
                <%--  #CC01--%>
            </ul>
            <ul>
                <li class="text">
                    <asp:Label ID="lblProductCategory" runat="server" Text="">Product Category:  <span class="error">&nbsp;</span></asp:Label>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlProductCategory" runat="server" CssClass="formselect"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlProductCategory_SelectedIndexChanged">
                    </asp:DropDownList>

                </li>
                <li class="text">
                    <asp:Label ID="lblsku" runat="server" Text="">Model Name: <span class="error">&nbsp;</span></asp:Label>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlModelName" CssClass="formselect" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlModelName_SelectedIndexChanged">
                    </asp:DropDownList>

                    <%-- <div style="width: 180px;">
                                                                        <asp:RequiredFieldValidator runat="server" ID="valprodcat" ControlToValidate="ddlModelName"
                                                                            CssClass="error" ValidationGroup="SalesReport1" ErrorMessage="Please select a Model Name "
                                                                            InitialValue="0" /></div>--%>
                </li>
                <li class="text">
                    <asp:Label ID="lblSkuName" runat="server" Text="">Sku Name: <span class="error">&nbsp;</span></asp:Label>
                </li>
                <li class="field">

                    <asp:DropDownList ID="ddlSku" runat="server" CssClass="formselect" AutoPostBack="false">
                    </asp:DropDownList>

                    <%--<div style="width: 140px;">
                                                                        <asp:RequiredFieldValidator runat="server" ID="valModel" ControlToValidate="ddlSku"
                                                                            CssClass="error" ValidationGroup="SalesReport1" ErrorMessage="Please select a SKU "
                                                                            InitialValue="0" /></div>--%>
                </li>
            </ul>
            <ul>
                <li class="text"></li>
                <li class="field">
                    <div>
                        <asp:CheckBox ID="chkZeroQuantity" runat="server" />
                        <asp:Label ID="lblZeroQuantity" CssClass="frmtxt1" runat="server" Text="">Show Zero Qty Records</asp:Label>
                    </div>
                </li>
                <li class="text"></li>
                <li class="field">
                    <div class="float-margin">
                        <asp:Button ID="btnSearch" Text="Download" runat="server" ValidationGroup="vgStockRpt"
                            ToolTip="Search" CssClass="buttonbg" CausesValidation="true" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-left">
                        <asp:Button ID="ExportToCSV" ToolTip="Export to CSV" ValidationGroup="vgStockRpt" CssClass="buttonbg" runat="server"
                            Text="ExportToCSV" OnClick="ExportToCSV_Click" Visible="false" />
                    </div>
                </li>

            </ul>
        </div>
    </div>
</asp:Content>
