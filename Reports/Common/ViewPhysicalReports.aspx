<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="ViewPhysicalReports.aspx.cs" Inherits="Reports_Common_ViewPhysicalReports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel ID="UpdMain" runat="server">
            <ContentTemplate>
                <div>
                    <uc4:ucMessage ID="ucMessage1" runat="server" />
                    <div class="clear"></div>
                    <div class="mainheading">
                        View Physical Reports
                    </div>
                    <div class="contentbox">
                        <div class="mandatory">
                            (*) Marked fields are mandatory. (+) Fill at least one of them.    
                        </div>
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">Created By:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <asp:DropDownList CssClass="formselect" ID="ddlfosandtsm" runat="server">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </li>
                                <li class="text">Sales Channel Type:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <asp:DropDownList ID="ddlsaleschanneltype" CssClass="formselect" OnSelectedIndexChanged="ddlsaleschanneltype_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                </li>
                                <li class="text">Sales Channel Name:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <asp:DropDownList CssClass="formselect" ID="ddlsaleschannelname" runat="server">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </li>
                                <li class="text">From Date:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>
                                <li class="text">To Date:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>       </ul>                          
                            <div class="setbbb">
                                    <div class="float-margin">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg"
                                            CausesValidation="true" ValidationGroup="Search" OnClick="btnSearch_Click" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg"
                                            CausesValidation="false" OnClick="btnCancel_Click" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="btnExportexcel" runat="server" Text="Export To Excel" CssClass="buttonbg"
                                            CausesValidation="true" OnClick="btnExportexcel_Click" />
                                    </div>
                                </div>
                         
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Panel ID="PnlGrid" runat="server">
                        <div class="mainheading">
                            View Physical Reports
                        </div>
                        <div class="clear"></div>
                        <div class="contentbox">
                            <div class="grid1">
                                <asp:GridView ID="gvPhysicalReportsDetail" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="Altrow"
                                    bgcolor="" BorderWidth="0px" DataKeyNames="EntityTypeID,SKUID,CreatedBy,CreationOn" OnRowCommand="gvPhysicalReportsDetail_RowCommand" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                    AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>'>
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <PagerStyle CssClass="gridfooter" />
                                    <Columns>

                                        <asp:BoundField DataField="SalesChannelTypeName" HeaderStyle-HorizontalAlign="Left" HeaderText="SalesChannel Type"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SalesChannelCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SalesChannel Code"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SalesChannelName" HeaderStyle-HorizontalAlign="Left" HeaderText="SalesChannel Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="StockEnterByName" HeaderStyle-HorizontalAlign="Left" HeaderText="Stock Entered By Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="StockEnterByCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Stock Entered By Code"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SKUCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Code"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SKUName" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Name"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ActualQuantity" HeaderStyle-HorizontalAlign="Left" HeaderText="Quantity"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CreationOn" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left" HeaderText="Physical StockDate"
                                            HtmlEncode="true">
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="View Serial Number" ItemStyle-Width="85px">
                                            <ItemStyle Wrap="False" />
                                            <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnViewSerialnumber" CssClass="elink2" runat="server" CommandName="ViewSerialNumber"
                                                    CommandArgument='<%#Eval("EntityID") %>'>View Serial Number</asp:LinkButton>

                                            </ItemTemplate>
                                        </asp:TemplateField>



                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="dvFooter" runat="server" class="pagination">
                                <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div>
                    <table id="tblTransactions" runat="server" width="100%" cellpadding="0" cellspacing="0"
                        border="0">
                        <tr>
                            <td>
                                <div class="mainheading">
                                    Transactions
                                </div>
                                <div class="clear">
                                    </div>
                                <div class="contentbox">
                                    <div class="grid">
                                        <asp:GridView ID="gvTransactions" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                                            AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                                            FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                                            GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                            HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" OnPageIndexChanging="gvTransactions_PageIndexChanging" RowStyle-HorizontalAlign="left"
                                            RowStyle-VerticalAlign="top" Width="100%" PageSize="20">
                                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                            <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                            <Columns>
                                                <asp:BoundField DataField="SKUCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Code"
                                                    HtmlEncode="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SKUName" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Name"
                                                    HtmlEncode="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SerialNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="SerialNumber"
                                                    HtmlEncode="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SerilizedNonSerilized" HeaderStyle-HorizontalAlign="Left" HeaderText="Serilized/NonSerilized"
                                                    HtmlEncode="true">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>


                                            </Columns>
                                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                            <AlternatingRowStyle CssClass="Altrow" />
                                            <PagerStyle CssClass="PagerStyle" />
                                        </asp:GridView>
                                    </div>                                    
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>

            </ContentTemplate>
            <Triggers>

                <asp:PostBackTrigger ControlID="btnExportexcel" />
                <asp:PostBackTrigger ControlID="btnCancel" />
                <asp:PostBackTrigger ControlID="btnSearch" />

            </Triggers>
        </asp:UpdatePanel>
    </div>


</asp:Content>

