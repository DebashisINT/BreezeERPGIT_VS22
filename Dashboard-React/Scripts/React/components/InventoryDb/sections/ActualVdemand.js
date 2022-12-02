import React from 'react';
import { DatePicker, Button, Form } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";

class ActulaVdemand extends React.Component {
    constructor(props) {
      super(props);
    }
    componentDidMount(){
        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end



            var chart = am4core.create('bottomChart', am4charts.XYChart)
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
                    category: 'Place #1',
                    first: 40,
                    second: 55
                },
                {
                    category: 'Place #2',
                    first: 30,
                    second: 78
                },
                {
                    category: 'Place #3',
                    first: 27,
                    second: 40
                },
                {
                    category: 'Place #4',
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
        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            // Create chart instance
            var chart = am4core.create("totalActivityChart", am4charts.PieChart);

            // Add data
            chart.data = [
              { "sector": "Agriculture", "size": 6.6 },
              { "sector": "Mining and Quarrying", "size": 0.6 },
              { "sector": "Manufacturing", "size": 23.2 },
              { "sector": "Electricity and Water", "size": 2.2 },
              { "sector": "Construction", "size": 4.5 },
              { "sector": "Trade (Wholesale, Retail, Motor)", "size": 14.6 },
              { "sector": "Transport and Communication", "size": 9.3 },
              { "sector": "Finance, real estate and business services", "size": 22.5 }
            ];

            // Add label
            chart.innerRadius = 60;
            var label = chart.seriesContainer.createChild(am4core.Label);
            label.text = "28";
            label.horizontalCenter = "middle";
            label.verticalCenter = "middle";
            label.fontSize = 25;

            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());
            pieSeries.dataFields.value = "size";
            pieSeries.dataFields.category = "sector";
            //pieSeries.slices.template.stroke = am4core.color("#fff");
            //pieSeries.slices.template.strokeWidth = 2;
            //pieSeries.slices.template.strokeOpacity = 1;

            pieSeries.labels.template.bent = true;
            pieSeries.labels.template.radius = 3;
            pieSeries.labels.template.padding(0, 0, 0, 0);
            pieSeries.ticks.template.disabled = true;
            pieSeries.labels.template.disabled = true;

            // Create a base filter effect (as if it's not there) for the hover to return to
            var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
            shadow.opacity = 0;
            // Create hover state
            var hoverState = pieSeries.slices.template.states.getKey("hover");
            // Slightly shift the shadow and make it more prominent on hover
            var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
            hoverShadow.opacity = 0.7;
            hoverShadow.blur = 5;


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
            var chart = am4core.create("dueTodayChart", am4charts.PieChart);

            // Add data
            chart.data = [
              { "sector": "Agriculture", "size": 6.6 },
              { "sector": "Mining and Quarrying", "size": 0.6 },
              { "sector": "Manufacturing", "size": 23.2 },
              { "sector": "Electricity and Water", "size": 2.2 },
              { "sector": "Construction", "size": 4.5 },
              { "sector": "Trade (Wholesale, Retail, Motor)", "size": 14.6 },
              { "sector": "Transport and Communication", "size": 9.3 },
              { "sector": "Finance, real estate and business services", "size": 22.5 }
            ];

