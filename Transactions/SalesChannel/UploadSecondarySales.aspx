<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadSecondarySales.aspx.cs" Inherits="Transactions_SalesChannel_UploadSecondarySales"  MasterPageFile="~/CommonMasterPages/MasterPage.master"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div align="center">
        <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
            <tr>
                <td align="left" valign="top" >
                    <table cellspacing="0" cellpadding="0" width="965" border="0">
                        <tr>
                            <td valign="top" align="left">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                       
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <uc1:ucMessage ID="ucMsg" runat="server" />
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td align="left" valign="top" height="5">
                                                            </td>
                                                            </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                        <td align="left" valign="top">
                           <table width="100%" border="0" cellspacing="0" cellpadding="0" style="float:left">
                                                            <tr>
                                                                <td align="left" valign="top" class="tableposition">
                                                                    <div class="mainheading">
                                                                        Upload</div>
                                                                </td>
                                                            </tr>
                                                        </table>
                        </td>
                    </tr>
                    
                                    <tr>
                                        <td align="left" valign="top" class="tableposition">
                                            <asp:UpdatePanel runat="server" ID="UpdControl" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                      <div class="contentbox">
                                                                    <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                                     <tr>
                                                <td colspan="4" align="left" valign="top" height="15" class="mandatory">
                                                    (*) Marked fields are mandatory
                                                </td>
                                            </tr>
                                             <tr>
                                              <td class="formtext" valign="top" align="right" width="10%" >
                                                                                Select Mode:<font class="error">*</font>
                                                                            </td>
                                                <td width="25%" align="left" valign="top" class="formtext" > <asp:RadioButtonList ID="rdModelList" runat="server" TextAlign="Right" 
                                                                                    RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0" BorderWidth="0"  
                                                                                    AutoPostBack="True" 
                                                                                    onselectedindexchanged="rdModelList_SelectedIndexChanged">
                                                                                    <asp:ListItem Text="Excel" Value="0" Selected="True"></asp:ListItem>
                                                                                    <asp:ListItem Text="Interface" Value="1"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                                 </td>
                                           
                                                                          <!--  <td class="formtext" valign="top" align="right" width="10%" height="35">
                                                                                Select Mode:<font class="error">*</font>
                                                                            </td>-->
                                                                            
                                                                        
                                                                         <%--   <td class="formtext" valign="top" align="right" width="13%" height="35">
                                                                                Select Return Date:<font class="error">*</font>
                                                                            </td>
                                                                            <td width="10%" align="left" valign="top">
                                                                                <uc2:ucDatePicker ID="ucSalesReturnDate" runat="server" ErrorMessage="Invalid date."
                                                                                    IsRequired="true" defaultDateRange="true" RangeErrorMessage="Date should be less then equal to current date."
                                                                                    ValidationGroup="SalesReturn" />
                                                                            </td>--%>
                                                                      
                                                                            <td class="formtext" valign="top" align="left" width="10%"  height="35">
                                                                                Select Excel<font class="error">*</font>
                                                                            </td>
                                                                            <td width="30%" align="left" valign="top">
                                                                                <asp:FileUpload ID="Fileupdload" CssClass="form_file" runat="server" />
                                                                                <br /><asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                                                                            </td>
                                                                      
                                                                           
                                                                            <td class="formtext" valign="top" align="left"   width="25%"  >
                                                                                <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" 
                                                                                    onclick="btnUpload_Click" />
                                                                            </td>
                                                                              </tr>
                                                                        <tr>
                                                                       <td valign="top" align="left" ></td>
                                                                            <td valign="top" align="left" colspan="4">
                                                                                <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" 
                                                                                    Text="Download Reference Code" CssClass="elink2" onclick="DwnldReferenceCodeTemplate_Click"
                                                                                   ></asp:LinkButton>&nbsp; &nbsp;&nbsp; &nbsp;
                                                                          
                                                                               <%-- <asp:LinkButton ID="DwnldSKUCodeTemplate" runat="server" Text="Download SKUCode" CssClass="elink2"
                                                                                    OnClick="DwnldSKUCodeTemplate_Click"></asp:LinkButton>&nbsp; &nbsp;&nbsp; &nbsp;--%>
                                                                                <a href="../../Excel/Templates/SecondarySales.xlsx" class="elink2">Download Secondary Sales Template</a>
                                                                            </td>
                                                                        </tr>
                                                                      
                                                                    </table>
                                                                </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="DwnldReferenceCodeTemplate" />
                                                  <%--  <asp:PostBackTrigger ControlID="DwnldSKUCodeTemplate" />--%>
                                                    <asp:PostBackTrigger ControlID="btnupload" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                       <tr>
                        <td height="10">
                        </td>
                    </tr>
                           
                               <tr>
                                                                            <td align="left" class="tableposition">
                                                                                <div class="mainheading" runat="server" id="pnlGrid" visible="false">
                                                                                    Upload Preview</div>
                                                                            </td>
                                                                        </tr>
                            
                        <tr>
                            <td align="left" valign="top" class="tableposition">
                                <div class="contentbox" runat="server" id="pnlGrid1" visible="false">
                                    <div class="grid1">
                                        <%--<asp:UpdatePanel runat="server" ID="updgrid1" UpdateMode="Conditional">
                                            <ContentTemplate>--%>
                                        <asp:GridView ID="gvSecondarySales" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            CellSpacing="1" DataKeyNames="SKUCode" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                                            GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                                            AlternatingRowStyle-CssClass="gridrow1" Width="100%" AllowPaging="false" 
                                           >
                                            <RowStyle CssClass="gridrow" />
                                            <Columns>
                                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="RetailerCode"
                                                    HeaderText="Retailer Code">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                
                                           
                                                
                                                <asp:TemplateField HeaderText="Sales Date">
                                                <ItemTemplate>
                                                 <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("SalesDate","{0:dd MMM yy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left"/>
                                                </asp:TemplateField>
                                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUCode"
                                                    HeaderText="SKU Code">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Quantity"
                                                    HeaderText="Quantity">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                               <%-- <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesMan"
                                                    HeaderText="Salesman">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>--%>
                                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Error"
                                                    HeaderText="Error">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                            </Columns>
                                            <HeaderStyle CssClass="gridheader" />
                                            <EditRowStyle CssClass="editrow" />
                                            <AlternatingRowStyle CssClass="gridrow1" />
                                        </asp:GridView>
                                        <%--  </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </div>
                                    
                                    <div style="float:left; padding-top:5px;">
                                     <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="always">
                                    <ContentTemplate>
                                        <asp:Button ID="btnSave" Text="Save" runat="server" ToolTip="Save" 
                                            CssClass="buttonbg" CausesValidation="true" ValidationGroup="SalesReturn" 
                                            onclick="btnSave_Click">
                                        </asp:Button>
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" ToolTip="Cancel" 
                                            CssClass="buttonbg" onclick="btnCancel_Click"></asp:Button>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnSave" />
                                        <asp:PostBackTrigger ControlID="btnCancel" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                </div>
                                </div>
                            </td>
                        </tr>
                       
                        <tr>
                            <td align="left" valign="top" height="25" class="formtext">
                               
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </td>
        </tr>
        </table>
        
    </div>
</asp:Content>
