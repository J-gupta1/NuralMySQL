<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="ManageOpeningStock.aspx.cs"
    Inherits="Transactions_Common_ManageOpeningStock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="Head1">
    <title>
        <%=(String)GetGlobalResourceObject("Messages", "PageTitleText")%></title>
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/Style.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/withoutmaster.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/Menu.css") %>" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="Scriptmanager1" runat="server">
    </asp:ScriptManager>
    <div id="wrapper">
        <!--Wrapper Start-->
        <div id="container">
            <div id="insideheaderarea2">
                <div class="logo">
                    <asp:HyperLink ID="hyplogo" CausesValidation="false" Width="188" Height="50" runat="server" /></div>
                <div class="headerright">
                    <div class="zedsalestrack">
                        <asp:Image runat="server" ID="ImgSideLogo" alt="Zed-Sales Track"
                            title="Zed-Sales Track" border="0" /></div>
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
            <div class="bodycontent">
                <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
                    <tr>
                        <td align="left" valign="top">
                            <table cellspacing="0" cellpadding="0" width="965" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                <tr>
                                                                    <td align="left" valign="top">
                                                                        <uc1:ucMessage ID="ucMsg" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top">
                                                    <%--    <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>--%>
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0" style="float: left;">
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td align="left" valign="top" class="tableposition">
                                                                            <div class="mainheading">
                                                                                Enter Opening Stock For Today</div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="left" class="tableposition">
                                                                <div class="contentbox">
                                                                    <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                                        <tr>
                                                                            <td colspan="6" height="20" class="mandatory" valign="top">
                                                                                (<font class="error">*</font>) marked fields are mandatory.
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="formtext" valign="top" align="right" width="15%" height="35">
                                                                                <asp:Label ID="lblOSDate" runat="server" AssociatedControlID="ucDatePicker" CssClass="formtext">Opening Stock Date:<font class="error">*</font></asp:Label>
                                                                            </td>
                                                                            <td width="20%" align="left" valign="top">
                                                                                <uc2:ucDatePicker ID="ucDatePicker" runat="server"
                                                                                    ValidationGroup="Save" />
                                                                            </td>
                                                                            <td class="formtext" valign="top" align="right" width="15%">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td valign="top" class="formtext" align="left" width="50%">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top" height="10">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="tableposition">
                                        <div class="mainheading">
                                            List</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tableposition">
                                        <div class="contentbox">
                                            <div class="grid1">
                                                <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvStockEntry" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                            CellSpacing="1" DataKeyNames="SKUID" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                                                            GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                                                            AlternatingRowStyle-CssClass="gridrow1" Width="100%" OnRowCommand="gvStockEntry_RowCommand">
                                                            <RowStyle CssClass="gridrow" />
                                                            <Columns>
                                                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUName"
                                                                    HeaderText="SKU Code"></asp:BoundField>
                                                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUCode"
                                                                    HeaderText="SKU Name"></asp:BoundField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Opening Stock"
                                                                    ShowHeader="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSKUID" runat="server" Text='<%# Eval("SKUID") %>' Visible="false"></asp:Label>
                                                                        <asp:TextBox ID="txtOpeningStock" runat="server" MaxLength="8" CssClass="form_input2"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftbeOs" runat="server" FilterMode="ValidChars" FilterType="Numbers"
                                                                            TargetControlID="txtOpeningStock" />
                                                                        <cc1:TextBoxWatermarkExtender ID="tbweQty" runat="server" TargetControlID="txtOpeningStock"
                                                                            WatermarkText="0">
                                                                        </cc1:TextBoxWatermarkExtender>
                                                                        <%-- <asp:RequiredFieldValidator ID="reqV" runat="server" ControlToValidate="txtOpeningStock"
                                                            CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Qty" ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="gridheader" />
                                                            <EditRowStyle CssClass="editrow" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                    <%--    <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />
                                            </Triggers>--%>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top" height="5" class="formtext">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" height="25" class="formtext">
                                        <asp:Button ID="btnSave" Text="Save" runat="server" ToolTip="Save" CssClass="buttonbg"
                                            OnClick="btnSave_Click" ValidationGroup="Save"></asp:Button>
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" ToolTip="Cancel" CssClass="buttonbg"
                                            OnClick="btnCancel_Click" CausesValidation="false"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top" height="5" class="formtext">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="footerarea">
            <div class="footer">
                <!--footer Start-->
                <div class="copyright">
                    &copy; Copyright 2011 Zed-Axis Technologies</div>
                <div class="footerright" style="display:none;">
                    <asp:HyperLink ID="hypfooterlogo" CausesValidation="false" runat="server" />
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
