<%@ Page Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="ManageSDSwapping.aspx.cs" Inherits="Masters_SalesChannel_ManageSDSwapping" %>

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
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        <asp:Label ID="lblStockTransfer2" Text=" Manage Sales Channel Transfer" runat="server" />
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">From Sales Channel : <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="cmbSDFrom" runat="server" CssClass="formselect" AutoPostBack="True"
                            OnSelectedIndexChanged="cmbSDFrom_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="reqoldsd" ControlToValidate="cmbSDFrom" CssClass="error" Display="Dynamic"
                            ValidationGroup="Add" InitialValue="0" runat="server" ErrorMessage="Please select From SalesChannel"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">To Sales Channel : <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="cmbSDTo" runat="server" CssClass="formselect" OnSelectedIndexChanged="cmbSDTo_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="reqnewsd" ControlToValidate="cmbSDTo" CssClass="error" Display="Dynamic"
                            ValidationGroup="Add" InitialValue="0" runat="server" ErrorMessage="Please select To SalesChannel"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="field3">
                    <asp:Button ID="btnReset" runat="Server" Text="Reset" CssClass="buttonbg" OnClick="btnReset_Click" />
                </li>
            </ul>
        </div>
    </div>
    <div>
        <div class="float-margin" style="width: 43%">
            <asp:Panel ID="Pnlfrom" runat="server" Visible="false">
                <asp:GridView ID="grdSDFrom" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                    RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" HeaderStyle-HorizontalAlign="left"
                    HeaderStyle-VerticalAlign="top" GridLines="None" AlternatingRowStyle-CssClass="Altrow"
                    RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                    CellSpacing="1" CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="False" PageSize='<%$ AppSettings:GridViewPageSize %>'
                    AllowPaging="True" SelectedStyle-CssClass="gridselected" DataKeyNames="SalesChannelID"
                    OnPageIndexChanging="grdfromPageIndexChanging">
                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridrow"></RowStyle>
                    <Columns>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelName"
                            HeaderText="Sales Channel Name"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ActivationDate" DataFormatString="{0: dd MMM yy}"
                            HeaderText="Activation Date"></asp:BoundField>
                    </Columns>
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridheader"></HeaderStyle>
                    <AlternatingRowStyle CssClass="Altrow"></AlternatingRowStyle>
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </asp:Panel>
        </div>
        <div class="float-margin" style="width: 10%; margin-top:3%">
            <asp:Button ID="btnswapsd" runat="Server" Text="Transfer" OnClick="btnswapsd_Click"
                CssClass="buttonbg" ValidationGroup="Add" />
        </div>
        <div class="float-left" style="width: 43%">
            <asp:Panel ID="pnlto" runat="server" Visible="false">
                <asp:GridView ID="grdSDTo" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                    RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" HeaderStyle-HorizontalAlign="left"
                    HeaderStyle-VerticalAlign="top" GridLines="none" AlternatingRowStyle-CssClass="Altrow"
                    RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                    CellSpacing="1" CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>'
                    AllowPaging="True" SelectedStyle-CssClass="gridselected" DataKeyNames="" OnPageIndexChanging="grdToPageIndexChanging">
                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                    <Columns>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelName"
                            HeaderText="Sales Channel Name"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ActivationDate" DataFormatString="{0: dd MMM yy}"
                            HeaderText="Activation Date"></asp:BoundField>
                    </Columns>
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
