<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageUserType_new.aspx.cs" Inherits="Masters_Common_ManageUserType_new" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
   <%-- <div class="headingarea">
        Manage User Menu
    </div>--%>
    <asp:UpdatePanel ID="updpnlSaveData" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="box1">
                <div class="subheading">
                    <asp:UpdatePanel ID="updpnlMsg" runat="server">
                        <ContentTemplate>
                        <div class="float-left">&nbsp;</div>
                        <div class="clear">
                            <uc1:ucMessage ID="ucMsg" runat="server" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="innerarea1">
                    <div class="fieldsarea22">
                        <ul>
                            <li class="text7">EntityType-Role :<span class="mandatory-img">&nbsp;</span></li>
                            <li class="field2">
                                <asp:DropDownList ID="ddlUserType" CssClass="formselect" runat="server">
                                </asp:DropDownList>
                                <br />
                                <asp:RequiredFieldValidator ID="reqddlUserType" runat="server" ControlToValidate="ddlUserType"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please select User Type!" InitialValue="0"
                                    ValidationGroup="search"></asp:RequiredFieldValidator>
                            </li>
                            <li class="text" style="width: 300px;">
                                <asp:Button CssClass="button2" ID="btnModuleSearch" runat="server" Text="Search"
                                    TabIndex="2" OnClick="btnModuleSearch_Click" ValidationGroup="search" />
                                <%--<asp:Button CssClass="button2" ID="btnModuleShowAll" runat="server" Text="Show All"
                                    TabIndex="3" OnClick="btnModuleShowAll_Click" />--%></li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="box1">
                <div class="subheading">
                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="gpMapping"
                        ShowMessageBox="True" ShowSummary="False" />
                    <asp:ValidationSummary ID="ValidationSummary4" runat="server" EnableClientScript="true"
                        Enabled="true" ShowMessageBox="true" ShowSummary="false" ValidationGroup="VgroupTypeModuleMapping" />
                </div>
                <div class="innerarea1">
                    <asp:Panel ID="Panel2" runat="server" Visible="false">
                        <div class="grid2">
                            <asp:GridView ID="gvList" runat="server" ShowFooter="True" AutoGenerateColumns="False"
                                DataKeyNames="usermoduleid" Width="100%" BorderWidth="0px" CellPadding="4"
                                CellSpacing="1" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                                AlternatingRowStyle-CssClass="Altrow" GridLines="none" FooterStyle-CssClass="gridfooter"
                                HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="left" RowStyle-VerticalAlign="Top"
                                SelectedRowStyle-CssClass="selectedrow" RowStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="left"
                                FooterStyle-VerticalAlign="Top" onrowdatabound="gvList_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Module List">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkBoxModule" runat="server" Text='<%#Eval("MenuName")%>' />
                                        </ItemTemplate>
                                       
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Module Permissions">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMenuID" runat="server" Text='<%#Eval("MenuID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblEntityTypeRoleID" runat="server" Text='<%#Eval("EntityTypeRoleID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblusermoduleid" runat="server" Text='<%#Eval("usermoduleid") %>' Visible="false"></asp:Label>
                                            <asp:CheckBoxList ID="chklstACLTag" RepeatDirection="Horizontal" runat="server" ></asp:CheckBoxList>
                                        </ItemTemplate>                                       
                                    </asp:TemplateField>                                   
                                </Columns>
                            </asp:GridView>
                        </div>
                        <ul>
                            <li>
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button2" OnClick="btnSubmit_Click" />
                            </li>
                        </ul>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
