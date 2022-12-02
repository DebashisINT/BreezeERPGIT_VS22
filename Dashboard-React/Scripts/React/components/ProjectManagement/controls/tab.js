import React, { Component } from 'react';
import('./tabs.css');

class Tabs extends Component {
    constructor(props) {
        super(props);
        this.state = {  }
    }
    render() { 
        return ( 
            <React.Fragment>
                <div className="clearfix" style={{marginTop: "15px"}}>
                    <ul className="tabluLer clearfix" id="nav-tab">
                        {
                            this.props.settings.LiProjectSummary ? (
                            <li id="liPerformance" 
                                className={this.props.activeModule =="ProjectSummary" ? "active" : ""}
                                onClick={() => {this.props.loadComponent('ProjectSummary')}}>
                                <div className="text-center">
                                    <span className="ic"><i className="fa fa-bolt"></i></span>
                                </div>
                                <div className="tb_txt">Project Summary</div>
                            </li>
                            ) : null
                        }
                        {
                            this.props.settings.LiProjectDetails ? (
                                <li className={this.props.activeModule =="ProjectDetails" ? "active" : ""} 
                                onClick={() => {this.props.loadComponent('ProjectDetails')}}>
                                    <div className="text-center">
                                        <span className="ic"><i className="fa fa-tasks"></i></span>
                                    </div>
                                    <div className="tb_txt">Project Details</div>
                                </li>
                            ) : null
                        }
                        {
                            this.props.settings.LiTimeline ? (
                                <li className={this.props.activeModule =="Timeline" ? "active" : ""}
                                onClick={() => {this.props.loadComponent('Timeline')}}>  
                                    <div className="text-center">
                                        <span className="ic"><i className="fa fa-clock-o"></i></span>
                                    </div>
                                    <div className="tb_txt">Timeline</div>
                                </li>
                             ) : null
                        }
                        {
                            this.props.settings.LiCostBreakup ? (
                                <li className={this.props.activeModule =="CostBreakup" ? "active" : ""}
                                    onClick={() => {this.props.loadComponent('CostBreakup')}} >
                                        <div className="text-center">
                                            <span className="ic"><i className="fa fa-calculator"></i></span>
                                        </div>
                                        <div className="tb_txt">Cost Breakup</div>
                                    </li>
                            ) : null
                        }   
                    </ul>
                </div>
            </React.Fragment>
         );
    }
}
 
export default Tabs;