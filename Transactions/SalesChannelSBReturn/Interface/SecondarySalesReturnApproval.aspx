<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="SecondarySalesReturnApproval.aspx.cs" Inherits="Transactions_SalesChannelSBReturn_Interface_SecondarySalesReturnApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/SalesEntryGridWithoutOrder.ascx" TagName="ucSalesEntryGrid"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc4" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0" align="center" style="float: left;">
        <tr>
            <td valign="top" align="left">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <td align="left" valign="top">
                                    <uc1:ucMessage ID="ucMsg" runat="server" />
                                </td>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </tr>
                    <tr>
                        <td height="5" align="left" valign="top"></td>
                        <tr>
                            <td align="left" class="tableposition">
                                <div class="mainheading_rpt">
                                    <div class="mainheading_rpt_left">
                                    </div>
                                    <div class="mainheading_rpt_mid">
                                        Manage Sales Return Approval
                                    </div>
                                    <div class="mainheading_rpt_right">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <td align="left" valign="top">
                                        <div class="contentbox">
                                            <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                                <tr>
                                                    <td colspan="6" align="left" valign="top" height="15" class="mandatory">(*) Marked fields are mandatory
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="12%" align="right" class="formtext" valign="top" height="30">Status: <font class="error">*</font>
                                                    </td>
                                                    <td width="20%" align="left" class="formtext" valign="top">
                                                        <div style="float: left; width: 135px;">
                                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form_select">
                                                                <asp:ListItem Value="101" Selected="True">Select</asp:ListItem>
                                                                <asp:ListItem Value="0">Pending</asp:ListItem>
                                                                <asp:ListItem Value="1">Approved</asp:ListItem>
                                                                <asp:ListItem Value="2">Rejected</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div style="float: left; width: 135px;">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlStatus"
                                                                CssClass="error" ValidationGroup="Ack" InitialValue="101" runat="server" ErrorMessage="Please Select Status."></asp:RequiredFieldValidator>
                                                        </div>
                                                    </td>
                                                    <td width="12%" align="right" class="formtext" valign="top">Invoice Number:
                                                    </td>
                                                    <td width="20%" align="left" class="formtext" valign="top">
                                                        <div style="float: left; width: 135px;">
                                                            <asp:TextBox ID="txtInvoice" runat="server" MaxLength="50" CssClass="form_input2"></asp:TextBox>
                                                            <br />
                                                        </div>
                                                        <div style="float: left; width: 135px;">
                                                        </div>
                                                    </td>
                                                    <td width="12%" align="right" class="formtext" valign="top">Sales Channel Code:
                                                    </td>
                                                    <td align="left" class="formtext" valign="top">
                                                        <div style="float: left; width: 135px;">
                                                            <asp:TextBox ID="txtSalesChannelCode" runat="server" MaxLength="20" CssClass="form_input2"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" class="formtext" valign="top" height="30">Invoice Date From:
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <uc2:ucDatePicker ID="ucFromDate" IsRequired="false" runat="server" ErrorMessage="Invalid date."
                                                            defaultDateRange="false" />
                                                    </td>
                                                    <td align="right" class="formtext" valign="top">Invoice Date To:
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <uc2:ucDatePicker ID="ucToDate" IsRequired="false" runat="server" ErrorMessage="Invalid date."
                                                            defaultDateRange="false" />
                                                    </td>
                                                    <td align="right" class="formtext" valign="top">Sales Channel Type:
                                                    </td>
                                                    <td align="left" class="formtext" valign="top">
                                                        <asp:DropDownList ID="ddlSalesChannelType" runat="server" CssClass="form_select" AutoPostBack="true" OnSelectedIndexChanged="ddlSalesChannelType_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                    </td>
                                                    </tr>

                                                <tr>
                                                    <td align="right" class="formtext" valign="top" height="30">Sales Channel :
                                                    </td>
                                                    <td align="left" class="formtext" valign="top">
                                                        <asp:DropDownList ID="ddlSalesChannel" runat="server" CssClass="form_select">
                                                        </asp:DropDownList>

                                                    </td>
                                                    <%--#CC01 Added Vijay Kumar Prajapati Started--%>
                                                     <td align="right" class="formtext" valign="top" height="30">Return Type :<font class="error">*</font></div>
                                                    </td>
                                                    <td align="left" class="formtext" valign="top">
                                                        
                                                        <asp:DropDownList ID="ddlReturnType" runat="server" CssClass="form_select">
                                                        </asp:DropDownList>
                                                        
                                                        <div style="float: left; width: 135px;">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="ddlReturnType"
                                                                CssClass="error" ValidationGroup="Ack" InitialValue="0" runat="server" ErrorMessage="Please Select Return Type."></asp:RequiredFieldValidator>
                                                        </div>
                                                    </td>
                                                   <%--#CC01 Added Vijay Kumar Prajapati End--%>
                                                    <td colspan="2" align="left" valign="top">
                                                        <div class="float-margin">
                                                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg" CausesValidation="true"
                                                                OnClick="btnSearch_Click" ValidationGroup="Ack" />
                                                        </div>
                                                        <div class="float-left">
                                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="false"
                                                                OnClick="btnCancel_Click" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </tr>
                        <tr>
                            <td height="10"></td>
                        </tr>
                        <tr>
                            <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <td align="left" valign="top">
                                        <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                                            <table id="tblGrid" cellpadding="0" cellspacing="0">
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
                                                        <div class="float-right">
                                                            <asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="contentbox">
                                                            <div class="grid2">
                                                                <asp:GridView ID="gvAck" runat="server" Width="100%" AutoGenerateColumns="false"
                                                                    CssClass="" BorderWidth="0px" CellPadding="4"
                                                                    PageSize='<%$ AppSettings:GridViewPageSize %>' CellSpacing="1" HeaderStyle-CssClass="gridheader"
                                                                    RowStyle-CssClass="gridrow" OnRowCommand="gvAck_RowCommand" AlternatingRowStyle-CssClass="gridrow1"
                                                                    GridLines="none" FooterStyle-CssClass="gridfooter" HeaderStyle-VerticalAlign="Middle"
                                                                    HeaderStyle-HorizontalAlign="left" RowStyle-VerticalAlign="Top"
                                                                    RowStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="left" FooterStyle-VerticalAlign="Top">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ReturnFrom" HeaderText="Return From Name ( Code )" HeaderStyle-HorizontalAlign="Left">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ReturnTo" HeaderText="Return To Name ( Code )" HeaderStyle-HorizontalAlign="Left">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="SecondarySalesReturnNo" HeaderText="Sales Return No" HeaderStyle-HorizontalAlign="Left">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="RequestDate" HeaderText="Request Date" HeaderStyle-HorizontalAlign="Left">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="TotalQuantity" HeaderText="Total Quantity" HeaderStyle-HorizontalAlign="Left">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <%-- <asp:HiddenField ID="hdnIMEIAck" Value='<%#Eval("IMEIAck") %>' runat="server" />--%>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Action">
                                                                            <ItemTemplate>
                                                                                <%-- <asp:Label ID="lblReceived" runat="server" Text='<%# Bind("IsReceived") %>' Visible="false"></asp:Label>--%>
                                                                                <asp:Button ID="btnDetails" CausesValidation="false" runat="server" CssClass="buttonbg"
                                                                                    Text="Details" CommandName="Details" CommandArgument='<%# Eval("SecondarySalesReturnMainID") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                                <div class="clear">
                                                                </div>
                                                            </div>
                                                            <div id="dvFooter" runat="server" class="pagination">
                                                                <uc4:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top"></td>
                                                </tr>
                                                <tr>
                                                    <td height="10"></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </tr>
                        <tr>
                            <asp:UpdatePanel ID="upddetails" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <td align="left" valign="top">
                                        <asp:Panel ID="pnldetail" runat="server" Visible="false">
                                            <table id="Table1" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" class="tableposition">
                                                        <div class="mainheading_rpt">
                                                            <div class="mainheading_rpt_left">
                                                            </div>
                                                            <div class="mainheading_rpt_mid">
                                                                Detail List
                                                            </div>
                                                            <div class="mainheading_rpt_right">
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="contentbox">
                                                            <%--OnDetailRowGetButtonVisibility="grdDetails_DetailRowGetButtonVisibility"--%>
                                                            <dxwgv:ASPxGridView ID="grdDetails" ClientInstanceName="grdvList" runat="server"
                                                                Styles-AlternatingRow-BackColor="" KeyFieldName="SecondarySalesReturnID" Width="100%"
                                                                Styles-Header-HorizontalAlign="Left" Styles-Header-Font-Bold="true" Styles-Header-ForeColor="White"
                                                                Styles-Header-BackColor="#fbfcfd">
                                                                <Columns>
                                                                    <dxwgv:GridViewDataColumn FieldName="InvoiceNumber" VisibleIndex="0" Caption="Invoice Number" />

                                                                    <dxwgv:GridViewDataColumn FieldName="InvoiceDate" VisibleIndex="0" Caption="Invoice Date" />

                                                                    <dxwgv:GridViewDataColumn FieldName="SKUName" VisibleIndex="0" Caption="SKU Name" />

                                                                    <dxwgv:GridViewDataColumn FieldName="Quantity" VisibleIndex="1" Caption="Quantity" />
                                                                    <dxwgv:GridViewDataColumn FieldName="Mode" VisibleIndex="2" Caption="Mode" />
                                                                    <dxwgv:GridViewDataColumn FieldName="Amount" VisibleIndex="3" Caption="Amount" />
                                                                    <%--<dxwgv:GridViewDataColumn FieldName="SerialisedMode" VisibleIndex="4" Caption="SerialisedMode"
                                                                        Visible="false" />--%>
                                                                </Columns>
                                                                <Templates>
                                                                    <%-- --%>
                                                                    <DetailRow>
                                                                        <dxwgv:ASPxGridView ID="detailGrid" runat="server" KeyFieldName="SecondarySalesReturnID" Width="100%"
                                                                            OnBeforePerformDataSelect="detailGrid_DataSelect" EnableCallBacks="False" SettingsBehavior-AllowSort="false"
                                                                            Styles-Header-HorizontalAlign="Left" Styles-Header-Font-Bold="true" Styles-Header-ForeColor="White"
                                                                            Styles-Header-BackColor="#fbfcfd">
                                                                            <Columns>
                                                                                <dxwgv:GridViewDataTextColumn FieldName="SerialNo" ReadOnly="True" VisibleIndex="0">
                                                                                </dxwgv:GridViewDataTextColumn>
                                                                            </Columns>
                                                                            <Settings ShowFooter="True" />
                                                                            <SettingsDetail ShowDetailRow="False" IsDetailGrid="True" />
                                                                            <SettingsBehavior AllowFocusedRow="True" ProcessSelectionChangedOnServer="False" />
                                                                        </dxwgv:ASPxGridView>
                                                                    </DetailRow>
                                                                </Templates>
                                                                <SettingsDetail ShowDetailRow="true" />
                                                            </dxwgv:ASPxGridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" height="10"></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" runat="server" id="tdPanel" visible="false">
                                                        <table width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td align="left" valign="top" class="tableposition">
                                                                    <div class="mainheading_rpt">
                                                                        <div class="mainheading_rpt_left">
                                                                        </div>
                                                                        <div class="mainheading_rpt_mid">
                                                                            Upload Serial Number
                                                                        </div>
                                                                        <div class="mainheading_rpt_right">
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" valign="top">
                                                                    <div class="contentbox">
                                                                        <table width="100%" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td align="left" valign="top">
                                                                                    <asp:Panel runat="server" ID="pnlIMEIAck" Visible="false">
                                                                                        <div style="padding-bottom: 10px;">
                                                                                            <asp:FileUpload ID="flUpload" runat="server" CssClass="form_file" />
                                                                                        </div>
                                                                                        <div>
                                                                                            <a class="elink2" href="../../../Excel/Templates/PrimaryIMEIAck.xlsx">Download Template</a>
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" valign="top"></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="10"></td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <div class="float-margin">
                                                            <asp:Button ID="btnAccept" CssClass="buttonbg" runat="server" Text="Accept" CausesValidation="False"
                                                                OnClick="btnAccept_Click" />
                                                        </div>
                                                        <div class="float-left">
                                                            <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="buttonbg" CausesValidation="false"
                                                                OnClick="btnReject_Click" />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="10"></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </tr>
                        <tr>
                            <td height="10"></td>
                        </tr>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
