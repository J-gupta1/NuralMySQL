<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageViewNotification.aspx.cs" Inherits="Masters_Common_ManageViewNotification" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:Label ID="lbltvErrorMsg" runat="server" Visible="false"></asp:Label>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMsg" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updAddUserMain" runat="server">
            <ContentTemplate>
                <div class="mainheading">
                    View Notification
                </div>
                <div class="contentbox">
                    <div class="mandatory">
                        (*) Marked fields are mandatory
                    </div>
                    <div class="H25-C4-S">
                        <ul>
                            <li class="text">Date From: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                                    ValidationGroup="ViewNotification" defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />

                            </li>
                            <li class="text">Date To: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="ViewNotification"
                                    defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                            </li>
                            <li class="field3">
                                <div class="float-margin">
                                    <asp:Button ID="btnSearch" Text="Search" runat="server" ValidationGroup="ViewNotification"
                                        ToolTip="Search" CssClass="buttonbg" CausesValidation="true" OnClick="btnSearch_Click" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                        OnClick="btnCancel_Click" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>

                <asp:UpdatePanel ID="updgrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="mainheading">
                            Notification
                        </div>
                        <div class="contentbox">
                            <div class="grid1">
                                <asp:GridView ID="grdNotification" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                                    AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                                    DataKeyNames="NotificationId" EmptyDataText="No record found" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                                    RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                                    Width="100%" OnRowCommand="grdNotification_RowCommand" OnPageIndexChanging="grdNotification_PageIndexChanging">
                                    <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="600" HeaderText="Notification">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNotification" runat="server" Text='<%#Eval("NotificationText") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Display To">
                                            <ItemTemplate>
                                                <div class="float-left">
                                                    <asp:Label ID="lblDisplayTo" runat="server" Text='<%#Eval("DisplayTo") %>'></asp:Label>
                                                </div>
                                                <div class="float-right">
                                                    <asp:LinkButton ID="lnkView" CssClass="elink2" runat="server" Visible='<%#Convert.ToBoolean(Eval("DisplayView")) %>'
                                                        Text="View" CommandArgument='<%#Eval("NotificationId") %>' CommandName="Notification"></asp:LinkButton>
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="RecordCreationDate" HeaderStyle-HorizontalAlign="Left"
                                            HeaderText="Notification Date" HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderStyle-HorizontalAlign="Left" HeaderText="Status"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="mainheading">
                            Notification Details
                        </div>
                        <div class="contentbox">
                            <div class="grid1">
                                <asp:GridView Visible="false" ID="grdNotificationDetails" runat="server" AllowPaging="True"
                                    AlternatingRowStyle-CssClass="Altrow" AutoGenerateColumns="false" BorderWidth="0px"
                                    CellPadding="4" CellSpacing="1" EmptyDataText="No record found" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                                    RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                                    Width="100%" OnPageIndexChanging="grdNotificationDetails_PageIndexChanging">
                                    <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <Columns>
                                        <asp:BoundField DataField="SendType" HeaderStyle-HorizontalAlign="Left" HeaderText="Notification To"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SendTo" HeaderStyle-HorizontalAlign="Left" HeaderText="Details"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
