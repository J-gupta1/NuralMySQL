<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageFullSMSRptGionee.aspx.cs" Inherits="Reports_SalesChannel_ManageFullSMSRptGionee" %>

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
                <div class="H25-C4-S">
                    <ul>
                        <%-- <td valign="top" align="right" width="14%" height="30">
                                                                SMS Date Type:<font class="error">*</font>
                                                            </td>
                                                            <td align="left" valign="top" width="20%">
                                                                <div style="float: left; width: 135px;">
                                                                    <asp:DropDownList ID="ddlSMSDateType" AutoPostBack="true" runat="server" 
                                                                        CssClass="form_select4" 
                                                                        onselectedindexchanged="ddlSMSDateType_SelectedIndexChanged">
                                                                        <asp:ListItem Text="SMS Tertiary" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="System Tertiary" Value="2"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <br />
                                                                </div>
                                                               
                                                            </td>--%>
                        <li class="text">
                            <asp:Label ID="lblserfrmDate" runat="server" Text="">From Date: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateFrom" runat="server" IsEnabled="True" defaultDateRange="True" />
                        </li>
                        <li class="text">
                            <asp:Label ID="lblsertodate" runat="server" Text="">To Date:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateTo" runat="server" IsEnabled="True" defaultDateRange="True" />
                        </li>
                        <%--======#CC02 Added Started--%>
                        <li class="text">
                            <asp:Label ID="LblActivation" runat="server" Text="">Activation Type<font class="error">*</font></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlActivationType" runat="server" class="formselect">
                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Web" Value="1"></asp:ListItem>
                                <asp:ListItem Text="SMS" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </li>
                        <%--======#CC02 Added End--%>
                        <li class="field3">
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
