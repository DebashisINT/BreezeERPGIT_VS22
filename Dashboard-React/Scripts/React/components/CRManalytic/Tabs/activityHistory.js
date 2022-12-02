import React from 'react';
import { DatePicker, Button, Form, Table, Popover, Select, Input, Modal } from 'antd';

import { SearchOutlined } from '@ant-design/icons';
import moment from 'moment';
import ExportReactCSV from '../../UIComponents/exportCSV';
const { Option } = Select;
const { Column, ColumnGroup } = Table;
import axios from 'axios';


class ActivityHistory extends React.Component {
    constructor(props) {
        super(props);
        this.state = { 
            loading:false,
            frmdate: '11-02-2020',
            visitCount :[],
            visible: false,
            salesmanData: [],
            salesanKeys: [],
            salesmanSelected: {
                EmpCode: "",
                EmpName: "",
                id: ""
            },
         }
    }
   
    componentDidMount() {
        // axios({
        //     method: 'post',
        //     url: '../ajax/crmAnalytic/crmAnalytic.aspx/getPageload',
        //     data: {}
        //   }).then(res => {
        //       console.log('watchlistDetails', res); 
        //       this.setState({
        //           ...this.state,
        //           frmdate:res.data.d.FromDateActHis 
        //       })    
        //   });
    }
    SubmitVisitCount = (fields) => {
        const smid = this.state.salesmanSelected.EmpCode;
         const values = {
            ...fields,
            'frmdate': fields['frmDate'].format('YYYY-MM-DD'),
            'todate': fields['toDate'].format('YYYY-MM-DD'),
            "ActivityType" : fields['status'],
            "his" : fields['his'],
        }
        console.log(values, smid)
        axios({
            method: 'post',
            url: '../ajax/crmAnalytic/crmAnalytic.aspx/GetCampaignCost',
            data: {
                frmdate: values.frmdate, todate: values.todate, 
                smanId: smid, LastCount:values.his, ActivityType: values.ActivityType 
            }
          }).then(res => {
             console.log('Getdata', res);
             
          });
     }
     handleCancel = () =>{
        this.setState({
            ...this.state,
            visible: false,
        });
    }
    salesmanSearch = (field)=>{
        console.log('search', field.searchSalesman)
        axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/GetEmployee',
            data: {
                SerarchKey: field.searchSalesman
            }
          }).then(res => {
             console.log('GetEmployee', res);
             let data = res.data.d;
             const arrayOfObj = data.map(({ id: key, ...rest }) => ({ key, ...rest }));
             this.setState({
                 ...this.state,
                 salesmanData: arrayOfObj
             })
          });
    }
    getSalesman = (e) => {
        console.log('tr', this.state)
        this.setState({
            ...this.state,
            visible: true,
          });
        console.log('tr', this.state)
    }
    clearSalesman = (e)=> {
        console.log(e);
        this.setState({
            ...this.state,
            salesmanSelected:{}
        })
    }
    render() { 
        const fromdate = this.state.frmdate;
        const dateFormat = 'DD-MM-YYYY';
        const rlworker = {
            frmDate: moment(fromdate, dateFormat),
            toDate: moment(new Date(), dateFormat),
            status: '3',
            his: '1',

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
        const rowSelection = {
            onChange: (selectedRowKeys, selectedRows) => {
              console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows);
              this.setState({
                  ...this.state,
                  salesanKeys: selectedRowKeys,
                  salesmanSelected: selectedRows[0],
              })
              console.log('to state', this.state)
            }
        }
        const Scolumns = [
            {
              title: 'Slaesman Name',
              dataIndex: 'EmpName',
            },
            {
              title: 'Unique Id',
              dataIndex: 'EmpCode',
              width: 250
            }
          ];
        const salesmanV = this.state.salesmanSelected.EmpName;
        console.log(salesmanV)
        return ( 
            <React.Fragment>
                <Modal
                        title="Salesman search"
                        visible={this.state.visible}
                        onOk={this.handleCancel}
                        className={'searchModal'}
                        width={1000}
                        onCancel={this.handleCancel}
                        >
                        <div className="searchCustForm">
                            <Form name="projectMSearch" 
                                onFinish={this.salesmanSearch} 
                            
                                >
                                <Form.Item name="searchSalesman">
                                    <Input placeholder="Search Customer" id="data1" className="searchCust" />
                                </Form.Item>
                            </Form>
                        </div>
                        <div className="colorTable">
                            <Table
                                rowSelection={{
                                type: 'radio',
                                    ...rowSelection,
                                }}
                                scroll={{ y: 300 }}
                                pagination={false}
                                columns={Scolumns}
                                dataSource={this.state.salesmanData}
                                size="small"
                            />
                        </div>
                    </Modal>
                <div className="row">
                    <Form name="actHistryForm" 
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
                            <label>Activity Status</label>
                            <Form.Item name="status">
                            <Select>
                                <Option value="3">Future Sale</Option>
                                <Option value="5">Clarification Required</Option>
                                <Option value="1">Document Collection</Option>
                                <Option value="2">Closed</Option>
                                </Select>
                            </Form.Item>   
                        </div> 
                        <div className="col-md-2">
                            <label>No of History</label>
                            <Form.Item name="his">
                                <Input />
                            </Form.Item>   
                        </div> 
                        <div className="col-md-2">
                            <label>SalesMan</label>
                            
                                <Input 
                                    placeholder="Select Salesman"
                                    name="salesman"
                                    type="text" 
                                    onChange={(e) => {this.clearSalesman(e)}}                                          
                                    value={salesmanV}
                                    onClick={this.getSalesman} 
                                    //allowClear
                                    />
                             
                        </div> 
                        <div className="col-md-2">
                            <Button type="primary" 
                                style={{marginTop: "26px"}} 
                                icon={<SearchOutlined />} 
                                htmlType="submit">Search</Button>
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
                            bordered
                            scroll={{ x: 1500 }}
                            size="small" >
                                <ColumnGroup title="Activity Details">
                                    <Column title="Salesman" dataIndex="Salesman" key="Salesman" />
                                    <Column title="Customer" dataIndex="Customer" key="Customer" />
                                    <Column title="Product/Class" dataIndex="Class" key="Class" />
                                    <Column title="Budget" dataIndex="Budget" key="Budget" />
                                </ColumnGroup>
                                <ColumnGroup title="Outcome 1">
                                    <Column title="Date" dataIndex="Date" key="Date" />
                                    <Column title="Outcome" dataIndex="Outcome" key="Outcome" />
                                    <Column title="Remarks" dataIndex="Remarks" key="Remarks" />
                                </ColumnGroup>
                                <ColumnGroup title="Feedback 1">
                                    <Column title="FeedBack" dataIndex="FeedBack" key="FeedBack" />
                                    <Column title="Date" dataIndex="fdate" key="fdate" />
                                </ColumnGroup>
                            </Table>
                    </div>
                </div>
            </React.Fragment>
         );
    }
}

export default ActivityHistory;