import React, {lazy} from 'react';
import axios from 'axios';
import {Form, Button, DatePicker, Select, Skeleton} from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import('./kpiDB.css');
const { Option } = Select;


const Performance = lazy(() => import('./sections/performance')); 
const AsyncActivities = lazy(() => import('./sections/activities')); 
const EmpInfo = lazy(() => import('./sections/empInfo')); 
const Resolution = lazy(() => import('./sections/resolution')); 

class KpiDB extends React.Component {
    constructor(props) {
        super(props); 
        this.state = {
            loading: false,
            selectedDate: "",
            selectedEmployee: "",
            selectedBranch: "",
            activeModule: "performance",
            branchArr: [],
            empArr: [],
            performanceTop: {
                LDCNT: 0,
                LDTOTAMT: 0,
                INQCNT: 0,
                INQTOTAMT:0,
                QOCNT: 0,
                QOTOTAMT: 0,
                SOCNT: 0,
                SOTOTAMT: 0,
                SICNT: 0,
                SITOTAMT: 0,
                CRPCNT: 0,
                CRPTOTAMT: 0
            },
            leadStatus :[],
            InquiryBreakdown: [],
            QuotationBreakdown: [],
            OrderBreakdown: [],
            InvoiceBreakdown: [],
            activitiesTop: {
                ACTIVCNT: 0,
                CALLSMSCNT: 0,
                EMAILCNT: 0,
                OTHERSCNT: 0,
                SOCIALCNT: 0,
                VISITCNT: 0       
            },
            TransacVolume: [],
            TaskVolume: [],
            settings:{
                liActivities: true,
                liEmployeeInfo: true,
                liPerformance: true,
                liResolution: true
            },
            EmployeeData:{
                HALFDAYS: "0",
                LEAVES: "0",
                PRESENTS: "7",
                WORKINGDAYS: "0",
            }
          }
    }
    
