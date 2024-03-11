<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewRetailerDetail.aspx.cs"
    Inherits="Masters_HO_Retailer_ViewRetailerDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     <link rel="stylesheet" id="StyleCss" runat="server"  type="text/css"/>
</head>
<body>
    <div align="center">
        <form id="form1" name="form1" runat="server">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            
            <td align="left">
                <asp:Label ID="lblHeader" runat="server" CssClass="error"></asp:Label>
                <asp:DataList ID="RetailerDetailList" HorizontalAlign="Center" EnableViewState="false"
                    runat="server" Width="98%" CellPadding="0" CellSpacing="0">
                    <HeaderTemplate>
                        <table width="100%" align="center" cellpadding="2" cellspacing="1" border="0" class="gridheader">
                            <tr>
                                <td align="left" height="15">
                                    View Retailer Details
                                </td>
                                <td align="right">
                            <%--        <a href="#" onclick="window.close();">
                                        <img alt="Close" title="Close" border="0" src="../../Assets/Beetel/CSS/images/error.png" /></a>--%>
                                </td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table align="left" cellpadding="4" cellspacing="1" width="100%" class="gridbg" border="0">
                            <tr class="gridrow">
                                <td width="30%" align="left">
                                    Retailer Name
                                </td>
                                <td align="left">
                                    <%#(DataBinder.Eval(Container.DataItem, "Retailername").ToString())%>
                                </td>
                            </tr>
                            <tr class="gridrow1">
                                <td width="30%" align="left">
                                    Retailer Code
                                </td>
                                <td align="left">
                                    <%#(DataBinder.Eval(Container.DataItem, "RetailerCode").ToString())%>
                                </td>
                            </tr>
                            <tr class="gridrow">
                                <td align="left">
                                    Sales Channel Name
                                </td>
                                <td align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "SalesChannelName")%>
                                </td>
                            </tr>
                            <tr class="gridrow1">
                                <td align="left">
                                    Salesman Name
                                </td>
                                <td align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "SalesmanName")%>
                                </td>
                            </tr>
                            <tr class="gridrow">
                                <td align="left">
                                    Address
                                </td>
                                <td align="left">
                                    <%#(DataBinder.Eval(Container.DataItem, "Address1").ToString())%><br />
                                    <%#DataBinder.Eval(Container.DataItem, "Address2").ToString()%>
                                </td>
                            </tr>
                            <tr class="gridrow1">
                                <td align="left">
                                    Country
                                </td>
                                <td align="left">
                                    <%#(DataBinder.Eval(Container.DataItem, "CountryName").ToString())%>
                                </td>
                            </tr>
                            
                            <tr class="gridrow">
                                <td align="left">
                                    State
                                </td>
                                <td align="left">
                                    <%#(DataBinder.Eval(Container.DataItem, "StateName").ToString())%>
                                </td>
                            </tr>
                            <tr class="gridrow1">
                                <td align="left">
                                    City
                                </td>
                                <td align="left">
                                    <%#(DataBinder.Eval(Container.DataItem, "CityName").ToString())%>
                                </td>
                            </tr>
                            <tr class="gridrow">
                                <td align="left">
                                    District
                                </td>
                                <td align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "DistrictName")%>
                                </td>
                            </tr>
                            <tr class="gridrow1">
                                <td align="left">
                                    Area
                                </td>
                                <td align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "AreaName")%>
                                </td>
                            </tr>
                            <tr class="gridrow">
                                <td align="left">
                                    Pin Code
                                </td>
                                <td align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "PinCode")%>
                                </td>
                            </tr>
                            <tr class="gridrow1">
                                <td align="left">
                                    Contact Person
                                </td>
                                <td align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "ContactPerson")%>
                                </td>
                            </tr>
                            <tr class="gridrow">
                                <td align="left">
                                    Phone No
                                </td>
                                <td align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "PhoneNumber")%>
                                </td>
                            </tr>
                            <tr class="gridrow1">
                                <td align="left">
                                    Mobile
                                </td>
                                <td align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "MobileNumber")%>
                                </td>
                            </tr>
                            
                            <tr class="gridrow">
                                <td align="left">
                                    Counter Size
                                </td>
                                <td align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "CounterSize")%>
                                </td>
                            </tr>
                            <tr class="gridrow1">
                                <td align="left">
                                    EMail
                                </td>
                                <td align="left">
                                    <a href="mailto:<%#DataBinder.Eval(Container.DataItem, "EMail")%>">
                                        <%#DataBinder.Eval(Container.DataItem, "EMail")%></a>
                                </td>
                            </tr>
                            <tr class="gridrow">
                                <td align="left">
                                    Tin No.
                                </td>
                                <td align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "TinNumber")%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
            </td>
            </tr>
        </table>
        </form>
    </div>
</body>
</html>
