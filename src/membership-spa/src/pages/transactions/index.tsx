/* eslint-disable jsx-a11y/label-has-associated-control */
import React from 'react';
import { Table } from 'antd';
import 'antd/dist/antd.min.css';
import { useTransactionHook } from './useTransactions';

const Transactions: React.FunctionComponent = () => {
	const columns = [
		{ title: 'First Name', dataIndex: 'firstName', key: 'firstName' },
		{ title: 'Last Name', dataIndex: 'lastName', key: 'lastName' },
        { title: 'Email', dataIndex: 'email', key: 'email' },
		{ title: 'Loan Number', dataIndex: 'loanNumber', key: 'bookingDate' },
		{ title: 'Amount', dataIndex: 'amount', key: 'amount' },
		{
			title: 'Transaction Date',
			dataIndex: 'transactionDate',
			key: 'transactionDate',
		},
	];
	const { data } = useTransactionHook();

	return (
		<div className="w-full min-h-screen mb-10">
			<div className="py-2 mx-8 mt-12 space-y-8 lg:mx-32 width-full ">
				{/* Page heading */}
				<div className="mx-auto space-y-2">
					<p className="max-w-4xl text-2xl text-gray-500">
						Hi There <span className="font-bold text-indigo-500">User</span>{' '}
						Welcome.
					</p>
					<h2 className="text-sm font-medium leading-6 text-gray-900">
						Here are your transactions.
					</h2>
				</div>

				{/* divider */}
				<div className="hidden sm:block" aria-hidden="true">
					<div className="py-5">
						<div className="border-t border-gray-200" />
					</div>
				</div>
				{/* Member Transaction List */}

				<div>
					<Table columns={columns} dataSource={data!} />
				</div>
			</div>
		</div>
	);
};
export default Transactions;
