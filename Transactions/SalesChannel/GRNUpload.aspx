<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" 
AutoEventWireup="true" CodeFile="GRNUpload.aspx.cs" Inherits="Transactions_SalesChannel_GRNUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" Runat="Server">

<table cellspacing="0" cellpadding="0" width="965" border="0" align="center" style="float: left;">
        <tr>
            <td valign="top" align="left">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                   
                    <tr>
                        <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
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
                                <%--<asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
                                    <ContentTemplate>--%>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="padding-left:10px;">
                                            <tr>
                                                <td colspan="4" align="left" valign="top" height="20" class="mandatory">
                                                    (*) Marked fields are mandatory
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%" height="35" align="right" class="formtext" valign="top">
                                                    Upload File: <font class="error">*</font>
                                                </td>
                                                <td width="30%" align="left" valign="top">
                                                    <asp:FileUpload ID="FileUpload1" CssClass="form_file" runat="server" />
                                                </td>
                                                <td width="15%" align="left" valign="top">
                                                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                                                        OnClick="btnUpload_Click" />
                                                </td>
                                                <td width="45%" align="left" valign="top">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" colspan="4">
                                                    <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download SKU Code"
                                                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>&nbsp;
                                                    &nbsp;&nbsp; &nbsp;
                                                     <asp:LinkButton ID="btnwarehousecode" runat="server" Text="Download WareHouse Code"
                                                        CssClass="elink2" OnClick="DwnldWarehouseTemplate_Click"></asp:LinkButton>&nbsp;
                                                    &nbsp;&nbsp; &nbsp;
                                                    
                                                    <%--<asp:LinkButton ID="DwnldSKUCodeTemplate" runat="server" Text="Download SKUCode"
                                                        OnClick="DwnldSKUCodeTemplate_Click" CssClass="elink2"></asp:LinkButton>--%>
                                                    &nbsp;&nbsp;&nbsp; <a class="elink2" href="../../Excel/Templates/GRN.xlsx">
                                                        Download Template</a>
                                                </td>
                                            </tr>
                                        </table>
                                    <%--</ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpload" />
                                        <asp:PostBackTrigger ControlID="DwnldReferenceCodeTemplate" />
                                         <asp:PostBackTrigger ControlID="DwnldDustributorCodeTemplate" />--%>
                                  <%--  </Triggers>
                                </asp:UpdatePanel>--%>
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
                                                <asp:GridView ID="GridGRN" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                    CellSpacing="1" DataKeyNames="SKUCode" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                                                    GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                                                    AlternatingRowStyle-CssClass="gridrow1" Width="100%" AllowPaging="false" OnPageIndexChanging="GridGRN_PageIndexChanging">
                                                    <RowStyle CssClass="gridrow" />
                                                    <Columns>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelCode"
                                                            HeaderText="Sales Channel Code">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                         <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="GRNNumber"
                                                            HeaderText="GRN Number">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="GRNDate"
                                                            HeaderText="GRN Date"  DataFormatString="{0:MM/dd/yy}">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="PONumber"
                                                            HeaderText="PO Number">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:MM/dd/yy}" DataField="PODate"
                                                            HeaderText="PO Date">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="InvoiceNumber"
                                                            HeaderText="Invoice Number">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="InvoiceDate"
                                                            HeaderText="Invoice Date" DataFormatString="{0:MM/dd/yy}">
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
                                                    <PagerStyle CssClass="PagerStyle" />
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div style="float: left; padding-top: 10px; width: 300px;">
                                           </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                        <td align="left" height="5"></td>
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
                        <td align="left" height="10"></td>
                        </tr>
                </table>
            </td>
        </tr>
    </table>


</asp:Content>

