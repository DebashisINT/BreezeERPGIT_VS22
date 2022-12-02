import React, {Component } from 'react';
import { Table } from 'antd';
import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
import axios from 'axios';
class ProjectDetails extends Component {
    constructor(props) {
        super(props);
        this.barChartInit = this.barChartInit.bind(this);
        this.state = { 
            details: true,
            subData: []
        } 
    } 

    getInfoProject = (id) => {
      const obj = this.props.formData;
      axios({
        method: 'post',
        url: '../ajax/projectManagement/projDb.aspx/GetProjDetailSingle',
        data: {
            toDate: obj.toDate,
            SearchKey:obj.SearchKey,
            partyId: obj.partyId,
            Pid: id
        }
      }).then(res => {
          console.log('GetProjDetailSingle', res)  
          let arr =[];
          arr.push(res.data.d[0])
          this.setState({
            subData: arr
          })
      }); 
    }
    render() { 
        const pieData = this.props.pieData;
        const barData = this.props.barData;
       
        if(pieData != null){
          let result = Object.keys(pieData).map(function (key) {
            return {
                "project": key,
                "value": pieData[key]
              };
          });
          this.pieChartInit(result);
        }
        if(barData != null) {
          this.barChartInit(barData)
        }
        
        return ( 
            <React.Fragment>
               <div className="row">
                    <div className="col-md-6">
                        <div className="chartBox relative">
                            {/* <div className="pull-right " onClick={(e) => {this.makeFullScreen(e)}}>
                                 <i className="fa fa-arrows-alt"></i>
                            </div>  */}
                            <div className="hader">Projects Stagewise</div>  
                            <div id="chartdiv" style={{height: "414px"}}></div>
                        </div>
                    </div>
                    <div className="col-md-6">
                        <div className="chartBox relative">
                            {/* <div className="pull-right tt">
                                 <i className="fa fa-arrows-alt"></i>
                            </div> */}
                            <div className="hader">Project Details</div>
                            <div id="chartdiv2" style={{height: "280px"}}></div>
                            <div className="stageIndicator">
                                <ul className="clearfix">
                                    <li><span className="indi color1"></span><span>New</span></li>
                                    <li><span className="indi color2"></span><span>Qualify</span></li>
                                    <li><span className="indi color3"></span><span>Planning</span></li>
                                    <li><span className="indi color4"></span><span>Execution</span></li>
                                    <li><span className="indi color5"></span><span>Deliver</span></li>
                                    <li><span className="indi color6"></span><span>Complete</span></li>
                                    <li><span className="indi color7"></span><span>Close</span></li>                                                 
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
                {
                    this.state.details ? (
                        <div className="row" style={{marginTop: "15px"}}>
                            <div className="col-md-12">
                                <div className="colorTable relative">
                                    <div className="hader">Project Details</div>
                                    <Table
                                        dataSource={this.state.subData} 
                                        columns={columns}
                                        size="small"
                                        scroll={{ x: 1300 }}
                                     />
                                </div>
                            </div>
                        </div>
                    ): null
                }
                
            </React.Fragment>
         );
    }
    
