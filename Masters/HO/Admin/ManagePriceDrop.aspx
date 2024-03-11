<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManagePriceDrop.aspx.cs" Inherits="Masters_HO_Admin_ManagePriceDrop" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Upload
    </div>
    <div class="contentbox">
        <%--<asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
                                    <ContentTemplate>--%>
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />

                    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                            OnClick="btnUpload_Click" ValidationGroup="Upload" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="false"
                            OnClick="btnCancel_Click" />
                    </div>
                </li>
            </ul>
            <ul>
                <li class="link">
                    <asp:LinkButton ID="DwnldReferenceCode" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DwnldReferenceCode_Click"></asp:LinkButton>&nbsp;
                </li>
                <li class="link">
                    <a class="elink2" href="../../../Excel/Templates/PriceDropTemplate.xlsx">Download Template </a>
                </li>
                <li class="link">
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3"></asp:HyperLink>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
