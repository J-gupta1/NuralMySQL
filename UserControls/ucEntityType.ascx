<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucEntityType.ascx.cs"
    Inherits="UserControl_ucEntityType" %>
<div>
    <asp:DropDownList ID="ddlEntityType" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlEntityType_SelectedIndexChanged">
    </asp:DropDownList>
</div>
<div>
    <asp:RequiredFieldValidator ID="rvServiceEntity" runat="server" ControlToValidate="ddlEntityType" SetFocusOnError="true" CssClass="error"></asp:RequiredFieldValidator>
</div>
