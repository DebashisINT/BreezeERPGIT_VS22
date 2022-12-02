import React from 'react';
import('../kpiDB.css');
import('../../CRMdash/CRMdash.css');

class EmpInfo extends React.Component {
    render(){
        return (
           <React.Fragment>
            <div className="backgroundedBoxes">
                    <div className="clearfix col-md-12">
                        <div className="flex-row space-between align-items-center">
                            <div className="flex-item itemType relative">
                                <div className="">
                                   
                                    <div className="valRound semiRound" >
                                        <span>{this.props.data.WORKINGDAYS}</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">WORKING DAYS</div>
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    
                                    <div className="valRound semiRound" style={{background: "#f76d6d"}}>
                                        <span id="emailValue">{this.props.data.PRESENTS}</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">PRESENTS</div>
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    
                                    <div className="valRound semiRound" style={{background: "#6e98f5"}}>
                                        <span id="callsmsValue">{this.props.data.LEAVES}</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">LEAVES</div>  
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    <div className="valRound semiRound" style={{background: "#44c8cd"}}>
                                        <span id="visitValue">{this.props.data.HALFDAYS}</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">HALF DAYS</div> 
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                   
                                    <div className="valRound semiRound" style={{background: "#9db53c"}}>
                                        <span id="socialValue">0</span>
                                    </div>
                                    <div className="smallmuted ">Total</div>
                                    <div className="hdTag">CTC</div> 
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    
                                    <div className="valRound semiRound" style={{background: "#8846c5"}}>
                                        <span id="otherValue">0</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">OTHERS EXPENCES</div>  
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                   
              
           </React.Fragment>
       )
    }
}
export default EmpInfo;