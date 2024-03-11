<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="DownloadSalesRptLastSale.aspx.cs" Inherits="Reports_SalesChannel_DownloadSalesRptLastSale" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMsg" runat="server" />
    <div class="mainheading">
        Sales Report Search
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Sales Type:<span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlSalesType" runat="server" CssClass="formselect">
                        </asp:DropDownList>

                        <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlSalesType"
                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales type."
                            SetFocusOnError="true" ValidationGroup="SalesReport"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">Date From: <span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                        ValidationGroup="SalesReport" defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />

                </li>
                <li class="text">Date To: <span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="SalesReport"
                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                </li>
            </ul>
            <ul>
                <li class="text">
                    <asp:Label ID="lbllocation" runat="server" Text="">Region:  <span class="error">&nbsp;</span></asp:Label>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddllocation" runat="server" CssClass="formselect" AutoPostBack="false">
                    </asp:DropDownList>
                </li>
                <li class="text">
                    <asp:Label ID="lblState" runat="server" Text="">State:  <span class="error">&nbsp;</span></asp:Label>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlState" runat="server" CssClass="formselect" AutoPostBack="false">
                    </asp:DropDownList>
                </li>
                <li class="text">
                    <asp:Label ID="lblProductCategory" runat="server" Text="">Product Category:  <span class="error">&nbsp;</span></asp:Label>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlProductCategory" runat="server" CssClass="formselect"
                        AutoPostBack="false">
                    </asp:DropDownList>
                </li>
            </ul>
            <ul>
                <li class="text">
                    <asp:Label ID="lblsku" runat="server" Text="">Model Name: <span class="error">&nbsp;</span></asp:Label>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlModelName" CssClass="formselect" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlModelName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator runat="server" ID="valprodcat" ControlToValidate="ddlModelName"
                            CssClass="error" ValidationGroup="SalesReport1" ErrorMessage="Please select a Model Name "
                            InitialValue="0" />
                    </div>
                </li>
                <li class="text">
                    <asp:Label ID="lblSkuName" runat="server" Text="">Sku Name: <span class="error">&nbsp;</span></asp:Label>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlSku" runat="server" CssClass="formselect" AutoPostBack="false">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator runat="server" ID="valModel" ControlToValidate="ddlSku"
                            CssClass="error" ValidationGroup="SalesReport1" ErrorMessage="Please select a SKU "
                            InitialValue="0" />
                    </div>
                </li>
                <li class="text"></li>
                <li class="field">
                    <asp:CheckBox ID="chkZeroQuantity" runat="server" />
                    <asp:Label ID="lblZeroQuantity" runat="server" Text="" CssClass="frmtxt1">Show Zero Qty Records</asp:Label>

                </li>
            </ul>
            <ul>
                <li class="text"></li>
                <li class="field">
                    <asp:CheckBox ID="chkSB" runat="server" Text="With Serial /Batch" TextAlign="Right" Checked="True" Enabled="False" />
                </li>
                <li>
                    <div class="float-margin">
                        <asp:HyperLink ID="btnExportToExcel" runat="server" Visible="false" NavigateUrl="~/Excel/Download/BcpFile/Stock.csv">Open File</asp:HyperLink>

                        <asp:Button ID="btnSearch" Text="Download" runat="server" ValidationGroup="SalesReport"
                            ToolTip="Download" CssClass="buttonbg" CausesValidation="true" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-left">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                            OnClick="btnCancel_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
