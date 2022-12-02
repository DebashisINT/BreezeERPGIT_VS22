import React from 'react';

import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import UiBox from '../../UIComponents/uiBox';
import axios from 'axios';

class Leads extends React.Component {
    constructor(props) {
      super(props);
    }
    state = {
        lblTotal: "0",
        lblQl: "0",
        lblOpen: "0",
        lblLost: "0"
    }
    componentDidMount(){
        axios({
            method: 'post',
            url: '../ajax/CRMdash/CRMdash.aspx/GetReportsLink',
            data: {}
            })
            .then(res => {
                console.log(res)
               this.setState({
                   ...this.state,
                    lblTotal: res.data.d.TotalLeads,
                    lblQl: res.data.d.QualifiedL,
                    lblOpen: res.data.d.openL,
                    lblLost: res.data.d.LostL
               })
            });
            axios({
                method: 'post',
                url: '../ajax/CRMdash/CRMdash.aspx/LeadbyCampaign',
                data: {}
                })
                .then(res => {
                    console.log(res)
                    am4core.ready(function () {
                        am4core.useTheme(am4themes_animated);
                        var chart3 = am4core.create("chartdiv3", am4charts.XYChart);
                        chart3.data = res.data.d;
                        var yAxis = chart3.yAxes.push(new am4charts.CategoryAxis());
                        yAxis.dataFields.category = "Campaign_Nam";
                        yAxis.renderer.grid.template.location = 0;
                        yAxis.renderer.labels.template.fontSize = 10;
                        yAxis.renderer.minGridDistance = 10;
                        var xAxis = chart3.xAxes.push(new am4charts.ValueAxis());
                        var series = chart3.series.push(new am4charts.ColumnSeries());
                        series.dataFields.valueX = "Campaigncnt";
                        series.dataFields.categoryY = "Campaign_Nam";
                        series.columns.template.tooltipText = " [bold]{valueX}[/]";
                        series.columns.template.strokeWidth = 0;
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
                        chart3.cursor = new am4charts.XYCursor();
                    }); // end am4core.ready()
                });
            axios({
                method: 'post',
                url: '../ajax/CRMdash/CRMdash.aspx/GetLeadbyIndustry',
                data: {}
                })
                .then(res => {
                    am4core.ready(function () {
                        // Themes begin
                        am4core.useTheme(am4themes_animated);
                        // Create chart
                        var chart4 = am4core.create("chartdiv4", am4charts.PieChart);
                        chart4.hiddenState.properties.opacity = 0; // this creates initial fade-in
                        chart4.data = res.data.d;
                        var series = chart4.series.push(new am4charts.PieSeries());
                        series.dataFields.value = "IndutryCost";
                        series.ticks.template.disabled = true;
                        series.labels.template.fill = am4core.color("white");
                        //chart4.legend.maxHeight = 150;
                        //chart4.legend.scrollable = true;
                        series.dataFields.category = "IndutryName";
                        series.slices.template.cornerRadius = 2;
                        series.colors.step = 3;
                        series.hiddenState.properties.endAngle = -90;
                        chart4.legend = new am4charts.Legend();
                        chart4.legend.position = "right";
                    }); // end am4core.ready()
                });

                this.generateBottomCharts()
    }
    render(){
        return (
            <div className="tabInside">
                <div className="row">
                    <div className="col-md-12">
                    <div className="backgroundedBoxes">
                        <div className="d-flex justify-content-center mainDashBoxes">
                            <div className="flex-itm scr">
                                <UiBox
                                imageURI="../assests/images/DashboardIcons/TotalLead.png"
                                color="#0068bd"
                                titletext="Total Lead(s)"
                                data={this.state.lblTotal}
                                subTitle="As on Today"
                                />
                            </div>
                            <div className="flex-itm scr">
                                <UiBox
                                imageURI="../assests/images/DashboardIcons/OpenLead.png"
                                color="#5b4a4a"
                                titletext="Open Lead(s)"
                                data={this.state.lblQl}
                                subTitle="As on Today"
                                />
                            </div>
                            <div className="flex-itm scr">
                                <UiBox
                                imageURI="../assests/images/DashboardIcons/QaLead.png"
                                color="#ee0067"
                                titletext="Qualified Lead(s)"
                                data={this.state.lblOpen}
                                subTitle="As on Today"
                                />
                            </div>
                            <div className="flex-itm scr">
                                <UiBox
                                imageURI="../assests/images/DashboardIcons/LostLead.png"
                                color="#f14b2f"
                                titletext="Lost & Closed"
                                data={this.state.lblLost}
                                subTitle="As on Today"
                                />
                            </div>
                            
                        </div>
                    </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-6">
                        <div className="chartBox">
                            <div className="hader text-center">LEADS BY SOURCE CAMPAIGN</div>
                            <div id="chartdiv3" style={{height: "280px"}}></div>
                        </div>
                    </div>
                    <div className="col-md-6">
                        <div className="chartBox">
                            <div className="hader text-center">LEADS BY INDUSTRY (TOP 10)</div>
                            <div id="chartdiv4" style={{height: "280px"}}></div>
                        </div>
                    </div>
                </div>
                <div className="row" style={{marginTop: "15px"}}>
                    <div className="col-md-4">
                        <div className="chartBox">
                            <div className="hader text-center">LEADS BY RATING</div>
                            <div id="lead1Chart" style={{height: "280px"}}></div>
                        </div>
                    </div>
                    <div className="col-md-4">
                        <div className="chartBox">
                            <div className="hader text-center">LEADS BY AMOUNT</div>
                            <div id="lead2Chart" style={{height: "280px"}}></div>
                        </div>
                    </div>
                    <div className="col-md-4">
                        <div className="chartBox">
                            <div className="hader text-center">LEADS BY STAGE AMOUNT</div>
                            <div id="lead3Chart" style={{height: "280px"}}></div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    generateBottomCharts = () =>{
        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            // Create chart instance
            var chart = am4core.create("lead1Chart", am4charts.PieChart);

            // Add data
            chart.data = [
              { "sector": "Hot", "size": 5 },
              { "sector": "WARM", "size": 21 },
              { "sector": "Cold", "size": 3 }
            ];

            // Add label
            chart.innerRadius = 0;
            var label = chart.seriesContainer.createChild(am4core.Label);
            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());

            pieSeries.dataFields.value = "size";
            pieSeries.dataFields.category = "sector";

            // Let's cut a hole in our Pie chart the size of 30% the radius
            chart.innerRadius = am4core.percent(0);

            // Put a thick white border around each Slice
            pieSeries.slices.template.stroke = am4core.color("#fff");
            pieSeries.slices.template.strokeWidth = 2;
            pieSeries.slices.template.strokeOpacity = 1;
            
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
            chart.legend.maxWidth = 100;
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

            //var colorSet = new am4core.ColorSet();
            //colorSet.list = ["#388E3C", "#FBC02D", "#0288d1", "#F44336", "#8E24AA"].map(function (color) {
            //    return new am4core.color(color);
            //});
            //pieSeries.colors = colorSet;
        }); // end am4core.ready()
        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            // Create chart instance
            var chart = am4core.create("lead2Chart", am4charts.PieChart);

