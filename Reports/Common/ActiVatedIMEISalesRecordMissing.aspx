<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ActivatedIMEISalesRecordMissing.aspx.cs" Inherits="Reports_Common_ActivatedIMEISalesRecordMissing" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--  <style type="text/css">
        .style1 {
            height: 27px;
        }

        .style2 {
            width: 127px;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Activated IMEI Sales Record Missing Report
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">
                    <asp:Label ID="lblserfrmDate" runat="server" Text="">From Date: <span class="error">*</span></asp:Label>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="Invalid from date."
                        ValidationGroup="insert" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                </li>
                <li class="text">
                    <asp:Label ID="lblsertodate" runat="server" Text="">To Date:<span class="error">*</span></asp:Label>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="Invalid to  date."
                        ValidationGroup="insert" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                </li>
                <li class="text">
                    <asp:Label ID="lbseruser" runat="server" Text="">Sale Channel:</asp:Label>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlSaleChannal" runat="server" CssClass="formselect">
                        </asp:DropDownList>
                    </div>
                    <%-- <div style="float: left; width: 135px;">
                                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbUsername"
                                                                CssClass="error" ErrorMessage="Please select a user   " InitialValue="0"
                                                                ValidationGroup="insert" />
                                                        </div>--%>

                </li>
                <li class="text"></li>
                <li class="field">
                    <div class="float-margin">
                        <asp:Button ID="Button2" runat="server" Text="Download" CssClass="buttonbg" OnClick="btnDownload_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" OnClick="btnCancel_Click" />
                    </div>
                </li>

            </ul>
            <%--<tr>
                                                    <td class="formtext" valign="top" align="right" colspan="6">
                                                        <asp:Button ID="Button1" runat="server" Text="Download" CssClass="buttonbg" OnClick="btnDownload_Click" />

                                                    </td>
                                                    <td class="formtext" valign="top" align="right" colspan="2"></td>
                                                </tr>--%>
        </div>
    </div>
    <%--<tr>
                        <td align="left" height="10">
                            <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="buttonbg" OnClick="btnDownload_Click" />

                        </td>
                    </tr>--%>
</asp:Content>
