import React, { Component, lazy } from 'react';
import { Input, DatePicker, Button, Form, Modal, Table, Skeleton, Icon } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import Highlighter from 'react-highlight-words';
import axios from 'axios';
import { convertToDate } from '../helpers/helperFunction';
import('./ProjectManagement.css');
import moment from 'moment';
// component import
import ListSelectModal from '../UIComponents/listSelectionModal'
const Tabs = lazy(() => import('./controls/tab'));
const ProjectSummary = lazy(() => import('./tabviews/projectsummary'));
const ProjectDetails = lazy(() => import('./tabviews/projectDetails'));
const Timeline = lazy(() => import('./tabviews/timeline'));
const CostBreakup = lazy(() => import('./tabviews/costBreakup'));



class ProjectManagement extends React.Component {
    constructor(props) {
        super(props);  
    }
    state = { 
        // ajax loader
        loading: false,
        // object for dataCall
        obj : {
            toDate: '',
            SearchKey:'',
            partyId: ''
        },
        // Data for projects
        projectdata : [],
        // data for customer search 
        customerData: [],
        visible: false,
        projectCtl: false,
        projectDisable: false,
        selectedRowKeys: [],
        projectSlected:[],
        customerKeys: 0,
        customerSelected:{
            key: "", 
            name: "", 
            age: "", 
            address: ""
        },
        activeCompo : "ProjectSummary",
        projectSummaryData: {
            Cost_Booked: "0.00",
            Est_Balance: "0.00",
            Est_Revised: "0.00",
            Initial_Revenue: "0.00",
            Order_Value: "0.00",
            Profit: "0.00",
            RevProfit: "0.00",
            Revenue_balance: "0.00",
            Revenue_booked: "0.00",
            Revised: "0.00",
            initial_Est_Cost: "0.00"
        },
        costData1:[],
        costData2:[],
        // Project Details Data
        PieData:[],
        BarData:[],
        timelineData: [],
        //validation msg
        messageProject: '',
        // settings
        settings: {
            LiCostBreakup: false,
            LiProjectDetails: false,
            LiProjectSummary: false,
            LiTimeline: false
        }     
        
    }
    componentDidMount() {
        axios({
            method: 'post',url: '../ajax/projectManagement/projDb.aspx/GetSettings',data: {}
            })
            .then(res => {
                console.log('GetSettings', res)
                this.setState({
                    settings: res.data.d
                });
            });
        axios({
            method: 'post',
            url: '../ajax/projectManagement/projDb.aspx/GetProjects',
            data: {}
          }).then(res => {
              console.log("GetProjects",res)
              let data = res.data.d;
              const newArrayOfObj = data.map((element, index) => ({key: index ,...element}))
              //const newArrayOfObj = data.map(({ Proj_Code: key, ...rest }) => ({ key, ...rest }));
              console.log('newArrayOfObj', newArrayOfObj)
             this.setState({
                 ...this.state,
                 projectdata: newArrayOfObj
             })
          });
    }
    onHandleSearch = fields=>{
        var data1 = document.getElementById('customerInput').value;
        var prjArr =  this.state.projectSlected.map(function (el) { return el.Proj_Id; });
        let data2 = prjArr.join(',');
        let data3 = fields['toDate'].format('YYYY-MM-DD');
        let ActiveCom = this.state.activeCompo;

        let queryobj= {
            toDate: data3,
            SearchKey:data2,
            partyId: data1
        }
        if(data2 == '' || undefined){
            this.setState({
                ...this.state,
                messageProject: 'Project is Required'
            })
            return false;
        }else {
            this.setState({
                ...this.state,
                messageProject: ''
            })
        }
        
        this.setState({
            ...this.state,
            loading:true,
            obj: {
                toDate: data3,
                SearchKey:data2,
                partyId: data1
            }
        })
        if(ActiveCom =='ProjectSummary') {
            this.getProjectSum(queryobj);
        }else if(ActiveCom =='ProjectDetails'){
            this.getProjectDetails(queryobj);
        }else if(ActiveCom =='CostBreakup'){
            this.getCostBreakup(queryobj);
        }else if(ActiveCom =='Timeline'){
            this.getTimeline(queryobj);
        }else {
            this.getProjectSum(queryobj);
        }
        
    }
    getProjectSum = (obj)=>{
        axios({
            method: 'post',
            url: '../ajax/projectManagement/projDb.aspx/GetProjSum',
            data: obj
          }).then(res => {
              console.log(res) 
              this.setState({
                  ...this.state,
                  loading:false,
                  projectSummaryData: res.data.d[0]
              })
          });
    }
    // Get cost Breakup
    getCostBreakup = (obj) =>{
        axios({
            method: 'post',
            url: '../ajax/projectManagement/projDb.aspx/GetCostBreakUp',
            data: obj
          }).then(res => {
              console.log(res)
              this.setState({
                  ...this.state,
                  loading:false,
                  costData1: res.data.d[0]
              })
          });
          axios({
            method: 'post',
            url: '../ajax/projectManagement/projDb.aspx/GetCostBreakUpR',
            data: obj
          }).then(res => {
              console.log('GetCostBreakUpR', res)
              this.setState({
                ...this.state,
                loading:false,
                costData2: res.data.d[0]
            })
          });
    }
    // Get Project Details
    getProjectDetails = (obj) =>{
        axios({
            method: 'post',
            url: '../ajax/projectManagement/projDb.aspx/GetProjDetail',
            data: obj
          }).then(res => {
              console.log('GetProjDetail', res)
              this.setState({
                  ...this.state,
                  loading:false,
                  PieData: res.data.d[0]
              })
          });
          axios({
            method: 'post',
            url: '../ajax/projectManagement/projDb.aspx/GetProjDetailBar',
            data: obj
          }).then(res => {
              console.log('GetProjDetailBar', res)
              this.setState({
                  ...this.state,
                  loading:false,
                  BarData: res.data.d
              })
          });
    }
    // Get TimeLine
    // Get Project Details
    getTimeline = async (obj) =>{
        console.log("timelineObj", obj)
        axios({
            method: 'post',
            url: '../ajax/projectManagement/projDb.aspx/GetTimeLineTble',
            data: obj
          }).then(res => {
              console.log('returned', res)
              this.setState({
                  ...this.state,
                  loading:false,
                  timelineData: res.data.d
              })
              //console.log('returned State', this.state)
          });
    }
    // search customer
    searchCustomer = (field)=>{
        console.log('search', field.searchCust)
        axios({
            method: 'post',
            url: '../ajax/projectManagement/projDb.aspx/GetCustomer',
            data: {
                SearchKey: field.searchCust
            }
          }).then(res => {
              console.log("GetCustomer", res)
              let data = res.data.d;
              const arrayOfObj = data.map(({ id: key, ...rest }) => ({ key, ...rest }));
              console.log('arrayOfObj', arrayOfObj)
             this.setState({
                 ...this.state,
                 customerData: arrayOfObj
             })
          });
    }

