<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewSalesOfIMEI.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Reports_POC_ViewSalesOfIMEI" %>

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



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
            <div class="mainheading">
                View Sales Channel
            </div>
            <div class="contentbox">
                 <div class="H25-C3-S">
                    <ul>
                        <li class="text">Retailer Name:
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlRetailer" runat="server" CssClass="formselect">
                                </asp:DropDownList>                               
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="req" runat="server" CssClass="error" InitialValue="0"
                                    ControlToValidate="ddlRetailer" ErrorMessage="Please select Retailer" Display="Dynamic"
                                    ValidationGroup="Serach"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">IMEI No:
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtIMEINo" runat="server" CssClass="formfields" MaxLength="15"></asp:TextBox>
                        </li>                        
                    </ul>
                    <ul>
                        <li>
                            <asp:Panel ID="pnlBrand" runat="server" Visible="false">
                               <%-- <table width="100%" border="0" cellspacing="0" cellpadding="4">
                                    <tr>
                                        <td align="right" valign="top" class="formtext" width="42%"></td>
                                        <td align="left" valign="top" class="formtext" width="58%"></td>
                                    </tr>
                                </table>--%>
                            </asp:Panel>
                        </li>
                        <li class="field3">
                            <div class="float-margin">
                            <asp:Button ID="btnSearch" runat="server" ValidationGroup="Serach" CausesValidation="true"
                                CssClass="buttonbg" Text="Search" OnClick="btnSearch_Click" />
                            </div>
                            <div class="float-margin">
                            <asp:Button ID="btnShowAll" runat="server" CssClass="buttonbg" Text="Show All" ToolTip="Show All"
                                OnClick="btnShowAll_Click" />
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
                    <asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" />
                </div>
                <%--      <asp:UpdatePanel ID="UpdGrid" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>--%>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="GridIMEI" runat="server" AlternatingRowStyle-CssClass="Altrow"
                            AutoGenerateColumns="false" bgcolor="" AllowPaging="true" PageSize='<%$ AppSettings:GridViewPageSize %>'
                            BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="RetailerID"
                            FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                            GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                            RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                            OnPageIndexChanging="GridSalesChannel_PageIndexChanging">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                            <Columns>
                                <asp:BoundField DataField="RetailerName" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true" HeaderText="RetailerName">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RetailerCode" HeaderStyle-HorizontalAlign="Left" HeaderText="RetailerCode"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SalesDate" HeaderStyle-HorizontalAlign="Left" HeaderText="SalesDate"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ISDName" HeaderStyle-HorizontalAlign="Left" HeaderText="ISDName"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="StateName" HeaderStyle-HorizontalAlign="Left" HeaderText="StateName"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DistrictName" HeaderStyle-HorizontalAlign="Left" HeaderText="DistrictName"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CityName" HeaderStyle-HorizontalAlign="Left" HeaderText="CityName"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Model" HeaderStyle-HorizontalAlign="Left" HeaderText="Model"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="IMEI" HeaderStyle-HorizontalAlign="Left" HeaderText="IMEI"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                            <AlternatingRowStyle CssClass="Altrow" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
                <%-- </ContentTemplate>
                                                      
                                                    </asp:UpdatePanel>--%>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
