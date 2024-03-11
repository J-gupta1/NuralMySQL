<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmGRNPrint.aspx.cs" Inherits="DOA_frmGRNPrint" %>

<!DOCTYPE html>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="dvhiddenGrid" runat="server" align="center" style="font-family: Arial; font-size: 12px;">
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
                                                    <asp:Image ID="Image2" ImageUrl="~/Assets/Comio/CSS/Images/WOPrint.gif" runat="server" />
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
                                            <td align="left" valign="top" class="frmtxt">
                                                <asp:Image ID="Image1" ImageUrl="~/Assets/Comio/CSS/Images/logo.gif" runat="server" /></td>
                                            <td align="left" valign="top" class="frmtxt" width="65%">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                       
                                                        <td align="left" valign="top">
                                                            <strong>
                                                                <asp:Label ID="lblSaleschannelname" runat="server"></asp:Label></strong><br /><br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="left" valign="top" width="30%">
                                                                        <strong>Address: &nbsp;</strong>
                                                                    </td>
                                                                    <td align="left" valign="top" width="70%">
                                                                        <asp:Label ID="lblWarehouseAddress" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top" width="30%" >
                                                                        <strong>City: &nbsp;</strong>
                                                                    </td>
                                                                    <td align="left" valign="top" width="70%">
                                                                        <asp:Label ID="lblWarehouseCity" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top" width="30%" >
                                                                        <strong>Pin Code: &nbsp;</strong>
                                                                    </td>
                                                                    <td align="left" valign="top" width="70%">
                                                                        <asp:Label ID="lblWarehousePincode" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top" width="30%">
                                                                        <strong>State Name: &nbsp;</strong>
                                                                    </td>
                                                                    <td align="left" valign="top" width="70%">
                                                                        <asp:Label ID="lblWarehouseStateName" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top" width="30%" >
                                                                        <strong>State Code: &nbsp;</strong>
                                                                    </td>
                                                                    <td align="left" valign="top" width="70%">
                                                                        <asp:Label ID="lblWarehouseStateCode" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top" width="30%">
                                                                        <strong>GSTIN NO: &nbsp;</strong>
                                                                    </td>
                                                                    <td align="left" valign="top" width="70%">
                                                                        <asp:Label ID="lblGstnid" runat="server"></asp:Label><br />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top" width="30%">
                                                                        <strong>Mobile NO: &nbsp;</strong>
                                                                    </td>
                                                                    <td align="left" valign="top" width="70%">
                                                                        <asp:Label ID="lblWarehousemobileno" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" valign="top" height="5">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" valign="top">
                                                            <strong>
                                                                <asp:Label ID="lblDispatchinformation" runat="server" Font-Size="Medium" Text="GOODS RECEIPT NOTE"></asp:Label></strong>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="frmtxt">
                                                <strong>
                                                    <asp:Label ID="lblconsigneeinformation" runat="server" Text="Vendor Address"></asp:Label></strong>
                                                <br />
                                                <br />
                                                <strong>Name: &nbsp;</strong>
                                                <asp:Label ID="lblconsigneename" runat="server"></asp:Label><br />
                                                <strong>Address: &nbsp;</strong><asp:Label ID="lblconsigneeaddress" runat="server"></asp:Label><br />
                                                <strong>City:</strong><asp:Label ID="lblDistributorCity" runat="server"></asp:Label><br />
                                                <strong>Pin Code:</strong>
                                                <asp:Label ID="lbldispincode" runat="server"></asp:Label><br />
                                                <strong>State Name:</strong>
                                                <asp:Label ID="lblconsigneestatename" runat="server"></asp:Label><br />
                                                <strong>State Code: &nbsp;</strong>
                                                <asp:Label ID="lblStateCode" runat="server"></asp:Label><br />
                                                <strong>GSTIN No: &nbsp;</strong>
                                                <asp:Label ID="lblTinnumber" runat="server"></asp:Label><br />
                                                <strong>Mobile No.:</strong>
                                                <asp:Label ID="lblMobilenumber" runat="server"></asp:Label>

                                            </td>
                                            <td align="left" valign="top">
                                                <table width="100%" border="0" cellpadding="4" cellspacing="0">
                                                    <tr>
                                                        <td colspan="2" align="left" valign="top"><strong>Information</strong>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="55%" align="left" valign="top"><strong>GRN No.: &nbsp;</strong>
                                                            <asp:Label ID="lblSTNNumber" runat="server"></asp:Label>
                                                        </td>
                                                        <td align="left" valign="top"><strong>GRN Date: &nbsp;</strong>
                                                            <asp:Label ID="lblSTNDate" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top">
                                                            <strong>Invoice No.: &nbsp;</strong>
                                                            <asp:Label ID="lblInvoicenumber" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top">
                                                            <strong>Docket No./Person Name: &nbsp;</strong>
                                                            <asp:Label ID="lbldocketno" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top"><strong>Store Location: &nbsp;</strong>
                                                            <asp:Label ID="lblstorelocation" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td align="left" valign="top">
                                                            <strong>GR No.: &nbsp;</strong>
                                                            <asp:Label ID="lblGCNNumber" runat="server"></asp:Label>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <strong>GR Date: &nbsp;</strong>
                                                            <asp:Label ID="lblGCNDate" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>--%>
                                                </table>
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
                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="HSN CODE" HeaderText="HSN Code" ItemStyle-Width="92">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ReceiveQuantity" HeaderText="Qty." ItemStyle-Width="22">
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
                                            <td align="left"></td>
                                            <td align="left"></td>
                                            <td align="left"></td>
                                            <td align="left"></td>
                                            <td align="left"></td>
                                            <td align="left"></td>
                                            <td align="left"><%--<asp:Label ID="Label3" runat="server"></asp:Label>--%></td>
                                            <td align="left">
                                                <asp:Label ID="lblQuantity" runat="server"></asp:Label></td>
                                            <td align="left"></td>
                                            <td align="left">
                                                <asp:Label ID="lblTotalAmount" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <asp:GridView ID="gvTaxdetail" runat="server" AutoGenerateColumns="False" EnableViewState="False" BackColor="White" BorderColor="#000000" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" EnableModelValidation="True">
                                        <Columns>

                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="TaxName" HeaderText="Tax" ItemStyle-Width="246">
                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="TaxAmount" HeaderText="Tax Amount" ItemStyle-Width="62">
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
                            <table width="100%" border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse">
                                <tr>
                                    <td width="75%" align="left" valign="top">
                                        <table width="100%" border="0" cellpadding="4" cellspacing="0">
                                            <tr>
                                                <td align="right" valign="top">Total After Tax:</td>
                                                <td align="right" valign="top">
                                                    <asp:Label ID="lblTotalaftertax" runat="server"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse">
                            <tr>
                                <td>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="75%" align="left" valign="top">
                                                <table width="100%" border="0" cellpadding="4" cellspacing="0">
                                                    <tr>
                                                        <td align="left" valign="top">Grand Total:</td>
                                                        <td align="left" valign="top">
                                                            <asp:Label ID="lblTotalinWords" runat="server"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top">Remarks:</td>
                                                        <td align="left" valign="top">
                                                            <asp:Label ID="lblRemark" runat="server"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top">Received By:</td>
                                                        <td align="left" valign="top">
                                                            <asp:Label ID="lblreceiveby" runat="server"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <table width="100%" border="0" cellpadding="4" cellspacing="0">
                                                    <tr>
                                                        <td align="center" valign="top">
                                                            <asp:Label ID="lblGrandTotal" runat="server"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" valign="top"></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" valign="top">Authorised Signatory</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </td>
                </tr>
            </table>
        </div>

        <div id="divnorecord" runat="server" align="center" style="font-family: Arial; font-size: 12px;">
            <table width="630" border="0" cellspacing="0" cellpadding="0" style="border-collapse: collapse">
                <tr>

                    <td align="center" class="bg5">
                        <div style="padding-top: 35%; font-size: 14px;">
                            <asp:Label ID="lblnorecord" runat="server"></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
