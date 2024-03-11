<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SMSLogRpt.aspx.cs" Inherits="Reports_SAP_SMSLogRpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMsg" runat="server" />
    <div class="mainheading">
        SMS Log Search
    </div>
    <div>
        <div class="contentbox">
             <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
            <div class="H25-C3-S">
                <ul>
                    <li class="text">Mobile No:
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtMobileNo" runat="server" CssClass="formfields" MaxLength="10"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FtbMNo" runat="server" FilterMode="ValidChars" FilterType="Numbers"
                            TargetControlID="txtMobileNo" />
                    </li>
                    <li class="text">IMEI No:
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtIMEINo" runat="server" CssClass="formfields" MaxLength="16"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FtbIMEI" runat="server" FilterMode="ValidChars"
                            FilterType="Numbers" TargetControlID="txtIMEINo" />
                    </li>
                </ul>
                <div class="clear"></div>
                <ul>
                    <li class="text">Date From:<span class="error">*</span>
                    </li>
                    <li class="field">
                        <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                            ValidationGroup="Report" defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />

                    </li>
                    <li class="text">Date To:<span class="error">*</span>
                    </li>
                    <li class="field">
                        <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="Report"
                            defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                    </li>
                    <li class="field">
                        <asp:Button ID="btnSearch" Text="Search" runat="server" ValidationGroup="Report"
                            ToolTip="Search" CssClass="buttonbg" CausesValidation="true" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                            OnClick="btnCancel_Click" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                <div class="mainheading">
                    Log List
                </div>
                <div class="export">
                    <asp:Button ID="btnExportToExcel" runat="server" OnClick="exportToExel_Click" CssClass="excel"
                        CausesValidation="False" />
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="grdSMSReport" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                            BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-HorizontalAlign="Left"
                            FooterStyle-VerticalAlign="Top" EmptyDataText="No Record Found" GridLines="none" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" PageSize='<%$ AppSettings:GridViewPageSize %>' RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="Altrow"
                            RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                            RowStyle-VerticalAlign="top" Width="100%" OnPageIndexChanging="grdSMSReport_PageIndexChanging">
                            <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                            <Columns>
                                <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="IMEI" HeaderText="IMEI No" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Circle" HeaderText="Circle" NullDisplayText="N/A" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Date" HeaderText="Date" HtmlEncode="false">
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
        <%-- <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExportToExcel" />
                                </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
