<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InterMediarySales2InterfaceAddV1SKUWise.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Transactions_SalesChannel_InterMediarySales2InterfaceAddV1SKUWise" %>

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
    <table cellspacing="0" cellpadding="0" width="965" border="0" align="center">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td valign="top" align="left">
                            <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <uc1:ucMessage ID="ucMsg" runat="server" />
                                    <asp:HiddenField ID="hdnDirectSalesOfSerialAllowed" Value="0" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td align="left" valign="top" class="tableposition">
                                        <div class="mainheading_rpt">
                                            <div class="mainheading_rpt_left">
                                            </div>
                                            <div class="mainheading_rpt_mid">
                                                Intermediary Sales Entry</div>
                                            <div class="mainheading_rpt_right">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <%--  <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                        <td align="left" valign="top">
                            <div class="contentbox">
                                <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="4" align="left" valign="top" height="15" class="mandatory">
                                            (*) Marked fields are mandatory
                                        </td>
                                    </tr>
                                   <%--  <tr>
                                        <td class="formtext" valign="top" align="right" width="13%">
                                           Select Mode:<span class="error">*</span>
                                        </td>
                                        <td align="left" valign="top" width="30%" class="formtext" style="padding-top: 0px;
                                            margin-top: 0px;">
                                         <asp:RadioButtonList ID="rdoSelectMode" runat="server" RepeatDirection="Horizontal"
                                                CellPadding="0" CellSpacing="0" AutoPostBack="true" OnSelectedIndexChanged="rdoSelectMode_SelectedIndexChanged">
                                                <asp:ListItem Value="1" Text="Excel Template"></asp:ListItem>
                                                <asp:ListItem Selected="True" Value="2" Text="On-Screen Entry"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                       
                                    </tr>--%>
                                    <tr>
                                         <td width="15%" valign="top" align="right" class="formtext">
                                            Select <asp:Label ID="lblChange" runat="server"></asp:Label>: <span class="error">*</span>
                                        </td>
                                        <td width="18%" valign="top" align="left" class="formtext">
                                            <asp:DropDownList ID="ddlTD" runat="server" CssClass="form_select">
                                            </asp:DropDownList>
                                            <div style="float: left; width: 100%;">
                                                <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlTD" CssClass="error"
                                                    ValidationGroup="Add" InitialValue="0" runat="server" ErrorMessage="Please select."
                                                    Display="Dynamic"></asp:RequiredFieldValidator></div>
                                        </td>
                                        <td valign="top" align="right" class="formtext">
                                            Invoice Number: <span class="error">*</span>
                                        </td>
                                        <td valign="top" align="left">
                                            <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="50" CssClass="form_input2"></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="RequInvoiceNumber" ControlToValidate="txtInvoiceNo"
                                                ValidationGroup="Add" runat="server" CssClass="error" ErrorMessage="Please enter invoice number."
                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                        <td valign="top" align="right" class="formtext">
                                            Invoice Date: <span class="error">*</span>
                                        </td>
                                        <td valign="top" align="left">
                                            <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                                                ValidationGroup="Add" />
                                            <asp:Label ID="lblValidationDays" runat="server" Text="" CssClass="error"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <%-- </ContentTemplate>
                                
                                </asp:UpdatePanel>--%>
                    </tr>
                    <tr>
                        <td height="10">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <%-- <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                            <table id="tblGrid" runat="server" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left" class="tableposition">
                                        <div class="mainheading_rpt">
                                            <div class="mainheading_rpt_left">
                                            </div>
                                            <div class="mainheading_rpt_mid">
                                                Enter Details</div>
                                            <div class="mainheading_rpt_right">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="contentbox">
                                            <uc4:PartLookupClientSide ID="PartLookupClientSide1" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" CausesValidation="true"
                                            OnClick="btnSave_Click" ValidationGroup="Add" />
                                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" OnClick="btnReset_Click" CausesValidation="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="10">
                                    </td>
                                </tr>
                            </table>
                            <table id="tblDirectSerialPanel" runat="server" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left" class="tableposition">
                                        <div class="mainheading_rpt">
                                            <div class="mainheading_rpt_left">
                                            </div>
                                            <div class="mainheading_rpt_mid">
                                                Enter Serial No.
                                            </div>
                                            <div class="mainheading_rpt_right">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="contentbox">
                                        <table cellpadding="4" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="right" valign="top" height="30" width="10%">
                                                     Serial No.:
                                                </td>
                                                <td align="right" valign="top" width="1%">
                                                <span class="error">*</span>
                                                </td>
                                                <td align="left" valign="top" width="20%">
                                                    <div>
                                                        <asp:TextBox ID="txtDirectSerialNumber" runat="server" CssClass="form_textarea" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div style="margin-top: 2px;" class="error">
                                                        (Comma separated)</div>
                                                </td>
                                                <td align="left" valign="top">
                                                    <div class="float-margin">
                                                        <asp:Button ID="btnSubmitDirectSerialSale" CssClass="buttonbg" runat="server" Text="Save"
                                                            ValidationGroup="Add" CausesValidation="true" OnClick="btnSubmitDirectSerialSale_Click" />
                                                    </div>
                                                    <div class="float-left">
                                                        <asp:Button ID="btnResetDirect" runat="server" Text="Reset" CssClass="buttonbg" OnClientClick="BlankTo();"
                                                            OnClick="btnReset_Click" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%-- <td valign="top" align="right" class="formtext"> Salesman: <span class="error">*</span>
    </td> <td align="left" valign="top"> <asp:TextBox ID="txtSalesman" runat="server"
    MaxLength="100" CssClass="form_input2" ></asp:TextBox> <br> </br> <asp:RequiredFieldValidator
    ID="RequiredFieldValidator1" ControlToValidate="txtSalesman" CssClass="error" ValidationGroup="Add"
    runat="server" ErrorMessage="Please enter salesman name."></asp:RequiredFieldValidator>
    </td>--%>
</asp:Content>
