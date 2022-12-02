import React from 'react';
import { DatePicker, Button, Form, Table, Popover } from 'antd';
import axios from 'axios';
import { SearchOutlined } from '@ant-design/icons';
import moment from 'moment';
import ExportReactCSV from '../../UIComponents/exportCSV';



class PendingFollowup extends React.Component {
    constructor(props) {
        super(props);
        this.state = { 
            loading:false,
            pendigData :[]
         }
    }
    SubmitPendingF = (fields) => {
         let frm = fields['sledtedFrom'] == undefined ? "" : fields['sledtedFrom'].format('YYYY-MM-DD');
         //const date= fieldsValue
         console.log(fields)
         axios({
             method: 'post',
             url: '../ajax/followUp/followup.aspx/getPendingFollowup',
             data: {
                 fromdate: frm
             }
           }).then(res => {
               console.log(res)
              this.setState({
                  ...this.state,
                  pendigData:res.data.d
              })
           });
     }
    render() { 
        const dateFormat = 'DD-MM-YYYY';
        const worker = {
            sledtedFrom: moment(new Date(), dateFormat),
        };
        const columns = [
            {
              title: 'Customer',
              dataIndex: 'name',
              key: 'name',
            },{
                title: 'Pending Followup',
                dataIndex: 'cnt',
                key: 'cnt',
                align: 'center',
              }
          ];
        const content = (
            <div>
                <p>This report shows the Count of Total Pending Activities. Pending Activities means 
                  those activities which are having 'Next Activity Date' &lt;= 'As On Date'.</p>
                <p>Customer:Shows the list of customer not completed task for next activity date.</p>
                <p>Pending Followup:Shows the count of total pending followup for the respective customer.</p>
            </div>
        );
        const headers = [
            { label: "Customer", key: "name" },
            { label: "Pending Followup", key: "cnt" }
          ];
        return ( 
            <React.Fragment>
                <div className="row">
                    <Form name="pendingFollowupForm" 
                        onFinish={this.SubmitPendingF} 
                        initialValues={worker}>
                        <div className="col-md-2">
                            <label>From</label>
                            <Form.Item name="sledtedFrom">
                                <DatePicker format={dateFormat}/>
                            </Form.Item>   
                        </div> 
                        <div className="col-md-6">
                            <Button type="primary" 
                                style={{marginTop: "26px"}} 
                                icon={<SearchOutlined />} 
                                htmlType="submit">Generate</Button>
                                {this.state.pendigData.length ? <ExportReactCSV csvData={this.state.pendigData} fileName="pendingFollowup.csv" headers={headers} /> : null }
                                <Popover content={content} title="Information" trigger="hover" style={{width: "25%"}}>
                                    <span style={{display:"inline-block", cursor:"pointer", marginLeft: "10px"}}><i className="fa fa-question-circle"></i></span>
                                </Popover>  
                        </div> 
                    </Form>
                </div>
                <div className="row">
                    <div className="col-md-12 colorTable">
                        <Table 
                            dataSource={this.state.pendigData} 
                            columns={columns}  size="small" />
                    </div>
                </div>
            </React.Fragment>
         );
    }
}
 
export default PendingFollowup;