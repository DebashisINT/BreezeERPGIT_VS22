$(function () {
    $.fn.datepicker.defaults.format = "dd-mm-yyyy";
    $(".datepicker").datepicker({
        //format: 'YYYY-MM-DD',
        autoclose: true,
        todayHighlight: true
    }).datepicker('update', new Date());

    $('.toggleFullScreen').click(function () {
        $(this).parent('.box-full').toggleClass('full-screen');
    });
});
function getdata() {
    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();

    var inputDatefrm = fromDate;
    var inputDateto = toDate;

    var newDatefrm = changeDateFormat(inputDatefrm);
    var newDateto = changeDateFormat(inputDateto);

    console.log(inputDatefrm);

    getTotalOrderCount(newDatefrm, newDateto);
    getTotalOrderValue(newDatefrm, newDateto);
    getAvgOrderValue(newDatefrm, newDateto);
    getOrderDelivered(newDatefrm, newDateto);

    gettop10orderValue(newDatefrm, newDateto);
    gettop10orderQty(newDatefrm, newDateto);

    getCustomer(newDatefrm, newDateto);
    getOrderStateWise(newDatefrm, newDateto);

    getOrderCountChart(newDatefrm, newDateto);
    getOrderTotalChart(newDatefrm, newDateto);
    getOrderDeliverChart(newDatefrm, newDateto);
}
function changeDateFormat(inputDate) {  // expects Y-m-d
    var splitDate = inputDate.split('-');
    if (splitDate.count == 0) {
        return null;
    }

    var year = splitDate[2];
    var month = splitDate[1];
    var day = splitDate[0];

    return year + '-' + month + '-' + day;
}


//order count chart
function getOrderCountChart(fromDate, toDate) {
    var obj = {};
    obj.fromDate = fromDate;
    obj.toDate = toDate;
    var Url = '@Url.Action("OrdercountChart", "DashboardMenu")';
    $.ajax({
        type: "POST",
        url: Url,
        data: JSON.stringify(obj),
        processData: false,
        contentType: 'application/json',
        success: function (msg) {
            //console.log(msg);
            if (msg.length > 0) {
                am4core.ready(function () {

                    am4core.useTheme(am4themes_animated);

                    var chart = am4core.create("chartLine", am4charts.XYChart);



                    chart.data = msg;

                    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                    categoryAxis.renderer.grid.template.location = 0;
                    categoryAxis.renderer.ticks.template.disabled = true;
                    categoryAxis.renderer.line.opacity = 0;
                    categoryAxis.renderer.grid.template.disabled = true;
                    categoryAxis.renderer.minGridDistance = 40;
                    categoryAxis.dataFields.category = "ORDERDATE";
                    categoryAxis.startLocation = 0.4;
                    categoryAxis.endLocation = 0.6;


                    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
                    valueAxis.tooltip.disabled = true;
                    valueAxis.renderer.line.opacity = 0;
                    valueAxis.renderer.ticks.template.disabled = true;
                    valueAxis.min = 0;

                    var lineSeries = chart.series.push(new am4charts.LineSeries());
                    lineSeries.dataFields.categoryX = "ORDERDATE";
                    lineSeries.dataFields.valueY = "ORDCNT";
                    lineSeries.tooltipText = "Order Count: {valueY.value}";
                    lineSeries.fillOpacity = 0.5;
                    lineSeries.strokeWidth = 3;
                    lineSeries.propertyFields.stroke = "lineColor";
                    lineSeries.propertyFields.fill = "lineColor";

                    var bullet = lineSeries.bullets.push(new am4charts.CircleBullet());
                    bullet.circle.radius = 6;
                    bullet.circle.fill = am4core.color("#fff");
                    bullet.circle.strokeWidth = 3;

                    chart.cursor = new am4charts.XYCursor();
                    chart.cursor.behavior = "panX";
                    chart.cursor.lineX.opacity = 0;
                    chart.cursor.lineY.opacity = 0;

                    chart.scrollbarX = new am4core.Scrollbar();
                    chart.scrollbarX.parent = chart.bottomAxesContainer;

                }); // end am4core.ready()
            } else {
                $('#chartLine').html('<div class="nCheck">No Data Available</div>');
            }

        }
    });
}
//order Total chart
function getOrderTotalChart(fromDate, toDate) {
    var obj = {};
    obj.fromDate = fromDate;
    obj.toDate = toDate;
    var Url = '@Url.Action("OrderTotalChart", "DashboardMenu")';
    $.ajax({
        type: "POST",
        url: Url,
        data: JSON.stringify(obj),
        processData: false,
        contentType: 'application/json',
        success: function (msg) {
            //console.log('total');
            //console.log(msg);
            if (msg.length > 0) {
                // Use themes
                am4core.useTheme(am4themes_animated);

                // Create chart instance
                var chart = am4core.create("chartLine2", am4charts.XYChart);
                chart.paddingRight = 20;

                // Add data
                chart.data = msg;

                // Create axes
                var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "ORDERDATE";
                categoryAxis.renderer.minGridDistance = 50;
                categoryAxis.renderer.grid.template.location = 0.5;
                categoryAxis.startLocation = 0.5;
                categoryAxis.endLocation = 0.5;

                // Pre zoom
                chart.events.on("datavalidated", function () {
                    categoryAxis.zoomToIndexes(Math.round(chart.data.length * 0.4), Math.round(chart.data.length * 0.55));
                });

                // Create value axis
                var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
                valueAxis.baseValue = 0;

                // Create series
                var series = chart.series.push(new am4charts.LineSeries());
                series.dataFields.valueY = "ORDVALUE";
                series.dataFields.categoryX = "ORDERDATE";
                series.strokeWidth = 3;
                series.tensionX = 0.8;
                series.tooltipText = "Order Value: {valueY.value}";

                var bullet = series.bullets.push(new am4charts.CircleBullet());
                bullet.strokeWidth = 0;

                bullet.adapter.add("fill", function (fill, target) {
                    var values = target.dataItem.values;

                    return values.valueY.value >= 0
                      ? am4core.color("red")
                      : fill;
                });

                //var range = valueAxis.createSeriesRange(series);
                //range.value = 0;
                //range.endValue = 1000;
                //range.contents.stroke = am4core.color("#FF0000");
                //range.contents.fill = range.contents.stroke;

                // Add scrollbar
                chart.cursor = new am4charts.XYCursor();
                chart.cursor.behavior = "panX";
                chart.cursor.lineX.opacity = 0;
                chart.cursor.lineY.opacity = 0;

                chart.scrollbarX = new am4core.Scrollbar();
                chart.scrollbarX.parent = chart.bottomAxesContainer;

            } else {
                $('#chartLine2').html('<div class="nCheck">No Data Available</div>');
            }

        }
    });
}

