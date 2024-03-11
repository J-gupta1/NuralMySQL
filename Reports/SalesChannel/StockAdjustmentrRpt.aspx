<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StockAdjustmentrRpt.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Reports_SalesChannel_StockAdjustmentrRpt" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Search Stock Adjustment
    </div>
    <div class="contentbox">
       <%-- <asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblrole" runat="server" Text="">Sales Channel Type:</asp:Label><span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbSalesChannelType" CssClass="formselect"
                                    runat="server">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valtype" ControlToValidate="cmbSalesChannelType"
                                    CssClass="error" ErrorMessage="Please select a sales Channel Type " InitialValue="0" ValidationGroup="Search" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblsaleschannelname" runat="server" Text="">Sales Channel Name:</asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtsaleschannel" CssClass="formfields" MaxLength="100"
                                runat="server"></asp:TextBox>
                        </li>                    
                        <li class="text">
                            <asp:Label ID="lblserfrmDate" runat="server" Text="">From Date: </asp:Label><span class="error">*</span>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="Invalid from date."
                                defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." ValidationGroup="Search" />
                        </li>
                        <li class="text">
                            <asp:Label ID="lblsertodate" runat="server" Text="">To Date:</asp:Label><span class="error">*</span></td>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="Invalid to  date."
                                defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." ValidationGroup="Search" />
                        </li>                    
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerch_Click"
                                    ValidationGroup="Search" CssClass="buttonbg" CausesValidation="True" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                    CssClass="buttonbg" CausesValidation="False" />
                            </div>
                        </li>
                    </ul>
                </div>
           <%-- </ContentTemplate>--%>
            <%-- <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                </Triggers>--%>
       <%-- </asp:UpdatePanel>--%>
    </div>
   <%-- <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
        <ContentTemplate>--%>
            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                <div class="mainheading">
                    List
                </div>
                <div class="export">
                    <asp:Button ID="btnExportToExcel" Text="" runat="server" OnClick="exportToExel_Click"
                        CssClass="excel" />
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="grdStockAdjustment" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                            BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-HorizontalAlign="Left"
                            FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="Altrow"
                            RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                            RowStyle-VerticalAlign="top" Width="100%" EnableViewState="false"
                            OnPageIndexChanging="grdStockAdjustment_PageIndexChanging" PageSize='<%$ AppSettings:GridViewPageSize %>'>
                            <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                            <Columns>

                                <asp:BoundField DataField="HL1Name" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName1 %>" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="HL2Name" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName2 %>" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
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
                                        <asp:Label ID="lbluseraffdate" runat="server" Text='<%# Eval("StockAdjustmentDate","{0:dd-MMM-yy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <%--#CC03 START ADDED--%>
                                <asp:BoundField DataField="StockType" HeaderText="Stock Type" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <%--#CC03 END ADDED--%>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
       <%-- </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>
