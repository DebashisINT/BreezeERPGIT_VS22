import React from 'react';
// Amachart section imports
import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import { Empty } from 'antd';

class TopVendorChart extends React.Component {
    constructor(props) {
      super(props);
    }
    
    componentDidMount() {
        console.log('receipt Mounted', this.props);
        this.generateTNSalChart(this.props.data)
    }
    componentDidUpdate(){
        console.log('receipt updated', this.props);
        this.generateTNSalChart(this.props.data)
    }
    generateTNSalChart = (data) =>{
        am4core.ready(function() {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            
            // Create chart instance
            var chart = am4core.create("vendorChart", am4charts.XYChart3D);
            
            // Add data
            chart.data = data;
            
            // Create axes
            var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "Name";
            categoryAxis.numberFormatter.numberFormat = "#";
            categoryAxis.renderer.inversed = true;
            
            var  valueAxis = chart.xAxes.push(new am4charts.ValueAxis()); 
            
            // Create series
            var series = chart.series.push(new am4charts.ColumnSeries3D());
            series.dataFields.valueX = "AmtVal";
            series.dataFields.categoryY = "Name";
            series.name = "Amount";
            series.columns.template.propertyFields.fill = "color";
            series.columns.template.tooltipText = "Amount in {categoryY}: {valueX}";
            series.columns.template.column3D.stroke = am4core.color("#fff");
            series.columns.template.column3D.strokeOpacity = 0.2;
            
            }); // end am4core.ready()
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
                    
                    </div>
                    {
                        this.props.data.length ? (
                            <div id="vendorChart" style={{ width: "100%", height: "400px" }}></div>
                        ) : <Empty />
                    }  
              </div>
          )
      }

}
export default  TopVendorChart;