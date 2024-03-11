<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StockAdjustment.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Transactions_Common_StockAdjustment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />


    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

    <script language="javascript" type="text/javascript">
        function StockCheckAdjustment(tranQty) {

            if (isNaN(tranQty.value)) {
                alert("Only numeric value is allowed")

                tranQty.focus();
            }
            else if (tranQty.value.indexOf(".") != -1) {
                alert("value must be integer");
                tranQty.focus();
            }

            else {
                var stockid = tranQty.id;

                var str = stockid.substr(0, stockid.indexOf('txtQuantity'));

                var strDispatchedQnty = str + 'hdnid';

                var hdnscQutyval = parseInt(document.getElementById(strDispatchedQnty).value);
                if (tranQty.value < 0) {
                    if ((-1) * tranQty.value > hdnscQutyval) {
                        alert("Insufficient stock in hand");
                        tranQty.focus();
                    }
                }

            }
        }


        function popup() {
            var dropdownIndex = document.getElementById("ctl00_contentHolderMain_cmbChannelType").selectedIndex;
            var value = document.getElementById("ctl00_contentHolderMain_cmbChannelType").options[dropdownIndex].value;
            if (value == 0) {
                alert("Please Select SalesChannel Type");
                return false;
            }
            else {
               
                WinSearchChannelCode = dhtmlmodal.open("SearchSalesChannel", "iframe", "../../Masters/SalesChannel/ViewSalesChannelControlDetails.aspx?" + value, "Sales Channel Detail", "width=800px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
                WinSearchChannelCode.onclose = function() {

                    return true;
                }
               return false;
            }
        }
    </script>

    <script type="text/javascript">
  

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">

    <asp:HiddenField ID="hdnID" runat="server" />
    <asp:HiddenField ID="hdnName" runat="server" />
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Stock Adjustment
    </div>
    <div class="contentbox">
        <div class="mandatory float-left">
            (*) Marked fields are mandatory            
        </div>
        <div class="export">
            <asp:Button ID="btnCheck" runat="server" Text="&nbsp;Search Sales Channel&nbsp;" CssClass="buttonbg"
                CausesValidation="true" ValidationGroup="Entry" />
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Sales Channel Type: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="cmbChannelType" runat="server" CssClass="formselect" AutoPostBack="false">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="cmbChannelType" Display="Dynamic"
                        CssClass="error" ValidationGroup="Entry" InitialValue="0" runat="server" ErrorMessage="Please select a Sales Channel Type "></asp:RequiredFieldValidator>
                </li>
                <li class="text">Sales Channel Name: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtSalesChannelName" runat="server" MaxLength="20" CssClass="formfields" Enabled="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequInvoiceNumber" ControlToValidate="txtSalesChannelName" Display="Dynamic"
                        ValidationGroup="Entry" runat="server" CssClass="error" ErrorMessage="Please enter SalesChannel Name."></asp:RequiredFieldValidator>
                </li>
                <%--<td width="15%" height="35" align="right" class="formtext" valign="top">
                                        Sales Channel: <font class="error">*</font>
                                    </td>
                                    <td width="40%" align="left" class="formtext" valign="top">
                                        <div style="float:left; width:135px;"> <asp:DropDownList ID="cmbSalesChannel" runat="server" OnSelectedIndexChanged="cmbSalesChannel_SelectedIndexChanged"
                                            CssClass="form_select" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <br /></div>  <div style="float:left; width:300px;">
                                        <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="cmbSalesChannel" CssClass="error"
                                            ValidationGroup="Entry" InitialValue="0" runat="server" ErrorMessage="Please select a Sales Channel "></asp:RequiredFieldValidator></div>
                                    </td>--%>
            </ul>
            <ul>
                <asp:Panel ID="PnlHide" runat="server" Visible="false">
                    <li class="text">Opening Stock Date:
                    </li>
                    <li class="field">
                        <asp:Label ID="lblOpeningdate" runat="server"></asp:Label>
                    </li>
                    <li class="text">Stock Adjustment Date : <span class="error">*</span>
                    </li>
                    <li class="field">
                        <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                            RangeErrorMessage="Date should be less then equal to current date." ValidationGroup="save" />
                    </li>
                </asp:Panel>
            </ul>
            <ul>            
                <li class="field3">
                    <asp:Button ID="BtnSubmit" runat="server" Text="&nbsp;Go&nbsp;" CssClass="buttonbg"
                        CausesValidation="true" ValidationGroup="Entry" OnClick="BtnSubmit_Click" />
                </li>
            </ul>
        </div>
    </div>
    <asp:Panel ID="pnlGrid" runat="server">
        <div id="tblGrid">
            <div class="mainheading">
                Enter Stock Adjustment Details
            </div>

            <%-- <td>--%>
            <%-- <uc3:ucSalesEntryGrid ID="ucSalesEntryGrid1" runat="server" />--%>
            <%-- </td>--%>


            <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="contentbox">
                        <div class="grid1">
                            <asp:GridView ID="gvStockEntry" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                CellSpacing="1" DataKeyNames="SKUID" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                                GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow" EmptyDataRowStyle-CssClass="Emptyrow"
                                AlternatingRowStyle-CssClass="Altrow" Width="100%" OnRowDataBound="gvStockEntry_RowDataBound">
                                <RowStyle CssClass="gridrow" />
                                <Columns>
                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUName"
                                        HeaderText="SKU Name"></asp:BoundField>
                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUCode"
                                        HeaderText="SKU Code"></asp:BoundField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Stock In Hand"
                                        ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStockInhand" runat="server" Text='<%# Eval("StockInhand") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Quantity" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSKUID" runat="server" Text='<%# Eval("SKUID") %>' Visible="false"></asp:Label>
                                            <asp:TextBox ID="txtQuantity" runat="server" MaxLength="8" Text='<%# Eval("Quantity") %>'
                                                CssClass="formfields"></asp:TextBox>
                                            <asp:HiddenField ID="hdnid" runat="server" Value='<%# Eval("StockInhand")%>' />
                                            <cc1:FilteredTextBoxExtender ID="ftbeOs" runat="server" FilterType="Custom" ValidChars="-0123456789"
                                                TargetControlID="txtQuantity" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="gridheader" />
                                <EditRowStyle CssClass="editrow" />
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="float-margin">
                <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="save"
                    CausesValidation="true" OnClick="btnSave_Click" />
            </div>
            <div class="float-margin">
                <asp:Button ID="btnReset" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="false"
                    OnClick="btnReset_Click" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
