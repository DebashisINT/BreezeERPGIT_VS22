
function CustomerButnClick(s, e) {
    $('#CustModel').modal('show');
}

function Customer_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#CustModel').modal('show');
    }
}

function Customerkeydown(e) {
    var OtherDetails = {}

    if ($.trim($("#txtCustSearch").val()) == "" || $.trim($("#txtCustSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtCustSearch").val();
           

    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Customer Name");
        HeaderCaption.push("Unique Id");
        HeaderCaption.push("Address");

        if ($("#txtCustSearch").val() != "") {
            callonServer("ProjectDB.aspx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[customerindex=0]"))
            $("input[customerindex=0]").focus();
    }
}

function ValueSelected(e, indexName) {
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var Id = e.target.parentElement.parentElement.cells[0].innerText;
        var name = e.target.parentElement.parentElement.cells[1].children[0].value;
        if (Id) {
            if (indexName == "customerIndex") {
                SetCustomer(Id, name);
            }
        }
    }
    else if (e.code == "ArrowDown") {
        thisindex = parseFloat(e.target.getAttribute(indexName));
        thisindex++;
        if (thisindex < 10)
            $("input[" + indexName + "=" + thisindex + "]").focus();
    }
    else if (e.code == "ArrowUp") {
        thisindex = parseFloat(e.target.getAttribute(indexName));
        thisindex--;
        if (thisindex > -1)
            $("input[" + indexName + "=" + thisindex + "]").focus();
        else {
            if (indexName == "customerIndex")
                $('#txtCustSearch').focus();
        }
    }
}


function SetCustomer(Id, Name) {


    var key = Id;
    if (key != null && key != '') {
        $('#CustModel').modal('hide');
        ctxtCustName.SetText(Name);
        $("#hdnCustomerId").val(Id);
    }
}

function  ProjectCustomerWiseSearch() 
{
    am4core.disposeAllCharts();

    var SearchKey = clookup_Project.gridView.GetSelectedKeysOnPage();
    var fromDate = $('#toDateRE').val();
    var toDate = changeDateFormat(fromDate);

    var CustomerId = $("#hdnCustomerId").val();
           
    //localStorage.setItem("InvoiceList_ToDate", tstartdate.GetDate().format('yyyy-MM-dd'));
    var quote_Id = clookup_Project.gridView.GetSelectedKeysOnPage();
    if (clookup_Project.GetText() == "")
    {
        jAlert("Please Select Project code.");
        return false;
    }
    if (fromDate == "") {
        jAlert("Please Select Date");
        return false;
    }
    if (quote_Id.length>3) {
        jAlert("Please Select Maximum 3 Projects.");
        return false;
    }
    var ProjectId = clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex());
    console.log('projectId', ProjectId)
    var dt = {};
    dt.toDate = toDate;
    dt.SearchKey = SearchKey.join(',');
    dt.partyId = CustomerId;
    $('#showTbleInfo').html('<tr><td colspan="6">Select Project to Generate Data</td></tr>');
    //console.log(dt);
    $.ajax({
        type: "POST",
        url: "projectDB.aspx/GetProjSum",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async:false,
        success: function (data) {
                    
            var d = data.d;
            var intCost = numFormatter(parseFloat(d[0].initial_Est_Cost));
            var rvsCost = numFormatter(parseFloat(d[0].Est_Revised));
            var estCost = numFormatter(parseFloat(d[0].Order_Value));
            var costBooked = numFormatter(parseFloat(d[0].Cost_Booked));
            var orderBalance = numFormatter(parseFloat(d[0].Est_Balance));
            var intRev = numFormatter(parseFloat(d[0].Initial_Revenue));
            var rvsRev = numFormatter(parseFloat(d[0].Revised));
            var rvBooked = numFormatter(parseFloat(d[0].Revenue_booked));
            var odBalance = numFormatter(parseFloat(d[0].Revenue_balance));
            var intProfit = numFormatter(parseFloat(d[0].Profit));
            var revProfit = numFormatter(parseFloat(d[0].RevProfit));
            //actual numbers
            $('#intCostF').text(numberWithCommas(d[0].initial_Est_Cost));
            $('#rvsCostF').text(numberWithCommas(d[0].Est_Revised));
            $('#estCostF').text(numberWithCommas(d[0].Order_Value));
            $('#costBookedF').text(numberWithCommas(d[0].Cost_Booked));
            $('#orderBalanceF').text(numberWithCommas(d[0].Est_Balance));
            $('#intRevF').text(numberWithCommas(d[0].Initial_Revenue));
            $('#rvsRevF').text(numberWithCommas(d[0].Revised));
            $('#rvBookedF').text(numberWithCommas(d[0].Revenue_booked));
            $('#odBalanceF').text(numberWithCommas(d[0].Revenue_balance));
            $('#intProfitF').text(numberWithCommas(d[0].Profit));
            $('#rvProfitF').text(numberWithCommas(d[0].RevProfit));
            console.log(d[0]);
            //formated numbers
            $('#intCost').text(intCost);
            $('#rvsCost').text(rvsCost);
            $('#estCost').text(estCost);
            $('#costBooked').text(costBooked);
            $('#orderBalance').text(orderBalance);
            $('#intRev').text(intRev);
            $('#rvsRev').text(rvsRev);
            $('#rvBooked').text(rvBooked);
            $('#odBalance').text(odBalance);
            $('#intProfit').text(intProfit);
            $('#rvProfit').text(revProfit);

            $('#loaderP').hide();
        },
        error: function (data) {
            console.log(data);
        }
    });
    showPdetail();
    showTimeline();
    showCostBreakUp();

}
        
