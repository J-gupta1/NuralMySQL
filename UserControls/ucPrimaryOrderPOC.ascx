<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucPrimaryOrderPOC.ascx.cs" Inherits="UserControls_PrimaryOrder" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>

<script language="javascript" type="text/javascript">
  
    function ChangeMe(oRate, oAmount, oQty) {
        debugger;
        var lblAmountNew = document.getElementById(oAmount);
        var Rate = document.getElementById(oRate).outerText;
        var Qty = document.getElementById(oQty).value;
       
        var Lbl = document.getElementById('<%=lblinfo.ClientID %>');
        var lblnetamountvalue = Lbl.outerText;
        var lblAmountvalue = lblAmountNew.value;
        if (lblnetamountvalue!="" && isNaN(lblnetamountvalue) == false) {
            Lbl.innerHTML = parseInt(lblnetamountvalue) - parseInt(lblAmountvalue);
        } else {
                lblnetamountvalue = "0";
        }

        var amount = Rate * Qty;
        lblAmountNew.value = amount;       
        Lbl.innerHTML = parseInt(lblnetamountvalue) + amount;

    }
  

</script>

<uc1:ucMessage ID="ucMessage1" runat="server" ShowCloseButton="false" />
<div class="contentbox">
    <div class="grid1">
        <asp:GridView ID="gvStockEntry" runat="server" AutoGenerateColumns="False" CellPadding="4"
            CellSpacing="1" DataKeyNames="SKUID" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
            GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
            AlternatingRowStyle-CssClass="gridrow1" Width="100%"
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
                <%-- <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderNumber"
                    HeaderText="Order Number"></asp:BoundField>
                     <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderQty"
                    HeaderText="Order Quantity"></asp:BoundField>--%>
                     <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Rate" >
                    <ItemTemplate>
                     <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
                    </ItemTemplate>
                    </asp:TemplateField>
                    
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Quantity" ShowHeader="False">
                    <ItemTemplate>
                        <asp:Label ID="lblSKUID" runat="server" Text='<%# Eval("SKUID") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtQuantity" runat="server" MaxLength="8" Text='<%# Eval("Quantity") %>'
                            CssClass="form_input2"></asp:TextBox>
                        <asp:HiddenField ID="hdnid" runat="server" Value='<%# Eval("StockInhand")%>' />
                        <asp:HiddenField ID="HdnQty" runat="server" Value='<%# Eval("Quantity")%>' />
                        <cc1:FilteredTextBoxExtender ID="ftbeOs" runat="server" FilterMode="ValidChars" FilterType="Numbers"
                            TargetControlID="txtQuantity"  />
                     
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                
               
                       <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Amount" >
                    <ItemTemplate>
                    <asp:TextBox ID="lblAmount" runat="server" CssClass="form_input2"  ></asp:TextBox>
               
                    </ItemTemplate>
                    </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="gridheader" />
            <EditRowStyle CssClass="editrow" />
        </asp:GridView>
       
    </div>
  <tr>
   
     <td>
     <asp:Label ID="lbl" runat ="server" Text ="Total Amount : " CssClass="formtext" ></asp:Label>
        <asp:Label ID="lblinfo" runat="server" ></asp:Label>
        </td>
         </tr>
</div>