import React from 'react';
import axios from 'axios';
import { DatePicker, Button, Select, Input } from 'antd';
const { Option } = Select;
const { RangePicker } = DatePicker;
import moment from 'moment';
import { SearchOutlined } from '@ant-design/icons';

import { Tabs } from 'antd';
const { TabPane } = Tabs;
// multi popup


import('./salesDb.css');

import TotalSales from './sections/totalSales';
import TotalReceipt from './sections/totalReceipt';
import TotalDue from './sections/totalDue';
import TopNSalesman  from './sections/topNSalesman';
import TopNCustomer from './sections/topNCustomer';
import TotalOrder from './sections/totalOrder';

import whatWeek , { getDateRangeOfWeek} from '../helpers/helperFunction'
import ListSelectModal from '../UIComponents/listSelectionModal'
// columns for popup
const columnsClass =[
  {
      title: 'Class Name',
      dataIndex: 'Name',
  }
]
const columnsProduct =[
  {
      title: 'Code',
      dataIndex: 'Code',
  },{
      title: 'Name',
      dataIndex: 'Name',
  },{
      title: 'Hsn',
      dataIndex: 'Hsn',
  }
]
class SalesDB extends React.Component {
  constructor(props) {
    super(props);
  }
    state = {
        totSalesAmount: 0,
        startDate: new Date(),
        endDate: new Date(),
        saleArr: [],
        activeTab:'TotalSale',
        ReceiptArr: [],
        totReceiptAmount: 0,
        DueArr: [],
        totDueAmount: 0,
        salesmanArr: [],
        customerArr: [],
        orderArr: [],
        orderAmount:0,
        branchOption :[],
        branchValue: 0,
        // Class modal
        showClassModal: false,
        classData:[],
        classKeys: [],
        classSelected:[{
            key: "", 
            id: "", 
            Name: "", 
        }],
        // Product selection 
        // class selection states
        showProductModal: false,
        ProductData:[],
        ProductKeys: [],
        ProductSelected:[{
            key: "", 
            id: "", 
            Code: "",
            Name: "",
            Hsn: "",
        }],
        rangeS: new Date(),
        rangeE: new Date(),
        dateDisabled:false
      }
      componentDidMount() {
        console.log(this.state)
        var date =  moment(new Date()).format("DD-MM-YYYY");
        let today = this.changeDateFormat(date)
        this.getToSaleData(today, today)

        // getting branch
        axios({
          method: 'post',
          url: '../ajax/inventory/inventoryDB.aspx/GetBaranchData',
          data: {}
        }).then(async (res) => {
            console.log('GetBaranchData', res)
            let branchData = res.data.d;
              this.setState({
                  ...this.state,
                  branchOption:branchData,
                  branchValue:branchData[0].value
              })
        }); 
    }
    //Get first Tab data 
    getToSaleData = (fromDate, toDate) =>{
      const branchid = this.state.branchValue;
      const ProdClass = this.state.classKeys.toString();
      const Prodid = this.state.ProductKeys.toString();

      axios({
        method: 'post',
        url: '../ajax/Sales/sales.aspx/LoadTotalSale',
        data: {
          FromDtae : fromDate,
          toDate: toDate,
          branchid: branchid,
          ProdClass: ProdClass,
          Prodid: Prodid
        }
      }).then(res => {
          console.log('LoadTotalSale', res.data);
          let chartData = res.data.d.AxisValue;
          let totalAmount= res.data.d.totValue;
          this.setState({
            ...this.state,
            totSalesAmount:totalAmount,
            saleArr:chartData 
          });
          console.log(this.state)
      });
    }
    // get Receipt challan
    getTotalReceiptData = (fromDate, toDate) =>{
      const branchid = this.state.branchValue;
      const ProdClass = this.state.classKeys.toString();
      const Prodid = this.state.ProductKeys.toString();
      axios({
        method: 'post',
        url: '../ajax/Sales/sales.aspx/LoadTotalReceipt',
        data: {
          FromDtae : fromDate,
          toDate: toDate,
          branchid: branchid,
          ProdClass: ProdClass,
          Prodid: Prodid
        }
      }).then(res => {
          console.log('LoadTotalReceipt', res.data);
          let recData = res.data.d.AxisValue;
          let recAmount= res.data.d.totValue;
          this.setState({
            ...this.state,
            totReceiptAmount:recAmount,
            ReceiptArr:recData 
          });
      });
    }
    //LoadTotalDue 
    getTotalDueData = (fromDate, toDate) =>{
      const branchid = this.state.branchValue;
      const ProdClass = this.state.classKeys.toString();
      const Prodid = this.state.ProductKeys.toString();
      axios({
        method: 'post',
        url: '../ajax/Sales/sales.aspx/LoadTotalDue',
        data: {
          FromDtae : fromDate,
          toDate: toDate,
          branchid: branchid,
          ProdClass: ProdClass,
          Prodid: Prodid
        }
      }).then(res => {
        let dueData = res.data.d.AxisValue;
        let dueAmount= res.data.d.totValue;
        this.setState({
          ...this.state,
          totDueAmount:dueAmount,
          DueArr:dueData 
        });
      });
    }
    //LoadTotalOrder 
    getTotalOrderData = (fromDate, toDate) =>{
      const branchid = this.state.branchValue;
      const ProdClass = this.state.classKeys.toString();
      const Prodid = this.state.ProductKeys.toString();
      axios({
        method: 'post',
        url: '../ajax/Sales/sales.aspx/LoadTotalOrder',
        data: {
          FromDtae : fromDate,
          toDate: toDate,
          branchid: branchid,
          ProdClass: ProdClass,
          Prodid: Prodid
        }
      }).then(res => {
          console.log('LoadTotalOrder', res.data);
          let orderData = res.data.d.AxisValue;
          let orderAmount = res.data.d.totValue;
          this.setState({
            ...this.state,
            orderArr: orderData,
            orderAmount: orderAmount
          });
      });
    }
    //GetTopNSalesMan 
    getSalesmanData = (fromDate, toDate) =>{
      const branchid = this.state.branchValue;
      const ProdClass = this.state.classKeys.toString();
      const Prodid = this.state.ProductKeys.toString();
    
      axios({
        method: 'post',
        url: '../ajax/Sales/sales.aspx/GetTopNSalesMan',
        data: {
          FromDtae : fromDate,
          toDate: toDate,
          branchid: branchid,
          ProdClass: ProdClass,
          Prodid: Prodid
        }
      }).then(res => {
        let salesmanData = res.data.d;
        this.setState({
          ...this.state,
          salesmanArr:salesmanData 
        });
      });
    }
    //LoadnewCust 
    getNewCustData = (fromDate, toDate) =>{
      const branchid = this.state.branchValue;
      const ProdClass = this.state.classKeys.toString();
      const Prodid = this.state.ProductKeys.toString();
      axios({
        method: 'post',
        url: '../ajax/Sales/sales.aspx/LoadnewCust',
        data: {
          FromDtae : fromDate,
          toDate: toDate,
          branchid: branchid,
          ProdClass: ProdClass,
          Prodid: Prodid
        }
      }).then(res => {
          console.log('LoadnewCust', res.data);
      });
    }
    //GetTopNCustomer 
    getTopNCustData = (fromDate, toDate) =>{
      const branchid = this.state.branchValue;
      const ProdClass = this.state.classKeys.toString();
      const Prodid = this.state.ProductKeys.toString();
      axios({
        method: 'post',
        url: '../ajax/Sales/sales.aspx/GetTopNCustomer',
        data: {
          FromDtae : fromDate,
          toDate: toDate,
          branchid: branchid,
          ProdClass: ProdClass,
          Prodid: Prodid
        }
      }).then(res => {
          console.log('GetTopNCustomer', res.data);
          let custData = res.data.d;
          this.setState({
            ...this.state,
            customerArr:custData 
          });
      });
    }
    onSearchClick = () => {
      var x =  moment(this.state.rangeS).format("DD-MM-YYYY");
      let y =  moment(this.state.rangeE).format("DD-MM-YYYY");
      let startDate = this.changeDateFormat(x);
      let endDate = this.changeDateFormat(y)
      this.setState({
        ...this.state,
        //activeTab: 'TotalSale'
      })
      const mKey = this.state.activeTab;
      
      this.loadDataforSpecificTab(mKey)
    }
    numberWithCommas = (x) => {
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
    changeDateFormat = (inputDate) =>{ 
      var splitDate = inputDate.split('-');
      if (splitDate.count == 0) {
          return null;
      }
      var year = splitDate[2];
      var month = splitDate[1];
      var day = splitDate[0];
      return year + '-' + month + '-' + day;
    }
    componentWillUnmount() {
      if (this.chart) {
        this.chart.dispose();
      }
    }
    setDateValue = (date, dateString) => {
      console.log(dateString);
      let sDate = this.changeDateFormat(dateString[0]);
      let eDate = this.changeDateFormat(dateString[1]);
     
      this.setState({
        ...this.state,
        rangeS: new Date(sDate),
        rangeE: new Date(eDate)
      })
    }
    tabChange = (key) =>{
      var x =  moment(this.state.rangeS).format("DD-MM-YYYY");
      let y =  moment(this.state.rangeE).format("DD-MM-YYYY");
      let startDate = this.changeDateFormat(x);
      let endDate = this.changeDateFormat(y)
      console.log(key);
      this.loadDataforSpecificTab(key)
    }
    loadDataforSpecificTab = (key) => {
      var x =  moment(this.state.rangeS).format("DD-MM-YYYY");
      let y =  moment(this.state.rangeE).format("DD-MM-YYYY");
      let startDate = this.changeDateFormat(x);
      let endDate = this.changeDateFormat(y)
      console.log(startDate, endDate)
      if(key =='TotalSale'){
        this.setState({
          ...this.state,
          activeTab: 'TotalSale'
        })
        this.getToSaleData(startDate, endDate)
        return;
      }
      if(key =='TotalReceipt'){
        this.setState({
          ...this.state,
          activeTab: 'TotalReceipt'
        })
        this.getTotalReceiptData(startDate, endDate);
        return;
      }
      if(key =='TotalDue'){
        this.setState({
          ...this.state,
          activeTab: 'TotalDue'
        })
        this.getTotalDueData(startDate, endDate);
        return;
      }
      if(key =='NewCustomer'){
        this.setState({
          ...this.state,
          activeTab: 'NewCustomer'
        })
        this.getNewCustData(startDate, endDate);
        return;
      }
      if(key =='TotalOrder'){
        this.setState({
          ...this.state,
          activeTab: 'TotalOrder'
        })
        this.getTotalOrderData(startDate, endDate);
        return;
      }
      if(key =='Top10Salesman'){
        this.setState({
          ...this.state,
          activeTab: 'Top10Salesman'
        })
        this.getSalesmanData(startDate, endDate);
        return;
      }
      if(key =='Top10Customer'){
        this.setState({
          ...this.state,
          activeTab: 'Top10Customer'
        })
        this.getTopNCustData(startDate, endDate);
        return;
      }
    }

    // Branch functions
    handleChangeBranch = value => {
      this.setState({
        ...this.state,
          branchValue:value
      });
    };
    // class selection 
    getClass = () => {
      this.setState({
        ...this.state,
          showClassModal: true,
        });
  }
  clearClass = (e)=> {
      console.log(e);
      this.setState({
          ...this.state,
          classSelected:[]
      })
  }
  handleCancelClass = () =>{
      this.setState({
          ...this.state,
          showClassModal: false,
      });
  }
  handleOkClass = () => {
      this.setState({
          showClassModal: false,
        });
  }
  searchClass = (field)=>{
      console.log('search', field.searchCust)
      axios({
          method: 'post',
          url: '../ajax/Service/reactMaster.asmx/GetClass',
          data: {SearchKey:field.searchCust}
        }).then(async (res) => {
            console.log('GetClass', res)  
            let data = res.data.d;
            const arrayOfObj = data.map(({ id: key, ...rest }) => ({ key, ...rest }));
            console.log('arrayOfObj', arrayOfObj)
           this.setState({
               ...this.state,
               classData: arrayOfObj
           })  
        });
  }
  // Product selection
  // class selection 
  getProduct = () => {
      this.setState({
          showProductModal: true,
        });
  }
  clearProduct = (e)=> {
      console.log(e);
      this.setState({
          ...this.state,
          ProductSelected:[]
      })
  }
  handleCancelProduct = () =>{
      this.setState({
          ...this.state,
          showProductModal: false,
      });
  }
  handleOkProduct = () => {
      this.setState({
          showProductModal: false,
        });
  }
  searchProduct = (field)=>{
      console.log('search', field.searchCust)
      let ClassID = this.state.classKeys.join(", ");
      let SearchKey = field.searchCust;
      if(ClassID == undefined){
          console.log(ClassID)
          ClassID ="";
      }
      if(SearchKey == undefined){
          return;
      }
      axios({
          method: 'post',
          url: '../ajax/Service/reactMaster.asmx/GetClassWiseProduct',
          data: {SearchKey:SearchKey, ClassID: ClassID}
        }).then(async (res) => {
            console.log('GetClassWiseProduct', res)  
            let data = res.data.d;
            const arrayOfObj = data.map(({ id: key, ...rest }) => ({ key, ...rest }));
            console.log('arrayOfObj', arrayOfObj)
           this.setState({
               ...this.state,
               ProductData: arrayOfObj
           })  
        });
  }
  // custom Date set
  
  handleChangeCustomDate =(value) =>{
    let opt= { 
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',}
    if (value == "0") {
      this.setState({
        //rangeS:new Date(), 
        //rangeE: new Date(),
        dateDisabled:false
      })
    } else if (value == "WTD") {
        var today = new Date();
        var currentWeekNumber = today.whatWeek();
        var wTD = getDateRangeOfWeek(currentWeekNumber);
        var slicd = wTD.split(" ");
        var fst = slicd[0].replaceAll("/", "-");
        var lst = slicd[2].replaceAll("/", "-");
  
        this.setState({
          rangeS:new Date(fst), 
          rangeE: new Date(lst),
          dateDisabled:true
        })
    } else if (value == "MTD") {
        var date = new Date();
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1).toLocaleDateString('en-US',opt).replaceAll("/", "-");
        // cFormDate.SetDate(new Date(firstDay));
        // ctoDate.SetDate(new Date());
        // cFormDate.SetEnabled(false);
        // ctoDate.SetEnabled(false);
        this.setState({
          rangeS:new Date(firstDay), 
          rangeE: new Date(),
          dateDisabled:true
        })
    } else if (value == "YTD") {
        var date = new Date();
        var firstDay = new Date(date.getFullYear(), 3, 1).toLocaleDateString('en-US',opt).replaceAll("/", "-");
        this.setState({
          rangeS:new Date(firstDay), 
          rangeE: new Date(),
          dateDisabled:true
        })
    } else if (value == "QTD") {
      

    } else {
       // cFormDate.SetEnabled(false);
       // ctoDate.SetEnabled(false);
    }
}
    render() {
        const dateFormat = 'DD-MM-YYYY';
        const options = this.state.branchOption.map(d => <Option key={d.value}>{d.label}</Option>);
        // For modal selected items
        const Selectedclass = this.state.classSelected.map(item=>{
              let arr=[];
              arr.push(item.Name);
              return arr.join(", ");
          });
          const SelectedProduct = this.state.ProductSelected.map(item=>{
              let arrP=[];
              arrP.push(item.Name);
              return arrP.join(", ");
          });
          //  Class modal
        const rowSelectionClass = {
          onChange: (selectedRowKeys, selectedRows) => {
            console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows);
            this.setState({
                ...this.state,
                classKeys: selectedRowKeys,
                classSelected: selectedRows,
            })
            console.log('to state', this.state)
          }
      }
      //  Product modal
      const rowSelectionProduct = {
          onChange: (selectedRowKeys, selectedRows) => {
            console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows);
            this.setState({
                ...this.state,
                ProductKeys: selectedRowKeys,
                ProductSelected: selectedRows,
            })
            console.log('to state', this.state)
          }
      }
      
      
        return (
            <div>
              <ListSelectModal 
                    title="Select Product Class"
                    visible={this.state.showClassModal}
                    onOkClick={this.handleOkClass}
                    classes='searchModal'
                    widthSize={1000}
                    onCancelProp={this.handleCancelClass}
                    onformFinish={this.searchClass}
                    colData={columnsClass}
                    sourcedata={this.state.classData}
                    selectType='checkbox'
                    rowSelection={rowSelectionClass}
                     />
                  {/* End */}
                  <ListSelectModal 
                    title="Select Product "
                    visible={this.state.showProductModal}
                    onOkClick={this.handleOkProduct}
                    classes='searchModal'
                    widthSize={1000}
                    onCancelProp={this.handleCancelProduct}
                    onformFinish={this.searchProduct}
                    colData={columnsProduct}
                    sourcedata={this.state.ProductData}
                    selectType='checkbox'
                    rowSelection={rowSelectionProduct}
                     />
                <div className="headerArea">
                    <h1>
                        <span className="pull-left backButton">
                            <a href="javascript:void(0);" onClick={()=> {this.props.changComponent('home')}}>
                                <i className="fa fa-arrow-left"></i>
                            </a>
                        </span>Sales Dashboard
                        <span className="pull-right optionHolder">
                            
                        </span>
                    </h1>
                </div>
                <div className="row">
                    <div className="col-md-12">
                        <div className="cardContainer mt-3">
                            <div className="cardContent">
                              <div className="row">
                                <div className="col-md-3">
                                        <label className="bold-lebel mTop0">Select Branch</label>
                                        <div>
                                        <Select
                                            style={{ width: "100%" }}
                                            showSearch
                                            value={this.state.branchValue}
                                            placeholder="Select Branch"
                                            defaultActiveFirstOption={true}
                                            showArrow={true}
                                            filterOption={false}
                                            //onSearch={this.handleSearch}
                                            onChange={this.handleChangeBranch}
                                            filterOption={(input, option) =>
                                                option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                                            }
                                            allowClear={true}
                                        >
                                            {options}
                                        </Select>
                                        </div>
                                    </div>
                                 <div className="col-md-3 col-sm-4">
                                    <label className="bold-lebel mTop0">Select Date</label>
                                    <div>
                                      <RangePicker 
                                       
                                        value={[ moment(this.state.rangeS, dateFormat),
                                          moment(this.state.rangeE, dateFormat)
                                        ]}
                                        format={dateFormat}
                                        onChange={this.setDateValue}
                                        disabled={this.state.dateDisabled}
                                        allowClear={false}
                                       />
                                    </div>
                                 </div>
                                 <div className="col-md-2">
                                        <label className="bold-lebel mTop0">Select Class</label>
                                        <div>
                                        <Input 
                                            placeholder="Select Class"
                                            name="clas"
                                            type="text" 
                                            id="classInput"
                                            onChange={(e) => {this.clearClass(e)}}                                          
                                            value={Selectedclass}
                                            onClick={this.getClass} 
                                            allowClear

                                                />
                                        </div>
                                    </div>
                                    <div className="col-md-2">
                                        <label className="bold-lebel mTop0">Select Product</label>
                                        <div>
                                        <Input 
                                            placeholder="Select Product"
                                            name="clas"
                                            type="text" 
                                            id="classInput"
                                            onChange={(e) => {this.clearProduct(e)}}                                          
                                            value={SelectedProduct}
                                            onClick={this.getProduct} 
                                            allowClear
                                                />
                                        </div>
                                    </div>
                                    <div className="col-md-2">
                                      <label className="bold-lebel mTop0">Custom</label>
                                      <div>
                                      <Select defaultValue="0" style={{ width: "100%" }} onChange={this.handleChangeCustomDate}>
                                        <Option value="0">None</Option>
                                        <Option value="YTD">YTD</Option>
                                        <Option value="QTD">QTD</Option>
                                        <Option value="MTD">MTD</Option>
                                        <Option value="WTD">WTD</Option>
                                      </Select>
                                      </div>
                                    </div>
                                    <div style={{clear:'both'}}></div>
                                 <div className="col-md-1 col-sm-2">
                                    <div style={{marginTop:"9px"}}></div>
                                    <Button  type="primary" icon={<SearchOutlined />} onClick={this.onSearchClick}>Search</Button>
                                 </div>
                              </div>
                              <div className="row">
                                  <div className="col-md-12 mt-3">
                                  <Tabs activeKey={this.state.activeTab} onChange={this.tabChange}>
                                    <TabPane tab="Total Sale" key="TotalSale">
                                      <TotalSales data={this.state.saleArr} totAmount={this.state.totSalesAmount} charttitle="Sales" />
                                    </TabPane>
                                    <TabPane tab="Total Receipt" key="TotalReceipt">
                                        <TotalReceipt data={this.state.ReceiptArr} totAmount={this.state.totReceiptAmount} charttitle="Receipt" />
                                    </TabPane>
                                    <TabPane tab="Total Due" key="TotalDue">
                                        <TotalDue data={this.state.DueArr} totAmount={this.state.totDueAmount} charttitle="Due" />
                                    </TabPane>
                                    <TabPane tab="Top 10 Salesman" key="Top10Salesman">
                                       <TopNSalesman  data={this.state.salesmanArr} />
                                    </TabPane>
                                    <TabPane tab="Top 10 Customer" key="Top10Customer">
                                      <TopNCustomer data={this.state.customerArr} />
                                    </TabPane>
                                    <TabPane tab="New Customer" key="NewCustomer">
                                      Content of Tab Pane 3
                                    </TabPane>
                                    <TabPane tab="Total Order" key="TotalOrder">
                                        <TotalOrder data={this.state.orderArr} totAmount={this.state.orderAmount} />
                                    </TabPane>
                                  </Tabs>
                                  </div>
                              </div>
                              
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
export default SalesDB;