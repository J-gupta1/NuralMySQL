<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageOpeningStockSerialWise.aspx.cs"
    Inherits="Transactions_CommanSerial_ManageOpeningStockSerialWise" %>

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
    <%-- Add END --%>
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/bootstrap.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/style.css") %>" />
    <%--<link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/withoutmaster.css") %>" />--%>
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/Menu.css") %>" />
    <script src="../../Assets/Jscript/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="Scriptmanager1" runat="server">
        </asp:ScriptManager>
        <div id="wrapper">
            <!--Wrapper Start-->
            <div id="container">
                <header>
                    <div class="container">
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
                                        Welcome
                            <asp:Label ID="lblUserNameDesc" CssClass="logintime" Visible="true" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3 col-sm-3 col-xs-4">
                                <div class="zedsalestrack">
                                    <div class="right-header">
                                        <%--<asp:Image runat="server" ID="ImgSideLogo" alt="Zed-Sales Track" title="Zed-Sales Track"
                                        border="0" />--%>
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
                                <uc1:ucMessage ID="ucMsg" runat="server" />
                                <%--  #CC02 COMMENTED <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <div>--%>
                                <uc1:ucMessage ID="ucMessage1" runat="server" />
                                <%-- #CC02 COMMENTED </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                <div class="msg">
                                    <strong>Important Note</strong> - Please note that if any of your Purchases are already uploaded in the system, 
                                                        the stock against them does not get added in your stock.
                                                        You will need to request for stock adjustment to add stock for those invoices.
                                                        For example - If you are entering opening stock for
                                            <asp:Label ID="lblInfoDate" runat="server" Text=""></asp:Label>
                                    and your purchases for that date or later are already uploaded, the stock for those invoices will not add to your stock.
                                </div>
                                <div class="mainheading">
                                    Enter Opening Stock
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
                                                    <asp:ListItem Text="Excel" Value="0" Selected="True"></asp:ListItem>
                                                    <%--==========#CC03 Commented=============--%>
                                                    <%-- <asp:ListItem Text="Interface" Value="1"></asp:ListItem>--%>
                                                    <%--==========#CC03 Commented=============--%>
                                                </asp:RadioButtonList>
                                            </li>
                                            <li class="field3">
                                                <asp:Button ID="btnInsert" CssClass="buttonbg" runat="server" Text="Proceed With Zero Quantity"
                                                    ValidationGroup="Save" CausesValidation="true" OnClick="btnInsert_Click" />
                                            </li>
                                        </ul>
                                        <div class="clear"></div>
                                        <ul>
                                            <li class="text">
                                                <asp:Label ID="lblOSDate" runat="server" AssociatedControlID="ucDatePicker" CssClass="formtext">Opening Stock Date:<span class="error">*</span></asp:Label>
                                            </li>
                                            <li class="field">
                                                <uc2:ucDatePicker ID="ucDatePicker" runat="server" RangeErrorMessage="Date should be less then equal to current date."
                                                    ValidationGroup="Save" ErrorMessage="Invalid Date!" IsRequired="true" />
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="text">Upload File: <span class="error">*</span>
                                            </li>
                                            <li class="field">
                                                <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                                            </li>
                                            <li class="field3">
                                                <div>
                                                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                                                        CausesValidation="false" OnClick="btnUpload_Click" />
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="formlink">
                                        <ul>
                                            <li>
                                                <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                                                    CausesValidation="false" CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>
                                            </li>
                                            <%--<li>
                                                <a class="elink2" href="../../Excel/Templates/OpeningStockSerialWiseWithValidation.xlsx"
                                                    runat="server" id="hyWithValidation" visible="false">Download Template</a>
                                            </li>--%>
                                            
                                            <li>
                                                <a class="elink2" href="../../Excel/Templates/OpeningStockSerialWise.xlsx"
                                                    runat="server" id="hyWithOutValidation" visible="true">Download Template</a>
                                            </li>
                                            <li>
                                                <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                                            </li>
                                            <li>
                                                <asp:HyperLink ID="hlnkDuplicateNotinUse" runat="server" CssClass="elink3"></asp:HyperLink>
                                            </li>
                                            <li>
                                                <asp:HyperLink ID="hlnkBlankNotinUse" runat="server" CssClass="elink3"></asp:HyperLink>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <asp:Panel ID="pnlGrid" runat="server">
                                    <%--  #CC02 COMMENTED <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                    <div id="tblGrid">
                                        <div class="mainheading">
                                            Enter Opening Stock Details
                                        </div>
                                        <div class="float-right" style="width: 240px;">
                                            <div class="float-margin">
                                                <asp:Label ID="lblTotal" runat="server" Visible="false" Text="" Font-Bold="true"></asp:Label>
                                            </div>
                                            <%--<div class="float-margin">
                                                <asp:Button ID="btnSave1" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="Save"
                                                    CausesValidation="true" OnClick="btnSave_Click" />
                                            </div>--%>
                                            <div class="float-left">
                                                <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="false"
                                                    OnClick="btnReset_Click" />
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <div class="contentbox">
                                            <div class="grid1">
                                                
                                                <asp:GridView ID="gvStockEntry" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                    CellSpacing="1" DataKeyNames="SKUCode" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                                                    GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                                                    AlternatingRowStyle-CssClass="Altrow" Width="100%">
                                                    <RowStyle CssClass="gridrow" />
                                                    <Columns>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUCode"
                                                            HeaderText="SKU Code"></asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Quantity"
                                                            HeaderText="Quantity"></asp:BoundField>
                                                        <%--   <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Serial#1"
                                                                        HeaderText="Serial Number1"></asp:BoundField>
                                                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Serial#2"
                                                                        HeaderText="Serial Number2"></asp:BoundField>
                                                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Serial#3"
                                                                        HeaderText="Serial Number3"></asp:BoundField>
                                                                          <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Serial#4"
                                                                        HeaderText="Serial Number4"></asp:BoundField>
                                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BatchNo"
                                                                        HeaderText="BatchNo"></asp:BoundField>
                                                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Error"
                                                                        HeaderText="Error"></asp:BoundField>--%>
                                                    </Columns>
                                                    <HeaderStyle CssClass="gridheader" />
                                                    <EditRowStyle CssClass="editrow" />
                                                </asp:GridView>
                                                <%--       </contenttemplate>
                                                    </asp:UpdatePanel>--%>
                                            </div>
                                        </div>
                                        <div class="margin-bottom">
                                            <%--<asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="Save"
                                                CausesValidation="true" OnClick="btnSave_Click" Visible="false" />
                                            <asp:Button ID="btnReset" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="false"
                                                OnClick="btnReset_Click" Visible="false" />--%>
                                        </div>
                                    </div>
                                    <%-- #CC02 COMMENTED </ContentTemplate>
                                </asp:UpdatePanel>--%>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </article>
            </div>
            <footer>
                <div class="container">
                    <!--footer Start-->
                    <div class="copyright">
                        &copy; Copyright 2018 Zed-Axis Technologies
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
