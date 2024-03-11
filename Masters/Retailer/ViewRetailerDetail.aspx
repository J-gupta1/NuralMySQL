<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewRetailerDetail.aspx.cs"
    Inherits="Masters_HO_Retailer_ViewRetailerDetail" %>

<%--#CC01,Vijay Kumar Prajapati,31-10-2017,Change ISP on Counter To CSA on Counter.--%>
<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">

    <%--Add START  --%>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <%--Add END  --%>

    <title></title>
    <link rel="stylesheet" id="StyleCss" runat="server" type="text/css" />
</head>
<body>
    <div>
        <form id="form1" name="form1" runat="server">

            <asp:Label ID="lblHeader" runat="server" CssClass="error"></asp:Label>
            <div class="contentbox">
            <div class="grid1">
                <asp:DataList ID="RetailerDetailList" HorizontalAlign="Center" EnableViewState="false"
                    runat="server" Width="100%" CellPadding="0" CellSpacing="0">
                    <HeaderTemplate>
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" class="gridheader">
                            <tr>
                                <td align="left" width="30%" height="15">View Retailer Details
                                </td>
                                <td align="left">
                                    <%--        <a href="#" onclick="window.close();">
                                        <img alt="Close" title="Close" border="0" src="../../Assets/Beetel/CSS/images/error.png" /></a>--%>
                                </td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table cellpadding="4" cellspacing="0" border="0" width="100%">
                            <tr class="gridrow">
                                <td align="left" valign="top" width="30%" class="frmtxt1">Retailer Type
                                </td>
                                <td align="left" valign="top">
                                    <%#(DataBinder.Eval(Container.DataItem, "RetailerTypeName").ToString())%>
                                </td>
                            </tr>
                            <tr class="Altrow">
                                <td align="left" valign="top" class="frmtxt1">Retailer Code
                                </td>
                                <td align="left" valign="top">
                                    <%#(DataBinder.Eval(Container.DataItem, "RetailerCode").ToString())%>
                                </td>
                            </tr>
                            <tr class="gridrow">
                                <td align="left" valign="top" class="frmtxt1">Retailer Name
                                </td>
                                <td align="left" valign="top">
                                    <%#(DataBinder.Eval(Container.DataItem, "Retailername").ToString())%>
                                </td>
                            </tr>
                            <tr class="Altrow">
                                <td align="left" valign="top" class="frmtxt1">Retailer Code
                                </td>
                                <td align="left" valign="top">
                                    <%#(DataBinder.Eval(Container.DataItem, "RetailerCode").ToString())%>
                                </td>
                            </tr>
                            <%-- ======#CC03 Commented Started=====--%>
                            <%--<tr class="gridrow">
                                        <td align="left" valign="top" class="frmtxt1">Sales Channel Name
                                        </td>
                                        <td align="left" valign="top">
                                            <%#DataBinder.Eval(Container.DataItem, "SalesChannelName")%>
                                        </td>
                                    </tr>
                                    <tr class="Altrow">
                                        <td align="left" valign="top" class="frmtxt1">Salesman Name
                                        </td>
                                        <td align="left" valign="top">
                                            <%#DataBinder.Eval(Container.DataItem, "SalesmanName")%>
                                        </td>
                                    </tr>--%>
                            <%-- ======#CC03 Commented End=====--%>
                            <tr class="gridrow">
                                <td align="left" valign="top" class="frmtxt1">Address
                                </td>
                                <td align="left" valign="top">
                                    <div style="word-wrap: break-word; width: 300px; overflow: hidden; padding: 0px; margin: 0px;">
                                        <%#(DataBinder.Eval(Container.DataItem, "Address1").ToString())%><br />
                                        <%#DataBinder.Eval(Container.DataItem, "Address2").ToString()%>
                                    </div>
                                </td>
                            </tr>
                            <tr class="Altrow">
                                <td align="left" valign="top" class="frmtxt1">Country
                                </td>
                                <td align="left" valign="top">
                                    <%#(DataBinder.Eval(Container.DataItem, "CountryName").ToString())%>
                                </td>
                            </tr>
                            <tr class="gridrow">
                                <td align="left" valign="top" class="frmtxt1">State
                                </td>
                                <td align="left" valign="top">
                                    <%#(DataBinder.Eval(Container.DataItem, "StateName").ToString())%>
                                </td>
                            </tr>
                            <tr class="Altrow">
                                <td align="left" valign="top" class="frmtxt1">City
                                </td>
                                <td align="left" valign="top">
                                    <%#(DataBinder.Eval(Container.DataItem, "CityName").ToString())%>
                                </td>
                            </tr>
                            <tr class="gridrow">
                                <td align="left" valign="top" class="frmtxt1">District
                                </td>
                                <td align="left" valign="top">
                                    <%#DataBinder.Eval(Container.DataItem, "DistrictName")%>
                                </td>
                            </tr>
                            <tr class="Altrow">
                                <td align="left" valign="top" class="frmtxt1">Area
                                </td>
                                <td align="left" valign="top">
                                    <%#DataBinder.Eval(Container.DataItem, "AreaName")%>
                                </td>
                            </tr>
                            <tr class="gridrow">
                                <td align="left" valign="top" class="frmtxt1">Pin Code
                                </td>
                                <td align="left" valign="top">
                                    <%#DataBinder.Eval(Container.DataItem, "PinCode")%>
                                </td>
                            </tr>
                            <tr class="Altrow">
                                <td align="left" valign="top" class="frmtxt1">Contact Person
                                </td>
                                <td align="left" valign="top">
                                    <%#DataBinder.Eval(Container.DataItem, "ContactPerson")%>
                                </td>
                            </tr>
                            <tr class="gridrow">
                                <td align="left" valign="top" class="frmtxt1">Phone No
                                </td>
                                <td align="left" valign="top">
                                    <%#DataBinder.Eval(Container.DataItem, "PhoneNumber")%>
                                </td>
                            </tr>
                            <tr class="Altrow">
                                <td align="left" valign="top" class="frmtxt1">Mobile
                                </td>
                                <td align="left" valign="top">
                                    <%#DataBinder.Eval(Container.DataItem, "MobileNumber")%>
                                </td>
                            </tr>
                            <tr class="gridrow">
                                <%--#CC01 Comment Start --%>
                                <%-- <td align="left" class="frmtxt1" >
                                        Counter Size --%>
                                <%--#CC01 Comment End--%>
                                <%--#CC01 Add Start--%>
                                <td align="left" valign="top" class="frmtxt1" nowrap="nowrap">Counter Potential in Volume
                                            <%--#CC01 Add End--%>
                                </td>
                                <td align="left" valign="top">
                                    <%#DataBinder.Eval(Container.DataItem, "CounterSize")%>
                                </td>
                            </tr>
                            <tr class="Altrow">
                                <td align="left" valign="top" class="frmtxt1">EMail
                                </td>
                                <td align="left" valign="top">
                                    <a href="mailto:<%#DataBinder.Eval(Container.DataItem, "EMail")%>">
                                        <%#DataBinder.Eval(Container.DataItem, "EMail")%></a>
                                </td>
                            </tr>
                            <tr class="gridrow">
                                <td align="left" valign="top" class="frmtxt1">
                                    <%--#CC02 Add Start --%>
                                    <asp:Label ID="txtTinNoHeading" runat="server" class="frmtxt1" Text="Tin No."></asp:Label>
                                    <%--#CC02 Add End --%>
                                    <%--  Tin No. #CC02 Commented --%>
                                </td>
                                <td align="left" valign="top">
                                    <%#DataBinder.Eval(Container.DataItem, "TinNumber")%>
                                </td>
                            </tr>
                            <tr class="Altrow">
                                <td align="left" valign="top" class="frmtxt1"><%--#CC01 Commented ISP on Counter --%> CSA on Counter
                                </td>
                                <td align="left" valign="top">
                                    <%#(DataBinder.Eval(Container.DataItem, "ISPOnCounter").ToString())%>
                                </td>
                            </tr>
                            <tr class="gridrow">
                                <td align="left" valign="top" class="frmtxt1">Date of Birth
                                </td>
                                <td align="left" valign="top">
                                    <%#DataBinder.Eval(Container.DataItem, "DOB","{0:MM/dd/yyyy}")%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
            </div>
            </div>
            <%-- ======#CC03 Added Started=====--%>
            <div class="contentbox">
            <div class="grid1">
            <asp:GridView ID="Gridsaleschanneldetails" runat="server" bgcolor="" BorderWidth="0px" CellPadding="4" CellSpacing="1" FooterStyle-CssClass="gridfooter"
                FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" GridLines="none"
                HeaderStyle-CssClass="gridheader" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                RowStyle-CssClass="gridrow" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                SelectedStyle-CssClass="gridselected" Width="100%" AllowPaging="false" AlternatingRowStyle-CssClass="Altrow" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="MappedSaleschannelname" HeaderStyle-HorizontalAlign="Left" HeaderText="SalesChannel Name"
                        HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MappedSalesmanName" HeaderStyle-HorizontalAlign="Left" HeaderText="Salesman Name"
                        HtmlEncode="true">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            </div>
            </div>
            <%-- =====#CC03 Added End======--%>
        </form>
    </div>
</body>
</html>
