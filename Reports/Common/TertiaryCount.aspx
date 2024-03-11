<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPageTertiary.master"
    AutoEventWireup="true" CodeFile="TertiaryCount.aspx.cs" Inherits="Reports_Common_TertiaryCount" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="BussinessLogic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div class="mainheading">
        Tertiary Count
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H25-C4-S">
            <ul>
                <li class="text">
                    <asp:Label ID="lblserfrmDate" runat="server" Text="">Date: <span class="error">*</span></asp:Label>
                </li>
                <li class="field">
                    <uc2:ucDatePicker ID="ucDatePicker1" runat="server" ErrorMessage="Invalid from date."
                        ValidationGroup="insert" defaultDateRange="True" RangeErrorMessage="Date should be less or equal then current date." />
                </li>
                <li class="text">
                    <asp:Label ID="lblBrand" runat="server" Text="">Brand</asp:Label>
                </li>
                <%--====#CC03 Added Started--%>
                <li class="field">
                    <asp:DropDownList ID="ddlBrand" runat="server" CssClass="formselect"></asp:DropDownList>
                </li>
                <li class="field3">
                    <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSerch_Click"
                        ValidationGroup="insert" CssClass="buttonbg" CausesValidation="True" />
                </li>
                <%--====#CC03 Added End--%>
            </ul>
        </div>
    </div>
    <div class="contentbox">        
        <div class="H20-C2-S">
            <ul>
                <li class="text">
                    <asp:Label ID="Label1" runat="server" Text="">SMS Activation Count :</asp:Label>
                </li>
                <li class="field">
                    <asp:Label ID="lblSMSActivation" CssClass="frmtxt1" runat="server" Text=""></asp:Label>
                </li>
                <%--#CC01 Add Start--%>
                <li class="text">
                    <asp:Label ID="LblSmsLastDateText" runat="server" Text="Last Record Date :"></asp:Label>
                </li>
                <li class="field">
                    <asp:Label ID="LblSmsLastDate" CssClass="frmtxt1" runat="server" Text=""></asp:Label>
                </li>
                <%--#CC01 Add End--%>
            </ul>
            <ul>
                 <li class="text">
                    <asp:Label ID="Label3" runat="server" Text="">Web Activation Count :</asp:Label></li>
                <li class="field">
                    <asp:Label ID="lblWebActivation" CssClass="frmtxt1" runat="server" Text=""></asp:Label>
                </li>
                <%--#CC01 Add Start--%>
                 <li class="text">
                    <asp:Label ID="LblWebLastDateText" runat="server" Text="Last Record Date :"></asp:Label></li>
                <li class="field">
                    <asp:Label ID="LblWebLastDate" CssClass="frmtxt1" runat="server" Text=""></asp:Label>
                </li>
                <%--#CC01 Add End--%>
            </ul>
            <ul>
                <li class="text">
                    <asp:Label ID="Label2" runat="server" Text="">Tertiary Considered Count :</asp:Label></li>
                <li class="field">
                    <asp:Label ID="lblTertiaryConsidered" CssClass="frmtxt1" runat="server" Text=""></asp:Label>
                </li>
                <%--#CC01 Add Start--%>
                <%--<td colspan="2"></td>--%>
                <%--#CC01 Add End--%>
            </ul>
        </div>

        <%--#CC02 ADDED START--%>
        <div>
            <div id="dvTime" runat="server">
                <asp:Label ID="lblTIme" runat="server" CssClass="frmtxt1"></asp:Label>
            </div>
            <div class="grid1">
                <asp:UpdatePanel runat="server" ID="updgrid">
                    <ContentTemplate>
                        <asp:GridView ID="grdvwTertairyCount" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                            RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                            HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                            AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                            HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                            BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                            PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected">
                            <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                            <Columns>
                                <%-- <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SMSLastDate"
                                                            HeaderText="SMS Last Date"></asp:BoundField>--%>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="RecordDate"
                                    HeaderText="Date"></asp:BoundField>

                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SMSActivationCount"
                                    HeaderText="SMS Activation Count"></asp:BoundField>
                                <%--            <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="WebLastDate"
                                                            HeaderText="Web Last Date"></asp:BoundField>--%>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="WebActivationCount"
                                    HeaderText="Web Activation Count"></asp:BoundField>
                                <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="TertiaryConsideredCount"
                                    HeaderText="Tertiary Considered Count"></asp:BoundField>

                            </Columns>
                            <PagerStyle CssClass="PagerStyle" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <%--#CC02 ADDED END--%>

    </div>

</asp:Content>
