<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SonyMobile-Scheme.aspx.cs" Inherits="Masters_HO_Admin_SonyMobile_Scheme" %>

<%@ Register Src="../../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker"
    TagPrefix="uc1" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:HiddenField ID="hdnCase" runat="server" />
    <asp:HiddenField ID="hdnFileName" runat="server" />
    <asp:HiddenField ID="hdnSchemeID" runat="server" />
    <div id="ucmsg">
        <uc2:ucMessage ID="ucMsg" runat="server" />
    </div>
    <div id="addScheme">
        <div class="mainheading">
            Add Scheme
        </div>
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Scheme Name: <span class="error">* </span>
                </li>
                <li class="field">
                    <div>
                        <asp:TextBox ID="txtSchemeName" runat="server" CausesValidation="True" CssClass="formfields"
                            ValidationGroup="aa"></asp:TextBox>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter scheme name."
                            ControlToValidate="txtSchemeName" ValidationGroup="aa"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">Description:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtDescripption" runat="server" TextMode="MultiLine" CssClass="form_textarea"></asp:TextBox>
                </li>
                <li class="text">Scheme Document: <span class="error">* </span>
                </li>
                <li class="field">
                    <div>
                        <asp:FileUpload CssClass="fileuploads" ID="fuDocument" runat="server" />
                    </div>
                </li>
            </ul>
            <ul>
                <li class="text">Start Date: <span class="error">* </span>
                </li>
                <li class="field">
                    <uc1:ucDatePicker ID="ucdpStartDate" runat="server" ErrorMessage="Please select start date."
                        RangeErrorMessage="Invalid date." ValidationGroup="aa" />
                </li>
                <li class="text">End Date: <span class="error">* </span>
                </li>
                <li class="field">
                    <uc1:ucDatePicker ID="ucdpEndDate" runat="server" ErrorMessage="Please select end date."
                        RangeErrorMessage="Invalid date." ValidationGroup="aa" />
                </li>
                <li class="text">
                    <div class="float-margin">Status </div>
                    <div class="float-margin">
                        <asp:CheckBox ID="chkStatus" runat="server" Checked="True" />
                    </div>
                </li>
                <li class="field">
                    <div class="float-margin">
                        <asp:Button ID="btnSubmit" CssClass="buttonbg" runat="server" Text="Submit" ValidationGroup="aa"
                            OnClick="btnSubmit_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancel" CssClass="buttonbg" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div id="SearchScheme">
        <div class="mainheading">
            Search Scheme
        </div>
    </div>
    <div class="contentbox">
        <div class="H25-C3-S">
            <ul>
                <li class="text">Scheme Code:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtsrcSchemeCode" runat="server" CssClass="formfields"></asp:TextBox>
                </li>
                <li class="text">Scheme Name:
                </li>
                <li class="field">
                    <asp:TextBox ID="txtsrcSchememName" runat="server" CssClass="formfields"></asp:TextBox>
                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button ID="btnSearch" CssClass="buttonbg" runat="server" Text="Search" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancelSearch" CssClass="buttonbg" runat="server" Text="Cancel"
                            OnClick="btnCancelSearch_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div id="viewScheme">
        <div class="mainheading">
            View Scheme
        </div>
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:GridView ID="gvViewOfflineScheme" runat="server" AutoGenerateColumns="False"
                Width="100%" CellSpacing="1" CellPadding="4" EditRowStyle-CssClass="editrow"
                GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                AlternatingRowStyle-CssClass="Altrow" HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Middle"
                AllowPaging="True" OnPageIndexChanging="gvViewOfflineScheme_PageIndexChanging"
                OnRowCommand="gvViewOfflineScheme_RowCommand">
                <RowStyle CssClass="gridrow" />
                <Columns>
                    <asp:TemplateField HeaderText="Scheme Name" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="lblEmailKeyword" runat="server" Text='<%#Eval("SchemeName") %>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Scheme Description" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <div style="width: 220px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("SchemeDescription") %>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Scheme Document File Name" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="Label3" runat="server" Text='<%#Eval("SchemeDocumentFileName") %>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Scheme Start Date" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <div style="width: 130px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("SchemeStartDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Scheme End Date" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <div style="width: 130px; overflow: hidden; word-wrap: break-word;">
                                <asp:Label ID="Label5" runat="server" Text='<%#Eval("SchemeEndDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgBtnStatus" runat="server" CommandName="Status" CommandArgument='<%#Eval("OfflineSchemeID") %>'
                                ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:ImageButton ID="Editimg" runat="server" CausesValidation="false" CommandArgument='<%#Eval("OfflineSchemeID") %>'
                                CommandName="editcmd" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="gridheader" />
                <EditRowStyle CssClass="editrow" />
            </asp:GridView>
        </div>
    </div>
    <div class="clear" style="height: 10px;">
    </div>
</asp:Content>
