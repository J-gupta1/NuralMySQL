<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrimarySales2InterfaceAddV1.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Transactions_SalesChannel_PrimarySales2InterfaceV1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/SalesEntryGrid.ascx" TagName="ucSalesEntryGrid"
    TagPrefix="uc3" %>
<%@ Register Src="../../../UserControls/PartLookupClientSide.ascx" TagName="PartLookupClientSide"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/media/css/demo_page.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/css/demo_table.css") %>" />

    <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/jquery.js"></script>

    <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/jquery.dataTables.js"></script>

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
    <div class="clear"></div>
    <div class="mainheading">
        Intermediary Sales Entry
    </div>

    <%--  <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>

    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Select Mode:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:RadioButtonList ID="rdoSelectMode" runat="server" RepeatDirection="Horizontal" CssClass="radio-rs"
                        CellPadding="2" CellSpacing="0" AutoPostBack="true" OnSelectedIndexChanged="rdoSelectMode_SelectedIndexChanged">
                        <asp:ListItem Value="1" Text="Excel Template"></asp:ListItem>
                        <asp:ListItem Selected="True" Value="2" Text="On-Screen Entry"></asp:ListItem>
                    </asp:RadioButtonList>
                </li>
                <li class="text">Select
                                            <asp:Label ID="lblChange" runat="server"></asp:Label>: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlTD" runat="server" CssClass="formselect">
                    </asp:DropDownList>
                    <div style="float: left; width: 100%;">
                        <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlTD" CssClass="error"
                            ValidationGroup="Add" InitialValue="0" runat="server" ErrorMessage="Please select."
                            Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </li>
            </ul>
            <ul>
                <li class="text">Invoice Number: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequInvoiceNumber" ControlToValidate="txtInvoiceNo"
                        ValidationGroup="Add" runat="server" CssClass="error" ErrorMessage="Please enter invoice number."
                        Display="Dynamic"></asp:RequiredFieldValidator>
                </li>
                <li class="text">Invoice Date: <span class="error">*</span>
                </li>
                <li class="field" style="height: auto">
                    <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                        ValidationGroup="Add" />
                    <asp:Label ID="lblValidationDays" runat="server" Text="" CssClass="error"></asp:Label>
                </li>
            </ul>
        </div>
    </div>

    <%-- </ContentTemplate>
                                
                                </asp:UpdatePanel>--%>



    <%-- <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
    <div id="tblGrid" runat="server">
        <div class="mainheading">
            Enter Details                    
        </div>
        <div class="contentbox">
            <uc4:PartLookupClientSide ID="PartLookupClientSide1" runat="server" />
        </div>
        <div>
            <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" CausesValidation="true"
                OnClick="btnSave_Click" ValidationGroup="Add" />
            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" OnClick="btnReset_Click" />
        </div>
    </div>
    <div id="tblDirectSerialPanel" runat="server">
        <div class="mainheading">
            Enter Serial No.
        </div>
        <div class="contentbox">
            <div class="H25-C3-S">
                <ul>
                    <li class="text">Serial No.:               
                        <span class="error">*</span>
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
                                ValidationGroup="Add" CausesValidation="true" OnClick="btnSubmitDirectSerialSale_Click" />
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
    <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>


    <%-- <td valign="top" align="right" class="formtext"> Salesman: <span class="error">*</span>
    </td> <td align="left" valign="top"> <asp:TextBox ID="txtSalesman" runat="server"
    MaxLength="100" CssClass="form_input2" ></asp:TextBox> <br> </br> <asp:RequiredFieldValidator
    ID="RequiredFieldValidator1" ControlToValidate="txtSalesman" CssClass="error" ValidationGroup="Add"
    runat="server" ErrorMessage="Please enter salesman name."></asp:RequiredFieldValidator>
    </td>--%>
</asp:Content>
