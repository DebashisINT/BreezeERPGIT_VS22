import React from 'react';
import { DatePicker, Button, Form, Table, Popover, Select } from 'antd';

import { SearchOutlined } from '@ant-design/icons';
import moment from 'moment';
import ExportReactCSV from '../../UIComponents/exportCSV';
const { Option } = Select;
import axios from 'axios';


class VisitLog extends React.Component {
    constructor(props) {
        super(props);
        this.state = { 
            loading:false,
            frmdate: '07-02-2020',
            visitCount :[]
         }
    }
   
    componentDidMount() {
        axios({
            method: 'post',
            url: '../ajax/crmAnalytic/visitCount.aspx/getPageloadCall',
            data: {}
          }).then(res => {
              console.log('watchlistDetails', res); 
              this.setState({
                  ...this.state,
                  frmdate:res.data.d.FromDateActHis 
              })    
          });
    }
    SubmitVisitCount = (fields) => {
         const values = {
            ...fields,
            'frmdate': fields['frmDate'].format('YYYY-MM-DD'),
            'todate': fields['toDate'].format('YYYY-MM-DD'),
            "ActivityType" : fields['status']
        }
        axios({
            method: 'post',
            url: '../ajax/crmAnalytic/visitCount.aspx/getVisitCount',
            data: {
                frmdate: values.frmdate,
                todate: values.todate,
                ActivityType: values.ActivityType 
            }
          }).then(res => {
              console.log('getVisitCount', res);
             this.setState({
                 ...this.state,
                 visitCount: res.data.d
             })
          });
     }
    render() { 
        const fromdate = this.state.frmdate;
        const dateFormat = 'DD-MM-YYYY';
        const rlworker = {
            frmDate: moment(fromdate, dateFormat),
            toDate: moment(new Date(), dateFormat),
            status: '3'
        };
        const columns = [
            {
              title: 'Salesman Name',
              dataIndex: 'SalesManName',
              key: 'SalesManName',
              
            },{
                title: 'Sales Visit',
                dataIndex: 'SvCount',
                key: 'SvCount',
                align: 'center',
            },{
                title: 'Customer Count',
                dataIndex: 'custCount',
                key: 'custCount',
                align: 'center',
            }
          ];
          const headers = [
            {label: 'Salesman Name', key: 'SalesmanName'},
            {label: 'Sales Visit', key: 'SvCount'},
            {label: 'Customer Count', key: 'custCount'} 
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
                    <Form name="closedFollowupForm" 
                        onFinish={this.SubmitVisitCount} 
                        initialValues={rlworker}>
                        <div className="col-md-2">
                            <label>From</label>
                            <Form.Item name="frmDate">
                                <DatePicker format={dateFormat}/>
                            </Form.Item>   
                        </div>
                        <div className="col-md-2">
                            <label>To</label>
                            <Form.Item name="toDate">
                                <DatePicker format={dateFormat}/>
                            </Form.Item>   
                        </div>
                        <div className="col-md-2">
                            <label>Status</label>
                            <Form.Item name="status">
                            <Select>
                                <Option value="3">Future Sale</Option>
                                <Option value="5">Clarification Required</Option>
                                <Option value="1">Document Collection</Option>
                                <Option value="2">Closed</Option>
                                </Select>
                            </Form.Item>   
                        </div>   
                        <div className="col-md-6">
                            <Button type="primary" 
                                style={{marginTop: "26px"}} 
                                icon={<SearchOutlined />} 
                                htmlType="submit">Generate</Button>
                                {this.state.visitCount.length ? <ExportReactCSV csvData={this.state.visitCount} fileName="Visit.csv" headers={headers} /> : null }
                                <Popover  content={content} title="Information" trigger="hover" style={{width: "25%"}}>
                                    <span style={{display:"inline-block", cursor:"pointer", marginLeft: "10px"}}><i className="fa fa-question-circle"></i></span>
                                </Popover>  
                        </div> 
                    </Form>
                </div>
                <div className="row">
                    <div className="col-md-12 colorTable">
                        <Table 
                            loading={this.state.loading}
                            dataSource={this.state.visitCount} 
                            columns={columns}  size="small" />
                    </div>
                </div>
            </React.Fragment>
         );
    }
}

export default VisitLog;