<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageRetailerTypeMapping.aspx.cs" Inherits="Masters_Retailer_ManageRetailerTypeMapping" %>

<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:HiddenField ID="hdnManageClient" runat="server" />
    <div>
        <uc1:ucMessage ID="ucMsg" runat="server" />
    </div>
    <div class="mainheading">
        Add Panel
    </div>    
    <div class="contentbox">
        <div class="mandatory">
        (*) Marked fields are mandatory
    </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Sales Channel Type: <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlSaelsChannelType" runat="server" CausesValidation="True" CssClass="formselect"
                            ValidationGroup="aa">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="error" runat="server" ErrorMessage="Please Select Sales Channel Type"
                            ControlToValidate="ddlSaelsChannelType" SetFocusOnError="True" ValidationGroup="aa"
                            InitialValue="0"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">Retailer Type: <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlRetailerType" runat="server" CssClass="formselect" CausesValidation="True" ValidationGroup="aa">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="error" runat="server" ErrorMessage="Please Select Retailer Type"
                            ControlToValidate="ddlRetailerType" InitialValue="0" SetFocusOnError="True" ValidationGroup="aa"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">Status:
                </li>
                <li class="field">
                    <asp:CheckBox ID="chkStatus" runat="server" Checked="True" />
                </li>
                 <li class="text">
                </li>
                <li class="field">
                    <div class="float-margin">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonbg" ValidationGroup="aa"
                            OnClick="btnSubmit_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnClear" runat="server" Text="Cancel" CssClass="buttonbg"
                            CausesValidation="False" OnClick="btnClear_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <%--<uc1:ucMessage ID="ucMsg" runat="server" />--%>

    <div class="mainheading">
        View                   
    </div>
    <div class="export">
        <asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" />
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:GridView ID="gvRetailerTypeMapping" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                RowStyle-VerticalAlign="top" Width="100%"
                OnRowCommand="gvRetailerTypeMapping_RowCommand"
                OnPageIndexChanging="gvRetailerTypeMapping_PageIndexChanging">
                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Sales Channel Type
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="word-wrap: break-word; overflow: hidden; width: 150px;">
                                <%# DataBinder.Eval(Container.DataItem, "SalesChannelTypeName")%>
                            </div>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Retailer Type
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="word-wrap: break-word; overflow: hidden; width: 200px;">
                                <%# DataBinder.Eval(Container.DataItem, "RetailerTypeName")%>
                            </div>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                        <ItemStyle Wrap="False" />
                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                        <ItemTemplate>
                            <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("RetailerTypeMappingID") %>'
                                CommandName="togglecmdStatus" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                <AlternatingRowStyle CssClass="Altrow" />
                <PagerStyle CssClass="PagerStyle" />
            </asp:GridView>
        </div>
        <div class="clear">
        </div>
    </div>

</asp:Content>
