<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucTextboxMultiline.ascx.cs"
    Inherits="UserControls_ucTextboxMultiline" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

    
<asp:TextBox ID="txtMultiline" runat="server" TextMode="MultiLine"
    CssClass="form_textarea" CausesValidation="true" OnTextChanged="txtMultiline_TextChanged"
    onKeyDown="textCounter()" onKeyUp="textCounter()"></asp:TextBox><%--#CC01 :removed Width="150px"--%>
<cc1:TextBoxWatermarkExtender ID="tbweMultiline" runat="server" TargetControlID="txtMultiline">
</cc1:TextBoxWatermarkExtender>
<div class="clear"></div><%--#CC01: div tag added--%>
<div class="float-left">
    <asp:RequiredFieldValidator ID="rfvMultiline" runat="server" ControlToValidate="txtMultiline"
        Display="Dynamic" ErrorMessage="Please Input." CssClass="error"></asp:RequiredFieldValidator></div>
<div class="float-right">
     <div class="padding-right"><%--#CC01: style replaced with class--%>
        <asp:Label ReadOnly="true" runat="server" ID="remLen1" /><%--#CC01: removed Width="20px"--%>
        Chars left</div>
</div>
<asp:HiddenField ID="hdnCharsLeft" runat="server" Value="0" />
