<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/ReportPage.master"
    AutoEventWireup="true" CodeFile="ManageMenu-ClientValues.aspx.cs" Inherits="Masters_HO_Admin_ManageMenu_ClientValues" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:HiddenField ID="hdnProsess1" runat="server" />
    <div>
        <uc1:ucMessage ID="ucMsg" runat="server" />
    </div>
    <div class="clear"></div>
    <div class="mainheading">
        Add Client-Features
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Client Name:  <span class="error">* </span>
                </li>

                <li class="field">
                    <asp:DropDownList ID="ddlClientName" runat="server" CausesValidation="True" ValidationGroup="aa" CssClass="formselect">
                    </asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please aelect a client"
                            ControlToValidate="ddlClientName" InitialValue="0" SetFocusOnError="True" ValidationGroup="aa"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button ID="btnProceed" runat="server" Text="Proceed" OnClick="btnProceed_Click" CssClass="buttonbg"
                            ValidationGroup="aa" />
                    </div>
                    <div class="float-left">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="buttonbg" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div class="clear"></div>
    <div>
        <div class="col-30" style="height: auto">
            <div class="mainheading">
                Default Menu Tree
            </div>
            <div class="clear"></div>
            <div class="grid1">
                <dx:ASPxTreeList ID="ASPxTreeList1" runat="server" KeyFieldName="MenuID" Width="100%"
                    ParentFieldName="ParentMenuID" AutoGenerateColumns="False" CssFilePath="~/App_Themes/SoftOrange/{0}/styles.css"
                    CssPostfix="SoftOrange">
                    <SettingsSelection Enabled="True" Recursive="True" />
                    <Styles CssFilePath="~/App_Themes/SoftOrange/{0}/styles.css" CssPostfix="SoftOrange">
                    </Styles>
                    <Images SpriteCssFilePath="~/App_Themes/SoftOrange/{0}/sprite.css">
                        <LoadingPanel Url="~/App_Themes/SoftOrange/TreeList/Loading.gif">
                        </LoadingPanel>
                    </Images>
                    <SettingsLoadingPanel ImagePosition="Top" />
                    <Settings GridLines="Both" SuppressOuterGridLines="True" />
                    <SettingsBehavior ExpandCollapseAction="NodeDblClick" />
                    <SettingsPager Mode="ShowPager">
                    </SettingsPager>
                    <Columns>
                        <dx:TreeListTextColumn CellStyle-Wrap="True" Caption="Menu Name" FieldName="MenuName"
                            VisibleIndex="0" CellStyle-CssClass="" ReadOnly="True">
                            <EditFormSettings VisibleIndex="0" />
                            <CellStyle Wrap="True">
                            </CellStyle>
                        </dx:TreeListTextColumn>
                        <dx:TreeListTextColumn CellStyle-Wrap="True" Caption="Menu Description" FieldName="MenuDescription"
                            VisibleIndex="1" CellStyle-CssClass="" ReadOnly="True">
                            <EditFormSettings VisibleIndex="1" />
                            <CellStyle Wrap="True">
                            </CellStyle>
                        </dx:TreeListTextColumn>
                    </Columns>
                </dx:ASPxTreeList>
            </div>
        </div>
        <div class="col-10" style="height: auto; vertical-align: middle; padding-top: 3%">
            <asp:Button ID="btnload" runat="server" Text="Load &gt;" OnClick="btnload_Click" CssClass="buttonbg"
                ValidationGroup="aa" />
        </div>
        <div class="col-60" style="height: auto">
            <%-- <dx:ASPxTreeList ID="ASPxTreeList2" runat="server" KeyFieldName="MenuID" ParentFieldName="ParentMenuID"  
                    CssFilePath="~/App_Themes/SoftOrange/{0}/styles.css" CssPostfix="SoftOrange" 
                    Width="600px" AutoGenerateColumns="False">
                    <Styles CssFilePath="~/App_Themes/SoftOrange/{0}/styles.css" 
                        CssPostfix="SoftOrange">
                    </Styles>
                    <Images SpriteCssFilePath="~/App_Themes/SoftOrange/{0}/sprite.css">
                        <LoadingPanel Url="~/App_Themes/SoftOrange/TreeList/Loading.gif">
                        </LoadingPanel>
                    </Images>
                    <Settings GridLines="Both" SuppressOuterGridLines="True" />
                    <SettingsBehavior ExpandCollapseAction="NodeDblClick" />
                    <SettingsLoadingPanel ImagePosition="Top" />
                    <SettingsSelection Enabled="True" Recursive="True" />
                    <Settings SuppressOuterGridLines="True" />
                    <SettingsPager Mode="ShowPager">
                    </SettingsPager>
                    <Columns>
                        <dx:TreeListTextColumn CellStyle-Wrap="True" Caption="Menu Name" FieldName="MenuName"
                            VisibleIndex="0" CellStyle-CssClass="" ReadOnly="True">
                            <EditFormSettings VisibleIndex="0" />
                            <CellStyle Wrap="True">
                            </CellStyle>
                        </dx:TreeListTextColumn>
                        <dx:TreeListTextColumn CellStyle-Wrap="True" Caption="Menu Description" FieldName="MenuDescription"
                            VisibleIndex="1" CellStyle-CssClass="" ReadOnly="True">
                            <EditFormSettings VisibleIndex="1" />
                            <CellStyle Wrap="True">
                            </CellStyle>
                        </dx:TreeListTextColumn>
                        <dx:TreeListTextColumn Caption="Access Role" VisibleIndex="2" FieldName="AccessRole">
                        </dx:TreeListTextColumn>
                        <dx:TreeListTextColumn Caption="Allow In Menu" VisibleIndex="3" FieldName="AllowInMenu">
                        </dx:TreeListTextColumn>
                        <dx:TreeListTextColumn Caption="Status" VisibleIndex="4" FieldName="Status">
                        </dx:TreeListTextColumn>
                        <dx:TreeListCommandColumn Caption="Edit" VisibleIndex="5">
                            <EditButton Visible="True">
                            </EditButton>
                        </dx:TreeListCommandColumn>
                    </Columns>
                </dx:ASPxTreeList>--%>
            <div class="mainheading">
                Client Menu Tree
            </div>
            <div class="clear"></div>
            <div class="grid1">
                <dx:ASPxTreeList ID="ASPxTreeList2" runat="server" AutoGenerateColumns="False" Width="100%"
                    KeyFieldName="MenuID" ParentFieldName="ParentMenuID"
                    OnNodeUpdating="ASPxTreeList2_NodeUpdating" EnableCallbacks="False"
                    CssFilePath="~/App_Themes/SoftOrange/{0}/styles.css"
                    CssPostfix="SoftOrange" OnProcessDragNode="ASPxTreeList2_ProcessDragNode">
                    <SettingsEditing AllowNodeDragDrop="True" AllowRecursiveDelete="True" />
                    <Styles CssFilePath="~/App_Themes/SoftOrange/{0}/styles.css" CssPostfix="SoftOrange">
                    </Styles>
                    <Images SpriteCssFilePath="~/App_Themes/SoftOrange/{0}/sprite.css">
                        <LoadingPanel Url="~/App_Themes/SoftOrange/TreeList/Loading.gif">
                        </LoadingPanel>
                    </Images>
                    <SettingsLoadingPanel ImagePosition="Top" />
                    <SettingsSelection Enabled="True" />
                    <SettingsPager Mode="ShowPager">
                    </SettingsPager>
                    <Settings GridLines="Both" SuppressOuterGridLines="True" />
                    <SettingsBehavior ExpandCollapseAction="NodeDblClick" />
                    <SettingsEditing Mode="EditFormAndDisplayNode" />
                    <Templates>
                        <EditForm>
                            <%--Start  --%>
                            <div class="contentbox">
                                <div class="H25-C2">
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
                                    </ul>
                                    <ul>
                                        <li class="text">Avilable for HO role only:
                                        </li>
                                        <li class="field">
                                            <asp:CheckBox ID="chkAccessRole" runat="server" Checked='<%#Convert.ToString(Eval("AccessRole"))=="All"?false :true %>' />
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
                        <dx:TreeListTextColumn Caption="Access Role" FieldName="AccessRole" VisibleIndex="2">
                            <EditFormSettings VisibleIndex="2" />
                        </dx:TreeListTextColumn>
                        <dx:TreeListTextColumn Caption="Allow In Menu" FieldName="AllowInMenu" VisibleIndex="3">
                            <EditFormSettings VisibleIndex="3" />
                        </dx:TreeListTextColumn>
                        <dx:TreeListTextColumn Caption="Status" FieldName="Status" VisibleIndex="4">
                            <EditFormSettings VisibleIndex="4" />
                        </dx:TreeListTextColumn>
                        <dx:TreeListCommandColumn VisibleIndex="5" Caption="Edit">
                            <EditButton Visible="True">
                            </EditButton>
                        </dx:TreeListCommandColumn>
                    </Columns>
                </dx:ASPxTreeList>
            </div>
        </div>
        <div class="clear" style="height: 10px"></div>
    </div>
    <div>
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
                    ToolTip="Down" Height="16px" Width="16px" ImageUrl="~/Assets/ZedSales/CSS/Images/down.png" />
            </div>
            <div class="float-left">
                <asp:Button ID="btnDelete" runat="server" Text="Delete Selected Node" CssClass="buttonbg"
                    OnClick="btnDelete_Click" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="clear" style="height: 10px"></div>
    </div>

</asp:Content>
