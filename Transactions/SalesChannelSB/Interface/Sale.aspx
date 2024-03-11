<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sale.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Transactions_Sale" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>

<%@ Register Src="~/UserControls/PartLookupClientSide.ascx" TagName="PartLookupClientSide"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/media/css/demo_page.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/css/demo_table.css") %>" />

    <%--Commented by Adnan
        <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../../../Assets/Jscript/jquery.dataTables.js"></script>--%>

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
            <td align="left" class="tableposition">
                <div class="mainheading_rpt">
                    <div class="mainheading_rpt_left">
                    </div>
                    <div class="mainheading_rpt_mid">
                        Manage Sales
                    </div>
                    <div class="mainheading_rpt_right">
                    </div>
                </div>
            </td>
        </tr>
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
            <td align="left" valign="top">
                <div class="contentbox">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">


                        <tr>
                            <td width="12%" align="right" class="formtext" valign="top">Select Sales Type: <span id="span1" runat="server"><font class="error">*</font></span>
                            </td>
                            <td width="21%" align="left" class="formtext" valign="top">
                                <div style="float: left; width: 135px;">
                                    <asp:DropDownList ID="ddlSalesType" runat="server" CssClass="formselect">
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div style="float: left; width: 135px;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlSalesType"
                                        CssClass="error" ValidationGroup="EntryValidation" InitialValue="0" runat="server"
                                        ErrorMessage="Please select."></asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td width="21%" align="left" class="formtext" valign="top">
                                <asp:RadioButtonList ID="rdbSalesWith" runat="server" RepeatDirection="Horizontal" AutoPostBack="false">
                                    <asp:ListItem Text="On Interface" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Excel Upload" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>

                            </td>
                            <td>
                                <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="buttonbg" CausesValidation="false"
                                    OnClick="btnGo_Click" />
                                <asp:Button ID="btnResetSaleType" runat="server" Text="Reset" CssClass="buttonbg" CausesValidation="false"
                                    OnClick="btnResetSaleType_Click" />
                            </td>
                            <td>
                                <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="false"></asp:HyperLink>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr id="UISale" runat="server">
            <td valign="top" align="left">
                <table id="tbUI" runat="server" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td align="left" valign="top">
                            <div class="mainheading">
                                Sale
                            </div>
                            <div class="clear"></div>
                            <div class="subheading" runat="server" id="Div1">
                                Note : Sale entry on user interface having line items more than 100 may be slow. If in your invoice there are more than 100 line items please use excel upload.
                            </div>
                            <div class="clear"></div>
                            <div class="contentbox">
                                <div class="H25-C3-S">
                                    <ul>
                                        <li class="text">Seller:<font class="error">*</font></li>
                                        <li class="field">
                                            <div class="float-margin">
                                                <asp:DropDownList ID="ddlFromSC" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlFromSC_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="float-margin">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="ddlFromSC"
                                                    CssClass="error" ValidationGroup="EntryValidation" InitialValue="0" runat="server"
                                                    ErrorMessage="Please select."></asp:RequiredFieldValidator>
                                            </div>
                                        </li>
                                        <li class="text">Select Salesman:</li>
                                        <li class="field">
                                            <asp:DropDownList ID="ddlSalesman" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlSalesman_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </li>
                                        <li class="text">Buyer:<font class="error">*</font></li>
                                        <li class="field">
                                            <div class="float-margin">
                                                <asp:DropDownList ID="ddlRetailer" runat="server" CssClass="formselect">
                                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="float-margin">
                                                <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlRetailer" CssClass="error"
                                                    ValidationGroup="EntryValidation" InitialValue="0" runat="server" ErrorMessage="Please select."></asp:RequiredFieldValidator>
                                            </div>
                                        </li>

                                        <li class="text">Invoice Number:<%--<font class="error">*</font>--%></li>
                                        <li class="field">
                                            <div class="float-margin">
                                                <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>
                                            </div>
                                            <%--<div class="float-margin">
                                                <asp:RequiredFieldValidator ID="RequInvoiceNumber" ControlToValidate="txtInvoiceNo"
                                                    ValidationGroup="EntryValidation" runat="server" CssClass="error" ErrorMessage="Please enter invoice number."></asp:RequiredFieldValidator>
                                            </div>--%>
                                        </li>
                                        <li class="text">Invoice Date:<font class="error">*</font></li>
                                        <li class="field">
                                            <div class="float-margin">
                                                <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="false"
                                                    ValidationGroup="EntryValidation" />
                                            </div>
                                        </li>
                                        <li class="text">
                                            <asp:Label ID="lblInfo" runat="server" Text="" CssClass="error"></asp:Label></li>
                                        <li class="field"></li>
                                    </ul>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td height="10"></td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">

                            <div class="mainheading" id="tblGrid" runat="server">
                                List
                            </div>

                            <div class="contentbox">

                                <uc4:PartLookupClientSide ID="PartLookupClientSide1" runat="server" />

                            </div>

                            <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="EntryValidation"
                                CausesValidation="true" OnClick="btnSave_Click" />
                            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" CausesValidation="false"
                                OnClick="btnReset_Click" />
                            <br />
                            <br />
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
                                                        <asp:TextBox ID="txtDirectSerialNumber" runat="server" CssClass="formfields" TextMode="MultiLine"></asp:TextBox>
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
        <tr id="UploadSale" runat="server">
            <td valign="top" align="left">
                <asp:UpdatePanel ID="updateBulk" runat="server">
                    <ContentTemplate>
                        <div class="mainheading">
                            Upload
                        </div>
                        <div class="contentbox">
                            <div class="mandatory">
                                (*) Marked fields are mandatory
                            </div>
                            <div class="subheading" runat="server" id="DivNote">
                                Note 1 : Template File has sample data, please remove it before adding the sale data in template file. 
                            </div>
                            <div class="clear"></div>
                            <div class="mainheading" runat="server" id="ForSaveTemplateheading">
                                Step 1 : Download  Template For Save Record 
                            </div>
                            <div class="contentbox" runat="server" id="ForSaveTemplatedownload" >
                                <div class="H25-C3-S">

                                    <ul>
                                        <li>
                                        <a class="elink2" href="../../../Excel/Templates/SecondarySales-SB.xlsx">Download Template</a>
                                    </li>


                                    </ul>
                                </div>
                            </div>
                            <div class="mainheading" runat="server" id="ReferenceIdForsaveheading" >
                                Step 2 : Download Reference Code For Save Record 
                            </div>
                            <div class="contentbox" runat="server" id="ReferenceIdForsave" >
                                <div class="H25-C3-S">
                                    <ul>
                                        <li>
                                        <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                                            CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>
                                    </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="mainheading">
                                Step 3 : Upload Excel File
                            </div>
                            <div class="contentbox">
                                <div class="H25-C3-S">

                                    <ul>
                                    <li class="text">Upload File: <span class="error">*</span>
                                    </li>
                                    <li class="field">
                                        <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />

                                        <asp:Label ID="Label1" runat="server" CssClass="error" Text=""></asp:Label>
                                    </li>
                                    <li class="field3">
                                        <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11" OnClick="btnUpload_Click" />
                                    </li>
                                </ul>
                                </div>
                            </div>



                            
                            
                            
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="DwnldReferenceCodeTemplate" />
                        <asp:PostBackTrigger ControlID="btnUpload" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>

