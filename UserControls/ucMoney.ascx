<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucMoney.ascx.cs" Inherits="ZedEBS.Controls.ucMoney" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div>
    <asp:TextBox ID="txtAmount" runat="server" MaxLength="7" Width="70px" Style="text-align: right;"
        OnTextChanged="txtAmount_TextChanged" CssClass="formfields"></asp:TextBox>
    <div>
        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtAmount"
            FilterMode="ValidChars" ValidChars=".0123456789">
        </cc1:FilteredTextBoxExtender>
        <%--<asp:RegularExpressionValidator ID="regexpAmount" runat="server" ControlToValidate="txtAmount"
            CssClass="error" Display="Dynamic" ErrorMessage="Invalid amount!" SetFocusOnError="True"
            ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,7}(\.\d{1,2})?$"
            
            ></asp:RegularExpressionValidator>--%>
            
            <asp:RegularExpressionValidator ID="regexpAmount" runat="server" ControlToValidate="txtAmount"
            CssClass="error" Display="Dynamic" ErrorMessage="Invalid amount!" SetFocusOnError="True"
            ValidationExpression="^\$?(\d{1,3}(\,\d{3})*|(\d+))(\.\d{0,2})?$"
            
            ></asp:RegularExpressionValidator>
        <asp:RequiredFieldValidator ID="rfvAmount" runat="server" Display="Dynamic" ControlToValidate="txtAmount"
            CssClass="error" />
        <asp:RangeValidator ID="rvAmount" runat="server" Display="Dynamic" ControlToValidate="txtAmount"
            CssClass="error" />
        <%--<cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtAmount"
            Mask="9999.99" MaskType="Number" MessageValidatorTip="true" InputDirection="RightToLeft" 
            DisplayMoney="None" AcceptNegative="None" />
        <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
            ControlToValidate="txtAmount" Display="Dynamic" IsValidEmpty="true" InvalidValueMessage="Amount is invalid" />--%>
    </div>
</div>
