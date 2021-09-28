import {createAsyncThunk} from "@reduxjs/toolkit";
import http from "../../../infrastructure/http/http";

// TODO: move model to the correct place
export interface RegisterModel {
    userName: string;
    email: string;
    password: string;
    passwordConfirm: string;
}

export interface RegisterState {
    id: string;
    username: string;
    email: string;
}

export const sendRegisterRequest = createAsyncThunk(
  'authentication/registerUser',
    (registerModel: RegisterModel) => {
      return http.post('/api/register', registerModel);
    }
);

export default sendRegisterRequest;