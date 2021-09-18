import React from 'react';
import CenterContainer from "../shared/centerContainer/CenterContainer";
import LoginForm from "./LoginForm";
import {Card} from "antd";

interface LoginComponentStyle {
  width: number;
  height: number;
}

const style : LoginComponentStyle = {
    width: 560,
    height: 300
};

const LoginComponent = () => (
    <CenterContainer>
        <Card style={style}>
            <LoginForm/>
        </Card>
    </CenterContainer>
);

export default LoginComponent;