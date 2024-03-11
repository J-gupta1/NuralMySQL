<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ViewPriceDrop.aspx.cs" Inherits="Masters_HO_Admin_ViewPriceDrop" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
   
            <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
                <tr>
                    <td align="left" valign="top">
                        <table cellspacing="0" cellpadding="0" width="965" border="0">
                            <tr>
                                <td align="left" valign="top">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td align="left" valign="top"> 
                                                <asp:UpdatePanel ID="updMsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
                                                <uc1:ucMessage ID="ucMessage1" runat="server" />
                                                <asp:HiddenField ID="txt" runat="server" />

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

                                </table>
                        </td>
                        </tr>
                <tr>
                                <td valign="top" align="left">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="70%" align="left" valign="top" class="tableposition">
                                                            <div class="mainheading">
                                                                View Price Drop</div>
                                                        </td>
                                                        <td width="30%" align="right" style="padding-right: 10px;">
                                                           
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left" class="tableposition">
                                                <div class="contentbox">
                                                    <table cellspacing="0" cellpadding="4" border="0" width="100%">
                                                        <tr>
                                                            <td colspan="6" class="mandatory" valign="top">
                                                                (*) Marked fields are mandatory.
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="35" width="14%" align="right" valign="top">
                                                               From Date : 
                                                            </td>
                                                            <td width="15%" align="left" valign="top">
                                                                <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="Please enter a from date." 
                                                                                     defaultDateRange="True"  ValidationGroup="Serach"  />

                                                                
                                                            </td>
                                                           <td height="35" width="14%" align="right" valign="top">
                                                                                Date To:
                                                                            </td>
                                                                             <td width="15%" align="left" valign="top">
                                                                                <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="Please enter a to date."
                                                                                    defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date."
                                                                                    ValidationGroup="Serach"
                                                                                     />
                                                                            </td>
                                                            <td align="right" valign="top" width="14%">
                                                                Sales Channel Code:
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtSalesChannelCodeSearch" runat="server" CssClass="form_input2" MaxLength="50"></asp:TextBox>
                                                               
                                                            </td>
                                                        </tr>
                                                        <tr>

                                                             <td align="right" width="14%" valign="top">
                                                                IMEI:
                                                            </td>
                                                            <td align="left" valign="top" width="18%">
                                                                <asp:TextBox ID="txtSerialNumberSearch" runat="server" CssClass="form_input2" MaxLength="100"></asp:TextBox>
                                                            </td>

                                                            <td align="right" height="35" valign="top">
                                                                Status: <span class="mandatory">&nbsp;</span>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form_select">
                                                                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                                                                    <asp:ListItem Text="Approve" Value="Approve"></asp:ListItem>
                                                                    <asp:ListItem Text="Reject" Value="Reject"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            
                                                            <td>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:Button ID="btnSearch" runat="server" ValidationGroup="Serach" CausesValidation="false"
                                                                    CssClass="buttonbg" Text="Search" OnClick="btnSearch_Click"  />
                                                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonbg" Text="Cancel" ToolTip="Cancel"
                                                                    OnClick="btnCancel_Click" CausesValidation="false" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" height="10" valign="top">
                                            </td>
                                        </tr>
                                        
                                          </table>
                                    </td>
                                </tr>

                 <asp:UpdatePanel ID="UpdGrid" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                                        <div id="dvhide" runat="server" visible="false">
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td width="90%" align="left" class="tableposition">
                                                                <div class="mainheading">
                                                                    &nbsp;List</div>
                                                            </td>
                                                            <td width="10%" align="right">
                                                                <asp:Button ID="ExportToExcel" CssClass="excel" runat="server" Text="" OnClick="ExportToExcel_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" class="tableposition">
                                                  
                                                    <div class="contentbox" style="margin-bottom: 10px;">
                                                        <div class="grid2">
                                                            <asp:GridView ID="GridPriceDrop" runat="server" AlternatingRowStyle-CssClass="gridrow1"
                                                                AutoGenerateColumns="false" bgcolor="" AllowPaging="true" 
                                                                BorderWidth="0px" CellPadding="4" CellSpacing="1"
                                                                FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                                                                GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                                                                HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                                                                RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                                                               >
                                                                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                <Columns>
                                                               
                                                                 
                                                                     <asp:TemplateField HeaderText="SalesChannel/Retailer Name" ItemStyle-Width="85px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSalesChannelName" runat="server" Text='<%#Eval("SalesChannelName")%>' ></asp:Label>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                      <asp:TemplateField HeaderText="SalesChannel/Retailer Code" ItemStyle-Width="85px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRetailerCode" runat="server" Text='<%#Eval("SalesChannelCode")%>' ></asp:Label>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                      <asp:TemplateField HeaderText="ModelName" ItemStyle-Width="85px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblModelName" runat="server" Text='<%#Eval("ModelName")%>' ></asp:Label>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                       <asp:TemplateField HeaderText="ModelName" ItemStyle-Width="85px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblModelName" runat="server" Text='<%#Eval("ModelName")%>' ></asp:Label>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                       <asp:TemplateField HeaderText="SKUName" ItemStyle-Width="85px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbSKUName" runat="server" Text='<%#Eval("SKUName")%>' ></asp:Label>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                       <asp:TemplateField HeaderText="QTY" ItemStyle-Width="85px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQTY" runat="server" Text='<%#Eval("QTY")%>' ></asp:Label>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    
                                                                       <asp:TemplateField HeaderText="IMEI" ItemStyle-Width="85px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIMEI" runat="server" Text='<%#Eval("IMEI")%>' ></asp:Label>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="85px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' ></asp:Label>
                                                                    </ItemTemplate>                                                                        
                                                                    </asp:TemplateField>
                                                                    
                                                                        
                                                                       <asp:TemplateField HeaderText="Remarks" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>' ></asp:Label>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                 
                                                                </Columns>
                                                                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                <AlternatingRowStyle CssClass="gridrow1" />
                                                                <PagerStyle CssClass="PagerStyle" />
                                                            </asp:GridView>
                                                        </div> 
                                                         <div id="dvFooter" runat="server" class="pagination">
                                                             <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
                                                     </div>
                                             </div>        
                                                </td>
                                            </tr>
                                        </div>
                            </ContentTemplate>
                        <Triggers>
                           <asp:PostBackTrigger ControlID="btnSearch" />
                            <asp:PostBackTrigger ControlID="ExportToExcel" />
                        </Triggers>
                    </asp:UpdatePanel>

                                    </table>
                              
          
       
    
</asp:Content>
