
$(function () {
    $('#loaderP, .loaderPC').hide();
    $.fn.datepicker.defaults.format = "dd-mm-yyyy";
    $(".datepicker").datepicker({
        //format: 'yyyy-mm-dd',
        autoclose: true,
        todayHighlight: true
    }).datepicker('update', new Date());
    $('body').on('click', '.toggleFullScreen', function (e) {
        $(this).parent(".box-full").toggleClass("full-screen");
    });

    var dateSplitArr = new Date().toLocaleDateString().split('/');
    function dateString(x) {
        var month = '';
        var day = '';
        var year = '';
            if (x[0].length > 1) {
                month = x[0]
            } else {
                month = '0' +x[0]
            }
            if (x[1].length > 1) {
                day = x[1]
            } else {
                day = '0' + x[1]
            }
            if (x[2].length > 1) {
                year = x[2]
            } else {
                year = '0' + x[2]
            }
            return year + '-' + month + '-' + day
        }
    var nBranch = cddlBranch.GetValue();
    if (nBranch == "0") {
        nBranch = "";
    }
    var asOnDate = dateString(dateSplitArr);
        var obj = {};
        obj.date = asOnDate;
        obj.branchid = cddlBranch.GetValue();
        obj.ProdClass = $('#hdnClassId').val();
        obj.Prodid = $('#hdnProductId').val();
        $.ajax({
            type: "POST",
            url: "../service/general.asmx/GetRequisition",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(obj),
            dataType: "json",
            success: function (data) {
                console.log('InventoryDB.aspx', data)
                $('#brReq').text(data.d[0].BRANCHREQ);
                $('#brReqAP').text(data.d[0].APPRPENDING);
                $('#brReqOP').text(data.d[0].OPENREQ);
                $('#brReqCL').text(data.d[0].CLOSEREQ);
                $('#brReqAR').text(data.d[0].APPRREQ);
            }
        })
        $.ajax({
            type: "POST",
            url: "../service/general.asmx/GetProcurement",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(obj),
            dataType: "json",
            success: function (data) {
                //console.log('GetProcurement', data)
                $('#PURCHASEREQ').text(data.d[0].PURCHASEREQ);
                $('#OPENREQ').text(data.d[0].OPENREQ);
                $('#TOTPO').text(data.d[0].TOTPO);
                $('#APPRPO').text(data.d[0].APPRPO);
                $('#APPRREQ').text(data.d[0].APPRREQ);
            }
        })
    });
