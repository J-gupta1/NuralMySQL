<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="BulkRetailerTransfer.aspx.cs" Inherits="Masters_SalesMan_BulkRetailerTransfer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/SalesEntryGrid.ascx" TagName="ucSalesEntryGrid"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0" align="center" style="float: left;">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td align="left" valign="top">
                            <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <uc1:ucMessage ID="ucMessage1" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" class="tableposition">
                            <div class="mainheading_rpt">
                                <div class="mainheading_rpt_left">
                                </div>
                                <div class="mainheading_rpt_mid">
                                    <asp:Label ID="lblStockTransfer2" Text=" Bulk Retailer Transfer" runat="server" /></div>
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
                <div class="contentbox">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td colspan="5" align="left" valign="top" height="15" class="mandatory">
                                (*) Marked fields are mandatory
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top" class="hd5" height="15" style="padding-bottom: 5px;
                                padding-top: 5px;">
                                Transfer From: &nbsp;&nbsp;&nbsp;
                            </td>
                            <td align="left" valign="top">
                            </td>
                            <td align="right" valign="top" class="hd5">
                                Transfer To: &nbsp;&nbsp;&nbsp;
                            </td>
                            <td align="left" valign="top" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td class="formtext" valign="top" align="right" width="15%" height="35">
                                <asp:Label ID="Label1" runat="server" Text="">Sales Channel From:<font class="error">*</font></asp:Label>
                            </td>
                            <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <td width="20%" align="left" valign="top">
                                        <div style="width: 135px;">
                                            <asp:DropDownList ID="cmbTransferFrom" CssClass="form_select" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="cmbTransferFrom_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <br />
                                        </div>
                                        <div style="width: 140px;">
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="cmbTransferFrom"
                                                CssClass="error" ErrorMessage="Please select a SalesChannel whose retailers you want to transfer"
                                                InitialValue="0" ValidationGroup="Add" /></div>
                                    </td>
                                    <td class="formtext" valign="top" align="right" width="15%" height="35">
                                        <asp:Label ID="Label3" runat="server" Text="">Sales Channel To:<font class="error">*</font></asp:Label>
                                    </td>
                                    <td width="20%" align="left" valign="top">
                                        <div style="width: 135px;">
                                            <asp:DropDownList ID="cmbTransferTo" CssClass="form_select" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="cmbTransferTo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <br />
                                        </div>
                                        <div style="width: 140px;">
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="cmbTransferTo"
                                                CssClass="error" ErrorMessage="Please select a SalesChannel whose retailers you want to transfer"
                                                InitialValue="0" ValidationGroup="Add" />
                                        </div>
                                    </td>
                                    <td width="17%" align="right" valign="top" class="formtext" runat="server" id="tdLabel">
                                        Reporting Hierarchy Name:<font class="error">*</font>
                                    </td>
                                    <td align="left" valign="top">
                                        <div style="width: 170px;">
                                            <asp:DropDownList ID="ddlOrghierarchy" runat="server" CssClass="form_select" ValidationGroup="Add">
                                            </asp:DropDownList>
                                        </div>
                                        <div style="width: 180px;">
                                            <asp:RequiredFieldValidator ID="reqOrgnhierarchy" runat="server" ControlToValidate="ddlOrghierarchy"
                                                CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please Select Orgn. Hierarchy Name."
                                                SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </ContentTemplate>
                                <%-- <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                </Triggers>--%>
                            </asp:UpdatePanel>
                        </tr>
                        <tr>
                            <td align="right" class="formtext" valign="top">
                                Salesman From : <%--<font class="error">*</font>--%>
                                <span class="error" id="saleFrom" runat="server">*</span>
                            </td>
                            <td align="left" class="formtext" valign="top">
                                <div style="width: 135px;">
                                    <asp:DropDownList ID="cmbSalesManfrom" runat="server" CssClass="form_select" AutoPostBack="True"
                                        OnSelectedIndexChanged="cmbSalesManFrom_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div style="float: left; width: 180px;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="cmbSalesManfrom"
                                        CssClass="error" ValidationGroup="Add" InitialValue="0" runat="server" ErrorMessage="Please select a  from Sales Channel "></asp:RequiredFieldValidator></div>
                            </td>
                            <td height="35" align="right" class="formtext" valign="top">
                                Salesman To:<%-- <font class="error">*</font>--%>
                                 <span class="error" id="saleTo" runat="server">*</span>
                            </td>
                            <td align="left" class="formtext" valign="top">
                                <div style="width: 135px;">
                                    <asp:DropDownList ID="cmbSalesManTo" runat="server" CssClass="form_select" OnSelectedIndexChanged="cmbSalesManTo_SelectedIndexChanged"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </div>
                                <div style="width: 180px;">
                                    <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="cmbSalesManTo" CssClass="error"
                                        ValidationGroup="Add" InitialValue="0" runat="server" ErrorMessage="Please select a Sales Channel name "></asp:RequiredFieldValidator></div>
                            </td>
                            <td align="left" valign="top">
                                <asp:Button ID="btnCancel" runat="server" Text="&nbsp;Cancel&nbsp;" CssClass="buttonbg"
                                    CausesValidation="false" OnClick="BtnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <div class="contentbox">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td width="40%" valign="top">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td valign="top">
                                            <asp:UpdatePanel ID="updFrom" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="Pnlfrom" runat="server" Visible="false">
                                                        <div class="gridborder">
                                                            <asp:GridView ID="grdRetailerFrom" runat="server" FooterStyle-VerticalAlign="Top"
                                                                FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                                                                HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="None"
                                                                AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                                                                HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                                                                BorderWidth="0px" Width="100%" AutoGenerateColumns="False" AllowPaging="True"
                                                                SelectedStyle-CssClass="gridselected" DataKeyNames="RetailerID" EmptyDataText="No record found"
                                                                OnPageIndexChanging="grdfromPageIndexChanging">
                                                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                                                <RowStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridrow"></RowStyle>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Sales Man From" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkRetailerTransfer" runat="server" />
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RetailerName")%>'></asp:Label>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RetailerID")%>'
                                                                                Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridheader"></HeaderStyle>
                                                                <AlternatingRowStyle CssClass="gridrow1"></AlternatingRowStyle>
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="60%" valign="top">
                                <asp:UpdatePanel ID="updto" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlto" runat="server" Visible="false">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="30%" valign="top" style="padding-left: 10px;">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td height="40">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnTransferselected" runat="Server" Text="Transfer Selected" Width="140px"
                                                                        OnClick="btnTransferselected_Click" CssClass="buttonbg" CausesValidation="true"
                                                                        ValidationGroup="Add" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnTransferAll" runat="Server" Text="Transfer All" Width="140px"
                                                                        OnClick="btnTransferAll_Click" CssClass="buttonbg" CausesValidation="true" ValidationGroup="Add" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td width="70%">
                                                        <div class="gridborder">
                                                            <asp:GridView ID="grdRetailerTo" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                                                                RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" HeaderStyle-HorizontalAlign="left"
                                                                HeaderStyle-VerticalAlign="top" GridLines="none" AlternatingRowStyle-CssClass="gridrow1"
                                                                RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                                                                CellSpacing="1" CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="false"
                                                                AllowPaging="True" SelectedStyle-CssClass="gridselected" DataKeyNames="RetailerID"
                                                                OnPageIndexChanging="grdToPageIndexChanging" EmptyDataText="No record found">
                                                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                                                <Columns>
                                                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="RetailerName"
                                                                        HeaderText="Sales Man  To"></asp:BoundField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td height="10">
            </td>
        </tr>
    </table>
</asp:Content>
