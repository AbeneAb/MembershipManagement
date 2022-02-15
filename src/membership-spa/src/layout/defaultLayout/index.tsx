import React from 'react';
import { NavBar } from '../../common/components/navBar';
import { Footer } from '../../common/components/footer';

const DefaultLayout: React.FunctionComponent = ({ children }) => (
	<div className="relative flex flex-col flex-grow min-h-screen overflow-scroll bg-gray-50">
		<NavBar />
		<>{children}</>
		<Footer />
	</div>
);
export default DefaultLayout;
