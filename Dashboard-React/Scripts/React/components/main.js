import React, { Suspense, lazy } from 'react';
//import 'antd/dist/antd.css';
import { Spin } from 'antd';
import './main.css';
import axios from 'axios';

const AsyncHome = lazy(() => import('./home')); 
const AsyncAccount = lazy(() => import('./AccountDb/account')); 
const AsyncSalesDB =  lazy(() => import('./SalesDb/salesDb'));   
const AsyncMngNot = lazy(() => import('./ManagementNotification/ManagementNotification'));    
const AttendanceDB = lazy(() => import('./AttendanceDB/AttendanceDB'));   
const AsyncTask = lazy(() => import('./Task/task'));
const AsyncPurchaseAnalytics = lazy(() => import('./PurchaseAnalytics/PurchaseAnalytics'));
const AsyncApprovalWaiting = lazy(() => import('./ApprovalWaiting/approvalWaiting'));
const ServiceManagement = lazy(() => import('./ServiceManagement/ServiceManagement'));
const InventoryDB = lazy(() => import('./InventoryDb/InventoryDb'));
const CRMdash = lazy(() => import('./CRMdash/CRMdash.js'));
const AsyncKPIdb = lazy(() => import('./KPISummary/kpiDB'));
const PaymentFollowup = lazy(() => import('./PaymentFollowup/paymentFoloowup'));
const Finance = lazy(() => import('./FinancialDB/FinancialDB'));
const ProjectManagement = lazy(() => import('./ProjectManagement/ProjectManagement'));
const CRManalytics = lazy(() => import('./CRManalytic/crmAnalytics'));
const STB = lazy(() => import('./STBManagement/STB'));
const CUSTOMERPROFILE = lazy(() => import('./customerProfile/CustomerProfile'));

