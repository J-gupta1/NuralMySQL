<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="BulletinDetail.aspx.cs" Inherits="BulletinBoard_BulletinDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <div class="export">
            <asp:LinkButton ID="LBViewBulletin" Text="View Bulletin List" runat="server" CausesValidation="False"
                CssClass="elink7" OnClick="LBViewBulletin_Click"></asp:LinkButton>
        </div>
        <div class="contentbox">
            <asp:Panel ID="DetailPanel" runat="server">
                <asp:Repeater ID="RptDisplay" runat="server">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>                       
                            <div class="H20-C2-S">
                                <ul>
                                    <li class="text">
                                        <b>Subject:</b>
                                    </li>
                                    <li class="field">
                                        <%#Eval("BulletinSubject")%>
                                    </li>
                                </ul>
                                <div class="clear"></div>
                                <ul>
                                    <li class="text">
                                        <b>Category Name:</b>
                                    </li>
                                    <li class="field">
                                        <%#Eval("CategoryName")%>
                                    </li>
                                    <li class="text">
                                        <b>SubCategory Name:</b>
                                    </li>
                                    <li class="field">
                                        <%#Eval("SubCategoryName")%>
                                    </li>
                                </ul>
                                <ul>
                                    <li class="text">
                                        <b>PublishDate:</b>
                                    </li>
                                    <li class="field">
                                        <%#Eval("PublishDate", "{0:dd-MMM-yyyy}")%>
                                    </li>
                                    <li class="text">
                                        <b>Expiry Date:</b>
                                    </li>
                                    <li class="field">
                                        <%#Eval("ExpiryDate", "{0:dd-MMM-yyyy}")%>
                                    </li>
                                </ul>
                                <ul>
                                    <li class="text">
                                        <strong>Description:</strong>
                                    </li>
                                    <li class="field" style="height:auto">
                                        <%#Eval("Description")%>
                                    </li>
                                </ul>
                            </div>                       
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
