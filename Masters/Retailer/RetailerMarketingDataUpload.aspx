<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="RetailerMarketingDataUpload.aspx.cs" Inherits="Masters_Retailer_RetailerMarketingDataUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0" align="center">
        <tr>
            <td valign="top" align="left">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td align="left" valign="top">

                            <uc1:ucMessage ID="ucmassege1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td align="left" valign="top" class="tableposition">
                                        <div class="mainheading">
                                            Upload Retailer Marketing Data
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <div class="contentbox">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="4" align="left" valign="top" class="mandatory" height="20">(*) Marked fields are mandatory
                                        </td>
                                    </tr>

                                    <tr>
                                        <td height="35" align="right" width="10%" class="formtext" valign="top">Upload File:<font class="error">*</font>
                                        </td>
                                        <td width="30%" align="left" class="formtext" valign="top">
                                            <asp:FileUpload ID="FileUploadRetailerMarketingData" CssClass="form_file" runat="server" />
                                        </td>
                                        <td align="left" width="10%" class="formtext" valign="top">
                                            <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                                                OnClick="btnUpload_Click" />
                                        </td>
                                        <td align="left" valign="top" height="35">
                                            <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                                                CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>&nbsp;
                                            &nbsp;&nbsp; &nbsp;
                                        </td>
                                        <td align="left" valign="top">


                                            <a class="elink2" href="../../Excel/Templates/RetailerMarketingDataTemplate.xlsx">Download Template </a>

                                            &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;
                                             <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3"></asp:HyperLink>

                                        </td>
                        </td>
                    </tr>
                </table>
                </div>
            </td>
        </tr>
        <tr>
            <td height="10"></td>
        </tr>
        <div runat="server" id="dvhide" visible="true" style="float: left">
            <tr>
                <td align="left" valign="top">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td align="left" valign="top" class="tableposition">
                                <div class="mainheading">
                                    Details
                                </div>
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
                                                Search Retailer Marketing Data
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
                                            <td valign="top" align="right" width="17%" height="35">Retailer Zone:
                                            </td>
                                            <td width="19%" align="left" valign="top">
                                                <div style="width: 160px;">
                                                    <asp:DropDownList CausesValidation="true" ID="ddlRetailerZone" runat="server"
                                                        CssClass="form_select">
                                                    </asp:DropDownList>
                                                </div>
                                                <div style="width: 160px;">
                                                    <asp:RequiredFieldValidator ID="ReqUserGroup" runat="server" ControlToValidate="ddlRetailerZone"
                                                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select Retailer Zone."
                                                        SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                            <td valign="top" align="right" width="15%">Status:
                                            </td>
                                            <td valign="top" align="left" width="19%">
                                                <div style="width: 135px;">
                                                    <asp:DropDownList ID="ddlActiveInactive" runat="server" CausesValidation="true"
                                                        CssClass="form_select">
                                                        <asp:ListItem Text="Select" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>

                                                </div>
                                                <div style="width: 160px;">
                                                    <asp:RequiredFieldValidator ID="ReqUserGroup0" runat="server" ControlToValidate="ddlActiveInactive"
                                                        CssClass="error" Display="Dynamic" ErrorMessage="Please select status."
                                                        InitialValue="2" SetFocusOnError="true" ValidationGroup="AddUserValidationGroup"></asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                            <td align="right" valign="top" width="15%">CSA Type:
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:DropDownList ID="ddlCSAType" runat="server" CausesValidation="true" CssClass="form_select">
                                                    <asp:ListItem Text="Select" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right" width="18%">From date:<font class="error">*</font>
                                            </td>
                                            <td align="left" valign="top" width="20%">
                                                <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="To date required." ValidationGroup="vgStockRpt"
                                                    defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                            </td>
                                            <td valign="top" align="right" width="18%">To date:<font class="error">*</font>
                                            </td>
                                            <td align="left" valign="top" width="20%">
                                                <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="vgStockRpt"
                                                    defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" height="35"></td>
                                            <td align="left" valign="top">
                                                <asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="buttonbg"
                                                    Text="Search" ToolTip="Search" OnClick="btnSearch_Click" />
                                                <asp:Button ID="Button1" runat="server" CssClass="buttonbg" Text="Cancel" ToolTip="Cancel" OnClick="Button1_Click" />
                                            </td>
                                            <td width="10%" align="right">
                                                <asp:Button ID="btnExprtToExcel" Text="" runat="server" CssClass="excel" CausesValidation="False" Visible="true" OnClick="btnExprtToExcel_Click" />
                                            </td>
                                            <td align="left" valign="top" colspan="3">&nbsp;
                                            </td>
                                            <td align="left" valign="top">&nbsp;
                                            </td>
                                        </tr>
                                    </table>
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
                                    <td align="left" height="10"></td>
                                </tr>
    <tr>
        <td align="left" height="10"></td>
    </tr>
    <asp:Panel ID="pnlHide" runat="server" Visible="false">
        <tr>
            <td>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="90%" align="left" class="tableposition">
                            <div class="mainheading">
                                Retailer Marketing Data
                            </div>
                        </td>
                        <td width="10%" align="right"></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" class="tableposition">
                <div class="contentbox">
                    <div class="grid2">
                        <asp:GridView ID="grdRetailerMarketingData" runat="server" FooterStyle-VerticalAlign="Top"
                            FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" EmptyDataText="No Record found"
                            RowStyle-VerticalAlign="top" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                            GridLines="none" AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow"
                            FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader" CellSpacing="1"
                            CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="false"
                            AllowPaging="True" PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected"
                            OnPageIndexChanging="grdRetailerMarketingData_PageIndexChanging" DataKeyNames="RetailerID"
                            OnRowCommand="grdRetailerMarketingData_RowCommand">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <Columns>

                                <asp:BoundField DataField="RetailerName" HeaderStyle-HorizontalAlign="Left"
                                    HeaderText="Retailer Name" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RetailerCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Retailer Code"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SignageAgency" HeaderStyle-HorizontalAlign="Left" HeaderText="Signage Agency"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MediaACPSFT" HeaderStyle-HorizontalAlign="Left" HeaderText="MediaACPSFT"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MEDIAGSB" HeaderStyle-HorizontalAlign="Left" HeaderText="MEDIAGSB"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MediaNORMALGSB" HeaderStyle-HorizontalAlign="Left" HeaderText="MEDIAGSB"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MediaIsbSft" HeaderStyle-HorizontalAlign="Left" HeaderText="MediaIsbSft"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Vendor" HeaderStyle-HorizontalAlign="Left" HeaderText="Vendor"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderStyle-HorizontalAlign="Left" HeaderText="Status"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TotalSales" HeaderStyle-HorizontalAlign="Left" HeaderText="Total Sales"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="EffectiveSales" HeaderStyle-HorizontalAlign="Left" HeaderText="Total Effective Sales"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CSA1" HeaderStyle-HorizontalAlign="Left" HeaderText="CSA1"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                            </Columns>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                        <tr>
                            <td align="right" valign="top" height="5" class="formtext"></td>
                        </tr>

                    </div>
                </div>
            </td>
        </tr>
    </asp:Panel>
    </div>
                    <tr>
                        <td height="10"></td>
                    </tr>
    </table>
            </td>
        </tr>
    </table>
</asp:Content>

