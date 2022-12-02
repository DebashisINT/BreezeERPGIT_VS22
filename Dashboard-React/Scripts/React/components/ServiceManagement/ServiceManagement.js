import React from 'react';
import { Drawer, Button } from 'antd';
import axios from 'axios';
import('./ServiceManagement.css');

class ServiceManagement extends React.Component {
    constructor(props) {
      super(props);
    }
    state = {
        drawer: false,
        DivAssignJob: false,
        DivDelivery: false,
        DivJobsheetEntry: false,
        DivReceiptChallan: false,
        DivReport: false,
        DivSearch: false,
        DivServiceDatas: false,
        warrantyDiv: false,
        DivToptalOpen: "0",
        DivTotalChallan: "0",
        DivTotalDelivered: "0",
        DivTotalReady: "0",
        DivTotalToken: "0",
        DivTotalUndelivered: "0",
        notifyPara: 'No notification for today.',
        notifyTitle: ''
    }  
    componentDidMount() {
        this.getBoxesData();
        axios({
            method: 'post',
            url: '../ajax/ServiceManagement/ServiceMng.aspx/AnnouncementDetails',
            data: {reqStr: ""}
          }).then(res => {
              console.log('AnnouncementDetails', res)
              let list = res.data.d;
              if (list != null && list.length > 0) {
                for (var i = 0; i < list.length; i++) {
                    var head = '';
                    var body = '';
                    head = list[i].split('|')[0];
                    body = list[i].split('|')[1];
                }
                this.setState({
                    ...this.state,
                    drawer:true,
                    notifyPara: body,
                    notifyTitle: head
                })
            }
            else {
                this.setState({
                    ...this.state,
                    drawer:false,
                    notifyPara: 'No notification for today.',
                    notifyTitle: ''
                });
            }
          });
    }      
    componentWillUnmount() {   
    }
    showDrawer = () => {
       this.setState({
            ...this.state,
            drawer:true 
       })
    };
    onClose = () => {
        this.setState({
             ...this.state,
             drawer:false 
        })
     };

     getBoxesData = () => {
        axios({
            method: 'post',
            url: '../ajax/ServiceManagement/ServiceMng.aspx/GetBoxsData',
            data: {}
          }).then(res => {
                this.setState({
                    ...this.state,
                    DivAssignJob: res.data.d.DivAssignJob,
                    DivDelivery: res.data.d.DivDelivery,
                    DivJobsheetEntry: res.data.d.DivJobsheetEntry,
                    DivReceiptChallan: res.data.d.DivReceiptChallan,
                    DivReport: res.data.d.DivReport,
                    DivSearch: res.data.d.DivSearch,
                    DivServiceDatas: res.data.d.DivServiceDatas,
                    warrantyDiv: res.data.d.warrantyDiv,
                    DivToptalOpen: res.data.d.DivToptalOpen,
                    DivTotalChallan: res.data.d.DivTotalChallan,
                    DivTotalDelivered: res.data.d.DivTotalDelivered,
                    DivTotalReady: res.data.d.DivTotalReady,
                    DivTotalToken: res.data.d.DivTotalToken,
                    DivTotalUndelivered: res.data.d.DivTotalUndelivered
                })
                console.log(this.state)
          });
     }

