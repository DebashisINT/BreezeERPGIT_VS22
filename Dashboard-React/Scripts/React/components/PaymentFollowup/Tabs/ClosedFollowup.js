import React from 'react';
import { DatePicker, Button, Form, Table, Popover } from 'antd';
import axios from 'axios';
import { SearchOutlined } from '@ant-design/icons';
import moment from 'moment';
import ExportReactCSV from '../../UIComponents/exportCSV';



class ClosedFollowup extends React.Component {
    constructor(props) {
        super(props);
        this.state = { 
            loading:false,
            closedData :[]
         }
    }
    SubmitClosedF = (fields) => {
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
             url: '../ajax/followUp/followup.aspx/getClosedFollowup',
             data: {
                fromdate: frm,
                todate: to
             }
           }).then(res => {
               console.log(res)
              this.setState({
                  ...this.state,
                  closedData: res.data.d,
                  loading:false, 
              })
           });
     }
    render() { 
        const dateFormat = 'DD-MM-YYYY';
        let date = new Date();
        date.setDate(date.getDate() - 30);
        //let dateString = date.toISOString().split('T')[0]; // "2020-06-08"
        const clworker = {
            frmDate: moment(date, dateFormat),
            toDate: moment(new Date(), dateFormat)
        };
        const columns = [
            {
              title: 'Closed by',
              dataIndex: 'user_name',
              key: 'user_name',
            },{
                title: 'Count',
                dataIndex: 'cnt',
                key: 'cnt',
                align: 'center',
              },{
                title: 'Ratio',
                dataIndex: 'ratio',
                key: 'ratio',
                align: 'center',
              }
          ];
        const content = (
            <div>
                <p>This report shows the Count of Total Followup Activities which are marked as 
                    Closed in the system for the selected from date and to date and it is Document-wise.</p>
                <p>Closed By: Shows the respective name who marked the follow up as 'Closed'.</p>
                <p>Count: Shows the total count of followup activities marked as Closed.</p>
                <p>Closed Followup : One new column 'Conversion Ratio', 
                    formula=(Activity wise total followup/activity wise closed followup)*100</p>
            </div>
        );
        const headers = [
            { label: "Closed by", key: "user_name" },
            { label: "Count", key: "cnt" },
            { label: "Ratio", key: "ratio" },

          ];
        return ( 
            <React.Fragment>
                <div className="row">
                    <Form name="closedFollowupForm" 
                        onFinish={this.SubmitClosedF} 
                        initialValues={clworker}>
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
                                {this.state.closedData.length ? <ExportReactCSV csvData={this.state.closedData} fileName="Closed Followup.csv" headers={headers} /> : null }
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
                            dataSource={this.state.closedData} 
                            columns={columns}  size="small" />
                    </div>
                </div>
            </React.Fragment>
         );
    }
}
 
export default ClosedFollowup;