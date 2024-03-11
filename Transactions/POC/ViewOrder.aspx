<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewOrder.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master"
    Inherits="Transactions_POC_ViewOrder" EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/dhtmlwindow.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/modal.css") %>" />

    <script type="text/javascript" src="../../Assets/Jscript/dhtmlwindow.js"></script>

    <script type="text/javascript" src="../../Assets/Jscript/modal.js"></script>

    <script type="text/javascript">
  

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel ID="updmsg" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        View&nbsp; Secondary Orders
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="mandatory">
                    (*) Marked fields are mandatory. (+) Marked fields are optional.
                </div>
                <div class="H25-C3-S">
                    
                    <%--<tr>--%>


                    <%--<td width="15%" align="right" class="formtext" valign="top">
                                Sales Channel Type: <font class="error">*</font>
                            </td>
                            <td width="20%" align="left" class="formtext" valign="top">
                                <div style="float: left; width: 135px;">
                                    <asp:DropDownList ID="cmbChannelType" runat="server" CssClass="form_select" AutoPostBack="True"
                                        OnSelectedIndexChanged="cmbChannelType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div style="float: left; width: 180px;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="cmbChannelType"
                                        CssClass="error" ValidationGroup="Entry" InitialValue="0" runat="server" ErrorMessage="Please select a Sales Channel Type "></asp:RequiredFieldValidator></div>
                            </td>--%>
                    <%--<td width="15%" align="right" class="formtext" valign="top">
                                Sales Channel Name: <font class="error">*</font>
                            </td>
                            <td width="20%" align="left" class="formtext" valign="top">
                                <div style="float: left; width: 135px;">
                                    <asp:DropDownList ID="ddlSalesChannelName" runat="server" CssClass="form_select"
                                        AutoPostBack="False">
                                    </asp:DropDownList>
                                    <br />--%>
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlSalesChannelName"
                                        CssClass="error" ValidationGroup="Entry" InitialValue="0" runat="server" ErrorMessage="Please select a Sales Channel Name "></asp:RequiredFieldValidator>--%>
                    <%--  <br />
                                </div>
                                <div style="float: left; width: 180px;">
                                </div>
                            </td>
                            <td width="15%" align="left" class="formtext" valign="top">
                            </td>
                            <td align="left" class="formtext" valign="top">
                            </td>--%>
                    <%-- </tr>--%>
                    <ul>
                        <li class="text">Retailer Code: <span class="error">*</span></li>
                        <li class="field">
                            <div>
                                <asp:TextBox ID="txtRetailerCode" runat="server" CssClass="formselect">
                                </asp:TextBox>
                            </div>
                            <%--<div style="float: left; width: 180px;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtRetailerCode"
                                        CssClass="error" ValidationGroup="Entry1"  runat="server" ErrorMessage="Retailer Code is Required "></asp:RequiredFieldValidator></div>--%></li>
                        <%--<td width="15%" align="right" class="formtext" valign="top">
                                Sales Channel Type: <font class="error">*</font>
                            </td>
                            <td width="20%" align="left" class="formtext" valign="top">
                                <div style="float: left; width: 135px;">
                                    <asp:DropDownList ID="cmbChannelType" runat="server" CssClass="form_select" AutoPostBack="True"
                                        OnSelectedIndexChanged="cmbChannelType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <br />
                                </div>
                                <div style="float: left; width: 180px;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="cmbChannelType"
                                        CssClass="error" ValidationGroup="Entry" InitialValue="0" runat="server" ErrorMessage="Please select a Sales Channel Type "></asp:RequiredFieldValidator></div>
                            </td>--%><%--<td width="15%" align="right" class="formtext" valign="top">
                                Sales Channel Name: <font class="error">*</font>
                            </td>
                            <td width="20%" align="left" class="formtext" valign="top">
                                <div style="float: left; width: 135px;">
                                    <asp:DropDownList ID="ddlSalesChannelName" runat="server" CssClass="form_select"
                                        AutoPostBack="False">
                                    </asp:DropDownList>
                                    <br />--%><%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlSalesChannelName"
                                        CssClass="error" ValidationGroup="Entry" InitialValue="0" runat="server" ErrorMessage="Please select a Sales Channel Name "></asp:RequiredFieldValidator>--%><%--  <br />
                                </div>
                                <div style="float: left; width: 180px;">
                                </div>
                            </td>
                            <td width="15%" align="left" class="formtext" valign="top">
                            </td>
                            <td align="left" class="formtext" valign="top">
                            </td>--%>
                    </ul>
                    <ul>
                        <li class="text">Order From: <span class="error">+</span></li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateFrom" runat="server" defaultDateRange="true" ErrorMessage="Invalid date." RangeErrorMessage="Date should be less then equal to current date." ValidationGroup="save1" />
                            </td>
                            <li class="text">Order To: <span class="error">+</span></li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateTo" runat="server" defaultDateRange="true" ErrorMessage="Invalid date." RangeErrorMessage="Date should be less then equal to current date." ValidationGroup="save1" />
                        </li>
                        <%--<td align="left" class="formtext" valign="top">
                                Status:<font class="error">+</font>
                            </td>
                            <td align="left" class="formtext" valign="top">
                                <div style="float: left; width: 135px;">
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form_select" AutoPostBack="false">
                                        <asp:ListItem Text="Select" Value="0">
                                        </asp:ListItem>
                                        <asp:ListItem Text="Approved" Value="2">
                                        </asp:ListItem>
                                        <asp:ListItem Text="Pending" Value="1">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                </div>
                            </td>--%>
                    </ul>
                    <ul>
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="BtnSearch" runat="server" CausesValidation="true" CssClass="buttonbg" OnClick="BtnSearch_Click" Text="Search" ValidationGroup="Entry" />
                            </div>
                            <div class="float-left">
                                <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="buttonbg" OnClick="btnCancel_Click" Text="Cancel" ValidationGroup="Entry" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--<asp:Panel ID="pnlGrid" runat="server">  </asp:Panel>--%>

    <%--   <div id="dvheading" runat="server" visible="false">
                <div class="mainheading">
                   Orders List
                </div>
             </div>--%>

    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="dvheading" runat="server" visible="false">
                <div class="mainheading">
                    Orders List
                </div>
            </div>
            <div id="dvAction" runat="server" visible="false">
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="gvOrder" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            CellSpacing="1" DataKeyNames="SecondaryOrderId" EditRowStyle-CssClass="editrow"
                            EmptyDataText="No Record Found" GridLines="None" HeaderStyle-CssClass="gridheader"
                            RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" Width="100%"
                            OnRowDataBound="gvOrder_RowDataBound" OnRowCommand="gvOrder_RowCommand">
                            <RowStyle CssClass="gridrow" />
                            <Columns>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderNumber"
                                    HeaderText="OrderNumber"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderDateDisplay"
                                    HeaderText="OrderDate"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderFrom"
                                    HeaderText="OrderFrom"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderTo"
                                    HeaderText="OrderTo"></asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDetailID" runat="server" Text='<%# (DataBinder.Eval(Container.DataItem,"SecondaryOrderId"))%>'
                                                        Visible="false"></asp:Label>
                                                    <%--<asp:ImageButton ID="imgOnline" runat="server" CausesValidation="false" CommandArgument='<%#Eval("PrimaryOrderDetailID") %>'
                                            ImageAlign="Top" CommandName="Online" ToolTip="" ImageUrl="" />--%>
                                                    <asp:Button ID="btnDetails" runat="server" Text="Details" CssClass="buttonbg" CausesValidation="false"
                                                        CommandArgument='<%#Eval("SecondaryOrderId") %>' CommandName="Details" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="gridheader" />
                            <EditRowStyle CssClass="editrow" />
                        </asp:GridView>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updDetail" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div runat="server" id="dvDetail" visible="false">
                <div id="tblGrid">
                    <div class="mainheading">
                        View Secondary Order Detail
                    </div>
                    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="contentbox">
                                <div class="grid1">
                                    <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        CellSpacing="1" DataKeyNames="OrderDetailID" EditRowStyle-CssClass="editrow"
                                        EmptyDataText="No Record Found" GridLines="None" HeaderStyle-CssClass="gridheader"
                                        RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" Width="100%"
                                        OnRowDataBound="gvDetail_RowDataBound">
                                        <RowStyle CssClass="gridrow" />
                                        <Columns>
                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SkuCode"
                                                HeaderText="Sku Code"></asp:BoundField>
                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SkuName"
                                                HeaderText="Sku Name"></asp:BoundField>
                                            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="OrderedQuantity"
                                                HeaderText="Ordered Quantity"></asp:BoundField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="OrderQuantity"
                                                Visible="false" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderedQuantity" runat="server" Text='<%# Eval("OrderedQuantity") %>'
                                                        Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Approved Quantity"
                                                ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSKUID" runat="server" Text='<%# Eval("SKUID") %>' Visible="false"></asp:Label>
                                                    <asp:TextBox ID="txtApprovedQuantity" runat="server" MaxLength="8" Text='<%# Eval("ApproveQuantity") %>'
                                                        CssClass="formfields-W70" ValidationGroup="save"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftbeOs" runat="server" FilterType="Custom" ValidChars="-0123456789"
                                                        TargetControlID="txtApprovedQuantity" />
                                                    <asp:RangeValidator ID="valReceQty" runat="server" ControlToValidate="txtApprovedQuantity"
                                                        ErrorMessage="Value can not be greater than Order Quantity" Display="Dynamic"
                                                        Type="Integer" ValidationGroup="save"></asp:RangeValidator>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="gridheader" />
                                        <EditRowStyle CssClass="editrow" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <div class="margin-bottom">
                        <div class="float-margin">
                            <asp:Button ID="btnApprove" CssClass="buttonbg" runat="server" Text="Approve" ValidationGroup="save"
                                CausesValidation="true" OnClick="btnApprove_Click" />
                        </div>
                        <div class="float-margin">
                            <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="buttonbg" CausesValidation="false"
                                OnClick="btnReject_Click" />
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
