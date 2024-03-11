﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageInterfacePrimarySalesReturn1-SB.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Transactions_SalesChannelSBReturn_Interface_ManageInterfacePrimarySalesReturn1_SB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/PartLookupClientSideForReturn.ascx" TagName="PartLookupClientSide"
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

    <script type="text/javascript">
        function popup() {
            WinSearchChannelCode = dhtmlmodal.open("SearchSalesChannelCode", "iframe", "../../SalesChannel/SearchSalesChannelCode.aspx", "Sales Channel Detail", "width=800px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
            WinSearchChannelCode.onclose = function () {
                return true;
            }
            return false;
        }
        function BlankTo() {
            document.getElementById('<%= hdnName.ClientID  %>').value = '';

        }


    </script>

    <script type="text/javascript">
        // <![CDATA[ 
        var startTime;
        function OnBeginCallback(s, e) {
            startTime = new Date();
        }
        function OnEndCallback(s, e) {
            var result = new Date() - startTime;
            result /= 1000;
            result = result.toString();
            if (result.length > 4)
                result = result.substr(0, 4);
            ClientTimeLabel.SetText(result.toString() + " sec");
            ClientCaptionLabel.SetText("Time to retrieve the last data:");
        }
        // ]]> 
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
            <asp:HiddenField ID="hdnDirectSalesOfSerialAllowed" Value="0" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Primary Sales Return Entry
    </div>
    <div class="contentbox">
        <div class="float-left">
            <div class="mandatory">
                (*) Marked fields are mandatory
            </div>
        </div>
        <div class="float-right">
            <asp:Button ID="btnsearch" runat="server" Text="Search Sales Channel" CssClass="buttonbg"
                OnClick="btnsearch_Click" Enabled="true" />
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Select Mode:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:RadioButtonList ID="rdModelList" runat="server" CssClass="radio-rs" TextAlign="Right" RepeatDirection="Horizontal"
                        CellPadding="2" CellSpacing="0" BorderWidth="0" AutoPostBack="True" OnSelectedIndexChanged="rdModelList_SelectedIndexChanged">
                        <asp:ListItem Text="Excel" Value="0"></asp:ListItem>
                        <%--<asp:ListItem Text="Interface" Value="1" Selected="True"></asp:ListItem>--%>
                        <asp:ListItem Text="On-Screen Entry" Value="1" Selected="True"></asp:ListItem>
                    </asp:RadioButtonList>
                </li>
                <%--  </td>
                                         <td align="right" valign="top" colspan="2">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                               <tr>
                                       
                                         </tr>
                                           </table>
                                        </td>
                                        <td valign="top" align="right" class="formtext" width="52%">
                                            Invoice Date:<span class="error">*</span>
                                        </td>
                                        <td valign="top" align="left" width="48%">
                                            <uc2:ucDatePicker ID="ucInvoiceDate" runat="server" ErrorMessage="Invalid date."
                                                defaultDateRange="true" />
                                                <asp:Label ID="lblInvoiceDAte" runat="server" Text="" Visible="false"></asp:Label>
                                        </td>--%>
                </ul>
            <div class="clear"></div>
            <ul>
                <li class="text">Select <asp:Label ID="lblChange" runat="server"></asp:Label>
                    <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="formselect" AutoPostBack="false">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlWarehouse" CssClass="error" Display="Dynamic"
                            ValidationGroup="EntryValidation" InitialValue="0" runat="server" ErrorMessage="Please select Warehouse Name."></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">Select
                                            <asp:Label ID="lblChangeto" runat="server"></asp:Label>: <span class="error">*</span>
                </li>
                <li class="field">
                    <%--   <asp:DropDownList ID="ddlTD" runat="server" CssClass="form_select">
                                            </asp:DropDownList>
                                            <div style="width: 100%;">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlTD"
                                                    CssClass="error" ValidationGroup="EntryValidation" InitialValue="0" runat="server"
                                                    ErrorMessage="Please select." Display="Dynamic"></asp:RequiredFieldValidator></div>--%>
                    <asp:TextBox ID="txtSearchedName" runat="server" CssClass="formfields" Enabled="false"></asp:TextBox>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtSearchedName" Display="Dynamic"
                            ValidationGroup="EntryValidation" runat="server" CssClass="error" ErrorMessage="Please Enter Sales Channel To"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">Return Date:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                        ValidationGroup="EntryValidation" />
                    <asp:Label ID="lblValidationDays" runat="server" Text="" CssClass="error"></asp:Label>
                </li>
            </ul>
            <ul runat="server" id="dvStockBinType" visible="true">
                <li class="text">Select Stock Type: <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlStockBinType" runat="server" CssClass="formselect">
                            <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="reqStockBinType" ControlToValidate="ddlStockBinType" Display="Dynamic"
                            CssClass="error" ValidationGroup="EntryValidation" InitialValue="0" runat="server"
                            ErrorMessage="Please Select Stock Type"></asp:RequiredFieldValidator>
                    </div>
                </li>
            </ul>
            <ul>
                <li class="text"></li>
                <li class="field">
                    <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="buttonbg" Enabled="true"
                        ValidationGroup="EntryValidation" OnClick="btnGo_Click" CausesValidation="true" />
                </li>
                <li>
                    <asp:HiddenField ID="hdnCode" runat="server" />
                    <asp:HiddenField ID="hdnName" runat="server" />
                    <asp:HiddenField ID="hdnSalesChannelID" runat="server" />
                </li>
            </ul>
        </div>
    </div>
    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
        <div id="tblGrid" runat="server">
            <div class="mainheading">
                Enter Details                                                
            </div>
            <div class="contentbox">
                <uc4:PartLookupClientSide ID="PartLookupClientSide1" runat="server" />
            </div>
            <div class="margin-bottom">
                <div class="float-margin">
                <asp:Button ID="btnSave1" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="EntryValidation"
                    CausesValidation="true" />
                </div>
                <div class="float-margin">
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" OnClientClick="BlankTo();"
                    OnClick="btnReset_Click" />
                </div>
            </div>
            <div class="clear"></div>
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
    </asp:Panel>
</asp:Content>
