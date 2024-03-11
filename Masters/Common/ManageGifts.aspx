<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
 AutoEventWireup="true" CodeFile="ManageGifts.aspx.cs" Inherits="Masters_Common_ManageGifts" %>
 
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div align="center">
        <%-- <uc2:header ID="Header1" runat="server" />--%>
        <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left">
            <tr>
                <td align="left" valign="top" height="420">
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
                                                                <uc1:ucMessage ID="ucMessage1" runat="server" />
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
                                                                                Add / Edit Gifts</div>
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
                                                                            <td colspan="5" height="20" class="mandatory" valign="top">
                                                                                (<font class="error">*</font>) marked fields are mandatory.
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="formtext" valign="top" align="right" width="10%">
                                                                                <asp:Label ID="lblColornm" runat="server" AssociatedControlID="txtGiftName" CssClass="formtext">Gift Name:<font class="error">*</font></asp:Label>
                                                                            </td>
                                                                            <td valign="top" align="left" width="15%">
                                                                                <asp:TextBox ID="txtGiftName" runat="server" CssClass="form_input2" MaxLength="30"
                                                                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>
                                                                                <br />
                                                                                <asp:RequiredFieldValidator ID="reqGiftName" runat="server" ControlToValidate="txtGiftName"
                                                                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter Gift Name." SetFocusOnError="true"
                                                                                    ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator><br />
                                                                                <asp:RegularExpressionValidator ID="regFUserName" ControlToValidate="txtGiftName"
                                                                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,30}"
                                                                                    ValidationGroup="AddUserValidationGroup" runat="server" />
                                                                            </td>
                                                                             <td class="formtext" valign="top" align="right" width="20%">
                                                                                <asp:Label ID="Label1" runat="server" AssociatedControlID="txtEligiblity" CssClass="formtext">Eligiblity Points:<font class="error">*</font></asp:Label>
                                                                            </td>
                                                                            <td valign="top" align="left" width="20%">
                                                                                <asp:TextBox ID="txtEligiblity" runat="server" CssClass="form_input2" MaxLength="30"
                                                                                    ValidationGroup="AddUserValidationGroup"></asp:TextBox>
                                                                                <br />
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEligiblity"
                                                                                    CssClass="error" Display="Dynamic" ErrorMessage="Please enter eligiblity points." SetFocusOnError="true"
                                                                                    ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator><br />
                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtEligiblity"
                                                                                    CssClass="error" ErrorMessage="Invalid" ValidationExpression="[^()<>/\@%]{1,30}"
                                                                                    ValidationGroup="AddUserValidationGroup" runat="server" />
                                                                            </td>
                                                                            
                                                                            <td class="formtext" valign="top" align="right" width="10%">
                                                                                <asp:Label ID="lblchkActive" runat="server" AssociatedControlID="chkActive" CssClass="formtext"> Status :</asp:Label>
                                                                            </td>
                                                                            <td align="left" valign="top" width="10%">
                                                                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                                                            </td>
                                                                            <td align="left" width="30%">
                                                                                <asp:Button ID="btnCreate" Text="Submit" runat="server" CausesValidation="True" ValidationGroup="AddUserValidationGroup"
                                                                                    ToolTip="Add " CssClass="buttonbg" OnClick="btnCreate_Click" />
                                                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                                                                    OnClick="btncancel_click" CausesValidation="False" />
                                                                            </td>
                                                                        </tr>
                                                                        
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
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
                            <td align="left" height="10">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td align="left" class="tableposition">
                                            <div class="mainheading">
                                                Search Gifts</div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" class="tableposition">
                                            <div class="contentbox">
                                                <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                            <tr>
                                                                <td align="right" valign="top" width="10%" height="35" class="formtext">
                                                                    Gift Name:
                                                                </td>
                                                                <td align="left" valign="top" width="25%">
                                                                    <asp:TextBox ID="txtSerName" runat="server" MaxLength="100" CssClass="form_input2"> </asp:TextBox>
                                                                </td>
                                                                <td align="right" valign="top" width="10%" height="25" class="formtext">
                                                                    <asp:Button ID="btnserch" Text="Search" runat="server" ToolTip="Search" CssClass="buttonbg"
                                                                        OnClick="btnSearch_Click" CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td align="left" valign="top" width="25%" height="25" class="formtext">
                                                                    <asp:Button ID="getalldata" Text="Show All Data" runat="server" ToolTip="Search"
                                                                        CssClass="buttonbg" OnClick="btngetalldta_Click" CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td align="left" valign="top" width="30%">
                                                                    &nbsp;
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
                                <table border="0" cellpadding="0" cellspacing="0" style="height: 26px" width="100%">
                                    <tr>
                                        <td align="left" class="tableposition" width="90%">
                                            <div class="mainheading">
                                                &nbsp;List</div>
                                        </td>
                                        <td align="right" width="10%">
                                            <asp:Button ID="Exporttoexcel" runat="server" CausesValidation="False" CssClass="excel"
                                                OnClick="Exporttoexcel_Click" Text="" ToolTip="Export to Excel" />
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
                                                <asp:GridView ID="grdGift" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                                                    RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" HeaderStyle-HorizontalAlign="left"
                                                    HeaderStyle-VerticalAlign="top" GridLines="none" AlternatingRowStyle-CssClass="gridrow1"
                                                    RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                                                    CellSpacing="1" CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="false"
                                                    AllowPaging="True" SelectedStyle-CssClass="gridselected" DataKeyNames="GiftID"
                                                    PageSize='<%$ AppSettings:GridViewPageSize %>' OnRowCommand="grdColor_RowCommand"
                                                    EmptyDataText="No record found" OnPageIndexChanging="grdColor_PageIndexChanging">
                                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                                    <Columns>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="GiftName"
                                                            HeaderText="Gift Name"></asp:BoundField>
                                                             <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="EligiblityPoint"
                                                            HeaderText="Eligiblity Points"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnActiveDeactive" runat="server" CommandArgument='<%#Eval("GiftId") %>'
                                                                    CommandName="Active" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Active"))) %>'
                                                                    ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Active"))) %>' /></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit" ItemStyle-Width="85px">
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                            <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <asp:ImageButton CommandArgument='<%#Eval("GiftId") %>' runat="server" ID="btnEdit"
                                                                    CommandName="cmdEdit" ImageUrl='<%#"~/" + strAssets + "/CSS/Images/edit.png"%>'
                                                                    ToolTip="Edit User" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerStyle CssClass="PagerStyle" />
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <%-- <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCreateUser" EventName="Click" />
                                            </Triggers>--%>
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
        <%--     <uc3:footer ID="Footer1" runat="server" />--%>
    </div>
</asp:Content>