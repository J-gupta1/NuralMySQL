<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewSchemePerformance.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Masters_Common_ViewSchemePerformance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

    <script type="text/javascript" language="JavaScript">

        function popup(url) {
            var WinDetails = dhtmlmodal.open("ViewDetails", "iframe", "ViewTargetDetail.aspx?TargetID=" + url, "Target Detail", "width=500px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div align="center">
        <asp:UpdatePanel ID="UpdMain" runat="server">
            <ContentTemplate>
                <div>
                    <table cellspacing="0" cellpadding="0" width="965" border="0">
                        <tr>
                            <td align="left" valign="top">
                                <table cellspacing="0" cellpadding="0" width="965" border="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <uc4:ucMessage ID="ucMessage1" runat="server" ShowCloseButton="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <div class="mainheading_rpt">
                                                <div class="mainheading_rpt_left">
                                                </div>
                                                <div class="mainheading_rpt_mid">
                                                    View Scheme Performance</div>
                                                <div class="mainheading_rpt_right">
                                                </div>
                                            </div>
                                            <div style="float: right; margin-right: 10px;">
                                                <asp:Button ID="btnCalculateSchemePerformance" runat="server" Text="CalculateSchemePerformance"
                                                    CssClass="buttonbg" CausesValidation="false" OnClick="btnCalculateSchemePerformance_Click" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <div class="contentbox">
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td class="mandatory" colspan="6" height="20" valign="top" align="left">
                                                            (<font class="error">+</font>) marked fields are Optional.
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formtext" width="20%" valign="top" align="right" height="35">
                                                            <asp:Label ID="Label1" CssClass="formtext" runat="server" AssociatedControlID="ddlSaleschannelType">Select Sales Channel Type:<font class="error">+</font></asp:Label>
                                                        </td>
                                                        <td align="left" valign="top" width="10%">
                                                            <div style="width: 150px;">
                                                                <asp:DropDownList CausesValidation="true" ID="ddlSaleschannelType" runat="server"
                                                                    CssClass="form_select4" AutoPostBack="True" OnSelectedIndexChanged="ddlSaleschannelType_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div>
                                                                <asp:Label Style="display: none;" runat="server" ID="Label2" CssClass="error"></asp:Label>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlSaleschannelType"
                                                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select hierarchy level."
                                                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </td>
                                                        <td align="right" valign="top" class="formtext">
                                                            Select SalesChannel:<font class="error">+</font>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <div style="width: 135px;">
                                                                <asp:DropDownList ID="ddlSalesChannel" runat="server" CssClass="form_select" Enabled="false">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <%--    <div>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="Search"
                                                                            runat="server" Display="Dynamic" InitialValue="0" ControlToValidate="ddlSalesChannel"
                                                                            CssClass="error" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                    </div>--%>
                                                        </td>
                                                        <td align="right" valign="top" class="formtext" width="10%">
                                                            Status:&nbsp;
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <div style="float: left; width: 135px;">
                                                                <asp:DropDownList ID="ddlSchemeStatus" runat="server" CssClass="form_select">
                                                                    <asp:ListItem Selected="True" Value="2" Text="All"></asp:ListItem>
                                                                    <asp:ListItem Value="1" Text="Active"></asp:ListItem>
                                                                    <asp:ListItem Value="0" Text="Inactive"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 30px;">
                                                        <td height="35" width="20%" align="right" valign="top" class="formtext">
                                                            Select Scheme:<font class="error">+</font>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <div>
                                                                <asp:DropDownList ID="ddlScheme" runat="server" AutoPostBack="false" CssClass="form_select">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <%--  <div style="width: 150px;">
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Search"
                                                                            runat="server" Display="Dynamic" InitialValue="0" ControlToValidate="ddlScheme"
                                                                            CssClass="error" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                    </div>--%>
                                                        </td>
                                                        <td class="formtext" valign="top" align="right">
                                                            Date From:<font class="error">+</font>
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <uc1:ucDatePicker ID="ucDatePicker1" ErrorMessage="Invalid from date." ValidationGroup="Search"
                                                                runat="server" IsRequired="false" />
                                                        </td>
                                                        <td class="formtext" valign="top" align="right">
                                                            To:<font class="error">+</font>
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <uc1:ucDatePicker ID="ucDatePicker2" ErrorMessage="Invalid to date." ValidationGroup="Search"
                                                                runat="server" IsRequired="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top">
                                                            &nbsp;
                                                        </td>
                                                        <td align="left" colspan="5" valign="top">
                                                            <div style="float: left; margin-right: 10px;">
                                                                <asp:Button ID="Search" runat="server" Text="&nbsp;Search&nbsp;" CssClass="buttonbg"
                                                                    CausesValidation="false" ValidationGroup="Search" OnClick="Search_Click" />
                                                            </div>
                                                            <div>
                                                                <asp:Button ID="Cancel" runat="server" Text="&nbsp;Cancel&nbsp;" CssClass="buttonbg"
                                                                    CausesValidation="false" OnClick="Cancel_Click" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="10">
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <asp:Panel ID="dvhide" runat="server" Visible="true">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin-top: 10px;">
                                        <tr>
                                            <td width="90%" align="left">
                                                <div class="mainheading_rpt">
                                                    <div class="mainheading_rpt_left">
                                                    </div>
                                                    <div class="mainheading_rpt_mid">
                                                        List</div>
                                                    <div class="mainheading_rpt_right">
                                                    </div>
                                                </div>
                                            </td>
                                            <td width="10%" align="right">
                                                <asp:Button ID="btnExport" runat="server" CssClass="excel" Text="" OnClick="btnExport_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <div class="contentbox">
                                                    <div class="grid1">
                                                        <asp:GridView ID="GridScheme" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="gridrow1"
                                                            bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="None"
                                                            DataKeyNames="SchemePerformanceCalculationID" HeaderStyle-CssClass="gridheader"
                                                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow"
                                                            RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected"
                                                            Width="100%" AutoGenerateColumns="false" OnPageIndexChanging="GridScheme_PageIndexChanging">
                                                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                                            <PagerStyle CssClass="gridfooter" />
                                                            <Columns>
                                                                <asp:BoundField DataField="SalesChannelName" HeaderStyle-HorizontalAlign="Left" HeaderText="SalesChannelName"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SalesChannelCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SalesChannelCode"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SchemeName" HeaderStyle-HorizontalAlign="Left" HeaderText="SchemeName"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="TargetBasedOn" HeaderStyle-HorizontalAlign="Left" HeaderText="TargetBasedOn"
                                                                    HtmlEncode="true" Visible="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Target" HeaderStyle-HorizontalAlign="Left" HeaderText="Target"
                                                                    HtmlEncode="true" Visible="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Achievement" HeaderStyle-HorizontalAlign="Left" HeaderText="Achievement"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ExcludedAchievement" HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderText="ExcludedAchievement" HtmlEncode="true" Visible="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PayOutCalculatedOn" HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderText="PayOutCalculatedOn" HtmlEncode="true" Visible="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Payout" HeaderStyle-HorizontalAlign="Left" HeaderText="Payout"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PayoutRate" HeaderStyle-HorizontalAlign="Left" HeaderText="Payout Rate"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Gift" HeaderStyle-HorizontalAlign="Left" HeaderText="Gift"
                                                                    HtmlEncode="true" Visible="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SchemeStatus" HeaderStyle-HorizontalAlign="Left" HeaderText="SchemeStatus"
                                                                    HtmlEncode="true" Visible="false">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--   <asp:TemplateField HeaderText="View Details">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="HLDetails" runat="server" CssClass="buttonbg"></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="85px">
                                                                    <ItemStyle Wrap="False" />
                                                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="img2" runat="server" CausesValidation="false" OnClientClick="return ConfirmDelete();"
                                                                            CommandArgument='<%#Eval("TargetID") %>' CommandName="cmdDelete" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/delete.png"%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" height="10">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
                <asp:PostBackTrigger ControlID="Search" />
                <asp:PostBackTrigger ControlID="btnCalculateSchemePerformance" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
