<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="STockTransferFlatReport.aspx.cs" Inherits="Reports_SalesChannel_STockTransferFlatReport" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Search Stock Transfer
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblrole" runat="server" Text="">Sales Channel Type:</asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbSalesChannelType" CssClass="formselect"
                                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbSalesChannelType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valtype" ControlToValidate="cmbSalesChannelType"
                                    CssClass="error" ErrorMessage="Please select a sales Channel Type " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="labelinscode" runat="server" Text="">STN Number:</asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSTNNumber" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                        </li>
                    </ul>
                    <div class="clear"></div>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblserfrmDate" runat="server" Text="">From Date: </asp:Label>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="Please select a from date."
                                defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." ValidationGroup="insert" />
                        </li>
                        <li class="text">
                            <asp:Label ID="lblsertodate" runat="server" Text="">To Date:</asp:Label>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="Please select a to date."
                                defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." ValidationGroup="insert" />
                        </li>
                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerch_Click"
                                    ValidationGroup="insert" CssClass="buttonbg" CausesValidation="True" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                    CssClass="buttonbg" CausesValidation="False" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
            <%-- <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                </Triggers>--%>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                <div class="mainheading">
                    List
                </div>
                <div class="export">
                    <asp:Button ID="btnExportToExcel" Text="" runat="server" OnClick="exportToExel_Click"
                        CssClass="excel" />
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="grdStockTransfer" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                            BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="STNNumber" FooterStyle-HorizontalAlign="Left"
                            FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="Altrow"
                            RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                            RowStyle-VerticalAlign="top" Width="100%" OnPageIndexChanging="grdStockTransfer_PageIndexChanging">
                            <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                            <Columns>
                                <asp:BoundField DataField="HOName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName1 %>" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RBHName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName2 %>" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ZSMName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName3 %>" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SBHName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName4 %>" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ASOName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName5 %>" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FromSalesChannelName" HeaderText="From" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ToSalesChannelName" HeaderText="To" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="STNNumber" HeaderText="STN Number" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DocketNumber" HeaderText="Docket Number" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Transaction Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lbluseraffdate" runat="server" Text='<%# Eval("TransactionDate","{0:dd-MMM-yy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="SKUName" HeaderText="SKU" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

