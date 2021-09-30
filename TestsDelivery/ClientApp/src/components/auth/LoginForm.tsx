import React from "react";
import {Button, Checkbox, Form, Input} from "antd";
import {useDispatch} from "react-redux";
import sendLoginRequest from "../../store/slices/auth/sendLoginRequest";
import {Typography} from "antd";

const {Title} = Typography;

const LoginForm = () => {
    // TODO: Remember me = save token to storage
    // TODO: Error handling
    // TODO: Button align
    // TODO: Fields styling
    const dispatch = useDispatch();
    const onFinish = (values: any) => {
        console.log('Success:', values);
        dispatch(sendLoginRequest({
            userName: values.username,
            password: values.password,
            rememberMe: values.rememberMe
        }));
    };

    const onFinishFailed = (errorInfo: any) => {
        console.error(errorInfo);
    };

    return (
        <>
            <Title level={3} style={{textAlign: "center"}}>Login</Title>
            <Form
                name="basic"
                initialValues={{remember: true}}
                layout="vertical"
                onFinish={onFinish}
                onFinishFailed={onFinishFailed}
            >
                <Form.Item
                    label="Username"
                    name="username"
                    rules={[{required: true, message: 'Please input your username!'}]}
                >
                    <Input/>
                </Form.Item>

                <Form.Item
                    label="Password"
                    name="password"
                    rules={[{required: true, message: 'Please input your password!'}]}
                >
                    <Input.Password/>
                </Form.Item>

                <Form.Item name="remember" valuePropName="checked">
                    <Checkbox>Remember me</Checkbox>
                </Form.Item>

                <Form.Item>
                    <Button type="primary" htmlType="submit">
                        Submit
                    </Button>
                </Form.Item>
            </Form>
        </>
    );
};

export default LoginForm;