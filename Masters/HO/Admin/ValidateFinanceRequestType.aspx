<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="ValidateFinanceRequestType.aspx.cs" Inherits="Masters_HO_Admin_ValidateFinanceRequestType" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div align="center">
        <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
            <tr>
                <td align="left" valign="top">
                    <table cellspacing="0" cellpadding="0" width="965" border="0">
                        <tr>
                            <td valign="top" align="left">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <uc1:ucMessage ID="ucMsg" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0" style="float: left;">
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td align="left" valign="top" class="tableposition">
                                                                            <div class="mainheading">
                                                                                API User Request Type Mapping
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="left" class="tableposition">
                                                                <div class="contentbox">
                                                                    <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                                        <tr>
                                                                            <td colspan="6" height="20" class="mandatory" valign="top">(*) marked fields are mandatory.
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="formtext" valign="top" align="right" width="224" height="50">
                                                                                <asp:Label ID="lblAPIUserRequestTypeMapping" CssClass="formtext" runat="server">API User Request Type Mapping:<font class="error">*</font></asp:Label>
                                                                            </td>
                                                                            <td width="224" align="left" valign="top">                                                                
                                                                                <div style="float: left; width: 135px;">
                                                                                    <asp:CheckBoxList ID="ChkListAPIUserRequestTypeMapping" runat="server"></asp:CheckBoxList>
                                                                                  
                                                                                </div>
                                                                            </td>
                                                                            <td class="formtext" valign="top" align="right" width="87"></td>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                        </tr>

                                                        <asp:Panel ID="pnlRegion" Visible="false" runat="server">
                                                            <tr>
                                                                <td align="left" valign="top" class="formtext">Select Location:<font class="error">*</font>
                                                                </td>
                                                                <td align="left" valign="top" colspan="5">
                                                                    <div style="width: 790px; overflow: auto;">
                                                                        <asp:CheckBoxList ID="chkRegion" runat="server" CssClass="gridspace" RepeatColumns="4"
                                                                            RepeatDirection="Horizontal" Width="755px">
                                                                        </asp:CheckBoxList>
                                                                       
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </asp:Panel>
                                                        <tr>
                                                            <td align="left" valign="top">&nbsp;
                                                            </td>
                                                            <td align="left">
                                                                <asp:Button ID="btnCreateUser" Text="Submit" runat="server" CausesValidation="true"
                                                                    ValidationGroup="AddUserValidationGroup" OnClick="btnCreateUser_Click" ToolTip="Submit"
                                                                    CssClass="buttonbg" />&nbsp;&nbsp;
                                                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                                                                    OnClick="btnCancel_Click" />
                                                            </td>
                                                            <td align="left" class="formtext" valign="top"></td>
                                                            <td></td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="left" valign="top">&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                    <asp:PostBackTrigger ControlID="btnCreateUser" />

                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" height="10"></td>
                        </tr>
                        <tr>
                            <td align="left" valign="top"></td>
                        </tr>

                        <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="90%" align="left" class="tableposition">
                                            <div class="mainheading">
                                                User List
                                            </div>
                                        </td>
                                        <td width="10%" align="right">&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" class="tableposition">
                                <div class="contentbox">
                                    <div class="grid2">
                                        <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="grdvwUserList" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                                                    RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                                                    HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                                                    AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                                                    HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                                                    BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                                                    PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected"
                                                    OnPageIndexChanging="grdvwUserList_PageIndexChanging" DataKeyNames="UserID" 
                                                   >
                                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Login Name" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <div style="width: 100px; overflow: hidden; word-wrap: break-word;">
                                                                    <asp:Label runat="server" Text='<%#Eval("LoginName") %>'></asp:Label>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Display Name" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <div style="width: 100px; overflow: hidden; word-wrap: break-word;">
                                                                    <asp:Label runat="server" Text='<%#Eval("DisplayName") %>'></asp:Label>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:ImageButton CommandArgument='<%#Eval("UserID") %>' runat="server" ID="btnEdit"
                                                                    ImageUrl='<%#"~/" + strAssets + "../CSS/Images/edit.png"%>' OnClick="btnEdit_Click"
                                                                    ToolTip="Edit User" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerStyle CssClass="PagerStyle" />
                                                </asp:GridView>

                                            </ContentTemplate>
 
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" height="10"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <%--     <uc3:footer ID="Footer1" runat="server" />--%>
    </div>
</asp:Content>

