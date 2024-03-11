<%@ Page Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="OrganHierarchyMappingReport.aspx.cs" Inherits="Reports_SalesChannel_OrganHierarchyMappingReport" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>


<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0">
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
                                                    <uc1:ucMessage ID="ucMessage1" runat="server" ShowCloseButton="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="display: block;">

                                    <td align="left" valign="top">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" style="float: left">
                                            <tr>
                                                <td align="left" valign="top" class="tableposition">
                                                    <div class="mainheading_rpt">
                                                        <div class="mainheading_rpt_left">
                                                        </div>
                                                        <div class="mainheading_rpt_mid">
                                                            Organisation Hierarchy Mapping Report
                                                        </div>
                                                        <div class="mainheading_rpt_right">
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="display: block;">

                                    <td align="left" valign="top">
                                        <asp:UpdatePanel ID="updExport" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td align="left" valign="top"></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="left" class="tableposition">
                                                            <div class="contentbox">
                                                                <table cellspacing="0" cellpadding="4" width="100%" border="0">

                                                                    <tr>
                                                                        <td class="formtext" valign="top" align="right" width="12%">
                                                                            <asp:Label ID="lblHierarchyLevel" runat="server" Text="">Hierarchy Level:</asp:Label>
                                                                        </td>
                                                                        <td width="20%" align="left" valign="top">
                                                                            <div style="float: left; width: 135px;">
                                                                                <asp:DropDownList ID="ddlHierarchyLevel" CssClass="form_select" runat="server" AutoPostBack="True"
                                                                                    OnSelectedIndexChanged="ddlHierarchyLevel_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                                <br />
                                                                            </div>

                                                                        </td>
                                                                        <td class="formtext" valign="top" align="right" width="10%">
                                                                            <asp:Label ID="lblLocation" runat="server" Text="">Location:</asp:Label>
                                                                        </td>
                                                                        <td valign="top" align="left" width="20%">
                                                                            <div style="float: left; width: 135px;">
                                                                                <asp:DropDownList ID="ddllocation" runat="server" CssClass="form_select">
                                                                                </asp:DropDownList>

                                                                            </div>
                                                                        </td>
                                                                        <td valign="top" align="left" width="38%">

                                                                            <asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" />

                                                                        </td>
                                                                    </tr>

                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="ExportToExcel" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" height="10" valign="top"></td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
</asp:Content>