// get Cost BreakUp 
function showCostBreakUp() {
    var SearchKey = clookup_Project.gridView.GetSelectedKeysOnPage();
    var fromDate = $('#toDateRE').val();
    var toDate = changeDateFormat(fromDate);

    var CustomerId = $("#hdnCustomerId").val();
    var quote_Id = clookup_Project.gridView.GetSelectedKeysOnPage();
    if (clookup_Project.GetText() == "") {
        jAlert("Please Select Project code.");
        return false;
    }
    if (quote_Id.length > 3) {
        jAlert("Please Select Maximum 3 Projects.");
        return false;
    }
    var ProjectId = clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex());
    //localStorage.setItem("InvoiceList_ToDate", tstartdate.GetDate().format('yyyy-MM-dd'));

    var dt = {};
    dt.toDate = toDate;
    dt.SearchKey = SearchKey.join(',');
    dt.partyId = CustomerId;
    console.log(dt);
    $.ajax({
        type: "POST",
        url: "projectDB.aspx/GetCostBreakUp",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            console.log('cost breakup', data);
            var d = data.d[0];
            if (data.d[0].Initial_Material_Cost == "0.00" && data.d[0].Initial_Service_Cost == "0.00") {
                $('#chartcosting1').addClass('flexViewCenter').text('No Data Found');
                return false;
            }
            var result = Object.keys(d).map(function (key) {
                var itemK;
                        
                if (key == 'Initial_Material_Cost') {
                    itemK = 'Initial Material Cost';
                } else if (key == 'Initial_Service_Cost') {
                    itemK = 'Initial Service Cost';
                }
                return {
                    "project": itemK,
                    "cost": d[key]
                };
            });
            am4core.ready(function () {
                        
                am4core.useTheme(am4themes_animated); 
                var chart = am4core.create("chartcosting1", am4charts.PieChart);
                var pieSeries = chart.series.push(new am4charts.PieSeries());
                pieSeries.dataFields.value = "cost";
                pieSeries.dataFields.category = "project";
                chart.innerRadius = am4core.percent(0);
                pieSeries.slices.template.stroke = am4core.color("#fff");
                pieSeries.slices.template.strokeWidth = 2;
                pieSeries.slices.template.strokeOpacity = 1;
                pieSeries.slices.template
                  // change the cursor on hover to make it apparent the object can be interacted with
                  .cursorOverStyle = [
                    {
                        "property": "cursor",
                        "value": "pointer"
                    }
                  ];

                pieSeries.alignLabels = false;
                //pieSeries.labels.template.bent = true;
                pieSeries.labels.template.radius = 6;
                pieSeries.labels.template.padding(0, 0, 0, 0);
                pieSeries.ticks.template.disabled = true;

                        
                var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
                shadow.opacity = 0;
                var hoverState = pieSeries.slices.template.states.getKey("hover"); // normally we have to create the hover state, in this case it already exists

                // Slightly shift the shadow and make it more prominent on hover
                var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
                hoverShadow.opacity = 0.7;
                hoverShadow.blur = 5;
                // Add a legend
                chart.legend = new am4charts.Legend();
                        
                chart.data = result;

            }); // end am4core.ready()

        },
        error: function (data) {
            console.log(data);
        }
    });
    $.ajax({
        type: "POST",
        url: "projectDB.aspx/GetCostBreakUpR",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            console.log('cost breakup', data);
            var d = data.d[0];
            if (data.d[0].Revised_Material_Cost == "0.00" && data.d[0].Revised_Service_Cost == "0.00") {
                $('#chartcosting2').addClass('flexViewCenter').text('No Data Found');
                return false;
            }
            var result = Object.keys(d).map(function (key) {
                var itemK;
                if (key == 'Revised_Material_Cost') {
                    itemK = 'Revised Material Cost';
                } else if (key == 'Revised_Service_Cost') {
                    itemK = 'Revised Service Cost';
                }
                return {
                    "project": itemK,
                    "cost": d[key]
                };
            });
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end
                        
                // Create chart instance
                var chart = am4core.create("chartcosting2", am4charts.PieChart);

                // Add and configure Series
                var pieSeries = chart.series.push(new am4charts.PieSeries());
                pieSeries.dataFields.value = "cost";
                pieSeries.dataFields.category = "project";

                // Let's cut a hole in our Pie chart the size of 30% the radius
                chart.innerRadius = am4core.percent(0);

                // Put a thick white border around each Slice
                pieSeries.slices.template.stroke = am4core.color("#fff");
                pieSeries.slices.template.strokeWidth = 2;
                pieSeries.slices.template.strokeOpacity = 1;
                pieSeries.slices.template
                  // change the cursor on hover to make it apparent the object can be interacted with
                  .cursorOverStyle = [
                    {
                        "property": "cursor",
                        "value": "pointer"
                    }
                  ];

                pieSeries.alignLabels = false;
                //pieSeries.labels.template.bent = true;
                pieSeries.labels.template.radius = 6;
                pieSeries.labels.template.padding(0, 0, 0, 0);

                //pieSeries.ticks.template.disabled = true;

                // Create a base filter effect (as if it's not there) for the hover to return to
                var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
                shadow.opacity = 0;

                // Create hover state
                var hoverState = pieSeries.slices.template.states.getKey("hover"); // normally we have to create the hover state, in this case it already exists

                // Slightly shift the shadow and make it more prominent on hover
                var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
                hoverShadow.opacity = 0.7;
                hoverShadow.blur = 5;

                // Add a legend
                chart.legend = new am4charts.Legend();

                chart.data = result;

            }); // end am4core.ready()
        },
        error: function (data) {
            console.log(data);
        }
    });
}

        
function start(data) {

    g = new JSGantt.GanttChart(document.getElementById('GanttChartDIV'), 'days');
    //console.log(data);


    //const newDataurl = data;
    const vDebug = document.querySelector('#debug:checked') ? true : false;
    //vDebug = true;
    const vEditable = document.querySelector('#editable:checked') ? true : false;
    //const vUseSort = document.querySelector('#sort:checked') ? true : false;
    //const newtooltiptemplate = document.getElementById('tooltiptemplate').value ? document.getElementById('tooltiptemplate').value : null;
    //let vColumnOrder;
    //if (document.querySelector('#vColumnOrder').value) {
    //    vColumnOrder = document.querySelector('#vColumnOrder').value.split(',')
    //}
    const vScrollTo = 'today'; // or new Date() or a Date object with a specific date
    // SET LANG FROM INPUT
    g.setOptions({
        vCaptionType: 'Complete', // Set to Show Caption : None,Caption,Resource,Duration,Complete,
        vQuarterColWidth: 36,
        vDateTaskDisplayFormat: 'day dd month yyyy', // Shown in tool tip box
        vDayMajorDateDisplayFormat: 'mon yyyy - Week ww', // Set format to display dates in the "Major" header of the "Day" view
        vWeekMinorDateDisplayFormat: 'dd mon', // Set format to display dates in the "Minor" header of the "Week" view
        vLang: 'en',
        vUseSingleCell: 1000, // Set the threshold at which we will only use one cell per table row (0 disables).  Helps with rendering performance for large charts.
        vShowRes: 0,
        vShowCost: 0,
        vShowAddEntries: 0,
        vShowComp: 0,
        vShowDur: 0,
        vShowStartDate: 1,
        vShowEndDate: 1,
        vShowPlanStartDate: 0,
        vShowPlanEndDate: 0,
        vAdditionalHeaders: 0,
        vTotalHeight: 500,
        vEventClickRow: console.log,
        vShowTaskInfoLink: 0, // Show link in tool tip (0/1)
        vShowEndWeekDate: 1, // Show/Hide the date for the last day of the week in header for daily view (1/0)
        vShowWeekends: 1, // Show weekends days in the vFormat day
        vTooltipDelay: 150,
        vDebug: false,
        vEditable: false,
                
        //vColumnOrder,
        //vScrollTo,
        // vUseSort,
        vFormat: 'day',
        vFormatArr: ['Day', 'Week', 'Month', 'Quarter'], // Even with setUseSingleCell using Hour format on such a large chart can cause issues in some browsers
    });
    g.setDayColWidth(72);
    g.setDateInputFormat("dd/mm/yyyy");
    g.setWeekColWidth(300);
    g.setMonthColWidth(350);
    g.setQuarterColWidth(250);
    g.setRowHeight(50);
    g.addLang('en2', { 'format': 'Select', 'comp': 'Complete' });
    data = JSON.parse(data);
    JSGantt.addJSONTask(g, data)
    g.Draw();
}
function showTimeline() {
    var SearchKey = clookup_Project.gridView.GetSelectedKeysOnPage();
    var fromDate = $('#toDateRE').val();
    var toDate = changeDateFormat(fromDate);

    var CustomerId = $("#hdnCustomerId").val();
    var quote_Id = clookup_Project.gridView.GetSelectedKeysOnPage();
    if (clookup_Project.GetText() == "") {
        jAlert("Please Select Project code.");
        return false;
    }
    if (quote_Id.length > 3) {
        jAlert("Please Select Maximum 3 Projects.");
        return false;
    }
    var ProjectId = clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex());
    //localStorage.setItem("InvoiceList_ToDate", tstartdate.GetDate().format('yyyy-MM-dd'));

    var dt = {};
    dt.toDate = toDate;
    dt.SearchKey = SearchKey.join(',');
    dt.partyId = CustomerId;
    console.log(dt);
    $.ajax({
        type: "POST",
        url: "projectDB.aspx/GetTimeLineTble",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            console.log(data);
            var d = data.d;
            var eTable = ""
            for (var i = 0; i < d.length; i++) {
                eTable += "<tr data-ide='" + d[i]['PROJECT_ID'] + "'>"
                eTable += "<td>" + d[i]['PARTYID'] + "</td>"
                eTable += "<td>" + d[i]['PROJECT_CODE'] + "</td>"
                eTable += "<td>" + d[i]['PROJECT_NAME'] + "</td>"
                eTable += "<td>" + d[i]['CUSTOMERNAME'] + "</td>"
                eTable += "<td>" + d[i]['PROJECT_MANAGER'] + "</td>"
                eTable += "<td>" + d[i]['PROJECTSTAGE'] + "</td>"
                eTable += "<td>" + d[i]['PROJECTSTATUS'] + "</td>"
                eTable += "<td>" + d[i]['PROJ_ACTUALSTARTDATE'] + "</td>"
                eTable += "<td>" + d[i]['PROJ_ESTIMATESTARTDATE'] + "</td>"
                eTable += "<td>" + d[i]['PROJ_ACTUALENDDATE'] + "</td>"
                eTable += "<td>" + d[i]['PROJ_ESTIMATEHOURS'] + "</td>"
                eTable += "<td>" + d[i]['PROJ_ACTUALHOURS'] + "</td>"
                eTable += "<td>" + d[i]['PROJ_ESTLABOURCOST'] + "</td>"
                eTable += "<td>" + d[i]['PROJ_ACTUALLABOURCOST'] + "</td>"
                eTable += "<td>" + d[i]['PROJ_ESTTOTALCOST'] + "</td>"
                eTable += "</tr>"
            }
            eTable += "";
            $('#showTbleInfoTimeline').html(eTable);
        },
        error: function (data) {
            console.log(data);
        }
    });
            
}
        

