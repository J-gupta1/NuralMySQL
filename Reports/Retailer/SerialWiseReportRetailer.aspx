<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="SerialWiseReportRetailer.aspx.cs" Inherits="Reports_Retailer_SerialWiseReportRetailer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />
    <link href="../../Assets/Beetel/CSS/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    
    <link href="../../Assets/Beetel/CSS/modal.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

    <script language="javascript" type="text/javascript">
        function popup(url) {
            var WinSalesChannelDetail = dhtmlmodal.open("ViewDetails", "iframe", "ViewSalesChannelDetail.aspx?SalesChannelId=" + url, "Sales Channel Detail", "width=500px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
            <div class="mainheading">
                View Retailer Sales Report
            </div>
            <div class="contentbox">
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Retailer Name: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlRetailerName" runat="server" CssClass="formfields"
                                    AutoPostBack="false">
                                </asp:DropDownList>                               
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="req" runat="server" CssClass="error" InitialValue="0"
                                    ControlToValidate="ddlRetailerName" ErrorMessage="Please Select Retailer."
                                    Display="Dynamic" ValidationGroup="Serach"></asp:RequiredFieldValidator>
                            </div>
                        </li>                   
                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" runat="server" ValidationGroup="Serach" CausesValidation="true"
                                    CssClass="buttonbg" Text="Search" OnClick="btnSearch_Click" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnShowAll" runat="server" CssClass="buttonbg" Text="Show All" ToolTip="Show All"
                                    OnClick="btnShowAll_Click" Visible="false" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" ToolTip="Cancel"
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
                    <asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" Visible="false" />
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="GridRetailer" runat="server" AlternatingRowStyle-CssClass="Altrow"
                            AutoGenerateColumns="false" bgcolor="" AllowPaging="true" PageSize='<%$ AppSettings:GridViewPageSize %>'
                            BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="RetailerID,ISPID"
                            FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                            GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                            RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                            OnPageIndexChanging="GridSalesChannel_PageIndexChanging" OnRowCommand="GridSalesChannel_RowCommand">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                            <Columns>
                                <asp:BoundField DataField="RetailerName" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="RetailerName">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:BoundField DataField="RetailerCode" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="Retailer Code">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:BoundField DataField="IMEI" HeaderStyle-HorizontalAlign="Left" HeaderText="IMEI"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SkuName" HeaderStyle-HorizontalAlign="Left" HeaderText="SkuName"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:BoundField DataField="CustomerName" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText="CustomerName" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ISPName" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText="ISD Name" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                            <AlternatingRowStyle CssClass="Altrow" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
