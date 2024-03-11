<%@ Page Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="LogFeedback.aspx.cs" Inherits="Masters_Feedback_LogFeedback" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucTextboxMultiline.ascx" TagName="ucTextboxMultiline"
    TagPrefix="uc2" %>

<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMessage1" runat="server" ShowCloseButton="false" />
    <div style="display: block;">
        <div class="mainheading">
            Log Feedback
        </div>
    </div>
    <div style="display: block;">
        <div class="contentbox">
            <div class="mandatory">
                (*) Marked fields are mandatory            
            </div>
            <div class="H25-C3-S">
                <ul>
                    <li class="text">
                        <asp:Label ID="Label1" runat="server" Text="Feedback"></asp:Label>:
                                                                <span class="error">*</span>
                    </li>
                    <li class="field" style="height: auto">
                        <uc2:ucTextboxMultiline ID="txtfeedback" runat="server" IsRequired="true" CharsLength="500"
                            TextBoxWatermarkText="Please enter feeback." ErrorMessage="Please enter feeback."
                            ValidationGroup="Add" />
                        <%--    <asp:TextBox ID="txtfeedback2" runat="server" CssClass="form_textarea" TextMode="MultiLine"
                                                                        ValidationGroup="Add" MaxLength="500"  style="width:350px"></asp:TextBox>
                                                                    <br />
                                                                    <asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="txtfeedback2"
                                                                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter feeback."
                                                                        SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>--%>
                    </li>
                    <li class="field3">
                        <asp:Button ID="btnSaveFeedback" runat="server" CssClass="buttonbg" CausesValidation="true" ValidationGroup="Add" Text="Save" OnClick="btnSaveFeedback_Click" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