function getGrantt(id) {
    //$('#gnatt-wrap, #gnatt-wrap-backdrop').show();
    $('#TmChart').hide();
    var data = {};
    data.id = id;
             
    $.ajax({
        type: "POST",
        url: "projectDB.aspx/getGranttData",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            //console.log(response);
            var Data = JSON.stringify(response.d);
            if (response.d.length > 0) {
                console.log('gnattDta', Data)
                $('#TmChart').show();
                start(Data);
            } else {
                //alert('Please make WBS')
            }
            //
        }
    });
}
function showPdetail() {
    var SearchKey = clookup_Project.gridView.GetSelectedKeysOnPage();
    var fromDate = $('#toDateRE').val();
    var toDate = changeDateFormat(fromDate);

    var CustomerId = $("#hdnCustomerId").val();
    var quote_Id = clookup_Project.gridView.GetSelectedKeysOnPage();
    if (clookup_Project.GetText() == "") {
        jAlert("Please Select Project code.");
        return false;
    }
    if (quote_Id.length > 3) {
        jAlert("Please Select Maximum 3 Projects.");
        return false;
    }
    var ProjectId = clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex());
    //localStorage.setItem("InvoiceList_ToDate", tstartdate.GetDate().format('yyyy-MM-dd'));

    var dt = {};
    dt.toDate = toDate;
    dt.SearchKey = SearchKey.join(',');
    dt.partyId = CustomerId;
           
    $.ajax({
        type: "POST",
        url: "projectDB.aspx/GetProjDetail",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {

            var d = data.d[0];
                   
            var result = Object.keys(d).map(function (key) {
                return {
                    "project": key,
                    "value": d[key]
                };
            });
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end
                      
                // Create chart instance
                var chart = am4core.create("chartdiv", am4charts.PieChart);

                // Add and configure Series
                var pieSeries = chart.series.push(new am4charts.PieSeries());
                pieSeries.dataFields.value = "value";
                pieSeries.dataFields.category = "project";

                // Let's cut a hole in our Pie chart the size of 30% the radius
                chart.innerRadius = am4core.percent(30);

                // Put a thick white border around each Slice
                pieSeries.slices.template.stroke = am4core.color("#fff");
                pieSeries.slices.template.strokeWidth = 2;
                pieSeries.slices.template.strokeOpacity = 1;
                pieSeries.slices.template
                // change the cursor on hover to make it apparent the object can be interacted with
                .cursorOverStyle = [
                    {
                        "property": "cursor",
                        "value": "pointer"
                    }
                ];

                pieSeries.alignLabels = false;
                pieSeries.labels.template.bent = true;
                pieSeries.labels.template.radius = 3;
                pieSeries.labels.template.padding(0, 0, 0, 0);
                pieSeries.labels.template.text = "";
                pieSeries.slices.template.tooltipText = "{category}: {value}";
                        
                pieSeries.ticks.template.disabled = true;
                        
                        
                        
                // Create a base filter effect (as if it's not there) for the hover to return to
                var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
                shadow.opacity = 0;

                // Create hover state
                var hoverState = pieSeries.slices.template.states.getKey("hover"); // normally we have to create the hover state, in this case it already exists

                // Slightly shift the shadow and make it more prominent on hover
                var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
                hoverShadow.opacity = 0.7;
                hoverShadow.blur = 5;

                // Add a legend
                chart.legend = new am4charts.Legend();
                pieSeries.legendSettings.valueText = "{ }";
                pieSeries.legendSettings.labelText = "{category}: {value}";
                pieSeries.colors.list = [
                am4core.color("#5ccd86"),
                am4core.color("#6771dc"),
                am4core.color("#ee4040"),
                am4core.color("#FF9671"),
                am4core.color("#FFC75F"),
                am4core.color("#F9F871"),
                ];
                chart.data = result;
            }); // end am4core.ready()
                    
        },
        error: function (data) {
            console.log(data);
        }
    });
    // project details bar chart
    $.ajax({
        type: "POST",
        url: "projectDB.aspx/GetProjDetailBar",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
                    
                    
            am4core.ready(function () {

                // Themes begin
                am4core.useTheme(am4themes_animated);
                // Themes end
                       
                // Create chart instance
                var chart = am4core.create("chartdiv2", am4charts.XYChart);

                // Add data
                chart.data = data.d;

                // Create axes
                var yAxis = chart.yAxes.push(new am4charts.CategoryAxis());
                yAxis.dataFields.category = "Proj_Name";
                yAxis.renderer.grid.template.location = 0;
                yAxis.renderer.labels.template.fontSize = 10;
                yAxis.renderer.minGridDistance = 10;
                yAxis.cursorTooltipEnabled = false;
                var xAxis = chart.xAxes.push(new am4charts.ValueAxis());

                // Create series
                var series = chart.series.push(new am4charts.ColumnSeries());
                var columnTemplate = series.columns.template;
                series.dataFields.valueX = "STAGESTATUS";
                series.dataFields.categoryY = "Proj_Name";
                columnTemplate.tooltipText = "";
                columnTemplate.strokeWidth = 0;
                columnTemplate.fill = am4core.color("#a55");

                series.columns.template.column.cornerRadiusTopRight = 10;
                series.columns.template.column.cornerRadiusBottomRight = 10;
                series.columns.template.events.on("hit", function (ev) {
                    getInfoProject(ev.target.dataItem.dataContext.Proj_Id);
                });

                columnTemplate.adapter.add("fill", function (fill, target) {
                    if (target.dataItem && (target.dataItem.valueX == null)) {
                        return am4core.color("#474747");
                    } else if (target.dataItem && (target.dataItem.valueX == 83.33)) {
                        return am4core.color("#32C47E");
                    } else if (target.dataItem && (target.dataItem.valueX == 66.67)) {
                        return am4core.color("#6871DC");
                    }
                    else if (target.dataItem && (target.dataItem.valueX == 50.00)) {
                        return am4core.color("#D8F1E2");
                    }
                    else if (target.dataItem && (target.dataItem.valueX == 33.33)) {
                        return am4core.color("#6286A3");
                    }
                    else if (target.dataItem && (target.dataItem.valueX == 0.00)) {
                        return am4core.color("#A35E5E");
                    }
                    else {
                        return fill;
                    }
                });

                // Add ranges
                function addRange(label, start, end, color) {
                    var range = yAxis.axisRanges.create();
                    range.category = start;
                    range.endCategory = end;
                    range.label.text = label;
                    range.label.disabled = false;
                    range.label.fill = color;
                    range.label.location = 0;
                    range.label.dx = -130;
                    range.label.dy = 12;
                    range.label.fontWeight = "bold";
                    range.label.fontSize = 12;
                    range.label.horizontalCenter = "left"
                    range.label.inside = true;

                    range.grid.stroke = am4core.color("#396478");
                    range.grid.strokeOpacity = 1;
                    range.tick.length = 200;
                    range.tick.disabled = false;
                    range.tick.strokeOpacity = 0.6;
                    range.tick.stroke = am4core.color("#396478");
                    range.tick.location = 0;

                    range.locations.category = 1;
                }

                chart.cursor = new am4charts.XYCursor();

            }); // end am4core.ready()
                    
        },
        error: function (data) {
            console.log(data);
        }
    });
}
        
