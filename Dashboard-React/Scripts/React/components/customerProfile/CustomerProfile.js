import React from 'react';
import axios from 'axios';
import ('./customerP.css');

//import Select from "react-select";
import ListRow from './listRow';

import { Table, Modal, Select } from 'antd';
const { Option } = Select;

const columns = [
    {
      title: 'Customer ID',
      dataIndex: 'CUSTOMER_ID',
      key: 'CUSTOMER_ID',
      width: 150,
    },
    {
      title: 'Customer Name',
      dataIndex: 'CUSTOMER',
      key: 'CUSTOMER',
      width: 150,
    },
    {
      title: 'Assigned Branch',
      dataIndex: 'ASSIGNEDBRANCH',
      key: 'ASSIGNEDBRANCH',
      width: 150,
    },
    {
        title: 'Assigned On',
        dataIndex: 'BRANCH_ASSIGNED_ON',
        key: 'BRANCH_ASSIGNED_ON',
        width: 150,
    },
    {
        title: 'Technician',
        dataIndex: 'ASSIGNEDTECHNICIAN',
        key: 'ASSIGNEDTECHNICIAN',
    },
    {
        title: 'Tech Assigned On',
        dataIndex: 'TECHNICIAN_ASSIGNED_ON',
        key: 'TECHNICIAN_ASSIGNED_ON',
        width: 180,
    },
    {
        title: 'Product Name',
        dataIndex: 'sProducts_Name',
        key: 'sProducts_Name',
        width: 150,
    },
    {
        title: 'Description',
        dataIndex: 'sProducts_Description',
        key: 'sProducts_Description',
    },
    {
        title: 'Code',
        dataIndex: 'sProducts_Code',
        key: 'sProducts_Code',
    },
    {
        title: 'Quantity',
        dataIndex: 'QUANTITY',
        key: 'QUANTITY',
    },
    {
        title: 'Schedule Date',
        dataIndex: 'SCHEDULE_DATE',
        key: 'SCHEDULE_DATE',
        width: 150,
    },
    {
        title: 'SCH Code',
        dataIndex: 'SCH_CODE',
        key: 'SCH_CODE',
        width: 150,
    },
    {
        title: 'Sch Status',
        dataIndex: 'SCH_STATUS',
        key: 'SCH_STATUS',
        width: 150,
    },
    {
        title: 'Segment 1',
        dataIndex: 'SEGMENT1',
        key: 'SEGMENT1',
        width: 150,
    },
    {
        title: 'Segment 2',
        dataIndex: 'SEGMENT2',
        key: 'SEGMENT2',
        width: 150,
    },
    {
        title: 'Service',
        dataIndex: 'SERVICE',
        key: 'SERVICE',
    },
    {
        title: 'Status',
        dataIndex: 'STATUS',
        key: 'STATUS',
    },
  ];

