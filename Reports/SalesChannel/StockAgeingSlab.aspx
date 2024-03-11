<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StockAgeingSlab.aspx.cs" Inherits="Reports_StockAgeingSlab" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
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
                                                        Search User Tracking</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" class="tableposition">
                                        <div class="contentbox">
                                            <%--<asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Always">
                                                <ContentTemplate>--%>
                                                    <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                        <tr>
                                                            <td colspan="8" class="mandatory" valign="top">
                                                                (<font class="error">*</font>)marked fields are mandatory.
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="formtext" valign="top" align="right" width="7%" >
                                                                <asp:Label ID="lblReportFor" runat="server" Text="">Report For:</asp:Label>
                                                            </td>
                                                            <td width="20%" align="left" valign="top">
                                                               <div style="float:left; width:135px;">  <asp:DropDownList ID="cmbRoleType" CssClass="form_select4" runat="server" >
                                                                   <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                                   <asp:ListItem Text="SalesChannel" Value="1"></asp:ListItem>
                                                                   <asp:ListItem Text="Retailer" Value="2"></asp:ListItem>
                                                                 </asp:DropDownList><br /></div>  <div style="float:left; width:135px;">
                                                                 <asp:RequiredFieldValidator runat="server" ID="valpricename" ControlToValidate="cmbRoleType"
                                                                                CssClass="error" ErrorMessage="Please select report for" InitialValue="0"
                                                                                ValidationGroup="insert" /></div>
                                                            </td>
                                                            <td class="formtext" valign="top" align="left" >
                                                                <asp:Button ID="btnExport" Text="Export To Excel" runat="server" OnClick="btnExport_Click"
                                                                    ValidationGroup="insert" CssClass="buttonbg" CausesValidation="True" />
                                                                
                                                            </td>
                                                        </tr>
                                                        
                                                    </table>
                                                
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