// Order Delivered
function getOrderDeliverChart(fromDate, toDate) {
    var obj = {};
    obj.fromDate = fromDate;
    obj.toDate = toDate;
    var Url = '@Url.Action("OrderDeliveredChart", "DashboardMenu")';
    $.ajax({
        type: "POST",
        url: Url,
        data: JSON.stringify(obj),
        processData: false,
        contentType: 'application/json',
        success: function (msg) {
            console.log('Delivered');
            console.log(msg);
            if (msg.length > 0) {
                // Use themes
                am4core.useTheme(am4themes_animated);

                // Create chart instance
                var chart = am4core.create("chartLine3", am4charts.XYChart);
                chart.paddingRight = 20;

                // Add data
                chart.data = msg;

                // Create axes
                var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "ORDERDATE";
                categoryAxis.renderer.minGridDistance = 50;
                categoryAxis.renderer.grid.template.location = 0.5;
                categoryAxis.startLocation = 0.5;
                categoryAxis.endLocation = 0.5;

                // Pre zoom
                chart.events.on("datavalidated", function () {
                    categoryAxis.zoomToIndexes(Math.round(chart.data.length * 0.4), Math.round(chart.data.length * 0.55));
                });

                // Create value axis
                var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
                valueAxis.baseValue = 0;

                // Create series
                var series = chart.series.push(new am4charts.LineSeries());
                series.dataFields.valueY = "BILLVALUE";
                series.dataFields.categoryX = "ORDERDATE";
                series.strokeWidth = 3;
                series.tensionX = 0.8;
                series.tooltipText = "Delivered Value: {valueY.value}";

                var bullet = series.bullets.push(new am4charts.CircleBullet());
                bullet.strokeWidth = 0;

                bullet.adapter.add("fill", function (fill, target) {
                    var values = target.dataItem.values;

                    return values.valueY.value >= 0
                      ? am4core.color("red")
                      : fill;
                });

                //var range = valueAxis.createSeriesRange(series);
                //range.value = 0;
                //range.endValue = 1000;
                //range.contents.stroke = am4core.color("#FF0000");
                //range.contents.fill = range.contents.stroke;

                // Add scrollbar
                chart.cursor = new am4charts.XYCursor();
                chart.cursor.behavior = "panX";
                chart.cursor.lineX.opacity = 0;
                chart.cursor.lineY.opacity = 0;

                chart.scrollbarX = new am4core.Scrollbar();
                chart.scrollbarX.parent = chart.bottomAxesContainer;

            } else {
                $('#chartLine3').html('<div class="nCheck">No Data Available</div>');
            }

        }
    });
}


