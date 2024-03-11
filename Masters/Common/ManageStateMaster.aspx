<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ManageStateMaster.aspx.cs" Inherits="Masters_HO_Common_ManageStateMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
        <tr>
            <td align="left" valign="top">
                <table cellspacing="0" cellpadding="0" width="965" border="0">
                    <tr>
                        <td valign="top" align="left">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td align="left" valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <uc1:ucMessage ID="ucMessage1" runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" valign="top">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td align="left" valign="top" class="tableposition">
                                                                <div class="mainheading">
                                                                    Add / Edit State</div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" class="tableposition">
                                                    <div class="contentbox">
                                                        <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="conditional">
                                                            <ContentTemplate>
                                                                <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                                    <tr>
                                                                        <td colspan="6" height="20" class="mandatory" valign="top">
                                                                            (<font class="error">*</font>) marked fields are mandatory.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formtext" valign="top" align="right" width="13%" height="35">
                                                                            <asp:Label ID="lblstatename" runat="server" Text="">State Name:<font class="error">*</font></asp:Label>
                                                                        </td>
                                                                        <td width="22%" align="left" valign="top">
                                                                            <asp:TextBox ID="txtInsertName" runat="server" CssClass="form_input2" MaxLength="70"></asp:TextBox><br />
                                                                            <asp:RequiredFieldValidator runat="server" ID="valname" ControlToValidate="txtInsertName"
                                                                                CssClass="error" ErrorMessage="Please insert  Name " ValidationGroup="insert" /><br />
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtInsertName"
                                                                                InvalidChars="<>!@#$%^&*(){}" FilterType="Custom" FilterMode="InvalidChars">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td class="formtext" valign="top" align="right" width="13%">
                                                                            <asp:Label ID="lblcstatecode" runat="server" Text="">State Code:<font class="error">*</font></asp:Label>
                                                                        </td>
                                                                        <td width="20%" align="left" valign="top">
                                                                            <asp:TextBox ID="txtInsertCode" runat="server" CssClass="form_input2" MaxLength="20"></asp:TextBox><br />
                                                                            <asp:RequiredFieldValidator runat="server" ID="valcode" ControlToValidate="txtInsertCode"
                                                                                CssClass="error" ErrorMessage="Please insert  code " ValidationGroup="insert" /><br />
                                                                            <cc1:FilteredTextBoxExtender ID="txtnameValid" runat="server" TargetControlID="txtInsertCode"
                                                                                InvalidChars="<>!@#$%^&*(){}" FilterType="Custom" FilterMode="InvalidChars">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td class="formtext" valign="top" align="right" width="12%">
                                                                            <asp:Label ID="Label1" Text="" runat="server" />Country:<font class="error">*</font>
                                                                        </td>
                                                                        <td valign="top" align="left" width="20%">
                                                                            <div style="float: left; width: 135px;">
                                                                                <asp:DropDownList ID="cmbInsCountry" runat="server" CssClass="form_select" AutoPostBack="True">
                                                                                </asp:DropDownList>
                                                                                <br />
                                                                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbInsCountry"
                                                                                    CssClass="error" ErrorMessage="Please select a Country " InitialValue="0" ValidationGroup="insert" /></div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="formtext" valign="top" align="right">
                                                                            <asp:Label ID="lblprlist" Text="" runat="server" />Price List Name:<font class="error">*</font>
                                                                        </td>
                                                                        <td valign="top" align="left">
                                                                            <div style="float: left; width: 135px;">
                                                                                <asp:DropDownList ID="cmbInsertPriceList" runat="server" CssClass="form_select" AutoPostBack="True">
                                                                                </asp:DropDownList>
                                                                                <br />
                                                                                <asp:RequiredFieldValidator runat="server" ID="valpricename" ControlToValidate="cmbInsertPriceList"
                                                                                    CssClass="error" ErrorMessage="Please select a Price Name " InitialValue="0"
                                                                                    ValidationGroup="insert" /></div>
                                                                        </td>
                                                                        <td class="formtext" valign="top" align="right">
                                                                            <asp:Label ID="lblpreffdt" runat="server" Text="">Price Effective Date:<font class="error">*</font></asp:Label>
                                                                        </td>
                                                                        <td valign="top" align="left">
                                                                            <uc2:ucDatePicker ID="ucDatePicker" runat="server" RangeErrorMessage="Invalid date."
                                                                                ErrorMessage="Please select a date" ValidationGroup="insert" />
                                                                        </td>
                                                                        <td class="formtext" valign="top" align="right">
                                                                            Status:
                                                                        </td>
                                                                        <td class="formtext" valign="top" align="left">
                                                                            <asp:CheckBox ID="chkstatus" runat="server" Checked="true" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                        </td>
                                                                        <td class="formtext" valign="top" align="left" colspan="5">
                                                                            <asp:Button ID="btnsubmit" runat="server" OnClick="btninsert_click" Text="Submit"
                                                                                ValidationGroup="insert" CssClass="buttonbg" />
                                                                            <asp:Button ID="btncancel" runat="server" CausesValidation="False" OnClick="btncancel_Click"
                                                                                Text="Cancel" CssClass="buttonbg" />
                                                                        </td>
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
                                            Search State</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" class="tableposition">
                                        <div class="contentbox">
                                            <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0" cellpadding="4" border="0" width="100%">
                                                        <tr>
                                                            <td align="right" valign="top" width="13%" height="35" class="formtext">
                                                                <asp:Label ID="lblfndstatecode" runat="server" Text="State Code:"></asp:Label>
                                                            </td>
                                                            <td align="left" valign="top" width="22%">
                                                                <asp:TextBox ID="txtSerCode" runat="server" CssClass="form_input2"></asp:TextBox><br />
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtSerCode"
                                                                    CssClass="error" ErrorMessage="Invalid  Searching parameter" ValidationExpression="[^()<>/\@%]{1,50}"
                                                                    ValidationGroup="search" runat="server" />
                                                            </td>
                                                            <td align="right" valign="top" height="25" width="10%" class="formtext">
                                                                <asp:Label ID="lblstatefnname" runat="server" Text="State Name:"></asp:Label>
                                                            </td>
                                                            <td align="left" valign="top" width="20%">
                                                                <asp:TextBox ID="txtSerName" runat="server" CssClass="form_input2"></asp:TextBox><br />
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txtSerName"
                                                                    CssClass="error" ErrorMessage="Invalid Serching Parameter " ValidationExpression="[^()<>/\@%]{1,50}"
                                                                    ValidationGroup="search" runat="server" />
                                                            </td>
                                                            <td align="right" valign="top" width="12%" height="35" class="formtext">
                                                                <asp:Label ID="Label2" runat="server" Text="Country:"></asp:Label>
                                                            </td>
                                                            <td align="left" valign="top" width="23%" class="formtext">
                                                                <div style="float: left; width: 135px;">
                                                                    <asp:DropDownList ID="cmbSerCountry" runat="server" CssClass="form_select">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" valign="top" width="12%" height="35" class="formtext">
                                                                <asp:Label ID="lblserchprice" runat="server" Text="Price List Name:"></asp:Label>
                                                            </td>
                                                            <td align="left" valign="top" width="23%" class="formtext">
                                                                <div style="float: left; width: 135px;">
                                                                    <asp:DropDownList ID="cmbSerPriceList" runat="server" CssClass="form_select">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </td>
                                                            <td align="right">
                                                            </td>
                                                            <td align="left" valign="top" class="formtext" colspan="3">
                                                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click"
                                                                    CssClass="buttonbg" CausesValidation="False" ValidationGroup="search" />
                                                                <asp:Button ID="btnGetallData" runat="server" Text="Show All Data" OnClick="btnGetallData_Click"
                                                                    CssClass="buttonbg" CausesValidation="False" />
                                                            </td>
                                                            <td align="right">
                                                            </td>
                                                            <td align="right">
                                                            </td>
                                                            <td align="right">
                                                            </td>
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
                        <td align="left" height="10">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="60%" align="left" class="tableposition">
                                        <div class="mainheading">
                                            &nbsp;List</div>
                                    </td>
                                    <td width="40%" align="right">
                                        <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" OnClick="btnExprtToExcel_Click"
                                            CausesValidation="False" />
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
                                            <asp:GridView ID="grdState" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="gridrow1"
                                                AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                                                DataKeyNames="StateID" FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left"
                                                FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-CssClass="gridheader"
                                                HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" OnRowCommand="grdState_RowCommand"
                                                RowStyle-CssClass="gridrow" EmptyDataText="No record found" RowStyle-HorizontalAlign="left"
                                                RowStyle-VerticalAlign="top" Width="100%" 
                                                OnPageIndexChanging="grdState_PageIndexChanging" >
                                                <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                                                <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                <Columns>
                                                    <asp:BoundField DataField="StateCode" HeaderStyle-HorizontalAlign="Left" HeaderText="State Code"
                                                        HtmlEncode="true">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="StateName" HeaderStyle-HorizontalAlign="Left" HeaderText="State Name"
                                                        HtmlEncode="true">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CountryName" HeaderStyle-HorizontalAlign="Left" HeaderText="Country"
                                                        HtmlEncode="true">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PriceListName" HeaderStyle-HorizontalAlign="Left" HeaderText="Price List Name"
                                                        HtmlEncode="true">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PriceListEffectiveDate" HeaderStyle-HorizontalAlign="Left"
                                                        HeaderText="Price Effective Date" HtmlEncode="true" Visible="false">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Price Effective Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPriceListEffectiveFrom" runat="server" Text='<%# Eval("PriceListEffectiveDate","{0:dd-MMM-yy}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                                        <ItemStyle Wrap="False" />
                                                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("StateID") %>'
                                                                CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                                                ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                                        <ItemStyle Wrap="False" />
                                                        <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="img1" runat="server" CausesValidation="false" CommandArgument='<%#Eval("StateID") %>'
                                                                CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                          
                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                <PagerStyle CssClass="PagerStyle" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                        <%--<Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="img1" EventName="cmdEdit" />
                                                 <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />
                                      </Triggers>     --%>
                                    </asp:UpdatePanel>
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
</asp:Content>
