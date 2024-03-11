<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageSKU.aspx.cs" Inherits="Masters_HO_Common_ManageSKU" %>

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
        Add / Edit SKU
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblsku" runat="server" Text="">Product Category:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbInsertProdCat" CssClass="formselect" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="insertProdcat_selectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valprodcat" ControlToValidate="cmbInsertProdCat"
                                    CssClass="error" ErrorMessage="Please select a Product Category " InitialValue="0"
                                    ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblmodel" runat="server" Text="">Model:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbInsertModel" runat="server" CssClass="formselect" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valModel" ControlToValidate="cmbInsertModel"
                                    CssClass="error" ErrorMessage="Please select a Model " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblinsColor" runat="server" Text="">Color:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbInsertColor" runat="server" CssClass="formselect" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valColor" ControlToValidate="cmbInsertColor"
                                    CssClass="error" ErrorMessage="Please select a color " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblinssku" runat="server" Text="">SKU Name:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtInsertName" runat="server" CssClass="formfields" MaxLength="70"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valname" ControlToValidate="txtInsertName" Width="160px"
                                    CssClass="error" ErrorMessage="Please insert SKU Name " ValidationGroup="insert" />

                                <asp:RegularExpressionValidator ID="refname" ControlToValidate="txtInsertname" CssClass="error"
                                    ErrorMessage="Invalid SKU Name " ValidationExpression="[^<>@%]{1,70}" ValidationGroup="insert"
                                    runat="server" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="labelinscode" runat="server" Text="">SKU Code:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtInsertCode" runat="server" CssClass="formfields" MaxLength="20"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valcode" ControlToValidate="txtInsertCode" Width="160px"
                                    CssClass="error" ErrorMessage="Please insert SKU Code " ValidationGroup="insert" />

                                <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtInsertCode"
                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,20}"
                                    ValidationGroup="insert" runat="server" />
                            </div>
                        </li>
                        <li class="text">SKU Description: </li>
                        <li class="field">
                            <asp:TextBox ID="txtInsertDesc" runat="server" CssClass="formfields"
                                MaxLength="70"></asp:TextBox>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">Status:
                        </li>
                        <li class="field">
                            <asp:CheckBox ID="chkstatus" runat="server" />
                        </li>
                    </ul>
                     <%--#CC05 START ADDED--%>
                    <ul id="dvExpiryStatus" runat="server">
                        <li class="text">Vouchers ExpiryDate Status:
                        </li>
                        <li class="field">
                            <asp:CheckBox ID="ChkExpiryStatus" runat="server" />
                        </li>
                    </ul>
                     <%--#CC05 START ADDED--%>
                    <%--#CC02 START ADDED--%>

                    <ul id="dvCounterSize" runat="server" style="display: none;">
                        <li class="text">Carton Size:<span id="dvReq" runat="server"><span class="error">* &nbsp; </span></span>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtCartonSize" runat="server" MaxLength="5" CssClass="formfields"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="ReqtxtCartonSize" ControlToValidate="txtCartonSize" Width="160px"
                                CssClass="error" ErrorMessage="Please insert carton size " ValidationGroup="insert" />
                            <cc1:FilteredTextBoxExtender ID="FiltertxtCounterSize" runat="server" FilterType="Numbers" TargetControlID="txtCartonSize"></cc1:FilteredTextBoxExtender>
                        </li>
                    </ul>
                    <%--====#CC04 Added Started--%>
                    <ul>
                        <li class="text">Keyword:<span class="error">*</span></span></li>
                        <li class="field">
                            <asp:TextBox ID="txtKeyword" runat="server" CausesValidation="True" CssClass="form_textarea"
                                        ValidationGroup="insert" TextMode="MultiLine"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="error"
                                        ErrorMessage="Enter Keyword" ControlToValidate="txtKeyword" ValidationGroup="insert"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </li>
                    </ul>
                     <%--====#CC04 Added End--%>
                    <%--#CC02 END ADDED--%>
                </div>
                <div class="clear"></div>
                <div class="mainheading">
                    Aditional Attributes
                </div>
                 <div class="contentbox">                    
                    <div class="H20-C3-S">
                        <ul>
                            <li class="text">Attribute 1 :
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtProperty1" runat="server" CssClass="formfields" MaxLength="80"></asp:TextBox>
                            </li>
                            <li class="text">Attribute 2 :
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtProperty2" runat="server" CssClass="formfields" MaxLength="80"></asp:TextBox>
                            </li>
                        </ul>
                    </div>
                     <div class="clear"></div>
                </div>               
                <div class="margin-top">
                    <div class="float-margin">
                        <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btninsert_click"
                            ValidationGroup="insert" CssClass="buttonbg" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                            CssClass="buttonbg" />
                    </div>  
                </div>
            </ContentTemplate>
            <%--#CC01 Add Start --%>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSubmit" />
            </Triggers>
            <%--#CC01 Add End --%>
            <%-- <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                </Triggers>--%>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="mainheading">
                Search SKU
            </div>
            <div class="contentbox">
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblserprodcat" runat="server" Text="Product Category:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerProdCat" runat="server" OnSelectedIndexChanged="cmbSerProdcat_SelectedIndexChanged"
                                CssClass="formselect" AutoPostBack="true">
                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblsermodel" runat="server" Text="Model:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSerModel" runat="server" CssClass="formselect" AutoPostBack="True">
                            </asp:DropDownList>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblsercolor" runat="server" Text="Color :"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="cmbSercolor" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                    </ul>
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblsername" runat="server" Text="SKU Name:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerName" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblserCode" runat="server" Text="SKU Code:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerCode" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerchSku_Click"
                                    CssClass="buttonbg" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="fillallgrid" Text="Show All Data" runat="server" OnClick="btngetdata_Click"
                                    CssClass="buttonbg" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </ContentTemplate>
        <%--#CC01 Add Start --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
        </Triggers>
        <%--#CC01 Add End --%>
    </asp:UpdatePanel>

    <div class="mainheading">
        List
    </div>
    <div class="export">
        <asp:Button ID="exportToExel" Text="" runat="server" OnClick="exportToExel_Click"
            CssClass="excel" />
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grdSKU" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                        BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="SKUID" FooterStyle-HorizontalAlign="Left"
                        FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                        HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="Altrow"
                        RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                        RowStyle-VerticalAlign="top" Width="100%" OnRowCommand="grdSKU_RowCommand" EmptyDataText="No record found"
                        OnPageIndexChanging="grdSKUpage_indexchanging">
                        <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                        <Columns>
                            <asp:BoundField DataField="SKUCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Code "
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="SKUName" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="SKUDesc" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Description"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductCategoryName" HeaderStyle-HorizontalAlign="Left"
                                HeaderText="Product Category" HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductName" HeaderStyle-HorizontalAlign="Left" HeaderText="Product"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="BrandName" HeaderStyle-HorizontalAlign="Left" HeaderText="Brand"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ModelName" HeaderStyle-HorizontalAlign="Left" HeaderText="Model"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ColorName" HeaderStyle-HorizontalAlign="Left" HeaderText="Color"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Attribute1" HeaderStyle-HorizontalAlign="Left" HeaderText="Attribute1"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Attribute2" HeaderStyle-HorizontalAlign="Left" HeaderText="Attribute2"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <%--#CC02 START ADDED--%>
                            <asp:BoundField DataField="Carton Size" HeaderStyle-HorizontalAlign="Left" HeaderText="Carton Size"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <%--#CC02 END ADDED--%>
                             <%--#CC05  ADDED--%>
                             <asp:BoundField DataField="ShowingridExpiryDateStatus" HeaderStyle-HorizontalAlign="Left" HeaderText="ExpiryDate Status"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                             <%--#CC05 END ADDED--%>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SKUID") %>'
                                        CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SKUID") %>'
                                        CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </ContentTemplate>
                <%--#CC01 Add Start --%>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdSKU" />
                </Triggers>
                <%--#CC01 Add End --%>
                <%-- <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />
                                            </Triggers>--%>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
