<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/ReportPage.master"
    AutoEventWireup="true" CodeFile="TargetReport.aspx.cs" Inherits="Reports_Common_TargetReport" %>

<%@ Register Src="~/UserControls/Footer.ascx" TagName="ucFooter" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Header.ascx" TagName="ucHeader" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMsg" runat="server" />
    <div class="mainheading">
        Target Report
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>

                <li class="text">Select TSI:
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlTSIList" CssClass="formselect" runat="server" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </li>

                <li class="text">Date From:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" />
                </li>

                <li class="text">Date To:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateTo" runat="server" />
                </li>

            </ul>
            <ul>
                <li class="text"></li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:HiddenField ID="hfSearch" runat="server" Value="0" Visible="false" />
                        <asp:Button ID="btnSearch" Text="Search" runat="server" ValidationGroup="rpt" ToolTip="Search"
                            CssClass="buttonbg" CausesValidation="true" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                            OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnExport" Text="Export Report" runat="server" ValidationGroup="rpt" ToolTip="Search"
                            CssClass="buttonbg" CausesValidation="true" OnClick="btnExport_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <asp:Panel ID="PnlGrid" Visible="false" runat="server">
        <div class="mainheading">
            View Target Report
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:GridView ID="GridDetails" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="Altrow"
                    bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                    DataKeyNames="" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                    AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>'>
                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                    <PagerStyle CssClass="gridfooter" />
                    <Columns>
                        <%--#CC01 Add Start--%>
                        <asp:BoundField DataField="Territory" HeaderStyle-HorizontalAlign="Left" HeaderText="Territory"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <%--#CC01 Add End--%>
                        <asp:BoundField DataField="City" HeaderStyle-HorizontalAlign="Left" HeaderText="City"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Branch" HeaderStyle-HorizontalAlign="Left" HeaderText="Branch"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RDCustomerID" HeaderStyle-HorizontalAlign="Left" HeaderText="RD Customer ID"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Zone" HeaderStyle-HorizontalAlign="Left" HeaderText="Zone"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Region" HeaderStyle-HorizontalAlign="Left" HeaderText="Region"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TSM_Name" HeaderStyle-HorizontalAlign="Left" HeaderText="TSM Name"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RD_Name" HeaderStyle-HorizontalAlign="Left" HeaderText="RD Name"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Department" HeaderStyle-HorizontalAlign="Left" HeaderText="Department"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OpeningStock" HeaderStyle-HorizontalAlign="Left" HeaderText="Opening Stock"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Sellthru_TGT" HeaderStyle-HorizontalAlign="Left" HeaderText="Sellthru TGT"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Sellthru_ACHMT_MTD" HeaderStyle-HorizontalAlign="Left" HeaderText="Sellthru ACHMT MTD"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Sellthru_ACHMT_MTD_per" HeaderStyle-HorizontalAlign="Left" HeaderText="Sellthru ACHMT MTD %"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Sellout_TGT" HeaderStyle-HorizontalAlign="Left" HeaderText="Sellout TGT"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Sellout_ACHMT_MTD" HeaderStyle-HorizontalAlign="Left" HeaderText="Sellout ACHMT MTD"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Sellout_ACHMT_MTD_per" HeaderStyle-HorizontalAlign="Left" HeaderText="Sellout ACHMT MTD %"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="WOD_TGT" HeaderStyle-HorizontalAlign="Left" HeaderText="WoD TGT"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="WOD_ACHMT" HeaderStyle-HorizontalAlign="Left" HeaderText="WoD ACHMT"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="WOD_ACHMT_per" HeaderStyle-HorizontalAlign="Left" HeaderText="WoD ACHMT %"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Stock_As_On_Date" HeaderStyle-HorizontalAlign="Left" HeaderText="Stock As On Date"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Weeks_of_Inventory" HeaderStyle-HorizontalAlign="Left" HeaderText="Weeks of Inventory"
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
</asp:Content>

