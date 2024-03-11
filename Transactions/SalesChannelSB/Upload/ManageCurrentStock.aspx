<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="ManageCurrentStock.aspx.cs"
    Inherits="ManageCurrentStock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <meta charset="utf-8">
    <meta http-equiv="content-type" content="text/html; charset=UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <%-- Add END --%>
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/bootstrap.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/style.css") %>" />
    <%--<link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/withoutmaster.css") %>" />--%>
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/Menu.css") %>" />
    <script src="../../Assets/Jscript/bootstrap.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
        <%--<asp:ScriptManager ID="Scriptmanager1" runat="server">
        </asp:ScriptManager>--%>
        <div id="wrapper">
            <!--Wrapper Start-->
            <div id="container">
                
                <article>
                    <div class="container">
                        <div class="content-wrapper">
                            <div class="bodycontent">
                                <uc1:ucMessage ID="ucMessage1" runat="server" />
                               
                                
                               
                                <div class="mainheading">
                                    Upload Current Stock
                                </div>
                                <div class="contentbox">
                                    <div class="mandatory">
                                        (*) Marked fields are mandatory
                                    </div>
                                    <div class="H25-C3-S">
                                        
                                        <ul>
                                            <li class="text">
                                                <asp:Label ID="lblOSDate" runat="server" AssociatedControlID="ucDatePicker" CssClass="formtext">Stock Date:<span class="error">*</span></asp:Label>
                                            </li>
                                            <li class="field">
                                                <uc2:ucDatePicker ID="ucDatePicker" runat="server" RangeErrorMessage="Date should be less then equal to current date."
                                                    ValidationGroup="Save" ErrorMessage="Invalid Date!" IsRequired="true" />
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="text">Upload File: <span class="error">*</span>
                                            </li>
                                            <li class="field">
                                                <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                                            </li>
                                            <li class="field3">
                                                <div>
                                                    <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                                                        CausesValidation="false" OnClick="btnUpload_Click" />
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="formlink">
                                        <ul>
                                            <li>
                                                <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download Stock"
                                                    CausesValidation="false" CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>
                                            </li>
                                            <%--<li>
                                                <a class="elink2" href="../../Excel/Templates/OpeningStockSerialWise.xlsx"
                                                    runat="server" id="hyWithOutValidation" visible="true">Download Template</a>
                                            </li>--%>
                                            <li>
                                                <asp:HyperLink ID="hlnkInvalid" runat="server" CssClass="elink3" Visible="true"></asp:HyperLink>
                                            </li>
                                            
                                        </ul>
                                    </div>
                                </div>
                                <asp:Panel ID="pnlGrid" runat="server">
                                    <div id="tblGrid">
                                        <div class="mainheading">
                                            Current Stock Details
                                        </div>
                                        <div class="float-right" style="width: 240px;">
                                            <div class="float-margin">
                                                <asp:Label ID="lblTotal" runat="server" Visible="false" Text="" Font-Bold="true"></asp:Label>
                                            </div>
                                           
                                            <div class="float-left">
                                                <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="false"
                                                    OnClick="btnReset_Click" />
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <div class="contentbox">
                                            <div class="grid1">
                                                
                                                <asp:GridView ID="gvStockEntry" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                    CellSpacing="1" DataKeyNames="SKUCode" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                                                    GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                                                    AlternatingRowStyle-CssClass="Altrow" Width="100%">
                                                    <RowStyle CssClass="gridrow" />
                                                    <Columns>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUCode"
                                                            HeaderText="SKU Code"></asp:BoundField>
                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Quantity"
                                                            HeaderText="Quantity"></asp:BoundField>
                                                        <%--   <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Serial#1"
                                                                        HeaderText="Serial Number1"></asp:BoundField>
                                                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Serial#2"
                                                                        HeaderText="Serial Number2"></asp:BoundField>
                                                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Serial#3"
                                                                        HeaderText="Serial Number3"></asp:BoundField>
                                                                          <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Serial#4"
                                                                        HeaderText="Serial Number4"></asp:BoundField>
                                                                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BatchNo"
                                                                        HeaderText="BatchNo"></asp:BoundField>
                                                                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Error"
                                                                        HeaderText="Error"></asp:BoundField>--%>
                                                    </Columns>
                                                    <HeaderStyle CssClass="gridheader" />
                                                    <EditRowStyle CssClass="editrow" />
                                                </asp:GridView>
                                                <%--       </contenttemplate>
                                                    </asp:UpdatePanel>--%>
                                            </div>
                                        </div>
                                        <div class="margin-bottom">
                                            <%--<asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="Save"
                                                CausesValidation="true" OnClick="btnSave_Click" Visible="false" />
                                            <asp:Button ID="btnReset" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="false"
                                                OnClick="btnReset_Click" Visible="false" />--%>
                                        </div>
                                    </div>
                                    <%-- #CC02 COMMENTED </ContentTemplate>
                                </asp:UpdatePanel>--%>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </article>
            </div>
            
        </div>
</asp:Content>
