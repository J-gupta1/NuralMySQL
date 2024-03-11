<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StockingUser.aspx.cs" Inherits="Reports_SalesChannel_StockHoldingUser" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMsg" runat="server" />
    <div class="mainheading">
        Stock Holding User Search
    </div>

    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Sales Channel Type:<span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="formselect">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlType"
                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel type."
                            SetFocusOnError="true" ValidationGroup="vgUser"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">Date From:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                        ValidationGroup="vgUser" defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />

                </li>
                <li class="text">Date To:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="vgUser"
                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                </li>
                <li class="text"></li>
                <li class="field">
                    <div class="float-margin">
                        <asp:HiddenField ID="hfSearch" Value="0" Visible="false" runat="server" />
                        <asp:Button ID="btnShow" Text="Search" runat="server" ValidationGroup="vgUser"
                            ToolTip="Search" CssClass="buttonbg" CausesValidation="true" OnClick="btnShow_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                            CssClass="buttonbg" CausesValidation="False" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                <div class="mainheading">
                    List
                </div>
                <div class="export">
                    <asp:Button ID="btnExportToExcel" Text="" runat="server" OnClick="exportToExel_Click"
                        CssClass="excel" CausesValidation="False" />
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="grdUser" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                            BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="SalesChannelID"
                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                            AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                            OnPageIndexChanging="grdUser_PageIndexChanging" HeaderStyle-CssClass="gridheader"
                            RowStyle-VerticalAlign="top" Width="100%">
                            <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                            <Columns>
                                <asp:BoundField DataField="HO" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName1 %>"
                                    HtmlEncode="false" NullDisplayText="N/A">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RBH" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName2 %>"
                                    HtmlEncode="false" NullDisplayText="N/A">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ZBH" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName3 %>"
                                    HtmlEncode="false" NullDisplayText="N/A">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SBH" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName4 %>"
                                    HtmlEncode="false" NullDisplayText="N/A">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ASO" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName5 %>"
                                    HtmlEncode="false" NullDisplayText="N/A">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SalesChannelTypeName" HeaderText="SalesChannelType" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SalesChannelName" HeaderText="SalesChannel" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OpeningStockDate" DataFormatString="{0:dd-MMM-yy}" HeaderText="Opening Stock Date"
                                    HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <%--  <asp:TemplateField HeaderText="User Activity Date">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbluseraffdate" runat="server" Text='<%# Eval("UserActivityDate","{0:dd-MMM-yy}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
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
