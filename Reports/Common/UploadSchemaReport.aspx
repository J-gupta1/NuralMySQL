<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" 
AutoEventWireup="true" CodeFile="UploadSchemaReport.aspx.cs" Inherits="Reports_Common_UploadSchemaReport" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Excel Upload Schema
    </div>
    <div class="contentbox">        
        <div class="H25-C3-S">
            <ul>
                <li class="text">
                    <asp:Label ID="lblrole" runat="server" Text="">Form Name:</asp:Label>
                </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="cmbTableName" CssClass="formselect"
                            runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="cmbTableName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator runat="server" ID="valtype" ControlToValidate="cmbTableName"
                            CssClass="error" ErrorMessage="Please select a form " InitialValue="0" ValidationGroup="insert" />
                    </div>
                </li>
                <li class="field3">
                    <asp:Button ID="btnsearch" Text="Search" runat="server" OnClick="btnSerch_Click"
                        ValidationGroup="insert" CssClass="buttonbg" CausesValidation="True" />
                </li>
            </ul>
        </div>
    </div>
    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
        <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="mainheading">
                    List
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="grdUploadSchema" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                            BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="ColumnName" FooterStyle-HorizontalAlign="Left"
                            FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="Altrow"
                            RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                            RowStyle-VerticalAlign="top" Width="100%" OnPageIndexChanging="grdUploadSchema_PageIndexChanging" EmptyDataText="No Data Found">
                            <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                            <Columns>
                                <asp:BoundField DataField="ColumnName" HeaderText="Column Name" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ColumnConstraint" HeaderText="Validations" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DataType" HeaderText="data Format" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MinLength" HeaderText="Minimum Length" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MaxLength" HeaderText="Maximum Length" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ColumnSequence" HeaderText="Column Sequence" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>

