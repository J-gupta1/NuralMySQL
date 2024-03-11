<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="ViewBeatPlanDetail.aspx.cs" Inherits="Reports_Common_ViewBeatPlanDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel ID="UpdMain" runat="server">
            <ContentTemplate>
                <div>
                    <div>
                        <uc4:ucMessage ID="ucMessage1" runat="server" />
                    </div>
                    <div class="clear"></div>
                    <div class="mainheading">
                        View Beat Plan 
                    </div>
                    <div class="contentbox">
                        <div class="mandatory">
                            (*) Marked fields are mandatory. (+) Marked fields are optional.
                        </div>
                        <div class="H30-C3">
                            <ul>
                                <li class="text">Created By:<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <asp:DropDownList CssClass="formselect" ID="ddlfosandtsm" runat="server">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </li>
                                <li class="text">Sales Channel Type:<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <asp:DropDownList ID="ddlsaleschanneltype" CssClass="formselect" OnSelectedIndexChanged="ddlsaleschanneltype_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                </li>
                                <li class="text">Sales Channel Name:<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <asp:DropDownList CssClass="formselect" ID="ddlsaleschannelname" runat="server">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </li>
                                <li class="text">Beat Plan From Date:<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>
                                <li class="text">Beat Plan To Date:<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>
                                <li class="text">Status:<span class="error">+</span></li>
                                <li class="field">
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="formselect"></asp:DropDownList></li>
                                <li class="text"></li>
                                <li class="field">
                                    <div class="float-margin">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg"
                                            CausesValidation="true" ValidationGroup="Search" OnClick="btnSearch_Click" />
                                    </div>
                                    <div class="float-left">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg"
                                            CausesValidation="false" OnClick="btnCancel_Click" />
                                    </div>
                                </li>
                                <li class="field3">
                                    <div class="float-margin">
                                        <asp:Button ID="btnExportexcel" runat="server" Text="Export To Excel" CssClass="buttonbg"
                                            CausesValidation="true" OnClick="btnExportexcel_Click" />
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Panel ID="PnlGrid" runat="server">
                        <div class="mainheading">
                            View Beat Plan Details
                        </div>
                        <div class="contentbox">
                            <div class="grid1">
                                <asp:GridView ID="gvBeatPlanDetail" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="Altrow"
                                    bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                    DataKeyNames="SalesChannelID" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                    AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>'>
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <PagerStyle CssClass="gridfooter" />
                                    <Columns>

                                        <asp:BoundField DataField="BeatPlanDate" HeaderStyle-HorizontalAlign="Left" HeaderText="Beat Plan Date"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TSM_ASM_FOS" HeaderStyle-HorizontalAlign="Left" HeaderText="Created By Type"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TSM_FOSName" HeaderStyle-HorizontalAlign="Left" HeaderText="Created By"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="SalesChannelTypeName" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Type Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SalesChannelName" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Name/Retailer Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="StateName" HeaderStyle-HorizontalAlign="Left" HeaderText="State Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CityName" HeaderStyle-HorizontalAlign="Left" HeaderText="City Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>


                                        <asp:BoundField DataField="Status" HeaderStyle-HorizontalAlign="Left" HeaderText="Status"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>

                                    </Columns>
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
                <asp:PostBackTrigger ControlID="btnExportexcel" />
                <asp:PostBackTrigger ControlID="btnCancel" />
                <asp:PostBackTrigger ControlID="btnSearch" />

            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
