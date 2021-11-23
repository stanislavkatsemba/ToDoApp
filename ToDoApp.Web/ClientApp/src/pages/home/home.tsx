import React from 'react';
import './home.scss';
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import { Button } from 'devextreme-react/button';
import DataGrid, { Pager, Paging } from 'devextreme-react/data-grid';
import CustomStore from 'devextreme/data/custom_store';
import DataSource from 'devextreme/data/data_source';
import notification from '../../utils/notification';

interface IHomeState {
    connectionStarted: boolean,
    dataSource: DataSource,
}

export default class Home extends React.Component<{}, IHomeState> {

    hubConnection: HubConnection = null as any;

    constructor(props: {}) {
        super(props);
        this.state = { connectionStarted: false, dataSource: null as any };
    }

    componentDidMount() {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl('/hubs/toDoItems')
            .withAutomaticReconnect()
            .build();

        const store = new CustomStore({
            load: () => this.hubConnection.invoke('GetAllToDoItems'),
            key: 'id',
        });

        const dataSource = new DataSource({ store: store, reshapeOnPush: true });

        this.hubConnection
            .start()
            .then(() => {
                this.hubConnection.on('ReceiveToDoItem', (data) => {
                    store.push([{ type: "remove", key: data.id }, { type: "insert", data: data }]);
                });
                this.setState({ connectionStarted: true, dataSource: dataSource });
            });
    }

    componentWillUnmount() {
        this.hubConnection.stop();
    }

    onClick = () => {
        const item = {
            name: "User",
            description: "Test to do"
        };

        return this.hubConnection.send("CreateToDoItem", item).catch(_ => {
            notification.error('Noch keine Verbindung zum Server.', 5000);
        });
    }

    render() {

        return (
            <>
                <h2 className={'content-block'}>Aufgaben</h2>
                <div className={'content-block'}>
                    <div className={'dx-card responsive-paddings'}>
                        <Button text="Create test ToDo" onClick={this.onClick} />
                        <hr />

                        {this.state.connectionStarted
                            &&
                            <DataGrid
                                className={'dx-card wide-card'}
                                dataSource={this.state.dataSource}
                                showBorders={false}
                                columnAutoWidth={true}
                                columnHidingEnabled={true}
                            >
                                <Paging defaultPageSize={10} />
                                <Pager showPageSizeSelector={true} showInfo={true} />
                            </DataGrid>
                        }

                    </div>
                </div>
            </>
        );
    }
}
