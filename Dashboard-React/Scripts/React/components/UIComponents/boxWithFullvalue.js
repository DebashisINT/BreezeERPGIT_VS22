import React from 'react';
import { numFormatterLocal, numberWithCommas } from '../helpers/helperFunction';
import('./withFull.css');
const UiBox = (props) => {
    let bgIcon = props.icColor;
    return (
        <div className="flex-item itemType w18 relative">
            <div className="widgBox" {...props}>
                <div class="valRound" style={{background: bgIcon }}>
                    <div class="showFullInfo">{props.value ? numberWithCommas(props.value) : 0.00}</div>
                    <div>{props.value ? numFormatterLocal(props.value) : 0}</div>
                </div>
                <div class="hdTag">{props.title}</div>
                <div class="smallmuted">{props.subtitle}</div> 
            </div>
        </div>
    )
}
export default UiBox;