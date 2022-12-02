import React from 'react';
import { DatePicker, Button, Form, Skeleton, Select } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
const { Option } = Select;

import('./stockAlert.css');

class StockAlert extends React.Component {
    constructor(props) {
      super(props);
    }
    state = {
        loadingInline: false,
    }  
    componentDidMount() {
        //this.getAllBox();   
    }      
    componentWillUnmount() {   
    }

    stockFinish = fieldsValue => {
        console.log(fieldsValue)
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
                                    <Form.Item name="product" rules={[{ required: true }]}>
                                        <Select
                                            placeholder="Select Product"
                                            allowClear
                                            
                                            >
                                            <Option value="all">All</Option>
                                            <Option value="1">Product 1</Option>
                                            <Option value="2">Product 2</Option>
                                        </Select>
                                    </Form.Item>   
                                </div>
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
                                <div className="chartBox">
                                    <div className="hader text-center" style={{marginBottom: "10px"}}>STOCK ALERT</div>
                                    <div className="d-flex justify-content-center mainDashBoxes">
                                        <div className="flex-itm scr">
                                            <div className="widgBox">
                                                <div className="Numb " style={{color: "#2061d6"}}>0</div>
                                                <div className="text-center">
                                                <div>Total Products</div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="flex-itm scr">
                                            <div className="widgBox">
                                                <div className="Numb " style={{color: "#258836"}}>0</div>
                                                <div className="text-center">
                                                <div>Total Products</div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="flex-itm scr">
                                            <div className="widgBox" >
                                                <div className="Numb " style={{color: "#bb4d23"}}>0</div>
                                                <div className="text-center">
                                                <div>Total Value</div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="flex-itm scr">
                                            <div className="widgBox">
                                                <div className="Numb " style={{color: "#7d0859"}}>0</div>
                                                <div className="text-center">
                                                <div>Above Max</div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="flex-itm scr">
                                            <div className="widgBox">
                                                <div className="Numb " style={{color: "#3c148c"}}>0</div>
                                                <div className="text-center">
                                                <div>Below Min</div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="flex-itm scr">
                                            <div className="widgBox">
                                                <div className="Numb" style={{color: "#bb4d23"}}>0</div>
                                                <div className="text-center">
                                                <div>Below Max & Above Min</div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="flex-itm scr">
                                            <div className="widgBox">
                                                <div className="Numb" style={{color: "#b30909"}}>0</div>
                                                <div className="text-center">
                                                <div>Reorder</div>
                                                </div>
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
export default  StockAlert;