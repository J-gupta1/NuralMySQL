<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="BulkRetailerTransferUpload.aspx.cs" Inherits="Masters_SalesMan_BulkRetailerTransferUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMessage" runat="server" />
    <div class="mainheading">
        Upload
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                            OnClick="btnUpload_Click" />
                    </div>
                    <div class="float-left">
                        <asp:Button ID="btnReset" CssClass="buttonbg" runat="server" Text="Reset"
                            OnClick="btnReset_Click" CausesValidation="false" />
                    </div>
                </li>
                <li class="link">
                    <asp:LinkButton ID="DwnldReferenceCode" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DwnldReferenceCode_Click"></asp:LinkButton>
                </li>
                <li class="link">
                    <a class="elink2" href="../../Excel/Templates/BulkRetailerTransferUploadTemplate.xlsx">Download Template </a>
                </li>
                <li class="link">
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3"></asp:HyperLink>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
