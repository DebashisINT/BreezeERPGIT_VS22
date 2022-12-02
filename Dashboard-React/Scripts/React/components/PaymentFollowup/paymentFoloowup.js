import React, {Component, lazy} from 'react';
import { Tabs} from 'antd';
const { TabPane } = Tabs;

import TotalFollowup from './Tabs/totalFollowup';
const PendingFollowup = lazy(() => import('./Tabs/pendingFollowup'));
const ClosedFollowup = lazy(() => import('./Tabs/ClosedFollowup'));
const RelationRatio = lazy(() => import('./Tabs/RelationRatio'));
const FollowupHistory = lazy(() => import('./Tabs/followupHistory'));
const FollowupUsing = lazy(() => import('./Tabs/Using'));


import('../SalesDb/salesDb.css');
class PaymentFollowup extends Component {
    constructor(props) {
        super(props);
        this.state = { 
            activeTab: 'totalFollowup',
         }
    }
    tabChange = (key) => {
        console.log(key)
        this.setState({
            ...this.state,
            activeTab: key
        })
    }
    render() { 
        return ( 
            <div>
                <div className="headerArea">
                    <h1>
                        <span className="pull-left backButton">
                            <a href="javascript:void(0);" onClick={()=> {this.props.changComponent('home')}}>
                            <i className="fa fa-arrow-left"></i></a>
                        </span>Payment Followup
                    </h1>
                </div>
                <div style={{
                        padding:"20px"
                    }}>
                    <div className="row">
                        <div className="cardContainer">
                            <div className="cardContent">
                                <Tabs defaultActiveKey={this.state.activeTab} onChange={this.tabChange}>
                                    <TabPane tab="Total Followup" key="totalFollowup">
                                        <TotalFollowup />
                                    </TabPane>
                                    <TabPane tab="Pending Followup" key="pendingFollowup">
                                        <PendingFollowup />
                                    </TabPane>
                                    <TabPane tab="Closed Followup" key="closedFollowup">
                                        <ClosedFollowup />
                                    </TabPane>
                                    <TabPane tab="Customer Relation Ratio" key="RelationRatio">
                                        <RelationRatio />
                                    </TabPane>
                                    <TabPane tab="Followup History" key="history">
                                        <FollowupHistory />
                                    </TabPane>
                                    <TabPane tab="Followup Using" key="FollowupUsing">
                                        <FollowupUsing />
                                    </TabPane>
                                </Tabs>
                            </div>
                        </div>
                        
                    </div>
                </div>
            </div>
         );
    }
}
 
export default PaymentFollowup;