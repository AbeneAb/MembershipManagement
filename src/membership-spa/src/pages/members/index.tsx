/* eslint-disable jsx-a11y/label-has-associated-control */
import React from 'react';
import { Table } from 'antd';
import 'antd/dist/antd.min.css';
import { useMembersFroms } from './useMembers';

const MemberTransaction: React.FunctionComponent = () => {
	const columns = [
		{ title: 'First Name', dataIndex: 'firstName', key: 'firstName' },
		{ title: 'Last Name', dataIndex: 'lastName', key: 'lastName' },
		{ title: 'Loan Number', dataIndex: 'loanNumber', key: 'bookingDate' },
		{ title: 'Amount', dataIndex: 'amount', key: 'amount' },
        { title: 'Transaction Date', dataIndex: 'transactionDate', key: 'transactionDate' },

	];
	const {
		register,
		enableTable,
		memberList,
		memberTransactionList,
		handleSubmit,
		watch,
		onValidSubmitHandler,
		onInvalidSubmitHandler,
	} = useMembersFroms();

	const watchMemberId = watch('memberId');
	const placeHolderForMember = 'Select Member';

	return (
		<div className="w-full min-h-screen mb-10">
			<form
				onSubmit={handleSubmit(onValidSubmitHandler, onInvalidSubmitHandler)}>
				<div className="py-2 mx-8 mt-12 space-y-8 lg:mx-32 width-full ">
					{/* Page heading */}
					<div className="mx-auto space-y-2">
						<p className="max-w-4xl text-2xl text-gray-500">
							Hi There <span className="font-bold text-indigo-500">User</span>{' '}
							Welcome.
						</p>
						<h2 className="text-sm font-medium leading-6 text-gray-900">
							Add loan transaction for a given member.
						</h2>
					</div>
					{/* First Part */}
					<div>
						<div className="md:grid md:grid-cols-3 md-gap-6">
							<div className="px-4 md:col-span-1 sm:px-1 mr-8">
								<div>
									<label
										htmlFor="testCenter"
										className="block text-sm font-medium text-gray-700">
										Member Name
									</label>
									<div className="relative mt-1 rounded-md shadow-sm">
										<select
											{...register('memberId')}
											defaultValue={
												watchMemberId === placeHolderForMember
													? 'Selext Member First'
													: 'Select Member'
											}
											className="w-full h-full px-3 py-2 text-gray-500 bg-transparent bg-white border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-1 focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm disabled:bg-gray-100">
											<option disabled>{placeHolderForMember}</option>
											{memberList?.map(c => (
												<option key={c.id} value={c.id}>
													{c.firstName} {c.lastName}
												</option>
											))}
										</select>
									</div>
								</div>
								<p className="mt-1 text-sm text-gray-600">
									Pick the member for which we will create loan transaction.
								</p>
							</div>

							{enableTable && (
								<div className="mt-5 ml-4 md:mt-0 md:col-span-2">
									<div className="px-4 py-5 space-y-6 bg-white shadow sm:rounded-md sm:overflow-hidden sm:p-6">
										<div className="col-span-3 sm:col-span-2">
											{/* Loan Number */}
											<div>
												<label
													htmlFor="loanNumber"
													className="block text-sm font-medium text-gray-700">
													Loan Number
												</label>
												<div className="mt-1 rounded-md shadow-sm">
													<input
														type="text"
														className="flex-1 block w-full px-3 py-2 border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-1 focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
														placeholder="Provide loan number"
														{...register('loanNumber')}
													/>
												</div>
											</div>
											<div>
												<label
													htmlFor="amount"
													className="block text-sm font-medium text-gray-700">
													Amount
												</label>
												<div className="mt-1 rounded-md shadow-sm">
													<input
														type="text"
														className="flex-1 block w-full px-3 py-2 border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-1 focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
														placeholder="0.00"
														{...register('amount', { valueAsNumber: true })}
													/>
												</div>
											</div>
											<div>
												<label
													htmlFor="transactionDate"
													className="block text-sm font-medium text-gray-700">
													Transaction Date
												</label>
												<div className="mt-1 rounded-md shadow-sm">
													<input
														type="text"
														className="flex-1 block w-full px-3 py-2 border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-1 focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
														placeholder="02/15/2022"
														{...register('transactionDate', {
															valueAsDate: true,
														})}
													/>
												</div>
											</div>
										</div>
									</div>
								</div>
							)}
						</div>
					</div>
					{enableTable && (
						<div>
							<div className="md:grid mg:grid-cols-1 md-gap-6 justify-items-end">
								<div className="items-center justify-center flex-1 w-full md:col-end-4 md:col-span-1">
									<button
										type="submit"
										className="items-center justify-center w-full px-6 py-3 text-sm font-medium text-white bg-yellow-400 border border-transparent rounded-md shadow-sm sm:text-base sm:inline-flex hover:bg-yellow-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-yellow-600">
										Save Transaction
										<div
											className={`hidden {isSubmitting ? 'inline-flex'} flex items-center justify-center `}>
											<div className="w-4 h-4 ml-10 border-b-2 rounded-full border-gray-50 animate-spin" />
										</div>
									</button>
								</div>
							</div>
						</div>
					)}

					{/* divider */}
					<div className="hidden sm:block" aria-hidden="true">
						<div className="py-5">
							<div className="border-t border-gray-200" />
						</div>
					</div>
					{/* Member Transaction List */}
					{enableTable && (
						<div>
							<Table columns={columns} dataSource={memberTransactionList!} />
						</div>
					)}
				</div>
			</form>
		</div>
	);
};
export default MemberTransaction;
