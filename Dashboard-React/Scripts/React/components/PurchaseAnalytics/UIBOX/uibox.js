
import React from 'react';
import { render } from 'react-dom';
import('./uibox.css');

class uiBox  extends React.Component {
    constructor(props) {
        super(props);
      }
      componentDidMount(){
          console.log('props', this.props)
      }
    render() {
        let value = "0.00";
        if(this.props.data){
            value = this.numberWithCommas(this.props.data)
        }
        return (
            
            <div className="col-md-3">
                <div className="widget" style={
                    {
                        background: this.props.bgColor
                    }
                }>
                    <div className="iconBox">
                        <img src={this.props.iconUrl} style={{maxWidth: "32px"}} />
                    </div>
                        <div className="textInbox">
                            <div className="wdgLabel">{this.props.uititle}</div>
                            <div className="wdgNumber">{value}</div>
                        </div>
                    </div>
            </div>
        )
        
    }
    numberWithCommas = (x) => {
        x = x.toString();
        if (x.toString().indexOf('.') == -1) {
            var lastThree = x.substring(x.length - 3);
            var otherNumbers = x.substring(0, x.length - 3);
            if (otherNumbers != '')
                lastThree = ',' + lastThree;
            var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
        } else {
            var dec = x.substr(x.indexOf('.') + 1, x.length);
            x = x.substr(0, x.indexOf('.'))
            var lastThree = x.substring(x.length - 3);
            var otherNumbers = x.substring(0, x.length - 3);
            if (otherNumbers != '')
                lastThree = ',' + lastThree;
            var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree + '.' + dec;
        }
        return res;
    }   
}
export default uiBox;