import React from 'react';
import('./AttendanceDB.css');
import axios from 'axios';
import { Drawer, Button, Empty, Table } from 'antd';


class AttendanceDB extends React.Component {
    constructor(props) {
      super(props);
    }
    state = {
        drawer: false,
        TotalEmp: "0",
        Present: "0",
        todaysAbsent: "0",
        LateCount: "0",
        LeaveCount: "0",
        LateData: [],
        // permission
        EmpCount: false,
        PresentCount :false,
        AbsentCount :false,
        LateComersCount :false,
        OnLeaveCount :false,
        InOutCount :false,
        RecentAtt :false,
        AttendanceDet :false,
    }
    showDrawer = () => {
        this.setState({
             ...this.state,
             drawer:true 
        })
        axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/TodaysLate',
            data: {}
            })
            .then(res => {
                console.log(res)
                this.setState({
                    ...this.state,
                    LateData:res.data.d
                })
            })
     };
     onClose = () => {
         this.setState({
              ...this.state,
              drawer:false 
         })
      };
    componentDidMount(){
        
        axios({
            method: 'post',url: '../ajax/attendance/attendance.aspx/getPageloadPerm',data: {}
            })
            .then(res => {
                console.log('attP', res.data)
                this.setState({
                    ...this.state, 
                    EmpCount: res.data.d.EmpCount,
                    PresentCount :res.data.d.PresentCount,
                    AbsentCount :res.data.d.AbsentCount,
                    LateComersCount :res.data.d.LateComersCount,
                    OnLeaveCount :res.data.d.OnLeaveCount,
                    InOutCount :res.data.d.InOutCount,
                    RecentAtt :res.data.d.RecentAtt,
                    AttendanceDet :res.data.d.AttendanceDet,
                });
            })

        axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/GetTodaysAttendance',
            data: {}
            })
            .then(res => {
                console.log('GetTodaysAttendance', res)
                this.setState({
                    ...this.state,
                    TotalEmp: res.data.d.TotalEmp,
                    Present: res.data.d.Present,
                    todaysAbsent: res.data.d.todaysAbsent,
                    LateCount: res.data.d.LateCount,
                    LeaveCount: res.data.d.LeaveCount,
                })
            })
        
    }
    
    render(){
        const columns = [
            {
              title: 'Employee Name',
              dataIndex: 'LeaveName',
              key: 'LeaveName',
            },
            {
              title: 'Late',
              dataIndex: 'unit',
              key: 'unit',
            },
            {
              title: 'Late Hours',
              dataIndex: 'latetime',
              key: 'latetime',
            }
          ];
          
          const data = [
            
          ];
        return (
            <div>
                <Drawer
                    className="noPadding"
                    bodyStyle={{padding: "0"}}
                    width={450}
                    placement="right"
                    closable={false}
                    onClose={this.onClose}
                    visible={this.state.drawer}
                    > 
                        <div>
                            <Table columns={columns} dataSource={this.state.LateData}></Table>
                        </div>
                    </Drawer>
                <div className="headerArea">
                    <h1>
                        <span className="pull-left backButton">
                            <a href="javascript:void(0);" onClick={()=> {this.props.changComponent('home')}}>
                            <i className="fa fa-arrow-left"></i></a>
                        </span>Today's Attendance 
                    </h1>
                </div>
                <div className="row">
                    <div className="col-md-12">
                        <div className="cardContainer mt-3">
                            
                        <div className="cardContent">
                                <div className="flexConatiner">
                                    {this.state.EmpCount ? (
                                        <div className="flexItem">
                                            <div className="top">
                                                <span className="icon c3"><i className="fa fa-users"></i></span>
                                                <div className="">
                                                    <div className="f14">Employees</div>
                                                    <div className="number">{this.state.TotalEmp}</div>
                                                </div>
                                            </div>
                                        </div>
                                    ) : null}
                                    {this.state.PresentCount ? (
                                        <div className="flexItem ">
                                            <div className="top">
                                                <span className="icon c2"><i className="fa fa-user"></i></span>
                                                <div className="">
                                                    <div className="f14">Present</div>
                                                    <div className="number">{this.state.Present}</div>
                                                </div>
                                            </div>
                                        </div>
                                    ) : null }
                                    
                                    {this.state.AbsentCount ? (
                                        <div className="flexItem ">
                                            <div className="top">
                                                <span className="icon "><i className="fa fa-info"></i></span>
                                                <div className="">
                                                    <div className="f14">Absent</div>
                                                    <div className="number">{this.state.todaysAbsent}</div>
                                                </div>
                                            </div>
                                        </div>
                                    ) : null }
                                    {this.state.LateComersCount ? (
                                        <div className="flexItem">
                                            <div className="top" onClick={this.showDrawer} style={{cursor:"pointer"}}>
                                                <span className="icon c4"><i className="fa fa-twitter"></i></span>
                                                <div className="">
                                                    <div className="f14">Late Comers</div>
                                                    <div className="number">{this.state.LateCount}</div>
                                                </div>
                                            </div>
                                        </div>
                                    ) : null }
                                    {this.state.OnLeaveCount ? (
                                        <div className="flexItem">
                                            <div className="top">
                                                <span className="icon c5"><i className="fa fa-plane"></i></span>
                                                <div className="">
                                                    <div className="f14">On Leave</div>
                                                    <div className="number">{this.state.LeaveCount}</div>
                                                </div>
                                            </div>
                                        </div>
                                    ) : null }
                                    
                                </div>
                            </div>
                            {/* top boxes ends */}

                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-4">
                        <div className="cardContainer">
                            <div class="cardHeader">In-Out Count</div>
                            <div className="cardContent">
                                
                            </div>
                        </div>
                    </div>
                    <div className="col-md-4">
                        <div className="cardContainer">
                            <div class="cardHeader">Attendance For Last <span><input type="text" /></span> </div>
                            <div className="cardContent">
                                
                            </div>
                        </div>
                    </div>
                    <div className="col-md-4">
                        <div className="cardContainer">
                            <div class="cardHeader">Recent Attendance</div>
                            <div className="cardContent">
                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
export default AttendanceDB;