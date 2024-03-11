<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UploadMappingSchema.aspx.cs" Inherits="Masters_Common_UploadMappingSchema" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMsg" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Uplaod
    </div>
    <div class="contentbox">
        <%--<asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
                                    <ContentTemplate>--%>
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Master:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="cmbTableName" runat="server" CssClass="formselect">
                    </asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="req" runat="server" CssClass="error" InitialValue="0"
                            ControlToValidate="cmbTableName" ErrorMessage="Please select sales channel type"
                            Display="Dynamic" ValidationGroup="Serach"></asp:RequiredFieldValidator>
                    </div>
                </li>
                <li class="text">Upload File: <span class="error">*</span>
                </li>
                <li class="field">
                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                </li>
                <li class="field3">
                    <div class="float-margin">
                        <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                            OnClick="btnUpload_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancelall" CssClass="buttonbg" runat="server" Text="Cancel" TabIndex="12"
                            OnClick="btnCancelAll_Click" />
                    </div>
                </li>
            </ul>
        </div>
        <%--</ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpload" />
                                        <asp:PostBackTrigger ControlID="DwnldReferenceCodeTemplate" />
                                         <asp:PostBackTrigger ControlID="DwnldDustributorCodeTemplate" />--%>
        <%--  </Triggers>
                                </asp:UpdatePanel>--%>
    </div>
    <asp:Panel runat="server" ID="pnlMapppingdata" Visible="false">
        <div class="mainheading">
            Mapping
        </div>
        <div class="contentbox">
            <div class="H25-C3-S">
                <ul>
                    <li class="text">Excel Sheet Columns:<span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="cmbExcelSheetSequence" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="error" InitialValue="0"
                                ControlToValidate="cmbExcelSheetSequence" ErrorMessage="Please select a excel sheet column to be map ith the valid system defined columns"
                                Display="Dynamic" ValidationGroup="map"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="text-field">
                        <asp:Button ID="btnMap" runat="server" CssClass="buttonbg" Text="Map"
                            OnClick="BtnMap_Click" />
                    </li>
                    <li class="text">Valid Format:<span class="error">*</span>
                    </li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="cmbSystemSequence" runat="server" CssClass="formselect">
                            </asp:DropDownList>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="error" InitialValue="0"
                                ControlToValidate="cmbSystemSequence" ErrorMessage="Please select select the column to e mapped withh the excel sheet column"
                                Display="Dynamic" ValidationGroup="Map"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </asp:Panel>
    <asp:UpdatePanel ID="updMappedGrd" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlMappedGrid" Visible="false" runat="server">
                <div class="mainheading">
                    Mapping Preview
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="grdMappedData" runat="server" CellPadding="4" AutoGenerateColumns="false"
                            CellSpacing="1" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                            GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                            AlternatingRowStyle-CssClass="Altrow" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Excel Column" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExcelName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ExcelSheetColumn")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="System Column" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSysName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SystemColumn")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle CssClass="gridrow" />
                            <HeaderStyle CssClass="gridheader" />
                            <EditRowStyle CssClass="editrow" />
                            <AlternatingRowStyle CssClass="Altrow" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>

                        <%--<div style="float: left; padding-top: 10px; width: 300px;">
                        </div>--%>
                    </div>
                </div>
                <div class="margin-bottom">
                    <div class="float-margin">
                        <asp:Button ID="btnProceedMap" runat="server" CssClass="buttonbg" Text="Proceed" ValidationGroup="Add"
                            OnClick="BtnProceedmap_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnCancelMap" runat="server" CssClass="buttonbg" Text="Cancel" OnClick="BtnCancelMapping_Click" />
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updFinalGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlFinalGrid" Visible="false" runat="server">
                <div class="mainheading">
                    Final Table
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="grdFinal" runat="server" AutoGenerateColumns="True" CellPadding="4"
                            CellSpacing="1" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                            GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                            AlternatingRowStyle-CssClass="Altrow" Width="100%">
                            <RowStyle CssClass="gridrow" />
                            <HeaderStyle CssClass="gridheader" />
                            <EditRowStyle CssClass="editrow" />
                            <AlternatingRowStyle CssClass="Altrow" />
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                        <%--  <div style="float: left; padding-top: 10px; width: 300px;">
                        </div>--%>
                    </div>
                </div>
                <div class="margin-bottom">
                    <div class="float-margin">
                        <asp:Button ID="btnFinalSubmit" runat="server" CssClass="buttonbg" Text="Submit" ValidationGroup="Add"
                            OnClick="BtnFinalSubmit_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnFinalCancel" runat="server" CssClass="buttonbg" Text="Cancel" OnClick="BtnFinalCancel_Click" />
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

