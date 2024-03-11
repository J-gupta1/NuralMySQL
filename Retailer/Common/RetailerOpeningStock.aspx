    <%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="RetailerOpeningStock.aspx.cs"
    Inherits="Retailer_Common_RetailerOpeningStock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Header.ascx" TagName="ucHeader" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/PartLookupClientSideOpeningStock.ascx" TagName="PartLookupClientSide"
    TagPrefix="uc3" %>
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
    <%-- Add END --%>
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/bootstrap.min.css") %>" />
	
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/style.css") %>" />
	
	<%--  <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("" + strAssets + "/CSS/style.css") %>" /> --%>
	  
    <%-- <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/withoutmaster.css") %>" />--%>
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/Menu.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/media/css/demo_page.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/css/demo_table.css") %>" />

    <script src="../../Assets/Jscript/bootstrap.min.js"></script>
    <script type="text/javascript" src="<%# Page.ResolveClientUrl("~/Assets/Jscript/JSAsynRequest.js") %>"></script>

    <script type="text/javascript" language="javascript" src="../../Assets/Jscript/jquery.js"></script>

    <script type="text/javascript" language="javascript" src="../../Assets/Jscript/jquery.dataTables.js"></script>

    <link href="../../Assets/Beetel/CSS/dhtmlwindow.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        var baseUrl = '<%# ResolveUrl("~/") %>';
    </script>

    <link href="../../Assets/Beetel/CSS/modal.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

    <script type="text/javascript">
        function confirmation(url) {
            alert("Opening Stock entered successfully.");
            window.location = url;
            //OnClientClick="return confirm('Are you sure you want to proceed with ZERO Opening Stock?')"
        }
        function TableCount() {
            if (oTable.fnGetData().length == 0) {
                alert('There is no row to be submitted.');

            }
        }
    </script>

</head>
<body onload="changeCSSDropDown();">
    <form id="aspnetForm" runat="server">
        <asp:ScriptManager ID="Scriptmanager1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/CommonService.asmx" />
            </Services>
        </asp:ScriptManager>
        <div id="wrapper">
            <!--Wrapper Start-->
            <div id="container">
                <header>
                    <!--header Start-->
                    <div>
                        <uc1:ucHeader ID="ucHeader" runat="server" />
                    </div>
                    <div class="heading2">
                        <div class="container">
                            <div class="hd1">
                                <div id="tblHeading" runat="server">
                                    <ul>
                                        <li>
                                            <asp:Label ID="lblPageHeading" runat="server" Visible="false"></asp:Label>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="LBSwitchToBrand" runat="server" CausesValidation="False" CssClass="elink7"
                                                Text="" OnClick="LBSwitchToBrand_Click" Visible="false"></asp:LinkButton>
                                        </li>
                                    </ul>
                                </div>
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
                                            <li class="text">Select Retailer: <span class="error">*</span>
                                            </li>
                                            <li class="field">
                                                <div>
                                                    <asp:DropDownList ID="ddlRetailer" runat="server" CssClass="formselect"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlRetailer_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div>
                                                    <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlRetailer" CssClass="error"
                                                        ValidationGroup="Save" InitialValue="0" runat="server" ErrorMessage="Please select retailer."></asp:RequiredFieldValidator>
                                                </div>
                                            </li>
                                            <li class="text">
                                                <asp:Label ID="lblOSDate" runat="server" AssociatedControlID="ucDatePicker" CssClass="formtext">Opening Stock Date: <span class="error">*</span></asp:Label>
                                            </li>
                                            <li class="">
                                                <uc2:ucDatePicker ID="ucDatePicker" runat="server" IsRequired="true" ErrorMessage="Enter date." ValidationGroup="Save" />
                                            </li>
                                            <li class="ty">
                                                <asp:Button ID="btnInsert" CssClass="buttonbg modfy" runat="server" Text="Proceed with 'ZERO' Opening Stock" ValidationGroup="Save"
                                                    CausesValidation="true" OnClick="btnInsertOp_Click"  />
                                            </li>
                                            <li class="field">
                                                <asp:Label ID="lblMsg" runat="server"
                                                    Text="There is no Retailer for Opening Stock Entry." Visible="False"
                                                    ForeColor="Red"></asp:Label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div id="tblGridPanel" runat="server">
                                    <div class="mainheading">
                                        Add SKU
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
                                    </div>
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
                        Powered by <span class="ftrlogo"><img src='<%=strSiteUrl%>Assets/ZedSales/CSS/Images/footerlogo.png'></span> 
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
