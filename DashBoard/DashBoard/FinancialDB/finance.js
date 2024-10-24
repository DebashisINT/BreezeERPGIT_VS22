﻿
        $(function () {
            var swiper = new Swiper('.SLIDEsCROLL', {
                slidesPerView: 5,
                spaceBetween: 10,
                pagination: {
                    el: '.swiper-pagination',
                    clickable: true,
                },
                navigation: {
                    nextEl: '.snavNext',
                    prevEl: '.snavPrev',
                },
                autoplay: false
            });
            $('.fullmake').click(function(){
                $(this).closest('.cardContainer').toggleClass('fullScreen');
                $('#chartdiv, #chartdiv2, #funnelCahrt').toggleClass('resizeChart');
            })
            getBoxData();
        });

function fnum(x) {
    if(isNaN(x)) return x;

    if(x < 9999) {
        return x;
    }
    //if(x < 1000000) {
    //    return Math.round(x/1000) + "K";
    //}
    if( x < 10000000) {
        return (x/1000000).toFixed(2) + "M";
    }

    if(x < 1000000000) {
        return Math.round((x/1000000)) + "M";
    }

    if(x < 1000000000000) {
        return Math.round((x/1000000)) + "M";
    }

    return "1T+";
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
function GetonRefresh(){
    $('.calecaleCnt .dbLd').removeClass('hide');
    $.ajax({
        type: "POST",
        url: "FinancialDB.aspx/GetonRefresh",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: false,
        success: function (data) {
            console.log('calecaleCnt', data) 
            $('.calecaleCnt .dbLd').addClass('hide');
            $('.calecaleCnt .cmp').removeClass('hide');
        }
 
    });
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

        
function getBoxData() {
    $.ajax({
        type: "POST",
        url: "FinancialDB.aspx/GetTopBoxData",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        // async: false,
        success: function (data) {
            console.log('financTop', data);
            var grsPer = data.d[0].Gross_Profit_percent;
            var netprPer = data.d[0].Nett_Profit_percent;
            var grsPr = data.d[0].Gross_Profit;
            $('#grsPrPer').text((Math.round(grsPer * 100) / 100).toFixed(2));
            $('#netPrPer').text((Math.round(netprPer * 100) / 100).toFixed(2));
            $('#tlCogs').text(fnum(grsPr));
            $('#expens').text(fnum(data.d[0].Expenses));
            $('#grsProfit').text(fnum(data.d[0].Gross_Profit));
            $('#otherIncomes').text(fnum(data.d[0].Other_income));
            $('#otherExpenses').text(fnum(data.d[0].Other_Expenses));
            $('#netProfit').text(fnum(data.d[0].Nett_Profit));
            $('#revenue').text(fnum(data.d[0].Reveneu));

            //Title
            $('#revenue').attr('title', numberWithCommas(data.d[0].Reveneu));
            $('#tlCogs').attr('title', numberWithCommas(grsPr));
            $('#expens').attr('title', numberWithCommas(data.d[0].Expenses));
            $('#grsProfit').attr('title', numberWithCommas(data.d[0].Gross_Profit));
            $('#otherIncomes').attr('title', numberWithCommas(data.d[0].Other_income));
            $('#otherExpenses').attr('title', numberWithCommas(data.d[0].Other_Expenses));
            $('#netProfit').attr('title', numberWithCommas(data.d[0].Nett_Profit));

            var temp1 = data.d[0];
            var arrS = []
            Object.keys(temp1).map(key =>{
                var mk = key;
            if(mk =="Gross_Profit_percent"){ 
								
            }else if(mk =="Other_income"){
							   
            }else if(mk =="Other_Expenses"){
							   
            }else if(mk =="Nett_Profit_percent"){
							   
            }else{
                arrS.push({ name:key, value: temp1[key] })
            }
        });
            
    var newArr = arrS.sort((a, b) => Number(b.value) - Number(a.value));
    funnelgenerate(newArr);
    GetChartData();
}
});
}
Array.prototype.sortBy = function(p) {
    return this.slice(0).sort(function(a,b) {
        return (a[p] > b[p]) ? 1 : (a[p] < b[p]) ? -1 : 0;
    });
}
function GetChartData(){
    $.ajax({
        type: "POST",
        url: "FinancialDB.aspx/GetchartsData",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: false,
        success: function (data) {
            console.log(data)
            var arr = data.d;
                    
            am4core.ready(function () {
                        
                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end

                // Create chart instance
                var chart2 = am4core.create("chartdiv", am4charts.XYChart);
                chart2.scrollbarX = new am4core.Scrollbar();
                //chart2.colors.step = 5;
                chart2.maskBullets = false;

                // Add data
                chart2.data = arr;

                // Create axes
                var date2Axis = chart2.xAxes.push(new am4charts.DateAxis());
                date2Axis.renderer.grid.template.location = 0;
                date2Axis.renderer.minGridDistance = 50;
                date2Axis.renderer.grid.template.disabled = true;
                date2Axis.renderer.fullWidthTooltip = false;
                date2Axis.renderer.cellStartLocation = 0.1;
                date2Axis.renderer.cellEndLocation = 0.8;


                var distance2Axis = chart2.yAxes.push(new am4charts.ValueAxis());
                distance2Axis.title.text = "Revenue";
                distance2Axis.renderer.grid.template.disabled = true;
                distance2Axis.renderer.minGridDistance = 30;

                var duration2Axis = chart2.yAxes.push(new am4charts.ValueAxis());
                duration2Axis.title.text = " ";
                ////duration2Axis.baseUnit = "minute";
                //duration2Axis.renderer.grid.template.disabled = true;
                duration2Axis.renderer.opposite = true;


                var latitude2Axis = chart2.yAxes.push(new am4charts.ValueAxis());
                latitude2Axis.renderer.grid.template.disabled = true;
                latitude2Axis.renderer.labels.template.disabled = true;

                // Create series
                var distance2Series = chart2.series.push(new am4charts.ColumnSeries());
                distance2Series.dataFields.valueY = "REVENEU";
                distance2Series.dataFields.dateX = "MONTHYEAR";
                distance2Series.yAxis = distance2Axis;
                distance2Series.tooltipText = "Revenue : {valueY} ";
                distance2Series.name = "Revenue";
                distance2Series.columns.template.fillOpacity = 1;
                distance2Series.columns.template.propertyFields.strokeDasharray = "dashLength";
                distance2Series.columns.template.fill = am4core.color("#59B6AA");
                distance2Series.tooltip.background.fill = false;
                //distance2Series.columns.template.propertyFields.fillOpacity = "alpha";
                distance2Series.cursorTooltipEnabled = false;

                var disatnce2State = distance2Series.columns.template.states.create("hover");
                disatnce2State.properties.fillOpacity = 0.9;

                var duration2Series = chart2.series.push(new am4charts.ColumnSeries());
                duration2Series.dataFields.valueY = "COGS";
                duration2Series.dataFields.dateX = "MONTHYEAR";
                duration2Series.yAxis = distance2Axis;
                duration2Series.name = "COGS";
                duration2Series.strokeWidth = 0;
                duration2Series.propertyFields.strokeDasharray = "dashLength";
                duration2Series.tooltipText = "COGS : {valueY}";
                duration2Series.columns.template.fill = am4core.color("#e03434");
                duration2Series.tooltip.background.fill = false;
                duration2Series.cursorTooltipEnabled = false;

                var duration2Bullet = duration2Series.bullets.push(new am4charts.Bullet());
                var durationRectangle = duration2Bullet.createChild(am4core.Rectangle);
                duration2Bullet.horizontalCenter = "middle";
                duration2Bullet.verticalCenter = "middle";
                duration2Bullet.width = 1;
                duration2Bullet.height = 1;
                durationRectangle.width = 1;
                durationRectangle.height = 1;

                var durationState = duration2Bullet.states.create("hover");
                durationState.properties.scale = 1.2;

                var latitude2Series = chart2.series.push(new am4charts.LineSeries());
                latitude2Series.dataFields.valueY = "GROSS_PROFIT";
                latitude2Series.dataFields.dateX = "MONTHYEAR";
                latitude2Series.yAxis = distance2Axis;
                latitude2Series.name = "GROSS_PROFIT";
                latitude2Series.strokeWidth = 2;
                latitude2Series.propertyFields.strokeDasharray = "dashLength";
                latitude2Series.tooltipText = "GROSS PROFIT: {valueY}";
                latitude2Series.stroke = am4core.color("#333333");
                latitude2Series.strokeDasharray = "8,4";
                var latitude2Bullet = latitude2Series.bullets.push(new am4charts.CircleBullet());
                latitude2Bullet.circle.fill = am4core.color("#fff");
                latitude2Bullet.circle.strokeWidth = 2;
                latitude2Bullet.circle.propertyFields.radius = "0";
                latitude2Series.tooltip.background.fill = false;
                latitude2Series.cursorTooltipEnabled = false;

                var latitudeState = latitude2Bullet.states.create("hover");
                latitudeState.properties.scale = 1.2;

                // var latitude2Label = latitude2Series.bullets.push(new am4charts.LabelBullet());
                // latitude2Label.label.text = "{townName2}";
                // latitude2Label.label.horizontalCenter = "left";
                // latitude2Label.label.dx = 14;
                       
                // Add legend
                chart2.legend = new am4charts.Legend();

                // Add cursor
                chart2.cursor = new am4charts.XYCursor();
                chart2.cursor.fullWidthLineX = true;
                //chart2.cursor.xAxis = dateAxis;
                chart2.cursor.lineX.strokeOpacity = 0;
                chart2.cursor.lineX.fill = am4core.color("#000");
                chart2.cursor.lineX.fillOpacity = 0.1;

            }); // end am4core.ready()


            //second chart
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end

                // Create chart instance
                var chart2 = am4core.create("chartdiv2", am4charts.XYChart);
                chart2.scrollbarX = new am4core.Scrollbar();
                        
                chart2.maskBullets = false;

                // Add data
                chart2.data = arr;

                // Create axes
                var date2Axis = chart2.xAxes.push(new am4charts.DateAxis());
                date2Axis.renderer.grid.template.location = 0;
                date2Axis.renderer.minGridDistance = 50;
                date2Axis.renderer.grid.template.disabled = true;
                date2Axis.renderer.fullWidthTooltip = false;
                date2Axis.renderer.cellStartLocation = 0.1;
                date2Axis.renderer.cellEndLocation = 0.8;

                var distance2Axis = chart2.yAxes.push(new am4charts.ValueAxis());
                distance2Axis.title.text = "Profit";
                distance2Axis.renderer.grid.template.disabled = true;

                var duration2Axis = chart2.yAxes.push(new am4charts.ValueAxis());
                duration2Axis.title.text = "Net Profit Percentage";
                //duration2Axis.baseUnit = "minute";
                duration2Axis.renderer.grid.template.disabled = true;
                duration2Axis.renderer.opposite = true;

                        

                var latitude2Axis = chart2.yAxes.push(new am4charts.ValueAxis());
                latitude2Axis.renderer.grid.template.disabled = true;
                latitude2Axis.renderer.labels.template.disabled = true;

                        

                var duration2Series = chart2.series.push(new am4charts.ColumnSeries());
                duration2Series.dataFields.valueY = "NETT_PROFIT";
                duration2Series.dataFields.dateX = "MONTHYEAR";
                duration2Series.yAxis = distance2Axis;
                duration2Series.name = "Net Profit";
                duration2Series.strokeWidth = 0;
                duration2Series.propertyFields.strokeDasharray = "dashLength";
                duration2Series.tooltipText = "NET PROFIT :{valueY}";
                duration2Series.cursorTooltipEnabled = false;

                duration2Series.columns.template.adapter.add("fill", function(fill, target) {
                    if (target.dataItem && (target.dataItem.valueY < 0)) {
                        return am4core.color("#E03434");
                    }
                    else {
                        return am4core.color("#69bdb2");
                    }
                });

                var duration2Bullet = duration2Series.bullets.push(new am4charts.Bullet());
                var durationRectangle = duration2Bullet.createChild(am4core.Rectangle);
                duration2Bullet.horizontalCenter = "middle";
                duration2Bullet.verticalCenter = "middle";
                duration2Bullet.width = 1;
                duration2Bullet.height = 1;
                durationRectangle.width = 1;
                durationRectangle.height = 1;

                var durationState = duration2Bullet.states.create("hover");
                durationState.properties.scale = 1.2;

                var latitude2Series = chart2.series.push(new am4charts.LineSeries());
                latitude2Series.dataFields.valueY = "NETT_PROFIT_PERCENTAGE";
                latitude2Series.dataFields.dateX = "MONTHYEAR";
                latitude2Series.yAxis = duration2Axis;
                latitude2Series.name = "Net Profit";
                latitude2Series.strokeWidth = 2;
                latitude2Series.propertyFields.strokeDasharray = "dashLength";
                latitude2Series.tooltipText = "NET PROFIT PERCENTAGE: {valueY}";
                latitude2Series.stroke = am4core.color("#333333");
                latitude2Series.strokeDasharray = "8,4";
                var latitude2Bullet = latitude2Series.bullets.push(new am4charts.CircleBullet());
                latitude2Bullet.circle.fill = am4core.color("#fff");
                latitude2Bullet.circle.strokeWidth = 2;
                latitude2Bullet.circle.propertyFields.radius = "townSize";
                latitude2Series.cursorTooltipEnabled = false;

                var latitudeState = latitude2Bullet.states.create("hover");
                latitudeState.properties.scale = 1.2;

                // var latitude2Label = latitude2Series.bullets.push(new am4charts.LabelBullet());
                // latitude2Label.label.text = "{townName2}";
                // latitude2Label.label.horizontalCenter = "left";
                // latitude2Label.label.dx = 14;

                // Add legend
                chart2.legend = new am4charts.Legend();

                // Add cursor
                chart2.cursor = new am4charts.XYCursor();
                chart2.cursor.fullWidthLineX = true;
                //chart2.cursor.xAxis = dateAxis;
                chart2.cursor.lineX.strokeOpacity = 0;
                chart2.cursor.lineX.fill = am4core.color("#000");
                chart2.cursor.lineX.fillOpacity = 0.1;

            }); // end am4core.ready()

            getAssestsData ()
   
        }
                
    })
}
        
