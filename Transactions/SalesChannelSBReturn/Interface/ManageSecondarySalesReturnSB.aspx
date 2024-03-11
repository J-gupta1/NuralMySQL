<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageSecondarySalesReturnSB.aspx.cs" Inherits="Transactions_SalesChannelSBReturn_Interface_ManageSecondarySalesReturnSB" %>

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
        Secondary Sales Return Entry
    </div>

    <div class="contentbox">
        <%--#CC05 Add Start--%>
        <asp:UpdatePanel ID="updddl" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <%--#CC05 Add End--%>
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Select Mode:<span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:RadioButtonList ID="rdModelList" runat="server" CssClass="radio-rs" TextAlign="Right" RepeatDirection="Horizontal"
                                CellPadding="2" CellSpacing="0" BorderWidth="0" AutoPostBack="True" OnSelectedIndexChanged="rdModelList_SelectedIndexChanged">
                                <asp:ListItem Text="Excel" Value="0"> </asp:ListItem>
                                <asp:ListItem Text="On-Screen Entry" Value="1" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </li>
                    </ul>
                    <div class="clear"></div>
                    <ul>
                        <li class="text">Select
                                            <asp:Label ID="lblChange" runat="server"></asp:Label>
                            <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlSalesChannel" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlWarehouse_SelectedIndexChanged"
                                    AutoPostBack="false">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlSalesChannel" CssClass="error"
                                    ValidationGroup="EntryValidation" InitialValue="0" runat="server" ErrorMessage="Please select Warehouse Name."></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Select Salesman:  <span id="spanSalesManOptional" runat="server"><span class="error">*</span></span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlSalesman" runat="server" CssClass="formselect" OnSelectedIndexChanged="ddlSalesman_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlSalesman"
                                    CssClass="error" ValidationGroup="EntryValidation" InitialValue="0" runat="server"
                                    ErrorMessage="Please Select Salesman"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Select Retailer: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlRetailer" runat="server" CssClass="formselect">
                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlRetailerForCode" runat="server" CssClass="formselect" Visible="false">
                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="reqRetailer" ControlToValidate="ddlRetailer" CssClass="error"
                                    ValidationGroup="EntryValidation" InitialValue="0" runat="server" ErrorMessage="Please Select Retailer"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Return Date: <span class="error">*</span>
                        </li>
                        <li class="field" style="height:auto">
                            <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                                ValidationGroup="EntryValidation" />
                            <div>
                                <asp:Label ID="lblValidationDays" runat="server" Text="" CssClass="error"></asp:Label>
                            </div>
                        </li>
                    </ul>
                    <ul runat="server" id="dvStockBinType" style="display: none">
                        <li class="text">Select Stock Type: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlStockBinType" runat="server" CssClass="formselect">
                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="reqStockBinType" ControlToValidate="ddlStockBinType"
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
                            <asp:HiddenField ID="hdnCode" runat="server" />
                            <asp:HiddenField ID="hdnName" runat="server" />
                        </li>
                    </ul>
                </div>
                <%--#CC05 Add Start--%>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnGo" />
            </Triggers>
        </asp:UpdatePanel>
        <%--#CC05 Add End--%>
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
        </div>
        <div id="tblDirectSerialPanel" runat="server">
            <div class="mainheading">
                Enter Serial No.
            </div>
            <div class="contentbox">
                <div class="H35-C3-S">
                    <ul>
                        <li class="text">Serial No.:<span class="error">*</span>
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
