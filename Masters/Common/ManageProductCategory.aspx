<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageProductCategory.aspx.cs" Inherits="Masters_HO_Common_ManageProductCategory" %>


<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<style type="text/css">
        .style1
        {
            width: 321px;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <%-- <uc2:header ID="Header1" runat="server" />--%>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMessage2" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mainheading">
                    Add / Edit Product Category
                </div>
                <div class="contentbox">
                    <div class="mandatory">
                        (*) Marked fields are mandatory            
                    </div>
                    <div class="H25-C3">
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblprodcatnm" runat="server" AssociatedControlID="txtProdCatName"
                                    CssClass="formtext">Product Category Name: <span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtProdCatName" runat="server" CssClass="formfields" MaxLength="70"
                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqProdCatName" runat="server" ControlToValidate="txtProdCatName"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter Product Category Name."
                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtProdCatName" CssClass="error" Display="Dynamic"
                                    ErrorMessage="Invalid Product Category name" ValidationExpression="[^()<>/\@%]{1,70}" ValidationGroup="AddUserValidationGroup"
                                    runat="server" />
                            </li>
                            <li class="text">
                                <asp:Label ID="lblProdCatCode" runat="server" AssociatedControlID="txtProdCatCode"
                                    CssClass="formtext">Product Category Code:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtProdCatCode" runat="server" CssClass="formfields"
                                    ValidationGroup="AddUserValidationGroup" MaxLength="20"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqProdCatCode" runat="server" ControlToValidate="txtProdCatCode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter product category code."
                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtProdCatCode" CssClass="error" Display="Dynamic"
                                    ErrorMessage="Invalid Product catecory Code" ValidationExpression="[^()<>/\@%]{1,20}" ValidationGroup="AddUserValidationGroup"
                                    runat="server" />
                            </li>
                            <li class="text">
                                <div class="float-margin">
                                    <asp:Label ID="lblchkActive" runat="server" AssociatedControlID="chkActive" CssClass="formtext"> Status: </asp:Label>
                                </div>
                                <div class="float-left">
                                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                </div>
                            </li>
                        </ul>
                        <ul>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnCreate" Text="Submit" runat="server" CausesValidation="True" ValidationGroup="AddUserValidationGroup"
                                        ToolTip="Add " CssClass="buttonbg" OnClick="btnCreate_Click" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel"
                                        CssClass="buttonbg" OnClick="btncancel_click" />
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnCreate" />
                <asp:PostBackTrigger ControlID="btnCancel" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="mainheading">
            Search Product Category
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="H20-C3">
                        <ul>
                            <li class="text">Product Category Name:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtSerProdCatName" runat="server" MaxLength="100" CssClass="formfields"> </asp:TextBox>
                            </li>
                            <li class="text">Product Category Code:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtSerProdCatCode" runat="server" CssClass="formfields" MaxLength="100"> </asp:TextBox>
                            </li>
                            <li class="field3">
                                <div class="float-margin">
                                    <asp:Button ID="btnserch" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                        OnClick="btnSearch_Click"></asp:Button>
                                </div>
                                <div class="float-left">
                                    <asp:Button ID="getalldata" Text="Show All Data" runat="server" ToolTip="Search" CssClass="buttonbg"
                                        OnClick="btnGetAlldata_Click"></asp:Button>
                                </div>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnserch" />
                    <asp:PostBackTrigger ControlID="getalldata" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="mainheading">
            List
        </div>
        <div class="export">
            <asp:Button ID="Exporttoexcel" runat="server" CausesValidation="False" ToolTip="Export to Excel"
                CssClass="excel" Text="" OnClick="Exporttoexcel_Click"></asp:Button>
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="grdProdCat" runat="server" FooterStyle-VerticalAlign="Top"
                            FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                            AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                            HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                            BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                            SelectedStyle-CssClass="gridselected" DataKeyNames="ProductCategoryID"
                            OnRowCommand="grdProdCat_RowCommand" EmptyDataText="No record found"
                            OnPageIndexChanging="grdProdCat_SelectedIndex" PageSize='<%$ AppSettings:GridViewPageSize %>'>
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ProductCategoryCode"
                                    HeaderText="Product Category Code"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ProductCategoryName"
                                    HeaderText="Product Category Name"></asp:BoundField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnActiveDeactive" runat="server" CommandArgument='<%#Eval("ProductCategoryID") %>'
                                            CommandName="Active" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%#Eval("ProductCategoryID") %>' runat="server" ID="btnEdit" CommandName="cmdEdit"
                                            ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>'
                                            ToolTip="Edit User" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="grdProdCat" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <%--     <uc3:footer ID="Footer1" runat="server" />--%>
    </div>
</asp:Content>

