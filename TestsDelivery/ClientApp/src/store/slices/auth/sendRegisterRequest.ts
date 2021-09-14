import {createAsyncThunk} from "@reduxjs/toolkit";
import {RegisterState} from "./authSlice";

// TODO: move model to the correct place
export interface RegisterModel {
    userName: string;
    email: string;
    password: string;
    passwordConfirm: string;
}

const sendRegisterRequest = createAsyncThunk(
  'authentication/registerUser',
    async (registerModel: RegisterModel) => {
      const response = await fetch('/api/register', {
          method: 'POST',
          body: JSON.stringify(registerModel),
          headers: {
              'Content-Type': 'application/json'
          },
      });

      return (await response.json()) as RegisterState;
    }
);

export default sendRegisterRequest;