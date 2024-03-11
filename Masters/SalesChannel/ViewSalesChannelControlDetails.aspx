<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="ViewSalesChannelControlDetails.aspx.cs"
    Inherits="Masters_SalesChannel_ViewSalesChannelControlDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/SalesChannelInfoFilterControl.ascx" TagName="SalesChannelSearch"
    TagPrefix="uc3" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Search SalesChannel Code</title>
    <link id="LnkPopUP" runat="server" rel="stylesheet" type="text/css" />
    <link id="lnkStyle" runat="server" rel="stylesheet" type="text/css" />
    
    
</head>
<body>
    <div align="center">
        <form id="form1" name="form1" runat="server">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td align="left">
                    <uc3:SalesChannelSearch ID="ucSalesChannel" runat="server" />
                </td>
            </tr>
        </table>
        <%-- <table cellspacing="0" cellpadding="0" width= "100%" border="0" style="float: left;">
            <tr>
                <td align="left" valign="top" width="10">&nbsp;
                  <uc3:SalesChannelSearch ID = "ucSalesChannel" runat = "server" />  
                </td>
                
                <td align="left" valign="top" width="15%">&nbsp;
                    
                </td>
            </tr>
        </table>--%>
        </form>
    </div>
</body>
</html>
