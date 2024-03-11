<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ViewSchemeDetails.aspx.cs" Inherits="Masters_HO_Admin_ViewSchemeDetails" %>

<%@ Register Src="../../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker"
    TagPrefix="uc1" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:HiddenField ID="hdnCase" runat="server" />
    <asp:HiddenField ID="hdnFileName" runat="server" />
    <asp:HiddenField ID="hdnSchemeID" runat="server" />
    <div id="ucmsg">
        <uc2:ucMessage ID="ucMsg" runat="server" />
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
                <li class="text">
                    Scheme Code:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtsrcSchemeCode" runat="server" CssClass="formfields"></asp:TextBox>
                </li>
                <li class="text">
                    Scheme Name:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtsrcSchememName" runat="server" CssClass="formfields"></asp:TextBox>
                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button CssClass="buttonbg" ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button CssClass="buttonbg" ID="btnCancelSearch" runat="server" Text="Cancel"
                            OnClick="btnCancelSearch_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>    
    <div>
        <div id="viewScheme">
            <div class="mainheading">
                View Scheme
            </div>
        </div>
    </div>
     <div class="clear">
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:GridView ID="gvViewOfflineScheme" runat="server" AutoGenerateColumns="False"
                CssClass="customers" Width="100%" CellSpacing="0" CellPadding="4" EditRowStyle-CssClass=""
                GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="alt" AlternatingRowStyle-CssClass="tt2"
                HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Middle" AllowPaging="True"
                OnPageIndexChanging="gvViewOfflineScheme_PageIndexChanging">
                <RowStyle CssClass="alt" />
                <Columns>
                    <asp:TemplateField HeaderText="Scheme Code">
                        <ItemTemplate>
                            <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="lblEmailKeyword" runat="server" Text='<%#Eval("SchemeCode") %>'></asp:Label></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Scheme Name">
                        <ItemTemplate>
                            <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="lblEmailKeyword" runat="server" Text='<%#Eval("SchemeName") %>'></asp:Label></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="From Date">
                        <ItemTemplate>
                            <div style="width: 130px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("SchemeStartDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="End Date">
                        <ItemTemplate>
                            <div style="width: 130px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="Label5" runat="server" Text='<%#Eval("SchemeEndDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="lblEmailKeyword" runat="server" Text='<%#Eval("RunningStatus") %>'></asp:Label></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="View Details">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" CssClass="elink2" runat="server" NavigateUrl='<%#  "~/Transactions/DSR/Downloading.aspx?FileName=" +Eval("SchemeDocumentFileName")+"&FilePath="+"~/Excel/Upload/OffLineScheme/"  %>'
                                Text="View"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="gridheader" />
                <EditRowStyle CssClass="" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
