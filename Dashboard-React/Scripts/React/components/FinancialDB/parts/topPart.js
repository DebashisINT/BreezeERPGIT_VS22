import React from 'react';
import SwiperCore, { Navigation, Pagination, Scrollbar, A11y } from 'swiper';
import { Swiper, SwiperSlide } from 'swiper/react';
import 'swiper/swiper-bundle.min.css';
SwiperCore.use([Navigation, Pagination, Scrollbar, A11y]);
import { Skeleton} from 'antd';
import * as helpers from '../../helpers/helperFunction';

const TopPart = (props) => {

    
    console.log(props)
    let view = <div className="cardContainer" style={{marginTop: "10px"}}><div className="text-center" style={{padding: "8px", color: "#69bdb2"}}>Procesing Financial Data.This might take some time..</div><Skeleton active /></div>
    if(Object.keys(props.data).length != 0){
        view = (<div className="clearfix" style={{marginTop: "10px"}}>
                <div className="totalContainer popi">
                    <div className="highlighter">
                        <div className="hdTag">REVENUE</div>
                        <div className="value">
                            <span title="">{helpers.numFormatterGlobal(props.data.Reveneu)}</span>
                        </div>
                        <div>Last Month 1.52 M ( +135%)</div>
                    </div>
                    <div className="fl-1 dataCont relative">
                        <div className="tabSlideContainer swArea">
                                <Swiper
                                    spaceBetween={10}
                                    slidesPerView={6}
                                    navigation
                                    onSwiper={(swiper) => console.log(swiper)}
                                    >
                                        <SwiperSlide>
                                            <div className="text-center">
                                                <div className="hdTag">Total COGS</div>
                                                <div className="value dataColor1">
                                                    <span id="tlCogs" title="">{helpers.numFormatterGlobal(props.data.cogs)}</span>
                                                </div>
                                                <div className="smallFont">Last Month 1.06 M ( -102.33%)</div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className="text-center">
                                                <div className="hdTag">Expenses</div>
                                                <div className="value dataColor1">
                                                    <span id="expens" title="">{helpers.numFormatterGlobal(props.data.Expenses)} </span> <span className="sIcon"><i className="fa fa-exclamation"></i></span>
                                                </div>
                                                <div className="smallFont">Last Month 0.46 M ( +219.4%)</div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className="text-center">
                                                <div className="hdTag">Gross Profit</div>
                                                <div className="value clPrim">
                                                    <span id="grsProfit" title="">{helpers.numFormatterGlobal(props.data.Gross_Profit)} </span> <span className="sIcon"><i className="fa fa-check"></i></span>
                                                </div>
                                                <div className="smallFont">Last Month 30.42%</div>	
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className="text-center">
                                                <div className="hdTag">Gross Profit %</div>
                                                <div className="value clPrim">
                                                    <span id="grsPrPer">{(Math.round(props.data.Gross_Profit_percent * 100) / 100).toFixed(2)}</span>%<span className="sIcon"><i className="fa fa-check"></i></span>
                                                </div>
                                                <div className="smallFont">Last Month -0.35 M ( +135%)</div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                        <div className="text-center">
                                            <div className="hdTag">Other Incomes</div>
                                            <div className="value clPrim">
                                                <span id="otherIncomes" title="">{helpers.numFormatterGlobal(props.data.Other_income)}</span><span className="sIcon"><i className="fa fa-check"></i></span>
                                            </div>
                                            <div className="smallFont">Last Month 23%</div>
                                        </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className="text-center">
                                                <div className="hdTag">Indirect Expenses</div>
                                                <div className="value dataColor1">
                                                    <span id="otherExpenses" title="">{helpers.numFormatterGlobal(props.data.Other_Expenses)}</span><span className="sIcon"><i className="fa fa-check"></i></span>
                                                </div>
                                                <div className="smallFont">Last Month 23%</div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className="text-center">
                                                <div className="hdTag">Net Profit</div>
                                                <div className="value clPrim">
                                                    <span id="netProfit" title="">{helpers.numFormatterGlobal(props.data.Nett_Profit)}</span><span className="sIcon"><i className="fa fa-check"></i></span>
                                                </div>
                                                <div className="smallFont">Goal 23%</div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className="text-center">
                                                <div className="hdTag">Net Profit % </div>
                                                <div className="value clPrim">
                                                    <span id="netPrPer">{(Math.round(props.data.Nett_Profit_percent * 100) / 100).toFixed(2)}</span>% <span className="sIcon"><i className="fa fa-check"></i></span></div>
                                                <div className="smallFont">Last Month 23%</div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className="text-center">
                                                <div className="hdTag">&nbsp;</div>
                                                <div className="value clPrim">
                                                    <span id="netPrPer">&nbsp;</span></div>
                                                <div className="smallFont">&nbsp;</div>
                                            </div>
                                        </SwiperSlide>
                                </Swiper>
                        </div>  
                    </div>
                </div>
            </div>
            )
    }
    return ( 
        <React.Fragment>
            {view}
        </React.Fragment>
     );
}
 
export default TopPart;