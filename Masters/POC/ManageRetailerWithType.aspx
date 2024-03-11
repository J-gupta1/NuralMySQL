<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageRetailerWithType.aspx.cs" Inherits="Masters_HO_POC_ManageRetailerWithType" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="mainheading">
                Manage Retailer
                                                                           <%-- <asp:Label ID="lblHeading" runat="server" Text="Mange Retailer"></asp:Label>--%>
            </div>
            <div class="export">
                <asp:LinkButton ID="LBViewRetailer" runat="server" CausesValidation="False" OnClick="LBViewRetailer_Click"
                    CssClass="elink7">View List</asp:LinkButton>
            </div>

            <div class="contentbox">
                <div class="mandatory">
                    (*) Marked fields are mandatory
                </div>
                <div class="H35-C3-S">
                    <%-- <tr>
                                                                        <td height="35" align="right" valign="top" class="formtext" >
                                                                            Select Mode:<font class="error">*</font></td>
                                                                            <td colspan="5" class="formtext" valign="top">
                                                                                <asp:RadioButtonList ID="rdoSelectMode" runat="server" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0"
                                                                                    AutoPostBack="true" OnSelectedIndexChanged="rdoSelectMode_SelectedIndexChanged">
                                                                                    <asp:ListItem  Value="1" Text="Excel Template"></asp:ListItem>
                                                                                    <asp:ListItem Value="2" Text="Interface" Selected="True"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                    </tr>--%>
                    <ul>
                        <li class="text">Retailer Type: <span class="error">*</span></li>
                        <li class="field">
                            <asp:DropDownList ID="cmbRetailerType" runat="server"
                                CssClass="formselect"
                                ValidationGroup="Add">
                            </asp:DropDownList>
                            <div>
                                <asp:RequiredFieldValidator ID="req2" runat="server"
                                    ControlToValidate="cmbRetailerType" CssClass="error" Display="Dynamic"
                                    ErrorMessage="Please select retailer type." InitialValue="0"
                                    SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                    </ul>
                   
                    <ul>
                        <li class="text">Sales Channel: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbsaleschannel" runat="server" AutoPostBack="true"
                                    CssClass="formselect"
                                    OnSelectedIndexChanged="cmbsaleschannel_SelectedIndexChanged"
                                    ValidationGroup="Add">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="req1" runat="server"
                                    ControlToValidate="cmbsaleschannel" CssClass="error" Display="Dynamic"
                                    ErrorMessage="Please select sales channel." InitialValue="0"
                                    SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Salesman: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlSalesman" runat="server" AutoPostBack="true"
                                    CssClass="formselect">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                    ControlToValidate="ddlSalesman" CssClass="error" Display="Dynamic"
                                    ErrorMessage="Please select salesman." InitialValue="0" SetFocusOnError="true"
                                    ValidationGroup="Add"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Contact Person: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtcontactperson" runat="server" CssClass="formfields"
                                    MaxLength="60"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txtcontactperson" CssClass="error" Display="Dynamic"
                                    ErrorMessage="Please enter contact person name." SetFocusOnError="true"
                                    ValidationGroup="Add"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">Retailer Code: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtretailercode" runat="server" CssClass="formfields" MaxLength="20" Enabled="false" Text="autogenerated"></asp:TextBox>
                        </li>
                        <li class="text">Retailer Name: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtretailername" runat="server" CssClass="formfields" MaxLength="90"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtretailername"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter sales channel name."
                                    SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">State: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbstate" runat="server" AutoPostBack="true" CssClass="formselect"
                                    OnSelectedIndexChanged="cmbstate_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="cmbstate"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please select state." InitialValue="0"
                                    SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">City: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbcity" runat="server" AutoPostBack="true" CssClass="formselect"
                                    OnSelectedIndexChanged="cmbcity_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="req" runat="server" ControlToValidate="cmbcity" CssClass="error"
                                    Display="Dynamic" ErrorMessage="Please select city" InitialValue="0" SetFocusOnError="true"
                                    ValidationGroup="Add"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Area:
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbArea" runat="server" CssClass="formselect">
                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <%-- 
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="cmbArea"
                                                                                CssClass="error" Display="Dynamic" ErrorMessage="Please select area" InitialValue="0"
                                                                                SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>--%>
                            </div>
                        </li>

                        <li class="text">Address1: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtAddress1" runat="server" CssClass="form_textarea" TextMode="MultiLine"
                                    ValidationGroup="Add"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAddress1"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter address1." ForeColor=""
                                    ValidationGroup="Add"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="fncChkSize"
                                    ControlToValidate="txtAddress1" CssClass="error" Display="Dynamic" ErrorMessage="Address1 should not be greater than 250"
                                    ForeColor="" ValidationGroup="Add">
                                </asp:CustomValidator>
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">Address2: <span class="error"> </span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtAddress2" runat="server" CssClass="form_textarea" TextMode="MultiLine"
                                    ValidationGroup="Add"></asp:TextBox>
                            </div>
                            <div>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="fncChkSize"
                                    ControlToValidate="txtAddress2" CssClass="error" Display="Dynamic" ErrorMessage="Address2 should not be greater than 250"
                                    ForeColor="" ValidationGroup="Add">
                                </asp:CustomValidator>
                            </div>
                        </li>

                        <li class="text">Pin Code: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtpincode" runat="server" CssClass="formfields" MaxLength="6"
                                    ValidationGroup="Add"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtpincode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter pin code." ForeColor=""
                                    ValidationGroup="Add"></asp:RequiredFieldValidator>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtpincode"
                                    ValidChars="0123456789">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RangeValidator ID="rngpin" runat="server" ControlToValidate="txtpincode" CssClass="error"
                                    Display="Dynamic" ErrorMessage="Pin code should be Proper 6 digits" ForeColor=""
                                    MaximumValue="999999" MinimumValue="100000" SetFocusOnError="True" Type="Integer"
                                    ValidationGroup="Add"></asp:RangeValidator>
                            </div>
                        </li>
                        <li class="text">Phone No: <span class="error"> </span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtphone" runat="server" CssClass="formfields" MaxLength="15"></asp:TextBox>
                            </div>
                            <div>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtphone"
                                    ValidChars="0123456789+-().,/">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">Mobile No: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtmobile" runat="server" CssClass="formfields" MaxLength="10"
                                    ValidationGroup="All"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtmobile"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter mobile no." ForeColor=""
                                    ValidationGroup="Add"></asp:RequiredFieldValidator>

                                <cc1:FilteredTextBoxExtender ID="txtmobile_FilteredTextBoxExtender" runat="server"
                                    TargetControlID="txtmobile" ValidChars="0123456789.">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                        </li>
                        <li class="text">Email ID: <span class="error"> </span>
                            <%--<font class="error">*</font>--%>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtemail" runat="server" CssClass="formfields" MaxLength="80"
                                    ValidationGroup="Add"></asp:TextBox>
                            </div>
                            <div>
                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtemail"
                                                                                CssClass="error" Display="Dynamic" ErrorMessage="Please enter Contact Person Email ID."
                                                                                ForeColor="" ValidationGroup="Add"></asp:RequiredFieldValidator>--%>
                                <asp:RegularExpressionValidator ID="RegularEmail" runat="server" ControlToValidate="txtemail"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter valid emailid."
                                    ForeColor="" meta:resourcekey="RegularEmail" SetFocusOnError="true" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ValidationGroup="Add"></asp:RegularExpressionValidator>
                            </div>
                        </li>
                        <li class="text">Counter Size: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtcountersize" runat="server" CssClass="formfields" MaxLength="3" ValidationGroup="Add"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtcountersize"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter counter size." ForeColor=""
                                    ValidationGroup="Add"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="rdNumeric" runat="server" CssClass="error" ControlToValidate="txtcountersize" ValidationGroup="Add" ValidationExpression="^\d+$" ErrorMessage="Please Enter Number only"></asp:RegularExpressionValidator>
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">TIN No: <span class="error"> </span>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txttinno" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                        </li>

                        <li class="text">
                            <asp:Label ID="lblUserStatus" runat="server" AssociatedControlID="chkactive">Status :</asp:Label>
                        </li>
                        <li class="field">
                            <asp:CheckBox ID="chkactive" runat="server" Checked="true" />
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSubmit" runat="server" CausesValidation="true" CssClass="buttonbg"
                                    OnClick="btnSubmit_Click" Text="Submit" ValidationGroup="Add" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" OnClick="btnCancel_Click"
                                    Text="Cancel" ToolTip="Cancel" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
