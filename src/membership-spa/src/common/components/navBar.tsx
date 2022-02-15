import { Disclosure, Menu, Transition } from '@headlessui/react';
import {
	ChatIcon,
	LogoutIcon,
	MailIcon,
	MenuIcon,
	SearchIcon,
	UsersIcon,
	XIcon,
	BookOpenIcon,
    HeartIcon
} from '@heroicons/react/outline';
import React, { Fragment } from 'react';

const navigation = [
	{ name: 'Members', href: '/', current: true, icon: SearchIcon },
	{ name: 'Transactions', href: '/transactions', current: false, icon: BookOpenIcon },
	{ name: 'Heart Beat', href: '/healthData', current: false, icon: HeartIcon },
	{ name: 'Help Center', href: '#', current: false, icon: ChatIcon },
];

const settingsNavigation = [
	{ name: 'Profile', href: '#', icon: UsersIcon },
	{ name: 'Support', href: '#', icon: MailIcon },
	{ name: 'Logout', href: '#', icon: LogoutIcon },
];

function classNames(...classes: string[]) {
	return classes.filter(Boolean).join(' ');
}
export const NavBar = () => (
	<Disclosure as="header" className="bg-white shadow">
		{({ open }) => (
			<>
				<div className="w-auto lg:divide-y lg:divide-gray-200 ">
					<div className="relative flex justify-between h-16 px-2 mx-auto sm:px-4 lg:px-6">
						<div className="relative z-10 flex px-2 lg:px-0">
							<div className="flex items-center flex-shrink-0"/>
						</div>

						<div className="relative z-10 flex items-center lg:hidden">
							{/* Mobile menu button */}
							<Disclosure.Button className="inline-flex items-center justify-center p-2 text-gray-400 rounded-md hover:bg-gray-100 hover:text-gray-500 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-indigo-500">
								<span className="sr-only">Open menu</span>
								{open ? (
									<XIcon className="block w-6 h-6" aria-hidden="true" />
								) : (
									<MenuIcon className="block w-6 h-6" aria-hidden="true" />
								)}
							</Disclosure.Button>
						</div>
						<div className="hidden lg:relative lg:z-10 lg:ml-4 lg:flex lg:items-center">
							{/* settings dropdown */}
							<Menu as="div" className="relative flex-shrink-0 ml-4">
								{({ open: openMenu }) => (
									<>
										<div>
											<Menu.Button className="flex text-center bg-white rounded-full focus:outline-none">
												<span>Settings</span>
											</Menu.Button>
										</div>
										<Transition
											show={openMenu}
											as={Fragment}
											enter="transition ease-out duration-100"
											enterFrom="transform opacity-0 scale-95"
											enterTo="transform opacity-100 scale-100"
											leave="transition ease-in duration-75"
											leaveFrom="transform opacity-100 scale-100"
											leaveTo="transform opacity-0 scale-95">
											<Menu.Items
												static
												className="absolute right-0 w-48 py-1 mt-2 origin-top-right bg-white rounded-md shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
												{settingsNavigation.map(item => (
													<Menu.Item key={item.name}>
														{({ active }) => (
															<>
																<a
																	href={item.href}
																	className={classNames(
																		active ? 'bg-gray-100' : '',
																		'block py-2 px-4 text-sm text-gray-700',
																		'inline-flex items-center',
																	)}>
																	<span className="mr-3">
																		{item.icon({
																			height: '20px',
																			width: '15px',
																		})}
																	</span>
																	{item.name}
																</a>
															</>
														)}
													</Menu.Item>
												))}
											</Menu.Items>
										</Transition>
									</>
								)}
							</Menu>
						</div>
					</div>
					<nav
						className="items-center justify-between hidden px-8 lg:px-32 lg:py-2 lg:flex "
						aria-label="Global">
						{navigation.map(item => (
							<a
								key={item.name}
								href={item.href}
								className={classNames(
									item.current
										? 'bg-indigo-100 text-indigo-600'
										: 'text-gray-900 hover:bg-gray-50 hover:text-gray-900',
									'rounded-md py-2 px-3 inline-flex items-center text-sm font-medium',
								)}
								aria-current={item.current ? 'page' : undefined}>
								<span className="mr-1">
									{item.icon({ height: '20px', width: '15px' })}
								</span>
								{item.name}
							</a>
						))}
					</nav>
				</div>

				<Disclosure.Panel as="nav" className="lg:hidden" aria-label="Global">
					<div className="px-2 pt-2 pb-3 space-y-1">
						{navigation.map(item => (
							<a
								key={item.name}
								href={item.href}
								className={classNames(
									item.current
										? 'bg-gray-100 text-gray-900'
										: 'text-gray-900 hover:bg-gray-50 hover:text-gray-900',
									'block rounded-md py-2 px-3 text-base font-medium',
								)}
								aria-current={item.current ? 'page' : undefined}>
								{item.name}
							</a>
						))}
					</div>
					<div className="pt-4 pb-3 border-t border-gray-200">
						<div className="flex items-center px-4">
							<button
								type="button"
								className="flex-shrink-0 p-1 ml-auto text-gray-400 bg-white rounded-full hover:text-gray-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500">
								<span className="w-6 h-6">Settings</span>
							</button>
						</div>
						<div className="px-2 mt-3 space-y-1">
							{settingsNavigation.map(setting => (
								<a
									key={setting.name}
									href={setting.href}
									className="block px-3 py-2 text-base font-medium text-gray-500 rounded-md hover:bg-gray-50 hover:text-gray-900">
									<span className="inline">{setting.name}</span>
								</a>
							))}
						</div>
					</div>
				</Disclosure.Panel>
			</>
		)}
	</Disclosure>
);
