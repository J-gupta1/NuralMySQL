<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IntermediaryOrder.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    Inherits="Transactions_SalesChannel_PrimaryOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/SalesEntryGrid.ascx" TagName="ucSalesEntryGrid"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0" align="center" style="float: left;">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                  
                    <tr>
                        <td valign="top" align="left">
                            <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <uc1:ucMessage ID="ucMsg" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="float: left">
                                <tr>
                                    <td align="left" valign="top" class="tableposition">
                                        <div class="mainheading">
                                            Intermediary Order</div>
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
                                        <td colspan="7" align="left" valign="top" height="15" class="mandatory">
                                            (*) Marked fields are mandatory
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="23%" height="35" valign="top" align="right" class="formtext">
                                            Select Parent Saleschannel: <font class="error">*</font>
                                        </td>
                                        <td width="15%" valign="top" align="left">
                                             <div style="float:left; width:135px;"> <asp:DropDownList ID="ddlPsalesChannel" runat="server" CssClass="form_select">
                                            </asp:DropDownList>
                                               <br /></div>  <div style="float:left; width:170px;">
                                            <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlPsalesChannel" CssClass="error"
                                                ValidationGroup="EntryValidation" InitialValue="0" runat="server" ErrorMessage="Please select sales channel name."></asp:RequiredFieldValidator></div>
                                        </td>
                                        <td width="15%" valign="top" align="right" class="formtext">
                                            Order Number: <font class="error">*</font>
                                        </td>
                                        <td width="15%" valign="top" align="left">
                                            <asp:TextBox ID="txtOrderNo" runat="server" MaxLength="20" CssClass="form_input2"></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="RequInvoiceNumber" ControlToValidate="txtOrderNo"
                                                ValidationGroup="Add" runat="server" CssClass="error" ErrorMessage="Please enter Order number."></asp:RequiredFieldValidator>
                                        </td>
                                   
                                        <td height="35" width="15%" valign="top" align="right" class="formtext">
                                            Order Date: <font class="error">*</font>
                                        </td>
                                        <td valign="top" align="left" width="12%">
                                            <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                                                ValidationGroup="Add" />
                                        </td>
                                  
                                        <td align="left" valign="top" width="15%">
                                            <asp:Button ID="BtnSubmit" runat="server" Text="&nbsp;Go&nbsp;" CssClass="buttonbg"
                                                CausesValidation="true" ValidationGroup="EntryValidation" OnClick="BtnSubmit_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td height="10">&nbsp;
                            
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                                <table id="tblGrid" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left" class="tableposition">
                                            <div class="mainheading">
                                                Enter Details
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" class="tableposition">
                                          <%--  <div class="contentbox">
                                                <div class="grid1">--%>
                                                    <uc3:ucSalesEntryGrid ID="Ucgrid" runat="server" />
                                              <%--  </div>
                                            </div>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="10">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save"  ValidationGroup="Add"
                                                CausesValidation="true" OnClick="btnSave_Click" />
                                            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" OnClick="btnReset_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="10">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
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
