<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PrimarySales2ReturnInterface.aspx.cs" Inherits="Transactions_SalesChannel_PrimarySales2ReturnInterface" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/SalesReturnGrid.ascx" TagName="ucSalesReturnGrid"
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
                                            Primary Sales Return Interface</div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <%--  <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                        <td align="left" valign="top">
                            <div class="contentbox">
                                <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="4" align="left" valign="top" height="15" class="mandatory">
                                            (*) Marked fields are mandatory
                                        </td>
                                    </tr>
                                    <tr>
                                    <td class="formtext" valign="top" align="right" width="10%" >
                                                                                Select Mode:<font class="error">*</font>
                                                                            </td>
                                        <td  align="left" valign="top" width="21%" class="formtext">
                                            <asp:RadioButtonList ID="rdoSelectMode" runat="server" RepeatDirection="Horizontal"
                                                AutoPostBack="true" OnSelectedIndexChanged="rdoSelectMode_SelectedIndexChanged">
                                                <asp:ListItem Value="1" Text="Excel Template"></asp:ListItem>
                                                <asp:ListItem Selected="True" Value="2" Text="Interface"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                   
                                        <td width="14%"  valign="top" align="right" class="formtext">
                                            Select <asp:Label ID="lblChange" runat="server" ></asp:Label>: <font class="error">*</font>
                                        </td>
                                        <td width="20%" valign="top" align="left">
                                             <div style="float:left; width:135px;"><asp:DropDownList ID="ddlTD" runat="server" CssClass="form_select">
                                            </asp:DropDownList>
                                             <br /></div>  <div style="float:left; width:170px;">
                                            <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlTD" CssClass="error"
                                                ValidationGroup="EntryValidation" InitialValue="0" runat="server" ErrorMessage="Please select TD Name."></asp:RequiredFieldValidator></div>
                                        </td>
                                        <td width="15%" align="right" valign="top" class="formtext">
                                            Invoice Number: <font class="error">*</font>
                                        </td>
                                        <td width="20%" valign="top" align="left">
                                            <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="50" CssClass="form_input2"></asp:TextBox><br />
                                            <asp:RequiredFieldValidator ID="RequInvoiceNumber" ControlToValidate="txtInvoiceNo"
                                                ValidationGroup="EntryValidation" runat="server" CssClass="error" ErrorMessage="Please enter invoice number."></asp:RequiredFieldValidator>
                                        </td>
                                         </tr>
                                           <tr>
                                         <td valign="top" align="right" class="formtext">
                                                Return Date<font class="error">*</font>
                                            </td>
                                            
                                           
                                            <td align="left" valign="top">
                                                <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                                                    ValidationGroup="EntryValidation" />
                                                    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text="" Visible="false"></asp:Label>
                                               
                                            </td>
                               
                                        <td align="right" valign="top" colspan="2">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
 <asp:Panel ID="PnlHide" runat="server" Visible="false">
                                        <tr>
                                            <td  align="right" valign="top" width="40%" class="formtext"> Invoice Date:<font class="error">*</font>
                                            </td>
                                            <td align="left" valign="top" width="60%" style="padding-top:4px;">
                                             <%--  <asp:Label ID="lblInvoiceDate" runat="server" CssClass="formtext2" Visible="false"></asp:Label>--%>
                                                        <uc2:ucDatePicker ID="lblInvoiceDate" runat="server" ErrorMessage="Invalid date." 
                                                    ValidationGroup="Add" />                           
                                            </td>
                                           
                                           
                                        </tr>
                                    </asp:Panel>
</table>

                                        </td>
                                        <td align="right" valign="top">
                                        </td>
                                        
                                        <td align="left" valign="top">
                                            <asp:Button ID="BtnSubmit" runat="server" Text="&nbsp;Add/Edit&nbsp;" CssClass="buttonbg"
                                                CausesValidation="true" ValidationGroup="EntryValidation" OnClick="BtnSubmit_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <%-- </ContentTemplate>
                                
                                </asp:UpdatePanel>--%>
                    </tr>
                    <tr>
                        <td height="10">&nbsp;
                            
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <%-- <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                                <table id="tblGrid" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left" class="tableposition">
                                            <div class="mainheading">
                                                Enter Primary Sales 2 Interface Details
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%-- <uc3:ucSalesReturnGrid ID="ucSalesReturnGrid" runat="server" />--%>
                                            <uc3:ucSalesReturnGrid ID="ucSalesReturnGrid" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="10">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="Add"
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
                            <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
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
