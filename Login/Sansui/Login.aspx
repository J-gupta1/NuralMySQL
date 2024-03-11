<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login_Beetel_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>
        <%=(String)GetGlobalResourceObject("Messages", "PageTitleText")%></title>
    <link href="../../Assets/Sansui/CSS/login.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../../Assets/Jscript/jquery-1.2.6.min.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/jquery.ie6blocker.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="header">
        <div class="wrapper">
            <div class="login_header">
                <div class="logo" style="background: url(../../assets/Sansui/css/images/logo.png) no-repeat;">
                </div>
                <div class="header-right">
                    <img src="../../assets/Sansui/css/images/zed_logo.png" alt="" />
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
    <div id="loginwrapper">
        <div id="logincontainer">
            <%--      <div id="logintop">
                <div class="loginlogo">
                    <img src="../../assets/Sony/css/images/logo.png" width="215" height="50" /></div>
                <div class="loginlogoright">
                    <img src="../../assets/Sony/css/images/zed_logo.png" width="178" height="31" /></div>
            </div>--%>
            <div id="loginleft">
            </div>
            <div id="loginright">
                <div id="loginbox">
                    <div class="loginhead">
                        Login</div>
                    <div class="errorarea">
                        <asp:Label runat="server" ID="lblHeader" CssClass="error"></asp:Label></div>
                    <div class="loginfield">
                        <div class="logintext">
                            User name :</div>
                        <asp:TextBox ID="txtUsername" runat="server" MaxLength="30" CssClass="login_input "></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqUsername" runat="server" ControlToValidate="txtUsername"
                            ForeColor="" CssClass="error" ErrorMessage="*" ValidationGroup="valgrpLogin"
                            Display="Dynamic" />
                    </div>
                    <div class="loginfield">
                        <div class="logintext">
                            Password :</div>
                        <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" MaxLength="20" class="login_input"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqPassword" runat="server" ControlToValidate="txtPassword"
                            ForeColor="" CssClass="error" ErrorMessage="*" ValidationGroup="valgrpLogin"
                            Display="Dynamic" />
                    </div>
                    <div class="loginfield">
                        <div class="logintext">
                            &nbsp;</div>
                        <asp:Button Text="Submit" runat="server" ID="btnSubmit" CssClass="login_button" ValidationGroup="valgrpLogin"
                            OnClick="btnSubmit_Click"></asp:Button>
                        <div class="gorgotpwd">
                            <a href="ForgotPassword.aspx" class="blacklink11">Forgot Password</a></div>
                    </div>
                    <%--<div class="fgt_pass">
                        <a href="ForgotPassword.aspx" class="blacklink11">Forgot Password?</a></div>--%>
                </div>
            </div>
            <%--<div id="loginfooter">
                <div class="loginfooterleft">
                    &copy; Copyright 2011 Zed-Axis Technologies </div>
                    
                     <div class="loginfootermid">ie users should use version 7 or above</div>
                    
                <div class="loginfooterright">
                    <a  href='<%= (String)ConfigurationSettings.AppSettings["RedirectUrl"] %>'  target="_blank">
                        <img src="../../assets/Sansui/css/images/zed_axislogo.gif" border="0" /></a></div>
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