function getdataForBox() {
    var fromDate = $('#toDateRE').val();
    var inputDatefrm = fromDate;
    var newDatefrm = changeDateFormat(inputDatefrm);
    var nBranch = cddlBranch.GetValue();
    if (nBranch == "0") {
        nBranch = "";
    }

    $('.boxLoad #loaderP').show();
    var dt = {};
    dt.date = newDatefrm;
    dt.branchid = nBranch;
    dt.ProdClass = $('#hdnClassId').val();
    dt.Prodid = $('#hdnProductId').val();
    console.log(dt);
    $.ajax({
        type: "POST",
        url: "../service/general.asmx/GetInventoryBox",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            console.log(data.d)
            var vst = Math.round(data.d[0].TOTALVAL)
                
            $('#TotalItems').text(data.d[0].TOTALINVPROD);
            $('#StInHand').text(numberWithCommas(vst));
            $('#AvforSale').text(data.d[0].TOTALPROD);
            $('#CmforSale').text(data.d[0].TOTALPRODSC);
            $('#ItQtyIn').text(data.d[0].TOTALPRODIN);
            $('#ItQtyOut').text(data.d[0].TOTALPRODOUT);
            $('.boxLoad #loaderP').hide();
        }
    });
}
//function kFormatter(num) {
//    return Math.abs(num) > 999 ? Math.sign(num) * ((Math.abs(num) / 1000).toFixed(1)) + 'k' : Math.sign(num) * Math.abs(num)
//}
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
function numberWithCommas(x) {
    //x = x.toString();
    //var pattern = /(-?\d+)(\d{3})/;
    //while (pattern.test(x))
    //    x = x.replace(pattern, "$1,$2");
    //return x; 
    x = x.toString();
    if (x.toString().indexOf('.') == -1) {
        var lastThree = x.substring(x.length - 3);
        var otherNumbers = x.substring(0, x.length - 3);
        if (otherNumbers != '')
            lastThree = ',' + lastThree;
        var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;

    } else {
        var dec = x.substr(x.indexOf('.') + 1, x.length);
        x = x.substr(0, x.indexOf('.'))
        var lastThree = x.substring(x.length - 3);
        var otherNumbers = x.substring(0, x.length - 3);
        if (otherNumbers != '')
            lastThree = ',' + lastThree;
        var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree + '.' + dec;
    }

    return res;
}
function getdataForChart() {

     
    var fromDate = $('#toDateChart').val();
    var inputDatefrm = fromDate;
    var newDatefrm = changeDateFormat(inputDatefrm);
    var nBranch = cddlBranch.GetValue();
    if (nBranch == "0") {
        nBranch = "";
    }
    $('.chartLoad .loaderPC').show();
    var dt = {};
    dt.date = newDatefrm;
    dt.branchid = nBranch;
    dt.ProdClass = $('#hdnClassId').val();
    dt.Prodid = $('#hdnProductId').val();
    getdataForBox();
    $.ajax({
        type: "POST",
        url: "../service/general.asmx/GetInventoryChartOne",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            console.log('GetInventoryChartOne', data)
            
            am4core.ready(function() {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end

                // Create chart instance
                var chart = am4core.create("chartdiv1", am4charts.XYChart3D);
                chart.scrollbarX = new am4core.Scrollbar();

                // Add data
                chart.data = data.d;

                let categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "PRODDESC";
                categoryAxis.renderer.labels.template.rotation = 270;
                categoryAxis.renderer.labels.template.hideOversized = false;
                categoryAxis.renderer.minGridDistance = 20;
                categoryAxis.renderer.labels.template.horizontalCenter = "right";
                categoryAxis.renderer.labels.template.verticalCenter = "middle";
                categoryAxis.tooltip.label.rotation = 270;
                categoryAxis.tooltip.label.horizontalCenter = "right";
                categoryAxis.tooltip.label.verticalCenter = "middle";
                categoryAxis.cursorTooltipEnabled = false;
                chart.tooltip.label.maxWidth = 350;
                chart.tooltip.label.wrap = true;
                var label = categoryAxis.renderer.labels.template;
                label.truncate = true;
                //label.wrap = true;
                chart.logo.disabled = true;
                label.maxWidth = 160;


                let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
                valueAxis.title.text = "Values";
                valueAxis.title.fontWeight = "bold";
                valueAxis.cursorTooltipEnabled = false;
                // Create series
                var series = chart.series.push(new am4charts.ColumnSeries3D());
                series.dataFields.valueY = "QTY";
                series.dataFields.categoryX = "PRODDESC";
                series.name = "Products";
                series.tooltipText = "{categoryX}: [bold]{valueY}[/]";
                series.columns.template.fillOpacity = .8;
                series.tooltip.label.wrap = true;

                // optional
                series.tooltip.label.width = 300;
                var columnTemplate = series.columns.template;
                columnTemplate.strokeWidth = 2;
                columnTemplate.strokeOpacity = 1;
                columnTemplate.stroke = am4core.color("#FFFFFF");

                columnTemplate.adapter.add("fill", function(fill, target) {
                    return chart.colors.getIndex(target.dataItem.index);
                })

                columnTemplate.adapter.add("stroke", function(stroke, target) {
                    return chart.colors.getIndex(target.dataItem.index);
                })

                chart.cursor = new am4charts.XYCursor();
                chart.cursor.lineX.strokeOpacity = 0;
                chart.cursor.lineY.strokeOpacity = 0;



                //// Create axes
                //var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                //categoryAxis.dataFields.category = "PRODDESC";
                //categoryAxis.renderer.grid.template.location = 0;
                //categoryAxis.renderer.minGridDistance = 30;
                //categoryAxis.renderer.labels.template.horizontalCenter = "right";
                //categoryAxis.renderer.labels.template.verticalCenter = "middle";
                //categoryAxis.renderer.labels.template.rotation = 270;
                //categoryAxis.tooltip.disabled = true;
                //categoryAxis.renderer.minHeight = 110;
                //var label = categoryAxis.renderer.labels.template;
                //label.truncate = true;
                ////label.wrap = true;
                //chart.logo.disabled = true;
                //label.maxWidth = 160;
                //label.tooltipText = "{categoryX}";

                //var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
                //valueAxis.renderer.minWidth = 50;

                //// Create series
                //var series = chart.series.push(new am4charts.ColumnSeries());
                //series.sequencedInterpolation = true;
                //series.dataFields.valueY = "QTY";
                //series.dataFields.categoryX = "PRODDESC";
                //series.tooltipText = "dfg[{valueX}: bold]{valueY}[/]";
                //series.columns.template.strokeWidth = 0;

                //series.tooltip.pointerOrientation = "vertical";

                //series.columns.template.column.cornerRadiusTopLeft = 10;
                //series.columns.template.column.cornerRadiusTopRight = 10;
                //series.columns.template.column.fillOpacity = 0.8;

                //// on hover, make corner radiuses bigger
                //var hoverState = series.columns.template.column.states.create("hover");
                //hoverState.properties.cornerRadiusTopLeft = 0;
                //hoverState.properties.cornerRadiusTopRight = 0;
                //hoverState.properties.fillOpacity = 1;

                //series.columns.template.adapter.add("fill", function(fill, target) {
                //    return chart.colors.getIndex(target.dataItem.index);
                //});

                //// Cursor
                //chart.cursor = new am4charts.XYCursor();

            }); // end am4core.ready()
            $('.chartLoad .loaderPC.l1').hide();
        }
    });
    $.ajax({
        type: "POST",
        url: "../service/general.asmx/GetInventoryChartTwo",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            //createBankList(data.d)
            console.log('GetInventoryChartTwo', data);
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end

                // Create chart instance
                var chart = am4core.create("chartdiv2", am4charts.XYChart);
                chart.scrollbarX = new am4core.Scrollbar();

                // Add data
                chart.data = data.d;
                let categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "PRODDESC";
                categoryAxis.renderer.labels.template.rotation = 270;
                categoryAxis.renderer.labels.template.hideOversized = false;
                categoryAxis.renderer.minGridDistance = 20;
                categoryAxis.renderer.labels.template.horizontalCenter = "right";
                categoryAxis.renderer.labels.template.verticalCenter = "middle";
                categoryAxis.tooltip.label.rotation = 270;
                categoryAxis.tooltip.label.horizontalCenter = "right";
                categoryAxis.tooltip.label.verticalCenter = "middle";
                categoryAxis.cursorTooltipEnabled = false;
                chart.tooltip.label.maxWidth = 350;
                chart.tooltip.label.wrap = true;
                var label = categoryAxis.renderer.labels.template;
                label.truncate = true;
                //label.wrap = true;
                chart.logo.disabled = true;
                label.maxWidth = 160;


                let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
                valueAxis.title.text = "Values";
                valueAxis.title.fontWeight = "bold";
                valueAxis.cursorTooltipEnabled = false;
                // Create series
                var series = chart.series.push(new am4charts.ColumnSeries3D());
                series.dataFields.valueY = "QTY";
                series.dataFields.categoryX = "PRODDESC";
                series.name = "Products";
                series.tooltipText = "{categoryX}: [bold]{valueY}[/]";
                series.columns.template.fillOpacity = .8;
                series.tooltip.label.wrap = true;

                // optional
                series.tooltip.label.width = 300;
                var columnTemplate = series.columns.template;
                columnTemplate.strokeWidth = 2;
                columnTemplate.strokeOpacity = 1;
                columnTemplate.stroke = am4core.color("#FFFFFF");

                columnTemplate.adapter.add("fill", function (fill, target) {
                    return chart.colors.getIndex(target.dataItem.index);
                })

                columnTemplate.adapter.add("stroke", function (stroke, target) {
                    return chart.colors.getIndex(target.dataItem.index);
                })

                chart.cursor = new am4charts.XYCursor();
                chart.cursor.lineX.strokeOpacity = 0;
                chart.cursor.lineY.strokeOpacity = 0;
                // Create axes
                //var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                //categoryAxis.dataFields.category = "PRODDESC";
                //categoryAxis.renderer.grid.template.location = 0;
                //categoryAxis.renderer.minGridDistance = 30;
                //categoryAxis.renderer.labels.template.horizontalCenter = "right";
                //categoryAxis.renderer.labels.template.verticalCenter = "middle";
                //categoryAxis.renderer.labels.template.rotation = 270;
                //categoryAxis.tooltip.disabled = true;
                //categoryAxis.renderer.minHeight = 110;
                //var label = categoryAxis.renderer.labels.template;
                //label.truncate = true;
                ////label.wrap = true;
                //chart.logo.disabled = true;
                //label.maxWidth = 160;
                //label.tooltipText = "{categoryX}";
                //var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
                //valueAxis.renderer.minWidth = 50;

                //// Create series
                //var series = chart.series.push(new am4charts.ColumnSeries());
                //series.sequencedInterpolation = true;
                //series.dataFields.valueY = "QTY";
                //series.dataFields.categoryX = "PRODDESC";
                //series.tooltipText = "[{categoryX}: bold]{valueY}[/]";
                //series.columns.template.strokeWidth = 0;

                //series.tooltip.pointerOrientation = "vertical";

                //series.columns.template.column.cornerRadiusTopLeft = 10;
                //series.columns.template.column.cornerRadiusTopRight = 10;
                //series.columns.template.column.fillOpacity = 0.8;

                //// on hover, make corner radiuses bigger
                //var hoverState = series.columns.template.column.states.create("hover");
                //hoverState.properties.cornerRadiusTopLeft = 0;
                //hoverState.properties.cornerRadiusTopRight = 0;
                //hoverState.properties.fillOpacity = 1;

                //series.columns.template.adapter.add("fill", function (fill, target) {
                //    return chart.colors.getIndex(target.dataItem.index);
                //});

                //// Cursor
                //chart.cursor = new am4charts.XYCursor();

            }); // end am4core.ready()
            $('.chartLoad .loaderPC.l2').hide();
        }
    });
    $.ajax({
        type: "POST",
        url: "../service/general.asmx/GetInventoryChartThree",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            //createBankList(data.d)
            console.log('GetInventoryChartThree', data)
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end

                // Create chart instance
                var chart = am4core.create("chartdiv3", am4charts.XYChart);
                chart.scrollbarX = new am4core.Scrollbar();

                // Add data
                chart.data = data.d;
                let categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "PRODDESC";
                categoryAxis.renderer.labels.template.rotation = 270;
                categoryAxis.renderer.labels.template.hideOversized = false;
                categoryAxis.renderer.minGridDistance = 20;
                categoryAxis.renderer.labels.template.horizontalCenter = "right";
                categoryAxis.renderer.labels.template.verticalCenter = "middle";
                categoryAxis.tooltip.label.rotation = 270;
                categoryAxis.tooltip.label.horizontalCenter = "right";
                categoryAxis.tooltip.label.verticalCenter = "middle";
                chart.tooltip.label.maxWidth = 200;
                chart.tooltip.label.wrap = true;
                var label = categoryAxis.renderer.labels.template;
                categoryAxis.cursorTooltipEnabled = false;
                label.truncate = true;
                //label.wrap = true;
                chart.logo.disabled = true;
                label.maxWidth = 160;


                let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
                valueAxis.title.text = "Values";
                valueAxis.title.fontWeight = "bold";
                valueAxis.cursorTooltipEnabled = false;
                // Create series
                var series = chart.series.push(new am4charts.ColumnSeries3D());
                series.dataFields.valueY = "QTY";
                series.dataFields.categoryX = "PRODDESC";
                series.name = "Products";
                series.tooltipText = "{categoryX}: [bold]{valueY}[/]";
                series.columns.template.fillOpacity = .8;
                series.tooltip.label.wrap = true;

                // optional
                series.tooltip.label.width = 300;
                var columnTemplate = series.columns.template;
                columnTemplate.strokeWidth = 2;
                columnTemplate.strokeOpacity = 1;
                columnTemplate.stroke = am4core.color("#FFFFFF");

                columnTemplate.adapter.add("fill", function (fill, target) {
                    return chart.colors.getIndex(target.dataItem.index);
                })

                columnTemplate.adapter.add("stroke", function (stroke, target) {
                    return chart.colors.getIndex(target.dataItem.index);
                })

                chart.cursor = new am4charts.XYCursor();
                chart.cursor.lineX.strokeOpacity = 0;
                chart.cursor.lineY.strokeOpacity = 0;
                // Create axes
                //var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                //categoryAxis.dataFields.category = "PRODDESC";
                //categoryAxis.renderer.grid.template.location = 0;
                //categoryAxis.renderer.minGridDistance = 30;
                //categoryAxis.renderer.labels.template.horizontalCenter = "right";
                //categoryAxis.renderer.labels.template.verticalCenter = "middle";
                //categoryAxis.renderer.labels.template.rotation = 270;
                //categoryAxis.tooltip.disabled = true;
                //categoryAxis.renderer.minHeight = 110;
                //var label = categoryAxis.renderer.labels.template;
                //label.truncate = true;
                ////label.wrap = true;
                //chart.logo.disabled = true;
                //label.maxWidth = 160;
                //label.tooltipText = "{categoryX}";
                //var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
                //valueAxis.renderer.minWidth = 50;

                //// Create series
                //var series = chart.series.push(new am4charts.ColumnSeries());
                //series.sequencedInterpolation = true;
                //series.dataFields.valueY = "QTY";
                //series.dataFields.categoryX = "PRODDESC";
                //series.tooltipText = "[{categoryX}: bold]{valueY}[/]";
                //series.columns.template.strokeWidth = 0;

                //series.tooltip.pointerOrientation = "vertical";

                //series.columns.template.column.cornerRadiusTopLeft = 10;
                //series.columns.template.column.cornerRadiusTopRight = 10;
                //series.columns.template.column.fillOpacity = 0.8;

                //// on hover, make corner radiuses bigger
                //var hoverState = series.columns.template.column.states.create("hover");
                //hoverState.properties.cornerRadiusTopLeft = 0;
                //hoverState.properties.cornerRadiusTopRight = 0;
                //hoverState.properties.fillOpacity = 1;

                //series.columns.template.adapter.add("fill", function (fill, target) {
                //    return chart.colors.getIndex(target.dataItem.index);
                //});

                //// Cursor
                //chart.cursor = new am4charts.XYCursor();

            }); // end am4core.ready()
            $('.chartLoad .loaderPC.l3').hide();
        }
    });
    $.ajax({
        type: "POST",
        url: "../service/general.asmx/GetInventoryChartFour",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            console.log('GetInventoryChartFour', data)
            //createBankList(data.d)
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end

                // Create chart instance
                var chart = am4core.create("chartdiv4", am4charts.XYChart);
                chart.scrollbarX = new am4core.Scrollbar();

                // Add data
                chart.data = data.d;
                let categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "PRODDESC";
                categoryAxis.renderer.labels.template.rotation = 270;
                categoryAxis.renderer.labels.template.hideOversized = false;
                categoryAxis.renderer.minGridDistance = 20;
                categoryAxis.renderer.labels.template.horizontalCenter = "right";
                categoryAxis.renderer.labels.template.verticalCenter = "middle";
                categoryAxis.tooltip.label.rotation = 270;
                categoryAxis.tooltip.label.horizontalCenter = "right";
                categoryAxis.tooltip.label.verticalCenter = "middle";
                categoryAxis.cursorTooltipEnabled = false;
                chart.tooltip.label.maxWidth = 200;
                chart.tooltip.label.wrap = true;
                var label = categoryAxis.renderer.labels.template;
                label.truncate = true;
                //label.wrap = true;
                chart.logo.disabled = true;
                label.maxWidth = 160;


                let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
                valueAxis.title.text = "Values";
                valueAxis.title.fontWeight = "bold";
                valueAxis.cursorTooltipEnabled = false;
                // Create series
                var series = chart.series.push(new am4charts.ColumnSeries3D());
                series.dataFields.valueY = "QTY";
                series.dataFields.categoryX = "PRODDESC";
                series.name = "Products";
                series.tooltipText = "{categoryX}: [bold]{valueY}[/]";
                series.columns.template.fillOpacity = .8;
                series.tooltip.label.wrap = true;

                // optional
                series.tooltip.label.width = 300;
                var columnTemplate = series.columns.template;
                columnTemplate.strokeWidth = 2;
                columnTemplate.strokeOpacity = 1;
                columnTemplate.stroke = am4core.color("#FFFFFF");

                columnTemplate.adapter.add("fill", function(fill, target) {
                    return chart.colors.getIndex(target.dataItem.index);
                })

                columnTemplate.adapter.add("stroke", function(stroke, target) {
                    return chart.colors.getIndex(target.dataItem.index);
                })

                chart.cursor = new am4charts.XYCursor();
                chart.cursor.lineX.strokeOpacity = 0;
                chart.cursor.lineY.strokeOpacity = 0;


                // Create axes
                //var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                //categoryAxis.dataFields.category = "PRODDESC";
                //categoryAxis.renderer.grid.template.location = 0;
                //categoryAxis.renderer.minGridDistance = 30;
                //categoryAxis.renderer.labels.template.horizontalCenter = "right";
                //categoryAxis.renderer.labels.template.verticalCenter = "middle";
                //categoryAxis.renderer.labels.template.rotation = 270;
                //categoryAxis.tooltip.disabled = true;
                //categoryAxis.renderer.minHeight = 110;
                //var label = categoryAxis.renderer.labels.template;
                //label.truncate = true;
                ////label.wrap = true;
                //chart.logo.disabled = true;
                //label.maxWidth = 160;
                //label.tooltipText = "{categoryX}";
                //var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
                //valueAxis.renderer.minWidth = 50;

                //// Create series
                //var series = chart.series.push(new am4charts.ColumnSeries());
                //series.sequencedInterpolation = true;
                //series.dataFields.valueY = "QTY";
                //series.dataFields.categoryX = "PRODDESC";
                //series.tooltipText = "[{categoryX}: bold]{valueY}[/]";
                //series.columns.template.strokeWidth = 0;

                //series.tooltip.pointerOrientation = "vertical";

                //series.columns.template.column.cornerRadiusTopLeft = 10;
                //series.columns.template.column.cornerRadiusTopRight = 10;
                //series.columns.template.column.fillOpacity = 0.8;

                //// on hover, make corner radiuses bigger
                //var hoverState = series.columns.template.column.states.create("hover");
                //hoverState.properties.cornerRadiusTopLeft = 0;
                //hoverState.properties.cornerRadiusTopRight = 0;
                //hoverState.properties.fillOpacity = 1;

                //series.columns.template.adapter.add("fill", function (fill, target) {
                //    return chart.colors.getIndex(target.dataItem.index);
                //});

                //// Cursor
                //chart.cursor = new am4charts.XYCursor();

            }); // end am4core.ready()
            $('.chartLoad .loaderPC.l4').hide();
        }
    });
}





