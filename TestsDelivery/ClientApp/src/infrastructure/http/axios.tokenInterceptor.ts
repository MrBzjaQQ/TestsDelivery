import {AxiosError, AxiosRequestConfig} from "axios";
import localStorage from "../../store/helpers/localStorage";

export const onFulfilled = (request: AxiosRequestConfig) => {
    const token = localStorage.getAuthToken();
    console.log('onFulfilled', request);
    if (!token)
        return request;

    request.headers.authorization = `Bearer ${token}`;
    return request;
};

export const onRejected = (error: AxiosError) => {
    return Promise.reject(error);
};

export default {
    onFulfilled,
    onRejected
};