<%@ Page Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    EnableEventValidation="false" AutoEventWireup="true" CodeFile="ManageSalesOrder.aspx.cs"
    Inherits="Transactions_Billing_ManageSalesOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc8" %>

<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucTextboxMultiline.ascx" TagName="ucTextboxMultiline"
    TagPrefix="uc6" %>
<%@ Register Src="~/UserControls/GridClientSide.ascx" TagName="GridClientSide"
    TagPrefix="uc7" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../../Assets/Css/dhtmlwindow.css" rel="stylesheet" />
    <link href="../../Assets/Css/modal.css" rel="stylesheet" />
    <script src="../../Assets/Scripts/modal.js"></script>
    <script src="../../Assets/Scripts/dhtmlwindow1.js"></script>

    <style type="text/css">
        .hidden {
            display: none;
        }
    </style>

    <script language="javascript" type="text/javascript">

        function OnCheckUnCheck(spanChk, Quantity, rfvQuantity) {
            var varchkSelect = document.getElementById(spanChk);
            var chkPart = spanChk;
            var qty = document.getElementById(Quantity);

            if (!varchkSelect.checked) {
                qty.disabled = true;
            }
            else {
                var count = 1;
                var collection = document.getElementById('ctl00_contentHolderMain_grdvList').getElementsByTagName('INPUT');
                for (var x = 0; x < collection.length; x++) {
                    if (collection[x].type.toUpperCase() == 'CHECKBOX') {
                        if (!collection[x].checked) {
                            count = 0;

                        }
                    }
                    if (varchkSelect.checked = true) {
                        qty.disabled = false;


                    }
                }
            }
        }

        function popup(orderid) {
            var WinProductDetails = dhtmlmodal.open("WinPopup1", "iframe", "../../Transactions/Billing/SalesOrderPrint.aspx?ReferenceID=" + orderid, "Print Order", "width=800px,height=550px,top=25,resize=0,scrolling=auto ,center=1")
            WinProductDetails.onclose = function () {
                var btn = document.getElementById("ctl00_contentHolderMain_btn");
                __doPostBack(btn.name, "OnClick");
                return true;
            }
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div class="box1">
        <asp:HiddenField ID="txthattrexit" Value="0" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <uc8:ucMessage ID="ucMessage1" runat="server" Style="display: none" />
                <div class="clear">
                </div>
                <div id="divMsg" runat="server" style="display: none;">
                    <span class="float-margin">
                        <asp:LinkButton ID="hlkFinal" runat="server"></asp:LinkButton></span>
                </div>
                <div class="clear">
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="click" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="subheading">
            Order
        </div>
    </div>
    <div class="box1">
        <div class="innerarea" id="PnlOrder" runat="server">
            <div class="H30-C3-S">
                <asp:Panel ID="pmlCmbControls" runat="server">
                    <asp:Label ID="Button1" Text="0" runat="server" Style="display: none"></asp:Label>
                    <ul>
                        <li class="text">Order From:<span class="error">*</span></li>
                        <li class="field">
                            <asp:DropDownList ID="cmbOrderFrom" runat="server" CssClass="formselect" ValidationGroup="order"
                                AutoPostBack="True" OnSelectedIndexChanged="cmbOrderFrom_SelectedIndexChanged1" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="cmbOrderFrom" Display="Dynamic"
                                CssClass="error" ValidationGroup="order" InitialValue="0" runat="server" ErrorMessage="Please Select Order From."></asp:RequiredFieldValidator>
                        </li>
                        <li class="text">Order To:<span class="error">*</span></li>
                        <li class="field">
                            <asp:DropDownList ID="cmbOrderTo1" runat="server" ValidationGroup="order" CssClass="formselect" />

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="cmbOrderTo1" Display="Dynamic"
                                CssClass="error" ValidationGroup="order" InitialValue="0" runat="server" ErrorMessage="Please Select Order To."></asp:RequiredFieldValidator>
                        </li>
                        <li class="text">Ref Number:</li>

                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtPoNumber" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                            </div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPoNumber"
                                CssClass="error" Display="Dynamic" ValidationExpression="[^&lt;&gt;/\@%()]{1,50}"
                                ValidationGroup="order"></asp:RegularExpressionValidator>
                        </li>
                        <li class="text">Order Date:<span class="mandatory-img">&nbsp;</span></li>
                        <li class="field">
                            <uc1:ucDatePicker ID="ucOrderDate" ErrorMessage="Please select date!" ValidationGroup="order"
                                runat="server" IsEnabled="true" />

                        </li>
                        <li class="text">Remarks:</li>
                        <li class="field">
                            <uc6:ucTextboxMultiline ID="txtRemarks" runat="server" CharsLength="250" TextBoxWatermarkText="Enter Order Remarks" />
                        </li>
                        <li class="text" id="dyctrl"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnLoad" runat="server" CssClass="buttonbg" Text="Next" OnClick="btnLoad_Click"
                                    ValidationGroup="order" CausesValidation="true" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" OnClick="btnCancel_Click"
                                    Text="Cancel" CausesValidation="false" />
                            </div>
                        </li>
                    </ul>
                </asp:Panel>
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlLabel" runat="server" Visible="false">
        <asp:Label ID="Label1" Text="0" runat="server" Style="display: none"></asp:Label>
        <div class="box1">
            <div class="innerarea">
                <div class="H30-C3-S">
                    <ul>
                        <li class="text">Order From:</li>
                        <li class="field">
                            <asp:Label ID="lblOrderFrom" runat="server" CssClass="frmtxt1"></asp:Label>
                            <asp:HiddenField ID="hdnfOrderFromId" runat="server" />
                            <asp:HiddenField ID="hdnfEntityId" runat="server" />
                            <asp:HiddenField ID="hdnfSalesOrderId" runat="server" />
                            <asp:HiddenField ID="hdfOrderType" runat="server" Value="1" />

                        </li>
                        <li class="text">Order To:</li>
                        <li class="field">
                            <asp:Label ID="lblOrderTo" runat="server" CssClass="frmtxt1"></asp:Label>
                            <asp:HiddenField ID="hdnfOrderToId" runat="server" />
                        </li>
                        <li class="text">Ref Number:</li>
                        <li class="field">
                            <asp:Label ID="lblRefNumber" runat="server" CssClass="frmtxt1"></asp:Label>
                        </li>
                        <li class="text">Order Date:</li>
                        <li class="field">
                            <asp:Label ID="lblOrderDate" runat="server" CssClass="frmtxt1"></asp:Label>
                        </li>
                        <li class="text">Remarks:</li>
                        <li class="field">
                            <asp:Label ID="lblRemarks" runat="server" CssClass="frmtxt1"></asp:Label>
                        </li>
                        <li class="text">Order Number:</li>
                        <li class="field">
                            <asp:Label ID="lblOrderNumber" runat="server" CssClass="frmtxt1"></asp:Label>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:UpdatePanel ID="updSearch" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div runat="server" id="div2">
                <div class="box1">
                    <div class="subheading">
                        Search SKU/Product
                    </div>
                    <div class="innerarea">
                        <div class="H30-C3-S">
                            <ul>
                                <li class="text">SKU Name:<span class="optional-img">&nbsp;</span></li>
                                <li class="field">
                                    <asp:TextBox ID="txtSerPartName" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>

                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtenderPartName" runat="server" TargetControlID="txtSerPartName"
                                        MinimumPrefixLength="3" ContextKey="0"
                                        CompletionSetCount="5" ServiceMethod="GetSKUNameList" ServicePath="../../CommonService.asmx">
                                    </cc1:AutoCompleteExtender>
                                </li>
                                <li class="text">SKU Code:<span class="optional-img">&nbsp;</span></li>
                                <li class="field">
                                    <asp:TextBox ID="txtPartCode" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>

                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtenderPartCode" runat="server" TargetControlID="txtPartCode"
                                        MinimumPrefixLength="3" ContextKey="Partcode" CompletionSetCount="5"
                                        ServiceMethod="GetSKUListByCodesList" ServicePath="../../CommonService.asmx">
                                    </cc1:AutoCompleteExtender>

                                </li>
                                <li class="text">Model Name:<span class="optional-img">&nbsp;</span></li>

                                <li class="field">
                                    <div>
                                        <asp:TextBox ID="txtModelNumber" class="formfields" runat="server"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtenderModel" runat="server" TargetControlID="txtModelNumber"
                                            MinimumPrefixLength="3" ContextKey="ModelName"
                                            CompletionSetCount="5" ServiceMethod="GetModelNameList" ServicePath="../../CommonService.asmx">
                                        </cc1:AutoCompleteExtender>

                                    </div>
                                </li>
                                <li class="text"></li>
                                <li class="field">
                                    <div class="float-margin">
                                        <asp:Button ID="btnSearch" runat="server" CssClass="buttonbg" Text="Search" OnClick="btnSearch_Click"
                                            ValidationGroup="Search" />
                                    </div>
                                    <div class="float-left">
                                        <asp:Button ID="DownloadProductPartMappingList" ValidationGroup="Search" runat="server" CssClass="buttonbg" Text="Download SKU List" OnClick="DownloadProductPartMappingList_Click" />
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="box1" runat="server" id="divgrd">
                    <div class="subheading">
                        <div class="float-left">
                            SKU/Product List
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="innerarea">

                        <div class="content">
                            <span class="frmtxt2" id="Currentledgerbalanceid" runat="server">Current ledger balance:</span>
                            <asp:Label ID="lblCurrLedgerBal" runat="server" Text="0"></asp:Label>
                        </div>
                        <div class="clear2">
                        </div>
                        <div class="grid1">
                            <asp:GridView ID="grdvList" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                SelectedRowStyle-CssClass="selectedrow" CellSpacing="1" DataKeyNames="PartId"
                                EmptyDataText="No Record Found" GridLines="None" Width="100%" HeaderStyle-CssClass="gridheader"
                                RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" EditRowStyle-CssClass="editrow"
                                OnRowDataBound="grdvList_RowDataBound">
                                <RowStyle CssClass="gridrow" />
                                <SelectedRowStyle CssClass="selectedrow" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkboxPartID" runat="server" Checked="false" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SKU Name">
                                        <ItemTemplate>
                                            <div style="width: 250px; overflow: hidden; word-wrap: break-word;">
                                                <asp:Label ID="lblPartname" runat="server" Text='<%# Bind("PartName") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SKU Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartCode" runat="server" Text='<%# Bind("PartCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="0px" ItemStyle-Width="0px" ItemStyle-CssClass="hidden"
                                        HeaderStyle-CssClass="hidden">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMSL" runat="server" Text="0" Style="display: none"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="0px" ItemStyle-Width="0px" ItemStyle-CssClass="hidden"
                                        HeaderStyle-CssClass="hidden">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBackOrderQty" runat="server" Text="0" Style="display: none"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="0px" ItemStyle-Width="0px" ItemStyle-CssClass="hidden"
                                        HeaderStyle-CssClass="hidden">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOpenQty" runat="server" Text="0" Style="display: none"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Stock in Hand(Order From)" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("FromStock") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="Label51" runat="server" Text='<%# Bind("PartID") %>' Style="display: none"></asp:Label>
                                            <asp:HiddenField ID="hdfPartID" runat="server" Value='<%# Eval("PartID") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" CssClass="text-hidden" />
                                        <ItemStyle CssClass="text-hidden" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="formfields-W70" MaxLength="5"
                                                Enabled="false"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="ftQuantity" runat="server" ValidChars="0123456789"
                                                FilterMode="ValidChars" TargetControlID="txtQuantity">
                                            </cc1:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity"
                                                CssClass="error" Display="Dynamic" ForeColor="" ValidationGroup="vrgpSubmit"> </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfGrossAmount" runat="server" />
                                            <asp:HiddenField ID="hdfnetAmount" runat="server" />
                                            <asp:HiddenField ID="hdnfID" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Stock In Hand(Order To)" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("ToStock") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Price">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPrice" runat="server" CssClass="formfields-W70" Text='<%# Bind("GrossPrice") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tax Price">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTaxPrice" runat="server" Text='<%# Bind("NetPrice") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                                <SelectedRowStyle CssClass="selectedrow" />
                                <HeaderStyle CssClass="gridheader" />
                                <EditRowStyle CssClass="editrow" />
                                <SelectedRowStyle CssClass="selectedrow" />
                                <AlternatingRowStyle CssClass="Altrow" />
                            </asp:GridView>
                        </div>
                        <div id="dvFooter" runat="server" class="pagination">
                            <uc3:UCPagingControl ID="ucPagingControl1" runat="server" Visible="false" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                        </div>
                        <div class="clear2">
                        </div>
                        <div>
                            <asp:Button ID="btnAddtolist" runat="server" OnClick="btnAddtolist_Click" CausesValidation="false" CssClass="buttonbg" Text="Add to list" ValidationGroup="vrgpSubmit" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="grdvList" />
            <asp:PostBackTrigger ControlID="DownloadProductPartMappingList" />
            <asp:AsyncPostBackTrigger ControlID="btnAddtolist" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdPnlPart" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div runat="server" id="div3">
                <div class="box1">
                    <div class="innerarea">
                        <div class="grid1">
                            <asp:GridView ID="grdvPart" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                SelectedRowStyle-CssClass="selectedrow" CellSpacing="1" DataKeyNames="PartId"
                                EmptyDataText="No Record Found" GridLines="None" Width="100%" HeaderStyle-CssClass="gridheader"
                                RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" EditRowStyle-CssClass="editrow"
                                OnRowCommand="grdvPart_RowCommand">
                                <RowStyle CssClass="gridrow" />
                                <SelectedRowStyle CssClass="selectedrow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SKU Name">
                                        <ItemTemplate>
                                            <div style="width: 100px; overflow: hidden; word-wrap: break-word;">
                                                <asp:Label ID="lblPartname" runat="server" Text='<%# Bind("SKUName") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SKU Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartCode" runat="server" Text='<%# Bind("SKUCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="100px" ItemStyle-CssClass="hidden"
                                        HeaderStyle-CssClass="hidden">
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Amount" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("GrossAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remove" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Button ID="btnRemove" runat="server" Text='Remove' CommandName="Remove" CausesValidation="false" CssClass="buttonbg" CommandArgument='<%# Bind("PartId") %>'></asp:Button>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                </Columns>
                                <SelectedRowStyle CssClass="selectedrow" />
                                <HeaderStyle CssClass="gridheader" />
                                <EditRowStyle CssClass="editrow" />
                                <SelectedRowStyle CssClass="selectedrow" />
                                <AlternatingRowStyle CssClass="Altrow" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="box1" id="divlast" runat="server" visible="false">
                <div class="clear">
                </div>
                <div class="padding">
                    <asp:Button ID="btnSave" runat="server" CssClass="buttonbg" Text="Save" CausesValidation="false"
                        OnClick="btnSave_Click" ValidationGroup="VrgSubmit" />
                    <asp:Button ID="btnCancelGrd" runat="server" CssClass="buttonbg" Text="Cancel" CausesValidation="false"
                        OnClick="btnCancelGrd_Click" />
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="grdvPart" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
