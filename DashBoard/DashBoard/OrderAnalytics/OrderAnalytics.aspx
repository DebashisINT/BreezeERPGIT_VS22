<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderAnalytics.aspx.cs" Inherits="DashBoard.DashBoard.OrderAnalytics.OrderAnalytics" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="OrderAnalytics.css" rel="stylesheet" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="~/assests/css/pignose.calendar.min.css" rel="stylesheet" />
    <script src="../Js/jquery3.3.1.min.js"></script>
    <link href="../Js/datePicker/datepicker.css" rel="stylesheet" />
    <script src="../Js/moment.min.js"></script>
<script src="~/assests/pluggins/pignose.calendar.min.js"></script>
    <script src="../Js/datePicker/bootstrap-datepicker.js"></script>

<!-- Resources -->
    <script src="../Js/amChart/core.js"></script>
    <script src="../Js/amChart/charts.js"></script>
    <script src="../Js/amChart/animated.js"></script>

<script src="OrderAnalytics.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-12 text-right clearfix py-3">
                            <table class="tbleChoosedate">
                                <tr>
                                    <td>From</td>
                                    <td class="dateWidth">
                                        <input type="text" id="fromDate" class="form-control datepicker" />
                                    </td>
                                    <td>To</td>
                                    <td class="dateWidth">
                                        <input type="text" id="toDate"  class="form-control datepicker" />
                                    </td>
                                    <td><button type="button" class="btn btn-success" onclick="getdata()">Show Analysis</button></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <div class="colorBox">
                                <div class="infoWrp">
                                    <i class="fa fa-shopping-bag fltd" aria-hidden="true"></i>
                                    <div class="nums" id="Total_oc">0</div>

                                </div>
                                <div class="mrLinks">
                                    Total Order Count
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="colorBox c1">
                                <div class="infoWrp">
                                    <i class="fa fa-inr fltd" aria-hidden="true"></i>
                                    <div class="nums"><span id="TOTAL_ov">0</span> </div>
                        
                                </div>
                                <div class="mrLinks">
                                    Total Order Value
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="colorBox c2">
                                <div class="infoWrp">
                                    <i class="fa fa-inr fltd" aria-hidden="true"></i>
                                    <div class="nums" id="TOTAL_aov">0</div>
                       
                                </div>
                                <div class="mrLinks">
                                    Average Order Value
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="colorBox c3">
                                <div class="infoWrp">
                                    <i class="fa fa-truck fltd"
                                       aria-hidden="true"></i>
                                    <div class="nums" id="TOTAL_ORDDELV">0</div>
                        
                                </div>
                                <div class="mrLinks">
                                    Order Delivered
                                </div>
                            </div>
                        </div>
                    </div>     
                </div>
            </div>

            <div class="row">
                <div class="col-md-6">
                    <div class="box-holder mTop10 box-full">
                        <span class="toggleFullScreen" data-toggle="tooltip" data-placement="top" title="Toggle Fullscreen">
                            <i class="fa fa-arrows-alt" aria-hidden="true"></i>
                        </span>
                        <div class="clearfix">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="hdd-rp">
                                        Top 10 Items(s) on Order value
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="bxd-cont">
                            <div class="row">
                                <div class="col-md-12">
                                    <div id="pieOne" class="chartHolder font-normal"><div class="nCheck">Select date to refresh data</div></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="box-holder mTop10 box-full">
                        <span class="toggleFullScreen" data-toggle="tooltip" data-placement="top" title="Toggle Fullscreen">
                            <i class="fa fa-arrows-alt" aria-hidden="true"></i>
                        </span>
                        <div class="clearfix">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="hdd-rp">
                                        Top 10 Items(s) on Order Quantity
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="bxd-cont">
                            <div class="row">
                                <div class="col-md-12">
                                    <div id="pieTwo" class="chartHolder font-normal"><div class="nCheck">Select date to refresh data</div></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row ">
                <div class="col-md-12">
                    <div class="box-holder mTop10">

                        <div class="bxd-cont">
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="box-full">
                                        <span class="toggleFullScreen" style="top:10px" data-toggle="tooltip" data-placement="top" title="Toggle Fullscreen">
                                            <i class="fa fa-arrows-alt" aria-hidden="true"></i>
                                        </span>
                                        <div id="chartLine" class="chartHolder font-normal pt-5"><div class="nCheck">Select date to refresh data</div></div>
                                        <div class="hdd-rp text-center">Order Count</div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="box-full">
                                        <span class="toggleFullScreen" style="top:10px" data-toggle="tooltip" data-placement="top" title="Toggle Fullscreen">
                                            <i class="fa fa-arrows-alt" aria-hidden="true"></i>
                                        </span>
                                        <div id="chartLine2" class="chartHolder font-normal pt-5"><div class="nCheck">Select date to refresh data</div></div>
                                        <div class="hdd-rp text-center">Order Total</div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="box-full">
                                        <span class="toggleFullScreen" style="top:10px" data-toggle="tooltip" data-placement="top" title="Toggle Fullscreen">
                                            <i class="fa fa-arrows-alt" aria-hidden="true"></i>
                                        </span>
                                        <div id="chartLine3" class="chartHolder font-normal pt-5"><div class="nCheck">Select date to refresh data</div></div>
                                        <div class="hdd-rp text-center">Order Delivered</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="box-holder mTop10">
                        <div class="clearfix">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="hdd-rp">
                                        Top Party on Order Value
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="bxd-cont">
                            <div class="row">
                                <div class="col-md-12 font-normal">
                                    <table class="usrTabl">
                                        <thead>
                                            <tr>
                                                <th>Party</th>
                                                <th class="text-right">Order value</th>
                                            </tr>
                                        </thead>
                                        <tbody id="topCustomersTble">
                                
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="box-holder mTop10">
                        <div class="clearfix">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="hdd-rp">
                                        Statewise Top Orders on Order Value
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="bxd-cont">
                            <div class="row">
                                <div class="col-md-12 font-normal">
                                    <table class="usrTabl">
                                        <thead>
                                            <tr>
                                                <th>State</th>
                                                <th class="text-right">Order value</th>
                                            </tr>
                                        </thead>
                                        <tbody id="stateWise">
                                
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        <!-- Chart code -->
       </div>
    </form>
</body>
</html>
