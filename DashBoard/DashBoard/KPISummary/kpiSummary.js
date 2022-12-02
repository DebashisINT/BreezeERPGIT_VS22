
$(function () {
    
    // Get branch
    $.ajax({
        type: "POST",
        url: "../service/general.asmx/GetBranch",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(),
        dataType: "json",
        success: function (data) {
            createBranchList(data.d)
            //console.log(data);
        }
    });
    $("#empSelect").change(function () {
        var emp = document.getElementById("empSelect");
        var empID = emp.options[emp.selectedIndex].value;
        console.log(empID);
    });
    //Branch change
    //$("#branchSelect").change(function () {
    //    var bran = document.getElementById("branchSelect");
    //    var branID = bran.options[bran.selectedIndex].value;
        
    //    console.log(branID)
    //    getEmpByBranch(branID);
    //});

    //button click
    $('#getData').click(function () {
        var brnch = $('#branchSelect').val();
        var date = $('#toDateRE').val();
        var emp = $('#empSelect').val();
        console.log(date, brnch, emp)
        GetperformanceTop(date, brnch, emp);
        getChart(date, brnch, emp);
        getActivities(date, brnch, emp);
        getEmplyee(date, brnch, emp);
    });
    var brnch = $('#branchSelect').val();
    console.log('brnch', brnch)
    getEmpByBranch(brnch);
})

// get Employee info

function getEmplyee(date, brnch, emp) {
    var asDate = changeDateFormat(date);
    var dt = {};
    dt.ASONDATE = asDate;
    dt.BRANCH_ID = "";
    dt.EMPID = emp;
    $.ajax({
        type: "POST",
        url: "kpisummary.aspx/GetEmployeeTab",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            console.log('GetEmployeetab', data);
            $('#WORKINGDAYS').html(data.d[0].WORKINGDAYS);
            $('#PRESENTS').html(data.d[0].PRESENTS);
            $('#LEAVES').html(data.d[0].LEAVES);
            $('#HALFDAYS').html(data.d[0].HALFDAYS);
            $('#EMPCTC').html(numFormatter(data.d[0].EMPCTC));
            $('#EXPAMT').html(data.d[0].EXPAMT);
            
        }
    });
       
}

function getActivities(date, brnch, emp) {
    var asDate = changeDateFormat(date);
    var dt = {};
    dt.ASONDATE = asDate;
    dt.BRANCH_ID = brnch;
    dt.EMPID = emp;
    $.ajax({
        type: "POST",
        url: "kpisummary.aspx/GetActivitiesBox",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            console.log('GetActivitiesBox', data);
            var Data = data.d;
            //act
            if (Data[0].ACTIVCNT == "" || Data[0].ACTIVCNT == null) {
                $('#actValue').text("0");
            } else {
                $('#actValue').text(Data[0].ACTIVCNT);
            }
            //email
            if (Data[0].EMAILCNT == "" || Data[0].EMAILCNT == null) {
                $('#emailValue').text("0");
            } else {
                $('#emailValue').text(Data[0].EMAILCNT);
            }
            //call
            if (Data[0].CALLSMSCNT == "" || Data[0].CALLSMSCNT == null) {
                $('#callsmsValue').text("0");
            } else {
                $('#callsmsValue').text(Data[0].CALLSMSCNT);
            }
            //visitValue
            if (Data[0].VISITCNT == "" || Data[0].VISITCNT == null) {
                $('#visitValue').text("0");
            } else {
                $('#visitValue').text(Data[0].VISITCNT);
            }
            //socialValue
            if (Data[0].SOCIALCNT == "" || Data[0].SOCIALCNT == null) {
                $('#socialValue').text("0");
            } else {
                $('#socialValue').text(Data[0].SOCIALCNT);
            }
            //otherValue
            if (Data[0].OTHERSCNT == "" || Data[0].OTHERSCNT == null) {
                $('#otherValue').text("0");
            } else {
                $('#otherValue').text(Data[0].OTHERSCNT);
            }
        }
    });
    $.ajax({
        type: "POST",
        url: "kpisummary.aspx/GetTransacVolume",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            console.log('GetTransacVolume', data);
            var arr = [data.d]
            //table.destroy();
            $('#TransacVolumeTAble').DataTable().destroy();
            $('#TransacVolumeTAble').DataTable({
                data: data.d,
                bSort : false,
                columns: [
                   { 'data': 'DOCTYPE' },
                   { 'data': 'TODAYCNT', className: "text-center" },
                   { 'data': 'TOTAL', className: "text-right" }
                ]
            });
        }
    });
    $.ajax({
        type: "POST",
        url: "kpisummary.aspx/GetTaskVolume",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            console.log('GetTaskVolume', data);
            var arr = [data.d]
            $('#TaskVolumeTAble').DataTable().destroy();
            $('#TaskVolumeTAble').DataTable({
                data: data.d,
                bSort: false,
                columns: [
                   { 'data': 'TOPIC' },
                   { 'data': 'POINT', className: "text-center" },
                   { 'data': 'RATING', className: "text-center" }
                ]
            });
        }
    });
}



