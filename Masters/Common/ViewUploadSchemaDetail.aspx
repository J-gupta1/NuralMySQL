<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ViewUploadSchemaDetail.aspx.cs" Inherits="Masters_Common_ViewUploadSchemaDetail" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
            <div class="mainheading">
                Table Specification
            </div>
            <div class="export">
                <asp:LinkButton ID="LBViewSalesChannel" runat="server" CausesValidation="False" OnClick="LBViewSchema_Click"
                    CssClass="elink7">Create Schema</asp:LinkButton>
            </div>
            <div class="contentbox">
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Table Name:
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbTableName" runat="server" CssClass="formselect"
                                    AutoPostBack="True" OnSelectedIndexChanged="cmbTableName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="req" runat="server" CssClass="error" InitialValue="0"
                                    ControlToValidate="cmbTableName" ErrorMessage="Please select sales channel type"
                                    Display="Dynamic" ValidationGroup="Serach"></asp:RequiredFieldValidator>
                            </div>
                        </li>
                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" runat="server" ValidationGroup="Serach" CausesValidation="true"
                                    CssClass="buttonbg" Text="Search" OnClick="btnSearch_Click" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" ToolTip="Cancel"
                                    OnClick="btnCancel_Click" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                        <div class="mainheading">
                            List
                        </div>
                        <%--      <asp:UpdatePanel ID="UpdGrid" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>--%>
                        <div class="contentbox">
                            <div class="grid1">
                                <asp:GridView ID="grdUploadSchema" runat="server" AlternatingRowStyle-CssClass="Altrow"
                                    AutoGenerateColumns="False" bgcolor="" AllowPaging="True"
                                    BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="UploadTableID"
                                    FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                                    GridLines="None" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected"
                                    Width="100%" OnRowCancelingEdit="grdUploadSchema_RowCancelingEdit"
                                    OnRowCommand="grdUploadSchema_RowCommand"
                                    OnRowDataBound="grdUploadSchema_RowDataBound"
                                    OnRowEditing="grdUploadSchema_RowEditing"
                                    OnRowUpdating="grdUploadSchema_RowUpdating">
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="TableID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTableID" runat="server" Text='<%# Bind("UploadTableID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Table Column Name">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtTblColumnName" runat="server" CssClass="formfields" Text='<%# Bind("TableColumnName") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTblColumnName" runat="server" Text='<%# Bind("TableColumnName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Table Column Data Type">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="cmbTblColumnDataType" CssClass="formselect" runat="server">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTblColumnDataType" runat="server" Text='<%# Bind("TableColumnDataType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Excel Sheet Column Name">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtExcelColumnName" runat="server" CssClass="formfields" 
                                                    Text='<%# Bind("ExcelSheetColumnName") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblExcelColumnName" runat="server"
                                                    Text='<%# Bind("ExcelSheetColumnName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Excel Sheet Data Type">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="cmbExcelDataType" CssClass="formselect" runat="server">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblExcelDataType" runat="server" Text='<%# Bind("ExcelSheetDataType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Column Constraint">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="cmbColumnConstraints" CssClass="formselect" runat="server">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblColumnConstraints" runat="server" Text='<%# Bind("ColumnConstraint") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Max Length">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtMaxLength" runat="server" CssClass="formfields" Text='<%# Bind("MaxLength") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblMaxLength" runat="server" Text='<%# Bind("MaxLength") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Column Sequence">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="cmbColumnSequence" CssClass="formselect" runat="server" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblColumnSequence" runat="server" Text='<%# Bind("ColumnSequence") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit">
                                            <EditItemTemplate>
                                                <div style="width: 60px">
                                                    <div class="float-margin">
                                                        <asp:ImageButton ID="btnUpdate" runat="server" CausesValidation="false"
                                                            CommandName="Update" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/icon_update.gif"%>' />
                                                    </div>
                                                    <div class="float-margin">
                                                        <asp:ImageButton ID="btnCancel" runat="server" CausesValidation="false"
                                                            CommandName="Cancel" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/icon_cancel.gif"%>' />
                                                    </div>
                                                </div>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false"
                                                    CommandName="Edit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/icon_edit.gif"%>' CommandArgument='<%# Eval("UploadTableID")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                            <ItemStyle Wrap="False"></ItemStyle>
                                            <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnActiveDeactive" runat="server" CommandArgument='<%#Eval("UploadTableID") %>'
                                                    CommandName="Active" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                                    ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <AlternatingRowStyle CssClass="Altrow" />
                                    <PagerStyle CssClass="PagerStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                        <%-- </ContentTemplate>
                                                      
                                                    </asp:UpdatePanel>--%>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
        <%-- <Triggers>
            <asp:PostBackTrigger ControlID="ExportToExcel" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
