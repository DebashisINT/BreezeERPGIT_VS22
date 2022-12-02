import React from 'react'
import {render} from 'react-dom';



class NewsTickerPage extends React.Component{
  constructor(props) {
    super(props);
    this.state = { 
      feed: []
     };
  }

  componentDidMount() {
    
  }

  render() {
    return (
    <div>      
      
      <h1>News Ticker</h1>
      
     
      {this.state.items && this.state.items.map((items, i) => (
                <div key={i}>
                    <h1>{items.title}</h1>
                    <a href="">{items.link}</a>
                </div>
      ))}

    </div>
    );
  }
}
