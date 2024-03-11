<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="PriceTypeMaster.aspx.cs" Inherits="Transactions_Billing_PriceTypeMaster" %>
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
        Add / Edit PriceType Master
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
                            <asp:Label ID="lblPriceTypeKeyword" runat="server" Text="">Price Type Keyword: <span class="error">*</span></asp:Label></li>
                        <li class="field">
                            <asp:TextBox ID="txtPricetypekeyword" runat="server" CssClass="formfields" MaxLength="70"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="valname" ControlToValidate="txtPricetypekeyword" Display="Dynamic"
                                CssClass="error" ErrorMessage="Please insert Price Type Keyword" ValidationGroup="insert" />
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtPricetypekeyword"
                                InvalidChars="<>!@#$%^&*(){}" FilterType="Custom" FilterMode="InvalidChars">
                            </cc1:FilteredTextBoxExtender>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblPriceTypeDescription" runat="server" Text="">Price Type Description: <span class="error">*</span></asp:Label></li>
                        <li class="field">
                            <asp:TextBox ID="txtPricetypeDescription" runat="server" CssClass="formfields" MaxLength="70"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtPricetypeDescription" Display="Dynamic"
                                CssClass="error" ErrorMessage="Please insert Price Type Description" ValidationGroup="insert" />
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtPricetypeDescription"
                                InvalidChars="<>!@#$%^&*(){}" FilterType="Custom" FilterMode="InvalidChars">
                            </cc1:FilteredTextBoxExtender>
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
        Search Price Type
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="H20-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblSearchPriceTypeKeyword" runat="server" Text="Price Type Keyword:"></asp:Label>
                        </li>
                        <li class="field">
                            <asp:TextBox ID="txtSerPriceTypedescription" runat="server" CssClass="formfields"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtSerPriceTypedescription"
                                InvalidChars="<>!@#$%^&*(){}" FilterType="Custom" FilterMode="InvalidChars">
                            </cc1:FilteredTextBoxExtender>
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
                    <asp:GridView ID="grdPriceType" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                        AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                        DataKeyNames="PriceTypeID" FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left"
                        FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-CssClass="gridheader"
                        HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                        OnRowCommand="grdPriceType_RowCommand" RowStyle-CssClass="gridrow" EmptyDataText="No record found"
                        RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" Width="100%"
                        OnPageIndexChanging="grdPriceType_PageIndexChanging">
                        <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <Columns>
                            <asp:BoundField DataField="PriceTypeKeyword" HeaderStyle-HorizontalAlign="Left" HeaderText="Price Type Keyword"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PriceTypeDescription" HeaderStyle-HorizontalAlign="Left" HeaderText="Price Type Description"
                                HtmlEncode="true">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("PriceTypeID") %>'
                                        CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Active"))) %>'
                                        ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Active"))) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("PriceTypeID") %>'
                                        CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdPriceType" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

