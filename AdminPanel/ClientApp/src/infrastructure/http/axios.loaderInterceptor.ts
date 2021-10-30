import {AxiosError, AxiosRequestConfig, AxiosResponse} from "axios";
import store from "../../store/store";
import { loaderSlice } from "../../store/slices/loader/loaderSlice";
const { incrementLoader, decrementLoader } = loaderSlice.actions;

export const onFulfilledRequest = (config: AxiosRequestConfig): AxiosRequestConfig => {
   store.dispatch(incrementLoader());
   return config;
};

export const onRejectedRequest = (error: AxiosError) => {
   return Promise.reject(error);
};

export const onFulfilledResponse = (response: AxiosResponse): AxiosResponse => {
   store.dispatch(decrementLoader());
   return response;
};

export const onRejectedResponse = (error: AxiosError) => {
   store.dispatch(decrementLoader());
   return Promise.reject(error);
};

export default {
   onFulfilledRequest,
   onRejectedRequest,
   onFulfilledResponse,
   onRejectedResponse
};

