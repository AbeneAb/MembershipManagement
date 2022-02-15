/* eslint-disable sonarjs/no-duplicate-string */


import React from 'react';
import { useLocation } from 'react-router-dom';

function classNames(...classes: string[]) {
	return classes.filter(Boolean).join(' ');
}

export const Footer: React.FC = () => {
	const location = useLocation();
	const { pathname } = location;
	const splitLocation = pathname.split('/');
	return (
		<div className="flex flex-col items-center justify-between w-full px-2 mx-auto text-white bg-indigo-600 sm:flex-row h-14 sm:px-4 lg:px-6">
			<div className="flex-1 mt-3 space-y-2 sm:mt-0">
				<p className="text-xs font-medium md:text-base md:">
					Stay Safe. Wash your hands
				</p>
			</div>
			<div className="flex flex-1 space-x-4" aria-label="footer-menus">
				<button
					type="button"
					className={classNames(
						splitLocation?.[1] === 'about'
							? 'bg-indigo-700 text-gray-50 font-medium'
							: 'text-indigo-100 hover:text-white',
						'px-3 py-2 font-medium text-sm rounded-md',
					)}
					aria-current={splitLocation?.[1] === 'aboutus' ? 'page' : undefined}>
					About us
				</button>

				<button
					type="button"
					className={classNames(
						splitLocation?.[1] === 'services'
							? 'bg-indigo-700 text-gray-50 font-medium'
							: 'text-indigo-100 hover:text-white',
						'px-3 py-2 font-medium text-sm rounded-md',
					)}
					aria-current={splitLocation?.[1] === 'services' ? 'page' : undefined}>
					Services
				</button>

				<button
					type="button"
					className={classNames(
						splitLocation?.[1] === 'contact-us'
							? 'bg-indigo-700 text-gray-50 font-medium'
							: 'text-indigo-100 hover:text-white',
						'px-3 py-2 font-medium text-sm rounded-md',
					)}
					aria-current={
						splitLocation?.[1] === 'contact-us' ? 'page' : undefined
					}>
					Contact us
				</button>
			</div>
		</div>
	);
};
