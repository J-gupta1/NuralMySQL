﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucDatePicker.ascx.cs"
    Inherits="Web.Controls.ucDatePicker" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div>
    <div class="float-margin">
        <asp:TextBox ID="txtDate" runat="server" CssClass="datepicker"></asp:TextBox>
    </div>
    <div class="calander-icon">
        <asp:Button runat="Server" ID="imgCalender" AlternateText="Click to show calendar"
            CssClass="calimg" CausesValidation="false" />
    </div>
    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDate"
        Format="MM/dd/yyyy" PopupPosition="Right" PopupButtonID="imgCalender" />
        <div class="clear"></div>
    <div>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="error" runat="server"
            Display="Dynamic" ControlToValidate="txtDate" />
        <asp:RangeValidator ID="RangeValidator1" runat="server" CssClass="error" Display="Dynamic"
            Type="Date" ControlToValidate="txtDate" />
        <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureName="en-US"
            Mask="99/99/9999" MaskType="Date" TargetControlID="txtDate" />
        <%-- <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
            ControlToValidate="txtDate" IsValidEmpty="true" EmptyValueMessage="Date required" 
            InvalidValueMessage="Date is invalid" Display="Dynamic" InvalidValueBlurredMessage="Invalid date"/>--%>
        <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
            ControlToValidate="txtDate" IsValidEmpty="true" EmptyValueMessage="Date required"
            Cssclass="error" InvalidValueMessage="Date is invalid" Display="Dynamic" />
    </div>
</div>