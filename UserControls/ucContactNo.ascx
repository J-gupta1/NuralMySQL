<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucContactNo.ascx.cs" Inherits="UserControls_ucContactNo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<div>
    <asp:TextBox ID="txtContactNo" runat="server" MaxLength="20"  CssClass="formfields"></asp:TextBox>
    <div>
        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
            FilterType="Numbers" TargetControlID="txtContactNo" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtContactNo"
            CssClass="error" />
        <%--#cc01:commented start--%>
        <%--<asp:RegularExpressionValidator ID="regexpContact1" runat="server" ToolTip="Please enter valid 10 digit contact no."
             ControlToValidate="txtContactNo" ValidationExpression="^[123456789][0-9]{9}" ErrorMessage="Enter valid 10 digit No."
            Display="Dynamic"  CssClass="error" />--%>
        <%--#cc01:commented end--%>
         <%--#cc01:added start--%>
         <asp:RegularExpressionValidator ID="regexpContact1" runat="server" ToolTip="Please enter valid 10 digit contact no."
             ControlToValidate="txtContactNo"
            Display="Dynamic"  CssClass="error" />
         <%--#cc01:added end--%>
           
    </div>
</div>
