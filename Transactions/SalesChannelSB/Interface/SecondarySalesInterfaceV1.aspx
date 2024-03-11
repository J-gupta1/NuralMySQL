<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SecondarySalesInterfaceV1.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Transactions_SalesChannel_SecondarySalesInterfaceV1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/SalesEntryGridWithoutOrder.ascx" TagName="ucSalesEntryGrid"
    TagPrefix="uc3" %>
<%@ Register Src="../../../UserControls/PartLookupClientSide.ascx" TagName="PartLookupClientSide"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/media/css/demo_page.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/css/demo_table.css") %>" />

    <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/jquery.js"></script>

    <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/jquery.dataTables.js"></script>

    <%--<link href="../../../Assets/Beetel/CSS/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <link href="../../../Assets/Beetel/CSS/modal.css" rel="stylesheet" type="text/css" />


    <script type="text/javascript" src="../../../Assets/Jscript/dhtmlwindow.js"></script>
    <script type="text/javascript" src="../../../Assets/Jscript/modal.js"></script>--%>
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <script type="text/javascript" src="<%# Page.ResolveClientUrl("~/Assets/Jscript/dhtmlwindow.js") %>"></script>

    <script type="text/javascript" src="<%# Page.ResolveClientUrl("~/Assets/Jscript/modal.js") %>"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
            <asp:HiddenField ID="hdnDirectSalesOfSerialAllowed" Value="0" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Manage Secondary Sales
    </div>
    <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H30-C3-S">
            <ul>
                <li class="text">Select Mode:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:RadioButtonList ID="rdoSelectMode" runat="server" RepeatDirection="Horizontal" CssClass="radio-rs"
                        CellPadding="2" CellSpacing="0" BorderWidth="0" AutoPostBack="true" OnSelectedIndexChanged="rdoSelectMode_SelectedIndexChanged">
                        <asp:ListItem Value="1" Text="Excel Template"></asp:ListItem>
                        <asp:ListItem Selected="True" Value="2" Text="On-Screen Entry"></asp:ListItem>
                    </asp:RadioButtonList>
                </li>
                <li class="text">Select Salesman: <span id="spanSalesManOptional" runat="server"><span class="error">*</span></span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlSalesman" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlSalesman_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlSalesman" Display="Dynamic"
                            CssClass="error" ValidationGroup="EntryValidation" InitialValue="0" runat="server"
                            ErrorMessage="Please select salesman."></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">Select Retailer: <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlRetailer" runat="server" CssClass="formselect">
                            <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlRetailer" CssClass="error" Display="Dynamic"
                            ValidationGroup="EntryValidation" InitialValue="0" runat="server" ErrorMessage="Please select retailer."></asp:RequiredFieldValidator>
                    </div>
                </li>
            </ul>
            <ul>
                <li class="text">Invoice Date: <span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="false"
                        ValidationGroup="EntryValidation" />
                    <asp:Label ID="lblInfo" runat="server" Text="" CssClass="error"></asp:Label>
                </li>
                <li class="text">Invoice Number: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequInvoiceNumber" ControlToValidate="txtInvoiceNo" Display="Dynamic"
                        ValidationGroup="EntryValidation" runat="server" CssClass="error" ErrorMessage="Please enter invoice number."></asp:RequiredFieldValidator>
                </li>
            </ul>
        </div>
    </div>
    <div id="tblGrid" runat="server">
        <div class="mainheading">
            List
        </div>
        <div class="contentbox">
            <uc4:PartLookupClientSide ID="PartLookupClientSide1" runat="server" />
        </div>
        <div class="padding-bottom">
            <div class="float-margin">
                <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="EntryValidation"
                    CausesValidation="true" OnClick="btnSave_Click" />
            </div>
            <div class="float-left">
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" CausesValidation="false"
                    OnClick="btnReset_Click" />
            </div>
            <div class="clear"></div>
        </div>
    </div>
    <div id="tblDirectSerialPanel" runat="server">
        <div class="mainheading">
            Enter Serial No.
        </div>
        <div class="contentbox">
            <div class="H35-C3-S">
                <ul>
                    <li class="text">Serial No.: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:TextBox ID="txtDirectSerialNumber" runat="server" CssClass="form_textarea" TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <div style="margin-top: 2px;" class="error">
                            (Comma separated)
                        </div>
                    </li>
                    <li class="field3">
                        <div class="float-margin">
                            <asp:Button ID="btnSubmitDirectSerialSale" CssClass="buttonbg" runat="server" Text="Save"
                                ValidationGroup="EntryValidation" CausesValidation="true" OnClick="btnSubmitDirectSerialSale_Click" />
                        </div>
                        <div class="float-left">
                            <asp:Button ID="btnResetDirect" runat="server" Text="Reset" CssClass="buttonbg" OnClientClick="BlankTo();"
                                OnClick="btnReset_Click" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
