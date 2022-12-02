import React from 'react';
import UiBox from '../../UIComponents/uiBox';
import { getCurrentDate } from '../../helpers/helperFunction';
import axios from 'axios';
class StockRequisition extends React.Component {
    constructor(props) {
      super(props);
    }
    state = {
        APPRPENDING: "0",
        APPRREQ: "0",
        BRANCHREQ: "0",
        CLOSEREQ: "0",
        OPENREQ: "0"
    }  
    componentDidMount() {
        let asonDate = getCurrentDate()
        axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/GetRequisition',
            data: {
                date: asonDate
            }
          }).then(res => {
              console.log('requisition', res)
              this.setState({
                  ...this.state,
                    APPRPENDING: res.data.d[0].APPRPENDING,
                    APPRREQ: res.data.d[0].APPRREQ,
                    BRANCHREQ: res.data.d[0].BRANCHREQ,
                    CLOSEREQ: res.data.d[0].CLOSEREQ,
                    OPENREQ: res.data.d[0].OPENREQ
              })
          }).catch(err => console.log(err));
        
        //this.getAllBox();   
    }      
    componentWillUnmount() {   
    }

    render(){
        
          return(      
            <div className="tabInside">
                <div className="chartBox">
                    <div className="hader text-center" style={{marginBottom: "10px"}}>STOCK REQUISITION</div>
                    <div className="d-flex justify-content-center mainDashBoxes">
                        <div className="flex-itm scr">
                            <UiBox
                            imageURI="../assests/images/DashboardIcons/chall.png"
                            color="#0068bd"
                            titletext="Barnch Requisition"
                            data={this.state.BRANCHREQ}
                            subTitle="Today"
                            />
                        </div>
                        <div className="flex-itm scr">
                            <UiBox
                            imageURI="../assests/images/DashboardIcons/chall.png"
                            color="#5b4a4a"
                            titletext="Approval Pending"
                            data={this.state.APPRPENDING}
                            subTitle="Total"
                            />
                        </div>
                        <div className="flex-itm scr">
                            <UiBox
                            imageURI="../assests/images/DashboardIcons/chall.png"
                            color="#ee0067"
                            titletext="Open Requisation"
                            data={this.state.OPENREQ}
                            subTitle="Total"
                            />
                        </div>
                        <div className="flex-itm scr">
                            <UiBox
                            imageURI="../assests/images/DashboardIcons/chall.png"
                            color="#9c3dec"
                            titletext="Close Requisition"
                            data={this.state.CLOSEREQ}
                            subTitle="Today"
                            />
                        </div>
                        <div className="flex-itm scr">
                            <UiBox
                            imageURI="../assests/images/DashboardIcons/chall.png"
                            color="#f14b2f"
                            titletext="Approved Requisition"
                            data={this.state.APPRREQ}
                            subTitle="Today"
                            />
                        </div>
                    </div>
                    
                </div>
            </div>
        )
    }
}
export default  StockRequisition;