// top customers
function getCustomer(fromDate, toDate) {
    var obj = {};
    obj.fromDate = fromDate;
    obj.toDate = toDate;
    var Url = '@Url.Action("GettopCustomers", "DashboardMenu")';
    $.ajax({
        type: "POST",
        url: Url,
        data: JSON.stringify(obj),
        processData: false,
        contentType: 'application/json',
        success: function (msg) {
            //console.log(msg)
            if (msg.length > 0) {
                //alert('hi')
                var Ltable = "";
                for (i = 0; i < msg.length; i++) {
                    Ltable += "<tr>"
                    Ltable += "<td>" + msg[i].SHOPNAME + "</td>"
                    Ltable += "<td class='text-center'>" + msg[i].ORDVALUE + "</td>"
                    Ltable += "</tr>"
                }
                Ltable += "";
                //console.log(obj)
                $('#topCustomersTble').html(Ltable)
            } else {
                $('#topCustomersTble').html("<tr><td colspan='6' class='text-center'>No Data Found</td></tr>")
            }
        }
    });
}
// top order 
function getOrderStateWise(fromDate, toDate) {
    var obj = {};
    obj.fromDate = fromDate;
    obj.toDate = toDate;
    var Url = '@Url.Action("GettopOrdersStateWise", "DashboardMenu")';
    $.ajax({
        type: "POST",
        url: Url,
        data: JSON.stringify(obj),
        processData: false,
        contentType: 'application/json',
        success: function (msg) {
            //console.log(msg)
            if (msg.length > 0) {
                var Ltable = "";
                for (i = 0; i < msg.length; i++) {
                    Ltable += "<tr>"
                    Ltable += "<td>" + msg[i].STATENAME + "</td>"
                    Ltable += "<td class='text-right'>" + msg[i].ORDVALUE + "</td>"
                    Ltable += "</tr>"
                }
                Ltable += "";
                //console.log(obj)
                $('#stateWise').html(Ltable)
            } else {
                $('#stateWise').html("<tr><td colspan='6' class='text-center'>No Data Found</td></tr>")
            }
        }
    });
}

