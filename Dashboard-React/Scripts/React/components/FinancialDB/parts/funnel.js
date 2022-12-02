import React from "react";
import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";

class Funnel extends React.Component {
    componentDidMount () {
        this.props.funnelData.length ? this.gChart(this.props.funnelData) : null
    }
    componentDidUpdate(){
        this.props.funnelData.length ? this.gChart(this.props.funnelData) : null
    }
    gChart = (data)=>{
        am4core.ready(function () {
            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            var chart = am4core.create("funnelCahrt", am4charts.SlicedChart);
            chart.data = data;
            var series = chart.series.push(new am4charts.FunnelSeries());
            series.dataFields.value = "value";
            series.dataFields.category = "name";
            series.alignLabels = true;
        }); // end am4core.ready()
    }
    render(){
        return ( 
            <React.Fragment>
                <div id="funnelCahrt" style={{height: "200px"}}></div>
            </React.Fragment>
         );
    }   
}
 
export default Funnel;