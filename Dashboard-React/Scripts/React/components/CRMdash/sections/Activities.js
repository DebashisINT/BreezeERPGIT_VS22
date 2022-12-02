import React from 'react';
import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";

class Activities extends React.Component {
    constructor(props) {
      super(props);
    }
    componentDidMount(){
        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            // Create chart instance
            var chart = am4core.create("pie1", am4charts.PieChart);

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
            var chart = am4core.create("pie2", am4charts.PieChart);

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
            var chart = am4core.create("pie3", am4charts.PieChart);

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
            var chart = am4core.create("pie4", am4charts.PieChart);

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
        return (
            <div className="tabInside">
                <div className="backgroundedBoxes">
                    <div className="clearfix col-md-12">
                        <div className="flex-row space-between align-items-center">
                            <div className="flex-item itemType relative">
                                <div className="">
                                    <div className="text-center">
                                        <img src="../assests/images/DashboardIcons/online-activity.png" style={{width: "40px", marginBottom: "12px"}} />
                                    </div>
                                    <div className="valRound semiRound" >
                                        <span>21</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">Activities</div>
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    <div className="text-center">
                                        <img src="../assests/images/DashboardIcons/gmail.png" style={{width: "40px", marginBottom: "12px"}} />
                                    </div>
                                    <div className="valRound semiRound" style={{background: "#f76d6d"}}>
                                        <span id="emailValue">1</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">Emails</div>
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    <div className="text-center">
                                        <img src="../assests/images/DashboardIcons/smartphone.png" style={{width: "40px", marginBottom: "12px"}} />
                                    </div>
                                    <div className="valRound semiRound" style={{background: "#6e98f5"}}>
                                        <span id="callsmsValue">1</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">Call/SMS</div>  
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    <div className="text-center">
                                        <img src="../assests/images/DashboardIcons/visitor.png" style={{width: "40px", marginBottom: "12px"}} />
                                    </div>
                                    <div className="valRound semiRound" style={{background: "#44c8cd"}}>
                                        <span id="visitValue">3</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">Visits</div> 
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    <div className="text-center">
                                        <img src="../assests/images/DashboardIcons/account.png" style={{width: "40px", marginBottom: "12px"}} />
                                    </div>
                                    <div className="valRound semiRound" style={{background: "#9db53c"}}>
                                        <span id="socialValue">0</span>
                                    </div>
                                    <div className="smallmuted ">Total</div>
                                    <div className="hdTag">Social</div> 
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    <div className="text-center">
                                        <img src="../assests/images/DashboardIcons/clipboards.png" style={{width: "40px", marginBottom: "12px"}} />
                                    </div>
                                    <div className="valRound semiRound" style={{background: "#8846c5"}}>
                                        <span id="otherValue">16</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">Others</div>  
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-3">
                        <div className="chartBox">
                            <div className="hader">ACTIVITIES TODAY</div>
                            <div id="pie1" style={{height: "280px"}}></div>
                        </div>
                    </div>
                    <div className="col-md-3">
                        <div className="chartBox">
                            <div className="hader">DUE TODAY</div>
                            <div id="pie2" style={{height: "280px"}}></div>
                        </div>
                    </div>
                    <div className="col-md-3">
                        <div className="chartBox">
                            <div className="hader">COMPLETED TODAY</div>
                            <div id="pie3" style={{height: "280px"}}></div>
                        </div>
                    </div>
                    <div className="col-md-3">
                        <div className="chartBox">
                            <div className="hader">PENDING TODAY</div>
                            <div id="pie4" style={{height: "280px"}}></div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

export default Activities;