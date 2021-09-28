import { createSlice } from "@reduxjs/toolkit";
import { AuthState, sendLoginRequest } from "./sendLoginRequest";
import sendRegisterRequest from "./sendRegisterRequest";

// TODO: move types to the correct state (?)
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