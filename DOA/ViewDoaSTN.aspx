<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="ViewDoaSTN.aspx.cs" Inherits="DOA_ViewDoaSTN" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function Popup(StockDispatchID) {
            if (StockDispatchID.length > 0) {
                window.open("frmPrintDispatchDOA.aspx?StockDispatchID=" + StockDispatchID, "mywindow3", "menubar=0,width=700,height=600,left=10,top=10,scrollbars=yes");
            }
            return false;
        }
    </script>
    <script type="text/javascript">
        function Popupgrn(stockReceiveId) {
            if (stockReceiveId.length > 0) {
                window.open("frmGRNPrint.aspx?stockReceiveId=" + stockReceiveId, "mywindow3", "menubar=0,width=700,height=600,left=10,top=10,scrollbars=yes");
            }
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel ID="UpdMain" runat="server">
            <ContentTemplate>
                <div>
                    <uc4:ucMessage ID="ucMessage1" runat="server" />
                    <div class="mainheading">
                        View DOA STN
                    </div>
                    <div class="contentbox">
                        <div class="mandatory">
                             (*) Marked fields are mandatory. (+) Marked fields are optional.          
                        </div>
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">STN No:<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <asp:TextBox ID="txtSTNNo" runat="server" CssClass="formfields" MaxLength="17"></asp:TextBox>
                                </li>
                                <li class="text">IMEI Serial No:<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <asp:TextBox ID="txtIMEINo" runat="server" CssClass="formfields" MaxLength="30"></asp:TextBox>
                                </li>
                                <li class="text">DOA Certificate No:<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <asp:TextBox ID="txtDoaCertificatenumber" runat="server" CssClass="formfields" MaxLength="30"></asp:TextBox>
                                </li>
                                <li class="text">Dispatch From Date:<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>
                                <li class="text">Dispatch To Date:<span class="error">+</span>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>
                                <li class="text">Receive Status:<span class="error">+</span>
                                    <%-- <div class="float-margin">
                                                                <asp:Button ID="Search" runat="server" Text="Search" CssClass="buttonbg"
                                                                    CausesValidation="true" ValidationGroup="Search" OnClick="Search_Click" />
                                                            </div>
                                                            <div class="float-left">
                                                                <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="buttonbg"
                                                                    CausesValidation="false" OnClick="Cancel_Click" />
                                                            </div>--%>
                                </li>
                                <li class="field">
                                    <asp:DropDownList ID="ddlReceivestatus" CssClass="formselect" runat="server"></asp:DropDownList>
                                </li>
                                <li class="text">GRN Number:<span class="error">+</span></li>
                                <li class="field">
                                    <asp:TextBox ID="txtGRNNO" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox></li>
                                <li class="text">
                                    <asp:HiddenField ID="hdfreceiveid" runat="server" />
                                </li>
                                <li class="field3">
                                    <div class="float-margin">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg"
                                            CausesValidation="true" ValidationGroup="Search" OnClick="btnSearch_Click" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg"
                                            CausesValidation="false" OnClick="btnCancel_Click" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="btnExportexcel" runat="server" Text="Export To Excel" CssClass="buttonbg"
                                            CausesValidation="true" ValidationGroup="Search" OnClick="btnExportexcel_Click" />
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Panel ID="dvhidegrid" runat="server">
                        <div class="mainheading">
                            List
                        </div>
                        <div class="contentbox">
                            <div class="grid1">
                                <asp:GridView ID="GridDOA" runat="server" AllowPaging="false" AlternatingRowStyle-CssClass="Altrow"
                                    bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                    DataKeyNames="DispatchItemID" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                    AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>'>
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <PagerStyle CssClass="gridfooter" />
                                    <Columns>
                                        <asp:BoundField DataField="receiveStatus" HeaderStyle-HorizontalAlign="Left" HeaderText="DOA Receive Status."
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
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
                    </asp:Panel>
                </div>
                <div>
                    <asp:Panel ID="PnlViewStnGrid" runat="server" Visible="false">
                        <div class="mainheading">
                            View STN Details
                        </div>
                        <div class="contentbox">
                            <div class="grid1">
                                <asp:GridView ID="gvSTNDetail" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="Altrow"
                                    bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                    DataKeyNames="StockDispatchID,StockReceiveID" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%" OnRowDataBound="gvSTNDetail_RowDataBound"
                                    AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>'>
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <PagerStyle CssClass="gridfooter" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="View Dispatch">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkViewDetails" runat="server" AutoPostBack="True" OnCheckedChanged="ChkViewDetails_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debit Note Print">
                                            <ItemTemplate>
                                                <asp:Button ID="btndebitprint" runat="server" CommandName="DebitnotePrint" CommandArgument='<%#Eval("StockDispatchID") %>' Text="Debit Note Print" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GRN Print">
                                            <ItemTemplate>
                                                <asp:Button ID="btnGrnprint" runat="server" Text="GRN Print" Visible='<%# Convert.ToBoolean(Eval("ReceiveStatus")) %>' />
                                                <asp:Label ID="lblGrnprint" runat="server" Text='<%# Eval("StockReceiveID") %>' Style="display: none"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="STNNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="STN No"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GRNNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="GRN No"
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
                            <div id="dvFooter" runat="server" class="pagination">
                                <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>

            </ContentTemplate>
            <Triggers>

                <asp:PostBackTrigger ControlID="btnExportexcel" />
                <asp:PostBackTrigger ControlID="btnCancel" />
                <asp:PostBackTrigger ControlID="btnSearch" />

            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>


