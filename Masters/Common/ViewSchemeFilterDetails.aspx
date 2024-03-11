<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewSchemeFilterDetails.aspx.cs"
    Inherits="Masters_Common_ViewSchemeFilterDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" id="StyleCss" runat="server" type="text/css" />
</head>
<body>
    <div align="center">
        <form id="form1" name="form1" runat="server">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left" valign="top">
                    <asp:Label ID="lblHeader" runat="server" CssClass="error" Text="Scheme Filter Details"></asp:Label>
                    <div class="contentbox">
                        <div class="grid1">
                            <asp:GridView ID="GridScheme" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="gridrow1"
                                bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                DataKeyNames="SchemePerformanceCalculationFilterActualValueID" HeaderStyle-CssClass="gridheader"
                                HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow"
                                RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected"
                                Width="100%" AutoGenerateColumns="false">
                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                <PagerStyle CssClass="gridfooter" />
                                <Columns>
                                    <asp:BoundField DataField="FilterActualValue" HeaderStyle-HorizontalAlign="Left"
                                        HeaderText="Actual Value" HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Result" HeaderStyle-HorizontalAlign="Left" HeaderText="Result"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ComponentFilterName" HeaderStyle-HorizontalAlign="Left"
                                        HeaderText="ComponentFilterName" HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FilterValue" HeaderStyle-HorizontalAlign="Left" HeaderText="Filter Value"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top" height="10">
                    <div class="mainheading">
                        Scheme PayOut</div>
                    <div class="contentbox">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td height="35" align="left" valign="top" class="formtext" width="17%">
                                    PayOut Criteria:
                                </td>
                                <td width="30%" align="left" valign="top" class="text_bold">
                                    <asp:Label ID="lblCriterialType" runat="server" Text=""></asp:Label>
                                </td>
                                <td height="35" align="left" valign="top" class="formtext" width="17%">
                                    Payout Type:
                                </td>
                                <td align="left" valign="top" class="text_bold">
                                    <asp:Label ID="lblPayoutType" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="contentbox">
                        <%--      <asp:UpdatePanel ID="updPayout" runat="server" UpdateMode="always">
                            <ContentTemplate>--%>
                        <asp:Panel ID="pnlPayout" runat="server" Visible="false">
                            <div class="grid1" runat="server" id="Div1" visible="True">
                                <asp:GridView ID="grdPayout" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                    CellSpacing="1" DataKeyNames="SchemeComponentPayoutSlabID" EditRowStyle-CssClass="editrow"
                                    EmptyDataText="No Record Found" GridLines="None" HeaderStyle-CssClass="gridheader"
                                    RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="gridrow1" Width="100%"
                                    AllowPaging="True" OnPageIndexChanging="grdPayout_PageIndexChanging">
                                    <RowStyle CssClass="gridrow" />
                                    <Columns>
                                        <asp:BoundField DataField="AchievementFrom" HeaderStyle-HorizontalAlign="Left" HeaderText="AchievementFrom"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AchievementTo" HeaderStyle-HorizontalAlign="Left" HeaderText="AchievementTo"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PayoutRate" HeaderStyle-HorizontalAlign="Left" HeaderText="PayoutRate"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                    <HeaderStyle CssClass="gridheader" />
                                    <EditRowStyle CssClass="editrow" />
                                    <AlternatingRowStyle CssClass="gridrow1" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <%--      </ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </div>
                </td>
            </tr>
        </table>
        </form>
    </div>
</body>
</html>
