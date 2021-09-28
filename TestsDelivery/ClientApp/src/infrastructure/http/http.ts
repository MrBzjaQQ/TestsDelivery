import axios from 'axios';
import {
    onFulfilled,
    onRejected
} from "./axios.tokenInterceptor";

import {
    onFulfilledResponse,
    onFulfilledRequest,
    onRejectedResponse,
    onRejectedRequest
} from "./axios.loaderInterceptor";

axios.interceptors.request.use(onFulfilled, onRejected);

axios.interceptors.request.use(onFulfilledRequest, onRejectedRequest);
axios.interceptors.response.use(onFulfilledResponse, onRejectedResponse);

export default axios;