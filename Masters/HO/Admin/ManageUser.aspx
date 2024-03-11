<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageUser.aspx.cs" Inherits="Masters_HO_Admin_ManageUser" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucNavigationLinks.ascx" TagName="ucLinks" TagPrefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .grid22 {
            width: 100%;
            height: auto;
            max-height: 300px;
            overflow: auto;
            background-color: #f5f5f5;
            scrollbar-base-color: #fff;
            scrollbar-3dlight-color: #fff;
            scrollbar-arrow-color: #505050;
            scrollbar-darkshadow-color: #fff;
            scrollbar-face-color: #6b6d6e;
            scrollbar-highlight-color: #fff;
            scrollbar-shadow-color: #fff;
            scrollbar-track-color: #fff;
            border-radius: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMsg" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="clear"></div>
        <div class="tabArea">
            <uc7:ucLinks ID="ucLinks" runat="server" XmlFilePath="~/Assets/XML/LinksXML.xml" />
        </div>
        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mainheading">
                    Add / Edit Location
                </div>
                <div class="contentbox">
                    <div class="mandatory">
                        (*) Marked fields are mandatory            
                    </div>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblddlHierarchyLevel" CssClass="formtext" runat="server" AssociatedControlID="ddlHierarchyLevel">Hierarchy Level:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:DropDownList CausesValidation="true" ID="ddlHierarchyLevel" runat="server" CssClass="formselect"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlHierarchyLevel_SelectedIndexChanged">
                                </asp:DropDownList>
                                <div>
                                    <asp:Label Style="display: none;" runat="server" ID="lblHierarchyLevel" CssClass="error"></asp:Label><asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlHierarchyLevel" CssClass="error"
                                        Display="Dynamic" InitialValue="0" ErrorMessage="Please select hierarchy level."
                                        SetFocusOnError="true" ValidationGroup="AddOrgValidationGroup"></asp:RequiredFieldValidator>
                                </div>
                            </li>
                            <li class="text">
                                <asp:Label ID="lblParentHierarchy" runat="server" AssociatedControlID="ddlParentHierarchy"
                                    CssClass="formtext">Parent Hierarchy Name:</asp:Label>
                            </li>
                            <li class="field">
                                <asp:DropDownList CausesValidation="true" ID="ddlParentHierarchy" runat="server"
                                    CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="text">
                                <asp:Label ID="lblLocationName" runat="server" AssociatedControlID="txtLocationName"
                                    CssClass="formtext">Location Name:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtLocationName" runat="server" CssClass="formfields" MaxLength="50"
                                    ValidationGroup="AddOrgValidationGroup"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqVLocationName" runat="server" ControlToValidate="txtLocationName"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter location Name."
                                    SetFocusOnError="true" ValidationGroup="AddOrgValidationGroup"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="reqLocationName" ControlToValidate="txtLocationName" Display="Dynamic"
                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,30}"
                                    ValidationGroup="AddOrgValidationGroup" runat="server" />
                            </li>
                        </ul>
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblLocationCode" runat="server" AssociatedControlID="txtLocationCode"
                                    CssClass="formtext">Location Code:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtLocationCode" runat="server" CssClass="formfields" MaxLength="20"
                                    ValidationGroup="AddOrgValidationGroup"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqLocationCode" runat="server" ControlToValidate="txtLocationCode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter location code."
                                    SetFocusOnError="true" ValidationGroup="AddOrgValidationGroup"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regLocationCode" ControlToValidate="txtLocationCode" Display="Dynamic"
                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,30}"
                                    ValidationGroup="AddOrgValidationGroup" runat="server" />
                            </li>
                            <li class="text">
                                <asp:Label ID="lblState" runat="server"
                                    CssClass="formtext">State:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlState" runat="server" CssClass="formselect" AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlState" CssClass="error"
                                    Display="Dynamic" InitialValue="0" ErrorMessage="Please select State."
                                    SetFocusOnError="true" ValidationGroup="AddOrgValidationGroup"></asp:RequiredFieldValidator>
                            </li>

                            <li class="text">
                                <asp:Label ID="Label1" runat="server"
                                    CssClass="formtext">City:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlCity" runat="server" CssClass="formselect"></asp:DropDownList>
                                <asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlCity" CssClass="error"
                                    Display="Dynamic" InitialValue="0" ErrorMessage="Please select City."
                                    SetFocusOnError="true" ValidationGroup="AddOrgValidationGroup"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">
                                <asp:Label ID="lblchkActive" runat="server" AssociatedControlID="chkActive" CssClass="formtext"> Status :</asp:Label>
                            </li>
                            <li class="field">
                                <asp:CheckBox ID="chkorg" runat="server" Checked="true" />
                            </li>
                        </ul>
                        <div class="clear"></div>
                        <div class="contentbox">
                            <div class="H25-C3-S">
                                <div>
                                    <asp:Label ID="lblBrandCategory" runat="server" CssClass="formtext"> Brand Category Mapping:</asp:Label></div>
                                <div>
                                    <asp:CheckBox ID="chkAll" Text="Select All" runat="server" OnCheckedChanged="chkAll_CheckedChanged" AutoPostBack="true" /></div>
                                <div class="grid22" style="border: 1px solid #dddddd; overflow: auto">
                                    <asp:CheckBoxList ID="chkBrandCategory" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" CellPadding="4"></asp:CheckBoxList>
                                </div>

                            </div>
                        </div>
                        <div class="clear"></div>
                        <ul>
                            <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnCreate" Text="Submit" runat="server" CausesValidation="true" ValidationGroup="AddOrgValidationGroup"
                                        ToolTip="Add Location" CssClass="buttonbg" OnClick="btnCreate_Click" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnOrgCancle" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                        OnClick="btnOrgCancle_Click" />
                                </div>
                            </li>

                        </ul>
                    </div>
                </div>
                <div class="mainheading">
                    Add / Edit User
                </div>
                <div class="contentbox">
                    <div class="mandatory">
                        (*) Marked fields are mandatory
                    </div>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblUserRole" CssClass="formtext" runat="server" AssociatedControlID="ddlUserRole">User Role:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList CausesValidation="true" ID="ddlUserRole" runat="server" CssClass="formselect"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlUserRole_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:Label Style="display: none;" runat="server" ID="lblddlCheck" CssClass="error"></asp:Label>
                                    <asp:RequiredFieldValidator ID="ReqUserGroup" runat="server" ControlToValidate="ddlUserRole" CssClass="error"
                                        Display="Dynamic" InitialValue="0" ErrorMessage="Please select user type." SetFocusOnError="true"
                                        ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                </div>
                            </li>
                            <li class="text">
                                <asp:Label ID="lblFUserName" runat="server" AssociatedControlID="txtFUserName" CssClass="formtext"><%--First--%> Full Name:<span class="error">*</span></asp:Label>
                                <%--#CC04 First Name changed to Full Name--%>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtFUserName" runat="server" CssClass="formfields" MaxLength="50"
                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqFUserName" runat="server" ControlToValidate="txtFUserName"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter first name." SetFocusOnError="true"
                                    ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtFUserName" Display="Dynamic"
                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,50}"
                                    ValidationGroup="AddUserValidationGroup" runat="server" />
                            </li>
                            <%-- #CC04 Comment Start  <td class="formtext" valign="top" align="right" width="83">
                                                                                <asp:Label ID="lblLUserName" runat="server" AssociatedControlID="txtLUserName" CssClass="formtext">Last Name:<font class="error">*</font></asp:Label>
                                                                            </td>
                                                                            <td width="198" align="left" valign="top">
                                                                                <asp:TextBox ID="txtLUserName" runat="server" CssClass="form_input2" MaxLength="50"
                                                                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>
                                                                                <br />
                                                                                <asp:RequiredFieldValidator ID="reqFLUserName" runat="server" ControlToValidate="txtLUserName"
                                                                                    CssClass="error" Display="Dynamic" SetFocusOnError="true" ErrorMessage="Please enter last name."
                                                                                    ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                                                                <asp:RegularExpressionValidator ID="regLUserName" ControlToValidate="txtLUserName"
                                                                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,50}"
                                                                                    ValidationGroup="AddUserValidationGroup" runat="server" />
                                                                            </td>  #CC04 Comment End --%>
                        </ul>
                        <ul>
                            <%--   #CC04 Comment Start  <td class="formtext" valign="top" align="right" height="35">
                                                                                <asp:Label ID="lblDUserName" runat="server" AssociatedControlID="txtDUserName" CssClass="formtext">Display Name:</asp:Label>
                                                                            </td>
                                                                            <td valign="top" align="left">
                                                                                <asp:TextBox ID="txtDUserName" runat="server" CssClass="form_input2" MaxLength="20"></asp:TextBox><br />
                                                                                <asp:RegularExpressionValidator ID="regEDUserName" ControlToValidate="txtDUserName"
                                                                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,50}"
                                                                                    ValidationGroup="AddUserValidationGroup" runat="server" />
                                                                            </td> #CC04 Comment End --%>
                            <%--#CC04 Add Start --%>
                            <li class="text">
                                <asp:Label ID="lblDUserName" runat="server" CssClass="formtext">Mobile Number:</asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtmobile" runat="server" CssClass="formfields" MaxLength="10"
                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>

                                <asp:RegularExpressionValidator ID="regexValidatorMobileNo" runat="server" ControlToValidate="txtmobile"
                                    ValidationExpression="^[1-9]([0-9]{9})$" ValidationGroup="AddUserValidationGroup" ErrorMessage="Please enter 10 digit number without 0 prefix."
                                    CssClass="error" Display="Dynamic"></asp:RegularExpressionValidator>
                                <cc1:FilteredTextBoxExtender ID="txtmobile_FilteredTextBoxExtender" runat="server"
                                    TargetControlID="txtmobile" ValidChars="0123456789.">
                                </cc1:FilteredTextBoxExtender>

                            </li>

                            <%--#CC04 Add End--%>
                            <li class="text">
                                <asp:Label ID="Label2" runat="server" AssociatedControlID="txtLoginName" CssClass="formtext">Login Name:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtLoginName" runat="server" CssClass="formfields" MaxLength="30"
                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="ReqLoginName" runat="server" ControlToValidate="txtLoginName"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter Login Name." SetFocusOnError="true"
                                    ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regELoginName" ControlToValidate="txtLoginName" Display="Dynamic"
                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\%]{1,50}"
                                    ValidationGroup="AddUserValidationGroup" runat="server" />
                            </li>

                            <%if (Convert.ToInt32(ViewState["EditUserId"]) == 0)
                                { %>
                            <li class="text">Password:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="formfields" MaxLength="30"
                                    TextMode="Password" ValidationGroup="AddUserValidationGroup"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqRfv" runat="server" ControlToValidate="txtPassword"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter password." SetFocusOnError="true"
                                    ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>

                                <cc1:PasswordStrength ID="PStrength1" runat="server" TargetControlID="txtPassword"
                                    DisplayPosition="BelowLeft" MinimumNumericCharacters="1" MinimumSymbolCharacters="1"
                                    PreferredPasswordLength="6" PrefixText="Password Strength:" TextCssClass="error"
                                    TextStrengthDescriptions="Poor;Weak;Strong">
                                </cc1:PasswordStrength>
                            </li>


                        </ul>
                        <caption>
                            <br />
                            <%}
                            %>
                        </caption>
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblEmail" CssClass="formtext" runat="server" AssociatedControlID="txtEmail">Email Id:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmail"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter email." SetFocusOnError="true"
                                    ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                        ID="regemail" runat="server" CssClass="error" ControlToValidate="txtEmail" meta:resourcekey="RegularEmail"
                                        ValidationGroup="AddUserValidationGroup" Display="Dynamic" ValidationExpression="^([0-9a-zA-Z.-]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{1,9})$"
                                        ErrorMessage="Please enter valid email" SetFocusOnError="true"></asp:RegularExpressionValidator>
                            </li>


                            <li class="text">
                                <asp:Label ID="lblLatitude" runat="server"
                                    CssClass="formtext">Latitude:</asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtLatitude" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                            </li>
                            <li class="text">
                                <asp:Label ID="lblLongitute" runat="server"
                                    CssClass="formtext">Longitude:</asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtLongitude" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                            </li>
                            <li class="text">
                                <asp:Label ID="lblGeoFacing" runat="server"
                                    CssClass="formtext">GeoFancingRadius:</asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtGeoFancingRadius" runat="server" CssClass="formfields" MaxLength="8"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtGeoFancingRadius"
                                    ValidChars="0123456789">
                                </cc1:FilteredTextBoxExtender>
                            </li>
                            <li class="text">
                                <asp:Label ID="lblUserStatus" runat="server" AssociatedControlID="chkActive" CssClass="formtext">User Status :</asp:Label>
                            </li>
                            <li class="field">
                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                            </li>

                            <asp:Panel ID="pnlAllowAllHierarchy" runat="server" Visible="false">
                                <li class="text">
                                    <asp:Label ID="lblAllowHierarchy" runat="server" AssociatedControlID="chkAllowAllHierarchy"
                                        CssClass="formtext">Allow All Hierarchy :</asp:Label>
                                </li>
                                <li class="field">
                                    <asp:CheckBox ID="chkAllowAllHierarchy" runat="server" />
                                </li>
                            </asp:Panel>
                        </ul>

                        <ul id="OtherDetail" runat="server" visible="false">
                            <li class="text">
                                <asp:Label ID="lblGender" runat="server" CssClass="formtext">Gender:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:RadioButtonList runat="server" ID="rbtGender" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Male" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Female" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </li>

                            <li class="text">
                                <asp:Label ID="lblEmployeeId" runat="server" CssClass="formtext">EmployeeId:</asp:Label></li>
                            <li class="field">
                                <asp:TextBox ID="txtEmployeeId" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                            </li>


                            <li class="text">
                                <asp:Label ID="lblOmsId" runat="server" CssClass="formtext">OSM Id:</asp:Label></li>
                            <li class="field">
                                <asp:TextBox ID="txtOsmId" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                            </li>

                            <li class="text">
                                <asp:Label ID="lblClientEmpId" runat="server" CssClass="formtext">Client EmpId:</asp:Label></li>
                            <li class="field">
                                <asp:TextBox ID="txtClientEmpId" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                            </li>

                            <li class="text">
                                <asp:Label ID="lblWeekoff" runat="server" CssClass="formtext">Week Off:</asp:Label></li>
                            <li class="field">
                                <asp:TextBox ID="txtweekoff" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                            </li>

                            <li class="text">
                                <asp:Label ID="lblDateofbirth" runat="server" CssClass="formtext">Date Of Birth:</asp:Label></li>
                            <li class="field">
                                <uc2:ucDatePicker ID="ucDateofBirth" ErrorMessage="Please Enter Date Of Birth."
                                    ValidationGroup="AddUserValidationGroup" runat="server" defaultDateRange="True" RangeErrorMessage="Date should be less then equal to current date." />
                            </li>
                            <li class="text">
                                <asp:Label ID="lblStreet" runat="server" CssClass="formtext">Street:</asp:Label></li>
                            <li class="field">
                                <asp:TextBox ID="txtStreet" runat="server" CssClass="formfields" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                            </li>

                            <li class="text">
                                <asp:Label ID="lblBuildingName" runat="server" CssClass="formtext">Building Name:</asp:Label></li>
                            <li class="field">
                                <asp:TextBox ID="txtbuildingname" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                            </li>

                            <li class="text">
                                <asp:Label ID="lblEmployeeCountry" runat="server" CssClass="formtext">Employee Country:</asp:Label></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlEmployeeCountry" runat="server" CssClass="formselect"></asp:DropDownList>
                            </li>

                            <li class="text">
                                <asp:Label ID="lblEmployeeState" runat="server" CssClass="formtext">Employee State:</asp:Label></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlEmpstate" runat="server" CssClass="formselect"></asp:DropDownList>
                            </li>

                            <li class="text">
                                <asp:Label ID="lblEmployeeCity" runat="server" CssClass="formtext">Employee City:</asp:Label></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlEmployeeCity" runat="server" CssClass="formselect"></asp:DropDownList>
                            </li>

                            <li class="text">
                                <asp:Label ID="lblPinCode" runat="server" CssClass="formtext">Pin Code:</asp:Label></li>
                            <li class="field">
                                <asp:TextBox ID="txtPincode" runat="server" CssClass="formfields" MaxLength="6"></asp:TextBox>
                            </li>

                            <li class="text">
                                <asp:Label ID="lblDateofJoining" runat="server" CssClass="formtext">Date Of Join:</asp:Label></li>
                            <li class="field">
                                <uc2:ucDatePicker ID="UcDateofJoin" ErrorMessage="Please Enter Date Of Join."
                                    ValidationGroup="AddUserValidationGroup" runat="server" defaultDateRange="True" RangeErrorMessage="Date should be less then equal to current date." />
                            </li>

                            <li class="text">
                                <asp:Label ID="lblDepartmentName" runat="server" CssClass="formtext">Department Name:</asp:Label></li>
                            <li class="field">
                                <asp:TextBox ID="txtDepartmentName" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                            </li>

                            <li class="text">
                                <asp:Label ID="lblWorkLocation" runat="server" CssClass="formtext">Work Location:</asp:Label></li>
                            <li class="field">
                                <asp:TextBox ID="txtWorklocation" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                            </li>

                            <li class="text">
                                <asp:Label ID="lblBranchCode" runat="server" CssClass="formtext">Branch Code:</asp:Label></li>
                            <li class="field">
                                <asp:TextBox ID="txtBranchCode" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                            </li>

                            <li class="text">
                                <asp:Label ID="lblEmployeeType" runat="server" CssClass="formtext">Employee Type:</asp:Label></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlEmployeeType" runat="server" CssClass="formselect"></asp:DropDownList>
                            </li>

                            <li class="text">
                                <asp:Label ID="lblEmployeeStatus" runat="server" AssociatedControlID="chkActive" CssClass="formtext">Employee Status :</asp:Label>
                            </li>
                            <li class="field">
                                <asp:CheckBox ID="ChkEmployeeStatus" runat="server" Checked="true" />
                            </li>
                            <li class="text">
                                <asp:Label ID="lblESICnumber" runat="server" CssClass="formtext">Employee ESIC No:</asp:Label></li>
                            <li class="field">
                                <asp:TextBox ID="txtESICNumber" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                            </li>

                            <li class="text">
                                <asp:Label ID="lblPayRegionName" runat="server" CssClass="formtext">Pay Region Name:</asp:Label></li>
                            <li class="field">
                                <asp:TextBox ID="txtPayRegionName" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>
                            </li>
                        </ul>

                        <asp:Panel ID="pnlRegion" Visible="false" runat="server">
                            <div class="clear"></div>
                            <ul>
                                <li class="text">Select Location:<span class="error">*</span>
                                </li>
                                <li class="text-field3" style="height: auto">
                                    <div style="width: 99%; overflow: auto;">
                                        <asp:CheckBoxList ID="chkRegion" runat="server" CssClass="gridspace" RepeatColumns="4"
                                            RepeatDirection="Horizontal" Width="100%">
                                        </asp:CheckBoxList>
                                    </div>
                                </li>
                            </ul>
                        </asp:Panel>




                        <ul>
                            <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnCreateUser" Text="Submit" runat="server" CausesValidation="true"
                                        ValidationGroup="AddUserValidationGroup" OnClick="btnCreateUser_Click" ToolTip="Submit"
                                        CssClass="buttonbg" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                        OnClick="btnCancel_Click" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                <asp:PostBackTrigger ControlID="btnCreateUser" />
                <asp:PostBackTrigger ControlID="btnCreate" />
                <asp:PostBackTrigger ControlID="chkAll" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="mainheading">
            Search User
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="H20-C3-S">
                        <ul>
                            <li class="text"><%--Display --%> Full Name:&nbsp; <%--#CC04 Instead of "Display Name" "Full Name" gets displayed--%>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtDisplayname" runat="server" MaxLength="100" CssClass="formfields">
                                </asp:TextBox>
                            </li>
                            <li class="text">User Role:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlUserType" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="text">User Status:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlUserStatus" runat="server" CssClass="formselect">
                                    <asp:ListItem Selected="True" Value="2" Text="All"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Active"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="Inactive"></asp:ListItem>
                                </asp:DropDownList>
                            </li>
                            <%--#CC04 Add Start --%>
                        </ul>
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblMobileNumberSearch" runat="server" CssClass="formtext">Mobile Number:</asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtMobileNumberSearch" runat="server" CssClass="formfields" MaxLength="10"></asp:TextBox>

                                <asp:RegularExpressionValidator ID="regexMobileNumberSearch" runat="server" ControlToValidate="txtMobileNumberSearch"
                                    ValidationExpression="^[1-9]([0-9]{9})$" ErrorMessage="Please enter 10 digit number without 0 prefix."
                                    CssClass="error" Display="Dynamic"></asp:RegularExpressionValidator>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                    TargetControlID="txtMobileNumberSearch" ValidChars="0123456789.">
                                </cc1:FilteredTextBoxExtender>

                            </li>
                            <li class="text">
                                <asp:Label ID="lblEmailIDSearch" CssClass="formtext" runat="server" AssociatedControlID="txtEmail">Email Id: </asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtEmailIDSearch" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="regexEmailIDSearch" runat="server" CssClass="error"
                                    ControlToValidate="txtEmailIDSearch" meta:resourcekey="RegularEmail" Display="Dynamic"
                                    ValidationExpression="^([0-9a-zA-Z.-]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{1,9})$"
                                    ErrorMessage="Please enter valid email" SetFocusOnError="true"></asp:RegularExpressionValidator>
                            </li>
                            <%--#CC04 Add End--%>
                            <li class="text">Brand:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlBrand" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>


                        </ul>
                        <ul>
                            <li class="text">Product Category:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlProdCat" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnSearchUser" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                        OnClick="btnSearchUser_Click" CausesValidation="False"></asp:Button>
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnShow" runat="server" CssClass="buttonbg" Text="Show All" ToolTip="Show All"
                                        OnClick="btnShow_Click" />
                                </div>
                                <div class="export">
                                    <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" CausesValidation="False"
                                        OnClick="btnExprtToExcel_Click" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExprtToExcel" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="mainheading">
            User List
        </div>

        <div class="contentbox">
            <div class="grid1">
                <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="grdvwUserList" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                            RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                            AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                            HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                            BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                            PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected"
                            OnPageIndexChanging="grdvwUserList_PageIndexChanging" DataKeyNames="UserID" OnRowDataBound="grdvwUserList_RowDataBound"
                            OnRowCommand="grdvwUserList_RowCommand">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="RoleName"
                                    HeaderText="User Role"></asp:BoundField>
                                <%-- <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <div style="width: 100px; overflow: hidden; word-wrap: break-word;">
                                                                    <%#Server.HtmlDecode(Eval("FirstName").ToString()).Replace("<", "&lt;") + " " + Server.HtmlDecode(Eval("LastName").ToString()).Replace("<", "&lt;")%>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField> #CC04 Commented --%>
                                <%--<asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="DisplayName"
                                                            HeaderText="Display Name"></asp:BoundField>--%>

                                <asp:TemplateField HeaderText="Full Name" HeaderStyle-HorizontalAlign="Left"><%--#CC04 header name changed from "Display Name" to "Full Name" --%>
                                    <ItemTemplate>
                                        <div style="width: 100px; overflow: hidden; word-wrap: break-word;">
                                            <asp:Label runat="server" Text='<%#Eval("DisplayName") %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Email"
                                                            HeaderText="Email ID"></asp:BoundField>--%>

                                <asp:TemplateField HeaderText="Email ID" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                            <asp:Label runat="server" Text='<%#Eval("Email") %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--#CC04 Add Start--%>
                                <asp:TemplateField HeaderText="Mobile Number" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div style="width: 100px; overflow: hidden; word-wrap: break-word;">
                                            <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo") %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--#CC04 Add End--%>


                                <%--<asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="LoginName"
                                                            HeaderText="Login Name"></asp:BoundField>--%>
                                <%--#CC03 Add Start--%>
                                <asp:TemplateField HeaderText="Location Codes" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div style="width: 100px; overflow: hidden; word-wrap: break-word;">
                                            <asp:Label runat="server" Text='<%#Eval("UserLocationCodes") %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--#CC03 Add End--%>
                                <asp:TemplateField HeaderText="Login Name" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div style="width: 100px; overflow: hidden; word-wrap: break-word;">
                                            <asp:Label runat="server" Text='<%#Eval("LoginName") %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>



                                <asp:TemplateField HeaderText="Password">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPassword" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"Password"))%>'
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="lblPasswordSalt" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"PasswordSalt"))%>'
                                            Visible="false"></asp:Label>
                                        <asp:LinkButton ID="hlPassword" runat="server" Text="Password"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%#Eval("UserID") %>' runat="server" ID="btnEdit"
                                            ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' OnClick="btnEdit_Click"
                                            ToolTip="Edit User" />

                                        <asp:ImageButton CommandArgument='<%#Eval("OrgnhierarchyID") %>' runat="server" ID="btnEditLocation"
                                            ImageUrl='<%#"~/" + strAssets + "/CSS/Images/calendar-pre.png"%>' OnClick="btnEditLocation_Click"
                                            ToolTip="Edit Location" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"Status"))%>'
                                            Visible="false"></asp:Label>
                                        <asp:ImageButton ID="btnActiveDeactive" OnClick="btnActiveDeactive_Click" runat="server"
                                            CommandArgument='<%#Eval("UserID") %>' CommandName='<%#Eval("Status")%>' ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Online User" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblOnline" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"isLogin"))%>'
                                            Visible="false"></asp:Label>
                                        <asp:ImageButton ID="imgOnline" runat="server" CausesValidation="false" CommandArgument='<%#Eval("UserID") %>'
                                            ImageAlign="Top" CommandName="Online" ToolTip='<%#LoginToolTip(Convert.ToInt16(Eval("isLogin"))) %>'
                                            ImageUrl='<%# LoginStatus(Convert.ToInt16(Eval("isLogin"))) %>' />&nbsp;&nbsp;
                                                                <asp:Label ID="lblLocked" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"isLockedOut"))%>'
                                                                    Visible="false"></asp:Label>
                                        <asp:ImageButton ID="imgLocked" runat="server" CausesValidation="false" CommandArgument='<%#Eval("UserID") %>'
                                            ImageAlign="Top" CommandName="unlock" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <%--<asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" /> #CC01 Commented --%>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <%--     <uc3:footer ID="Footer1" runat="server" />--%>
    </div>
</asp:Content>
