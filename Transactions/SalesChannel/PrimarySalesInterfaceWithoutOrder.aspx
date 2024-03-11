<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrimarySalesInterfaceWithoutOrder.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Transactions_SalesChannel_PrimarySalesInterface" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/UserControls/ucMessage.ascx" tagname="ucMessage" tagprefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register src="~/UserControls/SalesEntryGridWithoutOrder.ascx" tagname="ucSalesEntryGrid" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />
<script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>
<script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>
 <script type ="text/javascript">
     function popup() {
         WinSearchChannelCode = dhtmlmodal.open("SearchSalesChannelCode", "iframe", "SearchSalesChannelCode.aspx", "Sales Channel Detail", "width=800px,height=430px,top=25,resize=0,scrolling=auto ,center=1")
         WinSearchChannelCode.onclose = function() {
             return true;
         }
         return false;
     }
        </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="780" border="0" align="center" style="float: left;">
       
        <tr>
        <td><table cellspacing="0" cellpadding="0" width="100%" border="0">
                    
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
                                            Primary Sales Interface</div>
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
                                                <td colspan="5" align="left" valign="top" height="15" class="mandatory">
                                                    (*) Marked fields are mandatory
                                                </td>
                                                <td  align="left" valign="top" > <asp:Button ID="btnsearch" runat="server" Text="Search Sales Channel" CssClass="buttonbg" onclick="btnsearch_Click"/></td>
                                            </tr>
                                            <tr>
                                             <td class="formtext" valign="top" align="right" width="13%" >
                                                                    Select Mode:<font class="error">*</font>
                                                                </td>
                                                <td  align="left" valign="top" width="10%"  class="formtext" >
                                                    <asp:RadioButtonList ID="rdoSelectMode" runat="server" RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0"
                                                        AutoPostBack="true" OnSelectedIndexChanged="rdoSelectMode_SelectedIndexChanged">
                                                        <asp:ListItem  Value="1" Text="Excel Template"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Value="2" Text="Interface"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                          
                                            <td width="18%"   valign="top" align="right" class="formtext">
                                                Select <asp:Label ID="lblChange" runat="server" ></asp:Label> From :<font class="error">*</font>
                                                </td>
                                                <td width="16%"  valign="top" align="left" class="formtext">
                                                <div style="float: left; width: 135px;"> <asp:DropDownList ID="ddlWarehouse" runat="server"  CssClass="form_select" 
                                                         ></asp:DropDownList><br /></div>  <div style="float:left; width:170px;">
                                                   <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="ddlWarehouse" CssClass="error" ValidationGroup="EntryValidation" InitialValue="0" runat="server" ErrorMessage="Please select Warehouse Name."></asp:RequiredFieldValidator></div>
                                                    
                                                </td>
                                                  <td width="16%"   valign="top" align="right" class="formtext">
                                               <asp:Label ID="lblChangeTo" runat="server" ></asp:Label> To:<font class="error">*</font>
                                                </td>
                                                <td width="19%"  valign="top" align="left" class="formtext">
                                                <%--<asp:DropDownList ID="ddlDistri" runat="server"  CssClass="form_select" 
                                                         ></asp:DropDownList>--%>
                                                         <asp:TextBox ID="txtSearchedName" runat="server" CssClass="form_input2" Enabled="false"></asp:TextBox>
                                                         <br />
                                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtSearchedName" ValidationGroup="EntryValidation" runat="server" CssClass="error" ErrorMessage="Please Enter SalesChannelTo."></asp:RequiredFieldValidator>
                                                        
                                               
                                                    
                                                </td>
                                              
                                               </tr>
                                               <tr>
                                                <td   valign="top"  align="right" class="formtext">
                                                   Invoice Number: <font class="error">*</font>
                                                </td>
                                                <td   valign="top" align="left">
                                                    <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="50" CssClass="form_input2" 
                                                         ></asp:TextBox><br />
                                                    <asp:RequiredFieldValidator ID="RequInvoiceNumber" ControlToValidate="txtInvoiceNo" ValidationGroup="EntryValidation" runat="server" CssClass="error" ErrorMessage="Please enter invoice number."></asp:RequiredFieldValidator>
                                                </td>
                                                
                                              <td align="right"  valign="top" colspan="2">
                                              <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                               <asp:Panel ID="PnlHide" runat="server" Visible="false" >
  <tr>
    <td   valign="top" align="right" class="formtext" width="48%">
                                                   Invoice Date: <font class="error">*</font></td>
     <td  valign="top" align="left" width="52%">
                                                    <uc2:ucDatePicker ID="ucDatePicker"  runat="server" ErrorMessage="Invalid date." defaultDateRange="true"
                                                     ValidationGroup="Add" />
                                                     <asp:Label ID="lblValidationDays" runat="server" Text="" CssClass="error"></asp:Label>
                                                </td>
  </tr>
   </asp:Panel>
</table>

                                              </td> 
                                             
                                               <td align="right"  valign="top"  ></td>
                                             <td align="left" valign="top">
                                             <asp:HiddenField ID="hdnCode" runat="server"/>
                                              <asp:HiddenField ID="hdnName" runat="server"/>
                                             
                <asp:Button ID="BtnSubmit" runat="server" Text="&nbsp;Add/Edit&nbsp;" CssClass="buttonbg" CausesValidation="true" ValidationGroup="EntryValidation" onclick="BtnSubmit_Click" />
               
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
                    <asp:Panel ID="pnlGrid" runat="server" Visible=false>
                        <table id="tblGrid" cellpadding="0" cellspacing="0">
                         <tr>
                                <td align="left" class="tableposition">
                                    <div class="mainheading">
                                        Enter Primary Sales Interface Details
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <uc3:ucSalesEntryGrid ID="ucSalesEntryGrid1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                              <td height="10"></td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="Add" CausesValidation="true" OnClick="btnSave_Click" />  <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="buttonbg" onclick="btnReset_Click" />
                                </td>
                              
                            </tr>
                            <tr>
                              <td height="10"></td>
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
                </table></td>
        </tr>
    </table>
   <%-- <td   valign="top" align="right" class="formtext">
                                                 Salesman: <font class="error">*</font>
                                                </td>
                                                <td  align="left"  valign="top">
                                                   <asp:TextBox ID="txtSalesman" runat="server" MaxLength="100" CssClass="form_input2"  ></asp:TextBox>
                                                    <br>
                                                   </br>
                                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtSalesman" CssClass="error" ValidationGroup="Add"  runat="server" ErrorMessage="Please enter salesman name."></asp:RequiredFieldValidator>
                                                </td>--%>
</asp:Content>