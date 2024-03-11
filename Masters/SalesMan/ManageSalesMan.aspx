<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageSalesMan.aspx.cs" Inherits="Masters_SalesMan_ManageSalesMan" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="mainheading">
                Add / Edit Salesman
            </div>
            <div class="contentbox">
                <div class="mandatory">
                    (*) Marked fields are mandatory. (+) Fill at least one of them.
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Sales Channel: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlSalesChannel" CssClass="formselect" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="error"
                                    ControlToValidate="ddlSalesChannel" ErrorMessage="Please Select Sales Channel."
                                    SetFocusOnError="True" ValidationGroup="AddUserValidationGroup" InitialValue="0"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblSalesMan" runat="server" AssociatedControlID="txtSalesManName">Salesman Name :
                            </asp:Label>
                            <span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtSalesManName" runat="server" CssClass="formfields" MaxLength="70"
                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>
                            </div>
                            <asp:Label Style="display: none;" runat="server" ID="lblddlCheck" CssClass="error"></asp:Label>
                            <asp:RequiredFieldValidator ID="reqSalesMan" runat="server" ControlToValidate="txtSalesManName"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Salesman Name."
                                SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>

                        </li>

                        <%-- #CC02 Comment Start
                                                                               <td class="formtext" valign="top" align="right" width="11%">
                                                                                <asp:Label ID="lblSalesManCode" runat="server" AssociatedControlID="txtSalesManCode"
                                                                                    CssClass="formtext">Salesman Code:</asp:Label>
                                                                            </td>
                                                                            <td width="1%" align="left" valign="top" class="error"></td>
                                                                            <td valign="top" align="left" colspan="2">
                                                                                <asp:TextBox ID="txtSalesManCode" runat="server" CssClass="form_input2" MaxLength="20"
                                                                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>

                                                                            </td>
                                                                               #CC02 Comment End
                        --%>
                    </ul>
                </div>
                <div class="clear"></div>
                <div class="H35-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblAddress" runat="server" AssociatedControlID="txtAddress" CssClass="formtext">Address:</asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" CssClass="form_textarea"></asp:TextBox>
                            </div>
                            <div>
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="fncChkSize"
                                    ControlToValidate="txtAddress" CssClass="error" ValidationGroup="AddUserValidationGroup" Display="Dynamic" ErrorMessage="Address1 should not be greater than 250"
                                    ForeColor="">
                                </asp:CustomValidator>
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblMobile" runat="server" AssociatedControlID="txtMobile" CssClass="formtext">Mobile No:</asp:Label>
                            <span class="error">+</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="formfields" MaxLength="10"
                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>
                            </div>
                            <div>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtMobile"
                                    ValidChars="0123456789+-().,/">
                                </cc1:FilteredTextBoxExtender>
                                <%--#CC03 Add Start --%>
                                <asp:RegularExpressionValidator ID="regexValidatorMobileNo" runat="server" ControlToValidate="txtMobile"
                                    ValidationExpression="^[1-9]([0-9]{9})$" ValidationGroup="AddUserValidationGroup" ErrorMessage="Please enter 10 digit number without 0 prefix."
                                    CssClass="error" Display="Dynamic"></asp:RegularExpressionValidator>
                                <%--#CC03 Add End --%>
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblEmailid" runat="server" CssClass="formtext">EmailID:</asp:Label>
                            <span class="error">+</span>
                        </li>
                        <%--#CC01 Add Start--%>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtemail" runat="server" CssClass="formfields" MaxLength="80"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularEmail" runat="server" ControlToValidate="txtemail"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter valid emailid." ValidationGroup="AddUserValidationGroup"
                                    ForeColor="" meta:resourcekey="RegularEmail" SetFocusOnError="true" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </div>
                        </li>
                        <%--#CC01 Add End--%>
                    </ul>
                    <ul id="tdlogindetails" runat="server">
                        <li class="text">
                            <asp:Label ID="lblusername" runat="server" CssClass="formtext">UserName:</asp:Label></td>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtUserName" runat="server" CssClass="form_input2" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqUserName" runat="server" ControlToValidate="txtUserName"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please Enter User Name." SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblpassword" runat="server" CssClass="formtext">Password:</asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtpassword" runat="server" CssClass="form_input2" MaxLength="20"
                                TextMode="Password" ValidationGroup="Add1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqpassword" runat="server" ControlToValidate="txtpassword"
                                CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Password." SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblStatus" runat="server" AssociatedControlID="chkActive" CssClass="formtext">Status :</asp:Label>
                        </li>
                        <li class="field">
                            <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnCreateUser" Text="Submit" runat="server" CausesValidation="true"
                                    ValidationGroup="AddUserValidationGroup" OnClick="btnCreateUser_Click" ToolTip="Submit"
                                    CssClass="buttonbg" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                    OnClick="btnCancel_Click" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="gvSalesMan" EventName="DataBound" /> #CC05 Commented --%>
            <asp:PostBackTrigger ControlID="btnCreateUser" />
            <%-- #CC01 Added--%>
        </Triggers>
    </asp:UpdatePanel>

    <div class="mainheading">
        Search
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Salesman Name:
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSalesManSearch" runat="server" MaxLength="70" CssClass="formfields">
                            </asp:TextBox>
                        </li>
                        <%-- <td align="right" valign="top" width="60" height="25" class="formtext">
                                                                    User Role:&nbsp;
                                                                </td>--%>
                        <li class="text">Salesman Code:
                        </li>

                        <li class="field">
                            <asp:TextBox ID="txtSalesManCodeSearch" runat="server" CssClass="formfields" MaxLength="20">
                            </asp:TextBox>
                        </li>

                        <%--#CC06 Add Start --%>
                        <li class="text">SalesChannel Code:
                        </li>

                        <li class="field">
                            <asp:TextBox ID="txtSaleChannelCodeSearch" runat="server" CssClass="formfields" MaxLength="20">
                            </asp:TextBox>
                        </li>
                        <li class="text">SalesMan Mobile Number:
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSalesManMobileNumberSearch" runat="server" CssClass="formfields" MaxLength="10">
                            </asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="fltrExtenderSalesmanMobileNumber" runat="server" TargetControlID="txtSalesManMobileNumberSearch"
                                ValidChars="0123456789+-().,/">
                            </cc1:FilteredTextBoxExtender>

                            <asp:RegularExpressionValidator ID="regexSalesManMobileSearch" runat="server" ControlToValidate="txtSalesManMobileNumberSearch"
                                ValidationExpression="^[1-9]([0-9]{9})$" ErrorMessage="Please enter 10 digit number without 0 prefix."
                                ValidationGroup="SearchSalesman" CssClass="error" Display="Dynamic"></asp:RegularExpressionValidator>

                        </li>
                        <li class="text">Email ID:
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtEmailIDSearch" runat="server" CssClass="formfields" MaxLength="80">
                            </asp:TextBox>
                            <div>
                                <asp:RegularExpressionValidator ID="regexEmailSearch" runat="server" ControlToValidate="txtEmailIDSearch"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter valid emailid." ValidationGroup="SearchSalesman"
                                    ForeColor="" meta:resourcekey="RegularEmail" SetFocusOnError="true" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </div>
                        </li>
                        <li class="text"></li>
                        <%--#CC06 Add End --%>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSearchUser" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                    OnClick="btnSearchUser_Click" ValidationGroup="SearchSalesman"></asp:Button>
                                <%--#CC06 Validation group added and causevalidation="False" removed--%>
                            </div>
                            <div class="float-left">
                                <asp:Button ID="btnShow" runat="server" CssClass="buttonbg" Text="Show All" ToolTip="Show All"
                                    OnClick="btnShow_Click" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="mainheading">
        Salesman List
    </div>
    <div class="export">
        <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" CausesValidation="False"
            OnClick="btnExprtToExcel_Click" />
    </div>
    <div class="contentbox">
        <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
            <ContentTemplate>
                <%--#CC04 START COMMENTED AllowPaging="True" OnPageIndexChanging="gvSalesMan_PageIndexChanging"  #CC04 END COMMENTED--%>
                <div class="grid1">
                    <asp:GridView ID="gvSalesMan" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                        RowStyle-HorizontalAlign="left" EmptyDataText="No Record Found" RowStyle-VerticalAlign="top"
                        HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                        AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                        HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                        BorderWidth="0px" Width="100%" AutoGenerateColumns="false"
                        PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected"
                        DataKeyNames="SalesmanID"
                        OnRowDataBound="gvSalesMan_RowDataBound">
                        <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                        <Columns>
                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelCode"
                                HeaderText="Sales Channel Code"></asp:BoundField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Sales Channel Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; overflow: hidden; width: 150px;">
                                        <%# DataBinder.Eval(Container.DataItem, "SalesChannelName")%>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>



                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Salesman Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; overflow: hidden; width: 150px;">
                                        <%# DataBinder.Eval(Container.DataItem, "SalesmanName")%>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Login Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; overflow: hidden; width: 150px;">
                                        <%# DataBinder.Eval(Container.DataItem, "LoginName")%>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
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

                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesmanCode"
                                HeaderText="Salesman Code" NullDisplayText="N/A"></asp:BoundField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Address
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="word-wrap: break-word; overflow: hidden; width: 150px;">
                                        <%# (Eval("Address")==null || Eval("Address")=="") ? "N/A" :Eval("Address")%>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <%-- <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelName"
                                                            HeaderText="Sales Channel Name"></asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesmanName"
                                                            HeaderText="Salesman Name"></asp:BoundField>--%>
                            <%-- <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Address"
                                                            HeaderText="Address" NullDisplayText="N/A"></asp:BoundField>--%>
                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="MobileNumber"
                                HeaderText="Mobile Number" NullDisplayText="N/A"></asp:BoundField>
                            <%--#CC02 Add Start --%>
                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Email"
                                HeaderText="Email ID"></asp:BoundField>
                            <%--#CC02 Add End --%>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False"></ItemStyle>
                                <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%#Eval("SalesmanID") %>' runat="server" ID="btnEdit"
                                        ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' OnClick="btnEdit_Click"
                                        ToolTip="Edit User" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False"></ItemStyle>
                                <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnActiveDeactive" OnClick="btnActiveDeactive_Click" runat="server"
                                        CommandArgument='<%#Eval("SalesmanID") %>' CommandName='<%#Eval("Status")%>'
                                        ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>' ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                    <%--#CC04 START ADDED--%>
                    <div class="clear">
                    </div>
                </div>
                <div id="dvFooter" runat="server" class="pagination">
                    <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                </div>

                <%--#CC04 END ADDED--%>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                <asp:PostBackTrigger ControlID="gvSalesMan" />
                <%--#CC05 Added --%>
                <%--<asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />--%> <%--#CC01 Commented--%>
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <%--     <uc3:footer ID="Footer1" runat="server" />--%>
</asp:Content>
