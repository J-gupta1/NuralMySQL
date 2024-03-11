﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="SupervisorDashboard.aspx.cs" Inherits="MobileWeb_common_SupervisorDashboard" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<%--<%@ Register Src="~/UserControls/ucServiceCenterListDvEx.ascx" TagPrefix="uc9" TagName="ucServiceCenterListDvEx" %>--%>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />--%>
    <meta http-equiv="Content-Security-Policy" content="block-all-mixed-content">
   <%-- <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=AIzaSyCaW0xQkMicdbnWGevDKB230pSZbY4cXFI&libraries=places&callback=initMap""></script>--%>
   <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=AIzaSyAd696VJUY9eD25AddY4GSvPaS8OB0U5Z4&libraries=places&callback=initMap""></script>
      
    <link href="../../Assets/ZedSales/Css/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <link href="../../Assets/ZedSales/Css/modal.css" rel="stylesheet" type="text/css" />
    <script src="../../Assets/ZedSales/Scripts/dhtmlwindow.js" type="text/javascript"></script>
    <script src="../../Assets/ZedSales/Scripts/modal.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <%--   <script type="text/javascript">

        function PopupEnigneerjobDetail(EngId,Date) {
            var WinCallingRequired = dhtmlmodal.open("WinPopup1", "iframe", "EngineerJobdetail.aspx?EngId=" + EngId + "&Date=" +Date, "Engineer Day Info", "width=995px,height=500px,top=5,resize=0,scrolling=No ,center=1")
            WinCallingRequired.onclose = function() {
                return true;
            }
            return false;
        }
    </script>--%>

    <script type="text/javascript">
        var markers = [
        <asp:Repeater ID="rptMarkers" runat="server">
        <ItemTemplate>
                 {
                     "title": '<%# Eval("FullName") %>',
                     "lat": '<%# Eval("Latitude") %>',
                     "lng": '<%# Eval("Longitude") %>',
                     "description": '<%# Eval("StartTime") %>'
                 }
    </ItemTemplate>
    <SeparatorTemplate>
    ,
    </SeparatorTemplate>
    </asp:Repeater>
        ];
    </script>
    <script type="text/javascript">
        window.onload = function () {
            var mapOptions = {
                //center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
                center: new google.maps.LatLng(28.7041, 77.1025),
                zoom: 13,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);
            var infoWindow = new google.maps.InfoWindow();
            var lat_lng = new Array();
            var latlngbounds = new google.maps.LatLngBounds();
            for (i = 0; i < markers.length; i++) {
                var data = markers[i]
                var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                lat_lng.push(myLatlng);   
                var marker = new google.maps.Marker({
                    position: myLatlng,  
                    icon:'https://qa.zed-axis.com/ZedSalesSAAS_qa//Assets/ZedSales/Css/Images/eng.png',
                    map: map,
                    title: data.title,
                    label: data.title
                });   
                latlngbounds.extend(marker.position);
                (function (marker, data) {
                    google.maps.event.addListener(marker, "click", function (e) {
                        infoWindow.setContent(data.description);
                        infoWindow.open(map, marker);
                    });
                })(marker, data);
            }
            map.setCenter(latlngbounds.getCenter());
            map.fitBounds(latlngbounds);
 
            //***********ROUTING****************//
 
            //Initialize the Path Array
            //var path = new google.maps.MVCArray();
 
            ////Initialize the Direction Service
            //var service = new google.maps.DirectionsService();
 
            ////Set the Path Stroke Color
            //var poly = new google.maps.Polyline({ map: map, strokeColor: '#4986E7', strokeOpacity: 1.0,
            //    strokeWeight: 2,
            //    geodesic: true });

           
 
            //Loop and Draw Path Route between the Points on MAP
            //for (var i = 0; i < lat_lng.length; i++) {
            //    if ((i + 1) < lat_lng.length) {
            //        var src = lat_lng[i];
            //        var des = lat_lng[i + 1];
            //        path.push(src);
            //        poly.setPath(path);
            //        service.route({
            //            origin: src,
            //            destination: des,
            //            travelMode: google.maps.DirectionsTravelMode.DRIVING
            //        }, function (result, status) {
            //            if (status == google.maps.DirectionsStatus.OK) {
            //                for (var i = 0, len = result.routes[0].overview_path.length; i < len; i++) {
            //                    path.push(result.routes[0].overview_path[i]);
            //                }
            //            }
            //        });
            //    }
            //}
        }

        //        window.onload = function () {
        //    if(markers.length>0)
        //{        
        //var mapOptions = {
        //                center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
        //                zoom: 13,
        //                mapTypeId: google.maps.MapTypeId.ROADMAP
        //            };
        //}
        //else
        //{
        //var mapOptions = {
        //                center: new google.maps.LatLng(28.7041, 77.1025),
        //                zoom: 13,
        //                mapTypeId: google.maps.MapTypeId.ROADMAP
        //            };
        //}
        //            var infoWindow = new google.maps.InfoWindow();
        //            var map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);
        //            for (i = 0; i < markers.length; i++) {
        //                var data = markers[i]
        //                var myLatlng = new google.maps.LatLng(data.lat, data.lng);
        //                var marker = new google.maps.Marker({
        //                    position: myLatlng,
        //                    //animation:google.maps.Animation.BOUNCE, 
        //                    icon:'https://imsats.firstdata.com/imsats/Assets/Images/Icon/eng.png',
        //                    map: map,
        //                    title: data.title,
        //                    label: data.title
        //                });
        //                (function (marker, data) {
        //                    google.maps.event.addListener(marker, "click", function (e) {
        //                        infoWindow.setContent(data.description);
        //                        infoWindow.open(map, marker);
        //                        var pos = map.getZoom();
        //                        map.setZoom(18);
        //                        map.setCenter(marker.getPosition());
        //                        window.setTimeout(function () { map.setZoom(pos); }, 6000);
        //                    });
        //                })(marker, data);

        //                //////(function (marker, data,map) {
        //                //    google.maps.event.addListener(marker, "doubleclick", function (e) {
        //                //        var pos = map.getZoom();
        //                //        map.setZoom(18);
        //                //        map.setCenter(marker.getPosition());
        //                //        window.setTimeout(function() {map.setZoom(pos);},6000);
        //                //    });
        //                //})(marker, data);

        //            }
        //        }
    </script>
    <%--<asp:UpdatePanel ID="updSearch" runat="server">
        <ContentTemplate>--%>
    <div class="box11">
        <div class="subheading">
            Search          
        </div>
        <uc4:ucMessage ID="ucMessage1" runat="server" />
        <div class="contentbox">
            <div class="H25-C3-S">
                <ul>
                    <%--  <li class="text"><%=Resources.ApplicationKeyword.ServiceCentre %> :<span class="mandatory-img">&nbsp;</span></li>
                    <li class="field7">
                        <uc9:ucServiceCenterListDvEx ID="ucServiceCentre" runat="server" AutoPostBack="false" ValidationGroup="val" IsRequired="true" InitialValue="Please Select" ErrorMessage="Please Select" />
                    </li>--%>

                    <li class="text">Date :<span class="mandatory-img">&nbsp;</span></li>
                    <li class="field">
                        <uc3:ucDatePicker ID="ucSearchDate" runat="server" IsRequired="true" ValidationGroup="val" ErrorMessage="Please Select Date" />
                    </li>

                    <li class="text">Type :<span class="optional-img">&nbsp;</span></li>
                    <li class="field">
                        <asp:DropDownList ID="ddlEntityType" runat="server" CssClass="formselect" ValidationGroup="val" AutoPostBack="true" OnSelectedIndexChanged="ddlEntityType_SelectedIndexChanged">
                        </asp:DropDownList>
                    </li>

                    <li class="text">User :<span class="optional-img">&nbsp;</span></li>
                    <li class="field">
                        <asp:DropDownList ID="ddlUser" runat="server" CssClass="formselect" ValidationGroup="val">
                        </asp:DropDownList>
                    </li>


                    <li class="text">
                        <%--<cc2:ZedButton ActionTag="View" ID="btnSearch" runat="server" Text="View Detail" CssClass="buttonbg" ValidationGroup="val" OnClick="btnSearch_Click" />--%>
                        <asp:Button ID="btnSearch" runat="server" Text="View Detail" CssClass="buttonbg" ValidationGroup="val" OnClick="btnSearch_Click" />
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updpnlGrid" UpdateMode="Always" runat="server">
        <ContentTemplate>--%>
    <div class="box11" runat="server" id="divPresentAbsent" style="display: none;">
        <div class="subheading">
            <div class="float-left">
                Present/Absent
            </div>
            <div class="float-right">
                <img id="imgPresentGreen" runat="server" src="../../Assets/Images/PresentGreen.png" />&nbsp;Present&nbsp;<img id="imgabsentred" runat="server" src="../../Assets/Images/AbsentRed.png" />&nbsp;Absent
              
            </div>
        </div>
        <div class="contentbox">
            <div class="grid1">
                <asp:DataList ID="DatalistEngineerAbsent" runat="server" EnableViewState="False" Width="100%" RepeatDirection="Horizontal" RepeatColumns="4"
                    CellPadding="0" CellSpacing="0" HeaderStyle-CssClass="" AlternatingItemStyle-HorizontalAlign="Left" AlternatingItemStyle-VerticalAlign="Top"
                    HeaderStyle-VerticalAlign="Middle" AlternatingItemStyle-CssClass="Altrow" ItemStyle-CssClass="gridrow" HorizontalAlign="Left" OnItemDataBound="DatalistEngineerAbsent_ItemDataBound">

                    <ItemTemplate>
                        <table width="100%" cellpadding="2" cellspacing="0" border="0" class="gridd">
                            <tr class="gridheader">
                                <th>
                                    <asp:Label ID="lblthUserNameHeading" runat="server"></asp:Label>
                                </th>

                            </tr>
                            <tr>
                                <td width="240" align="left" valign="top" height="20">
                                    <div class="float-margin">
                                        <%-- <img id="imgdtListEngineerAbsent" runat="server"  src="../../assets/images/absentred.png" />  --%>
                                        <asp:Image ID="imgdtListEngineerAbsent" runat="server" />
                                    </div>
                                    <div class="float-left">
                                        <%--   <img src="../../Assets/Images/AbsentRed.png" />--%>
                                        <asp:Label runat="server" ID="Label2" Text='<%# Eval("FullName")+" ("+ Eval("RoleName") +")" %>' />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UppnlEngDetail" UpdateMode="Always" runat="server">
        <ContentTemplate>--%>
    <div class="box11" runat="server" id="divgvEngDetail" style="display: none;">
        <div class="contentbox">
            <div class="grid1">
                <asp:GridView EmptyDataText="No data to display." EmptyDataRowStyle-CssClass="error"
                    GridLines="None" ID="gvEngDetail" runat="server" AllowPaging="false" AutoGenerateColumns="false"
                    BorderWidth="0px" CellPadding="4" CellSpacing="1" DataKeyNames="EntityID"
                    FooterStyle-HorizontalAlign="Left" FooterStyle-VerticalAlign="Top" HeaderStyle-CssClass="gridheader"
                    SelectedRowStyle-CssClass="selectedrow" HeaderStyle-HorizontalAlign="left" HeaderStyle-VerticalAlign="top"
                    RowStyle-CssClass="gridrow" AlternatingRowStyle-CssClass="Altrow" RowStyle-HorizontalAlign="left" RowStyle-VerticalAlign="top"
                    Width="100%" OnRowDataBound="gvEngDetail_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image ID="imggrdPresentGreen" runat="server" />
                                <%--<img src="../../Assets/Images/PresentGreen.png" />--%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Display Name" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDisplayName" runat="server" Text='<%# Eval("FullName")+" ("+ Eval("RoleName") +")" %>'></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Day Start Time">
                            <ItemTemplate>
                                <%#Eval("StartTime")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="First Call In-Time">
                            <ItemTemplate>
                                <%#Eval("FirstInTime")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Call In-Time">
                            <ItemTemplate>
                                <%#Eval("LastInTime")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Working Hour">
                            <ItemTemplate>
                                <%#Eval("Workinghour")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Jobs Visited">
                            <ItemTemplate>
                                <%#Eval("JobVisited")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Jobs Closed">
                            <ItemTemplate>
                                <%#Eval("JobClosed")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Call Out-Time">
                            <ItemTemplate>
                                <%#Eval("LastOutTime")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Day End Time">
                            <ItemTemplate>
                                <%#Eval("EndTime")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>

    <%--<asp:UpdatePanel ID="UpdatePanelMap" UpdateMode="Always" runat="server">
        <ContentTemplate>--%>
    <div class="box11">
        <div class="contentbox">
            <div id="dvMap" style="width: 100%; height: 600px">
            </div>
        </div>
    </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
