<%@ Page Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="IMEIAccXchangeUpdateReport.aspx.cs" Inherits="Reports_SalesChannel_IMEIAccXchangeUpdateReport" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>

<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" ShowCloseButton="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="mainheading">
        IMEI Update through AccXchange
    </div>
    <div class="contentbox">
        <asp:UpdatePanel runat="server" ID="upd" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="display: none;">
                    <div class="mandatory">
                        (*) Marked fields are mandatory            
                    </div>
                </div>
               <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblfrmDate" runat="server" Text="From Date: "></asp:Label><font class="error">*</font>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucFromDate" runat="server" ErrorMessage="Invalid from date."
                                defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                        </li>
                        <li class="text">
                            <asp:Label ID="lblsertodate" runat="server" Text="To Date: "></asp:Label><font class="error">*</font>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucToDate" runat="server" ErrorMessage="Invalid to date."
                                defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                        </li>
                        <li class="text">
                            <asp:Label ID="Label1" runat="server" Text="National Distributor"></asp:Label>:
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlND" OnSelectedIndexChanged="ddlND_SelectedIndexChanged"
                                runat="server" CssClass="formselect" AutoPostBack="true">
                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblRDS" runat="server" Text="RDS"></asp:Label>:
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlRDS" runat="server" CssClass="formselect"></asp:DropDownList>
                        </li>                   
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-left">
                                <asp:Image ID="imgDownloadMappingInfo" runat="server" AlternateText="Download Retailer Mapping Info" />
                            </div>
                            <div class="float-margin" style="padding-top: 3px; padding-left: 3px">
                                <asp:LinkButton ID="lnkExportToExcel" runat="server" Text="Export to Excel"
                                    CssClass="elink2" Style="color: green" OnClick="lnkExportToExcel_Click"></asp:LinkButton>

                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
