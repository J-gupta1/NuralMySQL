<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPageTertiary.master"
    AutoEventWireup="true" CodeFile="TertiaryTrendReport.aspx.cs" Inherits="Reports_Common_TertiaryTrendReport" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMessage" runat="server" ShowCloseButton="false" />
    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Always">
        <ContentTemplate>
            <div class="mainheading">
                Tertiary Count
            </div>
            <div class="contentbox">
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C4-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblfrmDate" runat="server" Text="">From Date: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucFromDate" runat="server" ErrorMessage="Invalid from date."
                                ValidationGroup="insert" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                        </li>
                        <li class="text">
                            <asp:Label ID="lblToDate" runat="server" Text="">To Date: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucToDate" runat="server" ErrorMessage="Invalid to date."
                                ValidationGroup="insert" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                        </li>
                        <li class="text">Day Range
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlDayRange" runat="server" CssClass="formselect" AutoPostBack="true" OnSelectedIndexChanged="ddlDayRange_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Text="-- Select Range--" Value="0">                                                                  
                                </asp:ListItem>
                                <asp:ListItem Text="Last 3 days" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Last 7 days" Value="7"></asp:ListItem>
                                <asp:ListItem Text="Last 30 days" Value="30"></asp:ListItem>
                            </asp:DropDownList>
                        </li>
                          <%--====#CC03 Added Started--%>
                        <li class="text">
                            <asp:Label ID="lblBrand" runat="server" Text="">Brand</asp:Label>
                        </li>
                      
                        <li class="field">
                            <asp:DropDownList ID="ddlBrand" runat="server" CssClass="formselect"></asp:DropDownList>
                        </li>
                        <%--====#CC03 Added End--%>
                        <li class="field3">
                            <div>
                                <div class="float-margin">
                                    <asp:Button ID="btnExportToExcel" Text="" runat="server" CssClass="excel" OnClick="btnExportToExcel_Click" />
                                </div>
                                <div class="float-left">
                                    <asp:Button ID="btnReset" Text="Reset" runat="server" CssClass="buttonbg" OnClick="btnReset_Click" />
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </ContentTemplate>
        <%--  <Triggers>
                            <asp:PostBackTrigger ControlID="btnExportToExcel" />
                        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
