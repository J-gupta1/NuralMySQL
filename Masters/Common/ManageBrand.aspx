<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="ManageBrand.aspx.cs" Inherits="Masters_HO_Common_ManageBrand" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <%-- <uc2:header ID="Header1" runat="server" />--%>
        <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
            <ContentTemplate>
                <uc1:ucMessage ID="ucMessage1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mainheading">
                    Add / Edit Brand
                </div>
                <div class="contentbox">
                    <div class="mandatory">
                        (*) Marked fields are mandatory            
                    </div>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblbrandnm" runat="server" AssociatedControlID="txtBrandName"
                                    CssClass="formtext">Brand Name:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtBrandName" runat="server" CssClass="formfields" MaxLength="70"
                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqBrandName" runat="server" ControlToValidate="txtBrandName"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter brand Name."
                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtBrandName" CssClass="error"
                                    ErrorMessage="Invalid Brand Name" ValidationExpression="[^()<>/\@%]{1,70}" ValidationGroup="AddUserValidationGroup" Display="Dynamic"
                                    runat="server" />
                            </li>
                            <li class="text">
                                <asp:Label ID="lblBrandCode" runat="server" AssociatedControlID="txtBrandCode"
                                    CssClass="formtext">Brand Code:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtBrandCode" runat="server" CssClass="formfields" MaxLength="20"
                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqBrandCode" runat="server" ControlToValidate="txtBrandCode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter brand code."
                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtBrandCode" CssClass="error" Display="Dynamic"
                                    ErrorMessage="Invalid Brand Code" ValidationExpression="[^()<>/\@%]{1,20}" ValidationGroup="AddUserValidationGroup"
                                    runat="server" />
                            </li>
                            <li class="text">
                                <div class="float-margin">
                                    <asp:Label ID="lblchkActive" runat="server" AssociatedControlID="chkActive" CssClass="formtext"> Status :</asp:Label>
                                </div>
                                <div class="float-left">
                                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                </div>
                            </li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnCreate" Text="Submit" runat="server" CausesValidation="true" ValidationGroup="AddUserValidationGroup"
                                        ToolTip="Add " CssClass="buttonbg" OnClick="btnCreate_Click" />
                                </div>
                                <div class="float-left">
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
            Search Brand
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="H20-C3-S">
                        <ul>
                            <li class="text">Brand Name:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtSerBrandName" runat="server" MaxLength="100" CssClass="formfields"> </asp:TextBox>
                            </li>
                            <li class="text">Brand Code:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtSerBrandCode" runat="server" CssClass="formfields" MaxLength="100"> </asp:TextBox>
                            </li>
                            <li class="field3">
                                <div class="float-margin">
                                    <asp:Button ID="btnserch" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                        OnClick="btnSearch_Click"></asp:Button>
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="getalldata" Text="Show All Data" runat="server" ToolTip="Search" CssClass="buttonbg"
                                        OnClick="btngetalldata_Click"></asp:Button>
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
                        <asp:GridView ID="grdBrand" runat="server" FooterStyle-VerticalAlign="Top"
                            FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                            AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                            HeaderStyle-CssClass="gridheader" EmptyDataText="No data found" CellSpacing="1" CellPadding="4" bgcolor=""
                            BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True" PageSize='<%$ AppSettings:GridViewPageSize %>'
                            SelectedStyle-CssClass="gridselected" DataKeyNames="BrandID"
                            OnRowCommand="grdBrand_RowCommand"
                            OnPageIndexChanging="grdBrand_PageIndexChanging">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BrandCode"
                                    HeaderText="Brand Code"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BrandName"
                                    HeaderText="Brand Name"></asp:BoundField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnActiveDeactive" runat="server" CommandArgument='<%#Eval("BrandID") %>'
                                            CommandName="Active" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%#Eval("BrandID") %>' runat="server" ID="btnEdit" CommandName="cmdEdit"
                                            ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>'
                                            ToolTip="Edit User" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="grdBrand" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <%--     <uc3:footer ID="Footer1" runat="server" />--%>
    </div>
</asp:Content>
