import React, { useEffect, useState } from 'react';
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import {
	DeepMap,
	FieldError,
	useForm,
	UseFormHandleSubmit,
	UseFormRegister,
	UseFormWatch,
} from 'react-hook-form';
import { AxiosError } from 'axios';
import { apiClient, errorHandler } from '../../util/axios';
import {
	ErrorResponse,
	ValidationErrorResponse,
} from '../../common/types/errorResponse';
import {
	useMemberTransaction,
	MemeberTransactionType,
} from '../../common/data/memberTransactions';

import { useMembers, MemberType } from '../../common/data/members';

export interface MemberTransactionForm {
	memberId?: string;
	transactionDate?: string;
	loanNumber?: string;
	amount?: number;
}
const schema = yup.object().shape({
	memberId: yup.string().required(),
	transactionDate: yup.date().required(),
	loanNumber: yup.string().required(),
	amount: yup.number().required().min(0.1),
});

export interface MemberTransactionFormReturnType {
	classNames: (...classes: string[]) => string;

	enableTable: boolean;
	setEnableTable: React.Dispatch<React.SetStateAction<boolean>>;

	loadingMemberTransactions: boolean;
	errorLoadingMemberTransactions:
		| ErrorResponse
		| ValidationErrorResponse
		| null;
	memberTransactionList: MemeberTransactionType[] | null;

	loadingMembers: boolean;
	errorLoadingMembers: ErrorResponse | ValidationErrorResponse | null;
    memberList : MemberType[] | null;

	register: UseFormRegister<MemberTransactionForm>;
	handleSubmit: UseFormHandleSubmit<MemberTransactionForm>;
	watch: UseFormWatch<MemberTransactionForm>;
	errors: DeepMap<MemberTransactionForm, FieldError>;
	isSubmitting: boolean;
	onValidSubmitHandler: (data: MemberTransactionForm) => void;
	onInvalidSubmitHandler: (
		errors: DeepMap<MemberTransactionForm, FieldError>,
	) => void;
}
export const useMembersFroms = (): MemberTransactionFormReturnType => {
	const classNames = (...classes: string[]) =>
		classes.filter(Boolean).join(' ');

	const [enableTable, setEnableTable] = useState<boolean>(false);
    const [loadMemrberTransaction,setLoadmemberTransaction] = useState<boolean>(false);
	const {
		register,
		handleSubmit,
		watch,
		formState: { errors, isSubmitting },
	} = useForm<MemberTransactionForm>({ resolver: yupResolver(schema) });

	const {
		execute: getMemberTransaction,
		loading: loadingMemberTransactions,
		error: errorLoadingMemberTransactions,
		data: memberTransactionList,
	} = useMemberTransaction({
		immediate: false,
	});

	const {
		loading: loadingMembers,
		error: errorLoadingMembers,
		data: memberList,
	} = useMembers({ immediate: true });

	const watchMemberId = watch('memberId');

	useEffect(() => {
		if (watchMemberId && watchMemberId !== 'Select Member' || loadMemrberTransaction) {
			getMemberTransaction(watchMemberId!);
			setEnableTable(true);
		} else {
			setEnableTable(false);
		}
	}, [watchMemberId,loadMemrberTransaction]);

	const onValidSubmitHandler = async (data: MemberTransactionForm) => {
		console.log(data);
		const resp = await apiClient
			.post<string>('/Transaction/create', data)
			.catch((reason: AxiosError<ErrorResponse>) => {
				const errorMessage = errorHandler(reason);
				console.log(errorMessage);
			});

		if (resp && resp?.data) {
            setLoadmemberTransaction(true);
			console.log(resp.data);
		}
	};
	const onInvalidSubmitHandler = (
		err: DeepMap<MemberTransactionForm, FieldError>,
	) => {
		console.log(err);
	};
	return {
		classNames,

		setEnableTable,
		enableTable,

		loadingMemberTransactions,
		errorLoadingMemberTransactions,
		memberTransactionList,

        loadingMembers,
        errorLoadingMembers,
        memberList,

		register,
		handleSubmit,
		watch,
		errors,
		isSubmitting,
		onValidSubmitHandler,
		onInvalidSubmitHandler,
	};
};
