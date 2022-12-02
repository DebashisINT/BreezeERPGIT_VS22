import React from 'react';
import UiBox from '../../UIComponents/uiBox';
class Procurement extends React.Component {
    constructor(props) {
      super(props);
    }
    state = {
       // procurement state
       
    }  
    componentDidMount() {
        
    } 
         
    componentWillUnmount() {   
    }

    render(){
        const {APPRPO,APPRREQ,OPENREQ,PURCHASEREQ, TOTPO} = this.props.data;
          return(      
            <div className="tabInside">
                <div className="chartBox">
                    <div className="hader text-center" style={{marginBottom: "10px"}}>PROCUREMENT REQUISITION</div>
                    <div className="d-flex justify-content-center mainDashBoxes">
                        <div className="flex-itm scr">
                            <UiBox
                            imageURI="../assests/images/DashboardIcons/chall.png"
                            color="#0068bd"
                            titletext="Purchase Requisition"
                            data={PURCHASEREQ}
                            subTitle="Today"
                            />
                        </div>
                        <div className="flex-itm scr">
                            <UiBox
                            imageURI="../assests/images/DashboardIcons/chall.png"
                            color="#5b4a4a"
                            titletext="Open purc. Requisition"
                            data={OPENREQ}
                            subTitle="Total"
                            />
                        </div>
                        <div className="flex-itm scr">
                            <UiBox
                            imageURI="../assests/images/DashboardIcons/chall.png"
                            color="#ee0067"
                            titletext="Purchase Order"
                            data={TOTPO}
                            subTitle="Today"
                            />
                        </div>
                        <div className="flex-itm scr">
                            <UiBox
                            imageURI="../assests/images/DashboardIcons/chall.png"
                            color="#9c3dec"
                            titletext="Order Approved"
                            data={APPRPO}
                            subTitle="Total"
                            />
                        </div>
                        <div className="flex-itm scr">
                            <UiBox
                            imageURI="../assests/images/DashboardIcons/chall.png"
                            color="#f14b2f"
                            titletext="Approved Requisition"
                            data={APPRREQ}
                            subTitle="Today"
                            />
                        </div>
                    </div>    
                </div>
            </div>
        )
    }
}
export default  Procurement;