<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="BulkSalesChannelRetailerTransfer.aspx.cs" Inherits="Masters_SalesChannel_BulkSalesChannelRetailerTransfer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/SalesEntryGrid.ascx" TagName="ucSalesEntryGrid"
    TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Transfer Retailers
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H35-C3-S">
            <ul>
                <li class="text">
                    <asp:Label ID="Label1" runat="server" Text="">Parent SalesChannel: <span class="error">*</span></asp:Label>
                </li>
                <li class="field">
                    <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div>
                                <asp:DropDownList ID="cmbTransferFrom" CssClass="formselect" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="cmbTransferFrom_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbTransferFrom" Display="Dynamic"
                                    CssClass="error" ErrorMessage="Please select a SalesChannel whose retailers you want to transfer" InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </ContentTemplate>
                        <%-- <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                </Triggers>--%>
                    </asp:UpdatePanel>
                </li>
                <li class="text">
                    <asp:Label ID="Label3" runat="server" Text="">Transfer To: <span class="error">*</span></asp:Label>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="cmbTransferTo" CssClass="formselect" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="cmbTransferTo"
                            CssClass="error" ErrorMessage="Please select a SalesChannel whose retailers you want to transfer" InitialValue="0" ValidationGroup="insert" />
                    </div>
                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button ID="btnTransfer" Text="Transfer" runat="server" OnClick="btntransfer_click"
                            ValidationGroup="insert" CssClass="buttonbg" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                            CssClass="buttonbg" />
                    </div>
                </li>
            </ul>

        </div>
    </div>
    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
        <div class="mainheading">
            List
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="grdRetailer" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                            BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="RetailerID" EmptyDataText="No data founnd"
                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                            AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" PageSize='<%$ AppSettings:GridViewPageSize %>' FooterStyle-CssClass="gridfooter"
                            HeaderStyle-CssClass="gridheader" RowStyle-VerticalAlign="top" Width="100%"
                            OnPageIndexChanging="grdRetailer_PageIndexChanging">
                            <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="Sales Man From" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RetailerID")%>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="RetailerName" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Name"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="RetailerCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Code"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="SalesManName" HeaderStyle-HorizontalAlign="Left" HeaderText="Salesman Name"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                    <%-- <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />
                                            </Triggers>--%>
                </asp:UpdatePanel>

            </div>
        </div>
    </asp:Panel>
</asp:Content>


