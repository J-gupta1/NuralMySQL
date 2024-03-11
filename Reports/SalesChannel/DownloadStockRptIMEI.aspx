<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="DownloadStockRptIMEI.aspx.cs" Inherits="Reports_DownloadStockRptIMEI" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<%--05 June 2018,Rajnish Kumar,#CC01,SalesChannelId and CityId Filter.--%>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0">
        <tr>
            <td align="left" valign="top" height="420">
                <table cellspacing="0" cellpadding="0" width="965" border="0">
                    <tr>
                        <td valign="top" align="left">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td align="left" valign="top">
                                        <uc1:ucMessage ID="ucMsg" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td align="left" class="tableposition">
                                                    <div class="mainheading">
                                                        Search Stock Report</div>
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
                                                                <td valign="top" align="right" width="17%" height="35">
                                                                    Sales Channel Type: <font class="error">*</font>
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form_select4" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                    <div style="width: 180px;">
                                                                        <asp:RequiredFieldValidator ID="reqSales" runat="server" ControlToValidate="ddlType"
                                                                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select sales channel type."
                                                                            SetFocusOnError="true" ValidationGroup="vgStockRpt"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </td>
                                                                <td valign="top" align="right" width="12%">
                                                                    <asp:Label ID="Label2" runat="server" Text="">Sales Channel: <font class="error">*</font></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top" width="20%">
                                                                    <asp:DropDownList ID="DdlSaleschannel" CssClass="form_select4" runat="server">
                                                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td valign="top" align="right" width="15%">
                                                                    Closing as on date: <font class="error">*</font>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="To date required." ValidationGroup="vgStockRpt"
                                                                        defaultDateRange="True" RangeErrorMessage="Date should be less or equal to current date." />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="right" height="35">
                                                                    <asp:Label ID="lbllocation" runat="server" Text="">Region:  <font class="error">&nbsp;</font></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:DropDownList ID="ddllocation" runat="server" CssClass="form_select4" AutoPostBack="false">
                                                                    </asp:DropDownList>
                                                                </td>                                                            
                                                                <td valign="top" align="right">
                                                                    <asp:Label ID="lblState" runat="server" Text="">State:  <font class="error">&nbsp;</font></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <div style="width: 135px;">
                                                                        <asp:DropDownList ID="ddlState" runat="server" CssClass="form_select4" AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </td>
                                                                <%--  #CC01--%>                                                           
                                                        
                                                     <td valign="top" align="right">
                                                        <asp:Label ID="Label3" runat="server" Text="">City: <font class="error">*</font></asp:Label>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:DropDownList ID="ddlCity" CssClass="form_select4" runat="server">
                                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>                                               
                                                             <%--  #CC01--%>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="right" height="35">
                                                                    <asp:Label ID="lblProductCategory" runat="server" Text="">Product Category:  <font class="error">&nbsp;</font></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <div style="width: 135px;">
                                                                        <asp:DropDownList ID="ddlProductCategory" runat="server" CssClass="form_select4"
                                                                            AutoPostBack="false">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </td>
                                                                <td valign="top" align="right">
                                                                    <asp:Label ID="lblsku" runat="server" Text="">Model Name: <span class="error">&nbsp;</span></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <div style="width: 135px;">
                                                                        <asp:DropDownList ID="ddlModelName" CssClass="form_select4" runat="server" AutoPostBack="True"
                                                                            OnSelectedIndexChanged="ddlModelName_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                   <%-- <div style="width: 180px;">
                                                                        <asp:RequiredFieldValidator runat="server" ID="valprodcat" ControlToValidate="ddlModelName"
                                                                            CssClass="error" ValidationGroup="SalesReport1" ErrorMessage="Please select a Model Name "
                                                                            InitialValue="0" /></div>--%>
                                                                </td>                                                           
                                                                <td valign="top" align="right">
                                                                    <asp:Label ID="lblSkuName" runat="server" Text="">Sku Name: <span class="error">&nbsp;</span></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <div style="width: 135px;">
                                                                        <asp:DropDownList ID="ddlSku" runat="server" CssClass="form_select4" AutoPostBack="false">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <%--<div style="width: 140px;">
                                                                        <asp:RequiredFieldValidator runat="server" ID="valModel" ControlToValidate="ddlSku"
                                                                            CssClass="error" ValidationGroup="SalesReport1" ErrorMessage="Please select a SKU "
                                                                            InitialValue="0" /></div>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td valign="top" align="left">
                                                                    <div class="float-left">
                                                                        <asp:CheckBox ID="chkZeroQuantity" runat="server" />
                                                                    </div>
                                                                    <div class="float-left" style="margin-top: 3px;">
                                                                        <asp:Label ID="lblZeroQuantity" runat="server" Text="">Show Zero Qty Records</asp:Label></div>
                                                                </td>
                                                                
                                                                <td valign="top" align="right">
                                                                    <asp:Button ID="btnSearch" Text="Download" runat="server" ValidationGroup="vgStockRpt"
                                                                        ToolTip="Search" CssClass="buttonbg" CausesValidation="true" OnClick="btnSearch_Click" />
                                                                </td>
                                                                 <td valign="top" align="left">
                                                                  <asp:Button ID="ExportToCSV" ToolTip="Export to CSV" ValidationGroup="vgStockRpt" CssClass="buttonbg" runat="server" 
                                                                      Text="ExportToCSV" OnClick="ExportToCSV_Click"  Visible="false"/>
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
                        <td height="10">
                        </td>
                    </tr>
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
