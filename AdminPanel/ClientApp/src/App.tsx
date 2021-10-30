import * as React from 'react';
import {Route} from 'react-router';
import { Layout } from "antd";
import LoginComponent from "./components/auth/LoginComponent";
import RegisterComponent from "./components/auth/RegisterComponent";

import 'antd/dist/antd.css';
import './custom.css'
import UserInfo from "./components/header/UserInfo/UserInfo";

const { Header, Footer, Content } = Layout;

// TODO: move styles to the separate file

export default () => (
    <Layout style={{height: '100%'}}>
        <Header style={{ zIndex: 1, width: '100%' }} >
            <UserInfo className="user-info" />
        </Header>
        <Content style={{minHeight: 'calc(100% - 134px)'}}>
            <Route exact path={'/login'} component={LoginComponent}/>
            <Route exact path={'/register'} component={RegisterComponent}/>
        </Content>
        <Footer style={{ textAlign: 'center' }}>TODO: header & footer text</Footer>
    </Layout>
);
