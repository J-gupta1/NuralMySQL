<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="BatchWiseSecondrySale.aspx.cs" Inherits="Transactions_POC_BatchWiseSecondrySale" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Sales Details
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">
                    <asp:Label ID="lblrole" runat="server" Text="">Party:</asp:Label>
                </li>
                <li class="field">
                    <asp:DropDownList ID="cmbParty" CssClass="formselect" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="valtype" ControlToValidate="cmbParty"
                        CssClass="error" ErrorMessage="Please select a Party " InitialValue="0" ValidationGroup="insert" />
                </li>
                <li class="text">
                    <asp:Label ID="labelinscode" runat="server" Text="">Invoice Number:</asp:Label>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>
                </li>
                <li class="text">
                    <asp:Label ID="lblserfrmDate" runat="server" Text="">Invoice Date: </asp:Label>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucInvoiceDate" runat="server" defaultDateRange="True" ErrorMessage="Please select Invoice Date."
                        RangeErrorMessage="Date should be less than or equall to the current date." ValidationGroup="insert" />
                </li>
                <li class="text"></li>
                <li class="field">
                    <div class="float-margin">
                        <asp:Button ID="btnInitial" runat="server" CausesValidation="True" CssClass="buttonbg"
                            OnClick="btnInitial_click" Text="Submit" ValidationGroup="insert" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btncancelInitial" runat="server" CausesValidation="False" CssClass="buttonbg"
                            OnClick="btncancelInitial_Click" Text="Cancel" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <%--  <asp:UpdatePanel runat="server" ID="updProduct" UpdateMode="Conditional">
                            <ContentTemplate>--%>
    <asp:Panel ID="pnlProduct" runat="server" Visible="false">
        <div class="mainheading">
            Select Products
        </div>
        <div class="contentbox">
            <div class="H20-C3-S">
                <ul>
                    <li class="text">
                        <asp:Label ID="Label1" runat="server" Text="">Product:</asp:Label>
                    </li>
                    <li class="field">
                        <asp:DropDownList ID="cmbProduct" CssClass="formselect" AutoPostBack="true" runat="server"
                            OnSelectedIndexChanged="cmbProduct_selectedindexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbProduct" Display="Dynamic"
                            CssClass="error" ErrorMessage="Please select a Product " InitialValue="0" ValidationGroup="search1" />
                    </li>
                    <li class="text">
                        <asp:Label ID="Label2" runat="server" Text="">SKU Code:</asp:Label>
                    </li>
                    <li class="field">
                        <asp:DropDownList ID="cmbSKUCode" CssClass="formselect" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="cmbSKUCode" Display="Dynamic"
                            CssClass="error" ErrorMessage="Please select a SKU Code " InitialValue="0" ValidationGroup="search1" />
                    </li>
                    <li class="field3">
                        <div class="float-margin">
                            <asp:Button ID="btnProduct" runat="server" CausesValidation="True" CssClass="buttonbg"
                                OnClick="btnProduct_click" Text="Search" ValidationGroup="search1" />
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnCancelProduct" runat="server" CausesValidation="False" CssClass="buttonbg"
                                OnClick="btncancelProduct_Click" Text="Cancel" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </asp:Panel>
    <%--  </ContentTemplate>
                        </asp:UpdatePanel>--%>


    <%-- <asp:UpdatePanel runat="server" ID="updBatchInfo" UpdateMode="Conditional">
                                <ContentTemplate>--%>
    <asp:Panel ID="pnlBatchInfo" runat="server" Visible="false">
        <div class="mainheading">
            Select Batch
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:GridView ID="grdBatchInfo" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-HorizontalAlign="Left"
                    FooterStyle-VerticalAlign="Top" GridLines="None" HeaderStyle-HorizontalAlign="left"
                    HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="Altrow"
                    RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                    RowStyle-VerticalAlign="top" Width="100%">
                    <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkBatch" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BatchNumber">
                            <ItemTemplate>
                                <asp:Label ID="lblBatchNumber" runat="server" Text='<%# Bind("BatchCode") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BatchID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBatchID" runat="server" Text='<%# Bind("BatchID") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BatchDate">
                            <ItemTemplate>
                                <asp:Label ID="lblBatchDate" runat="server" Text='<%# Bind("StockInDate") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Stock In Hand">
                            <ItemTemplate>
                                <asp:Label ID="lblStock" runat="server" Text='<%# Bind("QuantityInHand") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SalesChannelBatchStockID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBatchStockID" runat="server" Text='<%# Bind("SalesChannelBatchStockID") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                    <PagerStyle CssClass="PagerStyle" />
                    <AlternatingRowStyle CssClass="Altrow" />
                </asp:GridView>
            </div>
        </div>
        <div class="float-margin">
            <asp:Button ID="btnBatchInfo" runat="server" CssClass="buttonbg" OnClick="btnBatchInfo_click"
                Text="Add In List" />
        </div>
        <div class="float-margin">
            <asp:Button ID="btnCancelBatchinfo" runat="server" CssClass="buttonbg" OnClick="btncancelBatchInfo_Click"
                Text="Cancel" />
        </div>
        <div class="clear"></div>
    </asp:Panel>
    <%--    </ContentTemplate>
                            </asp:UpdatePanel>--%>


    <%--   <asp:UpdatePanel runat="server" ID="updFinalGrid" UpdateMode="Conditional">
                                <ContentTemplate>--%>
    <asp:Panel ID="pnlFinalGrid" runat="server" Visible="false">
        <div class="mainheading">
            List
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:GridView ID="grdFinal" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-HorizontalAlign="Left"
                    FooterStyle-VerticalAlign="Top" GridLines="None" HeaderStyle-HorizontalAlign="left"
                    HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="Altrow"
                    RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                    RowStyle-VerticalAlign="top" Width="100%">
                    <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="BatchNumber">
                            <ItemTemplate>
                                <asp:Label ID="lblBatchNumber" runat="server" Text='<%# Bind("BatchNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BatchNumber">
                            <ItemTemplate>
                                <asp:Label ID="lblBatchID" runat="server" Text='<%# Bind("BatchID") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="BatchDate">
                            <ItemTemplate>
                                <asp:Label ID="lblBatchDate" runat="server" Text='<%# Bind("BatchDate") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SKU">
                            <ItemTemplate>
                                <asp:Label ID="lblSku" runat="server" Text='<%# Bind("SKUCode") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SalesChannelBatchStockID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBatchStockID" runat="server" Text='<%# Bind("BatchStockID") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Stock" HeaderStyle-HorizontalAlign="Left" HeaderText="Stock"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                    <PagerStyle CssClass="PagerStyle" />
                    <AlternatingRowStyle CssClass="Altrow" />
                </asp:GridView>
            </div>
        </div>
        <div class="float-margin">
            <asp:Button ID="btnFinal" runat="server" CssClass="buttonbg" OnClick="btnFinal_click"
                Text="Save" />
        </div>
        <div class="float-margin">
            <asp:Button ID="btnCancelFinal" runat="server" CssClass="buttonbg" OnClick="btncancelfinal_Click"
                Text="Cancel" />
        </div>
        <div class="clear"></div>
    </asp:Panel>
    <%--   </ContentTemplate>
                            </asp:UpdatePanel>--%>
</asp:Content>