    componentDidMount() {
        axios({method: 'post',url: '../ajax/KpiSummary/kpi.aspx/GetSettings', data: {}}).then(res => {
            console.log('sett', res)
        })
        Promise.all([this.getBranches(), this.getEmployees()])
            .then(function (results) {
                const branch = results[0];
                const emp = results[1];
                console.log(this.state)

                this.setState({
                    ...this.state,
                    branchArr: branch.data.d,
                    empArr:emp.data.d
                })
            }.bind(this));
    }
    getPerformance = ()=>{

    }
    // All performance call
    PerformanceTop = (payload)=>{
        return axios({method: 'post',url: '../ajax/KpiSummary/kpi.aspx/GetPerformanceTopBox', data: payload})
    }
    GetPLead = (payload)=>{
        return axios({method: 'post',url: '../ajax/KpiSummary/kpi.aspx/GetPLead', data: payload})
    }
    GetPSInv = (payload)=>{
        return axios({method: 'post',url: '../ajax/KpiSummary/kpi.aspx/GetPSInv', data: payload})
    }
    GetPOrder = (payload)=>{
        return axios({method: 'post',url: '../ajax/KpiSummary/kpi.aspx/GetPOrder', data: payload})
    }
    GetPQuotation = (payload)=>{
        return axios({method: 'post',url: '../ajax/KpiSummary/kpi.aspx/GetPQuotation', data: payload})
    }
    GetPInquery = (payload)=>{
        return axios({method: 'post',url: '../ajax/KpiSummary/kpi.aspx/GetPInquery', data: payload})
    }
    // branch and employee
    getBranches = ()=>{
        return axios({method: 'post',url: '../ajax/Service/General.asmx/GetBranch',data: {}})
    }
    getEmployees = ()=>{
        return axios({method: 'post',url: '../ajax/KpiSummary/kpi.aspx/GetEmplyee',data: {branchid: ''}})
    }
    kpiSearchFinish = fieldsValue => {
        
        let branch = fieldsValue['branch'] == undefined ? "" : fieldsValue['branch'];
        let employee = fieldsValue['employee'] == undefined ? "" : fieldsValue['employee'];
        //const date= fieldsValue
        const values = {
            ...fieldsValue,
            'date': fieldsValue['date'].format('YYYY-MM-DD'),
            branch: branch,
            employee: employee

        }
        this.setState({
            ...this.state,
            loading: true,
            selectedDate: values.date,
            selectedEmployee: values.employee,
            selectedBranch: values.branch
        })
        let obj = {
            ASONDATE:values.date,
            BRANCH_ID:values.branch,
            EMPID:values.employee
        }
        Promise.all(
            [
                this.PerformanceTop(obj), 
                this.GetPLead(obj), 
                this.GetPSInv(obj), 
                this.GetPOrder(obj), 
                this.GetPQuotation(obj), 
                this.GetPInquery(obj)
            ])
            .then(function (results) {
                const PerformanceTop = results[0].data.d;
                const GetPLead = results[1].data.d;
                const GetPSInv = results[2].data.d;
                const GetPOrder = results[3].data.d;
                const GetPQuotation = results[4].data.d;
                const GetPInquery = results[5].data.d;
             
                console.log('GetPLead',GetPLead);
                console.log('GetPSInv',GetPSInv);
                console.log('GetPOrder',GetPOrder);
                console.log('GetPQuotation',GetPQuotation);
                console.log('GetPInquery',GetPInquery)
                this.setState({
                    ...this.state,
                    leadStatus :GetPLead,
                    InquiryBreakdown: GetPSInv,
                    QuotationBreakdown:GetPOrder,
                    OrderBreakdown:GetPQuotation,
                    InvoiceBreakdown: GetPInquery,
                    performanceTop: {
                        LDCNT: PerformanceTop.one[0].LDCNT,
                        LDTOTAMT: PerformanceTop.one[0].LDTOTAMT,
                        INQCNT:  PerformanceTop.two[0].INQCNT,
                        INQTOTAMT: PerformanceTop.two[0].INQTOTAMT,
                        QOCNT: PerformanceTop.three[0].QOCNT,
                        QOTOTAMT: PerformanceTop.three[0].QOTOTAMT,
                        SOCNT: PerformanceTop.four[0].SOCNT,
                        SOTOTAMT: PerformanceTop.four[0].SOTOTAMT,
                        SICNT: PerformanceTop.five[0].SICNT,
                        SITOTAMT: PerformanceTop.five[0].SITOTAMT,
                        CRPCNT: PerformanceTop.six[0].CRPCNT,
                        CRPTOTAMT: PerformanceTop.six[0].CRPTOTAMT
                    },
                    loading: false,
                    //activeModule:'performance',
                })
            }.bind(this));  
    }
    getForm = () => {
        console.log(this.state.selectedEmployee)
    }
    render() {
        function onChange(value) {
        console.log(`selected ${value}`);
        }
        
        function onBlur() {
        console.log('blur');
        }
        
        function onFocus() {
        console.log('focus');
        }
        
        function onSearch(val) {
        console.log('search:', val);
        }
      const config = {
          rules: [{ type: 'object', required: true, message: 'Please select Date!' }],
      };
      const dateFormat = 'DD-MM-YYYY';
      //let brancList= "";
      let content = null;
      if( this.state.activeModule == "performance"){
        content = <Performance 
        top={this.state.performanceTop}
        leadStatus={this.state.leadStatus}
        InquiryAr={this.state.InquiryBreakdown}
        QuotationBreakdown={this.state.QuotationBreakdown}
        OrderBreakdown={this.state.OrderBreakdown}
        InvoiceBreakdown={this.state.InvoiceBreakdown}
         />
      }else if(this.state.activeModule == "Activities"){
        content = <AsyncActivities 
                    top={this.state.activitiesTop} 
                    TaskVolume={this.state.TaskVolume}
                    TransacVolume={this.state.TransacVolume} />
      }else if(this.state.activeModule == "EmpInfo"){
            content = <EmpInfo data={this.state.EmployeeData} />
      }else if(this.state.activeModule == "Resolution"){
          content = <Resolution />
      }
      
        return (
            <div>
               <div className="headerArea">
                        <h1>
                            <span className="pull-left backButton">
                                <a href="javascript:void(0);" onClick={()=> {this.props.changComponent('home')}}>
                                <i className="fa fa-arrow-left"></i></a>
                            </span>KPI Summary 
                        </h1>
                    </div>
                    <div className="">
                        <div className="cardContainer mt-3">
                            <div className="cardContent">
                                <div className="boxSelection">
                                <div className="row"> 
                                    <Form name="kpiSearchControl"  onFinish={this.kpiSearchFinish}>
                                        <div className="col-md-2 col-sm-3">
                                            <label className="bold-lebel mTop0">Select Date</label>
                                            <Form.Item name="date" {...config}>
                                                <DatePicker  
                                                  //defaultValue={moment(new Date(), dateFormat)} 
                                                  ref={this.googleInput}
                                                  format={dateFormat} />
                                            </Form.Item> 
                                        </div>
                                        <div className="col-md-3 col-sm-3">
                                            <label className="bold-lebel mTop0">Select Branch</label>
                                            <Form.Item name="branch">
                                                <Select
                                                    showSearch
                                                    defaultValue="all"
                                                    allowClear
                                                    optionFilterProp="children"
                                                    onChange={onChange}
                                                    onFocus={onFocus}
                                                    onBlur={onBlur}
                                                    onSearch={onSearch}
                                                    filterOption={(input, option) =>
                                                    option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                                                    }
                                                    >
                                                    {
                                                        this.state.branchArr.length ? (
                                                            this.state.branchArr.map(br => {
                                                                return <Option value={br.ID}>{br.NAME}</Option>
                                                            })
                                                        ): null
                                                    }
                                                </Select>
                                            </Form.Item> 
                                        </div>
                                        <div className="col-md-3 col-sm-3">
                                            <label className="bold-lebel mTop0">Select Employee</label>
                                            <Form.Item name="employee">
                                                <Select
                                                    showSearch
                                                    defaultValue="all"
                                                    allowClear
                                                    optionFilterProp="children"
                                                    filterOption={(input, option) =>
                                                    option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                                                    }
                                                    >
                                                    {
                                                        this.state.empArr.length ? (
                                                            this.state.empArr.map(emp => {
                                                                return <Option value={emp.EMP_CODE}>{emp.EMP_NAME}</Option>
                                                            })
                                                        ): null
                                                    }
                                                </Select>
                                            </Form.Item>    
                                        </div>
                                        <div className="col-md-3">
                                            <Form.Item>
                                                <Button type="primary" style={{marginTop: "26px"}} icon={<SearchOutlined />} htmlType="submit">Search</Button>
                                            </Form.Item>
                                        </div>
                                    </Form>
                                </div>
                                </div>
                                <div className="clearfix" style={{marginTop: "15px"}}>
                                  <ul className="tabluLer clearfix" id="nav-tab">
                                      {
                                          this.state.settings.liPerformance ? (
                                            <li id="liPerformance" className={this.state.activeModule =="performance" ? "active" : ""} onClick={() => {this.loadComponent('performance')}}>
                                                <div className="text-center">
                                                    <span className="ic"><i className="fa fa-bolt"></i></span>
                                                </div>
                                                <div className="tb_txt">Performance</div>
                                            </li>
                                          ) : null
                                      }
                                      {
                                          this.state.settings.liActivities ? (
                                            <li className={this.state.activeModule =="Activities" ? "active" : ""} 
                                            onClick={() => {this.loadComponent('Activities')}}>
                                              <div className="text-center">
                                                  <span className="ic"><i className="fa fa-tasks"></i></span>
                                              </div>
                                              <div className="tb_txt">Activities</div>
                                           
                                          </li>
                                          ) : null
                                      }
                                      {
                                          this.state.settings.liEmployeeInfo ? (
                                            <li className={this.state.activeModule =="EmpInfo" ? "active" : ""}
                                                onClick={() => {this.loadComponent('EmpInfo')}}>  
                                                <div className="text-center">
                                                    <span className="ic"><i className="fa fa-users"></i></span>
                                                </div>
                                                <div className="tb_txt">Employee Info</div>
                                            </li>
                                          ) : null
                                      }
                                      {
                                          this.state.settings.liResolution ? (
                                            <li className={this.state.activeModule =="Resolution" ? "active" : ""}
                                                onClick={() => {this.loadComponent('Resolution')}} >
                                                    <div className="text-center">
                                                        <span className="ic"><i className="fa fa-database"></i></span>
                                                    </div>
                                                    <div className="tb_txt">Resolution</div>
                                                </li>
                                          ) : null
                                      }
                                    </ul>
                                </div>
                                {/* tab end */}
                                <div className="clearfix">
                                {
                                    this.state.loading ? <Skeleton  active /> : content   
                                }
                                
                                        
                                </div>

                            </div>
                        </div>
                    </div>
            </div>
        );
    }
    loadComponent = (value) => {
       
        this.setState({
            ...this.state,
            activeModule: value
        });
        console.log('brforeData', this.state)
        if(!this.state.selectedDate == ''){
            if(value == 'Activities'){
               
                this.setState({
                    ...this.state,
                    loading:true,
                    activeModule:'Activities'
                })
                let obj = {
                    ASONDATE: this.state.selectedDate,
                    EMPID: this.state.selectedEmployee,
                    BRANCH_ID: this.state.selectedBranch
                }
                Promise.all([this.GetActivitiesBox(obj), this.GetTransacVolume(obj), this.GetTaskVolume(obj)])
                    .then(function (results) {
                        console.log(results)
                        this.setState({
                            ...this.state,
                            loading:false,
                            activitiesTop: results[0].data.d[0],
                            TransacVolume: results[1].data.d,
                            TaskVolume: results[2].data.d
                        })
                    }.bind(this));
            }
        }
        if(value == 'Resolution'){
            this.setState({
                ...this.state,
                activeModule: "Resolution"
            });
        }
        if(value == 'EmpInfo'){
            this.setState({
                ...this.state,
                loading:true,
                activeModule:'EmpInfo'
            })
            let obj = {
                ASONDATE: this.state.selectedDate,
                EMPID: this.state.selectedEmployee
            }
            axios({method: 'post',url: '../ajax/KpiSummary/kpi.aspx/GetEmployeeTab', data: obj}).then(res => {
                console.log('EmployeeData', res)
                this.setState({
                    ...this.state,
                    loading:false,
                    EmployeeData: res.data.d[0]
                })
            })
        }
        
    }
    GetActivitiesBox = (payload) =>{
        return axios({method: 'post',url: '../ajax/KpiSummary/kpi.aspx/GetActivitiesBox', data: payload})
    }
    GetTransacVolume = (payload) =>{
        return axios({method: 'post',url: '../ajax/KpiSummary/kpi.aspx/GetTransacVolume', data: payload})
    }
    GetTaskVolume = (payload) =>{
        return axios({method: 'post',url: '../ajax/KpiSummary/kpi.aspx/GetTaskVolume', data: payload})
    }
}

export default KpiDB;