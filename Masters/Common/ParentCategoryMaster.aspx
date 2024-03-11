<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ParentCategoryMaster.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Masters_Common_ParentCategoryMaster" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/UCUserDetail.ascx" TagName="UCUserDetail" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/ucStatusControl.ascx" TagName="ucStatus" TagPrefix="uc5" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div class="box1">
        <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div runat="server" id="dvMsg">
                    <uc4:ucMessage ID="ucMsg" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="subheading">
           Parent Category  Save
           
        </div>
        <asp:Panel ID="pnlSubmit" runat="server" DefaultButton="btnSubmit">
            <asp:UpdatePanel ID="updSave" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="innerarea">
                        <div class="H25-C3">

                            <ul>
                                <li class="text">Parent Category Name:<span class="error">*</span></li>
                                <li class="field">
                                    <div>
                                        <asp:TextBox ID="txtParentCategoryName" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>
                                    </div>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvtxtcategoryname" runat="server" ErrorMessage="Please Enter Parent Category Name"
                                            ControlToValidate="txtParentCategoryName" SetFocusOnError="True" ValidationGroup="submit" CssClass="error" Display="Dynamic"
                                            ></asp:RequiredFieldValidator>
                                    </div>
                                </li>
                                <li class="text">Category For:<span class="error">*</span></li>
                                <li class="field">
                                    <div>
                                        <asp:DropDownList ID="ddlCategoryFor" runat="server" CssClass="formselect">
                                        </asp:DropDownList>
                                    </div>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rvddlCategoryFor" runat="server" ErrorMessage="Select Category For"
                                            ControlToValidate="ddlCategoryFor" SetFocusOnError="True" ValidationGroup="submit" CssClass="error" Display="Dynamic"
                                            InitialValue="255"></asp:RequiredFieldValidator>
                                    </div>
                                </li>
                                <li class="text">
                                    <div class="float-margin padding-right">
                                        Active:<span class="optional-img">&nbsp;</span>
                                    </div>
                                    <div class="float-margin">
                                        <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                    </div>
                                </li>
                                <li class="field">
                                    <div class="float-margin">

                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonbg" ValidationGroup="submit" OnClick="btnSubmit_Click" />
                                    </div>
                                    <div class="float-margin">

                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonbg" OnClick="btnCancel_Click" />
                                    </div>
                                </li>
                            </ul>

                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <div class="clear">
    </div>
    <div class="box1">
        <div class="subheading">
             Search Parent Category
        </div>
        <asp:UpdatePanel ID="updSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="innerarea">
                    <div class="H20-C3">
                        <ul>
                            <li class="text">Parent Category Name:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:TextBox ID="txtParentCategoryNameSearch" runat="server" MaxLength="50" CssClass="formfields"></asp:TextBox>
                            </li>
                            <li class="text">Category For:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlCategoryForSearch" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="text"></li>
                            <li class="field">
                                <div class="float-margin">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg"
                                        OnClick="btnSearch_Click" />
                                </div>
                                <div class="float-margin">
                                    <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="buttonbg"
                                        OnClick="btnShowAll_Click" />
                                </div>
                            </li>
                        </ul>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="updpnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="box1" runat="server" id="divgrd">
                <div class="subheading">
                    <div class="float-left">
                       Parent Category List
                    </div>

                </div>
                <div class="export">
                    <asp:Button ID="Exporttoexcel" runat="server"
                        OnClick="Exporttoexcel_Click" CausesValidation="False" CssClass="excel" />
                </div>
                <div class="innerarea">
                    <div class="grid1">
                        <asp:GridView ID="grdvList" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            SelectedRowStyle-CssClass="selectedrow" CellSpacing="1" EmptyDataText="No Record Found"
                            GridLines="None" Width="100%" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                            AlternatingRowStyle-CssClass="Altrow" EditRowStyle-CssClass="editrow" OnRowCommand="grdvList_RowCommand"
                            OnRowEditing="grdvList_RowEditing" OnRowDataBound="grdvList_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ParentCategoryName" HeaderText="Parent Category Name" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="CategoryForName" HeaderText="CategoryFor"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="CreatedOn" HeaderText="Created On" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="80px" />
                                <asp:TemplateField ShowHeader="False" HeaderText="Action"  HeaderStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="110px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCategoryID" runat="server" Visible="false" Text='<%#Eval("ParentCategoryID") %>'></asp:Label>
                                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>'
                                            CommandName="edit" CommandArgument='<%#Eval("ParentCategoryID") %>' />
                                        <asp:ImageButton ID="btnActive" runat="server" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Active"))) %>'
                                            CommandName="active" CommandArgument='<%#Eval("ParentCategoryID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="dvFooter" runat="server" class="pagination">
                        <uc3:UCPagingControl ID="ucPagingControl1" runat="server" Visible="false" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Exporttoexcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
