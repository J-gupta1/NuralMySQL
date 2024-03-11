<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ViewSerialNumberMovement.aspx.cs" Inherits="Reports_Inventory_ViewSerialNumberMovement" %>

<%@ Import Namespace="BussinessLogic" %>
<%@ Register Src="../../UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:HiddenField ID="hdncmd" runat="server" />
    <div>
        <uc1:ucMessage ID="ucMsg" runat="server" />
    </div>
    <div>
        <div class="mainheading">
            Search Panel
        </div>
        <div class="contentbox">
            <div class="mandatory">
                (*) Marked fields are mandatory            
            </div>
            <div class="H35-C3-S">
                <ul>
                    <li class="text">Enter Serial Numbers <span class="error">*</span>
                    </li>
                    <li class="field" style="height:auto;">
                        <div>
                            <asp:TextBox ID="txtSerialNumber" runat="server" CausesValidation="True" CssClass="form_textarea" Height="80px"
                                ValidationGroup="aa" TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="error" Display="Dynamic"
                                ErrorMessage="Enter Serial Number" ControlToValidate="txtSerialNumber" ValidationGroup="aa"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                        <%--#CC01 START ADDED--%>
                        <div class="error">
                            <asp:Label ID="lblIMEItrackingMsg" runat="server"></asp:Label>
                        </div>
                        <%--#CC01 END ADDED--%>
                    </li>
                    <li class="text">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg" ValidationGroup="aa"
                            OnClick="btnSearch_Click" />
                    </li>
                    <li class="field">
                        <div class="float-right">
                            <asp:Button ID="Exporttoexcel" runat="server" CausesValidation="True" CssClass="excel"
                                OnClick="Exporttoexcel_Click" Text="" ToolTip="Export to Excel" ValidationGroup="aa"
                                Visible="false" />
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <%--<uc1:ucMessage ID="ucMsg" runat="server" />--%>
    </div>
    <div id="tblCurrentOwner" runat="server">
        <div class="mainheading">
            Current Owner
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:GridView ID="gvCurrentOwner" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                    AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                    FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                    GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                    RowStyle-VerticalAlign="top" Width="100%" OnRowCommand="gvCurrentOwner_RowCommand"
                    OnPageIndexChanging="gvCurrentOwner_PageIndexChanging">
                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                    <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                    <Columns>
                        <asp:BoundField DataField="SerialNumber" HeaderStyle-HorizontalAlign="Left" HeaderText="Serial Number"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SerialNumber2" HeaderStyle-HorizontalAlign="Left" HeaderText="Serial Number2"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SkuCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Code"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SKUName" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Name"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <%--#CC02 Add Start --%>
                        <asp:BoundField DataField="CartonNo" HeaderStyle-HorizontalAlign="Left" HeaderText="Carton No."
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SWVersion" HeaderStyle-HorizontalAlign="Left" HeaderText="Software Version."
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <%--#CC02 Add End --%>

                        <asp:BoundField DataField="SalesChannel" HeaderStyle-HorizontalAlign="Left" HeaderText="Sales Channel"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ChannelType" HeaderStyle-HorizontalAlign="Left" HeaderText="Type"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SerialStatus" HeaderStyle-HorizontalAlign="Left" HeaderText="Status"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ModifiedOn" HeaderStyle-HorizontalAlign="Left" HeaderText="Last Updated On"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="View Transactions" ItemStyle-Width="85px">
                            <ItemStyle Wrap="False" />
                            <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnViewTransactions" CssClass="elink2" runat="server" CommandName="ViewTransactions"
                                    CommandArgument='<%#Eval("SerialMasterID") %>'>View Transactions</asp:LinkButton>
                                <%--<asp:ImageButton ID="imgStatus" runat="server" CausesValidation="false" CommandArgument='<%#Eval("SMSOutboundTransID") %>'
                                            CommandName="togglecmd" ImageAlign="Top" ImageUrl='<%#PageBase.ImageChange(Convert.ToInt16(Eval("Status"))) %>'
                                            ToolTip='<%#PageBase.ToolTipeChange(Convert.ToInt16(Eval("Status"))) %>' />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                    <AlternatingRowStyle CssClass="Altrow" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <div id="tblTransactions" runat="server">
        <div class="mainheading">
            Transactions
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:GridView ID="gvTransactions" runat="server" AllowPaging="True" AlternatingRowStyle-CssClass="Altrow"
                    AutoGenerateColumns="false" BorderWidth="0px" CellPadding="4" CellSpacing="1"
                    FooterStyle-CssClass="gridfooter" FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top"
                    GridLines="none" HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                    RowStyle-VerticalAlign="top" Width="100%" PageSize="50">
                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                    <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                    <Columns>
                        <asp:BoundField DataField="TransType" HeaderStyle-HorizontalAlign="Left" HeaderText="Transaction Type"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FromChannel" HeaderStyle-HorizontalAlign="Left" HeaderText="From Channel"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FromChannelType" HeaderStyle-HorizontalAlign="Left" HeaderText="From Channel Type"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ToChannel" HeaderStyle-HorizontalAlign="Left" HeaderText="To Channel"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ToChannelType" HeaderStyle-HorizontalAlign="Left" HeaderText="To Channel Type"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TransDate" HeaderStyle-HorizontalAlign="Left" HeaderText="Transaction Date"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SKUCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SKU Code"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RefDocNo" HeaderStyle-HorizontalAlign="Left" HeaderText="Refrence Document No"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CreatedOn" HeaderStyle-HorizontalAlign="Left" HeaderText="Created On"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <%-- <asp:TemplateField HeaderText="View Transactions" ItemStyle-Width="85px">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle HorizontalAlign="left" Width="85px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server">View Transactions</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                    </Columns>
                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                    <AlternatingRowStyle CssClass="Altrow" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
