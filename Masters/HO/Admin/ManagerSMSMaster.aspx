<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManagerSMSMaster.aspx.cs" Inherits="Masters_HO_Admin_ManagerSMSMaster" %>

<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <uc1:ucMessage ID="ucMsg" runat="server" />
    </div>
    <div>
        <div class="mainheading">
            Manage Email
        </div>
        <div class="export">
            <a href="ViewSMSMaster.aspx" class="elink7">View SMS Master</a>
        </div>
        <div class="contentbox">
            <div class="mandatory">
                (*) Marked fields are mandatory
            </div>
            <div class="H35-C3-S">
                <ul>
                    <li class="text">SMS Keyword: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtSMSKeyword" runat="server" CausesValidation="True" CssClass="formfields"
                            MaxLength="20"></asp:TextBox>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="error" runat="server"
                                ControlToValidate="txtSMSKeyword" ErrorMessage="Please Enter SMS Keyword" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="text">SMS Description: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtSMSDescription" runat="server" TextMode="MultiLine" CssClass="form_textarea"
                            CausesValidation="True" MaxLength="200"></asp:TextBox>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="error" runat="server"
                                ControlToValidate="txtSMSDescription" ErrorMessage="Please Enter SMS Description"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="text">SMS From: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:TextBox ID="txtSMSFrom" runat="server" Style="margin-right: 0px" CssClass="formfields"
                                CausesValidation="True" MaxLength="20"></asp:TextBox>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSMSFrom"
                                CssClass="error" ErrorMessage="Please Enter SMS From" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                </ul>
                <ul>
                    <li class="text">SMS Content:  <span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtSMSContent" runat="server" TextMode="MultiLine" CssClass="form_textarea"
                            CausesValidation="True" MaxLength="500"></asp:TextBox>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtSMSContent"
                                CssClass="error" ErrorMessage="Please Enter SMS Content" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="text">SMS Expiry:  <span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:TextBox ID="txtSMSExpiry" runat="server" CausesValidation="True" CssClass="formfields"
                                MaxLength="3"></asp:TextBox>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtSMSExpiry" Display="Dynamic"
                                CssClass="error" ErrorMessage="Please Enter SMS Expiry  Hrs" Width="160px" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtSMSExpiry" Display="Dynamic"
                                CssClass="error" ErrorMessage="Enter Only Numeric Value" SetFocusOnError="True"
                                ValidationExpression="^[0-9]*$"></asp:RegularExpressionValidator>
                        </div>
                    </li>
                    <li class="text">Status:                                 
                                    <asp:CheckBox ID="chkStatus" runat="server" Checked="True" />
                    </li>
                    <li class="field">
                        <div class="float-margin">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonbg" OnClick="btnSubmit_Click" />
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" ValidationGroup="aa" CssClass="buttonbg"
                                OnClick="btnCancel_Click" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>

    </div>
</asp:Content>