function getEmpByBranch(branID) {
    var dt = {};
    dt.branchid = branID;
    $.ajax({
        type: "POST",
        url: "kpisummary.aspx/GetEmplyee",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            console.log('emp', data);
            var empList = data.d;
            var eTable = "<option value=''>All</option>"
            for (var i = 0; i < empList.length; i++) {
                eTable += "<option value='" + empList[i]['EMP_CODE'] + "'>" + empList[i]['EMP_NAME'] + "</option>";
            }
            eTable += "";
            $('#empSelect').html(eTable);
        }
    });
}
function createBranchList(data) {
    //console.log(data);
    var eTable = ""
    for (var i = 0; i < data.length; i++) {
        eTable += "<option value='" + data[i]['ID'] + "'>" + data[i]['NAME'] +"</option>";
    }
    eTable += "";
    $('#branchSelect').html(eTable);
}
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
    
});

function getChart(date, brnch, emp) {
    var asDate = changeDateFormat(date);
    var dt = {};
    dt.ASONDATE = asDate;
    dt.BRANCH_ID = brnch;
    dt.EMPID = emp;
    $.ajax({
        type: "POST",
        url: "kpisummary.aspx/GetPInquery",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            //console.log('inChart', data);
            var d = data.d[0];
            var result = Object.keys(d).map(function (key) {
                return {
                    "name": key,
                    "value": d[key]
                };
            });
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end

                // Create chart instance
                var chart = am4core.create("chartdivL1", am4charts.XYChart);

                // Add data
                chart.data = result;

                // Create axes

                var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "name";
                categoryAxis.renderer.grid.template.location = 0;
                categoryAxis.renderer.minGridDistance = 30;

                

                var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
                valueAxis.numberFormatter = new am4core.NumberFormatter();
                valueAxis.numberFormatter.numberFormat = "#,##,###.##";
                
                // Create series
                var series = chart.series.push(new am4charts.ColumnSeries());
                series.dataFields.valueY = "value";
                series.dataFields.categoryX = "name";
                series.name = "name";
                series.columns.template.tooltipText = "{categoryX}: [bold]{valueY}[/]";
                series.columns.template.fillOpacity = .8;

                var columnTemplate = series.columns.template;
                columnTemplate.strokeWidth = 2;
                columnTemplate.strokeOpacity = 1;

            }); // end am4core.ready()
        }
    });
    // quotation
    $.ajax({
        type: "POST",
        url: "kpisummary.aspx/GetPQuotation",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            //console.log('GetPQuotation', data);
            var d = data.d[0];
            var result = Object.keys(d).map(function (key) {
                return {
                    "name": key,
                    "value": d[key]
                };
            });
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end

                // Create chart instance
                var chart = am4core.create("chartdivL2", am4charts.XYChart);

                // Add data
                chart.data = result;

                // Create axes

                var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "name";
                categoryAxis.renderer.grid.template.location = 0;
                categoryAxis.renderer.minGridDistance = 30;

                //categoryAxis.renderer.labels.template.adapter.add("dy", function (dy, target) {
                //    if (target.dataItem && target.dataItem.index & 2 == 2) {
                //        return dy + 25;
                //    }
                //    return dy;
                //});

                var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

                // Create series
                var series = chart.series.push(new am4charts.ColumnSeries());
                series.dataFields.valueY = "value";
                series.dataFields.categoryX = "name";
                series.name = "name";
                series.columns.template.tooltipText = "{categoryX}: [bold]{valueY}[/]";
                series.columns.template.fillOpacity = .8;

                var columnTemplate = series.columns.template;
                columnTemplate.strokeWidth = 2;
                columnTemplate.strokeOpacity = 1;

            }); // end am4core.ready()
        }
    });
    //
    $.ajax({
        type: "POST",
        url: "kpisummary.aspx/GetPOrder",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            //console.log('GetPOrder', data);
            var d = data.d[0];
            var result = Object.keys(d).map(function (key) {
                return {
                    "name": key,
                    "value": d[key]
                };
            });
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end

                // Create chart instance
                var chart = am4core.create("chartdivL3", am4charts.XYChart);

                // Add data
                chart.data = result;

                // Create axes

                var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "name";
                categoryAxis.renderer.grid.template.location = 0;
                categoryAxis.renderer.minGridDistance = 30;

                //categoryAxis.renderer.labels.template.adapter.add("dy", function (dy, target) {
                //    if (target.dataItem && target.dataItem.index & 2 == 2) {
                //        return dy + 25;
                //    }
                //    return dy;
                //});

                var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

                // Create series
                var series = chart.series.push(new am4charts.ColumnSeries());
                series.dataFields.valueY = "value";
                series.dataFields.categoryX = "name";
                series.name = "name";
                series.columns.template.tooltipText = "{categoryX}: [bold]{valueY}[/]";
                series.columns.template.fillOpacity = .8;

                var columnTemplate = series.columns.template;
                columnTemplate.strokeWidth = 2;
                columnTemplate.strokeOpacity = 1;

            }); // end am4core.ready()
        }
    });

    $.ajax({
        type: "POST",
        url: "kpisummary.aspx/GetPSInv",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            //console.log('GetPSInv', data);
            var d = data.d[0];
            var result = Object.keys(d).map(function (key) {
                return {
                    "name": key,
                    "value": d[key]
                };
            });
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end

                // Create chart instance
                var chart = am4core.create("chartdivL4", am4charts.XYChart);

                // Add data
                chart.data = result;

                // Create axes

                var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "name";
                categoryAxis.renderer.grid.template.location = 0;
                categoryAxis.renderer.minGridDistance = 30;
                
                categoryAxis.renderer.labels.template.adapter.add("dy", function (dy, target) {
                    console.log('rnTempl', dy)
                    return dy;
                });

                var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

                // Create series
                var series = chart.series.push(new am4charts.ColumnSeries());
                series.dataFields.valueY = "value";
                series.dataFields.categoryX = "name";
                series.name = "name";
                series.columns.template.tooltipText = "{categoryX}: [bold]{valueY}[/]";
                series.columns.template.fillOpacity = .8;

                var columnTemplate = series.columns.template;
                columnTemplate.strokeWidth = 2;
                columnTemplate.strokeOpacity = 1;

            }); // end am4core.ready()
        }
    });
    $.ajax({
        type: "POST",
        url: "kpisummary.aspx/GetPLead",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
            if (data.d == "") {
                $('#chartdivDonut').html('No Data Found').css({ 'display': 'flex', 'align-items': 'center', 'justify-content': 'center' });
                $('.chartSec').css({ 'visibility': 'visible' });
            } else {
                am4core.ready(function () {
                    am4core.useTheme(am4themes_animated);
                    var chart = am4core.create("chartdivDonut", am4charts.PieChart);
                    chart.data = data.d;

                    // Add label
                    chart.innerRadius = 20;
                    var label = chart.seriesContainer.createChild(am4core.Label);
                    //label.text = "15K";
                    label.horizontalCenter = "middle";
                    label.verticalCenter = "middle";
                    label.fontSize = 50;

                    // Add and configure Series
                    var pieSeries = chart.series.push(new am4charts.PieSeries());
                    pieSeries.dataFields.value = "LDCNT";
                    pieSeries.dataFields.category = "LDSTATUS";
                    pieSeries.ticks.template.disabled = true;
                    pieSeries.labels.template.disabled = true;
                }); // end am4core.ready()
                $('.chartSec').css({ 'visibility': 'visible' });
            }
        }
    });
}
function GetperformanceTop(date, brnch, emp) {
    var asDate = changeDateFormat(date);

    var dt = {};
    dt.ASONDATE = asDate;
    dt.BRANCH_ID = brnch;
    dt.EMPID = emp;
    $.ajax({
        type: "POST",
        url: "kpisummary.aspx/GetPerformanceTopBox",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dt),
        dataType: "json",
        success: function (data) {
        
            var BoxData = data.d;
            //leads
            if (BoxData.one[0].LDTOTAMT == "" || BoxData.one[0].LDTOTAMT == null) {
                $('#ldAmt, #ldAmtF').text("0");
            } else {
                var a = numFormatter(BoxData.one[0].LDTOTAMT);
                $('#ldAmt').text(a);
                $('#ldAmtF').text(numberWithCommas(BoxData.one[0].LDTOTAMT));
            }
            $('#ldcnt').text(BoxData.one[0].LDCNT);
            //inquiry
            if (BoxData.two[0].INQTOTAMT == "" || BoxData.two[0].INQTOTAMT == null) {
                $('#InqAmt, #InqAmtF').text("0");
            } else {
                var b = numFormatter(BoxData.two[0].INQTOTAMT);
                $('#InqAmt').text(b);
                $('#InqAmtF').text(numberWithCommas(BoxData.two[0].INQTOTAMT));
            }
            $('#Inqcnt').text(BoxData.two[0].INQCNT);
            //QUOTATION
            if (BoxData.three[0].QOTOTAMT == "" || BoxData.three[0].QOTOTAMT == null) {
                $('#qAmt, #qAmtF').text("0");
            } else {
                var c = numFormatter(BoxData.three[0].QOTOTAMT);
                $('#qAmt').text(c);
                $('#qAmtF').text(numberWithCommas(BoxData.three[0].QOTOTAMT));
            }
            $('#qcnt').text(BoxData.three[0].QOCNT);
            // ORDER
            if (BoxData.four[0].SOTOTAMT == "" || BoxData.four[0].SOTOTAMT == null) {
                $('#oAmt, #oAmtF').text("0");
            } else {
                var d = numFormatter(BoxData.four[0].SOTOTAMT);
                $('#oAmt').text(d);
                $('#oAmtF').text(numberWithCommas(BoxData.four[0].SOTOTAMT));
            }
            $('#ocnt').text(BoxData.four[0].SOCNT);
            //SALES
            if (BoxData.five[0].SITOTAMT == "" || BoxData.five[0].SITOTAMT == null) {
                $('#sAmt, #sAmtF').text("0");
            } else {
                var e = numFormatter(BoxData.five[0].SITOTAMT);
                $('#sAmt').text(e);
                $('#sAmtF').text(numberWithCommas(BoxData.five[0].SITOTAMT));
            }
            $('#scnt').text(BoxData.five[0].SICNT);
            //COLLECTION
            if (BoxData.six[0].CRPTOTAMT == "" || BoxData.six[0].CRPTOTAMT == null) {
                $('#cAmt, #cAmtF').text("0");
            } else {
                var f = numFormatter(BoxData.six[0].CRPTOTAMT);
                $('#cAmt').text(f);
                $('#cAmtF').text(numberWithCommas(BoxData.six[0].CRPTOTAMT));
            }
            $('#ccnt').text(BoxData.six[0].CRPCNT);
        }
    });
}
//for date format
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
function numFormatter(num) {
    if (num > 999.99 && num < 100000) {
        return (num / 1000).toFixed(0) + 'K'; // convert to K for number from > 1000 < 1 million 
    } else if (num < 0 && num > -100000) {
        return (num / 1000).toFixed(0) + 'K'; // convert to K for number from > 1000 < 1 million 
    } else if (num < -99999.99 && num > -10000000) {
        return (num / 100000).toFixed(0) + 'L'; // convert to K for number from > 1000 < 1 million 
    } else if (num > 99999.99 && num < 10000000) {
        return (num / 100000).toFixed(0) + 'L'; // convert to M for number from > 1 million 
    } else if (num > 9999999.99) {
        return (num / 10000000).toFixed(0) + 'C'; // convert to M for number from > 1 million 
    } else if (num < -9999999.99) {
        return (num / 10000000).toFixed(0) + 'C'; // convert to M for number from > 1 million 
    } else if (num < 900) {
        return (num * 1).toFixed(0); // if value < 1000, nothing to do
    }
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