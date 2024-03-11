<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="AccountStatementRequest.aspx.cs" Inherits="Transactions_Common_AccountStatementRequest" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <script language="javascript" type="text/javascript">
		

        function Popup(SID, pagename) {
            if (SID.length > 0) {
                window.open(pagename + "?AccountStatementReqId=" + SID, "mywindow3", "menubar=0,width=700,height=600,left=10,top=10,scrollbars=yes");
            }
            return false;
        }
	</script>
    <div id="wrapper">
     
            <div align="center">
        <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
            <tr>
                <td align="left" valign="top">
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
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:UpdatePanel ID="updAddUserMain" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0" cellpadding="0" width="100%" border="0" style="float: left;">
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td align="left" valign="top" class="tableposition">
                                                                           
                                                                            
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" align="left" class="tableposition">
                                                                <div class="contentbox">
                                                                    <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                                        <tr>
                                                                            <td colspan="6" height="20" class="mandatory" valign="top">(*) marked fields are mandatory.
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                          <td class="formtext" valign="top" align="right" height="35">
                                                                                <asp:Label ID="lblFromDate" CssClass="formtext" runat="server" AssociatedControlID="ucFromDate">From Date:<font class="error">*</font></asp:Label>
                                                                            </td>   
                                                                             <td valign="top" align="right" width="20%">
                                                                                <uc2:ucDatePicker ID="ucFromDate" runat="server" IsRequired="true" ErrorMessage="From date required."
                                                                                    ValidationGroup="AddUserValidationGroup" />
                                                                            </td>
                                                                            <td align="right" class="formtext" valign="top" width="15%">
                                                                                <asp:Label ID="lblToDate" runat="server" AssociatedControlID="ucToDate" CssClass="formtext">To Date:<font class="error">*</font></asp:Label>
                                                                            </td>
                                                                            <td align="left" valign="top" width="20%">
                                                                                <uc2:ucDatePicker ID="ucToDate" runat="server" IsRequired="true" ErrorMessage="To date required." RangeErrorMessage="Date should be greater then equal to current date."
                                                                                    ValidationGroup="AddUserValidationGroup" />
                                                                                <td align="right" class="formtext" valign="top" width="13%"></td>
                                                                                <td align="left" valign="top" width="20%"></td>
                                                                           
                                                                            
                                                                        </tr>  
                                                                        
                                                                        
                                                                        <tr>
                                                                            <td align="left" valign="top">&nbsp;
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Button ID="btnCreateUser" Text="Submit" runat="server" CausesValidation="true"
                                                                                    ValidationGroup="AddUserValidationGroup" OnClick="btnCreateUser_Click" ToolTip="Submit"
                                                                                    CssClass="buttonbg" />&nbsp;&nbsp;
                                                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="buttonbg"
                                                                                    OnClick="btnCancel_Click" />
                                                                            </td>
                                                                            <td align="left" class="formtext" valign="top"></td>
                                                                            <td></td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td align="left" valign="top">&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                    <asp:PostBackTrigger ControlID="btnCreateUser" />
                                                   
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" height="10"></td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td align="left" class="tableposition">
                                            <div class="mainheading">
                                                Search Account Statement Request
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" class="tableposition">
                                            <div class="contentbox">
                                                <asp:UpdatePanel ID="UpdSearch" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr>
                                                                <td class="formtext" valign="top" align="right" height="35">
                                                                                <asp:Label ID="lblSearchFromDate" CssClass="formtext" runat="server" AssociatedControlID="ucSearchFromDate">From Date:<font class="error">*</font></asp:Label>
                                                                            </td> 
                                                                <td align="left" valign="top" width="120">
                                                                    
                                                                                <uc3:ucDatePicker ID="ucSearchFromDate" runat="server" IsRequired="true" ErrorMessage="From date required."
                                                                                   ValidationGroup="AddUserSerarhValidationGroup" />
                                                                                                                                         </td>

                                                                <td align="left" valign="top" width="60" height="25" class="formtext">
                                                                    
                                                                                <asp:Label ID="lblSearchToDate" runat="server" AssociatedControlID="ucSearchToDate" CssClass="formtext">To Date:<font class="error">*</font></asp:Label>
                                                                            
                                                                </td>
                                                                <td align="left" valign="top" width="220">
                                                                     <uc3:ucDatePicker ID="ucSearchToDate" runat="server" IsRequired="true" ErrorMessage="To date required." RangeErrorMessage="Date should be greater then equal to current date."
                                                                                   ValidationGroup="AddUserSerarhValidationGroup" />
                                                                </td>
                                                                <td align="left" valign="top" width="70" class="formtext">
                                                                </td>
                                                                <td align="left" valign="top" width="120">
                                                                    
                                                                </td>
                                                               
                                                            </tr>
                                                            <tr>
                                                               
                                                               <td align="left" valign="top" width="60"></td>
                                                                <td align="left" valign="top" width="60">
                                                                    <asp:Button ID="btnSearchUser" Text="Search" runat="server"  ToolTip="Search" CssClass="buttonbg"
                                                                        OnClick="btnSearchUser_Click" CausesValidation="False"></asp:Button>
                                                                </td>
                                                                <td align="left" valign="top" width="100">
                                                                    
                                                                </td>
                                                            </tr>


                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" height="10"></td>
                        </tr>
                        <%--------------------------Panelhide Started---------------------------------------------%>
                       <%-- <asp:Panel ID="dvPanel" runat="server" Visible="false">--%>
                        <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="90%" align="left" class="tableposition">
                                            <div class="mainheading">
                                               Account Statement List
                                            </div>
                                        </td>
                                        <td width="10%" align="right">
                                            
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" class="tableposition">
                                <div class="contentbox">
                                    <div class="grid2">
                                        <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="grdvwUserList"  OnRowDataBound="grdvwUserList_RowDataBound" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                                                    RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                                                    HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                                                    AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                                                    HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                                                    BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                                                    PageSize='<%$ AppSettings:GridViewPageSize %>' OnPageIndexChanging="grdvwUserList_PageIndexChanging" SelectedStyle-CssClass="gridselected"
                                                     DataKeyNames="AccountStatementReqId ,RequestStatus" 
                                                    >
                                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                                    <Columns>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="DateFrom"
                                                            HeaderText="Request From Date"  DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                                        
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="DateTo"
                                                            HeaderText="Request To Date"  DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                                        
                                                         <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="RequestStatus"
                                                            HeaderText="Request Status"></asp:BoundField> 
                                                          
                                                        <asp:TemplateField HeaderText="Print Preview" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Button ID="BtnPrint"  CommandName ="BtnPrint" runat="server" CssClass="buttonbg" Text="Print" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField> 
                                                    </Columns>
                                                    <PagerStyle CssClass="PagerStyle" />
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                               
                                                <asp:AsyncPostBackTrigger ControlID="btnSearchUser" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </td>
                        </tr>
                           <%-- </asp:Panel>--%>
                        <%--------------------------------------------Panelhide End ----------------------------------------------------------%>
                        <tr>
                            <td align="left" height="10"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <%--     <uc3:footer ID="Footer1" runat="server" />--%>
    </div>
        </div>
        
    
</asp:Content>

