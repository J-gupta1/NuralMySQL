<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="AcknowledgementDOA.aspx.cs" Inherits="DOA_AcknowledgementDOA" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Import Namespace="BussinessLogic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function SelectCheckboxes(headerChk, grdId, columnIndex) {
            var IsChecked = headerChk.checked;
            var tbl = document.getElementById('<%= GridDOA.ClientID %>');
            for (i = 1; i < tbl.rows.length; i++) {
                var curTd = tbl.rows[i].cells[columnIndex];
                var item = curTd.getElementsByTagName('input');
                for (j = 0; j < item.length; j++) {
                    if (item[j].type == "checkbox") {
                        if (item[j].checked != IsChecked) {
                            item[j].click();
                        }
                    }
                }
            }
        }
        function DisplayGCNDateMsg() {

            if (document.getElementById('<%= ddlDispatchMode.ClientID %>').value == '1') //by Courier
            {
                document.getElementById('<%=lbldocketno.ClientID%>').innerText = "Docket No.";
                // document.getElementById('<%=lblGCNDateMsg.ClientID%>').innerText = "If GCN Number entered, GCN date will be captured as Today’s date.";
            }
            else if (document.getElementById('<%= ddlDispatchMode.ClientID %>').value == '2') //by Bike pickup;
            {
                document.getElementById('<%=lbldocketno.ClientID%>').innerText = "Person Name:";
                document.getElementById('<%=lblGCNDateMsg.ClientID%>').innerText = "";
            }
            else {
                document.getElementById('<%=lbldocketno.ClientID%>').innerText = "Docket No.";
                document.getElementById('<%=lblGCNDateMsg.ClientID%>').innerText = "";
            }
    }

    function Popup(StockDispatchID) {
        if (StockDispatchID.length > 0) {
            window.open("frmPrintDispatchDOA.aspx?StockDispatchID=" + StockDispatchID, "mywindow3", "menubar=0,width=700,height=600,left=10,top=10,scrollbars=yes");
        }
        return false;
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div align="center">
        <asp:UpdatePanel ID="UpdMain" runat="server">
            <ContentTemplate>
                <div>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td align="left" valign="top">
                                <uc4:ucMessage ID="ucMessage1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <asp:LinkButton ID="lnkPrintDispatch" runat="server" CssClass="elink2"><u><b>Click here to print this dispatch note</b></u></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <div class="mainheading_rpt">
                                    <div class="mainheading_rpt_left">
                                    </div>
                                    <div class="mainheading_rpt_mid">
                                        View DOA
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
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td class="mandatory" colspan="5" height="20" valign="top" align="left">(<font class="error">*</font>) marked fields are mandatory.
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="15%" align="right" valign="top" class="formtext" height="35">DOA Certificate No:<font class="error">+</font>
                                                        </td>
                                                        <td width="20%" align="left" valign="top">
                                                            <asp:TextBox ID="txtDoaCertificateno" CssClass="form_input2" runat="server" MaxLength="25"></asp:TextBox>

                                                        </td>
                                                        <td width="10%" align="right" valign="top" class="formtext">IMEI:<font class="error">+</font>
                                                        </td>
                                                        <td width="20%" align="left" valign="top">
                                                            <asp:TextBox ID="txtIMEINo" runat="server" CssClass="form_input2" MaxLength="20"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatortxtIMEINo" ValidationGroup="Search"
                                                                        runat="server" Display="Dynamic" ControlToValidate="txtIMEINo"
                                                                        CssClass="error" ErrorMessage="Please Enter IMEI!"></asp:RequiredFieldValidator>--%>
                                                        </td>
                                                        <td width="10%" align="right" valign="top" class="formtext">Status:<font class="error">+</font>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:DropDownList ID="ddldoastatus" Width="120px" runat="server" CssClass="form_select">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="right" class="formtext">From Date:<font class="error">+</font>
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <uc1:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date." ValidationGroup="SearchNo"
                                                                runat="server" />
                                                        </td>
                                                        <td align="right" valign="top" class="formtext">To Date:<font class="error">+</font>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <uc1:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date." ValidationGroup="SearchNo"
                                                                runat="server" />
                                                        </td>
                                                        <td align="left" valign="top"></td>
                                                        <td align="left" valign="top">
                                                            <asp:Button ID="Search" runat="server" Text="Search" CssClass="buttonbg"
                                                                CausesValidation="true" ValidationGroup="Search" OnClick="Search_Click" />
                                                            <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="buttonbg"
                                                                CausesValidation="false" OnClick="Cancel_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%-- <td align="left" valign="top" height="10"><asp:HiddenField ID="hdfDOAID" runat="server" /></td>--%>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="5"></td>
                        </tr>
                        <%-------------------Panel For DOA Settelement started------------------------------------------------%>
                        <asp:Panel ID="PnlMainDoaSellement" runat="server" Visible="false">
                            <tr>
                                <td align="left" valign="top">
                                    <div class="mainheading_rpt">
                                        <div class="mainheading_rpt_left">
                                        </div>
                                        <div class="mainheading_rpt_mid">
                                            Settlement DOA
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
                                            <td align="left" valign="top">
                                                <asp:HiddenField ID="hdfDOAID" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left">
                                                <div class="contentbox">
                                                    <table cellspacing="0" cellpadding="2" width="100%" border="0">
                                                        <tr>
                                                            <td height="35" width="15%" align="right" valign="top" class="formtext">DOA settlement By:<font class="error">*</font>
                                                            </td>
                                                            <td width="20%" align="left" valign="top">
                                                                <div style="float: left; align-content: flex-start; width: 180px;">
                                                                    <asp:RadioButton ID="RbtCreditNote" GroupName="g1" Text="Credit Note" runat="server" AutoPostBack="true" OnCheckedChanged="RbtCreditNote_CheckedChanged" />
                                                                </div>
                                                            </td>
                                                            <td width="15%" align="right" valign="top">
                                                                <asp:RadioButton ID="RbtSwapImei" GroupName="g1" Text="Swap IMEI" runat="server" AutoPostBack="true" OnCheckedChanged="RbtSwapImei_CheckedChanged" />
                                                            </td>
                                                            <td width="20%"></td>
                                                            <td></td>
                                                        </tr>
                        </asp:Panel>
                        <%---------------------------Panel For DOA Settelement End---------------------------------------%>
                        <%-- ----------Panel For Save DOA Settelement started------------------------------%>
                        <div id="PnlSaveDoa" runat="server" visible="false">
                            <tr>
                                <td width="15%" align="right" valign="top" class="formtext">
                                    <asp:Label ID="lblCreditnoteno" runat="server" Text="Credit Note No:"></asp:Label>
                                </td>
                                <td width="20%" align="left" valign="top">
                                    <asp:TextBox ID="txtCreditNumber" CssClass="form_input2" runat="server" MaxLength="17"></asp:TextBox>
                                </td>
                                <td width="15%" align="right" valign="top" class="formtext">
                                    <asp:Label ID="lblSwapimei" runat="server" Text="Swap IMEI No:"></asp:Label>
                                </td>
                                <td width="20%" align="left" valign="top">
                                    <asp:TextBox ID="txtImeiSettlement" CssClass="form_input2" runat="server" MaxLength="17"></asp:TextBox>
                                </td>
                                <td align="left" valign="top">
                                    <asp:Button ID="btnSubmitsettlementDoa" CssClass="buttonbg" runat="server" OnClick="btnSubmitsettlementDoa_Click" Text="Save" />
                                </td>
                            </tr>
                        </div>
                        <%-- -------------------Panel For Save DOA Settelement End----------------------------%>

                        <%------------------------------------------------------------------------------%>
                        <%------------------------------------------------------------------------------%>
                    </table>
                </div>
                <div>
                    <asp:Panel ID="PnlDoaDispatch" runat="server" Visible="true">
                        <div>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="90%" align="left" class="tableposition">
                                                    <div class="mainheading">
                                                        &nbsp;DOA Dispatch
                                                    </div>
                                                </td>
                                                <td width="10%" align="right">
                                                    <asp:HiddenField ID="hdnDoaStatus" runat="server" />
                                                </td>

                                                <td width="10%" align="right"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td valign="top" align="left">
                                                    <div class="contentbox">
                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr>
                                                                <td class="mandatory" colspan="5" height="10" valign="top" align="left"></td>
                                                            </tr>
                                                            <tr>
                                                                <td height="45" width="10%" align="right" valign="top" class="formtext">Dispatch Mode:<font class="error">*</font>
                                                                </td>
                                                                <td width="20%" align="left" valign="top">
                                                                    <asp:DropDownList ID="ddlDispatchMode" CssClass="form_select7" onchange="javascript:DisplayGCNDateMsg();"
                                                                        runat="server" AutoPostBack="false" ValidationGroup="vGroup1">
                                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                                        <asp:ListItem Value="1">By Courier</asp:ListItem>
                                                                        <asp:ListItem Value="2">By Hand</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td width="10%" align="right" valign="top" class="formtext">Remarks:<font class="error">*</font>
                                                                </td>
                                                                <td width="20%" align="left" valign="top">
                                                                    <asp:TextBox ID="txtRemarks" TextMode="MultiLine" runat="server" MaxLength="300"></asp:TextBox>

                                                                </td>
                                                                <td width="10%" class="formtext" valign="top" align="right">
                                                                    <asp:Label ID="lbldocketno" runat="server" Text="Docket No."></asp:Label><font class="error">*</td>
                                                                <td valign="top" align="left">
                                                                    <asp:TextBox CssClass="form_input2" ID="txtdocketnumber" runat="server" MaxLength="30"
                                                                        ValidationGroup="vGroup1"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formtext" valign="top" align="right" height="35">Invoice No:<font class="error">*</font>

                                                                </td>
                                                                <td valign="top" align="left">
                                                                    <asp:TextBox CssClass="form_input2" ID="txtinvoiceno" runat="server" MaxLength="30"
                                                                        ValidationGroup="vGroup1"></asp:TextBox></td>
                                                                <td class="formtext" valign="top" align="right">Courier Name:<font class="error">*</font>
                                                                </td>
                                                                <td valign="top" align="left">
                                                                    <asp:TextBox CssClass="form_input2" ID="txtcouriername" runat="server" MaxLength="30"
                                                                        ValidationGroup="vGroup1"></asp:TextBox>
                                                                </td>
                                                                <td class="formtext" valign="top" align="right">Dispatch To:<font class="error">*</font>

                                                                </td>
                                                                <td valign="top" align="left">
                                                                    <asp:DropDownList ID="ddlDispatchto" CssClass="form_select7" runat="server"></asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formtext" valign="top" align="right">Invoice Date:<font class="error">*</font></td>
                                                                <td valign="top" align="left">
                                                                    <uc1:ucDatePicker ID="ucInvoiceDate" ErrorMessage="Invalid Invoice date." ValidationGroup="SearchNo"
                                                                runat="server" />
                                                                </td>
                                                                <td class="formtext" valign="top" align="right"></td>
                                                                <td ></td>
                                                                <td class="formtext" valign="top" align="right"><asp:Button ID="btnDispatch" runat="server" CausesValidation="true" CssClass="buttonbg" OnClick="btnDispatch_Click" Text="Dispatch" ValidationGroup="vGroup1" /></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" valign="top" height="35">&nbsp;</td>
                                                                <td align="left" valign="top">
                                                                   
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:Label ID="lblGCNDateMsg" CssClass="error" runat="server"></asp:Label></td>
                                                                <td></td>
                                                                <td align="left" valign="top"> </td>
                                                                <td></td>

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
                    </asp:Panel>
                </div>
                <div>
                    <asp:Panel ID="dvhide" runat="server" Visible="true">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td height="5"></td>
                                        </tr>
                                        <tr>
                                            <td width="60%" align="left" class="tableposition">
                                                <div class="mainheading">
                                                    &nbsp;List
                                                </div>
                                            </td>
                                            <td width="20%" align="right">
                                                <asp:Button ID="btnAcknowledgement" runat="server" CssClass="buttonbg" Text="Acknowledgement" OnClick="btnAcknowledgement_Click" />
                                            </td>

                                            <td width="13%" align="right">
                                                <asp:Button ID="btnExport" runat="server" CssClass="excel" Text="" OnClick="btnExport_Click" />
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
                                                    <div class="grid1">
                                                        <asp:GridView ID="GridDOA" runat="server" AlternatingRowStyle-CssClass="gridrow1"
                                                            bgcolor="" BorderWidth="0px" EmptyDataText="No record found" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                                            DataKeyNames="DOAID,DOAStatus" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                                            HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                                            RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="1250px"
                                                            AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>' OnRowCommand="GridDOA_RowCommand">
                                                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                                            <PagerStyle CssClass="gridfooter" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="checkAll" Text="Select" AutoPostBack="True" OnCheckedChanged="checkAll_CheckedChanged" ToolTip="Select Acknowlegement" runat="server" onclick="SelectCheckboxes(this,'<%=GridDOA.ClientID %>', 0);" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkRow" runat="server" AutoPostBack="True" OnCheckedChanged="chkRow_CheckedChanged" Visible='<%# Convert.ToBoolean(Eval("receiveAllowed")) %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="checkAllDispatch" Text="Select Dispatch" AutoPostBack="true" OnCheckedChanged="checkAllDispatch_CheckedChanged" ToolTip="Dispatch" runat="server" onclick="SelectCheckboxes(this,'<%=GridDOA.ClientID %>', 1);" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkDispatchRow" AutoPostBack="true" OnCheckedChanged="chkDispatchRow_CheckedChanged" runat="server" Visible='<%# Convert.ToBoolean(Eval("Dispatch")) %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Settlement">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkSettlement" runat="server" Text="Settlement" CommandName="DOASettlement" CommandArgument='<%#Eval("DOAID") %>' Visible='<%# Convert.ToBoolean(Eval("SettlementAllowed")) %>'></asp:LinkButton>

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="DOACertificateNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="DOA Certificate No."
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="StateName" HeaderStyle-HorizontalAlign="Left" HeaderText="DOA State"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SvcCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SVC Code"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ModelName" HeaderStyle-HorizontalAlign="Left" HeaderText="Model Name"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SKUCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU CODE"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="IMEINumber" HeaderStyle-HorizontalAlign="Left" HeaderText="IMEI Number"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RetailerName" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Name"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DOACertificateGenerateDate" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left" HeaderText="Doa Date"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DoaReceiveDate" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left" HeaderText="Doa ReceiveDate"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <%--<asp:BoundField DataField="Dispatch Date" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left" HeaderText="Dispatch Date"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>--%>
                                                                <asp:BoundField DataField="Price" HeaderStyle-HorizontalAlign="Left" HeaderText="Price"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="InvoiceNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="Invoice Number"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="DOAStatus" HeaderStyle-HorizontalAlign="Left" HeaderText="DOA Status"
                                                                    HtmlEncode="true">
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                    <div id="dvFooter" runat="server" class="pagination">
                                                        <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" height="5">
                                                <%-- <div id="dvFooter" runat="server" class="pagination">
                                                    <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                                                </div>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" height="5"></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>

            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
                <asp:PostBackTrigger ControlID="Search" />
                <asp:PostBackTrigger ControlID="Cancel" />
                <asp:PostBackTrigger ControlID="btnSubmitsettlementDoa" />
                <asp:PostBackTrigger ControlID="btnAcknowledgement" />
                <asp:PostBackTrigger ControlID="btnDispatch" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
