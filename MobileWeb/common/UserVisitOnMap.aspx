﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserVisitOnMap.aspx.cs" MasterPageFile="~/CommonMasterPages/MasterPage.master" Inherits="MobileWeb_common_UserVisitOnMap" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/UCPagingControl.ascx" TagName="UCPagingControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/ucTimePicker.ascx" TagName="ucTimePicker" TagPrefix="uc6" %>
<%@ Import Namespace="BussinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta http-equiv="Content-Security-Policy" content="block-all-mixed-content">
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=AIzaSyCaW0xQkMicdbnWGevDKB230pSZbY4cXFI"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">

    <script type="text/javascript">
        var markers = [
        <asp:Repeater ID="rptMarkers" runat="server">
        <ItemTemplate>
                     {
                         "title": '<%# Eval("FullName") %>',
                          "lat": '<%# Eval("Latitude") %>',
                          "lng": '<%# Eval("Longitude") %>',
                          "description": '<%# Eval("Description") %>'
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
                center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
                zoom: 15,
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
                if ((i + 1) == (markers.length)) {
                    var marker = new google.maps.Marker({
                        position: myLatlng,       
                        map: map,
                        title: data.title
                    });
                }
                else {
                   
                    if (i === 0) {
                        var marker = new google.maps.Marker({
                            position: myLatlng,  
                            icon:'https://qa.zed-axis.com/ZedSalesSAAS_qa//Assets/ZedSales/Css/Images/uservisit.png',
                            map: map,
                            title: data.title
                        });
                    }
                    else{
                        var marker = new google.maps.Marker
                        (
                        {
                            position: myLatlng,
                            icon:'https://qa.zed-axis.com/ZedSalesSAAS_qa//Assets/ZedSales/Css/Images/usergooglemap.png',
                            map: map,
                            title: data.title
                       
                        }
                        );}
                }
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
            var path = new google.maps.MVCArray();
 
            //Initialize the Direction Service
            var service = new google.maps.DirectionsService();
 
            //Set the Path Stroke Color
            var poly = new google.maps.Polyline({ map: map, strokeColor: '#4986E7', strokeOpacity: 1.0,
                strokeWeight: 2,
                geodesic: true });

           
 
            //Loop and Draw Path Route between the Points on MAP
            for (var i = 0; i < lat_lng.length; i++) {
                if ((i + 1) < lat_lng.length) {
                    var src = lat_lng[i];
                    var des = lat_lng[i + 1];
                    path.push(src);
                    poly.setPath(path);
                    service.route({
                        origin: src,
                        destination: des,
                        travelMode: google.maps.DirectionsTravelMode.DRIVING
                    }, function (result, status) {
                        if (status == google.maps.DirectionsStatus.OK) {
                            for (var i = 0, len = result.routes[0].overview_path.length; i < len; i++) {
                                path.push(result.routes[0].overview_path[i]);
                            }
                        }
                    });
                }
            }
        }


       
    </script>

    <div>
        <div>
            <uc4:ucMessage ID="ucMessage1" runat="server" />
            <div class="clear"></div>
            <div class="mainheading">
                View  Visit Detail 
            </div>
            <div class="contentbox">
                <div class="mandatory">
                    (*) Marked fields are mandatory. (+) Fill at least one of them.
                </div>
                <div class="H25-C3-S">
                    <ul>
                        <li class="text">Entity Type:<font class="error">*</font>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList CssClass="formselect" ID="ddlEntityType" AutoPostBack="true" OnSelectedIndexChanged="ddlEntityType_SelectedIndexChanged" runat="server">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlEntityType"
                                    CssClass="error" ErrorMessage="Please select Entity Type" InitialValue="0" ValidationGroup="Search" />
                            </div>
                        </li>
                        <li class="text">Entity Type Name:<font class="error">*</font>
                        </li>
                        <li class="field">
                            <div>
                                <asp:DropDownList ID="ddlEntityTypeName" CssClass="formselect" runat="server">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ddlEntityTypeName"
                                    CssClass="error" ErrorMessage="Please select Entity Type Name" InitialValue="0" ValidationGroup="Search" />
                            </div>
                        </li>
                        <li class="text">From Date:<font class="error">*</font>
                        </li>
                        <li class="field">
                            <uc1:ucDatePicker ID="ucFromDate" ErrorMessage="Invalid from date." ValidationGroup="SearchNo" IsRequired="true"
                                runat="server" />
                        </li>
                        <li class="text">To Date:<font class="error">*</font>
                        </li>
                        <li class="field">
                            <uc1:ucDatePicker ID="ucToDate" ErrorMessage="Invalid to date." ValidationGroup="SearchNo" IsRequired="true"
                                runat="server" />
                        </li>
                        <li class="text">From Time:</li>
                        <li class="field">
                            <uc6:ucTimePicker ID="ucInTime" runat="server" CssClass="formselect" RangeErrorMessage="Invalid time!"
                                IsRequired="true" />
                        </li>
                        <li class="text">To Time:</li>
                        <li class="field">
                            <uc6:ucTimePicker ID="ucOutTime" runat="server" CssClass="formselect" RangeErrorMessage="Invalid time!"
                                IsRequired="true" />
                        </li>
                        <li class="field3">
                            <div class="float-margin">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonbg"
                                    CausesValidation="true" ValidationGroup="Search" OnClick="btnSearch_Click" />
                            </div>
                            <div class="float-margin">
                                <asp:Button ID="btnCancel" runat="server" Text="Reset" CssClass="buttonbg"
                                    CausesValidation="false" OnClick="btnCancel_Click" />
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="box11">
            <div class="contentbox" runat="server">
                <div id="dvMap" style="width: 100%; height: 600px">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
