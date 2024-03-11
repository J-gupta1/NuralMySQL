<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrimaryOrder.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Transactions_POC_PrimaryOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />
    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>
    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>
    <script type="text/javascript">
        function popup(str) {
            var WinSearchChannelCode = dhtmlmodal.open("Reward Detail", "iframe", "RewardPoint.aspx?iD= " + str, "Reward Detail", "width=800px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
            WinSearchChannelCode.onclose = function () {
                return true;
            }
            return false;
        }
    </script>

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
                        <li class="text">Select Warehouse: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:DropDownList ID="ddlWareHouse" ValidationGroup="Upload" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </li>
                        <li class="text">Order date: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDatePicker" ErrorMessage="Invalid date."
                                runat="server" />
                        </li>
                        <li class="field3">
                            <asp:Button ID="Go" runat="server" Text="GO" CssClass="buttonbg" OnClick="Go_Click" />
                        </li>
                    </ul>
                    <asp:Panel ID="pnl1" runat="server">
                        <ul>
                            <li class="text">Select SKU: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:DropDownList ID="ddlSKU" runat="server" AutoPostBack="true" CssClass="formselect" OnSelectedIndexChanged="ddlSKU_SelectedIndexChanged">
                                </asp:DropDownList>
                            </li>
                            <li class="text">Quantity: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:TextBox ID="txtQuantity" runat="server" Text="0" MaxLength="9" CssClass="formfields"
                                    OnTextChanged="txtQuantity_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </li>
                        </ul>
                        <ul>
                            <li class="text">Rate: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:Label ID="lblRate" runat="server" Text="" CssClass="frmtxt1" Visible="false"></asp:Label>
                            </li>
                            <li class="text">Amount:<span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:Label ID="lblAmount" runat="server" Text="" CssClass="frmtxt1" Visible="false"></asp:Label>
                            </li>
                            <li class="field3">
                                <asp:Button ID="btnSubmit" runat="server" Text="Add In List" CssClass="buttonbg"
                                    OnClick="btnSubmit_Click" />
                            </li>
                        </ul>
                    </asp:Panel>
                    <asp:Panel ID="Panel1" runat="server">
                        <ul>
                            <li class="text">Offer: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:LinkButton ID="lnkOffer" runat="server" Text="Offer" CssClass="elink2"
                                    OnClick="lnkOffer_Click"></asp:LinkButton>
                            </li>
                            <li class="text">Elligibility: <span class="error">*</span>
                            </li>
                            <li class="field">
                                <asp:Label ID="lblElligibily" runat="server" CssClass="frmtxt1" Text="" Visible="false"></asp:Label>
                            </li>
                        </ul>
                    </asp:Panel>
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
                        <asp:GridView ID="GridTarget" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow" AutoGenerateColumns="false"
                            bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                            HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                            RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="Center" RowStyle-VerticalAlign="top"
                            SelectedStyle-CssClass="gridselected" Width="100%">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                            <PagerStyle CssClass="gridfooter" />
                            <Columns>
                                <asp:BoundField HeaderText="OfferName" DataField="OfferName" />
                                <asp:BoundField HeaderText="SKUCode" DataField="SKUCode" />
                                <asp:BoundField HeaderText="Amount" DataField="Amount" />
                                <asp:BoundField HeaderText="Quantity" DataField="Quantity" />
                                <asp:BoundField HeaderText="Amount" DataField="Amount" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="clear"></div>
                <div class="float-margin">
                    <asp:Button ID="btnSaveTarget" CssClass="buttonbg" runat="server" Text="Submit" OnClick="btnSaveTarget_Click" Visible="false" ValidationGroup="Upload" />
                </div>
                <div class="float-margin">
                    <asp:Button ID="btnReset" CssClass="buttonbg" runat="server" OnClick="btnReset_Click" Text="Reset" />
                </div>
                <div class="float-margin">
                    <asp:Button ID="btnProcess" CssClass="buttonbg" runat="server" Text="Process" OnClick="btnProcess_Click" />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
