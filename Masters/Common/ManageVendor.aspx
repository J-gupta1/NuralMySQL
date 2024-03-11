<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageVendor.aspx.cs" Inherits="Masters_Common_ManageVendor" %>

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
                    Add / Edit Product
                </div>
                <div class="contentbox">
                     <div class="mandatory">
                        (*) Marked fields are mandatory
                    </div>
                    <div class="H25-C3-S">
                        <ul>
                            <li class="text">
                                <asp:Label ID="lblproductnm" runat="server"
                                    CssClass="formtext">Vendor Name:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtVendorName" runat="server" CssClass="formfields" MaxLength="70"
                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqVendorName" runat="server" ControlToValidate="txtVendorName"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter Vendor  Name."
                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator><br />
                                <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtVendorName" CssClass="error"
                                    ErrorMessage="Invalid Vendor Name " ValidationExpression="[^()<>/\@%]{1,70}" ValidationGroup="AddUserValidationGroup"
                                    runat="server" />
                            </li>
                            <li class="text">
                                <asp:Label ID="lblProductCode" runat="server"
                                    CssClass="formtext">Vendor Code:<span class="error">*</span></asp:Label>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtVendorCode" runat="server" CssClass="formfields"
                                    ValidationGroup="AddUserValidationGroup" MaxLength="20"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="reqProductCode" runat="server" ControlToValidate="txtVendorCode"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter Vendor code."
                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator><br />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtVendorCode" CssClass="error"
                                    ErrorMessage="Invalid Vendor Code " ValidationExpression="[^()<>/\@%]{1,20}" ValidationGroup="AddUserValidationGroup"
                                    runat="server" />
                            </li>
                            <li class="text">
                                <asp:Label ID="lblchkActive" runat="server" AssociatedControlID="chkActive" CssClass="formtext"> Status:</asp:Label></li>
                            <li class="field">
                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" /></li>
                        </ul>
                        <ul>
                            <li class="text"></li>
                            <li class="field">
                                <asp:Button ID="btnCreate" Text="Submit" runat="server" CausesValidation="True" ValidationGroup="AddUserValidationGroup"
                                    ToolTip="Add " CssClass="buttonbg" OnClick="btnCreate_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel"
                                    CssClass="buttonbg" OnClick="btncancel_click" />
                            </li>

                        </ul>
                    </div>
                </div>
            </ContentTemplate>
            <%-- <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                </Triggers>--%>
        </asp:UpdatePanel>

        <div class="mainheading">
            Search Product
        </div>
        <div class="contentbox">
            <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                   <div class="H20-C3-S">
                        <ul>
                            <li class="text">Vendor Name:
                            </li>
                             <li class="field">
                                <asp:TextBox ID="txtSerName" runat="server" MaxLength="100" CssClass="formfields"> </asp:TextBox>
                            </li>
                            <li class="text">Vendor Code:
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtSerCode" runat="server" CssClass="formfields" MaxLength="100"> </asp:TextBox>
                            </li>
                            <li class="field3">
                                <div class="float-margin">
                                    <asp:Button ID="btnserch" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                        OnClick="btnSearch_Click"></asp:Button>
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="getalldata" Text="Show All Data" runat="server"
                                        ToolTip="Search" CssClass="buttonbg"
                                        OnClick="btnGetAlldata_Click"></asp:Button>
                                </div>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>             
        <div class="mainheading">
            List
        </div>
        <div class="export">
            <asp:Button ID="Exporttoexcel" runat="server" CausesValidation="False" ToolTip=""
                CssClass="excel" Text="" OnClick="Exporttoexcel_Click"></asp:Button>
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="grdVendor" runat="server" FooterStyle-VerticalAlign="Top"
                            FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                            AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                            HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                            BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                            SelectedStyle-CssClass="gridselected" DataKeyNames="VendorID" EmptyDataText="No record found"
                            OnRowCommand="grdProduct_RowCommand" PageSize='<%$ AppSettings:GridViewPageSize %>'
                            OnPageIndexChanging="grdProduct_PageIndexChanging"
                            OnSelectedIndexChanged="grdProduct_SelectedIndexChanged">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="VendorCode"
                                    HeaderText="Vendor Code">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="VendorName"
                                    HeaderText="Vendor Name">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnActiveDeactive" runat="server" CommandArgument='<%#Eval("VendorID") %>'
                                            CommandName="Active" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%#Eval("VendorID") %>' runat="server" ID="btnEdit" CommandName="cmdEdit"
                                            ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>'
                                            ToolTip="Edit User" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <PagerStyle CssClass="PagerStyle" />
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                            <AlternatingRowStyle CssClass="Altrow" />
                        </asp:GridView>
                    </ContentTemplate>
                    <%-- <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />
                                            </Triggers>--%>
                </asp:UpdatePanel>
            </div>
        </div>
        <%--     <uc3:footer ID="Footer1" runat="server" />--%>
    </div>

</asp:Content>

