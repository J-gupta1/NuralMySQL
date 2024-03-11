<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UserTrackingRptV2.aspx.cs" Inherits="Reports_Common_UserTrackingRptV2" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0">
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
                                                    <div class="mainheading">
                                                        Search User Tracking
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" class="tableposition">
                                        <div class="contentbox">
                                            <asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                        <tr>
                                                            <td colspan="8" class="mandatory" valign="top">(<font class="error">*</font>)marked fields are mandatory.
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="formtext" valign="top" align="right" width="7%">
                                                                <asp:Label ID="lblrole" runat="server" Text="">Role:</asp:Label>
                                                            </td>
                                                            <td width="11%" align="left" valign="top">
                                                                <div style="float: left; width: 110px;">
                                                                    <asp:DropDownList ID="cmbRoleType" CssClass="form_select4" runat="server" OnSelectedIndexChanged="ddrSerRole_SelectedIndexChanged"
                                                                        AutoPostBack="True">
                                                                    </asp:DropDownList><br />
                                                                </div>
                                                                <div style="float: left; width: 110px;">
                                                                    <asp:RequiredFieldValidator runat="server" ID="valpricename" ControlToValidate="cmbRoleType"
                                                                        CssClass="error" ErrorMessage="Please select a role Type  " InitialValue="0"
                                                                        ValidationGroup="insert" />
                                                                </div>
                                                            </td>
                                                            <td class="formtext" valign="top" align="right" width="7%">
                                                                <asp:Label ID="Label1" runat="server" Text="">Status :</asp:Label>
                                                            </td>
                                                            <td width="11%" align="left" valign="top">
                                                                <div style="float: left; width: 110px;">
                                                                    <asp:DropDownList ID="ddlStatus" CssClass="form_select4" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                                                        <asp:ListItem Text=" -- Select All -- " Value="255" Selected="True" />
                                                                        <asp:ListItem Text=" Active " Value="1" />
                                                                        <asp:ListItem Text=" In-Active " Value="0" />
                                                                    </asp:DropDownList>
                                                                </div>

                                                            </td>
                                                            <td class="formtext" valign="top" align="right" width="8%">
                                                                <asp:Label ID="lbseruser" runat="server" Text="">User Name:</asp:Label>
                                                            </td>
                                                            <td valign="top" align="left" width="11%">
                                                                <div style="float: left; width: 110px;">
                                                                    <asp:DropDownList ID="cmbUsername" runat="server" CssClass="form_select4">
                                                                    </asp:DropDownList><br />
                                                                </div>
                                                                <div style="float: left; width: 110px;">
                                                            </td>

                                                            <td align="right" valign="top" class="formtext" width="8%">
                                                                <asp:Label ID="lblserfrmDate" runat="server" Text="">From Date: <font class="error">*</font></asp:Label>
                                                            </td>
                                                            <td align="left" valign="top" width="11%">
                                                                <uc2:ucDatePicker ID="ucDatePicker1" runat="server" ErrorMessage="Invalid from date."
                                                                    ValidationGroup="insert" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                                                            </td>
                                                            <td align="right" valign="top" width="10%" class="formtext">
                                                                <asp:Label ID="lblsertodate" runat="server" Text="">To Date:<font class="error">*</font></asp:Label>
                                                            </td>
                                                            <td valign="top" align="left">
                                                                <uc2:ucDatePicker ID="ucDatePicker2" runat="server" ErrorMessage="Invalid to  date."
                                                                    ValidationGroup="insert" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="formtext" valign="top" align="right" colspan="6"></td>
                                                            <td class="formtext" valign="top" align="right" colspan="2">
                                                                <asp:HiddenField ID="hfSearch" Value="0" Visible="false" runat="server" />
                                                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerch_Click"
                                                                    ValidationGroup="insert" CssClass="buttonbg" CausesValidation="True" />
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
                        <td align="left" height="10"></td>
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
                                                                &nbsp;List
                                                            </div>
                                                        </td>
                                                        <td width="10%" align="right">
                                                            <asp:Button ID="btnExportToExcel" Text="" runat="server" OnClick="exportToExel_Click"
                                                                CssClass="excel" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="tableposition">
                                                <div class="contentbox">
                                                    <div class="grid2">
                                                        <asp:GridView ID="grdUserTrackRpt" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                                            BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="RoleID" FooterStyle-HorizontalAlign="Left"
                                                            FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                                                            HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="gridrow1"
                                                            RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                                                            RowStyle-VerticalAlign="top" Width="100%" OnPageIndexChanging="grdArea_PageIndexChanging">
                                                            <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                                                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                                                            <Columns>
                                                                <asp:BoundField DataField="RoleName" HeaderText="Role" HtmlEncode="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="UserName" HeaderText="User Name" HtmlEncode="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="MenuName" HeaderText="Menu Name" HtmlEncode="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="User Activity Date">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbluseraffdate" runat="server" Text='<%# Eval("UserActivityDate","{0:dd-MMM-yy}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Last Login On">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllastlogindate" runat="server" Text='<%# Eval("LastLoginOn","{0:dd-MMM-yy}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="UserIP" HeaderText="User IP" HtmlEncode="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ServerIP" HeaderText="Server IP" HtmlEncode="false">
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
                        <td align="left" height="10"></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
