<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageParallelOrgnHierarchy.aspx.cs" Inherits="Masters_HO_Admin_ManageParallelOrgnHierarchy" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMsg" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="mainheading">
                    Add / Edit Parallel Location
                </div>
                <div class="contentbox">
                    <div class="mandatory">
                        (*) Marked fields are mandatory            
                    </div>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblddlHierarchyLevelText" CssClass="formtext" runat="server" AssociatedControlID="ddlHierarchyLevel">Hierarchy Level:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <div>
                                    <asp:DropDownList CausesValidation="true" ID="ddlHierarchyLevel" runat="server" CssClass="formselect">
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <asp:Label Style="display: none;" runat="server" ID="lblHierarchyLevelText" CssClass="error"></asp:Label>
                                    <asp:RequiredFieldValidator
                                        ID="ReqUserGroup" runat="server" ControlToValidate="ddlHierarchyLevel" CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select hierarchy level." SetFocusOnError="true" ValidationGroup="AddParallelOrgnrValidationGroup">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </li>
                            <li class="text">
                                <asp:Label ID="lblLocationCodeText" runat="server" AssociatedControlID="txtLocationCode" CssClass="formtext">Location Code:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtLocationCode" runat="server" CssClass="formfields" MaxLength="20" ValidationGroup="AddParallelOrgnrValidationGroup"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqLocationCode" runat="server" ControlToValidate="txtLocationCode" CssClass="error" Display="Dynamic" ErrorMessage="Please enter location code." SetFocusOnError="true" ValidationGroup="AddParallelOrgnrValidationGroup"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regLocationCode" ControlToValidate="txtLocationCode" CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,30}" ValidationGroup="AddParallelOrgnrValidationGroup" runat="server" Display="Dynamic" />
                            </li>
                            <li class="text">
                                <asp:Label ID="lblLocationNameText" runat="server" AssociatedControlID="txtLocationName" CssClass="formtext">Location Name:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtLocationName" runat="server" CssClass="formfields" MaxLength="50" ValidationGroup="AddParallelOrgnrValidationGroup"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqVLocationName" runat="server" ControlToValidate="txtLocationName" CssClass="error" Display="Dynamic" ErrorMessage="Please enter location Name." SetFocusOnError="true" ValidationGroup="AddParallelOrgnrValidationGroup"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="reqLocationName" ControlToValidate="txtLocationName" CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,30}" ValidationGroup="AddParallelOrgnrValidationGroup" runat="server" Display="Dynamic" />
                            </li>
                        </ul>
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblFUserName" runat="server" CssClass="formtext">Full Name:<span class="error">*</span></asp:Label>

                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtFullName" runat="server" CssClass="formfields" MaxLength="50" ValidationGroup="AddParallelOrgnrValidationGroup"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqFUserName" runat="server" ControlToValidate="txtFullName" CssClass="error" Display="Dynamic" ErrorMessage="Please enter first name." SetFocusOnError="true" ValidationGroup="AddParallelOrgnrValidationGroup"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtFullName" CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,50}" ValidationGroup="AddParallelOrgnrValidationGroup" runat="server" />
                            </li>

                            <li class="text">
                                <asp:Label ID="Label2" runat="server" AssociatedControlID="txtLocationCode" CssClass="formtext">Login Name:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtLoginName" runat="server" CssClass="formfields" MaxLength="30" ValidationGroup="AddParallelOrgnrValidationGroup"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="ReqLoginName" runat="server" ControlToValidate="txtLoginName" CssClass="error" Display="Dynamic" ErrorMessage="Please enter Login Name." SetFocusOnError="true" ValidationGroup="AddParallelOrgnrValidationGroup"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regELoginName" ControlToValidate="txtLoginName" CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,50}" ValidationGroup="AddParallelOrgnrValidationGroup" runat="server" Display="Dynamic" />
                            </li>

                            <%if (Convert.ToInt32(ViewState["EditParallelOrgnhierarchyID"]) == 0)
                              { %>
                            <li class="text">Password:<span class="error">*</span></li>
                            <li class="field">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="formfields" MaxLength="30" TextMode="Password" ValidationGroup="AddParallelOrgnrValidationGroup"></asp:TextBox>
                                <div>
                                    <asp:RequiredFieldValidator ID="reqRfv" runat="server" ControlToValidate="txtPassword" CssClass="error" Display="Dynamic" ErrorMessage="Please enter password." SetFocusOnError="true" ValidationGroup="AddParallelOrgnrValidationGroup"></asp:RequiredFieldValidator>
                                </div>
                                <div>
                                    <cc1:PasswordStrength ID="PStrength1" runat="server" TargetControlID="txtPassword" DisplayPosition="BelowLeft" MinimumNumericCharacters="1" MinimumSymbolCharacters="1" PreferredPasswordLength="6" PrefixText="Password Strength:" TextCssClass="error"
                                        TextStrengthDescriptions="Poor;Weak;Strong">
                                    </cc1:PasswordStrength>
                                </div>
                            </li>
                            <caption>

                                <%} 
                                %>
                            </caption>
                        </ul>
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblEmail" runat="server" CssClass="formtext">Email:</asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="formfields" MaxLength="80"
                                    ValidationGroup="AddParallelOrgnrValidationGroup"></asp:TextBox>

                                <asp:RegularExpressionValidator
                                    ID="regEmail" runat="server" CssClass="error" ControlToValidate="txtEmail" meta:resourcekey="RegularEmail"
                                    ValidationGroup="AddParallelOrgnrValidationGroup" Display="Dynamic" ValidationExpression="^([0-9a-zA-Z.-]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{1,9})$"
                                    ErrorMessage="Please enter valid email" SetFocusOnError="true"></asp:RegularExpressionValidator>
                                <%--<asp:RequiredFieldValidator ID="rqEmail" runat="server" ControlToValidate="TxtEmail"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Please Email."
                                        SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator><br />--%>

                            </li>
                            <li class="text">
                                <asp:Label ID="lblMobileNoText" runat="server" CssClass="formtext">Mobile No:</asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtMobileNo" runat="server" CssClass="formfields" MaxLength="10"
                                    ValidationGroup="AddParallelOrgnrValidationGroup"></asp:TextBox>

                                <asp:RegularExpressionValidator ID="regexValidatorMobileNo" runat="server" ControlToValidate="txtMobileNo" 
                                    ValidationExpression="^[1-9]([0-9]{9})$" ValidationGroup="AddParallelOrgnrValidationGroup" ErrorMessage="Please enter 10 digit number without 0 prefix."
                                    CssClass="error" Display="Dynamic"></asp:RegularExpressionValidator>
                                <cc1:FilteredTextBoxExtender ID="txtmobile_FilteredTextBoxExtender" runat="server"
                                    TargetControlID="txtMobileNo" ValidChars="0123456789.">
                                </cc1:FilteredTextBoxExtender>

                            </li>
                            <li class="text">
                                <asp:Label ID="lblchkActive" runat="server" AssociatedControlID="chkActive" CssClass="formtext"> Status :</asp:Label>
                            </li>
                            <li class="field">
                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                            </li>
                             <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                <asp:Button ID="btnCreate" Text="Submit" runat="server" CausesValidation="true" ValidationGroup="AddParallelOrgnrValidationGroup"
                                    ToolTip="Add Location" CssClass="buttonbg" OnClick="btnCreate_Click" />
                                </div>
                                 <div class="float-margin">
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                    OnClick="btnCancel_Click" /></div>
                            </li>
                        </ul>
                    </div>
                </div>

            </ContentTemplate>

            <Triggers>
                <asp:PostBackTrigger ControlID="btnCreate" />
            </Triggers>

        </asp:UpdatePanel>

        <div class="mainheading">
            Search
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="H20-C3-S">
                        <ul>
                            <li class="text">Hierarchy Level:
                            </li>
                            <li class="field">
                                <asp:DropDownList CausesValidation="true" ID="ddlSerHierarchyLevel" runat="server"
                                    CssClass="formselect" AutoPostBack="false">
                                </asp:DropDownList>
                            </li>
                            <li class="text">Location Name:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtLocationNameSearch" runat="server" MaxLength="100" CssClass="formfields">
                                </asp:TextBox>
                            </li>
                            <li class="text">Location Code:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtLocationCodeSearch" runat="server" CssClass="formfields" MaxLength="100">
                                </asp:TextBox>
                            </li>
                        </ul>
                        <ul>

                            <%--  <td  valign="top" width="12%" height="35" class="formtext">Parent Code:
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <div style="float: left; width: 135px;">
                                                                        <asp:TextBox ID="txtParentCode" CssClass="form_input6" runat="server" MaxLength="20"></asp:TextBox>
                                                                    </div>
                                                                </td>

                                                                <td align="right" valign="top" width="12%" height="35" class="formtext">Parent Location Name:
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <asp:TextBox ID="txtSerParentLocationName" runat="server" MaxLength="100" CssClass="form_input6">
                                                                    </asp:TextBox>
                                                                </td>--%>

                            <li class="text">User Name:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtUserNameSearch" CssClass="formfields" runat="server" MaxLength="100"></asp:TextBox>

                            </li>
                            <%--</tr>
                                                            <tr>
                                                                <td colspan="4"></td>--%>
                            <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnSearch" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                        OnClick="btnSearch_Click"></asp:Button>
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnShow" runat="server" CssClass="buttonbg" OnClick="btnShow_Click"
                                        Text="Show All" ToolTip="Search" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="mainheading">
            Location List
        </div>
        <div class="export">
            <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" CausesValidation="False"
                OnClick="btnExprtToExcel_Click" />
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Always">
                    <ContentTemplate>
                        <asp:GridView ID="gvParallelOrgnHierarchyUser" runat="server" FooterStyle-VerticalAlign="Top"
                            FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                            HeaderStyle-HorizontalAlign="left" EmptyDataText="No Record found" HeaderStyle-VerticalAlign="top"
                            GridLines="none" AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow"
                            FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader" CellSpacing="1"
                            PageSize='<%$ AppSettings:GridViewPageSize %>' CellPadding="4" bgcolor="" BorderWidth="0px"
                            Width="100%" AutoGenerateColumns="false" AllowPaging="True" SelectedStyle-CssClass="gridselected" OnRowCommand="gvParallelOrgnHierarchyUser_RowCommand">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <Columns>

                                <asp:TemplateField HeaderText="Location Name" HeaderStyle-Width="250px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLocationName" runat="server" Text='<%# Eval("LocationName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Location Code" HeaderStyle-Width="250px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLocationCode" runat="server" Text='<%# Eval("LocationCode") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Hierarchy Level" HeaderStyle-Width="250px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHierarchyLevelName" runat="server" Text='<%# Eval("HierarchyLevelName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="User Name" HeaderStyle-Width="250px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLoginName" runat="server" Text='<%# Eval("LoginName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Email" HeaderStyle-Width="250px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Mobile No." HeaderStyle-Width="250px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMobileNumber" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <%-- <asp:ImageButton CommandArgument='<%#Eval("ParallelOrgnhierarchyID") %>' runat="server" ID="btnEdit"
                                                                    ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' OnClick="btnEdit_Click"
                                                                    ToolTip="Edit" />--%>
                                        <asp:ImageButton runat="server" ID="btnEdit" CommandArgument='<%# Container.DataItemIndex+1 %>' CommandName="Edit"
                                            ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' ToolTip="Edit" />


                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <%--  <asp:ImageButton ID="btnActiveDeactive"
                                                                    runat="server" CommandArgument='<%#Eval("ParallelOrgnhierarchyID") %>' CommandName='<%#Eval("Status")%>'
                                                                    OnClick="btnActiveDeactive_Click"--%>
                                        <asp:ImageButton ID="btnActiveDeactive"
                                            runat="server" CommandArgument='<%# Container.DataItemIndex+1 %>' CommandName="Active"
                                            ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />

                                        <asp:HiddenField ID="hdnParallelOrgnHierarchy" runat="server" Value='<%#Eval("ParallelOrgnhierarchyID") %>' />
                                        <asp:HiddenField ID="hdnHierarchyLevelID" runat="server" Value='<%#Eval("HierarchyLevelID") %>' />
                                        <asp:HiddenField ID="hdnStatus" runat="server" Value='<%#Eval("Status") %>' />
                                        <asp:HiddenField ID="hdnFirstName" runat="server" Value='<%#Eval("FirstName") %>' />

                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                        <div class="clear">
                        </div>

                        <div id="dvFooter" runat="server" class="pagination">
                            <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <%-- <asp:AsyncPostBackTrigger ControlID="gvParallelOrgnHierarchyUser" EventName="DataBound" />--%>

                        <asp:PostBackTrigger ControlID="btnShow" />
                        <asp:PostBackTrigger ControlID="btnSearch" />
                        <asp:AsyncPostBackTrigger ControlID="gvParallelOrgnHierarchyUser" EventName="RowEditing" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
