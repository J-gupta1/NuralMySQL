﻿<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="ViewCheckInCheckOutDetails.aspx.cs" Inherits="Masters_WebInterface_ViewCheckInCheckOutDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>

<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="UpdMain" runat="server">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
            <div class="clear"></div>
            <div class="mainheading">
                View Check-In Check-Out Detail
            </div>
            <div class="contentbox">
                <div class="mandatory">
                     (*) Marked fields are mandatory. (+) Fill at least one of them.    
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Check-In By:<font class="error">+</font>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlFosTsmName" runat="server" CssClass="formselect">
                            </asp:DropDownList>

                        </li>
                        <li class="text">Sales Channel Type:<font class="error">+</font>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbsaleschanneltype" runat="server" CssClass="formselect" OnSelectedIndexChanged="cmbsaleschanneltype_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </li>
                        <li class="text">Sales Channel Name:<font class="error">+</font>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlSaleschannelName" runat="server" CssClass="formselect">
                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </li>
                        <li class="text">Check-In Date:<font class="error">+</font>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="To date required." ValidationGroup="vgStockRpt"
                                defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                        </li>
                        <li class="text">Check-Out Date:<font class="error">+</font>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="vgStockRpt"
                                defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                        </li>
                         <li class="text">Brand:
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlBrand" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                        <li class="text">Product Category:
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlproductcategory" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                        </ul>
                        <div class="setbbb">
                            <div class="float-margin">
                                <asp:Button ID="Button1" runat="server" ValidationGroup="Serach" CausesValidation="true"
                                    CssClass="buttonbg" Text="Search" OnClick="btnSearch_Click" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" OnClick="btnCancel_Click" Text="Cancel" ToolTip="Cancel" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="ExportToExcel" CssClass="buttonbg" runat="server" Text="Export To Excel" OnClick="ExportToExcel_Click" />
                            </div>
                        </div>
                  
                </div>
            </div>
            <div id="dvhide" runat="server" visible="false">
                <asp:Panel ID="PnlGrid" runat="server">
                    <div class="mainheading">
                        List
                    </div>
                    <div class="contentbox">
                        <div class="grid">
                            <asp:GridView ID="GridSalesChannel" runat="server" AlternatingRowStyle-CssClass="Altrow"
                                AutoGenerateColumns="false" OnRowDataBound="GridSalesChannel_RowDataBound" bgcolor="" AllowPaging="false" PageSize='<%$ AppSettings:GridViewPageSize %>'
                                BorderWidth="0px" CellPadding="4" CellSpacing="1" Width="100%" DataKeyNames="AppvisitId,SalesChannelID,ImageUpload"
                                FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                                GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected">
                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                <Columns>
                                    <asp:BoundField DataField="TSM_ASM_FOS" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="Check-In By EntityType">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TSM_FOSName" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="Check-In By EntityName">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SalesChannelTypename" HeaderStyle-HorizontalAlign="Left"
                                        HeaderText="Sales Channel Type Name" HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SalesChannelName" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="Sales Channel Name">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="StateName" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="States Name">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CityName" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="City Name">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ContactNo" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="Contact No.">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CheckInDateTime" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="CheckInDateTime">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CheckOutDateTime" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="CheckOutDateTime">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Plan_Unplan_Status" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="Plan/Unplan Visit Status">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PurposeofVisit" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="Purpose Of Visit">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Check-In Image Capture">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" CssClass="elink" Text="Download" CommandArgument='<%# Eval("ImageUpload") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Check-Out Image Capture">
                                        <ItemTemplate>
                                            <asp:GridView ID="gvCheckoutAttachedImages" runat="server" AutoGenerateColumns="false" BorderWidth="0px"
                                                ShowHeader="false" CellPadding="0" GridLines="None" CssClass="table-panel" HeaderStyle-VerticalAlign="top"
                                                HeaderStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                                                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                                                DataKeyNames="Attachement">
                                                <FooterStyle HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                                <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblImagePath" runat="server" Text='<%# Eval("Attachement") %>'
                                                                Style="display: none"></asp:Label>
                                                            <asp:LinkButton ID="lnkDownloadCheckoutimage" Text="Download" CssClass="elink" CommandArgument='<%# Eval("Attachement") %>' runat="server" OnClick="lnkDownloadCheckoutimage_Click"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <AlternatingRowStyle HorizontalAlign="Left" VerticalAlign="Top"></AlternatingRowStyle>
                                            </asp:GridView>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                <AlternatingRowStyle CssClass="Altrow" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                        </div>
                        <div id="dvFooter" runat="server" class="pagination">
                            <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ExportToExcel" />
            <asp:PostBackTrigger ControlID="btnCancel" />
            <asp:PostBackTrigger ControlID="Button1" />
            <asp:PostBackTrigger ControlID="GridSalesChannel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

