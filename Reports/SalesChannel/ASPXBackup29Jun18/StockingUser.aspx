<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StockingUser.aspx.cs" Inherits="Reports_SalesChannel_StockHoldingUser" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
        <tr>
            <td align="left" valign="top">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td valign="top" align="left">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td align="left" valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" valign="top">
                                                    <uc1:ucMessage ID="ucMsg" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" style="float: left">
                                            <tr>
                                                <td align="left" valign="top" class="tableposition">
                                                    <div class="mainheading">
                                                        Stock Holding User Search</div>
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
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" class="tableposition">
                                                    <div class="contentbox">
                                                        <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                            <tr>
                                                                <td colspan="7" class="mandatory" valign="top">
                                                                    (<font class="error">*</font>) marked fields are mandatory.
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="right" class="formtext" width="14%">
                                                                    Sales Channel Type:<font class="error">*</font>
                                                                </td>
                                                                <td align="left" valign="top" width="13%">
                                                                    <div style="float: left; width: 135px;">
                                                                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form_select4">
                                                                        </asp:DropDownList>
                                                                        <br />
                                                                    </div>
                                                                    <div style="float: left; width: 180px;">
                                                                        <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlType"
                                                                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel type."
                                                                            SetFocusOnError="true" ValidationGroup="vgUser"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </td>
                                                                <td valign="top" align="right" class="formtext" width="10%">
                                                                    Date From:<font class="error">*</font>
                                                                </td>
                                                                <td align="left" valign="top" width="13%">
                                                                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                                                                        ValidationGroup="vgUser" defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                    <br />
                                                                </td>
                                                                <td valign="top" align="right" class="formtext" width="10%">
                                                                    Date To:<font class="error">*</font>
                                                                </td>
                                                                <td valign="top" align="left" width="15%">
                                                                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="vgUser"
                                                                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                </td>
                                                                <td valign="top" align="left" width="18%">
                                                                    <asp:HiddenField ID="hfSearch" Value="0" Visible="false" runat="server" />
                                                                    <asp:Button ID="btnShow" Text="Search" runat="server" ValidationGroup="vgUser"
                                                                        ToolTip="Search" CssClass="buttonbg" CausesValidation="true" OnClick="btnShow_Click" />
                                                                          
                                                                    <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                                                        CssClass="buttonbg" CausesValidation="False" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td valign="top" align="left" class="tableposition">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td width="90%" align="left" class="tableposition">
                                                                <div class="mainheading">
                                                                    &nbsp;List</div>
                                                            </td>
                                                            <td width="10%" align="right">
                                                                <asp:Button ID="btnExportToExcel" Text="" runat="server" OnClick="exportToExel_Click"
                                                                    CssClass="excel" CausesValidation="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" class="tableposition">
                                                    <div class="contentbox">
                                                        <div class="grid2">
                                                            <asp:GridView ID="grdUser" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                                                BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="SalesChannelID"
                                                                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                                                HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                                                                AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                                                                OnPageIndexChanging="grdUser_PageIndexChanging" HeaderStyle-CssClass="gridheader"
                                                                RowStyle-VerticalAlign="top" Width="100%">
                                                                <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                                                                <Columns>
                                                                    <asp:BoundField DataField="HO" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName1 %>"
                                                                        HtmlEncode="false" NullDisplayText="N/A">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="RBH" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName2 %>"
                                                                        HtmlEncode="false" NullDisplayText="N/A">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ZBH" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName3 %>"
                                                                        HtmlEncode="false" NullDisplayText="N/A">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SBH" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName4 %>"
                                                                        HtmlEncode="false" NullDisplayText="N/A">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ASO" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName5 %>"
                                                                        HtmlEncode="false" NullDisplayText="N/A">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SalesChannelTypeName" HeaderText="SalesChannelType" HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SalesChannelName" HeaderText="SalesChannel" HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="OpeningStockDate" DataFormatString="{0:dd-MMM-yy}" HeaderText="Opening Stock Date"
                                                                        HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <%--  <asp:TemplateField HeaderText="User Activity Date">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbluseraffdate" runat="server" Text='<%# Eval("UserActivityDate","{0:dd-MMM-yy}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                                </Columns>
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                                                                <PagerStyle CssClass="PagerStyle" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                    </asp:Panel>
                                    <tr>
                                        <td height="10">
                                        </td>
                                    </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExportToExcel" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
</asp:Content>
