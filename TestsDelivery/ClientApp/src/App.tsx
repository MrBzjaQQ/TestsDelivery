import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import LoginComponent from "./components/auth/LoginComponent";
import RegisterComponent from "./components/auth/RegisterComponent";

import 'antd/dist/antd.css';
import './custom.css'

export default () => (
    <Layout>
        <Route exact path={'/login'} component={LoginComponent} />
        <Route exact path={'/register'} component={RegisterComponent} />
    </Layout>
);
