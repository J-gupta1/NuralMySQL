﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ViewRetailer.aspx.cs" Inherits="Masters_HO_Retailer_ViewRetailer" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucServiceEntity.ascx" TagName="ucServiceEntity"
    TagPrefix="uc2" %>
<%--#CC06 ADDED--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

    <script language="javascript" type="text/javascript">
        function popup(url) {
            var WinRetailerDetails = dhtmlmodal.open("ViewDetails", "iframe", "ViewRetailerDetail.aspx?RetailerId=" + url, "Retailer Detail", "width=500px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
        }

        //#CC03 added
        function showallclick() {
            $("[id$=cmbsaleschannel]").val("0");


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
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Search Retailer
    </div>
    <div class="export" id="divAddRetailer" runat ="server" visible="false">
        <asp:LinkButton ID="LBAddRetailer" runat="server" CausesValidation="False" OnClick="LBAddRetailer_Click"
            CssClass="elink7">Add Retailer</asp:LinkButton>
    </div>
    <div class="contentbox">
        <div class="H25-C3-S">
            <ul>
                <li class="text">Sales Channel:
                </li>
                <li class="field">

                    <%--#CC06 COMMENTED   <asp:DropDownList ID="cmbsaleschannel" runat="server" CssClass="formselect" EnableViewState="false">
                                                                    </asp:DropDownList>--%>
                    <asp:UpdatePanel ID="updateSales" runat="server">
                        <ContentTemplate>
                            <uc2:ucServiceEntity ID="ucServiceEntity" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--#CC06 ADDED--%>

                </li>
                <li class="text">Retailer Name:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtRetailername" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>
                </li>

                <%-- #CC07 ADD Start --%>
                <li class="text">Retailer Code:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtRetailerCode" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                </li>
                <li class="text">Status: <span class="error">&nbsp;</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="formselect">
                        <asp:ListItem Text="All" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                        <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </li>

                <%--#CC14 Add Start--%>
                <li class="text">
                    <asp:Label ID="lblStateHeading" runat="server" Text="State:"></asp:Label>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlState" runat="server" CssClass="formselect">
                    </asp:DropDownList></li>
                <li class="text">
                    <asp:Label ID="lblNDHeading" runat="server" Text="ND:"></asp:Label>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlND" runat="server" CssClass="formselect">
                    </asp:DropDownList>

                </li>
                <li class="text">SalesChannel Code:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtSalesChannelCode" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                </li>
                <li class="text">Brand:
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlBrand" runat="server" CssClass="formselect">
                    </asp:DropDownList>
                </li>
                <li class="text">Product Category:
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlproductcategory" runat="server" CssClass="formselect">
                    </asp:DropDownList>
                </li>
                </ul>
             <div class="setbbb">
                    <%-- #CC07 ADD End --%>

                    <div class="float-margin">
                        <asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="buttonbg"
                            Text="Search" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-margin">
                        <%--<asp:Button ID="btnShowAll" OnClientClick="javascript:showallclick()" runat="server"
                                                                            CssClass="buttonbg" Text="Show All" OnClick="btnShowAll_Click" />&nbsp;&nbsp; #CC14 Commented --%>
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" ToolTip="Cancel"
                            OnClick="btnCancel_Click" />
                    </div>

                </div>
                <ul>
                <li class="text-field">
                    <div class="float-left">
                        <asp:Image ID="imgDownloadMappingInfo" runat="server" AlternateText="Download Retailer Mapping Info" />
                    </div>                    
                    <div class="float-margin" style="padding-top: 3px; padding-left: 3px">
                        <asp:LinkButton ID="DwnldRetailerMappingInfo" runat="server" Text="Download Retailer Mapping Info"
                            CssClass="elink2" Style="color: green" OnClick="DwnldRetailerMappingInfo_Click"></asp:LinkButton>
                    </div>
                    <div class="float-left">
                        <asp:Image ID="imgRetailerDetail" runat="server" AlternateText="Download Retailer Detail" />
                    </div>
                    <div class="float-margin" style="padding-top: 3px; padding-left: 3px">
                        <asp:LinkButton ID="ExportToExcel" runat="server" Text="Download Retailer Detail"
                            CssClass="elink2" Style="color: green" OnClick="ExportToExcel_Click"></asp:LinkButton>
                    </div>
                </li>
                <%--#CC14 Add End--%>
                
                <%--</tr> #CC14 Commented --%>

            </ul>
            <%--#CC14 Added --%>
        </div>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="dvhide" runat="server" visible="false">
                    <div class="mainheading">
                        List
                    </div>

                    <%--#CC14 Commnet Start
                                            <td width="20%" align="right">
                                            <asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" />
                                        </td> #CC14 Commnet End--%>

                    <div class="contentbox">
                        <div class="grid1">
                            <asp:GridView ID="GridRetailer" runat="server" AlternatingRowStyle-CssClass="Altrow"
                                AutoGenerateColumns="false" bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                                DataKeyNames="RetailerID" PageSize='<%$ AppSettings:GridViewPageSize %>' FooterStyle-CssClass="gridfooter"
                                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                                RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                                SelectedStyle-CssClass="gridselected" Width="100%" OnRowCommand="GridRetailer_RowCommand"
                                OnRowDataBound="GridRetailer_RowDataBound" AllowPaging="false">
                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                <Columns>
                                    <asp:BoundField DataField="RetailerCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Code"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RetailerName" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Name"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ParentRetailerName" HeaderStyle-HorizontalAlign="Left"
                                        HeaderText="Parent Retailer Name" HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="StateName" HeaderStyle-HorizontalAlign="Left" HeaderText="State"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CityName" HeaderStyle-HorizontalAlign="Left" HeaderText="City"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LoginName" HeaderStyle-HorizontalAlign="Left" HeaderText="Login ID"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Password">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPassword" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"Password"))%>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblPasswordSalt" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"PasswordSalt"))%>'
                                                Visible="false"></asp:Label>
                                            <asp:LinkButton ID="hlPassword" runat="server" Text="Password" Visible="false"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--#CC11 Add Start--%>
                                    <asp:TemplateField HeaderText="ND Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNDName" runat="server" Text='<%#Eval("ND")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--#CC11 Add End--%>
                                    <asp:BoundField DataField="SalesChannelName" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Name"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SalesChannelCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Code"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <%-- /*#CC23 Added Started*/--%>
                                    <asp:BoundField DataField="SalesmanName" HeaderStyle-HorizontalAlign="Left" HeaderText="Salesman Name"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Salesmanmappingstatus" HeaderStyle-HorizontalAlign="Left" HeaderText="Salesman Mapping Status"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RDSMappingstatus" HeaderStyle-HorizontalAlign="Left" HeaderText="SalesChannel Mapping Status"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <%-- /*#CC23 Added End*/--%>
                                    <%--<asp:BoundField DataField="LocationName" HeaderStyle-HorizontalAlign="Left" HeaderText="Location Name"
                                                                                    HtmlEncode="true">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:BoundField>--%>
                                    <%--<asp:BoundField DataField="SalesmanName" HeaderStyle-HorizontalAlign="Left" HeaderText="Salesman Name"
                                                                                    HtmlEncode="true">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:BoundField>--%>
                                    <%--<asp:BoundField DataField="Email" HeaderStyle-HorizontalAlign="Left" HeaderText="Email"
                                                                                    HtmlEncode="true">
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:BoundField>--%>
                                    <asp:BoundField DataField="MobileNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="Mobile"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="View Details">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HLDetails" runat="server" CssClass="buttonbg"></asp:HyperLink>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblParentSalesChannelId" runat="server" Text='<%#Eval("SalesChannelID")%>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                        <ItemStyle Wrap="False" />
                                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                        <ItemTemplate>

                                            <%--  #CC18 Added Start--%>
                                            <%--  #CC11 Comment Start--%>
                                            <%-- <asp:ImageButton ID="imgActive" runat="server" CausesValidation="false" CommandArgument='<%#Eval("RetailerID") %>'
                                                            CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />--%>
                                            <%-- #CC11 Comment End--%>
                                            <%--  #CC18 Added End--%>

                                            <%--#CC11 Add Start--%>
                                            <%--#CC18 Add Commented--%>
                                            <cc2:ZedImageButton ID="imgActive" runat="server" CausesValidation="false" CommandArgument='<%#Eval("RetailerID") %>'
                                                CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                                ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' AlternateText="Active" ActionTag="Active" />
                                            <%--#CC11 Add Start--%>
                                            <%--#CC18 Commented End--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px" Visible="false">
                                        <ItemStyle Wrap="False" />
                                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                        <ItemTemplate>
                                            
                                            <cc2:ZedImageButton ID="img1" runat="server" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>'
                                                ToolTip="(Edit)" CommandName="cmdEdit" CommandArgument='<%#Eval("RetailerID") %>'
                                                CausesValidation="False" AlternateText="Edit" ActionTag="Edit" />
                                           
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--/*#CC20 start*/--%>
                                    <asp:TemplateField HeaderText="Attachment">
                                        <ItemTemplate>
                                            <asp:GridView ID="gvAttachedImages" runat="server" AutoGenerateColumns="false" BorderWidth="0px"
                                                ShowHeader="false" CellPadding="0" GridLines="None" CssClass="table-panel" HeaderStyle-VerticalAlign="top"
                                                HeaderStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                                                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                                                DataKeyNames="Attachement">
                                                <FooterStyle HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                                <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblImagePath" runat="server" Text='<%# Eval("Attachement") %>'
                                                                Style="display: none"></asp:Label>
                                                            <asp:LinkButton ID="lnkDownload" Text="View Attachment" CssClass="elink2" CommandArgument='<%# Eval("Attachement") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <AlternatingRowStyle HorizontalAlign="Left" VerticalAlign="Top"></AlternatingRowStyle>
                                            </asp:GridView>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--/*#CC20 start*/--%>

                                </Columns>
                                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                <AlternatingRowStyle CssClass="Altrow" />
                                <PagerStyle CssClass="PagerStyle" />
                            </asp:GridView>
                            <div class="clear">
                            </div>
                        </div>
                        <div id="dvFooter" runat="server" class="pagination">
                            <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnCancel" />
                <%--<asp:AsyncPostBackTrigger ControlID="btnSearch" />--%> <%--#CC09 Commented--%>
                <asp:PostBackTrigger ControlID="btnSearch" />
                <%-- #CC09 Added --%>
                <%--   <asp:AsyncPostBackTrigger ControlID="btnShowAll" /> #CC14 Commented--%>
                <asp:PostBackTrigger ControlID="ExportToExcel" />
                <asp:PostBackTrigger ControlID="GridRetailer" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

</asp:Content>