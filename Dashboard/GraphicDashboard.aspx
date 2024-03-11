<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GraphicDashboard.aspx.cs" Inherits="Dashboard_GraphicDashboard" EnableViewState="true" %>

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
            URI = document.getElementById('<%=hdnAPIURL.ClientID%>').value;
            vrAPIConnStringKey = document.getElementById('<%=hdnConstr.ClientID%>').value;
            vrUserID = document.getElementById('<%=hdnRoleID.ClientID%>').value;
            vrRoleID = document.getElementById('<%=hdnUserID.ClientID%>').value;

        };
         <%--   var URI = "http://localhost/ZedSalesAppAPI/api/user/";  --%>

        var URI = "";
        var vrAPIConnStringKey = "";
        var vrUserID = "";
        var vrRoleID = "";




    <%--    function GetMapData() {

            $.ajax({

                url: URI + "GetStateWiseMTDData",
                type: "POST",
                dataType: 'json',
                headers: { 'ConnStringKey': vrAPIConnStringKey, "UserID": vrUserID, "RoleID": vrRoleID },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    drawRegionsMap(data); // calling method  

                },
                error: function () {
                    alert('error');
                }
            })
        };


        google.charts.load('current', {
            'packages': ['geochart'],
            // Note: you will need to get a mapsApiKey for your project.
            // See: https://developers.google.com/chart/interactive/docs/basic_load_libs#load-settings
            'mapsApiKey': 'AIzaSyD-9tSrke72PouQMnMX-a7eZSW0jkFMBWY'
        });
        google.charts.setOnLoadCallback(GetMapData);

        function drawRegionsMap(AllData) {
            //    alert("hello");
            var dataValues = AllData.d.ItemLists;
            var data = new google.visualization.DataTable();

            data.addColumn('string', 'State');
            data.addColumn('number', 'Qty');

            for (var i = 0; i < dataValues.length; i++) {
                data.addRow([dataValues[i].State, dataValues[i].Qty]);
            }


            var options = {
                height: 380,
                region: 'IN', // Africa
                resolution: 'provinces',
                colorAxis: { colors: ['#e31b23', 'yellow', '#00853f'] },
                backgroundColor: '#81d4fa',
                datalessRegionColor: '#ffffff',
                defaultColor: '#f0f0f0',
            };

            var chart = new google.visualization.GeoChart(document.getElementById('StateWiseMTD_div'));
            chart.draw(data, options);
        };

        state wise MTD Stock report END--%>
      <%--Target vs Achievement Start --%>
        google.charts.load('current', { 'packages': ['gauge'] });
        google.charts.setOnLoadCallback(GetTargetAchievement);

        function GetTargetAchievement() {
            $.ajax({
                url: URI + "/GetTargetAchievementData",
                type: "POST",
                dataType: 'json',
                headers: { 'ConnStringKey': vrAPIConnStringKey, "UserID": vrUserID, "RoleID": vrRoleID },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    drawTargetAchievement(data); // calling method                    
                },
                error: function () {
                    alert('error TargetAchievement');
                }
            })
        };
        function drawTargetAchievement(AllData) {
            //    alert("hello");
            var TargetAchievemetValues = AllData.d.ItemLists;

            var data = new google.visualization.DataTable();
            data.addColumn('string', 'TargetName');
            // data.addColumn('string', 'TargetBase');
            data.addColumn('number', 'TargetAchieve');

            for (var i = 0; i < TargetAchievemetValues.length; i++) {
                data.addRow([TargetAchievemetValues[i].TargetName, TargetAchievemetValues[i].TargetAchieve]);
            }

            var options = {
                width: 960, height: 115,
                redFrom: 0, redTo: 20,
                yellowFrom: 20, yellowTo: 80,
                greenFrom: 80, greenTo: 100,
                minorTicks: 5,
                
                animation: {
                    duration: 4000,
                    easing: 'in',
                }
            };

            var chart = new google.visualization.Gauge(document.getElementById('TargetVsAchievement_div'));
            chart.draw(data, options);
        };
        <%--Target vs Achievement end --%>            
           <%--Top Models Start --%>
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(GetTopModels);

        function GetTopModels() {

            $.ajax({
                url: URI + "/GetTopModels",
                type: "POST",
                dataType: 'json',
                headers: { 'ConnStringKey': vrAPIConnStringKey, "UserID": vrUserID, "RoleID": vrRoleID },
                contentType: "application/json; charset=utf-8",
              
                success: function (data) {
                    drawTopModels(data); // calling method                    
                },
                error: function () {
                    alert('error GetTopModels');
                }
            })
        };
        function drawTopModels(AllData) {
            //    alert("hello");
            var TopModels = AllData.d.ItemLists;

            var data = new google.visualization.DataTable();
            data.addColumn('string', 'ModelName');
            // data.addColumn('string', 'TargetBase');
            data.addColumn('number', 'SalePercentage');

            for (var i = 0; i < TopModels.length; i++) {
                data.addRow([TopModels[i].ModelName, TopModels[i].SalePercentage]);
            }

            var options = {
                legend: { position: 'right', width: "100%", alignment: 'start', textStyle: { color: 'black', fontSize: 9 } },
                chartArea: { left: 0, top: 5, width: "80%", height: "100%" },
                title: '',
                is3D: true,
                left: 0,


                animation: {
                    duration: 4000,
                    easing: 'in',
                }
            };

            var chart = new google.visualization.PieChart(document.getElementById('piechart_TopModel'));
            chart.draw(data, options);
        };
        <%--Top Models end --%>     
               <%--Last Day Stock --%>
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(GetLastDayStock);

        function GetLastDayStock() {
            $.ajax({
                url: URI + "/GetLastDayStock",
                type: "POST",
                dataType: 'json',
                headers: { 'ConnStringKey': vrAPIConnStringKey, "UserID": vrUserID, "RoleID": vrRoleID },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    drawLastDayStock(data); // calling method                    
                },
                error: function () {
                    alert('error GetLastDayStock');
                }
            })
        };
        function drawLastDayStock(AllData) {
            //    alert("hello");
            var LastDayStock = AllData.d.ItemLists;

            var data = new google.visualization.DataTable();
            data.addColumn('string', 'SaleChannelType');
            // data.addColumn('string', 'TargetBase');
            data.addColumn('number', 'StockPercantage');

            var dataforTable = new google.visualization.DataTable();
            dataforTable.addColumn('string', 'SaleChannelType');
            // data.addColumn('string', 'TargetBase');            
            dataforTable.addColumn('number', 'Qty');
            dataforTable.addColumn('number', 'StockPercantage');

            for (var i = 0; i < LastDayStock.length; i++) {
                data.addRow([LastDayStock[i].SaleChannelType, LastDayStock[i].StockPercantage]);
                dataforTable.addRow([LastDayStock[i].EntityType, LastDayStock[i].StockQty, LastDayStock[i].StockPercantage]);

            }

            var options = {
                legend: { position: 'right', width: "100%", alignment: 'start', textStyle: { color: 'black', fontSize: 9 } },
                chartArea: { left:0, top:10, width:"90%", height:"100%"},
                title: '',
                is3D: false,
                pieHole: 0.4,
                animation: {
                    duration: 4000,
                    easing: 'in',
                }
            };

            var chart = new google.visualization.PieChart(document.getElementById('piechart_lastdayStock'));
            chart.draw(data, options);

            google.charts.load('current', { 'packages': ['table'] });
            var table1 = new google.visualization.Table(document.getElementById('table_lastdayStock'));

            table1.draw(dataforTable, { showRowNumber: false, width: '100%', height: '100%' });

        };
        <%--Last Day Stock end --%>     
           
             <%--Sale data in table--%>
        google.charts.load('current', { 'packages': ['table'] });
        google.charts.setOnLoadCallback(GetSaleData);

        function GetSaleData() {
            $.ajax({
                url: URI + "/GetSaleData",
                type: "POST",
                dataType: 'json',
                headers: { 'ConnStringKey': vrAPIConnStringKey, "UserID": vrUserID, "RoleID": vrRoleID },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    drawSaleData(data); // calling method                    
                },
                error: function () {
                    alert('error GetSaleData');
                }
            })
        };
        function drawSaleData(AllData) {
            //    alert("hello");
            var AllSaleData = AllData.d.ItemLists;

            var data = new google.visualization.DataTable();
            data.addColumn('string', 'SaleType');
            data.addColumn('number', 'MTD');
            data.addColumn('number', 'LMTD');
            data.addColumn('number', 'Last Month');

            for (var i = 0; i < AllSaleData.length; i++) {
                data.addRow([AllSaleData[i].SaleType, AllSaleData[i].MTD, AllSaleData[i].LMTD, AllSaleData[i].LMSale]);
            }

            var table = new google.visualization.Table(document.getElementById('AllSaleData'));

            table.draw(data, { showRowNumber: false, width: '100%', height: '100%' });

        };
        <%--Sale data in table end --%>     
     <%--Top Distributors in table--%>
        google.charts.load('current', { 'packages': ['table'] });
        google.charts.setOnLoadCallback(GetTopDistributor);

     function GetTopDistributor() {
            $.ajax({
                url: URI + "/GetTopDistributor",
                type: "POST",
                dataType: 'json',
                headers: { 'ConnStringKey': vrAPIConnStringKey, "UserID": vrUserID, "RoleID": vrRoleID },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    drawGetTopDistributor(data); // calling method                    
                },
                error: function () {
                    alert('error TopDistributor');
                }
            })
        };
        function drawGetTopDistributor(AllData) {
            //    alert("hello");
            
           var GetTopDistributor = AllData.d.ItemLists;

            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Distibutor Name');
            // data.addColumn('string', 'TargetBase');
            data.addColumn('number', 'Sale');

            for (var i = 0; i < GetTopDistributor.length; i++) {
                data.addRow([GetTopDistributor[i].DistName, GetTopDistributor[i].TertSale]);
            }

            var table = new google.visualization.Table(document.getElementById('TopDist_div'));

            table.draw(data, { showRowNumber: false, width: '100%', height: '100%' });

        };
        <%--Top Distributors in table end --%>   
            <%--Top Distributors in Graph mode--%>
       <%-- google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(GetTopDistributor);

        function GetTopDistributor() {
            $.ajax({
                url: URI + "/GetTopDistributor",
                type: "POST",
                dataType: 'json',
                headers: { 'ConnStringKey': vrAPIConnStringKey, "UserID": vrUserID, "RoleID": vrRoleID },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    drawGetTopDistributor(data); // calling method                    
                },
                error: function () {
                    alert('error TopDistributor');
                }
            })
        };

        function drawGetTopDistributor(AllData) {
            //    alert("hello");
            var GetTopDistributor = AllData.d.ItemLists;

            var data = new google.visualization.DataTable();
            data.addColumn('string', 'DistName');
            // data.addColumn('string', 'TargetBase');
            data.addColumn('number', 'SecondarySale');

            for (var i = 0; i < GetTopDistributor.length; i++) {
                data.addRow([GetTopDistributor[i].DistName, GetTopDistributor[i].TertSale]);
            }

            var options = {
                legend: { position: 'top', maxLines: 2 },
                chart: {
                    
                    title: 'Top Distributors',
                    subtitle: 'Top Distributors on the basis of MTD tertiary sale',
                },
                bars: 'vertical',
                vAxis: { format: 'decimal' },
                colors: ['#1b9e77', '#d95f02', '#7570b3']
            };



            var chart = new google.visualization.ColumnChart(document.getElementById('TopDist_div'));
            chart.draw(data, options);
        };--%>
        <%--Top Distributors in Graph mode end --%>   
              <%--WOD Last few months--%>
        google.charts.load('current', { 'packages': ['corechart'] });
       // google.charts.setOnLoadCallback(GetWODLastMonths);

        function GetWODLastMonths() {
            $.ajax({
                url: URI + "/GetWODLastMonths",
                type: "POST",
                dataType: 'json',
                headers: { 'ConnStringKey': vrAPIConnStringKey, "UserID": vrUserID, "RoleID": vrRoleID },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    drawGetWODLastMonths(data); // calling method                    
                },
                error: function () {
                    alert('error WODLastMonths');
                }
            })
        };

        function drawGetWODLastMonths(AllData) {
            //    alert("hello");
            var GetWODLastMonths = AllData.d.ItemLists;

            var data = new google.visualization.DataTable();
            data.addColumn('string', 'MonthYear');
            data.addColumn('number', 'OldWOD');
            data.addColumn('number', 'NewWOD');

            for (var i = 0; i < GetWODLastMonths.length; i++) {
                data.addRow([GetWODLastMonths[i].MonthYear, GetWODLastMonths[i].OldWOD, GetWODLastMonths[i].NewWOD]);
            }

            var options = {
                legend: { position: 'top', maxLines: 2 },
                chart: {
                    title: 'WOD Last few months',
                    subtitle: 'Old and new WOD',
                },
                bars: 'vertical',
                vAxis: { format: 'decimal' },
                colors: ['#1b9e77', '#d95f02', '#7570b3']
            };



            var chart = new google.visualization.ColumnChart(document.getElementById('WOD_div'));
            chart.draw(data, options);
        };
        <%--WOD Last few months end --%>  
             <%--Last Vs Current month Sale--%>
        google.charts.load('current', { 'packages': ['annotationchart'] });
        google.charts.setOnLoadCallback(GetCurrentVsLastMonthSale);

        function GetCurrentVsLastMonthSale() {
            $.ajax({
                url: URI + "/GetCurrentVsLastMonthSale",
                type: "POST",
                dataType: 'json',
                headers: { 'ConnStringKey': vrAPIConnStringKey, "UserID": vrUserID, "RoleID": vrRoleID },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    drawGetCurrentVsLastMonthSale(data); // calling method                    
                },
                error: function () {
                    alert('error CurrentVsLastMonthSale');
                }
            })
        };

        function drawGetCurrentVsLastMonthSale(AllData) {
            //    alert("hello");
            var GetCurrentVsLastMonthSale = AllData.d.ItemLists;

          var data = new google.visualization.DataTable();
            data.addColumn('date', 'SaleDate');
            data.addColumn('number', 'LastMonthSale');
            data.addColumn('number', 'CurrentMonthSale');

            for (var i = 0; i < GetCurrentVsLastMonthSale.length; i++) {
                data.addRow([new Date(GetCurrentVsLastMonthSale[i].SaleDate), GetCurrentVsLastMonthSale[i].LastMonthSale, GetCurrentVsLastMonthSale[i].CurrentMonthSale]);
            }



            var options = {
                displayAnnotations: false
            };

            var chart = new google.visualization.AnnotationChart(document.getElementById('CurrentVsLastMonthSale_div'));
            chart.draw(data, options);
        };
        <%--Last Vs Current month Sale end --%>  
            <%--primary vs secondary vs intermediary vs tertiary month Sale--%>
        google.charts.load('current', { 'packages': ['annotationchart'] });
        google.charts.setOnLoadCallback(GetAllSaleDetail);

        function GetAllSaleDetail() {
            $.ajax({
                url: URI + "/GetAllSaleDetail",
                type: "POST",
                dataType: 'json',
                headers: { 'ConnStringKey': vrAPIConnStringKey, "UserID": vrUserID, "RoleID": vrRoleID },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    drawGetAllSaleDetail(data); // calling method                    
                },
                error: function () {
                    alert('error AllSaleDetail');
                }
            })
        };

 
          
        function drawGetAllSaleDetail(AllData) {
            //    alert("hello");
            var GetAllSaleDetail = AllData.d.ItemLists;

            var data = new google.visualization.DataTable();
            data.addColumn('date', 'SaleDate');
            data.addColumn('number', 'PrimarySale');
            data.addColumn('number', 'IntermediarySale');
            data.addColumn('number', 'SecondarySale');
            data.addColumn('number', 'TertiarySale');


          var PrimaryData = new google.visualization.DataTable();
            PrimaryData.addColumn('date', 'SaleDate');
            PrimaryData.addColumn('number', 'PrimarySale');

 var IntermediaryData = new google.visualization.DataTable();
            IntermediaryData.addColumn('date', 'SaleDate');
            IntermediaryData.addColumn('number', 'IntermediarySale');

 var SecondaryData = new google.visualization.DataTable();
            SecondaryData.addColumn('date', 'SaleDate');
            SecondaryData.addColumn('number', 'SecondarySale');
          
   var TertiaryData = new google.visualization.DataTable();
            TertiaryData.addColumn('date', 'SaleDate');
            TertiaryData.addColumn('number', 'TertiarySale');
          


            for (var i = 0; i < GetAllSaleDetail.length; i++) {
                data.addRow([new Date(GetAllSaleDetail[i].SaleDate), GetAllSaleDetail[i].PrimarySale, GetAllSaleDetail[i].IntermediarySale, GetAllSaleDetail[i].SecondarySale, GetAllSaleDetail[i].TertiarySale]);
   PrimaryData.addRow([new Date(GetAllSaleDetail[i].SaleDate), GetAllSaleDetail[i].PrimarySale]);
 IntermediaryData.addRow([new Date(GetAllSaleDetail[i].SaleDate), GetAllSaleDetail[i].IntermediarySale]);
SecondaryData.addRow([new Date(GetAllSaleDetail[i].SaleDate), GetAllSaleDetail[i].SecondarySale]);
      TertiaryData.addRow([new Date(GetAllSaleDetail[i].SaleDate), GetAllSaleDetail[i].TertiarySale]);
//data.addRow([new Date(2314, 2, [i]), GetAllSaleDetail[i].PrimarySale, GetAllSaleDetail[i].IntermediarySale, GetAllSaleDetail[i].SecondarySale, GetAllSaleDetail[i].TertiarySale]);

            }

            var options = {
                displayAnnotations: true
            };
            var chart = new google.visualization.AnnotationChart(document.getElementById('AllSaleDetail_div'));
            chart.draw(data, options);

 <%--Sale Trend - primary  
          --%>
            var Primarychart = new google.visualization.AnnotationChart(document.getElementById('SalesTrendPrimary_div'));
            Primarychart.draw(PrimaryData, options);
<%--End Sale Trend - primary  --%>
 <%--Sale Trend - Intermediary  --%>
    
          
            var IntermediaryChart = new google.visualization.AnnotationChart(document.getElementById('SalesTrendIntermediary_div'));
            IntermediaryChart.draw(IntermediaryData, options);
<%--End Sale Trend - Intermediary  --%>

 <%--Sale Trend - Secondary  --%>
     
            var SecondaryChart = new google.visualization.AnnotationChart(document.getElementById('SalesTrendSecondary_div'));
            SecondaryChart.draw(SecondaryData, options);
<%--End Sale Trend - Secondary  --%>
<%--Sale Trend - Tertiary  --%>
   
            var TertiaryChart = new google.visualization.AnnotationChart(document.getElementById('SalesTrendTertiary_div'));
            TertiaryChart.draw(TertiaryData, options);
<%--End Sale Trend - Tertiary  --%>
        };
        <%--primary vs secondary vs intermediary vs tertiary month Sale: END--%>
             <%--Last Vs Current year Sale--%>
        google.charts.load('current', { 'packages': ['annotationchart'] });
        google.charts.setOnLoadCallback(GetCurrentVsLastYearSale);

        function GetCurrentVsLastYearSale() {
            $.ajax({
                url: URI + "/GetCurrentVsLastYearSale",
                type: "POST",
                dataType: 'json',
                headers: { 'ConnStringKey': vrAPIConnStringKey, "UserID": vrUserID, "RoleID": vrRoleID },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    drawGetCurrentVsLastYearSale(data); // calling method                    
                },
                error: function () {
                    alert('error CurrentVsLastYearSale');
                }
            })
        };

        function drawGetCurrentVsLastYearSale(AllData) {           
            var GetCurrentVsLastYearSale = AllData.d.ItemLists;

            var data = new google.visualization.DataTable();
            data.addColumn('date', 'SaleDate');
            data.addColumn('number', 'LastYearSale');
            data.addColumn('number', 'CurrentYearSale');
//data.addColumn('number', 'DayValue');
//data.addColumn('number', 'MonthValue');
//data.addColumn('number', 'YearValue');


            for (var i = 0; i < GetCurrentVsLastYearSale.length; i++) {
  data.addRow([new Date(GetCurrentVsLastYearSale[i].SaleDate), GetCurrentVsLastYearSale[i].LastYearSale, GetCurrentVsLastYearSale[i].CurrentYearSale]);              
//  data.addRow([new Date(GetCurrentVsLastYearSale[i].YearValue,GetCurrentVsLastYearSale[i].MonthValue,GetCurrentVsLastYearSale[i].DayValue), GetCurrentVsLastYearSale[i].LastYearSale, GetCurrentVsLastYearSale[i].CurrentYearSale]);
            }

            var options = {
                height: 190,
                displayAnnotations: false
               
            };

            var chart = new google.visualization.AnnotationChart(document.getElementById('CurrentVsLastYearSale_div'));
            chart.draw(data, options);
        };
        <%--Last Vs Current Year Sale end --%> 
    <%--Region wise sale Start --%>
       google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(GetRegionWiseSale);

        function GetRegionWiseSale() {

            $.ajax({
                url: URI + "/GetRegionalSale",
                type: "POST",
                dataType: 'json',
                headers: { 'ConnStringKey': vrAPIConnStringKey, "UserID": vrUserID, "RoleID": vrRoleID },
                contentType: "application/json; charset=utf-8",
              
                success: function (data) {
                    drawRegionWiseSale(data); // calling method                    
                },
                error: function () {
                    alert('error GetRegionalSale');
                }
            })
        };
        function drawRegionWiseSale(AllData) {
            //    alert("hello");
            var RegionWiseSaleData = AllData.d.ItemLists;

            var Saledata = new google.visualization.DataTable();
            Saledata.addColumn('string', 'Region Name');
            // data.addColumn('string', 'TargetBase');
            Saledata.addColumn('number', 'Sale Percentage');

            for (var i = 0; i < RegionWiseSaleData.length; i++) {
                Saledata.addRow([RegionWiseSaleData[i].RegionName, RegionWiseSaleData[i].Sale]);
            }

            var options = {
                legend: { position: 'right', width: "100%", alignment: 'start', textStyle: { color: 'black', fontSize: 9 } },
              
            };

            var RegionPieChart = new google.visualization.PieChart(document.getElementById('RegionWiseSale_div'));
            RegionPieChart.draw(Saledata, options);
        };
        <%-- Region wise sale end --%> 
             <%--Last 6 months sale--%>
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(GetLast6MonthsSale);

        function GetLast6MonthsSale() {
            $.ajax({
                url: URI + "/GetLast6MonthsSale",
                type: "POST",
                dataType: 'json',
                headers: { 'ConnStringKey': vrAPIConnStringKey, "UserID": vrUserID, "RoleID": vrRoleID },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    drawGetLast6MonthsSale(data); // calling method                    
                },
                error: function () {
                    alert('error drawGetLast6MonthsSale');
                }
            })
        };

        function drawGetLast6MonthsSale(AllData) {
            //    alert("hello");
            var GetLast6MonthsSale = AllData.d.ItemLists;

            var data = new google.visualization.DataTable();
            data.addColumn('string', 'MonthYear');
            data.addColumn('number', 'PrimarySale');
            data.addColumn('number', 'SecondarySale');
            data.addColumn('number', 'IntermediarySale');
            data.addColumn('number', 'TertiarySale');

            for (var i = 0; i < GetLast6MonthsSale.length; i++) {
                data.addRow([GetLast6MonthsSale[i].MonthYear, GetLast6MonthsSale[i].PrimarySale, GetLast6MonthsSale[i].SecondarySale, GetLast6MonthsSale[i].IntermediarySale, GetLast6MonthsSale[i].TertiarySale]);
            }

            var options = {
                legend: { position: 'top', maxLines: 2 },
                chart: {
                    title: 'Last Six months Sale',
                    subtitle: 'Sale Data',
                },
                bars: 'vertical',
                vAxis: { format: 'decimal' },
                colors: ['#1b9e77', '#d95f02', '#7570b3']
            };



            var chart = new google.visualization.ColumnChart(document.getElementById('6MonthSale_div'));
            chart.draw(data, options);
        };
        <%--WOD Last few months end --%> 
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <uc1:Header runat="server" ID="Header" />
        <asp:HiddenField ID="hdnConstr" runat="server" />
        <asp:HiddenField ID="hdnAPIURL" runat="server" />
        <asp:HiddenField ID="hdnUserID" runat="server" />
        <asp:HiddenField ID="hdnRoleID" runat="server" />

        <asp:HiddenField ID="hdnEntityID" runat="server" />
        <asp:HiddenField ID="hdnEntityTypeID" runat="server" />
    </form>

    <div class="dash_container">
        <div class="dashboard">
             <div class="part_1">
                <div class="padding">
                    <asp:Label ID="Label1" runat="server" CssClass="title" Text="Sale Data"></asp:Label>
                    <div id="AllSaleData" style="height:190px"></div>
                </div>
            </div>
               <div class="part_1">
                <div class="padding">
                    <asp:Label ID="Label16" runat="server" CssClass="title" Text="Yesterday Stock"></asp:Label>
                    <div id="table_lastdayStock" style="height:190px"></div>
                </div>
            </div>
             <div class="part_3">
                <div class="padding">
                   <asp:Label ID="Label6" runat="server" CssClass="title" Text="Y-o-Y (Tertiary Sale)"></asp:Label>
                    <div id="CurrentVsLastYearSale_div"></div> 
                </div>
            </div>
              <div class="part_2">
                <div class="padding">
                    <asp:Label ID="Label9" runat="server" CssClass="title" Text="Top Model"></asp:Label>
                    <div id="piechart_TopModel"></div>
                </div>
            </div>
             <div class="part_5">
                <div class="padding">
                    <asp:Label ID="Label3" runat="server" CssClass="title" Text="Current Vs Last Month Sale"></asp:Label>
                    <div id="CurrentVsLastMonthSale_div"></div>
                </div>
            </div>
            <div class="part_4">
                <div class="padding">
                    <asp:Label ID="Label4" runat="server" CssClass="title" Text="Sale Trend Combined"></asp:Label>
                    <div id="AllSaleDetail_div"></div>
                </div>
            </div>
                <div class="part_4">
                <div class="padding">
                    <asp:Label ID="Label11" runat="server" CssClass="title" Text="Sales Trend - Primary"></asp:Label>
                    <div id="SalesTrendPrimary_div"></div>
                </div>
            </div>
       <div class="part_5">
                <div class="padding">
                    <asp:Label ID="lblInter" runat="server" CssClass="title" Text="Sales Trend - Intermediary"></asp:Label>
                    <div id="SalesTrendIntermediary_div"></div>
                </div>
            </div>
              <div class="part_5">
                <div class="padding">
                    <asp:Label ID="Label5" runat="server" CssClass="title" Text="Yesterday Stock"></asp:Label>
                    <div id="piechart_lastdayStock"></div>
                </div>
            </div>
               <div class="part_4">
                <div class="padding">
                    <asp:Label ID="Label12" runat="server" CssClass="title" Text="Sales Trend - Secondary"></asp:Label>
                    <div id="SalesTrendSecondary_div"></div>
                </div>
            </div>
               <div class="part_5">
                <div class="padding">
                    <asp:Label ID="Label13" runat="server" CssClass="title" Text="Sales Trend - Tertiary"></asp:Label>
                    <div id="SalesTrendTertiary_div"></div>
                </div>
            </div>
             <div class="part_5">
                <div class="padding">
                    <asp:Label ID="Label8" runat="server" CssClass="title" Text="Top 7 Distributors (MTD)"></asp:Label>
                    <div id="TopDist_div"></div>
                </div>
            </div>


            <div class="part_11" style="display: block">
                <div class="padding">
                    <asp:Label ID="Label2" runat="server" CssClass="title" Text="Target Vs Achievement"></asp:Label>
                    <div id="TargetVsAchievement_div" style="overflow: auto; overflow-y: hidden; position: relative;"></div>

                </div>
            </div>

            <div class="part_9">
                <div class="padding">
                    <asp:Label ID="Label15" runat="server" CssClass="title" Text="Last Six Months Sale"></asp:Label>
                    <div id="6MonthSale_div"></div>
                </div>
            </div> 
                    
              <div class="part_11">
                <div class="padding">
                    <asp:Label ID="Label14" runat="server" CssClass="title" Text="Region Wise Sale (MTD Secondary+Intermediary)"></asp:Label>
                    <div id="RegionWiseSale_div"></div>
                </div>
            </div>

            <div class="part_10" style="display:none">
                <div class="padding" ">
                    <asp:Label ID="Label10" runat="server" CssClass="title" Text="WOD"></asp:Label>
                    <div id="WOD_div"></div>
                </div>
            </div>
               
            <div class="part_11" style="display:none">
                <div class="padding">
                    <asp:Label ID="Label7" runat="server" CssClass="title" Text="State Wise MTD"></asp:Label>
                    <div id="StateWiseMTD_div"></div>
                </div>
            </div>
        
        </div>
    </div>
    <div class="clear"></div>
    <div class="footer">&copy; Zed-Axis Technologies 2018-19. All rights reserved.</div>
</body>

</html>
