<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" 
AutoEventWireup="true" CodeFile="STockTransferFlatReport.aspx.cs" Inherits="Reports_SalesChannel_STockTransferFlatReport" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" Runat="Server">

 <table cellspacing="0" cellpadding="0" width="965" border="0">
        <tr>
            <td valign="top" align="left">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td align="left" valign="top">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                              
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
                                            <ContentTemplate>
                                                <uc1:ucMessage ID="ucMessage1" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td align="left" valign="top">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td align="left" valign="top" class="tableposition">
                                                    <div class="mainheading">
                                                        Search Stock Transfer</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" class="tableposition">
                                        <div class="contentbox">
                                            <asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                        <tr>
                                                            <td colspan="4" class="style1" valign="top">
                                                                
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="formtext" valign="top" align="right" width="15%" height="35">
                                                                <asp:Label ID="lblrole" runat="server" Text="">Sales Channel Type:</asp:Label>
                                                            </td>
                                                            <td width="25%" align="left" valign="top">
                                                                <div style="float:left; width:135px;"> <asp:DropDownList ID="cmbSalesChannelType" CssClass="form_select4" 
                                                                    runat="server" AutoPostBack="True" onselectedindexchanged="cmbSalesChannelType_SelectedIndexChanged" 
                                                                    >
                                                                </asp:DropDownList><br /></div>  <div style="float:left; width:200px;">
                                                                 <asp:RequiredFieldValidator runat="server" ID="valtype" ControlToValidate="cmbSalesChannelType"
                                                                                CssClass="error" ErrorMessage="Please select a sales Channel Type " InitialValue="0" ValidationGroup="insert" /></div>
                                                                
                                                            </td>
                                                            <td class="formtext" valign="top" align="right" width="15%">
                                                           <asp:Label ID="labelinscode" runat="server" Text="">STN Number:</asp:Label>
       
                                                                            </td>
                                                                            <td align="left" valign="top" width="45%">
                                                        
                                                                                 <asp:TextBox ID="txtSTNNumber" runat="server" CssClass="form_input2"  MaxLength = "20"></asp:TextBox><br />
                                                                                 
                                                         </td>                        
                                                        </tr>
                                                        <tr>
                                                      
                                                        <tr>
                                                            <td align="right" valign="top" class="formtext">
                                                                <asp:Label ID="lblserfrmDate" runat="server" Text="">From Date: </asp:Label>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="Please select a from date."
                                                                    defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date."   ValidationGroup = "insert"/>
                                                            </td>
                                                            <td align="right" valign="top" height="35" class="formtext">
                                                                <asp:Label ID="lblsertodate" runat="server" Text="">To Date:</asp:Label>
                                                            </td>
                                                            <td valign="top" align="left">
                                                                <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="Please select a to date."
                                                                     defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." ValidationGroup = "insert"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="formtext" valign="top" align="right" colspan="3">
                                                            </td>
                                                            <td class="formtext" valign="top" align="left" >
                                                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerch_Click"
                                                                    ValidationGroup="insert" CssClass="buttonbg" CausesValidation="True" />
                                                                <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                                                    CssClass="buttonbg" CausesValidation="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <%-- <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                </Triggers>--%>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" height="10">
                        </td>
                    </tr>
                    <tr>
                    <td>
                    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
 
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="90%" align="left" class="tableposition">
                                                    <div class="mainheading">
                                                        &nbsp;List</div>
                                                </td>
                                                <td width="10%" align="right">
                                                    <asp:Button ID="btnExportToExcel" Text="" runat="server" OnClick="exportToExel_Click"
                                                        CssClass="excel" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                               </tr>
                                <tr>
                                    <td align="left" valign="top" class="tableposition">
                                        <div class="contentbox">
                                            <div class="grid2">
                                                <asp:GridView ID="grdStockTransfer" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                                    BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="STNNumber" FooterStyle-HorizontalAlign="Left"
                                                    FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                                                    HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="gridrow1"
                                                    RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                                                    RowStyle-VerticalAlign="top" Width="100%" OnPageIndexChanging="grdStockTransfer_PageIndexChanging">
                                                    <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                                                    <Columns>
                                                       <asp:BoundField DataField="HOName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName1 %>" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                    <asp:BoundField DataField="RBHName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName2 %>" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                    <asp:BoundField DataField="ZSMName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName3 %>" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SBHName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName4 %>" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="ASOName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName5 %>" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="FromSalesChannelName" HeaderText="From" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ToSalesChannelName" HeaderText="To" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="STNNumber" HeaderText="STN Number" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DocketNumber" HeaderText="Docket Number" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                          <asp:TemplateField HeaderText="Transaction Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbluseraffdate" runat="server" Text='<%# Eval("TransactionDate","{0:dd-MMM-yy}") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                       
                                                        <asp:BoundField DataField="SKUName" HeaderText="SKU" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                                                    <PagerStyle CssClass="PagerStyle" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                          
</table>
  </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExportToExcel" />          
                                            </Triggers>
                    </asp:UpdatePanel>
                    </td>
                    </tr>
                    <tr>
                        <td align="left" height="10">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>