    barChartInit = (data) =>{
        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end  
        // Create chart instance
        var chart = am4core.create("chartdiv2", am4charts.XYChart);
        // Add data
        chart.data = data;
        // Create axes
        var yAxis = chart.yAxes.push(new am4charts.CategoryAxis());
        yAxis.dataFields.category = "Proj_Name";
        yAxis.renderer.grid.template.location = 0;
        yAxis.renderer.labels.template.fontSize = 10;
        yAxis.renderer.minGridDistance = 10;
        yAxis.cursorTooltipEnabled = false;
        var xAxis = chart.xAxes.push(new am4charts.ValueAxis());
        // Create series
        var series = chart.series.push(new am4charts.ColumnSeries());
        var columnTemplate = series.columns.template;
        series.dataFields.valueX = "STAGESTATUS";
        series.dataFields.categoryY = "Proj_Name";
        columnTemplate.tooltipText = "";
        columnTemplate.strokeWidth = 0;
        columnTemplate.fill = am4core.color("#a55");

        series.columns.template.column.cornerRadiusTopRight = 10;
        series.columns.template.column.cornerRadiusBottomRight = 10;
        series.columns.template.events.on("hit", function (ev) {
            this.getInfoProject(ev.target.dataItem.dataContext.Proj_Id);
        }, this);
        columnTemplate.adapter.add("fill", function (fill, target) {
            if (target.dataItem && (target.dataItem.valueX == null)) {
                return am4core.color("#474747");
            } else if (target.dataItem && (target.dataItem.valueX == 83.33)) {
                return am4core.color("#32C47E");
            } else if (target.dataItem && (target.dataItem.valueX == 66.67)) {
                return am4core.color("#6871DC");
            }
            else if (target.dataItem && (target.dataItem.valueX == 50.00)) {
                return am4core.color("#D8F1E2");
            }
            else if (target.dataItem && (target.dataItem.valueX == 33.33)) {
                return am4core.color("#6286A3");
            }
            else if (target.dataItem && (target.dataItem.valueX == 0.00)) {
                return am4core.color("#A35E5E");
            }
            else {
                return fill;
            }
        });
        // Add ranges
        function addRange(label, start, end, color) {
            var range = yAxis.axisRanges.create();
            range.category = start;
            range.endCategory = end;
            range.label.text = label;
            range.label.disabled = false;
            range.label.fill = color;
            range.label.location = 0;
            range.label.dx = -130;
            range.label.dy = 12;
            range.label.fontWeight = "bold";
            range.label.fontSize = 12;
            range.label.horizontalCenter = "left"
            range.label.inside = true;
            range.grid.stroke = am4core.color("#396478");
            range.grid.strokeOpacity = 1;
            range.tick.length = 200;
            range.tick.disabled = false;
            range.tick.strokeOpacity = 0.6;
            range.tick.stroke = am4core.color("#396478");
            range.tick.location = 0;

            range.locations.category = 1;
        }
        chart.cursor = new am4charts.XYCursor();
        chart.logo.disabled=true;
    }
    // Pie chart
    pieChartInit = (data) =>{
      am4core.ready(function () {
        // Themes begin
          am4core.useTheme(am4themes_animated);     
          // Create chart instance
          var chart = am4core.create("chartdiv", am4charts.PieChart);
          // Add and configure Series
          var pieSeries = chart.series.push(new am4charts.PieSeries());
          pieSeries.dataFields.value = "value";
          pieSeries.dataFields.category = "project";
          // Let's cut a hole in our Pie chart the size of 30% the radius
          chart.innerRadius = am4core.percent(30);
          // Put a thick white border around each Slice
          pieSeries.slices.template.stroke = am4core.color("#fff");
          pieSeries.slices.template.strokeWidth = 2;
          pieSeries.slices.template.strokeOpacity = 1;
          pieSeries.slices.template
          // change the cursor on hover to make it apparent the object can be interacted with
          .cursorOverStyle = [
              {
                  "property": "cursor",
                  "value": "pointer"
              }
          ];
          pieSeries.alignLabels = false;
          pieSeries.labels.template.bent = true;
          pieSeries.labels.template.radius = 3;
          pieSeries.labels.template.padding(0, 0, 0, 0);
          pieSeries.labels.template.text = "";
          pieSeries.slices.template.tooltipText = "{category}: {value}";      
          pieSeries.ticks.template.disabled = true;       
          // Create a base filter effect (as if it's not there) for the hover to return to
          var shadow = pieSeries.slices.template.filters.push(new am4core.DropShadowFilter);
          shadow.opacity = 0;
          // Create hover state
          var hoverState = pieSeries.slices.template.states.getKey("hover"); // normally we have to create the hover state, in this case it already exists
          // Slightly shift the shadow and make it more prominent on hover
          var hoverShadow = hoverState.filters.push(new am4core.DropShadowFilter);
          hoverShadow.opacity = 0.7;
          hoverShadow.blur = 5;
          // Add a legend
          chart.legend = new am4charts.Legend();
          pieSeries.legendSettings.valueText = "{ }";
          pieSeries.legendSettings.labelText = "{category}: {value}";
          pieSeries.colors.list = [
          am4core.color("#5ccd86"),
          am4core.color("#6771dc"),
          am4core.color("#ee4040"),
          am4core.color("#FF9671"),
          am4core.color("#FFC75F"),
          am4core.color("#F9F871"),
          ];
          chart.data = data;
      }); // end am4core.ready()
    }
}					 							
  const columns = [
    {title: 'Code', dataIndex: 'PROJECT_CODE', key: 'PROJECT_CODE',},
    {title: 'Name', dataIndex: 'PROJECT_NAME', key: 'PROJECT_NAME',},
    {title: 'Party Id', dataIndex: 'PARTYID', key: 'PARTYID',},
    {title: 'Manager', dataIndex: 'PROJECT_MANAGER', key: 'PROJECT_MANAGER'},
    {title: 'Stage', dataIndex: 'PROJECTSTAGE', key: 'PROJECTSTAGE'},
    {title: 'Customer Name', dataIndex: 'CUSTOMERNAME', key: 'CUSTOMERNAME'},
    {title: 'Actual Start date', dataIndex: 'PROJ_ACTUALSTARTDATE', key: 'PROJ_ACTUALSTARTDATE'},
    {title: 'Estimated Start date', dataIndex: 'PROJ_ESTIMATESTARTDATE', key: 'PROJ_ESTIMATESTARTDATE'},
    {title: 'Actual End date', dataIndex: 'PROJ_ACTUALENDDATE', key: 'PROJ_ACTUALENDDATE'},
    {title: 'Est. Hours', dataIndex: 'PROJ_ESTIMATEHOURS', key: 'PROJ_ESTIMATEHOURS'},
    {title: 'Actual Hours', dataIndex: 'PROJ_ACTUALHOURS', key: 'PROJ_ACTUALHOURS'},
    {title: 'Est. Labour Cost', dataIndex: 'PROJ_ESTLABOURCOST', key: 'PROJ_ESTLABOURCOST'},
    {title: 'Actual Labour Cost', dataIndex: 'PROJ_ACTUALLABOURCOST', key: 'PROJ_ACTUALLABOURCOST'},
    {title: 'Est. Total Cost', dataIndex: 'PROJ_ESTTOTALCOST', key: 'PROJ_ESTTOTALCOST'},
  ];

export default ProjectDetails;