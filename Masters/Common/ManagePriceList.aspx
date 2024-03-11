<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManagePriceList.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="DataAccess.Masters_HO_ManagePriceList" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Add / Edit Price List
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
                            <asp:Label ID="lblpricelst" runat="server" Text="">Price List Name:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtInsertName" runat="server" CssClass="formfields"
                                MaxLength="30"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="reqName" ControlToValidate="txtInsertName" CssClass="error" Display="Dynamic"
                                ErrorMessage="Please enter the price list name" ValidationGroup="insert" />

                            <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtInsertName" CssClass="error" Display="Dynamic"
                                ErrorMessage="Invalid PriceList Name" ValidationExpression="[^()<>/\@%]{1,30}" ValidationGroup="insert"
                                runat="server" />
                        </li>
                        <li class="text">
                            <div class="float-margin">Status:</div>
                            <div class="float-margin">
                                <asp:CheckBox ID="chkActive" runat="server" />
                            </div>
                        </li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSubmit" runat="server"
                                    OnClick="btn_sumbitprice_Click" Text="Submit" ValidationGroup="insert" CssClass="buttonbg" CausesValidation="true" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btncancelprice" runat="server" Text="Cancel" CssClass="buttonbg"
                                    OnClick="btn_cancel_click" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSubmit" />
                <asp:PostBackTrigger ControlID="btncancelprice" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="mainheading">
        Search Price List
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lbl_searchprice" runat="server" Text="Search Price List:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerName" runat="server" CssClass="formfields"></asp:TextBox>
                        </li>
                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnsearch" runat="server" Text="Search"
                                    OnClick="btn_searchclick" CssClass="buttonbg" CausesValidation="False" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="Getalldata" runat="server" Text="Show All Data"
                                    OnClick="btn_Getalldataclick" CssClass="buttonbg" CausesValidation="False" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnsearch" />
                <asp:PostBackTrigger ControlID="Getalldata" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="mainheading">
        List
    </div>
    <div class="export">
        <asp:Button ID="exportToExel" Text="" runat="server"
            OnClick="exportToExel_Click" CssClass="excel" />
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grdPriceList" runat="server" AllowPaging="True"
                        AlternatingRowStyle-CssClass="Altrow" AutoGenerateColumns="false"
                        BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="PriceListID"
                        FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left"
                        FooterStyle-VerticalAlign="Top" GridLines="none" PageSize='<%$ AppSettings:GridViewPageSize %>'
                        HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                        HeaderStyle-VerticalAlign="top" OnRowCommand="grdPrice_RowCommand"
                        RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                        RowStyle-VerticalAlign="top" Width="100%" EmptyDataText="No record found"
                        OnPageIndexChanging="grdPriceList_PageIndexChanging"
                        OnSelectedIndexChanged="grdPriceList_SelectedIndexChanged">
                        <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <Columns>
                            <asp:BoundField DataField="PriceListName" HeaderStyle-HorizontalAlign="Left"
                                HeaderText="Price List Name " HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false"
                                        CommandArgument='<%#Eval("PriceListID") %>' CommandName="Active"
                                        ImageAlign="Top"
                                        ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="img1" runat="server" CausesValidation="false"
                                        CommandArgument='<%#Eval("PriceListID") %>' CommandName="cmdEdit"
                                        ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </ContentTemplate>
                 <Triggers>
                                                <asp:PostBackTrigger ControlID="grdPriceList"  />
                                               
                                            </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
