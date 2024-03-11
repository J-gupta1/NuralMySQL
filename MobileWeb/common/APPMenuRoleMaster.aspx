<%@ Page Language="C#" AutoEventWireup="true" CodeFile="APPMenuRoleMaster.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="MobileWeb_common_APPMenuRoleMaster" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/UCUserDetail.ascx" TagName="UCUserDetail" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/ucStatusControl.ascx" TagName="ucStatus" TagPrefix="uc5" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .grid22 {
            width: 100%;
            height: auto;
            max-height: 500px;
            overflow: auto;
            background-color: #dddcdc;
            scrollbar-base-color: #fff;
            scrollbar-3dlight-color: #fff;
            scrollbar-arrow-color: #505050;
            scrollbar-darkshadow-color: #fff;
            scrollbar-face-color: #6b6d6e;
            scrollbar-highlight-color: #fff;
            scrollbar-shadow-color: #fff;
            scrollbar-track-color: #fff;
            border-radius: 4px;
        }
    </style>
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
            APP Menu Role       
        </div>
        <div class="innerarea">
            <div class="H20-C3">
                <ul>
                    <li class="">Company Name/Access key:<span class="error">*</span></li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="ddlCompanyName" runat="server" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged" AutoPostBack="true" CssClass="formselect">
                            </asp:DropDownList>

                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="rfvtxtmenuname" runat="server" ErrorMessage="Please Select Company Name"
                                ControlToValidate="ddlCompanyName" SetFocusOnError="True" ValidationGroup="submit" CssClass="error" Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                    </li>
                    <li class="">Role:<span class="error">*</span></li>
                    <li class="field">
                        <div>
                            <asp:DropDownList ID="ddlRoleName" runat="server" OnSelectedIndexChanged="ddlRoleName_SelectedIndexChanged" AutoPostBack="true" CssClass="formselect">
                            </asp:DropDownList>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="rvddlddlParentCategoryName" runat="server" ErrorMessage="Please Select Role"
                                ControlToValidate="ddlRoleName" SetFocusOnError="True" ValidationGroup="submit" CssClass="error" Display="Dynamic"
                                InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                    </li>

                    <li class=""><div class="float-margin">
                            <asp:Button ID="btnUpdateMenu" runat="server" Text="Save/Update Menu Mapping" CssClass="buttonbg"
                                OnClick="btnUpdateMenu_Click"  ValidationGroup="submit"/>
                        </div></li>
                    <li class="field">
                        
                        <div class="float-margin">
                            <asp:Button ID="btnCancle" runat="server" Text="Reset" CssClass="buttonbg"
                                OnClick="btnCancle_Click" CausesValidation="false"/>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <div class="subheading">
            APP Menu List      
        </div>
        <div class="grid22" style="border: 1px solid #dddddd; overflow: auto">
            <asp:GridView ID="GvMenuName" runat="server" AutoGenerateColumns="false" AllowPaging="false" PageSize='<%$ AppSettings:GridViewPageSize %>'
                BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="MeniId" FooterStyle-HorizontalAlign="Left"
                FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" EmptyDataText="No record found"
                RowStyle-VerticalAlign="top" Width="100%" AlternatingRowStyle-CssClass="Altrow"
                RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                HeaderStyle-CssClass="gridheader" OnRowDataBound="GvMenuName_RowDataBound">
                <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                <Columns>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Select">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkboxMenuID" runat="server" Checked='<%# Convert.ToBoolean(Eval("Status")) %>' />
                             <asp:Label ID="lblMenuStatus" runat="server" Value='<%#Eval("Status") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="MenuName" HeaderText="Menu Name" HeaderStyle-HorizontalAlign="Left" />
                    <asp:TemplateField HeaderText="MenuType">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnmenutypeid" runat="server" Value='<%#Eval("MenuTypeID") %>' />
                            <asp:DropDownList ID="ddlMenuType" runat="server" CssClass="formselect">
                                <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Main Menu"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Check-In Menu"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Reports"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Approve"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Display Mode">
                        <ItemTemplate>
                            <asp:TextBox ID="txtdisplaymode" runat="server" CssClass="formfields-W70" MaxLength="10" Text='<%# Bind("DisplayOrder") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                <PagerStyle CssClass="PagerStyle" />
            </asp:GridView>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="clear">
    </div>
    <%--<div class="box1">
        <div class="subheading">
            Search APP Menu Role
        </div>
        <asp:UpdatePanel ID="updSearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="innerarea">
                    <div class="H20-C3">
                        <ul>
                            <li class="text">Menu Name:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlSearchMenu" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="text">Role:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlRoleNameSearch" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </li>
                            <li class="text">Status:<span class="optional-img">&nbsp;</span></li>
                            <li class="field">
                                <asp:DropDownList ID="ddlStatusSearch" runat="server" CssClass="formselect">
                                    <asp:ListItem Value="255" Text="Select"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="Inactive"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Active"></asp:ListItem>
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
    </div>--%>
    <%--<asp:UpdatePanel ID="updpnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="box1" runat="server" id="divgrd">
                <div class="subheading">
                    <div class="float-left">
                        APP Menu Role List
                    </div>

                </div>
                <div class="export">
                    <asp:Button ID="Exporttoexcel" runat="server"
                        OnClick="Exporttoexcel_Click" CausesValidation="False" CssClass="excel" />
                </div>
                <div class="innerarea">
                    <div class="grid1">
                        <asp:GridView ID="grdvList" runat="server" AutoGenerateColumns="false"
                            BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="MenuRoleId" FooterStyle-HorizontalAlign="Left"
                            FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" EmptyDataText="No record found"
                            RowStyle-VerticalAlign="top" Width="100%" AlternatingRowStyle-CssClass="Altrow"
                            RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                            HeaderStyle-CssClass="gridheader" OnRowCommand="grdvList_RowCommand">
                            <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                            <Columns>
                                <asp:BoundField DataField="MenuName" HeaderText="Menu Name" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="RoleName" HeaderText="Role" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="MenuType" HeaderText="MenuType"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="DisplayOrder" HeaderText="Display Order" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="CreatedOn" HeaderText="Created On" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="80px" />
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("MenuRoleId") %>'
                                            CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("MenuRoleId") %>'
                                            CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                            <PagerStyle CssClass="PagerStyle" />
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
    </asp:UpdatePanel>--%>
</asp:Content>
