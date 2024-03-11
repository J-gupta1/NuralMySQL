<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="GRNFlatReport.aspx.cs" Inherits="Reports_SalesChannel_GRNFlatReport" %>

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
        GRN Report
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="labelinscode" runat="server" Text="">GRN Number:</asp:Label>
                         </li>
                        <li class="field">
                            <asp:TextBox ID="txtGRNNumber" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                        </li>
                         <li class="text">
                            <asp:Label ID="lblserfrmDate" runat="server" Text="">From Date: </asp:Label>
                         </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateFrom" runat="server" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                        </li>
                        <li class="text">
                            <asp:Label ID="lblsertodate" runat="server" Text="">To Date:</asp:Label>
                         </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateTo" runat="server" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                        </li>
                         <li class="text"></li>
                        <li class="field">
                            <asp:CheckBox ID="chksb" runat="server" Text="With Serial/Batch" TextAlign="Right" />
                        </li>
                    </ul>
                    <ul>
                        <li class="text"></li>
                        <%--#CC02 Add Start--%>
                        <li>
                            <div class="float-margin">
                                <asp:Button ID="btnExportToExcel" runat="server" OnClick="exportToExel_Click" CssClass="excel"
                                    CausesValidation="False" />
                            </div>
                            <%--#CC02 Add End--%>

                            <%--#CC02 Commnet Start--%>
                            <%--<asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerch_Click"
                                                                    CssClass="buttonbg" CausesValidation="False" />--%>
                            <%--#CC02 Commnet End--%>
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
                <%--#CC02 Commnet Start--%>
                <%--<asp:Button ID="btnExportToExcel" runat="server" OnClick="exportToExel_Click" CssClass="excel"
                                                                        CausesValidation="False" />--%>
                <%--#CC02 Commnet End--%>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="grdGRNReport" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                            BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="GRNNumber" FooterStyle-HorizontalAlign="Left"
                            FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="Altrow"
                            RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                            RowStyle-VerticalAlign="top" Width="100%" OnPageIndexChanging="grdGRNReport_PageIndexChanging">
                            <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                            <Columns>
                                <asp:BoundField DataField="SalesChannelName" HeaderText="SalesChannel Name" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SalesChannelCode" HeaderText="SalesChannel Code" HtmlEncode="false">
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
                                <asp:BoundField DataField="ZSMName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName3 %>"
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
                                <asp:BoundField DataField="GRNNumber" HeaderText="GRN Number" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="GRNDate">
                                    <ItemTemplate>
                                        <asp:Label ID="lbluseraffdate" runat="server" Text='<%# Eval("GRNDate","{0:dd-MMM-yy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PONumber" HeaderText="PO Number" HtmlEncode="false" Visible="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="PO Date" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbluseraffdate" runat="server" Text='<%# Eval("PODate","{0:dd-MMM-yy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="InvoiceNumber" HeaderText="Invoice Number" HtmlEncode="false"
                                    Visible="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Transaction Date" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbluseraffdate" runat="server" Text='<%# Eval("InvoiceDate","{0:dd-MMM-yy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SKUName" HeaderText="SKU" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="SerialNumber">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSerialNumber" runat="server" Text='<%# Eval("SerialNumber1") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BatchCode">
                                    <ItemTemplate>
                                        <asp:Label ID="lblbatchNumber" runat="server" Text='<%# Eval("BatchCode") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--  <asp:BoundField DataField="SerialNumber1" HeaderText="SerialNumber" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="BatchCode" HeaderText="BatchCode" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>--%>
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
