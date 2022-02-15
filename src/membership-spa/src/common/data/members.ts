import axios, { AxiosError, CancelTokenSource } from 'axios';
import { useEffect, useRef, useState } from 'react';
import { apiClient, errorHandler } from '../../util/axios';
import { ErrorResponse, ValidationErrorResponse } from '../types/errorResponse';

const url = '/Member/all';

interface IMemberProps {
	immediate: boolean;
}
export interface MemberType {
    id:string;
	firstName: string;
	lastName: string;
	email: string;
	telephone?: string;
}
export interface MemberReturn {
	loading: boolean;
	data: MemberType[] | null;
	error: ErrorResponse | ValidationErrorResponse | null;
	execute: (memberId: string) => void;
}

export const useMembers = (props: IMemberProps): MemberReturn => {
	const cancelSource = useRef<CancelTokenSource | null>(null);
	const { immediate } = props;
	const [loading, setLoading] = useState<boolean>(false);
	const [data, setData] = useState<MemberType[] | null>(null);
	const [error, setError] = useState<
		ErrorResponse | ValidationErrorResponse | null
	>(null);
	const execute = async () => {
		setLoading(true);
		const resp = await apiClient
			.get<MemberType[]>(url, {
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
