import React from 'react';
import { DatePicker, Button, Form, Table, Popover, Select, Drawer } from 'antd';

import { SearchOutlined, CloseCircleOutlined  } from '@ant-design/icons';
import moment from 'moment';
import ExportReactCSV from '../../UIComponents/exportCSV';
const { Option } = Select;
import axios from'axios';

import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";


class PendingActivities extends React.Component {
    constructor(props) {
        super(props);
        this.state = { 
            loading:false,
            frmdate: '07-10-2020',
            callData :[],
            fromValues: [],
            modalLoader: false,
            detailsData: [],
            viewDetails: false,
            drawer:false
         }
    }
    componentDidMount() {
        axios({
            method: 'post',
            url: '../ajax/crmAnalytic/pendingActivities.aspx/getPageloadCall',
            data: {}
          }).then(res => {
              console.log('getPageloadCall', res); 
              this.setState({
                  ...this.state,
                  frmdate:res.data.d.FromDateActHis 
              })    
          });
    }
    
     SubmitCallcount = (fields) => {
        const values = {
            ...fields,
            'frmdate': fields['frmDate'].format('YYYY-MM-DD'),
            'status': fields['status'],
        }
        this.setState({...this.state,loading:true, fromValues:values});
        axios({
            method: 'post',
            url: '../ajax/crmAnalytic/pendingActivities.aspx/GetPendingAct',
            data: {
                asOnDate: values.frmdate,
                ActivityState: values.status
            }
          }).then(res => {
              console.log('', res);
             this.setState({
                 ...this.state,
                 callData: res.data.d,
                 loading:false
             })
          });
     }
     onClose = () =>{
         this.setState({
             ...this.state,
             drawer: false
         })
     }

    render() { 
        const showChart = ()=>{
            this.setState({
                ...this.state,
                drawer: true
            })
            this.getChart(this.state.callData);
        }
        const dateFormat = 'DD-MM-YYYY';
        const rlworker = {
            frmDate: moment(this.state.frmdate, dateFormat),
            status: '0'
        };
        const columns = [
            {
              title: 'Salesman',
              dataIndex: 'SalesManName',
              key: 'SalesManName',
            },{
                title: 'Activities',
                dataIndex: 'ActCount',
                key: 'ActCount'
            }
          ];
          const headers = [
            {label: 'Salesman name', key: 'SalesManName'},
            {label: 'Activities', key: 'ActCount'}
          ];
        const content = (
            <div>
                <p>This report shows the Count of Total Pending Activities.<br /> Pending Activities means 
                    those activities which are <br />having 'Next Activity Date' &#60; &#61; 'As On Date'   
                    and it is Document-wise.</p>
                <p><b>Salesmen:</b> Shows the list of salesmen not <br />  completed task for next activity date.</p>
                <p><b>Activities:</b> Shows the count of total pending <br /> 
                 task for the assigned salesmen in the given period.</p>
            </div>
        );
        
        return ( 
            <React.Fragment>
                <div className="row">
                    <Form name="closedpendingact" 
                        onFinish={this.SubmitCallcount} 
                        initialValues={rlworker}>
                        <div className="col-md-2">
                            <label>From</label>
                            <Form.Item name="frmDate">
                                <DatePicker format={dateFormat}/>
                            </Form.Item>   
                        </div>
                        <div className="col-md-2">
                            <label>Status</label>
                            <Form.Item name="status">
                            <Select>
                                <Option value="0">All</Option>
                                <Option value="3">Future Sale</Option>
                                <Option value="5">Clarification Required</Option>
                                <Option value="1">Document Collection</Option>
                                </Select>
                            </Form.Item>   
                        </div>  
                        <div className="col-md-8 clearfix relative">
                            <Button type="primary" 
                                style={{marginTop: "26px"}} 
                                icon={<SearchOutlined />} 
                                htmlType="submit">Generate</Button>
                                {this.state.callData.length ? (
                                    <ExportReactCSV csvData={this.state.callData} 
                                        fileName="Pending Activities.csv" 
                                        headers={headers} /> 
                                    ): null }
                                <Popover  content={content} title="Information" trigger="hover" style={{width: "25%"}}>
                                    <span style={{display:"inline-block", cursor:"pointer", marginLeft: "10px"}}><i className="fa fa-question-circle"></i></span>
                                </Popover>  
                                {this.state.callData.length ? (
                                    <div className="chartBtn" onClick={showChart}>
                                        <div><img src="../assests/images/DashboardIcons/graphic.png" 
                                        style={{
                                            width: "22px",
                                            marginRight: "5px"
                                        }}
                                        alt="" /> Chart View</div>
                                    </div>
                                ) : null }
                        </div> 
                    </Form>
                </div>
                <div className="row">
                    <div className="col-md-12 colorTable">
                        <Table 
                            loading={this.state.loading}
                            dataSource={this.state.callData} 
                            columns={columns}  size="small" />
                    </div>
                </div>

                <Drawer
                    height={530}
                    placement="top"
                    closable={false}
                    onClose={this.onClose}
                    visible={this.state.drawer}
                    getContainer={false}
                    > 
                        <div>
                            <div className="chartHeader">Pending Activity Salesmen wise</div>
                            <div id="chartPending" style={{height: "430px"}}></div>
                            <div className="chartClose" onClick={this.onClose}><CloseCircleOutlined /></div>
                        </div>
                    </Drawer>               
            </React.Fragment>
         );
    }
    getChart = (data) =>{
        console.log(data);
        am4core.ready(function() {
        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end
        // Create chart instance
        var chart = am4core.create("chartPending", am4charts.XYChart3D);
        // Add data
        chart.data = data;
        // Create axes
        let categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
        categoryAxis.dataFields.category = "SalesManName";
        categoryAxis.renderer.labels.template.rotation = 270;
        categoryAxis.renderer.labels.template.hideOversized = false;
        categoryAxis.renderer.minGridDistance = 20;
        categoryAxis.renderer.labels.template.horizontalCenter = "right";
        categoryAxis.renderer.labels.template.verticalCenter = "middle";
        categoryAxis.tooltip.label.rotation = 270;
        categoryAxis.tooltip.label.horizontalCenter = "right";
        categoryAxis.tooltip.label.verticalCenter = "middle";

        let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
        valueAxis.title.text = "SalesManName";
        valueAxis.title.fontWeight = "bold";
        // Create series
        var series = chart.series.push(new am4charts.ColumnSeries3D());
        series.dataFields.valueY = "ActCount";
        series.dataFields.categoryX = "SalesManName";
        series.name = "ActCount";
        series.tooltipText = "{categoryX}: [bold]{valueY}[/]";
        series.columns.template.fillOpacity = .8;
        var columnTemplate = series.columns.template;
        columnTemplate.strokeWidth = 2;
        columnTemplate.strokeOpacity = 1;
        columnTemplate.stroke = am4core.color("#FFFFFF");
        columnTemplate.adapter.add("fill", function(fill, target) {
        return chart.colors.getIndex(target.dataItem.index);
        })
        columnTemplate.adapter.add("stroke", function(stroke, target) {
        return chart.colors.getIndex(target.dataItem.index);
        })
        chart.cursor = new am4charts.XYCursor();
        chart.cursor.lineX.strokeOpacity = 0;
        chart.cursor.lineY.strokeOpacity = 0;
    }); // end am4core.ready()
    }
}
export default PendingActivities;