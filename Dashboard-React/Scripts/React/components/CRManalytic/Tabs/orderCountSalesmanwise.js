import React from 'react';
import { DatePicker, Button, Form, Table, Popover, Select,  } from 'antd';

import { SearchOutlined } from '@ant-design/icons';
import moment from 'moment';
import ExportReactCSV from '../../UIComponents/exportCSV';
const { Option } = Select;
import axios from'axios';


class OrderCountSalesman extends React.Component {
    constructor(props) {
        super(props);
        this.state = { 
            loading:false,
            frmdate: '06-10-2020',
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
            url: '../ajax/crmAnalytic/orderCountSalesmanwise.aspx/getPageloadCall',
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
            'todate': fields['toDate'].format('YYYY-MM-DD'),
        }
        this.setState({...this.state,loading:true, fromValues:values});
        axios({
            method: 'post',
            url: '../ajax/crmAnalytic/orderCountSalesmanwise.aspx/GetPendingAct',
            data: {
                frmDate: values.frmdate,
                todate: values.todate
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
        
        const dateFormat = 'DD-MM-YYYY';
        const rlworker = {
            frmDate: moment(this.state.frmdate, dateFormat),
            toDate: moment(new Date(), dateFormat)
        };
        const columns = [
            {
              title: 'Salesman',
              dataIndex: 'SalesManName',
              key: 'SalesManName',
            },{
                title: 'Invoiced Order(s)',
                dataIndex: 'TotCount',
                key: 'TotCount'
            }
          ];
          const headers = [
            {label: 'Salesman name', key: 'SalesManName'},
            {label: 'Invoiced Order(s)', key: 'TotCount'},
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
                        <div className="col-md-6">
                            <Button type="primary" 
                                style={{marginTop: "26px"}} 
                                icon={<SearchOutlined />} 
                                htmlType="submit">Generate</Button>
                                {this.state.callData.length ? <ExportReactCSV csvData={this.state.callData} fileName="Order count salesmanwise.csv" headers={headers} /> : null }
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

export default OrderCountSalesman;