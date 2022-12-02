import React from 'react';
import { DatePicker, Button, Form, Table, Popover } from 'antd';
import axios from 'axios';
import { SearchOutlined } from '@ant-design/icons';
import moment from 'moment';
import ExportReactCSV from '../../UIComponents/exportCSV';



class RelationRatio extends React.Component {
    constructor(props) {
        super(props);
        this.state = { 
            loading:false,
            frmdate: '10-04-2020',
            RelationData :[]
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
             url: '../ajax/followUp/followup.aspx/getRationuc',
             data: {
                fromdate: frm,
                todate: to
             }
           }).then(res => {
               console.log(res)
              this.setState({
                  ...this.state,
                  RelationData: res.data.d,
                  loading:false, 
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
              title: 'Customer',
              dataIndex: 'name',
              key: 'name',
            },{
                title: 'Ratio',
                dataIndex: 'Ratio',
                key: 'Ratio',
                align: 'center',
              }
          ];
          const headers = [
            { label: "Customer", key: "name" },
            { label: "Ratio", key: "Ratio" },

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
                                {this.state.RelationData.length ? <ExportReactCSV csvData={this.state.RelationData} fileName="Customer Relation RAtio.csv" headers={headers} /> : null }
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
                            dataSource={this.state.RelationData} 
                            columns={columns}  size="small" />
                    </div>
                </div>
            </React.Fragment>
         );
    }
}
 
export default RelationRatio;