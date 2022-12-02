
import React from 'react';
import('../ServiceManagement/ServiceManagement.css');
import axios from 'axios';
import { Table, Input, Button, Icon, Modal } from 'antd';
import Highlighter from 'react-highlight-words';
import ('./STB.css')

const dataTabl = [
    {
      key: '1',
      name: 'John Brown',
      age: 32,
      address: 'New York No. 1 Lake Park',
    },
    {
      key: '2',
      name: 'Joe Black',
      age: 42,
      address: 'London No. 1 Lake Park',
    },
    {
      key: '3',
      name: 'Jim Green',
      age: 32,
      address: 'Sidney No. 1 Lake Park',
    },
    {
      key: '4',
      name: 'Jim Red',
      age: 32,
      address: 'London No. 2 Lake Park',
    },
  ];

class STBMNG extends React.Component {
    constructor(props) {
        super(props);
      }
    state = {
        DivApproval: true,
        DivSchemeDirApproval: true,
        DivSchemeReceived: true,
        DivSchemeRegister: true,
        DivSchemeReqClose: true,
        DivSchemeSearch: false,
        DivSearch: true,
        DivWalletRecharge: true,
        DivInventory: true,
        DivReceipt: true,
        DivReports: true,
        DivReturnDispatch: true,
        DivSTBReqReturn: true,
        DivSTBRequisition: true,
        DivInventoryPending: "0",
        DivReceiptCancelReq: "0",
        DivReturnReqPendingBranch: "0",
        DivReturnReqPendingHO: "0",
        DivSTBRequisitionDirPending: "0",
        DivSTBRequisitionFinPending: "0",
        DivSTBRequisitionOnHold: "0",
        DivWalletRechargeCancelReq: "0",
        DivWalletRechargeOpenCash: "0",
        DivWalletRechargeOpenCheque: "0",
        searchText: '',
        searchedColumn: '',
        DetailsModal: false,
        detailsData :[]
    }
    componentDidMount() {  
        axios({
            method: 'post',
            url: '../ajax/STBMNG/STB.aspx/GetBoxsData',
            data: {}
          }).then(res => {
              console.log('GetBoxsData', res)
              let data = res.data.d;
              console.log('data', data)
              this.setState({
                ...this.state,
                    DivApproval: res.data.d.DivApproval,
                    DivSchemeDirApproval: res.data.d.DivSchemeDirApproval,
                    DivSchemeReceived: res.data.d.DivSchemeReceived,
                    DivSchemeRegister: res.data.d.DivSchemeRegister,
                    DivSchemeReqClose: res.data.d.DivSchemeReqClose,
                    DivSchemeSearch: res.data.d.DivSchemeSearch,
                    DivSearch: res.data.d.DivSearch,
                    DivWalletRecharge: res.data.d.DivWalletRecharge,
                    DivInventory: res.data.d.DivInventory,
                    DivReceipt: res.data.d.DivReceipt,
                    DivReports: res.data.d.DivReports,
                    DivReturnDispatch: res.data.d.DivReturnDispatch,
                    DivSTBReqReturn: res.data.d.DivSTBReqReturn,
                    DivSTBRequisition: res.data.d.DivSTBRequisition,
                    DivInventoryPending: res.data.d.DivInventoryPending,
                    DivReceiptCancelReq: res.data.d.DivReceiptCancelReq,
                    DivReturnReqPendingBranch: res.data.d.DivReturnReqPendingBranch,
                    DivReturnReqPendingHO: res.data.d.DivReturnReqPendingHO,
                    DivSTBRequisitionDirPending: res.data.d.DivSTBRequisitionDirPending,
                    DivSTBRequisitionFinPending: res.data.d.DivSTBRequisitionFinPending,
                    DivSTBRequisitionOnHold: res.data.d.DivSTBRequisitionOnHold,
                    DivWalletRechargeCancelReq: res.data.d.DivWalletRechargeCancelReq,
                    DivWalletRechargeOpenCash: res.data.d.DivWalletRechargeOpenCash,
                    DivWalletRechargeOpenCheque: res.data.d.DivWalletRechargeOpenCheque
              })
              console.log('state' )
          });
          //this.detailsDataHandler("STBRequisitionFinPending")
    }
    componentWillUnmount() {   
    }
    detailsDataHandler = (report) => {
        console.log(report);
        this.setState({
            DetailsModal: true,
        });
        axios({
            method: 'post',
            url: '../ajax/STBMNG/STB.aspx/ShowDashboardDetails',
            data: { report: report }
          }).then(res => {
              console.log('ShowDashboardDetails', res)
              this.setState({
                detailsData: res.data.d,
                });
          })
    }
    // Details Modal
      handleOk = e => {
        console.log(e);
        this.setState({
            DetailsModal: false,
        });
      };
    
