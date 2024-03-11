<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCCurrency.ascx.cs" Inherits="UserControls_UCCurrency" %>
<div>
    <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="formselect">
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="reqddlCurrency" runat="server" ControlToValidate="ddlCurrency"
    ErrorMessage="Please select!" ></asp:RequiredFieldValidator>
</div>