<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Web.Controls.Header" %>
<%--<script type="text/javascript" src="../../assets/jscripts/Jscript/JSAsynRequest.js"></script>--%>
<link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Work+Sans:ital,wght@0,100;0,300;0,400;0,700;0,900;1,100;1,200;1,400;1,700;1,900&display=swap" rel="stylesheet">

<div class="container">
    <div class="row">
        <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="logo">
                <a href='<%=strSiteUrl%>Default.aspx' class="elink6">
                    <asp:HyperLink ID="hyplogo" CausesValidation="false" runat="server" /></a>
            </div>
        </div>


        <div class="col-md-5 col-sm-12 col-xs-12">
            <div id="divstock" runat="server" visible="false" class="welcomeuser-margin">
                Opening Stock Date
                    <asp:Label ID="lblOpeningDate" CssClass="logintime" Visible="true" runat="server"></asp:Label>
            </div>
            <div class="b1">
                Welcome
                <span>
                    <asp:Label ID="lblUserNameDesc" Visible="true" runat="server"></asp:Label></span>
            </div>

        </div>





        <div class="col-md-4 col-sm-12 col-xs-12">
            <div class="righblk">

                <ul class="listv">
                    <li>
                        <div id="dvChngPssLnk" runat="server">
                            <a href='<%=strSiteUrl%>ChangePassword.aspx' title="Change Password" class="ctxt"><span class="change_pw_icon"></span><span class="icon_text">Change
                                <br>
                                Password </span></a>
                        </div>
                    </li>

                    <li>
                        <div class="logbn">
                            <a href='<%=strSiteUrl%>Logout.aspx'><span class="logout_icon"></span><span class="icon_text">Logout</span></a>
                        </div>
                    </li>



                </ul>

                <div class="rightlogo">
                    <img src='<%=strSiteUrl%><%= strAssets %>/CSS/Images/nsfalogo.png'>
                </div>



            </div>


        </div>
    </div>
