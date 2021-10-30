import thunkMiddleware from 'redux-thunk';
import rootReducer from './slices/rootReducer';
import {configureStore} from "@reduxjs/toolkit";

export const store = configureStore({
    reducer: rootReducer,
    middleware: getDefaultMiddleware => getDefaultMiddleware().concat(thunkMiddleware)
});

export type AppDispatch = typeof store.dispatch;

export default store;