<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrimaryOrderPOC.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Transactions_POC_PrimaryOrderPOC" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucPrimaryOrderPOC.ascx" TagName="ucSalesEntryGrid"
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
        Primary Order
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Select Warehouse: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="formselect">
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlWarehouse" CssClass="error" Display="Dynamic"
                        ValidationGroup="EntryValidation" InitialValue="0" runat="server" ErrorMessage="Please select Warehouse Name."></asp:RequiredFieldValidator>
                </li>
                <%-- <td width="15%" valign="top" align="right" class="formtext">
                                            Order Number: <font class="error">*</font>
                                        </td>
                                        <td width="15%" valign="top" align="left">
                                            <asp:TextBox ID="txtOrderNo" runat="server" MaxLength="20" CssClass="form_input2"></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="RequInvoiceNumber" ControlToValidate="txtOrderNo"
                                                ValidationGroup="EntryValidation" runat="server" CssClass="error" ErrorMessage="Please enter Order number."></asp:RequiredFieldValidator>
                                        </td>
                --%>
                <li class="text">Order Date: <span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                        ValidationGroup="Add" />
                </li>
                <li class="field">
                    <asp:Button ID="BtnSubmit" runat="server" Text="&nbsp;Go&nbsp;" CssClass="buttonbg"
                        CausesValidation="true" ValidationGroup="EntryValidation" OnClick="BtnSubmit_Click" />
                </li>
            </ul>
        </div>
    </div>
    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
        <div id="tblGrid">
            <div class="mainheading">
                Enter Primary Order Details
            </div>
            <div class="contentbox">
                <uc3:ucSalesEntryGrid ID="Ucgrid" runat="server" />
            </div>
            <div class="clear"></div>
            <div class="float-margin">
                <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="Add"
                    CausesValidation="true" OnClick="btnSave_Click" />
            </div>
            <div class="float-margin">
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" OnClick="btnReset_Click" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
