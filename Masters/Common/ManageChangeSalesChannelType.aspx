<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageChangeSalesChannelType.aspx.cs" Inherits="Masters_Common_ManageChangeSalesChannelType" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/SalesEntryGrid.ascx" TagName="ucSalesEntryGrid"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        <asp:Label ID="lblStockTransfer2" Text=" Bulk Retailer Transfer" runat="server" />
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3">
            <ul>
                <li class="text">
                    <div class="hd5 margin-bottom">Transfer From: </div>
                </li>
                <li class="text">
                    <asp:Label ID="Label1" runat="server" Text="">Sales Channel Type From:<span class="error">*</span></asp:Label>
                </li>
            </ul>
            <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <ul>
                        <li class="field" style="height:auto">
                            <div>
                                <asp:DropDownList ID="ddlSalesChannelType" CssClass="formselect" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlSalesChannelType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ddlSalesChannelType"
                                    CssClass="error" ErrorMessage="Please Select a Sales Channel Type From"
                                    InitialValue="0" ValidationGroup="Mapping" />
                            </div>
                        </li>
                    </ul>
                    <div class="clear"></div>
                    <ul>
                        <li class="text">
                            <div class="hd5 margin-bottom">Transfer To: </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="Label3" runat="server" Text="">Sales Channel Type To:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field" style="height:auto">
                            <div>
                                <asp:DropDownList ID="ddlSalesChannelTypeTo" CssClass="formselect" runat="server"
                                    AutoPostBack="false">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="ddlSalesChannelTypeTo"
                                    CssClass="error" ErrorMessage="Please Select Sales Channel Type in which you want to transfer."
                                    InitialValue="0" ValidationGroup="Mapping" />
                            </div>
                        </li>
                        <li runat="server" id="tdLabel"></li>
                    </ul>
                </ContentTemplate>
            </asp:UpdatePanel>
            <ul>               
                <li class="field3">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonbg"
                        CausesValidation="True" OnClick="btnSubmit_Click" ValidationGroup="Mapping" />

                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg"
                        CausesValidation="false" OnClick="btnCancel_Click" />
                </li>
            </ul>
        </div>
    </div>
    <div class="contentbox">
        <asp:Panel ID="Pnlfrom" runat="server" Visible="false">
            <asp:UpdatePanel ID="updFrom" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="gridborder">
                        <asp:GridView ID="grdSalesChannelFrom" runat="server" FooterStyle-VerticalAlign="Top"
                            FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="None"
                            AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                            HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor="" PagerStyle-CssClass="PagerStyle"
                            BorderWidth="0px" Width="100%" AutoGenerateColumns="False" AllowPaging="True"
                            SelectedStyle-CssClass="gridselected" DataKeyNames="SalesChannelID" EmptyDataText="No record found"
                            OnPageIndexChanging="grdfromPageIndexChanging">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridrow"></RowStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="Check" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRetailerTransfer" runat="server" />
                                        <%--<asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RetailerName")%>'></asp:Label>--%>
                                        <asp:Label ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SalesChannelID")%>'
                                            Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SalesChannelName" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Name"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="SalesChannelCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel Code "
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="SalesChannelTypeName" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText="Sales Channel Type Name" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="LoginName" HeaderStyle-HorizontalAlign="Left" HeaderText="Login Name"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="OpeningStockStatus" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText="Opening Stock Status" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridheader"></HeaderStyle>
                            <AlternatingRowStyle CssClass="Altrow"></AlternatingRowStyle>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
</asp:Content>
