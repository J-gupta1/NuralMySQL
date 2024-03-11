<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="ManageRetailerType.aspx.cs" Inherits="Masters_Retailer_ManageRetailerType" %>

<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:HiddenField ID="hdnRetailerTypeID" runat="server" />
    <div>
        <uc1:ucMessage ID="ucMsg" runat="server" />
    </div>
    <div class="mainheading">
        Add
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Retailer Type: <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:TextBox ID="txtRetailerType" runat="server" CausesValidation="True" MaxLength="30" CssClass="formfields"
                            ValidationGroup="aa"></asp:TextBox>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Enter Retailer Type" CssClass="error"
                            ControlToValidate="txtRetailerType" ValidationGroup="aa"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">Status:               
                    <asp:CheckBox ID="chkStatus" runat="server" Checked="True" />
                </li>
                <li class="field">
                    <div class="float-margin">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonbg"
                            ValidationGroup="aa" OnClick="btnSubmit_Click" />
                    </div>
                    <div class="float-left">
                        <asp:Button ID="btnClear" runat="server" Text="Cancel" CssClass="buttonbg"
                            OnClick="btnClear_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>

    <%--<uc1:ucMessage ID="ucMsg" runat="server" />--%>

    <div class="mainheading">
        View
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:GridView ID="gvRetailerType" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4"
                CellSpacing="1" FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left"
                FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-CssClass="gridheader"
                HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                RowStyle-VerticalAlign="top" Width="100%"
                OnPageIndexChanging="gvRetailerType_PageIndexChanging"
                OnRowCommand="gvRetailerType_RowCommand">
                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="RetailerTypeName" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Type"
                        HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                        <ItemStyle Wrap="False" />
                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                        <ItemTemplate>
                            <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ReatilerTypeID") %>'
                                CommandName="togglecmd" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                        <ItemStyle Wrap="False" />
                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                        <ItemTemplate>
                            <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ReatilerTypeID") %>'
                                CommandName="editcmd" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                <AlternatingRowStyle CssClass="Altrow" />
                <PagerStyle CssClass="PagerStyle" />
            </asp:GridView>
        </div>
    </div>

</asp:Content>

