<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SecondarySalesInterfaceV1SKUWise.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Transactions_SalesChannel_SecondarySalesInterfaceV1SKUWise" %>

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
    <table cellspacing="0" cellpadding="0" width="965" border="0" align="center" style="float: left;">
        <tr>
            <td valign="top" align="left">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td align="left" valign="top">
                            <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <uc1:ucMessage ID="ucMsg" runat="server" />
                                    <asp:HiddenField ID="hdnDirectSalesOfSerialAllowed" Value="0" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="tableposition">
                            <div class="mainheading_rpt">
                                <div class="mainheading_rpt_left">
                                </div>
                                <div class="mainheading_rpt_mid">
                                    Manage Secondary Sales
                                </div>
                                <div class="mainheading_rpt_right">
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="contentbox">
                                <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="6" align="left" valign="top" height="15" class="mandatory">(*) Marked fields are mandatory
                                        </td>
                                    </tr>
                                    <tr>
<%--                                        <td class="formtext" valign="top" align="right" width="10%">Select Mode:<font class="error">*</font>
                                        </td>
                                        <td width="25%" align="left" valign="top" class="formtext" style="padding-top: 0px; margin-top: 0px;">
                                            <asp:RadioButtonList ID="rdoSelectMode" runat="server" RepeatDirection="Horizontal"
                                                CellPadding="0" CellSpacing="0" BorderWidth="0" AutoPostBack="true" OnSelectedIndexChanged="rdoSelectMode_SelectedIndexChanged">
                                                <asp:ListItem Value="1" Text="Excel Template"></asp:ListItem>
                                                <asp:ListItem Selected="True" Value="2" Text="On-Screen Entry"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>--%>
                                        <td width="12%" align="right" class="formtext" valign="top">Select Salesman: <span id="spanSalesManOptional" runat="server"><font class="error">*</font></span>
                                        </td>
                                        <td width="21%" align="left" class="formtext" valign="top">
                                            <div style="float: left; width: 135px;">
                                                <asp:DropDownList ID="ddlSalesman" runat="server" CssClass="form_select" OnSelectedIndexChanged="ddlSalesman_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div style="float: left; width: 135px;">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlSalesman"
                                                    CssClass="error" ValidationGroup="EntryValidation" InitialValue="0" runat="server"
                                                    ErrorMessage="Please select salesman."></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                        <td width="12%" align="right" class="formtext" valign="top">Select Retailer: <font class="error">*</font>
                                        </td>
                                        <td width="21%" align="left" class="formtext" valign="top">
                                            <div style="float: left; width: 135px;">
                                                <asp:DropDownList ID="ddlRetailer" runat="server" CssClass="form_select">
                                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div style="float: left; width: 135px;">
                                                <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlRetailer" CssClass="error"
                                                    ValidationGroup="EntryValidation" InitialValue="0" runat="server" ErrorMessage="Please select retailer."></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                   
                                        <td align="right" class="formtext" valign="top">Invoice Date: <font class="error">*</font>
                                        </td>
                                        <td align="left" valign="top">
                                            <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="false"
                                                ValidationGroup="EntryValidation" />
                                            <asp:Label ID="lblInfo" runat="server" Text="" CssClass="error"></asp:Label>
                                        </td> </tr>
                                    <tr>
                                        <td valign="top" align="right" class="formtext">Invoice Number: <font class="error">*</font>
                                        </td>
                                        <td valign="top" align="left">
                                            <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="50" CssClass="form_input2"></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="RequInvoiceNumber" ControlToValidate="txtInvoiceNo"
                                                ValidationGroup="EntryValidation" runat="server" CssClass="error" ErrorMessage="Please enter invoice number."></asp:RequiredFieldValidator>
                                        </td>

                                        <td colspan="6" ></td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td height="10"></td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table id="tblGrid" runat="server" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left" class="tableposition">
                                        <div class="mainheading_rpt">
                                            <div class="mainheading_rpt_left">
                                            </div>
                                            <div class="mainheading_rpt_mid">
                                                List
                                            </div>
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
                                    <td height="10"></td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="EntryValidation"
                                            CausesValidation="true" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" CausesValidation="false"
                                            OnClick="btnReset_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="10"></td>
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
                                                <td align="right" valign="top" height="30" width="10%">Serial No.:
                                                </td>
                                                <td align="right" valign="top" width="1%">
                                                    <span class="error">*</span>
                                                </td>
                                                <td align="left" valign="top" width="20%">
                                                    <div>
                                                        <asp:TextBox ID="txtDirectSerialNumber" runat="server" CssClass="form_textarea" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                    <div style="margin-top: 2px;" class="error">
                                                        (Comma separated)
                                                    </div>
                                                </td>
                                                <td align="left" valign="top">
                                                    <div class="float-margin">
                                                        <asp:Button ID="btnSubmitDirectSerialSale" CssClass="buttonbg" runat="server" Text="Save"
                                                            ValidationGroup="EntryValidation" CausesValidation="true" OnClick="btnSubmitDirectSerialSale_Click" />
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
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
