<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManagerEmailMaster.aspx.cs" Inherits="Masters_HO_Admin_ManagerEmailMaster" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
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
            <a href="ViewEmailMaster.aspx" class="elink2">View Email Master</a>
        </div>
        <div class="contentbox">
            <div class="mandatory">
                (*) Marked fields are mandatory
            </div>
            <div class="H35-C3-S">
                <ul>
                    <li class="text">Email Keyword: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtEmailKeyword" runat="server" CausesValidation="True" CssClass="formfields"
                            MaxLength="20"></asp:TextBox>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="error" runat="server"
                                ControlToValidate="txtEmailKeyword" ErrorMessage="Please Enter Email Keyword"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="text">Email Description: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtEmailDescription" runat="server" TextMode="MultiLine" CssClass="form_textarea"></asp:TextBox>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="error" runat="server"
                                ControlToValidate="txtEmailDescription" ErrorMessage="Please Enter Email Description"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="text">Email From: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:TextBox ID="txtEmailFrom" runat="server" Style="margin-right: 0px" CssClass="formfields"
                                CausesValidation="True"></asp:TextBox>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEmailFrom"
                                CssClass="error" ErrorMessage="Please Enter Email From" SetFocusOnError="True"></asp:RequiredFieldValidator>

                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="error"
                                ControlToValidate="txtEmailFrom" ErrorMessage="Enter valid Email ID" SetFocusOnError="True"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </div>
                    </li>
                </ul>
                <ul>
                    <li class="text">Subject Line:  <span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtSubjectLine" runat="server" CausesValidation="True" CssClass="formfields"></asp:TextBox>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtSubjectLine"
                                CssClass="error" ErrorMessage="Please Enter Subject Line" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="text">Status:
                    </li>
                    <li class="field">
                        <asp:CheckBox ID="chkstatus" runat="server" Checked="True" />
                    </li>
                    <li class="text">Email Expiry Hours: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtHrs" CssClass="formfields" Width="70px" runat="server" CausesValidation="True" MaxLength="3"></asp:TextBox>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="error" Display="Dynamic"
                                ErrorMessage="Please Enter Hours" ControlToValidate="txtHrs"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                            
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" CssClass="error" Display="Dynamic"
                                runat="server" ControlToValidate="txtHrs"
                                ErrorMessage="Please Enter Numaric Value" SetFocusOnError="True"
                                ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                        </div>
                    </li>
                </ul>
                <ul>
                    <li class="text">Body:
                    </li>
                    <li class="text-field3" style="height:auto">
                        <FCKeditorV2:FCKeditor ID="FCKBody" runat="server" BasePath="~/Assets/FCKEditor/">
                        </FCKeditorV2:FCKeditor>
                    </li>
                </ul>
                <div class="clear"></div>
                <ul>
                    <li class="text"></li>
                    <li class="field">
                        <div class="float-margin">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonbg" OnClick="btnSubmit_Click" />
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" ValidationGroup="a" CssClass="buttonbg"
                                OnClick="btnCancel_Click" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
