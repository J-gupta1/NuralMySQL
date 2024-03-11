<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="ManageWarehouseGRN.aspx.cs"
    Inherits="Transactions_SalesChannelSB_ManageWarehouseGRN"  MasterPageFile="~/CommonMasterPages/MasterPage.master" %> 

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker"
    TagPrefix="uc2" %>
<%@ Register Src="../../../UserControls/PartLookupClientSideOpeningStock.ascx" TagName="PartLookupClientSide"
    TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/media/css/demo_page.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/css/demo_table.css") %>" />

    <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/jquery.js"></script>

    <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/jquery.dataTables.js"></script>
    
    <script type="text/javascript">
        function TableCount() {
            if (oTable.fnGetData().length == 0) {
                alert('There is no row to be submitted.');

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="Head1">
    <title>
        <%=(String)GetGlobalResourceObject("Messages", "PageTitleText")%></title>
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/Style.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/withoutmaster.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/Menu.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/media/css/demo_page.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/css/demo_table.css") %>" />

    <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/jquery.js"></script>

    <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/jquery.dataTables.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="Scriptmanager1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/CommonService.asmx" />
        </Services>
    </asp:ScriptManager>
    <div id="wrapper">
        <!--Wrapper Start-->
        <div id="container">
            <div id="insideheaderarea2">
                <div class="logo">
                    <asp:HyperLink ID="hyplogo" CausesValidation="false" Width="188" Height="50" runat="server" /></div>
                <div class="headerright">
                    <div class="zedsalestrack">
                        <asp:Image runat="server" ID="ImgSideLogo" alt="Zed-Sales Track" title="Zed-Sales Track"
                            border="0" /></div>
                    <div class="linesep">
                    </div>
                    <div id="welcome">
                        <div class="welcomeuser">
                            Welcome
                            <asp:Label ID="lblUserNameDesc" CssClass="logintime" Visible="true" runat="server"></asp:Label></div>
                        <div class="toplinks">
                            <a href='<%=strSiteUrl%>Logout.aspx' class="elink6">Logout</a></div>
                    </div>
                </div>
            </div>
            <div class="bodycontent">--%>
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--    <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>--%>

    <div class="mainheading">
        Enter Warehouse GRN
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">           
            <ul>
                <li class="text">Select Mode:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:RadioButtonList ID="rdModelList" runat="server" CssClass="radio-rs" TextAlign="Right" RepeatDirection="Horizontal"
                        CellPadding="2" CellSpacing="0" BorderWidth="0" AutoPostBack="True" OnSelectedIndexChanged="rdModelList_SelectedIndexChanged">
                        <asp:ListItem Text="Excel" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Interface" Value="1" Selected="True"></asp:ListItem>
                    </asp:RadioButtonList>
                </li>           
                <li class="text">
                    <asp:Label ID="lblOSDate" runat="server" AssociatedControlID="ucDatePicker" CssClass="formtext">GRN Date:<span class="error">*</span></asp:Label>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Pleaes enter date." ValidationGroup="Save" />
                </li>
                <li class="text">Warehouse: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlSalesChannel" runat="server" CssClass="formselect"
                        Enabled="false" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlSalesChannel"
                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select."
                        ValidationGroup="Save"></asp:RequiredFieldValidator>
                </li>
                <li class="text">GRN No.:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtGRNNo" runat="server" MaxLength="20" CssClass="formfields"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtGRNNo" Display="Dynamic"
                        CssClass="error" ValidationGroup="Save" runat="server" ErrorMessage="Please enter a grn no."></asp:RequiredFieldValidator>
                </li>                
            </ul>
        </div>
    </div>
    <div class="mainheading">
        List
    </div>
    <div class="contentbox">
        <uc3:PartLookupClientSide ID="PartLookupClientSide1" runat="server" />
    </div>
    <div class="margin-bottom">
        <div class="float-margin">
        <asp:Button ID="btnSave" Text="Save" runat="server" ToolTip="Save" CssClass="buttonbg"
            OnClick="btnSave_Click" ValidationGroup="Save" OnClientClick="TableCount();"></asp:Button>
        </div>
        <div class="float-margin">
        <asp:Button ID="btnCancel" Text="Cancel" runat="server" ToolTip="Cancel" CssClass="buttonbg"
            OnClick="btnCancel_Click" CausesValidation="false"></asp:Button>
        </div>
        <div class="clear"></div>
    </div>
    <%-- </div>
        </div>
        <div class="footerarea" >
            <div class="footer">
                <!--footer Start-->
                <div class="copyright">
                    &copy; Copyright 2011 Zed-Axis Technologies</div>
                <div class="footerright">
                    <asp:HyperLink ID="hypfooterlogo" CausesValidation="false" runat="server" />
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>--%>
</asp:Content>