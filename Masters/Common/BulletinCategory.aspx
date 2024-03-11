<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="BulletinCategory.aspx.cs" Inherits="Masters_Common_BulletinCategory" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="UcMsg" runat="server" />
            <div class="mainheading">
                Add / Edit Manage Category
            </div>
            <div class="contentbox">
                <div class="mandatory">
                        (*) Marked fields are mandatory
                    </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Category Name:<span class="error2">*</span>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtCategoryName" runat="server" CssClass="formfields"></asp:TextBox>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valcategory" ControlToValidate="txtCategoryName"
                                    CssClass="error" ErrorMessage="Please insert a Category "
                                    ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">Status:                         
                            <asp:CheckBox ID="chkStatus" runat="server" Checked="true" />
                        </li>
                        </ul>
                        <div class="setbbb">
                            <div class="float-margin">
                                <asp:Button ID="btnSubmit" runat="server" CausesValidation="true"
                                    CssClass="buttonbg" Text="Submit" OnClick="btnSubmit_Click"
                                    ValidationGroup="insert" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg"
                                    Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" />
                            </div>
                        </div>
                    
                </div>
            </div>
            <div class="mainheading">
                Search Category
            </div>
            <div class="contentbox">
                <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">
                                    <asp:Label ID="lblSerCategory" Text="Category Name:" runat="server" />
                                </li>
                                <li class="field">
                                    <asp:TextBox ID="txtSerCAT" runat="server" CssClass="formfields"></asp:TextBox>
                                </li>
                                <li class="field3">
                                    <div class="float-margin">
                                        <asp:Button ID="btnserchC" Text="Search" runat="server" OnClick="btnserCategory_Click"
                                            CssClass="buttonbg" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="getalldata" Text="View All " runat="server" OnClick="btngetalldata_click"
                                            CssClass="buttonbg" />
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnserchC" />
                          <asp:PostBackTrigger ControlID="getalldata" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="mainheading">
                List
            </div>
            <div class="export">
                <asp:Button ID="exporttoexel" Text="" runat="server" OnClick="exporttoexel_Click"
                    CssClass="excel" />
            </div>
            <asp:UpdatePanel ID="updgrdView" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="contentbox">
                        <div class="grid1">
                            <asp:GridView ID="grdCAT" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                                AutoGenerateColumns="false" bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                                DataKeyNames="CategoryID" FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left"
                                FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-CssClass="gridheader"
                                HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow"
                                RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected"
                                Width="100%" OnPageIndexChanging="grdCAT_PageIndexChanging" OnRowCommand="grdCAT_RowCommand"
                                EmptyDataText="No Record" PageSize='<%$ AppSettings:GridViewPageSize %>'>
                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                <Columns>
                                    <asp:BoundField DataField="CategoryName" HeaderStyle-HorizontalAlign="Left" HeaderText="Category Name"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                        <ItemStyle Wrap="False" />
                                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgActive" runat="server" CausesValidation="false" CommandArgument='<%#Eval("CategoryID") %>'
                                                CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                                ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                        <ItemStyle Wrap="False" />
                                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="img1" runat="server" CausesValidation="false"
                                                CommandArgument='<%#Eval("CategoryID") %>' CommandName="cmdEdit"
                                                ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <PagerStyle CssClass="PagerStyle" />
                                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                <AlternatingRowStyle CssClass="Altrow" />
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdCAT" />
                </Triggers>
            </asp:UpdatePanel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="exporttoexel" />
            <asp:PostBackTrigger ControlID="btnSubmit" />
            <asp:PostBackTrigger ControlID="btnCancel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

