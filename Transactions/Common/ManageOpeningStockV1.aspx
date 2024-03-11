<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="ManageOpeningStockV1.aspx.cs"
    Inherits="Transactions_Common_ManageOpeningStockV1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Header.ascx" TagName="ucHeader" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/PartLookupClientSideOpeningStock.ascx" TagName="PartLookupClientSide"
    TagPrefix="uc3" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server" id="Head1">
    <title>
        <%=(String)GetGlobalResourceObject("Messages", "PageTitleText")%></title>
    <meta charset="utf-8">
    <meta http-equiv="content-type" content="text/html; charset=UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/bootstrap.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/style.css") %>" />
    <%--<link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/withoutmaster.css") %>" />--%>
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/Menu.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/media/css/demo_page.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/css/demo_table.css") %>" />
    <%--<link href="../../Assets/Beetel/CSS/modal.css" rel="stylesheet" type="text/css" />
    <link href="../../Assets/Beetel/CSS/dhtmlwindow.css" rel="stylesheet" type="text/css" />--%>

    <script src="../../Assets/Jscript/bootstrap.min.js"></script>
    <script type="text/javascript" language="javascript" src="../../Assets/Jscript/jquery.js"></script>

    <script type="text/javascript" language="javascript" src="../../Assets/Jscript/jquery.dataTables.js"></script>

    <script type="text/javascript">
        var baseUrl = '<%# ResolveUrl("~/") %>';
    </script>

    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

</head>
<body>
    <form id="aspnetForm" runat="server">
        <asp:ScriptManager ID="Scriptmanager1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/CommonService.asmx" />
            </Services>
        </asp:ScriptManager>
        <div id="wrapper">
            <!--Wrapper Start-->
            <div id="container">
                <!--header Start-->
                <header>
                    <div>
                        <uc1:ucHeader ID="ucHeader" runat="server" />
                    </div>
                    <div class="heading2">
                        <div class="container">
                            <div class="hd1">
                            </div>
                        </div>
                    </div>
                </header>
                <article>
                    <div class="container">
                        <div class="content-wrapper">
                            <div class="bodycontent">
                                <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
                                    <ContentTemplate>
                                        <uc1:ucMessage ID="ucMsg" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="msg">
                                    <strong>Important Note</strong> - Please note that if any of your Purchases are already uploaded in the system, 
                                                        the stock against them does not get added in your stock.
                                                        You will need to request for stock adjustment to add stock for those invoices.
                                                        For example - If you are entering opening stock for
                        <asp:Label ID="lblInfoDate" runat="server" Text=""></asp:Label>
                                    and your
                                                        purchases for that date or later are already uploaded, the stock for those invoices will not add to your stock.
                                </div>

                                <%--    <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>--%>
                                <div class="mainheading">
                                    Enter Opening Stock
                                </div>
                                <div class="contentbox">
                                    <div class="mandatory">
                                        (*) Marked fields are mandatory            
                                    </div>
                                    <div class="H25-C3-S">
                                        <ul>
                                            <li class="text">Select Mode: <span class="error">*</span>
                                            </li>
                                            <li class="field">
                                                <asp:RadioButtonList ID="rdModelList" runat="server" TextAlign="Right" RepeatDirection="Horizontal" CssClass="radio-rs"
                                                    CellPadding="4" CellSpacing="0" BorderWidth="0" AutoPostBack="True" OnSelectedIndexChanged="rdModelList_SelectedIndexChanged">
                                                    <asp:ListItem Text="Excel" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Interface" Value="1" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </li>                                                                                  
                                            <li class="text">
                                                <asp:Label ID="lblOSDate" runat="server" AssociatedControlID="ucDatePicker" CssClass="formtext">Opening Stock Date:<font class="error">*</font></asp:Label>
                                            </li>
                                            <li class="field">
                                                <uc2:ucDatePicker ID="ucDatePicker" runat="server" ValidationGroup="Save" ErrorMessage="Please Insert Opening Stock Date" />
                                            </li>
                                             <li class="field3">
                                                <asp:Button ID="btnInsert" CssClass="buttonbg" runat="server" Text="Proceed With Zero Quantity"
                                                    ValidationGroup="Save" CausesValidation="true" OnClick="btnInsert_Click"  />
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
                                <div class="float-margin">
                                    <asp:Button ID="btnSave" Text="Save" runat="server" ToolTip="Save" CssClass="buttonbg"
                                        OnClick="btnSave_Click" ValidationGroup="Save"></asp:Button>
                                </div>
                                <div class="float-left">
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" ToolTip="Cancel" CssClass="buttonbg"
                                        OnClick="btnCancel_Click" CausesValidation="false"></asp:Button>
                                </div>
                            </div>
                        </div>
                    </div>
                </article>
            </div>
            <footer>
                <div>
                    <div class="container">
                        <!--footer Start-->
                        <div class="copyright">
                            &copy; Copyright 2018 Zed-Axis Technologies
                        </div>
                        <div style="display: none;">
                            <asp:HyperLink ID="hypfooterlogo" CausesValidation="false" runat="server" />
                        </div>
                    </div>
                </div>
            </footer>
        </div>
    </form>
</body>
</html>
