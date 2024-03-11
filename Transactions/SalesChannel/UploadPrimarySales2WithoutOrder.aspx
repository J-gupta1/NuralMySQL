<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="UploadPrimarySales2WithoutOrder.aspx.cs" Inherits="Transactions_SalesChannel_UploadPrimarySales2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div align="center">
        <%-- <uc2:header ID="Header1" runat="server" />--%>
        <table cellspacing="0" cellpadding="0" width="965" border="0">
            <tr>
                <td>
                    <%-- <td align="left" valign="top">--%>
                    <table cellspacing="0" cellpadding="0" width="965" border="0">
                        <tr>
                            <td valign="top" align="left">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <%--  <tr>
                                                            <td align="left" valign="top">
                                                                <uc1:ucMessage ID="ucMsg" runat="server" />
                                                            </td>
                                                        </tr>--%>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <uc1:ucMessage ID="ucMsg" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
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
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <tr>
                                                <td valign="top" align="left" class="tableposition">
                                                    <div class="contentbox">
                                                        <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                            <tr>
                                                                <td colspan="6" align="left" valign="top" height="15" class="mandatory">
                                                                    (<font class="error">*</font>) Marked fields are mandatory
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="formtext" valign="top" align="left" width="10%" height="35">
                                                                    Select Mode:<font class="error">*</font>
                                                                </td>
                                                                <td width="20%" align="left" valign="top" style="padding-top: 0px; margin-top: 0px;">
                                                                    <%--<uc2:ucDatePicker ID="ucDatePicker"  runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                                                                            RangeErrorMessage="Date should be less then equal to current date." ValidationGroup="insert" />--%>
                                                                    <asp:RadioButtonList ID="rdModeList" runat="server" TextAlign="Right" RepeatDirection="Horizontal"
                                                                        AutoPostBack="true" CellPadding="0" CellSpacing="0" OnSelectedIndexChanged="rdModeList_SelectedIndexChanged"
                                                                        Width="150px">
                                                                        <asp:ListItem Text="Excel" Value="0" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Text="Interface" Value="1"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                                <td class="formtext" valign="top" align="right" width="10%" height="35">
                                                                    Select Excel<font class="error">*</font>
                                                                </td>
                                                                <td width="30%" align="left" valign="top">
                                                                    <asp:FileUpload ID="Fileupdload" CssClass="form_file" runat="server" />
                                                                    <br />
                                                                    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                                                                </td>
                                                                <td class="formtext" valign="top" align="left" width="30%">
                                                                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="left" colspan="6">
                                                                    <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                                                                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>&nbsp;
                                                                    &nbsp;&nbsp; &nbsp;
                                                                    <%--  <asp:LinkButton ID="DwnldSKUCodeTemplate" runat="server" CssClass="elink2" Text="Download SKUCode"
                                                                        OnClick="DwnldSKUCodeTemplate_Click"></asp:LinkButton>&nbsp; &nbsp;--%>
                                                                    <%--   <asp:LinkButton ID="DwnldSalesTemplate" runat="server" 
                                                                            Text="Download Sales Template" onclick="DwnldSalesTemplate_Click"></asp:LinkButton>--%>
                                                                    <a href="../../Excel/Templates/SalesTemplate.xlsx" class="elink2">Download Sales Template</a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnUpload" />
                                            <asp:PostBackTrigger ControlID="DwnldReferenceCodeTemplate" />
                                            <%-- <asp:PostBackTrigger ControlID="DwnldSKUCodeTemplate" />--%>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <%--kk--%>
                                    <tr>
                                        <td align="left" class="tableposition">
                                            <div class="mainheading" runat="server" id="dvUploadPreview" visible="false">
                                                Upload Preview</div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class="tableposition">
                                            <div class="contentbox" runat="server" id="pnlGrid">
                                                <div class="grid1">
                                                    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <%--<asp:Panel ID="pnlGrid" runat="server" Visible="true">--%>
                                                            <asp:GridView ID="gvSales" runat="server" AutoGenerateColumns="False" CellPadding="4"
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
                                                                    <%--   <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="InvoiceDate"
                                                    HeaderText="Invoice Date" DataFormatString="{0:dd-MMM-yy}">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>--%>
                                                                    <%--  <asp:TemplateField HeaderText="Invoice Date" ItemStyle-Width="85px">
                                                                        <ItemStyle Wrap="False" />
                                                                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblModifyDate" runat="server" Text='<%# Convert.ToDateTime(Eval("InvoiceDate")).ToString("dd MMM yy")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>--%>
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
                                                                    <%-- <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesMan"
                                                                        HeaderText="Salesman">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>--%>
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
                                                            <%--                                    </asp:Panel>--%></ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" valign="top" height="5">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" height="25" class="formtext">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="always">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnSave" Text="Save" runat="server" ToolTip="Save" CssClass="buttonbg"
                                                        OnClick="btnSave_Click"></asp:Button>
                                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" ToolTip="Cancel" CssClass="buttonbg"
                                                        OnClick="btnCancel_Click"></asp:Button>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnSave" />
                                                    <asp:PostBackTrigger ControlID="btnCancel" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <%--     <uc3:footer ID="Footer1" runat="server" />--%>
    </div>
</asp:Content>
