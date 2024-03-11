<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SecondarySalesInterfaceWithType.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Transactions_SalesChannel_SecondarySalesInterface" %>

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
            <td valign="top" align="left">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                   
                    <tr>
                      <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <td align="left" valign="top">
                                    <uc1:ucMessage ID="ucMsg" runat="server" />
                                </td>
                         </ContentTemplate>
                        </asp:UpdatePanel>
                    </tr>
                    <tr>
                        <td height="5" align="left" valign="top"></td>
                   <tr>
                                        <td align="left" class="tableposition">
                                            <div class="mainheading">
                                               Manage Secondary Sales</div>
                                        </td>
                                    </tr>
                    <tr>
                        <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                        <td align="left" valign="top">
                            <div class="contentbox">
                         
                                        <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                            <tr>
                                                <td colspan="6" align="left" valign="top" height="15" class="mandatory">
                                                    (*) Marked fields are mandatory
                                                </td>
                                            </tr>
                                            <tr>
                                             <td class="formtext" valign="top" align="right" width="10%" >
                                                                                Select Mode:<font class="error">*</font>
                                                                            </td>
                                                <td width="22%"  align="left" valign="top"  class="formtext">
                                                    <asp:RadioButtonList ID="rdoSelectMode" runat="server" RepeatDirection="Horizontal"  CellPadding="0" CellSpacing="0" BorderWidth="0"
                                                        AutoPostBack="true" OnSelectedIndexChanged="rdoSelectMode_SelectedIndexChanged">
                                                        <asp:ListItem  Value="1" Text="Excel Template"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Value="2" Text="Interface"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            
                                             <td width="12%"  align="right" class="formtext" valign="top">
                                                    Select Salesman: <font class="error">*</font>
                                                </td>
                                                <td width="21%" align="left" class="formtext" valign="top">
                                                    <div style="float:left; width:135px;"> <asp:DropDownList ID="ddlSalesman" runat="server" CssClass="form_select" OnSelectedIndexChanged="ddlSalesman_SelectedIndexChanged"  AutoPostBack="true" >
                                                    </asp:DropDownList>
                                                    
                                                    <br /></div>  <div style="float:left; width:135px;">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlSalesman"  CssClass="error"
                                                        ValidationGroup="EntryValidation" InitialValue="0" runat="server" ErrorMessage="Please select salesman."></asp:RequiredFieldValidator></div>
                                                </td>
                                               
                                                <td width="12%"  align="right" class="formtext" valign="top">
                                                    Select Retailer: <font class="error">*</font>
                                                </td>
                                                <td width="21%" align="left" class="formtext" valign="top">
                                                    <div style="float:left; width:135px;"> <asp:DropDownList ID="ddlRetailer" runat="server" OnSelectedIndexChanged="ddlRetailer_SelectedIndexChanged" AutoPostBack="true" CssClass="form_select">
                                                    <asp:ListItem Selected="True" Text="Select" Value="0" ></asp:ListItem>
                                                    </asp:DropDownList>
                                                    
                                                    <br /></div>  <div style="float:left; width:135px;">
                                                    <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlRetailer" CssClass="error"
                                                        ValidationGroup="EntryValidation" InitialValue="0" runat="server" ErrorMessage="Please select retailer."></asp:RequiredFieldValidator></div>
                                                </td>
                                                 </tr> 
                                                 <tr>
                                                <td     align="right" class="formtext"  valign="top">
                                                    Invoice Date: <font class="error">*</font>
                                                </td>
                                                <td   align="left"  valign="top">
                                                    <uc2:ucDatePicker ID="ucDatePicker" runat="server" ErrorMessage="Invalid date." defaultDateRange="false"
                                                         ValidationGroup="EntryValidation" />
                                                         
                                                    <asp:Label ID="lblInfo" runat="server" Text="" CssClass="error"></asp:Label>
                                                </td>
                                               
                                                
                                           
                                               
                                                   <td align="right"  valign="top"  >
                                                  

                                                   
                                                   </td> 
                                              
                                             <td align="left" valign="top" colspan="3">
                                                    <asp:Button ID="BtnSubmit" runat="server" Text="&nbsp;Add/Edit&nbsp;" CssClass="buttonbg"
                                                        CausesValidation="true" ValidationGroup="EntryValidation" OnClick="BtnSubmit_Click" />
                                                </td>
                                                 </tr>
                                          
                                           
                                        </table>
                            
                            </div>
                        </td>
                          </ContentTemplate>
                                </asp:UpdatePanel>
                    </tr>
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                     
                    <tr>
                     <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                        <td align="left" valign="top">
                         
                                    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                                        <table id="tblGrid" cellpadding="0" cellspacing="0">
                                        <tr>
                                        <td align="left" class="tableposition">
                                            <div class="mainheading">
                                             List</div>
                                        </td>
                                    </tr>
                                            <tr>
                                                <td >
                                                    <uc3:ucSalesEntryGrid ID="ucSalesEntryGrid1" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="10">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left"  valign="top">
                                                    <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="Add"
                                                        CausesValidation="true" OnClick="btnSave_Click" />
                                                
                                                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" CausesValidation="false" OnClick="btnReset_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="10">
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                             
                        </td>
                         </ContentTemplate>
                            </asp:UpdatePanel>
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