      render(){
        
          return(
              <div>
                  <Drawer
                    width={350}
                    placement="right"
                    closable={false}
                    onClose={this.onClose}
                    visible={this.state.drawer}
                    > 
                        <div style={{
                            textAlign:"center", 
                            height: "80vh", 
                            display: "flex", 
                            alignItems: "center", 
                            flexDirection:"column",
                            justifyContent: "center"}}>
                            <img src="../assests/images/DashboardIcons/pop_notification.png" 
                            style={{marginBottom: "20px"}} />
                            <h1 style={{ 
                                marginBottom:"24px !important", 
                                fontWeight: "800", 
                                fontSize: "28px"}}>{this.state.notifyTitle}</h1>
                            <p className="fontPop">{this.state.notifyPara}</p>
                            <Button type="primary" onClick={this.onClose}>OK</Button>
                        </div>
                    </Drawer>
                    <div className="headerArea relative ">
                        <h1 className="clearfix">
                            <span className="pull-left backtoERP">
                                <a href="javascript:void(0);" onClick={ () => {this.props.erpSwitch("ERP")}}>
                                <i className="fa fa-arrow-left"></i> <span>Switch to ERP</span></a>
                            </span> <span className="pageTitle">Service Management</span>
                            <span className="notifyService" onClick={this.showDrawer}><img src="../assests/images/DashboardIcons/notification.png" /></span>
                        </h1>
                    </div>
                    <div className="row">
                        <div className="col-md-12">
                            <div className="cardContainer mt-3" style={{background:"#5a84fb"}}>
                                <div className="cardContent">
                                    <div className="d-flex justify-content-center mainDashBoxes">
                                        <div className="flex-itm scr " >
                                            <div className="widgBox c2" >
                                                <div className="d-flex align-items-center">
                                                    <div className="icon"><img src="../assests/images/DashboardIcons/chall.png" /></div>
                                                    <div className="flex-grow-1 txt"> Challan</div>
                                                </div>
                                                <div className="Numb">{this.state.DivTotalChallan}</div>
                                                <div className="text-center">Today</div>
                                                <div className="text-right"><span className="lnr lnr-arrow-down dwn arrD"></span></div>
                                            </div>
                                        </div>
                                        <div className="flex-itm scr ">
                                            <div className="widgBox c3">
                                                <div className="d-flex  align-items-center">
                                                    <div className="icon"><img src="../assests/images/DashboardIcons/token.png" /></div>
                                                    <div className="flex-grow-1 txt"> Token</div>
                                                </div>
                                                <div className="Numb">{this.state.DivTotalToken}</div>
                                                <div className="text-center">Today</div>
                                                <div className="text-right"><span className="lnr lnr-arrow-down dwn arrD"></span></div>
                                            </div>
                                        </div>
                                        <div className="flex-itm scr ">
                                            <div className="widgBox c5">
                                                <div className="d-flex  align-items-center">
                                                    <div className="icon"><img src="../assests/images/DashboardIcons/topen.png" /></div>
                                                    <div className="flex-grow-1 txt">  Open</div>
                                                </div>
                                                <div className="Numb">{this.state.DivToptalOpen}</div>
                                                <div className="text-center">Total</div>
                                                <div className="text-right"><span className="lnr lnr-arrow-down dwn arrD"></span></div>
                                            </div>
                                        </div>
                                        <div className="flex-itm scr " >
                                            <div className="widgBox c4" >
                                                <div className="d-flex  align-items-center">
                                                    <div className="icon"><img src="../assests/images/DashboardIcons/ready.png" /></div>
                                                    <div className="flex-grow-1 txt"> Ready</div>
                                                </div>
                                                <div className="Numb">{this.state.DivTotalReady}</div>
                                                <div className="text-center">Today</div>
                                                <div className="text-right"><span className="lnr lnr-arrow-down dwn arrD"></span></div>
                                            </div>
                                        </div>
                                        <div className="flex-itm scr " >
                                            <div className="widgBox">
                                                <div className="d-flex  align-items-center">
                                                    <div className="icon"><img src="../assests/images/DashboardIcons/deliv.png" /></div>
                                                    <div className="flex-grow-1 txt"> Delivered</div>
                                                </div>
                                                <div className="Numb">{this.state.DivTotalDelivered}</div>
                                                <div className="text-center"> Today</div>
                                                <div className="text-right"><span className="lnr lnr-arrow-down dwn arrD"></span></div>
                                            </div>
                                        </div>
                                        <div className="flex-itm scr " >
                                            <div className="widgBox c">
                                                <div className="d-flex  align-items-center">
                                                    <div className="icon"><img src="../assests/images/DashboardIcons/undel.png" /></div>
                                                    <div className="flex-grow-1 txt">  Undelivered</div>
                                                </div>
                                                <div className="Numb">{this.state.DivTotalUndelivered}</div>
                                                <div className="text-center">Total</div>
                                                <div className="text-right"><span className="lnr lnr-arrow-down dwn arrD"></span></div>
                                            </div>
                                        </div>
                                    </div>  
                                </div>
                            </div>
                            <div className="container mt-5 ">
                                <div className="d-flex wrapFlex justify-content-center">
                                    {
                                        this.state.DivReceiptChallan ? (
                                        <div className="itmWidth"
                                             onClick={()=>{window.location.href("/ServiceManagement/Transaction/ReceiptChallanList.aspx")}}>
                                            <div className="itm-menu c1">
                                                <div>
                                                    <span className="icon-style"><img src="../assests/images/DashboardIcons/paperW.png" /></span>
                                                </div>
                                                <h4>Receipt Challan</h4>
                                            </div>
                                        </div>
                                        ) : null
                                    }
                                    {
                                        this.state.DivAssignJob ? (
                                            <div className="itmWidth" onClick={()=>{window.location.href("/ServiceManagement/Transaction/AssignedJob.aspx")}}>
                                                <div class="itm-menu c3">
                                                        <div>
                                                            <span className="icon-style "><img src="../assests/images/DashboardIcons/salesmanW.png" /></span>
                                                        </div>
                                                        <h4>Assign Job</h4>
                                                </div>
                                            </div>
                                        ) : null
                                    }
                                    {
                                        this.state.DivServiceDatas ? (
                                            <div className="itmWidth" onClick={()=>{window.location.href("/ServiceManagement/Transaction/serviceData/serviceDataList.aspx")}}>
                                                <div className="itm-menu c4">
                                                    <div>
                                                        <span className="icon-style "><img src="../assests/images/DashboardIcons/cogst.png" /></span>
                                                    </div>
                                                    <h4>Service Entry</h4>
                                                </div>
                                            </div>
                                        ) : null
                                    }
                                    {
                                        this.state.warrantyDiv ? (
                                            <div  className="itmWidth" 
                                                onClick={()=>{window.location.href("/ServiceManagement/Transaction/Warranty/WarrantyList.aspx")}}>
                                                <div className="itm-menu c9">
                                                    <div>
                                                        
                                                        <span className="icon-style "><img src="../assests/images/DashboardIcons/warranty.png" /></span>
                                                    </div>
                                                    <h4>Warranty Update</h4>
                                                </div>
                                            </div>
                                        ) : null
                                    }
                                    {
                                        this.state.DivDelivery ? (
                                            <div className="itmWidth" 
                                                onClick={()=>{window.location.href("/ServiceManagement/Transaction/Delivery/DeliveryList.aspx")}}>
                                                <div className="itm-menu c5">
                                                    <div>
                                                        <span className="icon-style "><img src="../assests/images/DashboardIcons/boxW.png" /></span>
                                                    </div>
                                                    <h4>Delivery</h4>
                                                </div>
                                            </div>
                                        ) : null
                                    }
                                    {
                                        this.state.DivSearch ? (
                                            <div className="itmWidth" onClick={()=>{window.location.href("/ServiceManagement/Transaction/search/searchqueries.aspx")}}>
                                                <div className="itm-menu c6">
                                                    <div>
                                                        <span className="icon-style "><img src="../assests/images/DashboardIcons/searchW.png" /></span>
                                                    </div>
                                                    <h4>Search</h4>
                                                </div>
                                            </div>
                                        ) : null
                                    }
                                    {
                                        this.state.DivJobsheetEntry ? (
                                            <div className="itmWidth" 
                                                onClick={()=>{window.location.href("/ServiceManagement/Transaction/jobsheet/jobsheetList.aspx")}}>
                                                <div className="itm-menu c7">
                                                    <div>
                                                        <span className="icon-style "><img src="../assests/images/DashboardIcons/EntryW.png" /></span>
                                                    </div>
                                                    <h4>Jobsheet Entry</h4>
                                                </div>
                                            </div>
                                        ) : null
                                    }
                                    {
                                        this.state.DivReport ? (
                                            <div className="itmWidth" >
                                                <div className="itm-menu c8" onClick={this.gotoReport}>
                                                    <div>
                                                        <span className="icon-style "><img src="../assests/images/DashboardIcons/reportW.png" /></span>
                                                    </div>
                                                    <h4>Report</h4>
                                                </div>
                                            </div>
                                        ) : null
                                    }
                                </div>
                            </div>
                        </div>
                    </div>
              </div>
          )
      }

      gotoReport = () =>{
            console.log("redirect report");
            window.location.href("/Reports/GridReports/SRVServiceRegisterReport.aspx")
      }
}
export default  ServiceManagement;