<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SalesReturnGrid.ascx.cs"
    Inherits="UserControls_SalesReturnGrid" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>

<script language="javascript" type="text/javascript">
    function SalesQtyCheck(tranQty) {

        if (isNaN(tranQty.value)) {
            alert("Only numeric value is allowed")

            tranQty.focus();
        }

        else if (tranQty.value < 0) {
            alert("negative value is not allowed");

            tranQty.focus();
        }
        else if (tranQty.value == 0) {
            alert("zero value is not allowed");

            tranQty.focus();
        }
        else if (tranQty.value.indexOf(".") != -1) {
            alert("value must be integer");
            tranQty.focus();
        }

//        else {
//            var stockid = tranQty.id;

//            var str = stockid.substr(0, stockid.indexOf('txtQuantity'));
//           
//            var strDispatchedQnty = str + 'hdnid';
//            var strDispatchedQnty1 = str + 'hdnStockInhand';
//        
//            var hdnscQutyval = parseInt(document.getElementById(strDispatchedQnty).value);
//            var hdnscQutyval1 = parseInt(document.getElementById(strDispatchedQnty1).value);
//            if (tranQty.value > hdnscQutyval ) {
//                alert("Return quantity should not be greater than remaining quantity.");
//                           
//                tranQty.focus();
//            }
////           else if (tranQty.value > hdnscQutyval1) {
////                alert("Return quantity should not be greater than stock in hand.");
////                tranQty.focus();
////            }
//        }
    }
    function StockInhandCheck(tranQty) {

        if (isNaN(tranQty.value)) {
            alert("Only numeric value is allowed")

            tranQty.focus();
        }

        else if (tranQty.value < 0) {
            alert("negative value is not allowed");

            tranQty.focus();
        }
        else if (tranQty.value == 0) {
            alert("zero value is not allowed");

            tranQty.focus();
        }
        else if (tranQty.value.indexOf(".") != -1) {
            alert("value must be integer");
            tranQty.focus();
        }

        else {
            var stockid = tranQty.id;

            var str = stockid.substr(0, stockid.indexOf('txtQuantity'));
            var strDispatchedQnty1  = str + 'hdnStockInhand';         
            var hdnscQutyval1 = parseInt(document.getElementById(strDispatchedQnty1).value);
            if (tranQty.value > hdnscQutyval1) {
                alert("Return quantity should not be greater than stock in hand.");
             
                tranQty.focus();
            }
        }
    }
</script>

<uc1:ucMessage ID="ucMessage1" runat="server" ShowCloseButton="false" />
<div class="contentbox">
    <div class="grid1">
        <asp:GridView ID="gvStockEntry" runat="server" AutoGenerateColumns="False" CellPadding="4"
            CellSpacing="1" DataKeyNames="SKUID" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
            GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
            AlternatingRowStyle-CssClass="gridrow1" Width="100%" OnRowCommand="gvStockEntry_RowCommand"
            OnRowDataBound="gvStockEntry_RowDataBound">
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
                 <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesQty"
                    HeaderText="Sales Quantity"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="RemainingQty"
                    HeaderText="Permissible Return Quantity"></asp:BoundField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Return Quantity" ShowHeader="False">
                    <ItemTemplate>
                        <asp:Label ID="lblSKUID" runat="server" Text='<%# Eval("SKUID") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtQuantity" runat="server" MaxLength="8" Text='<%# Eval("ReturnQty") %>'
                            CssClass="form_input2"></asp:TextBox>
                       <asp:HiddenField ID="hdnid" runat="server" Value='<%# Eval("RemainingQty")%>' />
                        <asp:HiddenField ID="hdnStockInhand" runat="server" Value='<%# Eval("StockInhand")%>' />
                       <cc1:FilteredTextBoxExtender ID="ftbeOs" runat="server" FilterMode="ValidChars" FilterType="Numbers"
                            TargetControlID="txtQuantity" />
                        <%-- <asp:RangeValidator ID="rng" runat="server" ControlToValidate="txtQuantity"  Display="Dynamic"
                                                        ></asp:RangeValidator>--%>
                        <%-- <asp:RequiredFieldValidator ID="reqV" runat="server" ControlToValidate="txtQuantity"
                                                            CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Qty" ></asp:RequiredFieldValidator>--%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="gridheader" />
            <EditRowStyle CssClass="editrow" />
        </asp:GridView>
    </div>
</div>
