<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SMSParsing.aspx.cs" Inherits="Integration_SMSParsing" %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>--%>

<%--
<%@ Page Language="VB" %> 
<%@ Import Namespace="System.Net" %> 
<%@ Import Namespace="System.IO" %> 

<%
'SAMPLE CODE FOR LONGCODE INTEGRATION WITH YOUR SERVER.
'(c) COPYRIGHT SNOWEBS SOFTWARE TECHNOLOGIES PRIVATE LIMITED, INDIA
' WWW.SNOWEBS.CO.IN


Dim msisdn as String
Dim msg as String
Dim replymsg as String

'FIRST WE WILL GET THE POSTED VALUES IN TWO VARIABLES WHICH ARE POSTED BY SNOWEBS
msisdn=request.querystring("mb")
msg=request.querystring("ms")

//DECODE THE RECEIVED VALUES, AS SNOWEBS SUBMIT DATA IN URL ENCODED FORMAT
msisdn=HttpUtility.URLDecode(msisdn)
msg=HttpUtility.URLDecode(msg)

'NOW YOU CAN PROCESS THE VALUES USING YOUR LOGIC AND STORE INTO YOUR DATABASE


'YOUR CODE ENDS HERE


'DO NOT PUT ANY HTML CODE SYNTAX, IS WORKS ON WYSIWYG
'NOW PRINT @success@ TO CONFIRM SUCCESSFULL TRANSMISSION, DO NOT REMOVE THIS

Response.Write ("@success@")

'Put the reply message in variable replymsg

replymsg="Sender is " & msisdn & " and message is " & msg

'NOW PRINT @autoreply@ IF YOU WANT REPLY TO SENDER, YOU MUST UPDATE SNOWEBS FOR ACTIVATION OF THIS FEATURE
Response.Write ("@autoreply@ Service Response: ")
'AFTER THIS PRINT THE REPLY MESSAGE
Response.Write (replymsg)

%>




--%>