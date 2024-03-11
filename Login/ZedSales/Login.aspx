<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login_Beetel_Login" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>
        <%=(String)GetGlobalResourceObject("Messages", "PageTitleText")%></title>
    <link id="LoginCSS" runat="server" rel="stylesheet" type="text/css" />
    <link id="BootstrapCSS" runat="server" rel="stylesheet" type="text/css" />
    <link id="StyleCSS" runat="server" rel="stylesheet" type="text/css" />
    <%--<link href='~/<%= strAssets %>/CSS/login.css' rel="stylesheet" type="text/css" />
    <link href='~/<%= strAssets %>/CSS/bootstrap.min.css' rel="stylesheet" />
    <link href='../../<%= strAssets %>/CSS/style.css' rel="stylesheet" />--%>
    <script src="~/JS/bootstrap.min.js"></script>
    <%--<script type="text/javascript" src="../../Assets/Jscript/jquery-1.2.6.min.js"></script>
    <script type="text/javascript" src="../../Assets/Jscript/jquery.ie6blocker.js"></script>--%>
    <style>
        .buttonbg {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <header>
            <div class="container">
                <div class="login-header center-block">
                </div>
            </div>
        </header>
        <article>
            <div class="container">
                <div class="login-left">
                    <div class="logincont_left">
                    </div>
                </div>
                <div class="login-right">
                    <div class="login-area">
                        <div class="login-box">
                            <div class="loginhead">
                            </div>
                            <div class="login-msg">
                                <asp:Label runat="server" ID="lblHeader" CssClass="error"></asp:Label>
                                <span class="linkbutton"><a id="HyplkLogOut" runat="server" visible="false">Logout</a></span>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="row">

                                <%-- ============================#CC03 Added Started================================================--%>
                                <div class="col-md-11 col-sm-11 col-xs-11">
                                    <div class="form-group">
                                        <div>
                                            <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="30" CssClass="form-control user-icon" placeholder=" Enter access key"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <%--============================#CC03 Added End================================================--%>
                                <div class="col-md-11 col-sm-11 col-xs-11">
                                    <div class="form-group">
                                        <div>
                                            <asp:TextBox ID="txtUsername" runat="server" MaxLength="50" CssClass="form-control user-icon" placeholder="Enter User Name"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-1 col-sm-1 col-xs-1">
                                    <asp:RequiredFieldValidator ID="reqUsername" runat="server" ControlToValidate="txtUsername"
                                        ForeColor="" CssClass="error" ErrorMessage="*" ValidationGroup="valgrpLogin"
                                        Display="Dynamic" />
                                </div>
                                <div class="clearfix"></div>
                                <div class="col-md-11 col-sm-11 col-xs-11">
                                    <div class="form-group">
                                        <div>
                                            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" MaxLength="20" CssClass="form-control password-icon" placeholder="Enter Password"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-1 col-sm-1 col-xs-1">
                                    <div class="pull-left">
                                        <asp:RequiredFieldValidator ID="reqPassword" runat="server" ControlToValidate="txtPassword"
                                            ForeColor="" CssClass="error" ErrorMessage="*" ValidationGroup="valgrpLogin"
                                            Display="Dynamic" />
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                                <div class="col-md-11 col-sm-11 col-xs-11">
                                    <div class="form-group">
                                        <div class="pull-right">
                                            <div class="forgotlink">
                                                <a href="ForgotPassword.aspx">Forgot Password</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-1 col-sm-1 col-xs-1">
                                </div>
                                <div class="clearfix"></div>
                                <div class="col-md-11 col-sm-11 col-xs-11">
                                    <div class="form-group">
                                        <asp:Button Text="Submit" runat="server" ID="btnSubmit" CssClass="buttonbg" ValidationGroup="valgrpLogin"
                                            OnClick="btnSubmit_Click"></asp:Button><%--CssClass="btn btn-login btn-block"--%>
                                    </div>
                                </div>
                            </div>
                            <%--<div class="powerdby">
                                <div class="zed-logo center-block">
                                    <a href='http://www.zed-axis.com/' target="_blank" data-toggle="tooltip" title="Zed-Sales" style="outline: none;">
                                        <div class="copyright">
                                        </div>
                                    </a>
                                </div>
                            </div>--%>
                        </div>
                    </div>
                </div>
                <div class="clearfix visible-xs-block visible-md-block visible-sm-block"></div>
            </div>
        </article>
        <footer>
            <div class="container">
                <div class="copyrightnew">
                    Power by <span class="ftrlogo">
                        <img src='<%=strSiteUrl%><%= strAssets %>/CSS/Images/footerlogo.png'></span>
                </div>
                <div class="clearfix"></div>
            </div>
        </footer>
    </form>
</body>
</html>
