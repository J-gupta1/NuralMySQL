<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HeaderResponsive.ascx.cs" Inherits="Web.Controls.HeaderResponsive" %>
<%--<script type="text/javascript" src="../../assets/jscripts/Jscript/JSAsynRequest.js"></script>--%>

<div class="head_container">
    <div class="row">
        <div class="col-md-3 col-sm-3 col-xs-3">
            <div class="logo">
                <a href='<%=strSiteUrl%>Default.aspx' class="elink6">
                    <asp:HyperLink ID="hyplogo" CausesValidation="false" runat="server" /></a>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-5">
            <div class="welcometext">
                <div>
                    <div id="divstock" runat="server" visible="false" class="welcomeuser-margin">
                        Opening Stock Date
                    <asp:Label ID="lblOpeningDate" CssClass="logintime" Visible="true" runat="server"></asp:Label>
                    </div>
                    Welcome
                <asp:Label ID="lblUserNameDesc" CssClass="logintime" Visible="true" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <div class="col-md-3 col-sm-3 col-xs-4">
            <div class="zedsalestrack">
                <div class="right-header">
                    <%--<img id="ImgSideLogo" runat="server" alt="" title="" border="0" />--%>
                </div>
                <%--<asp:Image runat="server" ID="ImgSideLogo" 
                Width="172" Height="43" alt="Zed-Sales Track" title="Zed-Sales Track" border="0" />--%>
                <div class="toplinks">
                    <div id="dvChngPssLnk" runat="server">
                        <a href='<%=strSiteUrl%>ChangePassword.aspx' title="Change Password" class="elink6"><span class="change_pw_icon"></span><span class="icon_text">Change Password </span></a>
                        <div class="float-padding">
                           
                        </div>
                    </div>
                    <div>
                        <a href='<%=strSiteUrl%>Logout.aspx' class="elink6"><span class="logout_icon"></span><span class="icon_text">Logout</span></a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>



     

