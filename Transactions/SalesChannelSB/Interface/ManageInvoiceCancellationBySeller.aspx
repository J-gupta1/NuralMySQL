<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageInvoiceCancellationBySeller.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Transactions_SalesChannelSB_Interface_ManageInvoiceCancellationBySeller" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/SalesEntryGridWithoutOrder.ascx" TagName="ucSalesEntryGrid"
    TagPrefix="uc3" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">

    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="mainheading">
        Manage Invoice Cancelled
    </div>
    <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
        <ContentTemplate>
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
                                <asp:ListItem Text="Excel" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Interface" Value="1" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </li>  
                    </ul> 
                     <div class="clear"></div> 
                    <ul>                
                        <li class="text">Status: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="formselect" Enabled="false">
                                    <asp:ListItem Value="101">Select</asp:ListItem>
                                    <asp:ListItem Value="0" Selected="True">Pending</asp:ListItem>
                                    <asp:ListItem Value="2">Received</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlStatus"
                                    CssClass="error" ValidationGroup="Ack" InitialValue="101" runat="server" ErrorMessage="Please Select Status."></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Invoice Number:
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtInvoice" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>
                            </div>
                        </li>                   
                        <li class="text">Invoice Date From:
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucFromDate" IsRequired="false" runat="server" ErrorMessage="Invalid date."
                                defaultDateRange="false" />
                        </li>
                        <li class="text">Invoice Date To:
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucToDate" IsRequired="false" runat="server" ErrorMessage="Invalid date."
                                defaultDateRange="false" />
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg" CausesValidation="true"
                                OnClick="btnSearch_Click" ValidationGroup="Ack" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="false"
                                OnClick="btnCancel_Click" />
                        </li>
                    </ul>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                <div id="tblGrid">
                    <div class="mainheading">
                        List                                
                    </div>
                    <div class="contentbox">
                        <div class="grid1">
                            <asp:GridView ID="gvAck" runat="server" Width="100%" AutoGenerateColumns="false"
                                DataKeyNames="SalesUniqueID" CssClass="" BorderWidth="1px" CellPadding="4"
                                AllowPaging="true" PageSize='<%$ AppSettings:GridViewPageSize %>' CellSpacing="1"
                                HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow" OnRowCommand="gvAck_RowCommand"
                                AlternatingRowStyle-CssClass="Altrow" GridLines="None" FooterStyle-CssClass="gridfooter"
                                HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="left" RowStyle-VerticalAlign="Top"
                                OnPageIndexChanging="gvAck_PageIndexChanging" RowStyle-HorizontalAlign="left"
                                FooterStyle-HorizontalAlign="left" FooterStyle-VerticalAlign="Top" PagerStyle-CssClass="PagerStyle">
                                <Columns>
                                    <asp:BoundField DataField="SalesFrom" HeaderText="Sales From" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="InvoiceNumber" HeaderText="Invoice Number" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="invoiceDateToDisplay" HeaderText="Invoice Date" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TotalQuantity" HeaderText="Total Quantity" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReceived" runat="server" Text='<%# Bind("IsReceived") %>' Visible="false"></asp:Label>
                                            <asp:Button ID="btnDetails" CausesValidation="false" runat="server" CssClass="buttonbg"
                                                Text="Details" CommandName="Details" CommandArgument='<%# Eval("SalesUniqueID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upddetails" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <asp:Panel ID="pnldetail" runat="server" Visible="false">
                <div id="Table1">
                    <div class="mainheading">
                        Detail List
                    </div>
                    <div class="contentbox">
                        <%--OnDetailRowGetButtonVisibility="grdDetails_DetailRowGetButtonVisibility"--%>
                        <dxwgv:ASPxGridView ID="grdDetails" ClientInstanceName="grdvList" runat="server"
                            KeyFieldName="DetailID" Width="100%" OnDetailRowExpandedChanged="grdDetails_DetailRowExpandedChanged"
                            OnDetailRowGetButtonVisibility="grdDetails_DetailRowGetButtonVisibility" Styles-AlternatingRow-BackColor=""
                            Styles-Header-HorizontalAlign="Left" Styles-Header-Font-Bold="true" Styles-Header-ForeColor="White"
                            Styles-Header-CssClass="gridheader">
                            <Columns>
                                <dxwgv:GridViewDataColumn FieldName="SKUName" VisibleIndex="0" Caption="SKU Name" />
                                <dxwgv:GridViewDataColumn FieldName="Quantity" VisibleIndex="1" Caption="Quantity" />
                                <dxwgv:GridViewDataColumn FieldName="Mode" VisibleIndex="2" Caption="Mode" />
                                <dxwgv:GridViewDataColumn FieldName="Amount" VisibleIndex="3" Caption="Amount" />
                                <dxwgv:GridViewDataColumn FieldName="SerialisedMode" VisibleIndex="4" Caption="SerialisedMode"
                                    Visible="false" />
                            </Columns>
                            <Templates>
                                <DetailRow>
                                    <dxwgv:ASPxGridView ID="detailGrid" runat="server" KeyFieldName="Skuinfo" Width="100%"
                                        OnBeforePerformDataSelect="detailGrid_DataSelect" EnableCallBacks="False" SettingsBehavior-AllowSort="false"
                                        Styles-Header-HorizontalAlign="Left" Styles-Header-Font-Bold="true" Styles-Header-ForeColor="White"
                                        Styles-Header-CssClass="gridheader">
                                        <Columns>
                                            <dxwgv:GridViewDataTextColumn FieldName="Skuinfo" ReadOnly="True" VisibleIndex="0">
                                            </dxwgv:GridViewDataTextColumn>
                                        </Columns>
                                        <Settings ShowFooter="True" />
                                        <SettingsDetail ShowDetailRow="False" IsDetailGrid="True" />
                                        <SettingsBehavior AllowFocusedRow="True" ProcessSelectionChangedOnServer="False" />
                                    </dxwgv:ASPxGridView>
                                </DetailRow>
                            </Templates>
                            <SettingsDetail ShowDetailRow="true" />
                        </dxwgv:ASPxGridView>
                    </div>
                    <div class="margin-bottom">
                        <asp:Button ID="btnReject" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="false"
                            OnClick="btnReject_Click" />
                    </div>
                </div>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
