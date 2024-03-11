<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucMenu.ascx.cs" Inherits="Web.Controls.ucMenu" %>
<%--<script type="text/javascript" src="../../assets/jscripts/Jscript/JSAsynRequest.js"></script>--%>

<div class="clear"></div>
<div id="menu">
    <div class="container">
        <%--<div class="nav">
            <div class="navbar navbar-default">
                <div class="container-fluid">--%>
                    <div class="container">
                        <asp:Menu ID="HeaderMenu"  Orientation="Horizontal" CssSelectorClass="SimpleEntertainmentMenu"  DataSourceID="xmlDataSource" runat="server"  DynamicHorizontalOffset="2" StaticSubMenuIndent="10px"  >
                              <DataBindings>
                                <asp:MenuItemBinding DataMember="MenuItem" NavigateUrlField="NavigationURL" TextField="MenuName" 
                                    ToolTipField="ToolTip" />
                            </DataBindings>
                              <DynamicHoverStyle  />
                              <DynamicMenuItemStyle />
                              <DynamicMenuStyle  />
                              <DynamicSelectedStyle  />
                              <StaticHoverStyle  />
                              <StaticMenuItemStyle />
                              <StaticSelectedStyle  />
                            </asp:Menu>                       
                        <asp:XmlDataSource ID="xmlDataSource" EnableCaching="false" runat="server" TransformFile='<%# "../" + strAssets + "/CSS/TransformXSLT.xsl" %>'
                            XPath="MenuItems/MenuItem" />
                    </div>
                </div>
            </div>
        <%--</div>
    </div>
</div>--%>
<div class="clear"></div>
