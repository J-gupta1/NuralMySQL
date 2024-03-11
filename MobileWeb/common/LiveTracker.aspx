<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CommonMasterPages/MasterPage.master" CodeFile="LiveTracker.aspx.cs" Inherits="MobileWeb_common_LiveTracker" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ucMessage.ascx" TagName="ucMessage" TagPrefix="uc4" %>
<%@ Register Assembly="ZedControlLib" Namespace="ZedControlLib" TagPrefix="cc2" %>
<%@ Register Src="~/UserControls/ucDatePicker.ascx" TagName="ucDatePicker" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta http-equiv="Content-Security-Policy" content="block-all-mixed-content">
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=AIzaSyCaW0xQkMicdbnWGevDKB230pSZbY4cXFI&libraries=places&callback=initMap""></script>
    <%--<link href="../../Assets/Css/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <link href="../../Assets/Css/modal.css" rel="stylesheet" type="text/css" />
    <script src="../../Assets/Scripts/dhtmlwindow.js" type="text/javascript"></script>
    <script src="../../Assets/Scripts/modal.js" type="text/javascript"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <script type="text/javascript">
        var markers = [
        <asp:Repeater ID="rptMarkers" runat="server">
        <ItemTemplate>
                 {
                     "title": '<%# Eval("StateName") %>',
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
                    icon:'http://dms.zedsales.in/Assets/ZedSales/Css/Images/StateWiseTracker.png',
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
        }

       
    </script>
    
    <div class="box11">
        <div class="subheading">
            Statewise Live Tracker          
        </div>
        <uc4:ucMessage ID="ucMessage1" runat="server" />
      <div class="contentbox">
            <div class="H25-C3-S">
                <ul>
                    <li class="text">Date :<span class="mandatory-img">&nbsp;</span></li>
                    <li class="field">
                        <uc3:ucDatePicker ID="ucSearchDate" runat="server" IsRequired="true" ValidationGroup="val" IsEnabled="false" ErrorMessage="Please Select Date" />
                    </li>
                    <%--<li class="text">Entity Type:<span class="optional-img">&nbsp;</span></li>
                    <li class="field">
                        <asp:DropDownList ID="ddlEntityType" runat="server" CssClass="formselect" ValidationGroup="val" AutoPostBack="true" OnSelectedIndexChanged="ddlEntityType_SelectedIndexChanged">
                        </asp:DropDownList>
                    </li>

                    <li class="text">User:<span class="optional-img">&nbsp;</span></li>
                    <li class="field">
                        <asp:DropDownList ID="ddlUser" runat="server" CssClass="formselect" ValidationGroup="val">
                        </asp:DropDownList>
                    </li>--%>


                    <li class="text">
                        
                        <asp:Button ID="btnSearch" runat="server" Text="View Detail" CssClass="buttonbg" ValidationGroup="val" OnClick="btnSearch_Click" />
                    </li>
                </ul>
            </div>
       </div>
    </div>
    <div class="clear"></div>
    <div class="box11" runat="server" id="divPresentAbsent">
        <div class="subheading">
            <div class="float-left">
                Present/Absent
            </div>
            <div class="float-right">
                <img id="imgPresentGreen" runat="server" src="../../Assets/Zedsales/Images/PresentGreen.png" />&nbsp;Present&nbsp;<img id="imgabsentred" runat="server" src="../../Assets/Images/AbsentRed.png" />&nbsp;Absent
              
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
                                        
                                        <asp:Image ID="imgdtListEngineerAbsent" runat="server" />
                                    </div>
                                    <div class="float-left">
                                        
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
    <div class="box11">
        <div class="contentbox">
            <div id="dvMap" style="width: 100%; height: 600px">
            </div>
        </div>
    </div>
</asp:Content>
