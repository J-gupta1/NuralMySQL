<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="StockTransferBCP.aspx.cs" Inherits="Transactions_StockTransferBCP" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0" align="center" style="float: left;">
        <tr>
            <td valign="top" align="left">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
                            <contenttemplate>
                                <td align="left" valign="top">
                                    <uc1:ucMessage ID="ucMsg" runat="server" />
                                </td>
                            </contenttemplate>
                        </asp:UpdatePanel>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="float: left">
                                <tr>
                                    <td align="left" valign="top" class="tableposition">
                                        <div class="mainheading_rpt">
                                            <div class="mainheading_rpt_left">
                                            </div>
                                            <div class="mainheading_rpt_mid">
                                                Upload</div>
                                            <div class="mainheading_rpt_right">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <div class="contentbox">
                                <%--<asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
                                    <ContentTemplate>--%>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="4" align="left" valign="top" height="20" class="mandatory">
                                            (<font class="error">*</font>) Marked fields are mandatory
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formtext" valign="top" align="right" width="10%">
                                            <%-- Select Mode:<font class="error">*</font>--%>
                                        </td>
                                        <td align="left" valign="top" class="formtext" colspan="3">
                                            <asp:RadioButtonList ID="rdModelList" runat="server" TextAlign="Right" RepeatDirection="Horizontal"
                                                CellPadding="0" CellSpacing="0" BorderWidth="0" AutoPostBack="True" Visible="false"
                                                OnSelectedIndexChanged="rdModelList_SelectedIndexChanged">
                                                <asp:ListItem Text="Excel" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="On-Screen Entry" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="10%" height="35" align="right" class="formtext" valign="top">
                                            Upload File: <font class="error">*</font>
                                        </td>
                                        <td width="30%" align="left" valign="top">
                                            <asp:FileUpload ID="FileUpload1" CssClass="form_file" runat="server" />
                                             <br />
                                            <%--<asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>--%>
                                        </td>
                                        <td width="15%" align="left" valign="top">
                                            <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                                                OnClick="btnUpload_Click" />
                                        </td>
                                        <td align="left" valign="top">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" colspan="4">
                                             <%--#CC02 START ADDED--%>
                                             <asp:LinkButton ID="DwnldBindCode" runat="server" Text="Download Bin Code"
                                                CssClass="elink2" OnClick="DwnldBindCode_Click"></asp:LinkButton>&nbsp;
                                            &nbsp;&nbsp; &nbsp;
                                            <%--#CC02 END ADDED--%>
                                            <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                                                CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>&nbsp;
                                            &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp; <a class="elink2" href="../../../Excel/Templates/StockTransferSB.xlsx">
                                                Download Template</a>
                                            <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                                            
                                            
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td height="10">
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