    // open customer popup
    getCustomer = () => {
        console.log('tr', this.state)
        this.setState({
            visible: true,
          });
        console.log('tr', this.state)
    }

    handleCancel = () =>{
        this.setState({
            ...this.state,
            visible: false,
        });
    }
    handleOk = () => {
        console.log(this.state)
        this.setState({
            visible: false,
          });
    }
    
    showModal = () => {
        this.setState({
          visible: true,
        });
    };
    
    deSelectAllProject = () =>{
        this.setState({
            ...this.state,
            selectedRowKeys: [],
            projectDisable: false,
            projectSlected: []
        })
    }
    getProject = () => {
        this.setState({
            ...this.state,
            projectCtl: true
        })
    }
    closeProjectSelection = () => {
        this.setState({
            ...this.state,
            projectCtl: false
        })
    }
    // load Tab component
    loadComponent = (value) =>{
        this.setState({
            ...this.state,
            activeCompo: value
        })
        let obj = this.state.obj;
        if(value == "ProjectDetails"){
            this.getProjectDetails(obj)
        }else if(value == "Timeline"){
            this.getTimeline(obj)
        }else if(value == "CostBreakup"){
            this.getCostBreakup(obj);
        }
    }
    clearCustomer = (e)=> {
        console.log(e);
        this.setState({
            ...this.state,
            customerSelected:{}
        })
    }

/// For table related
getColumnSearchProps = dataIndex => ({
    filterDropdown: ({ setSelectedKeys, selectedKeys, confirm, clearFilters }) => (
      <div style={{ padding: 8 }}>
        <Input
          ref={node => {
            this.searchInput = node;
          }}
          placeholder={`Search ${dataIndex}`}
          value={selectedKeys[0]}
          onChange={e => setSelectedKeys(e.target.value ? [e.target.value] : [])}
          onKeyPress={event => {
                if (event.key === 'Enter') {
                    this.handleSearch(selectedKeys, confirm, dataIndex)
                }
            }
          }
          style={{ width: 188, marginBottom: 8, display: 'block' }}
        />
        <Button
          type="primary"
          onClick={() => this.handleSearch(selectedKeys, confirm, dataIndex)}
          icon="search"
          size="small"
          style={{ width: 90, marginRight: 8 }}
        >
          <i className="fa fa-search"></i> Search
        </Button>
        <Button onClick={() => this.handleReset(clearFilters)} size="small" style={{ width: 90 }}>
          Reset
        </Button>
      </div>
    ),
    filterIcon: filtered => (
      <Icon type="search" style={{ color: filtered ? '#333' : undefined }} />
    ),
    onFilter: (value, record) =>
      record[dataIndex]
        .toString()
        .toLowerCase()
        .includes(value.toLowerCase()),
    onFilterDropdownVisibleChange: visible => {
      if (visible) {
        setTimeout(() => this.searchInput.select());
      }
    },
    render: text =>
      this.state.searchedColumn === dataIndex ? (
        <Highlighter
          highlightStyle={{ backgroundColor: '#ffc069', padding: 0 }}
          searchWords={[this.state.searchText]}
          autoEscape
          textToHighlight={text.toString()}
        />
      ) : (
        text
      ),
  });

