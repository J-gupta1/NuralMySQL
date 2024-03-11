<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ViewBulletin.aspx.cs" Inherits="BulletinBoard_ViewBulletin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function BulletinDetail(BulletinId) {
            window.location = "BulletinDetail.aspx?BulletinId=" + BulletinId;
            return false;
        }</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
            <div class="mainheading">
                Search Bulletin
            </div>
            <div class="export">
                <asp:LinkButton ID="LBAddBulletin" runat="server" CausesValidation="False" CssClass="elink7"
                    OnClick="LBAddBulletin_Click">Add Bulletin</asp:LinkButton>
            </div>
            <div class="contentbox">
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Category :
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" CssClass="formselect"
                                    OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="reqddlCategory" runat="server" CssClass="error" InitialValue="0"
                                    ControlToValidate="ddlCategory" ErrorMessage="Please select category" Display="Dynamic"
                                    ValidationGroup="Serach"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Sub Category:
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="formselect">
                                    <asp:ListItem Value="0" Text="Select" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="reqSubCategory" runat="server" CssClass="error" InitialValue="0"
                                    ControlToValidate="ddlSubCategory" ErrorMessage="Please select subcategory" Display="Dynamic"
                                    ValidationGroup="Serach"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" runat="server" ValidationGroup="Serach" CausesValidation="true"
                                    CssClass="buttonbg" Text="Search" OnClick="btnSearch_Click" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnShowAll" runat="server" CssClass="buttonbg" Text="Show All" ToolTip="Show All"
                                    OnClick="btnShowAll_Click" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="mainheading">
                Bulletin List
            </div>

            <%--  <asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" />--%>


            <%--      <asp:UpdatePanel ID="UpdGrid" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>--%>
            <div class="contentbox">
                <div class="grid1">
                    <asp:GridView ID="GvBulletin" runat="server" AlternatingRowStyle-CssClass="Altrow"
                        AutoGenerateColumns="false" bgcolor="" AllowPaging="true" PageSize='<%$ AppSettings:GridViewPageSize %>'
                        BorderWidth="0px" CellPadding="4" EmptyDataText="No Record Found" CellSpacing="1"
                        DataKeyNames="BulletinID" FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left"
                        FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-CssClass="gridheader"
                        HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow"
                        RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected"
                        Width="100%" OnPageIndexChanging="GvBulletin_PageIndexChanging" OnRowCommand="GvBulletin_RowCommand"
                        OnRowDataBound="GvBulletin_RowDataBound">
                        <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                        <Columns>
                            <asp:BoundField DataField="CategoryName" HeaderStyle-HorizontalAlign="Left" HeaderText="Category"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SubCategoryName" HeaderStyle-HorizontalAlign="Left" HeaderText="Sub Category"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BulletinSubject" HeaderStyle-HorizontalAlign="Left" HeaderText="Subject"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PublishDate" HeaderStyle-HorizontalAlign="Left" HeaderText="Publish Date"
                                HtmlEncode="true" DataFormatString="{0:dd-MMM-yyyy}">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ExpiryDate" HeaderStyle-HorizontalAlign="Left" HeaderText="Expiry Date"
                                HtmlEncode="true" DataFormatString="{0:dd-MMM-yyyy}">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="View Details" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HLDetails" runat="server" CssClass="lnkbutton" Text="View Details"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgActive" runat="server" CausesValidation="false" CommandArgument='<%#Eval("BulletinID") %>'
                                        CommandName="Active" ImageAlign="Top" OnClick="btnActiveDeactive_Click" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("BulletinID") %>'
                                        CommandName="cmdEdit" ToolTip="Edit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                        <AlternatingRowStyle CssClass="Altrow" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </div>
            </div>
            <%-- </ContentTemplate>
                                                      
                                                    </asp:UpdatePanel>--%>
        </ContentTemplate>
        <Triggers>
           <%-- <asp:PostBackTrigger ControlID="ExportToExcel" />--%>
             <asp:PostBackTrigger ControlID="GvBulletin" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