            // Add data
            chart.data = [
              { "sector": "Hot", "size": 45000 },
              { "sector": "WARM", "size": 54562 },
              { "sector": "Cold", "size": 4582 }
            ];

            // Add label
           
            var label = chart.seriesContainer.createChild(am4core.Label);
            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());

            pieSeries.dataFields.value = "size";
            pieSeries.dataFields.category = "sector";

            // Let's cut a hole in our Pie chart the size of 30% the radius
            chart.innerRadius = am4core.percent(40);

            // Put a thick white border around each Slice
            pieSeries.slices.template.stroke = am4core.color("#fff");
            pieSeries.slices.template.strokeWidth = 2;
            pieSeries.slices.template.strokeOpacity = 1;

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
            chart.legend.labels.template.maxWidth = 60;
            chart.legend.contentWidth = 60;
            
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

            //var colorSet = new am4core.ColorSet();
            //colorSet.list = ["#388E3C", "#FBC02D", "#0288d1", "#F44336", "#8E24AA"].map(function (color) {
            //    return new am4core.color(color);
            //});
            //pieSeries.colors = colorSet;
        }); // end am4core.ready()
        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            // Create chart instance
            var chart = am4core.create("lead3Chart", am4charts.PieChart);

            // Add data
            chart.data = [
              { "sector": "New", "size": 45000 },
              { "sector": "Qualified", "size": 54562 },
              { "sector": "Close", "size": 4582 },
              { "sector": "On Progress", "size": 522 }
            ];

            // Add label
            chart.innerRadius = 0;
            var label = chart.seriesContainer.createChild(am4core.Label);
            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());

            pieSeries.dataFields.value = "size";
            pieSeries.dataFields.category = "sector";

            // Let's cut a hole in our Pie chart the size of 30% the radius
            chart.innerRadius = am4core.percent(40);

            // Put a thick white border around each Slice
            pieSeries.slices.template.stroke = am4core.color("#fff");
            pieSeries.slices.template.strokeWidth = 2;
            pieSeries.slices.template.strokeOpacity = 1;

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
            chart.legend.maxWidth = 100;
            chart.legend.contentWidth = 80;
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
        }); // end am4core.ready()
    }
}

export default Leads;