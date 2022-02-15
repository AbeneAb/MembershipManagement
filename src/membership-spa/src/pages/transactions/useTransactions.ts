import { useEffect } from 'react';
import { useTransactions } from '../../common/data/transactions';

export const useTransactionHook = () => {
	const classNames = (...classes: string[]) =>
		classes.filter(Boolean).join(' ');

	const { data, error, loading, execute } = useTransactions({
		immediate: false,
	});
	useEffect(() => {
		execute();
	}, []);
	return { data, error, loading, classNames };
};
