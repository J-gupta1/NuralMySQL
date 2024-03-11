<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManagePrice.aspx.cs" Inherits="Masters_HO_Common_ManagePrice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../../Assets/Beetel/CSS/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <link href="../../Assets/Beetel/CSS/modal.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src=""></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMsg" runat="server" />
    <div class="mainheading">
        Add / Edit Price
    </div>
    <div class="export">
        <asp:LinkButton ID="LBViewPrice" runat="server" CausesValidation="False" CssClass="elink7"
            OnClick="LBViewPrice_Click">View List</asp:LinkButton>
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Price List:<span class="error">*</span>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbPriceList" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="reqPriceList" runat="server" ControlToValidate="cmbPriceList"
                                    CssClass="error" Display="Dynamic" ErrorMessage="Please select price list" InitialValue="0"
                                    SetFocusOnError="true" ValidationGroup="AddPrice"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="text">Effective Date:<span class="error">*</span>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." RangeErrorMessage="Date should be greater then equal to current date."
                                IsRequired="true" ValidationGroup="AddPrice" />
                        </li>
                    </ul>
                    <div class="clear"></div>
                    <ul>
                        <li class="text">Upload File: <span class="error">*</span>
                        </li>
                        <li class="field">
                            <asp:FileUpload ID="fuploadPrice" CssClass="fileuploads" runat="server" />
                        </li>
                        <li class="field3">
                            <asp:Button ID="btnUpload" runat="server" CssClass="buttonbg" OnClick="btnUpload_Click"
                                TabIndex="11" Text="Upload" />
                        </li>
                        <li class="link">
                            <asp:LinkButton ID="DwnldSKUCodeTemplate" runat="server" Text="Download SKUCode"
                                OnClick="DwnldSKUCodeTemplate_Click" CssClass="elink2"></asp:LinkButton>
                        </li>
                        <li class="link">
                            <a class="elink2" href="../../Excel/Templates/PriceMaster.xlsx">Download Template</a>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnUpload" />
                <asp:PostBackTrigger ControlID="DwnldSKUCodeTemplate" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <asp:Panel ID="pnlGrid" Visible="true" runat="server">
        <asp:UpdatePanel ID="updGrid" runat="server">
            <ContentTemplate>
                <div class="mainheading">
                    Price List
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="gvPrice" runat="server" AutoGenerateColumns="false" FooterStyle-VerticalAlign="Top"
                            FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                            AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                            HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                            BorderWidth="0px" Width="100%" AllowPaging="false" SelectedStyle-CssClass="gridselected">
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUCode"
                                    HeaderText="SKU Code">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="WHPrice"
                                    HeaderText="WHPrice" DataFormatString="{0:n}">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SDPrice"
                                    HeaderText="SD Price" DataFormatString="{0:n}">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="MDPrice"
                                    HeaderText="MD Price" DataFormatString="{0:n}">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="RetailerPrice"
                                    HeaderText="Retailer Price" DataFormatString="{0:n}">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="MOP"
                                    HeaderText="MOP" DataFormatString="{0:n}">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="MRP"
                                    HeaderText="MRP" DataFormatString="{0:n}">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Error"
                                    HeaderText="Error">
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
            <div class="float-margin">
                <asp:Button ID="Btnsave" runat="server" CssClass="buttonbg" Text="Save" OnClick="Btnsave_Click"
                    ValidationGroup="AddPrice" />
            </div>
            <div class="float-margin">
                <asp:Button ID="BtnCancel" runat="server" CssClass="buttonbg" Text="Cancel" OnClick="BtnCancel_Click" />
            </div>
            <div class="clear"></div>
        </div>
    </asp:Panel>
</asp:Content>
