import React from 'react'; 
  
const ListRow=(props)=>
{
  let colClass ="col-sm-"+ props.sSize;
  return (
      <React.Fragment>
        <div className="col-sm-3">
            <h6 className="mb-0">{props.title? props.title : null}</h6>
        </div>
        <div className={colClass}>{props.value? props.value : null}</div>
    </React.Fragment>
  );
}
  
export default ListRow;