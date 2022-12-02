import React from 'react';
import SwiperCore, { Navigation, Pagination, Scrollbar, A11y } from 'swiper';
import { Swiper, SwiperSlide } from 'swiper/react';
import 'swiper/swiper-bundle.min.css';
import('./InventoryDB.css');
import axios from 'axios';
import { Select, Input } from 'antd';
const { Option } = Select;
import ListSelectModal from '../UIComponents/listSelectionModal';
import { getCurrentDate } from '../helpers/helperFunction';
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

import asyncComponent from '../AsyncComponent/AsyncComponent';
const StockStatus = asyncComponent(() => {
    return import('./sections/stockStatus');
});
const TopNProducts = asyncComponent(() => {
    return import('./sections/TopNProduct');
});
const ActualVdemand = asyncComponent(() => {
    return import('./sections/ActualVdemand');
});
const StockRequisition = asyncComponent(() => {
    return import('./sections/StockRequisition');
});
const Procurement = asyncComponent(() => {
    return import('./sections/Procurement');
});
const StockAlert = asyncComponent(() => {
    return import('./sections/StockAlert');
});
SwiperCore.use([Navigation, Pagination, Scrollbar, A11y]);
class InventoryDB extends React.Component {
    constructor(props) {
      super(props);
    }
    state = {
        activeCom : "Stock",
        branchOption :[],
        branchValue: 0, 
        // class selection states
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
        prData:{
            APPRPO: "0",
            APPRREQ: "0",
            OPENREQ: "0",
            PURCHASEREQ: "0",
            TOTPO: "0"
        }
    }  
    componentDidMount() {
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
         
          this.getProcurementData();

    } 
    getProcurementData = ()=> {
        const prodArr = this.state.ProductKeys;
        const classArr = this.state.classKeys;
        let obj={}
            obj.date = getCurrentDate();
            obj.branchid= parseInt(this.state.branchValue);
            obj.ProdClass= classArr.join();
            obj.Prodid= prodArr.join();
            console.log("getProcurementData", obj)
        //     //return;
        axios({
            method: 'post',
            url: '../ajax/inventory/inventoryDB.aspx/GetProcurement',
            data: obj
          }).then((res) => {
              console.log('GetProcurement', res)
              this.setState({
                  ...this.state,
                  prData:res.data.d[0]
              })
          }); 
    }
    handleChangeBranch = value => {
        this.setState({
            branchValue:value
        });
      };     
    componentWillUnmount() {   
    }
    viewSubComp = (val)=> {
        this.setState({
            ...this.state, 
            activeCom: val
        })
    }
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

      render(){
         const carStyle ={
                background: "#fff",
                borderRadius: "8px",
                width: "100%",
                padding: "8px 15px",
                boxShadow: "0px 3px 5px rgb(0 0 0 / 4%)"
         }
         const options = this.state.branchOption.map(d => <Option key={d.value}>{d.label}</Option>);
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
                  {/* class modal */}
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
                            </span>Inventory Dashboard
                        </h1>
                    </div>
                    <div className="row">
                        <div className="col-md-12">
                            <div className="clearfix mt-3" style={carStyle}>
                                <div className="cardContent">
                                <div className="row">
                                    <div className="col-md-3">
                                        <label>Select Branch</label>
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
                                    <div className="col-md-3">
                                        <label>Select Class</label>
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
                                    <div className="col-md-3">
                                        <label>Select Product</label>
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
                                    </div>
                                </div>
                            </div>
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
                                            <div className={this.state.activeCom == "Stock" ? "widget c1 DisableClass" : "widget c1"} onClick={() =>{this.viewSubComp("Stock")}}>
                                                <div className="textInbox">
                                                    <div style={{padding:"10px"}}><img src="../assests/images/DashboardIcons/bx1.png" /></div>
                                                    <div className="wdgLabel">Stock Status</div>
                                                </div>
                                                <div className="iconBox"><i className="fa fa-arrow-down"></i></div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className={this.state.activeCom == "TopNProd" ? "widget c2 DisableClass" : "widget c2"} onClick={() =>{this.viewSubComp("TopNProd")}}>
                                                <div className="textInbox">
                                                    <div style={{padding:"10px"}}><img src="../assests/images/DashboardIcons/bx1.png" /></div>
                                                    <div className="wdgLabel">Top N Product</div>
                                                </div>
                                                <div className="iconBox"><i className="fa fa-arrow-down"></i></div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className={this.state.activeCom == "ActualVdemand" ? "widget c3 DisableClass" : "widget c3"} onClick={() =>{this.viewSubComp("ActualVdemand")}}>
                                                <div className="textInbox">
                                                    <div style={{padding:"10px"}}><img src="../assests/images/DashboardIcons/bx1.png" /></div>
                                                    <div className="wdgLabel">Actual vs Demand</div>
                                                </div>
                                                <div className="iconBox"><i className="fa fa-arrow-down"></i></div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className={this.state.activeCom == "StockAlert" ? "widget c4 DisableClass" : "widget c4"} onClick={() =>{this.viewSubComp("StockAlert")}}>
                                                <div className="textInbox">
                                                    <div style={{padding:"10px"}}><img src="../assests/images/DashboardIcons/bx1.png" /></div>
                                                    <div className="wdgLabel">Stock Alert</div>
                                                </div>
                                                <div className="iconBox" ><i className="fa fa-arrow-down"></i></div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className={this.state.activeCom == "StockRequisition" ? "widget c5 DisableClass" : "widget c5"} onClick={() =>{this.viewSubComp("StockRequisition")}}>
                                                <div className="textInbox">
                                                    <div style={{padding:"10px"}}><img src="../assests/images/DashboardIcons/bx1.png" /></div>
                                                    <div className="wdgLabel">Stock Requisition</div>
                                                </div>
                                                <div className="iconBox" ><i className="fa fa-arrow-down"></i></div>
                                            </div>
                                        </SwiperSlide>
                                        <SwiperSlide>
                                            <div className={this.state.activeCom == "Procurement" ? "widget c6 DisableClass" : "widget c6"} onClick={() =>{this.viewSubComp("Procurement")}}>
                                                <div className="textInbox">
                                                    <div style={{padding:"10px"}}><img src="../assests/images/DashboardIcons/bx1.png" /></div>
                                                    <div className="wdgLabel">Procurement Requisition</div>
                                                </div>
                                                <div className="iconBox" ><i className="fa fa-arrow-down"></i></div>
                                            </div>
                                        </SwiperSlide>
                                    </Swiper>
                                    </div>
                                    {/* components */}
                                    {
                                        this.state.activeCom == "Stock" ?  <StockStatus /> : null
                                    }
                                    {
                                        this.state.activeCom == "TopNProd" ?  <TopNProducts /> : null
                                    }
                                    {
                                        this.state.activeCom == "ActualVdemand" ?  <ActualVdemand /> : null
                                    }
                                    {
                                        this.state.activeCom == "StockRequisition" ?  <StockRequisition /> : null
                                    }
                                    {
                                        this.state.activeCom == "Procurement" ?  <Procurement data={this.state.prData} /> : null
                                    }
                                    {
                                        this.state.activeCom == "StockAlert" ?  <StockAlert /> : null
                                    }
                                </div>
                            </div>
                        </div>
                    </div>
              </div>
          )
      }
}
export default  InventoryDB;