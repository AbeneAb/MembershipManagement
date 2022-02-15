import React from 'react';
import {
	BrowserRouter as Router,
	Redirect,
	Route,
	Switch,
} from 'react-router-dom';
import NotFoundPage from '../404';
import DefaultLayout from '../../layout/defaultLayout';
import MemberTransaction from '../members/index';
import Transactions from '../transactions/index';
import HealthDashboard from '../healthStatus/index';

function App() {
	return (
		<Router>
			<Switch>
				<Route path="/" exact>
					<Redirect to="/member" />
				</Route>
				<Route
					path={['/member', '/transactions', '/healthData', '/lab', '/report']}>
					<DefaultLayout>
						<Route path="/member" component={MemberTransaction} />
						<Route path="/transactions" component={Transactions} />
						<Route path="/healthData" component={HealthDashboard} />
					</DefaultLayout>
				</Route>
				<Route path="*">
					<DefaultLayout>
						<NotFoundPage />
					</DefaultLayout>
				</Route>
			</Switch>
		</Router>
	);
}

export default App;
