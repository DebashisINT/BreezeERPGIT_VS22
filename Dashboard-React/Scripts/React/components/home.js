import React from 'react';
//import Ticker, { FinancialTicker, NewsTicker } from 'nice-react-ticker';
import Ticker from 'react-ticker';

import { Skeleton } from 'antd';
import { ArrowRightOutlined  } from '@ant-design/icons';
import axios from 'axios';
import  './main.css';
import  './home.css';

import { connect } from 'react-redux'
import { fetchRights, GetAnnouncement, getDashboardRedirect, setpermissionFromLocal, GetAllNotificationsAcction } from '../store/reducers/homeSlice';
class Home extends React.Component {

    constructor (props){
        super(props);
        this.state = {
            showTickerAb : false
        }
    }
    
    componentDidUpdate(){
        //console.log('updateprops', this.state.permissions)
    }
    componentWillMount(){
        
    }
    componentDidMount(){
        
        const homePermissions = this.props.homePermissions;
        const notifyCalled = this.props.notifyCalled;
        const permissionsLocal = localStorage.getItem('permissions');
        if(!permissionsLocal){
            this.props.getDashboardRedirect();
        }else{
            let perobj = JSON.parse(permissionsLocal)
            this.props.setpermissionFromLocal(perobj)
        }
        if(!notifyCalled) {
            this.props.GetAllNotificationAll(); 
        }
        //fetching announcement
        this.fetchDataAnn();
    }
    fetchDataposts = () =>{
        this.props.fetchAnnouncement();
    }
    fetchDataAnn = () =>{
        this.props.fetchAnnouncement();
    }
    render() {
        const permissions = this.props.permissions;
        
        let SWbtnSwitch;
        if(this.props.permissions.hdnDefaultDashboardSTB) {
            SWbtnSwitch =  <button type="button" className="button button-round yellow"  
            onClick={()=>{this.props.changComponent('STB')}}> 
            <ArrowRightOutlined /> Switch to STB Management</button>
        }else if(this.props.permissions.hdnDefaultDashboardService){
            SWbtnSwitch = <button type="button" className="button button-round yellow"  
                onClick={ () => {this.props.serviceSwitch("SRV")}}> 
                <ArrowRightOutlined /> Switch to Service Management</button>
        }else {
            SWbtnSwitch = null
        }
        return (
            <div style={{margin:"5px 0"}}>
                <div className="row" style={{marginTop:"15px"}}>
                    <div className="col-md-12">
                        {SWbtnSwitch}
                        {/* <button type="button" className="button button-round green"
                            onClick={() => {window.location.href="/OMS/management/ProjectMainPage.aspx" } }>
                            Old Dashboard</button> */}
                    </div>
                </div>

                { this.props.skeletenLoad ? (
                    <div className="cardContainer" style={{marginTop:"15px"}}>
                        <div className="cardContent"><Skeleton active /></div>
                    </div>
                )   : null}
                <div className="switchBox-wrap">
                    {
                        this.props.permissions.SalesDbButton ? (
                            <div className="switchBox" onClick={()=>{this.props.changComponent('sales')}}>
                                <div className="nBoxes">
                                    <img src="../assests/images/DashboardIcons/sale.png" alt="" />
                                    <div className="Ntilt">Sales Analytics</div>
                                </div>
                            </div>
                        )  :null
                    }
                    
                    {
                        this.props.permissions.PurchaseDbButton ? (
                            <div className="switchBox" onClick={()=>{this.props.changComponent('purchaseAnalytics')}}>
                                    <div className="nBoxes">
                                        <img src="../assests/images/DashboardIcons/kpi.png" alt="" />
                                        <div className="Ntilt">Purchase Analytics</div>
                                    </div> 
                            </div> 
                        )  :null
                    }
                    {
                        this.props.permissions.CRMButton ? (
                            <div className="switchBox" onClick={()=>{this.props.changComponent('CRManalytics')}}>
                                <div className="nBoxes">
                                    <img src="../assests/images/DashboardIcons/crm.png" alt="" />
                                    <div className="Ntilt">CRM Analytics</div>
                                </div>
                            </div>  
                        )  :null
                    }
                    {
                        this.props.permissions.Attbtn ? (
                            <div className="switchBox" onClick={()=>{this.props.changComponent('AttendanceDB')}}>
                                <div className="nBoxes">
                                    <img src="../assests/images/DashboardIcons/attendance.png" alt="" />
                                    <div className="Ntilt">Todays's Attendance</div>
                                </div>
                            </div>
                        )  :null
                    }
                    {
                        this.props.permissions.followupBtn ? (
                            <div className="switchBox" onClick={()=>{this.props.changComponent('paymentFollowup')}}>
                                <div className="nBoxes">
                                    <img src="../assests/images/DashboardIcons/analytics%20(1).png" alt="" />
                                    <div className="Ntilt">Payment Followup</div>
                                </div>
                            </div>
                        )  :null
                    }
                    {
                        this.props.permissions.AccountsBtn ? (
                            <div className="switchBox" onClick={()=>{this.props.changComponent('account')}}>
                                    <div className="nBoxes">
                                        <img src="../assests/images/DashboardIcons/accounting.png" alt="" />
                                        <div className="Ntilt">Account Dashboard</div>
                                    </div>
                            </div>
                        ) : null
                    }
                    {
                        this.props.permissions.tasklistbtn ? (
                            <div className="switchBox" onClick={()=>{this.props.changComponent('task')}}>  
                                <div className="nBoxes">
                                    <img src="../assests/images/DashboardIcons/task.png" alt="" />
                                    <div className="Ntilt">Task List</div>
                                </div>
                            </div>
                        )  :null
                    }
                     {
                        this.props.permissions.PMSButton ? (
                            <div className="switchBox" onClick={()=>{this.props.changComponent('ProjectManagement')}}>
                                <div className="nBoxes">
                                    <img src="../assests/images/DashboardIcons/project.png" alt="" />
                                    <div className="Ntilt">Project Management</div>
                                </div>
                            </div>
                        )  :null
                    }
                    {
                        this.props.permissions.FinancialButton ? (
                            <div className="switchBox" onClick={()=>{this.props.changComponent('FinancialKPI')}}>
                                <div className="nBoxes">
                                    <img src="../assests/images/DashboardIcons/stock.png" alt="" />
                                    <div className="Ntilt">Financial KPI</div>
                                </div>
                            </div>
                        )  :null
                    }
                    {
                        this.props.permissions.CustRMButton ? (
                            <div className="switchBox" onClick={()=>{this.props.changComponent('CRMdash')}}>
                                <div className="nBoxes">
                                    <img src="../assests/images/DashboardIcons/customer.png" alt="" />
                                    <div className="Ntilt">Customer Relationship...</div>
                                </div>
                            </div>
                        )  :null
                    }
                    {
                        this.props.permissions.dvKPISummary ? (
                            <div className="switchBox" onClick={()=>{this.props.changComponent('kpi')}}>
                                <div className="nBoxes">
                                    <img src="../assests/images/DashboardIcons/kpi.png" alt="" />
                                    <div className="Ntilt">KPI Summary</div>
                                </div>
                            </div>
                        )  :null
                    }
                    {
                        this.props.permissions.dvInveDashboard ? (
                            <div className="switchBox" onClick={()=>{this.props.changComponent('inventory')}}>
                                <div className="nBoxes">
                                    <img src="../assests/images/DashboardIcons/inventory.png" alt="" />
                                    <div className="Ntilt">Inventory Dashboard</div>
                                </div>
                            </div>
                        )  :null
                    }
                    {
                        this.props.permissions.dvManagementNotification ? (
                            <div className="switchBox" onClick={()=>{this.props.changComponent('managementNot')}}>
                                <div className="nBoxes">
                                <span className="notLight">{this.props.mngCount}</span>
                                    <img src="../assests/images/DashboardIcons/mangementNotify.png" alt="" />
                                    <div className="Ntilt">Manamgement Notification</div>
                                </div>
                            </div>
                        )  :null
                    }                
                    
                    {
                        this.props.permissions.dvApprovalWaiting ? (
                            <div className="switchBox" onClick={()=>{this.props.changComponent('approvalWaiting')}}>
                                <div className="nBoxes">
                                    <span className="notLight">{this.props.approvalCount}</span>
                                    <img src="../assests/images/DashboardIcons/analytics%20(2).png" alt="" />
                                    <div className="Ntilt">Approval Waiting</div>
                                </div>
                            </div>
                        )  :null
                    }
                    {
                        this.props.permissions.CUSTButton ? (
                            <div className="switchBox" onClick={()=>{this.props.changComponent('CUSTOMER')}}>
                                <div className="nBoxes">
                                    <img src="../assests/images/DashboardIcons/customerProfile.png" alt="" />
                                    <div className="Ntilt">Customer Profile</div>
                                </div>
                            </div>  
                        )  :null
                    }
                    
                </div>    
                {/* ANNOUNCEMENT ON PAGE */}
                {
                    this.props.announcement.length ? (
                    <div style={{padding:"10px 0 45px 0"}}>
                        <h3 style= {{
                            fontFamily: "'Poppins', sans-serif !IMPORTANT",
                            fontSize: "18px",
                            fontWeight: "400"
                        }}>Today's Announcement 
                            <img style= {{
                                width:"20px",
                                marginLeft: "8px"
                            }} src="/assests/svg/speaker.svg" />
                        </h3>
                        <div className="clearfix" id="announcementDiv" style={{
                            fontFamily:"'Poppins', sans-serif !IMPORTANT",
                            fontSize: "13px",
                            fontWeight: "400"
                        }}>
                        {this.props.announcement.map((item, index) => (
                            <div className="col-md-12 cont">
                                <div className="alert-message ">
                                    <h4>{item.title}</h4>  
                                    <div dangerouslySetInnerHTML={{__html: item.anninHtml}}></div>
                                </div>
                            </div>
                        ))}
                            
                        </div>
                    </div>
                 ): null
                }
                {/* ANNOUNCEMENT ATTHE BOTTOM TICKER */}
                {
                   this.props.announcement.length ? (
                        this.state.showTickerAb ? (
                            <div id="newsSettingTicker" className="clearfix relative">
                                <div className="tickerNws ">
                                    <div className="tag nCC" style={{background:"#4e8cda"}}>
                                        <img src="/assests/images/announcement.svg" />
                                    </div>
                                    <div className="TickerNews" id="T2">
                                        <Ticker offset="run-in" speed={8}>
                                            {() => (
                                                <>
                                                    <div> 
                                                    {this.props.announcement.map((item, index) => (
                                                            <a href="#announcementDiv">
                                                                <span className="t2title">{item.title}</span>
                                                                {/* <span>{item.msg}</span> */}
                                                            </a>
                                                        ))}
                                                    </div>
                                                </>      
                                            )}
                                        </Ticker>
                                    </div>
                                </div>
                            </div>
                        ) : null
                   ) : null  
                }

                
                
            </div>
        );
    }
}

const mapStateToProps = state => {
    return { 
        announcement: state.posts.announcement,
        showTickerA: state.posts.showTickerA,
        homePermissions: state.posts.homePermissions,
        notifyCalled: state.posts.notifyCalled,
        permissions: state.posts.permissions,
        approvalCount: state.posts.approvalCount,
        mngCount: state.posts.mngCount,
     };
};
const mapDispatchToProps = dispatch => {
    return{
        fetchAnnouncement: () => dispatch(GetAnnouncement()),
        getDashboardRedirect: ()=> dispatch(getDashboardRedirect()),
        setpermissionFromLocal: (data)=> dispatch(setpermissionFromLocal(data)),
        GetAllNotificationAll: (data)=> dispatch(GetAllNotificationsAcction())
    }    
}
export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Home);

