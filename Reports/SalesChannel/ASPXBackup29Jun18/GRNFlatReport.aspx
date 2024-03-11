<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="GRNFlatReport.aspx.cs" Inherits="Reports_SalesChannel_GRNFlatReport" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
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
                                <tr>
                                    <td align="left" valign="top" height="5"></td>
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
                                                        GRN Report
                                                    </div>
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
                                                            <td class="formtext" valign="top" align="right" width="10%" height="35">
                                                                <asp:Label ID="labelinscode" runat="server" Text="">GRN Number:</asp:Label>
                                                            </td>
                                                            <td align="left" valign="top" width="20%">
                                                                <asp:TextBox ID="txtGRNNumber" runat="server" CssClass="form_input2" MaxLength="20"></asp:TextBox><br />
                                                            </td>
                                                            <td align="right" valign="top" width="10%" class="formtext">
                                                                <asp:Label ID="lblserfrmDate" runat="server" Text="">From Date: </asp:Label>
                                                            </td>
                                                            <td align="left" valign="top" width="15%">
                                                                <uc2:ucDatePicker ID="ucDateFrom" runat="server" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                                                            </td>
                                                            <td align="right" valign="top" width="8%" class="formtext">
                                                                <asp:Label ID="lblsertodate" runat="server" Text="">To Date:</asp:Label>
                                                            </td>
                                                            <td valign="top" align="left">
                                                                <uc2:ucDatePicker ID="ucDateTo" runat="server" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                                                            </td>
                                                            <td valign="top" align="left">
                                                                <asp:CheckBox ID="chksb" runat="server" Text="With Serial/Batch" TextAlign="Right" />
                                                            </td> 
                                                            </tr>
                                                            <tr>
                                                            <td colspan="5"></td>                                                
                                                            <%--#CC02 Add Start--%>
                                                            
                                                            <td class="formtext" valign="top" align="left"  height="35" >

                                                                <asp:Button ID="btnExportToExcel" runat="server" OnClick="exportToExel_Click" CssClass="excel"
                                                                    CausesValidation="False" />

                                                            </td>
                                                            <%--#CC02 Add End--%>
                                                            <td class="formtext" valign="top" align="left">
                                                                <%--#CC02 Commnet Start--%>
                                                                <%--<asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerch_Click"
                                                                    CssClass="buttonbg" CausesValidation="False" />--%>
                                                                <%--#CC02 Commnet End--%>

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
                        <td align="left" height="10"></td>
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
                                                                    &nbsp;List
                                                                </div>
                                                            </td>
                                                            <td width="10%" align="right">
                                                                <div style="float: right">
                                                                    <%--#CC02 Commnet Start--%>
                                                                    <%--<asp:Button ID="btnExportToExcel" runat="server" OnClick="exportToExel_Click" CssClass="excel"
                                                                        CausesValidation="False" />--%>
                                                                    <%--#CC02 Commnet End--%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" class="tableposition">
                                                    <div class="contentbox">
                                                        <div class="grid2">
                                                            <asp:GridView ID="grdGRNReport" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                                                BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="GRNNumber" FooterStyle-HorizontalAlign="Left"
                                                                FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                                                                HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="gridrow1"
                                                                RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                                                                RowStyle-VerticalAlign="top" Width="100%" OnPageIndexChanging="grdGRNReport_PageIndexChanging">
                                                                <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                                                                <Columns>
                                                                    <asp:BoundField DataField="SalesChannelName" HeaderText="SalesChannel Name" HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SalesChannelCode" HeaderText="SalesChannel Code" HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="HOName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName1 %>"
                                                                        HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="RBHName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName2 %>"
                                                                        HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ZSMName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName3 %>"
                                                                        HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SBHName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName4 %>"
                                                                        HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ASOName" HeaderText="<%$ Resources:SalesHierarchy, HierarchyName5 %>"
                                                                        HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="GRNNumber" HeaderText="GRN Number" HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="GRNDate">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbluseraffdate" runat="server" Text='<%# Eval("GRNDate","{0:dd-MMM-yy}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="PONumber" HeaderText="PO Number" HtmlEncode="false" Visible="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="PO Date" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbluseraffdate" runat="server" Text='<%# Eval("PODate","{0:dd-MMM-yy}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="InvoiceNumber" HeaderText="Invoice Number" HtmlEncode="false"
                                                                        Visible="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Transaction Date" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbluseraffdate" runat="server" Text='<%# Eval("InvoiceDate","{0:dd-MMM-yy}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="SKUName" HeaderText="SKU" HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="SerialNumber">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSerialNumber" runat="server" Text='<%# Eval("SerialNumber1") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="BatchCode">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblbatchNumber" runat="server" Text='<%# Eval("BatchCode") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%--  <asp:BoundField DataField="SerialNumber1" HeaderText="SerialNumber" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="BatchCode" HeaderText="BatchCode" HtmlEncode="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>--%>
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
                        <td align="left" height="10"></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
