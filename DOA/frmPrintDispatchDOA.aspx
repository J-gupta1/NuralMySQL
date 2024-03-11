<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmPrintDispatchDOA.aspx.cs" Inherits="DOA_frmPrintDispatchDOA" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div align="center" style="font-family: Arial; font-size: 12px;">
            <table width="630" border="0" cellspacing="0" cellpadding="0" style="border-collapse: collapse">
                <tr>
                    <td align="center" class="bg5">
                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="right" class="frmtxt">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="4">
                                        <tr>
                                            <td></td>
                                            <td align="right" valign="top">
                                                <a href="javascript:window.print();" class="elink2" title="Click here to print">
                                                    <img src="../Assets/Comio/CSS/Images/WOPrint.gif" />
                                                    Click here to print
                                                </a>&nbsp;| 
                                                <a href="javascript:window.close();" class="elink2">Close</a>&nbsp;
                                            </td>

                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <table width="100%" border="1" cellpadding="4" cellspacing="0" style="border-collapse: collapse">
                                        <tr>
                                            <td colspan="2" align="center"><strong>
                                                <asp:Label ID="lblDispatchinformation" runat="server" Font-Size="Medium" Text="Debit Note"></asp:Label></strong></td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="frmtxt" width="40%">
                                                <strong>
                                                    <asp:Label ID="lblSaleschannelname" runat="server"></asp:Label></strong><br />
                                                <br />
                                                <strong>Address:</strong><asp:Label ID="lblWarehouseAddress" runat="server"></asp:Label><br />
                                                <strong>City:</strong><asp:Label ID="lblWarehouseCity" runat="server"></asp:Label><br />
                                                <strong>Pin Code:</strong><asp:Label ID="lblpincodewarehouse" runat="server"></asp:Label><br />
                                                <strong>State Name:</strong><asp:Label ID="lblWarehouseStateName" runat="server"></asp:Label><br />
                                                <strong>State Code:</strong><asp:Label ID="lblWarehouseStateCode" runat="server"></asp:Label><br />
                                                <strong>GSTIN NO:</strong><asp:Label ID="lblGstnid" runat="server"></asp:Label><br />
                                                <strong>Mobile NO:</strong><asp:Label ID="lblmobilenodistributor" runat="server"></asp:Label><br />
                                            </td>




                                            <td align="left" valign="top" class="frmtxt" rowspan="2">
                                                <table width="100%" border="0" cellpadding="4" cellspacing="0">
                                                    <tr>
                                                        <td width="23%"><strong>STN No.:</strong></td>
                                                        <td width="30%">
                                                            <asp:Label ID="lblSTNNumber" runat="server"></asp:Label></td>
                                                        <td width="20%"><strong>STN Date:</strong></td>
                                                        <td>
                                                            <asp:Label ID="lblSTNDate" runat="server"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <strong>Invoice No.:</strong>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblinvoiceno" runat="server"></asp:Label></td>
                                                        <td>
                                                            <strong>Docket No./Person Name:</strong>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbldocketno" runat="server"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <strong>Invoice Date:</strong>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblinvoicedate" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="frmtxt">
                                                <strong>
                                                    <asp:Label ID="lblconsigneeinformation" runat="server" Text="Buyer Address"></asp:Label></strong><br />
                                                <br />
                                                <strong>Name:</strong><asp:Label ID="lblconsigneename" runat="server"></asp:Label><br />
                                                <strong>Address:</strong>
                                                <asp:Label ID="lblconsigneeaddress" runat="server"></asp:Label><br />
                                                <strong>City:</strong><asp:Label ID="lblDistributorCity" runat="server"></asp:Label><br />
                                                <strong>Pin Code:</strong>
                                                <asp:Label ID="lbldispincode" runat="server"></asp:Label><br />
                                                <strong>State Name:</strong>
                                                <asp:Label ID="lblconsigneestatename" runat="server"></asp:Label><br />
                                                <strong>State Code:</strong>
                                                <asp:Label ID="lblconsigneestatecode" runat="server"></asp:Label><br />
                                                <strong>GSTIN No.:</strong>
                                                <asp:Label ID="lblCstnumber" runat="server"></asp:Label><br />
                                                <strong>Mobile No.:</strong>
                                                <asp:Label ID="lblMobilenumber" runat="server"></asp:Label>
                                                <%--<strong><asp:Label ID="lblConsinerinformation" runat="server" Text="Consigner  Detail"></asp:Label></strong><br /><br />
                                         <asp:Label ID="lblConsignername" runat="server"></asp:Label><br />
                                         <asp:Label ID="lblConsigneraddress" runat="server"></asp:Label><br />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <asp:GridView ID="gvDispatch" runat="server" AutoGenerateColumns="False" EnableViewState="False" BackColor="White" BorderColor="#000000" BorderStyle="None" BorderWidth="1px" CellPadding="4" EnableModelValidation="True">
                                        <Columns>
                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SNo" HeaderText="SNo." ItemStyle-Width="22">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUCode" HeaderText="SKU Code" ItemStyle-Width="246">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ModelName" HeaderText="Model Name" ItemStyle-Width="62">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="StockQuantity" HeaderText="Qty." ItemStyle-Width="22">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="HSNCode" HeaderText="HSN Code " ItemStyle-Width="22">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="IMEI_SerialNo" HeaderText="Serial No." ItemStyle-Width="246">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="PrimaryInvoiceNo" HeaderText="Primary Invoice No." ItemStyle-Width="62">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="PrimarySale_Price" HeaderText="Amount">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                        </Columns>
                                        <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                        <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="#ffffff" />
                                        <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                        <RowStyle BackColor="White" ForeColor="#330099" />
                                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <table width="100%" border="1" cellpadding="4" cellspacing="0" style="border-collapse: collapse">
                                        <tr>
                                            <td align="left" width="22"></td>
                                            <td align="left" width="246" colspan="2">Total</td>
                                            <td align="left" width="72">
                                                <asp:Label ID="lblQuantity" runat="server"></asp:Label></td>
                                            <td align="left" width="72"></td>
                                            <td align="left" width="72"></td>
                                            <td align="left" width="72">
                                                <asp:Label ID="lblTotalAmount" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <table width="100%" border="1" cellpadding="4" cellspacing="0" style="border-collapse: collapse">
                            <tr>
                                <td align="Right">Authorised Signatory
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
