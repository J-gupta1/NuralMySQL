<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/ReportPage.master"
    AutoEventWireup="true" CodeFile="ManageMenuTree.aspx.cs" Inherits="Masters_HO_Admin_ManageMenuTree" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <link rel="stylesheet" id="menuCss" runat="server" type="text/css" />--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
   <ContentTemplate>--%>
    <asp:HiddenField ID="hdnNavigation" runat="server" />
    <asp:HiddenField ID="hdnMenuType" runat="server" />

    <uc1:ucMessage ID="ucMsg" runat="server" />
    <div class="contentbox">
        <div class="grid1" style="padding-bottom: 15px;">
            <dx:ASPxTreeList ID="ASPxTreeList1" runat="server" AutoGenerateColumns="False" Width="100%"
                KeyFieldName="MenuID" ParentFieldName="ParentMenuID" OnNodeUpdating="ASPxTreeList1_NodeUpdating"
                OnNodeInserting="ASPxTreeList1_NodeInserting" OnNodeValidating="ASPxTreeList1_NodeValidating"
                OnProcessDragNode="ASPxTreeList1_ProcessDragNode" EnableCallbacks="False" OnCommandColumnButtonInitialize="ASPxTreeList1_CommandColumnButtonInitialize"
                OnStartNodeEditing="ASPxTreeList1_StartNodeEditing" OnPreRender="ASPxTreeList1_PreRender"
                CssFilePath="~/App_Themes/SoftOrange/{0}/styles.css" CssPostfix="SoftOrange">
                <SettingsEditing AllowNodeDragDrop="True" AllowRecursiveDelete="True" />
                <Styles CssFilePath="~/App_Themes/SoftOrange/{0}/styles.css" CssPostfix="SoftOrange">
                </Styles>
                <Images SpriteCssFilePath="~/App_Themes/SoftOrange/{0}/sprite.css">
                    <LoadingPanel Url="~/App_Themes/SoftOrange/TreeList/Loading.gif">
                    </LoadingPanel>
                </Images>
                <SettingsLoadingPanel ImagePosition="Top" />
                <SettingsSelection Enabled="True" />
                <Settings GridLines="Both" SuppressOuterGridLines="True" />
                <SettingsBehavior ExpandCollapseAction="NodeDblClick" />
                <SettingsEditing Mode="EditFormAndDisplayNode" />
                <Templates>
                    <EditForm>
                        <%--Start  --%>
                        <div class="contentbox">
                            <div class="H25-C3">
                                <ul>
                                    <li class="text">Menu Name: <span class="error">*</span>
                                    </li>
                                    <li class="field">
                                        <asp:TextBox CssClass="formfields" ID="txtMenuName" runat="server" MaxLength="70"
                                            Text='<%#Eval("MenuName") %>' CausesValidation="True"></asp:TextBox>
                                    </li>
                                    <li class="text">Menu Description: <span class="error">*</span>
                                    </li>
                                    <li class="field">
                                        <asp:TextBox CssClass="formfields" ID="txtMenuDescription" runat="server" MaxLength="150"
                                            Text='<%#Eval("MenuDescription") %>'></asp:TextBox>
                                    </li>
                                    <li class="text">Navigation URL:
                                    </li>
                                    <li class="field">
                                        <asp:TextBox CssClass="formfields" ID="txtNavigationURL" runat="server" MaxLength="200"
                                            Text='<%#Eval("NavigationURL") %>'></asp:TextBox>
                                    </li>
                                </ul>
                                <ul>
                                    <li class="text">Status:
                                    </li>
                                    <li class="field">
                                        <asp:CheckBox ID="chkStatus" runat="server" Checked='<%#Convert.ToString(Eval("Status"))=="True"?true :false %>' />
                                    </li>
                                    <li class="text">Allow In Menu:
                                    </li>
                                    <li class="field">
                                        <asp:CheckBox ID="chkAllowInMenu" runat="server" Checked='<%#Convert.ToString(Eval("AllowInMenu"))=="True"?true :false %>' />
                                    </li>
                                    <li class="text">Access Only For SuperAdmin:
                                    </li>
                                    <li class="field">
                                        <asp:CheckBox ID="chkAccessFor" runat="server" Checked='<%#Convert.ToString(Eval("AccessFor"))=="All"?false :true %>' />
                                    </li>
                                </ul>
                                <ul>
                                    <li class="text">Avilable for HO role only:
                                    </li>
                                    <li class="field">
                                        <asp:CheckBox ID="chkAccessRole" runat="server" Checked='<%#Convert.ToString(Eval("AccessRole"))=="Available for all"?false :true %>' />
                                    </li>
                                    <li class="text">Menu Type:
                                    </li>
                                    <li class="field">
                                        <asp:DropDownList ID="ddlMenuType" runat="server" CssClass="formselect">
                                            <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Web"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="WAP"></asp:ListItem>
                                        </asp:DropDownList>
                                    </li>
                                    <li class="text"></li>
                                    <li class="field">
                                        <asp:HiddenField ID="hdnMenuID" runat="server" Value='<%#Eval("MenuID") %>' />
                                        <asp:HiddenField ID="hdnParentMenuID" runat="server" Value='<%#Eval("ParentMenuID") %>' />
                                        <%--End --%>
                                        <div style="padding-top: 8px" class="elink2">
                                            <dx:ASPxTreeListTemplateReplacement ID="ASPxTreeListTemplateReplacement1" runat="server"
                                                ReplacementType="UpdateButton" />
                                            <dx:ASPxTreeListTemplateReplacement ID="ASPxTreeListTemplateReplacement2" runat="server"
                                                ReplacementType="CancelButton" />
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </EditForm>
                </Templates>
                <Columns>
                    <dx:TreeListTextColumn CellStyle-Wrap="True" Caption="Menu Name" FieldName="MenuName"
                        VisibleIndex="0" CellStyle-CssClass="">
                        <EditFormSettings VisibleIndex="0" />
                        <CellStyle Wrap="True">
                        </CellStyle>
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn CellStyle-Wrap="True" Caption="Menu Description" FieldName="MenuDescription"
                        VisibleIndex="1" CellStyle-CssClass="">
                        <EditFormSettings VisibleIndex="1" />
                        <CellStyle Wrap="True">
                        </CellStyle>
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn Caption="Access For" FieldName="AccessFor" VisibleIndex="2">
                        <EditFormSettings VisibleIndex="2" />
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn Caption="Allow In Menu" FieldName="AllowInMenu" VisibleIndex="3">
                        <EditFormSettings VisibleIndex="3" />
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn Caption="Status" FieldName="Status" VisibleIndex="4">
                        <EditFormSettings VisibleIndex="4" />
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn Caption="Menu Type" FieldName="MenuType" VisibleIndex="5">
                        <EditFormSettings VisibleIndex="5" />
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn Caption="Access Role" FieldName="AccessRole" VisibleIndex="6">
                        <EditFormSettings VisibleIndex="6" />
                    </dx:TreeListTextColumn>
                    <dx:TreeListCommandColumn VisibleIndex="7" ShowNewButtonInHeader="True" DeleteButton-Visible="False">
                        <EditButton Visible="True">
                        </EditButton>
                        <NewButton Visible="True">
                        </NewButton>
                        <%--<DeleteButton Visible="false" >
                                </DeleteButton>--%>
                    </dx:TreeListCommandColumn>
                </Columns>
            </dx:ASPxTreeList>
        </div>
    </div>
    <div class="clear" style="height: 10px;">
    </div>
    <div class="float-right">
        <div class="float-margin">
            Move Selected Node :
        </div>
        <div class="float-margin">
            <asp:ImageButton ID="imgbtnUP" runat="server" OnClick="imgbtnUP_Click" ToolTip="UP"
                Height="16px" Width="16px" ImageUrl="~/Assets/ZedSales/CSS/Images/up.png" />
        </div>
        <div class="float-margin">
            <asp:ImageButton ID="imgbtnDown" runat="server" OnClick="imgbtnDown_Click"
                ToolTip="Down" Height="16px" Width="16px" ImageUrl="~/Assets/ZedSales/CSS/Images/down.png" />&nbsp;
                    
        </div>
        <div class="float-left">
            <asp:Button ID="btnDelete" runat="server" Text="Delete Selected Node" CssClass="buttonbg"
                OnClick="btnDelete_Click" />
        </div>
        <div class="clear"></div>
    </div>
    <div class="clear"></div>

    <%--   </ContentTemplate>
</asp:UpdatePanel>--%>
</asp:Content>
