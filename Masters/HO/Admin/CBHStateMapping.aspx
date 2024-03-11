<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="CBHStateMapping.aspx.cs" Inherits="HO_Admin_CBHStateMapping" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
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
                                                        Manage CBH to State Mapping
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" class="tableposition">
                                        <div class="contentbox">
                                            <asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                        <tr>
                                                            <td colspan="4" class="mandatory" valign="top">(<font class="error">*</font>)marked fields are mandatory.
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="formtext" valign="top" align="right" width="5%">
                                                                <asp:Label ID="lblrole" runat="server" Text="">CBH :</asp:Label>
                                                            </td>
                                                            <td width="20%" align="left" valign="top">
                                                                <asp:DropDownList ID="ddlOrgnHierarchy" runat="server" CssClass="form_select7">
                                                                </asp:DropDownList>
                                                                <br />
                                                                <asp:RequiredFieldValidator ID="reqOrgnHierarchySave" runat="server" ControlToValidate="ddlOrgnHierarchy"
                                                                    InitialValue="0" CssClass="error" Display="Dynamic" ErrorMessage="Please select CBH."
                                                                    ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                <asp:RequiredFieldValidator ID="rqOrgnHierarchySearch" runat="server" ControlToValidate="ddlOrgnHierarchy"
                                                                    InitialValue="0" CssClass="error" Display="Dynamic" ErrorMessage="Please select CBH."
                                                                    ValidationGroup="Search"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="formtext" valign="top" align="left">
                                                                <div class="float-margin">
                                                                    <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click"
                                                                        ValidationGroup="Search" CssClass="buttonbg" CausesValidation="True" />
                                                                </div>
                                                                <div class="float-margin">
                                                                    <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                                                        CssClass="buttonbg" CausesValidation="False" />
                                                                </div>
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
                        <td align="left" height="10"></td>
                    </tr>

                    <tr>
                        <td align="left" valign="top" class="tableposition">
                            <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Always">
                                <ContentTemplate>
                                    <div id="dvhide" runat="server" visible="false" style="float: left">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="80%" align="left" class="tableposition" valign="top">
                                                    <div class="mainheading">
                                                        &nbsp;List
                                                    </div>
                                                </td>
                                                <td align="right" valign="top" width="10%">
                                                    <div class="float-margin">
                                                        <asp:DropDownList ID="ddlExportoption" runat="server" CssClass="form_select">
                                                            <asp:ListItem Selected="True" Text="CBH Wise" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </td>
                                                <td align="right" valign="top">
                                                    <div class="float-margin">
                                                        <asp:Button ID="btnExportToExcel" Text="" runat="server" OnClick="btnExportToExcel_Click"
                                                            CssClass="excel" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="contentbox">
                                            <div class="grid2">

                                                <asp:GridView ID="gvCBHToState" runat="server" AlternatingItemStyle-CssClass="gridrow1" GridLines="none"
                                                    AutoGenerateColumns="False" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" HeaderStyle-CssClass="gridheader"
                                                    HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="Middle" RowStyle-CssClass="gridrow"
                                                    meta:resourcekey="gvPartResource1" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                                                    ShowFooter="false" Width="100%" OnRowCommand="gvCBHToState_RowCommand">
                                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="State" HeaderStyle-Width="250px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStateName" runat="server" Text='<%# Eval("StateName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <div style="padding-bottom: 5px;">
                                                                    State Name
                                                                </div>
                                                                <div>
                                                                    <asp:DropDownList ID="ddlState" runat="server" Width="200px" CssClass="form_select">
                                                                    </asp:DropDownList>
                                                                    <br />
                                                                    <asp:RequiredFieldValidator ID="rqddlState" runat="server" ControlToValidate="ddlState"
                                                                        CssClass="error" Display="Dynamic" ErrorMessage="Please select State." ValidationGroup="Save"
                                                                        InitialValue="0"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </HeaderTemplate>
                                                            <HeaderStyle CssClass="gridheader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgActive" runat="server" CausesValidation="false" CommandArgument='<%#Eval("OrgnHierarchyStateMappingID") %>'
                                                                    CommandName="Active" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                                                    ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />
                                                                <asp:HiddenField ID="hdnStatus" runat="server" Value='<%# Eval("Status") %>' />
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <div style="padding-bottom: 5px;">
                                                                    Status
                                                                </div>
                                                                <div>
                                                                    <asp:Button ID="btnAddPart" runat="server" CommandName="AddCBHtoState" CssClass="buttonbg"
                                                                        Text="Add" ValidationGroup="Save" />
                                                                </div>
                                                            </HeaderTemplate>
                                                            <HeaderStyle CssClass="gridheader" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <AlternatingRowStyle CssClass="gridrow1" />
                                                    <PagerStyle CssClass="PagerStyle" />
                                                </asp:GridView>


                                            </div>
                                            <div class="clear">
                                            </div>
                                            <div id="dvFooter" runat="server" class="pagination">
                                                <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                                            </div>

                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExportToExcel" />
                                    <asp:PostBackTrigger ControlID="gvCBHToState" />
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
    </table>

</asp:Content>
