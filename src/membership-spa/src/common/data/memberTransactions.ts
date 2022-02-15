import axios, { AxiosError, CancelTokenSource } from 'axios';
import { useEffect, useRef, useState } from 'react';
import { apiClient, errorHandler } from '../../util/axios';
import { ErrorResponse, ValidationErrorResponse } from '../types/errorResponse';

const url = '/Transaction/getformember';

interface ICallImmediate {
	immediate: boolean;
}

interface IMemberTransactionProps extends ICallImmediate {
	memberGuid?: string;
}
export interface MemeberTransactionType {
	firstName: string;
	lastName: string;
	email: string;
	loanNumber: string;
	amount: number;
	transactionDate: string;
}
export interface MemberTransactionReturn {
	loading: boolean;
	data: MemeberTransactionType[] | null;
	error: ErrorResponse | ValidationErrorResponse | null;
	execute: (memberGuid: string) => void;
}

export const useMemberTransaction = (
	props: IMemberTransactionProps,
): MemberTransactionReturn => {
	const cancelSource = useRef<CancelTokenSource | null>(null);
	const { immediate, memberGuid } = props;
	const [loading, setLoading] = useState<boolean>(false);
	const [data, setData] = useState<MemeberTransactionType[] | null>(null);
	const [error, setError] = useState<
		ErrorResponse | ValidationErrorResponse | null
	>(null);
	const execute = async (memberId: string) => {
		setLoading(true);
		const resp = await apiClient
			.get<MemeberTransactionType[]>(url, {
				cancelToken: cancelSource?.current?.token,
				params: { memberId },
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

		if (immediate && memberGuid) {
			execute(memberGuid);
		}
		return () => {
			cancelSource?.current?.cancel();
		};
	}, [immediate, memberGuid]);

	return { loading, data, error, execute };
};
