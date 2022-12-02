import React from 'react';
// Amachart section imports
import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import { Empty } from 'antd';

class topItemsChart extends React.Component {
    constructor(props) {
      super(props);
    }
    
    componentDidMount() {
        this.generateTNSalChart(this.props.data)
    }
    componentDidUpdate(){
    
        this.generateTNSalChart(this.props.data)
    }
    generateTNSalChart = (data) =>{
        
        am4core.ready(function() {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            
            var chart = am4core.create("itemsChart", am4charts.PieChart3D);
            chart.hiddenState.properties.opacity = 0; // this creates initial fade-in
            //chart.depth = 120;
            chart.legend = new am4charts.Legend();
            chart.legend.labels.template.maxWidth = 120
            chart.data = data;
            
            var series = chart.series.push(new am4charts.PieSeries3D());
            series.dataFields.value = "AmtVal";
            series.dataFields.category = "Name";
            series.slices.template.cornerRadius = 5;
            series.colors.step = 6;    
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
                            <div id="itemsChart" style={{ width: "100%", height: "400px" }}></div>
                        ) : <Empty />
                    }  
              </div>
          )
      }

}
export default  topItemsChart;