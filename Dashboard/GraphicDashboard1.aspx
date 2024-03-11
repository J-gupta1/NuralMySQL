<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GraphicDashboard1.aspx.cs" Inherits="Dashboard_GraphicDashboard1" EnableViewState="true" %>

<%@ Register Src="~/UserControls/HeaderResponsive.ascx" TagPrefix="uc1" TagName="Header" %>


<!DOCTYPE html>

<html>
<head>
     <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Graphic Dashboard</title>

    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.0.3.min.js"></script>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
     <%--<link href="../Assets/Karbonn/CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="../Assets/Karbonn/CSS/GraphicDashboard.css" rel="stylesheet" />--%>


    <link id="lnkBootstrap" rel="stylesheet" type="text/css" runat="server" />
    <link id="lnkGraphics" rel="stylesheet" type="text/css" runat="server" />
    <%--state wise MTD Stock report start--%>
    <script type="text/javascript">
        window.onload = function () {
         
TempPost();
        };
        
       

function TempPost() {

            $.ajax({

                url: "http://salemobileapp.zedsales.in/api/user/temppost",
                type: "POST",
                dataType: 'json',
                 contentType: "application/json; charset=utf-8",
                success: function (data) {
                    alert('success on TempPost'); // calling method  

                },
                error: function () {
                    alert('error on TempPost');
                }
            })
        };

    </script>

</head>
<body>
    <form id="form1" runat="server"/>
       
</body>

</html>
