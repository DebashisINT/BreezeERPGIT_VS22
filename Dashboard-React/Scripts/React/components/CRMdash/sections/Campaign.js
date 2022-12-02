import React from 'react';
import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import axios from 'axios';


class Campaign extends React.Component {
    constructor(props) {
      super(props);
    }
    componentDidMount(){
        axios({
            method: 'post',
            url: '../ajax/CRMdash/CRMdash.aspx/GetCampaignCost',
            data: {}
            })
            .then(res => {
                console.log(res)
                this.chartBars(res.data.d);
                this.chartBars2(res.data.d)
            });
        this.campChartMultiple();
    }
    chartBars = (data)=>{
        am4core.ready(function () {

            am4core.useTheme(am4themes_animated);
            var chart = am4core.create("chartdiv2", am4charts.XYChart);
            // Add data
            chart.data = data;
            // Create axes
            var yAxis = chart.yAxes.push(new am4charts.CategoryAxis());
            yAxis.dataFields.category = "CampaignName";
            yAxis.renderer.grid.template.location = 0;
            yAxis.renderer.labels.template.fontSize = 10;
            yAxis.renderer.minGridDistance = 10;
            var xAxis = chart.xAxes.push(new am4charts.ValueAxis());
            // Create series
            var series = chart.series.push(new am4charts.ColumnSeries());
            series.dataFields.valueX = "CampaignCost";
            series.dataFields.categoryY = "CampaignName";
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
            chart.cursor = new am4charts.XYCursor();
        }); 
    }
    chartBars2 = (data) =>{
        am4core.ready(function () {
            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            // Create chart instance
            var chart = am4core.create("chartdivRev2", am4charts.XYChart);
            // Add data
            chart.data = data;
            // Create axes
            var yAxis = chart.yAxes.push(new am4charts.CategoryAxis());
            yAxis.dataFields.category = "CampaignName";
            yAxis.renderer.grid.template.location = 0;
            yAxis.renderer.labels.template.fontSize = 10;
            yAxis.renderer.minGridDistance = 10;

            var xAxis = chart.xAxes.push(new am4charts.ValueAxis());

            // Create series
            var series = chart.series.push(new am4charts.ColumnSeries());
            series.dataFields.valueX = "CampaignCost";
            series.dataFields.categoryY = "CampaignName";
            series.columns.template.tooltipText = " [bold]{valueX}[/]";
            series.columns.template.strokeWidth = 0;
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

        });
    }
    render(){
        return (
            <div className="tabInside">
                <div className="row">
                    <div className="col-md-6">
                        <div className="chartBox">
                            <div className="hader text-center">CAMPAIGN BY COST</div>
                            <div id="chartdiv2" style={{height: "280px"}}></div>
                        </div>
                    </div>
                    <div className="col-md-6">
                        <div className="chartBox">
                            <div className="hader text-center">CAMPAIGN BY REVENUE</div>
                            <div id="chartdivRev2" style={{height: "280px"}}></div>
                        </div>
                    </div>
                </div>
                <div className="row" style={{marginTop: "15px"}}>
                    <div className="col-md-6">
                        <div className="chartBox">
                            <div className="hader text-center">CAMPAIGN BY TYPES</div>
                            <div id="campChart3" style={{height: "280px"}}></div>
                        </div>
                    </div>
                    <div className="col-md-6">
                        <div className="chartBox">
                            <div className="hader text-center">CAMPAIGN BY STATUS</div>
                            <div id="campChart4" style={{height: "280px"}}></div>
                        </div>
                    </div>
                </div>
                <div className="row" style={{marginTop: "15px"}}>
                    <div className="col-md-12">
                        <div className="chartBox">
                            <div className="hader text-center">COST OF CAMPAIGN VS ESTEMATED REVENUE</div>
                            <div id="campChart5" style={{height: "280px"}}></div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    campChartMultiple = () =>{
        // campChart3 
        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            // Create chart instance
            var chart = am4core.create("campChart3", am4charts.PieChart);

            // Add data
            chart.data = [
              { "sector": "Advertisement", "size": 8 },
              { "sector": "Co-Branding", "size": 12 },
              { "sector": "Event", "size": 5 },
              { "sector": "Others", "size": 9 }
            ];

            // Add label
            chart.innerRadius = 0;
            var label = chart.seriesContainer.createChild(am4core.Label);
            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());

            pieSeries.dataFields.value = "size";
            pieSeries.dataFields.category = "sector";

            // Let's cut a hole in our Pie chart the size of 30% the radius
            chart.innerRadius = am4core.percent(5);

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

        // campChart4 
        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            // Create chart instance
            var chart = am4core.create("campChart4", am4charts.PieChart);

            // Add data
            chart.data = [
              { "sector": "Advertisement", "size": 8 },
              { "sector": "Co-Branding", "size": 12 },
              { "sector": "Event", "size": 5 },
              { "sector": "Others", "size": 9 }
            ];

            // Add label
            chart.innerRadius = 0;
            var label = chart.seriesContainer.createChild(am4core.Label);
            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());

            pieSeries.dataFields.value = "size";
            pieSeries.dataFields.category = "sector";

            // Let's cut a hole in our Pie chart the size of 30% the radius
            chart.innerRadius = am4core.percent(5);

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

        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end



            var chart = am4core.create('campChart5', am4charts.XYChart)
            chart.colors.step = 2;

            chart.legend = new am4charts.Legend()
            chart.legend.position = 'top'
            chart.legend.paddingBottom = 20
            chart.legend.labels.template.maxWidth = 95

            var xAxis = chart.xAxes.push(new am4charts.CategoryAxis())
            xAxis.dataFields.category = 'category'
            xAxis.renderer.cellStartLocation = 0.1
            xAxis.renderer.cellEndLocation = 0.9
            xAxis.renderer.grid.template.location = 0;

            var yAxis = chart.yAxes.push(new am4charts.ValueAxis());
            yAxis.min = 0;

            function createSeries(value, name) {
                var series = chart.series.push(new am4charts.ColumnSeries())
                series.dataFields.valueY = value
                series.dataFields.categoryX = 'category'
                series.name = name
                //series.tooltipText = " {categoryX}: [bold]{valueY}[/]";
                series.events.on("hidden", arrangeColumns);
                series.events.on("shown", arrangeColumns);

                var bullet = series.bullets.push(new am4charts.LabelBullet())
                bullet.interactionsEnabled = false
                bullet.dy = 30;
                bullet.label.text = '{valueY}'
                bullet.label.fill = am4core.color('#ffffff')

                return series;
            }

            chart.data = [
                {
                    category: 'Branding',
                    first: 40,
                    second: 55
                },
                {
                    category: 'Marketing',
                    first: 30,
                    second: 78
                },
                {
                    category: 'Events',
                    first: 27,
                    second: 40
                },
                {
                    category: 'Others',
                    first: 50,
                    second: 33
                }
            ]


            createSeries('first', 'The Thirst');
            createSeries('second', 'The Second');
           

            function arrangeColumns() {

                var series = chart.series.getIndex(0);

                var w = 1 - xAxis.renderer.cellStartLocation - (1 - xAxis.renderer.cellEndLocation);
                if (series.dataItems.length > 1) {
                    var x0 = xAxis.getX(series.dataItems.getIndex(0), "categoryX");
                    var x1 = xAxis.getX(series.dataItems.getIndex(1), "categoryX");
                    var delta = ((x1 - x0) / chart.series.length) * w;
                    if (am4core.isNumber(delta)) {
                        var middle = chart.series.length / 2;

                        var newIndex = 0;
                        chart.series.each(function (series) {
                            if (!series.isHidden && !series.isHiding) {
                                series.dummyData = newIndex;
                                newIndex++;
                            }
                            else {
                                series.dummyData = chart.series.indexOf(series);
                            }
                        })
                        var visibleCount = newIndex;
                        var newMiddle = visibleCount / 2;

                        chart.series.each(function (series) {
                            var trueIndex = chart.series.indexOf(series);
                            var newIndex = series.dummyData;

                            var dx = (newIndex - trueIndex + middle - newMiddle) * delta

                            series.animate({ property: "dx", to: dx }, series.interpolationDuration, series.interpolationEasing);
                            series.bulletsContainer.animate({ property: "dx", to: dx }, series.interpolationDuration, series.interpolationEasing);
                        })
                    }
                }
            }
        }); // end am4core.ready()
    }
}

export default Campaign;