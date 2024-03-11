<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ViewSchemePayout.aspx.cs" Inherits="Masters_HO_Admin_ViewSchemePayout" %>

<%@ Register Src="../../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker"
    TagPrefix="uc1" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:HiddenField ID="hdnCase" runat="server" />
    <div id="ucmsg">
        <uc2:ucMessage ID="ucMsg" runat="server" />
    </div>
    <div class="clear">
    </div>
    <div>
        <div id="SearchScheme">
            <div class="mainheading">
                Search Scheme
            </div>
        </div>
    </div>
    <div class="contentbox">
        <div class="H25-C3-S">
            <ul>
                <li class="text">Scheme Name:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtsrcSchemeName" runat="server" CssClass="formfields"></asp:TextBox>
                </li>
                <li class="text">
                    <asp:Label ID="lblRetailerName" runat="server" Text="Retailer Name:"></asp:Label>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtsrcRetailerName" runat="server" CssClass="formfields"></asp:TextBox>
                </li>
                <%--<td align="right" valign="top" width="13%">
                    &nbsp;
                </td>
                <td align="left" valign="top">
                    &nbsp;
                </td>
               </tr>
                <tr>
                <td align="left" valign="top"></td>--%>
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button CssClass="buttonbg" ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button CssClass="buttonbg" runat="server" Text="Clear" ID="btnCancel" OnClick="btnCancel_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div class="clear">
    </div>
    <div>
        <div id="viewScheme">
            <div class="mainheading">
                View Scheme
            </div>
        </div>
    </div>
    <div class="contentbox">
        <div class="grid1" style="padding-bottom: 3px; overflow-y: hidden; overflow-x: scroll;">
            <asp:GridView ID="gvViewOfflineSchemePayout" runat="server" AutoGenerateColumns="False"
                CssClass="customers" Width="100%" CellSpacing="0" CellPadding="4" EditRowStyle-CssClass=""
                GridLines="None" HeaderStyle-CssClass="" RowStyle-CssClass="alt" AlternatingRowStyle-CssClass="tt2"
                HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Middle" AllowPaging="True"
                OnPageIndexChanging="gvViewOfflineSchemePayout_PageIndexChanging">
                <RowStyle CssClass="alt" />
                <Columns>
                    <asp:TemplateField HeaderText="Scheme Name" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="lblSchemeName" runat="server" Text='<%#Eval("SchemeName") %>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RetailerName" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="lblRetailerName" runat="server" Text='<%#Eval("RetailerName") %>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Scheme Performance" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div style="width: 130px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="lblSchemePerformance" runat="server" Text='<%#Eval("SchemePerformance") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Scheme Payout" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="lblSchemePayout" runat="server" Text='<%#Eval("SchemePayout") %>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Processing Status" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="lblProcessingStatus" runat="server" Text='<%#Eval("ProcessingStatus") %>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status Date" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="lblStatusDate" runat="server" Text='<%#Eval("StatusDate") %>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Scheme Status" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="lblSchemeStatus" runat="server" Text='<%#Eval("SchemeStatus") %>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remarks" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div style="width: 130px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="" />
                <EditRowStyle CssClass="" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
