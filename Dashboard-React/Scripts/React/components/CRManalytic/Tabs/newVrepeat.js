import React from 'react';
import { DatePicker, Button, Form, Table, Popover, Select } from 'antd';

import { SearchOutlined } from '@ant-design/icons';
import moment from 'moment';
import ExportReactCSV from '../../UIComponents/exportCSV';
const { Option } = Select;
import axios from 'axios';
class NewVsRepeat extends React.Component {
    constructor(props) {
        super(props);
        this.state = { 
            loading:false,
            frmdate: '07-02-2020',
            tblData :[]
         }
    } 
    componentDidMount() {
       
    }
    SubmitVisitCount = (fields) => {
         const values = {
            ...fields,
            'frmdate': fields['frmDate'].format('YYYY-MM-DD'), 
            "ActivityType" : fields['status']
        }
        axios({
            method: 'post',
            url: '../ajax/crmAnalytic/newVrepeat.aspx/GetNewVrepeat',
            data: {
                fromdate: values.frmdate,
                prodClass: values.ActivityType 
            }
          }).then(res => {
             console.log('prodClass', res);
             this.setState({
                 ...this.state,
                 tblData: res.data.d
             })
          });
     }
    render() { 
        const dateFormat = 'DD-MM-YYYY';
        const rlworker = {
            frmDate: moment(new Date(), dateFormat),
            status: '1'
        };
        const columns = [
            {
              title: 'New',
              dataIndex: 'New',
              key: 'New',
              align: 'center',
            },{
                title: 'Repeat',
                dataIndex: 'Regular',
                key: 'Regular',
                align: 'center',
            },{
                title: 'Salesman Name',
                dataIndex: 'SalesManName',
                key: 'SalesManName',
            }
          ];
          const headers = [
            {label: 'New', key: 'New'},
            {label: 'Repeat', key: 'Regular'},
            {label: 'Salesman Name', key: 'SalesManName'} 
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
                            <label>Status</label>
                            <Form.Item name="status">
                            <Select>
                                <Option value="1">Product wise</Option>
                                <Option value="2">Class wise</Option>
                                </Select>
                            </Form.Item>   
                        </div>   
                        <div className="col-md-6">
                            <Button type="primary" 
                                style={{marginTop: "26px"}} 
                                icon={<SearchOutlined />} 
                                htmlType="submit">Generate</Button>
                                {this.state.tblData.length ? <ExportReactCSV csvData={this.state.tblData} fileName="NewVSrepeat.csv" headers={headers} /> : null }
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
                            dataSource={this.state.tblData} 
                            columns={columns}  size="small" />
                    </div>
                </div>
            </React.Fragment>
         );
    }
}

export default NewVsRepeat;