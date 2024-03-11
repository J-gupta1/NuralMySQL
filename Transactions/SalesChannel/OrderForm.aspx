<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderForm.aspx.cs" Inherits="Transactions_POC_OrderForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    
    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>
    
    <script language="javascript" type="text/javascript">
        function funcwindowclose() {
            debugger;
            parent.PurchaseOrder.hide();
            return true;
        }
</script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div align="center">
            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="left" valign="top" class="border2">
                    <div style="font-family: Arial; font-size:11px; width:650px;">
                        <table width="750px" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td rowspan="2" align="left">
                                    <img src="../../Assets/ZedSales/CSS/Images/logo.jpg" width="120" height="30" alt="World Ace"  />
                                </td>
                                <td height="20" align="right" valign="top" id="print">
                                    <a href="javascript:window.print();" class="elink3" title="Click here to print">
                                        <img src="../../Assets/ZedSales/CSS/Images/WOPrint.gif" width="25" height="25" alt="Click here to print"
                                            align="absmiddle" />Click here to print</a> | <asp:LinkButton Text = "Close" id = "lkclose" runat = "server"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td align="Right" valign="top">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblMessage" CssClass="error" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td height="30" colspan="2" align="left">
                                    <b class="pagehd">PURCHASE ORDER</b>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" colspan="2">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="2" align="left" valign="top" class="border5">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr class="frmtxt">
                                                    <td height="5" colspan="4" align="left" valign="top">
                                                    </td>
                                                </tr>
                                                
                                                 <tr class="frmtxt">
                                                  <td width="18%" align="left" valign="top">
                                                        <b>Order Number :</b>
                                                    </td>
                                                    <td width="33%" align="left" valign="top">
                                                        <asp:Label ID="lblPONo" runat="server"></asp:Label>
                                                    </td>
                                                   <td height="20" align="left" valign="top">
                                                        <b>Order Date :</b>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:Label ID="lblPODate" runat="server"></asp:Label>
                                                    </td>
                                                    
                                                </tr>
                                                <tr class="frmtxt">
                                                    <td width="16%" height="20" align="left" valign="top">
                                                        <b>Order From :</b>
                                                    </td>
                                                    <td width="33%" align="left" valign="top">
                                                        <asp:Label ID="lblOrderFrom" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="16%" height="20" align="left" valign="top">
                                                        <b>Order To :</b>
                                                    </td>
                                                    <td width="33%" align="left" valign="top">
                                                        <asp:Label ID="lblOrderto" runat="server"></asp:Label>
                                                    </td>
                                                  
                                                </tr>
                                               
                                                <tr class="frmtxt">
                                                
                                                 <td rowspan="2" align="left" valign="top">
                                                        <b>From Address :</b>
                                                    </td>
                                                    <td align="left" valign="top" rowspan="2">
                                                        <asp:Label ID="Label1" runat="server"></asp:Label>
                                                        &nbsp;,
                                                        <asp:Label ID="Label2" runat="server"></asp:Label>
                                                        ,
                                                        <asp:Label ID="Label3" runat="server"></asp:Label>
                                                    </td>
                                                   
                                                 <td rowspan="2" align="left" valign="top">
                                                        <b>To Address :</b>
                                                    </td>
                                                    <td align="left" valign="top" rowspan="2">
                                                        <asp:Label ID="lblServiceAddress" runat="server"></asp:Label>
                                                        &nbsp;,
                                                        <asp:Label ID="lblServicePhone" runat="server"></asp:Label>
                                                        ,
                                                        <asp:Label ID="lblServiceMobile" runat="server"></asp:Label>
                                                    </td>
                                                
                                                </tr>
                                               
                                               
                                                <tr class="frmtxt">
                                                <td></td>
                                               </tr>
                                                <tr class="frmtxt">
                                                    <td height="20" align="left" valign="top">
                                                        <strong>TIN No :</strong>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:Label ID="lblTINNo" runat="server"></asp:Label>
                                                    </td>
                                                   <td height="20" align="left" valign="top">
                                                        <strong>CST No :</strong>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:Label ID="lblCST" runat="server"></asp:Label>
                                                    </td>
                                                
                                                </tr>
                                            </table>
                                        </td>
                                        </tr>
                                        </table>
                                        </td>
                          </tr>
                            <tr>
                                <td align="left" valign="top" colspan="2">
                                    <br />
                                    Dear Sir/Madam,<br />
                                    <br />
                                    Please arrange to dispatch the following items at earliest against this PRR.
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td height="25" colspan="2" align="right" valign="top">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="border2">
                                        <tr>
                                            <td height="20" colspan="2" align="left" valign="top" class="bg7">
                                                <asp:GridView ID="gvPOPrint" runat="server" AutoGenerateColumns="False" BorderWidth="1px"
                                                    CellPadding="4" CellSpacing="1" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                                                    AlternatingRowStyle-CssClass="gridrow1" GridLines="both" HeaderStyle-VerticalAlign="top"
                                                    HeaderStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                                                    FooterStyle-HorizontalAlign="Left" Width="100%" FooterStyle-VerticalAlign="Top"
                                                    FooterStyle-CssClass="gridfooter">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:BoundField HeaderText="Product Type" DataField="Product_Name" />
