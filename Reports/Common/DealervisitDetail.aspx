<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DealervisitDetail.aspx.cs" Inherits="Reports_Common_DealervisitDetail" %>

<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" id="StyleCss" runat="server" type="text/css" />
    <style type="text/css">
        .GridPager a {
            display: block;
            height: 18px;
            width: 18px;
            background-color: #c8c8c8;
            color: #fff;
            font-weight: bold;
            border: 1px solid #c8c8c8;
            text-align: center;
            text-decoration: none;
        }

        .GridPager span {
            display: block;
            height: 18px;
            width: 18px;
            background-color: #fff;
            color: #c8c8c8;
            font-weight: bold;
            border: 1px solid #c8c8c8;
            text-align: center;
            text-decoration: none;
        }

        .buttonbg {
            font-size: 11px;
            font-weight: normal;
            color: #fff;
            white-space: inherit;
            padding: 1px 8px;
            background-color: #0090a6;
            border: 1px #0090a6 solid;
            outline: none;
            cursor: pointer;
            height: 22px;
            text-decoration: none;
            border-radius: 4px;
        }
        .export {
    float: right; /*width: 100px;*/
    text-align: right;
    padding-right: 5px;
    margin-top: 7px;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="export">
            <asp:Button ID="exportToExel" Text="Click Here To Download File In Exel" CssClass="buttonbg" runat="server" OnClick="exportToExel_Click" />
        </div>
        <div></div>
        <div class="contentbox">

            <div class="grid">
                <asp:GridView ID="gvVisitAnalysis" runat="server" EmptyDataText="No Record Found" AlternatingRowStyle-CssClass="Altrow"
                    bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                    HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left"
                    HeaderStyle-VerticalAlign="top" RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left"
                    RowStyle-VerticalAlign="top" SelectedStyle-CssClass="gridselected" Width="100%"
                    AutoGenerateColumns="false" PageSize='<%$ AppSettings:GridViewPageSize %>' PagerStyle-CssClass="GridPager" AllowPaging="true" OnPageIndexChanging="gvVisitAnalysis_PageIndexChanging">
                    <FooterStyle CssClass="gridfooter" HorizontalAlign="Left" VerticalAlign="Top" />
                    <PagerStyle CssClass="GridPager" />
                    <Columns>
                        <asp:BoundField DataField="Designation" HeaderStyle-HorizontalAlign="Left" HeaderText="Designation"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="EmployeeName" HeaderStyle-HorizontalAlign="Left" HeaderText="Employee Name"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="EmployeeCode" HeaderStyle-HorizontalAlign="Left" HeaderText="Employee Code"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DealerName" HeaderStyle-HorizontalAlign="Left" HeaderText="Dealer Name"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="State" HeaderStyle-HorizontalAlign="Left" HeaderText="State"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CheckinDate" HeaderStyle-HorizontalAlign="Left" HeaderText="Check-in Date"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TimeSpent" HeaderStyle-HorizontalAlign="Left" HeaderText="Time Spent"
                            HtmlEncode="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
