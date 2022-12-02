import React from 'react';
// Amachart section imports
import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import { Empty } from 'antd';
import('../salesDb.css');

class TotalOrder extends React.Component {
    constructor(props) {
      super(props);
    }
    
    componentDidMount() {
        this.generateTNSalChart(this.props.data)
    }
    componentDidUpdate(prevprops){
        if(prevprops.totAmount != this.props.totAmount) {
            this.generateTNSalChart(this.props.data)
        }
    }
    generateTNSalChart = (data) =>{
             
                am4core.useTheme(am4themes_animated);
                var chart = am4core.create('OrderChart', am4charts.XYChart3D)
                chart.colors.step = 2;

                chart.legend = new am4charts.Legend()
                chart.legend.position = 'bottom'
                //chart.legend.paddingBottom = 20
                //chart.legend.labels.template.maxWidth = 95

                var xAxis = chart.xAxes.push(new am4charts.CategoryAxis())
                xAxis.dataFields.category = 'Date'
                xAxis.renderer.cellStartLocation = 0.1
                xAxis.renderer.cellEndLocation = 0.9
                xAxis.renderer.grid.template.location = 0;

                var yAxis = chart.yAxes.push(new am4charts.ValueAxis());
                yAxis.min = 0;

                function createSeries(value, name) {
                    var series = chart.series.push(new am4charts.ColumnSeries3D())
                    series.dataFields.valueY = value
                    series.dataFields.categoryX = 'Date'
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
                chart.data = data;
                createSeries('OrderAmt', 'Order Amount');
                createSeries('InvoiceAmt', 'Invoice Amount');

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
                            chart.series.each(function(series) {
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

                            chart.series.each(function(series) {
                                var trueIndex = chart.series.indexOf(series);
                                var newIndex = series.dummyData;

                                var dx = (newIndex - trueIndex + middle - newMiddle) * delta

                                series.animate({ property: "dx", to: dx }, series.interpolationDuration, series.interpolationEasing);
                                series.bulletsContainer.animate({ property: "dx", to: dx }, series.interpolationDuration, series.interpolationEasing);
                            })
                        }
                    }
                }
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
                            <div id="OrderChart" style={{ width: "100%", height: "400px" }}></div>
                        ) : <Empty />
                    }  
              </div>
          )
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

}
export default  TotalOrder;