            // Add label
            chart.innerRadius = 60;
            var label = chart.seriesContainer.createChild(am4core.Label);
            label.text = "28";
            label.horizontalCenter = "middle";
            label.verticalCenter = "middle";
            label.fontSize = 25;

            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());
            pieSeries.dataFields.value = "size";
            pieSeries.dataFields.category = "sector";
            //pieSeries.slices.template.stroke = am4core.color("#fff");
            //pieSeries.slices.template.strokeWidth = 2;
            //pieSeries.slices.template.strokeOpacity = 1;

            pieSeries.labels.template.bent = true;
            pieSeries.labels.template.radius = 3;
            pieSeries.labels.template.padding(0, 0, 0, 0);
            pieSeries.ticks.template.disabled = true;
            pieSeries.labels.template.disabled = true;

            // Create a base filter effect (as if it's not there) for the hover to return to
            var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
            shadow.opacity = 0;
            // Create hover state
            var hoverState = pieSeries.slices.template.states.getKey("hover");
            // Slightly shift the shadow and make it more prominent on hover
            var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
            hoverShadow.opacity = 0.7;
            hoverShadow.blur = 5;


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
            var chart = am4core.create("completedtodayChart", am4charts.PieChart);

            // Add data
            chart.data = [
              { "sector": "Agriculture", "size": 6.6 },
              { "sector": "Mining and Quarrying", "size": 0.6 },
              { "sector": "Manufacturing", "size": 23.2 },
              { "sector": "Electricity and Water", "size": 2.2 },
              { "sector": "Construction", "size": 4.5 },
              { "sector": "Trade (Wholesale, Retail, Motor)", "size": 14.6 },
              { "sector": "Transport and Communication", "size": 9.3 },
              { "sector": "Finance, real estate and business services", "size": 22.5 }
            ];

            // Add label
            chart.innerRadius = 60;
            var label = chart.seriesContainer.createChild(am4core.Label);
            label.text = "28";
            label.horizontalCenter = "middle";
            label.verticalCenter = "middle";
            label.fontSize = 25;

            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());
            pieSeries.dataFields.value = "size";
            pieSeries.dataFields.category = "sector";
            //pieSeries.slices.template.stroke = am4core.color("#fff");
            //pieSeries.slices.template.strokeWidth = 2;
            //pieSeries.slices.template.strokeOpacity = 1;

            pieSeries.labels.template.bent = true;
            pieSeries.labels.template.radius = 3;
            pieSeries.labels.template.padding(0, 0, 0, 0);
            pieSeries.ticks.template.disabled = true;
            pieSeries.labels.template.disabled = true;

            // Create a base filter effect (as if it's not there) for the hover to return to
            var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
            shadow.opacity = 0;
            // Create hover state
            var hoverState = pieSeries.slices.template.states.getKey("hover");
            // Slightly shift the shadow and make it more prominent on hover
            var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
            hoverShadow.opacity = 0.7;
            hoverShadow.blur = 5;


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
            var chart = am4core.create("pendingTodayChart", am4charts.PieChart);

            // Add data
            chart.data = [
              { "sector": "Agriculture", "size": 6.6 },
              { "sector": "Mining and Quarrying", "size": 0.6 },
              { "sector": "Manufacturing", "size": 23.2 },
              { "sector": "Electricity and Water", "size": 2.2 },
              { "sector": "Construction", "size": 4.5 },
              { "sector": "Trade (Wholesale, Retail, Motor)", "size": 14.6 },
              { "sector": "Transport and Communication", "size": 9.3 },
              { "sector": "Finance, real estate and business services", "size": 22.5 }
            ];

            // Add label
            chart.innerRadius = 60;
            var label = chart.seriesContainer.createChild(am4core.Label);
            label.text = "28";
            label.horizontalCenter = "middle";
            label.verticalCenter = "middle";
            label.fontSize = 25;

            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());
            pieSeries.dataFields.value = "size";
            pieSeries.dataFields.category = "sector";
            //pieSeries.slices.template.stroke = am4core.color("#fff");
            //pieSeries.slices.template.strokeWidth = 2;
            //pieSeries.slices.template.strokeOpacity = 1;

            pieSeries.labels.template.bent = true;
            pieSeries.labels.template.radius = 3;
            pieSeries.labels.template.padding(0, 0, 0, 0);
            pieSeries.ticks.template.disabled = true;
            pieSeries.labels.template.disabled = true;

            // Create a base filter effect (as if it's not there) for the hover to return to
            var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
            shadow.opacity = 0;
            // Create hover state
            var hoverState = pieSeries.slices.template.states.getKey("hover");
            // Slightly shift the shadow and make it more prominent on hover
            var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
            hoverShadow.opacity = 0.7;
            hoverShadow.blur = 5;


            //var colorSet = new am4core.ColorSet();
            //colorSet.list = ["#388E3C", "#FBC02D", "#0288d1", "#F44336", "#8E24AA"].map(function (color) {
            //    return new am4core.color(color);
            //});
            //pieSeries.colors = colorSet;
        }); // end am4core.ready()
    }
    render(){
        const config = {
            rules: [{ type: 'object', required: true, message: 'Please select Date!' }],
        };
        const dateFormat = 'DD-MM-YYYY';
        return (
            <div className="tabInside">
                <div className="row" style={{marginBottom: "10px"}}> 
                    <Form name="stock_controls" onFinish={this.stockFinish}>
                        <div className="col-md-3 col-sm-6">
                            <label className="bold-lebel mTop0">Select Date</label>
                            <Form.Item name="stock-date" {...config}>
                                <DatePicker  format={dateFormat} />
                            </Form.Item>   
                        </div>
                        <div className="col-md-3">
                            <Form.Item>
                                <Button type="primary" style={{marginTop: "26px"}} icon={<SearchOutlined />} htmlType="submit">Show Analysis</Button>
                            </Form.Item>
                        </div>
                    </Form>
                </div>
                <div className="row">
                    <div className="col-md-3">
                        <div className="chartBox">
                            <div>CURRENT STOCK</div>
                            <div id="totalActivityChart" style={{height: "220px"}}></div>
                        </div>
                    </div>
                    <div className="col-md-3">
                        <div className="chartBox">
                        <div>OPEN SALES ORDER</div>
                        <div id="dueTodayChart" style={{height: "220px"}}></div>
                        </div>
                    </div>
                    <div className="col-md-3">
                        <div className="chartBox">
                            <div>OPEN MANUFATURING ORDER</div>
                            <div id="completedtodayChart" style={{height: "220px"}}></div>
                        </div>
                    </div>
                    <div className="col-md-3">
                        <div className="chartBox">
                            <div>OPEN BRANCH REQUISATION</div>
                            <div id="pendingTodayChart" style={{height: "220px"}}></div>
                        </div>
                    </div>
                </div>

                <div className="row" style={{marginTop: "25px"}}>
                    <div className="col-md-12">
                        <div className="chartBox">
                            <div id="bottomChart" style={{height: "280px"}}></div>
                            <div className="hader text-center">STOCK IN HAND VS DEMAND BY QUANTITY</div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

export default ActulaVdemand;