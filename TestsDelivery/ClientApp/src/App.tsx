import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Login from "./components/Auth/Login";
import RegistrationForm from "./components/Auth/Register";

import 'antd/dist/antd.css';
import './custom.css'

export default () => (
    <Layout>
        <Route exact path={'/login'} component={Login} />
        <Route exact path={'/register'} component={RegistrationForm} />
    </Layout>
);
