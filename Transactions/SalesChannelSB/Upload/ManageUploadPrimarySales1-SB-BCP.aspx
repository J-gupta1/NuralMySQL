<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="ManageUploadPrimarySales1-SB-BCP.aspx.cs" Inherits="Transactions_SalesChannelSB_Upload_ManageUploadPrimarySales1_SB_BCP" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Upload
    </div>
    <div class="contentbox">
        <%--<asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
                                    <ContentTemplate>--%>
       <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li>
                    <%-- Select Mode:<font class="error">*</font>--%>
                </li>
                <li>
                    <asp:RadioButtonList ID="rdModelList" runat="server" TextAlign="Right" RepeatDirection="Horizontal"
                        CellPadding="0" CellSpacing="0" BorderWidth="0" AutoPostBack="True" Visible="false"
                        OnSelectedIndexChanged="rdModelList_SelectedIndexChanged">
                        <asp:ListItem Text="Excel" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="On-Screen Entry" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </li>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                </li>
                <li class="field3">
                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                        OnClick="btnUpload_Click" />
                </li>
                <li class="link">

                    <%--#CC02 START ADDED--%>
                    <asp:LinkButton ID="DwnldBindCode" runat="server" Text="Download Bin Code"
                        CssClass="elink2" OnClick="DwnldBindCode_Click"></asp:LinkButton>                                         
                    <%--#CC02 END ADDED--%>

                </li>
                <li class="link">
                    <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>

                </li>
                <li class="link">
                    <a class="elink2" href="../../../Excel/Templates/PrimarySales1-SB.xlsx">Download Template</a>
                </li>
                <li class="link">
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                    <asp:HyperLink ID="hlnkDuplicateNotInUse" runat="server" CssClass="elink3"></asp:HyperLink>
                    <asp:HyperLink ID="hlnkBlankNotInUse" runat="server" CssClass="elink3"></asp:HyperLink>
                </li>
            </ul>
        </div>
    </div>
    <asp:Panel ID="pnlGrid" Visible="false" runat="server">
        <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>



                <div runat="server" id="dvUploadPreview" visible="false">
                    <div class="mainheading">
                        Upload Preview
                    </div>
                    <div class="float-right" style="width: 240px;">
                        <div class="float-margin">
                            <asp:Label ID="lblTotal" runat="server" Visible="false" Text="" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnSave1" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="Save"
                                CausesValidation="true" OnClick="Btnsave_Click" />
                        </div>
                        <div class="float-left">
                            <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="false"
                                OnClick="BtnCancel_Click" />
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="GridGRN" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            CellSpacing="1" DataKeyNames="SKUCode" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                            GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                            AlternatingRowStyle-CssClass="Altrow" Width="100%" AllowPaging="false" OnPageIndexChanging="GridGRN_PageIndexChanging">
                            <RowStyle CssClass="gridrow" />
                            <Columns>

                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="skuCode"
                                    HeaderText="Sku Code">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Quantity"
                                    HeaderText="Quantity">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>


                            </Columns>
                            <HeaderStyle CssClass="gridheader" />
                            <EditRowStyle CssClass="editrow" />
                            <AlternatingRowStyle CssClass="Altrow" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                        <%--<div style="float: left; padding-top: 10px; width: 300px;">
                        </div>--%>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="padding-top padding-bottom">
            <asp:Button ID="Btnsave" runat="server" CssClass="buttonbg" Text="Save" ValidationGroup="Add"
                OnClick="Btnsave_Click" Visible="false" />
            <asp:Button ID="BtnCancel" runat="server" CssClass="buttonbg" Text="Cancel" OnClick="BtnCancel_Click" Visible="false" />
        </div>
    </asp:Panel>
</asp:Content>

