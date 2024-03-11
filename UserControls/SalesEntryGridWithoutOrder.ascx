<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SalesEntryGridWithoutOrder.ascx.cs"
    Inherits="UserControls_WebUserControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>

<script language="javascript" type="text/javascript">

    function StockCheck(tranQty) {

        if (isNaN(tranQty.value)) {
            alert("Only numeric value is allowed")

            tranQty.focus();
        }

        else if (tranQty.value < 0) {
            alert("negative value is not allowed");

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


            if (tranQty.value > hdnscQutyval) {
                alert("Insufficient stock in hand");
                //                alert("Quantity should be less than stock in hand");
                tranQty.focus();
            }
            ShowTotal()
        }
    }
    function StockCheckSales(tranQty) {

        if (isNaN(tranQty.value)) {
            alert("Only numeric value is allowed")

            tranQty.focus();
        }

        else if (tranQty.value < 0) {
            alert("negative value is not allowed");

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
            var strDispatchedQnty1 = str + 'HdnQty';

            var hdnscQutyval = parseInt(document.getElementById(strDispatchedQnty).value);
            var hdnscQutyval1 = parseInt(document.getElementById(strDispatchedQnty1).value);
            if (hdnscQutyval1 == 0) {
                if (tranQty.value > hdnscQutyval) {
                    alert("Insufficient stock in hand");
                    //                alert("Quantity should be less than stock in hand");
                    tranQty.focus();
                }
            }
            else if (hdnscQutyval1 != 0) {
                if (hdnscQutyval - (tranQty.value - hdnscQutyval1) < 0) {
                    alert("Insufficient stock in hand");

                    tranQty.focus();
                }
            }
            ShowTotal();

        }
    }
    function ShowTotal() {
        var gridview = document.getElementById('<%= gvStockEntry.ClientID %>');
        var txtsumTotal = document.getElementById('<%= txtsumTotal.ClientID %>');
        var gridViewControls = gridview.getElementsByTagName("input");
        var Sum = 0;
        for (i = 0; i <= gridViewControls.length - 1; i++) {
            // if this input type is button, disable
            if (gridViewControls[i].type == "text") {
                var insertedvalue = 0;
                var insertedvalue = Math.round(gridViewControls[i].value);
                Sum = parseInt(Sum) + insertedvalue;

            }
        }
        txtsumTotal.innerText = Sum;

    }

</script>

<uc1:ucMessage ID="ucMessage1" runat="server" ShowCloseButton="false" />
<div class="contentbox">
    <div class="float-right">
        <div class="float-margin">
            <span class="formtext2">Total :</span>
        </div>
        <div class="float-margin">
            <asp:TextBox ID="txtsumTotal" runat="server" Text="" CssClass="form_select" Enabled="false"></asp:TextBox></div>
    </div>
    <div class="clear" style="height: 4px;">
    </div>
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
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Quantity" ShowHeader="False">
                    <ItemTemplate>
                        <asp:Label ID="lblSKUID" runat="server" Text='<%# Eval("SKUID") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtQuantity" runat="server" MaxLength="8" Text='<%# Eval("Quantity") %>'
                            CssClass="form_select"></asp:TextBox>
                        <asp:HiddenField ID="hdnid" runat="server" Value='<%# Eval("StockInhand")%>' />
                        <asp:HiddenField ID="HdnQty" runat="server" Value='<%# Eval("Quantity")%>' />
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
