<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageSapInfoOnida.aspx.cs"
    MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="Reports_Common_ManageSapInfoOnida" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<%--    <style type="text/css">
        .style1
        {
            height: 27px;
        }
        .style2
        {
            width: 127px;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
        <ContentTemplate>
            <uc1:ucMessage ID="ucMessage1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="mainheading">
        Sap Info
    </div>
    <div class="contentbox">
        <asp:UpdatePanel ID="updsearch" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                 <div class="mandatory">
                        (*) Marked fields are mandatory            
                    </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">
                            <asp:Label ID="lblmodule" runat="server" Text="">Select Module: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">                            
                                <asp:DropDownList ID="ddlModule" runat="server" CssClass="formselect">
                                </asp:DropDownList>                          
                        </li>
                        <li class="text">
                            <asp:Label ID="lblserfrmDate" runat="server" Text="">From Date: <span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateFrom" runat="server" ErrorMessage="Invalid from date."
                                IsRequired="true" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                        </li>
                        <li class="text">
                            <asp:Label ID="lblsertodate" runat="server" Text="">To Date:<span class="error">*</span></asp:Label>
                        </li>
                        <li class="field">
                            <uc2:ucDatePicker ID="ucDateTo" runat="server" ErrorMessage="Invalid to Date." defaultDateRange="True"
                                IsRequired="true" RangeErrorMessage="Date should be less or equal then current date." />
                        </li>                   
                        <li class="text"></li>
                        <li class="field">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerch_Click"
                                    CssClass="buttonbg" CausesValidation="true" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btncancel" Text="Cancel" runat="server" OnClick="btncancel_Click"
                                    CssClass="buttonbg" CausesValidation="False" />
                            </div>
                        </li>
                    </ul>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel runat="server" ID="updgrid" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                <div class="mainheading">
                    List
                </div>
                <div class="contentbox">
                    <div class="grid1">
                        <asp:GridView ID="gvSapInfo" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                            BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="SapServiceListDetailID,ErrorDocNo"
                            FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                            AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                            HeaderStyle-CssClass="gridheader" RowStyle-VerticalAlign="top"
                            Width="100%" OnPageIndexChanging="gvSapInfo_PageIndexChanging"
                            OnRowCommand="gvSapInfo_RowCommand"
                            OnRowDataBound="gvSapInfo_RowDataBound">
                            <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                            <Columns>
                                <asp:BoundField DataField="ModuleName" HeaderText="Module Name" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SapServiceMethodName" HeaderText="SapService MethodName"
                                    HtmlEncode="false" Visible="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SapFileName" HeaderText="File Name" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="StatusValue" HeaderText="StatusValue" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <%--   <asp:BoundField DataField="MessageDetail" HeaderText="MessageDetail" HtmlEncode="false">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>--%>
                                <asp:BoundField DataField="XMLData" HeaderText="Error Log" HtmlEncode="false" Visible="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LogType" HeaderText="LogType" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:TemplateField Visible="false">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreationDate" runat="server" Text='<%# Eval("RecordCreationDate","{0:dd MMM yy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField Visible="false">
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblserviceDoc" runat="server" Text='<%#Eval("ErrorDocNo")%>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False"></ItemStyle>
                                    <HeaderStyle Width="85px" HorizontalAlign="left" Wrap="False"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Button ID="btnDetail" Text="Detail" runat="server" CssClass="buttonbg" CausesValidation="true"
                                            CommandName="Detail" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </div>
                </div>
                <div class="mainheading" runat="server" id="dvDetail" visible="false">
                    Detail List
                </div>
                <div class="export">
                    <asp:Button ID="exporttoexel" Text="" runat="server" OnClick="exporttoexel_Click"
                        CssClass="excel" Visible="false" />
                </div>
                <div class="grid1" runat="server" id="dvgrdDetail" visible="false">
                    <asp:GridView ID="gvSapInfoDetail" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                        BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="RecordID"
                        FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                        HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" RowStyle-HorizontalAlign="left"
                        AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                        HeaderStyle-CssClass="gridheader" RowStyle-VerticalAlign="top"
                        Width="100%" OnPageIndexChanging="gvSapInfoDetail_PageIndexChanging">
                        <FooterStyle CssClass="" HorizontalAlign="Left" VerticalAlign="Top" />
                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top"></RowStyle>
                        <Columns>
                            <asp:BoundField DataField="MaterialCode" HeaderText="MaterialCode" HtmlEncode="false">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Plant" HeaderText="Plant"
                                HtmlEncode="false">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SoldToPartyCode" HeaderText="SoldToPartyCode" HtmlEncode="false">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ErrorDescription" HeaderText="ErrorDescription" HtmlEncode="false">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top"></HeaderStyle>
                        <PagerStyle CssClass="PagerStyle" />
                    </asp:GridView>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="exporttoexel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
