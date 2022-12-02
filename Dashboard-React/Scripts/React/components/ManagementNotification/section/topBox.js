
import React from 'react';

class TopBox  extends React.Component {
    constructor(props) {
        super(props);
    }
    componentDidMount(){
        console.log('props', this.props)
    }
    render() {
        return (
            <div className={`flex-item itemType relative shwDet ${this.props.addClass}`} onClick={() =>{this.props.clicked(this.props.Title)}}>
                <div className="">
                    <div className="valRound" style={{background:this.props.bgColor}}>
                        <span>{this.props.value}</span>
                    </div>
                    <div className="smallmuted">{this.props.smtl}</div>
                    <div className="hdTag">{this.props.Title} </div>
                </div>
            </div>
        )
    }
}
export default TopBox;
        