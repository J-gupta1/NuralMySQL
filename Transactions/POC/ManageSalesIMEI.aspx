<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageSalesIMEI.aspx.cs" Inherits="Transactions_POC_ManageSalesIMEI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Manage Sales IMEI
    </div>
    <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="contentbox">
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Select Retailer: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlRetailer" runat="server" CssClass="formselect" AutoPostBack="false">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="ddlRetailer"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please select retailer." InitialValue="0"
                                SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                        </li>
                        <li class="text">Model Name: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlModel" runat="server" ValidationGroup="Add" CssClass="formselect"
                                AutoPostBack="false">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlModel"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please select Model." InitialValue="0"
                                SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                        </li>
                        <li class="text">Invoice Number:<span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="formfields"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtInvoiceNumber"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please select retailer." SetFocusOnError="true"
                                ValidationGroup="Add"></asp:RequiredFieldValidator>
                        </li>
                        <li class="text">Invoice Date: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="false"
                                ValidationGroup="Add" />
                            <asp:Label ID="lblInfo" runat="server" Text="" CssClass="error"></asp:Label>
                        </li>
                        <li class="text">Quantity: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="formfields"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtQuantity"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please Insert Quantity." SetFocusOnError="true"
                                ValidationGroup="Add"></asp:RequiredFieldValidator>
                        </li>
                        <li class="text">Serial Number: <span class="error">*</span>
                        </li>
                        <li class="field" style="height:auto">
                            <asp:TextBox ID="txtSerialNumber" runat="server" TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSerialNumber"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please select retailer." SetFocusOnError="true"
                                ValidationGroup="Add"></asp:RequiredFieldValidator>
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <asp:Button ID="BtnSubmit" runat="server" Text="Add" CssClass="buttonbg"
                                CausesValidation="true" ValidationGroup="Add" OnClick="BtnSubmit_Click" />
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
                            <asp:GridView ID="grdvList" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                SelectedStyle-CssClass="gridselected" CellSpacing="1" DataKeyNames="InvoiceNumber,ModelID"
                                EmptyDataText="No Record Found" GridLines="None" Width="100%" HeaderStyle-CssClass="gridheader" FooterStyle-CssClass="gridfooter"
                                RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" EditRowStyle-CssClass="editrow"
                                ShowFooter="false">
                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                <PagerStyle CssClass="gridfooter" />
                                <Columns>
                                    <asp:TemplateField HeaderText="InvoiceNumber" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%# Eval("InvoiceNumber")%>
                                        </ItemTemplate>
                                        <ItemStyle Width="200px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RetailerName" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%# Eval("RetailerName")%>
                                        </ItemTemplate>
                                        <ItemStyle Width="200px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="ModelName" ShowHeader="False">
                                        <ItemTemplate>
                                            <%# Eval("ModelName")%>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Quantity" ShowHeader="False">
                                        <ItemTemplate>
                                            <%# Eval("Quantity")%>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="IMEI" ShowHeader="False">
                                        <ItemTemplate>
                                            <%# Eval("IMEI")%>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                                <SelectedRowStyle CssClass="selectedrow" />
                                <HeaderStyle CssClass="gridheader" />
                                <EditRowStyle CssClass="editrow" />
                                <AlternatingRowStyle CssClass="Altrow" />
                                <EmptyDataTemplate>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="Add1"
                            CausesValidation="true" OnClick="btnSave_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" CausesValidation="false"
                            OnClick="btnReset_Click" />
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
