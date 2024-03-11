<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ViewPrice.aspx.cs" Inherits="Masters_HO_Admin_ViewPrice" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Search Price
    </div>
    <div class="export">
        <asp:LinkButton ID="LBViewPrice" runat="server" CausesValidation="False" CssClass="elink7"
            OnClick="LBViewPrice_Click">Add Price</asp:LinkButton>
    </div>
    <div class="contentbox">
        <div class="H20-C3-S">
            <ul>
                <li class="text">Price List Name:
                </li>
                <li class="field">
                    <asp:DropDownList ID="cmbPriceList" runat="server" CssClass="formselect">
                    </asp:DropDownList>
                </li>
                <li class="text">SKU Code:
                </li>
                <li class="field">
                    <asp:DropDownList ID="cmbSKUName" runat="server" CssClass="formselect">
                    </asp:DropDownList>
                </li>
                <li class="text">From Date:
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="Invalid date." IsRequired="true"
                        RangeErrorMessage="Date should be greater then equal to current date." ValidationGroup="Add" />
                </li>
            </ul>
            <ul>
                <li class="text">To Date:
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="Invalid date." IsRequired="true"
                        RangeErrorMessage="Date should be greater then equal to current date." ValidationGroup="Add" />
                </li>
                <li class="text"></li>
                <li class="field">
                    <div class="float-margin">
                        <asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="buttonbg"
                            OnClick="btnSearch_Click" Text="Search" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="View All Data"
                            ToolTip="Cancel" OnClick="btnCancel_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <asp:Panel ID="pnlsearch" runat="server" Visible="false">
        <div class="mainheading">
            Price List
        </div>
        <div class="export">
            <asp:Button ID="ExportToExcel" runat="server" CssClass="excel" Text="" OnClick="ExportToExcel_Click" />
        </div>
        <asp:UpdatePanel ID="UpdGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="GridPrice" runat="server" AlternatingRowStyle-CssClass="Altrow"
                            AutoGenerateColumns="false" bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                            DataKeyNames="PriceMasterID" AllowPaging="True" PageSize='<%$ AppSettings:GridViewPageSize %>'
                            FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                            GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                            RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                            OnPageIndexChanging="GridPrice_PageIndexChanging" OnRowDataBound="GridPrice_RowDataBound">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                            <Columns>
                                <asp:BoundField DataField="PriceListName" HeaderStyle-HorizontalAlign="Left" HeaderText="PriceList Name"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SKUCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Code"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="WHPrice" HeaderStyle-HorizontalAlign="Left" HeaderText="Warehouse Price"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>


                                <asp:TemplateField ShowHeader="true" HeaderText="Effective Date" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEffectiveDateCheck" runat="server" Text='<%# Eval("EffectiveDateCheck") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SDPrice" HeaderStyle-HorizontalAlign="Left" HeaderText="SD Price"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MDPrice" HeaderStyle-HorizontalAlign="Left" HeaderText="MD Price"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RetailerPrice" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Price"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MOP" HeaderStyle-HorizontalAlign="Left" HeaderText="MOP"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MRP" HeaderStyle-HorizontalAlign="Left" HeaderText="MRP"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField ShowHeader="true" HeaderText="Effective Date" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Eval("EffectiveDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="ValidTill" HeaderStyle-HorizontalAlign="Left" HeaderText="Valid Till"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnActiveDeactive" OnClick="btnActiveDeactive_Click" runat="server"
                                            CommandArgument='<%#Eval("PriceMasterID") %>' CommandName='<%#Eval("Status")%>'
                                            ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>' ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' Visible="false" />
                                        <asp:ImageButton ID="btnDelete" OnClick="btnDelete_Click" runat="server"
                                            CommandArgument='<%#Eval("PriceMasterID") %>' CommandName='<%#Eval("Status")%>'
                                            ImageUrl='<%#"~/" + strAssets + "/CSS/Images/delete.png"%>' ToolTip="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                            <AlternatingRowStyle CssClass="Altrow" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
            <%-- #CC02 START COMMENTED <Triggers>
                                                    <asp:PostBackTrigger ControlID="ExportToExcel" />
                                                </Triggers> #CC02 END COMMENTED --%>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
