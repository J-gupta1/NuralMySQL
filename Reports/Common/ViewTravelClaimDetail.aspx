<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewTravelClaimDetail.aspx.cs" EnableEventValidation="false" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="ViewTravelClaimDetail" %>


<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="~/UserControls/ucPagingControl.ascx" TagName="ucPagingControl"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/ucMoney.ascx" TagName="ucMoney" TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updpnlSaveData" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="box1">
                <div class="subheading">
                    <div class="float-left">
                        View travel claim detail1
                    </div>

                    <div class="clear"></div>
                    <uc1:ucMessage ID="ucMessage1" runat="server" />

                </div>
                <div class="innerarea">
                    <div class="H30-C3-S">
                        <ul>

                            <li class="text">Date From :<span class="optional-img">&nbsp;</span><font class="error"></font></li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucFromDate" ErrorMessage="Please enter from date" runat="server"
                                    IsRequired="true" ValidationGroup="date" />
                            </li>
                            <li class="text">Date To:<span class="optional-img">&nbsp;</span><font class="error"></font></li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucTODate" ErrorMessage="Please enter to date" runat="server"
                                    IsRequired="true" ValidationGroup="date" />
                            </li>
                            <li class="text">Approval Status:<span class="optional-img">&nbsp;</span> </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlappstatus" runat="server" CssClass="formselect">
                                    <asp:ListItem Value="255">Select</asp:ListItem>
                                    <asp:ListItem Value="0">Pending</asp:ListItem>
                                    <asp:ListItem Value="1">Approved</asp:ListItem>
                                    <asp:ListItem Value="2">Rejected</asp:ListItem>
                                    <asp:ListItem Value="3">Partial Approved</asp:ListItem>
                                </asp:DropDownList>
                            </li>
                            <li class="text">Approval Level:<span class="mandatory-img">&nbsp;</span> </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlapprovalLevel" runat="server" CssClass="formselect">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                    <asp:ListItem Value="1">Level 1</asp:ListItem>
                                   <%-- <asp:ListItem Value="2">Level 2</asp:ListItem>--%>
                                </asp:DropDownList>
                                <div>
                                    <asp:RequiredFieldValidator ID="reqddlApprovalSearchExcel" runat="server" ControlToValidate="ddlapprovalLevel"
                                        CssClass="mandatory" Display="Dynamic" InitialValue="0" ErrorMessage="Please select approval level!" SetFocusOnError="true"
                                        ValidationGroup="Search"></asp:RequiredFieldValidator>
                                </div>
                            </li>
                        </ul>
                        <ul>
                            <li class="text">Entity Type:
                            </li>
                            <li class="field">
                                <asp:DropDownList CssClass="formselect" ID="ddlEntityType" AutoPostBack="true" OnSelectedIndexChanged="ddlEntityType_SelectedIndexChanged" runat="server">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </li>
                            <li class="text">Entity Type Name:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlEntityTypeName" CssClass="formselect" runat="server">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </li>
                            <li class="text"></li>
                            <li class="field">
                                <div class=" float-margin">
                                    <asp:Button ID="btnExportinexcel" runat="server" CssClass="buttonbg"
                                        Text="Export in Excel"  OnClick="btnExportinexcel_Click" />                                   
                                </div>
                                <div class=" float-margin">
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportinexcel" />
           
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
