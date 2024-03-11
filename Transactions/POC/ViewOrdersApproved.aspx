<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewOrdersApproved.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Transactions_POC_ViewOrdersApproved" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">

    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        View Orders List
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="mandatory">
                    (*) Marked fields are mandatory
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Sales Channel Type: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbChannelType" runat="server" CssClass="formselect" AutoPostBack="True"
                                    OnSelectedIndexChanged="cmbChannelType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="cmbChannelType" Display="Dynamic"
                                    CssClass="error" ValidationGroup="Entry" InitialValue="0" runat="server" ErrorMessage="Please select a Sales Channel Type "></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Sales Channel Name: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlSalesChannelName" runat="server" CssClass="formselect"
                                AutoPostBack="False">
                            </asp:DropDownList>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlSalesChannelName"
                                        CssClass="error" ValidationGroup="Entry" InitialValue="0" runat="server" ErrorMessage="Please select a Sales Channel Name "></asp:RequiredFieldValidator>--%>                           
                        </li>
                    </ul>
                    <div class="clear"></div>
                    <ul>
                        <li class="text">Order From: <span class="error">+</span>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                                RangeErrorMessage="Date should be less then equal to current date." ValidationGroup="save" />
                        </li>
                        <li class="text">Order To: <span class="error">+</span>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                                RangeErrorMessage="Date should be less then equal to current date." ValidationGroup="save" />
                        </li>
                    </ul>
                    <ul>
                        <li class="text">Status: <span class="error">+</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlStatus" runat="server" Enabled="false" CssClass="formselect"
                                AutoPostBack="false">
                                <asp:ListItem Text="Select" Value="0">
                                </asp:ListItem>
                                <asp:ListItem Text="Approved" Value="2" Selected="True">
                                </asp:ListItem>
                                <%--    <asp:ListItem Text="Pending" Value="1">
                                    </asp:ListItem>--%>
                            </asp:DropDownList>
                        </li>
                        <li class="text">Order No.: <span class="error">+</span>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtOrderNumber" runat="server" MaxLength="15" CssClass="formfields"></asp:TextBox>
                        </li>
                    </ul>
                    <ul>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="BtnSearch" runat="server" Text="Search" CssClass="buttonbg" CausesValidation="true"
                                    ValidationGroup="Entry" OnClick="BtnSearch_Click" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="False"
                                    OnClick="btnCancel_Click" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="dvheading" runat="server" visible="false">
                <div class="mainheading">
                    Orders List
                </div>
            </div>
            <div id="dvAction" runat="server" visible="false">
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="gvOrder" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            CellSpacing="1" DataKeyNames="OrderID" EditRowStyle-CssClass="editrow"
                            EmptyDataText="No Record Found" GridLines="None" HeaderStyle-CssClass="gridheader"
                            RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" Width="100%"
                            OnRowCommand="gvOrder_RowCommand">
                            <RowStyle CssClass="gridrow" />
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderNumber"
                                    HeaderText="OrderNumber"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderDateDisplay"
                                    HeaderText="OrderDateDisplay"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderFrom"
                                    HeaderText="OrderFrom"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderTo"
                                    HeaderText="OrderTo"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="DispatchStatus"
                                    HeaderText="Dispatch Status"></asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDetailID" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"OrderID"))%>'
                                            Visible="false"></asp:Label>
                                        <asp:Button ID="btnDetails" runat="server" Text="Details" CssClass="buttonbg" CausesValidation="false"
                                            CommandArgument='<%#Eval("OrderID") %>' CommandName="Details" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gridheader" />
                            <EditRowStyle CssClass="editrow" />
                        </asp:GridView>
                    </div>
                </div>               
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:UpdatePanel ID="updDetail" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div runat="server" id="dvDetail" visible="false">
                <table id="Table2" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <div class="mainheading">
                                View Order Detail
                            </div>
                            <div class="contentbox">
                                <div class="grid1">
                                    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                CellSpacing="1" DataKeyNames="OrderDetailID" EditRowStyle-CssClass="editrow"
                                                EmptyDataText="No Record Found" GridLines="None" HeaderStyle-CssClass="gridheader"
                                                RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" Width="100%">
                                                <RowStyle CssClass="gridrow" />
                                                <Columns>
                                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderNumber"
                                                        HeaderText="Order Number"></asp:BoundField>
                                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderDateDisplay"
                                                        HeaderText="Order Date"></asp:BoundField>
                                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderFrom"
                                                        HeaderText="Order From"></asp:BoundField>
                                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderTo"
                                                        HeaderText="Order To"></asp:BoundField>
                                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="skuCode"
                                                        HeaderText="Sku Code"></asp:BoundField>
                                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="skuName"
                                                        HeaderText="Sku Name"></asp:BoundField>
                                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderedQuantity"
                                                        HeaderText="Ordered Quantity"></asp:BoundField>
                                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ApprovedQuantity"
                                                        HeaderText="Approved Quantity"></asp:BoundField>

                                                </Columns>
                                                <HeaderStyle CssClass="gridheader" />
                                                <EditRowStyle CssClass="editrow" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
