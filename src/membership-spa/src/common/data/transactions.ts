import axios, { AxiosError, CancelTokenSource } from 'axios';
import { useEffect, useRef, useState } from 'react';
import { apiClient, errorHandler } from '../../util/axios';
import { ErrorResponse, ValidationErrorResponse } from '../types/errorResponse';
import {MemeberTransactionType} from './memberTransactions'


const url = '/Transaction/getall';

interface IMemberProps {
	immediate: boolean;
}

export interface TransactionReturn {
	loading: boolean;
	data: MemeberTransactionType[] | null;
	error: ErrorResponse | ValidationErrorResponse | null;
	execute: () => void;
}

export const useTransactions = (props: IMemberProps): TransactionReturn => {
	const cancelSource = useRef<CancelTokenSource | null>(null);
	const { immediate } = props;
	const [loading, setLoading] = useState<boolean>(false);
	const [data, setData] = useState<MemeberTransactionType[] | null>(null);
	const [error, setError] = useState<
		ErrorResponse | ValidationErrorResponse | null
	>(null);
	const execute = async () => {
		setLoading(true);
		const resp = await apiClient
			.get<MemeberTransactionType[]>(url, {
				cancelToken: cancelSource?.current?.token,
			})
			.catch((reason: AxiosError<ErrorResponse>) => {
				const errorMessage = errorHandler(reason);

				setError({ status: reason.response!.status, errors: errorMessage });
				setData(null);
				setLoading(false);
			});

		if (resp && resp?.data) {
			setData(resp.data);
			setLoading(false);
			setError(null);
		}
	};
	useEffect(() => {
		cancelSource.current = axios.CancelToken.source();

		if (immediate) {
			execute();
		}
		return () => {
			cancelSource?.current?.cancel();
		};
	}, [immediate]);

	return { loading, data, error, execute };
};
