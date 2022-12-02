import React from 'react';
import('../kpiDB.css');
import('../../CRMdash/CRMdash.css');

class Resolution extends React.Component {
    render(){
        return (
           <React.Fragment>
            <div className="backgroundedBoxes">
                    <div className="clearfix col-md-12">
                        <div className="flex-row space-between align-items-center">
                            <div className="flex-item itemType relative">
                                <div className="">
                                   
                                    <div className="valRound " >
                                        <span>0</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">CASES</div>
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    
                                    <div className="valRound " style={{background: "#f76d6d"}}>
                                        <span id="emailValue">0</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">RESOLVED</div>
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    
                                    <div className="valRound " style={{background: "#6e98f5"}}>
                                        <span id="callsmsValue">0</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">PENDING</div>  
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    
                                    <div className="valRound " style={{background: "#44c8cd"}}>
                                        <span id="visitValue">0</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">ON PROCESS</div> 
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                   
                                    <div className="valRound " style={{background: "#9db53c"}}>
                                        <span id="socialValue">0</span>
                                    </div>
                                    <div className="smallmuted ">Total</div>
                                    <div className="hdTag">UNDER SUBS</div> 
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    
                                    <div className="valRound " style={{background: "#8846c5"}}>
                                        <span id="otherValue">0</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">OUT OF SERVICE</div>  
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
           </React.Fragment>
       )
    }
}
export default Resolution;