<%@ Page Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="OrganHierarchyMappingReport.aspx.cs" Inherits="Reports_SalesChannel_OrganHierarchyMappingReport" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>


<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMessage1" runat="server" ShowCloseButton="false" />
    <div style="display: block;">
        <div class="mainheading">
            Organisation Hierarchy Mapping Report
        </div>
    </div>
    <div style="display: block;">
        <asp:UpdatePanel ID="updExport" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="contentbox">
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblHierarchyLevel" runat="server" Text="">Hierarchy Level:</asp:Label>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlHierarchyLevel" CssClass="formselect" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlHierarchyLevel_SelectedIndexChanged">
                                </asp:DropDownList>
                            </li>
                            <li class="text">
                                <asp:Label ID="lblLocation" runat="server" Text="">Location:</asp:Label>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddllocation" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="field3">
                                <asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" />
                            </li>
                        </ul>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="ExportToExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
