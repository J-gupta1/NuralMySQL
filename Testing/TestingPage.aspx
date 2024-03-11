<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/ReportPage.master"
    CodeFile="TestingPage.aspx.cs" Inherits="Testing_TestingPage" EnableEventValidation="false" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="server">
    <%-- <uc2:header ID="Header1" runat="server" />--%>
    <%--  <asp:RequiredFieldValidator ID="ReqVHierarchyLevel" runat="server" ControlToValidate="ddlHierarchyLevel"
                                                                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select hierarchy level."
                                                                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>--%>
    <div align="center">
        <%--<asp:RequiredFieldValidator ID="ReqUserGroup" runat="server" ControlToValidate="ddlSalesChanelType"
                                                                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales chanel."
                                                                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>--%>
        <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
            <tr>
                <td align="left" valign="top" height="420">
                    <table cellspacing="0" cellpadding="0" width="965" border="0">
                        <tr>
                            <td valign="top" align="left">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <uc1:ucMessage ID="UcMessage1" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td align="left" valign="top" class="tableposition">
                                                                            <div class="mainheading">
                                                                                Add / Edit Role</div>
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
                                                                            <td colspan="6" height="20" class="mandatory" valign="top">
                                                                                (<font class="error">*</font>) marked fields are mandatory.
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="formtext" valign="top" align="right" width="10%">
                                                                                <asp:Label ID="lblRoleName" runat="server" AssociatedControlID="txtRoleName" CssClass="formtext">Role Name:<font class="error">*</font></asp:Label>
                                                                            </td>
                                                                            <td width="20%" align="left" valign="top">
                                                                                <asp:TextBox ID="txtRoleName" runat="server" CssClass="form_input2" MaxLength="50"
                                                                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>
                                                                                <br />
                                                                                <asp:RequiredFieldValidator ID="reqvaltxtRoleName" runat="server" ControlToValidate="txtRoleName"
                                                                                    CssClass="error" Display="Dynamic" SetFocusOnError="true" ErrorMessage="Please enter role name."
                                                                                    ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td class="formtext" valign="top" align="right" width="15%">
                                                                                <asp:Label ID="lblHierarchyLevel" runat="server" AssociatedControlID="ddlHierarchyLevel"
                                                                                    CssClass="formtext">Hierarchy Level:<font class="error">*</font></asp:Label>
                                                                            </td>
                                                                            <td valign="top" align="left" width="20%">
                                                                                <asp:DropDownList CausesValidation="true" ID="ddlHierarchyLevel" AutoPostBack="true"
                                                                                    runat="server" CssClass="form_select4">
                                                                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                                <cc1:ModalPopupExtender ID="ddlHierarchyLevel_ModalPopupExtender" DropShadow="true"
                                                                                    runat="server" TargetControlID="ddlHierarchyLevel" PopupControlID="pnl">
                                                                                </cc1:ModalPopupExtender>
                                                                                <asp:Panel ID="pnl" runat="server">
                                                                                    <asp:DropDownList CausesValidation="true" ID="DropDownList1" AutoPostBack="true"
                                                                                        runat="server" CssClass="form_select4">
                                                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </asp:Panel>
                                                                                <br />
                                                                                <%--  <asp:RequiredFieldValidator ID="ReqVHierarchyLevel" runat="server" ControlToValidate="ddlHierarchyLevel"
                                                                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select hierarchy level."
                                                                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>--%>
                                                                            </td>
                                                                            <td class="formtext" valign="top" align="right" width="15%" height="35">
                                                                                <asp:Label ID="lblSalesChanel" CssClass="formtext" runat="server" AssociatedControlID="ddlSalesChanelType">Sales Chanel Type:<font class="error">*</font></asp:Label>
                                                                            </td>
                                                                            <td width="20%" align="left" valign="top">
                                                                                <asp:DropDownList CausesValidation="true" ID="ddlSalesChanelType" runat="server"
                                                                                    CssClass="form_select4">
                                                                                </asp:DropDownList>
                                                                                <br />
                                                                                <%--<asp:RequiredFieldValidator ID="ReqUserGroup" runat="server" ControlToValidate="ddlSalesChanelType"
                                                                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales chanel."
                                                                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>--%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="formtext" valign="top" align="right">
                                                                                <asp:Label ID="lblRoleStatus" runat="server" AssociatedControlID="chkActive" CssClass="formtext"> Status :</asp:Label>
                                                                            </td>
                                                                            <td align="left" valign="top">
                                                                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:Button ID="btnCreateRole" Text="Add Role" runat="server" CausesValidation="true"
                                                                                    ValidationGroup="AddUserValidationGroup" ToolTip="Add Role" CssClass="buttonbg" />&nbsp;&nbsp;
                                                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                                                                    OnClick="btnCancel_Click" />
                                                                            </td>
                                                                            <td align="left" class="formtext" valign="top">
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" height="10">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td align="left" class="tableposition">
                                            <div class="mainheading">
                                                Search User</div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" class="tableposition">
                                            <div class="contentbox">
                                                <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr>
                                                                <td align="right" valign="top" width="90" height="35" class="formtext">
                                                                    Role Name:&nbsp;
                                                                </td>
                                                                <td align="left" valign="top" width="120">
                                                                    <asp:TextBox ID="txtRoleSearch" runat="server" MaxLength="100" CssClass="form_input6"></asp:TextBox>
                                                                </td>
                                                                <td align="right" valign="top" width="80" height="25" class="formtext">
                                                                    Hierarchy Level:&nbsp;
                                                                </td>
                                                                <td align="left" valign="top" width="220">
                                                                    <asp:DropDownList ID="ddlHierarchLevelSearch" runat="server" CssClass="form_select6">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="left" valign="top" width="70" class="formtext">
                                                                    <%-- Sales Channel:&nbsp;--%>
                                                                </td>
                                                                <td align="left" valign="top" width="120">
                                                                    <%-- <asp:DropDownList ID="ddlSalesChanelSearch" runat="server" CssClass="form_select2">
                                                                       
                                                                    </asp:DropDownList>--%>
                                                                </td>
                                                                <td align="left" valign="top" width="60">
                                                                    <asp:Button ID="btnSearchUser" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg">
                                                                    </asp:Button>
                                                                </td>
                                                                <td align="left" valign="top" width="100">
                                                                    <asp:Button ID="btnShow" runat="server" CssClass="buttonbg" Text="Show All" ToolTip="Search" />
                                                                </td>
                                                            </tr>
                                                            <div>
                                                                <table>
                                                                    <tr>
                                                                        <td width="30%" align="left" valign="top">
                                                                            <asp:Label ID="lblUploadData" runat="server" Text="Click buttons to upload the Data"
                                                                                CssClass="formtext"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="30%" align="left" valign="top">
                                                                            <asp:FileUpload ID="Fileupdload" CssClass="form_file" runat="server" Visible="false" />
                                                                        </td>
                                                                        <td class="formtext" valign="top" align="left" width="30%">
                                                                            <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload MOD" OnClick="btnUpload_Click" />
                                                                        </td>
                                                                        <td class="formtext" valign="top" align="left" width="30%">
                                                                            <asp:Button ID="btnUploadBTM" CssClass="buttonbg" runat="server" Text="Upload BTM"
                                                                                OnClick="btnUploadBTM_Click" />
                                                                        </td>
                                                                        <td class="formtext" valign="top" align="left" width="30%">
                                                                            <asp:Button ID="btnUploadGrn" CssClass="buttonbg" runat="server" Text="Upload GRN"
                                                                                OnClick="btnUploadGrn_Click" />
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnFTP1" runat="server" Text="FTP" CssClass="buttonbg" OnClick="btnFTP_Click" />
                                                                            <asp:Button ID="BeetelFTP" runat="server" OnClick="btnBeetelFTP_Click" Text="BeetelFTP" />
                                                                            <asp:Button ID="BeetelOnida" runat="server" Text="BeetelOnida" OnClick="BeetelOnida_Click" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnUploadServer" runat="server" Text="UploadFileToServer" CssClass="buttonbg"
                                                                                OnClick="btnUploadServer_Click" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </table>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnupload" />
                                                        <asp:PostBackTrigger ControlID="BeetelOnida" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" height="10">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="90%" align="left" class="tableposition">
                                            <div class="mainheading">
                                                User List</div>
                                        </td>
                                        <td width="10%" align="right">
                                            <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" class="tableposition">
                                <div class="contentbox">
                                    <div class="grid1">
                                        <%--<asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                                            <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>--%>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" height="10">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <%--     <uc3:footer ID="Footer1" runat="server" />--%>
    </div>
</asp:Content>
