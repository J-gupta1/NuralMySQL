<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GRNUploadBatchWise.aspx.cs" Inherits="Transactions_SalesChannel_GRNUploadBatchWise" MasterPageFile="~/CommonMasterPages/MasterPage.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
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
                <li class="text">Select Warehouse: <span class="error">*</span></li>
                <li class="field">
                    <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="formselect">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlWarehouse" Display="Dynamic"
                        CssClass="error" ValidationGroup="EntryValidation" runat="server" InitialValue="0"
                        ErrorMessage="Please Select Warehouse."></asp:RequiredFieldValidator>
                </li>
                <li class="text">GRN Number: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtGRNNumber" runat="server" MaxLength="20" CssClass="formfields"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="txtGRNNumber" CssClass="error" Display="Dynamic"
                        ValidationGroup="EntryValidation" runat="server" ErrorMessage="Please Insert GRN Number."></asp:RequiredFieldValidator>
                </li>
                <li class="text">GRN Date:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateForGRN" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                        IsRequired="true" ValidationGroup="EntryValidation" />
                </li>
            </ul>
            <ul>
                <li class="text">Invoice Number:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtInvoiceNumber" runat="server" MaxLength="20" CssClass="formfields"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtInvoiceNumber" CssClass="error" Display="Dynamic"
                        ValidationGroup="EntryValidation" runat="server" ErrorMessage="Please Insert Invoice Number."></asp:RequiredFieldValidator>
                </li>
                <li class="text">Invoice Date:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateForInvoice" runat="server" ErrorMessage="Invalid date."
                        IsRequired="True" defaultDateRange="true" ValidationGroup="EntryValidation" />
                </li>
                <%--<td  align="right" class="formtext" valign="top" rowspan="2" visible="false">
                                            Remarks:
                                        </td>--%>
                <asp:Panel ID="pnlVendor" runat="server">
                    <li class="text">Vendor Name:<span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtVendor" runat="server" MaxLength="70" CssClass="formfields"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtInvoiceNumber" CssClass="error" Display="Dynamic"
                            ValidationGroup="EntryValidation" runat="server" ErrorMessage="Please Insert Vendor name."></asp:RequiredFieldValidator>
                    </li>
                </asp:Panel>
                <li>
                    <asp:TextBox ID="txtRemarks" runat="server" MaxLength="20" CssClass="form_textarea"
                        TextMode="MultiLine" Visible="false"></asp:TextBox>
                </li>
            </ul>
            <ul>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />

                </li>
                <li class="text">
                    <asp:Button ID="BtnUpload" runat="server" CssClass="buttonbg" Text="Upload"
                        ValidationGroup="EntryValidation" CausesValidation="true"
                        OnClick="BtnUpload_Click" />
                </li>
            </ul>
            <ul>
              <li class="link">
                    <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download SKU Code"
                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>
                </li>
                <li class="link">
                    <asp:LinkButton ID="btnwarehousecode" runat="server" Text="Download WareHouse Code"
                        CssClass="elink2" OnClick="DwnldWarehouseTemplate_Click"></asp:LinkButton>
                </li>
                <li class="link"><a class="elink2" href="../../Excel/Templates/GRNBatchOrSerial.xlsx">Download Template</a>
                </li>                
            </ul>
        </div>
    </div>
    <asp:Panel ID="pnlGrid" Visible="false" runat="server">
        <div id="Div1" class="mainheading" runat="server">
            Upload Preview
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="GridGRN" runat="server" AutoGenerateColumns="false" CellPadding="4"
                            CellSpacing="1" DataKeyNames="SKUCode,BatchNumber" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                            GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                            AlternatingRowStyle-CssClass="Altrow" Width="100%" AllowPaging="true" OnPageIndexChanging="GridGRN_PageIndexChanging">
                            <RowStyle CssClass="gridrow" />
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUCode"
                                    HeaderText="SKUCode">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Quantity"
                                    HeaderText="Quantity">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BatchNumber"
                                    HeaderText="BatchNumber">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SerialNumber"
                                    HeaderText="SerialNumber">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ERRORMessage"
                                    HeaderText="ErrorMessage">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="gridheader" />
                            <EditRowStyle CssClass="editrow" />
                            <AlternatingRowStyle CssClass="Altrow" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>                
            </div>
        </div>
        <div class="float-margin">
            <asp:Button ID="Btnsave" runat="server" CssClass="buttonbg" Text="Save"
                ValidationGroup="Add" OnClick="Btnsave_Click" />
        </div>
        <div class="float-margin">
            <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel"
                OnClick="btnCancel_Click" />
        </div>
    </asp:Panel>
</asp:Content>
