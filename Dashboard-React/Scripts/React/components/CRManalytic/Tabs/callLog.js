import React from 'react';
import { DatePicker, Button, Form, Table, Popover, Select, Tag, Modal } from 'antd';

import { SearchOutlined } from '@ant-design/icons';
import moment from 'moment';
import ExportReactCSV from '../../UIComponents/exportCSV';
const { Option } = Select;
import axios from'axios';


class CallLog extends React.Component {
    constructor(props) {
        super(props);
        this.state = { 
            loading:false,
            frmdate: '11-02-2020',
            callData :[],
            fromValues: [],
            modalLoader: false,
            detailsData: [],
            viewDetails: false
         }
    }
    componentDidMount() {
        axios({
            method: 'post',
            url: '../ajax/crmAnalytic/phoneCallCount.aspx/getPageloadCall',
            data: {}
          }).then(res => {
              console.log('watchlistDetails', res); 
              this.setState({
                  ...this.state,
                  frmdate:res.data.d.FromDateActHis 
              })    
          });
    }
    viewDetails = (e) => {
        console.log(e)
        const values = this.state.fromValues;
        axios({
            method: 'post',
            url: '../ajax/crmAnalytic/phoneCallCount.aspx/GetPhoneCountDetails',
            data: {
                frmdate: values.frmdate,
                todate: values.todate,
                smanId: e,
                ActivityType: values.ActivityType,
            }
          }).then(res => {
              console.log('details', res); 
              this.setState({
                  ...this.state,
                  viewDetails:true,
                  detailsData:res.data.d 
            })    
          });
    }
     SubmitCallcount = (fields) => {
        const values = {
            ...fields,
            'frmdate': fields['frmDate'].format('YYYY-MM-DD'),
            'todate': fields['toDate'].format('YYYY-MM-DD'),
            "ActivityType" : fields['status']
        }
        this.setState({...this.state,loading:true, fromValues:values});
        axios({
            method: 'post',
            url: '../ajax/crmAnalytic/phoneCallCount.aspx/GetPhoneCount',
            data: {
                frmdate: values.frmdate,
                todate: values.todate,
                ActivityType: values.ActivityType 
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
    render() { 
        const handleOk = () => {
            this.setState({
                ...this.state,
                viewDetails:false
            })
        }
        const dateFormat = 'DD-MM-YYYY';
        const rlworker = {
            frmDate: moment(this.state.frmdate, dateFormat),
            toDate: moment(new Date(), dateFormat),
            status: '3'
        };
        const columns = [
            {
              title: 'Salesman',
              dataIndex: 'SalesManName',
              key: 'SalesManName',
            },{
                title: 'Phone Call',
                dataIndex: 'CallCount',
                key: 'CallCount',
                align: 'center'
            },{
                title: 'Customer',
                dataIndex: 'custCount',
                key: 'custCount',
                align: 'center'
            },
            {
                title: 'Details',
                dataIndex: 'cnt_internalId',
                key: 'cnt_internalId',
                render: text =>(
                    <a onClick={() =>{this.viewDetails(text)}} 
                        className="detailsAcnchor"
                    >
                        <Tag color={'geekblue'} key={text}><SearchOutlined />{ 'View Details'}</Tag>
                    </a>
                ) 
            }
          ];
          const Dcolumns = [
            {
              title: 'Call Date',
              dataIndex: 'calldate',
              key: 'calldate',
              width: 120
            },{
                title: 'Note',
                dataIndex: 'note',
                key: 'note',
            }
          ];
          const headers = [
            {label: 'Salesman name', key: 'SalesManName'},
            {label: 'Phone Call', key: 'CallCount'},
            {label: 'Customer Count', key: 'custCount'},
            
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
                    {/* Details Modal */}
                    <Modal
                    className="noPadding-modal"
                    title="View Details"
                    visible={this.state.viewDetails}
                    onOk={handleOk}
                    cancelButtonProps={{ style: { display: 'none' } }}
                    width={700}
                    onCancel={handleOk}
                    >
                        <div className="colorTable">
                            <Table 
                            pagination={false}
                            loading={this.state.modalLoader}
                            dataSource={this.state.detailsData} 
                            columns={Dcolumns}  size="small"
                            scroll={{ y: 240 }} />
                        </div>
                    </Modal>
                    {/* Details Modal */}
                <div className="row">
                    <Form name="closedFollowupForm" 
                        onFinish={this.SubmitCallcount} 
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
                                {this.state.callData.length ? <ExportReactCSV csvData={this.state.callData} fileName="Callcount.csv" headers={headers} /> : null }
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
                            dataSource={this.state.callData} 
                            columns={columns}  size="small" />
                    </div>
                </div>
            </React.Fragment>
         );
    }
}

export default CallLog;