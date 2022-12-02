import React from 'react';
import TopBox from './topBox';
import('./approvalWaiting.css');
import { Table } from 'antd';
import axios from 'axios';

class ApprovalWaiting extends React.Component {
    constructor(props) {
      super(props);
    }
    state = {
        loading: false,
        detailTitle: "",
        details: [],
        activeComp: "",
        showDetails: true,
        boxData: {
            BranchRequisitonCount: "0",
            ProjectIndentCount: "0",
            ProjectPurchaseOrderCount: "0",
            ProjectSalesOrderCount: "0",
            PurchaseIndentCount: "0",
            PurchaseOrderCount: "0",
            SalesOrderCount: "0"
        },
        permissions: {
            divBranchRequisiton: false,
            divProjectIndent: false,
            divProjectPurchaseOrder: false,
            divProjectSalesOrder: false,
            divPurchaseIndent: false,
            divPurchaseOrder: false,
            divSalesOrder: false,
        }
    }  
    componentDidMount() {
        axios({
            method: 'post',
            url: '../ajax/approvalWaiting/approvalWaiting.aspx/getPermissions',
            data: {}
          }).then(res => {
              console.log('getPermissions', res)
              let data = res.data.d
              this.setState({
                ...this.state,
                permissions: data
            })
          });
        axios({
            method: 'post',
            url: '../ajax/approvalWaiting/approvalWaiting.aspx/GetAllCountData',
            data: {
                action: "ALLCount"
            }
          }).then(res => {
              console.log(res)
              let data = res.data.d[0];
              this.setState({
                  ...this.state,
                  boxData: data
              })
          });

    }  
    getDetailsData = (titleString) =>{
        console.log(titleString)
        if(titleString == "Branch Requisition") {
            this.setState({
                ...this.state,
                loading:true,
                detailTitle: titleString
            })
            axios({
                method: 'post',
                url: '../ajax/approvalWaiting/approvalWaiting.aspx/GetBranchRequisiton',
                data: {
                    action: "PopulateERPDocApprovalPendingListByUserLevel"
                }
              }).then(res => {
                  console.log(res)
                  let data = res.data.d;
                  this.setState({
                    ...this.state,
                    details: data,
                    loading:false,
                })
              });
        }else if(titleString == "Purchase Indent") {
            this.setState({
                ...this.state,
                loading:true,
                detailTitle: titleString
            })
            axios({
                method: 'post',
                url: '../ajax/approvalWaiting/approvalWaiting.aspx/GetPurchaseIndent',
                data: {
                    action: "PopulateERPDocApprovalPendingListByUserLevel"
                }
              }).then(res => {
                  console.log(res)
                  let data = res.data.d;
                  this.setState({
                      ...this.state,
                      details: data,
                      loading:false,
                  })
              });  
        }else if(titleString == "Project Indent") {
            this.setState({
                ...this.state,
                loading:true,
                detailTitle: titleString
            })
            axios({
                method: 'post',
                url: '../ajax/approvalWaiting/approvalWaiting.aspx/GetProjectIndent',
                data: {
                    action: "ProjectPopulateERPDocApprovalPendingListByUserLevel"
                }
              }).then(res => {
                  console.log(res)
                  let data = res.data.d;
                  this.setState({
                      ...this.state,
                      details: data,
                      loading:false,
                  })
              });
        }else if(titleString == "Purchase Order") {
            this.setState({
                ...this.state,
                loading:true,
                detailTitle: titleString
            })
            axios({
                method: 'post',
                url: '../ajax/approvalWaiting/approvalWaiting.aspx/GetPurchaseOrder',
                data: {
                    action: "PopulateERPDocApprovalPendingListByUserLevel"
                }
              }).then(res => {
                  console.log(res)
                  let data = res.data.d;
                  this.setState({
                      ...this.state,
                      details: data,
                      loading:false,
                  })
              });
        }else if(titleString == "Project Purchase Order") {
            this.setState({
                ...this.state,
                loading:true,
                detailTitle: titleString
            })
            axios({
                method: 'post',
                url: '../ajax/approvalWaiting/approvalWaiting.aspx/GetProjectPurchaseOrder',
                data: {
                    action: "ProjectPopulateERPDocApprovalPendingListByUserLevel"
                }
              }).then(res => {
                  console.log(res)
                  let data = res.data.d;
                  this.setState({
                      ...this.state,
                      details: data,
                      loading:false,
                  })
              });
        }else if(titleString == "Sales Order") {
            this.setState({
                ...this.state,
                loading:true,
                detailTitle: titleString
            });
            axios({
                method: 'post',
                url: '../ajax/approvalWaiting/approvalWaiting.aspx/GetSalesOrder',
                data: {
                    action: "PopulateERPDocApprovalPendingListByUserLevel"
                }
              }).then(res => {
                  console.log(res)
                  let data = res.data.d;
                  this.setState({
                      ...this.state,
                      details: data,
                      loading:false,
                  })
              });
        }else if(titleString == "Project Sales Order") {
            this.setState({
                ...this.state,
                loading:true,
                detailTitle: titleString
            })
            axios({
                method: 'post',
                url: '../ajax/approvalWaiting/approvalWaiting.aspx/GetProjectSalesOrder',
                data: {
                    action: "ProjectPopulateERPDocApprovalPendingListByUserLevel"
                }
              }).then(res => {
                  console.log(res)
                  let data = res.data.d;
                  this.setState({
                      ...this.state,
                      details: data,
                      loading:false,
                  })
              });
        }
    }    
    componentWillUnmount() {   
    }
      render(){
          return(
              <div>
                    <div className="headerArea">
                        <h1>
                            <span className="pull-left backButton">
                                <a href="javascript:void(0);" onClick={()=> {this.props.changComponent('home')}}>
                                <i className="fa fa-arrow-left"></i></a>
                            </span>Approval Waiting 
                        </h1>
                    </div>
                    <div className="row">
                        <div className="col-md-12">
                            <div className="cardContainer mt-3">
                                <div className="cardContent">
                                    <div className="row">
                                        <div className="col-md-12">
                                            <div class="BoxTypeGrey">
                                                <div class="clearfix">
                                                    <div class="flex-row align-items-center wraped">
                                                        {
                                                            this.state.permissions.divBranchRequisiton ? (
                                                                <TopBox 
                                                                clicked={this.getDetailsData}
                                                                Title="Branch Requisition" 
                                                                smtl="Today"
                                                                value={this.state.boxData.BranchRequisitonCount}
                                                                bgColor="#32ad6f"
                                                                action=""
                                                                addClass={this.state.activeComp =="Cust" ? "active" : ""}
                                                            ></TopBox>
                                                            ) : null
                                                        }
                                                        {
                                                            this.state.permissions.divPurchaseIndent ? (
                                                                <TopBox 
                                                                    clicked={this.getDetailsData}
                                                                    Title="Purchase Indent" 
                                                                    smtl="Today"
                                                                    value={this.state.boxData.PurchaseIndentCount}
                                                                    bgColor="#9db53c"
                                                                    action=""
                                                                    addClass={this.state.activeComp =="Cust" ? "active" : ""}
                                                                ></TopBox>
                                                            ) : null
                                                        }
                                                        {
                                                            this.state.permissions.divProjectIndent ? (
                                                                <TopBox 
                                                                    clicked={this.getDetailsData}
                                                                    Title="Project Indent" 
                                                                    smtl="Today"
                                                                    value={this.state.boxData.ProjectIndentCount}
                                                                    bgColor="#8846c5"
                                                                    action=""
                                                                    addClass={this.state.activeComp =="Cust" ? "active" : ""}
                                                                ></TopBox>
                                                            ) : null
                                                        } 
                                                        {
                                                            this.state.permissions.divPurchaseOrder ? (
                                                                <TopBox 
                                                                    clicked={this.getDetailsData}
                                                                    Title="Purchase Order" 
                                                                    smtl="Today"
                                                                    value={this.state.boxData.PurchaseOrderCount}
                                                                    bgColor="#383d82"
                                                                    action=""
                                                                    addClass={this.state.activeComp =="Cust" ? "active" : ""}
                                                                ></TopBox>
                                                            ) : null
                                                        }
                                                        {
                                                            this.state.permissions.divProjectPurchaseOrder ? (
                                                                <TopBox 
                                                                    clicked={this.getDetailsData}
                                                                    Title="Project Purchase Order" 
                                                                    smtl="Today"
                                                                    value={this.state.boxData.ProjectPurchaseOrderCount}
                                                                    bgColor="#ee0067"
                                                                    action=""
                                                                    addClass={this.state.activeComp =="Cust" ? "active" : ""}
                                                                ></TopBox>
                                                            ) : null
                                                        } 
                                                        {
                                                            this.state.permissions.divSalesOrder ? (
                                                                <TopBox 
                                                                    clicked={this.getDetailsData}
                                                                    Title="Sales Order" 
                                                                    smtl="Today"
                                                                    value={this.state.boxData.SalesOrderCount}
                                                                    bgColor="#c4c703"
                                                                    action=""
                                                                    addClass={this.state.activeComp =="Cust" ? "active" : ""}
                                                                ></TopBox>
                                                            ) : null
                                                        }
                                                        {
                                                            this.state.permissions.divProjectSalesOrder ? (
                                                                <TopBox
                                                                    clicked={this.getDetailsData} 
                                                                    Title="Project Sales Order" 
                                                                    smtl="Today"
                                                                    value={this.state.boxData.ProjectSalesOrderCount}
                                                                    bgColor="#565454"
                                                                    action=""
                                                                    addClass={this.state.activeComp =="" ? "" : ""}
                                                                ></TopBox>
                                                            ) : null
                                                        }    
                                                    </div>
                                                </div>
                                                
                                                <div className="clearfix" style={{marginTop:"10px"}}>
                                                    <div className="shadowBox">
                                                        <div className="bigHeading cap">{this.state.detailTitle} Details</div>
                                                        {
                                                            this.state.showDetails ? (
                                                                <div className="colorTable">
                                                                    <Table dataSource={this.state.details} 
                                                                        columns={columns}
                                                                        loading={this.state.loading}
                                                                        size="small"
                                                                    />
                                                                </div>
                                                            ) : null
                                                        }
                                                        
                                                    </div>
                                                </div>
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
}
export default  ApprovalWaiting;

  const columns = [
    {
      title: 'Branch',
      dataIndex: 'Branch',
      key: 'Branch',
    },
    {
      title: 'Document No.',
      dataIndex: 'DocumentNo',
      key: 'DocumentNo',
    },
    {
      title: 'Date',
      dataIndex: 'Date',
      key: 'Date',
    },
    {
      title: 'Requested By',
      dataIndex: 'RequestedBy',
      key: 'RequestedBy',
    },
  ];


{/* <table className="table styledTble">
    <thead>
    <tr>
        <th>Branch </th>
        <th>Document No </th>
        <th>Date</th>
        <th>Requested By </th>
        <th>Approve</th>
        <th>Reject</th>
    </tr>
    </thead>
    <tbody>
        {
                this.state.details.length ? (                                                                              
                    this.state.details.map((item, index) => {
                        return ( 
                            <tr key={index}>
                                <td>{item.CONTACTPERSON}</td>
                                <td>{item.COMPANY}</td>
                                <td>{item.EVENT_TYPE}</td>
                                <td>
                                    <img src="../assests/images/DashboardIcons/smsPh.png"
                                        style={{width:"18px", cursor:"pointer"}} 
                                    />
                                </td>
                                <td>
                                    <img src="../assests/images/DashboardIcons/email.png"
                                        style={{width:"18px", cursor:"pointer"}} 
                                        />
                                </td>
                                
                            </tr>
                        )
                    })
                ) : <tr><td>No Data Found</td></tr>
            
        }
    </tbody>
</table> */}