function getInfoProject(id) {
    var SearchKey = clookup_Project.gridView.GetSelectedKeysOnPage();
    var fromDate = $('#toDateRE').val();
    var toDate = changeDateFormat(fromDate);

    var CustomerId = $("#hdnCustomerId").val();
    var quote_Id = clookup_Project.gridView.GetSelectedKeysOnPage();
    if (clookup_Project.GetText() == "") {
        jAlert("Please Select Project code.");
        return false;
    }
    if (quote_Id.length > 3) {
        jAlert("Please Select Maximum 3 Projects.");
        return false;
    }
    var ProjectId = clookup_Project.GetGridView().GetRowKey(clookup_Project.GetGridView().GetFocusedRowIndex());

    var dt = {};
    dt.toDate = toDate;
    dt.SearchKey = SearchKey.join(',');
    dt.partyId = CustomerId;
    dt.Pid = id;
    console.log(dt);
    $.ajax({
        type: "POST",
        url: "projectDB.aspx/GetProjDetailSingle",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            console.log('detailTable', data);
            var d = data.d;
            var eTable = ""
            for (var i = 0; i < d.length; i++) {
                eTable += "<tr>"
                eTable += "<td>" + d[i]['PROJECT_CODE'] + "</td>"
                eTable += "<td>" + d[i]['PROJECT_NAME'] + "</td>"
                eTable += "<td>" + d[i]['PARTYID'] + "</td>"
                eTable += "<td>" + d[i]['PROJECT_MANAGER'] + "</td>"
                eTable += "<td>" + d[i]['PROJECTSTAGE'] + "</td>"
                eTable += "<td>" + d[i]['CUSTOMERNAME'] + "</td>"
                eTable += "<td>" + d[i]['PROJ_ACTUALSTARTDATE'] + "</td>"
                eTable += "<td>" + d[i]['PROJ_ESTIMATESTARTDATE'] + "</td>"
                eTable += "<td>" + d[i]['PROJ_ACTUALENDDATE'] + "</td>"
                eTable += "<td>" + d[i]['PROJ_ESTIMATEHOURS'] + "</td>"
                eTable += "<td>" + d[i]['PROJ_ACTUALHOURS'] + "</td>"
                eTable += "<td>" + d[i]['PROJ_ESTLABOURCOST'] + "</td>"
                eTable += "<td>" + d[i]['PROJ_ACTUALLABOURCOST'] + "</td>"
                eTable += "<td>" + d[i]['PROJ_ESTTOTALCOST'] + "</td>"
                eTable += "</tr>"
            }
            eTable += "";
            $('#showTbleInfo').html(eTable);
        },
        error: function (data) {
            console.log(data);
        }
    });
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
        return num; // if value < 1000, nothing to do
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
$(document).ready(function () {
            
    $('#TmChart, #loaderP').hide();
    $('body').on('click', '#showTbleInfoTimeline > tr', function () {
        $('#showTbleInfoTimeline > tr').removeClass('rActive');
        $(this).addClass('rActive');
        var dataId = $(this).data('ide');
        getGrantt(dataId);
    });
    $('.ful').click(function(){
        $(this).parent().parent('.shadowBox').toggleClass('full-screen');
    })
})


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