import React from 'react';
import { DatePicker, Button, Form } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import axios from 'axios';

class TopNProduct extends React.Component {
    constructor(props) {
      super(props);
    }
    state = {
        chartOne:[],
        chartTwo:[],
        chartThree:[],
        chartFour:[]
    }  
    componentDidMount() {
        //this.generateCharts();
    }      
    componentWillUnmount() {   
    }
    onFinish = fieldsValue => {
        const values = {
            ...fieldsValue,
            'datepicker': fieldsValue['date-picker'].format('YYYY-MM-DD'),
            "a" : "hello"
        }
        console.log('Received values of form: ', values.datepicker);
        axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/GetInventoryChartOne',
            data: {
                date :values.datepicker
            }
          }).then(res => {
              console.log('GetInventoryChartOne', res);
              this.setState({
                  ...this.state,
                  chartOne:res.data.d
              })  
              this.generateChartOne(this.state.chartOne) 
          });
          axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/GetInventoryChartTwo',
            data: {
                date :values.datepicker
            }
          }).then(res => {
              console.log('GetInventoryChartTwo', res);
              this.setState({
                  ...this.state,
                  chartTwo:res.data.d
              })  
              this.generateChartTwo(this.state.chartTwo) 
          });
          axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/GetInventoryChartThree',
            data: {
                date :values.datepicker
            }
          }).then(res => {
              console.log('GetInventoryChartThree', res);
              this.setState({
                  ...this.state,
                  chartThree:res.data.d
              })  
              this.generateChartThree(this.state.chartThree) 
          });
          axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/GetInventoryChartFour',
            data: {
                date :values.datepicker
            }
          }).then(res => {
              console.log('GetInventoryChartFour', res);
              this.setState({
                  ...this.state,
                  chartFour:res.data.d
              })  
              this.generateChartFour(this.state.chartFour) 
          });
    }
      render(){
        const config = {
            rules: [{ type: 'object', required: true, message: 'Please select Date!' }],
        };
        const dateFormat = 'DD-MM-YYYY';
          return(     
              <div>
                    <div className="tabInside">
                        <div className="row" style={{marginBottom: "10px"}}> 
                            <Form name="time_related_controls" onFinish={this.onFinish}>
                                <div className="col-md-3 col-sm-6">
                                    <label className="bold-lebel mTop0">Select Date</label>
                                    <Form.Item name="date-picker" {...config}>
                                        <DatePicker  format={dateFormat} />
                                    </Form.Item>   
                                </div>
                                <div className="col-md-3">
                                    <Form.Item>
                                        <Button type="primary" style={{marginTop: "26px"}} icon={<SearchOutlined />} htmlType="submit">Generate Chart</Button>
                                    </Form.Item>
                                </div>
                            </Form>
                        </div>
                        <div className="row">
                            <div className="col-md-6">
                                <div className="chartBox">
                                    <div className="hader">TOP 10 MOST SELLING INVENTORIES</div>
                                    <div id="chartdiv1" style={{height: "400px"}}></div>
                                </div>
                            </div>
                            <div className="col-md-6">
                                <div className="chartBox">
                                    <div className="hader">TOP 10 MOST PURCHASED INVENTORIES</div>
                                    <div id="chartdiv2" style={{height: "400px"}}></div>
                                </div>
                            </div>
                        </div>
                        <div className="row" style={{marginTop: "10px"}}>
                            <div className="col-md-6">
                                <div className="chartBox">
                                    <div className="hader">TOP 10 MOST SALES RETURN INVENTORIES</div>
                                    <div id="chartdiv3" style={{height: "400px"}}></div>
                                </div>
                            </div>
                            <div className="col-md-6">
                                <div className="chartBox">
                                    <div className="hader">TOP 10 MOST PURCHASE RETURN INVENTORIES</div>
                                    <div id="chartdiv4" style={{height: "400px"}}></div>
                                </div>
                            </div>
                        </div>
                    </div>
              </div>
        )
    }
    generateChartOne = (data) => {
        am4core.ready(function() {
         // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            // Create chart instance
            var chart = am4core.create("chartdiv1", am4charts.XYChart);
            // Add data
            chart.data = data;
            // Create axes
            var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "PRODDESC";
            categoryAxis.renderer.grid.template.location = 0;
            categoryAxis.renderer.minGridDistance = 30;
            categoryAxis.renderer.labels.template.horizontalCenter = "right";
            categoryAxis.renderer.labels.template.verticalCenter = "middle";
            categoryAxis.renderer.labels.template.rotation = 270;
            categoryAxis.tooltip.disabled = true;
            categoryAxis.renderer.minHeight = 110;

            var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
            valueAxis.renderer.minWidth = 50;

            // Create series
            var series = chart.series.push(new am4charts.ColumnSeries());
            series.sequencedInterpolation = true;
            series.dataFields.valueY = "QTY";
            series.dataFields.categoryX = "PRODDESC";
            series.tooltipText = "[{categoryX}: bold]{valueY}[/]";
            series.columns.template.strokeWidth = 0;

            series.tooltip.pointerOrientation = "vertical";
            series.columns.template.column.cornerRadiusTopLeft = 10;
            series.columns.template.column.cornerRadiusTopRight = 10;
            series.columns.template.column.fillOpacity = 0.8;
            // on hover, make corner radiuses bigger
            var hoverState = series.columns.template.column.states.create("hover");
            hoverState.properties.cornerRadiusTopLeft = 0;
            hoverState.properties.cornerRadiusTopRight = 0;
            hoverState.properties.fillOpacity = 1;

            series.columns.template.adapter.add("fill", function(fill, target) {
                return chart.colors.getIndex(target.dataItem.index);
            });
            // Cursor
            chart.cursor = new am4charts.XYCursor();

        }); // end am4core.ready()
    }
    generateChartTwo = (data) => {
        am4core.ready(function() {
         // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            // Create chart instance
            var chart = am4core.create("chartdiv2", am4charts.XYChart);
            // Add data
            chart.data = data;
            // Create axes
            var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "PRODDESC";
            categoryAxis.renderer.grid.template.location = 0;
            categoryAxis.renderer.minGridDistance = 30;
            categoryAxis.renderer.labels.template.horizontalCenter = "right";
            categoryAxis.renderer.labels.template.verticalCenter = "middle";
            categoryAxis.renderer.labels.template.rotation = 270;
            categoryAxis.tooltip.disabled = true;
            categoryAxis.renderer.minHeight = 110;

            var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
            valueAxis.renderer.minWidth = 50;

            // Create series
            var series = chart.series.push(new am4charts.ColumnSeries());
            series.sequencedInterpolation = true;
            series.dataFields.valueY = "QTY";
            series.dataFields.categoryX = "PRODDESC";
            series.tooltipText = "[{categoryX}: bold]{valueY}[/]";
            series.columns.template.strokeWidth = 0;

            series.tooltip.pointerOrientation = "vertical";
            series.columns.template.column.cornerRadiusTopLeft = 10;
            series.columns.template.column.cornerRadiusTopRight = 10;
            series.columns.template.column.fillOpacity = 0.8;
            // on hover, make corner radiuses bigger
            var hoverState = series.columns.template.column.states.create("hover");
            hoverState.properties.cornerRadiusTopLeft = 0;
            hoverState.properties.cornerRadiusTopRight = 0;
            hoverState.properties.fillOpacity = 1;

            series.columns.template.adapter.add("fill", function(fill, target) {
                return chart.colors.getIndex(target.dataItem.index);
            });
            // Cursor
            chart.cursor = new am4charts.XYCursor();

        }); // end am4core.ready()
    }
    generateChartThree = (data) => {
        am4core.ready(function() {
         // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            // Create chart instance
            var chart = am4core.create("chartdiv3", am4charts.XYChart);
            // Add data
            chart.data = data;
            // Create axes
            var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "PRODDESC";
            categoryAxis.renderer.grid.template.location = 0;
            categoryAxis.renderer.minGridDistance = 30;
            categoryAxis.renderer.labels.template.horizontalCenter = "right";
            categoryAxis.renderer.labels.template.verticalCenter = "middle";
            categoryAxis.renderer.labels.template.rotation = 270;
            categoryAxis.tooltip.disabled = true;
            categoryAxis.renderer.minHeight = 110;

            var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
            valueAxis.renderer.minWidth = 50;

            // Create series
            var series = chart.series.push(new am4charts.ColumnSeries());
            series.sequencedInterpolation = true;
            series.dataFields.valueY = "QTY";
            series.dataFields.categoryX = "PRODDESC";
            series.tooltipText = "[{categoryX}: bold]{valueY}[/]";
            series.columns.template.strokeWidth = 0;

            series.tooltip.pointerOrientation = "vertical";
            series.columns.template.column.cornerRadiusTopLeft = 10;
            series.columns.template.column.cornerRadiusTopRight = 10;
            series.columns.template.column.fillOpacity = 0.8;
            // on hover, make corner radiuses bigger
            var hoverState = series.columns.template.column.states.create("hover");
            hoverState.properties.cornerRadiusTopLeft = 0;
            hoverState.properties.cornerRadiusTopRight = 0;
            hoverState.properties.fillOpacity = 1;

            series.columns.template.adapter.add("fill", function(fill, target) {
                return chart.colors.getIndex(target.dataItem.index);
            });
            // Cursor
            chart.cursor = new am4charts.XYCursor();

        }); // end am4core.ready()
    }
    generateChartFour = (data) => {
        am4core.ready(function() {
         // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end
            // Create chart instance
            var chart = am4core.create("chartdiv4", am4charts.XYChart);
            // Add data
            chart.data = data;
            // Create axes
            var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "PRODDESC";
            categoryAxis.renderer.grid.template.location = 0;
            categoryAxis.renderer.minGridDistance = 30;
            categoryAxis.renderer.labels.template.horizontalCenter = "right";
            categoryAxis.renderer.labels.template.verticalCenter = "middle";
            categoryAxis.renderer.labels.template.rotation = 270;
            categoryAxis.tooltip.disabled = true;
            categoryAxis.renderer.minHeight = 110;

            var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
            valueAxis.renderer.minWidth = 50;
            // Create series
            var series = chart.series.push(new am4charts.ColumnSeries());
            series.sequencedInterpolation = true;
            series.dataFields.valueY = "QTY";
            series.dataFields.categoryX = "PRODDESC";
            series.tooltipText = "[{categoryX}: bold]{valueY}[/]";
            series.columns.template.strokeWidth = 0;

            series.tooltip.pointerOrientation = "vertical";
            series.columns.template.column.cornerRadiusTopLeft = 10;
            series.columns.template.column.cornerRadiusTopRight = 10;
            series.columns.template.column.fillOpacity = 0.8;
            // on hover, make corner radiuses bigger
            var hoverState = series.columns.template.column.states.create("hover");
            hoverState.properties.cornerRadiusTopLeft = 0;
            hoverState.properties.cornerRadiusTopRight = 0;
            hoverState.properties.fillOpacity = 1;

            series.columns.template.adapter.add("fill", function(fill, target) {
                return chart.colors.getIndex(target.dataItem.index);
            });
            // Cursor
            chart.cursor = new am4charts.XYCursor();

        }); // end am4core.ready()
    }
}
export default  TopNProduct;