import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from 'axios';
import { ErrorResponse } from '../common/types/errorResponse';

export const apiClient = axios.create({
	baseURL: process.env.REACT_APP_API_ENDPOINT,
	headers: { 'Content-Type': 'application/json' },
});

const requestHandler = (request: AxiosRequestConfig) =>
	// Token will be dynamic so we can use any app-specific way to always
	// fetch the new token before making the call
	request;
const responseHandler = (response: AxiosResponse) => response;
	
const requestErrorHandler = (error: any) => Promise.reject(error);

const responseErrorHandler = (error: AxiosResponse<ErrorResponse>) => Promise.reject(error)

apiClient.interceptors.request.use(
	request => requestHandler(request),
	error => requestErrorHandler(error),
);

apiClient.interceptors.response.use(
	response => responseHandler(response),
	error => responseErrorHandler(error),
);



export const errorHandler = (reason: AxiosError<ErrorResponse>) => {
	if (axios.isCancel(reason)) return 'Request Cancelled';
	if (reason.code === 'ECONNABORTED') return 'A timeout occurred';
	if (reason.response?.status === 404) return 'Resource Not Found';
	if (reason.response?.status === 400 || reason.response?.status === 500)
		return reason.response.data.title;

	return 'An unexpected error has occured';
};