//<%-- For Class multiselection--%>


$(document).ready(function () {
    $('#ClassModel').on('shown.bs.modal', function () {
        $('#txtClassSearch').focus();
    })

})
var ClassArr = new Array();
$(document).ready(function () {
    var ClassObj = new Object();
    ClassObj.Name = "ClassSource";
    ClassObj.ArraySource = ClassArr;
    arrMultiPopup.push(ClassObj);
})
function ClassButnClick(s, e) {
    $('#ClassModel').modal('show');
}

function Class_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#ClassModel').modal('show');
    }
}

function Classkeydown(e) {
    var OtherDetails = {}

    if ($.trim($("#txtClassSearch").val()) == "" || $.trim($("#txtClassSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtClassSearch").val();

    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Class Name");


        if ($("#txtClassSearch").val() != "") {
            callonServerM("../Service/Mastered.asmx/GetClass", OtherDetails, "ClassTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ClassSource");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[dPropertyIndex=0]"))
            $("input[dPropertyIndex=0]").focus();
    }
}

function SetfocusOnseach(indexName) {
    if (indexName == "dPropertyIndex")
        $('#txtClassSearch').focus();
    else
        $('#txtClassSearch').focus();
}

function SetSelectedValues(Id, Name, ArrName) {
    if (ArrName == 'ClassSource') {
        var key = Id;
        if (key != null && key != '') {
            $('#ClassModel').modal('hide');
            ctxtClass.SetText(Name);
            $('#hdnClassId').val(key);


            ctxtProdName.SetText('');
            $('#txtProdSearch').val('')

            var OtherDetailsProd = {}
            OtherDetailsProd.SearchKey = 'undefined text';
            OtherDetailsProd.ClassID = '';
            var HeaderCaption = [];
            HeaderCaption.push("Code");
            HeaderCaption.push("Name");
            HeaderCaption.push("Hsn");

            callonServerM("../Service/Mastered.asmx/GetClassWiseProduct", OtherDetailsProd, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");

        }
        else {
            ctxtClass.SetText('');
            //GetObjectID('hdnClassId').value = '';
            $('#hdnClassId').val('');
        }
    }
    else if (ArrName == 'ProductSource') {
        var key = Id;
        if (key != null && key != '') {
            $('#ProdModel').modal('hide');
            ctxtProdName.SetText(Name);
            //GetObjectID('').value = key;
            $('#hdnProductId').val(key);
        }
        else {
            ctxtProdName.SetText('');
            //GetObjectID('hdnProductId').value = '';
            $('#hdnProductId').val("");
        }
    }
}


//<%-- For Class multiselection--%>

var ProdArr = new Array();
$(document).ready(function () {
    var ProdObj = new Object();
    ProdObj.Name = "ProductSource";
    ProdObj.ArraySource = ProdArr;
    arrMultiPopup.push(ProdObj);
})

function ProductButnClick(s, e) {
    $('#ProdModel').modal('show');
}

function Product_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#ProdModel').modal('show');
    }
}
$(document).ready(function () {
    $('#ProdModel').on('shown.bs.modal', function () {
        $('#txtProdSearch').focus();
    })
})
function Productkeydown(e) {
    var OtherDetails = {}

    if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtProdSearch").val();
    OtherDetails.ClassID = "";

    if (e.code == "Enter" || e.code == "NumpadEnter") {

        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        HeaderCaption.push("Hsn");

        if ($("#txtProdSearch").val() != "") {
            callonServerM("../Service/Mastered.asmx/GetClassWiseProduct", OtherDetails, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[dPropertyIndex=0]"))
            $("input[dPropertyIndex=0]").focus();
    }
}

function SetfocusOnseach(indexName) {
    if (indexName == "dPropertyIndex")
        $('#txtProdSearch').focus();
    else
        $('#txtProdSearch').focus();
}

