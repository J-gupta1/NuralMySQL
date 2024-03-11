<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="PrimarySalesBatchUpload.aspx.cs" Inherits="Transactions_POC_PrimarySalesBatchUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Upload
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    
                    <%-- <tr>
                                                <td class="formtext" valign="top" align="left" 1 height="35">
                                                                    Select Mode:<font class="error">*</font>
                                                                </td>
                                                                <td  align="left" valign="top" class="formtext" style="margin-top: -5px;
                                                                    float: left;" colspan="2">
                                                                   <uc2:ucDatePicker ID="ucDatePicker"  runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                                                                            RangeErrorMessage="Date should be less then equal to current date." ValidationGroup="insert" />
                                                                    <asp:RadioButtonList ID="rdModeList" runat="server" TextAlign="Right" RepeatDirection="Horizontal"
                                                                        AutoPostBack="true" CellPadding="0" CellSpacing="0" OnSelectedIndexChanged="rdModeList_SelectedIndexChanged"
                                                                        Width="150px">
                                                                        <asp:ListItem Text="Excel" Value="0" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Text="Interface" Value="1"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                               
                                          </tr>--%>
                    <ul>
                        <li class="text">Upload File: <font class="error">*</font>
                        </li>
                        <li class="field">
                            <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                        </li>
                        <li class="field3">
                            <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                                OnClick="btnUpload_Click" />
                        </li>
                        <li class="link">
                            <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                                CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>
                        </li>
                        <li class="link">
                            <a class="elink2" href="../../Excel/Templates/PrimarySalesBatch.xlsx">Download Template</a>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnUpload" />
                <asp:PostBackTrigger ControlID="DwnldReferenceCodeTemplate" />
                <%-- <asp:PostBackTrigger ControlID="DwnldDustributorCodeTemplate" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <asp:Panel ID="pnlGrid" Visible="false" runat="server">
        <div class="mainheading" runat="server" id="dvUploadPreview" visible="false">
            Upload Preview
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="GridSales" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            CellSpacing="1" DataKeyNames="SKUCode" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                            GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                            AlternatingRowStyle-CssClass="Altrow" Width="100%" AllowPaging="false">
                            <RowStyle CssClass="gridrow" />
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelCode"
                                    HeaderText="Sales Channel Code">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="InvoiceNumber"
                                    HeaderText="Invoice Number">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="InvoiceDate"
                                    HeaderText="Invoice Date" DataFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUCode"
                                    HeaderText="SKU Code">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Quantity"
                                    HeaderText="Quantity">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="WarehouseCode"
                                    HeaderText="Warehouse Code">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderNumber"
                                    HeaderText="Order Number">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Error"
                                    HeaderText="Error">
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
            <asp:Button ID="Btnsave" runat="server" CssClass="buttonbg" Text="Save" ValidationGroup="Add"
                OnClick="Btnsave_Click" />
        </div>
        <div class="float-margin">
            <asp:Button ID="BtnCancel" runat="server" CssClass="buttonbg" Text="Cancel" OnClick="BtnCancel_Click" />
        </div>
    </asp:Panel>
</asp:Content>
