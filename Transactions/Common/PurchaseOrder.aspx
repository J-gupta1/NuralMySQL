<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseOrder.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Transactions_PurchaseOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />
    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>
    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="uclblMessage" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Input Parameters
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">PO From: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlPOFrom" ValidationGroup="Submit" runat="server" CssClass="formselect" 
                                OnSelectedIndexChanged="ddlPOFrom_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqPoFrom" runat="server" ControlToValidate="ddlPOFrom" InitialValue="0" ErrorMessage="Required" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                        </li>
                        <li class="text">PO To: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlWareHouse" ValidationGroup="Submit" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqPOto" runat="server" ControlToValidate="ddlWareHouse" InitialValue="0" ErrorMessage="Required" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                        </li>
                        <li class="text">Order date: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDatePicker" ErrorMessage="Date required." IsRequired="true" ValidationGroup="Submit"
                                runat="server" />
                        </li>
                        <li class="text">Brand: <span class="error"></span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlBrand" ValidationGroup="Upload" runat="server" CssClass="formselect" 
                                OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </li>
                        <li class="text">Product Category: <span class="error"></span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlProdCat" ValidationGroup="Upload" runat="server" CssClass="formselect" 
                                OnSelectedIndexChanged="ddlProdCat_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </li>
                        <li class="text">Model: <span class="error"></span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlModel" ValidationGroup="Add" runat="server" CssClass="formselect" 
                                OnSelectedIndexChanged="ddlModel_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqModel" runat="server" ControlToValidate="ddlModel" InitialValue="0" ErrorMessage="*" ValidationGroup="Add"></asp:RequiredFieldValidator>
                        </li>
                        <li class="text">SKU: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlSKU" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqSKU" runat="server" ControlToValidate="ddlSKU" InitialValue="0" ErrorMessage="*" ValidationGroup="Add"></asp:RequiredFieldValidator>
                        </li>
                        <li class="text">Quantity: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtQuantity" runat="server" Text="0" MaxLength="9" CssClass="formfields"
                                    ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqQty" runat="server" ControlToValidate="txtQuantity" InitialValue="" ErrorMessage="*" ValidationGroup="Add"></asp:RequiredFieldValidator>
                            </li>
                        <li class="field3">
                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="buttonbg" OnClick="btnAdd_Click" ValidationGroup="Add" CausesValidation="true" />
                        </li>
                    </ul>
                    
                    
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="updgrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                <div class="mainheading">
                    List
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="GridItem" runat="server" AllowPaging="false" AlternatingRowStyle-CssClass="Altrow" AutoGenerateColumns="false"
                            bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                            HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                            RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="Center" RowStyle-VerticalAlign="top"
                            SelectedStyle-CssClass="gridselected" Width="100%">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                            <PagerStyle CssClass="gridfooter" />
                            <Columns>
                                <asp:BoundField HeaderText="Model" DataField="Model" />
                                <asp:BoundField HeaderText="SKU Name" DataField="SKUName" />
                                <asp:BoundField HeaderText="Quantity" DataField="Quantity" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnRemove" Text="Remove" runat="server" CommandArgument='<%# Eval("SKUCode") %>' OnClick="btnRemove_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="clear"></div>
                <div class="float-margin">
                    <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Submit" OnClick="btnSave_Click" ValidationGroup="Submit" />
                </div>
                
                
            </asp:Panel>
        </ContentTemplate>
        <Triggers><asp:PostBackTrigger ControlID="btnSave" />
            <asp:PostBackTrigger ControlID="btnAdd" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
