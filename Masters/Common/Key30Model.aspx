<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Key30Model.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Masters_Common_Key30Model" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Add / Edit Key30 Model
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
                            <asp:Label ID="LblModel" runat="server" Text="Model Name: <span class='error'>*</span>"></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlModel" runat="server"
                                    CssClass="formselect" >
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlModel"
                                    CssClass="error" ErrorMessage="Please select  Model " InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">Status: 
                        </li>
                        <li class="field">
                            <asp:CheckBox ID="chkstatus" runat="server" Checked="True" />
                        </li>
                        
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btninsert_click"
                                    ValidationGroup="insert" CssClass="buttonbg" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                    CssClass="buttonbg" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSubmit" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="mainheading">
        Search Key30 Model
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblSerModel" Text="Model Name: " runat="server" />
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlSerModel" runat="server" 
                                CssClass="formselect" >
                            </asp:DropDownList>
                        </li>
                         <li class="text">
                            <asp:Label ID="LblSerStatus" runat="server" Text="Status:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlSearchStatus" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                    </ul>   
                    <ul>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSerchModel" Text="Search" runat="server" OnClick="btnSerchModel_Click"
                                    CssClass="buttonbg" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="getalldata" Text="Show All Data" runat="server" OnClick="btngetalldata_Click"
                                    CssClass="buttonbg" />
                            </div>
                        </li>
                    </ul>

                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSerchModel" />
                <asp:AsyncPostBackTrigger ControlID="getalldata" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
   <div class="mainheading">
        List
    </div>
    <div class="export">
        <asp:Button ID="exportToExel" Text=" " runat="server" OnClick="exportToExel_Click"
            CssClass="excel" />
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grd30KeyModel" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                        BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="KeyModelId" FooterStyle-HorizontalAlign="Left"
                        FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                        HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" EmptyDataText="No record found"
                        RowStyle-VerticalAlign="top" Width="100%" AlternatingRowStyle-CssClass="Altrow"
                        RowStyle-CssClass="gridrow" PageSize='<%$ AppSettings:GridViewPageSize %>' FooterStyle-CssClass="gridfooter"
                        HeaderStyle-CssClass="gridheader" OnRowCommand="grd30KeyModel_RowCommand" OnPageIndexChanging="grd30KeyModel_PageIndexChanging">
                        <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                        <Columns>
                            
                            <asp:BoundField DataField="ModelName" HeaderStyle-HorizontalAlign="Left" HeaderText="Model Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ModelCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Model Code"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="BrandName" HeaderStyle-HorizontalAlign="Left" HeaderText="Brand Name"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderStyle-HorizontalAlign="Left" HeaderText="Status"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("KeyModelId") %>'
                                        CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grd30KeyModel" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
