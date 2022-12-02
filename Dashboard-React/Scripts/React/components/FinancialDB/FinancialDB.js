import React, {Component} from 'react';
import('./finance.css');
import * as helpers from '../helpers/helperFunction';
import axios from 'axios';

import TopPart from './parts/topPart';
import ChartOne from './parts/chartOne';
import ChartTwo from './parts/chartTwo';
import Funnel from './parts/funnel';

class Finance extends Component {
    constructor(props) {
        super(props);
        this.state = { 
            topBox: {},
            assetData: {
                CUR_LIABILITY: "0",
                Cashbank_Amount: "0",
                Closing_Stock: "0",
                Current_Asset: "0",
                Deposit: "0",
                EQUITY: "0",
                Fixed_Asset: "0",
                LIABILITY: "0",
                Loan_Advance: "0",
                NON_CUR_LIABILITY: "0",
                PROVISION: "0",
                RELATED_PARTY: "0",
                TOTAL_PAY: "0",
                TOTAL_REC: "0",
                Total_Asset: "0",
            },
            chartData: [],
            funnelData: [],
         }
    }
    componentDidMount () {
        axios({
            method: 'post',
            url: '../ajax/FinanceKpi/FinanceKpi.aspx/GetTopBoxData',
            data: {}
          }).then(res => {
            let temp1 = res.data.d[0];
            let arrS = []
            Object.keys(temp1).map(key =>{
                let mk = key;
                if(mk =="Gross_Profit_percent"){ 
                        
                }else if(mk =="Other_income"){
                        
                }else if(mk =="Other_Expenses"){
                        
                }else if(mk =="Nett_Profit_percent"){
                        
                }else{
                    arrS.push({ name:key, value: temp1[key] })
                }
            });
            var newArr = arrS.sort((a, b) => Number(b.value) - Number(a.value));
            this.setState({
                ...this.state,
                topBox: res.data.d[0],
                funnelData: newArr 
            })
            console.log(this.state)
          });
         
          axios({
            method: 'post',
            url: '../ajax/FinanceKpi/FinanceKpi.aspx/GetchartsData',
            data: {}
          }).then(res => {
            //console.log('GetchartsData', res)
            this.setState({
                ...this.state,
                chartData: res.data.d
            })
          });
          axios({
            method: 'post',
            url: '../ajax/FinanceKpi/FinanceKpi.aspx/GetAssetsData',
            data: {}
          }).then(res => {
              console.log('asset', res.data.d)
            this.setState({
                ...this.state,
                assetData: res.data.d[0]
            })
          });
    }
    render() { 
        return ( 
            <React.Fragment>
                <div className="headerArea">
                        <h1>
                            <span className="pull-left backButton">
                                <a href="javascript:void(0);" onClick={()=> {this.props.changComponent('home')}}>
                                <i className="fa fa-arrow-left"></i></a>
                            </span>Financial KPI
                        </h1>
                    </div>
                    <TopPart data={this.state.topBox} />
                    <div className="row">
                        <div className="col-md-12">
                            <div className="cardContainer mt-3">
                                <div className="cardHeader">Operating Profit over Time</div>
                                <div className="cardContent">
                                     <ChartOne chartData={this.state.chartData} />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-12">
                            <div className="cardContainer mt-3">
                                <div className="cardHeader">Net Profit over Time</div>
                                <div className="cardContent">
                                    <ChartTwo chartData={this.state.chartData} />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-6">
                            <div className="cardContainer mt-3">
                                <div className="cardHeader">Assets And Liabilities</div>
                                <div className="clearfix">
                                    <div className="col-sm-9" 
                                        style={{
                                            borderLeft: "6px solid #69bdb2", 
                                            padding: "0 0 0 10px",
                                            color: "#3cad9e"
                                            }}>
                                        <div className="row">
                                            <div className="col-sm-4">
                                                <div id="Total_Asset" className="bigValue">{this.state.assetData.Total_Asset}</div>
                                                <div className="textS cll">Total Assets</div>

                                                <div id="Cashbank_Amount" className="bigValue">{this.state.assetData.Cashbank_Amount}</div>
                                                <div className="textS cll">Cash and Bank</div>
                                            </div>
                                            <div className="col-sm-4">
                                                <div id="Current_Asset" className="bigValue">{this.state.assetData.Current_Asset}</div>
                                                <div className="textS cll">Current Assets</div>

                                                <div id="Deposit" className="bigValue">{this.state.assetData.Current_Asset}</div>
                                                <div className="textS cll">Deposit,  adv and...</div>
                                            </div>
                                            <div className="col-sm-4">
                                                <div id="Fixed_Asset" className="bigValue">{this.state.assetData.Fixed_Asset}</div>
                                                <div className="textS cll">Fixed Assets</div>

                                                <div id="Closing_Stock" className="bigValue">{this.state.assetData.Closing_Stock}</div>
                                                <div className="textS cll">Inventory</div>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="col-sm-3" style={{paddingRight: "0"}}>
                                        <div className="col1Revert">
                                            <div id="TOTAL_REC" className="bigValue">{this.state.assetData.TOTAL_REC}</div>
                                            <div className="textS">Trade Received</div>
                                        </div>
                                    </div>
                                </div>
                                <div className="clearfix">
                                    <div className="col-sm-9" 
                                        style={{
                                            borderLeft: "6px solid #69bdb2",  
                                            padding: "0 0 0 10px",
                                            color: "#69bdb2"
                                            }}>
                                        <div className="row ">
                                            <div className="col-sm-4">
                                                <div id="LIABILITY" className="bigValue red">{this.state.assetData.LIABILITY}</div>
                                                <div className="textS cll">Liabilities & Equity</div>

                                                <div id="EQUITY" className="bigValue red">{this.state.assetData.EQUITY}</div>
                                                <div className="textS cll">Equity</div>
                                            </div>
                                            <div className="col-sm-4">
                                                <div id="CUR_LIABILITY" className="bigValue red">{this.state.assetData.CUR_LIABILITY}</div>
                                                <div className="textS cll">Current Liability</div>

                                                <div id="PROVISION" className="bigValue red">{this.state.assetData.PROVISION}</div>
                                                <div className="textS cll">Prov. & Accruals </div>
                                            </div>
                                            <div className="col-sm-4">
                                                <div id="NON_CUR_LIABILITY" className="bigValue red">{this.state.assetData.NON_CUR_LIABILITY}</div>
                                                <div className="textS cll">Non-Cur Liability</div>

                                                <div id="RELATED_PARTY" className="bigValue red">{this.state.assetData.RELATED_PARTY}</div>
                                                <div className="textS cll">Related Party Pybl</div>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="col-sm-3" style={{paddingRight: "0"}}>
                                        <div className="col2Revert">
                                            <div id="TOTAL_PAY" className="bigValue">{this.state.assetData.TOTAL_PAY}</div>
                                            <div className="textS">Trade Payables</div>
                                        </div>
                                    </div>
                                </div>
                             </div>
                        </div>
                        <div className="col-md-6">
                            <div className="cardContainer mt-3">
                                <div className="cardHeader">KPI Funnel</div>
                                <div className="cardContent">
                                    <Funnel funnelData={this.state.funnelData} />
                                </div>
                            </div>
                        </div>
                    </div>
            </React.Fragment>
         );
    }
}
 
export default Finance;