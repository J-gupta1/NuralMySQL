<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StockAdjustmentrRpt.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Reports_SalesChannel_StockAdjustmentrRpt" %>

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
                                                        Search Stock Adjustment </div>
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
                                                            <td class="formtext" valign="top" align="right" width="10%" >
                                                                <asp:Label ID="lblrole" runat="server" Text="">Sales Channel Type:</asp:Label><font class="error">*</font>
                                                            </td>
                                                            <td width="25%" align="left" valign="top">
                                                               <div style="float:left; width:160px;"> <asp:DropDownList ID="cmbSalesChannelType" CssClass="form_select4" 
                                                                    runat="server"  
                                                                    >
                                                                </asp:DropDownList><br /></div>  <div style="float:left; width:180px;">
                                                                 <asp:RequiredFieldValidator runat="server" ID="valtype" ControlToValidate="cmbSalesChannelType"
                                                                                CssClass="error" ErrorMessage="Please select a sales Channel Type " InitialValue="0" ValidationGroup="Search" /></div>
                                                                
                                                            </td>
<td class="formtext" valign="top" align="right" width="10%" >
                                                                <asp:Label ID="lblsaleschannelname" runat="server" Text="">Sales Channel Name:</asp:Label>
                                                            </td>
                                                            <td width="25%" align="left" valign="top">
                                                                <asp:TextBox ID="txtsaleschannel" CssClass="form_select4" MaxLength="100"
                                                                    runat="server" 
                                                                    ></asp:TextBox>
                                                                
                                                            </td>
                                                        </tr>
                                                     
                                                        <tr>
                                                            <td align="right" valign="top" class="formtext">
                                                                <asp:Label ID="lblserfrmDate" runat="server" Text="">From Date: </asp:Label><font class="error">*</font>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="Invalid from date."
                                                                    defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date."   ValidationGroup = "Search"/>
                                                            </td>
                                                            <td align="right" valign="top" class="formtext">
                                                                <asp:Label ID="lblsertodate" runat="server" Text="">To Date:</asp:Label><font class="error">*</font></td><td valign="top" align="left">
                                                                <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="Invalid to  date."
                                                                     defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." ValidationGroup = "Search"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="formtext" valign="top" align="right" colspan="3">
                                                            </td>
                                                            <td class="formtext" valign="top" align="left" >
                                                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerch_Click"
                                                                    ValidationGroup="Search" CssClass="buttonbg" CausesValidation="True" />
                                                                <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                                                    CssClass="buttonbg" CausesValidation="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <%-- <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                </Triggers>--%></asp:UpdatePanel></div></td></tr></table></td></tr><tr>
                        <td align="left" height="10">
                        </td>
                    </tr>
                    <tr>
                    <td>
                    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="float:left;">
                            <tr >
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="90%" align="left" class="tableposition">
                                                    <div class="mainheading">
                                                        &nbsp;List</div></td><td width="10%" align="right">
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
                                                <asp:GridView ID="grdStockAdjustment" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                                    BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-HorizontalAlign="Left"
                                                    FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                                                    HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="gridrow1"
                                                    RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                                                    RowStyle-VerticalAlign="top" Width="100%"  EnableViewState="false"
                                                    onpageindexchanging="grdStockAdjustment_PageIndexChanging" PageSize='<%$ AppSettings:GridViewPageSize %>'   >
                                                    <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                                                    <Columns>
                                                   
                                                    <asp:BoundField DataField="HL1Name" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName1 %>" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="HL2Name" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName2 %>" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" /> </asp:BoundField>
                                                        <asp:BoundField DataField="HL3Name" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName3 %>" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="HL4Name" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName4 %>" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                       
                                                        <asp:BoundField DataField="HL5Name" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName5 %>" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                          <asp:BoundField DataField="StockAdjustmentNo" HeaderText="Stock Adjustment No" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SalesChannel" HeaderText="SalesChannel" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        
                                                         
                                                        <asp:BoundField DataField="ProductCategoryName" HeaderText="Product Category" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="BrandName" HeaderText="Brand" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ProductName" HeaderText="Product" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ModelName" HeaderText="Model" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                          <asp:BoundField DataField="ColorName" HeaderText="Color" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                          
                                                     
                                                         <asp:BoundField DataField="SKUCode" HeaderText="SKU Code" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                          <asp:BoundField DataField="SKUName" HeaderText="SKU Name" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Stock Adjustment Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbluseraffdate" runat="server" Text='<%# Eval("StockAdjustmentDate","{0:dd-MMM-yy}") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                                                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                   <%--#CC03 START ADDED--%>
                                                           <asp:BoundField DataField="StockType" HeaderText="Stock Type" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>   <%--#CC03 END ADDED--%>
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