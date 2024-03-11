<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewSchemeDetailsPOC.aspx.cs"
    Inherits="Masters_Common_ViewSchemeDetailsPOC" %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%--<html xmlns="http://www.w3.org/1999/xhtml">--%>
<html lang="en">
<head id="Head5" runat="server">
    <%--Add START  --%>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <%--Add END  --%>
    <title></title>
    <link rel="stylesheet" id="StyleCss" runat="server" type="text/css" />
</head>
<body>
    <form id="form1" name="form1" runat="server">
        <asp:ScriptManager ID="s" runat="server">
        </asp:ScriptManager>
        <div>
            <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <uc1:ucMessage ID="ucMessage1" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <div class="mainheading">
                        Scheme Details
                    </div>
                    <div class="contentbox">
                        <asp:Panel ID="pnlConverage" runat="server" Visible="True">
                            <div class="H25-C3-S">
                                <ul>
                                    <li class="text">Scheme For Type:
                                    </li>
                                    <li class="field">
                                        <asp:Label ID="lblSchemeForType" CssClass="frmtxt1" runat="server" Text=""></asp:Label>
                                    </li>
                                    <li class="text">Scheme Location:
                                    </li>
                                    <li class="field">
                                        <asp:Label ID="lblSchemeLocation" CssClass="frmtxt1" runat="server" Text=""></asp:Label>
                                    </li>
                                </ul>
                            </div>
                        </asp:Panel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="mainheading" runat="server" id="dvSearchResult" visible="false">
                Scheme Conditions
            </div>
            <div class="mainheading">
                Scheme Conditions
            </div>
            <div class="contentbox">
                <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlFilter" runat="server" Visible="false">
                            <div class="grid1" runat="server" id="dvGrid" visible="True">
                                <%--   <asp:GridView ID="GridFilter" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                    CellSpacing="1" DataKeyNames="ComponentFilterID" EditRowStyle-CssClass="editrow"
                                                                    EmptyDataText="No Record Found" GridLines="None" HeaderStyle-CssClass="gridheader"
                                                                    RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="gridrow1" Width="100%"
                                                                    AllowPaging="True" OnPageIndexChanging="GridFilter_PageIndexChanging">
                                                                    <RowStyle CssClass="gridrow" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="ComponentFilterName" HeaderStyle-HorizontalAlign="Left"
                                                                            HeaderText="ComponentFilterName" HtmlEncode="true">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="FilterValue" HeaderStyle-HorizontalAlign="Left" HeaderText="FilterValue"
                                                                            HtmlEncode="true">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="gridheader" />
                                                                    <EditRowStyle CssClass="editrow" />
                                                                    <AlternatingRowStyle CssClass="gridrow1" />
                                                                    <PagerStyle CssClass="PagerStyle" />
                                                                </asp:GridView>--%>
                                <asp:DataList ID="dlstFilter" runat="server" RepeatColumns="1" RepeatDirection="Vertical"
                                    OnEditCommand="dlstFilter_EditCommand" OnCancelCommand="dlstFilter_CancelCommand"
                                    DataKeyField="SchemeComponentFilterValueID" OnUpdateCommand="dlstFilter_UpdateCommand"
                                    OnItemCommand="dlstFilter_ItemCommand" Width="100%" CellSpacing="0" CellPadding="0"
                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Top" HeaderStyle-CssClass="gridheader"
                                    ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="gridrow" AlternatingItemStyle-CssClass="Altrow"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" FooterStyle-CssClass="gridfooter"
                                    GridLines="None" OnItemDataBound="dlstFilter_ItemDataBound1">
                                    <SelectedItemStyle CssClass="tbl_selected" />
                                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <HeaderTemplate>
                                        <table cellpadding="4" cellspacing="0" width="100%" border="0" class="gridheader">
                                            <tr>
                                                <td width="33%" align="left">Component Filter Name
                                                </td>
                                                <td width="33%" align="left">Filter Value
                                                </td>
                                                <td width="24%" align="left">Operator
                                                </td>
                                                <td width="10%" align="left">Actions
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table cellpadding="4" cellspacing="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" width="33%" height="20" valign="top">
                                                    <asp:Label runat="server" ID="lblComponentFilterName" Text='<%# (DataBinder.Eval(Container.DataItem,"ComponentFilterName"))%>'></asp:Label>
                                                </td>
                                                <td align="left" width="33%" height="20" valign="top">
                                                    <asp:Label runat="server" ID="lblComponentFilterValue" Text='<%# (DataBinder.Eval(Container.DataItem,"FilterValue"))%>'
                                                        Visible="True"></asp:Label>
                                                </td>
                                                <td align="left" width="24%" height="20" valign="top">
                                                    <asp:Label runat="server" ID="lblSyntax" Text='<%# (DataBinder.Eval(Container.DataItem,"Syntax"))%>'
                                                        Visible="True"></asp:Label>
                                                </td>
                                                <td align="left" width="10%" valign="top">
                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" width="70%">
                                                                <asp:ImageButton ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' ID="img1"
                                                                    ImageAlign="Top" CommandName="Edit" runat="server" CommandArgument='<%#Eval("SchemeComponentFilterValueID") %>'
                                                                    CausesValidation="false" ToolTip="Edit" />
                                                            </td>
                                                            <td align="left" width="30%">
                                                                <%--   <cc2:ZedImageButton CommandName="Delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "EntityPriceGroupID") %>'
                                                                                                ID="imgDelete" runat="server" AlternateText="Delete" CausesValidation="false"
                                                                                                ActionTag="Delete" Visible='<%#ShowDelete(Eval("ValidFrom")) %>'
                                                                                                ImageAlign="Top" ToolTip="Active/Inactive" ImageUrl="~/Assets/Images/delete.png" />--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <AlternatingItemStyle CssClass="Altrow" />
                                    <ItemStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <EditItemTemplate>
                                        <table cellpadding="4" cellspacing="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" width="33%" valign="top">
                                                    <asp:Label runat="server" ID="lblComponentFilterID" Text='<%# (DataBinder.Eval(Container.DataItem,"ComponentFilterID"))%>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label runat="server" ID="lblDataType" Text='<%# (DataBinder.Eval(Container.DataItem,"Datatype"))%>'
                                                        Visible="false"></asp:Label>
                                                    <asp:DropDownList ID="ddlComponentFilterName" runat="server" CssClass="formselect" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" runat="server"
                                                        ControlToValidate="ddlComponentFilterName" ErrorMessage="Please Select Fiter Name."
                                                        ValidationGroup="EditPC" CssClass="error" InitialValue="0"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" width="33%" valign="top">
                                                    <asp:TextBox ID="txtFilterValue" runat="server" CssClass="formfields" Text='<%# (DataBinder.Eval(Container.DataItem,"FilterValue"))%>'
                                                        MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="reqValue" Display="Dynamic" runat="server" ControlToValidate="txtFilterValue"
                                                        ErrorMessage="Please Insert Filter Value." ValidationGroup="EditPC" CssClass="error"></asp:RequiredFieldValidator>
                                                    <%-- <asp:RegularExpressionValidator ID="regFilterValue" runat="server" ControlToValidate="txtFilterValue"
                                                                                        CssClass="error" Display="Dynamic" ErrorMessage="Please enter valid Information."
                                                                                        ForeColor="" SetFocusOnError="true" ValidationExpression="^\d$" ValidationGroup="EditPC"></asp:RegularExpressionValidator>--%>
                                                    <%-- <asp:RegularExpressionValidator ID="regFilterValueNumeric" runat="server" ControlToValidate="txtFilterValue"
                                                            CssClass="error" Display="Dynamic" ErrorMessage="Please enter valid Information."
                                                            ForeColor="" SetFocusOnError="true" ValidationExpression="^\d$"
                                                            ValidationGroup="EditPC"></asp:RegularExpressionValidator>--%>
                                                </td>
                                                <td align="left" width="24%" valign="top">
                                                    <asp:Label runat="server" ID="lblSyntax" Text='<%# (DataBinder.Eval(Container.DataItem,"Syntax"))%>'
                                                        Visible="True"></asp:Label>
                                                </td>
                                                <td align="left" width="10%">
                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" width="30%">
                                                                <asp:ImageButton AlternateText="Update" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/Icon_Update.gif"%>'
                                                                    ImageAlign="AbsMiddle" ID="UpdateState" runat="server" CommandName="Update" ValidationGroup="EditPC"
                                                                    CausesValidation="true" ActionTag="Edit" ToolTip="Update" />
                                                            </td>
                                                            <td align="left" width="70%">
                                                                <asp:ImageButton ImageUrl='<%#"~/" + strAssets + "/CSS/Images/icon_cancel.gif"%>'
                                                                    ID="CancelUpdate" runat="server" CommandName="Cancel" ImageAlign="AbsMiddle"
                                                                    CausesValidation="false" ToolTip="Cancel" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <table cellpadding="4" cellspacing="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" width="33%" valign="top">
                                                    <asp:DropDownList ID="ddlComponentFilterName" runat="server" CssClass="formselect" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" runat="server"
                                                        ControlToValidate="ddlComponentFilterName" ErrorMessage="Please Select FilterName."
                                                        ValidationGroup="addCM" CssClass="error" InitialValue="0"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" width="33%" valign="top">
                                                    <asp:TextBox ID="txtFilterValue" runat="server" CssClass="formfields" MaxLength="30"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="reqValue" Display="Dynamic" runat="server" ControlToValidate="txtFilterValue"
                                                        ErrorMessage="Please Insert Filter Value." ValidationGroup="addCM" CssClass="error"></asp:RequiredFieldValidator>
                                                   
                                                </td>
                                                <td align="left" width="24%" valign="top">
                                                    <asp:DropDownList ID="ddlsyntax" runat="server" CssClass="formselect" Width="70px">
                                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="<" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text=">" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="<>" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="&lt;=" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="&gt;=" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="=" Value="6"></asp:ListItem>

                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Display="Dynamic" runat="server"
                                                        ControlToValidate="ddlsyntax" ErrorMessage="Please Select Syntax." ValidationGroup="addCM"
                                                        CssClass="error" InitialValue="0"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="10%">
                                                    <asp:Button ID="btnAdd" runat="server" ValidationGroup="addCM" CssClass="buttonbg"
                                                        Text="+Add" CommandName="addFilter" ActionTag="Add" />
                                                </td>
                                            </tr>
                                        </table>
                                    </FooterTemplate>
                                </asp:DataList>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="mainheading">
                Scheme PayOut
            </div>
            <div class="contentbox">
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">PayOut Criteria:
                        </li>
                        <li class="field">
                            <asp:Label ID="lblCriterialType" runat="server" CssClass="frmtxt1" Text=""></asp:Label>
                        </li>
                       <li class="text">Payout Type:
                        </li>
                        <li class="field">
                            <asp:Label ID="lblPayoutType" runat="server" CssClass="frmtxt1" Text=""></asp:Label>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="contentbox">
                <asp:UpdatePanel ID="updPayout" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlPayout" runat="server" Visible="false">
                            <div class="grid1" runat="server" id="Div1" visible="True">
                                <%--<asp:GridView ID="grdPayout" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                    CellSpacing="1" DataKeyNames="SchemeComponentPayoutSlabID" EditRowStyle-CssClass="editrow"
                                                                    EmptyDataText="No Record Found" GridLines="None" HeaderStyle-CssClass="gridheader"
                                                                    RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="gridrow1" Width="100%"
                                                                    AllowPaging="True" OnPageIndexChanging="grdPayout_PageIndexChanging" OnRowDataBound="grdPayout_RowDataBound">
                                                                    <RowStyle CssClass="gridrow" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="AchievementFrom" HeaderStyle-HorizontalAlign="Left" HeaderText="AchievementFrom"
                                                                            HtmlEncode="true">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="AchievementTo" HeaderStyle-HorizontalAlign="Left" HeaderText="AchievementTo"
                                                                            HtmlEncode="true">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="PayoutRate" HeaderStyle-HorizontalAlign="Left" HeaderText="PayoutRate"
                                                                            HtmlEncode="true">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="gridheader" />
                                                                    <EditRowStyle CssClass="editrow" />
                                                                    <AlternatingRowStyle CssClass="gridrow1" />
                                                                    <PagerStyle CssClass="PagerStyle" />
                                                                </asp:GridView>--%>
                                <asp:DataList ID="dlstPayout" runat="server" RepeatColumns="1" RepeatDirection="Vertical"
                                    OnEditCommand="dlstPayout_EditCommand" OnCancelCommand="dlstPayout_CancelCommand"
                                    DataKeyField="SchemeComponentPayoutSlabID" OnUpdateCommand="dlstPayout_UpdateCommand"
                                    OnItemCommand="dlstPayout_ItemCommand" Width="100%" CellSpacing="0" CellPadding="0"
                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Top" HeaderStyle-CssClass="gridheader"
                                    ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top" ItemStyle-CssClass="gridrow"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" FooterStyle-CssClass="gridfooter"
                                    GridLines="None" OnItemDataBound="dlstPayout_ItemDataBound1">
                                    <SelectedItemStyle CssClass="tbl_selected" />
                                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <HeaderTemplate>
                                        <table cellpadding="4" cellspacing="0" width="100%" border="0" class="gridheader">
                                            <tr>
                                                <td width="33%" align="left">AchievementFrom
                                                </td>
                                                <td width="33%" align="left">AchievementTo
                                                </td>
                                                <td width="24%" align="left">Payout
                                                </td>
                                                <td width="10%" align="left">Actions
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table cellpadding="4" cellspacing="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" width="33%" height="20" valign="top">
                                                    <asp:Label runat="server" ID="lblAchievementFrom" Text='<%# (DataBinder.Eval(Container.DataItem,"AchievementFrom"))%>'></asp:Label>
                                                </td>
                                                <td align="left" width="33%" height="20" valign="top">
                                                    <asp:Label runat="server" ID="lblAchievementTo" Text='<%# (DataBinder.Eval(Container.DataItem,"AchievementTo"))%>'
                                                        Visible="True"></asp:Label>
                                                </td>
                                                <td align="left" width="24%" height="20" valign="top">
                                                    <asp:Label runat="server" ID="lblPayoutRate" Text='<%# (DataBinder.Eval(Container.DataItem,"PayoutRate"))%>'
                                                        Visible="True"></asp:Label>
                                                </td>
                                                <td align="left" width="10%" valign="top">
                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" width="70%">
                                                                <asp:ImageButton ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' ID="img1"
                                                                    ImageAlign="Top" CommandName="Edit" runat="server" CommandArgument='<%#Eval("SchemeComponentPayoutSlabID") %>'
                                                                    CausesValidation="false" ToolTip="Edit" />
                                                            </td>
                                                            <td align="left" width="30%">
                                                                <asp:ImageButton ImageUrl='<%#"~/" + strAssets + "/CSS/Images/Delete.png"%>' ID="ImageButton1"
                                                                    ImageAlign="Top" CommandName="Delete" runat="server" CommandArgument='<%#Eval("SchemeComponentPayoutSlabID") %>'
                                                                    CausesValidation="false" ToolTip="Delete" />
                                                                <%--   <cc2:ZedImageButton CommandName="Delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "EntityPriceGroupID") %>'
                                                                                                ID="imgDelete" runat="server" AlternateText="Delete" CausesValidation="false"
                                                                                                ActionTag="Delete" Visible='<%#ShowDelete(Eval("ValidFrom")) %>'
                                                                                                ImageAlign="Top" ToolTip="Active/Inactive" ImageUrl="~/Assets/Images/delete.png" />--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <AlternatingItemStyle CssClass="Altrow" />
                                    <ItemStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <EditItemTemplate>
                                        <table cellpadding="4" cellspacing="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" width="33%" valign="top">
                                                    <asp:TextBox ID="txtAchievementFrom" runat="server" CssClass="formfields" Text='<%# (DataBinder.Eval(Container.DataItem,"AchievementFrom"))%>'
                                                        MaxLength="100" Enabled="false"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" runat="server"
                                                        ControlToValidate="txtAchievementFrom" ErrorMessage="Please Insert Achievment From."
                                                        ValidationGroup="EditPC" CssClass="error"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" width="33%" valign="top">
                                                    <asp:TextBox ID="txtAchievementTo" runat="server" CssClass="formfields" Text='<%# (DataBinder.Eval(Container.DataItem,"AchievementTo"))%>'
                                                        MaxLength="100" Enabled="false"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="reqValue" Display="Dynamic" runat="server" ControlToValidate="txtAchievementTo"
                                                        ErrorMessage="Please Insert Achievment To." ValidationGroup="EditPC" CssClass="error"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" width="24%" valign="top">
                                                    <asp:TextBox ID="txtPayoutRate" runat="server" CssClass="formfields" Text='<%# (DataBinder.Eval(Container.DataItem,"PayoutRate"))%>'
                                                        MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" runat="server"
                                                        ControlToValidate="txtPayoutRate" ErrorMessage="Please Insert Payout." ValidationGroup="EditPC"
                                                        CssClass="error"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" width="10%">
                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" width="30%">
                                                                <asp:ImageButton AlternateText="Update" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/Icon_Update.gif"%>'
                                                                    ImageAlign="AbsMiddle" ID="UpdateState" runat="server" CommandName="Update" ValidationGroup="EditPC"
                                                                    CausesValidation="True" ActionTag="Edit" ToolTip="Update" />
                                                            </td>
                                                            <td align="left" width="70%">
                                                                <asp:ImageButton ImageUrl='<%#"~/" + strAssets + "/CSS/Images/icon_cancel.gif"%>'
                                                                    ID="CancelUpdate" runat="server" CommandName="Cancel" ImageAlign="AbsMiddle"
                                                                    CausesValidation="false" ToolTip="Cancel" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <table cellpadding="4" cellspacing="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" width="33%" valign="top">
                                                    <asp:TextBox ID="txtAchievementFrom" runat="server" CssClass="formfields" Text='<%# (DataBinder.Eval(Container.DataItem,"AchievementFrom"))%>'
                                                        MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" runat="server"
                                                        ControlToValidate="txtAchievementFrom" ErrorMessage="Please Insert Achievment From."
                                                        ValidationGroup="addPay" CssClass="error"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" width="33%" valign="top">
                                                    <asp:TextBox ID="txtAchievementTo" runat="server" CssClass="formfields" Text='<%# (DataBinder.Eval(Container.DataItem,"AchievementTo"))%>'
                                                        MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="reqValue" Display="Dynamic" runat="server" ControlToValidate="txtAchievementTo"
                                                        ErrorMessage="Please Insert Achievment To." ValidationGroup="addPay" CssClass="error"></asp:RequiredFieldValidator>
                                                  
                                                </td>
                                                <td align="left" width="24%" valign="top">
                                                    <asp:TextBox ID="txtPayoutRate" runat="server" CssClass="formfields" Text='<%# (DataBinder.Eval(Container.DataItem,"PayoutRate"))%>'
                                                        MaxLength="100"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" runat="server"
                                                        ControlToValidate="txtPayoutRate" ErrorMessage="Please Insert Payout." ValidationGroup="addPay"
                                                        CssClass="error"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" valign="top" width="10%">
                                                    <asp:Button ID="btnAdd" runat="server" ValidationGroup="addPay" CssClass="buttonbg"
                                                        Text="+Add" CommandName="addPayout" ActionTag="Add" />
                                                </td>
                                            </tr>
                                        </table>
                                    </FooterTemplate>
                                </asp:DataList>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
