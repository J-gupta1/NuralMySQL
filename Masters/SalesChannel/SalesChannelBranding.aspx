<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalesChannelBranding.aspx.cs"
    Inherits="Masters_SalesChannel_SalesChannelBranding" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">--%>
<!DOCTYPE html>
<html lang="en">
<head runat="server" id="Head1">
    <title>
        <%=(String)GetGlobalResourceObject("Messages", "PageTitleText")%></title>
    <%--Add START  --%>
    <meta charset="utf-8">
    <meta http-equiv="content-type" content="text/html; charset=UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <link rel="stylesheet" id="bootstrapCss" runat="server" type="text/css" />
    <%-- Add END --%>
    <link rel="stylesheet" id="cssStyle" runat="server" type="text/css" />
    <%--<link rel="stylesheet" id="csswithoutmaster" runat="server" type="text/css" />--%>
    <%--Add START  --%>
    <script src="../../Assets/Jscript/bootstrap.min.js"></script>
    <%-- Add END --%>
    <%--<link rel="stylesheet" id="csswithoutmaster" runat="server" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/Menu.css") %>" />--%>
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrapper">
            <!--Wrapper Start-->
            <div id="container">
                <header>
                    <div class="container">
                        <div class="row">
                            <div class="col-md-3 col-sm-3 col-xs-3">
                                <div class="logo">
                                    <asp:HyperLink ID="hyplogo" CausesValidation="false" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-5">
                               <div class="welcometext">
                                    <div>
                                        Welcome
                        <asp:Label ID="lblUserNameDesc" CssClass="logintime" Visible="true" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3 col-sm-3 col-xs-4">
                                <div class="zedsalestrack">
                                   <div class="right-header">
                                        <asp:Image runat="server" ID="ImgSideLogo" alt="Zed-Sales Track" title="Zed-Sales Track"
                                            border="0" />
                                    </div>
                                    <div class="toplinks">
                                        <div>
                                            <a href='<%=strSiteUrl%>Logout.aspx' class="elink6">Logout</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="heading2">                     
                    </div>
                </header>
                <article>
                    <div class="container">
                        <div class="content-wrapper">
                            <div class="bodycontent">
                                <asp:ScriptManager ID="Scriptmanager2" runat="server">
                                </asp:ScriptManager>
                                <div>
                                    <asp:UpdatePanel ID="updAddBranding" runat="server" UpdateMode="always">
                                        <ContentTemplate>
                                            <%--<asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
                                                        <ContentTemplate>--%>
                                            <uc1:ucMessage ID="ucMsg" runat="server" />
                                            <%--      </ContentTemplate>
                                                    </asp:UpdatePanel>--%>
                                            <div class="mainheading">
                                                Enter Brand Mapping
                                            </div>
                                            <div class="float-right">
                                                <asp:LinkButton ID="LBSwitchToBrand" runat="server" CausesValidation="False" CssClass="elink7" Text=""></asp:LinkButton>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="contentbox">
                                                <div class="H25-C3-S">
                                                    <ul>
                                                        <li class="text">
                                                            <asp:Label ID="lblddlBrand" runat="server">Choose Brand:</asp:Label>
                                                        </li>
                                                        <li class="field">
                                                            <div>
                                                                <asp:DropDownList runat="server" CssClass="formselect" ID="ddlBrandMapping"
                                                                    AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div>
                                                                <asp:Label Style="display: none;" runat="server" ID="lblHierarchyLevel" CssClass="error"></asp:Label>
                                                            </div>
                                                        </li>
                                                        <li class="text"></li>
                                                        <li class="field">
                                                            <asp:Button ID="btnSubmit" Text="Submit" runat="server" CausesValidation="true"
                                                                ToolTip="Add Brand" CssClass="buttonbg" OnClick="btnSubmit_Click" />
                                                        </li>
                                                    </ul>

                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </article>
            </div>
            <footer>
                <div class="container">
                    <!--footer Start-->
                   <div class="copyrightnew">
                        Power by <span class="ftrlogo"><img src="http://localhost/dms/Assets/ZedSales/CSS/Images/footerlogo.png"></span> 
                    </div>
                    <div style="display: none;">
                        <asp:HyperLink ID="hypfooterlogo" CausesValidation="false" runat="server" />
                    </div>
                </div>
            </footer>
        </div>
    </form>
</body>
</html>
