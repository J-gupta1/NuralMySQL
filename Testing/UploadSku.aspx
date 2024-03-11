<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UploadSku.aspx.cs" Inherits="Testing_UploadSku" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
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
                        <td align="left" valign="top">
                            <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <uc1:ucMessage ID="ucMessage1" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="tableposition">
                            <div class="mainheading">
                                <asp:Label ID="lblStockTransfer2" Text=" Bulk Retailer Transfer" runat="server" /></div>
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
                            <td colspan="5" align="left" valign="top" height="15" class="mandatory">
                                (*) Marked fields are mandatory
                            </td>
                        </tr>
                        <tr>
                            <td width="15%" align="right" class="formtext" valign="top">
                                Master To Upload : <font class="error">*</font>
                            </td>
                            <td width="20%" align="left" class="formtext" valign="top">
                                <div style="float: left; width: 135px;">
                                    <asp:DropDownList ID="cmbMaster" runat="server" CssClass="form_select" 
                                        AutoPostBack="True" onselectedindexchanged="cmbMaster_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    
                                    <div>
                                   
                                    </div>
                                </div>
                              
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="cmbSalesManfrom"
                                        CssClass="error" ValidationGroup="Add" InitialValue="0" runat="server" ErrorMessage="Please select a  from Sales Channel "></asp:RequiredFieldValidator>--%>
                                        
                            </td>
                            <td> 
                            <asp:Button ID="btnCancel" runat="server" Text="&nbsp;Cancel&nbsp;" CssClass="buttonbg"
                                    CausesValidation="false"  />
                            </td>
                            
                                 <td align="left" width="55%" valign="top">
                                 <asp:Label ID="lblInfo" runat="server" CssClass="error" Text="Upload File" Visible="false"></asp:Label>
                                 <asp:FileUpload ID="FileUpload1" CssClass="form_file" runat="server" />
                                                    
                                                </td>
                         <%--   <td width="10%" height="35" align="right" class="formtext" valign="top">
                                Transfer To: <font class="error">*</font>
                            </td>
                            <td width="25%" align="left" class="formtext" valign="top">
                                <div style="float: left; width: 135px;">
                                    <asp:DropDownList ID="cmbSalesManTo" runat="server" CssClass="form_select" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div style="float: left; width: 180px;">
                                    <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="cmbSalesManTo" CssClass="error"
                                        ValidationGroup="Add" InitialValue="0" runat="server" ErrorMessage="Please select a Sales Channel name "></asp:RequiredFieldValidator></div>
                            </td>--%>
                            <td align="left" valign="top" width="30%">
                               
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <div>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td width="40%" valign="top">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td valign="top">
                                            <asp:Panel ID="Pnlfrom" runat="server" Visible="false">
                                                <asp:UpdatePanel ID="updFrom" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="grdSku" runat="server" FooterStyle-VerticalAlign="Top"
                                                            FooterStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                                                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="None"
                                                            AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                                                            HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                                                            BorderWidth="0px" Width="100%" AutoGenerateColumns="False" AllowPaging="True"
                                                            SelectedStyle-CssClass="gridselected" DataKeyNames="ColumnName" 
                                                            EmptyDataText="No record found" PageSize="100">
                                                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridrow"></RowStyle>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sku Schema" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                       <%-- <asp:CheckBox ID="chkRetailerTransfer" runat="server" />--%>
                                                                        <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ColumnName")%>'></asp:Label>
                                                                        <asp:Label ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ColumnConstraint")%>'
                                                                            Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridheader"></HeaderStyle>
                                                            <AlternatingRowStyle CssClass="gridrow1"></AlternatingRowStyle>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="60%" valign="top">
                                <asp:Panel ID="pnlto" runat="server" Visible="true">
                                    <asp:UpdatePanel ID="updto" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <%-- <td width="30%" valign="top" style="padding-left: 10px;">
                                                       <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td height="40">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnTransferselected" runat="Server" Text="Transfer Selected" Width="140px"
                                                                      CssClass="buttonbg" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnTransferAll" runat="Server" Text="Transfer All" Width="140px" CssClass="buttonbg" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>--%>
                                                    <td width="70%">
                                                        <asp:GridView ID="grdUploadedSku" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                                                            RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top" HeaderStyle-HorizontalAlign="left"
                                                            HeaderStyle-VerticalAlign="top" GridLines="none" AlternatingRowStyle-CssClass="gridrow1"
                                                            RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                                                            CellSpacing="1" CellPadding="4" bgcolor="" BorderWidth="0px" Width="100%" AutoGenerateColumns="true"
                                                            AllowPaging="True" SelectedStyle-CssClass="gridselected" DataKeyNames=""
                                                            EmptyDataText="No record found">
                                                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                                          <%--  <Columns>
                                                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="FirstColumnName"
                                                                    HeaderText="Uploaded Sku Schema"></asp:BoundField>
                                                            </Columns>--%>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
              <%--  &nbsp;--%>
                   <asp:Button ID="btnUpload" runat="server" Text="&nbsp;Upload&nbsp;" CssClass="buttonbg"
                                    CausesValidation="false" onclick="btnUpload_Click1"  />
            </td>
        </tr>
        <tr>
            <td height="10">
            </td>
        </tr>
    </table>
</asp:Content>