class CustomerProfile extends React.Component {
    constructor(props) {
        super(props);
      }
    state = {
            showDetails:false,
            dropOption :[],
            custValue: "", 
            dropPhone :[],
            phoneValue: "",
            dataMain: {
                Client_Category: "",
                Collection_Cordinator: "",
                Contract_date: "",
                Custom_Cordinator: "",
                Customer: "",
                Customer_Since: "",
                Email: "",
                Existing_Warrenty: "",
                Frequency: "",
                Info_On_Outgoing_Work_Like_Termite: "",
                Lead_Name: "",
                Main_Service_Brannch: "",
                Mobile_no: "",
                Next_Servcie_Schedule: "",
                No_Of_Bills_Monthly: "",
                No_Of_Service_Pointy: "",
                RATING: "",
                Sales_Person: "",
                Service_Completed_For_tHe_Month: "",
                Type_Of_Service: "",
                Value: "",
                cnt_internalId: "",
                contactperson_Email: "",
                contactperson_Name: "",
                contactperson_Phone: ""
            },
            serviceData:[]
          }
    componentDidMount(){
        
        axios({
            method: 'post',
            url: '../ajax/CustomerProfile/CustomerProfile.aspx/GetCustomerProfileSearch',
            data: {}
          }).then(async (res) => {
              console.log('GetCustomerProfileSearch', res)
              let CustData = res.data.d;
                this.setState({
                    dropOption:CustData,
                    //dropPhone: customerPhone
                })     
          });
          axios({
            method: 'post',
            url: '../ajax/CustomerProfile/CustomerProfile.aspx/GetCustomerProfileSearchPhone',
            data: {}
          }).then(async (res) => {
              console.log('GetCustomerProfileSearch', res)
              let CustPhone = res.data.d;
                this.setState({
                    dropPhone: CustPhone
                })     
          });
    }
    onSearchHandler = ()=>{
        const custV = this.state.custValue;
        const custP = this.state.phoneValue;
        let obj={}
        if(custV =='' && custP =='') {
            console.log("onSearchHandler", obj)
            return;
        }else if(custV !='' || custV !=null) {
            obj = {cntID : custV}
        }else {
            obj = {cntID : custP}
        }
        this.callFordata(obj)
    }
    callFordata = async (obj)=> {
        axios({
            method: 'post',
            url: '../ajax/CustomerProfile/CustomerProfile.aspx/GetCustomer',
            data: obj
          }).then(async (res) => {
              console.log('GetCustomer', res)
                await this.setState({
                    dataMain:res.data.d[0]
                });
                console.log('GetCustomer', this.state)
          });
    }
    callFordataDetails = async (id)=> {
        let objDetails = {cntID : id}
        axios({
            method: 'post',
            url: '../ajax/CustomerProfile/CustomerProfile.aspx/GetCustomerService',
            data: objDetails
          }).then(async (res) => {
              //console.log('GetCustomerService', res)
                await this.setState({
                    serviceData:res.data.d[0]
                });
          });
    }
    handleChangeCust = value => {
        this.setState({
            custValue:value
        });
      };
      handleChangePhone = value => {
        this.setState({
            phoneValue:value
        });
      };
      serviceDetailshandler = (e)=> {
          console.log(e)
          this.callFordataDetails(e)
        this.setState({
            showDetails:true
        });
      };
      handleOk =  () => {
        this.setState({
            showDetails:false
        });
      }
      // antd select
      handleChange = async (value)=> {
        await this.setState({
            custValue:value
        });
        console.log(this.state)
      }
      handleChangePhone = async (value)=> {
        await this.setState({
            phoneValue:value
        });
        console.log(this.state)
      }
    render() {
        const options = this.state.dropOption.map(d => <Option key={d.value}>{d.label}</Option>);
        const optionsPhone = this.state.dropPhone.map(d => <Option key={d.value}>{d.label}</Option>);
        return (
            <div>
                <div className="headerArea">
                    <h1>
                        <span className="pull-left backButton">
                            <a href="javascript:void(0);" onClick={()=> {this.props.changComponent('home')}}>
                            <i className="fa fa-arrow-left"></i></a>
                        </span>Customer Profile
                    </h1>
                </div>
                <div className="card">
                    <div className="card-body">
                        <div className="row">
                            <div className="col-sm-3">
                                <label>Select Customer</label>
                                <div>
                                <Select
                                    style={{ width: "100%" }}
                                    showSearch
                                    value={this.state.custValue}
                                    placeholder="Select customer"
                                    defaultActiveFirstOption={false}
                                    showArrow={true}
                                    filterOption={false}
                                    //onSearch={this.handleSearch}
                                    onChange={this.handleChange}
                                    filterOption={(input, option) =>
                                        option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                                    }
                                    allowClear={true}
                                >
                                    {options}
                                </Select>
                                </div>
                            </div>
                            <div className="col-sm-3">
                                <label>Select Phone</label>
                                <div>
                                <Select
                                    style={{ width: "100%" }}
                                    showSearch
                                    value={this.state.phoneValue}
                                    placeholder="Select Phone"
                                    defaultActiveFirstOption={false}
                                    showArrow={true}
                                    filterOption={false}
                                    //onSearch={this.handleSearch}
                                    onChange={this.handleChangePhone}
                                    filterOption={(input, option) =>
                                        option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                                    }
                                    allowClear={true}
                                >
                                    {optionsPhone}
                                </Select>
                                </div>
                                {/* <Select 
                                value={this.state.phoneValue} 
                                options={this.state.dropPhone}
                                onChange={this.handleChangePhone}
                                isClearable= {() => {this.setState({phoneValue:'0'})} } /> */}
                            </div>
                            <div className="col-sm-3">
                                <button type="button" className="btn btn-success" onClick={this.onSearchHandler}>Serach</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-4">
                        <div className="card">
                            <div className="card-body">
                                    <div className="d-flex flex-column align-items-center text-center">
                                            <img src="https://bootdey.com/img/Content/avatar/avatar7.png" alt="Admin" className="rounded-circle" width="150"/>
                                            <div className="mt-3">
                                            <h4 >{this.state.Customer}</h4>
                                            <p className="text-secondary mb-1" >{this.state.dataMain.cnt_internalId}</p>
                                            <p className="text-muted font-size-sm" > ---- -- --- --</p>
                                            <p className="text-muted font-size-sm" >--- ---</p>
                                            <p className="text-muted font-size-sm">Rating : <span >{this.state.RATING}</span></p>
                                            <button className="btn btn-primary" type="button" onClick={() =>{this.serviceDetailshandler(this.state.dataMain.cnt_internalId)}}>Service Status Details</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                    </div>
                    <div className="col-md-8">
                        <div className="card">
                            <div className="card-body">
                                <div className="row"><ListRow title="Full Name" value={this.state.dataMain.contactperson_Name} sSize="9"/></div>
                                <hr/>
                                <div className="row"><ListRow title="Email" value={this.state.dataMain.contactperson_Email} sSize="9"/></div>
                                <hr/>
                                <div className="row"><ListRow title="Phone" value={this.state.dataMain.contactperson_Phone} sSize="9" /></div>
                                <hr/>
                                <div className="row"><ListRow title="Customer Since" value={this.state.dataMain.Contract_date}  sSize="9" /></div>
                                <hr/>
                                <div className="row"><ListRow title="Client category" value={this.state.dataMain.Client_Category}  sSize="9" /></div>
                                <hr/>
                                <div className="row"><ListRow title="Contract date" value={this.state.dataMain.Contract_date}  sSize="9" /></div>
                                <hr/>
                                <div className="row"><ListRow title="Type of Service" value={this.state.dataMain.Type_Of_Service}  sSize="9" /></div>
                                <hr/>
                                <div className="row">
                                    <ListRow title="Frequency" value={this.state.dataMain.Frequency} sSize="3" />
                                    <ListRow title="Value" value={this.state.dataMain.Value} sSize="3" />
                                </div>
                                <hr/>
                                <div className="row">
                                    <ListRow title="Existing Warrenty" value={this.state.dataMain.Existing_Warrenty} sSize="3" />
                                    <ListRow title="Number of Bills Monthly" value={this.state.dataMain.No_Of_Bills_Monthly} sSize="3" />
                                </div>
                                <hr/>
                                <div className="row">
                                    <ListRow title="No of Service Points" value={this.state.dataMain.No_Of_Service_Pointy} sSize="3" />
                                    <ListRow title="Next Service Schedule" value={this.state.dataMain.Next_Servcie_Schedule} sSize="3" />
                                </div>
                                <hr/>
                                <div className="row">
                                    <ListRow title="Sales person" value={this.state.dataMain.Sales_Person} sSize="3" />
                                    <ListRow title="Customer Coordinator" value={this.state.dataMain.Custom_Cordinator} sSize="3" />
                                </div>
                                <hr/>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-sm-6">
                                <div className="card" style={{marginTop:'10px'}}>
                                    <div className="card-body">
                                        <div className="row">
                                            <div className="col-sm-6">
                                                <h6 className="mb-0">Main Service Branch</h6>
                                            </div>
                                            <div className="col-sm-6 text-secondary" >
                                            {this.state.dataMain.Main_Service_Brannch}
                                            </div>
                                        </div>
                                        <hr/>
                                        <div className="row">
                                            <div className="col-sm-6">
                                                <h6 className="mb-0">Collection Coordinator</h6>
                                            </div>
                                            <div className="col-sm-6 text-secondary" >
                                            {this.state.dataMain.Collection_Cordinator}
                                            </div>
                                        </div>
                                        <hr/>
                                        <div className="row">
                                            <div className="col-sm-6">
                                                <h6 className="mb-0">Customer Comunication Details</h6>
                                            </div>
                                            <div className="col-sm-6 text-secondary" >
                                                <button class="btn btn-info btn-xs">Click to see Details</button>
                                            </div>
                                        </div>
                                        <hr/>
                                        <div className="row">
                                            <div className="col-sm-6">
                                                <h6 className="mb-0">Bills and Dockets</h6>
                                            </div>
                                            <div className="col-sm-6 text-secondary" >
                                                <button class="btn btn-info btn-xs">Click to see Details</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="col-sm-6">
                                <div className="card" style={{marginTop:'10px'}}>
                                    <div className="card-body">
                                        <div className="row">
                                            <div className="col-sm-6">
                                                <h6 className="mb-0">Info on Outgoing work like Terminate</h6>
                                            </div>
                                            <div className="col-sm-6 text-secondary" >
                                                {this.state.dataMain.Info_On_Outgoing_Work_Like_Termite}
                                            </div>
                                        </div>
                                        <hr/>
                                        <div className="row">
                                            <div className="col-sm-6">
                                                <h6 className="mb-0">Service Completed for the Month</h6>
                                            </div>
                                            <div className="col-sm-6 text-secondary" >
                                                {this.state.dataMain.Service_Completed_For_tHe_Month}
                                            </div>
                                        </div>
                                        <hr/>
                                        <div className="row">
                                            <div className="col-sm-6">
                                                <h6 className="mb-0">Outstanding and Payment Received Details with Amount and Date</h6>
                                            </div>
                                            <div className="col-sm-6 text-secondary" >
                                                <button class="btn btn-info btn-xs">Click to see Details</button>
                                            </div>
                                        </div>
                                        <hr/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            
                <Modal title="Service Details" 
                visible={this.state.showDetails} 
                onOk={this.handleOk} 
                onCancel={this.handleOk}
                width={1200}>
                    <Table dataSource={this.state.serviceData} columns={columns} scroll={{ x: 1000 }} />;
                </Modal>
            </div>
            
        )
    }   
}

export default CustomerProfile;