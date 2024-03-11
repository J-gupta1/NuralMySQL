<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucServiceEntity.ascx.cs"
    Inherits="UserControl_ucServiceEntity" %>
<div>
    <asp:DropDownList ID="ddlServiceEntity" runat="server" CssClass="formselect" 
       onselectedindexchanged="ddlServiceEntity_SelectedIndexChanged">
    </asp:DropDownList>
</div>
<div>
    <asp:RequiredFieldValidator ID="rvServiceEntity" runat="server" ControlToValidate="ddlServiceEntity" SetFocusOnError="true" CssClass="error"></asp:RequiredFieldValidator>
</div>
