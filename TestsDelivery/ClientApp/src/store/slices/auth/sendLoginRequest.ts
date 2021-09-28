import {createAsyncThunk} from "@reduxjs/toolkit";
import http from "../../../infrastructure/http/http";

// TODO: sendLoginRequest.fulfilled is undefined.
// TODO: move model to the correct place
export interface AuthState {
    userName?: string,
    authToken?: string
}

export interface LoginModel {
    userName: string,
    password: string,
    rememberMe: boolean
}

export const sendLoginRequest = createAsyncThunk(
    'authentication/loginUser',
     async (loginModel: LoginModel) => {
      return (await http.post('/api/login', loginModel)).data as AuthState;
    }
);

export default sendLoginRequest;