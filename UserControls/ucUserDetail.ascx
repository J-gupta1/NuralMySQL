<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucUserDetail.ascx.cs"
    Inherits="UserControls_ucUserDetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="ucContactNo.ascx" TagName="ucContactNo" TagPrefix="uc2" %>
<ul>
    <div id="dvCustomerCode" runat="server">
        <li class="text">
            <asp:Label ID="lblCodeHeader" runat="server" /></li>
        <li class="field">
            <asp:Label ID="lblCustomerCode" runat="server" /></li>
    </div>
    <%--<%=Resources.ApplicationKeyword.CustomerCompanyName %>--%>
    <li class="text">Customer Company Name:<asp:Label ID="lblCompanyManadatory" runat="server"
        class="mandatory" Visible="false">*</asp:Label></li>
    <li class="field">
        <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="100" CssClass="formfields"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" ControlToValidate="txtCompanyName" Display="Dynamic"
            ErrorMessage="Enter company name." CssClass="error" Enabled="false"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="regexCompanyName" runat="server" ControlToValidate="txtCompanyName"
            CssClass="error" Display="Dynamic" ErrorMessage="Invalid Char(s) entered." ValidationExpression="^[^&lt;^&gt;^@^%]{1,100}$"></asp:RegularExpressionValidator>
    </li>
    <li class="text">First Name:<font class="mandatory">*</font></li>
    <li class="field">
        <asp:TextBox ID="txtFirstName" runat="server" MaxLength="20" CssClass="formfields"></asp:TextBox>
        <br />
        <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
            ErrorMessage="Enter first name." Display="Dynamic" CssClass="error"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="regexFirstName" runat="server" ControlToValidate="txtFirstName"
            CssClass="error" Display="Dynamic" ErrorMessage="Invalid Char(s) entered." ValidationExpression="^[a-zA-Z]{1,20}$"></asp:RegularExpressionValidator></li>
    <li class="text">Middle Name:</li>
    <li class="field">
        <asp:TextBox ID="txtMiddleName" runat="server" MaxLength="20" CssClass="formfields"></asp:TextBox>
        <asp:RegularExpressionValidator ID="regexMiddleName" runat="server" ControlToValidate="txtMiddleName"
            CssClass="error" Display="Dynamic" ErrorMessage="Invalid Char(s) entered." ValidationExpression="^[a-zA-Z]{1,20}$"></asp:RegularExpressionValidator></li>
    <li class="text">Last Name:<font class="mandatory">*</font></li>
    <li class="field">
        <asp:TextBox ID="txtLastName" runat="server" MaxLength="20" CssClass="formfields"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
            Display="Dynamic" ErrorMessage="Enter last name." CssClass="error"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="regexLastName" runat="server" ControlToValidate="txtLastName"
            CssClass="error" Display="Dynamic" ErrorMessage="Invalid Char(s) entered." ValidationExpression="^[a-zA-Z]{1,20}$"></asp:RegularExpressionValidator>
    </li>
    <li class="text">Email-ID:</li>
    <li class="field">
        <asp:TextBox ID="txtEmailID" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>
        <asp:RegularExpressionValidator ID="regexEmail" runat="server" ErrorMessage="Invalid Email"
            Display="Dynamic" CssClass="error" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
            ControlToValidate="txtEmailID" />
    </li>
    <li class="text">Address Line1:<font class="mandatory">*</font>
        <br />
        <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress"
            Display="Dynamic" ErrorMessage="Enter address." CssClass="error" />
        <asp:RegularExpressionValidator ID="regexAddress" runat="server" ControlToValidate="txtAddress"
            CssClass="error" Display="Dynamic" ErrorMessage="Invalid Char(s) Or Max length(200) exceeds."
            ValidationExpression="^[^&lt;^&gt;^@^%]{1,200}$"></asp:RegularExpressionValidator>
    </li>
    <li class="field">
        <asp:TextBox ID="txtAddress" runat="server" MaxLength="200" TextMode="MultiLine"
            CssClass="form_textarea"></asp:TextBox>
    </li>
    <li class="text">Address Line2:<br />
        <asp:RegularExpressionValidator ID="regexAddress2" runat="server" ControlToValidate="txtStreet"
            CssClass="error" Display="Dynamic" ErrorMessage="Invalid Char(s) Or Max length(200) exceeds."
            ValidationExpression="^[^&lt;^&gt;^@^%]{1,200}$"></asp:RegularExpressionValidator></li>
    <li class="field">
        <asp:TextBox ID="txtStreet" runat="server" MaxLength="200" TextMode="MultiLine" CssClass="form_textarea"></asp:TextBox>
    </li>
    <li class="text">Land Mark:<br />
        <asp:RegularExpressionValidator ID="regexLandMark" runat="server" ControlToValidate="txtArea"
            CssClass="error" Display="Dynamic" ErrorMessage="Invalid Char(s) Or Max length(200) exceeds."
            ValidationExpression="^[^&lt;^&gt;^@^%]{1,200}$"></asp:RegularExpressionValidator>
    </li>
    <li class="field">
        <asp:TextBox ID="txtArea" runat="server" MaxLength="200" TextMode="MultiLine" CssClass="form_textarea"></asp:TextBox>
    </li>
    <li class="text">State:<font class="mandatory">*</font></li>
    <li class="field">
        <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlState_SelectedIndexChanged"
            CssClass="formselect">
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="efvState" runat="server" ControlToValidate="ddlState"
            ErrorMessage="Select state." CssClass="error" InitialValue="0"></asp:RequiredFieldValidator></li>
    <li class="text">City:<font class="mandatory">*</font></li>
    <li class="field">
        <asp:UpdatePanel ID="updpnlCity" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged"
                    CssClass="formselect" OnDataBound="ddlCity_DataBound">
                    <asp:ListItem Text="Select" Value="0" />
                </asp:DropDownList>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlState" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="ddlCity"
            ErrorMessage="Select City." CssClass="error" InitialValue="0" Display="Dynamic"></asp:RequiredFieldValidator></li>
