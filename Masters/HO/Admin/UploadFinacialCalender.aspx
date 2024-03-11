<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="UploadFinacialCalender.aspx.cs" Inherits="Masters_HO_Admin_ManageFinacialCalender" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 <script language="javascript" type="text/javascript">

     function OnYearChange(id) {
         debugger;
         if ((id == null) || (id.value == '')) return;
         SapIntegrationService.ISCalenderExists(id.value,
            OnChange = function OnChange(result, userContext) {
                if (result) {
                    alert('Calender(' + id.value + ') already defined.If Proceed then calender will be update.');
//                    id.value = '';
                }
            }, OnError);
        }
        function OnError(result) {
            alert("Error: " + result.get_message());
        }

       </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div align="center">
        <table cellspacing="0" cellpadding="0" width="965" border="0">
            <tr>
                <td align="left" valign="top" height="420">
                    <table cellspacing="0" cellpadding="0" width="965" border="0">
                        <tr>
                            <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <td align="left" valign="top" height="15">
                                        <uc4:ucMessage ID="uclblMessage" runat="server" />
                                    </td>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="top" class="tableposition">
                                            <div class="mainheading">
                                                Input Parameters</div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left">
                                <div class="contentbox">
                                    <asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td colspan="6" height="20" class="mandatory" valign="top">
                                                        (<font class="error">*</font>) marked fields are mandatory.
                                                    </td>
                                                </tr>
                                                <tr>
                                                                         
                                                        <td height="35" align="right" valign="top" class="formtext">
                                                        Calender Year:<font class="error">*</font>
                                                    </td>
                                                        <td align="left" valign="top">
                                                        <asp:TextBox ID="txtCalenderYear" runat ="server" MaxLength="50"  CssClass="form_input2" onchange="OnYearChange(this)"  ></asp:TextBox>
                                                       
                                                         
                                                            <br/>
                                                            
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                                                ControlToValidate="txtCalenderYear" CssClass="error" Display="Dynamic" 
                                                                ErrorMessage="Please enter calander name. " ValidationGroup="Upload"></asp:RequiredFieldValidator>
                                                          
                                                    </td>
                                                        <td align="right" height="35" valign="top" class="formtext">
                                                        Year End Date:<font class="error">*</font>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <uc1:ucDatePicker ID="ucEndDate" ErrorMessage="Please enter date."
                                                            runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                   
                                                  
                                                    <td height="35" align="right" valign="top" class="formtext">
                                                        Upload file:<font class="error">*</font>
                                                    </td>
                                                    <td align="left" colspan="2" valign="top">
                                                        <asp:FileUpload ID="FileUpload1" CssClass="form_file" runat="server" />
                                                    </td>
                                                      <td align="left" valign="top">
                                                              <asp:Button ID="btnUpload" runat="server" CausesValidation="true" 
                                                                  CssClass="buttonbg" OnClick="btnUpload_Click" Text="Upload" />
                                                          </td>
                                                          <td>
                                                    
                                                          <a class="elink2" href="../../../Excel/Templates/Finacial%20Calender.xlsx">
                                                          Download Template</a>
                                                        
                                                   
                                                    </td>
                                                </tr>
                                               
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                          
                                            <asp:PostBackTrigger ControlID="btnUpload" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td height="10">
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left">
                                <%--<asp:UpdatePanel ID="UpdGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                                        <tr>
                                            <td align="left" class="tableposition">
                                                <div class="mainheading">
                                                    Upload Preview</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">
                                                <div class="contentbox">
                                                    <div class="grid1">
                                                        <asp:GridView ID="GridCalender" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="gridrow1"
                                                            bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                                                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                                                            HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                                                            RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="Left" RowStyle-VerticalAlign="top"
                                                            SelectedStyle-CssClass="gridselected" Width="100%" AutoGenerateColumns="false">
                                                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                                                            <PagerStyle CssClass="gridfooter" />
                                                            <Columns>
                                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Fortnight"
                                                                        HeaderText="Fortnight" ><HeaderStyle HorizontalAlign="Left" />
                                                                           </asp:BoundField>
                                                             <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="StartDate"
                                                                        HeaderText="Start Date" DataFormatString="{0:dd-MMM-yyyy}"><HeaderStyle HorizontalAlign="Left" />
                                                                           </asp:BoundField>
                                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Quarter"
                                                                        HeaderText="Quarter" ><HeaderStyle HorizontalAlign="Left" />
                                                                           </asp:BoundField>
                                                                             <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Error"
                                                                        HeaderText="Error" ><HeaderStyle HorizontalAlign="Left" />
                                                                           </asp:BoundField>
                                                                        </Columns>
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
                                            <td align="left" valign="top">
                                                <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Submit" OnClick="btnSave_Click"
                                                    ValidationGroup="Upload" />&nbsp;
                                                <asp:Button ID="btnReset" CssClass="buttonbg" runat="server" OnClick="btnReset_Click"
                                                    Text="Reset" />
                                            </td>
                                        </tr>
                                   </asp:Panel>
                                </table>
                                <%--    </ContentTemplate>
                  </asp:UpdatePanel>--%>
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
    </div>
</asp:Content>

