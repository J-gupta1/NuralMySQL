<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login_Beetel_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%=(String)GetGlobalResourceObject("Messages", "PageTitleText")%></title>
    <link href="../../Assets/orchid/CSS/login.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Assets/Jscript/jquery-1.2.6.min.js"></script>
    <script type="text/javascript" src="../../Assets/Jscript/jquery.ie6blocker.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="loginwrapper">
            <div id="logincontainer">
                <div id="logintop">
                    <div class="loginlogo">
                        <img src="../../assets/orchid/css/images/logo.gif" width="179" height="60" />
                    </div>
                    <div class="loginlogoright">
                        <img src="../../assets/orchid/css/images/zed_logo.png" width="178" height="31" />
                    </div>
                </div>
                <div id="loginleft">
                </div>
                <div id="loginright">
                    <div id="saleshead">
                        <img src="../../assets/orchid/css/images/sales.png" width="381" height="21" /></div>

                    <div id="loginbox">
                        <div class="loginboxtop"></div>
                        <div class="loginboxmid">
                            <div class="loginheading">Login</div>
                            <div class="loginmid2">
                                <div class="errorarea">
                                    <asp:Label runat="server" ID="lblHeader" CssClass="error"></asp:Label><%--#CC01 Add Start--%>
                                    <span class="float-right"><span><a id="HyplkLogOut" class="linkbutton" runat="server" visible="false">Logout</a></span></span>
                                    <div class="clear"></div>
                                    <%--#CC01 Add End--%>
                                </div>
                                <div class="logintext">
                                    Username
                                </div>
                                <div class="logininput">
                                    <asp:TextBox ID="txtUsername" runat="server" MaxLength="30" CssClass="fields_input"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="reqUsername" runat="server" ControlToValidate="txtUsername" ForeColor=""
                                        CssClass="error" ErrorMessage="*" ValidationGroup="valgrpLogin"
                                        Display="Dynamic" />


                                </div>
                                <div class="logintext">
                                    Password
                                </div>
                                <div class="logininput">
                                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" MaxLength="20" CssClass="fields_input"></asp:TextBox>
                                    <asp:RequiredFieldValidator
                                        ID="reqPassword" runat="server" ControlToValidate="txtPassword" ForeColor=""
                                        CssClass="error" ErrorMessage="*" ValidationGroup="valgrpLogin"
                                        Display="Dynamic" />
                                </div>
                                <div class="buttonarea">

                                    <asp:Button Text="" runat="server" ID="btnSubmit" CssClass="loginsubmit" ValidationGroup="valgrpLogin" OnClick="btnSubmit_Click"></asp:Button>
                                </div>
                                <div class="fgt_pass">
                                    <a href="ForgotPassword.aspx" class="blacklink11">Forgot Password?</a>
                                </div>
                            </div>
                        </div>
                        <div class="loginboxbot"></div>


                    </div>
                </div>

            </div>
            <div id="loginfooter">
                <div id="loginfooter2">
                    <div class="loginfooterleft">
                        &copy; Copyright 2011 Zed-Axis Technologies
                    </div>
                    <div class="loginfooterright">
                        <a href='<%= (String)ConfigurationSettings.AppSettings["RedirectUrl"] %>' target="_blank">
                            <img src="../../assets/orchid/css/images/zed_axislogo.gif" border="0" /></a>
                    </div>
                </div>
            </div>
        </div>















    </form>
</body>
</html>
