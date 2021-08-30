import 'bootstrap/dist/css/bootstrap.css';

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { ConnectedRouter } from 'connected-react-router';
import { createBrowserHistory } from 'history';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import {Router} from "react-router";

// Create browser history to use in the Redux store
const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href') as string;
const history = createBrowserHistory({ basename: baseUrl });

// Get the application-wide store instance, prepopulating with state from the server where available.
// const store = configureStore(history);

/* ReactDOM.render(
    <ConnectedRouter history={history}>
        <App />
    </ConnectedRouter>,
    document.getElementById('root'));
 */

ReactDOM.render(
    <Router history={history}>
        <App />
    </Router>,
    document.getElementById('root'));

registerServiceWorker();
