<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="RevertTertiaryActivation.aspx.cs" Inherits="Transactions_Common_RevertTertiaryActivation" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucTextboxMultiline.ascx" TagName="ucTextboxMultiline" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:HiddenField ID="hdncmd" runat="server" />
    <div>
        <uc1:ucMessage ID="ucMsg" runat="server" />
    </div>
    <div class="mainheading">
        Revert Tertiary Activation                   
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Enter Serial Numbers <span class="error">*</span>
                </li>
                <li class="field" style="height: auto">
                    <div>
                        <asp:TextBox ID="txtSerialNumber" runat="server" CausesValidation="True" CssClass="form_textarea"
                            ValidationGroup="aa" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" runat="server" CssClass="error"
                            ErrorMessage="Enter Serial Number" ControlToValidate="txtSerialNumber" ValidationGroup="aa"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </div>
                    <div>
                        <asp:Label ID="lblIMEItrackingMsg" CssClass="error" runat="server"></asp:Label>
                    </div>
                </li>
                <li class="text">Enter Remark <span class="error">*</span>
                </li>
                <li class="field" style="height: auto">
                    <uc4:ucTextboxMultiline ID="txtRemarks" runat="server" IsRequired="true" CharsLength="500" TextBoxWatermarkText="Please enter Remark." ErrorMessage="Please enter Remark." ValidationGroup="aa" />
                </li>
                <li class="field3">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonbg" ValidationGroup="aa"
                        OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" OnClick="btnReset_Click" CausesValidation="false" />
                </li>
            </ul>
        </div>
    </div>
    <%--<uc1:ucMessage ID="ucMsg" runat="server" />--%>
</asp:Content>
