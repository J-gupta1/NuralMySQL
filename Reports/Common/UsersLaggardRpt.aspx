<%@ Page Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="UsersLaggardRpt.aspx.cs" Inherits="Reports_Common_UsersLaggardRpt" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucServiceEntity.ascx" TagName="ucServiceEntity" TagPrefix="uc3" %>
<%--#CC04 Added --%>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMessage1" runat="server" ShowCloseButton="false" />
    <div style="display: block;">
        <%--#CC02 ADDED style="display: none;"--%> <%--#CC04 style="display: none;" property changed to block --%>
        <div class="mainheading">
            Laggard Report
        </div>
    </div>
    <div style="display: block;">
        <%--#CC02 ADDED style="display: none;"--%> <%--#CC04 style="display: none;" property changed to block --%>
        <div class="contentbox">
            <div style="display: none;">
                <%--#CC04 style="display: none;" property Added  --%>
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
            </div>
            <div class="H25-C3-S">
                <ul style="display: none;">
                    <%--#CC04 style="display: none;" property Added  --%>
                    <asp:UpdatePanel runat="server" ID="updInput" UpdateMode="Conditional">
                        <ContentTemplate>
                            <li class="text">
                                <asp:Label ID="lblHierarchyLevel" runat="server" Text="">Hierarchy Level<span class="error">*</span>:</asp:Label>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="ddlHierarchyLevel" CssClass="formselect" runat="server" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlHierarchyLevel_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="valpricename" ControlToValidate="ddlHierarchyLevel"
                                        CssClass="error" ErrorMessage="Please select a HierarchyLevel  " InitialValue="0"
                                        ValidationGroup="insert" />
                                </div>
                            </li>
                            <li class="text">
                                <asp:Label ID="lblLocation" runat="server" Text="">Location<span class="error">*</span>:</asp:Label>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList ID="ddllocation" runat="server" CssClass="formselect">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddllocation"
                                        CssClass="error" ErrorMessage="Please select a location  " InitialValue="0" ValidationGroup="insert" />
                                </div>
                            </li>
                            <li>
                                <%-- #CC04 Comment Start  <asp:Button ID="btnSearch" runat="server" ValidationGroup="insert" CssClass="buttonbg"  CausesValidation="True" Text="Search" OnClick="btnSearch_Click" /> #CC04 Comment End--%>
                            </li>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ul>
                <%-- #CC04 Add Start --%>
                <ul>
                    <li class="text">
                        <asp:Label ID="Label1" runat="server" Text="National Distributor"></asp:Label>:
                    </li>
                    <li class="field">
                        <uc3:ucServiceEntity ID="ucServiceEntity" runat="server" />

                    </li>
                    <li class="text">
                        <asp:Label ID="lblStatusText" runat="server" Text="Status"></asp:Label>:
                    </li>
                    <li class="field">
                        <asp:DropDownList ID="ddlStatus" CssClass="formselect" runat="server">
                            <asp:ListItem Value="-1" Text="Select All" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Active"></asp:ListItem>
                            <asp:ListItem Value="0" Text="In-Active"></asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li class="text">
                        <asp:Button ID="btnDownload" runat="server" CssClass="buttonbg"
                            CausesValidation="false" Text="Download" OnClick="btnDownload_Click" />

                    </li>
                </ul>
                <%-- #CC04 Add End --%>
            </div>
        </div>
    </div>
    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                <div>
                    <div class="mainheading">
                        List
                    </div>
                    <div class="export">
                        <asp:Button ID="btnExportToExcel" Text="" runat="server" CssClass="excel" OnClick="btnExportToExcel_Click" />
                    </div>
                    <div class="contentbox">
                        <div class="grid1">
                            <%-- <asp:GridView ID="gridLaggard" AllowPaging="True" AutoGenerateColumns="true" runat="server"
                                                                        CssClass="gridbg" Width="100%" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                                                                        HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="gridrow1"
                                                                        GridLines="none" FooterStyle-CssClass="gridfooter" HeaderStyle-VerticalAlign="Middle"
                                                                        HeaderStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="left"
                                                                        FooterStyle-HorizontalAlign="left" FooterStyle-VerticalAlign="Top" OnPageIndexChanging="gridLaggard_PageIndexChanging">
                                                              <pagerstyle CssClass="pagerstyle" />
                                                                    </asp:GridView>--%>
                            <asp:GridView ID="gridLaggard" runat="server" AllowPaging="True" AutoGenerateColumns="false" EnableViewState="false"
                                BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="SalesChannelID"
                                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                                AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                                HeaderStyle-CssClass="gridheader" RowStyle-VerticalAlign="top" Width="100%" OnPageIndexChanging="gridLaggard_PageIndexChanging">
                                <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                                <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                                <Columns>
                                    <asp:BoundField DataField="SalesChannelTypeName" HeaderText="Type of Entity" HtmlEncode="false">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ParentSalesChannelName" HeaderText="Mapped Parent Entity Name" HtmlEncode="false">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SalesChannelCode" HeaderText="SalesChannel Code" HtmlEncode="false">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="SalesChannelName" HeaderText="SalesChannel Name" HtmlEncode="false">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="HOName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName1 %>"
                                        HtmlEncode="false">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RBHName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName2 %>"
                                        HtmlEncode="false">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ZBHName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName3 %>"
                                        HtmlEncode="false">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SBHName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName4 %>"
                                        HtmlEncode="false">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ASOName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName5 %>"
                                        HtmlEncode="false">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MaximumTransDate" HeaderText="Last Transaction Date" HtmlEncode="false" DataFormatString="{0:d}">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="OverDueDays" HeaderText="Ageing in Days" HtmlEncode="false">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AgingSlabText" HeaderText="Ageing In Slab" HtmlEncode="false">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LastTransactionCreationDate" HeaderText="Last Transaction Creation Date"
                                        HtmlEncode="false" ItemStyle-Width="120px">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="lastloginon" HeaderText="Last Login Date" HtmlEncode="false"
                                        ItemStyle-Width="120px">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
