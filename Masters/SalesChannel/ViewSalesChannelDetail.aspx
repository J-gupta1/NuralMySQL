<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewSalesChannelDetail.aspx.cs"
    Inherits="Masters_HO_SalesChannel_ViewSalesChannelDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 
    <title></title>

 <%-- <link href="../../Assets/Beetel/CSS/style.css" rel="stylesheet" type="text/css" />--%>
<%--   <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/CSS/style.css") %>" />--%>
    <link rel="stylesheet" id="StyleCss" runat="server"  type="text/css"/>
  <%--  <link  id="cssstyle" runat="server"  />--%>
   <%-- <style type="text/css">
      body					{ margin:0; padding:0; font:12px Arial, Helvetica, sans-serif; color:#000; background:#fff; repeat-x left top;  height:100%; min-height:100%;}
* {  outline:none; }
    </style>
  --%>
</head>
<body>
    <div align="center">
        <form id="form1" name="form1" runat="server">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
          
            <tr>
                <td align="left">
                    <asp:Label ID="lblHeader" runat="server" CssClass="error"></asp:Label>
                    <asp:DataList ID="SalesChannelDetailList" HorizontalAlign="Center" EnableViewState="false"
                        runat="server" Width="98%" CellPadding="0" CellSpacing="0">
                        <HeaderTemplate>
                            <table width="100%" align="center" cellpadding="4" cellspacing="1" border="0" class="gridheader">
                                <tr>
                                    <td align="left" height="22">
                                        View Sales Channel Details
                                    </td>
                                    <td align="right">
                                        <%--<a href="#" onclick="window.close();">--%>
                                     <%--   <img alt="Close" title="Close" border="0" onclick="parent.ViewDetails.hide();" src="../../Assets/Beetel/CSS/images/error.png" />
                                        <input type="button" value="Close"  onclick="parent.ViewDetails.hide();" />--%>
                                        </a>
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table align="left" cellpadding="4" cellspacing="1" width="100%" class="gridbg" border="0">
                                <tr class="gridrow">
                                    <td width="30%" align="left" >
                                      <strong> Sales Channel Name</strong> 
                                    </td>
                                    <td align="left">
                                        <%#(DataBinder.Eval(Container.DataItem, "SalesChannelname").ToString())%>
                                    </td>
                                </tr>
                                <tr class="gridrow1">
                                    <td width="30%" align="left">
                                         <strong>Sales Channel Code</strong>
                                    </td>
                                    <td align="left">
                                        <%#(DataBinder.Eval(Container.DataItem, "SalesChannelCode").ToString())%>
                                    </td>
                                </tr>
                                <tr class="gridrow">
                                    <td align="left">
                                         <strong>Parent Name</strong>
                                    </td>
                                    <td align="left">
                                        <%#DataBinder.Eval(Container.DataItem, "Parentname")%>
                                    </td>
                                </tr>
                                <asp:Panel runat="server" ID="pnlMultilocation">
                                    <tr class="gridrow1">
                                        <td align="left">
                                            <strong> Multi Location</strong>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container.DataItem, "Multilocation")%>
                                        </td>
                                    </tr>
                                    <tr class="gridrow">
                                        <td align="left">
                                             <strong>Parent SS</strong>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container.DataItem, "GroupParentName")%>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <tr class="gridrow1">
                                    <td align="left">
                                         <strong>Address</strong>
                                    </td>
                                    <td align="left">
                                        <%#(DataBinder.Eval(Container.DataItem, "Address1").ToString())%><br />
                                        <%#DataBinder.Eval(Container.DataItem, "Address2").ToString()%>
                                    </td>
                                </tr>
                                
                                
                                <tr class="gridrow">
                                    <td align="left">
                                        <strong> Country</strong>
                                    </td>
                                    <td align="left">
                                        <%#(DataBinder.Eval(Container.DataItem, "CountryName").ToString())%>
                                    </td>
                                </tr>
                                <tr class="gridrow1">
                                    <td align="left">
                                        <strong> State</strong>
                                    </td>
                                    <td align="left">
                                        <%#(DataBinder.Eval(Container.DataItem, "StateName").ToString())%>
                                    </td>
                                </tr>
                                <tr class="gridrow">
                                    <td align="left">
                                        <strong> City</strong>
                                    </td>
                                    <td align="left">
                                        <%#(DataBinder.Eval(Container.DataItem, "CityName").ToString())%>
                                    </td>
                                </tr>
                                <tr class="gridrow1">
                                    <td align="left">
                                        <strong> District</strong>
                                    </td>
                                    <td align="left">
                                        <%#DataBinder.Eval(Container.DataItem, "DistrictName")%>
                                    </td>
                                </tr>
                                <tr class="gridrow">
                                    <td align="left">
                                        <strong> Area</strong>
                                    </td>
                                    <td align="left">
                                        <%#DataBinder.Eval(Container.DataItem, "AreaName")%>
                                    </td>
                                </tr>
                                <tr class="gridrow1">
                                    <td align="left">
                                         <strong>Pin Code</strong>
                                    </td>
                                    <td align="left">
                                        <%#DataBinder.Eval(Container.DataItem, "PinCode")%>
                                    </td>
                                </tr>
                                <tr class="gridrow">
                                    <td align="left">
                                        <strong> Contact Person </strong>
                                    </td>
                                    <td align="left">
                                        <%#DataBinder.Eval(Container.DataItem, "ContactPerson")%>
                                    </td>
                                </tr>
                                <tr class="gridrow1">
                                    <td align="left">
                                        <strong> Phone No </strong>
                                    </td>
                                    <td align="left">
                                        <%#DataBinder.Eval(Container.DataItem, "PhoneNumber")%>
                                    </td>
                                </tr>
                                <tr class="gridrow">
                                    <td align="left">
                                        <strong> Mobile</strong>
                                    </td>
                                    <td align="left">
                                        <%#DataBinder.Eval(Container.DataItem, "MobileNumber")%>
                                    </td>
                                </tr>
                               <%-- <tr class="gridrow">
                                    <td align="left">
                                        Pin code
                                    </td>
                                    <td align="left">
                                        <%#DataBinder.Eval(Container.DataItem, "PinCode")%>
                                    </td>
                                </tr>--%>
                                <tr class="gridrow1">
                                    <td align="left">
                                       <strong>  Fax </strong>
                                    </td>
                                    <td align="left">
                                        <%#DataBinder.Eval(Container.DataItem, "Fax")%>
                                    </td>
                                </tr>
                                <tr class="gridrow">
                                    <td align="left">
                                        <strong> EMail</strong>
                                    </td>
                                    <td align="left">
                                        <a href="mailto:<%#DataBinder.Eval(Container.DataItem, "EMail")%>">
                                            <%#DataBinder.Eval(Container.DataItem, "EMail")%></a>
                                    </td>
                                </tr>
                                <tr class="gridrow1">
                                    <td align="left">
                                      <strong>   PAN No.</strong>
                                    </td>
                                    <td align="left">
                                        <%#DataBinder.Eval(Container.DataItem, "PanNO")%>
                                    </td>
                                </tr>
                                <tr class="gridrow">
                                    <td align="left">
                                       <strong>  TIN No.</strong>
                                    </td>
                                    <td align="left">
                                        <%#DataBinder.Eval(Container.DataItem, "TinNumber")%>
                                    </td>
                                </tr>
                                <tr class="gridrow1">
                                    <td align="left">
                                       <strong>  VAT No.</strong>
                                    </td>
                                    <td align="left">
                                        <%#DataBinder.Eval(Container.DataItem, "CStNumber")%>
                                    </td>
                                </tr>
                                <tr class="gridrow">
                                    <td align="left">
                                        <strong> Buisness Start Date</strong>
                                    </td>
                                    <td align="left">
                                        <%#DataBinder.Eval(Container.DataItem, "BussinessStartDate","{0:MM/dd/yyyy}")%>
                                    </td>
                                </tr>
                                 <tr class="gridrow1">
                                    <td align="left">
                                      <strong>   Date of Birth</strong>
                                    </td>
                                    <td align="left">
                                        <%#DataBinder.Eval(Container.DataItem, "DOB","{0:MM/dd/yyyy}")%>
                                    </td>
                                </tr>
                                 <tr class="gridrow">
                                    <td align="left">
                                       <strong>  Date of Anniversary</strong>
                                    </td>
                                    <td align="left">
                                        <%#DataBinder.Eval(Container.DataItem, "DOA","{0:MM/dd/yyyy}")%>
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
