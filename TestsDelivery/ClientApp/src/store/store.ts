import {configureStore} from "@reduxjs/toolkit";
import rootReducer from './slices/rootReducer';
import {useDispatch} from "react-redux";
import thunkMiddleware from 'redux-thunk';

export const store = configureStore({
    reducer: rootReducer,
    middleware: getDefaultMiddleware => getDefaultMiddleware().concat(thunkMiddleware)
});

export type AppDispatch = typeof store.dispatch;
export const useAppDispatch = () => useDispatch<AppDispatch>();

export default store;