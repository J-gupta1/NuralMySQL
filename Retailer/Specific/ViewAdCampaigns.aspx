<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ViewAdCampaigns.aspx.cs" Inherits="Retailer_Specific_ViewAdCampaigns" %>

<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div class="mainheading">
         Advertisements
    </div>
    <div class="clear">
    </div>
    <div style="margin-bottom: 0px;">
        <asp:DataList ID="dtlistAd" runat="server" RepeatColumns="3" Width="100%">
            <ItemTemplate>
                <div class="contentbox">
                    <div class="float-margin" style="border: 1px solid #dddddd;">
                        <a href='<%#Eval("AdURL") %>' target="_blank">
                            <asp:Image ID="Image1" runat="server" ImageUrl='<%#Convert.ToString(Eval("ThumbNailType"))=="0"?PageBase.NoImageFound():Eval("ThumbNailPath") %>'
                                Height="75px" Width="120px" /></a>
                    </div>
                    <div class="float-margin" style="word-wrap: break-word; overflow: hidden; width: 150px;">
                        <a class="elink2" href='<%#Eval("AdURL") %>' target="_blank">
                            <%#Eval("AdCaption") %></a>
                        <%--<asp:Label CssClass="elink2" ID="Label1" runat="server" Text='<%#Eval("AdCaption") %>'></asp:Label>--%>
                    </div>
                </div>
            </ItemTemplate>
        </asp:DataList>
    </div>
</asp:Content>
