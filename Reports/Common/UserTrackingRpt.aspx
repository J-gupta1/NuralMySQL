<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UserTrackingRpt.aspx.cs" Inherits="Reports_Common_UserTrackingRpt" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Search User Tracking
    </div>
    <div class="contentbox">
       <%-- <asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
                <div class="mandatory">
                    (*) Marked fields are mandatory            
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblrole" runat="server" Text="">Role:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbRoleType" CssClass="formselect" runat="server" OnSelectedIndexChanged="ddrSerRole_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="valpricename" ControlToValidate="cmbRoleType"
                                    CssClass="error" ErrorMessage="Please select a role Type  " InitialValue="0"
                                    ValidationGroup="insert" />
                            </div>
                        </li>
                        <li class="text">
                            <asp:Label ID="lbseruser" runat="server" Text="">User Name:</asp:Label>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="cmbUsername" runat="server" CssClass="formselect">
                                </asp:DropDownList>
                            </div>                            
                                <%-- #CC02 START COMMENTED  <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="cmbUsername"
                                                                                CssClass="error" ErrorMessage="Please select a user   " InitialValue="0"
                                                                                ValidationGroup="insert" /></div> #CC02 END COMMENTED  --%>
                        </li>
                        <li class="text">
                            <asp:Label ID="lblserfrmDate" runat="server" Text="">From Date: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDatePicker1" runat="server" ErrorMessage="Invalid from date."
                                ValidationGroup="insert" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                        </li>
                        <li class="text">
                            <asp:Label ID="lblsertodate" runat="server" Text="">To Date:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDatePicker2" runat="server" ErrorMessage="Invalid to  date."
                                ValidationGroup="insert" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                        </li>
                        <li class="text"></li>
                        <li class="field">
                            <asp:HiddenField ID="hfSearch" Value="0" Visible="false" runat="server" />
                            <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerch_Click"
                                ValidationGroup="insert" CssClass="buttonbg" CausesValidation="True" />
                            <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                CssClass="buttonbg" CausesValidation="False" />
                        </li>
                    </ul>
                </div>
          <%--  </ContentTemplate>--%>
            <%-- <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdvwUserList" EventName="DataBound" />
                                                </Triggers>--%>
        <%--</asp:UpdatePanel>--%>
    </div>
   <%-- <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
        <ContentTemplate>--%>
            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                <div class="mainheading">
                    List
                </div>
                <div class="export">
                    <asp:Button ID="btnExportToExcel" Text="" runat="server" OnClick="exportToExel_Click"
                        CssClass="excel" />
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="grdUserTrackRpt" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                            BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="RoleID" FooterStyle-HorizontalAlign="Left"
                            FooterStyle-VerticalAlign="Top" GridLines="none" HeaderStyle-HorizontalAlign="left"
                            HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left" AlternatingRowStyle-CssClass="Altrow"
                            RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter" HeaderStyle-CssClass="gridheader"
                            RowStyle-VerticalAlign="top" Width="100%" OnPageIndexChanging="grdArea_PageIndexChanging">
                            <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                            <Columns>
                                <asp:BoundField DataField="RoleName" HeaderText="Role" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UserName" HeaderText="User Name" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MenuName" HeaderText="Menu Name" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="User Activity Date">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lbluseraffdate" runat="server" Text='<%# Eval("UserActivityDate","{0:dd-MMM-yy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Login On">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lbllastlogindate" runat="server" Text='<%# Eval("LastLoginOn","{0:dd-MMM-yy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="UserIP" HeaderText="User IP" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ServerIP" HeaderText="Server IP" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
       <%-- </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>
