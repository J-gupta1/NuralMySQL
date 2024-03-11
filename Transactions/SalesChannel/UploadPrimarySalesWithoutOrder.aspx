<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UploadPrimarySalesWithoutOrder.aspx.cs" Inherits="Transactions_SalesChannel_UploadPrimarySales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0" align="center" style="float: left;">
        <tr>
            <td valign="top" align="left">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                   
                    <tr>
                        <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <td align="left" valign="top">
                                    <uc1:ucMessage ID="ucMsg" runat="server" />
                                </td>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="float: left">
                                <tr>
                                    <td align="left" valign="top" class="tableposition">
                                        <div class="mainheading">
                                            Upload</div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <div class="contentbox">
                                <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                         <td colspan="4" align="left" valign="top" height="20" class="mandatory">
                                                    (<font class="error">*</font>) Marked fields are mandatory
                                                </td>
                                        </tr>
                                            <tr>
                                                <td class="formtext" valign="top" align="left" 1 height="35">
                                                                    Select Mode:<font class="error">*</font>
                                                                </td>
                                                                <td  align="left" valign="top" class="formtext" style="margin-top: -5px;
                                                                    float: left;" colspan="2">
                                                                    <%--<uc2:ucDatePicker ID="ucDatePicker"  runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                                                                            RangeErrorMessage="Date should be less then equal to current date." ValidationGroup="insert" />--%>
                                                                    <asp:RadioButtonList ID="rdModeList" runat="server" TextAlign="Right" RepeatDirection="Horizontal"
                                                                        AutoPostBack="true" CellPadding="0" CellSpacing="0" OnSelectedIndexChanged="rdModeList_SelectedIndexChanged"
                                                                        Width="150px">
                                                                        <asp:ListItem Text="Excel" Value="0" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Text="Interface" Value="1"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                               
                                          </tr>
                                          <tr>
                                                <td width="10%" height="35" align="left" class="formtext" valign="top">
                                                    Upload File: <font class="error">*</font>
                                                </td>
                                                <td width="25%" align="left" valign="top">
                                                    <asp:FileUpload ID="FileUpload1" CssClass="form_file" runat="server" />
                                                </td>
                                                <td width="10%" align="left" valign="top">
                                                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                                                        OnClick="btnUpload_Click" />
                                                </td>
                                              
                                                <td valign="top" width="55%" align="left" colspan="3">
                                                    <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                                                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>&nbsp;
                                                    &nbsp;&nbsp; &nbsp;
                                                    <%--<asp:LinkButton ID="DwnldSKUCodeTemplate" runat="server" Text="Download SKUCode"
                                                        OnClick="DwnldSKUCodeTemplate_Click" CssClass="elink2"></asp:LinkButton>--%>
                                                    &nbsp;&nbsp;&nbsp; <a class="elink2" href="../../Excel/Templates/PrimarySales.xlsx">
                                                        Download Template</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpload" />
                                        <asp:PostBackTrigger ControlID="DwnldReferenceCodeTemplate" />
                                        <%-- <asp:PostBackTrigger ControlID="DwnldDustributorCodeTemplate" />--%>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                    <asp:Panel ID="pnlGrid" Visible="false" runat="server">
                        <tr>
                            <td align="left" class="tableposition">
                                <div class="mainheading" runat="server" id="dvUploadPreview" visible="false">
                                    Upload Preview</div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" class="tableposition">
                                <div class="contentbox">
                                    <div class="grid1">
                                        <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="GridSales" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                    CellSpacing="1" DataKeyNames="SKUCode" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                                                    GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                                                    AlternatingRowStyle-CssClass="gridrow1" Width="100%" AllowPaging="false">
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
                                                            HeaderText="WarehouseCode">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Error"
                                                            HeaderText="Error">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="gridheader" />
                                                    <EditRowStyle CssClass="editrow" />
                                                    <AlternatingRowStyle CssClass="gridrow1" />
                                                    <PagerStyle CssClass="PagerStyle" />
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                      
                                    </div>
                                </div>
                            </td>
                        </tr>
                         <tr>
                        <td height="5">
                        </td>
                    </tr>
                     <tr>
                        <td align="left">
                         <asp:Button ID="Btnsave" runat="server" CssClass="buttonbg" Text="Save" ValidationGroup="Add"
                                                OnClick="Btnsave_Click" />
                                            <asp:Button ID="BtnCancel" runat="server" CssClass="buttonbg" Text="Cancel" OnClick="BtnCancel_Click" />
                        </td>
                    </tr>
                    </asp:Panel>
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
