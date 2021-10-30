import {createSlice, Draft} from "@reduxjs/toolkit";
import { AuthState, sendLoginRequest } from "./sendLoginRequest";
import sendRegisterRequest from "./sendRegisterRequest";

const initialState: AuthState = {
    authToken: '',
    userName: ''
};

export const authSlice = createSlice({
    name: 'authentication',
    initialState,
    reducers: {
        clearState: (state: Draft<AuthState>) => {
            state.authToken = '';
            state.userName = '';
        },
    },
    extraReducers: (builder) => {
      builder.addCase(sendLoginRequest.fulfilled, (state, action) => {
          return action.payload;
      });

      builder.addCase(sendRegisterRequest.fulfilled, (state, action) => {
          console.log('Register action payload', action.payload);
      });
    },
});

export default authSlice.reducer;