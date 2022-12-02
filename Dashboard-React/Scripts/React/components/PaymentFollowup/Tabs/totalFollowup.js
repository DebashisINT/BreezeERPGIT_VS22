import React from 'react';
import { DatePicker, Button, Form, Table, Popover } from 'antd';
import axios from 'axios';
import { SearchOutlined } from '@ant-design/icons';
import moment from 'moment';
import ExportReactCSV from '../../UIComponents/exportCSV';
class TotalFollowup extends React.Component {
    constructor(props) {
        super(props);
        this.state = { 
            frmdate: '10-04-2020', 
            todate: '10-09-2020',
            maxdate: '', 
            mindate: '',
            totalFollowup: []
         }
    }
    componentDidMount () {
        axios({
            method: 'post',
            url: '../ajax/followUp/followup.aspx/getPageload',
            data: {}
          }).then(res => {
            console.log('res', res)
            this.setState({
                ...this.state,
                frmdate: res.data.d.allFormDate,
                todate: res.data.d.alltoDate,
            })
            console.log(this.state)
          });
    }
    SubmitTotalF = (fields) => {
        console.log(this.state)
         let frm = fields['sledtedFrom'] == undefined ? "" : fields['sledtedFrom'].format('YYYY-MM-DD');
         let to = fields['toD'] == undefined ? ""  : fields['toD'].format('YYYY-MM-DD');
         //const date= fieldsValue
         console.log(frm, to)
         axios({
             method: 'post',
             url: '../ajax/followUp/followup.aspx/getGrid',
             data: {
                 fromdate: frm,
                 todate: to
             }
           }).then(res => {
              this.setState({
                  ...this.state,
                  totalFollowup:res.data.d
              })
           });
     }

    render(){
        const config = {
            rules: [{ type: 'object', required: false, message: 'Please select Date!' }],
        };
        const columns = [
            {
              title: 'Customer',
              dataIndex: 'name',
              key: 'name',
            },
            {
              title: 'Followup By',
              dataIndex: 'user_name',
              key: 'user_name',
            },
            {
              title: 'Document',
              dataIndex: 'Document',
              key: 'Document',
            },{
                title: 'Count',
                dataIndex: 'cnt',
                key: 'cnt',
              }
          ];
          const content = (
            <div>
              <p>This report shows the Count of Total Followup Activities. Show data in the given from date and to date and it is Document-wise.</p>
              <p>Customer: Shows the respective customer for which follow up done.</p>
              <p>Follow up by: Shows the name who entered the followup entry in the system.</p>
              <p>Document: Shows the respective document/blank based on selection for which follow up done.</p>
              <p>Count: Shows the total count of followup done for Respective customer and selected document</p>
            </div>
          );
        const dateFormat = 'DD-MM-YYYY';
        const worker = {
            sledtedFrom: moment(this.state.frmdate, dateFormat),
            toD: moment(new Date())
          };
        return ( 
            <React.Fragment>
                <div className="row">
                    <Form name="ttlFlo" 
                        onFinish={this.SubmitTotalF} 
                        initialValues={worker}>
                    <div className="col-md-2">
                        <label>From</label>
                        <Form.Item name="sledtedFrom">
                            <DatePicker format={dateFormat}/>
                        </Form.Item>   
                    </div> 
                    <div className="col-md-2">
                        <label>To</label>
                        <Form.Item name="toD"  >
                            <DatePicker  format={dateFormat} />
                        </Form.Item> 
                    </div>
                    <div className="col-md-6">
                        <Button type="primary" 
                            style={{marginTop: "26px"}} 
                            icon={<SearchOutlined />} 
                            htmlType="submit">Generate</Button>
                            {this.state.totalFollowup.length ? <ExportReactCSV csvData={this.state.totalFollowup} fileName="totalFollowup.csv" /> : null }
                            <Popover content={content} title="Information" trigger="hover" style={{width: "25%"}}>
                                <span style={{display:"inline-block", cursor:"pointer", marginLeft: "10px"}}><i className="fa fa-question-circle"></i></span>
                            </Popover>  
                    </div> 
                    </Form>
                </div>
                <div className="row">
                    <div className="col-md-12 colorTable">
                        <Table 
                            dataSource={this.state.totalFollowup} 
                            columns={columns}  size="small" />
                    </div>
                </div>
            </React.Fragment>
         );
    }
    
}
 
export default TotalFollowup;