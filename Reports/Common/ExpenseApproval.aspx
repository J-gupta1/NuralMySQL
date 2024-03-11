<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExpenseApproval.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Reports_Common_ExpenseApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div>
        <asp:UpdatePanel ID="UpdMain" runat="server">
            <ContentTemplate>
                <div>
                    <uc4:ucMessage ID="ucMessage1" runat="server" />
                    <div class="clear"></div>
                    <div class="mainheading">
                        View Expense Detail For Approve/Reject 
                    </div>
                    <div class="contentbox">
                        <div class="mandatory">
                            (*) Marked fields are mandatory. (+) Fill at least one of them.
                        </div>
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">Entity Type:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <asp:DropDownList CssClass="formselect" ID="ddlEntityType" AutoPostBack="true" OnSelectedIndexChanged="ddlEntityType_SelectedIndexChanged" runat="server">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </li>
                                <li class="text">Entity Type Name:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <asp:DropDownList ID="ddlEntityTypeName" CssClass="formselect" runat="server">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </li>
                                <li class="text">Approval Status:<font class="error">+</font></li>
                                <li class="field">
                                    <asp:DropDownList ID="ddlApprovalStatus" CssClass="formselect" runat="server">
                                    </asp:DropDownList>
                                </li>
                                <li class="text">From Date:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>
                                <li class="text">To Date:<font class="error">+</font>
                                </li>
                                <li class="field">
                                    <uc1:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date." ValidationGroup="SearchNo"
                                        runat="server" />
                                </li>
                                <li class="field3">
                                    <div class="float-margin">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg"
                                            CausesValidation="true" ValidationGroup="Search" OnClick="btnSearch_Click" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg"
                                            CausesValidation="false" OnClick="btnCancel_Click" />
                                    </div>
                                    <div class="float-margin">
                                        <asp:Button ID="btnExportexcel" runat="server" Text="Export To Excel" CssClass="buttonbg"
                                            CausesValidation="true" OnClick="btnExportexcel_Click" />
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="mainheading">
                        Bulk Upload Expense For Approve/Reject 
                    </div>
                    <div class="contentbox">
                        <div class="H25-C3-S">
                            <ul>
                                <li class="text">Upload File: <span class="error">*</span>
                                </li>
                                <li class="field">
                                    <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />

                                    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                                </li>
                                <li class="field3">
                                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload"
                                        OnClick="btnUpload_Click" />
                                </li>

                                <li class="link">
                                    <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                                    <asp:HyperLink ID="hlnkDuplicateNotInUse" runat="server" CssClass="elink3"></asp:HyperLink>
                                    <asp:HyperLink ID="hlnkBlankNotInUse" runat="server" CssClass="elink3"></asp:HyperLink>
                                </li>

                            </ul>
                        </div>
                    </div>
                </div>
                <div>
                    <asp:Panel ID="PnlGrid" Visible="false" runat="server">
                        <div class="mainheading">
                            View Expense Details For Approve/Reject                                                  
                        </div>
                        <div class="contentbox">
                            <div class="grid">
                                <asp:GridView ID="gvExpenseDetail" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="Altrow"
                                    bgcolor="" BorderWidth="0px" DataKeyNames="ExpenseID" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                    AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>' OnRowDataBound="gvExpenseDetail_RowDataBound">
                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                    <PagerStyle CssClass="gridfooter" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkboxExpenseID" runat="server" Checked="false" Visible='<%# Convert.ToBoolean(Eval("ApproveStatus")) %>' />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Expense Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Expense Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpenseDate" runat="server" Text='<%# Bind("ExpenseDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="EntityType">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEntityType" runat="server" Text='<%# Bind("EntityType") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="User Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEntityTypeName" runat="server" Text='<%# Bind("EntityTypeName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Category Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategoryName" runat="server" Text='<%# Bind("CategoryName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Approve Amount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtApprovedgamount" runat="server" CssClass="formfields-W70" MaxLength="10" Text='<%# Bind("Amount") %>' Visible='<%# Convert.ToBoolean(Eval("ApproveStatus")) %>'></asp:TextBox>
                                                <asp:TextBox ID="txtamountapprove" runat="server" CssClass="formfields-W70" MaxLength="10" Enabled="false" Text='<%# Bind("ApproveAmount") %>' Visible='<%# Convert.ToBoolean(Eval("AmountApproveStatus")) %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Approve Remarks">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtApproveremarks" runat="server" CssClass="formfields-W70" MaxLength="50" Text='<%# Bind("ApproveRemark") %>' Enabled='<%# Convert.ToBoolean(Eval("ApproveRemarkStatus")) %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="dvFooter" runat="server" class="pagination">
                                <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                            </div>
                            <div class="clear2">
                            </div>
                            <div>
                                <asp:Button ID="btnApprove" runat="server" OnClick="btnApprove_Click" CausesValidation="false" CssClass="buttonbg" Text="Approve" ValidationGroup="vrgpSubmit" />


                                &nbsp;


                            <asp:Button ID="btnReject" runat="server" OnClick="btnReject_Click" CausesValidation="false" CssClass="buttonbg" Text="Reject" ValidationGroup="vrgpSubmit" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportexcel" />
                <asp:PostBackTrigger ControlID="btnCancel" />
                <asp:PostBackTrigger ControlID="btnSearch" />
                <asp:PostBackTrigger ControlID="gvExpenseDetail" />
                <asp:PostBackTrigger ControlID="btnApprove" />
                <asp:PostBackTrigger ControlID="btnReject" />
                <asp:PostBackTrigger ControlID="btnUpload" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
