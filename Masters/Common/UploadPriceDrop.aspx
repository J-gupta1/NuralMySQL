<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="UploadPriceDrop.aspx.cs" Inherits="Masters_Common_UploadPriceDrop" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div align="center">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td align="left" valign="top">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <asp:UpdatePanel ID="updMessage" runat="server">
                                <ContentTemplate>
                                    <td align="left" valign="top" height="15">
                                        <uc4:ucMessage ID="uclblMessage" runat="server" />
                                    </td>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top" class="tableposition">
                                            <div class="mainheading">
                                                Upload
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left">
                                <div class="contentbox">
                                    <asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td colspan="6" height="20" class="mandatory" valign="top">(*) Marked fields are mandatory.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="15" align="right" valign="top" class="formtext">Upload file:<font class="error">*</font>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:FileUpload ID="FileUpload1" CssClass="form_file" runat="server" />
                                                        &nbsp;&nbsp;
                                                        <asp:Button ID="btnUpload" runat="server" CssClass="buttonbg" CausesValidation="true"
                                                            Text="Upload" OnClick="btnUpload_Click" />
                                                    </td>
                                                    <td align="left" valign="top"></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" colspan="6">
                                                        <a class="elink2" href="../../Excel/Templates/UploadPriceDrop.xlsx">Download Template</a>

                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnUpload" />
                                             <asp:PostBackTrigger ControlID="btnExportexcel" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td height="10"></td>
                        </tr>
                        <tr>

                             <div>
                    <table cellspacing="0" cellpadding="0" width="965" border="0">
                        <tr>
                           <%-- <td align="left" valign="top">
                                <uc4:ucMessage ID="ucMessage1" runat="server" />

                            </td>--%>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <div class="mainheading_rpt">
                                    <div class="mainheading_rpt_left">
                                    </div>
                                    <div class="mainheading_rpt_mid">
                                        View Price Drop</div>
                                    <div class="mainheading_rpt_right">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <table cellspacing="0" cellpadding="0" width="965" border="0">
                                    <tr>
                                        <td valign="top" align="left">
                                            <div class="contentbox">
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td class="mandatory" colspan="6" height="20" valign="top" align="left">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="30" width="13%" align="right" valign="top" class="formtext">&nbsp;Serial No:<font class="error">+</font>
                                                        </td>
                                                        <td width="20%" align="left" valign="top">
                                                            <asp:TextBox ID="txtserialnumber" runat="server" CssClass="form_input2"  MaxLength="30"></asp:TextBox>
                                                        </td>
                                                        <td width="13%" align="right" valign="top" class="formtext">Type:<font class="error">+</font>
                                                        </td>
                                                        <td width="20%" align="left" valign="top">
                                                            <asp:DropDownList ID="ddlType" runat="server"></asp:DropDownList>
                                                        </td>
                                                        <td width="13%" align="right" valign="top" class="formtext">
                                                        </td>
                                                        <td align="left" valign="top">
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="top" class="formtext" height="40">Price Drop From Date:<font class="error">+</font>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <uc1:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date." ValidationGroup="SearchNo"
                                                                runat="server" />
                                                        </td>
                                                        <td align="right" valign="top" class="formtext">Price Drop To Date:<font class="error">+</font>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <uc1:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date." ValidationGroup="SearchNo"
                                                                runat="server" />
                                                        </td>
                                                        <td align="right" valign="top" class="formtext">
                                                            
                                                        </td>
                                                        <td align="left" valign="top">
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="30"></td>
                                                        <td></td>
                                                        <td align="right" valign="top" class="formtext"></td>
                                                        <td align="right" valign="top" class="formtext">
                                                            <div class="float-margin">
                                                                <asp:Button ID="btnExportexcel" runat="server" Text="Export To Excel" CssClass="buttonbg"
                                                                    CausesValidation="true" ValidationGroup="Search" OnClick="btnExportexcel_Click" />
                                                            </div>
                                                        </td>
                                                        <td  align="right" valign="top" class="formtext">
                                                            

                                                        </td>
                                                        <td align="left" valign="top" class="formtext">
                                                            
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td height="5"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                        </tr>
                        <tr>
                            <td align="left" height="10"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
