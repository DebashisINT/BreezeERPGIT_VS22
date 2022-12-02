
import React, {Component } from 'react';
import UiBoxWithFullvalue from '../../UIComponents/boxWithFullvalue';


class ProjectSummary extends Component {
    constructor(props) {
        super(props);
        this.state = {  }
    }
    render() { 
        const data = this.props.data;
        //console.log(`INITIAL_COST, ${INITIAL_COST.convertedValue}`)

        
        return ( 
            <React.Fragment>
                <div className="BoxTypeGrey">
                <div className="flex-row space-between align-items-center">
                    <UiBoxWithFullvalue title="INITIAL EST. COST"  
                    subtitle="AS ON DATE" 
                    icColor="#32ad6f"
                    value={data.INITIAL_COST}
                     />
                    <UiBoxWithFullvalue title="REVISED EST. COST"  
                    subtitle="AS ON DATE" icColor="#9db53c"
                    value={data.Est_Revised}
                     />
                    <UiBoxWithFullvalue title="ORDER PLACED AGST. ESTIMATE"  
                    subtitle="AS ON DATE" icColor="#8846c5"
                    value={data.Order_Value}
                     />
                    <UiBoxWithFullvalue title="COST BOOKED"  
                    subtitle="AS ON DATE" icColor="#383d82"
                    value={data.Cost_Booked}
                    />
                    <UiBoxWithFullvalue title="TOTAL ORDER BALANCE"  
                    subtitle="AS ON DATE" icColor="#ee0067"
                    value={data.Est_Balance} />
                </div>
                <div className="flex-row space-between align-items-center">
                    <UiBoxWithFullvalue title="INITIAL REVENUE"  
                    subtitle="AS ON DATE" icColor="#c4c703"
                    value={data.Initial_Revenue} />
                    <UiBoxWithFullvalue title="REVISED REVENUE" 
                     subtitle="AS ON DATE" icColor="#565454"
                    value={data.Revised} />
                    <UiBoxWithFullvalue title="REVENUE BOOKED"  
                    subtitle="AS ON DATE" icColor="#f76d6d"
                    value={data.Revenue_booked} />
                    <UiBoxWithFullvalue title="ORDER BALANCE" 
                     subtitle="AS ON DATE" icColor="#6e98f5" 
                    value={data.Revenue_balance} />
                    <UiBoxWithFullvalue title="INITIAL EST. PROFIT"  
                    subtitle="AS ON DATE" icColor="#44c8cd"
                    value={data.Profit} />
                </div>
                <div className="flex-row space-between align-items-center">
                    <UiBoxWithFullvalue title="REVISED EST. PROFIT"  
                    subtitle="AS ON DATE" icColor="#3285ad" 
                    value={data.RevProfit} />
                </div>
                </div> 
            </React.Fragment>
         );
    }
}
 
export default ProjectSummary;