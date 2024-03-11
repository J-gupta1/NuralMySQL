<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="TertioryReportFlatSMSWMCCMNCinfo.aspx.cs" Inherits="Reports_SalesChannel_TertioryReportFlatSMSWMCCMNCinfo" %>

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
                                    <td align="left" valign="top" height="5"></td>
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
                                                    <div class="mainheading_rpt">
                                                        <div class="mainheading_rpt_left">
                                                        </div>
                                                        <div class="mainheading_rpt_mid">
                                                            Tertiory Report
                                                        </div>
                                                        <div class="mainheading_rpt_right">
                                                        </div>
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
                                                            <td colspan="6" height="20" class="error" valign="top">(*) marked fields are mandatory.
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" valign="top" width="10%" height="30">
                                                                <asp:Label ID="lblserfrmDate" runat="server" Text="">From Date: </asp:Label>
                                                            </td>
                                                            <td align="left" valign="top" width="20%">
                                                                <uc2:ucDatePicker ID="ucDateFrom" runat="server" IsEnabled="True" defaultDateRange="True" />
                                                            </td>
                                                            <td align="right" valign="top" width="8%">
                                                                <asp:Label ID="lblsertodate" runat="server" Text="">To Date:</asp:Label>
                                                            </td>
                                                            <td valign="top" align="left">
                                                                <uc2:ucDatePicker ID="ucDateTo" runat="server" IsEnabled="True" defaultDateRange="True" />
                                                            </td>
                                                            <td align="right" valign="top" width="8%">Date Type:
                                                            </td>
                                                            <td valign="top" align="left">
                                                                <asp:DropDownList ID="ddlDateType" runat="server" CssClass="form_select4">
                                                                    <asp:ListItem Selected="True" Text="~Select~" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="SMS Activation Date" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Web Activation Date" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="ISP Tertiary Date" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="Tertiary Considered Date" Value="4"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>

                                                        <%--#CC04 START ADDED--%>
                                                        <tr>
                                                            <td align="right" valign="top" width="10%" height="30">
                                                                <asp:Label ID="Label1" runat="server" Text="">Model Name: </asp:Label>
                                                            </td>
                                                            <td align="left" valign="top" width="20%">

                                                                <asp:DropDownList ID="ddlModelName" CssClass="form_select4" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlModelName_SelectedIndexChanged">
                                                                </asp:DropDownList>

                                                            </td>
                                                            <td align="right" valign="top" width="8%">
                                                                <asp:Label ID="Label2" runat="server" Text="">SKU Name:</asp:Label>
                                                            </td>
                                                            <td valign="top" align="left">
                                                                <asp:DropDownList ID="ddlSku" runat="server" CssClass="form_select4" AutoPostBack="false">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="right" valign="top" width="8%"></td>
                                                            <td valign="top" align="left"></td>
                                                        </tr>

                                                        <%--#CC04 END ADDED--%>
                                                        <tr>
                                                            <td align="right" valign="top" height="30">IMEI:
                                                            </td>
                                                            <td align="left" valign="top" colspan="4">
                                                                <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="form_textarea" TextMode="MultiLine"></asp:TextBox>
                                                                <div style="margin-top: 2px;" class="error">
                                                                    (Comma separated)
                                                                </div>
                                                            </td>
                                                            <td valign="top" align="left">
                                                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerch_Click"
                                                                    CssClass="buttonbg" CausesValidation="False" />
                                                                <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                                                    CssClass="buttonbg" CausesValidation="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnSearch" />
                                                </Triggers>
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
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
