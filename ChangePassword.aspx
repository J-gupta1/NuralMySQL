<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" EnableEventValidation="true" CodeFile="ChangePassword.aspx.cs"
    Inherits="ChangePassword" %>

<%@ Register Src="UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">

        function TextValidate(oSrc, args) {
            args.IsValid = (args.Value.length >= 5);
        }
        function Chkchar(e) {
            var charCode = (e.which) ? e.which : event.keyCode
            if (charCode == 39) {
                charCode = 0;
                return false;
            }
            else {
                return true;
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
        <tr>
            <td align="left" valign="top">
                <table cellspacing="0" cellpadding="0" width="965" border="0">
                    <tr>
                        <td valign="top" align="left">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td align="left" valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">

                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:Label ID="lblmsg" CssClass="error" Visible="false" runat="server"></asp:Label>
                                                    <uc1:ucMessage ID="ucMsg" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" class="tableposition">
                                        <div class="mainheading">Change Password</div>
                                        <div class="contentbox">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td width="170" align="left" valign="top" class="formtext">Old Password<font class="error">*</font>
                                                    </td>
                                                    <td width="865" height="30" align="left" valign="top">
                                                        <asp:TextBox ID="txtOldpass" runat="server" CssClass="form_input2" MaxLength="20"
                                                            TextMode="Password" ValidationGroup="valgrpChangePassword"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="Reqoldpass" runat="server" ControlToValidate="txtOldpass"
                                                            ForeColor="" CssClass="error" ErrorMessage="Please enter old password." SetFocusOnError="true"
                                                            Display="Dynamic" ValidationGroup="valgrpChangePassword"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" class="formtext">New Password<font class="error">*</font>
                                                    </td>
                                                    <td height="30" align="left" valign="top">
                                                        <asp:TextBox ID="txtNewpass" runat="server" CssClass="form_input2" MaxLength="20"
                                                            TextMode="Password" ValidationGroup="valgrpChangePassword"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ForeColor="" ID="Reqpass" runat="server" ControlToValidate="txtNewpass"
                                                            CssClass="error" Display="Dynamic" ErrorMessage="Please enter new password."
                                                            ValidationGroup="valgrpChangePassword"></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="CValidpassword" ForeColor="" SetFocusOnError="true" runat="server"
                                                            ClientValidationFunction="TextValidate" ControlToValidate="txtNewpass" CssClass="error"
                                                            Display="Dynamic" ValidationGroup="valgrpChangePassword" ErrorMessage="Password must have at least 5 characters"></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" class="formtext">Confirm Password<font class="error">*</font>
                                                    </td>
                                                    <td height="30" align="left" valign="top" class="formtext">
                                                        <asp:TextBox ID="txtRetype" runat="server" CssClass="form_input2" MaxLength="20"
                                                            TextMode="Password"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="ReqRetypepass" runat="server" ControlToValidate="txtRetype"
                                                            CssClass="error" Display="Dynamic" ErrorMessage="Please retype password." ForeColor=""
                                                            ValidationGroup="valgrpChangePassword"></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="Comparepass" runat="server" ForeColor="" ControlToCompare="txtNewpass"
                                                            ControlToValidate="txtRetype" CssClass="error" ErrorMessage="Please retype same password." Display="Dynamic" ValidationGroup="valgrpChangePassword"></asp:CompareValidator>
                                                    </td>
                                                </tr>
                                                <tr>

                                                    <td height="30" align="left" colspan="2">
                                                        <asp:Panel ID="pnl" runat="server" DefaultButton="btnsubmit">
                                                            <asp:Button ID="btnsubmit" runat="server" Text="Submit" ValidationGroup="valgrpChangePassword"
                                                                CssClass="buttonbg" OnClick="btnsubmit_Click" />
                                                            <asp:Button ID="btnReset" runat="server" CssClass="buttonbg" Text="Reset" OnClick="btnReset_Click" />
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" height="10"></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
