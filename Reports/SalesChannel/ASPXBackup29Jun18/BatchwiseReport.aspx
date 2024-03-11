<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
 AutoEventWireup="true" CodeFile="BatchwiseReport.aspx.cs" Inherits="Reports_SalesChannel_BatchwiseReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/SalesEntryGrid.ascx" TagName="ucSalesEntryGrid"
    TagPrefix="uc3" %>

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
                                    <td align="left" valign="top" class="tableposition" width="70%">
                                        <div class="mainheading">
                                            Upload</div>
                                    </td>
                                    <td width="30%" align="right" style="padding-right: 10px;">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <div class="contentbox">
                                <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="6" align="left" valign="top" height="20" class="mandatory">
                                            (<font class="error">*</font>) Marked fields are mandatory
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="14%" align="right" class="formtext" valign="top">
                                            Batch From:
                                        </td>
                                        <td width="20%" align="left" valign="top">
                                            <div style="float: left; width: 135px;">
                                                <uc2:ucDatePicker ID="ucBatchDateFrom" runat="server" ErrorMessage="Invalid date."
                                                    IsRequired="false" defaultDateRange="true" />
                                                <br />
                                            </div>
                                            <div style="float: left; width: 170px;">
                                            </div>
                                        </td>
                                        <td width="10%" align="right" class="formtext" valign="top">
                                            Batch To:
                                        </td>
                                        <td width="26%" align="left" valign="top">
                                            <uc2:ucDatePicker ID="ucBatchDateTo" runat="server" ErrorMessage="Invalid date."
                                                IsRequired="False" defaultDateRange="true" />
                                            <br />
                                        </td>
                                        <td width="10%" align="right" class="formtext" valign="top">
                                            Batch Number:
                                        </td>
                                        <td width="20%" align="left" valign="top">
                                            <asp:TextBox ID="txtBatchNumber" runat="server" MaxLength="20" CssClass="form_input9"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="formtext" valign="top">
                                            SkuCode:
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="txtSkuCode" runat="server" MaxLength="20" CssClass="form_input9"></asp:TextBox>
                                        </td>
                                        <td align="right" valign="top" class="formtext">
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Button ID="btnSearch" runat="server" CssClass="buttonbg" Text="Search" ValidationGroup="EntryValidation"
                                                CausesValidation="true" OnClick="btnSearch_Click" />
                                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" ValidationGroup="EntryValidation"
                                                CausesValidation="true" OnClick="btnCancel_Click" />
                                        </td>
                                        <td align="right" class="formtext" valign="top" rowspan="2">
                                        </td>
                                        <td align="left" valign="top" rowspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left">
                                        </td>
                                        <td valign="top" align="left" colspan="4">
                                        </td>
                                        <td valign="top" align="left">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                    <asp:Panel ID="pnlGrid" Visible="false" runat="server"> 
                        <tr>
                        <td>
                        <table>
                        <tr>
                            <td align="left" class="tableposition">
                                <div id="Div1" class="mainheading" runat="server">
                                    List</div>
                            </td>
                            <td width="10%" align="right">
                                <asp:Button ID="btnExportToExcel" runat="server" CssClass="excel" 
                                    CausesValidation="False" onclick="btnExportToExcel_Click" />
                            </td>
                            </tr>
                            </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" class="tableposition">
                                <div class="contentbox">
                                    <div class="grid2">
                                        <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="GridBatchStock" runat="server" AutoGenerateColumns="false" CellPadding="4"
                                                    CellSpacing="1" DataKeyNames="BatchID" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                                                    GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                                                    AlternatingRowStyle-CssClass="gridrow1" Width="100%" AllowPaging="true" OnPageIndexChanging="GridBatchStock_PageIndexChanging">
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
                                                            <HeaderStyle HorizontalAlign="Left"/>
                                                </asp:TemplateField>
                                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUCode"
                                                                HeaderText="SKUCode">
                                                                <headerstyle horizontalalign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="QuantityIn"
                                                                HeaderText="QuantityIn">
                                                                <headerstyle horizontalalign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="QuantityInHand"
                                                                HeaderText="QuantityInHand">
                                                                <headerstyle horizontalalign="Left" />
                                                            </asp:BoundField>
                                                               <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Amount"
                                                                HeaderText="Amount">
                                                                <headerstyle horizontalalign="Left" />
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
                            <td align="left">
                                <%--<asp:Button ID="Btnsave" runat="server" CssClass="buttonbg" Text="Save" 
                                ValidationGroup="Add" onclick="Btnsave_Click" />
                         <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" 
                                onclick="btnCancel_Click"/>--%>
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

