<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductPerformance.aspx.cs" Inherits="DashBoard.DashBoard.ProductPerformance.ProductPerformance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title></title>
    <link href="~/Scripts/pluggins/multiselect/bootstrap-multiselect.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <script src="../Js/moment.min.js"></script>
    <script src="../Js/PurchaseDb.js"></script>
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
<link href="/Scripts/Charts/styles.css" rel="stylesheet" />

<link href="/Content/media.css" rel="stylesheet" />
<script src="/Scripts/Charts/apexcharts.min.js"></script>
<link href="/assests/pluggins/LightBox/lightbox.css" rel="stylesheet" />
<script src="/assests/pluggins/LightBox/lightbox.js"></script>
<script src="/Scripts/pluggins/multiselect/bootstrap-multiselect.js"></script>

    <link href="ProductPerformance.css?1.0.1" rel="stylesheet" />
    <script src="Productperformance.js?1.0.1"></script>
</head>
<body>
    <form id="form1" runat="server">
     <div  class="col-md-12 " id="productPerformance">
            <h1 class="leadH">Product Performance</h1>
            <div class="clearfix">
                <div class="col-md-12">
                                          
                </div>   
            </div>
            <div class="clearfix">
                <div class="row">
                    <div class="col-md-12">
                        <div class="card">
                            <div class="leaderboardHeader">MOST SELLING PRODUCTS(BY UNITS SOLD)</div>
                            <div id=""><img src="/assests/images/HDF4.jpg" class="resposive-image"></div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="card">
                            <div class="leaderboardHeader">CATEGORY WISE LEAD (BY UNITS)</div>
                            <div id=""><img src="/assests/images/CWLEAD.jpg" class="resposive-image"></div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="card">
                            <div class="leaderboardHeader">CATEGORY WISE SALE (BY UNITS)</div>
                            <div id=""><img src="/assests/images/CWSALE.jpg" class="resposive-image"></div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="card">
                            <div class="leaderboardHeader">LEAST SELLING PRODUCTS (BY UNITS SOLD)</div>
                            <div id=""><img src="/assests/images/LEASTSE.jpg" class="resposive-image"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script>

            function NoFileAlert() {

                jAlert('No data for export');
            }

            function mapcloseclick() {
                $("#map").addClass("hide");
            }



            var objData = {};
            $(document).ready(function () {

                //Surojit

                $('#cmbState').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    onDropdownHide: function (event) {
                        GetSateValue();
                    }
                }).multiselect('selectAll', false).multiselect('updateButtonText');
                stateids = $('#cmbState').val();

                var statelistcount = $('#hdnStateListCount').val();
                if (statelistcount > 0) {
                    var GroupBy = $('#hdnGridStatewiseSummaryGroupBy').val();
                    for (var i = objsettings.length - 1; i >= 0; i--) {
                        if (objsettings[i].ID == settingsid) {
                            objsettings.splice(i, 1);
                        }
                    }

                    if (settingsid == "1") {
                        var obj = {};
                        obj.ID = "1";
                        obj.action = "AT_WORK";
                        obj.rptype = "Summary";
                        obj.empid = "";
                        obj.stateid = stateids.join(',');// cmbState.GetValue();
                        obj.designid = "";
                        objsettings.push(obj);
                    }

                    WindowSize = $(window).width();


                    $("#lblAtWork").html("<img src='/assests/images/Spinner.gif' />");
                    $("#lblOnLeave").html("<img src='/assests/images/Spinner.gif' />");
                    $("#lblNotLoggedIn").html("<img src='/assests/images/Spinner.gif' />");
                    $("#lblTotal").html("<img src='/assests/images/Spinner.gif' />");
                    stateid = stateids.join(',');// cmbState.GetValue();
                    $("#salesmanheader").html("State wise Summary");
                    stateid = stateids.join(',');// cmbState.GetValue();
                    //GetAddress(stateid);
                    objData = {};

                    var hdnTotalEmployees = $('#hdnTotalEmployees').val();
                    var hdnAtWork = $('#hdnAtWork').val();
                    var hdnOnLeave = $('#hdnOnLeave').val();
                    var hdnNotLoggedIn = $('#hdnNotLoggedIn').val();
                    var hdnStatewiseSummary = $('#hdnStatewiseSummary').val();

                    var obj = {};
                    obj.stateid = stateid;

                    if (hdnTotalEmployees > 0 || hdnAtWork > 0 || hdnOnLeave > 0 || hdnNotLoggedIn > 0) {
                        $.ajax({
                            type: "POST",
                            url: "/DashboardMenu/GetDashboardData",
                            data: JSON.stringify(obj),
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                if (hdnAtWork > 0) {
                                    $("#lblAtWork").html(response.lblAtWork);
                                }
                                if (hdnOnLeave > 0) {
                                    $("#lblOnLeave").html(response.lblOnLeave);
                                }
                                if (hdnNotLoggedIn > 0) {
                                    $("#lblNotLoggedIn").html(response.lblNotLoggedIn);
                                }
                                if (hdnTotalEmployees > 0) {
                                    $("#lblTotal").html(response.lblTotal);
                                }


                                //$("#lbltotalshop").text(response.TotalVisit);
                                //$("#lblnewvisit").text(response.NewVisit);
                                //$("#lblrevisit").text(response.ReVisit);

                                //$("#lblavgvisits").text(response.AvgPerDay);
                                //$("#lblavgduration").text(response.AvgDurationPerShop);

                                //$("#lbltodaysale").text(response.TODAYSALES);
                                //$("#lblavgsale").text(response.AVGSALES);
                                //$("#lbltotalsale").text(response.TOTALSALES);




                            },
                            error: function (response) {
                                jAlert("Please try again later");
                            }
                        });
                    }
                    if (hdnStatewiseSummary > 0) {
                        gridsalesman.ClearFilter();
                        gridsalesman.Refresh();
                    }
                    //gridsalesman.Refresh();
                    // $("#gridsalesman_DXMainTable > tbody > tr > td.dxgvHeader_PlasticBlue").css({ 'background': divBGcolor });
                    //setTimeout(function () { gridsalesman.GroupBy(GroupBy); }, 2000);
                }
                else {
                    $('.bodymain_areastatewise').hide();
                }

            });

            function gridsummarydashboardExport() {
                var url = '@Url.Action("ExportDashboardSummaryGridView", "DashboardMenu", new { type = "_type_" })'

                window.location.href = url.replace("_type_", 3);


            }

            function gridsalesmanExport() {
                var url = '@Url.Action("ExportDashboardGridViewSalesmanDetail", "DashboardMenu", new { type = "_type_" })'
                window.location.href = url.replace("_type_", 3);
            }

            function griddashboardgridviewexport() {
                var url = '@Url.Action("ExportDashboardGridView", "DashboardMenu", new { type = "_type_" })'
                window.location.href = url.replace("_type_", 3);
            }

            function griddashboardgridviewdetailsexport() {
                var url = '@Url.Action("ExportDashboardGridViewDetails", "DashboardMenu", new { type = "_type_" })'
                window.location.href = url.replace("_type_", 3);
            }





                </script>
    </form>
</body>
</html>
