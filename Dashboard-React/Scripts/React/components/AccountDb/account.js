import React from 'react';
import axios from 'axios';
import DataTable, {defaultThemes} from 'react-data-table-component';
import { Progress } from 'antd';
import('./account.css');
//import * as am4core from '@amcharts/amcharts4/core';
import { useTheme, create, color, Rectangle, ready} from '@amcharts/amcharts4/core';
import { XYChart, DateAxis, ValueAxis, ColumnSeries, Bullet, LineSeries, XYCursor, CircleBullet, Legend } from "@amcharts/amcharts4/charts";
import am4themes_animated from "@amcharts/amcharts4/themes/animated";
class Account extends React.Component {
    constructor() {
        super();
        this.handleBranchChange = this.handleBranchChange.bind(this);
      }
      state = {
        showBranch: false,
        branch: [],
        banks: [],
        recentPayments: [],
        watchlist:[],
        DashboardAccDetails :[],
        bankAmount:0.00,

        // permissions
        Expensethismonth : false,
        bankBalance: false,
        incomeExpenseChart:false,
        RecentPayment: false,
        AccountWatchlist: false
     }
     genMonthwiseChart = (data) =>{
        console.log('stringP', data);
        ready(function () {
            useTheme(am4themes_animated);
            var chart2 = create("chartdiv", XYChart);
            //chart2.colors.step = 5;
            chart2.maskBullets = false;
            // Add data
            chart2.data = data;
            // Create axes
            var date2Axis = chart2.xAxes.push(new DateAxis());
            date2Axis.renderer.grid.template.location = 0;
            date2Axis.renderer.minGridDistance = 50;
            date2Axis.renderer.grid.template.disabled = true;
            date2Axis.renderer.fullWidthTooltip = true;
            var distance2Axis = chart2.yAxes.push(new ValueAxis());
            distance2Axis.title.text = "Income & Expenses";
            distance2Axis.renderer.grid.template.disabled = true;

            var duration2Axis = chart2.yAxes.push(new ValueAxis());
            duration2Axis.title.text = "PROFIT";
            //duration2Axis.baseUnit = "minute";
            //duration2Axis.renderer.grid.template.disabled = true;
            duration2Axis.renderer.opposite = true;
            var latitude2Axis = chart2.yAxes.push(new ValueAxis());
            latitude2Axis.renderer.grid.template.disabled = true;
            latitude2Axis.renderer.labels.template.disabled = true;
            // Create series
            var distance2Series = chart2.series.push(new ColumnSeries());
            distance2Series.dataFields.valueY = "REVENEU";
            distance2Series.dataFields.dateX = "MONTHYEAR";
            distance2Series.yAxis = distance2Axis;
            distance2Series.tooltipText = "Income : {valueY} ";
            distance2Series.name = "Income";
            distance2Series.columns.template.fillOpacity = 1;
            distance2Series.columns.template.propertyFields.strokeDasharray = "dashLength";
            distance2Series.columns.template.fill = color("#25c46f");
            //distance2Series.columns.template.propertyFields.fillOpacity = "alpha";
            var disatnce2State = distance2Series.columns.template.states.create("hover");
            disatnce2State.properties.fillOpacity = 0.9;
            var duration2Series = chart2.series.push(new ColumnSeries());
            duration2Series.dataFields.valueY = "COGS";
            duration2Series.dataFields.dateX = "MONTHYEAR";
            duration2Series.yAxis = distance2Axis;
            duration2Series.name = "Expense";
            duration2Series.strokeWidth = 0;
            duration2Series.propertyFields.strokeDasharray = "dashLength";
            duration2Series.tooltipText = "Expense : {valueY}";
            duration2Series.columns.template.fill = color("#e03434");
            var duration2Bullet = duration2Series.bullets.push(new Bullet());
            var durationRectangle = duration2Bullet.createChild(Rectangle);
            duration2Bullet.horizontalCenter = "middle";
            duration2Bullet.verticalCenter = "middle";
            duration2Bullet.width = 1;
            duration2Bullet.height = 1;
            durationRectangle.width = 1;
            durationRectangle.height = 1;
            var durationState = duration2Bullet.states.create("hover");
            durationState.properties.scale = 1.2;
            var latitude2Series = chart2.series.push(new LineSeries());
            latitude2Series.dataFields.valueY = "GROSS_PROFIT";
            latitude2Series.dataFields.dateX = "MONTHYEAR";
            latitude2Series.yAxis = duration2Axis;
            latitude2Series.name = "PROFIT";
            latitude2Series.strokeWidth = 2;
            latitude2Series.propertyFields.strokeDasharray = "dashLength";
            latitude2Series.tooltipText = "PROFIT: {valueY}";
            latitude2Series.stroke = color("#333333");
            latitude2Series.strokeDasharray = "8,4";
            var latitude2Bullet = latitude2Series.bullets.push(new CircleBullet());
            latitude2Bullet.circle.fill = color("#fff");
            latitude2Bullet.circle.strokeWidth = 2;
            latitude2Bullet.circle.propertyFields.radius = "0";
            var latitudeState = latitude2Bullet.states.create("hover");
            latitudeState.properties.scale = 1.2; 
            // Add legend
            chart2.legend = new Legend();
            // Add cursor
            chart2.cursor = new XYCursor();
            chart2.cursor.fullWidthLineX = true;
            //chart2.cursor.xAxis = dateAxis;
            chart2.cursor.lineX.strokeOpacity = 0;
            chart2.cursor.lineX.fill = color("#000");
            chart2.cursor.lineX.fillOpacity = 0.1;
        }); 
    }
      componentDidMount() {
        axios({
            method: 'post',url: '../ajax/accountDB/AccountDB.aspx/getPageloadPerm',data: {}
            })
            .then(res => {
                this.setState({
                    ...this.state,
                    Expensethismonth : res.data.d.Expensethismonth,
                    bankBalance: res.data.d.bankBalance,
                    incomeExpenseChart:res.data.d.incomeExpenseChart,
                    RecentPayment: res.data.d.RecentPayment,
                    AccountWatchlist: res.data.d.AccountWatchlist
                });
            })
            //Getting Branches list
            axios({
                method: 'post',
                url: '../ajax/Service/General.asmx/GetBranch',
                data: {}
              }).then(res => {
                  const branch = res.data.d;
                  this.setState({
                      ...this.state,
                      branch: branch
                  });
                  //Get bank
                  axios({
                    method: 'post',
                    url: '../ajax/Service/General.asmx/GetMainAccount',
                    data: {}
                  })
                  .then(res => {
                      console.log('Banks',res)
                      const banks = res.data.d;
                      this.setState({
                          ...this.state,
                          banks: banks
                      });
                      let bankId = this.refs.bankType.value;
                      let barnchId =this.refs.branchType.value;
                      this.getBankUpdate(bankId, barnchId);
                      this.getIncomeExpenseChart(barnchId);
                      this.getTablesData(barnchId)
                  });
              });
      }
      handleBranchChange(e) {
        var bankId = this.refs.bankType.value;
        let barnchId =this.refs.branchType.value;
        this.getBankUpdate(bankId, barnchId);
        this.getIncomeExpenseChart(barnchId);
        this.getTablesData(barnchId)
      }
      // get on bank change
      handleBankChange(e){
        var bankId = e.target.value;
        let barnchId =this.refs.branchType.value;
        this.getBankUpdate(bankId, barnchId);
      }
      getBankUpdate(bankId, barnchId){
        axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/getBankAmount',
            data: {
                BRANCHID :barnchId,
                BANKID :bankId
            }
          }).then(res => {
              let bakdata = this.numberWithCommas(res.data.d);
              this.setState({
                  ...this.state,
                  bankAmount:bakdata
              })
              console.log(this.state)
          });
      }
      getIncomeExpenseChart (barnchId) {
        axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/chartDataPull',
            data: {
                BRANCHID :barnchId
            }
          }).then(res => {
              console.log('fetch', res)
              //console.log('chartData', res);
              const chartArr =[];
              const expArr = res.data.d.Expenses;
              const IncomeArr = res.data.d.Income;
              const profitArr = res.data.d.profit;
              const MonthArr = res.data.d.Month;

              MonthArr.map((mnth, index)=> {
                  let year = new Date().getFullYear();
                  if(mnth == "April" || mnth == "May" || mnth == "June" || mnth == "July" ||  mnth == "August" || mnth == "September"|| mnth == "October"|| mnth == "November"|| mnth == "December"){
                    year = "2019"
                  }
                    let tempObj = {
                        MONTHYEAR:mnth+',' +year,
                        REVENEU: expArr[index].toString(),
                        COGS: IncomeArr[index].toString(),
                        GROSS_PROFIT: profitArr[index].toString()
                    }
                    chartArr.push(tempObj);
              });
              //console.log('newArr', chartArr)
              // Themes begin
              this.genMonthwiseChart(chartArr);
          });
      }
      
      getTablesData (barnchId) {
        axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/DashboardAccDetails',
            data: {
                BRANCHID :barnchId
            }
          }).then(res => {
              console.log('DashboardAccDetails', res); 
              this.setState({
                ...this.state,
                DashboardAccDetails: res.data.d
            });        
          });
          axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/watchlistDetails',
            data: {
                BRANCHID :barnchId
            }
          }).then(res => {
              console.log('watchlistDetails', res);  
              this.setState({
                ...this.state,
                watchlist: res.data.d
            });       
          });
          axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/DashboardRecentPayments',
            data: {
                BRANCHID :barnchId
            }
          }).then(res => {
              console.log('DashboardRecentPayments', res); 
              this.setState({
                ...this.state,
                recentPayments: res.data.d
            }); 
            console.log(this.state.recentPayments)     
          });
      }

        
    render() {
        //let isBranch = this.state.branch;
        let columns = [
            {
                name: 'Customer',
                sortable: false,
                selector: 'CUSTNAME'
            },
            {
                name: 'Invoice',
                selector: 'DOCNO',
                sortable: false,
                right: false,
            },
            {
                name: 'Date Paid',
                selector: 'DATE_PAID',
                sortable: false,
                right: false,
            },
            {
                name: 'Amount',
                selector: 'AMOUNT',
                sortable: false,
                right: true,
            }
        ];
        let watchColumns = [
            {
                name: 'Account',
                sortable: false,
                selector: 'MAINACCOUNT_NAME'
            },
            {
                name: 'This Month',
                selector: 'CURRBAL',
                sortable: false,
                right: true,
            },
            {
                name: 'Year to Date',
                selector: 'YTD',
                sortable: false,
                right: true,
            }
        ];
        const customStyles = {
            header: {
              style: {
                minHeight: '30px',
                color: 'white'
              },
            },
            headRow: {
              style: {
                borderTopStyle: 'transparent',
                borderTopWidth: '1px',
                background:'#333333',
                color: 'white',
                borderTopColor: defaultThemes.default.divider.default,
              },
            },
            headCells: {
              style: {
                '&:not(:last-of-type)': {
                  borderRightStyle: 'solid',
                  borderRightWidth: '1px',
                  borderRightColor: defaultThemes.default.divider.default,
                },
                color: 'white',
              },
            },
            cells: {
              style: {
                '&:not(:last-of-type)': {
                  borderRightStyle: 'solid',
                  borderRightWidth: '1px',
                  borderRightColor: defaultThemes.default.divider.default,
                },
              },
            },
        };

        const listItems = this.state.DashboardAccDetails.map((item, index) =>
            <tr>
                <td>{item.mainAccountName}</td>
                <td>{item.balance}</td>
                <td style={{width: "180px"}} className="relative">
                    <Progress percent={item.balancePercentage} size="small" status="active" />
               
                </td>
            </tr>
        );
        return (
            <div>
                <div className="headerArea">
                    <h1>
                        <span className="pull-left backButton">
                            <a href="javascript:void(0);" onClick={()=> {this.props.changComponent('home')}}>
                            <i className="fa fa-arrow-left"></i></a>
                        </span>Account Dashboard
                        <span className="pull-right optionHolder">
                            {this.state.branch ? (
                                <select className="selectBox" ref="branchType" onChange={this.handleBranchChange}>
                                    {this.state.branch.map(br => <option key={br.ID} value={br.ID}>{br.NAME}</option>)}
                                </select>
                            ) : (
                                null
                            )}
                        </span>
                    </h1>
                </div>
                <div style={{
                        padding:"20px"
                    }}>
                    <div className="row">
                        {/* Bank Balance Section */}
                        {
                            this.state.bankBalance ? (
                                <div className="col-md-4">
                                    <div className="cardContainer">
                                        <div className="cardHeader">Bank Balance As On Today</div>
                                        <div className="cardContent text-center">
                                            <div>Account Details</div>
                                            <div style={{maxWidth:"60%", margin: "15px auto"}}>
                                                <select className="selectBox" ref="bankType" onChange={this.handleBankChange.bind(this)}>
                                                {this.state.banks.map(bank => <option key={bank.ID} value={bank.ID} >{bank.NAME}</option>)}
                                                </select>
                                            </div>
                                            <div className="backInfo">
                                                <span id="lblCurrencySymbol" class="inr">INR</span>  
                                                <span id="balanceAmount">{this.state.bankAmount}</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            )
                            : null
                        }
                        {
                            this.state.Expensethismonth ? (
                                <div className="col-md-8">
                                    <div className="cardContainer">
                                        <div className="cardHeader">Expense This Month</div>
                                            <div className="cardContent" style={{
                                                maxHeight: "157px",
                                                overflowY: "auto",
                                                paddingTop:"0px"
                                            }}>
                                                <table className="table responsive BorderTopNone" 
                                                style={{marginBottom: "0px"}}>
                                                    <tbody>
                                                        {listItems}
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                
                            ) : null 
                        }
                    </div>
                    {
                        this.state.incomeExpenseChart ? (
                            <div className="row">
                                <div className="col-md-12">
                                    <div className="space"></div>
                                    <div className="cardContainer">
                                        <div className="cardHeader">Income & Expenses ( Last 12 Months )</div>
                                        <div className="cardContent">
                                            <div id="chartdiv" style={{height: "340px"}}></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        )  : null
                    }
                    
                    <div className="row">
                        <div className="space"></div>
                        {
                            this.state.RecentPayment ? (
                                <div className="col-md-6">
                                    <div className="cardContainer">
                                        <div className="cardHeader">Recent Payments (INR)</div>
                                        <div className="">
                                        <DataTable
                                            noHeader={true}
                                        // fixedHeader={true}
                                        // fixedHeaderScrollHeight="300px"
                                            pagination={true}
                                            responsive={true}
                                            columns={columns}
                                            data={this.state.recentPayments}
                                            customStyles={customStyles}
                                            dense
                                        />
                                        </div>
                                    </div>
                                </div>
                            ) : null 
                        }
                        {
                            this.state.AccountWatchlist ? (
                                <div className="col-md-6">
                                    <div className="cardContainer">
                                        <div className="cardHeader">Account Watchlist</div>
                                        <div className="">
                                        <DataTable
                                            noHeader={true}
                                            pagination={true}
                                            responsive={true}
                                            columns={watchColumns}
                                            data={this.state.watchlist}
                                            customStyles={customStyles}
                                            dense
                                            
                                        />
                                        </div>
                                    </div>
                                </div>
                            ) : null
                        } 
                    </div>
                </div>
            </div>
        );
    }
    numberWithCommas(x) {
        x = x.toString();
        if (x.toString().indexOf('.') == -1) {
            var lastThree = x.substring(x.length - 3);
            var otherNumbers = x.substring(0, x.length - 3);
            if (otherNumbers != '')
                lastThree = ',' + lastThree;
            var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
        } else {
            var dec = x.substr(x.indexOf('.') + 1, x.length);
            x = x.substr(0, x.indexOf('.'))
            var lastThree = x.substring(x.length - 3);
            var otherNumbers = x.substring(0, x.length - 3);
            if (otherNumbers != '')
                lastThree = ',' + lastThree;
            var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree + '.' + dec;
        }
        return res;
    }
    
}

export default Account;

