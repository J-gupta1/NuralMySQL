﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageUploadSecondarySalesReturnSB-BCP.aspx.cs" Inherits="Transactions_SalesChannelSBReturn_Upload_ManageUploadSecondarySalesReturnSB_BCP" %>

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
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C3-S">
            <ul id="pnlMode" runat="server" visible="false">
                <li class="text">Select Mode:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:RadioButtonList ID="rdModelList" runat="server" CssClass="radio-rs" TextAlign="Right" RepeatDirection="Horizontal"
                        CellPadding="2" CellSpacing="0" BorderWidth="0" AutoPostBack="True" OnSelectedIndexChanged="rdModelList_SelectedIndexChanged">
                        <asp:ListItem Text="Excel" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="On-Screen Entry" Value="1" Enabled="false"></asp:ListItem>
                    </asp:RadioButtonList>
                </li>
            </ul>
            <div class="clear"></div>
            <ul>
                <li class="text">Select Return Date:<span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucSalesReturnDate" runat="server" ErrorMessage="Invalid date."
                        IsRequired="true" defaultDateRange="true" RangeErrorMessage="Date should be less then equal to current date."
                        ValidationGroup="Add" />
                </li>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text="" Visible="true"></asp:Label>
                </li>
                <li class="field3">
                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                        OnClick="btnUpload_Click" />
                </li>
            </ul>
            <div class="clear"></div>
            <%--#CC05 Add Start --%>
            <ul runat="server" id="trSalesReturnFrom" visible="true">
                <li class="text">Select Sales Channel: <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlSalesReturnTo" runat="server" CssClass="formselect" AutoPostBack="true" OnSelectedIndexChanged="ddlSalesReturnTo_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="rqddlSalesReturnTo" ControlToValidate="ddlSalesReturnTo"
                            CssClass="error" InitialValue="0" runat="server" ErrorMessage="Please Select Sales Channel."></asp:RequiredFieldValidator>
                    </div>
                </li>
            </ul>
            <%--#CC05 Add End --%>
            <ul runat="server" id="dvStockBinType" visible="true">
                <li class="text">Select Stock Type: <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlStockBinType" runat="server" CssClass="formselect">
                            <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="reqStockBinType" ControlToValidate="ddlStockBinType"
                            CssClass="error" ValidationGroup="Add" InitialValue="0" runat="server" ErrorMessage="Please Select Stock Bin Type."></asp:RequiredFieldValidator>
                    </div>
                </li>
            </ul>
            <ul runat="server" id="Tr1" visible="true">
                <li class="text">Select Template Type: <span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlTemplate" runat="server" CssClass="formselect" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged">
                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Full Template" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Serial Only" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlTemplate"
                            CssClass="error" ValidationGroup="Add" InitialValue="0" runat="server" ErrorMessage="Please Select Template Type"></asp:RequiredFieldValidator>
                    </div>
                </li>
            </ul>
        </div>
        <div class="formlink">
            <ul>
                <li class="link">
                    <%--#CC04 START ADDED--%>
                    <asp:LinkButton ID="DwnldBindCode" runat="server" Text="Download Bin Code"
                        CssClass="elink2" OnClick="DwnldBindCode_Click"></asp:LinkButton>
                </li>
                <li class="link">
                    <%--#CC04 END ADDED--%>
                    <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>
                </li>
                <li class="link">
                    <a class="elink2" href="../../../Excel/Templates/SecondarySalesReturn-SB.xlsx"
                        visible="false" id="lnkFullTemplate" runat="server">Download Template</a>

                    <a class="elink2" href="../../../Excel/Templates/SalesReturnExistInSystem.xlsx"
                        visible="false" id="lnkTemplateSystemExist" runat="server">Download Template System
                                                Exist</a>
                </li>
                <li class="link">
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                </li>
            </ul>
        </div>
    </div>
    <asp:Panel ID="pnlGrid" runat="server">
        <%--Visible="false"--%>
        <%--<asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
        <div runat="server" id="dvUploadPreview">
            <%--visible="false"--%>
            <div>
                <%--style="display: none"--%>
                <div class="mainheading">
                    Upload Preview
                </div>
            </div>
            <div class="float-right" style="width: 343px;">
                <div class="float-margin">
                    <asp:Label ID="lblTotal" runat="server" Visible="false" Text="" Font-Bold="true"></asp:Label>
                </div>
                <div class="float-margin">
                    <asp:Button ID="btnSave1" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="Add"
                        CausesValidation="true" OnClick="Btnsave_Click" />
                </div>
                <div class="float-left">
                    <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="false"
                        OnClick="BtnCancel_Click" />
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <div class="contentbox">
            <%--style="display: none;"--%>
            <div class="grid1" runat="server" id="dvGrid">
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
        <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
        <div class="margin-bottom" style="display: none;">
            <div class="float-margin">
                <asp:Button ID="Btnsave" runat="server" CssClass="buttonbg" Text="Save" ValidationGroup="Add"
                    OnClick="Btnsave_Click" Visible="false" />
            </div>
            <div class="float-margin">
                <asp:Button ID="BtnCancel" runat="server" CssClass="buttonbg" Text="Cancel" OnClick="BtnCancel_Click"
                    Visible="false" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
