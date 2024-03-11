<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/ReportPage.master"
    AutoEventWireup="true" CodeFile="ManageUserMenuTree.aspx.cs" Inherits="Masters_HO_Admin_ManageUserMenuTree" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <uc1:ucMessage ID="ucMsg" runat="server" />
    </div>
    <div class="mainheading">
        Search Panel
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C2">
            <ul>
                <li class="text">Company Name/Access key:<span class="error">*</span></li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="ddlCompanyName" runat="server" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged" AutoPostBack="true" CssClass="formselect">
                            </asp:DropDownList>

                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="rfvtxtmenuname" runat="server" ErrorMessage="Please Select Company Name"
                                ControlToValidate="ddlCompanyName" SetFocusOnError="True" ValidationGroup="aa" CssClass="error" Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                    </li>


                <li class="text">Select Role For RoleMenu Mapping: <span class="error">* </span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="formselect" AutoPostBack="True" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged"
                        ValidationGroup="aa">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select a Role" CssClass="error"
                        ControlToValidate="ddlRole" InitialValue="0" SetFocusOnError="True" ValidationGroup="aa"></asp:RequiredFieldValidator>
                </li>
                <li class="field3">
                    <asp:Button ID="btnUpdate" runat="server" Text="Update Menu Mapping" CssClass="buttonbg"
                        OnClick="btnUpdate_Click" ValidationGroup="aa" />
                </li>
            </ul>
        </div>
    </div>
    <div class="mainheading">
        View                  
    </div>
    <div class="contentbox">
        <div class="grid1">
            <dx:ASPxTreeList ID="ASPxTreeList1" runat="server" AutoGenerateColumns="False" Width="100%"
                KeyFieldName="MenuID" ParentFieldName="ParentMenuID" CssFilePath="~/App_Themes/SoftOrange/{0}/styles.css"
                CssPostfix="SoftOrange" OnHtmlDataCellPrepared="ASPxTreeList1_HtmlDataCellPrepared">
                <Styles CssFilePath="~/App_Themes/SoftOrange/{0}/styles.css" CssPostfix="SoftOrange">
                </Styles>
                <Images SpriteCssFilePath="~/App_Themes/SoftOrange/{0}/sprite.css">
                    <LoadingPanel Url="~/App_Themes/SoftOrange/TreeList/Loading.gif">
                    </LoadingPanel>
                </Images>
                <SettingsLoadingPanel ImagePosition="Top" />
                <SettingsSelection Enabled="True" Recursive="True" />
                <Settings GridLines="Both" SuppressOuterGridLines="True" />
                <SettingsBehavior ExpandCollapseAction="NodeDblClick" />
                <Columns>
                    <dx:TreeListTextColumn CellStyle-Wrap="True" Caption="Menu Name"
                        FieldName="MenuName" VisibleIndex="0" CellStyle-CssClass="" ReadOnly="True">
                        <EditFormSettings VisibleIndex="0" />
                        <CellStyle Wrap="True"></CellStyle>
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn Caption="Staus" FieldName="Status" VisibleIndex="1">
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn Caption="Allow In Menu" FieldName="AllowInMenu" VisibleIndex="2">
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn Caption="Access For" FieldName="AccessFor" VisibleIndex="3">
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn Caption="Remarks" FieldName="Remarks" VisibleIndex="4">
                    </dx:TreeListTextColumn>
                </Columns>
            </dx:ASPxTreeList>
        </div>
    </div>
    <div class="clear" style="height: 10px;">
    </div>
    <div class="float-right">
        <%--Move Selected Node :
                    <asp:ImageButton ID="imgbtnUP" runat="server" OnClick="imgbtnUP_Click"
                        ToolTip="UP" Height="16px" Width="16px"
                        ImageUrl="~/Assets/ZedSales/CSS/Images/up.png"/>
                    &nbsp;<asp:ImageButton ID="imgbtnDown" runat="server" OnClick="imgbtnDown_Click"
                        ToolTip="Down" Height="16px"  Width="16px"
                        ImageUrl="~/Assets/ZedSales/CSS/Images/down.png"/>&nbsp;--%>
    </div>
    <div class="clear">
    </div>
</asp:Content>