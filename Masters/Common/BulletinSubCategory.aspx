<%@ Page Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="BulletinSubCategory.aspx.cs" Inherits="Masters_Common_BulletinSubCategory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">

    <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Always">
        <ContentTemplate>

            <uc1:ucMessage ID="ucMsg" runat="server" />

            <div class="mainheading">
                Add / Edit Manage Sub Category
            </div>

            <div class="contentbox">
                <div class="mandatory">
                    (*) Marked fields are mandatory
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblcategoryname" runat="server" Text="">Category Name: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbSelectCat" runat="server" CssClass="formselect" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valCat" ControlToValidate="cmbSelectCat"
                                    InitialValue="0" CssClass="error" ErrorMessage="Please select a Category " ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblSubCatname" runat="server" Text="">Sub Category Name:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtInsertSubCat" runat="server" CssClass="formfields" ValidationGroup="insert"></asp:TextBox>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valsubname" ControlToValidate="txtInsertSubCat"
                                    ErrorMessage="Please insert a sub category" ValidationGroup="insert" />
                            </div>
                        </li>
                            <li class="text">Status: <span class="error">&nbsp;</span>

                            <asp:CheckBox ID="chkstatus" Text="Active" runat="server" Checked="true" />
                        </li>
                    </ul>
              
                        <div class="setbbb">
                            <div class="float-margin">
                                <asp:Button ID="btnSubmit" Text="Submit" runat="server" ValidationGroup="insert"
                                    CssClass="buttonbg" CausesValidation="true" OnClick="btnSubmit_Click" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="Button2" Text="Cancel" runat="server" CssClass="buttonbg" OnClick="btnCancel_Click" />
                            </div>
                        </div>
                   
                </div>
            </div>

            <div class="mainheading">
                Search Sub Category
            </div>
            <div class="contentbox">
                <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">
                                    <asp:Label ID="lblSerCat" Text="Category Name:" runat="server" />
                                </li>
                                <li class="field">
                                    <asp:DropDownList ID="cmbSerCat" runat="server" CssClass="formselect">
                                    </asp:DropDownList>
                                </li>
                                <li class="text">
                                    <asp:Label ID="lblSerSubCategory" Text="Sub Category Name:" runat="server" />
                                </li>
                                <li class="field">
                                    <asp:TextBox ID="txtSerSubCat" runat="server" CssClass="formfields"></asp:TextBox>
                                </li>
                                <li class="field3">
                                    <div class="float-margin">
                                        <asp:Button ID="btnSerchD" Text="Search" runat="server" CssClass="buttonbg" OnClick="btnSerchD_Click1" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="getalldata" Text="View All" runat="server" CssClass="buttonbg" OnClick="getalldata_Click" />
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSerchD" />
                        <asp:PostBackTrigger ControlID="getalldata" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="mainheading">
                List
            </div>
            <div class="export">
                <asp:Button ID="exporttoexel" Text="" runat="server" OnClick="exporttoexel_Click"
                    CssClass="excel" CausesValidation="False" />
            </div>
            <asp:UpdatePanel ID="updgrid" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="contentbox">
                        <div class="grid1">
                            <asp:GridView ID="grdSubCat" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="SubCategoryID"
                                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="None"
                                HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                                RowStyle-VerticalAlign="top" Width="100%" AlternatingRowStyle-CssClass="Altrow"
                                RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                                OnRowCommand="grdSubCat_RowCommand" OnPageIndexChanging="grdSubCat_PageIndexChanging"
                                EmptyDataText="No Record">
                                <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                                <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                                <Columns>
                                    <asp:BoundField DataField="CategoryName" HeaderStyle-HorizontalAlign="Left" HeaderText="Category Name "
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SubCategoryName" HeaderStyle-HorizontalAlign="Left" HeaderText="Sub Category Name"
                                        HtmlEncode="true">
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                        <ItemStyle Wrap="False" />
                                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SubCategoryID") %>'
                                                CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                                ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                        <ItemStyle Wrap="False" />
                                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SubCategoryID") %>'
                                                CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                                <PagerStyle CssClass="PagerStyle" />
                                <AlternatingRowStyle CssClass="Altrow" />
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdSubCat" />
                </Triggers>
            </asp:UpdatePanel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="exporttoexel" />
            <asp:PostBackTrigger ControlID="btnSubmit" />
            <asp:PostBackTrigger ControlID="Button2" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
