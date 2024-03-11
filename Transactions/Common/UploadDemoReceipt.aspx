<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="UploadDemoReceipt.aspx.cs" Inherits="Transactions_Common_UploadDemoReceipt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div id="wrapper">
        <div align="left">
            <div id="dvError" runat="server" style="display: none">
                <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>
                <uc1:ucMessage ID="ucMessage1" runat="server" />
            </div>
            <div class="float-margin">
                <br />
                <asp:Label runat="server" CssClass="error" ID="lblMessage" Text="" Style="display: none"></asp:Label>
            </div>
            <table cellspacing="0" cellpadding="0" width="965" border="0" style="float: left;">
                <tr>
                    <td align="left" valign="top"></td>
                    <td valign="top" align="left" class="tableposition">
                        <div class="contentbox">
                            <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                <tr>
                                    <td colspan="6" height="20" align="right" class="mandatory" valign="top">(*) marked fields are mandatory.
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formtext" valign="top" align="left" height="35">
                                        <asp:Label ID="lblRetailerCode" CssClass="formtext" runat="server">Retailer Code:<font class="error">*</font></asp:Label>
                                    </td>

                                    <td valign="top" align="left" width="20%">
                                        <asp:TextBox ID="txtRetailorCode" runat="server"></asp:TextBox></td>
                                    <td align="right" class="formtext" valign="top" width="15%"></td>
                                    <td align="left" valign="top" width="20%"></td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:Label ID="lblToDate" runat="server" CssClass="formtext">Upload:<font class="error">*</font></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Panel ID="pnlParent" runat="server" Width="100%">
                                            <div class="float-margin">
                                                <asp:Panel ID="pnlFiles" runat="server" HorizontalAlign="Left">
                                                    <asp:FileUpload ID="IpFile" runat="server" CssClass="fileuploads" Width="300px" />
                                                </asp:Panel>
                                                <div class="gridrow_black">
                                                    File size should not exceed
                                                    <asp:Label ID="lblSizeDisplay" runat="server"></asp:Label>
                                                    . Only .JPG, .JPEG, .BMP, .GIF, .PNG, .PDF, .DOC, .DOCX files are allowed
                                                </div>
                                            </div>
                                            <div class="clear" style="padding-bottom: 10px;"></div>
                                            <div class="clear" style="padding-bottom: 10px;">
                                                <asp:Panel ID="pnlButton" runat="server" HorizontalAlign="left">
                                                    <asp:Button ID="btnUpload" runat="server" class="button1" OnClick="btnUpload_Click" Text="Save" Width="60px" />

                                                    <br />
                                                </asp:Panel>
                                                <asp:HiddenField ID="hdnFinalList" runat="server" />
                                            </div>
                                        </asp:Panel>
                                    </td>
                                    <td align="left" class="formtext" valign="top">

                                    </td>
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
        </div>
    </div>
    <div></div>
    <div class="contentbox">
                                    <div class="grid2">
                                      <%--  <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                                            <ContentTemplate>--%>
                                                <asp:GridView ID="grdDemoReceiptList"   runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                                                    RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                                                    HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                                                    AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                                                    HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                                                    BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                                                    PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected" DataKeyNames="DemoReceiptId">
                                                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                                                    <Columns>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="RetailerCode"
                                                            HeaderText="Retailer Code"></asp:BoundField>
                                                        
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ImageName"
                                                            HeaderText="File Name"></asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDownload" Text = "Download" CommandArgument = '<%# Eval("ImagePath") %>' runat="server" OnClick = "DownloadFile"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            </asp:TemplateField>
                                                    </Columns>
                                                    <PagerStyle CssClass="PagerStyle" />
                                                </asp:GridView>
                                          <%--  </ContentTemplate>
                                           
                                        </asp:UpdatePanel>--%>
                                    </div>
                                </div>
</asp:Content>