</ul> 
<asp:UpdatePanel ID="updpnlOtherCity" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
<ul>
   
            <div id="dvOtherCity" runat="server" visible="false">
                <li class="text">Other City:<font class="mandatory">*</font></li>
                <li class="field">
                    <asp:TextBox ID="txtOtherCity" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvOtherCity" runat="server" ControlToValidate="txtOtherCity"
                        ErrorMessage="Enter city name." CssClass="error"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regexpOtherCity" runat="server" ControlToValidate="txtOtherCity"
                        CssClass="error" Display="Dynamic" ErrorMessage="Spaces and special Char(s) are not allowed." ValidationExpression="^[^&lt;^&gt;^@^%]{1,50}$"></asp:RegularExpressionValidator>
                </li>
            </div></ul>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlCity" EventName="SelectedIndexChanged" />
        </Triggers>
    

</asp:UpdatePanel>
<ul>
    <li class="text">STD Code:</li>
    <li class="field">
        <asp:UpdatePanel ID="updpnlSTDCode" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblStdCode" runat="server"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlCity" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </li>
    <li class="text">Locality:</li>
    <li class="field">
        <asp:UpdatePanel ID="updpnlLocality" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:TextBox ID="txtRegion" runat="server" MaxLength="50" autocomplete="off" CssClass="formfields"
                    OnTextChanged="txtRegion_TextChanged" AutoPostBack="True"></asp:TextBox>
                <asp:RegularExpressionValidator ID="regexRegion" runat="server" ControlToValidate="txtRegion"
                    CssClass="error" Display="Dynamic" ErrorMessage="Invalid Char(s) entered." ValidationExpression="[^&lt;&gt;/\@%()]{1,50}$"></asp:RegularExpressionValidator>
                <cc1:AutoCompleteExtender ID="AutoCompleteExtenderModel" runat="server" MinimumPrefixLength="1"
                    ServiceMethod="GetRegionList" ServicePath="~/Services/Common/WBService.asmx" TargetControlID="txtRegion"
                    UseContextKey="true" ContextKey="CityID">
                </cc1:AutoCompleteExtender>
                <asp:HiddenField ID="hdnfldLocalityID" runat="server" Value="0" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlCity" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </li>
    <li class="text">Pin Code:</li>
    <li class="field">
        <asp:TextBox ID="txtPinCode" runat="server" MaxLength="6" CssClass="formfields"></asp:TextBox>
        <asp:RegularExpressionValidator ID="regexPincode" runat="server" ControlToValidate="txtPinCode"
            CssClass="error" Display="Dynamic" ErrorMessage="Input Numbers only." ValidationExpression="^[0-9]{1,6}$"></asp:RegularExpressionValidator></li>
    <li class="text">Phone Number:<font class="mandatory">+</font></li>
    <li class="field">
        <uc2:ucContactNo ID="ucPhoneNo" runat="server" IsRequired="false" ContactType="Phone"
            cssclass="formfields" />
    </li>
    <li class="text">Alt. Phone No.:</li>
    <li class="field">
        <uc2:ucContactNo ID="UCPhoneAlt" runat="server" ContactType="Phone" IsRequired="false" />
    </li>
    <li class="text">Mobile No.:<font class="mandatory">+</font></li>
    <li class="field">
        <uc2:ucContactNo ID="ucMobileNo" runat="server" ContactType="Mobile" IsRequired="false" />
    </li>
    <li class="text">Alt. Mobile No.:</li>
    <li class="field">
        <uc2:ucContactNo ID="ucAltMobile" runat="server" ContactType="Mobile" IsRequired="false" />
    </li>
    <li class="text">Date of birth:</li>
    <li class="field">
        <uc1:ucDatePicker ID="ucDateOfBirth" runat="server" IsRequired="false" AutoPostBack="True"
            OnDateChanged="ucDOB_TextBoxDate_TextChanged" />
    </li>
    <li class="text">Marriage Anniv:</li>
    <li class="field">
        <div style="width: 110px; float: left;">
            <uc1:ucDatePicker ID="ucMarraigeAnniversary" runat="server" IsRequired="false" AutoPostBack="True"
                OnDateChanged="ucMA_TextBoxDate_TextChanged" />
            <br />
            <asp:Label ID="lblmsgAniversary" runat="server" CssClass="error"></asp:Label>
        </div>
    </li>
    
