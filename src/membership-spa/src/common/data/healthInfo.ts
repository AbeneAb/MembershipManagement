import axios, { AxiosError, CancelTokenSource } from 'axios';
import { useEffect, useRef, useState } from 'react';
import { apiClient, errorHandler } from '../../util/axios';
import { ErrorResponse, ValidationErrorResponse } from '../types/errorResponse';

const url = '/Health/getall';

interface IHealthProps {
	immediate: boolean;
}
export interface IHelathInfoType {
	id: string;
	heartRate: number;
	systolic: number;
	diastolic: number;
	time: Date;
}
export interface HealthInfoReturn {
	loading: boolean;
	data: IHelathInfoType[] | null;
	error: ErrorResponse | ValidationErrorResponse | null;
	execute: () => void;
}

export const useHealthInfo = (props: IHealthProps): HealthInfoReturn => {
	const cancelSource = useRef<CancelTokenSource | null>(null);
	const { immediate } = props;
	const [loading, setLoading] = useState<boolean>(false);
	const [data, setData] = useState<IHelathInfoType[] | null>(null);
	const [error, setError] = useState<
		ErrorResponse | ValidationErrorResponse | null
	>(null);
	const execute = async () => {
		setLoading(true);
		const resp = await apiClient
			.get<IHelathInfoType[]>(url, {
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
