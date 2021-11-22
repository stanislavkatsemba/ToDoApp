import React from 'react';
import { Switch, Route, Redirect } from 'react-router-dom';
import appInfo from './app-info';
import routes from './app-routes';
import { SideNavOuterToolbar as SideNavBarLayout } from './layouts/index';
import { Footer } from './components';

export default function Content(props: any) {
    return (
        <SideNavBarLayout title={appInfo.title}>
            <Switch>
                {routes.map(({ path, component }) => (
                    <Route
                        exact
                        key={path}
                        path={path}
                        component={component}
                    />
                ))}
                <Redirect to={'/home'} />
            </Switch>
            <Footer>
                Copyright © 2021-{new Date().getFullYear()} {appInfo.title} GmbH.
            </Footer>
        </SideNavBarLayout>
    );
}
