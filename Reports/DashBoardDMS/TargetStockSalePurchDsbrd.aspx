<%-- * 18-Dec-2018, Rakesh Raj, #CC01, Added Top 1 Buletting in Marquee--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/ReportPage.master" AutoEventWireup="true" CodeFile="TargetStockSalePurchDsbrd.aspx.cs" Inherits="Reports_TargetStockSalePurchDsbrd" %>

<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <uc1:ucMessage ID="ucMsg" runat="server" />

   
    <%--#CC01 Start--%>
    <div style="padding-top:1px;" >
                <div class="elink float-margin padding-top">
                    Last Updated On:</div>
                  <div class="float-margin elink padding-top padding-right">
                 <asp:Label ID="lblLastUpdate" runat="server"></asp:Label>
               </div>            
               <%-- <div class="elink7 float-margin padding-top">Latest Bulletin:</div>            
                <div id="tdBulletin" runat="server" visible="true" class="float-margin">
                    <marquee direction="left" style="font-size:12px; padding:5px; width:400px; border:dotted 1px #dddddd; border-radius:5px; background-color: #fcffa9;"><a style=""  href='<%=BussinessLogic.PageBase.siteURL%>default.aspx?val=3'> <%=strLastestBulletin%></a></marquee>
                </div>--%>
          </div>
    
    <%--#CC01 End--%>
    <div class="clear"></div>
     

    <div class="mainheading">
        Target Dashboard
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:GridView ID="grdTarget" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected">
                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                <Columns>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="month_Name"
                        HeaderText="Month"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="TargetBase"
                        HeaderText="Target Base"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="TargetType"
                        HeaderText="Target Type"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ProductCategoryGroupName"
                        HeaderText="Product Group"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Target"
                        HeaderText="Target"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Achievement"
                        HeaderText="Achievement"></asp:BoundField>

                </Columns>
                <PagerStyle CssClass="PagerStyle" />
            </asp:GridView>
        </div>
    </div>
    <div class="mainheading">
        Stock & Sale Dashboard
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:GridView ID="grdStockDashBoard" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected">
                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                <Columns>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelType"
                        HeaderText="Sales channel"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="StockFP"
                        HeaderText="Stock FP"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="StockSP"
                        HeaderText="Stock SP"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Above90FP"
                        HeaderText="Above 90 Days FP Stock"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="Above90SP"
                        HeaderText="Above 90 Days SP Stock"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesMTDFP"
                        HeaderText="Sales FP MTD"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesMTDSP"
                        HeaderText="Sales SP MTD"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="TerSalesMTDFP"
                        HeaderText="Ter FP MTD"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="TerSalesMTDSP"
                        HeaderText="Ter SP MTD"></asp:BoundField>
                </Columns>
                <PagerStyle CssClass="PagerStyle" />
            </asp:GridView>
        </div>
    </div>
    <div class="mainheading">
        Purchase Dashboard
    </div>
    <div class="contentbox">
        <div class="grid1">
            <asp:GridView ID="grdPurchaseDashBoard" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected">
                <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                <Columns>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SalesChannelType"
                        HeaderText="Sales channel"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="PurchaseFP"
                        HeaderText="Purchase FP"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="StockFP"
                        HeaderText="Stock FP"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="NonReportedActivationFP"
                        HeaderText="Non Rep ACT FP MTD"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="TerSalesMTDFP"
                        HeaderText="Ter FP MTD"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="PurchaseSP"
                        HeaderText="Purchase SP"></asp:BoundField>
                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="StockSP"
                        HeaderText="Stock SP"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="NonReportedActivationSP"
                        HeaderText="Non Rep ACT SP MTD"></asp:BoundField>

                    <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="TerSalesMTDSP"
                        HeaderText="Ter SP MTD"></asp:BoundField>
                </Columns>
                <PagerStyle CssClass="PagerStyle" />
            </asp:GridView>
        </div>
    </div>

    <div class="mainheading">
        Top & Bottom 5 Retailer
    </div>
    <div class="contentbox">

        <div class="float-margin" style="width: 49%">
            <div class="gridspace">
                <asp:GridView ID="grdTopRetailer" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                    RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                    HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                    AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                    HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                    BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                    PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected">
                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                    <Columns>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="RetailerCode"
                            HeaderText="Retailer Code"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="MTDSaleVolFP"
                            HeaderText="Sale FP MTD"></asp:BoundField>

                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="MTDSaleVolSP"
                            HeaderText="Sale SP MTD"></asp:BoundField>


                    </Columns>
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
        </div>

        <div class="float-left" style="width: 49%">
            <div class="gridspace">
                <asp:GridView ID="grdBottomRetailer" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                    RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                    HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                    AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                    HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                    BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                    PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected">
                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                    <Columns>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="RetailerCode"
                            HeaderText="Retailer Code"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="MTDSaleVolFP"
                            HeaderText="Sale FP MTD"></asp:BoundField>

                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="MTDSaleVolSP"
                            HeaderText="Sale SP MTD"></asp:BoundField>


                    </Columns>
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <div class="mainheading">
        Top 5 Model SP & FP
    </div>
    <div class="contentbox">
        <div class="float-margin" style="width: 49%">
            <div class="gridspace">
                <asp:GridView ID="grdTopModelSP" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                    RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                    HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                    AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                    HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                    BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                    PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected">
                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                    <Columns>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ModelName"
                            HeaderText="Model Name"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SaleSP"
                            HeaderText="Sale SP MTD"></asp:BoundField>


                    </Columns>
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
        </div>
        <div class="float-left" style="width: 49%">
            <div class="gridspace">
                <asp:GridView ID="grdTopModelFP" runat="server" FooterStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="Left"
                    RowStyle-HorizontalAlign="left" EmptyDataText="No Record found" RowStyle-VerticalAlign="top"
                    HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top" GridLines="none"
                    AlternatingRowStyle-CssClass="gridrow1" RowStyle-CssClass="gridrow" FooterStyle-CssClass="gridfooter"
                    HeaderStyle-CssClass="gridheader" CellSpacing="1" CellPadding="4" bgcolor=""
                    BorderWidth="0px" Width="100%" AutoGenerateColumns="false" AllowPaging="True"
                    PageSize='<%$ AppSettings:GridViewPageSize %>' SelectedStyle-CssClass="gridselected">
                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top"></FooterStyle>
                    <Columns>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="ModelName"
                            HeaderText="Model Name"></asp:BoundField>
                        <asp:BoundField HtmlEncode="true" HeaderStyle-HorizontalAlign="Left" DataField="SaleFP"
                            HeaderText="Sale FP MTD"></asp:BoundField>


                    </Columns>
                    <PagerStyle CssClass="PagerStyle" />
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