// top 10 order value
function gettop10orderValue(fromDate, toDate) {
    var obj = {};
    obj.fromDate = fromDate;
    obj.toDate = toDate;
    var Url = '@Url.Action("Gettop10orderValue", "DashboardMenu")';
    $.ajax({
        type: "POST",
        url: Url,
        data: JSON.stringify(obj),
        processData: false,
        contentType: 'application/json',
        success: function (msg) {
            //console.log(msg);
            if (msg.length > 0) {
                am4core.ready(function () {

                    // Themes begin
                    am4core.useTheme(am4themes_animated);
                    // Themes end

                    // Create chart instance
                    var chart = am4core.create("pieOne", am4charts.PieChart);

                    // Add data
                    chart.data = msg;

                    // Add and configure Series
                    var pieSeries = chart.series.push(new am4charts.PieSeries());
                    pieSeries.dataFields.value = "ORDVALUE";
                    pieSeries.dataFields.category = "PRODUCT";

                    // Disable ticks and labels
                    //pieSeries.labels.template.disabled = true;
                    pieSeries.ticks.template.disabled = true;
                    pieSeries.alignLabels = false;
                    pieSeries.labels.template.text = "{value.percent.formatNumber('#.0')}%";
                    pieSeries.labels.template.radius = am4core.percent(-40);
                    pieSeries.labels.template.fill = am4core.color("white");

                    pieSeries.ticks.template.adapter.add("hidden", hideSmall);
                    pieSeries.labels.template.adapter.add("hidden", hideSmall);

                    function hideSmall(hidden, target) {
                        return target.dataItem.values.value.percent < 5 ? true : false;
                    }
                    // Add a legend
                    chart.legend = new am4charts.Legend();
                    chart.legend.position = "right";
                    chart.legend.maxHeight = 250;
                    chart.legend.scrollable = true;


                }); // end am4core.ready()
            } else {
                $('#pieOne').html('<div class="nCheck">No Data Available</div>');
            }

        }
    });
}
// top 10 order quantity
function gettop10orderQty(fromDate, toDate) {
    var obj = {};
    obj.fromDate = fromDate;
    obj.toDate = toDate;
    var Url = '@Url.Action("Gettop10orderQuantity", "DashboardMenu")';
    $.ajax({
        type: "POST",
        url: Url,
        data: JSON.stringify(obj),
        processData: false,
        contentType: 'application/json',
        success: function (msg) {
            if (msg.length > 0) {
                am4core.ready(function () {

                    // Themes begin
                    am4core.useTheme(am4themes_animated);
                    // Themes end

                    // Create chart instance
                    var chart = am4core.create("pieTwo", am4charts.PieChart);

                    // Add data
                    chart.data = msg;

                    // Add and configure Series
                    var pieSeries = chart.series.push(new am4charts.PieSeries());
                    pieSeries.dataFields.value = "ORDQTY";
                    pieSeries.dataFields.category = "PRODUCT";
                    pieSeries.colors.step = 4;
                    // Disable ticks and labels
                    // pieSeries.labels.template.disabled = true;
                    //pieSeries.ticks.template.disabled = true;
                    pieSeries.ticks.template.disabled = true;
                    pieSeries.alignLabels = false;
                    pieSeries.labels.template.text = "{value.percent.formatNumber('#.0')}%";
                    pieSeries.labels.template.radius = am4core.percent(-40);
                    pieSeries.labels.template.fill = am4core.color("white");
                    pieSeries.ticks.template.adapter.add("hidden", hideSmall);
                    pieSeries.labels.template.adapter.add("hidden", hideSmall);

                    function hideSmall(hidden, target) {
                        return target.dataItem.values.value.percent < 5 ? true : false;
                    }
                    // Add a legend
                    chart.legend = new am4charts.Legend();
                    chart.legend.position = "right";
                    chart.legend.maxHeight = 250;
                    chart.legend.scrollable = true;

                }); // end am4core.ready()
            } else {
                $('#pieTwo').html('<div class="nCheck">No Data Available</div>');
            }
            //console.log(msg);

        }
    });
}

function getTotalOrderCount(fromDate, toDate) {
    var obj = {};
    obj.fromDate = fromDate;
    obj.toDate = toDate;
    var Url = '@Url.Action("GetOrderAnalyticTotalOrderCount", "DashboardMenu")';
    $.ajax({
        type: "POST",
        url: Url,
        data: JSON.stringify(obj),
        processData: false,
        contentType: 'application/json',
        success: function (msg) {
            //console.log(msg);
            $('#Total_oc').text(msg[0].ORDCNT);
        }
    });
}
function getTotalOrderValue(fromDate, toDate) {
    var obj = {};
    obj.fromDate = fromDate;
    obj.toDate = toDate;
    var Url = '@Url.Action("GetOrderAnalyticTotalOrderValue", "DashboardMenu")';
    $.ajax({
        type: "POST",
        url: Url,
        data: JSON.stringify(obj),
        processData: false,
        contentType: 'application/json',
        success: function (msg) {
            //console.log(msg[0].ORDVALUE);
            $('#TOTAL_ov').text(msg[0].ORDVALUE);
        }
    });
}
function getAvgOrderValue(fromDate, toDate) {
    var obj = {};
    obj.fromDate = fromDate;
    obj.toDate = toDate;
    var Url = '@Url.Action("GetOrderAnalyticAvgOrderValue", "DashboardMenu")';
    $.ajax({
        type: "POST",
        url: Url,
        data: JSON.stringify(obj),
        processData: false,
        contentType: 'application/json',
        success: function (msg) {

            $('#TOTAL_aov').text(msg[0].AVGORDVALUE);
        }
    });
}
function getOrderDelivered(fromDate, toDate) {
    var obj = {};
    obj.fromDate = fromDate;
    obj.toDate = toDate;
    var Url = '@Url.Action("GetOrderAnalyticOrderDelivered", "DashboardMenu")';
    $.ajax({
        type: "POST",
        url: Url,
        data: JSON.stringify(obj),
        processData: false,
        contentType: 'application/json',
        success: function (msg) {
            $('#TOTAL_ORDDELV').text(msg[0].ORDDELV);
        }
    });
}
