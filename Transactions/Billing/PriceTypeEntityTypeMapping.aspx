<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="PriceTypeEntityTypeMapping.aspx.cs" Inherits="Transactions_Billing_PriceTypeEntityTypeMapping" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Add / Edit PriceType/EntityType Mapping
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblPriceType" runat="server" Text="">Price Type: <span class="error">*</span></asp:Label></li>
                        <li class="field">
                            <div>
                            <asp:DropDownList ID="ddlPriceType" CssClass="formselect" runat="server"></asp:DropDownList>
                            </div>
                             <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlPriceType"
                                    CssClass="error" ErrorMessage="Please select  Price Type" InitialValue="0" ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblEntityType" runat="server" Text="">Entity Type: <span class="error">*</span></asp:Label></li>
                        <li class="field">
                            <div>
                           <asp:DropDownList ID="ddlEntityType" CssClass="formselect" runat="server"></asp:DropDownList>
                                </div>
                             <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ddlEntityType"
                                    CssClass="error" ErrorMessage="Please select  Entity Type " InitialValue="0" ValidationGroup="insert" />
                            </div>
                            
                        </li>
                        <li class="text">
                            <div class="float-margin">Status: </div>
                            <div class="float-left">
                                <asp:CheckBox ID="chkstatus" runat="server" Checked="true" />
                            </div>
                        </li>
                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnsubmit" runat="server" OnClick="btninsert_click" Text="Submit"
                                    ValidationGroup="insert" CssClass="buttonbg" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btncancel" runat="server" CausesValidation="False" OnClick="btncancel_Click"
                                    Text="Cancel" CssClass="buttonbg" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnsubmit" />
                <asp:PostBackTrigger ControlID="btncancel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="mainheading">
        Search PriceType/EntityType Mapping
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblSearchPriceType" runat="server" Text="Price Type:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlSerPriceType" CssClass="formselect" runat="server"></asp:DropDownList>
                        </li>

                        <li class="text">
                            <asp:Label ID="lblSerEntityType" runat="server" Text="Entity Type:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlSerEntityType" CssClass="formselect" runat="server"></asp:DropDownList>
                        </li>
                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click"
                                    CssClass="buttonbg" CausesValidation="False" ValidationGroup="search" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnGetallData" runat="server" Text="Show All Data"
                                    OnClick="btnGetallData_Click" CssClass="buttonbg" CausesValidation="False" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSearch" />
                <asp:PostBackTrigger ControlID="btnGetallData" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="mainheading">
        List
    </div>
    <div class="export">
        <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" OnClick="btnExprtToExcel_Click" CausesValidation="False" />
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grdPriceTypeEntityMapping" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                        AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                        DataKeyNames="PriceTypeEntityTypeMappingID" FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left"
                        FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-CssClass="gridheader"
                        HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                        OnRowCommand="grdPriceTypeEntityMapping_RowCommand" RowStyle-CssClass="gridrow" EmptyDataText="No record found"
                        RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" Width="100%"
                        OnPageIndexChanging="grdPriceTypeEntityMapping_PageIndexChanging">
                        <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <Columns>
                            <asp:BoundField DataField="PriceTypeKeyword" HeaderStyle-HorizontalAlign="Left" HeaderText="Price Type"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EntityType" HeaderStyle-HorizontalAlign="Left" HeaderText="Entity Type"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("PriceTypeEntityTypeMappingID") %>'
                                        CommandName="Active" ImageAlign="Top" Visible='<%# Convert.ToBoolean(Eval("Active")) %>' ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Active"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Active"))) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="img1" Visible='<%# Convert.ToBoolean(Eval("Active")) %>' runat="server" CausesValidation="false" CommandArgument='<%#Eval("PriceTypeEntityTypeMappingID") %>'
                                        CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdPriceTypeEntityMapping" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
