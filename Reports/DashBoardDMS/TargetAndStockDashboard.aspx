<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/ReportPage.master" AutoEventWireup="true" CodeFile="TargetAndStockDashboard.aspx.cs" Inherits="Reports_TargetAndStockDashboard" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMsg" runat="server" />
    <div class="mainheading">
        Target And Stock Dashboard
    </div>
    <div class="contentbox">
        <div class="mandatory">
            (*) Marked fields are mandatory            
        </div>
        <div class="H30-C3-S">
            <ul>
                <li class="text">Month:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="formselect" Width="80px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqMonth" runat="server" ControlToValidate="ddlMonth"
                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select month."
                        SetFocusOnError="true" ValidationGroup="vgStockRpt"></asp:RequiredFieldValidator>

                </li>
                <li class="text">Year:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="formselect" Width="80px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqYear" runat="server" ControlToValidate="ddlYear"
                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select year."
                        SetFocusOnError="true" ValidationGroup="vgStockRpt"></asp:RequiredFieldValidator>

                </li>
                <li class="text">Target Type:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlTargetType" runat="server" CssClass="formselect" Width="80px">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Quantity" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Value" Value="2"></asp:ListItem>
                    </asp:DropDownList>

                    <%--<asp:RequiredFieldValidator ID="reqTargetType" runat="server" ControlToValidate="ddlTargetType"
                                                                            CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select target type."
                                                                            SetFocusOnError="true" ValidationGroup="vgStockRpt"></asp:RequiredFieldValidator>--%>
                                                                  
                </li>
            </ul>
            <ul>
                <li class="text">Type:<span class="error">*</span>
                </li>
                <li class="field">
                    <asp:DropDownList ID="ddlDashBoardType" runat="server" CssClass="formselect" Width="80px">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Target" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Stock" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Both" Value="3"></asp:ListItem>
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="reqDashboardType" runat="server" ControlToValidate="ddlDashBoardType"
                        CssClass="error" Display="Dynamic" InitialValue="0" ErrorMessage="Please select target type."
                        SetFocusOnError="true" ValidationGroup="vgStockRpt"></asp:RequiredFieldValidator>

                </li>
                <li class="text"></li>
                <li class="field">
                    <div class="float-margin">
                        <asp:Button ID="btnSearch" Text="Search" runat="server" CssClass="buttonbg" CausesValidation="true" ValidationGroup="vgStockRpt"
                            OnClick="btnSearch_Click" />
                    </div>
                    <div class="float-left padding-top">
                        <asp:LinkButton ID="lnkExportStockDetail" runat="server" Text="Export Stock Detail"
                            CssClass="elink2" Style="color: green" OnClick="lnkExportStockDetail_Click"></asp:LinkButton>
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div class="mainheading">
        Target Dashboard
    </div>
    <div class="export">
        <asp:Button ID="btnExprtTarget" Text="" runat="server" CssClass="excel" CausesValidation="False"
            OnClick="btnExprtTarget_Click" Visible="false" />
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:GridView ID="grdTarget" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected" OnPageIndexChanging="grdTarget_PageIndexChanging">
                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                <Columns>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Region"
                        HeaderText="Region"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BM"
                        HeaderText="BM"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="FP Target"
                        HeaderText="FP Target"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SP Target"
                        HeaderText="SP Target"></asp:BoundField>


                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="FP Ach"
                        HeaderText="FP Ach"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SP Ach"
                        HeaderText="SP Ach"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Total Target"
                        HeaderText="Total Target"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Total Ach"
                        HeaderText="Total Ach"></asp:BoundField>
                </Columns>
                <PagerStyle CssClass="PagerStyle" />
            </asp:GridView>
        </div>
    </div>

    <div class="mainheading">
        Stock Dashboard
    </div>
    <div class="export">
        <asp:Button ID="btnExportStockDashBoard" Text="" runat="server" CssClass="excel" CausesValidation="False"
            OnClick="btnExportStockDashBoard_Click" Visible="false" />
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:GridView ID="grdStockDashBoard" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                AlternatingRowStyle-CssClass="Altrow" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected" OnPageIndexChanging="grdStockDashBoard_PageIndexChanging">
                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                <Columns>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Region"
                        HeaderText="Region"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="BM"
                        HeaderText="BM"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="State"
                        HeaderText="State"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="No of Active Partner"
                        HeaderText="No of Active Partner"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="In-Transit Stock"
                        HeaderText="In-Transit Stock"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Opening Stock"
                        HeaderText="Opening Stock"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="WOD"
                        HeaderText="WOD"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Secondary"
                        HeaderText="Secondary"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Activation"
                        HeaderText="Activation"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Closing Stock"
                        HeaderText="Closing Stock"></asp:BoundField>
                </Columns>
                <PagerStyle CssClass="PagerStyle" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>

