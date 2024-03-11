<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="RSPDSRReport.aspx.cs" Inherits="Reports_Retailer_RSPDSRReport" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" charset="utf-8">


        function txtISPCodeChanged() {
            var ISPCode = document.getElementById('<%= txtISPCode.ClientID %>').value;
            var hdnISPCode = document.getElementById('<%= hdnISPCode.ClientID %>');
            hdnISPCode.value = ISPCode;
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
            <asp:HiddenField ID="hdnISPCode" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Search
    </div>
    <div class="contentbox">
        <%--<asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>--%>
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C4-S">
            <ul>
                <li class="text">From Date:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateFrom" IsRequired="true" runat="server" defaultDateRange="True"
                        RangeErrorMessage="Date should be less or equal then current date." ErrorMessage="Please insert Date from" />
                </li>
                <li class="text">To Date:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateTo" IsRequired="true" runat="server" defaultDateRange="True"
                        RangeErrorMessage="Date should be less or equal then current date." ErrorMessage="Please insert Date To" />
                </li>
                <li>
                    <%--ISP Code:--%>

                    <div>
                        <asp:TextBox ID="txtISPCode" onchange="txtISPCodeChanged();" runat="server" CssClass="formfields"
                            MaxLength="20" Visible="false"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtISPCode"
                        CssClass="error" Display="Dynamic" ValidationGroup="AddSalary" ErrorMessage="Please enter ISP Name."
                        SetFocusOnError="true"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regEDISPName" ControlToValidate="txtISPCode" Display="Dynamic"
                        CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,50}"
                        ValidationGroup="AddSalary" runat="server" />
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="wordWheel listMain .box"
                        CompletionListHighlightedItemCssClass="wordWheel itemsSelected" CompletionListItemCssClass="wordWheel itemsMain"
                        MinimumPrefixLength="3" ServiceMethod="GetISPCodesList" ServicePath="../../CommonService.asmx"
                        TargetControlID="txtISPCode" UseContextKey="true">
                    </cc1:AutoCompleteExtender>
                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button ID="btnExportToExcel" runat="server" CssClass="buttonbg" OnClick="exportToExel_Click"
                            CausesValidation="False" Text="Export To Excel" />
                    </div>
                    <div class="float-left">
                        <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                            CssClass="buttonbg" CausesValidation="False" />
                    </div>
                </li>
            </ul>
        </div>
        <%--</ContentTemplate>
                                            </asp:UpdatePanel>--%>
    </div>
</asp:Content>
