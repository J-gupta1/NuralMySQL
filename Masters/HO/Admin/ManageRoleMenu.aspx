<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageRoleMenu.aspx.cs" Inherits="Masters_HO_Admin_ManageRoleMenu" %>

<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel ID="updpnlSaveData" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMsg" runat="server" />
                <div class="mainheading">
                    Search
                </div>
                <div class="contentbox">
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">Role Name:
                            </li>
                            <li class="field">
                                <%--  <div style="float:left; width:135px;">--%><asp:DropDownList ID="ddlRole" CssClass="formselect" runat="server">
                                </asp:DropDownList><%--</div>--%>
                            </li>
                            <li class="field3">
                                <div class="float-margin">
                                    <asp:Button ID="btnModuleSearch" runat="server" Text="Search"
                                        TabIndex="2" OnClick="btnModuleSearch_Click" CssClass="buttonbg" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnModuleShowAll" runat="server" Text="Show All" CssClass="buttonbg"
                                        TabIndex="3" OnClick="btnModuleShowAll_Click" />
                                </div>
                            </li>
                        </ul>

                    </div>
                </div>
                <div class="mainheading">
                    Role Wise Menu Mapping
                </div>
                <div class="export">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="gpMapping"
                        ShowMessageBox="True" ShowSummary="False" />
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="gvMapping" runat="server" ShowFooter="True" AutoGenerateColumns="False"
                            DataKeyNames="RoleID" AllowPaging="True" PageSize="1" OnRowCommand="gvMapping_RowCommand"
                            OnRowCreated="gvMapping_RowCreated" OnRowDataBound="gvMapping_RowDataBound" OnRowCancelingEdit="gvMapping_RowCancelingEdit"
                            OnRowEditing="gvMapping_RowEditing" OnPageIndexChanging="gvMapping_PageIndexChanging"
                            OnRowUpdating="gvMapping_RowUpdating" Width="100%" BorderWidth="0px" CellPadding="4"
                            CellSpacing="1" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                            AlternatingRowStyle-CssClass="Altrow" GridLines="none" FooterStyle-CssClass="gridfooter"
                            HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="left" RowStyle-VerticalAlign="Top"
                            RowStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="left" FooterStyle-VerticalAlign="Top">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUsertypeId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RoleID")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Role Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUsertype" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RoleName")%>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEditUserType" runat="server" CssClass="formselect" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlUserType" CssClass="formselect" runat="server" />
                                        <asp:RequiredFieldValidator ID="rvUsertype" runat="server" ErrorMessage="User type required."
                                            ControlToValidate="ddlUserType" InitialValue="0" ValidationGroup="gpMapping"
                                            Display="None"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Menu Module &lt;span class='error'&gt;*&lt;/span&gt;">
                                    <ItemTemplate>
                                        <asp:DataList runat="server" DataKeyField="menuid" CssClass="content" ID="dlMenu"
                                            OnItemDataBound="dlMenu_ItemDataBound" AlternatingItemStyle-VerticalAlign="Top"
                                            HeaderStyle-VerticalAlign="Top" RepeatColumns="3" RepeatDirection="Horizontal">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMenuName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "menuname") %>'
                                                    Width="191px"></asp:Label><br />
                                                <asp:Panel ID="Panel1" runat="server" Height="100px" CssClass="content" ScrollBars="Vertical">
                                                    <asp:CheckBoxList ID="chkModules" CellPadding="2" runat="server">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <hr />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DataList runat="server" DataKeyField="menuid" ID="dlEditMenu" OnItemDataBound="dlEditMenu_ItemDataBound"
                                            RepeatColumns="3" RepeatDirection="Horizontal">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEditMenuName" CssClass="content" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "menuname") %>'
                                                    Width="191px"></asp:Label><br />
                                                <asp:Panel ID="Panel1" runat="server" Height="100px" CssClass="content" ScrollBars="Vertical">
                                                    <asp:CheckBoxList ID="chkEditModules" CellPadding="2" runat="server">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <br />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DataList runat="server" DataKeyField="menuid" ID="dlFooterMenu" OnItemDataBound="dlFooterMenu_ItemDataBound"
                                            RepeatColumns="3" RepeatDirection="Horizontal">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFooterMenuName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "menuname") %>'
                                                    Width="191px"></asp:Label><br />
                                                <asp:Panel ID="Panel1" runat="server" Height="100px" ScrollBars="Vertical">
                                                    <asp:CheckBoxList ID="chkFooterModules" CellPadding="2" runat="server">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <br />
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:CommandField HeaderText="Edit" ShowEditButton="True" ItemStyle-Width="100px">
                                    <ControlStyle CssClass="elink"  />
                                </asp:CommandField>
                                <asp:TemplateField ItemStyle-Width="80px">
                                    <FooterTemplate>
                                        <div style="width: 60px">
                                            <asp:Button ID="btnAdd" ValidationGroup="gpMapping" CssClass="buttonbg" runat="server"
                                                Text="+ Add" CommandName="btnAdd"></asp:Button>
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

