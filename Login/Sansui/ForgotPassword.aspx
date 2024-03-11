﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%=(String)GetGlobalResourceObject("Messages", "PageTitleText")%></title>
    <link rel="stylesheet" href="~/Assets/Sansui/CSS/login.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="header">
        <div class="wrapper">
            <div class="login_header">
                <div class="logo">
                    <img src="../../assets/Sansui/css/images/logo.png" alt="" /></div>
                <div class="header-right">
                    <img src="../../assets/Sansui/css/images/zed_logo.png" alt="" /></div>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
    <div id="loginwrapper">
        <div id="logincontainer">
            <%--<div id="logintop"> 
<div class="loginlogo"><img src="../../Assets/Sansui/CSS/Images/logo.png" width="215" alt="logo" height="50" /></div>
<div class="loginlogoright"><img  src="../../Assets/Sansui/CSS/Images/zed_logo.png" width="178" alt="zed-axis" height="31" /></div>
</div>--%>
            <div id="loginleft">
            </div>
            <div id="loginright">
                <div id="loginbox">
                    <div class="loginhead">
                        Forgot Password</div>
                    <div class="errorarea">
                        <asp:Label ID="LblHeader" runat="server" CssClass="error"></asp:Label>
                        <asp:RegularExpressionValidator Display="Static" CssClass="error" ID="REVEMail2"
                            ForeColor="#fdffca" runat="server" ControlToValidate="TxtEMail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            ErrorMessage="Please Enter Valid Email ID"></asp:RegularExpressionValidator></div>
                    <div class="loginfield">
                        <div class="logintext">
                            User name :</div>
                        <asp:TextBox ID="TxtLoginName" CssClass="login_input" MaxLength="30" runat="server"></asp:TextBox>
                    </div>
                    <div class="blacklink11" style="text-align: center; padding-top: 3px; clear: both;">
                        OR</div>
                    <div class="loginfield">
                        <div class="logintext">
                            Email ID :</div>
                        <asp:TextBox ID="TxtEMail" class="login_input" runat="server"></asp:TextBox>
                    </div>
                    <div class="loginfield">
                        <div class="logintext">
                            &nbsp;</div>
                        <asp:Button ID="BtnSubmit" CssClass="login_button" runat="server" Text="Submit" CausesValidation="true"
                            OnClick="BtnSubmit_Click"></asp:Button>
                        <div class="gorgotpwd">
                            <a href="Login.aspx" class="blacklink11">Back</a></div>
                    </div>
                    <div class="errorarea">
                        <%--<asp:validationsummary id="VSAll2" Runat="server" DisplayMode="List" ShowSummary="False" ShowMessageBox="True"></asp:validationsummary>--%>
                        <%--<asp:requiredfieldvalidator id="RFVLoginName2" runat="server" Display="None" ControlToValidate="TxtLoginName" ErrorMessage="* Please Enter Login Name!"></asp:requiredfieldvalidator>
  						<asp:requiredfieldvalidator id="RFVEMail2" runat="server" Display="None" ControlToValidate="TxtEMail" ErrorMessage="* Please Enter your Email ID!"></asp:requiredfieldvalidator>--%>
                    </div>
                </div>
            </div>
            <div class="clear">
            </div>
            <%--<div id="loginfooter">
                <div class="loginfooterleft">
                    &copy; Copyright 2011 Zed-Axis Technologies</div>
                <div class="loginfooterright">
                    <a href='<%= (String)ConfigurationSettings.AppSettings["RedirectUrl"] %>' target="_blank">
                        <img src="../../Assets/Sansui/CSS/Images/zed_axislogo.gif" border="0" /></a></div>
            </div>--%>
        </div>
    </div>
    <div class="footer">
        <div class="wrapper">
            <div class="footer-left">
                &copy; Zed-Axis Technologies 2015-16 All Rights Reserved
            </div>
            <div class="footer-center">
                Browser Compatibility: IE 7.0, 8.0 , Firefox - 4.0</div>
            <div class="zedservicelogo">
                <a href='<%= (String)ConfigurationSettings.AppSettings["RedirectUrl"] %>' target="_blank">
                    <img src="../../assets/Sansui/css/images/zed-saleslogo.png" border="0" /></a></div>
            <div class="powerdby">
                Powered by</div>
            <div class="clear">
            </div>
        </div>
    </div>
    </form>
</body>
</html>
