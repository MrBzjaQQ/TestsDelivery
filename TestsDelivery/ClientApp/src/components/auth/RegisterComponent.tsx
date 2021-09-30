import React from 'react';
import RegisterForm from "./RegisterForm";
import CenterContainer from "../shared/centerContainer/CenterContainer";
import {Card} from "antd";

interface RegisterComponentStyle {
    width: number;
    height: number;
}

const style: RegisterComponentStyle = {
    width: 500,
    height: 485
};

const RegisterComponent = () => (
    <CenterContainer>
        <Card style={style}>
            <RegisterForm/>
        </Card>
    </CenterContainer>
);

export default RegisterComponent;