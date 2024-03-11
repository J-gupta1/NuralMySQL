<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DownloadBulkTertioryData.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Reports_SalesChannel_DownloadBulkTertioryData" %>


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
                                                        Bulk Tertiory Report Search</div>
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
                                                                <td valign="top" align="right" class="formtext" width="10%">
                                                                    Date From:<font class="error">*</font>
                                                                </td>
                                                                <td align="left" valign="top" width="13%">
                                                                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                                                                        ValidationGroup="rptPurchase" defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                    <br />
                                                                </td>
                                                                <td valign="top" align="right" class="formtext" width="10%">
                                                                    Date To:<font class="error">*</font>
                                                                </td>
                                                                <td valign="top" align="left" width="15%">
                                                                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="rptPurchase"
                                                                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                </td>
                                                                <td valign="top" align="left" width="22%">
                                                                    <asp:Button ID="btnSearch" Text="Download" runat="server" ValidationGroup="rptPurchase"
                                                                        ToolTip="Download" CssClass="buttonbg" CausesValidation="true" OnClick="btnSearch_Click" />
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

