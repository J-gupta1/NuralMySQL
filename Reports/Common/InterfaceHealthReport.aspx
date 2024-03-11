<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InterfaceHealthReport.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="InterfaceHealthReport" %>

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
                    <uc4:ucMessage ID="ucMessage1" runat="server" />
                    <div class="clear"></div>
                    <div class="mainheading">
                        Tall Patch Health 
                    </div>
                    <div class="contentbox">
                        <div class="mandatory">
                            (*) Marked fields are mandatory. (+) Fill at least one of them.
                        </div>
                        <div class="H25-C3-S">
                            <ul>
                               
                               

                                <li class="text">From Post Date:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>
                                <li class="text">To Date:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>
                                 <li class="text">Last Days:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <asp:RadioButtonList ID="rdbLastDays" runat="server">
                                        <asp:ListItem Text="Last 7 Days" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="Last 15 Days" Value="15"></asp:ListItem>
                                        <asp:ListItem Text="Last 30 Days" Value="30"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </li>
                                <li class="field3">
                                    <div class="float-margin">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg"
                                            CausesValidation="true" ValidationGroup="Search" OnClick="btnSearch_Click" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg"
                                            CausesValidation="false" OnClick="btnCancel_Click" />
                                    </div>
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
                            View Attendance Details                                                    
                        </div>
                        <div class="contentbox">
                            <div class="grid">
                                <asp:GridView ID="gvPatchHealth" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="Altrow"
                                    bgcolor="" BorderWidth="0px"  CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"  
                                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                    AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>' >
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <PagerStyle CssClass="gridfooter" />
                                    <Columns>
                                         <asp:BoundField DataField="SrNo" HeaderStyle-HorizontalAlign="Left" HeaderText="SrNo"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SalesChannelCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SalesChannelCode"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                       
                                        <asp:BoundField DataField="SalesChannelName" HeaderStyle-HorizontalAlign="Left" HeaderText="SalesChannelName"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LastLogin" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:dd/MM/yyyy}" HeaderText="LastLogin"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="LastTransactionOn" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:dd/MM/yyyy}" HeaderText="LastTransactionOn"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NumberOfInvoice" HeaderStyle-HorizontalAlign="Left" HeaderText="NumberOfInvoice"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Quantity" HeaderStyle-HorizontalAlign="Left" HeaderText="Quantity"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PatchInstallationDate" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:dd/MM/yyyy}" HeaderText="PatchInstallationDate"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="StateName" HeaderStyle-HorizontalAlign="Left" HeaderText="State"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CityName" HeaderStyle-HorizontalAlign="Left" HeaderText="City"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PatchVersion" HeaderStyle-HorizontalAlign="Left" HeaderText="Patch Version"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <%--<asp:BoundField DataField="ReleasedVersion" HeaderStyle-HorizontalAlign="Left" HeaderText="Released Patch Version"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>--%>
                                        
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
                 <asp:PostBackTrigger ControlID="gvPatchHealth" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

