<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="BatchwiseReport.aspx.cs" Inherits="Reports_SalesChannel_BatchwiseReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/SalesEntryGrid.ascx" TagName="ucSalesEntryGrid"
    TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Upload
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Batch From:
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucBatchDateFrom" runat="server" ErrorMessage="Invalid date."
                        IsRequired="false" defaultDateRange="true" />
                </li>
                <li class="text">Batch To:
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucBatchDateTo" runat="server" ErrorMessage="Invalid date."
                        IsRequired="False" defaultDateRange="true" />
                </li>
                <li class="text">Batch Number:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtBatchNumber" runat="server" MaxLength="20" CssClass="formfields"></asp:TextBox>
                </li>            
                <li class="text">SkuCode:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtSkuCode" runat="server" MaxLength="20" CssClass="formfields"></asp:TextBox>
                </li>
                <li class="text"></li>
                <li class="field">
                    <div class="float-margin">
                        <asp:Button ID="btnSearch" runat="server" CssClass="buttonbg" Text="Search" ValidationGroup="EntryValidation"
                            CausesValidation="true" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" ValidationGroup="EntryValidation"
                            CausesValidation="true" OnClick="btnCancel_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <asp:Panel ID="pnlGrid" Visible="false" runat="server">
        <div id="Div1" class="mainheading" runat="server">
            List
        </div>
        <div class="export">
            <asp:Button ID="btnExportToExcel" runat="server" CssClass="excel"
                CausesValidation="False" OnClick="btnExportToExcel_Click" />
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="grid1">
                        <asp:GridView ID="GridBatchStock" runat="server" AutoGenerateColumns="false" CellPadding="4"
                            CellSpacing="1" DataKeyNames="BatchID" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                            GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                            AlternatingRowStyle-CssClass="Altrow" Width="100%" AllowPaging="true" OnPageIndexChanging="GridBatchStock_PageIndexChanging">
                            <RowStyle CssClass="gridrow" />
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BatchCode"
                                    HeaderText="BatchNumber">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BatchName"
                                    HeaderText="BatchName">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="BatchDate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBatchDate" runat="server" Text='<%# Eval("BatchDate","{0:dd MMM yy}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUCode"
                                    HeaderText="SKUCode">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="QuantityIn"
                                    HeaderText="QuantityIn">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="QuantityInHand"
                                    HeaderText="QuantityInHand">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Amount"
                                    HeaderText="Amount">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="gridheader" />
                            <EditRowStyle CssClass="editrow" />
                            <AlternatingRowStyle CssClass="Altrow" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--<div style="float: left; padding-top: 10px; width: 300px;">
                </div>--%>
        </div>
        <%--<asp:Button ID="Btnsave" runat="server" CssClass="buttonbg" Text="Save" 
                                ValidationGroup="Add" onclick="Btnsave_Click" />
                         <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" 
                                onclick="btnCancel_Click"/>--%>
    </asp:Panel>
</asp:Content>

