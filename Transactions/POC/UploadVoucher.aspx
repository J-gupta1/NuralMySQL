<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadVoucher.aspx.cs" Inherits="Transactions_POC_UploadVoucher"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMsg" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="mainheading">
            Upload
        </div>
        <asp:UpdatePanel runat="server" ID="UpdControl" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="contentbox">
                    <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                    <div class="H25-C3-S">                   
                        <ul>
                            <%--    <td class="formtext" valign="top" align="right" width="10%" >
                                                                                Select Mode:<font class="error">*</font>
                                                                            </td>
                                                <td width="25%" align="left" valign="top" class="formtext" > <asp:RadioButtonList ID="rdModelList" runat="server" TextAlign="Right" 
                                                                                    RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0" BorderWidth="0"  
                                                                                    AutoPostBack="True" 
                                                                                    onselectedindexchanged="rdModelList_SelectedIndexChanged">
                                                                                    <asp:ListItem Text="Excel" Value="0" Selected="True"></asp:ListItem>
                                                                                    <asp:ListItem Text="Interface" Value="1"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                                 </td>--%>
                            <!--  <td class="formtext" valign="top" align="right" width="10%" height="35">
                                                                                Select Mode:<font class="error">*</font>
                                                                            </td>-->
                            <%--   <td class="formtext" valign="top" align="right" width="13%" height="35">
                                                                                Select Return Date:<font class="error">*</font>
                                                                            </td>
                                                                            <td width="10%" align="left" valign="top">
                                                                                <uc2:ucDatePicker ID="ucSalesReturnDate" runat="server" ErrorMessage="Invalid date."
                                                                                    IsRequired="true" defaultDateRange="true" RangeErrorMessage="Date should be less then equal to current date."
                                                                                    ValidationGroup="SalesReturn" />
                                                                            </td>--%>
                            <li class="text">Select Excel: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:FileUpload ID="Fileupdload" CssClass="fileuploads" runat="server" />                                
                                <asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                            </li>
                            <li class="field3">
                                <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                            </li> 
                            <li class="link">
                                <a href="../../Excel/Templates/UploadVoucher.xlsx" class="elink2">Download Voucher Template</a>
                            </li>
                        </ul>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnupload" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="mainheading" runat="server" id="pnlGrid" visible="false">
            Upload Preview
        </div>
        <div class="contentbox" runat="server" id="pnlGrid1" visible="false">
            <div class="grid1">
                <%--<asp:UpdatePanel runat="server" ID="updgrid1" UpdateMode="Conditional">
                                            <ContentTemplate>--%>
                <asp:GridView ID="gvVoucher" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    CellSpacing="1" DataKeyNames="PartyCode" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                    GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                    AlternatingRowStyle-CssClass="Altrow" Width="100%" AllowPaging="false">
                    <RowStyle CssClass="gridrow" />
                    <Columns>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="VoucherNumber"
                            HeaderText="VoucherNumber">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Sales Date">
                            <ItemTemplate>
                                <asp:Label ID="lblVoucherDate" runat="server" Text='<%# Eval("VoucherDate","{0:dd MMM yy}") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="VoucherType"
                            HeaderText="VoucherType">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="DocNo"
                            HeaderText="DocNo">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="PartyCode"
                            HeaderText="PartyCode">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Amount"
                            HeaderText="Amount">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Narration"
                            HeaderText="Narration">
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
                </asp:GridView>
                <%--  </ContentTemplate>
                                        </asp:UpdatePanel>--%>
            </div>
            <div class="margin-top">
                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="always">
                    <ContentTemplate>
                        <div class="float-margin">
                            <asp:Button ID="btnSave" Text="Save" runat="server" ToolTip="Save" CssClass="buttonbg"
                                CausesValidation="true" ValidationGroup="SalesReturn" OnClick="btnSave_Click"></asp:Button>
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" ToolTip="Cancel" CssClass="buttonbg"
                                OnClick="btnCancel_Click"></asp:Button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSave" />
                        <asp:PostBackTrigger ControlID="btnCancel" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