<asp:BoundField HeaderText="Brand Type" DataField="BrandName" />--%>
                                                        <asp:BoundField HeaderText="SKU" DataField = "SKUName" />
                                                        <asp:BoundField ItemStyle-HorizontalAlign="Right" HeaderText = "Quantity" DataField="Quantity" />
                                                        <asp:BoundField ItemStyle-HorizontalAlign="Right" HeaderText = "Unit Price(in Rs.)"
                                                            DataField="UnitPrice" />
                                                        <asp:BoundField ItemStyle-HorizontalAlign="Right" HeaderText="Amount(price*Qty) (in Rs.)"
                                                            DataField="Amount" />
                                                    </Columns>
                                                    <RowStyle CssClass="gridrow" VerticalAlign="Top" />
                                                    <HeaderStyle CssClass="gridheader" VerticalAlign="Top" />
                                                    <AlternatingRowStyle CssClass="gridrow1" />
                                                </asp:GridView>
                                            </td>
                                            <tr>
                                                <td width="93%" height="20" align="right" valign="top">
                                                    &nbsp;
                                                </td>
                                                <td width="7%" align="right" valign="top">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="20" align="right" valign="top">
                                                    <strong>Total Amount (Rs.)</strong>
                                                </td>
                                                <td align="right" valign="top">
                                                    <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>&nbsp;
                                                </td>
                                            </tr>
                                            <%--<tr>
<td height="20" align="right" valign="top"><strong>Total Discount (Rs.)</strong></td>
<td align="right" valign="top"><asp:Label ID="lblTotalDiscount" runat="server"></asp:Label>&nbsp;</td>
</tr>
<tr>
<td height="20" align="right" valign="top"><strong>Total Tax (Rs.)</strong></td>
<td align="right" valign="top"><asp:Label ID="lblTotalTax" runat="server"></asp:Label>&nbsp;</td>
</tr>
<tr>
<td height="20" align="right" valign="top"><strong>Net Amount (Rs.)</strong></td>
<td align="right" valign="top"><asp:Label ID="lblNetAmount" runat="server"></asp:Label>&nbsp;</td>
</tr>
--%>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td height="22" align="left" colspan="2">
                                    <strong>Net Amount(in words): Rs.
                                        <asp:Label ID="lblAmtinwords" runat="server"></asp:Label>
                                    </strong>
                                </td>
                            </tr>
                            <tr>
                                <td height="55" align="left" valign="bottom">
                                    <table width="300" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td align="left" valign="top" class="border6">
                                                <b> Authorized Signatory</b>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="center" valign="bottom">
                                    <table width="230" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td align="left" valign="top" class="border6">
                                                <strong>For:
                                                    <asp:Label ID="lblServiceNamesig" runat="server"></asp:Label>
                                                </strong>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left" valign="top" height="8">
                    </td>
                </tr>
            </table>
            </td></tr></table>
        </div>
    </div>
    </form>
</body>
</html>
