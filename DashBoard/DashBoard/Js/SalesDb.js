
TotalSaleinInr = "0.00";

function RefreshAll() {
    HeaderReferesh();
    LoadTopSaleman();
    LoadCustomer();
}


function HeaderReferesh() {

    var OtherDetails = {}
    OtherDetails.FromDtae = moment(cFormDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.toDate = moment(ctoDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.Prodid = $('#hdnProductId').val();
    $.ajax({
        type: "POST",
        url: "SalesDb.aspx/GetSalesBalance",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var retObj = msg.d;

            $("#totSale").text(retObj.TotalSale);
            $("#totDue").text(retObj.TotDue);
            $("#totAdvance").text(retObj.totAdvance);
            $("#totOrder").text(retObj.totOrder);
        }
    });
}


function LoadTopSaleman() {

    var OtherDetails = {}
    OtherDetails.FromDtae = moment(cFormDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.toDate = moment(ctoDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.branchid = cddlBranch.GetValue();
    OtherDetails.ProdClass = $('#hdnClassId').val();
    OtherDetails.Prodid = $('#hdnProductId').val();
    $.ajax({
        type: "POST",
        url: "SalesDb.aspx/GetTopNSalesMan",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var retObj = msg.d;

            console.log(retObj);

            var PieObject = {
                "type": "pie",
                "theme": "light",
                "dataProvider": [],
                "valueField": "AmtVal",
                "titleField": "Name",
                "outlineAlpha": 0.8,
                "depth3D": 40,
                "balloonText": "[[title]]<br><span style='font-size:14px'><b>[[value]]</b> ([[percents]]%)</span>",
                "angle": 40,
                "export": {
                    "enabled": true
                }
            };
            PieObject.dataProvider = msg.d;
            if (msg.d.length > 0) {
                
                var chart = AmCharts.makeChart("SalesManPanel", PieObject);

            } else {
                $('#SalesManPanel').html('<div class="chartmsg" id="spanIdTotSale">  No Data within the selected Date!</div>');
                
            }
        }
    });
}


function LoadCustomer() {
    var OtherDetails = {}
    OtherDetails.FromDtae = moment(cFormDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.toDate = moment(ctoDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.branchid = cddlBranch.GetValue();
    OtherDetails.ProdClass = $('#hdnClassId').val();
    OtherDetails.Prodid = $('#hdnProductId').val();
    $.ajax({
        type: "POST",
        url: "SalesDb.aspx/GetTopNCustomer",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) { 

            var chartobj = {
                "theme": "light",
                "type": "serial",
                "dataProvider": [],
                "valueAxes": [{
                    "title": "Amount in Rs"
                }],
                "graphs": [{
                    "balloonText": "Amount in [[category]]:[[value]]",
                    "fillAlphas": 1,
                    "lineAlpha": 0.2,
                    "title": "Amount",
                    "type": "column",
                    "labelText": "[[value]]",
                    "valueField": "AmtVal"
                }],
                "depth3D": 20,
                "angle": 30,
                "rotate": true,
                "categoryField": "Name",
                "categoryAxis": {
                    "gridPosition": "start",
                    "fillAlpha": 0.05,
                    "position": "left"
                },
                "export": {
                    "enabled": true
                }
            };

            chartobj.dataProvider = msg.d;
            if (msg.d.length > 0) {
                var chart = AmCharts.makeChart("topNCustomer", chartobj);
            } else {
                $('#topNCustomer').html('<div class="chartmsg" id="spanidforcustomer">  No Data within the selected Date!</div>');
                
                
            }
        }
    });
}


function changeViewTopProd() {
    if (document.getElementById('SalesManPanel').style.height != "100%") {
        $('#topzSalesman').addClass("fullscreen");
        document.getElementById('SalesManPanel').style.height = '100%';
    } else {
        $('#topzSalesman').removeClass("fullscreen");
        document.getElementById('SalesManPanel').style.height = '350px';
    }
}




function changeViewcustZoom() {
    if (document.getElementById('topNCustomer').style.height != "100%") {
        $('#topzCustomer').addClass("fullscreen");
        document.getElementById('topNCustomer').style.height = '100%';
    } else {
        $('#topzCustomer').removeClass("fullscreen");
        document.getElementById('topNCustomer').style.height = '350px';
    }
}





//New Code Start Here
function RefreshWidget() {
    if (document.getElementsByClassName('DisableClass').length > 0) {

        if (document.getElementsByClassName('DisableClass')[0].id == 'top10Smanbtn') {
            LoadTopSaleman();
        }
        else if (document.getElementsByClassName('DisableClass')[0].id == 'top10Custbtn') {
            LoadCustomer();
        }

        else if (document.getElementsByClassName('DisableClass')[0].id == 'TotSalebtn') {
            LoadTotalSale();
        }
        else if (document.getElementsByClassName('DisableClass')[0].id == 'TotOrdbtn') {
            LoadOrder();
        }
        else if (document.getElementsByClassName('DisableClass')[0].id == 'TotReceiptbtn') {
            LoadTotReceipt();
        }
        else if (document.getElementsByClassName('DisableClass')[0].id == 'TotDuebtn') {
            LoadTotDue();
        }
        else if (document.getElementsByClassName('DisableClass')[0].id == 'newCustbtn') {
            LoadnewCust();
        }

    }

}

function changeViewTopSman() {
    if (document.getElementById('SalesManPanel').style.height != "100%") {
        $('#top10Sman').addClass("fullscreen");
        $('#top10Sman .card').addClass('full');
        document.getElementById('SalesManPanel').style.height = '100%';
    } else {
        $('#top10Sman').removeClass("fullscreen");
        $('#top10Sman .card').removeClass('full');
        document.getElementById('SalesManPanel').style.height = '350px';
    }
}


function ChangeviewTotReceipt() {
    if (document.getElementById('TotReceiptPanel').style.height != "100%") {
        $('#TotReceipt').addClass("fullscreen");
        $('#TotReceipt .card').addClass('full');
        document.getElementById('TotReceiptPanel').style.height = '100%';
    } else {
        $('#TotReceipt').removeClass("fullscreen");
        $('#TotReceipt .card').removeClass('full');
        document.getElementById('TotReceiptPanel').style.height = '350px';
    }
}

function changeViewNewcustZoom() {
    if (document.getElementById('topNCustomer').style.height != "90%") {
        $('#top10Cust').addClass("fullscreen");
        $('#top10Cust .card').addClass('full');
        document.getElementById('topNCustomer').style.height = '90%';
    } else {
        $('#top10Cust').removeClass("fullscreen");
        $('#top10Cust .card').removeClass('full');
        document.getElementById('topNCustomer').style.height = '350px';
    }
}

 

function ChangeviewtotSale() {
    if (document.getElementById('TotSalePanel').style.height != "90%") {
        $('#TotSale').addClass("fullscreen");
        $('#TotSale .card').addClass('full');
        document.getElementById('TotSalePanel').style.height = '90%';
    } else {
        $('#TotSale').removeClass("fullscreen");
        $('#TotSale .card').removeClass('full');
        document.getElementById('TotSalePanel').style.height = '350px';
    }
}

function ChangeviewtotOrder() {
    if (document.getElementById('TotOrdPanel').style.height != "90%") {
        $('#TotOrd').addClass("fullscreen");
        $('#TotOrd .card').addClass('full');
        document.getElementById('TotOrdPanel').style.height = '90%';
    } else {
        $('#TotOrd').removeClass("fullscreen");
        $('#TotOrd .card').removeClass('full');
        document.getElementById('TotOrdPanel').style.height = '350px';
    }
}


function ChangeviewTotDue() {
    if (document.getElementById('TotDuePanel').style.height != "90%") {
        $('#TotDue').addClass("fullscreen");
        $('#TotDue .card').addClass('full');
        document.getElementById('TotDuePanel').style.height = '90%';
    } else {
        $('#TotDue').removeClass("fullscreen");
        $('#TotDue .card').removeClass('full');
        document.getElementById('TotDuePanel').style.height = '350px';
    }
}


function ChangeviewTotnewCust() {
    if (document.getElementById('newCustPanel').style.height != "90%") {
        $('#newCust').addClass("fullscreen");
        $('#newCust .card').addClass('full');
        document.getElementById('newCustPanel').style.height = '90%';
    } else {
        $('#newCust').removeClass("fullscreen");
        $('#newCust .card').removeClass('full');
        document.getElementById('newCustPanel').style.height = '350px';
    }
}


function LoadTotalSale() {
    var OtherDetails = {}
    OtherDetails.FromDtae = moment(cFormDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.toDate = moment(ctoDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.branchid = cddlBranch.GetValue();
    OtherDetails.ProdClass = $('#hdnClassId').val();
    OtherDetails.Prodid = $('#hdnProductId').val();
    $.ajax({
        type: "POST",
        url: "SalesDashboard.aspx/LoadTotalSale",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            var PieObject = {
                "theme": "light",
                "type": "serial",
                "numberFormatter":{precision:-1, decimalSeparator:'.', thousandsSeparator:','},
                "startDuration": 2,
                "dataProvider": [],
                "valueAxes": [{
                    "position": "left",
                    "title": "Sale Value"
                }], 
                "graphs": [{
                    "balloonText": "<span style='display:none'>[[category]]: <b>[[value]]</b></span>",
                    //"fillColorsField": "color",
                    "fillAlphas": 1,
                    "lineAlpha": 0.1,  
                    "type": "column", 
                    "valueField": "xAxis"
                }, {
                    "id": "totSaleline",
                    "balloonText": "<span style='font-size:12px;'>[[title]] in [[category]]:<br><span style='font-size:20px;'>[[value]]</span> [[additional]]</span>",
                    "bullet": "round",
                    "lineThickness": 3,
                    "bulletSize": 7,
                    "bulletBorderAlpha": 1,
                    "bulletColor": "#FFFFFF",
                    "useLineColorForBulletBorder": true,
                    "bulletBorderThickness": 3,
                    "fillAlphas": 0,
                    "lineAlpha": 1,
                    "title": "Sale",
                    "valueField": "xAxis",

                    "labelText": "[[value]]",
                    "labelRotation": 270,
                    "labelPosition": "top",

                    "dashLengthField": "dashLengthLine"
                }],
                "depth3D": 20,
                "angle": 30,
                "chartCursor": {
                    "categoryBalloonEnabled": false,
                    "cursorAlpha": 0,
                    "zoomable": false
                },
                "categoryField": "yAxis",
                "categoryAxis": {
                    "gridPosition": "start",
                    "labelRotation": 45,
                    "title": "Date"
                },
                "export": {
                    "enabled": true
                }

            };
            TotalSaleinInr = msg.d.totValue;
            totalSaleCheckChange();
            //$('#TotsaleinInr').text(msg.d.totValue);
            PieObject.dataProvider = msg.d.AxisValue;
            if (msg.d.AxisValue.length > 0) {
                $('#TotSalePanel').removeClass('chartboxes');
                chart1 = AmCharts.makeChart("TotSalePanel", PieObject);
            } else {
                $('#TotSalePanel').html('<div class="chartmsg" id="spanIdTotSale">  No Data within the selected Date!</div>');
                
                
            }
        }
    });
}



function LoadOrder() {
    var OtherDetails = {}
    OtherDetails.FromDtae = moment(cFormDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.toDate = moment(ctoDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.branchid = cddlBranch.GetValue();
    OtherDetails.ProdClass = $('#hdnClassId').val();
    OtherDetails.Prodid = $('#hdnProductId').val();
    $.ajax({
        type: "POST",
        url: "SalesDashboard.aspx/LoadTotalOrder",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

             


            var PieObject = {
                "theme": "light",
                "type": "serial",
                "startDuration": 2,
                "dataProvider": [],
                "valueAxes": [{
                    "position": "left",
                    "title": "Order/Invoice Value"
                }],
                "graphs": [{
                    "balloonText": "<span style='font-size:12px;'>[[title]] in [[category]]:<br><span style='font-size:20px;'>[[value]]</span> [[additional]]</span>",
                    "bullet": "round",
                    "lineThickness": 3,
                    "bulletSize": 7,
                    "bulletBorderAlpha": 1,
                    "bulletColor": "#FFFFFF",
                    "useLineColorForBulletBorder": true,
                    "bulletBorderThickness": 3,
                    "fillAlphas": 0,
                    "lineAlpha": 1,
                    "title": "Order value",
                    "valueField": "OrderAmt",
                     
                    "dashLengthField": "dashLengthLine"
                }, {
                    "balloonText": "<span style='font-size:12px;'>[[title]] in [[category]]:<br><span style='font-size:20px;'>[[value]]</span> [[additional]]</span>",
                    "bullet": "round",
                    "lineThickness": 3,
                    "bulletSize": 7,
                    "bulletBorderAlpha": 1,
                    "bulletColor": "#FFFFFF",
                    "useLineColorForBulletBorder": true,
                    "bulletBorderThickness": 3,
                    "fillAlphas": 0,
                    "lineAlpha": 1,
                    "title": "Invoice value",
                    "valueField": "InvoiceAmt",

                  

                    "dashLengthField": "dashLengthLine"
                }], 
                "depth3D": 20,
                "angle": 30,
                "chartCursor": {
                    "categoryBalloonEnabled": false,
                    "cursorAlpha": 0,
                    "zoomable": false
                },
                "categoryField": "Date",
                "categoryAxis": {
                    "gridPosition": "start",
                    "labelRotation": 45,
                    "title": "Date"
                },
                "export": {
                    "enabled": true
                }

            };
             

            $('#TotOrdinInr').text(msg.d.totValue);
            PieObject.dataProvider = msg.d.AxisValue;
            if (msg.d.AxisValue.length > 0) {
                chart1 = AmCharts.makeChart("TotOrdPanel", PieObject);
                $('#ledgendbox').show();
            } else {
                $('#TotOrdPanel').html('<div class="chartmsg" id="spanidfororderValue">  No Data within the selected Date!</div>');
                
                
            }
        }
    });

}



function LoadTotReceipt() {
    var OtherDetails = {}
    OtherDetails.FromDtae = moment(cFormDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.toDate = moment(ctoDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.branchid = cddlBranch.GetValue();
    OtherDetails.ProdClass = $('#hdnClassId').val();
    OtherDetails.Prodid = $('#hdnProductId').val();
    $.ajax({
        type: "POST",
        url: "SalesDashboard.aspx/LoadTotalReceipt",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            var PieObject = {
                "theme": "light",
                "type": "serial",
                
                "startDuration": 2,
                "dataProvider": [],
                "valueAxes": [{
                    "position": "left",
                    "title": "Receipt Value"
                }],
                "graphs": [{
                    "balloonText": "<span style='display:none'>[[category]]: <b>[[value]]</b></span>",
                    //"fillColorsField": "color",
                   // "fillColors": "#29b320",
                    "fillAlphas": 1,
                    "lineAlpha": 0.1,
                    "type": "column",
                    "valueField": "xAxis"
                }, {
                    "id": "totSaleline",
                    "balloonText": "<span style='font-size:12px;'>[[title]] in [[category]]:<br><span style='font-size:20px;'>[[value]]</span> [[additional]]</span>",
                    "bullet": "round",
                    "lineThickness": 3,
                    "bulletSize": 7,
                    "bulletBorderAlpha": 1,
                    "bulletColor": "#FFFFFF",
                    "useLineColorForBulletBorder": true,
                    "bulletBorderThickness": 3,
                    "fillAlphas": 0,
                    "lineAlpha": 1,
                    "title": "Receipt",
                    "valueField": "xAxis",

                    "labelText": "[[value]]",
                    "labelRotation": 270,
                    "labelPosition": "top",

                    "dashLengthField": "dashLengthLine"
                }],
                "depth3D": 20,
                "angle": 30,
                "chartCursor": {
                    "categoryBalloonEnabled": false,
                    "cursorAlpha": 0,
                    "zoomable": false
                },
                "categoryField": "yAxis",
                "categoryAxis": {
                    "gridPosition": "start",
                    "labelRotation": 45,
                    "title": "Date"
                },
                "export": {
                    "enabled": true
                }

            };

            $('#TotReceiptinInr').text(msg.d.totValue);
            PieObject.dataProvider = msg.d.AxisValue;
            if (msg.d.AxisValue.length > 0) {
                var chart = AmCharts.makeChart("TotReceiptPanel", PieObject);
            } else {
                $('#TotReceiptPanel').html('<div class="chartmsg" id="spanidfortotReceipt">  No Data within the selected Date!</div>');
                
                
            }
        }
    });

}




function LoadTotDue() {
    var OtherDetails = {}
    OtherDetails.FromDtae = moment(cFormDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.toDate = moment(ctoDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.branchid = cddlBranch.GetValue();
    OtherDetails.ProdClass = $('#hdnClassId').val();
    OtherDetails.Prodid = $('#hdnProductId').val();
    $.ajax({
        type: "POST",
        url: "SalesDashboard.aspx/LoadTotalDue",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            var PieObject = {
                "theme": "light",
                "type": "serial",

                "startDuration": 2,
                "dataProvider": [],
                "valueAxes": [{
                    "position": "left",
                    "title": "Due Value"
                }],
                "graphs": [{
                    "balloonText": "<span style='display:none'>[[category]]: <b>[[value]]</b></span>",
                    //"fillColorsField": "color",
                   // "fillColors": "#9a1919",
                    "fillAlphas": 1,
                    "lineAlpha": 0.1,
                    "type": "column",
                    "valueField": "xAxis"
                }, {
                    "id": "totSaleline",
                    "balloonText": "<span style='font-size:12px;'>[[title]] in [[category]]:<br><span style='font-size:20px;'>[[value]]</span> [[additional]]</span>",
                    "bullet": "round",
                    "lineThickness": 3,
                    "bulletSize": 7,
                    "bulletBorderAlpha": 1,
                    "bulletColor": "#FFFFFF",
                    "useLineColorForBulletBorder": true,
                    "bulletBorderThickness": 3,
                    "fillAlphas": 0,
                    "lineAlpha": 1,
                    "title": "Due",
                    "valueField": "xAxis",

                    "labelText": "[[value]]",
                    "labelRotation": 270,
                    "labelPosition": "top", 
                    "dashLengthField": "dashLengthLine"
                }],
                "depth3D": 20,
                "angle": 30,
                "chartCursor": {
                    "categoryBalloonEnabled": false,
                    "cursorAlpha": 0,
                    "zoomable": false
                },
                "categoryField": "yAxis",
                "categoryAxis": {
                    "gridPosition": "start",
                    "labelRotation": 45,
                    "title": "Date"
                },
                "export": {
                    "enabled": true
                }

            };


            $('#TotDueinInr').text(msg.d.totValue);
            PieObject.dataProvider = msg.d.AxisValue;
            if (msg.d.AxisValue.length > 0) {
                var chart = AmCharts.makeChart("TotDuePanel", PieObject);
            } else {
                $('#TotDuePanel').html('<div class="chartmsg" id="spanidfortotaldue">  No Data within the selected Date!</div>');
                
                                
            }
        }
    });

}




function LoadnewCust() {
    var OtherDetails = {}
    OtherDetails.FromDtae = moment(cFormDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.toDate = moment(ctoDate.GetDate()).format('YYYY-MM-DD');
    OtherDetails.branchid = cddlBranch.GetValue();
    OtherDetails.ProdClass = $('#hdnClassId').val();
    OtherDetails.Prodid = $('#hdnProductId').val();
    $.ajax({
        type: "POST",
        url: "SalesDashboard.aspx/LoadnewCust",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            var PieObject = {
                "theme": "light",
                "type": "serial",

                "startDuration": 2,
                "dataProvider": [],
                "valueAxes": [{
                    "position": "left",
                    "title": "Customer Count"
                }],
                "graphs": [{
                    "balloonText": "<span>New Customer(s) on [[category]]: <b>[[value]]</b></span>", 
                    "fillAlphas": 1,
                    "lineAlpha": 0.1,
                    "type": "column",
                    "valueField": "xAxis"
                } ],
                "depth3D": 20,
                "angle": 30,
                "chartCursor": {
                    "categoryBalloonEnabled": false,
                    "cursorAlpha": 0,
                    "zoomable": false
                },
                "categoryField": "yAxis",
                "categoryAxis": {
                    "gridPosition": "start",
                    "labelRotation": 45,
                    "title": "Date"
                },
                "export": {
                    "enabled": true
                }

            };


            $('#TotalCustCount').text(msg.d.totValue);
            PieObject.dataProvider = msg.d.AxisValue;
            if (msg.d.AxisValue.length > 0) {
                var chart = AmCharts.makeChart("newCustPanel", PieObject);
            } else {
                $('#newCustPanel').html('<div class="chartmsg" id="spanidfornewCustomer">  No Data within the selected Date!</div>');
                
            }
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
        x=x.substr(0, x.indexOf('.'))
        var lastThree = x.substring(x.length - 3);
        var otherNumbers = x.substring(0, x.length - 3);
        if (otherNumbers != '')
            lastThree = ',' + lastThree;
        var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree+'.'+dec;
    }
    
    return res;
}


function totalSaleCheckChange() {
    if (document.getElementById('chkSaleShowDecimal').checked) {
        $('#TotsaleinInr').text(numberWithCommas(TotalSaleinInr));
    } else {
        $('#TotsaleinInr').text(numberWithCommas(Math.round(TotalSaleinInr)));
    }
}
 




// week work WTD
Date.prototype.getWeek = function () {
    var target = new Date(this.valueOf());
    var dayNr = (this.getDay() + 6) % 7;
    target.setDate(target.getDate() - dayNr + 3);
    var firstThursday = target.valueOf();
    target.setMonth(0, 1);
    if (target.getDay() != 4) {
        target.setMonth(0, 1 + ((4 - target.getDay()) + 7) % 7);
    }
    return 1 + Math.ceil((firstThursday - target) / 604800000);
}

function getDateRangeOfWeek(weekNo) {
    var d1 = new Date();
    numOfdaysPastSinceLastMonday = eval(d1.getDay() - 1);
    d1.setDate(d1.getDate() - numOfdaysPastSinceLastMonday);
    var weekNoToday = d1.getWeek();
    var weeksInTheFuture = eval(weekNo - weekNoToday);
    d1.setDate(d1.getDate() + eval(7 * weeksInTheFuture));
    var rangeIsFrom = eval(d1.getMonth() + 1) + "/" + d1.getDate() + "/" + d1.getFullYear();
    d1.setDate(d1.getDate() + 6);
    var rangeIsTo = eval(d1.getMonth() + 1) + "/" + d1.getDate() + "/" + d1.getFullYear();
    return rangeIsFrom + " to " + rangeIsTo;
};

Date.prototype.whatWeek = function () {
    var onejan = new Date(this.getFullYear(), 0, 1);
    var today = new Date(this.getFullYear(), this.getMonth(), this.getDate());
    var dayOfYear = ((today - onejan + 86400000) / 86400000);
    return Math.ceil(dayOfYear / 7)
};
// QTD
function quarter_of_the_year(date) {
    var month = date.getMonth() + 1;
    return (Math.ceil(month / 3));
}





$(document).ready(function () {
    $("#dynamiDate").change(function () {
        var value = $(this).val();

        if (value == "0") {
            cFormDate.SetEnabled(true);
            ctoDate.SetEnabled(true);
        } else if (value == "WTD") {
            var today = new Date();
            var currentWeekNumber = today.whatWeek();
            var wTD = getDateRangeOfWeek(currentWeekNumber);
            var slicd = wTD.split(" ");
            var fst = slicd[0];
            var lst = slicd[2];
            cFormDate.SetDate(new Date(fst));
            ctoDate.SetDate(new Date(lst));
            cFormDate.SetEnabled(false);
            ctoDate.SetEnabled(false);
        } else if (value == "MTD") {
            var date = new Date();
            var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            cFormDate.SetDate(new Date(firstDay));
            ctoDate.SetDate(new Date());
            cFormDate.SetEnabled(false);
            ctoDate.SetEnabled(false);
        } else if (value == "YTD") {
            var date = new Date();
            var firstDay = new Date(date.getFullYear(), 3, 1);
            cFormDate.SetDate(new Date(firstDay));
            ctoDate.SetDate(new Date());
            cFormDate.SetEnabled(false);
            ctoDate.SetEnabled(false);
        } else if (value == "QTD") {
            var yr = new Date().getFullYear();
            //1st QTD

            var dateFrom1 = new Date("04/01/" + yr);
            var dateTo1 = new Date("06/30/" + yr);
            // 2nd QTD
            var dateFrom2 = new Date("07/01/" + yr);
            var dateTo2 = new Date("09/30/" + yr);
            // 3rd 
            var dateFrom3 = new Date("10/01/" + yr);
            var dateTo3 = new Date("12/31/" + yr);
            //4th
            var dateFrom4 = new Date("01/01/" + yr);
            var dateTo4 = new Date("03/31/" + yr);

            var Today = new Date()


            if (Today > dateFrom1 && Today < dateTo1) {
                cFormDate.SetDate(new Date(dateFrom1));
                ctoDate.SetDate(new Date());
            } else if (Today > dateFrom2 && Today < dateTo2) {
                cFormDate.SetDate(new Date(dateFrom2));
                ctoDate.SetDate(new Date());
            } else if (Today > dateFrom3 && Today < dateTo3) {
                cFormDate.SetDate(new Date(dateFrom3));
                ctoDate.SetDate(new Date());
            } else if (Today > dateFrom4 && Today < dateTo4) {
                cFormDate.SetDate(new Date(dateFrom4));
                ctoDate.SetDate(new Date());
            } else {

            }
            cFormDate.SetEnabled(false);
            ctoDate.SetEnabled(false);

        } else {
            cFormDate.SetEnabled(false);
            ctoDate.SetEnabled(false);
        }
    })
});


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