
import React from 'react';
class TopBox  extends React.Component {
    constructor(props) {
        super(props);
    }
    componentDidMount(){
        console.log('props', this.props)
    }
    render() {
        let titleString = this.props.Title;
        return (
            <div className={`flex-item itemType relative shwDet ${this.props.addClass}`}
             onClick={()=>{ this.props.clicked(titleString)} }>
                <div className="">
                    <div className="valRound" style={{background:this.props.bgColor}}>
                        <span>{this.props.value}</span>
                    </div>
                    <div className="hdTag" style={{textTransform: "uppercase"}}>{this.props.Title} </div>
                    <div className="smallmuted">{this.props.smtl}</div>
                </div>
            </div>
        )
    }
}
export default TopBox;
        