function getAssestsData () {
    //GetAssetsData
    $.ajax({
        type: "POST",
        url: "FinancialDB.aspx/GetAssetsData",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async: false,
        success: function (data) {
            console.log('assetsDta', data)
            var res = data.d[0]
            $('#Total_Asset').text(numberWithCommas(res.Total_Asset));
            $('#TOTAL_REC').text(numberWithCommas(res.TOTAL_REC));
            $('#TOTAL_PAY').text(numberWithCommas(res.TOTAL_PAY));
            $('#CUR_LIABILITY').text(numberWithCommas(res.CUR_LIABILITY));
            $('#Cashbank_Amount').text(numberWithCommas(res.Cashbank_Amount));
            $('#Closing_Stock').text(numberWithCommas(res.Closing_Stock));
            $('#Current_Asset').text(numberWithCommas(res.Current_Asset));
            $('#Deposit').text(numberWithCommas(res.Deposit));
            $('#EQUITY').text(numberWithCommas(res.EQUITY));
            $('#Fixed_Asset').text(numberWithCommas(res.Fixed_Asset));
            $('#LIABILITY').text(numberWithCommas(res.LIABILITY));
            $('#NON_CUR_LIABILITY').text(numberWithCommas(res.NON_CUR_LIABILITY));
            $('#PROVISION').text(numberWithCommas(res.PROVISION));
            $('#RELATED_PARTY').text(numberWithCommas(res.RELATED_PARTY));

                    
        }
    });
}
