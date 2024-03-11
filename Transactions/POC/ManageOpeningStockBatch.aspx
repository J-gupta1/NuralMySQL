<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageOpeningStockBatch.aspx.cs"
    Inherits="Transactions_Common_ManageOpeningStockBatch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <title>
        <%=(String)GetGlobalResourceObject("Messages", "PageTitleText")%></title>
    <meta charset="utf-8">
    <meta http-equiv="content-type" content="text/html; charset=UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">

    <link rel="stylesheet" id="cssStyle" runat="server" type="text/css" />
    <link rel="stylesheet" id="cssBootstrap" runat="server" type="text/css" />

    <%--<link rel="stylesheet" id="csswithoutmaster" runat="server" type="text/css" />--%>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="Scriptmanager1" runat="server">
        </asp:ScriptManager>
        <div id="wrapper">
            <!--Wrapper Start-->
            <div id="container">
                <header>
                    <div class="container">
                        <div class="row">
                            <div class="col-md-3 col-sm-3 col-xs-12">
                                <div class="logo">
                                    <asp:HyperLink ID="hyplogo" CausesValidation="false" Width="188" Height="50" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-6">
                                <div class="welcometext">
                                    Welcome
                            <asp:Label ID="lblUserNameDesc" CssClass="logintime" Visible="true" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-3 col-sm-3 col-xs-6">
                                <div class="zedsalestrack">
                                    <div class="right-header">
                                        <asp:Image runat="server" ID="ImgSideLogo" alt="Zed-Sales Track" title="Zed-Sales Track"
                                            border="0" />
                                    </div>
                                </div>
                            </div>
                            <%-- <div class="toplinks">
                            <a href='<%=strSiteUrl%>Logout.aspx' class="elink6">Logout</a></div>--%>
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="heading2">
                        <div class="container">
                            <div class="hd1">
                            </div>
                        </div>
                    </div>
                </header>
                <article>
                    <div class="container">
                        <div class="content-wrapper">
                            <div class="bodycontent">
                                <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <uc1:ucMessage ID="ucMessage1" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="mainheading">
                                    Opening Stock
                                </div>
                                <div class="contentbox">
                                     <div class="mandatory">
                                        (*) Marked fields are mandatory            
                                    </div>
                                    <div class="H25-C3-S">
                                        <ul>
                                            <%-- <td width="15%" align="right" class="formtext" valign="top">
                                        Sales Channel Type: <font class="error">*</font>
                                    </td>
                                    <td width="20%" align="left" class="formtext" valign="top">
                                       <div style="float:left; width:135px;"> <asp:DropDownList ID="cmbChannelType" runat="server" 
                                            CssClass="form_select" AutoPostBack="True" 
                                            onselectedindexchanged="cmbChannelType_SelectedIndexChanged" >
                                        </asp:DropDownList></div>
                                        <br />
                                        <div style="float:left; width:200px;"><asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="cmbChannelType"
                                            CssClass="error" ValidationGroup="Save" InitialValue="0" runat="server" ErrorMessage="Please select a Sales Channel Type "></asp:RequiredFieldValidator></div>
                                    </td>--%>
                                            <%-- <td width="15%" height="35" align="right" class="formtext" valign="top">
                                        Sales Channel: <font class="error">*</font>
                                    </td>
                                    <td width="18%" align="left" class="formtext" valign="top">
                                        <div style="float:left; width:135px;">  <asp:DropDownList ID="cmbSalesChannel" runat="server" 
                                            CssClass="form_select" AutoPostBack="True">
                                        </asp:DropDownList></div>
                                        <br />
                                       <div style="float:left; width:180px;"> <asp:RequiredFieldValidator ID="RequCombo" ControlToValidate="cmbSalesChannel" CssClass="error"
                                            ValidationGroup="Save" InitialValue="0" runat="server" ErrorMessage="Please select a Sales Channel "></asp:RequiredFieldValidator></div>
                                    </td>
                                            --%>
                                            <li class="text">
                                                <asp:Label ID="lblOSDate" runat="server" AssociatedControlID="ucDatePicker" CssClass="formtext">Opening Stock Date:<span class="error">*</span></asp:Label>
                                            </li>
                                            <li class="field">
                                                <uc2:ucDatePicker ID="ucDatePicker" runat="server" RangeErrorMessage="Date should be less then equal to current date."
                                                    ValidationGroup="Save" />
                                            </li>
                                        </ul>
                                        <%-- <td  align="left" valign="top" >
                                        <asp:Button ID="BtnSubmit" runat="server" Text="&nbsp;Go&nbsp;" CssClass="buttonbg"
                                            CausesValidation="true" ValidationGroup="Entry" OnClick="BtnSubmit_Click" />
                                    </td>--%>
                                        <ul>
                                            <li class="text">Upload File: <span class="error">*</span>
                                            </li>
                                            <li class="field">
                                                <asp:FileUpload ID="FileUpload1" CssClass="fileuploads" runat="server" />
                                            </li>
                                            <li class="field3">
                                                <asp:Button ID="btnUpload" CssClass="buttonbg" runat="server" Text="Upload" TabIndex="11"
                                                    CausesValidation="false" OnClick="btnUpload_Click" />
                                            </li>
                                            <li class="link">
                                                <asp:LinkButton ID="DwnldReferenceCodeTemplate" runat="server" Text="Download SKU Code"
                                                    CausesValidation="false" CssClass="elink2" OnClick="DwnldReferenceCodeTemplate_Click"></asp:LinkButton>
                                            </li>
                                            <li class="link">
                                                <a class="elink2" href="../../Excel/Templates/BatchWiseStock.xlsx">Download Template</a>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <asp:Panel ID="pnlGrid" runat="server">
                                    <div id="tblGrid">
                                        <div class="mainheading">
                                            Enter Opening Stock Details
                                        </div>
                                        <div class="contentbox">
                                            <div class="grid1">
                                                <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvStockEntry" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                            CellSpacing="1" DataKeyNames="SKUCode" EditRowStyle-CssClass="editrow" EmptyDataText="No Record Found"
                                                            GridLines="None" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="gridrow"
                                                            AlternatingRowStyle-CssClass="Altrow" Width="100%">
                                                            <RowStyle CssClass="gridrow" />
                                                            <Columns>
                                                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BatchNumber"
                                                                    HeaderText="Batch Number"></asp:BoundField>
                                                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SKUCode"
                                                                    HeaderText="SKU Code"></asp:BoundField>
                                                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Quantity"
                                                                    HeaderText="Quantity"></asp:BoundField>
                                                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Error"
                                                                    HeaderText="Error"></asp:BoundField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="gridheader" />
                                                            <EditRowStyle CssClass="editrow" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="float-margin">
                                            <asp:Button ID="btnSave" CssClass="buttonbg" runat="server" Text="Save" ValidationGroup="Save"
                                                CausesValidation="true" OnClick="btnSave_Click" />
                                        </div>
                                        <div class="float-margin">
                                            <asp:Button ID="btnReset" runat="server" Text="Cancel" CssClass="buttonbg" CausesValidation="false"
                                                OnClick="btnReset_Click" />
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </article>
            </div>
            <footer>
                <div class="container">
                    <!--footer Start-->
                    <div class="copyright">
                        &copy; Copyright 2018 Zed-Axis Technologies
                    </div>
                    <div style="display: none;">
                        <asp:HyperLink ID="hypfooterlogo" CausesValidation="false" runat="server" />
                    </div>
                </div>
            </footer>
        </div>
    </form>
</body>
</html>
