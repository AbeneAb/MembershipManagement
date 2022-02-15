import { useEffect } from 'react';
import { useHealthInfo } from '../../common/data/healthInfo';

export const useHealthInfoHook = () => {
	const classNames = (...classes: string[]) =>
		classes.filter(Boolean).join(' ');

	const { data, error, loading, execute } = useHealthInfo({
		immediate: true,
	});
	useEffect(() => {
		execute();
	}, []);
	return { data, error, loading, classNames };
};
