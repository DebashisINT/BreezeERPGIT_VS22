import React from 'react';
import('./uiBox.css');
const UiBox = (props) => {
    return (
        <div className="widgBox" >
            <div className="d-flex align-items-center">
                <div className="icon" style={{background: props.color}}><img src={props.imageURI} style={{maxWidth: "32px"}} /></div>
                <div className="flex-grow-1 txt">{props.titletext}</div>
            </div>
            <div className="Numb">{props.data}</div>
            <div className="text-center">{props.subTitle}</div>
        </div>
    )
}
export default UiBox;