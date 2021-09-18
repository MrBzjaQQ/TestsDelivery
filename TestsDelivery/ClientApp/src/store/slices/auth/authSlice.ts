import { createSlice } from "@reduxjs/toolkit";
import sendLoginRequest from "./sendLoginRequest";
import sendRegisterRequest from "./sendRegisterRequest";

// TODO: move types to the correct state (?)
export interface AuthState {
    userName?: string,
    authToken?: string
}

export interface RegisterState {
    id: string;
    username: string;
    email: string;
}

const initialState: AuthState = {};

export const authSlice = createSlice({
    name: 'authentication',
    initialState,
    reducers: {

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