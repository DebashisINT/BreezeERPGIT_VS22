import React from 'react';
import { DatePicker, Button, Form, Table, Popover } from 'antd';
import axios from 'axios';
import { SearchOutlined } from '@ant-design/icons';
import moment from 'moment';
import ExportReactCSV from '../../UIComponents/exportCSV';
import { convertToDate } from '../../helpers/helperFunction'


class FollowupHistory extends React.Component {
    constructor(props) {
        super(props);
        this.state = { 
            loading:false,
            frmdate: '10-04-2020',
            historyData :[]
         }
    }
    componentDidMount() {
        axios({
            method: 'post',
            url: '../ajax/followUp/followup.aspx/getPageload',
            data: {}
          }).then(res => {
            console.log('res', res)
            this.setState({
                ...this.state,
                frmdate: res.data.d.allFormDate,
            })
            console.log(this.state)
          });
    }
    convertToDate = (x)=>{
        let a = Date(x)
        return new Date(a).toLocaleDateString()
     }
    SubmitRelation = (fields) => {
         let frm = fields['frmDate'] == undefined ? "" : fields['frmDate'].format('YYYY-MM-DD');
         let to = fields['toDate'] == undefined ? "" : fields['toDate'].format('YYYY-MM-DD');
         //const date= fieldsValue
         console.log(frm, to)
         this.setState({
             ...this.state,
             loading:true
         })
         
         axios({
             method: 'post',
             url: '../ajax/followUp/followup.aspx/getfollowpHistory',
             data: {
                frmdate: frm,
                todate: to
             }
           }).then(res => {
               let response = res.data.d;

               if(response.length > 0){
                const newArr = [];
                response.map(item => {
                    let DocDate = convertToDate(item.DocDate);
                    let FollowDate = convertToDate(item.FollowDate);
                    let NextFollowDate = convertToDate(item.NextFollowDate);
                    let obj = { 
                         "DocDate":DocDate,
                         "Document": item.Document,
                         "FollowDate": FollowDate,
                         "FollowUsing": item.FollowUsing,
                         "NextFollowDate": NextFollowDate,
                         "Remarks": item.Remarks,
                         "id": item.id,
                         "name": item.name,
                         "openClsoe": item.openClsoe,
                         "user_name": item.user_name 
                     }
                     newArr.push(obj)
                })
                    console.log('newArr', newArr)
                    this.setState({
                        ...this.state,
                        historyData: newArr,
                        loading:false, 
                    })
               }else{
                    this.setState({
                        ...this.state,
                        historyData: res.data.d,
                        loading:false, 
                    })
               } 
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
              title: 'Followup By',
              dataIndex: 'user_name',
              key: 'user_name',
            },{
                title: 'Followed on',
                dataIndex: 'FollowDate',
                key: 'FollowDate',
            },{
                title: 'Using',
                dataIndex: 'FollowUsing',
                key: 'FollowUsing',
            },
            {
                title: 'Document',
                dataIndex: 'Document',
                key: 'Document'
            },
            {
                title: 'Date',
                dataIndex: 'DocDate',
                key: 'DocDate'
            },
            {
                title: 'Customer',
                dataIndex: 'name',
                key: 'name'
            },
            {
                title: 'Status',
                dataIndex: 'openClsoe',
                key: 'openClsoe'
            },
            {
                title: 'Next Followup Date',
                dataIndex: 'NextFollowDate',
                key: 'NextFollowDate'
            },
            {
                title: 'Remarks',
                dataIndex: 'Remarks',
                key: 'Remarks'
            }
          ];
          const headers = [
            {label: 'Followup By', key: 'user_name'},
            {label: 'Followed on', key: 'FollowDate'},
            {label: 'Using', key: 'FollowUsing'},
            {label: 'Document', key: 'Document'},
            {label: 'Date', key: 'DocDate'},
            {label: 'Customer', key: 'name'},
            {label: 'Status', key: 'openClsoe'},
            {label: 'Next Followup Date', key: 'NextFollowDate'},
            {label: 'Remarks', key: 'Remarks'}
          ];
        const content = (
            <div>
                <p>This report shows the Count of Total Pending Activities. Pending Activities means 
                    those activities which are having 'Next Activity Date' &#60; &#61; 'As On Date' and it is Document-wise.</p>
                <p><b>Salesmen:</b> Shows the list of salesmen not completed task for next activity date.</p>
                <p><b>Activities:</b> Shows the count of total pending task for the assigned salesmen in the given period.</p>
            </div>
        );
        
        return ( 
            <React.Fragment>
                <div className="row">
                    <Form name="closedFollowupForm" 
                        onFinish={this.SubmitRelation} 
                        initialValues={rlworker}>
                        <div className="col-md-2">
                            <label>From</label>
                            <Form.Item name="frmDate">
                                <DatePicker format={dateFormat}/>
                            </Form.Item>   
                        </div>
                        <div className="col-md-2">
                            <label>From</label>
                            <Form.Item name="toDate">
                                <DatePicker format={dateFormat}/>
                            </Form.Item>   
                        </div>  
                        <div className="col-md-6">
                            <Button type="primary" 
                                style={{marginTop: "26px"}} 
                                icon={<SearchOutlined />} 
                                htmlType="submit">Generate</Button>
                                {this.state.historyData.length ? <ExportReactCSV csvData={this.state.historyData} fileName="Followup History.csv" headers={headers} /> : null }
                                <Popover content={content} title="Information" trigger="hover" style={{width: "25%"}}>
                                    <span style={{display:"inline-block", cursor:"pointer", marginLeft: "10px"}}><i className="fa fa-question-circle"></i></span>
                                </Popover>  
                        </div> 
                    </Form>
                </div>
                <div className="row">
                    <div className="col-md-12 colorTable">
                        <Table 
                            loading={this.state.loading}
                            dataSource={this.state.historyData} 
                            scroll={{ x: 1300}}
                            columns={columns}  size="small" />
                    </div>
                </div>
            </React.Fragment>
         );
    }
}
 
export default FollowupHistory;