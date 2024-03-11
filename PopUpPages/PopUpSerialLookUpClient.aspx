<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PopUpSerialLookUpClient.aspx.cs" Inherits="PopUpPages_PopUpSerialLookUpClient" %>

<%@ Register src="~/UserControls/SerialLookupClientSide.ascx" tagname="SerialLookupClientSide" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/css/style.css") %>" />  
       <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" +  strAssets + "/media/css/demo_page.css") %>" />  
    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/css/demo_table.css") %>" />
  <%--  <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/css/demo_table_jui.css") %>" />--%>
     <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/css/jquery.dataTables.css") %>" />
  <%--    <link rel="stylesheet" type="text/css" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/css/jquery.dataTables_themeroller.css") %>" />--%>
    
    
    
    <script type="text/javascript" language="javascript" src="../Assets/Jscript/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="../Assets/Jscript/jquery.dataTables.js"></script> 
    
<%--      <script type="text/javascript" language="javascript" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/src/core/core.constructor.js") %>"></script> 
         <script type="text/javascript" language="javascript" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/src/core/core.sort.js") %>"></script> 
    <script type="text/javascript" language="javascript" href="<%# Page.ResolveClientUrl("~/" + strAssets + "/media/src/core/core.length.js") %>"></script> 
    
--%>
</head>
<body >
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/CommonService.asmx" />
            </Services>
        </asp:ScriptManager>
      <uc1:SerialLookupClientSide ID="SerialLookupClientSide1" runat="server" />
    
    </div>
    </form>
</body>
</html>
