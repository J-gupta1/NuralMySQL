<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageModel.aspx.cs" Inherits="Masters_HO_Common_ManageModel" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Add / Edit Model
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblbrand" runat="server" Text="">Brand:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbInsertBrand" CssClass="formselect" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valbrand" ControlToValidate="cmbInsertBrand"
                                    CssClass="error" ErrorMessage="Please select a Brand " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblprodcat" runat="server" Text="">Product Category:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbInsertProdCat" runat="server" CssClass="formselect" AutoPostBack="True" 
                                    OnSelectedIndexChanged="cmbInsertProdCat_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valProdCat" ControlToValidate="cmbInsertProdCat"
                                    CssClass="error" ErrorMessage="Please select a Product Category " InitialValue="0"
                                    ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblinsproduct" runat="server" Text="">Product:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbinsertProduct" runat="server" CssClass="formselect" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valProduct" ControlToValidate="cmbinsertProduct"
                                    CssClass="error" ErrorMessage="Please select a Product " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="Label1" runat="server" Text="">Model Type:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlModeltype" CssClass="formselect" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="label2" runat="server" Text="">Model Mode:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlModelMode" CssClass="formselect" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblinsname" runat="server" Text="">Model Name:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtModelName" runat="server" CssClass="formfields" MaxLength="70"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valname" ControlToValidate="txtModelName"
                                    CssClass="error" ErrorMessage="Please insert Model Name " ValidationGroup="insert" />
                                <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtModelName"
                                    CssClass="error" ErrorMessage="Invalid Model Name" ValidationExpression="[^()<>/\@%]{1,70}"
                                    ValidationGroup="insert" runat="server" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="labelinscode" runat="server" Text="">Model Code:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtModelCode" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valcode" ControlToValidate="txtModelCode"
                                    CssClass="error" ErrorMessage="Please insert Model Code " ValidationGroup="insert" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtModelCode"
                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,20}"
                                    ValidationGroup="insert" runat="server" />
                            </div>
                        </li>
                        <li class="text">Status:
                        </li>
                        <li class="field">
                            <asp:CheckBox ID="chkstatus" runat="server" />
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btninsert_click"
                                    ValidationGroup="insert" CssClass="buttonbg" CausesValidation="true" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                    CssClass="buttonbg" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>

            <%--#CC01 Add Start --%>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSubmit" />
            </Triggers>
            <%--#CC01 Add End --%>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="mainheading">
                Search Model
            </div>
            <div class="contentbox">
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblserBrand" runat="server" Text="Brand:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbserBrand" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblserprodcat" runat="server" Text="Product Category:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerProdCat" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblserProduct" runat="server" Text="Product:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbserproduct" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblsername" runat="server" Text="Model Name:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtserName" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblserCode" runat="server" Text="Model Code:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtserCode" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSerchModel" Text="Search" runat="server" OnClick="btnSerchBodel_Click"
                                    CssClass="buttonbg" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnshowall" Text="Show All Data" runat="server" OnClick="btnshowalldata_Click"
                                    CssClass="buttonbg" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </ContentTemplate>

        <%--#CC01 Add Start --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSerchModel" />
        </Triggers>
        <%--#CC01 Add End --%>
    </asp:UpdatePanel>
    <div class="mainheading">
        Details
    </div>
    <div class="export">
        <asp:Button ID="exportToExel" Text="" runat="server" OnClick="exportToExel_Click"
            CssClass="excel" />
    </div>
    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="contentbox">
                <div class="grid1">
                    <asp:GridView ID="grdModel" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                        BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="ModelID" FooterStyle-HorizontalAlign="Left"
                        FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                        HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="Altrow"
                        RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                        RowStyle-VerticalAlign="top" Width="100%" PageSize='<%$ AppSettings:GridViewPageSize %>'
                        OnRowCommand="grdModel_RowCommand" EmptyDataText="No record found" OnPageIndexChanging="grdModel_PageIndexChanging">
                        <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                        <Columns>
                            <asp:BoundField DataField="ModelCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Model Code "
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ModelName" HeaderStyle-HorizontalAlign="Left" HeaderText="Model Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductName" HeaderStyle-HorizontalAlign="Left" HeaderText="Product"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductCategoryName" HeaderStyle-HorizontalAlign="Left"
                                HeaderText="Product Category" HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="BrandName" HeaderStyle-HorizontalAlign="Left" HeaderText="Brand"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ModelType" HeaderStyle-HorizontalAlign="Left" HeaderText="ModelType"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ModelMode" HeaderStyle-HorizontalAlign="Left" HeaderText="ModelMode"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ModelID") %>'
                                        CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ModelID") %>'
                                        CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
        <%--#CC01 Add Start --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="grdModel" />
        </Triggers>
        <%--#CC01 Add End --%>
        <%-- <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />
                                            </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
