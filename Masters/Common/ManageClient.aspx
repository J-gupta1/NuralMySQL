﻿<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageClient.aspx.cs" Inherits="Masters_Common_ManageClient" %>

<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:HiddenField ID="hdnManageClient" runat="server" />
    <div>
        <uc1:ucMessage ID="ucMsg" runat="server" />
    </div>
    <div class="mainheading">
        Add panel
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Client Name: <span class="error">* </span>
                </li>
                <li class="field">
                    <div>
                        <asp:TextBox ID="txtClientNm" runat="server" CssClass="formfields" MaxLength="100"></asp:TextBox>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="error"
                            ErrorMessage="Enter Client Name" ControlToValidate="txtClientNm" SetFocusOnError="True"
                            ValidationGroup="aa"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">Client Folder Name: <span class="error">* </span>
                </li>
                <li class="field">
                    <div>
                        <asp:TextBox ID="txtClientFolderNm" CssClass="formfields" runat="server" CausesValidation="True"
                            MaxLength="100" ValidationGroup="aa"></asp:TextBox>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="error" Display="Dynamic"
                           ErrorMessage="Enter Client Folder Name" ControlToValidate="txtClientFolderNm"
                            SetFocusOnError="True" ValidationGroup="aa"></asp:RequiredFieldValidator>

                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" CssClass="error" Display="Dynamic"
                            ControlToValidate="txtClientFolderNm" ErrorMessage="Do Not Put Space And Special Character"
                            SetFocusOnError="True" ValidationExpression="^[a-zA-Z]{1,100}$" ValidationGroup="aa"></asp:RegularExpressionValidator>
                    </div>
                </li>
                <li class="text">Application Title:  <span class="error">* </span>
                </li>
                <li class="field">
                    <div>
                        <asp:TextBox ID="txtApplicationTitle" CssClass="formfields" runat="server" CausesValidation="True"
                            MaxLength="100" ValidationGroup="aa"></asp:TextBox>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Enter Application Title"
                            CssClass="error" ControlToValidate="txtApplicationTitle" SetFocusOnError="True"
                            ValidationGroup="aa"></asp:RequiredFieldValidator>
                    </div>
                </li>
            </ul>
            <ul>
                <li class="text">Site Url:  <span class="error">* </span>
                </li>
                <li class="field">
                    <div>
                        <asp:TextBox ID="txtSiteUrl" runat="server" CssClass="formfields" CausesValidation="True"
                            MaxLength="300" ValidationGroup="aa"></asp:TextBox>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic"
                            ErrorMessage="Enter URL" ControlToValidate="txtSiteUrl" CssClass="error" SetFocusOnError="True"
                            ValidationGroup="aa"></asp:RequiredFieldValidator>

                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtSiteUrl" Display="Dynamic"
                            ErrorMessage="Incorrect URL" SetFocusOnError="True" CssClass="error" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?"
                            ValidationGroup="aa"></asp:RegularExpressionValidator>
                    </div>
                </li>
                <li class="text">Site Under Maintenance:
                </li>
                <li class="field">
                    <div style="margin-top: 0px;">
                        <asp:CheckBox ID="chkSUM" runat="server" />
                    </div>
                </li>
                <li class="text">Status:
                </li>
                <li class="field">
                    <div style="margin-top: 0px;">
                        <asp:CheckBox ID="chkStatus" runat="server" Checked="True" />
                    </div>
                </li>
            </ul>
            <ul>
                <li class="text">Email Signature:  </li>
                <li class="field">
                    <asp:TextBox ID="txtEmailSignature" runat="server" CausesValidation="True" CssClass="formfields"
                        MaxLength="100"></asp:TextBox>
                    <%--<div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="error"
                            ErrorMessage="Please Enter Email Signature"
                            ControlToValidate="txtEmailSignature" SetFocusOnError="True"
                            ValidationGroup="aa"></asp:RequiredFieldValidator>
                    </div>--%>
                </li>
                <li class="text">Copy Right Text:  </li>
                <li class="field">
                    <asp:TextBox ID="txtCopyRight" runat="server" CausesValidation="True" CssClass="formfields"
                        MaxLength="200"></asp:TextBox>
                    <%--<div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" CssClass="error"
                            ErrorMessage="Please Enter Copy Right Text" ControlToValidate="txtCopyRight"
                            SetFocusOnError="True" ValidationGroup="aa"></asp:RequiredFieldValidator>
                    </div>--%>
                </li>
                <li class="text">Company Logo</li>
                <li class="field">
                    <asp:FileUpload ID="FileUploadCompanyLogo" runat="server" CssClass="formfields" />
                    </li>
            </ul>
            <ul>
                <li class="field">
                    <div class="float-margin">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonbg" ValidationGroup="aa"
                            OnClick="btnSubmit_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnClear" runat="server" Text="Cancel" CssClass="buttonbg"
                            CausesValidation="False" OnClick="btnClear_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>

    <div class="mainheading">
        Company Settings
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:GridView ID="gvSettings" runat="server" AllowPaging="false" AlternatingRowStyle-CssClass="Altrow"
                AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                RowStyle-VerticalAlign="top" Width="100%" DataKeyNames="PropertyName" >
                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="PropertyName" HeaderStyle-HorizontalAlign="Left" HeaderText="Property Name"
                        HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:BoundField DataField="PropertyDescription" HeaderStyle-HorizontalAlign="Left" HeaderText="Description"
                        HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:TemplateField>
                        <HeaderTemplate>
                            Default Value
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID ="lblDefaultValue" Text='<%# DataBinder.Eval(Container.DataItem, "PropertyValue")%>' runat="server"></asp:Label>
                            
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Status
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID ="chkStatus" Checked='<%# Convert.ToBoolean( DataBinder.Eval(Container.DataItem, "Status"))%>' runat="server"></asp:CheckBox>
                            
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            New Value
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSameAsDefault" Text="Same as Default" runat="server" />
                            <asp:TextBox ID="txtNewPropertyValue" runat="server" MaxLength="100"></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    
                </Columns>
                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                <AlternatingRowStyle CssClass="Altrow" />
                <PagerStyle CssClass="PagerStyle" />
            </asp:GridView>

        </div>
        <div class="clear">
        </div>

    <div class="mainheading">
        View
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:GridView ID="gvManageClient" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                RowStyle-VerticalAlign="top" Width="100%" OnPageIndexChanging="gvManageClient_PageIndexChanging"
                OnRowCommand="gvManageClient_RowCommand">
                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Client Name
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="word-wrap: break-word; overflow: hidden; width: 100px;">
                                <%# DataBinder.Eval(Container.DataItem, "ClientName")%>
                            </div>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Client Folder Name
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="word-wrap: break-word; overflow: hidden; width: 100px;">
                                <%# DataBinder.Eval(Container.DataItem, "ClientFolderName")%>
                            </div>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Application Title
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="word-wrap: break-word; overflow: hidden; width: 100px;">
                                <%# DataBinder.Eval(Container.DataItem, "ApplicationTitle")%>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="SiteURL" HeaderStyle-HorizontalAlign="Left" HeaderText="Site URL"
                        HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:BoundField DataField="EmailSignature" HeaderStyle-HorizontalAlign="Left" HeaderText="Email Signature"
                        HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:BoundField DataField="CopyRightText" HeaderStyle-HorizontalAlign="Left" HeaderText="Copy Right Text"
                        HtmlEncode="true" HeaderStyle-Width="100px">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="Site Under Maintenance" ItemStyle-Width="55px">
                        <ItemStyle Wrap="False" />
                        <HeaderStyle HorizontalAlign="left" Width="55px" Wrap="True" />
                        <ItemTemplate>
                            <asp:ImageButton ID="imgSUM" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ClientID") %>'
                                CommandName="togglecmdSUM" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("SiteUnderMaintenance"))) %>'
                                ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("SiteUnderMaintenance"))) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="55px">
                        <ItemStyle Wrap="False" />
                        <HeaderStyle HorizontalAlign="left" Width="55px" Wrap="False" />
                        <ItemTemplate>
                            <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ClientID") %>'
                                CommandName="togglecmdStatus" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit" ItemStyle-Width="35px">
                        <ItemStyle Wrap="False" />
                        <HeaderStyle HorizontalAlign="left" Width="35px" Wrap="False" />
                        <ItemTemplate>
                            <asp:ImageButton ID="Editimg" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ClientID") %>'
                                CommandName="editcmd" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                <AlternatingRowStyle CssClass="Altrow" />
                <PagerStyle CssClass="PagerStyle" />
            </asp:GridView>

        </div>
        <div class="clear">
        </div>
    </div>

</asp:Content>
