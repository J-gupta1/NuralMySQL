<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="ViewDoaReport.aspx.cs" Inherits="DOA_ViewDoaDispatches" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Import Namespace="BussinessLogic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div align="center">
        <asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td align="left" valign="top">
                                <div id="divupmessage" runat="server">
                                    <uc4:ucMessage ID="ucMessage1" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top"></td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <div class="mainheading_rpt">
                                    <div class="mainheading_rpt_left">
                                    </div>
                                    <div class="mainheading_rpt_mid">
                                        View DOA Report
                                    </div>
                                    <div class="mainheading_rpt_right">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td valign="top" align="left">
                                            <div class="contentbox">
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td class="mandatory" colspan="5" height="20" valign="top" align="left">(<font class="error">*</font>) marked fields are mandatory.
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="15%" align="right" valign="top" class="formtext" height="35">DOA Certificate No:<font class="error">+</font>
                                                        </td>
                                                        <td width="20%" align="left" valign="top">
                                                            <asp:TextBox ID="txtDoaCertificateno" CssClass="form_input2" runat="server" MaxLength="17"></asp:TextBox>

                                                        </td>
                                                        <td width="10%" align="right" valign="top" class="formtext">IMEI:<font class="error">+</font>
                                                        </td>
                                                        <td width="20%" align="left" valign="top">
                                                            <asp:TextBox ID="txtIMEINo" runat="server" CssClass="form_input2" MaxLength="15"></asp:TextBox>

                                                        </td>
                                                        <td width="10%" align="right" valign="top" class="formtext">Status:<font class="error">+</font>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:DropDownList ID="ddldoastatus" Width="120px" runat="server" CssClass="form_select">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="right" class="formtext">From Date:<font class="error">+</font>
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <uc1:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date." ValidationGroup="SearchNo"
                                                                runat="server" />
                                                        </td>
                                                        <td align="right" valign="top" class="formtext">To Date:<font class="error">+</font>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <uc1:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date." ValidationGroup="SearchNo"
                                                                runat="server" />
                                                        </td>
                                                        <td width="15%" align="right" valign="top" class="formtext" height="35">Stock Receive Status:<font class="error">+</font></td>
                                                        <td align="left" valign="top">
                                                             <asp:DropDownList ID="ddlStockReceiveStatus" Width="120px" runat="server" CssClass="form_select">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                        <td align="left" valign="top"></td>
                                                        <td align="left" valign="top">
                                                            <asp:Button ID="Search" runat="server" Text="Export To Excel" CssClass="buttonbg"
                                                                CausesValidation="true" ValidationGroup="Search" OnClick="Search_Click" />
                                                            <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="buttonbg"
                                                                CausesValidation="false" OnClick="Cancel_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="5"></td>
                        </tr>

                    </table>
                </div>

            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="Search" />
                <asp:PostBackTrigger ControlID="Cancel" />

            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
