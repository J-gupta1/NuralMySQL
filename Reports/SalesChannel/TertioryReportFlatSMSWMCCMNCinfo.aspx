<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="TertioryReportFlatSMSWMCCMNCinfo.aspx.cs" Inherits="Reports_SalesChannel_TertioryReportFlatSMSWMCCMNCinfo" %>

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
                        <li class="text">Date Type:
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlDateType" runat="server" CssClass="formselect">
                                <asp:ListItem Selected="True" Text="~Select~" Value="0"></asp:ListItem>
                                <asp:ListItem Text="SMS Activation Date" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Web Activation Date" Value="2"></asp:ListItem>
                                <asp:ListItem Text="ISP Tertiary Date" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Tertiary Considered Date" Value="4"></asp:ListItem>
                                <asp:ListItem Text="Primary Date" Value="5"></asp:ListItem>
                                <asp:ListItem Text="Intermediary Date" Value="6"></asp:ListItem>
                            </asp:DropDownList>
                        </li>
                    </ul>

                    <%--#CC04 START ADDED--%>
                    <ul>
                        <li class="text">
                            <asp:Label ID="Label1" runat="server" Text="">Model Name: </asp:Label>
                        </li>
                       <li class="field">
                            <asp:DropDownList ID="ddlModelName" CssClass="formselect" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlModelName_SelectedIndexChanged">
                            </asp:DropDownList>

                        </li>
                        <li class="text">
                            <asp:Label ID="Label2" runat="server" Text="">SKU Name:</asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlSku" runat="server" CssClass="formselect" AutoPostBack="false">
                            </asp:DropDownList>
                        </li>                       
                    </ul>
                    <%--#CC04 END ADDED--%>
                    <ul>
                        <li class="text">IMEI:
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="form_textarea" TextMode="MultiLine"></asp:TextBox>
                            <div style="margin-top: 2px;" class="error">
                                (Comma separated)
                            </div>
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerch_Click"
                                    CssClass="buttonbg" CausesValidation="False" />
                            </div>
                            <div class="float-margin">
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
