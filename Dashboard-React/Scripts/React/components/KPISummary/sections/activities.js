import React from 'react';
import('../kpiDB.css');
import('../../CRMdash/CRMdash.css');
import { Table, Input, Button, Space } from 'antd';
import Highlighter from 'react-highlight-words';
import { SearchOutlined } from '@ant-design/icons';

const data = [
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
    {
        key: '5',
        name: 'Susanta',
        age: 32,
        address: 'London No. 2 Lake Park',
      },
      {
        key: '6',
        name: 'Indra',
        age: 32,
        address: 'London No. 2 Lake Park',
      },
      {
        key: '7',
        name: 'dgsagfsg',
        age: 32,
        address: 'London No. 2 Lake Park',
      },
      {
        key: '8',
        name: 'sdadsad ad',
        age: 32,
        address: 'London No. 2 Lake Park',
      },
      {
        key: '9',
        name: 'fafa asf',
        age: 32,
        address: 'London No. 2 Lake Park',
      },
      {
        key: '10',
        name: 'Jim das',
        age: 32,
        address: 'London No. 2 Lake Park',
      },
      {
        key: '11',
        name: 'Jim daaad',
        age: 32,
        address: 'London No. 2 Lake Park',
      },
  ];

class Activities extends React.Component {
    state = {
        searchText: '',
        searchedColumn: '',
        taransactionVolumn:[],
        taskVolumn: []
      };
      componentDidUpdate(){
        console.log(this.props)
      }
      componentDidMount(){
        console.log('update', this.props)
        this.setState({
          ...this.state,
          taransactionVolumn:  this.props.TransacVolume,
          taskVolumn:this.props.TaskVolume 
        })
      }

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
              onPressEnter={() => this.handleSearch(selectedKeys, confirm, dataIndex)}
              style={{ width: 188, marginBottom: 8, display: 'block' }}
            />
            <Space>
              <Button
                type="primary"
                onClick={() => this.handleSearch(selectedKeys, confirm, dataIndex)}
                icon={<SearchOutlined />}
                size="small"
                style={{ width: 90 }}
              >
                Search
              </Button>
              <Button onClick={() => this.handleReset(clearFilters)} size="small" style={{ width: 90 }}>
                Reset
              </Button>
            </Space>
          </div>
        ),
        filterIcon: filtered => <SearchOutlined style={{ color: filtered ? '#1890ff' : undefined }} />,
        onFilter: (value, record) =>
          record[dataIndex]
            ? record[dataIndex].toString().toLowerCase().includes(value.toLowerCase())
            : '',
        onFilterDropdownVisibleChange: visible => {
          if (visible) {
            setTimeout(() => this.searchInput.select(), 100);
          }
        },
        render: text =>
          this.state.searchedColumn === dataIndex ? (
            <Highlighter
              highlightStyle={{ backgroundColor: '#ffc069', padding: 0 }}
              searchWords={[this.state.searchText]}
              autoEscape
              textToHighlight={text ? text.toString() : ''}
            />
          ) : (
            text
          ),
      });
    
      handleSearch = (selectedKeys, confirm, dataIndex) => {
        console.log(selectedKeys)
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
    render() {
        
        const columns = [
          {
            title: 'Module',
            dataIndex: 'DOCTYPE',
            key: 'DOCTYPE',
            width: '40%',
            ...this.getColumnSearchProps('DOCTYPE'),
          },
          {
            title: 'Todays',
            dataIndex: 'TODAYCNT',
            key: 'TODAYCNT',
            width: '20%',
            align: 'right',
            ...this.getColumnSearchProps('TODAYCNT'),
          },
          {
            title: 'Total',
            dataIndex: 'TOTAL',
            key: 'TOTAL',
            align: 'right',
            ...this.getColumnSearchProps('TOTAL'),
          },
        ];
        const taskCol = [
          {
            title: 'Topic',
            dataIndex: 'TOPIC',
            key: 'TOPIC',
            width: '40%',
            ...this.getColumnSearchProps('TOPIC'),
          },
          {
            title: 'Points',
            dataIndex: 'POINT',
            key: 'POINT',
            width: '20%',
            align: 'right',
            ...this.getColumnSearchProps('POINT'),
          },
          {
            title: 'Rating',
            dataIndex: 'RATING',
            key: 'RATING',
            align: 'right',
            ...this.getColumnSearchProps('RATING'),
          },
        ];
        return (
            <div>
                <div className="backgroundedBoxes">
                    <div className="clearfix col-md-12">
                        <div className="flex-row space-between align-items-center">
                            <div className="flex-item itemType relative">
                                <div className="">
                                    <div className="text-center">
                                        <img src="../assests/images/DashboardIcons/online-activity.png" style={{width: "40px", marginBottom: "12px"}} />
                                    </div>
                                    <div className="valRound semiRound" >
                                        <span>{this.props.top.ACTIVCNT}</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">Activities</div>
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    <div className="text-center">
                                        <img src="../assests/images/DashboardIcons/gmail.png" style={{width: "40px", marginBottom: "12px"}} />
                                    </div>
                                    <div className="valRound semiRound" style={{background: "#f76d6d"}}>
                                        <span id="emailValue">{this.props.top.EMAILCNT}</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">Emails</div>
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    <div className="text-center">
                                        <img src="../assests/images/DashboardIcons/smartphone.png" style={{width: "40px", marginBottom: "12px"}} />
                                    </div>
                                    <div className="valRound semiRound" style={{background: "#6e98f5"}}>
                                        <span id="callsmsValue">{this.props.top.CALLSMSCNT}</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">Call/SMS</div>  
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    <div className="text-center">
                                        <img src="../assests/images/DashboardIcons/visitor.png" style={{width: "40px", marginBottom: "12px"}} />
                                    </div>
                                    <div className="valRound semiRound" style={{background: "#44c8cd"}}>
                                        <span id="visitValue">{this.props.top.VISITCNT}</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">Visits</div> 
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    <div className="text-center">
                                        <img src="../assests/images/DashboardIcons/account.png" style={{width: "40px", marginBottom: "12px"}} />
                                    </div>
                                    <div className="valRound semiRound" style={{background: "#9db53c"}}>
                                        <span id="socialValue">{this.props.top.SOCIALCNT}</span>
                                    </div>
                                    <div className="smallmuted ">Total</div>
                                    <div className="hdTag">Social</div> 
                                </div>
                            </div>
                            <div className="flex-item itemType relative">
                                <div className="">
                                    <div className="text-center">
                                        <img src="../assests/images/DashboardIcons/clipboards.png" style={{width: "40px", marginBottom: "12px"}} />
                                    </div>
                                    <div className="valRound semiRound" style={{background: "#8846c5"}}>
                                        <span id="otherValue">{this.props.top.OTHERSCNT}</span>
                                    </div>
                                    <div className="smallmuted">Total</div>
                                    <div className="hdTag">Others</div>  
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="row" style={{marginTop: "15px"}}>
                    <div className="col-md-6">
                        <div style={{fontSize: "16px", padding: "5px"}}>Lead Status</div>
                        <div className="colorTable">
                            <Table columns={columns} dataSource={this.state.taransactionVolumn} size="small" />
                        </div>   
                    </div>
                    <div className="col-md-6">
                        <div style={{fontSize: "16px", padding: "5px"}}>Task Volumn</div>
                        <div className="colorTable">
                            <Table columns={taskCol} dataSource={this.state.taskVolumn} size="small" />
                        </div> 
                    </div>
                   
                </div>
               
            </div>
        )
    }
}
export default Activities;