﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="UploadBeatPlan.aspx.cs" Inherits="Transactions_BulkUpload_Activation_UploadBeatPlan" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="clear"></div>
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

                    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                </li>
                <li class="field3">
                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                        OnClick="btnUpload_Click" />
                </li>
            </ul>
            <ul>
                <li class="link">
                    <a class="elink2" href="../../../Excel/Templates/BeatPlanDetails.xlsx">Download Template</a>
                </li>
                <li class="link">
                     <asp:LinkButton ID="DwnldReferenceCode" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DwnldReferenceCode_Click" ></asp:LinkButton>
                </li>
                
                <li class="link">
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                    <asp:HyperLink ID="hlnkDuplicateNotInUse" runat="server" CssClass="elink3"></asp:HyperLink>
                    <asp:HyperLink ID="hlnkBlankNotInUse" runat="server" CssClass="elink3"></asp:HyperLink>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>