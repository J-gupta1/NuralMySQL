<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    EnableEventValidation="false" CodeFile="TertioryReportFlatSMS.aspx.cs" Inherits="Reports_SalesChannel_TertioryReportFlatSMS" %>

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
        Tertiory Report
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">SMS Date Type:<span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlSMSDateType" AutoPostBack="false" runat="server" CssClass="formselect"
                                    OnSelectedIndexChanged="ddlSMSDateType_SelectedIndexChanged1">
                                    <asp:ListItem Text="Salect" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="SMS Tertiary" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="System Tertiary" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </li>
                        <li class="text" runat="server" id="tdNameid">Model Name:
                        </li>
                        <li class="field" runat="server" id="tdName">
                            <asp:DropDownList ID="ddlModalName" AutoPostBack="false" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                    </ul>
                    <div class="clear"></div>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblserfrmDate" runat="server" Text="">From Date: </asp:Label>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateFrom" runat="server" IsEnabled="True" defaultDateRange="True" />
                        </li>
                        <li class="text">
                            <asp:Label ID="lblsertodate" runat="server" Text="">To Date:</asp:Label>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateTo" runat="server" IsEnabled="True" defaultDateRange="True" />
                        </li>
                    </ul>
                    <ul>
                        <li class="text">IMEI:
                        </li>
                        <li class="field" style="height:auto;">
                            <div>
                                <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="form_textarea" TextMode="MultiLine"></asp:TextBox>
                            </div>
                            <div style="margin-top: 2px;" class="error">
                                (Comma separated)
                            </div>
                        </li>
                        <li class="text">
                        <li>
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" Text="Export To Excel" runat="server" OnClick="btnSerch_Click"
                                    CssClass="buttonbg" CausesValidation="False" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnCount" Text="Get Count" runat="server" OnClick="btnCount_Click"
                                    CssClass="buttonbg" CausesValidation="False" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                    CssClass="buttonbg" CausesValidation="False" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSearch" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
