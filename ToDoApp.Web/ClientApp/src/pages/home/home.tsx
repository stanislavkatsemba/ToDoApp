import React from 'react';
import './home.scss';
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import DataGrid, { Pager, Paging, Column, Editing, Form, FormItem } from 'devextreme-react/data-grid';
import CustomStore from 'devextreme/data/custom_store';
import DataSource from 'devextreme/data/data_source';
import notification from '../../utils/notification';
import { ToDoItemCreateInfo, ToDoItem } from '../../api/apiInterfaces';
import nameof from 'ts-nameof.macro';
import { CheckBox } from 'devextreme-react/check-box';
import { RequiredRule, SimpleItem } from 'devextreme-react/form';
import "devextreme/ui/text_area";

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

        const store = new CustomStore<ToDoItem>({
            load: () => this.hubConnection.invoke('GetAllToDoItems'),
            insert: (newItem) => {
                const item: ToDoItemCreateInfo = {
                    name: newItem.name,
                    description: newItem.description,
                    scheduledDate: newItem.scheduledDate,
                };
                this.hubConnection.send("CreateToDoItem", item).catch(_ => {
                    notification.error('Keine Verbindung zum Server.', 5000);
                });
                Promise.resolve({} as any);
            },
            key: nameof<ToDoItem>(x => x.id),
        });

        const dataSource = new DataSource({ store: store, reshapeOnPush: true });

        this.hubConnection
            .start()
            .then(() => {
                this.hubConnection.on('ReceiveToDoItem', (data: ToDoItem) => {
                    store.push([{ type: "remove", key: nameof<ToDoItem>(x => x.id) }, { type: "insert", data: data }]);
                });
                this.setState({ connectionStarted: true, dataSource: dataSource });
            });
    }

    componentWillUnmount() {
        this.hubConnection.stop();
    }

    render() {

        return (
            <>
                <h2 className={'content-block'}>Aufgaben</h2>
                <div className={'content-block'}>
                    <div className={'dx-card responsive-paddings'}>
                        {this.state.connectionStarted
                            &&
                            <DataGrid
                                className={'dx-card wide-card'}
                                dataSource={this.state.dataSource}
                                showBorders={false}
                                columnAutoWidth={true}
                                columnHidingEnabled={true}
                                showColumnHeaders={false}
                            >
                                <Editing
                                    mode="form"
                                    allowUpdating={true}
                                    allowAdding={true}
                                    allowDeleting={true}
                                >
                                    <Form
                                        showValidationSummary={true}
                                        colCount={1}
                                    >
                                        <SimpleItem
                                            dataField={nameof<ToDoItem>(x => x.name)}
                                            editorOptions={{ maxLength: 250 }}
                                        >
                                        </SimpleItem>
                                        <SimpleItem
                                            dataField={nameof<ToDoItem>(x => x.description)}
                                        >
                                        </SimpleItem>
                                        <SimpleItem
                                            dataField={nameof<ToDoItem>(x => x.scheduledDate)}
                                        >
                                        </SimpleItem>
                                    </Form>
                                </Editing>
                                <Paging defaultPageSize={10} />
                                <Pager showPageSizeSelector={true} showInfo={true} />
                                <Column
                                    dataField={nameof<ToDoItem>(x => x.id)}
                                    cellRender={this.cellRender}
                                />
                                <Column
                                    dataField={nameof<ToDoItem>(x => x.name)}
                                    visible={false}
                                    caption="Theme"
                                >
                                    <RequiredRule message="Thema ist erforderlich" />
                                </Column>
                                <Column
                                    caption="Beschreibung"
                                    dataField={nameof<ToDoItem>(x => x.description)}
                                    visible={false}
                                >
                                    <FormItem editorType="dxTextArea" />
                                </Column>
                                <Column
                                    caption="Planen"
                                    dataField={nameof<ToDoItem>(x => x.scheduledDate)}
                                    visible={false}
                                />
                            </DataGrid>
                        }
                    </div>
                </div>
            </>
        );
    }

    cellRender = (e: { data: ToDoItem }) => {
        return (
            <>
                <CheckBox value={e.data.isCompleted} />&nbsp;&nbsp;&nbsp;
                {e.data.name}
                <div className="item-description">{e.data.description}</div>
            </>
        );
    }
}
