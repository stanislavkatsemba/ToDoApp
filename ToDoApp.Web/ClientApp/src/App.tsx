import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import ToDoItemView from './components/ToDoItemsView';

import 'devextreme/dist/css/dx.light.css';
import 'devextreme/dist/css/dx.material.blue.light.css';

import './custom.css'

export default () => (
    <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/todos' component={ToDoItemView} />
        <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
    </Layout>
);
