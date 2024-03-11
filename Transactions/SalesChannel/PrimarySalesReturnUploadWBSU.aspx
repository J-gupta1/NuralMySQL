<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="PrimarySalesReturnUploadWBSU.aspx.cs" Inherits="Transactions_SalesChannel_PrimarySalesReturnUploadWBSU" %>

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
                        <td align="left" valign="top">
                            <uc1:ucMessage ID="ucMsg" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" height="5">
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="float: left">
                                <tr>
                                    <td align="left" valign="top" class="tableposition">
                                        <div class="mainheading">
                                           Upload Primary Sales 1 Return</div>
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
                                        <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                            <tr>
                                                <td colspan="5" align="left" valign="top" height="15" class="mandatory">
                                                    (*) Marked fields are mandatory
                                                </td>
                                            </tr>
                                            <div id="dvHide" runat="server" visible="false">
                                            </div>
                                            <tr>
                                                <td width="10%" align="left" class="formtext" valign="top">
                                                    Return Date:<font class="error">*</font>
                                                </td>
                                                <td width="15%" align="left" valign="top">
                                                    <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="false"
                                                        RangeErrorMessage="Date should be less then equal to current date." ValidationGroup="Add" />
                                                </td>
                                                <td width="10%" align="right" class="formtext" valign="top">
                                                    Upload File: <font class="error">*</font>
                                                </td>
                                                <td align="left" width="25%" valign="top">
                                                    <asp:FileUpload ID="FileUpload1" CssClass="form_file" runat="server" />
                                                    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text="" Visible="false"></asp:Label>
                                                </td>
                                                <td align="left" width="40%" valign="top">
                                                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                                                        OnClick="btnUpload_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left">&nbsp;
                                                    
                                                </td>
                                                <td valign="top" align="left" colspan="4">
                                                    <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                                                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>&nbsp;
                                                    &nbsp;&nbsp; &nbsp;
                                                    <%-- <asp:LinkButton ID="DwnldSKUCodeTemplate" runat="server" CssClass="elink2" 
                                                                            Text="Download SKUCode" onclick="DwnldSKUCodeTemplate_Click"></asp:LinkButton> &nbsp; &nbsp; &nbsp; --%>
                                                    <a class="elink2" href="../../Excel/Templates/PrimarySalesReturnMicromax.xlsx">Download Template</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpload" />
                                        <asp:PostBackTrigger ControlID="DwnldReferenceCodeTemplate" />
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
                        <td align="left" valign="top">
                            
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="90%" align="left" class="tableposition">
                                            <div class="mainheading">
                                                Sales List</div>
                                        </td>
                                        <td width="10%" align="right">
                                        </td>
                                    </tr>
                                </table>
                                <div class="contentbox">
                                    <div class="grid1">
                                        <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="GridSalesReturn" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                    AllowPaging="false" CellSpacing="1" DataKeyNames="SKUCode" EditRowStyle-CssClass="editrow"
                                                    EmptyDataText="No Record Found" GridLines="None" HeaderStyle-CssClass="gridheader"
                                                    RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="gridrow1" Width="100%">
                                                    <RowStyle CssClass="gridrow" />
                                                    <Columns>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelCode"
                                                            HeaderText="SalesChannelCode">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="InvoiceNumber"
                                                            HeaderText="Invoice Number">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="InvoiceDate"
                                                            HeaderText="Invoice Date" DataFormatString="{0:dd-MMM-yy}">
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
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Error"
                                                            HeaderText="Error">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="gridheader" />
                                                    <EditRowStyle CssClass="editrow" />
                                                    <AlternatingRowStyle CssClass="gridrow1" />
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
                        <td>
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


