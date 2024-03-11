<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    EnableEventValidation="false" CodeFile="TertioryReportFlatSMS.aspx.cs" Inherits="Reports_SalesChannel_TertioryReportFlatSMS" %>

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
                                                    <div class="mainheading_rpt">
                                                        <div class="mainheading_rpt_left">
                                                        </div>
                                                        <div class="mainheading_rpt_mid">
                                                            Tertiory Report</div>
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
                                                            <td colspan="4" height="20" class="error" valign="top">
                                                                (*) marked fields are mandatory.
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="right" width="14%" height="30">
                                                                SMS Date Type:<font class="error">*</font>
                                                            </td>
                                                            <td align="left" valign="top" width="20%">
                                                                <div style="width: 160px;">
                                                                    <asp:DropDownList ID="ddlSMSDateType" AutoPostBack="false" runat="server" CssClass="form_select4"
                                                                        OnSelectedIndexChanged="ddlSMSDateType_SelectedIndexChanged1">
                                                                        <asp:ListItem Text="Salect" Value="0"></asp:ListItem>
                                                                        <asp:ListItem Text="SMS Tertiary" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="System Tertiary" Value="2"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </td>
                                                            <td valign="top" align="right" width="20%" height="30" runat="server" id="tdNameid">
                                                                Model Name:
                                                            </td>
                                                            <td align="left" valign="top" runat="server" id="tdName">
                                                                <div style="width: 160px;">
                                                                    <asp:DropDownList ID="ddlModalName" AutoPostBack="false" runat="server" CssClass="form_select4">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" valign="top" height="30">
                                                                <asp:Label ID="lblserfrmDate" runat="server" Text="">From Date: </asp:Label>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <uc2:ucDatePicker ID="ucDateFrom" runat="server" IsEnabled="True" defaultDateRange="True" />
                                                            </td>
                                                            <td align="right" valign="top">
                                                                <asp:Label ID="lblsertodate" runat="server" Text="">To Date:</asp:Label>
                                                            </td>
                                                            <td valign="top" align="left">
                                                                <uc2:ucDatePicker ID="ucDateTo" runat="server" IsEnabled="True" defaultDateRange="True" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" valign="top" height="30">
                                                                IMEI:
                                                            </td>
                                                            <td align="left" valign="top" colspan="2">
                                                                <div>
                                                                    <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="form_textarea" TextMode="MultiLine"></asp:TextBox>
                                                                </div>
                                                                <div style="margin-top: 2px;" class="error">
                                                                    (Comma separated)</div>
                                                            </td>
                                                            <td valign="top" align="left" colspan="2">
                                                                <div class="float-margin">
                                                                    <asp:Button ID="btnSearch" Text="Export To Excel" runat="server" OnClick="btnSerch_Click"
                                                                        CssClass="buttonbg" CausesValidation="False" />
                                                                </div>
                                                                <div class="float-margin">
                                                                    <asp:Button ID="btnCount" Text="Get Count" runat="server" OnClick="btnCount_Click"
                                                                        CssClass="buttonbg" CausesValidation="False" />
                                                                </div>
                                                                <div class="float-margin">
                                                                    <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                                                        CssClass="buttonbg" CausesValidation="False" />
                                                                </div>
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
                        <td align="left" height="10">
                        </td>
                    </tr>
                    <tr>
                        <td>
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
