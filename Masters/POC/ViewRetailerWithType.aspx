<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ViewRetailerWithType.aspx.cs" Inherits="Masters_HO_POC_ViewRetailerWithType" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />
    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>
    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>
    <script language="javascript" type="text/javascript">
        function popup(url) {
            var WinRetailerDetails = dhtmlmodal.open("ViewDetails", "iframe", "ViewRetailerDetailWithType.aspx?RetailerId=" + url, "Retailer Detail", "width=500px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
        }
    </script>

    <%-- <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/JsValidate.js"></script>--%>

    <%-- <script type="text/javascript" language="JavaScript">
		function popup(url)
		{
		    window.open(url, "ViewDetails", "width=600,height=500,scrollbars = 1,menubar=no,toolbar=no,top=100,left=200,screenX=200,screenY=200")
			
		}
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
            <div class="mainheading">
                Search Retailer
            </div>
            <div class="export">
                <asp:LinkButton ID="LBAddRetailer" runat="server" CausesValidation="False" OnClick="LBAddRetailer_Click"
                    CssClass="elink7">Add Retailer</asp:LinkButton>
            </div>
            <div class="contentbox">
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Sales Channel:
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbsaleschannel" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                        <li class="text">Retailer Name:
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtRetailername" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>

                        </li>
                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="buttonbg"
                                    Text="Search" OnClick="btnSearch_Click" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnShowAll" runat="server" CssClass="buttonbg"
                                    Text="Show All" OnClick="btnShowAll_Click" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg"
                                    Text="Cancel" ToolTip="Cancel"
                                    OnClick="btnCancel_Click" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <div id="dvhide" runat="server" visible="false">
                <div class="mainheading">
                    List
                </div>
                <div class="export">
                    <asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" />
                </div>
                <%--   <asp:UpdatePanel ID="UpdGrid" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>--%>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="GridRetailer" runat="server" AlternatingRowStyle-CssClass="Altrow"
                            AutoGenerateColumns="false" bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                            DataKeyNames="RetailerID" PageSize='<%$ AppSettings:GridViewPageSize %>' FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left"
                            FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-CssClass="gridheader"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow"
                            RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected"
                            Width="100%" OnPageIndexChanging="GridRetailer_PageIndexChanging" OnRowCommand="GridRetailer_RowCommand"
                            OnRowDataBound="GridRetailer_RowDataBound" AllowPaging="True">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                            <Columns>
                                <asp:BoundField DataField="RetailerTypeName" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Type"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RetailerName" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Name"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RetailerCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Code"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SalesChannelName" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Name"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SalesmanName" HeaderStyle-HorizontalAlign="Left" HeaderText="Salesman Name"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Email" HeaderStyle-HorizontalAlign="Left" HeaderText="Email"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MobileNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="Mobile"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="View Details">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HLDetails" runat="server" CssClass="buttonbg"></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgActive" runat="server" CausesValidation="false" CommandArgument='<%#Eval("RetailerID") %>'
                                            CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("RetailerID") %>'
                                            CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                            <AlternatingRowStyle CssClass="Altrow" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
                <%--</ContentTemplate>
                                                                <Triggers>
                                                                    <asp:PostBackTrigger ControlID="ExportToExcel" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>--%>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
