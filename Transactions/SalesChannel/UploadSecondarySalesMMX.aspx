<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="UploadSecondarySalesMMX.aspx.cs" Inherits="Transactions_SalesChannel_UploadSecondarySalesMMX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0" align="center" style="float: left;">
        <tr>
            <td valign="top" align="left">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                   
                    <tr>
                        <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <td align="left" valign="top">
                                    <uc1:ucMessage ID="ucMsg" runat="server" />
                                </td>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="float: left">
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
                        <td align="left" valign="top">
                            <div class="contentbox">
                                <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                        
                                                <td colspan="4" align="left" valign="top" height="20" class="mandatory">
                                                    (<font class="error">*</font>) Marked fields are mandatory
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
                                            <td class="formtext" valign="top" align="right" width="15%" height="35">
                                                                                Sales Date:<font class="error">*</font>
                                                                            </td>
                                                                            <td width="12%" align="left" valign="top">
                                                                                <uc2:ucDatePicker ID="ucSalesDate" runat="server" ErrorMessage="Invalid date."
                                                                                    IsRequired="true" defaultDateRange="true" RangeErrorMessage="Date should be less then equal to current date."
                                                                                    ValidationGroup="Add" />
                                                                                    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                                                                            </td>
                                                                      
                                                <td width="10%" height="35" align="left" class="formtext" valign="top">
                                                    Upload File: <font class="error">*</font>
                                                </td>
                                                <td width="30%" align="left" valign="top">
                                                    <asp:FileUpload ID="FileUpload1" CssClass="form_file" runat="server" />
                                                     
                                                </td>
                                                <td width="15%" align="left" valign="top">
                                                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                                                        OnClick="btnUpload_Click" />
                                                </td>
                                                <td width="45%" align="left" valign="top">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left" colspan="4">
                                                <%--Link is uncommented for the Micromax new requirement Pankaj Dhingra --%>
                                                   <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                                                        CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>&nbsp;
                                                    &nbsp;&nbsp; &nbsp;
                                                    &nbsp;&nbsp;&nbsp;
                                                        <asp:LinkButton ID="lnkDownLoadTemplate" runat="server" Text="Download Template"
                                                        CssClass="elink2" onclick="lnkDownLoadTemplate_Click"></asp:LinkButton>&nbsp;
                                                     <%--<a class="elink2" href="../../Excel/Templates/PrimarySalesMicromax.xlsx">
                                                        Download Template</a>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpload" />
                                        <asp:PostBackTrigger ControlID="DwnldReferenceCodeTemplate" />
                                         <asp:PostBackTrigger ControlID="lnkDownLoadTemplate" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                     <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
               <asp:Panel ID="pnlGrid" Visible="false" runat="server">
                        <tr>
                            <td align="left" class="tableposition">
                                <div class="mainheading" runat="server" id="dvUploadPreview" visible="false">
                                    Upload Preview</div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" class="tableposition">
                                <div class="contentbox">
                                    <div class="grid1">
                                       
                                                <asp:GridView ID="GridSales" runat="server" AutoGenerateColumns="true" CellPadding="4"
                                                    CellSpacing="1" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                                                    GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                                                    AlternatingRowStyle-CssClass="gridrow1" Width="100%" AllowPaging="false">
                                                    <RowStyle CssClass="gridrow" />
                                                    <Columns>
                                                        </Columns>
                                                    <HeaderStyle CssClass="gridheader" />
                                                    <EditRowStyle CssClass="editrow" />
                                                    <AlternatingRowStyle CssClass="gridrow1" />
                                                    <PagerStyle CssClass="PagerStyle" />
                                                </asp:GridView>
                                         
                                    </div>
                                </div>
                            </td>
                        </tr>
                         <tr>
                        <td height="5">
                        </td>
                    </tr>
                     <tr>
                        <td align="left">
                         <asp:Button ID="Btnsave" runat="server" CssClass="buttonbg" Text="Save" ValidationGroup="Add"
                                                OnClick="Btnsave_Click" />
                                            <asp:Button ID="BtnCancel" runat="server" CssClass="buttonbg" Text="Cancel" OnClick="BtnCancel_Click" />
                        </td>
                    </tr>
                    </asp:Panel>
                       </ContentTemplate>
                                        </asp:UpdatePanel>
                                      
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>