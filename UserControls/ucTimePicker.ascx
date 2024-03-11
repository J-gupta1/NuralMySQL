<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucTimePicker.ascx.cs"
    Inherits="LuminousSMS.Controls.ucTimePicker" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div>
    <asp:TextBox ID="txtTime" runat="server" CssClass="datepicker"></asp:TextBox> <%--#CC01: Width="70px" removed--%>
    <div>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
            ControlToValidate="txtTime" CssClass="error"/>
        <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureName="en-GB"
            Mask="99:99" AcceptAMPM="true" MaskType="Time" TargetControlID="txtTime" />
        <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
            ControlToValidate="txtTime" IsValidEmpty="true" EmptyValueMessage="Time required" CssClass="error"
            InvalidValueMessage="Time is invalid" Display="Dynamic" InvalidValueBlurredMessage="Invalid Time" /><%--#CC02: CssClass added--%>
        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" WatermarkText="HH:MM AM/PM" TargetControlID="txtTime" />
    </div>
</div> 