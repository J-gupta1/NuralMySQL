<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="SecondarySalesReturnInterfaceWithType.aspx.cs" Inherits="Transactions_POC_SecondarySalesReturnInterfaceWithType" %>
<%@ Register Src="~/UserControls/SalesReturnGrid.ascx" TagName="ucSalesReturnGrid"
    TagPrefix="uc3" %>
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
                                            Secondary Sales Return</div>
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
                            <%--  <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
                                    <ContentTemplate>--%>
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
                                        <td  align="left" valign="top" width="23%"  class="formtext">
                                             <asp:RadioButtonList ID="rdoSelectMode" runat="server" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0" BorderWidth="0" 
                                                AutoPostBack="true" OnSelectedIndexChanged="rdoSelectMode_SelectedIndexChanged">
                                                <asp:ListItem Value="1" Text="Excel Template"></asp:ListItem>
                                                <asp:ListItem Selected="True" Value="2" Text="Interface"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    
                                    <td width="12%"  align="right" class="formtext" valign="top">
                                                    Select Salesman: <font class="error">*</font>
                                                </td>
                                                <td width="20%" align="left" class="formtext" valign="top">
                                                 <div style="float:left; width:135px;">   <asp:DropDownList ID="ddlSalesman" runat="server" CssClass="form_select" OnSelectedIndexChanged="ddlSalesman_SelectedIndexChanged"  AutoPostBack="true" >
                                                    </asp:DropDownList>
                                                    
                                                    <br /></div> <div style="float:left; width:160px;">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlSalesman"  CssClass="error"
                                                        ValidationGroup="EntryValidation" InitialValue="0" runat="server" ErrorMessage="Please select salesman."></asp:RequiredFieldValidator>
                                                </td>
                                                 
                                        <td width="15%"  valign="top" align="right" class="formtext">
                                            Select Retailer:<font class="error">*</font>
                                        </td>
                                        <td width="20%" valign="top" align="left" class="formtext">
                                        <div style="float:left; width:150px;">
                                            <asp:DropDownList ID="ddlRetailer" runat="server" CssClass="form_select" 
                                                onselectedindexchanged="ddlRetailer_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Text="Select" Value="0" ></asp:ListItem>
                                            </asp:DropDownList></div>
                                           <div style="float:left; width:150px;">
                                            <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlRetailer" CssClass="error"
                                                ValidationGroup="EntryValidation" InitialValue="0" runat="server" ErrorMessage="Please select Retailer Name."></asp:RequiredFieldValidator></div>
                                        </td>
                                      </tr>
                                                  <tr>
                                        
                                        <td valign="top"  align="right" class="formtext">
                                                Return Date:<font class="error">*</font>
                                            </td>
                                            <td align="left" valign="top" >
                                                <uc2:ucDatePicker ID="ucReturnDate" runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                                                    ValidationGroup="EntryValidation" />
                                                <asp:Label ID="lblInfo" runat="server" CssClass="error" Text="" Visible="True"></asp:Label>
                                               
                                            </td>
                                     <td align="right" valign="top">
                                        </td>
                                        
                                       <%-- <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                          <asp:Panel ID="PnlHide" runat="server" Visible="false">
   <tr>
                                            <td valign="top" align="right" class="formtext" width="32%">
                                                Sales Man:<font class="error">*</font>
                                            </td>
                                            <td align="left" valign="top" width="68%">
                                              
                                              <asp:TextBox ID="txtSalesman" runat="server" MaxLength="100" CssClass="form_input2"  ></asp:TextBox>
                                                <br>
                                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtSalesman" CssClass="error" Display="Dynamic"
                                                ValidationGroup="Add" runat="server" ErrorMessage="Please enter salesman."></asp:RequiredFieldValidator>
                                            </td>
                                           </tr>
                                           </asp:Panel>
</table>--%>

                                      
                                        
                                        <td align="left" valign="top" colspan="3" >
                                            <asp:Button ID="BtnSubmit" runat="server" Text="&nbsp;Add/Edit&nbsp;" CssClass="buttonbg"
                                                CausesValidation="true" ValidationGroup="EntryValidation" OnClick="BtnSubmit_Click" />
                                        </td>
                                    </tr>
                                </table>
                              <%--  </ContentTemplate>
                                
                                </asp:UpdatePanel>--%>
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
                   <%--  <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                        <td align="left" valign="top">
                         
                            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                                <table id="tblGrid" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left" class="tableposition">
                                            <div class="mainheading">
                                                Enter Secondary Sales Return Interface Details
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
                                            <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" CausesValidation="true" ValidationGroup="Add" 
                                                 OnClick="btnSave_Click" />
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
                         <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
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
