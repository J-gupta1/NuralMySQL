<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StockTransferV1.aspx.cs" Inherits="Transactions_SalesChannel_StockTransferV1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/SalesEntryGridWithoutOrder.ascx" TagName="ucSalesEntryGrid"
    TagPrefix="uc3" %>
<%@ Register Src="../../UserControls/PartLookupClientSide.ascx" TagName="PartLookupClientSide"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/media/css/demo_page.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/css/demo_table.css") %>" />
    <script type="text/javascript" language="javascript" src="../../Assets/Jscript/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../../Assets/Jscript/jquery.dataTables.js"></script>

    <link href="../../Assets/Beetel/CSS/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <link href="../../Assets/Beetel/CSS/modal.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0" align="center" style="float: left;">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <!-- <asp:Label ID = "lblStockTransfer1" Text = "Stock  Transfer" runat = "Server" ></asp:Label>-->
                    <tr>
                        <td align="left" valign="top">
                            <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <uc1:ucMessage ID="ucMessage1" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="tableposition">
                            <div class="mainheading">
                                <asp:Label ID="lblStockTransfer2" Text="Stock  Transfer" runat="server" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <div class="contentbox">
                  

                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td colspan="4" align="left" valign="top" height="15" class="mandatory">(*) Marked fields are mandatory
                                    </td>
                                </tr>
                                <tr>
                                    <td width="15%" align="right" class="formtext" valign="top">Sales Channel Type: <font class="error">*</font>
                                    </td>
                                    <td width="20%" align="left" class="formtext" valign="top">

                                        <div style="float: left; width: 135px;">
                                            <asp:DropDownList ID="cmbChannelType" runat="server" CssClass="form_select" AutoPostBack="True"
                                                OnSelectedIndexChanged="cmbChannelType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <br />
                                        </div>
                                        <div style="float: left; width: 170px;">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="cmbChannelType"
                                                CssClass="error" ValidationGroup="Add" InitialValue="0" runat="server" ErrorMessage="Please select a Sales Channel Type "></asp:RequiredFieldValidator>
                                        </div>

                                    </td>
                                    <td width="15%" height="35" align="right" class="formtext" valign="top">Stock Transfer From: <font class="error">*</font>
                                    </td>
                                    <td width="50%" align="left" class="formtext" valign="top">

                                        <div style="float: left; width: 135px;">
                                            <asp:DropDownList ID="cmbFrom" runat="server" OnSelectedIndexChanged="cmbFrom_SelectedIndexChanged"
                                                CssClass="form_select" AutoPostBack="True">
                                            </asp:DropDownList>
                                            <br />
                                        </div>
                                        <div style="float: left; width: 500px;">
                                            <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="cmbFrom" CssClass="error"
                                                ValidationGroup="Add" InitialValue="0" runat="server" ErrorMessage="Please select a Sales Channel name "></asp:RequiredFieldValidator>
                                        </div>

                                    </td>
                                </tr>
                                <asp:Panel ID="PnlHide" runat="server">
                                    <tr>
                                        <td align="right" class="formtext" valign="top">Stock Transfer To: <font class="error">*</font>
                                        </td>
                                        <td align="left" class="formtext" valign="top">
                                            <div style="float: left; width: 135px;">
                                                <asp:DropDownList ID="cmbTo" runat="server" CssClass="form_select">
                                                </asp:DropDownList>
                                                <br />
                                            </div>
                                            <div style="float: left; width: 135px;">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="cmbTo"
                                                    CssClass="error" ValidationGroup="Add" InitialValue="0" runat="server" ErrorMessage="Please select Sales Channel"></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                        <td align="right" class="formtext" valign="top">STN No.: <font class="error">*</font>
                                        </td>
                                        <td align="left" class="formtext" valign="top">
                                            <asp:TextBox ID="txtStnNo" runat="server" MaxLength="20" CssClass="form_input9"></asp:TextBox>
                                            <br />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtStnNo"
                                                CssClass="error" ValidationGroup="save" runat="server" ErrorMessage="Please enter STN No."></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="formtext" valign="top">Docket No.: <font class="error">*</font>
                                        </td>
                                        <td align="left" class="formtext" valign="top">
                                            <asp:TextBox ID="txtDocketNo" runat="server" MaxLength="20" CssClass="form_input9"></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtDocketNo"
                                                CssClass="error" ValidationGroup="save" runat="server" ErrorMessage="Please enter a docket no."></asp:RequiredFieldValidator>
                                        </td>
                                        <td align="right" class="formtext" valign="top">Transfer Date : <font class="error">*</font>
                                        </td>
                                        <td align="left" valign="top">
                                            <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                                                RangeErrorMessage="Date should be less then equal to current date." ValidationGroup="save" />
                                            <asp:Label ID="lblValidationDays" runat="server" Text="" CssClass="error"></asp:Label>
                                        </td>
                                    </tr>
                                </asp:Panel>

                                <%-- <tr>
                            <td align="left" valign="top">
                            </td>
                            <td align="left" valign="top">
                                <asp:Button ID="BtnSubmit" runat="server" Text="&nbsp;Search&nbsp;" CssClass="buttonbg"
                                    CausesValidation="true" Visible="false" ValidationGroup="Add" OnClick="BtnSubmit_Click" />
                            </td>
                        </tr>--%>
                            </table>
                        
                </div>
            </td>
        </tr>
        <tr>
            <td height="10">&nbsp;
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <asp:Panel ID="pnlGrid" runat="server">
                    <table id="tblGrid" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="left" class="tableposition">
                                <div class="mainheading">
                                    Enter Stock Transfer Details
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="contentbox">
                                    <%--#CC01 Add Start--%>
                                   
                                            <%--#CC01 Add End--%>
                                            <uc4:PartLookupClientSide ID="PartLookupClientSide1" runat="server" />
                                            <%--#CC01 Add Start--%>
                                       
                                    <%--#CC01 Add End--%>
                                    <div class="clear"></div>
                                </div>
                                <div class="clear"></div>
                            </td>
                        </tr>
                        <tr>
                            <td height="10"></td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="save"
                                    CausesValidation="true" OnClick="btnSave_Click" />
                                <asp:Button ID="btnReset" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="false"
                                    OnClick="btnReset_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td height="10"></td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td height="10"></td>
        </tr>
    </table>
    <%--  <div style="text-align: right; padding-bottom: 1em;">
        <button type="submit" id="submit1" name="submit1">
            Submit form</button>
    </div>--%>
</asp:Content>
