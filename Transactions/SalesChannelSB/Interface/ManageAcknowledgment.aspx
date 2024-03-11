<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageAcknowledgment.aspx.cs" Inherits="Transactions_SalesChannelSB_Interface_ManageSalesAcknowledge" %>

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
        Manage Sales Acknowlege
    </div>
    <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="contentbox">
                <div class="mandatory">
                    (*) Marked fields are mandatory
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Status: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="formselect">
                                    <asp:ListItem Value="101" Selected="True">Select</asp:ListItem>
                                    <asp:ListItem Value="0">Pending</asp:ListItem>
                                    <asp:ListItem Value="1">Auto Received</asp:ListItem>
                                    <asp:ListItem Value="2">Received</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlStatus" Display="Dynamic"
                                    CssClass="error" ValidationGroup="Ack" InitialValue="101" runat="server" ErrorMessage="Please Select Status."></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Invoice Number:
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtInvoice" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>
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
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg" CausesValidation="true"
                                    OnClick="btnSearch_Click" ValidationGroup="Ack" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="false"
                                    OnClick="btnCancel_Click" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                <table id="tblGrid" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="mainheading">
                                List
                            </div>
                            <div class="float-right">
                                <asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" />
                            </div>
                            <div class="clear"></div>
                            <div class="contentbox">
                                <div class="grid1">
                                    <asp:GridView ID="gvAck" runat="server" Width="100%" AutoGenerateColumns="false"
                                        DataKeyNames="SalesUniqueID" CssClass="" BorderWidth="0px" CellPadding="4" AllowPaging="true"
                                        PageSize='<%$ AppSettings:GridViewPageSize %>' CellSpacing="1" HeaderStyle-CssClass="gridheader"
                                        RowStyle-CssClass="gridrow" OnRowCommand="gvAck_RowCommand" AlternatingRowStyle-CssClass="Altrow"
                                        GridLines="none" FooterStyle-CssClass="gridfooter" HeaderStyle-VerticalAlign="Middle"
                                        HeaderStyle-HorizontalAlign="left" RowStyle-VerticalAlign="Top" OnPageIndexChanging="gvAck_PageIndexChanging"
                                        RowStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="left" FooterStyle-VerticalAlign="Top">
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
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnIMEIAck" Value='<%#Eval("IMEIAck") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upddetails" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnldetail" runat="server" Visible="false">
                <table id="Table1" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="mainheading">
                                Detail List
                            </div>
                            <div class="contentbox">
                                <%--OnDetailRowGetButtonVisibility="grdDetails_DetailRowGetButtonVisibility"--%>
                                <dxwgv:ASPxGridView ID="grdDetails" ClientInstanceName="grdvList" runat="server"
                                    KeyFieldName="DetailID" Width="100%" OnDetailRowExpandedChanged="grdDetails_DetailRowExpandedChanged"
                                    OnDetailRowGetButtonVisibility="grdDetails_DetailRowGetButtonVisibility" Styles-AlternatingRow-CssClass="Altrow" Styles-AlternatingRow-BackColor=""
                                    Styles-Header-HorizontalAlign="Left" Styles-Header-Font-Bold="true" Styles-Header-ForeColor="White" Styles-Header-CssClass="gridheader"
                                    OnDataBound="grdDetails_DataBound">
                                    <Columns>
                                        <dxwgv:GridViewDataColumn FieldName="SKUName" VisibleIndex="0" Caption="SKU Name" />
                                        <dxwgv:GridViewDataColumn FieldName="BatchCode" VisibleIndex="1" Caption="Batch Code" />
                                        <dxwgv:GridViewDataColumn FieldName="Quantity" VisibleIndex="2" Caption="Quantity" />
                                        <dxwgv:GridViewDataColumn FieldName="Mode" VisibleIndex="3" Caption="Mode" />
                                        <dxwgv:GridViewDataColumn FieldName="Amount" VisibleIndex="4" Caption="Amount" />
                                        <dxwgv:GridViewDataColumn FieldName="SerialisedMode" VisibleIndex="5" Caption="SerialisedMode"
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
                            <div runat="server" id="tdPanel" visible="false">
                                <div class="mainheading">
                                    Upload Serial Number                                            
                                </div>
                                <div class="contentbox">
                                    <asp:Panel runat="server" ID="pnlIMEIAck" Visible="false">
                                        <div class="H25-C3-S">
                                            <ul>
                                                <li class="field">
                                                    <asp:FileUpload ID="flUpload" runat="server" CssClass="fileuploads" />
                                                </li>
                                                <li class="field3">
                                                    <a class="elink2" href="../../../Excel/Templates/PrimaryIMEIAck.xlsx">Download Template</a>
                                                </li>
                                            </ul>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="margin-bottom">
                                <div class="float-margin">
                                    <asp:Button ID="btnAccept" CssClass="buttonbg" runat="server" Text="Accept" CausesValidation="False"
                                        OnClick="btnAccept_Click" />
                                </div>
                                <div class="float-left">
                                    <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="buttonbg" CausesValidation="false"
                                        OnClick="btnReject_Click" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
