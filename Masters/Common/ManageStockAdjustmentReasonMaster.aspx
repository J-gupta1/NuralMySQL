<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    EnableEventValidation="true" AutoEventWireup="true" CodeFile="ManageStockAdjustmentReasonMaster.aspx.cs"
    Inherits="Masters_Common_ManageStockAdjustmentReasonMaster" %>

<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%--<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>--%>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:HiddenField ID="hdnAdjReasonID" runat="server" />
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
                <li class="text">Adjustment Reason: <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:TextBox ID="txtAdjReason" CssClass="formfields" runat="server" CausesValidation="True"
                            MaxLength="50" ValidationGroup="aa"></asp:TextBox>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Enter The Adjustment Reason"
                            ControlToValidate="txtAdjReason" ValidationGroup="aa"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text"><span class="float-margin">Status :</span>
                    <span class="float-margin">
                        <asp:CheckBox ID="chkActive" runat="server" Checked="True" /></span>
                </li>
                <li class="field">
                    <div class="float-margin">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonbg" OnClick="btnSubmit_Click"
                            ValidationGroup="aa" />
                    </div>
                    <div class="float-left">
                        <asp:Button ID="btnClear" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="False"
                            OnClick="btnClear_Click" />
                    </div>
                </li>
            </ul>
        </div>
    </div>

    <%--<uc1:ucMessage ID="ucMsg" runat="server" />--%>

    <div class="mainheading">
        View panel
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:GridView ID="gvViewAdjReason" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                HeaderStyle-VerticalAlign="top" OnRowCommand="gvViewAdjReason_RowCommand" RowStyle-CssClass="gridrow"
                RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" Width="100%" OnPageIndexChanging="gvViewAdjReason_PageIndexChanging">
                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="ReasonDesc" HeaderStyle-HorizontalAlign="Left" HeaderText="Adjustment Reason"
                        HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                        <ItemStyle Wrap="False" />
                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                        <ItemTemplate>
                            <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("StockAdjustmentReasonID") %>'
                                CommandName="togglecmd" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Active"))) %>'
                                ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Active"))) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                        <ItemStyle Wrap="False" />
                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                        <ItemTemplate>
                            <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("StockAdjustmentReasonID") %>'
                                CommandName="editcmd" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
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
