<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucNavigationLinks.ascx.cs"
    Inherits="ZedEBS.Controls.UserControls_ucNavigationLinks" %>
<div class="float-left"><%--#CC02: style replaced with class--%>
    <asp:Button ID="btnOrganHierarchyUpload" runat="server"  Text="Upload Hierarchy" CssClass="buttonbg" OnClick="btnOrganHierarchyUpload_Click" /> 
    <asp:Button ID="btnSalesChannelUpload" runat="server" CssClass="buttonbg" Text="Upload Sales Channel" OnClick="btnSalesChannelUpload_Click" /> 
    <asp:Button ID="btnISPUpload" runat="server"  Text="Upload ISP" CssClass="buttonbg" OnClick="btnISPUpload_Click" /> 
    <asp:Button ID="btnHierarchyUser" runat="server" CssClass="buttonbg" Text="Manage Hierarchy Users" OnClick="btnHierarchyUser_Click" /> 
</div>
<div class="float-right padding-left"><%--#CC02: style replaced with class--%>
    <asp:HyperLink ID="hyBack" Visible="false" runat="server">Back</asp:HyperLink>
</div>
<div class="clear"></div>
<div>
    
</div>
