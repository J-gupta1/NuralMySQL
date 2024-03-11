<%@ Page Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="IMEIAccXchangeUpdateReport.aspx.cs" Inherits="Reports_SalesChannel_IMEIAccXchangeUpdateReport" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>

<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">


    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td align="left" valign="top">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always">
                    <ContentTemplate>
                        <uc1:ucMessage ID="ucMessage1" runat="server" ShowCloseButton="false" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportToExcel" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="float: left">
        <tr>
            <td align="left" valign="top" class="tableposition">
                <div class="mainheading_rpt">
                    <div class="mainheading_rpt_left">
                    </div>
                    <div class="mainheading_rpt_mid">
                        IMEI Update through AccXchange
                    </div>
                    <div class="mainheading_rpt_right">
                    </div>
                </div>
            </td>
        </tr>
    </table>

    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td align="left" valign="top"></td>
        </tr>
        <tr>
            <td valign="top" align="left" class="tableposition">
                <div class="contentbox">
                    <asp:UpdatePanel runat="server" ID="upd" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table cellspacing="0" cellpadding="4" width="100%" border="0">

                                <tr style="display: none;">

                                    <td class="mandatory" valign="top">(<font class="error">*</font>) marked fields are mandatory.
                                    </td>
                                </tr>
                                <tr>

                                    <td align="right" valign="top" class="formtext" style="width: 10%">
                                        <asp:Label ID="lblfrmDate" runat="server" Text="From Date: "></asp:Label><font class="error">*</font>
                                    </td>
                                    <td align="left" valign="top" style="width: 10%">
                                        <uc2:ucDatePicker ID="ucFromDate" runat="server" ErrorMessage="Invalid from date."
                                            defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                                    </td>
                                    <td align="right" valign="top" class="formtext" style="width: 10%">
                                        <asp:Label ID="lblsertodate" runat="server" Text="To Date: "></asp:Label><font class="error">*</font>
                                    </td>
                                    <td valign="top" align="left" style="width: 10%">
                                        <uc2:ucDatePicker ID="ucToDate" runat="server" ErrorMessage="Invalid to date."
                                            defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                                    </td>
                                    <td class="formtext" valign="top" align="right" width="15%">
                                        <asp:Label ID="Label1" runat="server" Text="National Distributor"></asp:Label>:
                                    </td>
                                    <td width="15%" align="left" valign="top">
                                        <asp:DropDownList ID="ddlND" OnSelectedIndexChanged="ddlND_SelectedIndexChanged"
                                            runat="server" CssClass="form_select2" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>

                                    <td class="formtext" valign="top" align="right" width="10%">
                                        <asp:Label ID="lblRDS" runat="server" Text="RDS"></asp:Label>:
                                    </td>
                                    <td valign="top" align="left" width="20%">
                                        <asp:DropDownList ID="ddlRDS" runat="server" CssClass="form_select"></asp:DropDownList>
                                    </td>



                                </tr>
                                <tr>
                                    <td colspan="7"></td>
                                    <td width="20%" align="right">
                                        <div>
                                            <div class="float-left">
                                                <asp:Image ID="imgDownloadMappingInfo" runat="server" AlternateText="Download Retailer Mapping Info" />
                                            </div>
                                            <div class="float-margin" style="padding-top: 3px; padding-left: 3px">
                                                <asp:LinkButton ID="lnkExportToExcel" runat="server" Text="Export to Excel"
                                                    CssClass="elink2" Style="color: green" OnClick="lnkExportToExcel_Click"></asp:LinkButton>

                                            </div>
                                        </div>
                                    </td>


                                </tr>
                            </table>
                        </ContentTemplate>


                    </asp:UpdatePanel>
                    <div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
