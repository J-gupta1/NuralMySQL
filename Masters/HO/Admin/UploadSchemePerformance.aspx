<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UploadSchemePerformance.aspx.cs" Inherits="Masters_HO_Admin_UploadSchemePerformance" %>

<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div id="msg">
        <uc1:ucMessage ID="ucMsg" runat="server" />
    </div>
    <div class="clear">
    </div>
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
                    <asp:FileUpload CssClass="fileuploads" ID="FileUpload1" runat="server" />
                </li>
                <li class="field3">
                    <asp:Button CssClass="buttonbg" ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                </li>
            </ul>
            <ul>
                <li class="link">
                    <a class="elink2" href="../../../Excel/Templates/ZedSalesTemplateForSchemePayout.xlsx">Download Upload Template</a>
                </li>
                <li class="link">
                    <asp:LinkButton ID="lnkbtnDownload" runat="server" CssClass="elink2" OnClick="lnkbtnDownload_Click">Download Scheme/Retailer </asp:LinkButton>
                </li>
                <li class="link">
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                </li>
                <li class="link">
                    <asp:HyperLink ID="hlnkDuplicateNotInUse" runat="server" CssClass="elink3"></asp:HyperLink>
                </li>
                <li class="link">
                    <asp:HyperLink ID="hlnkBlankNotInUse" runat="server" CssClass="elink3"></asp:HyperLink>
                </li>
            </ul>
        </div>
    </div>

</asp:Content>
