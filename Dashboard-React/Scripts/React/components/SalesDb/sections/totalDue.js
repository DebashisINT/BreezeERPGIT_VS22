import React from 'react';
// Amachart section imports
import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import { Empty } from 'antd';
import('../salesDb.css');

class TotalDue extends React.Component {
    constructor(props) {
      super(props);
    }
    
    componentDidMount() {
        console.log('receipt Mounted', this.props);
        this.generateRChart(this.props.data, this.props.charttitle)
    }
    componentDidUpdate(prevprops){
        if(prevprops.totAmount != this.props.totAmount) {
            this.generateRChart(this.props.data, this.props.charttitle)
        }
    }
    generateRChart = (data, charttitle) =>{
        am4core.useTheme(am4themes_animated);
        // Themes end
        // Create chart instance
        var chart = am4core.create("dueDiv", am4charts.XYChart3D);
        //chart.scrollbarX = new am4core.Scrollbar();
        // Add data
        chart.data = data;
        // Create axes
        var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
        categoryAxis.dataFields.category = "yAxis";
        categoryAxis.renderer.grid.template.location = 0;
        categoryAxis.renderer.minGridDistance = 30;
        categoryAxis.renderer.labels.template.horizontalCenter = "right";
        categoryAxis.renderer.labels.template.verticalCenter = "middle";
        categoryAxis.renderer.labels.template.rotation = 270;
        categoryAxis.tooltip.disabled = true;
        categoryAxis.renderer.minHeight = 110;
        var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
        valueAxis.renderer.minWidth = 50;
        valueAxis.title.text = charttitle + " Value";
        valueAxis.title.fontWeight = "bold";
        // Create series
        var series = chart.series.push(new am4charts.ColumnSeries3D());
        series.sequencedInterpolation = true;
        series.dataFields.valueY = "xAxis";
        series.dataFields.categoryX = "yAxis";
        series.tooltipText = charttitle + " in {categoryX}: {valueY}";
        series.columns.template.strokeWidth = 0;
        series.tooltip.pointerOrientation = "vertical";
        series.columns.template.column.cornerRadiusTopLeft = 10;
        series.columns.template.column.cornerRadiusTopRight = 10;
        series.columns.template.column.fillOpacity = 0.8;

        //create line
        var lineSeries = chart.series.push(new am4charts.LineSeries());
        lineSeries.dataFields.valueY = "xAxis";
        lineSeries.dataFields.categoryX = "yAxis";
        lineSeries.name = "";
        lineSeries.strokeWidth = 3; 
        lineSeries.stroke = am4core.color("#fdd400");
        //add bullets
        var circleBullet = lineSeries.bullets.push(new am4charts.CircleBullet());
        circleBullet.circle.fill = am4core.color("#fff");
        circleBullet.circle.strokeWidth = 2;

        // on hover, make corner radiuses bigger
        var hoverState = series.columns.template.column.states.create("hover");
        hoverState.properties.cornerRadiusTopLeft = 0;
        hoverState.properties.cornerRadiusTopRight = 0;
        hoverState.properties.fillOpacity = 1;
        // series.columns.template.adapter.add("fill", function(fill, target) {
        // return chart.colors.getIndex(target.dataItem.index);
        // });      
        // Cursor
        chart.cursor = new am4charts.XYCursor();  
    }

    numberWithCommas = (x) => {
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
    componentWillUnmount() {
        if (this.chart) {
          this.chart.dispose();
        }
      }
      render(){
          return(
              <div>
                    <div className="clearfix">
                    <span className="amountShowing">INR : {this.numberWithCommas(this.props.totAmount)}</span>
                    </div>
                    {
                        this.props.data.length ? (
                            <div id="dueDiv" style={{ width: "100%", height: "400px" }}></div>
                        ) : <Empty />
                    }  
              </div>
          )
      }

}
export default  TotalDue;