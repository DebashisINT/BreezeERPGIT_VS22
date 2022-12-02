/// <reference path="../../../../ajax/Service/General.asmx" />
import React from 'react';
import axios from 'axios';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import TaskItems from './taskItems';
import('./task.css');
import {Modal} from 'antd';
import { ExclamationCircleOutlined } from '@ant-design/icons';
const { confirm } = Modal;
class Task extends React.Component {
    constructor() {
        super();
        this.GetAllTodo = this.GetAllTodo.bind(this)
        this.GetOverDue = this.GetOverDue.bind(this)
        this.GetCompleted = this.GetCompleted.bind(this)
        this.GetPending = this.GetPending.bind(this);
        this.handleComplete = this.handleComplete.bind(this)
    }
    state = {
        taskList: [],
        totalTodo: 0,
        overDueTodo: 0,
        completedTodo: 0,
        pendingTodo:0,
        activeLink: 'all',
        events: []
    }
    componentDidMount() {
        this.GetAllTodo ()
    }
     
    render() {
        return (
            <div>
                <div className="headerArea">
                    <h1>
                        <span className="pull-left backButton">
                            <a href="javascript:void(0);" onClick={()=> {this.props.changComponent('home')}}>
                                <i className="fa fa-arrow-left"></i>
                            </a>
                        </span>Task Lists
                        <span className="pull-right optionHolder">
                            
                        </span>
                    </h1>
                </div>
                <div style={{
                        padding:"20px"
                    }}>
                    <div className="row">
                        <div className="col-md-5">
                            <div className="cardContainer">
                                <div className="cardHeader">Task by Categories</div>
                                <div>
                                    <ul className="tabbedLikeView">
                                        <li className={this.state.activeLink == 'all'? 'active' : null} onClick={this.GetAllTodo}>All <span className="taskCounter">{this.state.totalTodo}</span></li>
                                        <li className={this.state.activeLink == 'overdue'? 'active' : null} onClick={this.GetOverDue}>
                                            <img src="/assests/images/emoD.png" style={{width:"21px"}} /> 
                                            <span className="taskCounter danger">{this.state.overDueTodo}</span></li>
                                        <li className={this.state.activeLink == 'pending'? 'active' : null} onClick={this.GetPending}>
                                            <img src="/assests/images/emoW.png" style={{width:"21px"}} /> <span className="taskCounter warning">{this.state.pendingTodo}</span></li>
                                        <li className={this.state.activeLink == 'completed'? 'active' : null} onClick={this.GetCompleted}>
                                            <img src="/assests/images/emoS.png" style={{width:"21px"}} /> <span className="taskCounter success">{this.state.completedTodo}</span></li>
                                    </ul>
                                </div>
                                <div className="scrollbar">
                                    <TaskItems listofItems={this.state.taskList} toggelSwitch={this.handleComplete} />
                                </div> 
                            </div>
                        </div>
                        <div className="col-md-7">
                            <div className="cardContainer">
                                <div className="cardHeader">Calender View</div>
                                <div className="">
                                <FullCalendar
                                    plugins={[ dayGridPlugin ]}
                                    initialView="dayGridMonth"
                                    //weekends={false}
                                    events={this.state.events}
                                    />
                                </div> 
                            </div>
                        </div>
                    </div>         
                </div>
            </div>
        );
    }
    GetCalender = (data) => {
        var evt = [];       
        for (var i = 0; i < data.length; i++) {
            var obj = {};
            obj.title = data[i].TASK_SUBJECT;
            obj.start = data[i].TASK_DUEDATE;
            obj.description = data[i].TASK_DESCRIPTION;
            evt.push(obj);   
        }
        this.setState({
            ...this.state,
            events:evt
        });
        console.log('fromCal')
        console.log(this.state)
    }
    GetAllTodo () {
        
        axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/TodoData',
            data: {userid:'378'}
          })
          .then(res => {
               let array = res.data.d
                let overdue = array.reduce(function (n, todo) {
                    return n + (todo.WARNING == true);
                }, 0);
                let pending = array.reduce(function (n, todo) {
                    return n + (todo.WARNING == false && todo.ISCOMPLETED == false);
                }, 0);
                let completed = array.reduce(function (n, todo) {
                    return n + (todo.ISCOMPLETED == true);
                }, 0);
              this.setState({
                  ...this.state,
                  taskList: array,
                  totalTodo:array.length,
                  overDueTodo:overdue,
                  pendingTodo: pending,
                  completedTodo: completed,
                  activeLink:'all'
              })
              this.GetCalender(res.data.d);
          });
      }
      // get overdue items
      GetOverDue() {
        //console.log(this.state)
        axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/TodoData',
            data: {userid:'378'}
          })
          .then(res => {
            let updatedArr = []
            res.data.d.map(item => {
                if(item.WARNING == true){
                   updatedArr.push(item)
                }
             })
              this.setState({
                  ...this.state,
                  taskList: updatedArr,
                  activeLink:'overdue'
              })
          });
      }
      // get completed items
      GetCompleted() {
        axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/TodoData',
            data: {userid:'378'}
          })
          .then(res => {
            let updatedArr = []
            res.data.d.map(item => {
                if(item.ISCOMPLETED == true){
                   updatedArr.push(item)
                }
             })
              this.setState({
                  ...this.state,
                  taskList: updatedArr,
                  activeLink:'completed'
              })
          });
      }
      // get pending items
      GetPending() {
        axios({
            method: 'post',
            url: '../ajax/Service/General.asmx/TodoData',
            data: {userid:'378'}
          })
          .then(res => {
            let updatedArr = []
            res.data.d.map(item => {
                if(item.ISCOMPLETED == false && item.WARNING == false){
                   updatedArr.push(item)
                }
             })
              this.setState({
                  ...this.state,
                  taskList: updatedArr,
                  activeLink:'pending'
              })
          });
      }

      handleComplete = (switchId) => {
        let self = this;
        confirm({
            title: 'Do you Want to Proceed?',
            icon: <ExclamationCircleOutlined />,
            content: '',
            onOk() {
                axios({
                    method: 'post',
                    url: '../ajax/Service/General.asmx/UpdateTask',
                    data: {taskId:switchId, Status: '0'}
                  })
                  .then(res => {
                    self.GetAllTodo()
                  });
            },
            onCancel() {
              console.log('Canceled');
            },
          });
        
      } 
}

export default Task;