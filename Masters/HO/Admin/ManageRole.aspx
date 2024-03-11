<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" EnableEventValidation="false" CodeFile="ManageRole.aspx.cs"
    Inherits="Masters_HO_Admin_ManageRole" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>

        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMsg" runat="server" />
            </ContentTemplate>
            <%--#CC01 Add Start --%>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnCreateRole" />
                <asp:PostBackTrigger ControlID="btnSearchUser" />
                <asp:PostBackTrigger ControlID="btnShow" />
                <asp:AsyncPostBackTrigger ControlID="grdvwRoleList" EventName="RowEditing" />
            </Triggers>
            <%--#CC01 Add End --%>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="mainheading">
                    Add / Edit Role
                </div>
                <div class="contentbox">
                    <div class="mandatory">
                        (*) Marked fields are mandatory            
                    </div>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblRoleName" runat="server" AssociatedControlID="txtRoleName" CssClass="formtext">Role Name:</asp:Label>
                                <span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtRoleName" runat="server" CssClass="formfields" MaxLength="30"
                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqvaltxtRoleName" runat="server" ControlToValidate="txtRoleName"
                                    CssClass="error" Display="Dynamic" SetFocusOnError="true" ErrorMessage="Please enter role name."
                                    ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator><br />
                                <asp:RegularExpressionValidator ID="regExpRoleName" ControlToValidate="txtRoleName" CssClass="error" Display="Dynamic"
                                    ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,30}" ValidationGroup="AddUserValidationGroup"
                                    runat="server" />
                            </li>
                            <li class="text">
                                <asp:Label ID="lblHierarchyLevel" runat="server" AssociatedControlID="ddlHierarchyLevel" CssClass="formtext">Entity Type:</asp:Label>
                                <span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList CausesValidation="true" ID="ddlHierarchyLevel" runat="server"
                                    CssClass="formselect" AutoPostBack="True">
                                    <%-- OnSelectedIndexChanged="ddlHierarchyLevel_SelectedIndexChanged">--%>
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="ReqVHierarchyLevel" runat="server" ControlToValidate="ddlHierarchyLevel"
                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select Entity Type."
                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text">
                                <asp:Label ID="Label1" runat="server" AssociatedControlID="chkActive" CssClass="formtext"> Status:</asp:Label>
                            </li>
                            <li class="field">
                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                            </li>
                            <%--<td valign="top" align="right" width="15%">
                                                                                <asp:Label ID="lblSalesChanel" runat="server" AssociatedControlID="ddlSalesChanelType">Sales Chanel Type:</asp:Label>
                                                                            </td>
                                                                            <td width="1%" align="left" class="mandatory" valign="top">*</td>
                                                                            <td align="left" valign="top">
                                                                                <asp:DropDownList CausesValidation="true" ID="ddlSalesChanelType" runat="server"
                                                                                    CssClass="form_select5" AutoPostBack="True"
                                                                                    OnSelectedIndexChanged="ddlSalesChanelType_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                                <br />
                                                                                <%--<asp:RequiredFieldValidator ID="ReqUserGroup" runat="server" ControlToValidate="ddlSalesChanelType"
                                                                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales chanel."
                                                                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                                                            </td>--%>
                        </ul>
                        <ul>
                            <%--<td align="right" valign="top">Other Entity Type</td>
                                                                            <td align="left" class="mandatory" valign="top">*</td>
                                                                            <td align="left" valign="top">
                                                                                <asp:DropDownList ID="ddlOtherentityType" runat="server"
                                                                                    CausesValidation="true" CssClass="form_select4" AutoPostBack="True"
                                                                                    OnSelectedIndexChanged="ddlOtherentityType_SelectedIndexChanged">
                                                                                    <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                                                                    <asp:ListItem Value="1" Text="Retailer"></asp:ListItem>
                                                                                    <asp:ListItem Value="2" Text="ISD"></asp:ListItem>
                                                                                    <asp:ListItem Value="4" Text="ParallelOrgnHierarchy"></asp:ListItem>
                                                                                    <%--#CC01 Added
                                                                                </asp:DropDownList>
                                                                            </td>--%>

                            <%-- <td valign="top" align="right">
                                                                                <asp:Label ID="lblRoleStatus" runat="server" AssociatedControlID="chkActive"> Status :</asp:Label>
                                                                            </td>
                                                                            <td align="left" valign="top">
                                                                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                                                            </td>--%>
                            <li class="text">WAP Access: </li>
                            <li class="field">
                                <asp:CheckBox ID="chkWAPAccess" runat="server" />
                            </li>
                            <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnCreateRole" Text="Submit" runat="server" CausesValidation="true"
                                        ValidationGroup="AddUserValidationGroup" ToolTip="Add Role" CssClass="buttonbg"
                                        OnClick="btnCreateRole_Click" />
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
        </asp:UpdatePanel>
        <div class="mainheading">
            Search
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="H20-C3-S">
                        <ul>
                            <li class="text">Role Name:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtRoleSearch" runat="server" MaxLength="20" CssClass="formfields"></asp:TextBox>
                            </li>
                            <li class="text">Entity Type:
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlHierarchLevelSearch" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <%--  <td align="left" valign="top" width="10%" class="formtext">
                                                                    Sales Channel:
                                                                </td>
                                                               <td align="left" valign="top" width="25%">
                                                                    <asp:DropDownList ID="ddlSalesChanelSearch" runat="server" CssClass="form_select2">
                                                                       
                                                                    </asp:DropDownList>
                                                                </td>--%>
                            <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnSearchUser" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                        OnClick="btnSearchUser_Click"></asp:Button>
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnShow" runat="server" CssClass="buttonbg" Text="Show All" ToolTip="Search"
                                        OnClick="btnShow_Click" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="mainheading">
            Role List
        </div>
        <div class="export">
            <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" OnClick="btnExprtToExcel_Click"
                CausesValidation="False" />
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="grdvwRoleList" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                            RowStyle-HorizontalAlign="left" EmptyDataText="No Record found"
                            RowStyle-VerticalAlign="top" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" GridLines="none" AlternatingRowStyle-CssClass="Altrow"
                            RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                            CellSpacing="1" CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="false"
                            AllowPaging="True" PageSize='<%$ AppSettings:GridViewPageSize %>'
                            SelectedStyle-CssClass="gridselected" OnPageIndexChanging="grdvwRoleList_PageIndexChanging"
                            DataKeyNames="RoleID" OnRowDataBound="grdvwRoleList_RowDataBound">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="RoleName"
                                    HeaderText="Role Name"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="HierarchyLevelName"
                                    HeaderText="Entity Type" NullDisplayText="N/A"></asp:BoundField>
                                <%--<asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelTypeName"
                                                            HeaderText="Sales Channel Type" NullDisplayText="N/A"></asp:BoundField>

                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OtherEntityTypeName"
                                                            HeaderText="Other Entity Type" NullDisplayText="N/A"></asp:BoundField>--%>

                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="HasWAPAccess"
                                    HeaderText="Has WAP Access" NullDisplayText="N/A"></asp:BoundField>

                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%#Eval("RoleID") %>' runat="server" ID="btnEdit"
                                            ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' ToolTip="Edit User"
                                            OnClick="btnEdit_Click" />
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
                    </ContentTemplate>
                    <Triggers>
                        <%--      <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />--%>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
