import * as React from 'react';
import {useDispatch, useSelector} from "react-redux";
import {RootState} from "../../../store/slices/rootReducer";
import {Button} from "antd";
import {useHistory} from "react-router";
import { authSlice } from "../../../store/slices/auth/authSlice";
import "./userInfo.scss";

const { clearState } = authSlice.actions;

export interface UserInfoProps {
    className?: string
};

export const UserInfo = (props: UserInfoProps) => {
    const history = useHistory();
    const dispatch = useDispatch();
    const className = props?.className ?? 'user-info';

    const { authInfo } = useSelector((state : RootState) => ({
        authInfo: state.userInfo
    }));

    const logout = () => {
        dispatch(clearState());
        history.push({pathname: '/login'});
    };

    const login = () => {
        history.push({pathname: '/login'})
    };

    const content = authInfo?.userName
        ? <>
            <div className={`${className}__user-name`}>{ `You're logged in as ${authInfo.userName}` }</div>
            <Button
                type="link"
                className={`${className}__link`}
                onClick={logout}
            >Logout</Button>
        </>
        : <Button
            type="link"
            className={`${className}__link`}
            onClick={login}
        >Login</Button>;

    return <div className={className}>
        {content}
        </div>;
};

export default UserInfo;