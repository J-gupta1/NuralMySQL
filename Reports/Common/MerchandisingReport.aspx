<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="MerchandisingReport.aspx.cs" Inherits="Reports_Common_MerchandisingReport" %>
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
                        View Merchandising 
                    </div>
                    <div class="contentbox">
                        <div class="mandatory">
                            (+) Marked fields are optional.
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
                                <li class="text">From Date:<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>
                                <li class="text">To Date:<span class="error">+</span>
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
                    <asp:Panel ID="PnlGrid" Visible="false" runat="server">
                        <div class="mainheading">
                            View Merchandising Details
                        </div>
                        <div class="contentbox">
                            <div class="grid1">
                                <asp:GridView ID="gvMerchandisingDetail" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="Altrow"
                                    bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                    DataKeyNames="MerchandiseID,ImagePath" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                    AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>' OnRowDataBound="gvMerchandisingDetail_RowDataBound">
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <PagerStyle CssClass="gridfooter" />
                                    <Columns>

                                        <asp:BoundField DataField="CreatedDate" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left" HeaderText="Created Date"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CreatedByType" HeaderStyle-HorizontalAlign="Left" HeaderText="Created By Type"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CreatedBy" HeaderStyle-HorizontalAlign="Left" HeaderText="Created By"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="CreatedForType" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Type"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CreatedFor" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Name/Retailer Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ParentCategoryName" HeaderStyle-HorizontalAlign="Left" HeaderText="Parent CategoryName"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CategoryName" HeaderStyle-HorizontalAlign="Left" HeaderText="CategoryName"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MerchandisingStatus" HeaderStyle-HorizontalAlign="Left" HeaderText="Status"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                         <asp:TemplateField HeaderText="Image Capture">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" CssClass="elink" Text="Download" CommandArgument='<%# Eval("ImagePath") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="dvFooter" runat="server" visible="false" class="pagination">
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
                 <asp:PostBackTrigger ControlID="gvMerchandisingDetail" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>