      handleCancel = e => {
        console.log(e);
        this.setState({
            DetailsModal: false,
        });
      };

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
              //icon="search"
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
      // End of table

    render() {
        // For table columns
        const columns = [
            {
              title: 'Req No',
              dataIndex: 'ReqNo',
              key: 'ReqNo',
              width: '10%',
              ...this.getColumnSearchProps('ReqNo'),
              
            },
            {
              title: 'Req Date',
              dataIndex: 'ReqDate',
              key: 'ReqDate',
              width: '20%',
              ...this.getColumnSearchProps('ReqDate'),
            },
            {
              title: 'Location',
              dataIndex: 'Location',
              key: 'Location',
              ...this.getColumnSearchProps('Location'),
            },
            {
                title: 'Entity Code',
                dataIndex: 'EntityCode',
                key: 'EntityCode',
                ...this.getColumnSearchProps('EntityCode'),
              },
              {
                title: 'Qty',
                dataIndex: 'Qty',
                key: 'Qty',
                ...this.getColumnSearchProps('Qty'),
              },
              {
                title: 'Hold By',
                dataIndex: 'HoldBy',
                key: 'HoldBy',
                ...this.getColumnSearchProps('HoldBy'),
              },
              {
                title: 'Director',
                dataIndex: 'Director',
                key: 'Director',
                ...this.getColumnSearchProps('Director'),
              },
              {
                title: 'Model',
                dataIndex: 'Model',
                key: 'Model',
                width: '180px',
                ...this.getColumnSearchProps('Model'),
              },
        ];
        return (
            <div>
              <div className="headerArea relative ">
                  <h1 className="clearfix">
                      <span className="pull-left backtoERP">
                          <a href="javascript:void(0);" onClick={()=> {this.props.changComponent('home')}}>
                          <i className="fa fa-arrow-left"></i> <span>Switch to ERP </span></a>
                      </span> <span className="pageTitle">STB Management </span>
                      <span className="notifyService"><img src="../assests/images/DashboardIcons/notification.png" /></span>
                  </h1>
              </div>
              <div className="row">
                  <div className="col-md-12">
                      <div className="cardContainer mt-3" style={{background:"#5a84fb"}}>
                          <div className="cardContent">
                                <div className="clearfix">
                                    <div className="d-flex justify-content-center stbBoxes">
                                        <div className="flex-itm stbDbBox mr-5" 
                                            onClick={()=> {this.detailsDataHandler('STBRequisitionFinPending')}}>
                                            <div className="">
                                                <div className="bx-cont">
                                                    <div className="media d-flex">
                                                    <div className="media-left " style={{
                                                        display:"flex",
                                                        justifyContent:"center",
                                                        alignItems:"center"
                                                    }}>
                                                    <div className="bqBox c5"><img src="../assests/images/DashboardIcons/server.png" 
                                                    className="media-object" style={{maxWidth:"25px"}} /></div>
                                                    </div>
                                                    <div className="media-body">
                                                        <div className="bx-muted">Total </div>
                                                        <div className="bx-amt">{this.state.DivSTBRequisitionFinPending}</div>
                                                    </div>
                                                    </div>
                                                </div>
                                                <div className="bx-footer">STB Requisition(Fin Pending)</div>
                                            </div>
                                        </div>
                                        <div className="flex-itm stbDbBox mr-5" 
                                        onClick={()=> {this.detailsDataHandler('STBRequisitionDirPending')}}>
                                            <div className="">
                                                <div className="bx-cont">
                                                    <div className="media d-flex">
                                                    <div className="media-left" style={{
                                                        display:"flex",
                                                        justifyContent:"center",
                                                        alignItems:"center"
                                                    }}>
                                                        <div className="bqBox c1"><img src="../assests/images/DashboardIcons/server.png" className="media-object" style={{maxWidth:"25px"}} /></div>
                                                    </div>
                                                    <div className="media-body">
                                                        <div className="bx-muted">Total </div>
                                                        <div className="bx-amt">{this.state.DivSTBRequisitionDirPending}</div>
                                                    </div>
                                                    </div>
                                                </div>
                                                <div className="bx-footer">STB Requisition <div>(Dir Pending)</div></div>
                                            </div>
                                        </div>
                                        <div className="flex-itm stbDbBox mr-5">
                                            <div className="">
                                                <div className="bx-cont">
                                                    <div className="media d-flex">
                                                    <div className="media-left" style={{
                                                        display:"flex",
                                                        justifyContent:"center",
                                                        alignItems:"center"
                                                    }}>
                                                        <div className="bqBox c2"><img src="../assests/images/DashboardIcons/server.png" className="media-object" style={{maxWidth:"25px"}} /></div>
                                                    </div>
                                                    <div className="media-body">
                                                        <div className="bx-muted">Total </div>
                                                        <div className="bx-amt">{this.state.DivSTBRequisitionOnHold}</div>
                                                    </div>
                                                    </div>
                                                </div>
                                                <div className="bx-footer">STB Requisition <div>(On Hold)</div></div>
                                            </div>
                                        </div>
                                        <div className="flex-itm stbDbBox mr-5">
                                            <div className="">
                                                <div className="bx-cont">
                                                    <div className="media d-flex">
                                                    <div className="media-left" style={{
                                                        display:"flex",
                                                        justifyContent:"center",
                                                        alignItems:"center"
                                                    }}>
                                                        <div className="bqBox c3"><img src="../assests/images/DashboardIcons/006-storage.png" className="media-object" style={{maxWidth:"25px"}} /></div>
                                                    </div>
                                                    <div className="media-body">
                                                        <div className="bx-muted">Total </div>
                                                        <div className="bx-amt">{this.state.DivInventoryPending}</div>
                                                    </div>
                                                    </div>
                                                </div>
                                                <div className="bx-footer">Inventory Pending</div>
                                            </div>
                                        </div>
                                       
                                    </div>
                                    <div className="d-flex justify-content-center stbBoxes" style={{
                                        marginTop:"15px"
                                    }}>
                                        <div className="flex-itm stbDbBox mr-5">
                                            <div className="">
                                                <div className="bx-cont">
                                                    <div className="media d-flex">
                                                    <div className="media-left " style={{
                                                        display:"flex",
                                                        justifyContent:"center",
                                                        alignItems:"center"
                                                    }}>
                                                        <div className="bqBox c6"><img src="../assests/images/DashboardIcons/server.png" className="media-object" style={{maxWidth:"25px"}} /></div>
                                                    </div>
                                                    <div className="media-body">
                                                        <div className="bx-muted">Total </div>
                                                        <div className="bx-amt">{this.state.DivReturnReqPendingBranch}</div>
                                                    </div>
                                                    </div>
                                                </div>
                                                <div className="bx-footer">Return Req. Pending (Branch)</div>
                                            </div>
                                        </div>
                                        <div className="flex-itm stbDbBox mr-5">
                                            <div className="">
                                                <div className="bx-cont">
                                                    <div className="media d-flex">
                                                    <div className="media-left" style={{
                                                        display:"flex",
                                                        justifyContent:"center",
                                                        alignItems:"center"
                                                    }}>
                                                        <div className="bqBox c7"><img src="../assests/images/DashboardIcons/002-wallet.png" className="media-object" style={{maxWidth:"25px"}} /></div>
                                                    </div>
                                                    <div className="media-body">
                                                        <div className="bx-muted">Total </div>
                                                        <div className="bx-amt">{this.state.DivReturnReqPendingHO}</div>
                                                    </div>
                                                    </div>
                                                </div>
                                                <div className="bx-footer">Return Req. Pending (HO)</div>
                                            </div>
                                        </div>
                                        <div className="flex-itm stbDbBox mr-5">
                                            <div className="">
                                                <div className="bx-cont">
                                                    <div className="media d-flex">
                                                    <div className="media-left" style={{
                                                        display:"flex",
                                                        justifyContent:"center",
                                                        alignItems:"center"
                                                    }}>
                                                        <div className="bqBox c8"><img src="../assests/images/DashboardIcons/002-wallet.png" className="media-object" style={{maxWidth:"25px"}} /></div>
                                                    </div>
                                                    <div className="media-body">
                                                        <div className="bx-muted">Total </div>
                                                        <div className="bx-amt">{this.state.DivWalletRechargeCancelReq}</div>
                                                    </div>
                                                    </div>
                                                </div>
                                                <div className="bx-footer">Wallet Recharge cancel req.</div>
                                            </div>
                                        </div>
                                        <div className="flex-itm stbDbBox mr-5">
                                            <div className="">
                                                <div className="bx-cont">
                                                    <div className="media d-flex">
                                                    <div className="media-left" style={{
                                                        display:"flex",
                                                        justifyContent:"center",
                                                        alignItems:"center"
                                                    }}>
                                                        <div className="bqBox c9"><img src="../assests/images/DashboardIcons/002-wallet.png" className="media-object" style={{maxWidth:"25px"}} /></div>
                                                    </div>
                                                    <div className="media-body">
                                                        <div className="bx-muted">Total </div>
                                                        <div className="bx-amt">{this.state.DivReceiptCancelReq}</div>
                                                    </div>
                                                    </div>
                                                </div>
                                                <div className="bx-footer">Receipt cancel req.</div>
                                            </div>
                                        </div>
                                   </div>
                                </div>
                          </div>
                      </div>
                  </div>
              </div>
            <div className="row">
                  <div className="col-md-12">
                    <div className="text-center mt-5 " 
                        style={{marginTop:"20px"}}>
                        <div className="d-flex wrapFlex justify-content-center linkBoxes">
                            {
                                this.state.DivReceipt ? (
                                    <a href="/STBManagement/MoneyReceipt/index.aspx">
                                        <div className="linkBox c1"><img src="../assests/images/DashboardIcons/paperW.png" style={{width:"26px"}}  /></div>
                                        <div className="linkText">Money Receipt</div>
                                    </a> 
                                ) : null
                            }
                            
                            { this.state.DivWalletRecharge ? ( 
                            <a href="/STBManagement/WalletRecharge/index.aspx"><div className="linkBox c9"><img src="../assests/images/DashboardIcons/002-wallet.png" style={{width:"26px"}} /></div>
                            <div className="linkText">Wallet Recharge</div></a>) : null }
                            { this.state.DivSTBRequisition ? (
                            <a href="/STBManagement/Requisition/STBRequisition.aspx"><div className="linkBox c3"><img src="../assests/images/DashboardIcons/005-smart-tv.png" style={{width:"26px"}}  /></div>
                            <div className="linkText">STB Requisition</div></a>) : null }
                             { this.state.DivSTBReqReturn ? (<a href="/STBManagement/ReturnRequisition/ReturnRequisitionList.aspx"><div className="linkBox c4"><img src="../assests/images/DashboardIcons/004-return.png" style={{width:"26px"}}  /></div><div className="linkText">STB Req. Return</div></a>) : null }
                             { this.state.DivReturnDispatch ? (<a href="/STBManagement/ReturnDispatch/ReturnDispatch.aspx"><div className="linkBox c5"><img src="../assests/images/DashboardIcons/004-return.png" style={{width:"26px"}}  /></div><div className="linkText">Ret. Dispatch</div></a>) : null }
                             { this.state.DivApproval ? (
                             <a href="/STBManagement/Approval/index.aspx"><div className="linkBox c6">
                                 <img src="../assests/images/DashboardIcons/003-approval.png" style={{width:"26px"}}  /></div>
                                 <div className="linkText">Approval</div></a>) : null }
                             { this.state.DivInventory ? (<a href="/STBManagement/STBInventory/STBInventory.aspx"><div className="linkBox c6"><img src="../assests/images/DashboardIcons/006-storage.png" style={{width:"26px"}}  /></div><div className="linkText">Inventory</div></a>) : null }
                             { this.state.DivSearch ? (<a href="/STBManagement/Search/search.aspx"><div className="linkBox c7"><img src="../assests/images/DashboardIcons/searchW.png" style={{width:"26px"}}  /></div><div className="linkText">Search</div></a>) : null }

                             { this.state.DivReports ? (<a href="/Reports/GridReports/stbregister.aspx"><div className="linkBox c8"><img src="../assests/images/DashboardIcons/reportW.png" style={{width:"26px"}}  /></div><div className="linkText">Reports</div></a>) : null }
                             { this.state.DivSchemeReceived ? (<a href="/STBManagement/STBSchemeReceived/STBSchemeReceivedList.aspx"><div className="linkBox c4"><img src="../assests/images/DashboardIcons/paperW.png" style={{width:"26px"}}  /></div><div className="linkText">Scheme - Received</div></a>) : null }
                             { this.state.DivSchemeDirApproval ? (<a href="/STBManagement/STBSchemeDirectorApproval/SchemeDirectorApproval.aspx"><div className="linkBox c5"><img src="../assests/images/DashboardIcons/003-approval.png" style={{width:"26px"}}  /></div><div className="linkText">Scheme - Dir. Approval</div></a>) : null }
                             { this.state.DivSchemeReqClose ? (<a href="/STBManagement/STBSchemeRequisition/STBSchemeRequisitionList.aspx"><div className="linkBox c6"><img src="../assests/images/DashboardIcons/006-storage.png" style={{width:"26px"}}  /></div><div className="linkText">Scheme - Req. Close</div></a>) : null }
                             { this.state.DivSchemeRegister ? (<a href="/Reports/GridReports/STBSchemeRegister.aspx"><div className="linkBox c8"><img src="../assests/images/DashboardIcons/reportW.png" style={{width:"26px"}}  /></div><div className="linkText">STB Scheme - Register</div></a>) : null }
             
                        </div>
                    </div>
                  </div>
              </div>
              

            <Modal
                title="Basic Modal"
                visible={this.state.DetailsModal}
                onOk={this.handleOk}
                onCancel={this.handleCancel}
                width={1000}
                
                >
                <Table className="antTable" columns={columns} dataSource={this.state.detailsData} 
                size="small" bordered  />
            </Modal>               
        </div>
        );
        
    }     
}

export default STBMNG;



