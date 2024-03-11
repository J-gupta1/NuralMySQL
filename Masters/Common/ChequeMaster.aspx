<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ChequeMaster.aspx.cs" Inherits="Masters_Common_ChequeMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <%-- <uc2:header ID="Header1" runat="server" />--%>

        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMessage1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Panel ID="pnlMain" runat="server">
            <div class="mainheading">
                Select
            </div>
            <div class="contentbox">
                <div class="mandatory">
                    (*) Marked fields are mandatory
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Distributor:<span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbSalesChannel" runat="server" CssClass="formfields" AutoPostBack="True"
                                    OnSelectedIndexChanged="cmbSalesChannel_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </li>
                        <asp:Panel runat="server" ID="pnlRDO">
                            <li class="text">Select:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:RadioButtonList ID="rdoSelect" runat="server" CssClass="radio-rs" RepeatDirection="Horizontal" CellPadding="2"
                                    CellSpacing="0" BorderWidth="0" AutoPostBack="true" OnSelectedIndexChanged="rdoSelect_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Value="1" Text="Add More Cheques"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Deposit Cheques"></asp:ListItem>
                                </asp:RadioButtonList>
                            </li>
                        </asp:Panel>
                        <li class="field3">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                OnClick="btncancel_click" />
                        </li>
                    </ul>
                </div>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="updEnterCheque" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlEnterChq" runat="server" Visible="false">
                    <div class="mainheading">
                        Add More Cheques
                    </div>
                    <div class="contentbox">
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">Bank:
                                </li>
                                <li class="field">
                                    <div>
                                        <asp:TextBox ID="txtBank" runat="server" MaxLength="100" CssClass="formfields"> </asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtBank"
                                        CssClass="error" ErrorMessage="Please insert a bank name " ValidationGroup="insertcheque" />
                                </li>
                                <li class="text">Account No.:
                                </li>
                                <li class="field">
                                    <div>
                                        <asp:TextBox ID="txtAccountNo" runat="server" CssClass="formfields" MaxLength="100"> </asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtAccountNo"
                                        CssClass="error" ErrorMessage="Please insert account number " ValidationGroup="insertcheque" />
                                </li>
                                <li class="text">Cheque No. From:
                                </li>
                                <li class="field">
                                    <div>
                                        <asp:TextBox ID="txtChequeFrom" runat="server" CssClass="formfields" MaxLength="8"> </asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtChequeFrom"
                                        CssClass="error" ErrorMessage="Please insert cheque from no." ValidationGroup="insertcheque" />
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtChequeFrom"
                                        ValidChars="0123456789">
                                    </cc1:FilteredTextBoxExtender>
                                </li>
                            </ul>
                            <ul>
                                <li class="text">Cheque No. To:
                                </li>
                                <li class="field">
                                    <div>
                                        <asp:TextBox ID="txtChequeTo" runat="server" CssClass="formfields" MaxLength="8"> </asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtChequeTo"
                                        CssClass="error" ErrorMessage="Please insert cheque to no. " ValidationGroup="insertcheque" />
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtChequeTo"
                                        ValidChars="0123456789">
                                    </cc1:FilteredTextBoxExtender>
                                </li>
                                <li class="text"></li>
                                <li class="field">
                                    <div class="float-margin">
                                        <asp:Button ID="btnInsertChkDtls" Text="Submit" runat="server" ToolTip="Search" CssClass="buttonbg"
                                            OnClick="btnInsertChk_Click" ValidationGroup="insertcheque" CausesValidation="true"></asp:Button>
                                    </div>
                                    <div class="float-left">
                                        <asp:Button ID="btnInsertCancel" Text="Cancel" runat="server" ToolTip="Search" CssClass="buttonbg"
                                            OnClick="btInsertCancel_Click"></asp:Button>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <%--<asp:AsyncPostBackTrigger ControlID="btnInsertChkDtls" EventName= "Click" />--%>
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updSubmitAmt" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlSubmitAmt" runat="server" Visible="false">
                    <div class="mainheading">
                        Deposited Cheque
                    </div>
                    <div class="contentbox">
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">Cheque Number:
                                </li>
                                <li class="field">
                                    <div>
                                        <asp:TextBox ID="txtChqNo" runat="server" MaxLength="8" CssClass="formfields"> </asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="txtChqNo"
                                        CssClass="error" ErrorMessage="Please insert cheque no. " ValidationGroup="updateCheque" />
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtChqNo"
                                        ValidChars="0123456789">
                                    </cc1:FilteredTextBoxExtender>
                                </li>
                                <li class="text">Order Number:
                                </li>
                                <li class="field">
                                    <asp:TextBox ID="txtOrderNumber" runat="server" MaxLength="20" CssClass="formfields"> </asp:TextBox>
                                </li>
                                <li class="text">Amount:
                                </li>
                                <li class="field">
                                    <div>
                                        <asp:TextBox ID="txtAmount" runat="server" MaxLength="100" CssClass="formfields"> </asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="txtAmount"
                                        CssClass="error" ErrorMessage="Please insert amount " ValidationGroup="updateCheque" />
                                </li>
                            </ul>
                            <ul>
                                <li class="text">
                                    <asp:Label ID="lblpreffdt" runat="server" Text="">Deposit Date:<span class="error">*</span></asp:Label>
                                </li>
                                <li class="field">
                                    <uc2:ucDatePicker ID="ucDipdate" runat="server" RangeErrorMessage="Invalid date."
                                        ErrorMessage="Please select a date" ValidationGroup="updateCheque" />
                                </li>
                                <li class="text"></li>
                                <li class="field">
                                    <div class="float-margin">
                                        <asp:Button ID="btnUpdate" Text="Submit" runat="server" ToolTip="Search" CssClass="buttonbg"
                                            OnClick="btnUpdate_Click" ValidationGroup="updateCheque" CausesValidation="true"></asp:Button>
                                    </div>
                                    <div class="float-left">
                                        <asp:Button ID="btnCancelUpdate" Text="Cancel" runat="server" ToolTip="Search" CssClass="buttonbg"
                                            OnClick="btnCancelUpdate_Click"></asp:Button>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlSearch" Visible="false" runat="server">
                    <div class="mainheading">
                        Search
                    </div>
                    <div class="contentbox">
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">Cheque Number:<span>&nbsp;</span>
                                </li>
                                <li class="field">
                                    <asp:TextBox ID="txtSerChkNo" runat="server" MaxLength="8" CssClass="formfields"> </asp:TextBox>
                                </li>
                                <li class="text">Cheque Status:<span class="error">*</span>
                                </li>
                                <li class="field">
                                    <div>
                                        <asp:DropDownList ID="cmbChequeStatus" runat="server" CssClass="formselect">
                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                            <asp:ListItem Value="1">Pending</asp:ListItem>
                                            <asp:ListItem Value="2">Deposited</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </li>
                                <li class="text">
                                    <asp:Label ID="Label1" runat="server" Text="">From Date:<span class="error">*</span></asp:Label>
                                </li>
                                <li class="field">
                                    <uc2:ucDatePicker ID="UcDateFrom" runat="server" />
                                </li>
                            </ul>
                            <ul>
                                <li class="text">
                                    <asp:Label ID="Label2" runat="server" Text="">To Date:<span class="error">*</span></asp:Label>
                                </li>
                                <li class="field">
                                    <uc2:ucDatePicker ID="ucDateTo" runat="server" />
                                </li>
                                <li class="text"></li>
                                <li class="field">
                                    <div class="float-margin">
                                        <asp:Button ID="btnSearch" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                            OnClick="btnSearch_Click"></asp:Button>
                                    </div>
                                    <div class="float-left">
                                        <asp:Button ID="btnCancelSer" Text="Cancel" runat="server" ToolTip="Search" CssClass="buttonbg"
                                            OnClick="btnCancelSerch_Click"></asp:Button>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel runat="server" ID="updView" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                    <div class="mainheading">
                        List
                    </div>
                    <div class="contentbox">
                        <div class="grid1">
                            <asp:GridView ID="grdView" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                                RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" HeaderStyle-HorizontalAlign="left"
                                HeaderStyle-VerticalAlign="top" GridLines="none" AlternatingRowStyle-CssClass="Altrow"
                                RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                                CellSpacing="1" CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="false"
                                AllowPaging="True" SelectedStyle-CssClass="gridselected" DataKeyNames="AdvanceChequeDetailID"
                                EmptyDataText="No record found" PageSize='<%$ AppSettings:GridViewPageSize %>'
                                OnPageIndexChanging="grdView_PageIndexChanging">
                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                <Columns>
                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BankName"
                                        HeaderText="Bank">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="AccountNumber"
                                        HeaderText="Acoount No.">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderNumber"
                                        HeaderText="Order Number">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ChequeNumber"
                                        HeaderText="Cheque No.">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ChequeStatus"
                                        HeaderText="Cheque Status">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Amount"
                                        HeaderText="Amount">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="DateoFDeposite"
                                        HeaderText="Depositiondate">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <%-- <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="DateofClearance"
                                                      HeaderText="ClearenceDate">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>--%>
                                </Columns>
                                <PagerStyle CssClass="PagerStyle" />
                                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                <AlternatingRowStyle CssClass="Altrow" />
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <%--  </td>
            </tr>
        </table>--%>
        <%--     <uc3:footer ID="Footer1" runat="server" />--%>
    </div>
</asp:Content>
