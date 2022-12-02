import React from 'react';
import SwiperCore, { Navigation, Pagination, Scrollbar, A11y } from 'swiper';
import { Swiper, SwiperSlide } from 'swiper/react';
import 'swiper/swiper-bundle.min.css';
import('./CRMdash.css');

import asyncComponent from '../AsyncComponent/AsyncComponent';
const Opportunity = asyncComponent(() => {
    return import('./sections/Opportunity');
});
const Activities = asyncComponent(() => {
    return import('./sections/Activities');
});
const Campaign = asyncComponent(() => {
    return import('./sections/Campaign');
});
const Leads = asyncComponent(() => {
    return import('./sections/Leads');
});
SwiperCore.use([Navigation, Pagination, Scrollbar, A11y]);
class CRMdash extends React.Component {
    constructor(props) {
      super(props);
    }
    state = {
        activeCom : "Activities"
    }  
    componentDidMount() {
        //this.getAllBox();   
    }      
    componentWillUnmount() {   
    }
    viewSubComp = (val)=> {
        this.setState({
            ...this.state, 
            activeCom: val
        })
    }
      render(){
        
          return(
              <div>
                    <div className="headerArea">
                        <h1>
                            <span className="pull-left backButton">
                                <a href="javascript:void(0);" onClick={()=> {this.props.changComponent('home')}}>
                                <i className="fa fa-arrow-left"></i></a>
                            </span>Customer Relationship Mangement
                        </h1>
                    </div>
                    <div className="row">
                        <div className="col-md-12">
                            <div className="cardContainer mt-3">
                                <div className="cardContent">
                                    <div className="relative swArea">
                                        <Swiper
                                    spaceBetween={30}
                                    slidesPerView={4}
                                    navigation
                                    onSwiper={(swiper) => console.log(swiper)}
                                    >
                                        <SwiperSlide>
                                            <div className={this.state.activeCom == "Activities" ? "widget c1 DisableClass" : "widget c1"} onClick={() =>{this.viewSubComp("Activities")}}>
                                                <div className="textInbox">
                                                    <div style={{padding:"10px"}}><img src="../assests/images/DashboardIcons/bx1.png" /></div>
                                                    <div className="wdgLabel">Activities</div>
                                                </div>
                                                <div className="iconBox"><i className="fa fa-arrow-down"></i></div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className={this.state.activeCom == "Campaign" ? "widget c2 DisableClass" : "widget c2"} onClick={() =>{this.viewSubComp("Campaign")}}>
                                                <div className="textInbox">
                                                    <div style={{padding:"10px"}}><img src="../assests/images/DashboardIcons/bx1.png" /></div>
                                                    <div className="wdgLabel">Campaign</div>
                                                </div>
                                                <div className="iconBox"><i className="fa fa-arrow-down"></i></div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className={this.state.activeCom == "Leads" ? "widget c3 DisableClass" : "widget c3"} onClick={() =>{this.viewSubComp("Leads")}}>
                                                <div className="textInbox">
                                                    <div style={{padding:"10px"}}><img src="../assests/images/DashboardIcons/bx1.png" /></div>
                                                    <div className="wdgLabel">Leads</div>
                                                </div>
                                                <div className="iconBox"><i className="fa fa-arrow-down"></i></div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className={this.state.activeCom == "opportunity" ? "widget c4 DisableClass" : "widget c4"} onClick={() =>{this.viewSubComp("opportunity")}}>
                                                <div className="textInbox">
                                                    <div style={{padding:"10px"}}><img src="../assests/images/DashboardIcons/bx1.png" /></div>
                                                    <div className="wdgLabel">Opportunity</div>
                                                </div>
                                                <div className="iconBox"><i className="fa fa-arrow-down"></i></div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className={this.state.activeCom == "Accounts" ? "widget c5 DisableClass" : "widget c5"}>
                                                <div className="textInbox">
                                                    <div style={{padding:"10px"}}><img src="../assests/images/DashboardIcons/bx1.png" /></div>
                                                    <div className="wdgLabel">Accounts</div>
                                                </div>
                                                <div className="iconBox"><i className="fa fa-arrow-down"></i></div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className={this.state.activeCom == "Contacts" ? "widget c6 DisableClass" : "widget c6"}>
                                                <div className="textInbox">
                                                    <div style={{padding:"10px"}}><img src="../assests/images/DashboardIcons/bx1.png" /></div>
                                                    <div className="wdgLabel">Contacts</div>
                                                </div>
                                                <div className="iconBox"><i className="fa fa-arrow-down"></i></div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className={this.state.activeCom == "Cases" ? "widget c7 DisableClass" : "widget c7"}>
                                                <div className="textInbox">
                                                    <div style={{padding:"10px"}}><img src="../assests/images/DashboardIcons/bx1.png" /></div>
                                                    <div className="wdgLabel">Cases</div>
                                                </div>
                                                <div className="iconBox"><i className="fa fa-arrow-down"></i></div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className={this.state.activeCom == "Services" ? "widget c8 DisableClass" : "widget c8"}>
                                                <div className="textInbox">
                                                    <div style={{padding:"10px"}}><img src="../assests/images/DashboardIcons/bx1.png" /></div>
                                                    <div className="wdgLabel">Services</div>
                                                </div>
                                                <div className="iconBox"><i className="fa fa-arrow-down"></i></div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className={this.state.activeCom == "Collataral" ? "widget c2 DisableClass" : "widget c2"}>
                                                <div className="textInbox">
                                                    <div style={{padding:"10px"}}><img src="../assests/images/DashboardIcons/bx1.png" /></div>
                                                    <div className="wdgLabel">Collataral</div>
                                                </div>
                                                <div className="iconBox"><i className="fa fa-arrow-down"></i></div>
                                            </div>
                                        </SwiperSlide>
                                    </Swiper>
                                    </div>
                                    {/* components */}
                                    {
                                        this.state.activeCom == "opportunity" ?  <Opportunity /> : null
                                    }
                                    {
                                        this.state.activeCom == "Activities" ?  <Activities /> : null
                                    }
                                    {
                                        this.state.activeCom == "Campaign" ?  <Campaign /> : null
                                    }
                                    {
                                        this.state.activeCom == "Leads" ?  <Leads /> : null
                                    }
                                   
                                </div>
                            </div>
                        </div>
                    </div>
              </div>
          )
      }
}
export default  CRMdash;