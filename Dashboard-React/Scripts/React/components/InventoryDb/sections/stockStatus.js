import React from 'react';
import { DatePicker, Button, Form, Skeleton } from 'antd';
import { SearchOutlined } from '@ant-design/icons';

import axios from 'axios';

class StockStatus extends React.Component {
    constructor(props) {
      super(props);
    }
    state = {
        loadingInline: false,
        TOTALINVPROD: "0",
        TOTALPROD: "0",
        TOTALPRODIN: "0",
        TOTALPRODOUT: "0",
        TOTALPRODSC: "0",
        TOTALVAL: "0"
    }  
    componentDidMount() {
        //this.getAllBox();   
    }      
    componentWillUnmount() {   
    }

    stockFinish = fieldsValue => {
        this.setState({
            ...this.state,
            loadingInline: true
        })
        const values = {
            ...fieldsValue,
            'stockdate': fieldsValue['stock-date'].format('YYYY-MM-DD'),
            "a" : "hello"
        }
        console.log('Received values of form: ', values.stockdate);
        axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/GetInventoryBox',
            data: {
                date :values.stockdate
            }
          }).then(res => {
              console.log('GetInventoryBox', res);
              this.setState({
                  ...this.state,
                    TOTALINVPROD: res.data.d[0].TOTALINVPROD,
                    TOTALPROD: res.data.d[0].TOTALPROD,
                    TOTALPRODIN: res.data.d[0].TOTALPRODIN,
                    TOTALPRODOUT: res.data.d[0].TOTALPRODOUT,
                    TOTALPRODSC: res.data.d[0].TOTALPRODSC,
                    TOTALVAL: res.data.d[0].TOTALVAL,
                    loadingInline: false
              })
          });
    }
      render(){
        const config = {
            rules: [{ type: 'object', required: true, message: 'Please select Date!' }],
        };
        const dateFormat = 'DD-MM-YYYY';
          return(
              <div>
                    
                    <div className="tabInside">
                        <div className="row" style={{marginBottom: "10px"}}> 
                            <Form name="stock_controls" onFinish={this.stockFinish}>
                                <div className="col-md-3 col-sm-6">
                                    <label className="bold-lebel mTop0">Select Date</label>
                                    <Form.Item name="stock-date" {...config}>
                                        <DatePicker  format={dateFormat} />
                                    </Form.Item>   
                                </div>
                                <div className="col-md-3">
                                    <Form.Item>
                                        <Button type="primary" style={{marginTop: "26px"}} icon={<SearchOutlined />} htmlType="submit">Show Analysis</Button>
                                    </Form.Item>
                                </div>
                            </Form>
                        </div>
                        
                            {
                                this.state.loadingInline ? (
                                    <div className="row">
                                        <div className="col-md-12">
                                            <Skeleton active paragraph={{ rows: 4 }} loading={this.state.loadingInline} />
                                        </div>
                                    </div>
                                ) : (
                                    <div className="row">
                                        <div className="col-md-2">
                                            <div className="colorBox">
                                                <div className="infoWrp">
                                                    <i className="fa fa-inr fltd hide"></i>
                                                    <div className="nums">{this.state.TOTALINVPROD}</div>
                                                </div>
                                                <div className="mrLinks">
                                                    Total Items
                                                </div>
                                            </div>
                                        </div>
                                        <div className="col-md-2">
                                            <div className="colorBox" style={{background:"#309b60"}}>
                                                <div className="infoWrp">
                                                    <i className="fa fa-inr fltd hide"></i>
                                                    <div className="nums">{this.state.TOTALVAL}</div>
                                                </div>
                                                <div className="mrLinks">
                                                    Stock on Hand
                                                </div>
                                            </div>
                                        </div>
                                        <div className="col-md-2">
                                            <div className="colorBox" style={{background:"#3798c5"}}> 
                                                <div className="infoWrp">
                                                    <i className="fa fa-inr fltd hide"></i>
                                                    <div className="nums">{this.state.TOTALPROD}</div>
                                                </div>
                                                <div className="mrLinks">
                                                    Available for Sale
                                                </div>
                                            </div>
                                        </div>
                                        <div className="col-md-2">
                                            <div className="colorBox" style={{background:"#e68929"}}>
                                                <div className="infoWrp">
                                                    <i className="fa fa-inr fltd hide"></i>
                                                    <div className="nums">{this.state.TOTALPRODSC}</div>
                                                </div>
                                                <div className="mrLinks">
                                                    Commited for Sale
                                                </div>
                                            </div>
                                        </div>
                                        <div className="col-md-2">
                                            <div className="colorBox" style={{background:"#882626"}}>
                                                <div className="infoWrp">
                                                    <i className="fa fa-inr fltd hide"></i>
                                                    <div className="nums">{this.state.TOTALPRODIN}</div>
                                                </div>
                                                <div className="mrLinks">
                                                Items Qty IN
                                                </div>
                                            </div>
                                        </div>
                                        <div className="col-md-2">
                                            <div className="colorBox" style={{background:"#2646bb"}}>
                                                <div className="infoWrp">
                                                    <i className="fa fa-inr fltd hide"></i>
                                                    <div className="nums">{this.state.TOTALPRODOUT}</div>
                                                </div>
                                                <div className="mrLinks">
                                                    Items Qty OUT
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                ) 
                            }
                          
                    </div>
              </div>
        )
    }
}
export default  StockStatus;