import React, {Component } from 'react';
import { Table } from 'antd'
class Timeline extends Component {
    constructor(props) {
        super(props);
        this.state = {  }
    }
    render() { 
        const data = this.props.data;

        return ( 
            <React.Fragment>
                <div className="colorTable">
                <Table
                dataSource={data} 
                columns={columns}
                size="small"
                pagination={false}  
                scroll={{ x: 2800 }}
                bordered 
                />
                </div>
                
            </React.Fragment>
         );
    }
}								
const columns = [
    { title: 'Party Id', dataIndex: 'PARTYID', key: 'PARTYID', },
    { title: 'Project Code', dataIndex: 'PROJECT_CODE', key: 'PARTYID'},
    { title: 'Project Name', dataIndex: 'PROJECT_NAME', key: 'PARTYID', width: 300},
    { title: 'Customer Name', dataIndex: 'CUSTOMERNAME', key: 'PARTYID', width: 300},
    { title: 'Project Manager', dataIndex: 'PROJECT_MANAGER', key: 'PARTYID', width: 200},
    { title: 'Project Stage', dataIndex: 'PROJECTSTAGE', key: 'PARTYID', width: 150},
    { title: 'Project Status', dataIndex: 'PROJECTSTATUS', key: 'PARTYID', width: 200},
    { title: 'Actual Start Date', dataIndex: 'PROJ_ACTUALSTARTDATE', key: 'PARTYID', width: 230},
    { title: 'Est. Start Date', dataIndex: 'PROJ_ESTIMATESTARTDATE', key: 'PARTYID', width: 220},
    { title: 'End Date', dataIndex: 'PROJ_ACTUALENDDATE', key: 'PARTYID', width: 150},
    { title: 'Est. Hours', dataIndex: 'PROJ_ESTIMATEHOURS', key: 'PARTYID', width: 150},
    { title: 'Actual Hours', dataIndex: 'PROJ_ACTUALHOURS', key: 'PARTYID', width: 150},
    { title: 'Est. Labour Cost', dataIndex: 'PROJ_ESTLABOURCOST', key: 'PARTYID', width: 200},
    { title: 'Actual Labour Cost', dataIndex: 'PROJ_ACTUALLABOURCOST', key: 'PARTYID', width: 200},
    { title: 'Est. Total Cost', dataIndex: 'PROJ_ESTTOTALCOST', key: 'PARTYID', width: 200}
];
export default Timeline;