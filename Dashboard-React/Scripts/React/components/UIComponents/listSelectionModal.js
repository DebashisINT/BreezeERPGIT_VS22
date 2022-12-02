import React from "react";
import { Input,  Form, Modal, Table } from 'antd';
class ListSelectModal extends React.Component {
    constructor(props){
        super(props)
    }
    
    render(){
        const { title, visible, onOkClick, classes, widthSize, onCancelProp, onformFinish, 
            selectType, rowSelection, colData, sourcedata} = this.props;
            console.log("modal props", this.props)
        return (
            <React.Fragment>
                <Modal
                        title={title}
                        visible={visible}
                        onOk={onOkClick}
                        className={classes}
                        width={widthSize}
                        onCancel={onCancelProp}
                        >
                        <div className="searchCustForm">
                            <Form name="projectMSearch" 
                                onFinish={onformFinish} 
                            
                                >
                                <Form.Item name="searchCust">
                                    <Input placeholder="Search " id="data1" className="searchCust" autocomplete="off" />
                                </Form.Item>
                            </Form>
                        </div>
                        <div className="colorTable">
                            <Table
                                rowSelection={{
                                type: this.props.selectType,
                                    ...rowSelection,
                                }}
                                scroll={{ y: 300 }}
                                pagination={false}
                                columns={colData}
                                dataSource={sourcedata}
                                size="small"
                            />
                        </div>
                    </Modal>
            </React.Fragment>
        )
    }
}
export default ListSelectModal;

