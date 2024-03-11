﻿<%@ Page Title="" Language="C#" MasterPageFile="~/CommonMasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="DashboardSFA2.aspx.cs" Inherits="Dashboard_DashboardSFA2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="../dist/css/adminlte.min.css">
    <style>
        .radiobtn input[type="radio"] {
            width: 15px;
            Height: 15px;
        }

        .small-box {
            background: linear-gradient(30deg, rgb(19 166 197) 24%, rgba(22,159,195,1) 66%, rgba(5,207,211,1) 100%);
        }
    </style>

    <script>
        function travelSelf(ele) {
            var self = document.getElementById('travelSelf');
            var team = document.getElementById('travelTeam');
            self.style.display = '';
            team.style.display = 'none';
        }

        function travelTeam(ele) {
            var self = document.getElementById('travelSelf');
            var team = document.getElementById('travelTeam');
            self.style.display = 'none';
            team.style.display = '';
        }

    </script>

    <script>
        function attendanceSelf(ele1) {
            var selfAtt = document.getElementById('attendanceSelf');
            var teamAtt = document.getElementById('attendanceTeam');
            selfAtt.style.display = '';
            teamAtt.style.display = 'none';
        }

        function attendanceTeam(ele1) {
            var selfAtt = document.getElementById('attendanceSelf');
            var teamAtt = document.getElementById('attendanceTeam');
            selfAtt.style.display = 'none';
            teamAtt.style.display = '';
        }
    </script>

    <script type="text/javascript">

        function popitup(url) {
            window.open(url)
            return false;
        }

        /// Fixed Header 
        //$(window).scroll(function () {
        //    var sticky = $('header'),
        //        scroll = $(window).scrollTop();

        //    if (scroll >= 100) sticky.addClass('fixed');
        //    else sticky.removeClass('fixed');
        //});

    </script>

    <%--<style>
        .switch {
            position: relative;
            display: inline-block;
            width: 90px;
            height: 34px;
        }

            .switch input {
                display: none;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ca2222;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 26px;
                width: 26px;
                left: 4px;
                bottom: 4px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2ab934;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(55px);
            -ms-transform: translateX(55px);
            transform: translateX(55px);
        }

        /*------ ADDED CSS ---------*/
        .on {
            display: none;
        }

        .on, .off {
            color: white;
            position: absolute;
            transform: translate(-50%,-50%);
            top: 50%;
            left: 50%;
            font-size: 10px;
            font-family: Verdana, sans-serif;
        }

        input:checked + .slider .on {
            display: block;
        }

        input:checked + .slider .off {
            display: none;
        }

        /*--------- END --------*/

        /* Rounded sliders */
        .slider.round {
            border-radius: 34px;
        }

            .slider.round:before {
                border-radius: 50%;
            }
    </style>--%>

    <script>
        function test() {
            //var selfAtt = document.getElementById('attendanceSelf');
            //var teamAtt = document.getElementById('attendanceTeam');

            //if (ele1.document.value)
            //selfAtt.style.display = 'none';
            //teamAtt.style.display = '';

            alert(ele1.event.value);
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolderMain" runat="Server">
    <div class="wrapper">
        <!-- Content Wrapper. Contains page content -->
        <div>
            <!-- Content Header (Page header) -->
            <%--<div class="content-header">
                    <div class="container-fluid">
                        <div class="row mb-2">
                            <div class="col-sm-6">
                                <h1 class="m-0">Dashboard</h1>
                            </div>
                            <!-- /.col -->
                            <div class="col-sm-6">
                                <ol class="breadcrumb float-sm-right">
                                    <li class="breadcrumb-item"><a href="../Default.aspx">Home</a></li>
                                </ol>
                            </div>
                            <!-- /.col -->
                        </div>
                        <!-- /.row -->
                    </div>
                    <!-- /.container-fluid -->
                </div>--%>
            <!-- /.content-header -->

            <!-- Main content -->
            <%--<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server"></asp:ScriptManager>--%>
            <asp:UpdatePanel runat="server" ID="updMsg" UpdateMode="Always">
                <ContentTemplate>
                    <section class="content">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-12 col-sm-12">
                                    <div class="card card-primary card-outline card-tabs">
                                        <div class="card-header p-0 pt-1 border-bottom-0">
                                            <h2 id="clientName"><i class="fa fa-user" aria-hidden="true"></i><span></span></h2>
                                            <ul class="nav nav-tabs" id="custom-tabs-three-tab" role="tablist" style="width: 100%">
                                                <li class="nav-item">
                                                    <a class="nav-link active" runat="server" id="customtabsthreebusinesstab" data-toggle="pill" href="#custom-tabs-three-business" role="tab" aria-controls="custom-tabs-three-business" aria-selected="true">Business</a>
                                                </li>
                                                <li class="nav-item">
                                                    <a class="nav-link" runat="server" id="customtabsthreeattendancetab" data-toggle="pill" href="#custom-tabs-three-attendance" role="tab" aria-controls="custom-tabs-three-attendance" aria-selected="false">Attendance and leave</a>
                                                </li>
                                                <li class="nav-item">
                                                    <a class="nav-link" runat="server" id="customtabsthreetraveltab" data-toggle="pill" href="#custom-tabs-three-travel" role="tab" aria-controls="custom-tabs-three-travel" aria-selected="false">Travel and expense</a>
                                                </li>
                                            </ul>

                                        </div>
                                        <div class="card-body">
                                            <div class="tab-content" id="custom-tabs-three-tabContent">
                                                <div class="tab-pane fade show" id="custom-tabs-three-business" role="tabpanel" aria-labelledby="customtabsthreebusinesstab" style="opacity: 5 !important">
                                                    <div id="customtabsthreebusiness" runat="server">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="card card-info">
                                                                    <div class="card-header">
                                                                        <h3 class="card-title">Today :
                                                                            <asp:Label ID="lblbusinessToday" runat="server"></asp:Label>
                                                                            <span class="custom-border"></span>Yesterday :
                                                                            <asp:Label ID="lblbusinessYesterday" runat="server"></asp:Label>
                                                                            <span class="custom-border"></span>MTD :
                                                                            <asp:Label ID="lblbusinessMTD" runat="server"></asp:Label></h3>
                                                                    </div>
                                                                    <!-- /.card-body -->
                                                                </div>
                                                                <!-- /.card -->
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-lg-3 col-6 small-box-main" id="YesterdaySalewidget" runat="server">
                                                                <!-- small box -->
                                                                <div class="small-box">
                                                                    <div class="inner" style="text-align: center">
                                                                        <div class="inner-text">
                                                                            <h2>Yesterday Sale</h2>
                                                                            <p>
                                                                                Qty : 
                                                                                <asp:Label ID="lblSaleQty" runat="server"></asp:Label>
                                                                            </p>
                                                                            <p>
                                                                                Value : ₹<asp:Label ID="lblSaleValue" runat="server"></asp:Label>
                                                                            </p>
                                                                        </div>
                                                                        <div class="icon-image">
                                                                            <img src="../images/sale.jpg" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <!-- ./col -->
                                                            <div class="col-lg-3 col-6 small-box-main" id="YesterdayCollectionwidget" runat="server">
                                                                <!-- small box -->
                                                                <div class="small-box">
                                                                    <div class="inner" style="text-align: center">
                                                                        <div class="inner-text">
                                                                            <h2>Yday Collection</h2>
                                                                            <p>
                                                                                Total Value : ₹<asp:Label ID="lblytdcol" runat="server"></asp:Label>
                                                                            </p>
                                                                        </div>
                                                                        <div class="icon-image">
                                                                            <img src="../images/collection.jpg" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <!-- ./col -->
                                                            <div class="col-lg-3 col-6 small-box-main" id="YesterdayOrderwidget" runat="server">
                                                                <!-- small box -->
                                                                <div class="small-box">
                                                                    <div class="inner" style="text-align: center">
                                                                        <div class="inner-text">
                                                                            <h2>Yesterday Order</h2>
                                                                            <p>
                                                                                Qty : 
                                                                                <asp:Label ID="lblYesterdayOrderQty" runat="server"></asp:Label>
                                                                            </p>
                                                                            <p>
                                                                                Value :
                                                                                <asp:Label ID="lblYesterdayOrderAmount" runat="server"></asp:Label>
                                                                            </p>
                                                                        </div>
                                                                        <div class="icon-image">
                                                                            <img src="../images/order.jpg" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <!-- ./col -->
                                                            <div class="col-lg-3 col-6 small-box-main" id="YesterdayProspectwidget" runat="server">
                                                                <!-- small box -->
                                                                <div class="small-box">
                                                                    <div class="inner" style="text-align: center">
                                                                        <div class="inner-text">
                                                                            <h2>Yday Prospect</h2>
                                                                            <p>
                                                                                Count : 
                                                                                <asp:Label ID="lblYesterdayProspect" runat="server"></asp:Label>
                                                                            </p>
                                                                        </div>
                                                                        <div class="icon-image">
                                                                            <img src="../images/prospect.jpg" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <!-- ./col -->
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-lg-3 col-6 small-box-main" id="MTDSalewidget" runat="server">
                                                                <!-- small box -->
                                                                <div class="small-box">
                                                                    <div class="inner" style="text-align: center">
                                                                        <div class="inner-text">
                                                                            <h2>MTD Sale</h2>
                                                                            <p>
                                                                                Qty : 
                                                                                <asp:Label ID="lblMTDSaleQty" runat="server"></asp:Label>
                                                                            </p>
                                                                            <p>
                                                                                Value : ₹<asp:Label ID="lblMTDSaleValue" runat="server"></asp:Label>
                                                                            </p>
                                                                        </div>
                                                                        <div class="icon-image">
                                                                            <img src="../images/sale.jpg" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <!-- ./col -->
                                                            <div class="col-lg-3 col-6 small-box-main" id="MTDCollectionwidget" runat="server">
                                                                <!-- small box -->
                                                                <div class="small-box">
                                                                    <div class="inner" style="text-align: center">
                                                                        <div class="inner-text">
                                                                            <h2>MTD Collection</h2>
                                                                            <p>
                                                                                Total Value :  ₹<asp:Label ID="lblmtdcol" runat="server"></asp:Label>
                                                                            </p>
                                                                        </div>
                                                                        <div class="icon-image">
                                                                            <img src="../images/collection.jpg" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <!-- ./col -->
                                                            <div class="col-lg-3 col-6 small-box-main" id="MTDOrderwidget" runat="server">
                                                                <!-- small box -->
                                                                <div class="small-box">
                                                                    <div class="inner" style="text-align: center">
                                                                        <div class="inner-text">
                                                                            <h2>MTD Order</h2>
                                                                            <p>
                                                                                Qty : 
                                                                                <asp:Label ID="lblMTDOrderQty" runat="server"></asp:Label>
                                                                            </p>
                                                                            <p>
                                                                                Value :
                                                                                <asp:Label ID="lblMTDOrderAmount" runat="server"></asp:Label>
                                                                            </p>
                                                                        </div>
                                                                        <div class="icon-image">
                                                                            <img src="../images/order.jpg" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <!-- ./col -->
                                                            <div class="col-lg-3 col-6 small-box-main" id="MTDProspectwidget" runat="server">
                                                                <!-- small box -->
                                                                <div class="small-box">
                                                                    <div class="inner" style="text-align: center">
                                                                        <div class="inner-text">
                                                                            <h2>MTD Prospect</h2>
                                                                            <p>
                                                                                Count :
                                                                                <asp:Label ID="lblMTDProspect" runat="server"></asp:Label>
                                                                            </p>
                                                                        </div>
                                                                        <div class="icon-image">
                                                                            <img src="../images/prospect.jpg" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <!-- ./col -->
                                                        </div>
                                                        <div class="row" style="display: none">
                                                            <div class="col-6 col-md-3 text-center">
                                                                <div style="display: inline; width: 90px; height: 90px;">
                                                                    <canvas width="90" height="90"></canvas>
                                                                    <input type="text" class="knob" value="30" data-thickness="0.1" data-width="90" data-height="90" data-fgcolor="#00a65a" style="width: 49px; height: 30px; position: absolute; vertical-align: middle; margin-top: 30px; margin-left: -69px; border: 0px; background: none; font: bold 18px Arial; text-align: center; color: rgb(0, 166, 90); padding: 0px; appearance: none;">
                                                                </div>

                                                                <div class="knob-label">data-thickness="0.1"</div>
                                                            </div>
                                                            <%--<div class="col-6 col-md-3 text-center">
                                                                    <input type="text" class="knob" value="30" data-thickness="0.1" data-width="90" data-height="90"
                                                                           data-fgColor="#00a65a">

                                                                    <div class="knob-label">data-thickness="0.1"</div>
                                                                  </div>
                                                                <div class="col-6 col-md-3 text-center">
                                                                    <input type="text" class="knob" value="30" data-thickness="0.1" data-width="90" data-height="90"
                                                                           data-fgColor="#00a65a">

                                                                    <div class="knob-label">data-thickness="0.1"</div>
                                                                  </div>
                                                                <div class="col-6 col-md-3 text-center">
                                                                    <input type="text" class="knob" value="30" data-thickness="0.1" data-width="90" data-height="90"
                                                                           data-fgColor="#00a65a">

                                                                    <div class="knob-label">data-thickness="0.1"</div>
                                                                  </div>--%>
                                                        </div>
                                                        <div class="row">
                                                            <%--<div class="col-md-12">
                                                                <h3>Team</h3>
                                                            </div>--%>

                                                            <div class="col-12">
                                                                <div class="card card-solid" id="idTeamDiv" runat="server">
                                                                    <div class="card-header">
                                                                        <h3 class="pull-left">Top 5 Sales Person</h3>
                                                                    </div>
                                                                    <div class="card-body" style="max-height: 350px;">

                                                                        <!---------/// Test ------>

                                                                        <!----------/// Test End ------->

                                                                        <!------------/// Custom HTML Start -------->
                                                                        <!--------// Our Team ----->
                                                                        <div class="carousel-wrap">
                                                                            <div class="owl-carousel owl-theme">
                                                                                <asp:Repeater ID="rptTeams" runat="server" OnItemDataBound="OnItemDataBound">
                                                                                    <ItemTemplate>
                                                                                        <!------------/// Item Start ----->
                                                                                        <div class="item" data-toggle="modal" data-target="#myModal">
                                                                                            <div class="item-inner" data-toggle="modal" data-target="#myModal" data-entitytypeid="<%# Eval("UserId") %>" data-entitytypename="<%# Eval("DisplayName") %>">

                                                                                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("UserId") %>' Visible="false" />
                                                                                                <asp:Label ID="lblEntityTypeID" runat="server" Text='<%# Eval("EntityTypeID") %>' Visible="false" />
                                                                                                <div class="team-image">
                                                                                                    <img src="../images/testimonial.png">
                                                                                                    <%--<asp:Image ID="Image2" class="img-circle img-fluid" runat="server" ImageUrl='<%# Eval("ProfileImage") != null ? "../dist/img/defultimage.png" : "http://dms.zedsales.in/" + Eval("ProfileImage") %>' />--%>
                                                                                                </div>
                                                                                                <div class="team-details">
                                                                                                    <h2>
                                                                                                        <asp:Label ID="lbTeamlDisplayName" runat="server" Text='<%# Eval("DisplayName") %>' /></h2>
                                                                                                    <p>
                                                                                                        QTY:
                                                                                                        <asp:Label ID="Label11" runat="server" Text='<%# Eval("QTY") %>' />
                                                                                                    </p>
                                                                                                    <p>
                                                                                                        Sale:
                                                                                                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("Value", "{0:C}") %>' />
                                                                                                    </p>
                                                                                                    <%--<p>Lead: <asp:Label ID="Label9" runat="server" Text='<%# Eval("ProfileImage") %>' /></p>--%>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <!------------/// Item Start End ----->
                                                                                    </ItemTemplate>
                                                                                </asp:Repeater>

                                                                            </div>
                                                                        </div>
                                                                        <!----------/// Our Team End ------>


                                                                    </div>
                                                                </div>

                                                                <div class="card" id="idTeamNoData" runat="server">
                                                                    <h3>My Teams</h3>
                                                                    <div class="card-header">
                                                                        <div style="text-align: center">
                                                                            <h3 class="card-title">No Data Available</h3>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-12 active-tab-table" id="idRetailerDiv" runat="server">
                                                                <div class="card" id="idtopRetailer" runat="server">
                                                                    <div class="card-header">
                                                                        <div class="row">
                                                                            <div class="col-md-6">
                                                                                <div class="custom-control custom-radio">
                                                                                    <asp:RadioButton ID="rbtnRetailer" runat="server" Text="Top 5 Retailer" GroupName="Software"
                                                                                        Font-Bold="true" ForeColor="Navy" Checked="true" AutoPostBack="true" OnCheckedChanged="rbtn_CheckedChanged"
                                                                                        CssClass="radiobtn" />
                                                                                    &nbsp;&nbsp;&nbsp;
                                                                    <asp:RadioButton ID="rbtnDistributors" runat="server" Text="Top 5 Distributors" GroupName="Software"
                                                                        Font-Bold="true" ForeColor="Navy" OnCheckedChanged="rbtn_CheckedChanged"
                                                                        CssClass="radiobtn" />
                                                                                    <%--<input class="custom-control-input" type="radio" id="customRadio1" name="customRadio" checked>
                                                            <label for="customRadio1" class="custom-control-label">Top 5 Retailer</label>--%>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-6">
                                                                                <div class="custom-control custom-radio">
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <!-- /.card-header -->
                                                                    <div class="card-body table-responsive p-0" style="max-height: 300px;">
                                                                        <asp:Repeater ID="rptopRetailer" runat="server">
                                                                            <HeaderTemplate>
                                                                                <table id="example3" class="table table-bordered table-striped">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <th style="width: 25%">Name</th>
                                                                                            <th style="width: 25%">Qty</th>
                                                                                            <th style="width: 25%">Value</th>
                                                                                            <th style="width: 25%">Contribution(%)</th>
                                                                                        </tr>
                                                                                    </thead>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblBsRetDistName" runat="server" Text='<%# Eval("DistName") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblBsRetTertSale" runat="server" Text='<%# Eval("TertSale") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblBsRetTerSaleAmt" runat="server" Text='<%# Eval("TerSaleAmt", "{0:C}") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblBsRetContribution" runat="server" Text='<%# Eval("Contribution","{0:0.#}") %>' />
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                </table>
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                    </div>
                                                                    <!-- /.card-body -->
                                                                </div>
                                                                <div class="card" id="idtopRetailerNoData" runat="server">
                                                                    <div class="card-header">
                                                                        <div style="text-align: center">
                                                                            <h3 class="card-title">No Data Available</h3>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>

                                                            <div class="col-12" id="idDistributorsDiv" runat="server">
                                                                <div class="card" id="idtopDistributors">
                                                                    <!-- /.card-header -->
                                                                    <div class="card-body table-responsive p-0" style="max-height: 300px;">
                                                                        <asp:Repeater ID="rptDistributors" runat="server">
                                                                            <HeaderTemplate>
                                                                                <table id="example3" class="table table-bordered table-striped">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <th style="width: 25%">Name</th>
                                                                                            <th style="width: 25%">Qty</th>
                                                                                            <th style="width: 25%">Value</th>
                                                                                            <th style="width: 25%">Contribution(%)</th>
                                                                                        </tr>
                                                                                    </thead>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblBsDisDistName" runat="server" Text='<%# Eval("DistName") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblBsDisTertSale" runat="server" Text='<%# Eval("TertSale") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblBsDisTerSaleAmt" runat="server" Text='<%# Eval("TerSaleAmt", "{0:C}") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblBsDisContribution" runat="server" Text='<%# Eval("Contribution","{0:0.#}") %>' />
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                </table>
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                    </div>
                                                                    <!-- /.card-body -->
                                                                </div>
                                                                <div class="card" id="idtopDistributorsNoData" runat="server">
                                                                    <div class="card-header">
                                                                        <div style="text-align: center">
                                                                            <h3 class="card-title">No Data Available</h3>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="row">
                                                            <%--<div class="col-md-12">
                                                                <h3><b>Top 5 Models</b></h3>
                                                            </div>--%>
                                                            <div class="col-12">
                                                                <div class="card">
                                                                    <div class="card-header">
                                                                        <h3 class="pull-left">Top 5 Models</h3>
                                                                    </div>
                                                                    <div class="card-body table-responsive p-0" style="max-height: 300px;" id="topmodel" runat="server">
                                                                        <!-- /.card-header -->
                                                                        <asp:Repeater ID="rptTopModel" runat="server">
                                                                            <HeaderTemplate>
                                                                                <table id="example3" class="table table-bordered table-striped">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <th style="width: 25%">Model Name</th>
                                                                                            <th style="width: 25%">Qty</th>
                                                                                            <th style="width: 25%">Value</th>
                                                                                            <th style="width: 25%">Contribution(%)</th>
                                                                                        </tr>
                                                                                    </thead>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblModelName" runat="server" Text='<%# Eval("ModelName") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblMtdSaleVolume" runat="server" Text='<%# Eval("MtdSaleVolume") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblMtdSaleValue" runat="server" Text='<%# Eval("MtdSaleValue" , "{0:C}") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblSalePercentage" runat="server" Text='<%# Eval("SalePercentage","{0:0.#}") %>' />
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                </table>
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                        <!-- /.card-body -->
                                                                    </div>
                                                                </div>
                                                                <div class="card" id="topmodelNodata" runat="server">
                                                                    <div class="card-header">
                                                                        <h3 class="pull-left">Top 5 Models</h3>

                                                                    </div>
                                                                    <div class="card-body">
                                                                        <p class="card-title">No Data Available</p>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div class="row">
                                                            <div class="col-12" id="idBeatPlanDiv" runat="server">
                                                                <div class="card" id="idBeatPlan" runat="server">
                                                                    <!-- /.card-header -->
                                                                    <div class="card-header">
                                                                        <div class="col-md-6">
                                                                            <div class="custom-control custom-radio">
                                                                                <asp:RadioButton ID="rbtnBeatPlan" runat="server" Text="Beat Plan Visit MTD" GroupName="PlanGroup"
                                                                                    Font-Bold="true" ForeColor="Navy" Checked="true" AutoPostBack="true"
                                                                                    OnCheckedChanged="rbtnPlan_CheckedChanged" CssClass="radiobtn" />
                                                                                &nbsp;&nbsp;&nbsp;
                                                                    <asp:RadioButton ID="rbtnRanking" runat="server" Text="Your Ranking" GroupName="PlanGroup"
                                                                        Font-Bold="true" ForeColor="Navy" AutoPostBack="true"
                                                                        OnCheckedChanged="rbtnPlan_CheckedChanged" CssClass="radiobtn" Visible="false" />

                                                                                <%--<input class="custom-control-input" type="radio" id="customRadio3" name="customRadio1" checked>
                                                            <label for="customRadio3" class="custom-control-label">Beat Plan</label>--%>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6">

                                                                            <div class="custom-control custom-radio">


                                                                                <%--<input class="custom-control-input" type="radio" id="customRadio4" name="customRadio1">
                                                            <label for="customRadio4" class="custom-control-label">Your Ranking</label>--%>
                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                    <div class="card-body table-responsive p-0" style="max-height: 300px;">
                                                                        <asp:Repeater ID="rptBeatPlan" runat="server">
                                                                            <HeaderTemplate>
                                                                                <table id="example3" class="table table-bordered table-striped">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <th style="width: 20%">Name</th>
                                                                                            <th style="width: 20%">No. of Visits</th>
                                                                                            <th style="width: 20%">Planned Visits</th>
                                                                                            <th style="width: 20%">UnPlanned Visits</th>
                                                                                            <th style="width: 20%">Total</th>
                                                                                        </tr>
                                                                                    </thead>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblDisplayName" runat="server" Text='<%# Eval("DisplayName") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblTotalVisit" runat="server" Text='<%# Eval("TotalVisit") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblPlanned" runat="server" Text='<%# Eval("Planned") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblUnplanned" runat="server" Text='<%# Eval("Unplanned") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("TotalVisit") %>' />
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                </table>
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                    </div>
                                                                    <!-- /.card-body -->
                                                                </div>
                                                                <div class="card" id="idBeatPlanNoData" runat="server">
                                                                    <div class="card-header">
                                                                        <div style="text-align: center">
                                                                            <h3 class="card-title">No Data Available</h3>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>

                                                            <div class="col-12" id="idRankingDiv" runat="server">
                                                                <div class="card" id="idRanking" runat="server">
                                                                    <!-- /.card-header -->
                                                                    <div class="card-body table-responsive p-0" style="max-height: 300px;">
                                                                        <asp:Repeater ID="rptRanking" runat="server">
                                                                            <HeaderTemplate>
                                                                                <table id="example3" class="table table-bordered table-striped">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <th style="width: 25%">Name</th>
                                                                                            <th style="width: 25%">Areawise</th>
                                                                                            <th style="width: 25%">Regionwise</th>
                                                                                            <th style="width: 25%">National</th>
                                                                                        </tr>
                                                                                    </thead>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblRankingDisplayName" runat="server" Text='<%# Eval("DisplayName") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblAreawise" runat="server" Text='<%# Eval("Areawise") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblRegionwise" runat="server" Text='<%# Eval("Regionwise") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblNational" runat="server" Text='<%# Eval("National") %>' />
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                </table>
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                    </div>
                                                                    <!-- /.card-body -->
                                                                </div>
                                                                <div class="card" id="idRankingNoData" runat="server">
                                                                    <div class="card-header">
                                                                        <div style="text-align: center">
                                                                            <h3 class="card-title">No Data Available</h3>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="tab-pane fade" id="custom-tabs-three-attendance" role="tabpanel" aria-labelledby="customtabsthreeattendancetab" style="opacity: 5 !important">
                                                    <%--<div class="row">
                                                            <div class="col-md-12">
                                                                <div style="text-align: center">
                                                                    <input type="button" value="Self" id="btnAttendanceSelf" onclick="attendanceSelf(this)">
                                                                    &nbsp;&nbsp;
                                                                <input type="button" value="Team" id="btnAttendanceTeam" onclick="attendanceTeam(this)">
                                                                </div>
                                                            </div>
                                                        </div>--%>
                                                    <br />

                                                    <%--<label class="switch">
                                                            <input type="checkbox" name="TT_sticky_header" id="togBtn" onclick="test()">
                                                            <div class="slider round">
                                                                <!--ADDED HTML --><span class="on">Team</span>
                                                                <span class="off">Self</span><!--END--></div>
                                                        </label>--%>
                                                    <div style="clear: both"></div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="card card-info">
                                                                <div class="card-header">
                                                                    <h3 class="card-title">Today :
                                                                        <asp:Label ID="lblattendanceToday" runat="server"></asp:Label>
                                                                        <span class="custom-border"></span>Yesterday :
                                                                        <asp:Label ID="lblattendanceYesterday" runat="server"></asp:Label>
                                                                        <span class="custom-border"></span>MTD :
                                                                        <asp:Label ID="lblattendanceMTD" runat="server"></asp:Label></h3>
                                                                </div>
                                                                <!-- /.card-body -->
                                                            </div>
                                                            <!-- /.card -->
                                                        </div>
                                                    </div>
                                                    <div class="row" id="attendanceSelf">
                                                        <div class="col-lg-3 col-6 small-box-main" id="AttendanceMTDwidget" runat="server">
                                                            <!-- small box -->
                                                            <div class="small-box small-box-bg-1">
                                                                <div class="inner" style="text-align: center">
                                                                    <div class="inner-text">
                                                                        <h2>Attendance MTD</h2>
                                                                        <p>
                                                                            Present:
                                                                            <asp:Label ID="lblPresentDays" runat="server"></asp:Label>
                                                                        </p>
                                                                        <p>
                                                                            Absent:
                                                                            <asp:Label ID="lblAbsentDays" runat="server"></asp:Label>
                                                                        </p>
                                                                    </div>
                                                                    <div class="icon-image">
                                                                        <img src="../images/icon-2-1.jpg" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <!-- ./col -->
                                                        <div class="col-lg-3 col-6 small-box-main" id="TodayAttendancewidget" runat="server">
                                                            <!-- small box -->
                                                            <div class="small-box small-box-bg-2">
                                                                <div class="inner" style="text-align: center">
                                                                    <div class="inner-text">
                                                                        <h2>Today
                                                                            <br />
                                                                            Attendance</h2>
                                                                        <p id="ontime" runat="server">
                                                                            On Time:
                                                                            <asp:Label ID="lblOnTime" runat="server"></asp:Label>
                                                                        </p>
                                                                        <p id="late" runat="server">
                                                                            Late:
                                                                            <asp:Label ID="lblLateChecking" runat="server"></asp:Label>
                                                                        </p>
                                                                        <p id="leave" runat="server">
                                                                            Leave:
                                                                            <asp:Label ID="lblLeave" runat="server"></asp:Label>
                                                                        </p>
                                                                        <p id="nocheck" runat="server">
                                                                            No Checkin:
                                                                            <asp:Label ID="lblNotChecking" runat="server"></asp:Label>
                                                                        </p>
                                                                    </div>
                                                                    <div class="icon-image">
                                                                        <img src="../images/icon-2-2.jpg" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <!-- ./col -->
                                                        <div class="col-lg-3 col-6 small-box-main" id="leaveMTDwidget" runat="server">
                                                            <!-- small box -->
                                                            <div class="small-box small-box-bg-3" style="text-align: center">
                                                                <div class="inner" style="text-align: center">
                                                                    <div class="inner-text">
                                                                        <h2>Leave MTD <span>(Application / No. Of Days)</span></h2>
                                                                        <p>
                                                                            Approved:
                                                                            <asp:Label ID="lblLeaveApproved" runat="server"></asp:Label>
                                                                        </p>
                                                                        <p>
                                                                            Pending:
                                                                            <asp:Label ID="lblLeavePending" runat="server"></asp:Label>
                                                                        </p>
                                                                        <p>
                                                                            Rejected:
                                                                            <asp:Label ID="lblLeaveRejected" runat="server"></asp:Label>
                                                                        </p>
                                                                    </div>
                                                                    <div class="icon-image">
                                                                        <img src="../images/icon-2-3.jpg" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <!-- ./col -->
                                                    </div>
                                                    <div class="row" id="attendanceTeam">


                                                        <%--<div class="col-md-12">
                                                                <h3><b>Today Attendance</b></h3>
                                                            </div>--%>
                                                        <div class="col-12">
                                                            <div class="card" id="rptTodayAttendanceDiv" runat="server">
                                                                <!-- /.card-header -->
                                                                <div class="card-header">
                                                                    <h3 class="pull-left">Today Attendance</h3>
                                                                    <ul class="pull-right">
                                                                        <%--	      <li><img src="../images/pdf-icon.jpg"></li>
		  <li><img src="../images/excel-icon.jpg"></li>--%>
                                                                    </ul>
                                                                </div>
                                                                <div class="card-body table-responsive p-0" style="max-height: 300px;">
                                                                    <asp:Repeater ID="rptTodayAttendance" runat="server">
                                                                        <HeaderTemplate>
                                                                            <table id="example3" class="table table-bordered table-striped">
                                                                                <thead>
                                                                                    <tr>
                                                                                        <th style="width: 20%">User Name</th>
                                                                                        <th style="width: 20%">On Time</th>
                                                                                        <th style="width: 20%">Late</th>
                                                                                        <th style="width: 20%">Leave</th>
                                                                                        <th style="width: 20%">No Checkin</th>
                                                                                    </tr>
                                                                                </thead>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAttendanceUserName" runat="server" Text='<%# Eval("UserName") %>' />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAttendanceOnTime" runat="server" Text='<%# Eval("OnTime").ToString()=="No"?"--":"✓" %>' />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAttendanceLateChecking" runat="server" Text='<%# Eval("LateChecking").ToString()=="No"?"--":"✓" %>' />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAttendanceLeave" runat="server" Text='<%# Eval("Leave").ToString()=="No"?"--":"✓" %>' />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAttendanceNotChecking" runat="server" Text='<%# Eval("NotChecking").ToString()=="No"?"--":"✓" %>' />
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            </table>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                                <!-- /.card-body -->
                                                            </div>
                                                            <div class="card" id="rptTodayAttendanceNoData" runat="server">
                                                                <h3><b>Today Attendance</b></h3>
                                                                <div class="card-header">
                                                                    <div style="text-align: center">
                                                                        <h3 class="card-title">No Data Available</h3>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <%--<div class="col-md-12">
                                                                <h3><b>Attendance MTD</b></h3>
                                                            </div>--%>
                                                        <div class="col-12">
                                                            <div class="card" id="rptAttendanceDiv" runat="server">
                                                                <div class="card-header">
                                                                    <h3 class="pull-left">Attendance MTD</h3>
                                                                    <ul class="pull-right">
                                                                        <%--	      <li><img src="../images/pdf-icon.jpg"></li>
		  <li><img src="../images/excel-icon.jpg"></li>--%>
                                                                    </ul>
                                                                </div>
                                                                <div class="card-body table-responsive p-0" style="max-height: 300px;">
                                                                    <asp:Repeater ID="rptAttendance" runat="server">
                                                                        <HeaderTemplate>
                                                                            <table id="example3" class="table table-bordered table-striped">
                                                                                <thead>
                                                                                    <tr>
                                                                                        <th style="width: 33%">User Name</th>
                                                                                        <th style="width: 33%">Present</th>
                                                                                        <th style="width: 33%">Absent</th>
                                                                                    </tr>
                                                                                </thead>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAttendanceMTDUserName" runat="server" Text='<%# Eval("UserName") %>' />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAttendancePresentDays" runat="server" Text='<%# Eval("PresentDays") %>' />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAttendanceAbsentDays" runat="server" Text='<%# Eval("AbsentDays") %>' />
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            </table>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                                <!-- /.card-body -->
                                                            </div>
                                                            <div class="card" id="rptAttendanceNoData" runat="server">
                                                                <h3><b>Attendance MTD</b></h3>
                                                                <div class="card-header">
                                                                    <div style="text-align: center">
                                                                        <h3 class="card-title">No Data Available</h3>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>


                                                        <div class="col-12">
                                                            <div class="card" id="rptLeaveDiv" runat="server">
                                                                <!-- /.card-header -->
                                                                <div class="card-header">
                                                                    <h3 class="pull-left">Leave MTD</h3>
                                                                    <ul class="pull-right">
                                                                    </ul>
                                                                </div>
                                                                <div class="card-body table-responsive p-0" style="max-height: 300px;">
                                                                    <asp:Repeater ID="rptLeave" runat="server">
                                                                        <HeaderTemplate>
                                                                            <table id="example3" class="table table-bordered table-striped">
                                                                                <thead>
                                                                                    <tr>
                                                                                        <th style="width: 25%">User Name</th>
                                                                                        <th style="width: 20%">Approved
                                                                                            <asp:Label ID="lbl" runat="server" ForeColor="Gray" Style="color: Gray;"> (Application/Days)</asp:Label></th>
                                                                                        <th style="width: 20%">Pending<br />
                                                                                            <asp:Label ID="Label1" runat="server" ForeColor="Gray" Style="color: Gray;">  (Application/Days)</asp:Label></th>
                                                                                        <th style="width: 20%">Rejected
                                                                                            <asp:Label ID="Label2" runat="server" ForeColor="Gray" Style="color: Gray;">  (Application/Days)</asp:Label></th>
                                                                                    </tr>
                                                                                </thead>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblExpenceUserName" runat="server" Text='<%# Eval("UserName") %>' />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblExpenceApproved" runat="server" Text='<%# Eval("Approved") %>' />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblExpencePending" runat="server" Text='<%# Eval("Pending") %>' />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblExpenceRejected" runat="server" Text='<%# Eval("Rejected") %>' />
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            </table>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>

                                                                    <asp:GridView ID="GridView1" Visible="false" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                                                        OnPageIndexChanging="OnPageIndexChanging" PageSize="10">
                                                                        <Columns>
                                                                            <asp:BoundField ItemStyle-Width="150px" DataField="UserName" HeaderText="User Name" />
                                                                            <asp:BoundField ItemStyle-Width="150px" DataField="Approved" HeaderText="Approved" />
                                                                            <asp:BoundField ItemStyle-Width="150px" DataField="Pending" HeaderText="Pending(s)" />
                                                                            <asp:BoundField ItemStyle-Width="150px" DataField="Rejected" HeaderText="Rejected" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                                <!-- /.card-body -->
                                                            </div>
                                                            <div class="card" id="rptLeaveNoData" runat="server">
                                                                <h3><b>Leave MTD</b></h3>
                                                                <div class="card-header">
                                                                    <div style="text-align: center">
                                                                        <h3 class="card-title">No Data Available</h3>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>


                                                    </div>
                                                </div>
                                                <div class="tab-pane fade" id="custom-tabs-three-travel" role="tabpanel" aria-labelledby="custom-tabs-three-travel-tab" style="opacity: 5 !important">
                                                    <%-- <div style="text-align: center">
                                                            <input type="button" value="Self" id="btnTravelSelf" onclick="travelSelf(this)">
                                                            &nbsp;&nbsp;
                                                            <input type="button" value="Team" id="btnTravelTeam" onclick="travelTeam(this)">
                                                        </div>--%>
                                                    <%--<div>
                                                    <div class="form-group">
                                                        Self
                                                        <div class="custom-control custom-switch">
                                                            <input type="checkbox" class="custom-control-input" id="customSwitch1">
                                                            <label class="custom-control-label" for="customSwitch1">Team</label>
                                                        </div>
                                                    </div>
                                                </div>--%>
                                                    <br />
                                                    <div style="clear: both"></div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="card card-info">
                                                                <div class="card-header">
                                                                    <h3 class="card-title">Today :
                                                                        <asp:Label ID="lbltravelToday" runat="server"></asp:Label>
                                                                        <span class="custom-border"></span>Yesterday :
                                                                        <asp:Label ID="lbltravelYesterday" runat="server"></asp:Label>
                                                                        <span class="custom-border"></span>MTD :
                                                                        <asp:Label ID="lbltravelMTD" runat="server"></asp:Label></h3>
                                                                </div>
                                                                <!-- /.card-body -->
                                                            </div>
                                                            <!-- /.card -->
                                                        </div>
                                                    </div>
                                                    <div class="row" id="travelSelf">
                                                        <div class="col-lg-3 col-6 small-box-main" id="DistanceTravelwidget" runat="server">
                                                            <!-- small box -->
                                                            <div class="small-box small-box-bg-1">
                                                                <div class="inner" style="text-align: center">
                                                                    <div class="inner-text">
                                                                        <h2>Distance Travel (Km)</h2>
                                                                        <p>
                                                                            Yesterday:
                                                                            <asp:Label ID="lblYesterdayKm" runat="server"></asp:Label>
                                                                        </p>
                                                                        <p>
                                                                            MTD:
                                                                            <asp:Label ID="lblMTDKM" runat="server"></asp:Label>
                                                                        </p>
                                                                    </div>
                                                                    <div class="icon-image">
                                                                        <img src="../images/icon-3-1.jpg" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <!-- ./col -->
                                                        <div class="col-lg-3 col-6 small-box-main" id="TimespendMarketwidget" runat="server">
                                                            <!-- small box -->
                                                            <div class="small-box small-box-bg-2">
                                                                <div class="inner" style="text-align: center">
                                                                    <div class="inner-text">
                                                                        <h2>Time spend in Market (Hr)</h2>
                                                                        <p>
                                                                            Yesterday:
                                                                            <asp:Label ID="lblYesterdayTimeSpend" runat="server"></asp:Label>
                                                                        </p>
                                                                        <p>
                                                                            MTD:
                                                                            <asp:Label ID="lblMTDTimeSpend" runat="server"></asp:Label>
                                                                        </p>
                                                                    </div>
                                                                    <div class="icon-image">
                                                                        <img src="../images/icon-3-2.jpg" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <!-- ./col -->
                                                        <div class="col-lg-3 col-6 small-box-main" id="Expensewidget" runat="server">
                                                            <!-- small box -->
                                                            <div class="small-box small-box-bg-3" style="text-align: center">
                                                                <div class="inner" style="text-align: center">
                                                                    <div class="inner-text">
                                                                        <h2>Expense (Application/Amount)</h2>
                                                                        <p>
                                                                            Approved :
                                                                            <asp:Label ID="lblExpenseApproved" runat="server"></asp:Label>
                                                                        </p>
                                                                        <p>
                                                                            Pending:<asp:Label ID="lblExpensePending" runat="server"></asp:Label>
                                                                        </p>
                                                                        <p>
                                                                            Rejected:
                                                                            <asp:Label ID="lblExpenseRejected" runat="server"></asp:Label>
                                                                        </p>
                                                                    </div>
                                                                    <div class="icon-image">
                                                                        <img src="../images/icon-3-3.jpg" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <!-- ./col -->
                                                        <%-- <div class="col-lg-3 col-6 small-box-main" id="AverageTimeMarketwidget" runat="server">
                                                                <!-- small box -->
                                                                <div class="small-box small-box-bg-4">
                                                                    <div class="inner" style="text-align: center">
                                                                         <div class="inner-text">
                                                                        <h2>Average Time in Market</h2>
                                                                        <p>Yesterday: <asp:Label ID="lblYesterdayAverageTime" runat="server"></asp:Label></p>
                                                                         <p>MTD: <asp:Label ID="lblMTDAverageTime" runat="server"></asp:Label></p>
                                                                             </div>
                                                                        <div class="icon-image">
                                                                            <img src="../images/icon-3-4.jpg" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <!-- ./col -->
                                                        </div>
                                                        <div style="clear: both"></div>--%>
                                                        <div class="row" id="travelTeam">
                                                            <%--<div class="col-md-12">
                                                                <h3><b>Average Time Spend in Market (Hr)</b></h3>
                                                            </div>--%>
                                                            <%--<div class="col-12">
                                                                <div class="card" id="rptAverageTimeDiv" runat="server">
                                                                    <!-- /.card-header -->
                                                                     <div class="card-header">
                                                                        <h3 class="pull-left">Average Time Spend in Market (Hr)</h3>
                                                                             <ul class="pull-right">
<%--	      <li><img src="../images/pdf-icon.jpg"></li>
		  <li><img src="../images/excel-icon.jpg"></li>--%>
	   </ul>
                                                                         <%--   </div>
                                                                    <div class="card-body table-responsive p-0" style="max-height: 300px;">
                                                                        <asp:Repeater ID="rptAverageTime" runat="server">
                                                                            <HeaderTemplate>
                                                                                <table id="example3" class="table table-bordered table-striped">
                                                                                    <thead>
                                                                                        <tr>
                                                                                            <th style="width:33%">Team</th>
                                                                                            <th style="width:33%">Yesterday</th>
                                                                                            <th style="width:33%">MTD</th>
                                                                                        </tr>
                                                                                    </thead>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="lblAverageUserName" runat="server" Text='<%# Eval("UserName") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblAverageYesterdayAverageTime" runat="server" Text='<%# Eval("YesterdayAverageTime","{0:0.#}") %>' />
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="lblAverageMTDAverageTime" runat="server" Text='<%# Eval("MTDAverageTime","{0:0.#}") %>' />
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                </table>
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                    </div>
                                                                    <!-- /.card-body -->
                                                                </div>
                                                                <div class="card" id="rptAverageTimeNoData" runat="server">
                                                                    <h3><b>Average Time Spend in Market (Hr)</b></h3>
                                                                    <div class="card-header">
                                                                        <div style="text-align: center">
                                                                            <h3 class="card-title">No Data Available</h3>
                                                                        </div>
                                                                    </div>--%>
                                                        </div>
                                                    </div>

                                                    <%--<div class="col-md-12">
                                                                <h3><b>Distance Travel (Km)</b></h3>
                                                            </div>--%>
                                                    <div class="col-12">
                                                        <div class="card" id="rptDistanceTravelDiv" runat="server">
                                                            <!-- /.card-header -->
                                                            <div class="card-header">
                                                                <h3 class="pull-left">Distance Travel (Km)</h3>
                                                                <ul class="pull-right">
                                                                    <%--	      <li><img src="../images/pdf-icon.jpg"></li>
		  <li><img src="../images/excel-icon.jpg"></li>--%>
                                                                </ul>
                                                            </div>
                                                            <div class="card-body table-responsive p-0" style="max-height: 300px;">
                                                                <asp:Repeater ID="rptDistanceTravel" runat="server">
                                                                    <HeaderTemplate>
                                                                        <table id="example3" class="table table-bordered table-striped">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th style="width: 33%">Team</th>
                                                                                    <th style="width: 33%">Yesterday</th>
                                                                                    <th style="width: 33%">MTD</th>
                                                                                </tr>
                                                                            </thead>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblDistanceUserName" runat="server" Text='<%# Eval("UserName") %>' />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblDistanceYesterdayKm" runat="server" Text='<%# Eval("YesterdayKm","{0:0.#}") %>' />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblDistanceMTDKM" runat="server" Text='<%# Eval("MTDKM","{0:0.#}") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        </table>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                            </div>
                                                            <!-- /.card-body -->
                                                        </div>
                                                        <div class="card" id="rptDistanceTravelNoData" runat="server">
                                                            <h3><b>Distance Travel (Km)</b></h3>
                                                            <div class="card-header">
                                                                <div style="text-align: center">
                                                                    <h3 class="card-title">No Data Available</h3>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <%--<div class="col-md-12">
                                                                <h3><b>Time Spend in Market (Hr)</b></h3>
                                                            </div>--%>
                                                    <div class="col-12">
                                                        <div class="card" id="rptTimeSpendDiv" runat="server">
                                                            <!-- /.card-header -->
                                                            <div class="card-header">
                                                                <h3 class="pull-left">Time Spend in Market (Hr)</h3>
                                                                <ul class="pull-right">
                                                                    <%--	      <li><img src="../images/pdf-icon.jpg"></li>
		  <li><img src="../images/excel-icon.jpg"></li>--%>
                                                                </ul>
                                                            </div>
                                                            <div class="card-body table-responsive p-0" style="max-height: 300px;">
                                                                <asp:Repeater ID="rptTimeSpend" runat="server">
                                                                    <HeaderTemplate>
                                                                        <table id="example3" class="table table-bordered table-striped">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th style="width: 33%">Team</th>
                                                                                    <th style="width: 33%">Yesterday</th>
                                                                                    <th style="width: 33%">Average</th>
                                                                                </tr>
                                                                            </thead>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblTimeSpendUserName" runat="server" Text='<%# Eval("UserName") %>' />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblTimeSpendYesterdayTimeSpend" runat="server" Text='<%# Eval("YesterdayTimeSpend","{0:0.#}") %>' />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblTimeSpendMTDTimeSpend" runat="server" Text='<%# Eval("MTDTimeSpend","{0:0.#}") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        </table>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                            </div>
                                                            <!-- /.card-body -->
                                                        </div>
                                                        <div class="card" id="rptTimeSpendNoData" runat="server">
                                                            <h3><b>Time Spend in Market (Hr)</b></h3>
                                                            <div class="card-header">
                                                                <div style="text-align: center">
                                                                    <h3 class="card-title">No Data Available</h3>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <%--<div class="col-md-12">
                                                                <h3><b>Expence</b></h3>
                                                            </div>--%>
                                                    <div class="col-12">
                                                        <div class="card" id="rptExpenceDiv" runat="server">
                                                            <!-- /.card-header -->
                                                            <div class="card-header">
                                                                <h3 class="pull-left">Expense</h3>
                                                                <ul class="pull-right">
                                                                    <%--	      <li><img src="../images/pdf-icon.jpg"></li>
		  <li><img src="../images/excel-icon.jpg"></li>--%>
                                                                </ul>
                                                            </div>
                                                            <div class="card-body table-responsive p-0" style="max-height: 300px;">
                                                                <asp:Repeater ID="rptExpence" runat="server">
                                                                    <HeaderTemplate>
                                                                        <table id="example3" class="table table-bordered table-striped">
                                                                            <thead>
                                                                                <tr>
                                                                                    <th style="width: 25%">User Name</th>
                                                                                    <th style="width: 25%">Approved (Application/Amount)</th>
                                                                                    <th style="width: 25%">Pending (Application/Amount)</th>
                                                                                    <th style="width: 25%">Rejected (Application/Amount)</th>
                                                                                </tr>
                                                                            </thead>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblExpenceUserName" runat="server" Text='<%# Eval("UserName") %>' />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblExpenceApproved" runat="server" Text='<%# Eval("Approved","{0:0.#}") %>' />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblExpencePending" runat="server" Text='<%# Eval("Pending","{0:0.#}") %>' />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblExpenceRejected" runat="server" Text='<%# Eval("Rejected","{0:0.#}") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        </table>
                                                                    </FooterTemplate>
                                                                </asp:Repeater>
                                                            </div>
                                                            <!-- /.card-body -->
                                                        </div>
                                                        <div class="card" id="rptExpenceNoData" runat="server">
                                                            <h3><b>Expence</b></h3>
                                                            <div class="card-header">
                                                                <div style="text-align: center">
                                                                    <h3 class="card-title">No Data Available</h3>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                    <!-- /.card -->
                                </div>
                            </div>
                        </div>
                        </div>
                            </div>
                            <!-- /.container-fluid -->
                    </section>
                    <!-- /.content -->
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <!-- /.content-wrapper -->

        <aside class="control-sidebar control-sidebar-dark">
            <!-- Control sidebar content goes here -->
        </aside>
        <!-- /.control-sidebar -->
    </div>

    <!-- jQuery -->
    <script src="../plugins/jquery/jquery.min.js"></script>

    <!-- Bootstrap 4 -->
    <script src="../plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- ChartJS -->
    <%--<script src="../plugins/chart.js/Chart.min.js"></script>--%>
    <!-- AdminLTE App -->
    <%--<script src="../dist/js/adminlte.js"></script>--%>
    <%--<script src="../plugins/jquery-knob/jquery.knob.min.js"></script>--%>

    <script>

        $(document).ready(function () {

            $(document).on('click', '#contentHolderMain_rbtnDistributors', function () {

                if ($(this).is(":checked") == true) {
                    $('div').removeClass('active-tab-table');

                    $('#idtopDistributors').addClass('active-tab-table');
                }

            });

            $(document).on('click', '#contentHolderMain_rbtnRetailer', function () {

                if ($(this).is(":checked") == true) {
                    $('div').removeClass('active-tab-table');

                    $('#contentHolderMain_idRetailerDiv').addClass('active-tab-table');
                }

            });



        });



        $(function () {
            /* jQueryKnob */

            $('.knob').knob({
                /*change : function (value) {
                 //console.log("change : " + value);
                 },
                 release : function (value) {
                 console.log("release : " + value);
                 },
                 cancel : function () {
                 console.log("cancel : " + this.value);
                 },*/
                draw: function () {

                    // "tron" case
                    if (this.$.data('skin') == 'tron') {

                        var a = this.angle(this.cv)  // Angle
                            ,
                            sa = this.startAngle          // Previous start angle
                            ,
                            sat = this.startAngle         // Start angle
                            ,
                            ea                            // Previous end angle
                            ,
                            eat = sat + a                 // End angle
                            ,
                            r = true

                        this.g.lineWidth = this.lineWidth

                        this.o.cursor
                            && (sat = eat - 0.3)
                            && (eat = eat + 0.3)

                        if (this.o.displayPrevious) {
                            ea = this.startAngle + this.angle(this.value)
                            this.o.cursor
                                && (sa = ea - 0.3)
                                && (ea = ea + 0.3)
                            this.g.beginPath()
                            this.g.strokeStyle = this.previousColor
                            this.g.arc(this.xy, this.xy, this.radius - this.lineWidth, sa, ea, false)
                            this.g.stroke()
                        }

                        this.g.beginPath()
                        this.g.strokeStyle = r ? this.o.fgColor : this.fgColor
                        this.g.arc(this.xy, this.xy, this.radius - this.lineWidth, sat, eat, false)
                        this.g.stroke()

                        this.g.lineWidth = 2
                        this.g.beginPath()
                        this.g.strokeStyle = this.o.fgColor
                        this.g.arc(this.xy, this.xy, this.radius - this.lineWidth + 1 + this.lineWidth * 2 / 3, 0, 2 * Math.PI, false)
                        this.g.stroke()

                        return false
                    }
                }
            })
            /* END JQUERY KNOB */

            //INITIALIZE SPARKLINE CHARTS
            var sparkline1 = new Sparkline($('#sparkline-1')[0], { width: 240, height: 70, lineColor: '#92c1dc', endColor: '#92c1dc' })
            var sparkline2 = new Sparkline($('#sparkline-2')[0], { width: 240, height: 70, lineColor: '#f56954', endColor: '#f56954' })
            var sparkline3 = new Sparkline($('#sparkline-3')[0], { width: 240, height: 70, lineColor: '#3af221', endColor: '#3af221' })

            sparkline1.draw([1000, 1200, 920, 927, 931, 1027, 819, 930, 1021])
            sparkline2.draw([515, 519, 520, 522, 652, 810, 370, 627, 319, 630, 921])
            sparkline3.draw([15, 19, 20, 22, 33, 27, 31, 27, 19, 30, 21])

        });




    </script>


</asp:Content>

