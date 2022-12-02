import React from 'react';
import axios from 'axios';
import { DatePicker, Button, Select, Input } from 'antd';
const { Option } = Select;
const { RangePicker } = DatePicker;
import moment from 'moment';
import { SearchOutlined } from '@ant-design/icons';

import UiBox from './UIBOX/uibox';
import TopItemsChart from './topItemsChart';
import TopVendorChart from './topvendorChart';

import ('./PurchaseAnalytics.css');
// product class and branch
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
// component start
class PurchaseAnalytics extends React.Component {
    constructor(props) {
      super(props);
    }
    state = {
        startDate: new Date(),
        endDate: new Date(),
        TotDue: "0.00",    
        TotalSale: "0.00",  
        totAdvance: "0.00",  
        totOrder: "0.00",  
        topNitems: [],
        topNvendors: [],
        frmDate: new Date(),
        toDate: new Date(),
        //branch value
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
    getboxData = (fromDate, toDate) =>{
        const branchid = this.state.branchValue;
        const ProdClass = this.state.classKeys.toString();
        const Prodid = this.state.ProductKeys.toString();
        axios({
          method: 'post',
          url: '../ajax/purchase/purchaseDb.aspx/GetPurchaseBalance',
          data: {
            FromDtae : fromDate,
            toDate: toDate,
            branchid: branchid,
            ProdClass: ProdClass,
            Prodid: Prodid
          }
        }).then(res => {
            console.log('box', res.data);
            this.setState({
                ...this.state,
                TotDue: res.data.d.TotDue,
                TotalSale: res.data.d.TotalSale,
                totAdvance: res.data.d.totAdvance,
                totOrder: res.data.d.totOrder
            })
            console.log('state', this.state)
        });
      }

    getitemsData = (fromDate, toDate) =>{
        const branchid = this.state.branchValue;
      const ProdClass = this.state.classKeys.toString();
      const Prodid = this.state.ProductKeys.toString();
        axios({
          method: 'post',
          url: '../ajax/purchase/purchaseDb.aspx/GetTopNPurchaseMan',
          data: {
            FromDtae : fromDate,
            toDate: toDate,
            branchid: branchid,
            ProdClass: ProdClass,
            Prodid: Prodid
          }
        }).then(res => {
            console.log('dta', res.data);
            this.setState({
                ...this.state,
                topNitems:res.data.d
            })
        });
      }
    getvendorData = (fromDate, toDate) =>{
        const branchid = this.state.branchValue;
        const ProdClass = this.state.classKeys.toString();
        const Prodid = this.state.ProductKeys.toString();
        axios({
          method: 'post',
          url: '../ajax/purchase/purchaseDb.aspx/GetTopNCustomer',
          data: {
            FromDtae : fromDate,
            toDate: toDate,
            branchid: branchid,
            ProdClass: ProdClass,
            Prodid: Prodid
          }
        }).then(res => {
            console.log('vendors', res.data);
            this.setState({
                ...this.state,
                topNvendors:res.data.d
            })
        });
      }
    onSearchClick = () => {
        var x =  moment(this.state.rangeS).format("DD-MM-YYYY");
        let y =  moment(this.state.rangeE).format("DD-MM-YYYY");
        let startDate = this.changeDateFormat(x);
        let endDate = this.changeDateFormat(y)

        console.log(startDate, endDate)
        this.getitemsData(startDate, endDate);
        this.getvendorData(startDate, endDate);
        this.getboxData(startDate, endDate);
      }
    
    componentWillUnmount() {   
    }

    // Branch functions
    handleChangeBranch = value => {
        this.setState({
            branchValue:value
        });
    };
    // class selection 
    getClass = () => {
        this.setState({
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
    //end 

      render(){
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
          return(
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
                                <i className="fa fa-arrow-left"></i></a>
                            </span>Purchase Analytics 
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
                                            <label className="bold-lebel" style={{marginTop:'0px'}}>Select Date</label>
                                            <div>
                                            <RangePicker 
                                                value={[ moment(this.state.rangeS, dateFormat),
                                                    moment(this.state.rangeE, dateFormat)
                                                ]}
                                                format={dateFormat}
                                                onChange={this.setDateValue}
                                                disabled={this.state.dateDisabled}
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

                                        <div className="col-md-3 col-sm-4">
                                            <div style={{marginTop:"9px"}}></div>
                                            <Button  type="primary" icon={<SearchOutlined />} onClick={this.onSearchClick}>Search</Button>
                                        </div>
                                    </div>
                                    <div className="row" style={{marginTop: "8px"}}>
                                            <UiBox 
                                                bgColor="#8076e9"
                                                uititle="Total Purchase" 
                                                data={this.state.TotalSale} 
                                                iconUrl="/assests/images/bx1.png" />
                                            <UiBox 
                                                bgColor="#d8b861"
                                                uititle="Total Due" 
                                                data={this.state.TotDue}  
                                                iconUrl="/assests/images/bx9.png" />
                                            <UiBox 
                                                bgColor="#1a8a9b"
                                                uititle="Total Payment" 
                                                data={this.state.totAdvance} 
                                                iconUrl="/assests/images/bx6.png" />
                                            <UiBox 
                                                bgColor="#f46b4b"
                                                uititle="Total Return" 
                                                data={this.state.totOrder} 
                                                iconUrl="/assests/images/bx10.png" />                                       
                                    </div>
                                    <div className="row">
                                        <div className="col-md-6">
                                            <div className="chartBox">
                                                <TopItemsChart data={this.state.topNitems} />
                                            </div>
                                        </div>
                                        <div className="col-md-6">
                                            <div className="chartBox">
                                                <TopVendorChart data={this.state.topNvendors} />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div> 
                        </div>
                    </div>
              </div>
          )
      }
      setDateValue = (date, dateString) =>{
        let sDate = this.changeDateFormat(dateString[0]);
        let eDate = this.changeDateFormat(dateString[1]);
        console.log(date.d, dateString)
        this.setState({
          ...this.state,
          rangeS: new Date(sDate),
          rangeE: new Date(eDate)
        })
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
}
export default  PurchaseAnalytics;