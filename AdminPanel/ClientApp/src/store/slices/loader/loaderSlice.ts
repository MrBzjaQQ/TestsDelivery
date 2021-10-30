import {createSlice, Draft} from "@reduxjs/toolkit";

export interface LoaderState {
    loaderCount: number;
};

const initialState = {
    loaderCount: 0
} as LoaderState;

export const loaderSlice = createSlice({
    name: 'loader',
    initialState,
    reducers: {
        incrementLoader: (state: Draft<LoaderState>) => {
            state.loaderCount++;
        },
        decrementLoader: (state: Draft<LoaderState>) => {
            state.loaderCount--;
        }
    }
});

export default loaderSlice.reducer;