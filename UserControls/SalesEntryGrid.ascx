<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SalesEntryGrid.ascx.cs"
    Inherits="UserControls_SalesEntryGrid" %>
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

        }
    }

    function ChangeMe(oRate, oAmount, oQty) {

        var lblAmountNew = document.getElementById(oAmount);
        var Rate = document.getElementById(oRate).firstChild.data;
        var Qty = document.getElementById(oQty).value;
        var Lbl = document.getElementById('<%=lblinfo.ClientID %>');
        var lblnetamountvalue = Lbl.outerText;
        var lblAmountvalue = lblAmountNew.value;
       
        var amount = Rate * Qty;
        lblAmountNew.value = amount;
        if (lblnetamountvalue != "" && (lblnetamountvalue) != "0") {
            if (lblAmountvalue != "")
            {
                Lbl.innerHTML = parseInt(lblnetamountvalue) - parseInt(lblAmountvalue);
            }
        } else {
            lblnetamountvalue = "0";
        }
        lblnetamountvalue = Lbl.innerHTML;     
        Lbl.innerHTML = parseInt(lblnetamountvalue) + amount;

        
    }
  

</script>

<uc1:ucMessage ID="ucMessage1" runat="server" ShowCloseButton="false" />

<div class="grid1">
    <asp:GridView ID="gvStockEntry" runat="server" AutoGenerateColumns="False" CellPadding="4"
        CellSpacing="1" DataKeyNames="SKUID" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
        GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
        AlternatingRowStyle-CssClass="Altrow" Width="100%" OnRowCommand="gvStockEntry_RowCommand"
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
            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Stock In Hand(Warehouse)"
                ShowHeader="False">
                <ItemTemplate>
                    <asp:Label ID="lblStockInhandPranet" runat="server" Text='<%# Eval("StockInhandParent") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderNumber"
                HeaderText="Order Number"></asp:BoundField>
            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderDate"
                HeaderText="Order Date"></asp:BoundField>
            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderQty"
                HeaderText="Order Quantity"></asp:BoundField>
            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Rate">
                <ItemTemplate>
                    <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Quantity" ShowHeader="False">
                <ItemTemplate>
                    <asp:Label ID="lblSKUID" runat="server" Text='<%# Eval("SKUID") %>' Visible="false"></asp:Label>
                    <asp:TextBox ID="txtQuantity" runat="server" MaxLength="8" Text='<%# Eval("Quantity") %>'
                        CssClass="formfields"></asp:TextBox>
                    <asp:HiddenField ID="hdnid" runat="server" Value='<%# Eval("StockInhand")%>' />
                    <asp:HiddenField ID="HdnQty" runat="server" Value='<%# Eval("Quantity")%>' />
                    <cc1:FilteredTextBoxExtender ID="ftbeOs" runat="server" FilterMode="ValidChars" FilterType="Numbers"
                        TargetControlID="txtQuantity" />

                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
            </asp:TemplateField>


            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Amount">
                <ItemTemplate>
                    <asp:TextBox ID="lblAmount" runat="server" CssClass="formfields"></asp:TextBox>
                    <%--<asp:Label ID="lblAmount" runat="server"></asp:Label>--%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle CssClass="gridheader" />
        <EditRowStyle CssClass="editrow" />
    </asp:GridView>
</div>
<div class="clear padding"></div>
<div>
    <asp:Label ID="lbl" runat="server" Text="Total : " CssClass="formtext"></asp:Label>
    <asp:Label ID="lblinfo" runat="server"></asp:Label>
</div>

