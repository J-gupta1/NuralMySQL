<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="ManageEntityTypeMaster.aspx.cs" Inherits="Masters_Common_ManageEntityTypeMaster" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucStatusControl.ascx" TagName="ucStatus" TagPrefix="uc5" %>
<%@ Register Src="~/UserControls/ucTimePicker.ascx" TagName="ucTimePicker" TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .grid22 {
            width: 100%;
            height: auto;
            max-height: 100px;
            overflow: auto;
            background-color: #dddcdc;
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
    <asp:UpdatePanel ID="updpnlSaveData" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="box1">
                <asp:UpdatePanel ID="UpdateMessage" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div>
                            <uc2:ucMessage ID="ucMessage1" runat="server" />

                        </div>
                        <div class="subheading">
                            <div class="float-left">
                                Add Entity Type
                            </div>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="innerarea">
                    <div class="H25-C3-S">

                        <ul>
                            <li class="text">Entity Type:<span class="error">*</span></li>
                            <li class="field">
                                <asp:TextBox ID="txtEntityType" runat="server" CssClass="formfields" MaxLength="30"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqfldtxtEntityType" runat="server" ControlToValidate="txtEntityType"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please Enter Entity Type." ForeColor=""
                                    ValidationGroup="grpv"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regexptxtEntityType" runat="server" ControlToValidate="txtEntityType"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Invalid Chars" ValidationExpression="^([a-zA-Z]+(_[a-zA-Z]+)*)(\s([a-zA-Z]+(_[a-zA-Z]+)*))*$"
                                    ValidationGroup="grpv"></asp:RegularExpressionValidator>
                            </li>
                            <li class="text">Base Entity Type:<span class="error">*</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlBaseEntityType" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="regfldddlBaseEntityType" runat="server" ControlToValidate="ddlBaseEntityType"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Base Entity Type" ForeColor=""
                                    ValidationGroup="grpv" InitialValue="100"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Auto Code Mode:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlSAPCodeMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqSAPCodeMode" runat="server" ControlToValidate="ddlSAPCodeMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select SAP Code Mode." InitialValue="100"
                                    ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">SalesChannel Level:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlSalesChannelLavel" runat="server" CssClass="formselect">
                                </asp:DropDownList>


                            </li>
                            <li class="text">Report Hierarchy Level:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlReportHierarchyLevel" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                            </li>
                            <li class="text">Bill to Retailer:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlBilltoretailer" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="text">Stock Transfer Mode:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlStockTransfermode" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFStockTransfermode" runat="server" ControlToValidate="ddlStockTransfermode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Stock Transfer Mode Required."
                                    InitialValue="100" ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Target Mode:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlTargetMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqfldTargetMode" runat="server" ControlToValidate="ddlTargetMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Target Mode." InitialValue="100"
                                    ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">App Role TypeID:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlAppRoleTypeID" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                            </li>
                            <li class="text">Stock Maintain Mode:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlStockMaintainMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFddlStockMaintainMode" runat="server" ControlToValidate="ddlStockMaintainMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="StockMaintainMode Required."
                                    InitialValue="100" ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">BackDays Allowed For Sale Sale:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtNumberofBackDays" runat="server" CssClass="formfields"></asp:TextBox>
                                <cc2:FilteredTextBoxExtender ID="ftbeOs" runat="server" FilterType="Numbers" ValidChars="0123456789"
                                    TargetControlID="txtNumberofBackDays" />
                            </li>
                            <li class="text">BackDays Allowed For SaleReturn:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtBackDaysAllowedForSaleReturn" runat="server" CssClass="formfields"></asp:TextBox>
                                <cc2:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" ValidChars="0123456789"
                                    TargetControlID="txtBackDaysAllowedForSaleReturn" />
                            </li>
                            <li class="text">Show In App:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlShowInApp" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFddlShowInApp" runat="server" ControlToValidate="ddlShowInApp"
                                    CssClass="error" Display="Dynamic" ErrorMessage="ShowInApp Required."
                                    InitialValue="100" ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            <%--<li class="text">Brand Category Mapping Mode:<span class="error">*</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlBrandCategoryMappingMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                                <div>
                                    <asp:RequiredFieldValidator ID="reqfldddlBrandCategoryMappingMode" runat="server"
                                        ControlToValidate="ddlBrandCategoryMappingMode" CssClass="error" Display="Dynamic"
                                        ErrorMessage="Select Brand Category Mapping Mode." InitialValue="100" ValidationGroup="grpv"></asp:RequiredFieldValidator>
                                </div>
                            </li>
                            <li class="text">Group Mapping Mode:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlGroupMappingMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqfldddlGroupMappingMode" runat="server" ControlToValidate="ddlGroupMappingMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Group Mapping Mode."
                                    InitialValue="100" ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Weekly Off Mode:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlWeeklyOffMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                                <div>
                                    <asp:RequiredFieldValidator ID="reqfldWeeklyOffMode" runat="server" ControlToValidate="ddlWeeklyOffMode"
                                        CssClass="error" Display="Dynamic" ErrorMessage="Select Weekly Off Mode." InitialValue="100"
                                        ValidationGroup="grpv"></asp:RequiredFieldValidator>
                                </div>
                            </li>
                            <li class="text">Entity Contact Mode:<span class="error">*</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlEntityContactMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqfldddlEntityContactMode" runat="server" ControlToValidate="ddlEntityContactMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Entity Contact Mode."
                                    ForeColor="" ValidationGroup="grpv" InitialValue="100"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Entity Detail Mode:<span class="error">*</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlEntityDetailMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqfldddlEntityDetailMode" runat="server" ControlToValidate="ddlEntityDetailMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Entity Detail Mode."
                                    InitialValue="100" ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Application Working Mode:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlApplicationWorkingMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqfldApplicationWorkingMode" runat="server" ControlToValidate="ddlApplicationWorkingMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Application Working Mode."
                                    InitialValue="100" ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Entity Statutory Mode:<span class="error">*</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlEntityStatutoryMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqfldEntityStatutoryMode" runat="server" ControlToValidate="ddlEntityStatutoryMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Entity Statutory Mode."
                                    ForeColor="" ValidationGroup="grpv" InitialValue="100"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Entity Bank Mode:<span class="error">*</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlEntityBankMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqfldddlEntityBankMode" runat="server" ControlToValidate="ddlEntityBankMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Entity Bank Mode." InitialValue="100"
                                    ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Access Type:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlAccessType" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqfldddlAccessType" runat="server" ControlToValidate="ddlAccessType"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Access Type." InitialValue="100"
                                    ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Credit Term Mode:<span class="error">*</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlCreditTermMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqfldddlCreditTermMode" runat="server" ControlToValidate="ddlCreditTermMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Credit Term Mode." ForeColor=""
                                    ValidationGroup="grpv" InitialValue="100"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Journal Mode:<span class="error">*</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlJournalMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqfldJournalMode" runat="server" ControlToValidate="ddlJournalMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Journal Mode." InitialValue="100"
                                    ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            
                            <li class="text">Service Customer Mode:<span class="error">*</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlServiceCustomerMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqfldServiceCustomerMode" runat="server" ControlToValidate="ddlServiceCustomerMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Service Customer Mode."
                                    ForeColor="" ValidationGroup="grpv" InitialValue="100"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Service PJPMode:<span class="error">*</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlServicePJPMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqServicePJPMode" runat="server" ControlToValidate="ddlServicePJPMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Service PJPMode." InitialValue="100"
                                    ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            
                            <li class="text">Authorise For LeadPass: <span class="error">*</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlAuthoriseForLeadPass" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqfldAuthoriseForLeadPass" runat="server" ControlToValidate="ddlAuthoriseForLeadPass"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Authorise For LeadPass."
                                    InitialValue="100" ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">City Mapping Mode:<span class="error">*</span> </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlCityMappingMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqfldCityMappingMode" runat="server" ControlToValidate="ddlCityMappingMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select City Mapping Mode." InitialValue="100"
                                    ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Entity Mil Mode:<span class="error">*</span> </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlEntityMilMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqfldEntityMilMode" runat="server" ControlToValidate="ddlEntityMilMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Entity Mil Mode." InitialValue="100"
                                    ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Price Group Mode:<span class="error">*</span> </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlPriceGroupMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqfldPriceGroupMode" runat="server" ControlToValidate="ddlPriceGroupMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Price Group Mode." InitialValue="100"
                                    ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Entity Address Mode:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlEntityAddressMode" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="reqfldEntityAddressMode" runat="server" ControlToValidate="ddlEntityAddressMode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Select Entity Address Mode."
                                    InitialValue="100" ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Holiday Calendar Required:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlCalender" runat="server" CssClass="formselect">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="RFddlCalender" runat="server" ControlToValidate="ddlCalender"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Holiday Calendar Required."
                                    InitialValue="100" ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            
                            <li class="text">Is Pan Mandatory:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlIsPanMandatory" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFIsPanMandatory" runat="server" ControlToValidate="ddlIsPanMandatory"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Is Pan Mandatory Required."
                                    InitialValue="100" ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>--%>


                            <li class="text">
                                <div class="float-margin">
                                    Active:
                                </div>
                                <div class="float">
                                    <asp:CheckBox ID="chkActive" runat="server" CssClass="CheckBoxList2" Checked="True" />
                                </div>
                            </li>

                        </ul>
                        <div class="clear"></div>
                        <ul>
                            <li class="text">Reporting Organization Hierarchy:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlParentHierarchyType" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="reqReportingHierarchy" runat="server" ControlToValidate="ddlParentHierarchyType"
                                    CssClass="error" Display="Dynamic" ErrorMessage="StockMaintainMode Required."
                                    InitialValue="100" ValidationGroup="grpv"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">Working From Time:</li>
                            <li class="field">
                                <uc6:ucTimePicker ID="ucInTime" runat="server" CssClass="formselect" RangeErrorMessage="Invalid time!"
                                    IsRequired="true" ValidationGroup="Add" ErrorMessage="Please enter in-time!" />
                            </li>

                            <li class="text">Working To Time:</li>
                            <li class="field">
                                <uc6:ucTimePicker ID="ucOutTime" runat="server" CssClass="formselect" RangeErrorMessage="Invalid time!"
                                    IsRequired="true" ValidationGroup="Add" ErrorMessage="Please enter out-time!" />
                            </li>

                        </ul>
                        <div class="clear"></div>
                        <ul>
                            <li class="text">Parent Sales channel:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <div class="" style="overflow: auto">
                                    <asp:CheckBoxList ID="chkParentSalesChannelType" runat="server" RepeatColumns="1" RepeatDirection="Vertical" RepeatLayout="Flow"></asp:CheckBoxList>
                                </div>
                            </li>
                            <li class="text">Stock Transfer Mapping:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <div class="" style="overflow: auto">
                                    <asp:CheckBoxList ID="chkStockTransferType" runat="server" RepeatColumns="1" RepeatDirection="Vertical" RepeatLayout="Flow"></asp:CheckBoxList>
                                </div>
                            </li>
                        </ul>
 <%--                      <ul>
                            <li class="text"></li>
                        </ul>
                        <ul>
                            <li class="text"></li>
                        </ul>
                        <ul>
                            <li class="text"></li>
                        </ul>
                        <ul>
                            <li class="text"></li>
                        </ul>
                        <ul>
                            <li class="text"></li>
                        </ul>
                        <ul>
                            <li class="text"></li>
                        </ul>
                         <div class="clear"></div>
                        <div class="clear"></div>
                     --%>
                        <div class="clear"></div>
                        <div class="box1">
                            <div class="subheading">
                                Role
                            </div>
                            <div class="innerarea">

                                <%--<div class="H25-C3-S">--%>
                                <ul>
                                    <li class="text">Role name:
                                    </li>
                                    <li class="field">
                                        <asp:TextBox ID="txtRole" runat="server" CssClass="formfields" MaxLength="50"></asp:TextBox>

                                    </li>
                                    <li class="field">
                                        <div class="float-margin">
                                            <asp:Button ID="btnCreateRole" Text="Add Role" runat="server" CausesValidation="true"
                                                ValidationGroup="AddUserValidationGroup" ToolTip="Add Role" CssClass="buttonbg"
                                                OnClick="btnCreateRole_Click" />
                                        </div>
                                    </li>
                                </ul>

                                <div class="innerarea">

                                    <div class="grid1">

                                        <asp:GridView ID="grdvwRoleList" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                                            RowStyle-HorizontalAlign="left" EmptyDataText="No Record found"
                                            RowStyle-VerticalAlign="top" HeaderStyle-HorizontalAlign="left"
                                            HeaderStyle-VerticalAlign="top" GridLines="none" AlternatingRowStyle-CssClass="Altrow"
                                            RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                                            CellSpacing="1" CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="false"
                                            AllowPaging="True" PageSize='<%$ AppSettings:GridViewPageSize %>'
                                            SelectedStyle-CssClass="gridselected"
                                            DataKeyNames="RoleID" OnRowDataBound="grdvwRoleList_RowDataBound">
                                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                            <Columns>
                                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="RoleName"
                                                    HeaderText="Role Name"></asp:BoundField>
                                                <%-- <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="HierarchyLevelName"
                                                    HeaderText="Entity Type" NullDisplayText="N/A"></asp:BoundField>--%>

                                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton CommandArgument='<%#Eval("RoleID") %>' runat="server" ID="btnEditRole"
                                                            ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' ToolTip="Edit Role"
                                                            OnClick="btnEditRole_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                                    <ItemStyle Wrap="False"></ItemStyle>
                                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnActiveDeactive" OnClick="btnActiveDeactive_Click" runat="server"
                                                            CommandArgument='<%#Eval("RoleID") %>' CommandName='<%#Eval("Status")%>' ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="PagerStyle" />
                                        </asp:GridView>

                                    </div>
                                </div>

                            </div>
                        </div>
                        <ul>
                            <li class="field">
                                <div class="float-margin">

                                    <asp:Button ID="btnSave" runat="server" CssClass="buttonbg" Text="Save" OnClick="btnSave_Click" ValidationGroup="grpv" />
                                </div>
                                <div class="float-left">

                                    <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" OnClick="btnCancel_Click" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="box1">
        <div class="subheading">
            Search Entity Type
        </div>
        <div class="innerarea">

            <div class="H25-C3-S">

                <ul>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <li class="text">Entity Type:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:TextBox ID="txtSearchEntityType" runat="server" CssClass="formselect"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegFldtxtSearchEntityType" runat="server" ControlToValidate="txtSearchEntityType"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Invalid Chars" ValidationExpression="^([a-zA-Z]+(_[a-zA-Z]+)*)(\s([a-zA-Z]+(_[a-zA-Z]+)*))*$"
                                    ValidationGroup="grpv"></asp:RegularExpressionValidator>
                            </li>
                            <li class="text">Base Entity Type:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlsearchBaseEntityType" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li>
                                <uc5:ucStatus ID="ddlActive" runat="server" />
                             

</ul>


                               <div class="setbbb">
                                    <asp:Button ID="btnSearch" runat="server" CssClass="buttonbg" Text="Search" OnClick="btnSearch_Click" CausesValidation="false" />
                                    <asp:Button ID="btnViewAll" runat="server" CssClass="buttonbg" Text="ViewAll" OnClick="btnViewAll_Click" CausesValidation="false" />

                                </div>
                       
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ul>

            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="updpnlgrdvList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdfCurrentPage" runat="server" Value="1" />
            <div class="box1" runat="server" visible="true" id="dvGrid">
                <div class="subheading">
                    <div class="float-left">
                        Entity Type List
                    </div>
                </div>
                <div class="export">
                    <asp:Button ID="Exporttoexcel" runat="server"
                        OnClick="Exporttoexcel_Click" CausesValidation="False" CssClass="excel" />
                </div>
                <div class="innerarea">

                    <div class="grid1">

                        <asp:GridView ID="grdvList" runat="server" AutoGenerateColumns="False" HeaderStyle-CssClass="gridheader"
                            SelectedRowStyle-CssClass="selectedrow" RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow"
                            EditRowStyle-CssClass="editrow" FooterStyle-CssClass="gridfooter" CellSpacing="1"
                            CellPadding="4" DataKeyNames="EntityTypeID" EmptyDataText="No Record Found" GridLines="None"
                            OnRowCommand="grdvList_RowCommand" OnRowDataBound="grdvList_RowDataBound" Width="100%">
                            <Columns>
                                <asp:ButtonField CausesValidation="false" CommandName="Select" Text="Select" Visible="false"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="EntityTypeID" HeaderText="Entity Type ID" HeaderStyle-HorizontalAlign="Left"
                                    Visible="false" />
                                <asp:BoundField DataField="EntityType" HeaderText="Entity Type" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ActiveStatus" HeaderText="Active" HeaderStyle-HorizontalAlign="Left" />
                                <%--<asp:BoundField DataField="StockMaintainedBySystem" HeaderText="Stock Maintained BySystem"
                                    HeaderStyle-HorizontalAlign="Left" />--%>
                                <asp:BoundField DataField="BaseEntityTypeID" HeaderText="BaseEntityTypeID" HeaderStyle-HorizontalAlign="Left"
                                    Visible="false" />
                                <asp:BoundField DataField="BaseEntityTypeName" HeaderText="Base Entity TypeName"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="AutoCodeMode" HeaderText="Auto Code Mode"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Level" HeaderText="Level"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <%--<asp:BoundField DataField="GroupMappingMode" HeaderText="Group Mapping Mode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="WeeklyOffMode" HeaderText="Weekly Off Mode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="PriceGroupMode" HeaderText="Price Group Mode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="EntityAddressMode" HeaderText="Entity Address Mode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="EntityContactMode" HeaderText="Entity Contact Mode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="EntityDetailMode" HeaderText="Entity Detail Mode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ApplicationWorkingMode" HeaderText="Application WorkingMode"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="EntityStatutoryMode" HeaderText="Entity Statutory Mode"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="AccessType" HeaderText="Access Type" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="CreditTermMode" HeaderText="Credit Term Mode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="JournalMode" HeaderText="Journal Mode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="TargetMode" HeaderText="Target Mode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ServiceCustomerMode" HeaderText="Service Customer Mode"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ServicePJPMode" HeaderText="Service PJPMode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="SAPCodeMode" HeaderText="SAP CodeMode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="AuthoriseForLeadPass" HeaderText="Authorise ForLead Pass"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="CityMappingMode" HeaderText="City Mapping Mode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="EntityMilMode" HeaderText="Entity Mil Mode" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="HolidayCalendarRequired" HeaderText="Holiday Calendar Required"
                                    HeaderStyle-HorizontalAlign="Left" />--%>

                                <asp:TemplateField ShowHeader="False" HeaderText="Action" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div style="width: 65px;">
                                            <asp:Label ID="lblactive" Visible="false" runat="server" Text='<%# Eval("Active") %>'
                                                CommandArgument='<%# Eval("EntityTypeID") %>'></asp:Label>
                                            <asp:ImageButton ID="btnActive" runat="server"
                                                CommandName="activeEntityType" CommandArgument='<%# Eval("EntityTypeID") %>' ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Active"))) %>'
                                                CausesValidation="false"></asp:ImageButton>
                                            <asp:ImageButton ID="imgbtnEdit" runat="server" CommandArgument='<%# Eval("EntityTypeID") %>'
                                                CommandName="editEntityType" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' CausesValidation="false" />
                                            <asp:ImageButton ID="imgbtnDelete" runat="server" CommandArgument='<%# Eval("EntityTypeID") %>'
                                                CommandName="deleteEntityType" Visible="true" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/delete.png"%>'
                                                CausesValidation="false" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="dvFooter" runat="server" class="pagination">
                        <uc1:UCPagingControl ID="ucPagingControl1" runat="server" Visible="false" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Exporttoexcel" />
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click"></asp:AsyncPostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
