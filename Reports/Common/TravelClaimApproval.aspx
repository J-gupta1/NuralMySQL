<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="TravelClaimApproval.aspx.cs" Inherits="Reports_Common_TravelClaimApproval" %>

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
                Travel Claim Approval Report
            </div>
            <div class="contentbox">
                <div class="mandatory">
                     (*) Marked fields are mandatory. (+) Fill at least one of them.    
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Travel Claim Approved By:<font class="error">+</font>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlEntityType" runat="server" OnSelectedIndexChanged="ddlEntityType_SelectedIndexChanged" AutoPostBack="true" CssClass="formselect">
                            </asp:DropDownList>

                        </li>
                        <li class="text">Mapped Entity Name:<font class="error">+</font>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlEntityName" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                        <li class="text">Claim Submission No:<font class="error">+</font>
                        </li>
                        <li class="field">
                           <asp:TextBox ID="txtClaimSubmissionno" CssClass="formfields" runat="server"></asp:TextBox>
                        </li>
                        <li class="text">Date From:<font class="error">+</font>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="To date required." ValidationGroup="vgStockRpt"
                                defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                        </li>
                        <li class="text">Date To:<font class="error">+</font>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="vgStockRpt"
                                defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                        </li>
                        <li class="text">Approval Status:</li>
                        <li class="field">
                             <asp:DropDownList ID="ddlApprovalStatus" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" runat="server" ValidationGroup="Serach" CausesValidation="true"
                                    CssClass="buttonbg" Text="Search" OnClick="btnSearch_Click" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" OnClick="btnCancel_Click" Text="Cancel" ToolTip="Cancel" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="ExportToExcel" CssClass="buttonbg" runat="server" Text="Export To Excel" OnClick="ExportToExcel_Click" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <div id="dvhide" runat="server" visible="false">
                <asp:Panel ID="PnlGrid" runat="server">
                    <div class="mainheading">
                        List
                    </div>
                    <div class="contentbox">
                        <div class="grid">
                            <asp:GridView ID="GridTravelApproval" runat="server" AlternatingRowStyle-CssClass="Altrow"
                                AutoGenerateColumns="false"  bgcolor="" AllowPaging="false" PageSize='<%$ AppSettings:GridViewPageSize %>'
                                BorderWidth="0px" CellPadding="4" CellSpacing="1" Width="100%" DataKeyNames="TravelClaimSubmissionId"
                                FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                                GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected">
                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                <Columns>
                                    <asp:BoundField DataField="TravelClaimApprovedBy" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="TravelClaim Approved By">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TravelClaimSubmitBy" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="TravelClaim Submitted By">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TravelClaimSubmissionNo" HeaderStyle-HorizontalAlign="Left"
                                        HeaderText="TravelClaim Submission No" HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TotalClaimAmount" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="TotalClaim Amount">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ApprovedAmount" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="Approved Amount">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TravelClaimNo" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="TravelClaim No.">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TravelClaimDate" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="TravelClaim Date">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FinalApprovedAmount" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="Final Approved Amount">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ApprovalStatus" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="Approval Status">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
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
            <asp:PostBackTrigger ControlID="btnSearch" />    
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
