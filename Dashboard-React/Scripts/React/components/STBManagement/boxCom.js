import React from 'react';

class boxCom extends React.Component {
    constructor(props) {
      super(props);
    }
    
    componentDidMount() {
       
    }
    componentDidUpdate(){
        
    }
    componentWillUnmount() {
        if (this.chart) {
          this.chart.dispose();
        }
      }
      render(){
          
          return(
            <div className="flex-itm stbDbBox">
                <div className=" ">
                    <div className="bx-cont">
                        <div className="media d-flex">
                        <div className="media-left" style={{
                            display:"flex",
                            justifyContent:"center",
                            alignItems:"center"
                        }}>
                            <div className="bqBox c4"><img src="../assests/images/DashboardIcons/004-return.png" className="media-object" style={{maxWidth:"25px"}} /></div>
                        </div>
                        <div className="media-body">
                            <div className="bx-muted">Total </div>
                            <div className="bx-amt">250</div>
                        </div>
                        </div>
                    </div>
                    <div className="bx-footer">Return Req. Pending (Branch)</div>
                </div>
            </div>
          )
      }
    
}
export default  boxCom;



