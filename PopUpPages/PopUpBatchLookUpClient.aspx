<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="PopUpBatchLookUpClient.aspx.cs" Inherits="PopUpPages_PopUpBatchLookUpClient" %>

<%@ Register src="~/UserControls/BatchLookupClientSide.ascx" tagname="BatchLookupClientSide" tagprefix="uc1" %>

<%--<%@ Register src="~/UserControls/TempBatchLookupClientSide.ascx" tagname="BatchLookupClientSideTemp" tagprefix="uc1" %>--%>

<%@ Import Namespace="BussinessLogic" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="Head1">
     <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/css/style.css") %>" />  
     <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/media/css/demo_page.css") %>" />  
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/css/demo_table.css") %>" />
    <script type="text/javascript" language="javascript" src="../Assets/Jscript/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Assets/Jscript/jquery.dataTables.js"></script>
    
 
    
</head>
<body >
    <form id="form1" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
       <uc1:BatchLookupClientSide ID="BatchLookupClientSide1" runat="server" />
        <%--  <uc1:BatchLookupClientSideTemp ID="BatchLookupClientSide1" runat="server" />--%>
    
    </div>
    </form>
</body>
</html>
