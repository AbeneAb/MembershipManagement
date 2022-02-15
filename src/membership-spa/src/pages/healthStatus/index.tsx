/* eslint-disable jsx-a11y/label-has-associated-control */
import React from 'react';
import {
	Chart as ChartJS,
	CategoryScale,
	LinearScale,
	PointElement,
	LineElement,
	Title,
	Tooltip,
	Legend,
} from 'chart.js';
import { Line } from 'react-chartjs-2';

import { useHealthInfoHook } from './useHealthStatus';

ChartJS.register(
	CategoryScale,
	LinearScale,
	PointElement,
	LineElement,
	Title,
	Tooltip,
	Legend,
);

const HealthDashboard: React.FunctionComponent = () => {
	const pulseOptions = {
		responsive: true,
		plugins: {
			legend: {
				position: 'top' as const,
			},
			title: {
				display: true,
				text: 'Pulse Chart',
			},
		},
	};
	const bloodPressureOptions = {
		responsive: true,
		plugins: {
			legend: {
				position: 'top' as const,
			},
			title: {
				display: true,
				text: 'Blood Pressure chart',
			},
		},
	};
	const { data } = useHealthInfoHook();
	const time = data?.map(u => u.time);
	const systolic = data?.map(u => u.systolic);
	const diastolic = data?.map(u => u.diastolic);

	const chartData = {
		labels: time,
		datasets: [
			{
				label: 'Systolic',
				data: systolic,
				borderColor: 'rgb(255, 99, 132)',
				backgroundColor: 'rgba(255, 99, 132, 0.5)',
			},
			{
				label: 'Diastolic',
				data: diastolic,
				borderColor: 'rgb(53, 162, 235)',
				backgroundColor: 'rgba(53, 162, 235, 0.5)',
			},
		],
	};

	const heartBeatChartData = {
		labels: time,
		datasets: [
			{
				label: 'Pulse',
				data: data?.map(d => d.heartRate),
				borderColor: 'rgb(255, 99, 132)',
				backgroundColor: 'rgba(255, 99, 132, 0.5)',
			},
		],
	};

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
				<div>
					<div className="md:grid md:grid-cols-3 md-gap-6">
						<div className="px-4 md:col-span-1 sm:px-1">
							<h3 className="text-lg font-medium leading-6 text-gray-900">
								Systolic and Diastolic
							</h3>
							<p className="mt-1 text-sm text-gray-600">
								Diastole and systole are two phases of the cardiac cycle. They
								occur as the heart beats, pumping blood through a system of
								blood vessels that carry blood to every part of the body.
								Systole occurs when the heart contracts to pump blood out, and
								diastole occurs when the heart relaxes after contraction.
							</p>
						</div>
						<div className="mt-5 md:mt-0 md:col-span-2">
							<div className="px-4 py-5 space-y-6 bg-white shadow sm:rounded-md sm:overflow-hidden sm:p-6">
								<Line options={bloodPressureOptions} data={chartData} />
							</div>
						</div>
					</div>
				</div>

				{/* divider */}
				<div className="hidden sm:block" aria-hidden="true">
					<div className="py-5">
						<div className="border-t border-gray-200" />
					</div>
				</div>
				{/* Member Transaction List */}
				<div>
					<div className="md:grid md:grid-cols-3 md-gap-6">
						<div className="px-4 md:col-span-1 sm:px-1">
							<h3 className="text-lg font-medium leading-6 text-gray-900">
								Pulse Rate
							</h3>
							<p className="mt-1 text-sm text-gray-600">
								A normal resting heart rate for adults ranges from 60 to 100
								beats per minute. Generally, a lower heart rate at rest implies
								more efficient heart function and better cardiovascular fitness.
								For example, a well-trained athlete might have a normal resting
								heart rate closer to 40 beats per minute.
							</p>
						</div>
						<div className="mt-5 md:mt-0 md:col-span-2">
							<div className="px-4 py-5 space-y-6 bg-white shadow sm:rounded-md sm:overflow-hidden sm:p-6">
								<Line options={pulseOptions} data={heartBeatChartData} />
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
};
export default HealthDashboard;
