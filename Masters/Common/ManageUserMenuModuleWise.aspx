<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageUserMenuModuleWise.aspx.cs" Inherits="Masters_HO_Common_ManageUserMenuModuleWise" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="contentbox margin-top">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
         <div class="H25-C4-S">
            <ul>
                <li class="text">Module: <span class="error">*</span>
                </li>
                <li class="text-field">
                    <asp:DropDownList ID="ddlMenu" CssClass="formselect" runat="server">
                    </asp:DropDownList>
                </li>
                <li class="field3">
                    <asp:Button ID="btnSearch" runat="server" CssClass="buttonbg" Text="Search" OnClick="btnSearch_Click" />
                </li>
            </ul>
        </div>
    </div>
    <div class="contentbox margin-top">
        <asp:Panel ID="Panel2" runat="server" Visible="false">
            <div class="grid1">
                <asp:GridView ID="gvList" runat="server" ShowFooter="True" AutoGenerateColumns="False"
                    DataKeyNames="RoleID" Width="100%" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                    HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow"
                    GridLines="none" FooterStyle-CssClass="gridfooter" HeaderStyle-VerticalAlign="Middle"
                    HeaderStyle-HorizontalAlign="left" RowStyle-VerticalAlign="Top" SelectedRowStyle-CssClass="selectedrow"
                    RowStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="left" FooterStyle-VerticalAlign="Top"
                    OnRowDataBound="gvList_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Entity Type-Role">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkBoxModule" runat="server" Text='<%#Eval("EntityTypeRoleName")%>' /><%-- Enabled="false" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Module Permissions">
                            <ItemTemplate>
                                <asp:Label ID="lblusermoduleid" runat="server" Text='<%#Eval("usermoduleid") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblEntityTypeRoleID" runat="server" Text='<%#Eval("RoleID") %>' Visible="false"></asp:Label>
                                <asp:CheckBoxList ID="chklstACLTag" RepeatDirection="Horizontal" CellPadding="4" runat="server">
                                </asp:CheckBoxList>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div style="height: 10px">
            </div>
            <div>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonbg" OnClick="btnSubmit_Click" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
