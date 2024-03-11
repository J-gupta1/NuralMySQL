<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="UploadDOA.aspx.cs" Inherits="DOA_UploadDOA" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>

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
                                                    <td height="35" align="right" valign="top" class="formtext">SalesChannel:<font class="error">*</font>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:DropDownList ID="ddlSaleschannel" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td height="35" align="right" valign="top" class="formtext">Upload file:<font class="error">*</font>
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
                                                        <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                                                            CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>&nbsp; &nbsp;
                                                        &nbsp; &nbsp;
                                                         <asp:LinkButton ID="DwnldSKUCodeTemplate" runat="server" Text="Download SKU Code"
                                                            CssClass="elink2" OnClick="DwnldSKUCodeTemplate_Click" ></asp:LinkButton>  &nbsp; &nbsp;
                                                         <a class="elink2" href="../Excel/Templates/UploadDOA.xlsx">Download Template</a>


                                                        &nbsp;
                                                 
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnUpload" />
                                            <asp:PostBackTrigger ControlID="DwnldReferenceCodeTemplate" />
                                            <asp:PostBackTrigger ControlID="DwnldSKUCodeTemplate" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td height="10"></td>
                        </tr>
                        <tr>
                            <td valign="top" align="left"></td>
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
