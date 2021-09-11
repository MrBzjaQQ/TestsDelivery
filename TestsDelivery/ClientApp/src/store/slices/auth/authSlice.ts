import { createSlice } from "@reduxjs/toolkit";
import sendLoginRequest from "./sendLoginRequestThunk";

export interface AuthState {
    email?: string,
    authToken?: string
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
    },
});

// export const { loginUser } = authSlice.actions;

export default authSlice.reducer;