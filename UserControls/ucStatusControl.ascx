<%--<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucStatusControl.ascx.cs" 
Inherits="UserControls_ucStatusControl" %>
<ul>
<li class="text">Status:<span class="optional-img">&nbsp;</span>

<asp:DropDownList ID="cmbStatus" runat="server" CssClass="formselect" >
    <asp:ListItem Value="233">Select</asp:ListItem>
    <asp:ListItem Value="1">Active</asp:ListItem>
    <asp:ListItem Value="0">Inactive</asp:ListItem>
    <asp:ListItem Value="255">All</asp:ListItem>
    </asp:DropDownList>
</li>
</ul>--%>

<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucStatusControl.ascx.cs" 
Inherits="UserControls_ucStatusControl" %>
<%--<ul>--%> <%--#CC01 :commented --%>
<li class="text">Status: <span class="optional-img">&nbsp;</span>
</li>
<li class="field">
<asp:DropDownList ID="cmbStatus" runat="server" CssClass="formselect" >
    <asp:ListItem Value="233">Select</asp:ListItem>
    <asp:ListItem Value="1">Active</asp:ListItem>
    <asp:ListItem Value="0">Inactive</asp:ListItem>
    <asp:ListItem Value="255">All</asp:ListItem>
    </asp:DropDownList>
</li>
<%--</ul>--%> <%--#CC01 :commented --%>