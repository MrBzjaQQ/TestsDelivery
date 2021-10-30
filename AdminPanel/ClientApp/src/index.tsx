import 'bootstrap/dist/css/bootstrap.css';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import {ConnectedRouter} from 'connected-react-router';
import registerServiceWorker from './registerServiceWorker';
import store from "./store/store";
import {Provider} from 'react-redux';
import history from './store/slices/history';
import App from './App';

// Create browser history to use in the Redux store

ReactDOM.render(
    <Provider store={store}>
        <ConnectedRouter history={history}>
            <App/>
        </ConnectedRouter>
    </Provider>,
    document.getElementById('root'));

registerServiceWorker();
