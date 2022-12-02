import React from "react";
import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";

import { Skeleton } from 'antd';

class ChartOne extends React.Component {
    componentDidMount () {
        this.props.chartData.length ? this.gChart1(this.props.chartData) : null
    }
    componentDidUpdate(){
        this.props.chartData.length ? this.gChart1(this.props.chartData) : null
    }
    gChart1 = (data)=>{
        console.log('first chart', data)
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
            chart2.data = data;

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
    }
    render(){
        return ( 
            <React.Fragment>
                {
                    this.props.chartData.length ? <div id="chartdiv" style={{height: "340px"}}></div> : <Skeleton active />
                }
            </React.Fragment>
         );
    }   
}
 
export default ChartOne;