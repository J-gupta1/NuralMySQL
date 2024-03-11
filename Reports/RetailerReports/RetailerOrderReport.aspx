<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/ReportPage.master" AutoEventWireup="true" CodeFile="RetailerOrderReport.aspx.cs" Inherits="Reports_New_Reports_RetailerOrderReport" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl" TagPrefix="uc3" %>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel ID="UpdMain" runat="server">
            <ContentTemplate>
                <div>
                    <uc1:ucMessage ID="ucMsg" runat="server" />
                    <div class="clear"></div>
                    <div class="mainheading">
                        View Retailer Order Report 
                    </div>
                    <div class="contentbox">
                        <div class="mandatory">
                            (*) Marked fields are mandatory. (+) Fill at least one of them.
                        </div>
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">Entity Type:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <asp:DropDownList CssClass="formselect" ID="ddlEntityType" AutoPostBack="true" OnSelectedIndexChanged="ddlEntityType_SelectedIndexChanged" runat="server">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </li>
                                <li class="text">Entity Type Name:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <asp:DropDownList ID="ddlEntityTypeName" CssClass="formselect" runat="server">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </li>
                                <li class="text">From Date:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="To date required." ValidationGroup="vgStockRpt"
                                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                </li>
                                <li class="text">To Date:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="vgStockRpt"
                                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                </li>
                                 </ul>
                                <div class="setbbb">
                                    <div class="float-margin">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg" OnClick="btnSearch_Click" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="lnkExportToExcel" runat="server" Text="Export To Excel" CssClass="buttonbg"
                                            CausesValidation="true" OnClick="lnkExportToExcel_Click" />
                                    </div>
                                </div>
                           
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Panel ID="PnlGrid" Visible="false" runat="server">
                        <div class="mainheading">
                            View order Details                                                    
                        </div>
                        <div class="contentbox">
                            <div class="grid">
                                <asp:GridView ID="gvOrderDetail" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="Altrow"
                                    bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                    AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>'>
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <PagerStyle CssClass="gridfooter" />
                                    <Columns>
                                        <asp:BoundField DataField="OrderDate" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left" HeaderText="Order Date"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OrderFromName" HeaderStyle-HorizontalAlign="Left" HeaderText="Order From Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OrderFromCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Order From Code"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OrderNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="Order Number"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="OrderToName" HeaderStyle-HorizontalAlign="Left" HeaderText="Order To Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OrderToCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Order To Code"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="OrderRecommandedby" HeaderStyle-HorizontalAlign="Left" HeaderText="Order Created By"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="StateName" HeaderStyle-HorizontalAlign="Left" HeaderText="State"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SKUName" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SKUCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Code"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OrderQuantity" HeaderStyle-HorizontalAlign="Left" HeaderText="Order Quantity"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="dvFooter" runat="server" class="pagination" visible="false">
                                <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="lnkExportToExcel" />
                <asp:PostBackTrigger ControlID="btnSearch" />
                <asp:PostBackTrigger ControlID="gvOrderDetail" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