const SpinIt = (props) => {
    return (
        <div className="pageSpin" {...props}>
            <Spin tip="Loading..." />
        </div>
    );
};
class Main extends React.Component {
    constructor(props) {
        super(props);
        this.Changecomponent = this.Changecomponent.bind(this);
    }
    state = {
        showComponent: 'home', 
    }
    componentDidMount (){
        const permited = localStorage.getItem('permited');
        const defaultSer = localStorage.getItem('defaultSer');
        const defaultSTB = localStorage.getItem('defaultSTB');
        
        if(defaultSer == 'true') {
            console.log('defaultSer', defaultSer)
            this.setState({
                ...this.state,
                showComponent: "service"
            })
        }else if (defaultSTB == 'true') {
            this.setState({
                ...this.state,
                showComponent: "STB"
            })
        }else{
            console.log('defaultSerfalse', defaultSer)
            this.setState({
                ...this.state,
                showComponent: "home"
            })
        }
        
        if(permited == null || permited == undefined) {
            console.log('permited', permited)
            axios({method: 'post', url: '../ajax/permissions/permissions.aspx/getDashboardRedirect',data: {}
            }).then(res => {
                console.log('perm', res);
                const defaultDashboardSRV = res.data.d.hdnDefaultDashboardService;
                const defaultDashboardSTB = res.data.d.hdnDefaultDashboardSTB;
                const perimissionString = JSON.stringify(res.data.d);
                console.log("set")
                if(defaultDashboardSRV == "True") {
                    this.setState({
                        ...this.state,
                        showComponent: "service"
                    })
                    localStorage.setItem('defaultSer', true); 
                }
                 
                if(defaultDashboardSTB == "True") {
                    this.setState({
                        ...this.state,
                        showComponent: "STB"
                    })
                    localStorage.setItem('defaultSTB', true); 
                }
                localStorage.setItem('permited', true);
                localStorage.setItem('permissions', perimissionString); 
            });
        }  
    }
    Changecomponent(value) {
        //console.log(value)
        this.setState({
            ...this.state,
            showComponent: value
        });
    }
    swithToService = (val) => {
        axios({
            method: 'post',
            url: '../ajax/ServiceManagement/ServiceMng.aspx/SrvSession',
            data: {
                comment : val
            }
          }).then(res => {
            this.setState({
                ...this.state,
                showComponent: "service"
            });
          });
    }
    swithToErp = (val)=> {
        axios({
            method: 'post',
            url: '../ajax/ServiceManagement/ServiceMng.aspx/SrvSession',
            data: {
                comment : val
            }
          }).then(res => {
            this.setState({
                ...this.state,
                showComponent: "home"
            });
            localStorage.setItem('defaultSer', false);
          });
    }
    render() {
        let compose;
        if(this.state.showComponent == 'home'){
            compose = <Suspense fallback={<SpinIt />}><AsyncHome  changComponent={this.Changecomponent} serviceSwitch={this.swithToService}  /></Suspense>
        }else if(this.state.showComponent == 'account'){
            compose = <Suspense fallback={<SpinIt />}><AsyncAccount changComponent={this.Changecomponent} /></Suspense>
        }else if(this.state.showComponent == 'sales'){
            compose = <Suspense fallback={<SpinIt />}><AsyncSalesDB  changComponent={this.Changecomponent} /></Suspense>
        } else if(this.state.showComponent == 'purchaseAnalytics'){
            compose = <Suspense fallback={<SpinIt />}><AsyncPurchaseAnalytics changComponent={this.Changecomponent} /></Suspense>
        }else if(this.state.showComponent == 'task'){
            compose = <Suspense fallback={<SpinIt />}><AsyncTask changComponent={this.Changecomponent} /></Suspense>
        }else if(this.state.showComponent == 'managementNot'){
            compose = <Suspense fallback={<SpinIt />}><AsyncMngNot changComponent={this.Changecomponent} /></Suspense>
        }else if(this.state.showComponent == 'approvalWaiting'){
            compose = <Suspense fallback={<SpinIt />}><AsyncApprovalWaiting changComponent={this.Changecomponent} /></Suspense>
        }else if(this.state.showComponent == 'service'){
            compose = <Suspense fallback={<SpinIt />}><ServiceManagement erpSwitch={this.swithToErp} /></Suspense>
        }else if(this.state.showComponent == 'inventory'){
            compose = <Suspense fallback={<SpinIt />}><InventoryDB changComponent={this.Changecomponent} /></Suspense>
        }else if(this.state.showComponent == 'CRMdash'){
            compose = <Suspense fallback={<SpinIt />}><CRMdash changComponent={this.Changecomponent} /></Suspense>
        }else if(this.state.showComponent == 'AttendanceDB'){
            compose = <Suspense fallback={<SpinIt />}><AttendanceDB changComponent={this.Changecomponent} /></Suspense>
        }else if(this.state.showComponent == 'kpi'){
            compose = <Suspense fallback={<SpinIt />}><AsyncKPIdb changComponent={this.Changecomponent} /></Suspense>
        }else if(this.state.showComponent == 'paymentFollowup'){
            compose = <Suspense fallback={<SpinIt />}><PaymentFollowup changComponent={this.Changecomponent} /></Suspense>
        }else if(this.state.showComponent == 'FinancialKPI'){
            compose = <Suspense fallback={<SpinIt />}><Finance changComponent={this.Changecomponent} /></Suspense>
        }else if(this.state.showComponent == 'ProjectManagement'){
            compose = <Suspense fallback={<SpinIt />}><ProjectManagement changComponent={this.Changecomponent} /></Suspense>
        }else if(this.state.showComponent == 'CRManalytics'){
            compose = <Suspense fallback={<SpinIt />}><CRManalytics changComponent={this.Changecomponent} /></Suspense>
        }else if(this.state.showComponent == 'STB'){
            compose = <Suspense fallback={<SpinIt />}><STB changComponent={this.Changecomponent} /></Suspense>
        }else if(this.state.showComponent == 'CUSTOMER'){
            compose = <Suspense fallback={<SpinIt />}><CUSTOMERPROFILE changComponent={this.Changecomponent} /></Suspense>
        }else {
            compose =  <Suspense fallback={<SpinIt />}><AsyncHome changComponent={this.Changecomponent} /></Suspense>
        }
        return (
            <div>
                {compose} 
            </div>
        );
    }  
}
export default Main;

