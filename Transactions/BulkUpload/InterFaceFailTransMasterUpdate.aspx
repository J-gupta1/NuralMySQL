<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="InterFaceFailTransMasterUpdate.aspx.cs" Inherits="InterFaceFailTransMasterUpdate" %>

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
    <div class="clear"></div>
    <div class="mainheading">
        Tally/Busy Patch Fail Records
    </div>
    <div class="contentbox">
        <%-- <asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H30-C3-S">
            <ul>
                <li class="text">
                    <asp:Label ID="lblserfrmDate" runat="server" Text="">From Date: <span class="error">*</span></asp:Label>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDatePicker1" runat="server" ErrorMessage="Invalid from date."
                        ValidationGroup="insert" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                </li>
                <li class="text">
                    <asp:Label ID="lblsertodate" runat="server" Text="">To Date:<span class="error">*</span></asp:Label>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDatePicker2" runat="server" ErrorMessage="Invalid to  date."
                        ValidationGroup="insert" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                </li>

                <li class="field3">
                    <asp:Button ID="btnDownLoad" Text="Download Errors" runat="server" OnClick="btnDownLoad_Click"
                        CssClass="buttonbg" CausesValidation="True" />

                </li>
                <li class="field3">
                    <asp:Button ID="btnDownloadAll" Text="Download All Sales" runat="server" OnClick="btnDownLoadAll_Click"
                        CssClass="buttonbg" CausesValidation="True" />

                </li>
            </ul>
            <div class="clear"></div>
            <ul>
                <li class="text">Upload File:</li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />

                    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                </li>
                <li class="field3">
                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                        OnClick="btnUpload_Click" />
                </li>
                <li class="link padding-top">
                    <%--<a class="elink2" href="../../Excel/Templates/FailTransApiUpload.xlsx">Download Template</a>--%>
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>

                </li>
                <li class="link padding-top">
                    <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>
                </li>
            </ul>

        </div>


    </div>


</asp:Content>
