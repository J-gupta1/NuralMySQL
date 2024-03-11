<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ViewPISINV.aspx.cs" Inherits="ViewPISINV" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="ucDate" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMessage1" runat="server" />
    <div class="mainheading">
        View PIS/ Invoice
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">DN Number: </li>
                <li class="field">
                    <asp:TextBox ID="txtDN" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                </li>
                <li class="text">Invoice No: </li>
                <li class="field">
                    <asp:TextBox ID="txtInvoice" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                </li>
            </ul>
            <ul>
                <li class="text">From Party Code: <span class="error">&nbsp;</span> </li>
                <li class="field">
                    <asp:TextBox ID="txtFromCode" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                </li>
                <li class="text">To Party Code: <span class="error">&nbsp;</span> </li>
                <li class="field">
                    <asp:TextBox ID="txtToCode" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                </li>
            </ul>
            <ul>
                <li class="text">Date Type: </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="cmbDateType" runat="server" CssClass="formselect">
                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                            <asp:ListItem Text="DN Date" Value="1"></asp:ListItem>
                            <asp:ListItem Text="PSI Date" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Inv Date" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </li>
                <li class="text">From Date: </li>
                <li class="field">
                    <ucDate:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                        ValidationGroup="PIS" defaultDateRange="True" RangeErrorMessage="" />
                </li>
                <li class="text">To Date: </li>
                <li class="field">
                    <ucDate:ucDatePicker ID="UcDateTo" runat="server" ErrorMessage="From date required."
                        ValidationGroup="PIS" defaultDateRange="True" RangeErrorMessage="" />
                </li>
            </ul>
            <ul>
                <li class="text"></li>
                <li class="field">
                    <div class="float-margin">
                        <asp:Button ID="btnSearch" runat="server" ValidationGroup="Serach" CausesValidation="true"
                            CssClass="buttonbg" Text="Search" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnShowAll" runat="server" CssClass="buttonbg" Text="Show All" ToolTip="Show All"
                            OnClick="btnShowAll_Click" />
                    </div>
                </li>
                <li>
                    <div class="float-margin">
                        <asp:Button ID="ExportToExcel" CssClass="buttonbg" runat="server" Text="Export In Excel"
                            OnClick="ExportToExcel_Click2" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div id="dvhide" runat="server" visible="false">
        <div class="mainheading">
            List
        </div>
        <div class="export">
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:GridView ID="GridPSIInv" runat="server" AlternatingRowStyle-CssClass="Altrow"
                    AutoGenerateColumns="false" bgcolor="" AllowPaging="true" PageSize="20" BorderWidth="0px"
                    CellPadding="4" CellSpacing="1" DataKeyNames="SAPDNPSIId,InvoicePdfPath" FooterStyle-CssClass="gridfooter"
                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                    RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                    SelectedStyle-CssClass="gridselected" Width="100%" OnPageIndexChanging="GridPSIInv_PageIndexChanging"
                    OnRowDataBound="GridPSIInv_RowDataBound">
                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                    <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                    <Columns>
                        <asp:TemplateField HeaderText="Print Preview">
                            <ItemTemplate>
                                <asp:Button ID="BtnPrint" CommandName="BtnPrint" runat="server"  CssClass="buttonbg"
                                    Text="Print" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CancelPSI">
                            <ItemTemplate>
                                <asp:Button ID="BtnCancelPSI" CommandName="BtnCancelPSI" runat="server"  CssClass="buttonbg"
                                    Text="Cancel" Visible='<%# Eval("CancelPSI") %>' OnClick="BtnCancelPSI_Click" CommandArgument='<%# Eval("SAPDNPSIId") %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Download Invoice Pdf">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" CssClass="elink" Text="Download" CommandArgument='<%# Eval("InvoicePdfPath") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                        <asp:BoundField DataField="From" HeaderStyle-HorizontalAlign="Left" HeaderText="From"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FromCode" HeaderStyle-HorizontalAlign="Left" HeaderText="FromCode"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="To" HeaderStyle-HorizontalAlign="Left" HeaderText="To"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ToCode" HeaderStyle-HorizontalAlign="Left" HeaderText="ToCode"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DNNumber" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true"
                            HeaderText="DNNumber">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DNDate" HeaderStyle-HorizontalAlign="Left" HeaderText="DNDate"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PSIDate" HeaderStyle-HorizontalAlign="Left" HeaderText="PSIDate"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="InvoiceNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="InvoiceNumber"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="InvoiceDate" HeaderStyle-HorizontalAlign="Left" HeaderText="InvoiceDate"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <%--<asp:BoundField DataField="SKUCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SKUCode"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SKUDesc" HeaderStyle-HorizontalAlign="Left" HeaderText="SKUDesc"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>--%>
                        <%--<asp:BoundField DataField="SKUDesc" HeaderStyle-HorizontalAlign="Left" HeaderText="SKUDesc"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>--%>
                       <%-- <asp:BoundField DataField="DNQuantity" HeaderStyle-HorizontalAlign="Left" HeaderText="DNQuantity"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>--%>
                        <asp:BoundField DataField="PSIQuantity" HeaderStyle-HorizontalAlign="Left" HeaderText="PSIQuantity"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <%--<asp:BoundField DataField="SerialNumber1" HeaderStyle-HorizontalAlign="Left" HeaderText="SerialNumber1"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SerialNumber2" HeaderStyle-HorizontalAlign="Left" HeaderText="SerialNumber2"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>--%>
                    </Columns>
                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                    <AlternatingRowStyle CssClass="Altrow" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
                <div class="clear">
                </div>
            </div>
            <div id="dvFooter" runat="server" class="pagination">
                <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
            </div>
        </div>
    </div>
</asp:Content>
