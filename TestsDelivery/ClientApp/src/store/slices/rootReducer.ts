import { combineReducers } from '@reduxjs/toolkit';
import authReducer from "./auth/authSlice";
import {connectRouter} from "connected-react-router";
import history from "./history";

const rootReducer = combineReducers({
    authReducer,
    router: connectRouter(history)
});

export type RootState = ReturnType<typeof rootReducer>;
export default rootReducer;