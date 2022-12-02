import React from 'react';
import axios from 'axios';
import { Modal, Button, Space } from 'antd';

import TopBox from './section/topBox';
import('./ManagementNotification.css');

class ManagementNotification extends React.Component {
    constructor(props) {
      super(props);
    }
    state = {
        customer: 0,
        vendor: 0,
        employee: 0,
        influencer: 0,
        transporter: 0,
        details: [],
        activeComp: "",
        showDetails: false
    }  
    componentDidMount() {
        this.getAllBox();
    }      
    componentWillUnmount() {   
    }
    getAllBox = () =>{
        axios({
            method: 'post',
            url: '../ajax/mngNotification/mngNotification.aspx/GetAllNotificationData',
            data: {
                action :"ALL"
            }
          }).then(res => {
              console.log('GetAllNotificationData', res); 
              this.setState({
                    ...this.state,
                        customer: res.data.d[0].TOTCUST,
                        vendor: res.data.d[0].TOTVEND,
                        employee: res.data.d[0].TOTEMP,
                        influencer: res.data.d[0].CNTINF,
                        transporter: res.data.d[0].CNTTRANS,
                });
              console.log(this.state)
          });
    }
    sendEmail = (email) =>{
        console.log(email);
        Modal.warning({
            title: 'Email is not Cofigured',
            content: 'email funtionality is not available right at the moment',
          });
    }
    sendSms = (sms) =>{
        console.log(sms);
        Modal.warning({
            title: 'SMS is not Cofigured',
            content: 'SMS funtionality is not available right at the moment',
          });
    }
    boxClicked = (value) =>{
        console.log(value)
        switch(value) {
            case "Customer":
                this.getCustomer();
              break;
            case "VENDOR":
                this.getVendor();
              break;
            case "EMPLOYEE":
                this.getEmployee();
              break;
            case "INFLUENCER":
                this.getInfluencer();
              break;
            case "TRANSPORTER":
                this.getTransporter();
              break;
            default:
                this.getCustomer();
          }
    }
      render(){
          return(
              <div>
                    <div className="headerArea">
                        <h1>
                            <span className="pull-left backButton">
                                <a href="javascript:void(0);" onClick={()=> {this.props.changComponent('home')}}>
                                <i className="fa fa-arrow-left"></i></a>
                            </span>Management Notification 
                        </h1>
                    </div>
                    <div className="row">
                        <div className="col-md-12">
                            <div className="cardContainer mt-3">
                                <div className="cardContent">
                                    <div className="row">
                                        <div className="col-md-12">
                                            <div class="BoxTypeGrey">
                                                <div class="clearfix">
                                                    <div class="flex-row  align-items-center">
                                                        <TopBox 
                                                            Title="Customer" 
                                                            smtl="Today"
                                                            value={this.state.customer}
                                                            bgColor="#565454"
                                                            clicked={this.boxClicked}
                                                            addClass={this.state.activeComp =="Cust" ? "active" : ""}
                                                        ></TopBox>
                                                        <TopBox 
                                                        Title="VENDOR" 
                                                        smtl="Today"
                                                        value={this.state.vendor}
                                                        bgColor="#f76d6d"
                                                        clicked={this.boxClicked}   
                                                        addClass={this.state.activeComp =="Vend" ? "active" : ""}
                                                        ></TopBox>
                                                        <TopBox 
                                                        Title="EMPLOYEE" 
                                                        smtl="Todays"
                                                        value={this.state.employee}
                                                        bgColor="#6e98f5"
                                                        clicked={this.boxClicked}  
                                                        addClass={this.state.activeComp =="emp" ? "active" : ""} 
                                                        ></TopBox>
                                                        <TopBox 
                                                        Title="INFLUENCER" 
                                                        smtl="Today"
                                                        value={this.state.influencer}
                                                        bgColor="#44c8cd"
                                                        clicked={this.boxClicked}  
                                                        addClass={this.state.activeComp =="Influ" ? "active" : ""} 
                                                        ></TopBox>
                                                        <TopBox 
                                                        Title="TRANSPORTER" 
                                                        smtl="Today"
                                                        value={this.state.transporter}
                                                        bgColor="#9db53c"
                                                        clicked={this.boxClicked} 
                                                        addClass={this.state.activeComp =="Trans" ? "active" : ""}
                                                        ></TopBox>
                                                    </div>
                                                </div>
                                                {
                                                                this.state.showDetails ? (
                                                <div className="clearfix" style={{marginTop:"10px"}}>
                                                    <div className="shadowBox">
                                                        <div className="bigHeading">Details</div>
                                                        <div className="table-responsive">
                                                            
                                                                
                                                            <table className="table styledTble">
                                                                <thead>
                                                                <tr>
                                                                    <th>Name </th>
                                                                    <th>Company </th>
                                                                    <th>Event Type</th>
                                                                    <th>SMS </th>
                                                                    <th>Email</th>
                                                                </tr>
                                                                </thead>
                                                                <tbody>
                                                                    {
                                                                            this.state.details.length ? (                                                                              
                                                                             this.state.details.map((item, index) => {
                                                                                    return ( 
                                                                                        <tr key={index}>
                                                                                            <td>{item.CONTACTPERSON}</td>
                                                                                            <td>{item.COMPANY}</td>
                                                                                            <td>{item.EVENT_TYPE}</td>
                                                                                            <td>
                                                                                                <img src="../assests/images/DashboardIcons/smsPh.png"
                                                                                                 style={{width:"18px", cursor:"pointer"}} 
                                                                                                 onClick={(sms)=> { this.sendSms(item.PHNO)}} />
                                                                                            </td>
                                                                                            <td>
                                                                                                <img src="../assests/images/DashboardIcons/email.png"
                                                                                                 style={{width:"18px", cursor:"pointer"}} 
                                                                                                 onClick={(email)=> { this.sendEmail(item.EMAIL)}} />
                                                                                            </td>
                                                                                            
                                                                                        </tr>
                                                                                    )
                                                                                })
                                                                            ) : <tr><td>No Data Found</td></tr>
                                                                       
                                                                    }
                                                                </tbody>
                                                            </table>
                                                                
                                                        </div>
                                                    </div>
                                                </div>
                                                    ): null
                                                }
                                            </div>
                                        </div>
                                    </div>
                                    
                                </div>
                            </div> 
                        </div>
                    </div>
              </div>
          )
      }
      getCustomer = ()=>{
        axios({
            method: 'post',
            url: '../ajax/mngNotification/mngNotification.aspx/GetAllCustomer',
            data: {
                action :"Cust"
            }
          }).then(res => {

              console.log('GetAllCustomer', res); 
              this.setState({
                  ...this.state,
                  showDetails:true,
                  activeComp:"Cust",
                  details:res.data.d
                  //customerDetails:["hello"]
              });
              console.log(this.state)
          });
    }
    getVendor = ()=>{
        axios({
            method: 'post',
            url: '../ajax/mngNotification/mngNotification.aspx/GetAllVendor',
            data: {
                action :"Vend"
            }
          }).then(res => {
              console.log('GetAllCustomer', res); 
              this.setState({
                  ...this.state,
                  showDetails:true,
                  activeComp:"Vend",
                  details:res.data.d
              });
              console.log(this.state)
          });
    }
    getInfluencer = ()=>{
        axios({
            method: 'post',
            url: '../ajax/mngNotification/mngNotification.aspx/GetAllInfluencer',
            data: {
                action :"Influ"
            }
          }).then(res => {
              console.log('Influ', res); 
              this.setState({
                  ...this.state,
                  showDetails:true,
                  activeComp:"Influ",
                  details:res.data.d
              });
              console.log(this.state)
          });
    }
    getTransporter = ()=>{
        axios({
            method: 'post',
            url: '../ajax/mngNotification/mngNotification.aspx/GetAllTransporter',
            data: {
                action :"Trans"
            }
          }).then(res => {
              console.log('Trans', res); 
              this.setState({
                  ...this.state,
                  showDetails:true,
                  activeComp:"Trans",
                  details:res.data.d
              });
              console.log(this.state)
          });
    }
    getEmployee = ()=>{
        axios({
            method: 'post',
            url: '../ajax/mngNotification/mngNotification.aspx/GetAllEmployee',
            data: {
                action :"Emp"
            }
          }).then(res => {
              console.log('Emp', res); 
              this.setState({
                  ...this.state,
                  showDetails:true,
                  activeComp:"emp",
                  details:res.data.d
              });
              console.log(this.state)
          });
    }
}
export default  ManagementNotification;