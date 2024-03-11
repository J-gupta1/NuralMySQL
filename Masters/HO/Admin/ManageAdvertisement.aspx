<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageAdvertisement.aspx.cs" Inherits="Masters_HO_Admin_ManageAdvertisement" %>

<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <style type="text/css">
        .style1 {
            width: 100%;
        }

        .style2 {
            width: 47px;
        }

        .style3 {
            width: 100px;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:HiddenField ID="hdnAddID" runat="server" />
    <asp:HiddenField ID="hdnCondition" runat="server" />
    <div id="Header">
        <uc1:ucMessage ID="ucMsg" runat="server" />
    </div>
    <div class="clear">
    </div>
    <div class="mainheading">
        Add Advertisement
    </div>
    <div id="AddPanel">
        <div class="contentbox">
            <div class="mandatory">
                (*) Marked fields are mandatory
            </div>
            <div class="H25-C3-S">
                <ul>
                    <li class="text">Caption: <span class="error">* </span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:TextBox CssClass="formfields" ID="txtCaption" runat="server" ValidationGroup="aa"></asp:TextBox>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter Caption"
                                ControlToValidate="txtCaption" SetFocusOnError="True" ValidationGroup="aa"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="text">
                        <asp:Label ID="lblMode" runat="server" Text="Image Internet URL"></asp:Label>
                    </li>
                    <li class="field">
                        <div>
                            <asp:TextBox CssClass="formfields" ID="txtInternetURL" runat="server"
                                CausesValidation="True" ValidationGroup="aa"></asp:TextBox>
                            <asp:FileUpload ID="fuImage" runat="server" CssClass="fileuploads" />
                        </div>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ErrorMessage="Please enter the internet url."
                                ControlToValidate="txtInternetURL" SetFocusOnError="True"
                                ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?"
                                ValidationGroup="aa"></asp:RegularExpressionValidator>
                        </div>
                    </li>
                    <li class="text"></li>
                    <li class="field">
                        <asp:CheckBox ID="chkMode" runat="server" AutoPostBack="True" OnCheckedChanged="chkMode_CheckedChanged"
                            Text="Upload Mode" TextAlign="Right" />
                    </li>
                    <li class="text">Video Link
                    </li>
                    <li class="field">
                        <div>
                            <asp:TextBox CssClass="formfields" ID="txtVideoLink" runat="server" CausesValidation="True"
                                ValidationGroup="aa"></asp:TextBox>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="Pelase Enter Video Link"
                                ControlToValidate="txtVideoLink" SetFocusOnError="True" ValidationGroup="aa"></asp:RequiredFieldValidator>

                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic"
                                ErrorMessage="Please enter the internet url." ControlToValidate="txtVideoLink"
                                SetFocusOnError="True"
                                ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?"
                                ValidationGroup="aa"></asp:RegularExpressionValidator>
                        </div>
                    </li>
                    <li class="text">Status                    
                        <asp:CheckBox ID="chkStatus" runat="server" Checked="True" />
                    </li>
                    <li class="field">
                        <div class="float-margin">
                            <asp:Button ID="btnSubmit" CssClass="buttonbg" runat="server" Text="Submit" ValidationGroup="aa"
                                OnClick="btnSubmit_Click" />
                        </div>
                        <div class="float-left">
                            <asp:Button ID="btnCancel" CssClass="buttonbg" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
    <div class="mainheading">
        Search Advertisement
    </div>
    <div class="contentbox">
        <div class="H25-C3-S">
            <ul>
                <li class="text">Caption
                </li>
                <li class="field">
                    <asp:TextBox ID="txtsrcCaption" CssClass="formfields" runat="server"></asp:TextBox>
                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button ID="btnSearch" CssClass="buttonbg" runat="server" Text="Search" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnsrcCancel" CssClass="buttonbg" runat="server" Text="Cancel" OnClick="btnsrcCancel_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div class="clear">
    </div>
    <div class="mainheading">
        View Advertisement
    </div>
    <div id="ViewPanel">
        <div class="contentbox">
            <div class="grid1">
                <asp:GridView ID="gvAdvertisement" runat="server" AutoGenerateColumns="False" Width="100%"
                    CellSpacing="1" CellPadding="4" EditRowStyle-CssClass="editrow" GridLines="None"
                    HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow"
                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Middle" AllowPaging="True"
                    OnPageIndexChanging="gvAdvertisement_PageIndexChanging" OnRowCommand="gvAdvertisement_RowCommand">
                    <RowStyle CssClass="gridrow" />
                    <Columns>
                        <asp:TemplateField HeaderText="Advertisement Caption" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                    <asp:Label ID="lblEmailKeyword" runat="server" Text='<%#Eval("AdCaption") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Advertisement URl" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <div style="width: 220px; overflow: hidden; word-wrap: break-word;">
                                    <asp:Label ID="Label2" runat="server" Text='<%#Eval("AdURL") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ThumbNail" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <div style="width: 150px; overflow: hidden; word-wrap: break-word;">
                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%#Convert.ToString(Eval("ThumbNailType"))=="0"?PageBase.NoImageFound():Eval("ThumbNailPath") %>'
                                        Height="40px" Width="40px" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnStatus" runat="server" CommandName="Status" CommandArgument='<%#Eval("AdvertisementID") %>'
                                    ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:ImageButton ID="Editimg" runat="server" CausesValidation="false" CommandArgument='<%#Eval("AdvertisementID") %>'
                                    CommandName="editcmd" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="gridheader" />
                    <EditRowStyle CssClass="editrow" />
                    <AlternatingRowStyle CssClass="Altrow"></AlternatingRowStyle>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
