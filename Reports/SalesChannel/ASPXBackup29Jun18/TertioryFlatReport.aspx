<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="TertioryFlatReport.aspx.cs" Inherits="Reports_SalesChannel_TertioryFlatReport" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
        <tr>
            <td valign="top" align="left">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td align="left" valign="top">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
                                            <ContentTemplate>
                                                <uc1:ucMessage ID="ucMessage1" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" height="5">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td align="left" valign="top">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td align="left" valign="top" class="tableposition">
                                                    <div class="mainheading" visible="false" runat="server">
                                                        Teritory Report</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" class="tableposition">
                                        <div class="contentbox" visible="false" runat="server">
                                            <asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                        <tr>
                                                            <td align="right" valign="top" width="10%" class="formtext">
                                                                <asp:Label ID="lblserfrmDate" runat="server" Text="" Visible="false">From Date: </asp:Label>
                                                            </td>
                                                            <td align="left" valign="top" width="15%">
                                                                <uc2:ucDatePicker ID="ucDateFrom" runat="server" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                                                            </td>
                                                            <td align="right" valign="top" width="10%" class="formtext">
                                                                <asp:Label ID="lblsertodate" runat="server" Text="" Visible="false">To Date:</asp:Label>
                                                            </td>
                                                            <td valign="top" align="left" width="15%">
                                                                <uc2:ucDatePicker ID="ucDateTo" runat="server" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                                                            </td>
                                                            <td class="formtext" valign="top" align="left" width="20%">
                                                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerch_Click"
                                                                    CssClass="buttonbg" CausesValidation="False" />
                                                                <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                                                    CssClass="buttonbg" CausesValidation="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <%-- <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                </Triggers>--%>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" height="10">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                                        <tr>
                                            <td>
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td width="90%" align="left" class="tableposition">
                                                            <div class="mainheading">
                                                                &nbsp;List</div>
                                                        </td>
                                                        <td width="10%" align="right">
                                                            <asp:Button ID="btnExportToExcel" runat="server" OnClick="exportToExel_Click" CssClass="excel"
                                                                CausesValidation="False" Visible="false" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="tableposition">
                                                <div class="contentbox">
                                                    <div class="grid2">
                                                        <asp:GridView ID="grdTertioryReport" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                                            BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="" FooterStyle-HorizontalAlign="Left"
                                                            FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                                                            HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="gridrow1"
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
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExportToExcel" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" height="10">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
