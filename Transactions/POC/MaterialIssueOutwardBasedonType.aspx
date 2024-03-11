<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="MaterialIssueOutwardBasedonType.aspx.cs" Inherits="Transactions_POC_MaterialIssueOutwardBasedonType" %>

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
                <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="always">
                            <ContentTemplate>--%>
                <li class="text">Select Warehouse: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="formselect" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlWarehouse_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlWarehouse" Display="Dynamic"
                        CssClass="error" ValidationGroup="EntryValidation" runat="server" InitialValue="0"
                        ErrorMessage="Please Select Warehouse."></asp:RequiredFieldValidator>
                </li>
                <%--   </ContentTemplate>
                                          <Triggers>
                                          <asp:AsyncPostBackTrigger ControlID="ddlWarehouse" EventName="SelectedIndexChanged" />
                                          </Triggers>
                        </asp:UpdatePanel>--%>
                <li class="text">User Name: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlUser" runat="server" CssClass="formselect" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlUser_SelectedIndexChanged">
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlUser" Display="Dynamic"
                        CssClass="error" ValidationGroup="EntryValidation" runat="server" InitialValue="0"
                        ErrorMessage="Please Select User."></asp:RequiredFieldValidator>
                </li>
                <li class="text">Remarks: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtRemarks" runat="server" MaxLength="20" CssClass="form_textarea"
                        TextMode="MultiLine"></asp:TextBox>
                </li>
            </ul>
            <ul>
                <li class="text">Docket Number: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtDocketNo" runat="server" MaxLength="20" CssClass="formfields"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtDocketNo" Display="Dynamic"
                        CssClass="error" ValidationGroup="EntryValidation" runat="server" ErrorMessage="Please Insert Docket Number."></asp:RequiredFieldValidator>
                </li>
                <li class="text" visible="false">Docket Date: <span class="error">*</span>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDateForGRN" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                        IsRequired="true" ValidationGroup="EntryValidation" />
                </li>
                <li class="text">Mode of Receipt: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlmodeofReceipt" runat="server" CssClass="formselect">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                        <asp:ListItem Text="By Hand" Value="1"></asp:ListItem>
                        <asp:ListItem Text="By Courier" Value="2"></asp:ListItem>
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="ddlmodeofReceipt" Display="Dynamic"
                        CssClass="error" ValidationGroup="EntryValidation" runat="server" InitialValue="0"
                        ErrorMessage="Please Select Mode."></asp:RequiredFieldValidator>
                </li>
            </ul>
            <ul>
                <li class="text">Courier Name: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:TextBox ID="txtCourierName" runat="server" MaxLength="20" CssClass="formfields"></asp:TextBox>
                </li>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                </li>
                <li class="field3">
                    <asp:Button ID="BtnUpload" runat="server" CssClass="buttonbg" Text="Upload" ValidationGroup="EntryValidation"
                        CausesValidation="true" OnClick="BtnUpload_Click" />
                </li>
            </ul>
        </div>
        <div class="formlink">
            <ul>
                <li>
                    <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>
                </li>
                <li>
                    <a class="elink2" href="../../Excel/Templates/GRNBatchOrSerial.xlsx">Download Template</a>
                </li>
            </ul>
        </div>
    </div>
    <asp:Panel ID="pnlGrid" Visible="false" runat="server">
        <div id="Div1" class="mainheading" runat="server">
            Upload Preview
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="GridGRN" runat="server" AutoGenerateColumns="false" CellPadding="4"
                            CellSpacing="1" DataKeyNames="SKUCode,BatchNumber" EditRowStyle-CssClass="editrow"
                            EmptyDataText="No Record Found" GridLines="None" HeaderStyle-CssClass="gridheader"
                            RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" Width="100%"
                            AllowPaging="true" OnPageIndexChanging="GridGRN_PageIndexChanging">
                            <RowStyle CssClass="gridrow" />
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUCode"
                                    HeaderText="SKUCode">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Quantity"
                                    HeaderText="Quantity">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BatchNumber"
                                    HeaderText="BatchNumber">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SerialNumber"
                                    HeaderText="SerialNumber">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ERRORMessage"
                                    HeaderText="ErrorMessage">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="gridheader" />
                            <EditRowStyle CssClass="editrow" />
                            <AlternatingRowStyle CssClass="Altrow" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div style="float: left; padding-top: 10px; width: 300px;">
                </div>
            </div>
        </div>
        <div class="float-margin">
            <asp:Button ID="Btnsave" runat="server" CssClass="buttonbg" Text="Save" ValidationGroup="Add"
                OnClick="Btnsave_Click" />
        </div>
        <div class="float-margin">
            <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" OnClick="btnCancel_Click" />
        </div>
    </asp:Panel>
</asp:Content>
