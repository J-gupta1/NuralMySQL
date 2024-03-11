<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageOrgnHierarchy.aspx.cs" Inherits="Masters_HO_Admin_ManageOrgnHierarchy" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div align="center">
        <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
            <tr>
                <td align="left" valign="top" height="420">
                    <table cellspacing="0" cellpadding="0" width="965" border="0">
                        <tr>
                            <td valign="top" align="left">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Conditional">
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
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td align="left" valign="top" class="tableposition">
                                                                            <div class="mainheading">
                                                                                Add / Edit Location
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
                                                                            <td colspan="6" height="20" class="mandatory" valign="top">(<font class="error">*</font>) marked fields are mandatory.
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="formtext" valign="top" align="right" width="12%" height="35">
                                                                                <asp:Label ID="lblddlHierarchyLevel" CssClass="formtext" runat="server" AssociatedControlID="ddlHierarchyLevel">Hierarchy Level:<font class="error">*</font></asp:Label>
                                                                            </td>
                                                                            <td width="20%" align="left" valign="top">
                                                                                <div style="float: left; width: 135px;">
                                                                                    <asp:DropDownList CausesValidation="true" ID="ddlHierarchyLevel" runat="server" CssClass="form_select4"
                                                                                       AutoPostBack="True" OnSelectedIndexChanged="ddlHierarchyLevel_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <br />
                                                                                </div>
                                                                                <div style="float: left; width: 160px;">
                                                                                    <asp:Label Style="display: none;" runat="server" ID="lblHierarchyLevel" CssClass="error"></asp:Label><asp:RequiredFieldValidator
                                                                                        ID="ReqUserGroup" runat="server" ControlToValidate="ddlHierarchyLevel" CssClass="error"
                                                                                        Display="Dynamic" InitialValue="0" ErrorMessage="Please select hierarchy level."
                                                                                        SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                                                                </div>
                                                                            </td>
                                                                            <td class="formtext" valign="top" align="right" width="15%">
                                                                                <asp:Label ID="lblParentHierarchy" runat="server" AssociatedControlID="ddlParentHierarchy"
                                                                                    CssClass="formtext">Parent Hierarchy Name:</asp:Label>
                                                                            </td>
                                                                            <td width="20%" align="left" valign="top">
                                                                                <asp:DropDownList CausesValidation="true" ID="ddlParentHierarchy" runat="server"
                                                                                Width="150px"     CssClass="form_select4">
                                                                                </asp:DropDownList>
                                                                                <%--<br />
                                                                                <asp:Label Style="display: none;" runat="server" ID="Label1" CssClass="error"></asp:Label><asp:RequiredFieldValidator
                                                                                    ID="reqParentHierarchy" runat="server" ControlToValidate="ddlParentHierarchy"
                                                                                    CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select parent hierarchy."
                                                                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>--%>
                                                                            </td>
                                                                            <td class="formtext" valign="top" align="right" width="13%">
                                                                                <asp:Label ID="lblLocationName" runat="server" AssociatedControlID="txtLocationName"
                                                                                    CssClass="formtext">Location Name:<font class="error">*</font></asp:Label>
                                                                            </td>
                                                                            <td valign="top" align="left" width="20%">
                                                                                <asp:TextBox ID="txtLocationName" runat="server" CssClass="form_input2" MaxLength="50"
                                                                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>
                                                                                <br />
                                                                                <asp:RequiredFieldValidator ID="reqVLocationName" runat="server" ControlToValidate="txtLocationName"
                                                                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter location Name."
                                                                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator><br />
                                                                                <asp:RegularExpressionValidator ID="reqLocationName" ControlToValidate="txtLocationName"
                                                                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,30}"
                                                                                    ValidationGroup="AddUserValidationGroup" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="formtext" valign="top" align="right">
                                                                                <asp:Label ID="lblLocationCode" runat="server" AssociatedControlID="txtLocationCode"
                                                                                    CssClass="formtext">Location Code:<font class="error">*</font></asp:Label>
                                                                            </td>
                                                                            <td valign="top" align="left">
                                                                                <asp:TextBox ID="txtLocationCode" runat="server" CssClass="form_input2" MaxLength="20"
                                                                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>
                                                                                <br />
                                                                                <asp:RequiredFieldValidator ID="reqLocationCode" runat="server" ControlToValidate="txtLocationCode"
                                                                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter location code."
                                                                                    SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator><br />
                                                                                <asp:RegularExpressionValidator ID="regLocationCode" ControlToValidate="txtLocationCode"
                                                                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,30}"
                                                                                    ValidationGroup="AddUserValidationGroup" runat="server" />
                                                                            </td>
                                                                            <td class="formtext" valign="top" align="right">
                                                                                <asp:Label ID="lblchkActive" runat="server" AssociatedControlID="chkActive" CssClass="formtext"> Status :</asp:Label>
                                                                            </td>
                                                                            <td align="left" valign="top">
                                                                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                                                            </td>
                                                                            <td align="left" valign="top"></td>
                                                                            <td align="left">
                                                                                <asp:Button ID="btnCreate" Text="Submit" runat="server" CausesValidation="true" ValidationGroup="AddUserValidationGroup"
                                                                                    ToolTip="Add Location" CssClass="buttonbg" OnClick="btnCreate_Click" />
                                                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                                                                    OnClick="btnCancel_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <%--#CC01 Add Start --%>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnCreate" />
                                                </Triggers>
                                                <%--#CC01 Add End --%>
                                                <%-- <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                </Triggers>--%>
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
                            <td align="left" valign="top">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td align="left" class="tableposition">
                                            <div class="mainheading">
                                                Search
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" class="tableposition">
                                            <div class="contentbox">
                                                <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                            <tr>
                                                                <td align="right" valign="top" width="12%" height="35" class="formtext">Hierarchy Level:
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <div style="float: left; width: 135px;">
                                                                        <asp:DropDownList CausesValidation="true" ID="ddlSerHierarchyLevel" runat="server"
                                                                            CssClass="form_select4" AutoPostBack="false">
                                                                        </asp:DropDownList>
                                                                        <br />
                                                                    </div>
                                                                </td>
                                                                <td align="right" valign="top" width="12%" height="35" class="formtext">Location Name:
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <asp:TextBox ID="txtLocationNameSearch" runat="server" MaxLength="100" CssClass="form_input6">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td align="right" valign="top" width="15%" height="25" class="formtext">Location Code:
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <asp:TextBox ID="txtLocationCodeSearch" runat="server" CssClass="form_input6" MaxLength="100">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td align="right" valign="top" width="13%" height="35" class="formtext"></td>
                                                                <td align="left" valign="top" width="20%"></td>
                                                            </tr>

                                                            <tr>
                                                                <%-- #CC03 Comment Start <td align="right" valign="top" width="12%" height="35" class="formtext">Parent Hierarchy Level:
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                     <div style="float: left; width: 135px;">
                                                                        <asp:DropDownList CausesValidation="true" ID="ddlSerParentHierarchyLevel" runat="server"
                                                                            CssClass="form_select4" AutoPostBack="false">
                                                                        </asp:DropDownList>  
                                                                    <br />
                                                                    </div>
                                                                </td>#CC03 Comment End --%>
                                                                <%--#CC03 Add Start--%>
                                                                <td align="right" valign="top" width="12%" height="35" class="formtext">Parent Code:
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <div style="float: left; width: 135px;">
                                                                        <asp:TextBox ID="txtParentCode" CssClass="form_input6" runat="server" MaxLength="20"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <%--#CC03 Add End--%>
                                                                <td align="right" valign="top" width="12%" height="35" class="formtext">Parent Location Name:
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <asp:TextBox ID="txtSerParentLocationName" runat="server" MaxLength="100" CssClass="form_input6">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <%--#CC03 Comment Start
                                                                <td align="right" valign="top" width="15%" height="25" class="formtext">--%>
                                                                <%--Hierarchy Level--%>
                                                                <%--   <asp:Button ID="Button1" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                                                        OnClick="btnSearch_Click"></asp:Button>
                                                                </td>
                                                               <td align="left" valign="top" width="20%">
                                                                    <asp:Button ID="btnShow" runat="server" CssClass="buttonbg" OnClick="btnShow_Click"
                                                                        Text="Show All" ToolTip="Search" Width="60px" />
                                                                </td>
                                                                #CC03 Comment End--%>
                                                                <%--#CC03 Add Start--%>
                                                                <td align="right" valign="top" width="13%" height="35" class="formtext">User Name:
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <div style="float: left; width: 135px;">
                                                                        <asp:TextBox ID="txtUserName" CssClass="form_input6" runat="server" MaxLength="100"></asp:TextBox>
                                                                    </div>

                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4"></td>
                                                                <td align="right" valign="top" width="15%" height="25" class="formtext">
                                                                    <%--Hierarchy Level--%>
                                                                    <asp:Button ID="Button1" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                                                        OnClick="btnSearch_Click"></asp:Button>
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <asp:Button ID="btnShow" runat="server" CssClass="buttonbg" OnClick="btnShow_Click"
                                                                        Text="Show All" ToolTip="Search" Width="60px" />
                                                                </td>
                                                                <%--#CC03 Add End--%>

                                                                <td align="right" valign="top" width="13%" height="35" class="formtext"></td>
                                                                <td align="left" valign="top" width="20%"></td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" height="10"></td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="90%" align="left" class="tableposition">
                                            <div class="mainheading">
                                                &nbsp;Location List
                                            </div>
                                        </td>
                                        <td width="10%" align="right">
                                            <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" CausesValidation="False"
                                                OnClick="btnExprtToExcel_Click" />
                                        </td>
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
                                                <%--#CC03 Add Start--%>

                                                <%--#CC03 Add End--%>
                                                <asp:GridView ID="grdvLocationList" runat="server" FooterStyle-VerticalAlign="Top"
                                                    FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                                                    HeaderStyle-HorizontalAlign="left" EmptyDataText="No Record found" HeaderStyle-VerticalAlign="top"
                                                    GridLines="none" AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow"
                                                    FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader" CellSpacing="1"
                                                    PageSize='<%$ AppSettings:GridViewPageSize %>' CellPadding="4" bgcolor="" BorderWidth="0px"
                                                    Width="100%" AutoGenerateColumns="false" AllowPaging="True" SelectedStyle-CssClass="gridselected"
                                                    DataKeyNames="OrgnhierarchyID"
                                                    OnRowDataBound="grdvLocationList_RowDataBound">
                                                    <%--#CC02  OnPageIndexChanging removed--%>
                                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                                    <Columns>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left"
                                                            DataField="HierarchyLevelName" HeaderText="Hierarchy Level"></asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="LocationName"
                                                            HeaderText="Location Name"></asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="LocationCode"
                                                            HeaderText="Location Code"></asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="LocationUsername"
                                                            HeaderText="User Name"></asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left"
                                                            DataField="ParentLocationName" HeaderText="Parent Location"></asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left"
                                                            DataField="ParentLocationUsername" HeaderText="Parent User Name"></asp:BoundField>

                                                        <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:ImageButton CommandArgument='<%#Eval("OrgnhierarchyID") %>' runat="server" ID="btnEdit"
                                                                    ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' OnClick="btnEdit_Click"
                                                                    ToolTip="Edit" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnActiveDeactive"
                                                                    runat="server" CommandArgument='<%#Eval("OrgnhierarchyID") %>' CommandName='<%#Eval("Status")%>'
                                                                    OnClick="btnActiveDeactive_Click" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                                                    ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerStyle CssClass="PagerStyle" />
                                                </asp:GridView>
                                                <%--#CC03 Add Start--%>
                                                <div class="clear">
                                                </div>

                                                <div id="dvFooter" runat="server" class="pagination">
                                                    <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                                                </div>

                                                <%--#CC03 Add End--%>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="grdvLocationList" EventName="DataBound" />
                                                <%--#CC03 Add Start --%>
                                                <asp:PostBackTrigger ControlID="btnShow" />
                                                <asp:PostBackTrigger ControlID="Button1" />
                                                <%--#CC03 Add End --%>
                                                <%--<asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />--%>
                                            </Triggers>
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
    </div>
</asp:Content>
