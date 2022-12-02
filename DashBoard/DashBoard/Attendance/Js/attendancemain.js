$(document).ready(function () { 
    $.ajax({
        type: "POST",
        url: "../service/general.asmx/GetTodaysAttendance",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var retObj = msg.d; 
            $('#TotalEmp').text(retObj.TotalEmp);
            $('#Present').text(retObj.Present);
            $('#todaysAbsent').text(retObj.todaysAbsent);
            $('#LateCount').text(retObj.LateCount);
            $('#LeaveCount').text(retObj.LeaveCount);

              
        }
    });
});



function loadLateComers() {

    $.ajax({
        type: "POST",
        url: "../service/general.asmx/TodaysLate",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var retObj = msg.d;
            console.log(retObj);
            var htmlStr = "<table class='latecomeerclass table' cellspacing='0' cellpadding='0'><thead><tr><th width='25%'>Employee Name</th><th>Late</th><th width='20%' class='centrtext'>Late Hour(s)</th></tr> </thead>";

            var progressClass = "progress-bar-success";
            for (var i = 0; i < retObj.length; i++) {
                htmlStr = htmlStr + "<tr>";
                htmlStr = htmlStr + "<td> " + retObj[i].LeaveName + " </td>";
                if (retObj[i].unit > 100)
                    retObj[i].unit = 100;

                if (retObj[i].unit <= 40)
                    progressClass = "progress-bar-success";
                else if (retObj[i].unit <= 70)
                    progressClass = "progress-bar-warning";
                else 
                    progressClass = "progress-bar-danger";





                htmlStr = htmlStr + "<td> <div class='progress' data-toggle='tooltip' title='Late Hour:" + retObj[i].latetime + "'> <div  class='progress-bar " + progressClass + " ' role='progressbar'   aria-valuemin='0' aria-valuemax='100' style='width:" + retObj[i].unit + "%'>  </div>" + " </td><td class='centrtext'>" + retObj[i].latetime + "</td>";
                htmlStr = htmlStr + "</tr>";
            }
            htmlStr = htmlStr + "</table>";
            document.getElementById('latecomeerDiv').innerHTML = htmlStr;
            $('#latecomers').modal('show');
            $('[data-toggle="tooltip"]').tooltip(); 
        }
    });

}



var timeout;
function scheduleGridUpdate(grid) {
    window.clearTimeout(timeout);
    timeout = window.setTimeout(
        function () {
            if (grid)
            grid.Refresh();
        },
        2000
    );
}
function grid_Init(s, e) {
    scheduleGridUpdate(s);
}
function grid_BeginCallback(s, e) {
    window.clearTimeout(timeout);
}
function grid_EndCallback(s, e) {
    scheduleGridUpdate(s);
}


var timeoutcount;
function scheduleGridUpdatecount(grid) {
    window.clearTimeout(timeoutcount);
    timeoutcount = window.setTimeout(
        function () {
            if (grid)
            grid.Refresh();
        },
        2000
    );
}
function grid_Initcount(s, e) {
    scheduleGridUpdatecount(s);
}
function grid_BeginCallbackcount(s, e) {
    window.clearTimeout(timeoutcount);
}
function grid_EndCallbackcount(s, e) {
    scheduleGridUpdatecount(s);
}


function GenerateLastdaysReport() {
    var otherDet = {};
    otherDet.forLast = ctxtUpdateDays.GetText();
    $.ajax({
        type: "POST",
        url: "../service/general.asmx/GetLastDaysperformance",
        data: JSON.stringify(otherDet),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var retObj = msg.d;
            console.log(retObj); 


            var PieObject = {
                "type": "funnel",
                "theme": "light",
                "dataProvider": [],
                "valueField": "unit",
                "titleField": "LeaveName",
                "marginRight": 240,
                "marginLeft": 50,
                "startX": -500,
                "depth3D": 100,
                "angle": 40,
                "outlineAlpha": 1,
                "outlineColor": "#FFFFFF",
                "outlineThickness": 2,
                "labelPosition": "right",
                "balloonText": "[[title]]: [[value]]n[[description]]",
                "export": {
                    "enabled": true
                }
            };

            //var PieObject = {
                 
            //    "dataProvider": [],
            //    "type": "pie",
            //    "startDuration": 0,
            //    "theme": "light",
            //    "addClassNames": true,
            //    "legend": {
            //        "position": "right",
            //        "marginRight": 100,
            //        "autoMargins": false
            //    },
            //    "innerRadius": "30%",
            //    "defs": {
            //        "filter": [{
            //            "id": "shadow",
            //            "width": "200%",
            //            "height": "200%",
            //            "feOffset": {
            //                "result": "offOut",
            //                "in": "SourceAlpha",
            //                "dx": 0,
            //                "dy": 0
            //            },
            //            "feGaussianBlur": {
            //                "result": "blurOut",
            //                "in": "offOut",
            //                "stdDeviation": 5
            //            },
            //            "feBlend": {
            //                "in": "SourceGraphic",
            //                "in2": "blurOut",
            //                "mode": "normal"
            //            }
            //        }]
            //    },
            //    "valueField": "unit",
            //    "titleField": "LeaveName",
            //    "export": {
            //        "enabled": true
            //    }

            //};
            PieObject.dataProvider = retObj;
            var chart = AmCharts.makeChart("performanceChrtDiv", PieObject);

        }
    });
}