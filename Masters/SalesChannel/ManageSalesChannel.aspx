<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageSalesChannel.aspx.cs" Inherits="Masters_HO_SalesChannel_ManageSalesChannel"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucNavigationLinks.ascx" TagName="ucLinks" TagPrefix="uc7" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<%--CCO1:Pan Mandatory For Lemon--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">

    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="clear"></div>  
    <div class="tabArea">
                    <uc7:uclinks ID="ucLinks" runat="server" XmlFilePath="~/Assets/XML/LinksXML.xml"/></div>
    <div class="mainheading mrigh">
        Manage
        <asp:Label ID="lblpage" runat="server"></asp:Label>
    </div>
    <div class="export">
        <asp:LinkButton ID="LBViewSalesChannel" runat="server" CausesValidation="False" OnClick="LBViewSalesChannel_Click"
            CssClass="elink7" Visible="false">View List</asp:LinkButton>
    </div>

    <div class="contentbox" id="AdPannel" runat="server" visible="false">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H35-C3-S">
            <ul>
                <li class="text">
                    <asp:Label ID="lblType" runat="server"></asp:Label>
                    Type: <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="cmbsaleschanneltype" runat="server" ValidationGroup="Add" AutoPostBack="true"
                            CssClass="formselect"
                            OnSelectedIndexChanged="cmbsaleschanneltype_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="cmbsaleschanneltype"
                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel type."
                            SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">
                    <asp:Label ID="lblCode" runat="server"></asp:Label>
                    Code: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtsaleschannelcode" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>

                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtsaleschannelcode"
                        FilterMode="InvalidChars" InvalidChars="[,%,]">
                    </cc1:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtsaleschannelcode"
                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter sales channel code."
                        SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                </li>
                <li class="text">
                    <asp:Label ID="lblName" runat="server"></asp:Label>
                    Name: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtsaleschannelname" runat="server" CssClass="formfields" MaxLength="70"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtsaleschannelname"
                        FilterMode="InvalidChars" InvalidChars="[,%,]">
                    </cc1:FilteredTextBoxExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtsaleschannelname"
                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter sales channel name."
                        SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                </li>
            </ul>
            <ul>
                <li class="text">
                    <asp:Label ID="lblmapparent" runat="server" Text="Is Multi Location:" Visible="false"></asp:Label>
                </li>
                <li class="field">
                    <asp:RadioButtonList ID="rdomaplocation" CssClass="radio-rs" runat="server" OnSelectedIndexChanged="rdomaplocation_SelectedIndexChanged"
                        CellPadding="2" CellSpacing="0" AutoPostBack="true" Visible="false" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Text="No" Value="0"> </asp:ListItem>
                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </li>
                <li class="text">
                    <asp:Label ID="lblparentnamegroup" runat="server" Text="Parent Distributor: " Visible="false"></asp:Label>
                    <asp:Label ID="lblreqparengroupt" runat="server" CssClass="error" Text="*" Visible="false"></asp:Label>
                </li>
                <li class="field">
                    <asp:DropDownList ID="cmbparentsaleschannelgroup" CssClass="formselect" runat="server"
                        Visible="false" ValidationGroup="Add">
                    </asp:DropDownList>
                </li>
            </ul>
            <ul>
                <li class="text">Contact Person: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtcontactperson" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtcontactperson"
                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter contact person name."
                        SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                </li>
                <li class="text">Business Start Date: <span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucBuisnessstartdate" ErrorMessage="Please Enter business Date."
                        ValidationGroup="Add" runat="server" defaultDateRange="True" RangeErrorMessage="Date should be less then equal to current date." />
                </li>
                <li class="text">Login Name:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtloginname" runat="server" CssClass="formfields" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="req5" runat="server" ControlToValidate="txtloginname"
                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter login name." SetFocusOnError="true"
                        ValidationGroup="Add"></asp:RequiredFieldValidator>
                </li>
                <%--    <%if ((Request.QueryString["SalesChannelId"] == null) && (Request.QueryString["SalesChannelId"] == ""))
                                                                                  { %>--%>
                <%--   <%} 
																					%>--%>
            </ul>
            <ul>
                <li class="text">
                    <asp:Label ID="lblpassword" Text="Password: " runat="server"></asp:Label>
                    <asp:Label
                        ID="lblreqpassword" Text="*" CssClass="error" runat="server"></asp:Label>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtpassword" runat="server" CssClass="formfields" MaxLength="20"
                        TextMode="Password" ValidationGroup="Add"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqpassword" runat="server" ControlToValidate="txtpassword"
                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter password." SetFocusOnError="true"
                        ValidationGroup="Add1"></asp:RequiredFieldValidator>
                    <cc1:PasswordStrength ID="PStrength1" runat="server" TargetControlID="txtpassword"
                        DisplayPosition="BelowLeft" MinimumNumericCharacters="1" MinimumSymbolCharacters="1"
                        PreferredPasswordLength="6" PrefixText="Password Strength:" TextCssClass="error"
                        TextStrengthDescriptions="Poor;Weak;Strong">
                    </cc1:PasswordStrength>
                </li>
                <li class="text">Parent
                    <asp:Label ID="lblParentType" runat="server" Visible="true"></asp:Label>
                    : <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="cmbparentsaleschannel" runat="server" CssClass="formselect"
                            ValidationGroup="Add" OnSelectedIndexChanged="cmbparentsaleschannel_SelectedIndexChanged"
                            AutoPostBack="true" Visible="true">
                            <asp:ListItem Selected="True" Value="0" Text="Select"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </li>
                <li class="text">Reporting Hierarchy: <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="cmborghierarchy" runat="server" CssClass="form_select" ValidationGroup="Add" Visible="True">
                            <asp:ListItem Selected="True" Value="0" Text="Select"></asp:ListItem>
                        </asp:DropDownList>

                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="req3" runat="server" ControlToValidate="cmborghierarchy"
                            CssClass="error" Display="Dynamic" ErrorMessage="Please select rpt. hierarchy name."
                            InitialValue="0" SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                    </div>
                </li>
            </ul>
            <ul>
                <%--====================#CC18 Commented Started======================================--%>
                <%--<li class="text">Country: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:UpdatePanel runat="server" ID="updCountry" UpdateMode="Always">
                        <ContentTemplate>
                            <div>
                                <asp:DropDownList ID="cmbCountry" runat="server" CssClass="formselect" OnSelectedIndexChanged="cmbCountry_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="cmbCountry"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please select Country." InitialValue="0"
                                SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </li>--%>
                <%--=======================#CC18 Commented End===================================--%>
                <li class="text">State: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:UpdatePanel runat="server" ID="updState" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="cmbstate" runat="server" CssClass="formselect" OnSelectedIndexChanged="cmbstate_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <div>
                        <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="cmbstate"
                            CssClass="error" Display="Dynamic" ErrorMessage="Please select state." InitialValue="0"
                            SetFocusOnError="true" ValidationGroup="Add"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">City: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:UpdatePanel ID="updpnlCity" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div>
                                <asp:DropDownList ID="cmbcity" runat="server" AutoPostBack="true" CssClass="form_select"
                                    OnSelectedIndexChanged="cmbcity_SelectedIndexChanged" AutoCompleteMode="Suggest">
                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:RequiredFieldValidator ID="req" runat="server" ControlToValidate="cmbcity" CssClass="error"
                        Display="Dynamic" ErrorMessage="Please select city" InitialValue="0" SetFocusOnError="true"
                        ValidationGroup="Add"></asp:RequiredFieldValidator>
                </li>
            </ul>
            <div class="clear"></div>
            <ul>
                <li class="text">Area:
                </li>
                <li class="field">
                    <asp:UpdatePanel ID="updpnlArea" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div>
                                <asp:DropDownList ID="cmbArea" runat="server" AutoCompleteMode="Suggest" CssClass="formselect">
                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </li>
                <li class="text">Address Line 1: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtAddress1" runat="server" CssClass="form_textarea" TextMode="MultiLine"
                        ValidationGroup="Add"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAddress1"
                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter address1." ForeColor=""
                        ValidationGroup="Add"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="fncChkSize"
                        ControlToValidate="txtAddress1" CssClass="error" Display="Dynamic" ErrorMessage="Address1 should not be greater than 250"
                        ForeColor="" ValidationGroup="Add"> </asp:CustomValidator>
                </li>
                <li class="text">Address Line 2:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtAddress2" runat="server" CssClass="form_textarea" TextMode="MultiLine"
                        ValidationGroup="Add"></asp:TextBox>

                    <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="fncChkSize"
                        ControlToValidate="txtAddress2" CssClass="error" Display="Dynamic" ErrorMessage="Address2 should not be greater than 250"
                        ForeColor="" ValidationGroup="Add"> </asp:CustomValidator>
                </li>
            </ul>
            <ul>
                <li class="text">Pin Code:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtpincode" runat="server" CssClass="formfields" MaxLength="6"
                        ValidationGroup="Add"></asp:TextBox>
                    <%--     <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtpincode"
                                                            CssClass="error" Display="Dynamic" ErrorMessage="Please enter pin code." ForeColor=""
                                                            ValidationGroup="Add"></asp:RequiredFieldValidator>--%>
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtpincode"
                        ValidChars="0123456789">
                    </cc1:FilteredTextBoxExtender>
                    <asp:RangeValidator ID="rngpin" runat="server" ControlToValidate="txtpincode" CssClass="error"
                        Display="Dynamic" ErrorMessage="Pin code should be Proper 6 digits" ForeColor=""
                        MaximumValue="999999" MinimumValue="100000" SetFocusOnError="True" Type="Integer"
                        ValidationGroup="Add"></asp:RangeValidator>
                </li>
                <li class="text">Phone No:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtphone" runat="server" CssClass="formfields" MaxLength="10"></asp:TextBox><%-- #CC02 length decreased from 20 to 10--%>

                    <%--#CC02 Comment Start --%>
                    <%-- <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtphone"
                                                            ValidChars="0123456789+-().,/">
                                                        </cc1:FilteredTextBoxExtender>--%>
                    <%--#CC02 Comment End --%>
                    <%--#CC02 Add Start --%>
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtphone"
                        ValidChars="0123456789">
                    </cc1:FilteredTextBoxExtender>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                        ControlToValidate="txtphone" ValidationExpression="^[1-9]([0-9]{9})$" ValidationGroup="Add" ErrorMessage="Please enter 10 digit number without 0 prefix."
                        CssClass="error" Display="Dynamic"></asp:RegularExpressionValidator>
                    <%--#CC02 Add End --%>
                </li>
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
                        <%--#CC02 Add Start --%>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                            ControlToValidate="txtmobile" ValidationExpression="^[1-9]([0-9]{9})$" ValidationGroup="Add" ErrorMessage="Please enter 10 digit number without 0 prefix."
                            CssClass="error" Display="Dynamic"></asp:RegularExpressionValidator>
                        <%--#CC02 Add End --%>
                    </div>
                </li>
            </ul>
            <%--#CC03 Add Start--%>
            <ul>
                <li class="text">Mobile No.2:
                </li>
                <li class="field">
                    <div>
                        <asp:TextBox ID="txtMobileNumber2" runat="server" CssClass="formfields" MaxLength="10"
                            ValidationGroup="All"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server"
                            TargetControlID="txtMobileNumber2" ValidChars="0123456789.">
                        </cc1:FilteredTextBoxExtender>
                        <asp:RegularExpressionValidator ID="regexAltMobile" runat="server"
                            ControlToValidate="txtMobileNumber2" ValidationExpression="^[1-9]([0-9]{9})$" ValidationGroup="Add" ErrorMessage="Please enter 10 digit number without 0 prefix."
                            CssClass="error" Display="Dynamic"></asp:RegularExpressionValidator>
                    </div>
                </li>
                <li class="text">Fax:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtfax" runat="server" CssClass="formfields" MaxLength="15"></asp:TextBox>
                </li>
                <li class="text">PAN No:
                </li>
                <li class="field">
                    <%--#CCO1 max length increase 10 to 15--%>
                    <asp:TextBox ID="txtpanno" runat="server" CssClass="formfields" MaxLength="10"></asp:TextBox>
                    <asp:RegularExpressionValidator ErrorMessage="Please enter correct pan card number" Display="Dynamic"
                        ControlToValidate="txtpanno" runat="server" ValidationExpression="^[a-zA-Z0-9]{10}$" />
                    <%--#CCO1 max length increase 10 to 15--%>
                    <asp:RequiredFieldValidator ID="RequiredFieldPanNumber" runat="server" ControlToValidate="txtpanno"
                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter Pan Number." ForeColor=""
                        ValidationGroup="Add"></asp:RequiredFieldValidator>
               </li>
            </ul>
            <%--#CC03 Add End--%>
            <ul>
                <li class="text">Email ID: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtemail" runat="server" CssClass="formfields" MaxLength="100"
                        ValidationGroup="Add"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtemail"
                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter Email ID." ForeColor=""
                        ValidationGroup="Add"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularEmail" runat="server" ControlToValidate="txtemail"
                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter valid emailid."
                        ForeColor="" meta:resourcekey="RegularEmail" SetFocusOnError="true" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        ValidationGroup="Add"></asp:RegularExpressionValidator>
                </li>
                <li class="text"><%--CST #CC06 Commented--%> GST <%--#CC06 GST Added--%> &nbsp; No:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtcstno" runat="server" CssClass="formfields" MaxLength="15"></asp:TextBox>
                </li>
                <li class="text">TIN No:
                </li>
                <li class="field">
                    <asp:TextBox ID="txttinno" runat="server" CssClass="formfields" MaxLength="15"></asp:TextBox>
                    <%--         <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txttinno"
                                                            CssClass="error" Display="Dynamic" ErrorMessage="Please enter tin no." SetFocusOnError="true"
                                                            ValidationGroup="Add"></asp:RequiredFieldValidator>--%>
                </li>
            </ul>
            <ul>
                <li class="text">Date of Birth:
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDOB" ValidationGroup="Add" runat="server" IsRequired="false"
                        defaultDateRange="True" RangeErrorMessage="Date should be less then equal to current date." />
                </li>
                <li class="text">Date of Anniversary:
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDOA" ValidationGroup="Add" runat="server" IsRequired="false"
                        defaultDateRange="True" RangeErrorMessage="Date should be less then equal to current date." />
                </li>
                <li class="text">
                    <asp:Label ID="lblUserStatus" runat="server" AssociatedControlID="chkactive" CssClass="formtext">Status :</asp:Label>
                </li>
                <li class="field">
                    <asp:CheckBox ID="chkactive" runat="server" Checked="true" />
                </li>
            </ul>
            <ul>
                <li class="text">
                    <asp:Label ID="lblSubmitOpeningStock" runat="server" Text="Submit Opening Stock :"></asp:Label>
                </li>
                <li class="field">

                    <%-- <asp:CheckBox ID="ChkWantToSubmitOpeningStock" runat="server" Text="" AutoPostBack="true"
                                                                TextAlign="Left" OnCheckedChanged="ChkWantToSubmitOpeningStock_CheckedChanged" />--%>
                    <div class="float-margin">
                        <uc2:ucDatePicker ID="ucOpeningStock" Visible="True" IsRequired="true" ErrorMessage="Insert OpeningStock Date" runat="server" RangeErrorMessage="Date should be less then equal to current date." ValidationGroup="Add" />
                    </div>
                    <div class="clear"></div>
                    <div class="error" style="font-style: italic;" id="info" visible="false" runat="server">(With Zero Quantity)</div>

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
                    <div class="float-margin">
                        <asp:Button ID="btnReject" runat="server" CausesValidation="true" CssClass="buttonbg"
                            Visible="false" Text="Reject" ValidationGroup="Add" OnClick="btnReject_Click" /><%--#CC05 ADDED--%>
                    </div>
                    <asp:HiddenField ID="hdnApproveReject" runat="server" Value="0" />
                </li>
            </ul>
            <ul id="trApproval" runat="server" visible="false">
                <%--<tr>--%>
                <li class="text">Remark: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtApprovalRemarks" CssClass="form_textarea" runat="server" TextMode="MultiLine"
                        MaxLength="500" Visible="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldApprovalRemarks" runat="server" ControlToValidate="txtApprovalRemarks"
                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter Remarks." ForeColor=""
                        ValidationGroup="Add"></asp:RequiredFieldValidator>
                </li>
            </ul>
        </div>
    </div>
   <%-- <div class="contentbox">--%>
     <%--/*#CC18 Added Started*/--%>
    <asp:UpdatePanel ID="updateBulk" runat="server">
        <ContentTemplate>
     <div class="mainheading">
            If you want to upload SalesChannel data in bulk then use below interface.
        </div>
        <div class="clear"></div>
    
        <div class="mainheading">
            Step 1 : Click On Button For Save Or Update Process
        </div>
        <div class="contentbox">
          
            <div class="H25-C3-S">

                <ul>
                    <li class="link">
                        <asp:RadioButtonList ID="Rbtdownloadtemplate" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="radio-rs" CellPadding="2" CellSpacing="0" OnSelectedIndexChanged="Rbtdownloadtemplate_SelectedIndexChanged">
                            <asp:ListItem Value="1" Text="Save" Selected="True">
                            
                            </asp:ListItem>
                            <asp:ListItem Value="2" Text="Update">
                            
                            </asp:ListItem>
                        </asp:RadioButtonList>
                    </li>


                </ul>
            </div>
                   
        </div>
       
        <div class="mainheading" runat="server" id="ForSaveTemplateheading" visible="false">
            Step 2 : Download  Template For Save Record 
        </div>
        <div class="contentbox" runat="server" id="ForSaveTemplatedownload" visible="false">
            <div class="H25-C3-S">

                <ul>
                    <li class="link">
                        <asp:LinkButton ID="DwnldTemplate" runat="server" Text="Download Template File"
                            CssClass="elink2" OnClick="DwnldTemplate_Click"></asp:LinkButton>
                    </li>


                </ul>
            </div>
        </div>
        <div class="mainheading" runat="server" id="ForUploadTemplateheading" visible="false">
            Step 2 : Download  Template For  Update Record  
        </div>
        <div class="contentbox" runat="server" id="ForUpdateTemplatedownload" visible="false">
            <div class="H25-C3-S">

                <ul>
                    <li class="link">
                        <asp:LinkButton ID="UpdateTemplateFile" runat="server" Text="Download Template File"
                            CssClass="elink2" OnClick="UpdateTemplateFile_Click"></asp:LinkButton>
                    </li>
                     <li class="link">
                        Note : In Status column  enter Inactive or can be blank , if not enter anything that means you modified records else inactive SalesChannel.  
                        </li>
                </ul>
            </div>
        </div>
        <div class="mainheading" runat="server" id="ReferenceIdForsaveheading" visible="false">
            Step 3 : Download Reference Code For Save Record 
        </div>
        <div class="contentbox" runat="server" id="ReferenceIdForsave" visible="false">
            <div class="H25-C3-S">
                <ul>
                    <li class="link">
                        <asp:LinkButton ID="DownloadReferenceCodeForSave" runat="server" Text="Download Reference Code"
                            CssClass="elink2" OnClick="DownloadReferenceCodeForSave_Click"></asp:LinkButton>
                    </li>
                </ul>
            </div>
        </div>
        <div class="mainheading" runat="server" id="ReferenceIdForupdateheading" visible="false">
        Step 3 : Download Reference Code For Update Record 
    </div>
    <div class="contentbox" runat="server" id="ReferenceIdForupdate" visible="false">
        <div class="H25-C3-S">
            <ul>
                <li class="link">
                    <asp:LinkButton ID="DownloadReferenceCodeForUpdate" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DownloadReferenceCodeForUpdate_Click"></asp:LinkButton>
                </li>
                
            </ul>
        </div>
    </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Rbtdownloadtemplate" />
            <asp:PostBackTrigger ControlID="DwnldTemplate" />
             <asp:PostBackTrigger ControlID="UpdateTemplateFile" />
             <asp:PostBackTrigger ControlID="DownloadReferenceCodeForSave" />
             <asp:PostBackTrigger ControlID="DownloadReferenceCodeForUpdate" />

        </Triggers>
    </asp:UpdatePanel>
        <div class="mainheading">
            Step 4 : Upload Excel File
        </div>
        <div class="contentbox">
            <div class="H25-C3-S">

                <ul>
                    <li class="text">Upload File: <span class="error">*</span>
                    </li>
                    <li class="field">
                        <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />

                        <asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                    </li>
                    <li class="field3">
                        <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" 
                            OnClick="btnUpload_Click" />
                    </li>
                    <li class="link">
                        <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                    </li>
                </ul>
            </div>
            </div>
        <%--</div>--%>
    <%-- /*#CC18 Added Started*/--%>

</asp:Content>
