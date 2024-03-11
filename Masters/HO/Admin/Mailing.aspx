<%@ Page Title="" Language="C#" 
    AutoEventWireup="true" CodeFile="Mailing.aspx.cs" Inherits="Masters_HO_Admin_Mailing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">--%>
    <form id="form1" runat="server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
                <tr>
                    <td align="left" valign="top">
                        <table cellspacing="0" cellpadding="0" width="965" border="0">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                                    </asp:ScriptManager>
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <uc1:ucMessage ID="ucMessage1" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="70%" align="left" valign="top" class="tableposition">
                                                            <div class="mainheading">
                                                                View Sales Channel</div>
                                                        </td>
                                                        <td width="30%" align="right" style="padding-right: 10px;">
                                                            
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left" class="tableposition">
                                                <div class="contentbox">
                                                    <table cellspacing="0" cellpadding="4" border="0" style="width: 100%">
                                                        <tr>
                                                            <td height="35" width="15%" align="right" valign="top" class="formtext">
                                                                Sales Channel Type:
                                                            </td>
                                                            <td width="20%" align="left" valign="top">
                                                                <div style="float: left; width: 135px;">
                                                                    <asp:DropDownList ID="cmbsaleschanneltype" runat="server" CssClass="form_select">
                                                                    </asp:DropDownList>
                                                                    <br />
                                                                </div>
                                                                <div style="float: left; width: 160px;">
                                                                    <asp:RequiredFieldValidator ID="req" runat="server" CssClass="error" InitialValue="0"
                                                                        ControlToValidate="cmbsaleschanneltype" ErrorMessage="Please select sales channel type"
                                                                        Display="Dynamic" ValidationGroup="Serach"></asp:RequiredFieldValidator></div>
                                                            </td>
                                                            <td width="15%" align="right" valign="top" class="formtext">
                                                                &nbsp;</td>
                                                            <td width="25%" align="left" valign="top" class="formtext">
                                                                &nbsp;</td>
                                                            <td align="left" width="25%" valign="top">
                                                                <asp:Button ID="btnSearch" runat="server" ValidationGroup="Serach" CausesValidation="true"
                                                                    CssClass="buttonbg" Text="Send Mail" OnClick="btnSearch_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" height="10" valign="top">
                                            </td>
                                        </tr>
                                        <div id="dvhide" runat="server" visible="false" style="float: left">
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td width="90%" align="left" class="tableposition">
                                                                <div class="mainheading">
                                                                    </div>
                                                            </td>
                                                            <td width="10%" align="right">
                                                                &nbsp;</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" class="tableposition">
                                                    <%--      <asp:UpdatePanel ID="UpdGrid" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>--%>
                                                    <div class="contentbox">
                                                        <div class="grid2">
                                                        </div>
                                                    </div>
                                                    <%-- </ContentTemplate>
                                                      
                                                    </asp:UpdatePanel>--%>
                                                </td>
                                            </tr>
                                        </div>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        
    </asp:UpdatePanel>
</form>

<%--</asp:Content>--%>