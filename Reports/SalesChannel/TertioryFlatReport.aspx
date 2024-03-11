<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="TertioryFlatReport.aspx.cs" Inherits="Reports_SalesChannel_TertioryFlatReport" %>

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
    <div class="mainheading" visible="false" runat="server">
        Teritory Report
    </div>
    <div class="contentbox" visible="false" runat="server">
        <asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblserfrmDate" runat="server" Text="" Visible="false">From Date: </asp:Label>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateFrom" runat="server" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                        </li>
                        <li class="text">
                            <asp:Label ID="lblsertodate" runat="server" Text="" Visible="false">To Date:</asp:Label>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateTo" runat="server" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                        </li>
                        <li class="field3">
                            <div class="float-margin">
                            <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerch_Click"
                                CssClass="buttonbg" CausesValidation="False" />
                            </div>
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
                <div class="export">
                    <asp:Button ID="btnExportToExcel" runat="server" OnClick="exportToExel_Click" CssClass="excel"
                        CausesValidation="False" Visible="false" />
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="grdTertioryReport" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                            BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="" FooterStyle-HorizontalAlign="Left"
                            FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="Altrow"
                            RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                            RowStyle-VerticalAlign="top" Width="100%" OnPageIndexChanging="grdTertioryReport_PageIndexChanging">
                            <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                            <Columns>
                                <asp:BoundField DataField="TertioryDate" HeaderText="Tertiory Date" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" HtmlEncode="false">
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
