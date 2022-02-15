import React from 'react';
import { useHistory } from 'react-router-dom';

const NotFoundPage: React.FunctionComponent = () => {
	const history = useHistory();

	return (
		<div className="flex flex-col items-center justify-start flex-1 w-screen h-screen px-10 mt-24 bg-center bg-no-repeat bg-contain sm:bg-left-bottom">
			<p className="text-5xl font-bold font-dark">404</p>
			<p className="text-2xl font-light leading-normal md:text-3xl">
				{"Sorry we couldn't find this page. "}
			</p>
			<p className="">
				Maybe go back to the main page and try searching something else?
			</p>
			<p>
				Email{' '}
				<span className="text-indigo-600">thynksoftware.com</span> for
				help.
			</p>

			<button
				type="button"
				onClick={() => {
					history.push('/');
				}}
				className="items-center w-full col-span-1 px-6 py-3 mt-10 text-sm font-medium text-white bg-indigo-600 border border-transparent rounded-md shadow-sm sm:text-base sm:w-auto sm:inline-flex hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500">
				back to main page
			</button>
		</div>
	);
};

export default NotFoundPage;
