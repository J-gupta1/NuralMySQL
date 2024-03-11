<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageInvoiceCancellationBySellerUpload.aspx.cs" Inherits="Transactions_SalesChannelSB_Upload_ManageInvoiceCancellationBySellerUpload" %>

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
            <ul>
                <li class="text">Select Mode:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:RadioButtonList ID="rdModelList" runat="server" CssClass="radio-rs" TextAlign="Right" RepeatDirection="Horizontal"
                        CellPadding="2" CellSpacing="0" BorderWidth="0" AutoPostBack="True" OnSelectedIndexChanged="rdModelList_SelectedIndexChanged">
                        <asp:ListItem Text="Excel" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Interface" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>

                </li>
            </ul>
            <div class="clear"></div>
            <ul>
                <li class="text">Sales Type:<span class="error">*</span>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="ddlSalesType" runat="server" CssClass="formselect">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlSalesType"
                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales type."
                            SetFocusOnError="true" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                </li>
                <li class="field3">
                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                        OnClick="btnUpload_Click" />
                </li>
            </ul>
            <div class="clear"></div>
        </div>
        <div class="formlink">
            <ul>
                <li>
                    <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>
                </li>
                <li>
                    <a class="elink2" href="../../../Excel/Templates/InvoiceCancellationBySeller.xlsx">Download Template</a>
                </li>
                <li>
                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink ID="hlnkDuplicateNotInUse" runat="server" CssClass="elink3"></asp:HyperLink>
                </li>
                <li>
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
                        <div class="clear">
                        </div>
                    </div>
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="GridGRN" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            CellSpacing="1" DataKeyNames="InvoiceNumber" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                            GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                            PagerStyle-CssClass="PagerStyle"
                            AlternatingRowStyle-CssClass="Altrow" Width="100%" AllowPaging="True" OnPageIndexChanging="GridGRN_PageIndexChanging">
                            <RowStyle CssClass="gridrow" />
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="FromSalesChannelCode"
                                    HeaderText="From SalesChannel Code">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="InvoiceNumber"
                                    HeaderText="Invoice Number">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ToSalesChannelCode"
                                    HeaderText="To SalesChannel Code">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="gridheader" />
                            <EditRowStyle CssClass="editrow" />
                            <AlternatingRowStyle CssClass="Altrow" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="margin-bottom">
            <asp:Button ID="Btnsave" runat="server" CssClass="buttonbg" Text="Save" ValidationGroup="Add"
                OnClick="Btnsave_Click" Visible="false" />
            <%--  <asp:Button ID="BtnCancel" runat="server" CssClass="buttonbg" Text="Cddddancel" OnClick="BtnCancel_Click"
                                    Visible="false" />--%>
        </div>
    </asp:Panel>
</asp:Content>
