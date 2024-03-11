<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="ManageSecondarySales-SB.aspx.cs" Inherits="Transactions_SalesChannelSB_Upload_ManageSecondarySales_SB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <table cellspacing="0" cellpadding="0" width="965" border="0" align="center">
        <tr>
            <td valign="top" align="left">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="always">
                            <contenttemplate>
                                <td align="left" valign="top">
                                    <uc1:ucMessage ID="ucMsg" runat="server" />
                                </td>
                            </contenttemplate>
                        </asp:UpdatePanel>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td align="left" valign="top" class="tableposition">
                                        <div class="mainheading_rpt">
                                            <div class="mainheading_rpt_left">
                                            </div>
                                            <div class="mainheading_rpt_mid">
                                                Upload</div>
                                            <div class="mainheading_rpt_right">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <div class="contentbox">
                                <%--<asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Always">
                                    <ContentTemplate>--%>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="padding-left: 10px;">
                                    <tr>
                                        <td colspan="4" align="left" valign="top" height="20" class="mandatory">
                                            (<font class="error">*</font>) Marked fields are mandatory
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formtext" valign="top" align="right" width="10%" height="35">
                                            Select Mode:<font class="error">*</font>
                                        </td>
                                        <td width="25%" align="left" valign="top" class="formtext" colspan="3" style="padding-top: 0px; margin-top: 0px;">
                                            <asp:RadioButtonList ID="rdModelList" runat="server" TextAlign="Right" RepeatDirection="Horizontal"
                                                CellPadding="0" CellSpacing="0" BorderWidth="0" AutoPostBack="True" OnSelectedIndexChanged="rdModelList_SelectedIndexChanged">
                                                <asp:ListItem Text="Excel" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="On-Screen Entry" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="10%" height="35" align="right" class="formtext" valign="top">
                                            Upload File: <font class="error">*</font>
                                        </td>
                                        <td width="30%" align="left" valign="top">
                                            <asp:FileUpload ID="FileUpload1" CssClass="form_file" runat="server" />
                                            <br />
                                            <asp:Label ID="lblInfo" runat="server" CssClass="error" Text=""></asp:Label>
                                        </td>
                                        <td width="15%" align="left" valign="top">
                                            <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                                                OnClick="btnUpload_Click" />
                                        </td>
                                        <td align="left" valign="top">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" colspan="4">
                                            <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Reference Code"
                                                CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>&nbsp;
                                            &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp; <a class="elink2"
                                                href="../../../Excel/Templates/SecondarySales-SB.xlsx">Download Template</a>
                                            <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                                            <asp:HyperLink ID="hlnkDuplicateNotInUse" runat="server" CssClass="elink3"></asp:HyperLink>
                                            <asp:HyperLink ID="hlnkBlankNotInUse" runat="server" CssClass="elink3"></asp:HyperLink>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                    <asp:Panel ID="pnlGrid" Visible="false" runat="server">
                        <asp:UpdatePanel ID="updGrid" runat="server" UpdateMode="Conditional">
                            <contenttemplate>
                                <tr>
                                 <td align="left" valign="top" class="tableposition" runat="server" id="dvUploadPreview" visible="false">
                                        <div class="mainheading_rpt">
                                            <div class="mainheading_rpt_left">
                                            </div>
                                            <div class="mainheading_rpt_mid">
                                              Upload Preview</div>
                                            <div class="mainheading_rpt_right">
                                            </div>
                                        </div>
                                        
                                              <div class="float-right" style="width: 240px;">
                                       <div class="float-margin"> <asp:Label ID="lblTotal" runat="server" Visible="false" Text="" Font-Bold="true"></asp:Label></div>
                                        <div class="float-margin">
                                     <asp:Button ID="btnSave1" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="Save"
                                                CausesValidation="true" OnClick="Btnsave_Click" /></div>
                                             <div class="float-left"> <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="false"
                                                OnClick="BtnCancel_Click" /></div>
                                                <div class="clear"></div>
                                                </div>
                                        
                                    </td>
                                
                                
                                
                               
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="tableposition">
                                        <div class="contentbox">
                                            <div class="grid2">
                                                <asp:GridView ID="GridGRN" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                    CellSpacing="1" DataKeyNames="SKUCode" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                                                    GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                                                    AlternatingRowStyle-CssClass="gridrow1" Width="100%" AllowPaging="false" OnPageIndexChanging="GridGRN_PageIndexChanging">
                                                    <RowStyle CssClass="gridrow" />
                                                    <Columns>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="skuCode"
                                                            HeaderText="Sku Code">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Quantity"
                                                            HeaderText="Quantity">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="gridheader" />
                                                    <EditRowStyle CssClass="editrow" />
                                                    <AlternatingRowStyle CssClass="gridrow1" />
                                                    <PagerStyle CssClass="PagerStyle" />
                                                </asp:GridView>
                                                <div style="float: left; padding-top: 10px; width: 300px;">
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </contenttemplate>
                        </asp:UpdatePanel>
                        <tr>
                            <td align="left" height="5">
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Button ID="Btnsave" runat="server" CssClass="buttonbg" Text="Save" ValidationGroup="Add"
                                    OnClick="Btnsave_Click" Visible="false" />
                                <asp:Button ID="BtnCancel" runat="server" CssClass="buttonbg" Text="Cancel" OnClick="BtnCancel_Click" Visible="false" />
                            </td>
                        </tr>
                    </asp:Panel>
                    <tr>
                        <td align="left" height="10">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
