<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageMenu.aspx.cs" Inherits="Masters_HO_Admin_ManageMenu" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMsg" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="mainheading">
            Search
        </div>
        <div class="contentbox">
            <div class="mandatory">
                (*) Marked fields are mandatory            
            </div>
            <div class="H25-C3-S">
                <ul>
                    <li class="text">Menu Search:
                    </li>
                    <li class="field">
                        <asp:TextBox ID="txtKeyword" runat="server" CssClass="formfields"></asp:TextBox>
                    </li>
                    <li class="field3">
                        <div class="float-margin">
                            <asp:Button ID="btnSearch" CssClass="buttonbg" ToolTip="Search" runat="server" Text="Search"
                                OnClick="btnSearch_Click" />
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnViewAll" CssClass="buttonbg" ToolTip="View All" runat="server"
                                Text="View All" OnClick="btnViewAll_Click" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <cc1:TabContainer ID="TabMenu" runat="server" AutoPostBack="true" OnActiveTabChanged="TabMenu_ActiveTabChanged1"
            CssClass="ajax__tab_x">
            <cc1:TabPanel ID="CreateMenu" runat="server" TabIndex="0">
                <HeaderTemplate>
                    Create Menu
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="mainheading">
                        Menu
                    </div>
                    <div class="grid1">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DataList ID="dlstPrc" runat="server" AlternatingItemStyle-CssClass="Altrow"
                                    CellPadding="0" CellSpacing="0" DataKeyField="MenuID" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="None"
                                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Top"
                                    ItemStyle-CssClass="gridrow" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top"
                                    OnCancelCommand="dlstPrc_CancelCommand" OnEditCommand="dlstPrc_EditCommand" OnItemCommand="dlstPrc_ItemCommand"
                                    OnItemDataBound="dlstPrc_ItemDataBound" OnUpdateCommand="dlstPrc_UpdateCommand"
                                    RepeatColumns="1" RepeatDirection="Vertical" SelectedItemStyle-CssClass="tbl_selected"
                                    Width="100%">
                                    <SelectedItemStyle CssClass="tbl_selected" />
                                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <HeaderTemplate>
                                        <table border="0" cellpadding="4" cellspacing="0" width="100%" class="gridheader">
                                            <tr>
                                                <td align="left" valign="top" width="25%">
                                                    <div style="min-width: 200px">Menu Name</div>
                                                </td>
                                                <td align="left" valign="top" width="30%">
                                                    <div style="min-width: 200px">
                                                        Menu Description
                                                    </div>
                                                </td>
                                                <td align="left" valign="top" width="30%">
                                                    <div style="min-width: 50px">
                                                        Display Order
                                                    </div>
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <div style="width: 100px">
                                                        Actions
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" valign="top" width="25%">
                                                    <div style="min-width: 200px">
                                                        <asp:Label ID="lblMenuName2" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"MenuName").ToString())%>'
                                                            Visible="true"></asp:Label>
                                                    </div>
                                                </td>
                                                <td align="left" valign="top" width="30%">
                                                    <div style="min-width: 200px">
                                                        <asp:Label ID="lblDesc2" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"MenuDescription").ToString())%>'
                                                            Visible="true"></asp:Label>
                                                    </div>
                                                </td>
                                                <td align="left" valign="top" width="30%">
                                                    <div style="min-width: 50px">
                                                        <asp:Label ID="lblDO2" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"DisplayOrderNumber").ToString())%>'
                                                            Visible="true"></asp:Label>
                                                    </div>
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <div style="width: 100px">
                                                        <div class="float-margin">
                                                            <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandName="Edit"
                                                                ImageAlign="AbsMiddle" ToolTip="Edit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                                        </div>
                                                        <div class="float-margin">
                                                            <asp:ImageButton ID="imgActive2" runat="server" AlternateText="DeActive" CausesValidation="false"
                                                                CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MenuID") %>' CommandName="Active"
                                                                ImageAlign="AbsMiddle" ToolTip="Status" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>' />
                                                        </div>
                                                        <div class="float-margin">
                                                            <asp:ImageButton ID="ImgDelete2" runat="server" CausesValidation="False" CommandName="Delete"
                                                                ImageAlign="AbsMiddle" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/delete.png"%>'
                                                                ToolTip="Delete" />
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <AlternatingItemStyle CssClass="Altrow" />
                                    <ItemStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <EditItemTemplate>
                                        <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" valign="top" width="25%">
                                                    <div style="min-width: 200px">
                                                        <asp:TextBox ID="txtMenuE" runat="server" MaxLength="50" CssClass="formfields" Text='<%# (DataBinder.Eval(Container.DataItem,"MenuName").ToString())%>'></asp:TextBox>
                                                    </div>
                                                    <asp:RegularExpressionValidator ID="re2" runat="server" ControlToValidate="txtMenuE"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="Invalid Chars" ValidationExpression="^[a-zA-Z ]{1,50}"
                                                        ValidationGroup="updtgrpe"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="reqSearch" runat="server" ControlToValidate="txtMenuE"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="*" ValidationGroup="updtgrpe"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="30%">
                                                    <div style="min-width: 200px">
                                                        <asp:TextBox ID="txtDescE2" runat="server" MaxLength="100" CssClass="formfields"
                                                            Text='<%# (DataBinder.Eval(Container.DataItem,"MenuDescription").ToString())%>'></asp:TextBox>
                                                    </div>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtDescE2"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="Invalid Chars" ValidationExpression="^[a-zA-Z ]{1,255}"
                                                        ValidationGroup="updtgrpe"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDescE2"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="*" ValidationGroup="updtgrpe"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="30%">
                                                    <div style="min-width: 50px">
                                                        <asp:TextBox ID="txtDOE2" runat="server" MaxLength="2" CssClass="formfields" Text='<%# (DataBinder.Eval(Container.DataItem,"DisplayOrderNumber").ToString())%>'></asp:TextBox>
                                                    </div>
                                                    <cc1:FilteredTextBoxExtender ID="ftorder" runat="server" FilterMode="ValidChars"
                                                        FilterType="Custom" TargetControlID="txtDOE2" ValidChars="123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="reqSearch1" runat="server" ControlToValidate="txtDOE2"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="*" ValidationGroup="updtgrpe"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <div style="width: 100px">
                                                        <div class="float-margin">
                                                            <asp:ImageButton ID="UpdateState" runat="server" AlternateText="Update" CommandName="Update"
                                                                ImageAlign="AbsMiddle" ToolTip="Update" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/icon_update.gif"%>'
                                                                ValidationGroup="updtgrpe" />
                                                        </div>
                                                        <div class="float-margin">
                                                            <asp:ImageButton ID="CancelUpdate" runat="server" CausesValidation="false" CommandName="Cancel"
                                                                ImageAlign="AbsMiddle" ToolTip="Cancel" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/icon_cancel.gif"%>' />
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" valign="top" width="25%">
                                                    <div style="min-width: 200px">
                                                        <asp:TextBox ID="txtAdd" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>
                                                    </div>
                                                    <asp:RegularExpressionValidator ID="re2" runat="server" ControlToValidate="txtAdd"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="Invalid Chars" ValidationExpression="^[a-zA-Z ]{1,50}"
                                                        ValidationGroup="addf"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="reqSearch" runat="server" ControlToValidate="txtAdd"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="*" ValidationGroup="addf"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="30%">
                                                    <div style="min-width: 200px">
                                                        <asp:TextBox ID="txtDesc2" runat="server" MaxLength="100" CssClass="formfields"></asp:TextBox>
                                                    </div>
                                                    <asp:RegularExpressionValidator ID="re1" runat="server" ControlToValidate="txtDesc2"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="Invalid Chars" ValidationExpression="^[a-zA-Z0-9 ]{1,255}"
                                                        ValidationGroup="addf"></asp:RegularExpressionValidator><asp:RequiredFieldValidator
                                                            ID="reqSearch0" runat="server" ControlToValidate="txtDesc2" CssClass="error"
                                                            Display="Dynamic" ErrorMessage="*" ValidationGroup="addf"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="30%">
                                                    <div style="min-width: 50px">
                                                        <asp:TextBox ID="txtDO2" runat="server" MaxLength="2" CssClass="formfields"></asp:TextBox>
                                                    </div>
                                                    <cc1:FilteredTextBoxExtender ID="ftorder" runat="server" FilterMode="ValidChars"
                                                        FilterType="Custom" TargetControlID="txtDO2" ValidChars="123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="reqSearch1" runat="server" ControlToValidate="txtDO2"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="*" ValidationGroup="addf"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <div style="width: 100px">
                                                        <asp:Button ID="btnAdd2" runat="server" CommandName="addMenu" CssClass="buttonbg"
                                                            Text="Add" ToolTip="Add" ValidationGroup="addf" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </FooterTemplate>
                                </asp:DataList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="MenuDetails" runat="server" TabIndex="1">
                <HeaderTemplate>
                    Menu Details
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="mainheading">
                        Menu Details List
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <div class="grid1">
                                <asp:DataList ID="dlstPrc1" runat="server" AlternatingItemStyle-CssClass="Altrow"
                                    CellPadding="0" CellSpacing="0" DataKeyField="MenuID" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="None"
                                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Top"
                                    ItemStyle-CssClass="gridrow" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top"
                                    OnCancelCommand="dlstPrc1_CancelCommand" OnDeleteCommand="dlstPrc1_DeleteCommand"
                                    OnEditCommand="dlstPrc1_EditCommand" OnItemCommand="dlstPrc1_ItemCommand" OnItemDataBound="dlstPrc1_ItemDataBound"
                                    OnUpdateCommand="dlstPrc1_UpdateCommand" RepeatColumns="1" RepeatDirection="Vertical"
                                    SelectedItemStyle-CssClass="tbl_selected" Width="100%">
                                    <SelectedItemStyle CssClass="tbl_selected" />
                                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <HeaderTemplate>
                                        <table border="0" cellpadding="4" cellspacing="0" width="100%" class="gridheader">
                                            <tr>
                                                <td align="left" valign="top" width="21%">
                                                    <div style="min-width: 200px">Menu Name</div>
                                                </td>
                                                <td align="left" valign="top" width="21%">
                                                    <div style="min-width: 200px">Menu Description</div>
                                                </td>
                                                <td align="left" valign="top" width="21%">
                                                    <div style="min-width: 200px">Parent Name</div>
                                                </td>
                                                <td align="left" valign="top" width="21%">
                                                    <div style="min-width: 50px">Display Order</div>
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <div style="min-width: 100px">Actions</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" valign="top" width="21%">
                                                    <div style="min-width: 200px">
                                                        <asp:Label ID="lblMenuName" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"MenuName").ToString())%>'
                                                            Visible="true"></asp:Label>
                                                    </div>
                                                </td>
                                                <td align="left" valign="top" width="21%">
                                                    <div style="min-width: 200px">
                                                        <asp:Label ID="lblDesc" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"MenuDescription").ToString())%>'
                                                            Visible="true"></asp:Label>
                                                        <div style="min-width: 200px">
                                                </td>
                                                <td align="left" valign="top" width="21%">
                                                    <div style="min-width: 200px">
                                                        <asp:Label ID="lblParentName" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"ParentName").ToString())%>'
                                                            Visible="true"></asp:Label>
                                                        <div style="min-width: 200px">
                                                </td>
                                                <td align="left" valign="top" width="21%">
                                                    <div style="min-width: 50px">
                                                        <asp:Label ID="lblDO" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"DisplayOrderNumber").ToString())%>'
                                                            Visible="true"></asp:Label>
                                                    </div>
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <div style="min-width: 100px">
                                                        <div class="float-margin">
                                                            <asp:ImageButton ID="img11" runat="server" CausesValidation="false" CommandName="Edit"
                                                                ImageAlign="AbsMiddle" ToolTip="Edit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                                        </div>
                                                        <div class="float-margin">
                                                            <asp:ImageButton ID="imgActive" runat="server" AlternateText="DeActive" CausesValidation="false"
                                                                CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MenuID") %>' CommandName="Active"
                                                                ImageAlign="AbsMiddle" ToolTip="Status" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>' />
                                                        </div>
                                                        <div class="float-left">
                                                            <asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                ImageAlign="AbsMiddle" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/delete.png"%>'
                                                                ToolTip="Delete" />
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <AlternatingItemStyle CssClass="Altrow" />
                                    <ItemStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <EditItemTemplate>
                                        <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" valign="top" width="21%">
                                                    <div style="min-width: 200px">
                                                        <asp:TextBox ID="txtSubmenuE" runat="server" CssClass="formfields" MaxLength="100"
                                                            Text='<%#(DataBinder.Eval(Container.DataItem,"MenuName").ToString())%>' />
                                                    </div>
                                                    <%--  <asp:RegularExpressionValidator ID="re3" runat="server" ControlToValidate="txtSubmenuE"
                                                                                                        CssClass="error" Display="Dynamic" ErrorMessage="Invalid Chars" ValidationExpression="^[a-zA-Z ]{1,50}"
                                                                                                        ValidationGroup="grpdet"></asp:RegularExpressionValidator>--%>
                                                    <asp:RequiredFieldValidator ID="rf1" runat="server" ControlToValidate="txtSubmenuE"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="*" ValidationGroup="grpdet"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="21%">
                                                    <div style="min-width: 200px">
                                                        <asp:TextBox ID="txtDescE" runat="server" CssClass="formfields" MaxLength="100"
                                                            Text='<%# (DataBinder.Eval(Container.DataItem,"MenuDescription").ToString())%>'></asp:TextBox>
                                                    </div>
                                                    <%--  <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtDescE"
                                                                                                        CssClass="error" Display="Dynamic" ErrorMessage="Invalid Chars" ValidationExpression="^[a-zA-Z ]{1,255}"
                                                                                                        ValidationGroup="grpdet"></asp:RegularExpressionValidator>--%>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDescE"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="*" ValidationGroup="grpdet"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="21%">
                                                    <div style="min-width: 200px">
                                                        <asp:DropDownList ID="ddlParentNameE" runat="server" CssClass="formselect" />
                                                    </div>
                                                </td>
                                                <td align="left" valign="top" width="21%">
                                                    <div style="min-width: 50px">
                                                        <asp:TextBox ID="txtDOE" runat="server" CssClass="formfields" MaxLength="100" Text='<%# (DataBinder.Eval(Container.DataItem,"DisplayOrderNumber").ToString())%>'></asp:TextBox>
                                                    </div>
                                                    <cc1:FilteredTextBoxExtender ID="ftorders" runat="server" FilterMode="ValidChars"
                                                        FilterType="Custom" TargetControlID="txtDOE" ValidChars="123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rf6" runat="server" ControlToValidate="txtDOE" CssClass="error"
                                                        Display="Dynamic" ErrorMessage="*" ValidationGroup="grpdet"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <div style="min-width: 100px">
                                                        <div class="float-margin">
                                                            <asp:ImageButton ID="UpdateState1" runat="server" AlternateText="Update" CommandName="Update"
                                                                ImageAlign="AbsMiddle" ToolTip="Update" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/icon_update.gif"%>'
                                                                ValidationGroup="grpdet" />
                                                        </div>
                                                        <div class="float-margin">
                                                            <asp:ImageButton ID="CancelUpdate1" runat="server" CausesValidation="false"
                                                                CommandName="Cancel" ToolTip="Cancel" ImageAlign="AbsMiddle" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/icon_cancel.gif"%>' />
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" valign="top" width="21%">
                                                    <div style="min-width: 200px">
                                                        <asp:TextBox ID="txtSubmenu" runat="server" CssClass="formfields" />
                                                    </div>
                                                    <asp:RegularExpressionValidator ID="re3" runat="server" ControlToValidate="txtSubmenu"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="Invalid Chars" ValidationExpression="^[a-zA-Z ]{1,50}"
                                                        ValidationGroup="grpdetf"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="rf1" runat="server" ControlToValidate="txtSubmenu"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="*" ValidationGroup="grpdetf"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="21%">
                                                    <div style="min-width: 200px">
                                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="formfields"></asp:TextBox>
                                                    </div>
                                                    <asp:RegularExpressionValidator ID="re4" runat="server" ControlToValidate="txtDesc"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="Invalid Chars" ValidationExpression="^[a-zA-Z ]{1,255}"
                                                        ValidationGroup="grpdetf"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="rf2" runat="server" ControlToValidate="txtDesc" CssClass="error"
                                                        Display="Dynamic" ErrorMessage="*" ValidationGroup="grpdetf"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="21%">
                                                    <div style="min-width: 200px">
                                                        <asp:DropDownList ID="ddlParentName" runat="server" CssClass="formselect" />
                                                    </div>
                                                </td>
                                                <td align="left" valign="top" width="21%">
                                                    <div style="min-width: 50px">
                                                        <asp:TextBox ID="txtDO" runat="server" CssClass="formfields-W70" Width="40" MaxLength="2"></asp:TextBox>
                                                    </div>
                                                    <cc1:FilteredTextBoxExtender ID="ftorders" runat="server" FilterMode="ValidChars"
                                                        FilterType="Custom" TargetControlID="txtDO" ValidChars="123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rf6" runat="server" ControlToValidate="txtDO" CssClass="error"
                                                        Display="Dynamic" ErrorMessage="*" ValidationGroup="grpdetf"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <div style="min-width: 100px">
                                                        <asp:Button ID="btnAdd" runat="server" ToolTip="Add" CommandName="addParent" CssClass="buttonbg"
                                                            Text="+ Add" ValidationGroup="grpdetf" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </FooterTemplate>
                                </asp:DataList>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="TabModule" runat="server" TabIndex="2">
                <HeaderTemplate>
                    Module Definition
                </HeaderTemplate>
                <ContentTemplate>
                    <div class="mainheading">
                        Menu List
                    </div>
                    <asp:UpdatePanel ID="updl2" runat="server">
                        <ContentTemplate>
                            <div class="grid1">
                                <asp:DataList ID="dlstPrc2" runat="server" RepeatColumns="1" RepeatDirection="Vertical"
                                    OnEditCommand="dlstPrc2_EditCommand" OnCancelCommand="dlstPrc2_CancelCommand"
                                    DataKeyField="MenuID" SelectedItemStyle-CssClass="tbl_selected" OnUpdateCommand="dlstPrc2_UpdateCommand"
                                    OnItemDataBound="dlstPrc2_ItemDataBound" OnDeleteCommand="dlstPrc2_DeleteCommand"
                                    OnItemCommand="dlstPrc2_ItemCommand" Width="1200" CellSpacing="0" CellPadding="0"
                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Top" HeaderStyle-CssClass="gridheader"
                                    ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="gridrow"
                                    AlternatingItemStyle-CssClass="Altrow" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                                    FooterStyle-CssClass="gridfooter" GridLines="None">
                                    <SelectedItemStyle />
                                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <HeaderTemplate>
                                        <table cellpadding="4" cellspacing="0" width="1200" border="0" class="gridheader">
                                            <tr>
                                                <td width="15%" align="left">Menu Name
                                                </td>
                                                <td width="15%" align="left">Menu Description
                                                </td>
                                                <td width="15%" align="left">Parent Menu
                                                </td>
                                                <td width="5%" align="left">Order
                                                </td>
                                                <td width="30%" align="left">Module Name
                                                </td>
                                                <td width="5%" align="left">Allow In Menu
                                                </td>
                                                <td width="15%" align="left">Actions
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table cellpadding="4" cellspacing="0" width="1200" border="0">
                                            <tr>
                                                <td align="left" valign="top" width="15%">
                                                    <asp:Label ID="lblMenuName" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"MenuName").ToString())%>'
                                                        Visible="true"></asp:Label>
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <asp:Label runat="server" ID="lblDesc" Text='<%# (DataBinder.Eval(Container.DataItem,"MenuDescription").ToString())%>'
                                                        Visible="true"></asp:Label>
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <asp:Label runat="server" ID="lblParentName" Text='<%# (DataBinder.Eval(Container.DataItem,"ParentName").ToString())%>'
                                                        Visible="true"></asp:Label>
                                                </td>
                                                <td align="left" valign="top" width="5%">
                                                    <asp:Label runat="server" ID="lblDO" Text='<%# (DataBinder.Eval(Container.DataItem,"DisplayOrderNumber").ToString())%>'
                                                        Visible="true"></asp:Label>
                                                </td>
                                                <td align="left" valign="top" width="30%">
                                                    <div style="width: 340px; overflow: hidden; word-wrap: break-word;">
                                                        <asp:Label runat="server" ID="lblModuleName" Text='<%# (DataBinder.Eval(Container.DataItem,"NavigationURL").ToString())%>'
                                                            Visible="true"></asp:Label>
                                                    </div>
                                                </td>
                                                <td align="left" valign="top" width="5%">
                                                    <asp:Label runat="server" ID="lblAllowInMenu" Text='<%# (DataBinder.Eval(Container.DataItem,"AllowMenuDisplay").ToString())%>'
                                                        Visible="true"></asp:Label>
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <div class="float-margin">
                                                        <asp:ImageButton ID="img11" runat="server" CausesValidation="false" CommandName="Edit"
                                                            ToolTip="Edit" ImageAlign="AbsMiddle" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                                    </div>
                                                    <div class="float-margin">
                                                        <asp:ImageButton ID="imgActive" runat="server" AlternateText="DeActive" CausesValidation="false"
                                                            CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MenuID") %>' CommandName="Active"
                                                            ToolTip="Status" ImageAlign="AbsMiddle" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>' />
                                                    </div>
                                                    <div class="float-margin">
                                                        <asp:ImageButton ID="ImgDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                            ImageAlign="AbsMiddle" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/delete.png"%>'
                                                            ToolTip="Delete" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <AlternatingItemStyle CssClass="Altrow" />
                                    <ItemStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <EditItemTemplate>
                                        <table cellpadding="4" cellspacing="0" width="1200" border="0">
                                            <tr>
                                                <td align="left" valign="top" width="15%">
                                                    <asp:TextBox CssClass="formfields" runat="server" ID="txtModuleNameE"
                                                        Text='<%#(DataBinder.Eval(Container.DataItem,"MenuName").ToString())%>' MaxLength="200" />
                                                    <asp:RequiredFieldValidator ID="reqSearch" runat="server" ControlToValidate="txtModuleNameE"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="*" ValidationGroup="grpmde"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <asp:TextBox CssClass="formfields" runat="server" ID="txtDescE" Text='<%# (DataBinder.Eval(Container.DataItem,"MenuDescription").ToString())%>'
                                                        MaxLength="200"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDescE"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="*" ValidationGroup="grpmde"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <asp:DropDownList ID="ddlParentNameE" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlParentNameE_SelectedIndexChanged"
                                                        CssClass="formselect" />
                                                </td>
                                                <td align="left" valign="top" width="5%">
                                                    <asp:TextBox ID="txtDOE" runat="server" CssClass="formfields-W70" Width="30px" MaxLength="3"
                                                        Text='<%# (DataBinder.Eval(Container.DataItem,"DisplayOrderNumber").ToString())%>'></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftorders" runat="server" FilterMode="ValidChars"
                                                        FilterType="Custom" TargetControlID="txtDOE" ValidChars="0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rf6" runat="server" ControlToValidate="txtDOE" CssClass="error"
                                                        Display="Dynamic" ErrorMessage="*" ValidationGroup="grpmde"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="30%">
                                                    <asp:TextBox CssClass="formfields" runat="server" ID="txtNavigationURLE" Text='<%#(DataBinder.Eval(Container.DataItem,"NavigationURL").ToString())%>'
                                                        MaxLength="200" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNavigationURLE"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="*" ValidationGroup="grpmde"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="5%">
                                                    <asp:CheckBox CssClass="formfields" runat="server" ID="chkAllowInMenuE" />
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <div class="float-margin">
                                                        <asp:ImageButton AlternateText="Update" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/icon_update.gif"%>'
                                                            ImageAlign="AbsMiddle" ToolTip="Update" ID="UpdateState1" runat="server" CommandName="Update"
                                                            ValidationGroup="grpmde" />
                                                    </div>
                                                    <div class="float-margin">
                                                        <asp:ImageButton ImageUrl='<%#"~/" + strAssets + "/CSS/Images/icon_cancel.gif"%>'
                                                            ID="CancelUpdate1" ToolTip="Cancel" runat="server" CommandName="Cancel" ImageAlign="AbsMiddle"
                                                            CausesValidation="false" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <table cellpadding="4" cellspacing="0" width="1200" border="0">
                                            <tr>
                                                <td align="left" valign="top" width="15%">
                                                    <asp:TextBox ID="txtModuleName" runat="server" CssClass="formfields" MaxLength="200" />
                                                    <asp:RequiredFieldValidator ID="reqModuleName" runat="server" ControlToValidate="txtModuleName"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="*" ValidationGroup="grpmdef"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <asp:TextBox ID="txtDesc" runat="server" CssClass="formfields" MaxLength="200"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="reqDesc" runat="server" ControlToValidate="txtDesc"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="*" ValidationGroup="grpmdef"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <asp:DropDownList ID="ddlParentName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlParentName_SelectedIndexChanged"
                                                        CssClass="formselect" />
                                                </td>
                                                <td align="left" valign="top" width="5%">
                                                    <asp:TextBox ID="txtDO" runat="server" CssClass="formfields-W70" MaxLength="3" Width="30px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftorders" runat="server" FilterMode="ValidChars"
                                                        FilterType="Custom" TargetControlID="txtDO" ValidChars="0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rf6" runat="server" ControlToValidate="txtDO" CssClass="error"
                                                        Display="Dynamic" ErrorMessage="*" ValidationGroup="grpmdef"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="30%">
                                                    <asp:TextBox CssClass="formfields" runat="server" ID="txtNavigationURL" MaxLength="200" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNavigationURL"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="*" ValidationGroup="grpmdef"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="5%">
                                                    <asp:CheckBox ID="chkAllowInMenu" runat="server" />
                                                </td>
                                                <td align="left" valign="top" width="15%">
                                                    <asp:Button ID="btnAdd" runat="server" CommandName="addModule" CssClass="buttonbg"
                                                        Text="Add" ToolTip="Add" ValidationGroup="grpmdef" />
                                                </td>
                                            </tr>
                                        </table>
                                    </FooterTemplate>
                                </asp:DataList>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </ContentTemplate>
            </cc1:TabPanel>
        </cc1:TabContainer>
    </div>
</asp:Content>
