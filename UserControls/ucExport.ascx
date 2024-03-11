<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucExport.ascx.cs" Inherits="UserControls_ucExport" %>

<div>
    <asp:DropDownList ID="ddlExportType" runat="server" CssClass="form_select">
        <asp:ListItem  Value="0"  Selected="True">Select</asp:ListItem>
        <asp:ListItem  Value="1">xlsx</asp:ListItem>
        <asp:ListItem  Value="2">csv</asp:ListItem>
        
    </asp:DropDownList>
</div>
<div>
    <asp:RequiredFieldValidator ID="rvExportType" runat="server" ControlToValidate="ddlExportType" Display="Dynamic"
      ErrorMessage="Please select Export Type!" InitialValue="0"  SetFocusOnError="true" CssClass="error"></asp:RequiredFieldValidator>
</div>
