<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" %>

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
    <%--<link href="~/Assets/ZedSales/CSS/login.css" rel="stylesheet" type="text/css" />
    <link href="~/Assets/ZedSales/CSS/bootstrap.min.css" rel="stylesheet" />--%>

    <script src="~/JS/bootstrap.min.js"></script>
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
                <div>
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
                                    <asp:Label ID="LblHeader" runat="server" CssClass="error"></asp:Label>
                                </div>
                                <div class="row">
                                    <div class="col-md-11 col-sm-11 col-xs-11">
                                        <div class="form-group">
                                            <div class="loginfield">
                                                <asp:TextBox ID="TxtLoginName" CssClass="form-control user-icon" placeholder="Enter User Name" MaxLength="30" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-1 col-sm-1 col-xs-1">
                                    </div>
                                    <div class="col-md-11 col-sm-11 col-xs-11">
                                        <div class="form-group">
                                            <div class="text-center font-white">
                                                OR
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-1 col-sm-1 col-xs-1">
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="col-md-11 col-sm-11 col-xs-11">
                                        <div class="form-group">
                                            <div class="loginfield">
                                                <asp:TextBox ID="TxtEMail" CssClass="form-control mail-icon" placeholder="Enter E-Mail ID" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-1 col-sm-1 col-xs-1">
                                    </div>
                                    <div class="clearfix visible-xs-block"></div>
                                    <div class="col-md-11 col-sm-11 col-xs-11">
                                        <div class="form-group">
                                            <div class="pull-right">
                                                <div class="forgotlink">
                                                    <a href="Login.aspx" class="blacklink11">Back</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-1 col-sm-1 col-xs-1">
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="col-md-11 col-sm-11 col-xs-11">
                                        <div class="form-group">
                                            <asp:Button ID="BtnSubmit" CssClass="buttonbg" runat="server" Text="Submit" CausesValidation="true"
                                                OnClick="BtnSubmit_Click"></asp:Button>
                                        </div>
                                    </div>
                                    <div class="errorarea">
                                        <asp:RegularExpressionValidator ID="REVEMail2" runat="server" Display="None" ControlToValidate="TxtEMail"
                                            ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Please Enter Valid Email ID"></asp:RegularExpressionValidator>
                                    </div>

                                </div>
                                <%--<div class="powerdby">
                                    <div class="zed-logo center-block">
                                        <a href='http://www.zed-axis.com/' target="_blank" data-toggle="tooltip" title="Zed-Service" style="outline: none;">
                                            <div class="copyright">
                                            </div>
                                        </a>
                                    </div>
                                </div>--%>
                            </div>
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
