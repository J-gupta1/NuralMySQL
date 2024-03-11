<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="DownloadSMSLogRpt.aspx.cs" Inherits="Reports_SAP_DownloadSMSLogRpt" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" Runat="Server">
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
                                                                    Sales Report Search</div>
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
                                                                            <td valign="top" class="formtext" align="right" width="10%">
                                                                                Sales Type:<font class="error">*</font>
                                                                            </td>
                                                                            <td align="left" valign="top" width="20%">
                                                                                <div style="float:left; width:135px;"><asp:DropDownList ID="ddlSalesType" runat="server" CssClass="form_select4">
                                                                                </asp:DropDownList>
                                                                                <br />
                                                                                <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlSalesType"
                                                                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales type."
                                                                                    SetFocusOnError="true" ValidationGroup="SalesReport"></asp:RequiredFieldValidator></div>
                                                                            </td>
                                                                            <td valign="top" align="right" class="formtext" width="10%">
                                                                                Date From:<font class="error">*</font>
                                                                            </td>
                                                                            <td align="left" valign="top" width="13%">
                                                                                <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                                                                                    ValidationGroup="SalesReport" defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                                <br />
                                                                            </td>
                                                                            <td valign="top" align="right" class="formtext" width="10%">
                                                                                Date To:<font class="error">*</font>
                                                                            </td>
                                                                            <td valign="top" align="left" width="15%">
                                                                                <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required."
                                                                                    ValidationGroup="SalesReport" defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                            </td>
                                                                            <td valign="top" align="left" width="22%">
                                                                                <asp:Button ID="btnSearch" Text="Download" runat="server" ValidationGroup="SalesReport"
                                                                                    ToolTip="Download" CssClass="buttonbg" CausesValidation="true" OnClick="btnSearch_Click" />
                                                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                                                                    OnClick="btnCancel_Click" />
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
                                    <td height="10">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
</asp:Content>

