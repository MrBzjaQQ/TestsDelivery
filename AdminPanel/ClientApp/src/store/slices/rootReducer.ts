import { combineReducers } from '@reduxjs/toolkit';
import auth from "./auth/authSlice";
import {connectRouter} from "connected-react-router";
import history from "./history";

const rootReducer = combineReducers({
    userInfo: auth,
    router: connectRouter(history)
});

export type RootState = ReturnType<typeof rootReducer>;
export default rootReducer;