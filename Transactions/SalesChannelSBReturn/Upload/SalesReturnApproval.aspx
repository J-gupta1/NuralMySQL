<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master"
    CodeFile="SalesReturnApproval.aspx.cs" Inherits="SalesReturnApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="ucDate" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMessage1" runat="server" />
    <div class="mainheading">
        Sales Return Request
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory
        </div>
        <div class="H25-C3-S">
            <ul>
                <li class="text">Sales Channel Name: </li>
                <li class="field">
                    <asp:TextBox ID="txtSalesChannel" runat="server" CssClass="formfields" MaxLength="70"></asp:TextBox>
                </li>
                <li class="text">Sales Channel Code: </li>
                <li class="field">
                    <asp:TextBox ID="txtSCCode" runat="server" CssClass="formfields" MaxLength="30"></asp:TextBox>
                </li>
                <li class="text">Request Status: </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="cmbRequestStatus" runat="server" CssClass="formselect">
                            <asp:ListItem Text="Select" Value="255"></asp:ListItem>
                            <asp:ListItem Text="My Pending Approval" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Finally Approved" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Finally Reject" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Approved by Level 1" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Approved by Level 2" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </li>
            </ul>
            <ul>

                <li class="text">From Date: </li>
                <li class="field">
                    <ucDate:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="From date required."
                        ValidationGroup="PIS" defaultDateRange="True" RangeErrorMessage="" />
                </li>
                <li class="text">To Date: </li>
                <li class="field">
                    <ucDate:ucDatePicker ID="UcDateTo" runat="server" ErrorMessage="From date required."
                        ValidationGroup="PIS" defaultDateRange="True" RangeErrorMessage="" />
                </li>
                <li class="text">Return Type: </li>
                <li class="field">
                    <div>
                        <asp:DropDownList ID="cmbReturnType" runat="server" CssClass="formselect">
                            <asp:ListItem Text="Select" Value="255"></asp:ListItem>
                            <asp:ListItem Text="Intermediary" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Secondary" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </li>
            </ul>
            <ul>
                <li class="text"></li>
                <li class="field">
                    <div class="float-margin">
                        <asp:Button ID="btnSearch" runat="server" ValidationGroup="Serach" CausesValidation="true"
                            CssClass="buttonbg" Text="Search" OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-margin">
                        <asp:Button ID="btnShowAll" runat="server" CssClass="buttonbg" Text="Show All" ToolTip="Show All"
                            OnClick="btnShowAll_Click" />
                    </div>
                </li>
                <li>
                    <div class="float-margin">
                        <asp:Button ID="ExportToExcel" CssClass="buttonbg" runat="server" Text="Export In Excel"
                            OnClick="ExportToExcel_Click2" />
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div id="dvhide" runat="server" visible="false">
        <div class="mainheading">
            List
        </div>
        <div class="export">
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:GridView ID="GridSRQ" runat="server" AlternatingRowStyle-CssClass="Altrow"
                    AutoGenerateColumns="false" bgcolor="" AllowPaging="true" PageSize="20" BorderWidth="0px"
                    CellPadding="4" CellSpacing="1" DataKeyNames="SalesReturnRequestID" FooterStyle-CssClass="gridfooter"
                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                    RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                    SelectedStyle-CssClass="gridselected" Width="100%" OnPageIndexChanging="GridSRQ_PageIndexChanging">
                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                    <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                    <Columns>

                        <asp:TemplateField HeaderText="Approve">
                            <ItemTemplate>
                                <asp:Button ID="BtnDetail" CommandName="BtnDetail" runat="server" CssClass="buttonbg"
                                    Text="Detail" OnClick="BtnDetail_Click" CommandArgument='<%# Eval("SalesReturnRequestID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="ReturnFrom" HeaderStyle-HorizontalAlign="Left" HeaderText="From"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="REturnFromCode" HeaderStyle-HorizontalAlign="Left" HeaderText="FromCode"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ReturnTo" HeaderStyle-HorizontalAlign="Left" HeaderText="To"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ReturnToCode" HeaderStyle-HorizontalAlign="Left" HeaderText="ToCode"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ReturnRequestNo" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true"
                            HeaderText="Return Request No">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RequestDate" HeaderStyle-HorizontalAlign="Left" HeaderText="Request Date"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TotalQuantity" HeaderStyle-HorizontalAlign="Left" HeaderText="Quantity"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                    </Columns>
                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                    <AlternatingRowStyle CssClass="Altrow" />
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
                <div class="clear">
                </div>
            </div>
            <div id="dvFooter" runat="server" class="pagination">
                <uc3:UCPagingControl ID="ucPagingControl1" runat="server" OnSetControlRefresh="UCPagingControl1_SetControlRefresh" />
            </div>
        </div>
    </div>

    <div id="dvDetail" runat="server" visible="false">
        <ul>
         
            <li class="field">
              <asp:Label ID="lbltxtrequestno" Text="Return Request No:" Font-Bold="true" runat="server"></asp:Label>  <asp:Label ID="lblReturnRequestNo" Font-Bold="true" runat="server"></asp:Label>
            </li>
        </ul>
        <ul>
            <li class="grid1">
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="GridDetail" runat="server" AlternatingRowStyle-CssClass="Altrow"
                            AutoGenerateColumns="false" bgcolor="" AllowPaging="false" BorderWidth="0px"
                            CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                            HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                            RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                            SelectedStyle-CssClass="gridselected" Width="100%">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle CssClass="gridrow" HorizontalAlign="Left" VerticalAlign="Top" />
                            <Columns>

                                <asp:BoundField DataField="ModelCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Model Code"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SKUName" HeaderStyle-HorizontalAlign="Left" HeaderText="SKUName"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SKUCode" HeaderStyle-HorizontalAlign="Left" HeaderText="SKUCode"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Quantity" HeaderStyle-HorizontalAlign="Left" HeaderText="ToCode"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SerialNo" HeaderStyle-HorizontalAlign="Left" HtmlEncode="true"
                                    HeaderText="IMEI No">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Mode" HeaderStyle-HorizontalAlign="Left" HeaderText="Mode"
                                    HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>


                            </Columns>
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" VerticalAlign="Top" />
                            <AlternatingRowStyle CssClass="Altrow" />

                        </asp:GridView>
                        <div class="clear">
                        </div>
                    </div>

                </div>

            </li>
        </ul>
        <ul>
            <li class="text">Remark: </li>
            <li>
                <asp:TextBox ID="txtApprovalRemark" runat="server" TextMode="MultiLine" MaxLength="5"></asp:TextBox>
            </li>
            <li>
                <div class="margin-bottom">
                    <div class="float-margin">
                        <asp:Button ID="btnAccept" CssClass="buttonbg" runat="server" Text="Accept" CausesValidation="False"
                            OnClick="btnAccept_Click" />
                    </div>
                    <div class="float-left">
                        <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="buttonbg" CausesValidation="false"
                            OnClick="btnReject_Click" />
                    </div>
                </div>
            </li>
        </ul>
    </div>


</asp:Content>
