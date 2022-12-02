import React from 'react';
import('./task.css');
import Switch from "react-switch";
import { Empty } from 'antd';
 
class TaskItems extends React.Component {
    constructor() {
        super();
    }
    
    componentDidMount(){
        console.log(this.props.listofItems)   
    }
    render() {
        let item;
        item = this.props.listofItems.length ? (
            this.props.listofItems.map(list => {
                let imageUrl ='';
                let checkedStatus;
                if(list.ISCOMPLETED == true){
                    checkedStatus = true;
                }else{
                    checkedStatus = false;
                }

                console.log(checkedStatus);
                if(list.ISCOMPLETED == true){
                    imageUrl= '/assests/images/EV1.png';
                    checkedStatus = true;
                }else if(list.WARNING == true){
                    imageUrl= '/assests/images/EV2.png'
                }else {
                    imageUrl= '/assests/images/EV3.png'
                }
                let switchId=list.SCHEDULE_id;
                return  (
                        <div className="taskItem clearfix">
                        <div className="forIconStatus"><img src={imageUrl} /></div>
                        <div class="forContent">
                            <div className="row">
                                <div class="col-sm-9">
                                    <div class="row">
                                        <h5 class="TodoSub col-sm-12">{list.TASK_SUBJECT}</h5>
                                        <p class="col-sm-12">{list.TASK_DESCRIPTION}</p> 
                                    </div>
                                </div>
                                <div className="col-sm-3">
                                    {
                                        list.ISCOMPLETED ? 
                                        <Switch  
                                        checked={true} 
                                            onChange={() => {this.props.toggelSwitch(switchId)}}
                                        /> : 
                                        <Switch
                                        onChange={() => {this.props.toggelSwitch(switchId)}}
                                        checked={false}
                                        className="react-switch"
                                    />
                                    }
                                </div>
                            </div>
                            
                            <div className="row">
                            <div class="col-sm-12">
                                <div>
                                    <ul class="todoInfo">
                                        <li>
                                            <div class="hd">Due on</div>
                                            <div class="data red">{list.TASK_DUEDATEFor}</div>
                                        </li>
                                        
                                        <li>
                                            <div class="hd">Completed by</div>
                                            <div class="data">{list.CompletedBy} </div>
                                        </li>
                                        <li>
                                            <div class="hd">Completed on</div>
                                            <div class="data">{list.completedonFor}</div>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            </div>
                        </div>
                </div>
               ) 
            })
           ) : <Empty
                image="https://gw.alipayobjects.com/zos/antfincdn/ZHrcdLPrvN/empty.svg"
                imageStyle={{
                    height: 60,
                }}
                description={
                    <span>
                    No Data to Display
                    </span>
                }
                >
                </Empty>;
        return (
            <div className="taskContainer clearfix">
                {item}
            </div>
          );
    }
}
  export default TaskItems;