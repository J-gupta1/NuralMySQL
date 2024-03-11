<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="ReceiveDispatchDoa.aspx.cs" Inherits="DOA_ReceiveDispatchDoa" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function Popup(stockReceiveId) {
            if (stockReceiveId.length > 0) {
                window.open("frmGRNPrint.aspx?stockReceiveId=" + stockReceiveId, "mywindow3", "menubar=0,width=700,height=600,left=10,top=10,scrollbars=yes");
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div align="center">
        <asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div>
                    <table cellspacing="0" cellpadding="0" width="965" border="0">
                        <tr>
                            <td align="left" valign="top">
                                <uc4:ucMessage ID="ucMessage1" runat="server" />

                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">

                                <asp:LinkButton ID="lnkPrintDispatch" runat="server" Visible="false" CssClass="elink2"><u><b>Click here to  print GRN </b></u></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <div class="mainheading_rpt">
                                    <div class="mainheading_rpt_left">
                                    </div>
                                    <div class="mainheading_rpt_mid">
                                        View/Receive Dispatch
                                    </div>
                                    <div class="mainheading_rpt_right">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <table cellspacing="0" cellpadding="0" width="965" border="0">
                                    <tr>
                                        <td valign="top" align="left">
                                            <div class="contentbox">
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td class="mandatory" colspan="5" height="20" valign="top" align="left">(<font class="error">*</font>) marked fields are mandatory.
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="35" width="13%" align="right" valign="top" class="formtext">STN No:<font class="error">+</font>
                                                        </td>
                                                        <td width="20%" align="left" valign="top">
                                                            <asp:TextBox ID="txtSTNNo" runat="server" MaxLength="25"></asp:TextBox>
                                                        </td>
                                                        <td width="13%" align="right" valign="top" class="formtext">IMEI Serial No:<font class="error">+</font>
                                                        </td>
                                                        <td width="20%" align="left" valign="top">
                                                            <asp:TextBox ID="txtIMEINo" runat="server" MaxLength="25"></asp:TextBox>
                                                        </td>
                                                        <td width="13%" align="right" valign="top" class="formtext">DOA Certificate No:<font class="error">+</font>
                                                        </td>
                                                        <td width="20%" align="left" valign="top">
                                                            <asp:TextBox ID="txtDoaCertificatenumber" runat="server" MaxLength="15"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="35" width="13%" align="right" valign="top" class="formtext">Invoice No:<font class="error">+</font></td>
                                                        <td width="20%" align="left" valign="top">
                                                            <asp:TextBox ID="txtInvoiceno" runat="server" MaxLength="25"></asp:TextBox></td>
                                                        <td align="right" valign="top" class="formtext">Dispatch From Date:<font class="error">+</font></td>
                                                        <td align="left" valign="top">
                                                            <uc1:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date." ValidationGroup="SearchNo"
                                                                runat="server" />
                                                        </td>
                                                        <td align="right" valign="top" class="formtext">Dispatch To Date:<font class="error">+</font></td>
                                                        <td align="left" valign="top">
                                                            <uc1:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date." ValidationGroup="SearchNo"
                                                                runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="top" class="formtext">Receive Status:<font class="error">+</font></td>
                                                        <td align="left" valign="top"><asp:DropDownList ID="ddlReceivestatus" runat="server"></asp:DropDownList></td>
                                                        <td align="right" valign="top" class="formtext"></td>
                                                        <td align="left" valign="top"></td>
                                                        <td align="left" valign="top" colspan="2">
                                                            <div class="float-margin">
                                                                <asp:Button ID="Search" runat="server" Text="Search" CssClass="buttonbg"
                                                                    CausesValidation="true" ValidationGroup="Search" OnClick="Search_Click" />
                                                            </div>
                                                            <div class="float-left">
                                                                <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="buttonbg"
                                                                    CausesValidation="false" OnClick="Cancel_Click" />
                                                            </div>
                                                        </td>
                                                        <td align="left" valign="top"></td>
                                                    </tr>

                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="5"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>

                <asp:Panel ID="dvReceiveRemark" runat="server">
                    <table cellspacing="0" cellpadding="0" width="965" border="0">
                        <tr>
                            <td align="left" valign="top">
                                <%-- <uc4:ucMessage ID="ucMessage2" runat="server" ShowCloseButton="false" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <div class="mainheading_rpt">
                                    <div class="mainheading_rpt_left">
                                    </div>
                                    <div class="mainheading_rpt_mid">
                                        Receive Dispatch Remarks
                                    </div>
                                    <div class="mainheading_rpt_right">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td valign="top" align="left">
                                            <div class="contentbox">
                                                <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                    <tr>
                                                        <td height="35" width="13%" align="right" valign="top" class="formtext">Receive Remarks:<font class="error">*</font>
                                                            <%-- <asp:Label ID="lblReceiveRemarks" Text="Receive Remarks:" runat="server"></asp:Label>--%></td>
                                                        <td width="20%" align="left" valign="top">
                                                            <asp:TextBox ID="txtReceiveRemark" runat="server" TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                                                        </td>
                                                        <td height="35" width="13%" align="right" valign="top" class="formtext">CGST:<font class="error">+</font>

                                                        </td>
                                                        <td height="35" width="13%" align="left" valign="top" class="formtext">
                                                            <div>
                                                                <asp:TextBox ID="txtcgst" Width="85px" runat="server"  MaxLength="6"></asp:TextBox>%
                                                            </div>
                                                            <div align="left">
                                                                <asp:RegularExpressionValidator ID="Regexcgst" runat="server" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                                                                    ErrorMessage="Enter valid integer or decimal number with 2 decimal places."
                                                                    ControlToValidate="txtcgst" />

                                                            </div>
                                                            <div>
                                                                <asp:RangeValidator ID="percentageRangeValidator" runat="server"
                                                                    ControlToValidate="txtcgst" Display="Dynamic"
                                                                    ErrorMessage="Invalid CGST%" MaximumValue="100.00" MinimumValue="0.00"
                                                                    Type="Double"></asp:RangeValidator>
                                                            </div>
                                                        </td>
                                                        <td height="35" width="13%" align="right" valign="top" class="formtext">SGST:<font class="error">+</font>


                                                        </td>
                                                        <td height="35" width="13%" align="left" valign="top" class="formtext">
                                                            <div>
                                                                <asp:TextBox ID="txtsgst" Width="85px"  runat="server" MaxLength="6"></asp:TextBox>%
                                                            </div>
                                                            <div>
                                                                <asp:RegularExpressionValidator ID="Regexsgst" runat="server" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                                                                    ErrorMessage="Enter valid integer or decimal number with 2 decimal places."
                                                                    ControlToValidate="txtsgst" />
                                                            </div>
                                                            <div>
                                                                <asp:RangeValidator ID="percentageRangeValidatorSgst" runat="server"
                                                                    ControlToValidate="txtsgst" Display="Dynamic"
                                                                    ErrorMessage="Invalid SGST%" MaximumValue="100.00" MinimumValue="0.00"
                                                                    Type="Double"></asp:RangeValidator>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="35" width="13%" align="right" valign="top" class="formtext">IGST:<font class="error">+</font>

                                                        </td>
                                                        <td height="35" width="13%" align="left" valign="top" class="formtext">
                                                            <div>
                                                                <asp:TextBox ID="txtigst" Width="85px"  runat="server" MaxLength="6"></asp:TextBox>%
                                                            </div>
                                                            <div>
                                                                <asp:RegularExpressionValidator ID="Regexigst" runat="server" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                                                                    ErrorMessage="Enter valid integer or decimal number with 2 decimal places."
                                                                    ControlToValidate="txtigst" />
                                                            </div>
                                                            <div>
                                                                <asp:RangeValidator ID="percentageRangeValidatorigst" runat="server"
                                                                    ControlToValidate="txtigst" Display="Dynamic"
                                                                    ErrorMessage="Invalid IGST%" MaximumValue="100.00" MinimumValue="0.00"
                                                                    Type="Double"></asp:RangeValidator>
                                                            </div>

                                                        </td>
                                                        <td height="35" width="13%" align="right" valign="top" class="formtext">UTGST:<font class="error">+</font>

                                                        </td>
                                                        <td height="35" width="13%" align="left" valign="top" class="formtext">
                                                            <div>
                                                                <asp:TextBox ID="txtutgst" Width="85px"  runat="server" MaxLength="6"></asp:TextBox>%
                                                            </div>
                                                            <div>
                                                                <asp:RegularExpressionValidator ID="Regexutgst" runat="server" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                                                                    ErrorMessage="Enter valid integer or decimal number with 2 decimal places."
                                                                    ControlToValidate="txtutgst" />
                                                            </div>
                                                            <div>
                                                                <asp:RangeValidator ID="percentageRangeValidatorutgst" runat="server"
                                                                    ControlToValidate="txtutgst" Display="Dynamic"
                                                                    ErrorMessage="Invalid UTGST%" MaximumValue="100.00" MinimumValue="0.00"
                                                                    Type="Double"></asp:RangeValidator>
                                                            </div>
                                                        </td>
                                                        <td height="35" width="13%" align="right" valign="top" class="formtext">
                                                            <div class="float-margin">
                                                                <asp:Button ID="ReceiveDispatch" runat="server" Text="Receive Dispatch" CssClass="buttonbg"
                                                                    CausesValidation="true" ValidationGroup="Search" OnClick="ReceiveDispatch_Click" />
                                                            </div>
                                                        </td>
                                                        <td height="35" width="13%" align="right" valign="top" class="formtext">
                                                            <asp:HiddenField ID="hdfstockdispatchid" runat="server" />
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="5"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div>
                    <asp:Panel ID="dvhidegrid" runat="server">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td align="left" class="tableposition">
                                                <div class="mainheading_rpt">
                                                    <div class="mainheading_rpt_left">
                                                    </div>
                                                    <div class="mainheading_rpt_mid">
                                                        List
                                                    </div>
                                                    <div class="mainheading_rpt_right">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <div class="contentbox">
                                                    <div class="grid2">
                                                        <asp:GridView ID="GridDOA" runat="server" AllowPaging="false" AlternatingRowStyle-CssClass="gridrow1"
                                                            bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                                            DataKeyNames="DispatchItemID" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                                            HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                                            RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                                            AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>'>
                                                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                                            <PagerStyle CssClass="gridfooter" />
                                                            <Columns>

                                                                <asp:TemplateField HeaderText="Receive Dispatch">
                                                                    <ItemTemplate>
                                                                        <asp:RadioButtonList ID="rbtReceive" runat="server" Visible='<%# Convert.ToBoolean(Eval("receiveAllowed")) %>'>
                                                                            <asp:ListItem Value="1">Receive</asp:ListItem>
                                                                            <asp:ListItem Value="2">Short Receive</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                        <asp:Label ID="LblStnReceived" Visible='<%# Convert.ToBoolean(Eval("receiveAllReady")) %>' runat="server" Text='<%#Eval("receiveStatus") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:BoundField DataField="DOACertificateNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="DOA Certificate No."
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="STNNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="STN No"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DispatchDate" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left" HeaderText="Dispatch Date"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="GCNNo" HeaderStyle-HorizontalAlign="Left" HeaderText="Docket No"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DocketNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="Invoice No"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="IMEI_SerialNo" HeaderStyle-HorizontalAlign="Left" HeaderText="IMEI Number"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CourierName" HeaderStyle-HorizontalAlign="Left" HeaderText="Courier Name"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DistributorName" HeaderStyle-HorizontalAlign="Left" HeaderText="Distributor Name"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DispatchRemark" HeaderStyle-HorizontalAlign="Left" HeaderText="Dispatch Remark"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PrimaryInvoiceNo" HeaderStyle-HorizontalAlign="Left" HeaderText="Invoice No"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="PrimarySale_Price" HeaderStyle-HorizontalAlign="Left" HeaderText="Price"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>

                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="5"></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>

                <div>
                    <asp:Panel ID="PnlViewStnGrid" runat="server" Visible="false">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td align="left" class="tableposition">
                                                <div class="mainheading_rpt">
                                                    <div class="mainheading_rpt_left">
                                                    </div>
                                                    <div class="mainheading_rpt_mid">
                                                        View STN Details
                                                    </div>
                                                    <div class="mainheading_rpt_right">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <div class="contentbox">
                                                    <div class="grid2">
                                                        <asp:GridView ID="gvSTNDetail" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="gridrow1"
                                                            bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                                            DataKeyNames="StockDispatchID" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                                            HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                                            RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                                            AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>'>
                                                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                                            <PagerStyle CssClass="gridfooter" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="View Dispatch / Receive">
                                                                    <ItemTemplate>

                                                                        <asp:CheckBox ID="ChkViewDetails" runat="server" AutoPostBack="True" OnCheckedChanged="ChkViewDetails_CheckedChanged" />

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Receive Status">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblStnReceivedStatus" runat="server" Text='<%#Eval("ReceiveStatus") %>'></asp:Label>

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="STNNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="STN No"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DispatchDate" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left" HeaderText="STN Date"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Distributor Name" HeaderStyle-HorizontalAlign="Left" HeaderText="From Entity"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="TOEntityId" HeaderStyle-HorizontalAlign="Left" HeaderText="To Entity"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>

                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="dvFooter" runat="server" class="pagination">
                                        <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td height="5"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>

            </ContentTemplate>
            <Triggers>

                <asp:PostBackTrigger ControlID="Search" />
                <asp:PostBackTrigger ControlID="Cancel" />
                <asp:PostBackTrigger ControlID="ReceiveDispatch" />

            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

