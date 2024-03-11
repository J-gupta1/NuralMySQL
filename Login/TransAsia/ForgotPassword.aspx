﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=(String)GetGlobalResourceObject("Messages", "PageTitleText")%></title>
    <link rel="stylesheet"  href="~/Assets/Beetel/CSS/login.css"  type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div id="loginwrapper">
<div id="logincontainer">
<div id="logintop"> 
<div class="loginlogo"><img src="../../Assets/Beetel/CSS/Images/logo.png" width="215" alt="logo" height="50" /></div>
<div class="loginlogoright"><img  src="../../Assets/Beetel/CSS/Images/zed_logo.png" width="178" alt="zed-axis" height="31" /></div>
</div>
<div id="loginleft"></div>
<div id="loginright">
<div class="loginheading">Forgot Password</div>
<div id="loginbox">
<div class="errorarea"><asp:label id="LblHeader" runat="server" CssClass="error"></asp:label></div>
<div class="logintext">Username</div>
<div class="logininput"> <asp:textbox id="TxtLoginName" CssClass="fields_input" MaxLength="30" Runat="server"></asp:textbox></div> 
<div class="logintext" style="color:#ec5e00">OR</div>
<div class="logintext">Email ID</div>
<div class="logininput"><asp:textbox id="TxtEMail" CssClass="fields_input" Runat="server"></asp:textbox></div>
<div class="buttonarea"><asp:button id="BtnSubmit" CssClass="loginsubmit" Runat="server" Text="" 
                            CausesValidation="true" onclick="BtnSubmit_Click"></asp:button></div>
<div class="fgt_pass"><a href="Login.aspx" class="blacklink11">Back</a></div>
<div class="errorarea"><%--<asp:validationsummary id="VSAll2" Runat="server" DisplayMode="List" ShowSummary="False" ShowMessageBox="True"></asp:validationsummary>--%>
  						<%--<asp:requiredfieldvalidator id="RFVLoginName2" runat="server" Display="None" ControlToValidate="TxtLoginName" ErrorMessage="* Please Enter Login Name!"></asp:requiredfieldvalidator>
  						<asp:requiredfieldvalidator id="RFVEMail2" runat="server" Display="None" ControlToValidate="TxtEMail" ErrorMessage="* Please Enter your Email ID!"></asp:requiredfieldvalidator>--%>
  						<asp:regularexpressionvalidator id="REVEMail2" runat="server" Display="None" ControlToValidate="TxtEMail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Please Enter Valid Email ID"></asp:regularexpressionvalidator></div>
</div>
</div>
<div id="loginfooter">
<div class="loginfooterleft">&copy; Copyright 2011 Zed-Axis Technologies</div>
<div class="loginfooterright"><a href='<%= (String)ConfigurationSettings.AppSettings["RedirectUrl"] %>' target="_blank"><img  src="../../Assets/Beetel/CSS/Images/zed_axislogo.gif"  border="0" /></a></div>
</div>
 </div>

</div>

    </form>
</body>
</html>
