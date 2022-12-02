import React from 'react';
//import { useTheme, create, Scrollbar, LinearGradientModifier  } from '@amcharts/amcharts4/core';
//import { SlicedChart, DateAxis, ValueAxis, FunnelSeries, XYCursor } from '@amcharts/amcharts4/charts';
import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import axios from 'axios';

class Opprtunity extends React.Component {
    constructor(props) {
      super(props);
    }
    componentDidMount(){
        // Themes begin
        axios({
            method: 'post',
            url: '../ajax/CRMdash/CRMdash.aspx/getOpportunity',
            data: {}
            })
            .then(res => {
                console.log(res)
                this.generateOppotunityChart(res.data.d);
            });
    }
    generateOppotunityChart = (data) => {
        am4core.useTheme(am4themes_animated);
        var charta = am4core.create("OPPORTUNITYFunnel", am4charts.SlicedChart);
        charta.data = data;

        let series = charta.series.push(new am4charts.PyramidSeries());
        series.dataFields.value = "opportunityCount";
        series.dataFields.category = "ratingName";
        series.alignLabels = true;
        series.topWidth = am4core.percent(100);
        series.bottomWidth = am4core.percent(0);
        series.colors.step = 7;
        charta.colors.list = [
            am4core.color("#845EC2"),
            am4core.color("#D65DB1"),
            am4core.color("#FF6F91"),
            am4core.color("#FF9671"),
            am4core.color("#FFC75F"),
            am4core.color("#F9F871")
        ];
        charta.legend = new am4charts.Legend();
        charta.legend.position = "bottom";
    }
    render(){
        return (
            <div className="tabInside">
                <div className="row">
                    <div className="col-md-6">
                        <div className="chartBox">
                            <div className="hader">OPPORTUNITY</div>
                            <div id="OPPORTUNITYFunnel" style={{height: "280px"}}></div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

export default Opprtunity;