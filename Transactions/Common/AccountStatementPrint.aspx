<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountStatementPrint.aspx.cs" Inherits="Transactions_Common_AccountStatementPrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Assets/Comio/print.css" rel="stylesheet" />
    <link href="../../Assets/Comio/sony.css" rel="stylesheet" media="print" />
</head>
<body>
   <div align="center"><form id="Form1" runat="server">
  <table width="630" border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td><table width="630" border="0" cellspacing="0" cellpadding="0">
        
          <tr>
              <td align="left"><asp:Label ID="lblmessage" runat="server"></asp:Label></td>
          </tr>
         <tr>
            
          <td height="30" align="center" class="pagehead">
              <strong><asp:Label ID="lblsalesChannelName" runat="server" Visible="false"></asp:Label></strong></td>
        </tr>
        <tr>
         <%-- <td height="30" align="center" class="pagehd">
              <strong>
                  <asp:Label ID="lblSalesChannel" runat="server"></asp:Label></strong></td>--%>
        </tr>
          <tr>
              <td height="30" align="center" class="pagehd">
                  <asp:Label ID="lblheadingCin" runat="server" Visible="false"><strong>CIN:</strong></asp:Label>
                  <asp:Label ID="lblCin" runat="server" Visible="false"></asp:Label>
              </td>   
          </tr>
          <tr>
              <td height="30" align="center" class="pagehd">
                    <asp:Label ID="lblheadingLedger" runat="server" Visible="false"><strong>L E D G E R:</strong></asp:Label>(<asp:Label ID="lblheadingFrom" runat="server" Visible="false"><strong>From</strong></asp:Label> <asp:Label ID="lblFromdate" runat="server" Visible="false"></asp:Label> <asp:Label ID="lblheadingTO" runat="server" Visible="false"><strong>To</strong></asp:Label> <asp:Label ID="lbltodate" runat="server" Visible="false"></asp:Label>)
              </td>
          </tr>
          <tr>
              <td height="30" align="center" class="pagehd"><strong>
               <asp:Label ID="lblheadingAccount" runat="server" Visible="false"><strong>Account:</strong></asp:Label> <asp:Label ID="lbltaxIndiaServices" runat="server" Visible="false"></asp:Label></strong></td>
          </tr>
        <tr>
          <td align="left" valign="top" class="border2" ><table width="100%" border="0" cellpadding="0" cellspacing="1">
              <tr>
                <td align="center" class="bg5"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                      
                    <td align="right" class="frmtxt"><table width="638" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                          <td></td>
                        <td align="right" valign="top"><a href="javascript:window.print();" class="elink3" title="Click here to print"><img src="../../Assets/Comio/Images/print.gif" width="25px" height="25px" / alt="Click here to print"/>Click here to print</a> | <a href="javascript:window.close();" class="elink3">Close</a>&nbsp;</td>
                       
                      </tr>
                    </table>                      </td>
                    </tr>
                    
                    <tr>
                      <td align="left" valign="top">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:GridView ID="gvaccountdetails" Visible="False" runat="server" AutoGenerateColumns="False" EnableViewState="False" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" EnableModelValidation="True">
                                <Columns>
                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="StatementDate" HeaderText="Date"  DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                     <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="StatementType" HeaderText="Type">
                                     <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                     <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="VoucherNo" HeaderText="Vch No.">
                                     <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                     <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Particulars" HeaderText="Particulars">
                                     <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                     <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Narration" HeaderText="Narration">
                                      <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                     <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="DebitAmount" HeaderText="Debit">
                                     <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                     <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="CreditAmount" HeaderText="Credit">
                                      <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                     <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Balance" HeaderText="Balance">
                                      <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                                <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                <RowStyle BackColor="White" ForeColor="#330099" />
                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                         <td align="right" valign="top">
                          <asp:Label ID="lblheadingTotalDebit" runat="server" Visible="false"><strong>Total Debit Amount:</strong></asp:Label><asp:Label ID="totaldebit" runat="server" Visible="false"></asp:Label> <asp:Label ID="lblheadingTotalCredit" runat="server" Visible="false"><strong>Total Credit Amount:</strong></asp:Label><asp:Label ID="totalcreadit" runat="server" Visible="false"></asp:Label>
                       </td>
                    </tr>
                    <%--<tr>
                       <td align="right" valign="top">
                          <strong>Closing Balance<asp:Label ID="lblClosingblance" runat="server"></asp:Label></strong>
                       </td> 
                    </tr>--%>
                   <%-- <tr>
                        <td align="right" valign="top">__________________________________</td>
                    </tr>--%>
                    <%--<tr>
                       <td align="right" valign="top">
                          <strong>Grand Total</strong><asp:Label ID="lblgrandtotalcreadit" runat="server"></asp:Label><asp:Label ID="lblgrandtotaldebit" runat="server"></asp:Label>
                       </td> 
                    </tr>--%>
                     <%--<tr>
                        <td align="right" valign="top">__________________________________</td>
                    </tr>--%>
                </table></td>
              </tr>
          </table></td>
        </tr>
      </table>
        </td>
    </tr>
      </table>
  </form>
</div>
</body>
</html>