  handleSearch = (selectedKeys, confirm, dataIndex) => {
      console.log("enter")
    confirm();
    this.setState({
      searchText: selectedKeys[0],
      searchedColumn: dataIndex,
    });
  };

  handleReset = clearFilters => {
    clearFilters();
    this.setState({ searchText: '' });
  };
// 

    render() { 
        const columns = [
            {
              title: 'Customer Name',
              dataIndex: 'Na',
              key: 'Na',
              width: 180
            },
            {
              title: 'Unique Id',
              dataIndex: 'key',
              key: 'key',
              width: 120
            },
            {
              title: 'Address',
              dataIndex: 'add',
              key: 'add',
            },
          ];
        const projectColumns = [
            {
                title: 'Project Code',
                dataIndex: 'Proj_Code',
                key: 'Proj_Code',
            },
            {
                title: 'Project Name',
                dataIndex: 'Proj_Name',
                key: 'Proj_Name',
                ...this.getColumnSearchProps('Proj_Name')
            },
            {
                title: 'Customer',
                dataIndex: 'Customer',
                key: 'Customer',
                ...this.getColumnSearchProps('Customer')
            }
        ]
        //const { selectedRowKeys } = this.state;
        const dateFormat = 'DD-MM-YYYY';
        //console.log('isupdating', this.state)

        // customer selection
        const rowSelection = {
            onChange: (selectedRowKeys, selectedRows) => {
              //console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows);
              this.setState({
                  ...this.state,
                  customerKeys: selectedRowKeys,
                  customerSelected: selectedRows[0],
              })
              console.log('to state', this.state)
            }
        }
        // Project Selection
        const rowSelectionPrj = {
            selectedRowKeys: this.state.selectedRowKeys,
            hideSelectAll :true,
            onChange: (selectedRowKeys, selectedRows) => {
                console.log('selectedRows', selectedRows)
              this.setState({
                selectedRowKeys: selectedRowKeys,
                projectSlected: selectedRows,
              });
              
              if(selectedRowKeys.length > 2){
                    this.setState({
                        ...this.state,
                        projectSlected: selectedRows,
                        projectDisable:true,
                        selectedRowKeys:selectedRowKeys,
                    })
                } 
            },
            getCheckboxProps: record => ({
                disabled: this.state.projectDisable
            }), 
        }
        // pushing values in input project
        const projectValue = this.state.projectSlected.map(item =>{
            let tArr = []
            tArr.push(item.Proj_Code)
            return tArr
        })
        const customerSelected = this.state.customerSelected.Na
        // Project Selection End

        // show sub component
        let subComponent = null;
        if(this.state.activeCompo == "ProjectSummary"){
            subComponent = <ProjectSummary data={this.state.projectSummaryData} />
        }else if(this.state.activeCompo == "ProjectDetails"){
            subComponent = <ProjectDetails 
            pieData={this.state.PieData} barData={this.state.BarData} formData={this.state.obj} />
        }else if(this.state.activeCompo == "Timeline"){
            subComponent = <Timeline data={this.state.timelineData} />
        }else if(this.state.activeCompo == "CostBreakup"){
            subComponent = <CostBreakup data1={this.state.costData1}  data2={this.state.costData2} />
        }else{
            subComponent = null
        }
        return ( 
            <React.Fragment> 
                
                    <div className="headerArea">
                        <h1>
                            <span className="pull-left backButton">
                                <a href="javascript:void(0);" onClick={()=> {this.props.changComponent('home')}}>
                                <i className="fa fa-arrow-left"></i></a>
                            </span>Project Management DB
                        </h1>
                    </div>
                    <div className="row">
                        <div className="col-md-12">
                            <div className="cardContainer mt-3">
                                <div className="cardContent">
                                    <div className="clearfix borderdCont">
                                        <Form name="mform"
                                            onFinish={this.onHandleSearch} 
                                            //initialValues={intialValue}
                                           >
                                            
                                            <div className="col-md-2 customerInput">
                                                <label>Customer</label>
                                                    <Input 
                                                    placeholder="Select Customer"
                                                    name="cust"
                                                    type="text" 
                                                    id="customerInput"
                                                    onChange={(e) => {this.clearCustomer(e)}}                                          
                                                    value={customerSelected}
                                                    onClick={this.getCustomer} 
                                                    allowClear

                                                     />      
                                            </div>
                                            <div className="col-md-2">
                                                <label>Project Select</label>
                                                <div className="relative">
                                                    
                                                    <Input name="prj" 
                                                        placeholder="Select Project" 
                                                        id="projectInput"
                                                        value={projectValue} 
                                                        onClick={this.getProject} 
                                                         allowClear
                                                         readOnly
                                                         />
                                                         <p style={{color:"red"}}>{this.state.messageProject}</p>
                                                        <div className={
                                                            this.state.projectCtl ? 'projectCtl active' : 'projectCtl'
                                                        }>
                                                            <div className="colorTable">
                                                            <Table
                                                                rowSelection={{
                                                                type: 'Checkbox',
                                                                    ...rowSelectionPrj,
                                                                }}
                                                                scroll={{ y: 240 }}
                                                                hideSelectAll={true}
                                                                pagination={true}
                                                                columns={projectColumns}
                                                                dataSource={this.state.projectdata}
                                                                size="small"
                                                            />
                                                            </div>
                                                            <div style={{
                                                                textAlign:"right",
                                                                padding:"10px"
                                                            }}>
                                                                <Button style={{marginRight:"10px"}} onClick={this.deSelectAllProject}>Deselect All</Button>
                                                                <Button type="primary" onClick={this.closeProjectSelection}>OK</Button>
                                                            </div>
                                                        </div>
                                                </div> 
                                            </div>
                                            <div className="col-md-2">
                                                <label>Date</label>
                                                <Form.Item name="toDate"  initialValue={moment(new Date(), dateFormat)}>
                                                    <DatePicker name="" 
                                                    format={dateFormat} 
                                                    id="datePick"
                                                    />
                                                </Form.Item>
                                                
                                            </div>  
                                            <div className="col-md-6">
                                                <Button type="primary" 
                                                    style={{marginTop: "26px"}} 
                                                    icon={<SearchOutlined />} 
                                                    htmlType="submit">Generate</Button>  
                                            </div> 
                                        </Form>
                                    </div>
                                    {/* start tabs */}
                                    <Tabs 
                                    activeModule={this.state.activeCompo} 
                                    loadComponent={this.loadComponent}
                                    settings={this.state.settings}
                                     />
                                    {/* End tabs */}
                                    { this.state.loading ? <Skeleton active avatar paragraph={{ rows: 4 }} />  : subComponent}
                                                        
                                </div>
                            </div>
                        </div>
                    </div>
            
                    {/* customer modal  */}
                    <ListSelectModal 
                    title="Customer search"
                    visible={this.state.visible}
                    onOkClick={this.handleOk}
                    classes='searchModal'
                    widthSize={1000}
                    onCancelProp={this.handleCancel}
                    onformFinish={this.searchCustomer}
                    colData={columns}
                    sourcedata={this.state.customerData}
                    selectType='radio'
                    rowSelection={rowSelection}
                     />
                    {/* <Modal
                        title="Customer search"
                        visible={this.state.visible}
                        onOk={this.handleOk}
                        className={'searchModal'}
                        width={1000}
                        onCancel={this.handleCancel}
                        >
                        <div className="searchCustForm">
                            <Form name="projectMSearch" 
                                onFinish={this.searchCustomer} 
                            
                                >
                                <Form.Item name="searchCust">
                                    <Input placeholder="Search Customer" id="data1" className="searchCust" />
                                </Form.Item>
                            </Form>
                        </div>
                        <div className="colorTable">
                            <Table
                                rowSelection={{
                                type: 'radio',
                                    ...rowSelection,
                                }}
                                scroll={{ y: 300 }}
                                pagination={false}
                                columns={columns}
                                dataSource={this.state.customerData}
                                size="small"
                            />
                        </div>
                    </Modal> */}

                    
            </React.Fragment>
         );
         
    }
    

}
 
export default ProjectManagement;