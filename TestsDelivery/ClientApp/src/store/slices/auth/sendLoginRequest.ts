import { AuthState } from './authSlice';
import {createAsyncThunk} from "@reduxjs/toolkit";

// TODO: move model to the correct place
interface LoginModel {
    userName: string,
    password: string
}

const sendLoginRequest = createAsyncThunk(
    'authentication/loginUser',
    async (loginModel: LoginModel) => {
      const response = await fetch('/api/login', {
          method: 'POST',
          body: JSON.stringify(loginModel),
          headers: {
              'Content-Type': 'application/json'
          },
      });

      return (await response.json()) as AuthState;
    }
);

export default sendLoginRequest;