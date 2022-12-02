
import * as React from 'react';
import ReactDOM from 'react-dom';
import Main from './components/main';
import { store } from './store/store';
import { Provider } from 'react-redux';

// class Main extends React.Component {
//     render() {
//         return <h2>Hi, I am a Car!</h2>;
//     }
// }
// without redux
//ReactDOM.render(<Main />, document.getElementById('root'));

// With redux toolkit
ReactDOM.render(
    <Provider store={store}>
        <Main />
    </Provider>, 
    document.getElementById('root')
);