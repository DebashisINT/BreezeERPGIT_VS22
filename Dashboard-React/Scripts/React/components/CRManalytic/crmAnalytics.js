import React, { Component, lazy} from 'react';
const CallLog = lazy(() => import('./Tabs/callLog'));
const VisitLog = lazy(() => import('./Tabs/visitLog'));
const ActSalesmanWise =  lazy(() => import('./Tabs/ActSalesmanWise'));
const PendingActivities =  lazy(() => import('./Tabs/pendingActivities'));
const OrderCountSalesmenWise =  lazy(() => import('./Tabs/orderCountSalesmanwise'));
const EfficiencyRatio =  lazy(() => import('./Tabs/efficiencyRatio'));
const NewVsRepeat =  lazy(() => import('./Tabs/newVrepeat'));
const ActivityHistory =  lazy(() => import('./Tabs/activityHistory'));
import('./cmmAnalytic.css');
import { Tabs} from 'antd';
const { TabPane } = Tabs;

import axios from 'axios';

class CRManalytics extends Component {
    constructor(props) {
        super(props);
        this.state = { 
            activeTab: 'callLogs',
            //permissions
            permissions: {
                AhDiv: false,
                AhDivbtn: false,
                CallDiv: false,
                CallDivbtn: false,
                EFDiv: false,
                EFDivbtn: false,
                OrderCntdiv: false,
                OrderCntdivbtn: false,
                QuoteCountdiv: false,
                QuoteCountdivbtn: false,
                SVDiv: false,
                SVDivbtn: false,
                nrDiv: false,
                nrDivbtn: false,
                pendingActDiv: false,
                pendingActDivbtn: false,
                totEntDiv: false,
                totEntDivbtn: false,
            }
         }
    }
    componentDidMount () {
        axios({
            method: 'post',url: '../ajax/crmAnalytic/index.aspx/getPageloadPerm',data: {}
            })
            .then(res => {
                console.log('attP', res.data)
                this.setState({
                    ...this.state, 
                    permissions:res.data.d
                });
            })
    }
    tabChange = (tabName) =>{
        console.log(tabName)
        this.setState({...this.state,activeTab : tabName })
    }
    render() { 
        return ( 
            <React.Fragment>
                <div className="headerArea">
                        <h1>
                            <span className="pull-left backButton">
                                <a href="javascript:void(0);" onClick={()=> {this.props.changComponent('home')}}>
                                <i className="fa fa-arrow-left"></i></a>
                            </span>CRM Analytics
                        </h1>
                    </div>
                    <div className="row">
                        <div className="col-md-12">
                            <div className="cardContainer mt-3">
                                <div className="cardContent">
                                    <Tabs defaultActiveKey={this.state.activeTab} onChange={this.tabChange}>
                                        {this.state.permissions.CallDivbtn ? (
                                            <TabPane tab="Call Logs" key="callLogs">
                                                <CallLog />
                                            </TabPane>
                                            ) : null }
                                        {this.state.permissions.SVDivbtn ? (
                                            <TabPane tab="Visit Logs" key="VisitLogs">
                                                {this.state.activeTab == "VisitLogs" ? <VisitLog /> : null} 
                                            </TabPane>
                                        ) : null}
                                        {this.state.permissions.totEntDiv ? (
                                            <TabPane tab="Activities Salesmen Wise" key="ActivitiesSalesmenWise">
                                                {this.state.activeTab == "ActivitiesSalesmenWise" ? <ActSalesmanWise /> : null}
                                            </TabPane>
                                        ) : null}
                                        {this.state.permissions.pendingActDiv ? (
                                            <TabPane tab="Pending Activities" key="PendingActivities">
                                                {this.state.activeTab == "PendingActivities" ? <PendingActivities /> : null}
                                            </TabPane>
                                        ) : null} 
                                        {this.state.permissions.OrderCntdiv ? (
                                            <TabPane tab="Order Count Salesmen Wise" key="OrderCountSalesmenWise">
                                                {this.state.activeTab == "OrderCountSalesmenWise" ? <OrderCountSalesmenWise />: null}
                                            </TabPane>
                                        ) : null}
                                        {/* {this.state.permissions.QuoteCountdiv ? (
                                            <TabPane tab="Proforma/Quotation Salesman Wise" key="ProformaQuotationSalesmanWise">
                                                {this.state.activeTab == "ProformaQuotationSalesmanWise" ? "Coming Soon": null}
                                            </TabPane>
                                        ) : null} */}
                                        {this.state.permissions.EFDiv ? (
                                            <TabPane tab="Efficiency Ratio  Salesmen Wise" key="EfficiencyRatioSalesmenWise">
                                                {this.state.activeTab == "EfficiencyRatioSalesmenWise" ? <EfficiencyRatio />: null}
                                            </TabPane>
                                        ) : null} 
                                        {this.state.permissions.AhDiv ? (
                                            <TabPane tab="Activity History" key="ActivityHistory">
                                                {this.state.activeTab == "ActivityHistory" ? <ActivityHistory />: null}
                                            </TabPane>
                                        ) : null}
                                        {this.state.permissions.nrDiv ? (
                                            <TabPane tab="New Vs Repeat Sale" key="NewRepeatSale">
                                                {this.state.activeTab == "NewRepeatSale" ? <NewVsRepeat /> : null}
                                            </TabPane>
                                        ) : null}    
                                    </Tabs>
                                </div>
                            </div>
                        </div>
                    </div>
            </React.Fragment>
         );
    }
}
 
export default CRManalytics;